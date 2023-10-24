using System;
using System.Runtime.InteropServices;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace Aperture.Astcenc.Runtime
{
    public static class Encoder
    {
        private const int k_BytePerBlock = 16;

        private static bool GetBlockSize(BlockSize size, out uint x, out uint y, out uint z)
        {
            switch (size)
            {
                case BlockSize.ASTC_4x4:
                    x = y = 4;
                    z = 1;
                    return true;
                case BlockSize.ASTC_5x5:
                    x = y = 5;
                    z = 1;
                    return true;
                case BlockSize.ASTC_6x6:
                    x = y = 6;
                    z = 1;
                    return true;
                case BlockSize.ASTC_8x8:
                    x = y = 8;
                    z = 1;
                    return true;
                case BlockSize.ASTC_10x10:
                    x = y = 10;
                    z = 1;
                    return true;
                case BlockSize.ASTC_12x12:
                    x = y = 12;
                    z = 1;
                    return true;
            }
            x = y = z = 0;
            return false;
        }

        private static float GetQualityValue(CompressQuality quality)
        {
            return quality switch
            {
                CompressQuality.Fastest => 0.0f,
                CompressQuality.Fast => 10.0f,
                CompressQuality.Medium => 60.0f,
                CompressQuality.Thorough => 98.0f,
                CompressQuality.VeryThorough => 99.0f,
                CompressQuality.Exhaustive => 100.0f,
                _ => throw new ArgumentException("Unknown type of a CompressQuality:", nameof(quality)),
            };
        }

        private static astcenc_profile GetColorProfile(GraphicsFormat graphicsFormat)
        {
            return graphicsFormat switch
            {
                //LDR

                GraphicsFormat.R8_UNorm => astcenc_profile.ASTCENC_PRF_LDR,
                GraphicsFormat.R8_SRGB => astcenc_profile.ASTCENC_PRF_LDR_SRGB,
                GraphicsFormat.R8G8_UNorm => astcenc_profile.ASTCENC_PRF_LDR,
                GraphicsFormat.R8G8_SRGB => astcenc_profile.ASTCENC_PRF_LDR_SRGB,
                GraphicsFormat.R8G8B8_UNorm => astcenc_profile.ASTCENC_PRF_LDR,
                GraphicsFormat.R8G8B8_SRGB => astcenc_profile.ASTCENC_PRF_LDR_SRGB,
                GraphicsFormat.R8G8B8A8_UNorm => astcenc_profile.ASTCENC_PRF_LDR,
                GraphicsFormat.R8G8B8A8_SRGB => astcenc_profile.ASTCENC_PRF_LDR_SRGB,
                GraphicsFormat.B8G8R8_UNorm => astcenc_profile.ASTCENC_PRF_LDR,
                GraphicsFormat.B8G8R8_SRGB => astcenc_profile.ASTCENC_PRF_LDR_SRGB,
                GraphicsFormat.B8G8R8A8_UNorm => astcenc_profile.ASTCENC_PRF_LDR,
                GraphicsFormat.B8G8R8A8_SRGB => astcenc_profile.ASTCENC_PRF_LDR_SRGB,

                GraphicsFormat.R16_UNorm => astcenc_profile.ASTCENC_PRF_LDR,
                GraphicsFormat.R16G16_UNorm => astcenc_profile.ASTCENC_PRF_LDR,
                GraphicsFormat.R16G16B16_UNorm => astcenc_profile.ASTCENC_PRF_LDR,
                GraphicsFormat.R16G16B16A16_UNorm => astcenc_profile.ASTCENC_PRF_LDR,

                //HDR

                GraphicsFormat.R16_SFloat => astcenc_profile.ASTCENC_PRF_HDR,
                GraphicsFormat.R16G16_SFloat => astcenc_profile.ASTCENC_PRF_HDR,
                GraphicsFormat.R16G16B16_SFloat => astcenc_profile.ASTCENC_PRF_HDR,
                GraphicsFormat.R16G16B16A16_SFloat => astcenc_profile.ASTCENC_PRF_HDR,

                GraphicsFormat.R32_SFloat => astcenc_profile.ASTCENC_PRF_HDR,
                GraphicsFormat.R32G32_SFloat => astcenc_profile.ASTCENC_PRF_HDR,
                GraphicsFormat.R32G32B32_SFloat => astcenc_profile.ASTCENC_PRF_HDR,
                GraphicsFormat.R32G32B32A32_SFloat => astcenc_profile.ASTCENC_PRF_HDR,
                _ => throw new NotSupportedException("GraphicsFormat is not support:"),
            };
        }

        private static CodecFlags GetFlags(TextureType textureType, GraphicsFormat graphicsFormat)
        {
            CodecFlags flags = textureType == TextureType.Default ? CodecFlags.None : CodecFlags.MapNormal;

            switch (graphicsFormat)
            {
                //LDR

                case GraphicsFormat.R8_UNorm:
                case GraphicsFormat.R8_SRGB:
                case GraphicsFormat.R8G8_UNorm:
                case GraphicsFormat.R8G8_SRGB:
                case GraphicsFormat.R8G8B8_UNorm:
                case GraphicsFormat.R8G8B8_SRGB:
                    break;
                case GraphicsFormat.R8G8B8A8_UNorm:
                case GraphicsFormat.R8G8B8A8_SRGB:
                    flags |= CodecFlags.UseAlphaWeight;
                    break;

                case GraphicsFormat.B8G8R8_UNorm:
                case GraphicsFormat.B8G8R8_SRGB:
                    break;
                case GraphicsFormat.B8G8R8A8_UNorm:
                case GraphicsFormat.B8G8R8A8_SRGB:
                    flags |= CodecFlags.UseAlphaWeight;
                    break;

                case GraphicsFormat.R16_UNorm:
                case GraphicsFormat.R16G16_UNorm:
                case GraphicsFormat.R16G16B16_UNorm:
                    break;
                case GraphicsFormat.R16G16B16A16_UNorm:
                    flags |= CodecFlags.UseAlphaWeight;
                    break;

                //HDR

                case GraphicsFormat.R16_SFloat:
                case GraphicsFormat.R16G16_SFloat:
                case GraphicsFormat.R16G16B16_SFloat:
                    break;
                case GraphicsFormat.R16G16B16A16_SFloat:
                    flags |= CodecFlags.UseAlphaWeight;
                    break;

                case GraphicsFormat.R32_SFloat:
                case GraphicsFormat.R32G32_SFloat:
                case GraphicsFormat.R32G32B32_SFloat:
                    break;
                case GraphicsFormat.R32G32B32A32_SFloat:
                    flags |= CodecFlags.UseAlphaWeight;
                    break;
            }

            return flags;
        }

        private static astcenc_swizzle GetSwizzle(GraphicsFormat graphicsFormat, TextureType textureType)
        {
            if (textureType == TextureType.Normal)
            {
                astcenc_swizzle swizzle = new astcenc_swizzle
                {
                    r = astcenc_swz.ASTCENC_SWZ_G,
                    g = astcenc_swz.ASTCENC_SWZ_G,
                    b = astcenc_swz.ASTCENC_SWZ_G,
                    a = astcenc_swz.ASTCENC_SWZ_R,
                };
                return swizzle;
            }

            switch (graphicsFormat)
            {
                case GraphicsFormat.B8G8R8_UNorm:
                case GraphicsFormat.B8G8R8_SRGB:
                case GraphicsFormat.B8G8R8A8_UNorm:
                case GraphicsFormat.B8G8R8A8_SRGB:
                    {
                        astcenc_swizzle swizzle = new astcenc_swizzle
                        {
                            r = astcenc_swz.ASTCENC_SWZ_B,
                            g = astcenc_swz.ASTCENC_SWZ_G,
                            b = astcenc_swz.ASTCENC_SWZ_R,
                            a = astcenc_swz.ASTCENC_SWZ_A,
                        };
                        return swizzle;
                    }
                default:
                    {
                        astcenc_swizzle swizzle = new astcenc_swizzle
                        {
                            r = astcenc_swz.ASTCENC_SWZ_R,
                            g = astcenc_swz.ASTCENC_SWZ_G,
                            b = astcenc_swz.ASTCENC_SWZ_B,
                            a = astcenc_swz.ASTCENC_SWZ_A,
                        };
                        return swizzle;
                    }
            }
        }

        private static GraphicsFormat FitAstcGraphicsFormat(GraphicsFormat graphicsFormat, BlockSize blockSize)
        {
            switch (graphicsFormat)
            {
                case GraphicsFormat.R8_SRGB:
                case GraphicsFormat.R8G8_SRGB:
                case GraphicsFormat.R8G8B8_SRGB:
                case GraphicsFormat.R8G8B8A8_SRGB:

                case GraphicsFormat.B8G8R8_SRGB:
                case GraphicsFormat.B8G8R8A8_SRGB:
                    {
                        switch (blockSize)
                        {
                            case BlockSize.ASTC_4x4:
                                return GraphicsFormat.RGBA_ASTC4X4_SRGB;
                            case BlockSize.ASTC_5x5:
                                return GraphicsFormat.RGBA_ASTC5X5_SRGB;
                            case BlockSize.ASTC_6x6:
                                return GraphicsFormat.RGBA_ASTC6X6_SRGB;
                            case BlockSize.ASTC_8x8:
                                return GraphicsFormat.RGBA_ASTC8X8_SRGB;
                            case BlockSize.ASTC_10x10:
                                return GraphicsFormat.RGBA_ASTC10X10_SRGB;
                            case BlockSize.ASTC_12x12:
                                return GraphicsFormat.RGBA_ASTC12X12_SRGB;
                        }
                    }
                    break;
                case GraphicsFormat.R8_UNorm:
                case GraphicsFormat.R8G8_UNorm:
                case GraphicsFormat.R8G8B8_UNorm:
                case GraphicsFormat.R8G8B8A8_UNorm:
                case GraphicsFormat.B8G8R8_UNorm:
                case GraphicsFormat.B8G8R8A8_UNorm:

                case GraphicsFormat.R16_UNorm:
                case GraphicsFormat.R16G16_UNorm:
                case GraphicsFormat.R16G16B16_UNorm:
                case GraphicsFormat.R16G16B16A16_UNorm:
                    switch (blockSize)
                    {
                        case BlockSize.ASTC_4x4:
                            return GraphicsFormat.RGBA_ASTC4X4_UNorm;
                        case BlockSize.ASTC_5x5:
                            return GraphicsFormat.RGBA_ASTC5X5_UNorm;
                        case BlockSize.ASTC_6x6:
                            return GraphicsFormat.RGBA_ASTC6X6_UNorm;
                        case BlockSize.ASTC_8x8:
                            return GraphicsFormat.RGBA_ASTC8X8_UNorm;
                        case BlockSize.ASTC_10x10:
                            return GraphicsFormat.RGBA_ASTC10X10_UNorm;
                        case BlockSize.ASTC_12x12:
                            return GraphicsFormat.RGBA_ASTC12X12_UNorm;
                    }
                    break;
                case GraphicsFormat.R16_SFloat:
                case GraphicsFormat.R16G16_SFloat:
                case GraphicsFormat.R16G16B16_SFloat:
                case GraphicsFormat.R16G16B16A16_SFloat:

                case GraphicsFormat.R32_SFloat:
                case GraphicsFormat.R32G32_SFloat:
                case GraphicsFormat.R32G32B32_SFloat:
                case GraphicsFormat.R32G32B32A32_SFloat:
                    switch (blockSize)
                    {
                        case BlockSize.ASTC_4x4:
                            return GraphicsFormat.RGBA_ASTC4X4_UFloat;
                        case BlockSize.ASTC_5x5:
                            return GraphicsFormat.RGBA_ASTC5X5_UFloat;
                        case BlockSize.ASTC_6x6:
                            return GraphicsFormat.RGBA_ASTC6X6_UFloat;
                        case BlockSize.ASTC_8x8:
                            return GraphicsFormat.RGBA_ASTC8X8_UFloat;
                        case BlockSize.ASTC_10x10:
                            return GraphicsFormat.RGBA_ASTC10X10_UFloat;
                        case BlockSize.ASTC_12x12:
                            return GraphicsFormat.RGBA_ASTC12X12_UFloat;
                    }
                    break;
            }
            throw new ArgumentException();
        }

        private static bool IsSupportGraphicFormat(GraphicsFormat graphicsFormat)
        {
            switch (graphicsFormat)
            {
                //LDR

                case GraphicsFormat.R8_UNorm:
                case GraphicsFormat.R8_SRGB:
                case GraphicsFormat.R8G8_UNorm:
                case GraphicsFormat.R8G8_SRGB:
                case GraphicsFormat.R8G8B8_UNorm:
                case GraphicsFormat.R8G8B8_SRGB:
                case GraphicsFormat.R8G8B8A8_UNorm:
                case GraphicsFormat.R8G8B8A8_SRGB:

                case GraphicsFormat.B8G8R8_UNorm:
                case GraphicsFormat.B8G8R8_SRGB:
                case GraphicsFormat.B8G8R8A8_UNorm:
                case GraphicsFormat.B8G8R8A8_SRGB:

                case GraphicsFormat.R16_UNorm:
                case GraphicsFormat.R16G16_UNorm:
                case GraphicsFormat.R16G16B16_UNorm:
                case GraphicsFormat.R16G16B16A16_UNorm:

                //HDR

                case GraphicsFormat.R16_SFloat:
                case GraphicsFormat.R16G16_SFloat:
                case GraphicsFormat.R16G16B16_SFloat:
                case GraphicsFormat.R16G16B16A16_SFloat:

                case GraphicsFormat.R32_SFloat:
                case GraphicsFormat.R32G32_SFloat:
                case GraphicsFormat.R32G32B32_SFloat:
                case GraphicsFormat.R32G32B32A32_SFloat:
                    return true;
            }
            return false;
        }

        public static bool IsSupport(Texture texture)
        {
            return texture.isReadable && IsSupportGraphicFormat(texture.graphicsFormat);
        }

        private static bool CompressRawData<T>(IntPtr context, astcenc_swizzle swizzle, NativeArray<T> raw, int width, int height, int bitness, int mipCount, uint blockX, uint blockY, out IntPtr output, out int size) where T : struct, IRawData
        {
            //alloc a total memory to hold all ths mipmaps
            int total = 0;
            for (int i = 0; i < mipCount; i++)
            {
                int mipmapWidth = width / (1 << i);
                int mipmapHeight = height / (1 << i);
                int blockCountX = (mipmapWidth + (int)blockX - 1) / (int)blockX;
                int blockCountY = (mipmapHeight + (int)blockY - 1) / (int)blockY;
                int length = blockCountX * blockCountY * k_BytePerBlock;

                total += length;
            }
            output = Marshal.AllocHGlobal(total);
            size = total;

            int elementReaded = 0;
            int byteWrited = 0;

            try
            {
                for (int i = 0; i < mipCount; i++)
                {
                    int mipmapWidth = width / (1 << i);
                    int mipmapHeight = height / (1 << i);
                    int blockCountX = (mipmapWidth + (int)blockX - 1) / (int)blockX;
                    int blockCountY = (mipmapHeight + (int)blockY - 1) / (int)blockY;
                    int length = blockCountX * blockCountY * k_BytePerBlock;

                    int count = mipmapWidth * mipmapHeight;
                    NativeSlice<T> mipmapSlice = raw.Slice(elementReaded, count);

                    astcenc_image image = new astcenc_image();
                    try
                    {
                        image.LoadFrom(mipmapSlice, mipmapWidth, mipmapHeight, bitness);

                        astcenc_error status = astcenc_api.astcenc_compress_image(context, ref image, ref swizzle, output + byteWrited, length, 0);
                        if (status != astcenc_error.ASTCENC_SUCCESS)
                        {
                            throw new Exception(status.ToString());
                        }
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        image.Release();
                        astcenc_api.astcenc_compress_reset(context);
                    }

                    elementReaded += count;
                    byteWrited += length;
                }
            }
            catch
            {
                Marshal.FreeHGlobal(output);

                output = IntPtr.Zero;
                size = 0;

                return false;
            }
            return true;
        }

        private static bool CompressImage(Texture2D inputTexture, IntPtr context, astcenc_swizzle swizzle, uint blockX, uint blockY, out IntPtr output, out int size)
        {
            switch (inputTexture.graphicsFormat)
            {
                case GraphicsFormat.R8_UNorm:
                case GraphicsFormat.R8_SRGB:
                    {
                        NativeArray<R8> raw = inputTexture.GetRawTextureData<R8>();
                        return CompressRawData(context, swizzle, raw, inputTexture.width, inputTexture.height, 8, inputTexture.mipmapCount, blockX, blockY, out output, out size);
                    }
                case GraphicsFormat.R8G8_UNorm:
                case GraphicsFormat.R8G8_SRGB:
                    {
                        NativeArray<RG8> raw = inputTexture.GetRawTextureData<RG8>();
                        return CompressRawData(context, swizzle, raw, inputTexture.width, inputTexture.height, 8, inputTexture.mipmapCount, blockX, blockY, out output, out size);
                    }
                case GraphicsFormat.R8G8B8_UNorm:
                case GraphicsFormat.R8G8B8_SRGB:
                case GraphicsFormat.B8G8R8_UNorm:
                case GraphicsFormat.B8G8R8_SRGB:
                    {
                        NativeArray<RGB8> raw = inputTexture.GetRawTextureData<RGB8>();
                        return CompressRawData(context, swizzle, raw, inputTexture.width, inputTexture.height, 8, inputTexture.mipmapCount, blockX, blockY, out output, out size);
                    }
                case GraphicsFormat.R8G8B8A8_UNorm:
                case GraphicsFormat.R8G8B8A8_SRGB:
                case GraphicsFormat.B8G8R8A8_UNorm:
                case GraphicsFormat.B8G8R8A8_SRGB:
                    {
                        NativeArray<RGBA8> raw = inputTexture.GetRawTextureData<RGBA8>();
                        return CompressRawData(context, swizzle, raw, inputTexture.width, inputTexture.height, 8, inputTexture.mipmapCount, blockX, blockY, out output, out size);
                    }
                case GraphicsFormat.R16_UNorm:
                    {
                        NativeArray<R16> raw = inputTexture.GetRawTextureData<R16>();
                        return CompressRawData(context, swizzle, raw, inputTexture.width, inputTexture.height, 16, inputTexture.mipmapCount, blockX, blockY, out output, out size);
                    }
                case GraphicsFormat.R16G16_UNorm:
                    {
                        NativeArray<RG16> raw = inputTexture.GetRawTextureData<RG16>();
                        return CompressRawData(context, swizzle, raw, inputTexture.width, inputTexture.height, 16, inputTexture.mipmapCount, blockX, blockY, out output, out size);
                    }
                case GraphicsFormat.R16G16B16_UNorm:
                    {
                        NativeArray<RGB16> raw = inputTexture.GetRawTextureData<RGB16>();
                        return CompressRawData(context, swizzle, raw, inputTexture.width, inputTexture.height, 16, inputTexture.mipmapCount, blockX, blockY, out output, out size);
                    }
                case GraphicsFormat.R16G16B16A16_UNorm:
                    {
                        NativeArray<RGBA16> raw = inputTexture.GetRawTextureData<RGBA16>();
                        return CompressRawData(context, swizzle, raw, inputTexture.width, inputTexture.height, 16, inputTexture.mipmapCount, blockX, blockY, out output, out size);
                    }
                case GraphicsFormat.R16_SFloat:
                    {
                        NativeArray<R16F> raw = inputTexture.GetRawTextureData<R16F>();
                        return CompressRawData(context, swizzle, raw, inputTexture.width, inputTexture.height, 16, inputTexture.mipmapCount, blockX, blockY, out output, out size);
                    }
                case GraphicsFormat.R16G16_SFloat:
                    {
                        NativeArray<RG16F> raw = inputTexture.GetRawTextureData<RG16F>();
                        return CompressRawData(context, swizzle, raw, inputTexture.width, inputTexture.height, 16, inputTexture.mipmapCount, blockX, blockY, out output, out size);
                    }
                case GraphicsFormat.R16G16B16_SFloat:
                    {
                        NativeArray<RGB16F> raw = inputTexture.GetRawTextureData<RGB16F>();
                        return CompressRawData(context, swizzle, raw, inputTexture.width, inputTexture.height, 16, inputTexture.mipmapCount, blockX, blockY, out output, out size);
                    }
                case GraphicsFormat.R16G16B16A16_SFloat:
                    {
                        NativeArray<RGBA16F> raw = inputTexture.GetRawTextureData<RGBA16F>();
                        return CompressRawData(context, swizzle, raw, inputTexture.width, inputTexture.height, 16, inputTexture.mipmapCount, blockX, blockY, out output, out size);
                    }
                case GraphicsFormat.R32_SFloat:
                    {
                        NativeArray<R32F> raw = inputTexture.GetRawTextureData<R32F>();
                        return CompressRawData(context, swizzle, raw, inputTexture.width, inputTexture.height, 32, inputTexture.mipmapCount, blockX, blockY, out output, out size);
                    }
                case GraphicsFormat.R32G32_SFloat:
                    {
                        NativeArray<RG32F> raw = inputTexture.GetRawTextureData<RG32F>();
                        return CompressRawData(context, swizzle, raw, inputTexture.width, inputTexture.height, 32, inputTexture.mipmapCount, blockX, blockY, out output, out size);
                    }
                case GraphicsFormat.R32G32B32_SFloat:
                    {
                        NativeArray<RGB32F> raw = inputTexture.GetRawTextureData<RGB32F>();
                        return CompressRawData(context, swizzle, raw, inputTexture.width, inputTexture.height, 32, inputTexture.mipmapCount, blockX, blockY, out output, out size);
                    }
                case GraphicsFormat.R32G32B32A32_SFloat:
                    {
                        NativeArray<RGBA32F> raw = inputTexture.GetRawTextureData<RGBA32F>();
                        return CompressRawData(context, swizzle, raw, inputTexture.width, inputTexture.height, 32, inputTexture.mipmapCount, blockX, blockY, out output, out size);
                    }
            }

            output = IntPtr.Zero;
            size = 0;

            return false;
        }

        /// <summary>
        /// Compress the inputTexture and return the compressed data by AstcRaw, you should call Dispose() for the AstcRaw after use.
        /// </summary>
        /// <param name="inputTexture">inputTexture</param>
        /// <param name="output">output unmanaged data IntPtr</param>
        /// <param name="size">out put data size</param>
        /// <param name="textureType">Indicate that compress for color texture or notmal texture</param>
        /// <param name="blockSize">Astc compress ratio</param>
        /// <param name="quality">Compress quality setting</param>
        /// <returns>CompressedData</returns>
        public static AstcRaw CompressTexture(this Texture2D inputTexture, TextureType textureType, BlockSize blockSize, CompressQuality quality)
        {
            if (!IsSupport(inputTexture))
            {
                Debug.LogError("Texture format is not support or read/write is disabled.");
                return null;
            }

            uint threadCount = 1;
            GetBlockSize(blockSize, out uint blockX, out uint blockY, out uint blockZ);

            float qualityValue = GetQualityValue(quality);

            astcenc_profile profile = GetColorProfile(inputTexture.graphicsFormat);
            astcenc_swizzle swizzle = GetSwizzle(inputTexture.graphicsFormat, textureType);

            uint flags = (uint)GetFlags(textureType, inputTexture.graphicsFormat);

            astcenc_config config = new astcenc_config();
            astcenc_error status = astcenc_api.astcenc_config_init(profile, blockX, blockY, blockZ, qualityValue, flags, ref config);
            if (status != astcenc_error.ASTCENC_SUCCESS)
            {
                Debug.LogError("Codec config init failed:" + status);
                return null;
            }

            IntPtr context = IntPtr.Zero;
            status = astcenc_api.astcenc_context_alloc(ref config, threadCount, ref context);
            if (status != astcenc_error.ASTCENC_SUCCESS)
            {
                Debug.LogError("Codec context alloc failed:" + status);
                return null;
            }

            CompressImage(inputTexture, context, swizzle, blockX, blockY, out IntPtr output, out int size);

            astcenc_api.astcenc_context_free(context);

            return new AstcRaw(output, size);
        }

        /// <summary>
        ///  Fills texture pixels with astc raw preformatted data.
        /// </summary>
        /// <param name="inputTexture">Input texture must be uncompressed format and readable</param>
        /// <param name="raw">Astc raw data</param>
        public static void LoadRawTextureData(this Texture2D inputTexture, AstcRaw raw)
        {
            if (raw != null)
            {
                raw.ThrowIfDisposed();

                inputTexture.LoadRawTextureData(raw.Data, raw.Length);
            }
        }

        /// <summary>
        /// Compress texture at runtime to ASTC formats.
        /// </summary>
        /// <param name="inputTexutre">Input texture must be uncompressed format and readable</param>
        /// <param name="textureType">Indicate that compress for color texture or notmal texture</param>
        /// <param name="blockSize">Astc compress ratio</param>
        /// <param name="quality">Compress quality setting</param>
        /// <param name="makeNoLongerReadable">When set to true, Unity discards the copy of pixel data in CPU-addressable memory after this operation.</param>
        /// <returns>Success or not</returns>
        public static bool Compress(this Texture2D inputTexture, TextureType textureType = TextureType.Default, BlockSize blockSize = BlockSize.ASTC_6x6, CompressQuality quality = CompressQuality.Fast, bool makeNoLongerReadable = true)
        {
            using AstcRaw raw = inputTexture.CompressTexture(textureType, blockSize, quality);

            if (raw != null)
            {
                raw.ThrowIfDisposed();

                GraphicsFormat graphicsFormat = FitAstcGraphicsFormat(inputTexture.graphicsFormat, blockSize);

                if(inputTexture.Reinitialize(inputTexture.width, inputTexture.height, graphicsFormat, inputTexture.mipmapCount > 0))
                {
                    inputTexture.LoadRawTextureData(raw);
                    inputTexture.Apply(true, makeNoLongerReadable);

                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Encode texture2D to ASTC file with a astc header.(Not GC friendly)
        /// </summary>
        /// <param name="inputTexture"></param>
        /// <param name="textureType"></param>
        /// <param name="blockSize"></param>
        /// <param name="quality"></param>
        /// <returns></returns>
        public static byte[] EncodeToASTC(this Texture2D inputTexture, TextureType textureType = TextureType.Default, BlockSize blockSize = BlockSize.ASTC_6x6, CompressQuality quality = CompressQuality.Medium)
        {
            using AstcRaw raw = inputTexture.CompressTexture(textureType, blockSize, quality);

            if (raw != null)
            {
                raw.ThrowIfDisposed();

                byte[] output = new byte[16 + raw.Length];

                GetBlockSize(blockSize, out uint dimX, out uint dimY, out uint dimZ);
                using AstcHeader header = new AstcHeader(dimX, dimY, dimZ, inputTexture.width, inputTexture.height, 1);

                Marshal.Copy(header.Data, output, 0, 16);
                Marshal.Copy(raw.Data, output, 16, raw.Length);

                return output;
            }
            return null;
        }
    }
}


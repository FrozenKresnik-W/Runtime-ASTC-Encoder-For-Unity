using System;
using System.Runtime.InteropServices;
using Unity.Collections;

namespace Aperture.Astcenc.Runtime
{
    public static class ImageExtensions
    {
        private static astcenc_type GetDataType(int bitness)
        {
            if (bitness == 8)
            {
                return astcenc_type.ASTCENC_TYPE_U8;
            }
            else if (bitness == 16)
            {
                return astcenc_type.ASTCENC_TYPE_F16;
            }
            else// if (bitness == 32)
            {
                return astcenc_type.ASTCENC_TYPE_F32;
            }
        }

        internal static void LoadFrom<T>(this ref astcenc_image image, NativeSlice<T> rawData, int width, int height, int bitness) where T : struct, IRawData
        {
            if (image.data != IntPtr.Zero)
            {
                image.Release();
            }
            image.dim_x = (uint)width;
            image.dim_y = (uint)height;
            image.dim_z = 1;
            image.data_type = GetDataType(bitness);
            image.data = Marshal.AllocHGlobal(IntPtr.Size);

            IntPtr slicePtr = LoadData(rawData, bitness);
            Marshal.WriteIntPtr(image.data, 0, slicePtr);
        }

        internal static void Release(this ref astcenc_image image)
        {
            if (image.data != IntPtr.Zero)
            {
                for (int i = 0; i < image.dim_z; i++)
                {
                    IntPtr slicePtr = Marshal.ReadIntPtr(image.data, i * IntPtr.Size);
                    Marshal.FreeHGlobal(slicePtr);

                }
                Marshal.FreeHGlobal(image.data);
            }
            image.data = IntPtr.Zero;
        }

        private static IntPtr LoadData<T>(NativeSlice<T> raw, int bitness) where T : struct, IRawData
        {
            int bytes = bitness / 8;
            IntPtr dataPtr = Marshal.AllocHGlobal(raw.Length * bytes * 4);
            Copy(raw, dataPtr, bytes * 4);

            return dataPtr;
        }

        private static void Copy<T>(NativeSlice<T> raw, IntPtr destPtr, int stride) where T : struct, IRawData
        {
            for (int i = 0; i < raw.Length; i++)
            {
                T pixel = raw[i];
                pixel.WriteTo(destPtr + i * stride);
            }
        }
    }
}

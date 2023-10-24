using System;

namespace Aperture.Astcenc.Runtime
{
    [Flags]
    public enum CodecFlags : uint
    {
        None = 0,
        /**
        * @brief Enable normal map compression.
        *
        * Input data will be treated a two component normal map, storing X and Y, and the codec will
        * optimize for angular error rather than simple linear PSNR. In this mode the input swizzle should
        * be e.g. rrrg (the default ordering for ASTC normals on the command line) or gggr (the ordering
        * used by BC5n).
        */
        MapNormal = 1 << 0,
        /**
         * @brief Enable alpha weighting.
         *
         * The input alpha value is used for transparency, so errors in the RGB components are weighted by
         * the transparency level. This allows the codec to more accurately encode the alpha value in areas
         * where the color value is less significant.
         */
        UseAlphaWeight = 1 << 2,
        /**
         * @brief Enable perceptual error metrics.
         *
         * This mode enables perceptual compression mode, which will optimize for perceptual error rather
         * than best PSNR. Only some input modes support perceptual error metrics.
         */
        UsePerceptual = 1 << 3,
        /**
         * @brief Create a decompression-only context.
         *
         * This mode disables support for compression. This enables context allocation to skip some
         * transient buffer allocation, resulting in lower memory usage.
         */
        DecompressOnly = 1 << 4,
        /**
         * @brief Create a self-decompression context.
         *
         * This mode configures the compressor so that it is only guaranteed to be able to decompress images
         * that were actually created using the current context. This is the common case for compression use
         * cases, and setting this flag enables additional optimizations, but does mean that the context
         * cannot reliably decompress arbitrary ASTC images.
         */
        SelfDecompressOnly = 1 << 5,
        /**
         * @brief Enable RGBM map compression.
         *
         * Input data will be treated as HDR data that has been stored in an LDR RGBM-encoded wrapper
         * format. Data must be preprocessed by the user to be in LDR RGBM format before calling the
         * compression function, this flag is only used to control the use of RGBM-specific heuristics and
         * error metrics.
         *
         * IMPORTANT: The ASTC format is prone to bad failure modes with unconstrained RGBM data; very small
         * M values can round to zero due to quantization and result in black or white pixels. It is highly
         * recommended that the minimum value of M used in the encoding is kept above a lower threshold (try
         * 16 or 32). Applying this threshold reduces the number of very dark colors that can be
         * represented, but is still higher precision than 8-bit LDR.
         *
         * When this flag is set the value of @c rgbm_m_scale in the context must be set to the RGBM scale
         * factor used during reconstruction. This defaults to 5 when in RGBM mode.
         *
         * It is recommended that the value of @c cw_a_weight is set to twice the value of the multiplier
         * scale, ensuring that the M value is accurately encoded. This defaults to 10 when in RGBM mode,
         * matching the default scale factor.
         */
        MapRGBM = 1 << 6,
    }
}

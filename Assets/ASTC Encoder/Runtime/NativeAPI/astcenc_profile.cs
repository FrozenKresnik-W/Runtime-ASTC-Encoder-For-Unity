/**
 * @brief A codec color profile.
 */
public enum astcenc_profile
{
    /** @brief The LDR sRGB color profile. */
    ASTCENC_PRF_LDR_SRGB = 0,
    /** @brief The LDR linear color profile. */
    ASTCENC_PRF_LDR,
    /** @brief The HDR RGB with LDR alpha color profile. */
    ASTCENC_PRF_HDR_RGB_LDR_A,
    /** @brief The HDR RGBA color profile. */
    ASTCENC_PRF_HDR,
}

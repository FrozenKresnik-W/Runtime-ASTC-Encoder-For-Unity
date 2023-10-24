/**
 * @brief A codec component swizzle selector.
 */
public enum astcenc_swz
{
	/** @brief Select the red component. */
	ASTCENC_SWZ_R = 0,
	/** @brief Select the green component. */
	ASTCENC_SWZ_G = 1,
	/** @brief Select the blue component. */
	ASTCENC_SWZ_B = 2,
	/** @brief Select the alpha component. */
	ASTCENC_SWZ_A = 3,
	/** @brief Use a constant zero component. */
	ASTCENC_SWZ_0 = 4,
	/** @brief Use a constant one component. */
	ASTCENC_SWZ_1 = 5,
	/** @brief Use a reconstructed normal vector Z component. */
	ASTCENC_SWZ_Z = 6
}
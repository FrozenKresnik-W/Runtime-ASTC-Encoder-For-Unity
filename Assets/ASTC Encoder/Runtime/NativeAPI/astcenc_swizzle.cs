using System.Runtime.InteropServices;

/**
 * @brief A texel component swizzle.
 */
[StructLayout(LayoutKind.Sequential)]
public struct astcenc_swizzle
{
	/** @brief The red component selector. */
	public astcenc_swz r;
	/** @brief The green component selector. */
	public astcenc_swz g;
	/** @brief The blue component selector. */
	public astcenc_swz b;
	/** @brief The alpha component selector. */
	public astcenc_swz a;
}
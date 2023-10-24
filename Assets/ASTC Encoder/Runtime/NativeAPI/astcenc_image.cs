using System;
using System.Runtime.InteropServices;

/**
 * @brief An uncompressed 2D or 3D image.
 *
 * 3D image are passed in as an array of 2D slices. Each slice has identical
 * size and color format.
 */
[StructLayout(LayoutKind.Sequential)]
public struct astcenc_image
{
    /** @brief The X dimension of the image, in texels. */
    public uint dim_x;

    /** @brief The Y dimension of the image, in texels. */
    public uint dim_y;

    /** @brief The Z dimension of the image, in texels. */
    public uint dim_z;

    /** @brief The data type per component. */
    public astcenc_type data_type;

    /** @brief The array of 2D slices, of length @c dim_z. */
    public IntPtr data;
}
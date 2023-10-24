using System;
using System.Runtime.InteropServices;

public static class astcenc_api
{
#if UNITY_IOS && !UNITY_EDITOR
    internal const string LibraryName = "__Internal";
#elif UNITY_ANDROID && !UNITY_EDITOR
    internal const string LibraryName = "libastcenc-neon-shared";
#elif UNITY_EDITOR_OSX
    internal const string LibraryName = "libastcenc-shared";
#elif UNITY_EDITOR_WIN
    internal const string LibraryName = "astcenc-sse4.1-shared.dll";
#endif

    /**
     * Populate a codec config based on default settings.
     *
     * Power users can edit the returned config struct to fine tune before allocating the context.
     *
     * @param      profile   Color profile.
     * @param      block_x   ASTC block size X dimension.
     * @param      block_y   ASTC block size Y dimension.
     * @param      block_z   ASTC block size Z dimension.
     * @param      quality   Search quality preset / effort level. Either an
     *                       @c ASTCENC_PRE_* value, or a effort level between 0
     *                       and 100. Performance is not linear between 0 and 100.

     * @param      flags     A valid set of @c ASTCENC_FLG_* flag bits.
     * @param[out] config    Output config struct to populate.
     *
     * @return @c ASTCENC_SUCCESS on success, or an error if the inputs are invalid
     * either individually, or in combination.
     */
    [DllImport(LibraryName, EntryPoint = "astcenc_config_init", CallingConvention = CallingConvention.Cdecl)]
    public static extern astcenc_error astcenc_config_init(
        astcenc_profile profile, 
        uint block_x, 
        uint block_y, 
        uint block_z, 
        float quality, 
        uint flags, 
        ref astcenc_config config);

    /**
     * @brief Allocate a new codec context based on a config.
     *
     * This function allocates all of the memory resources and threads needed by the codec. This can be
     * slow, so it is recommended that contexts are reused to serially compress or decompress multiple
     * images to amortize setup cost.
     *
     * Contexts can be allocated to support only decompression using the @c ASTCENC_FLG_DECOMPRESS_ONLY
     * flag when creating the configuration. The compression functions will fail if invoked. For a
     * decompress-only library build the @c ASTCENC_FLG_DECOMPRESS_ONLY flag must be set when creating
     * any context.
     *
     * @param[in]  config         Codec config.
     * @param      thread_count   Thread count to configure for.
     * @param[out] context        Location to store an opaque context pointer.
     *
     * @return @c ASTCENC_SUCCESS on success, or an error if context creation failed.
     */
    [DllImport(LibraryName, EntryPoint = "astcenc_context_alloc", CallingConvention = CallingConvention.Cdecl)]
    public static extern astcenc_error astcenc_context_alloc(
        ref astcenc_config config, 
        uint thread_count, 
        ref IntPtr context);

    /**
     * @brief Compress an image.
     *
     * A single context can only compress or decompress a single image at a time.
     *
     * For a context configured for multi-threading, any set of the N threads can call this function.
     * Work will be dynamically scheduled across the threads available. Each thread must have a unique
     * @c thread_index.
     *
     * @param         context        Codec context.
     * @param[in,out] image          An input image, in 2D slices.
     * @param         swizzle        Compression data swizzle, applied before compression.
     * @param[out]    data_out       Pointer to output data array.
     * @param         data_len       Length of the output data array.
     * @param         thread_index   Thread index [0..N-1] of calling thread.
     *
     * @return @c ASTCENC_SUCCESS on success, or an error if compression failed.
     */
    //[DllImport(LibraryName, EntryPoint = "astcenc_compress_image", CallingConvention = CallingConvention.Cdecl)]
    //public static extern astcenc_error astcenc_compress_image(
    //    IntPtr context, 
    //    ref astcenc_image image, 
    //    ref astcenc_swizzle swizzle, 
    //    [In, Out] byte[] data_out, 
    //    [MarshalAs(UnmanagedType.SysInt)] int data_len, 
    //    uint thread_index);

    [DllImport(LibraryName, EntryPoint = "astcenc_compress_image", CallingConvention = CallingConvention.Cdecl)]
    public static extern astcenc_error astcenc_compress_image(
        IntPtr context,
        ref astcenc_image image,
        ref astcenc_swizzle swizzle,
        IntPtr data_out,
        [MarshalAs(UnmanagedType.SysInt)] int data_len,
        uint thread_index);
    /**
     * @brief Reset the codec state for a new compression.
     *
     * The caller is responsible for synchronizing threads in the worker thread pool. This function must
     * only be called when all threads have exited the @c astcenc_compress_image() function for image N,
     * but before any thread enters it for image N + 1.
     *
     * Calling this is not required (but won't hurt), if the context is created for single threaded use.
     *
     * @param context   Codec context.
     *
     * @return @c ASTCENC_SUCCESS on success, or an error if reset failed.
     */
    [DllImport(LibraryName, EntryPoint = "astcenc_compress_reset", CallingConvention = CallingConvention.Cdecl)]
    public static extern astcenc_error astcenc_compress_reset(
        IntPtr context);

    /**
     * Free the compressor context.
     *
     * @param context   The codec context.
     */
    [DllImport(LibraryName, EntryPoint = "astcenc_context_free", CallingConvention = CallingConvention.Cdecl)]
    public static extern void astcenc_context_free(
        IntPtr context);
}

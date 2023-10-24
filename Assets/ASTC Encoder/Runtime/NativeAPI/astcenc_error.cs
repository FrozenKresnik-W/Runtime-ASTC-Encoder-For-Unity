/**
 * @brief A codec API error code.
 */
public enum astcenc_error
{
	/** @brief The call was successful. */
	ASTCENC_SUCCESS = 0,
	/** @brief The call failed due to low memory, or undersized I/O buffers. */
	ASTCENC_ERR_OUT_OF_MEM,
	/** @brief The call failed due to the build using fast math. */
	ASTCENC_ERR_BAD_CPU_FLOAT,
	/** @brief The call failed due to an out-of-spec parameter. */
	ASTCENC_ERR_BAD_PARAM,
	/** @brief The call failed due to an out-of-spec block size. */
	ASTCENC_ERR_BAD_BLOCK_SIZE,
	/** @brief The call failed due to an out-of-spec color profile. */
	ASTCENC_ERR_BAD_PROFILE,
	/** @brief The call failed due to an out-of-spec quality value. */
	ASTCENC_ERR_BAD_QUALITY,
	/** @brief The call failed due to an out-of-spec component swizzle. */
	ASTCENC_ERR_BAD_SWIZZLE,
	/** @brief The call failed due to an out-of-spec flag set. */
	ASTCENC_ERR_BAD_FLAGS,
	/** @brief The call failed due to the context not supporting the operation. */
	ASTCENC_ERR_BAD_CONTEXT,
	/** @brief The call failed due to unimplemented functionality. */
	ASTCENC_ERR_NOT_IMPLEMENTED,
}
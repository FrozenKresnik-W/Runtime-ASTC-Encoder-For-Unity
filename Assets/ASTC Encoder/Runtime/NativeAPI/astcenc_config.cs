using System.Runtime.InteropServices;

/**
 * @brief The config structure.
 *
 * This structure will initially be populated by a call to astcenc_config_init, but power users may
 * modify it before calling astcenc_context_alloc. See astcenccli_toplevel_help.cpp for full user
 * documentation of the power-user settings.
 *
 * Note for any settings which are associated with a specific color component, the value in the
 * config applies to the component that exists after any compression data swizzle is applied.
 */
[StructLayout(LayoutKind.Sequential)]
public struct astcenc_config
{
    /** @brief The color profile. */
    public astcenc_profile profile;

    /** @brief The set of set flags. */
    public uint flags;

    /** @brief The ASTC block size X dimension. */
    public uint block_x;

    /** @brief The ASTC block size Y dimension. */
    public uint block_y;

    /** @brief The ASTC block size Z dimension. */
    public uint block_z;

    /** @brief The red component weight scale for error weighting (-cw). */
    public float cw_r_weight;

    /** @brief The green component weight scale for error weighting (-cw). */
    public float cw_g_weight;

    /** @brief The blue component weight scale for error weighting (-cw). */
    public float cw_b_weight;

    /** @brief The alpha component weight scale for error weighting (-cw). */
    public float cw_a_weight;

    /**
	 * @brief The radius for any alpha-weight scaling (-a).
	 *
	 * It is recommended that this is set to 1 when using FLG_USE_ALPHA_WEIGHT on a texture that
	 * will be sampled using linear texture filtering to minimize color bleed out of transparent
	 * texels that are adjacent to non-transparent texels.
	 */
    public uint a_scale_radius;

    /** @brief The RGBM scale factor for the shared multiplier (-rgbm). */
    public float rgbm_m_scale;

    /**
	 * @brief The maximum number of partitions searched (-partitioncountlimit).
	 *
	 * Valid values are between 1 and 4.
	 */
    public uint tune_partition_count_limit;

    /**
	 * @brief The maximum number of partitions searched (-2partitionindexlimit).
	 *
	 * Valid values are between 1 and 1024.
	 */
    public uint tune_2partition_index_limit;

    /**
	 * @brief The maximum number of partitions searched (-3partitionindexlimit).
	 *
	 * Valid values are between 1 and 1024.
	 */
    public uint tune_3partition_index_limit;

    /**
	 * @brief The maximum number of partitions searched (-4partitionindexlimit).
	 *
	 * Valid values are between 1 and 1024.
	 */
    public uint tune_4partition_index_limit;

    /**
	 * @brief The maximum centile for block modes searched (-blockmodelimit).
	 *
	 * Valid values are between 1 and 100.
	 */
    public uint tune_block_mode_limit;

    /**
	 * @brief The maximum iterative refinements applied (-refinementlimit).
	 *
	 * Valid values are between 1 and N; there is no technical upper limit
	 * but little benefit is expected after N=4.
	 */
    public uint tune_refinement_limit;

    /**
	 * @brief The number of trial candidates per mode search (-candidatelimit).
	 *
	 * Valid values are between 1 and TUNE_MAX_TRIAL_CANDIDATES.
	 */
    public uint tune_candidate_limit;

    /**
	 * @brief The number of trial partitionings per search (-2partitioncandidatelimit).
	 *
	 * Valid values are between 1 and TUNE_MAX_PARTITIONING_CANDIDATES.
	 */
    public uint tune_2partitioning_candidate_limit;

    /**
	 * @brief The number of trial partitionings per search (-3partitioncandidatelimit).
	 *
	 * Valid values are between 1 and TUNE_MAX_PARTITIONING_CANDIDATES.
	 */
    public uint tune_3partitioning_candidate_limit;

    /**
	 * @brief The number of trial partitionings per search (-4partitioncandidatelimit).
	 *
	 * Valid values are between 1 and TUNE_MAX_PARTITIONING_CANDIDATES.
	 */
    public uint tune_4partitioning_candidate_limit;

    /**
	 * @brief The dB threshold for stopping block search (-dblimit).
	 *
	 * This option is ineffective for HDR textures.
	 */
    public float tune_db_limit;

	/**
	 * @brief The amount of MSE overshoot needed to early-out trials.
	 *
	 * The first early-out is for 1 partition, 1 plane trials, where we try a minimal encode using
	 * the high probability block modes. This can short-cut compression for simple blocks.
	 *
	 * The second early-out is for refinement trials, where we can exit refinement once quality is
	 * reached.
	 */
	public float tune_mse_overshoot;

	/**
	 * @brief The threshold for skipping 3.1/4.1 trials (-2partitionlimitfactor).
	 *
	 * This option is further scaled for normal maps, so it skips less often.
	 */
	public float tune_2partition_early_out_limit_factor;

	/**
	 * @brief The threshold for skipping 4.1 trials (-3partitionlimitfactor).
	 *
	 * This option is further scaled for normal maps, so it skips less often.
	 */
	public float tune_3partition_early_out_limit_factor;

	/**
	 * @brief The threshold for skipping two weight planes (-2planelimitcorrelation).
	 *
	 * This option is ineffective for normal maps.
	 */
	public float tune_2plane_early_out_limit_correlation;
}
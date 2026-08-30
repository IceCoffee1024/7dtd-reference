using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace PI.NGSS
{
	// Token: 0x02001B26 RID: 6950
	[RequireComponent(typeof(Light))]
	[ExecuteInEditMode]
	public class NGSS_Directional : MonoBehaviour
	{
		// Token: 0x17001998 RID: 6552
		// (get) Token: 0x0600D098 RID: 53400 RVA: 0x004C0E4D File Offset: 0x004BF04D
		public Light DirLight
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				if (this._DirLight == null)
				{
					this._DirLight = base.GetComponent<Light>();
				}
				return this._DirLight;
			}
		}

		// Token: 0x17001999 RID: 6553
		// (get) Token: 0x0600D09A RID: 53402 RVA: 0x004C0E78 File Offset: 0x004BF078
		// (set) Token: 0x0600D099 RID: 53401 RVA: 0x004C0E6F File Offset: 0x004BF06F
		public Material mMaterial
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				if (this._mMaterial)
				{
					return this._mMaterial;
				}
				if (this.denoiserShader == null)
				{
					this.denoiserShader = Shader.Find("Hidden/NGSS_DenoiseShader");
				}
				this._mMaterial = new Material(this.denoiserShader);
				if (this._mMaterial)
				{
					return this._mMaterial;
				}
				Debug.LogWarning("NGSS Warning: can't find NGSS_DenoiseShader shader, make sure it's on your project.", this);
				base.enabled = false;
				return this._mMaterial;
			}
			[PublicizedFrom(EAccessModifier.Private)]
			set
			{
				this._mMaterial = value;
			}
		}

		// Token: 0x0600D09B RID: 53403 RVA: 0x004C0EF4 File Offset: 0x004BF0F4
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnDisable()
		{
			this.isInitialized = false;
			if (this.NGSS_KEEP_ONDISABLE)
			{
				return;
			}
			if (this.isGraphicSet)
			{
				this.isGraphicSet = false;
				GraphicsSettings.SetCustomShader(BuiltinShaderType.ScreenSpaceShadows, Shader.Find("Hidden/Internal-ScreenSpaceShadows"));
				GraphicsSettings.SetShaderMode(BuiltinShaderType.ScreenSpaceShadows, BuiltinShaderMode.UseBuiltin);
			}
			if (this.mMaterial)
			{
				global::UnityEngine.Object.DestroyImmediate(this.mMaterial);
				this.mMaterial = null;
			}
			this.RemoveCommandBuffer();
		}

		// Token: 0x0600D09C RID: 53404 RVA: 0x004C0F5C File Offset: 0x004BF15C
		[PublicizedFrom(EAccessModifier.Private)]
		public void RemoveCommandBuffer()
		{
			if (this._computeDenoiseCB == null || this._DirLight == null)
			{
				return;
			}
			foreach (CommandBuffer commandBuffer in this._DirLight.GetCommandBuffers(LightEvent.AfterScreenspaceMask))
			{
				if (!(commandBuffer.name != this._computeDenoiseCB.name))
				{
					this._DirLight.RemoveCommandBuffer(LightEvent.AfterScreenspaceMask, commandBuffer);
					break;
				}
			}
			this._computeDenoiseCB = null;
			foreach (CommandBuffer commandBuffer2 in this._DirLight.GetCommandBuffers(LightEvent.AfterScreenspaceMask))
			{
				if (!(commandBuffer2.name != this._blendDenoiseCB.name))
				{
					this._DirLight.RemoveCommandBuffer(LightEvent.AfterScreenspaceMask, commandBuffer2);
					break;
				}
			}
			this._blendDenoiseCB = null;
		}

		// Token: 0x0600D09D RID: 53405 RVA: 0x004C101A File Offset: 0x004BF21A
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnEnable()
		{
			if (this.IsNotSupported())
			{
				Debug.LogWarning("Unsupported graphics API, NGSS requires at least SM3.0 or higher and DX9 is not supported.", this);
				base.enabled = false;
				return;
			}
			this.Init();
		}

		// Token: 0x0600D09E RID: 53406 RVA: 0x004C1040 File Offset: 0x004BF240
		[PublicizedFrom(EAccessModifier.Private)]
		public void Init()
		{
			if (this.isInitialized)
			{
				return;
			}
			if (!this.isGraphicSet)
			{
				GraphicsSettings.SetShaderMode(BuiltinShaderType.ScreenSpaceShadows, BuiltinShaderMode.UseCustom);
				GraphicsSettings.SetCustomShader(BuiltinShaderType.ScreenSpaceShadows, Shader.Find("Hidden/NGSS_Directional"));
				this.isGraphicSet = true;
			}
			if (this.NGSS_NOISE_TEXTURE == null)
			{
				this.NGSS_NOISE_TEXTURE = Resources.Load<Texture2D>("BlueNoise_R8_8");
			}
			Shader.SetGlobalTexture("_BlueNoiseTextureDir", this.NGSS_NOISE_TEXTURE);
			bool flag = false;
			if (this.NGSS_DENOISER_ENABLED && !flag)
			{
				this.AddCommandBuffer();
			}
			this.isInitialized = true;
		}

		// Token: 0x0600D09F RID: 53407 RVA: 0x004C10C4 File Offset: 0x004BF2C4
		[PublicizedFrom(EAccessModifier.Private)]
		public void AddCommandBuffer()
		{
			if (this._DirLight == null)
			{
				return;
			}
			this._computeDenoiseCB = new CommandBuffer();
			this._blendDenoiseCB = new CommandBuffer();
			this._computeDenoiseCB.name = "NGSS_Directional Denoiser Computation";
			this._blendDenoiseCB.name = "NGSS_Directional Denoiser Blending";
			int num = Shader.PropertyToID("NGSS_DenoiseTexture1");
			int num2 = Shader.PropertyToID("NGSS_DenoiseTexture2");
			this._blendDenoiseCB.Blit(BuiltinRenderTextureType.None, BuiltinRenderTextureType.CurrentActive, this.mMaterial, 1);
			this._computeDenoiseCB.GetTemporaryRT(num, -1, -1, 0, FilterMode.Bilinear, RenderTextureFormat.R8, RenderTextureReadWrite.Linear);
			this._computeDenoiseCB.GetTemporaryRT(num2, -1, -1, 0, FilterMode.Bilinear, RenderTextureFormat.R8, RenderTextureReadWrite.Linear);
			for (int i = 1; i <= this.NGSS_DENOISER_PASSES; i++)
			{
				this._computeDenoiseCB.SetGlobalVector(this._denoiseKernelID, new Vector2(0f, 1f));
				if (i == 1)
				{
					this._computeDenoiseCB.Blit(BuiltinRenderTextureType.CurrentActive, num2, this.mMaterial, 0);
				}
				else
				{
					this._computeDenoiseCB.Blit(num, num2, this.mMaterial, 0);
				}
				this._computeDenoiseCB.SetGlobalVector(this._denoiseKernelID, new Vector2(1f, 0f));
				this._computeDenoiseCB.Blit(num2, num, this.mMaterial, 0);
			}
			this._computeDenoiseCB.SetGlobalTexture(this._denoiseTextureID, num);
			this._computeDenoiseCB.ReleaseTemporaryRT(num);
			this._computeDenoiseCB.ReleaseTemporaryRT(num2);
			this.mMaterial.SetFloat(this._denoiseEdgeToleranceID, this.NGSS_DENOISER_EDGE_SOFTNESS);
			this.mMaterial.SetFloat(this._denoiseSoftnessID, this.NGSS_DENOISER_SOFTNESS);
			bool flag = true;
			CommandBuffer[] array = this._DirLight.GetCommandBuffers(LightEvent.AfterScreenspaceMask);
			for (int j = 0; j < array.Length; j++)
			{
				if (!(array[j].name != this._computeDenoiseCB.name))
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				this._DirLight.AddCommandBuffer(LightEvent.AfterScreenspaceMask, this._computeDenoiseCB);
			}
			flag = true;
			array = this._DirLight.GetCommandBuffers(LightEvent.AfterScreenspaceMask);
			for (int j = 0; j < array.Length; j++)
			{
				if (!(array[j].name != this._blendDenoiseCB.name))
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				this._DirLight.AddCommandBuffer(LightEvent.AfterScreenspaceMask, this._blendDenoiseCB);
			}
		}

		// Token: 0x0600D0A0 RID: 53408 RVA: 0x004C133E File Offset: 0x004BF53E
		[PublicizedFrom(EAccessModifier.Private)]
		public bool IsNotSupported()
		{
			return SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLES2;
		}

		// Token: 0x0600D0A1 RID: 53409 RVA: 0x004C1348 File Offset: 0x004BF548
		[PublicizedFrom(EAccessModifier.Private)]
		public void Update()
		{
			if (Application.isPlaying && this.NGSS_NO_UPDATE_ON_PLAY && this.isSetup)
			{
				return;
			}
			if (this.DirLight.shadows == LightShadows.None || this.DirLight.type != LightType.Directional)
			{
				return;
			}
			Shader.SetGlobalFloat(this._ngssDirSamplingDistanceid, this.NGSS_SAMPLING_DISTANCE);
			this.NGSS_SAMPLING_TEST = Mathf.Clamp(this.NGSS_SAMPLING_TEST, 4, this.NGSS_SAMPLING_FILTER);
			Shader.SetGlobalFloat(this._ngssTestSamplersDirid, (float)this.NGSS_SAMPLING_TEST);
			Shader.SetGlobalFloat(this._ngssFilterSamplersDirid, (float)this.NGSS_SAMPLING_FILTER);
			Shader.SetGlobalFloat(this._ngssGlobalSoftnessid, (QualitySettings.shadowProjection == ShadowProjection.CloseFit) ? this.NGSS_SHADOWS_SOFTNESS : (this.NGSS_SHADOWS_SOFTNESS * 2f / (QualitySettings.shadowDistance * 0.66f) * ((QualitySettings.shadowCascades == 2) ? 1.5f : ((QualitySettings.shadowCascades == 4) ? 1f : 0.25f))));
			Shader.SetGlobalFloat(this._ngssGlobalSoftnessOptimizedid, this.NGSS_SHADOWS_SOFTNESS / QualitySettings.shadowDistance);
			int num = (int)Mathf.Sqrt((float)this.NGSS_SAMPLING_FILTER);
			Shader.SetGlobalInt(this._ngssOptimizedIterationsid, (num % 2 == 0) ? (num + 1) : num);
			Shader.SetGlobalInt(this._ngssOptimizedSamplersid, this.NGSS_SAMPLING_FILTER);
			if (this._DENOISER_ENABLED != this.NGSS_DENOISER_ENABLED)
			{
				this._DENOISER_ENABLED = this.NGSS_DENOISER_ENABLED;
				this.RemoveCommandBuffer();
				if (this.NGSS_DENOISER_ENABLED)
				{
					this.AddCommandBuffer();
				}
			}
			if (this._DENOISER_PASSES != this.NGSS_DENOISER_PASSES)
			{
				this._DENOISER_PASSES = this.NGSS_DENOISER_PASSES;
				this.RemoveCommandBuffer();
				if (this.NGSS_DENOISER_ENABLED)
				{
					this.AddCommandBuffer();
				}
			}
			if (this.NGSS_DENOISER_ENABLED)
			{
				this.mMaterial.SetFloat(this._denoiseEdgeToleranceID, this.NGSS_DENOISER_EDGE_SOFTNESS);
				this.mMaterial.SetFloat(this._denoiseSoftnessID, this.NGSS_DENOISER_SOFTNESS);
			}
			if (this.NGSS_RECEIVER_PLANE_BIAS)
			{
				Shader.EnableKeyword("NGSS_USE_RECEIVER_PLANE_BIAS");
			}
			else
			{
				Shader.DisableKeyword("NGSS_USE_RECEIVER_PLANE_BIAS");
			}
			Shader.SetGlobalFloat(this._ngssNoiseToDitheringScaleDirid, (float)this.NGSS_NOISE_TO_DITHERING_SCALE);
			if (this.NGSS_PCSS_ENABLED)
			{
				float num2 = this.NGSS_PCSS_SOFTNESS_NEAR * 0.25f;
				float num3 = this.NGSS_PCSS_SOFTNESS_FAR * 0.25f;
				Shader.SetGlobalFloat(this._ngssPcssFilterDirMinid, (num2 > num3) ? num3 : num2);
				Shader.SetGlobalFloat(this._ngssPcssFilterDirMaxid, (num3 < num2) ? num2 : num3);
				Shader.EnableKeyword("NGSS_PCSS_FILTER_DIR");
			}
			else
			{
				Shader.DisableKeyword("NGSS_PCSS_FILTER_DIR");
			}
			if (this.NGSS_SHADOWS_RESOLUTION != NGSS_Directional.ShadowMapResolution.UseQualitySettings)
			{
				this.DirLight.shadowCustomResolution = (int)this.NGSS_SHADOWS_RESOLUTION;
			}
			else
			{
				this.DirLight.shadowCustomResolution = 0;
				this.DirLight.shadowResolution = LightShadowResolution.FromQualitySettings;
			}
			if (QualitySettings.shadowCascades > 1)
			{
				Shader.SetGlobalFloat(this._ngssCascadesSoftnessNormalizationid, this.NGSS_CASCADES_SOFTNESS_NORMALIZATION);
				Shader.SetGlobalFloat(this._ngssCascadesCountid, (float)QualitySettings.shadowCascades);
				Shader.SetGlobalVector(this._ngssCascadesSplitsid, (QualitySettings.shadowCascades == 2) ? new Vector4(QualitySettings.shadowCascade2Split, 1f, 1f, 1f) : new Vector4(QualitySettings.shadowCascade4Split.x, QualitySettings.shadowCascade4Split.y, QualitySettings.shadowCascade4Split.z, 1f));
			}
			if (this.NGSS_CASCADES_BLENDING && QualitySettings.shadowCascades > 1)
			{
				Shader.EnableKeyword("NGSS_USE_CASCADE_BLENDING");
				Shader.SetGlobalFloat(this._ngssCascadeBlendDistanceid, this.NGSS_CASCADES_BLENDING_VALUE * 0.125f);
			}
			else
			{
				Shader.DisableKeyword("NGSS_USE_CASCADE_BLENDING");
			}
			this.isSetup = true;
		}

		// Token: 0x04009EEA RID: 40682
		[Header("MAIN SETTINGS")]
		public Shader denoiserShader;

		// Token: 0x04009EEB RID: 40683
		[Tooltip("If disabled, NGSS Directional shadows replacement will be removed from Graphics settings when OnDisable is called in this component.")]
		public bool NGSS_KEEP_ONDISABLE = true;

		// Token: 0x04009EEC RID: 40684
		[Tooltip("Check this option if you don't need to update shadows variables at runtime, only once when scene loads.\nUseful to save some CPU cycles.")]
		public bool NGSS_NO_UPDATE_ON_PLAY;

		// Token: 0x04009EED RID: 40685
		[Tooltip("Shadows resolution.\nUseQualitySettings = From Quality Settings, SuperLow = 512, Low = 1024, Med = 2048, High = 4096, Ultra = 8192, Mega = 16384.")]
		public NGSS_Directional.ShadowMapResolution NGSS_SHADOWS_RESOLUTION = NGSS_Directional.ShadowMapResolution.UseQualitySettings;

		// Token: 0x04009EEE RID: 40686
		[Header("BASE SAMPLING")]
		[Tooltip("Used to test blocker search and early bail out algorithms. Keep it as low as possible, might lead to white noise if too low.\nRecommended values: Mobile = 8, Consoles & VR = 16, Desktop = 24")]
		[Range(4f, 32f)]
		public int NGSS_SAMPLING_TEST = 16;

		// Token: 0x04009EEF RID: 40687
		[Tooltip("Number of samplers per pixel used for PCF and PCSS shadows algorithms.\nRecommended values: Mobile = 16, Consoles & VR = 32, Desktop Med = 48, Desktop High = 64, Desktop Ultra = 128")]
		[Range(8f, 128f)]
		public int NGSS_SAMPLING_FILTER = 48;

		// Token: 0x04009EF0 RID: 40688
		[Tooltip("New optimization that reduces sampling over distance. Interpolates current sampling set (TEST and FILTER) down to 4spp when reaching this distance.")]
		[Range(0f, 500f)]
		public float NGSS_SAMPLING_DISTANCE = 75f;

		// Token: 0x04009EF1 RID: 40689
		[Header("SHADOW SOFTNESS")]
		[Tooltip("Overall shadows softness.")]
		[Range(0f, 3f)]
		public float NGSS_SHADOWS_SOFTNESS = 1f;

		// Token: 0x04009EF2 RID: 40690
		[Header("PCSS")]
		[Tooltip("PCSS Requires inline sampling and SM3.5.\nProvides Area Light soft-shadows.\nDisable it if you are looking for PCF filtering (uniform soft-shadows) which runs with SM3.0.")]
		public bool NGSS_PCSS_ENABLED;

		// Token: 0x04009EF3 RID: 40691
		[Tooltip("How soft shadows are when close to caster.")]
		[Range(0f, 2f)]
		public float NGSS_PCSS_SOFTNESS_NEAR = 0.125f;

		// Token: 0x04009EF4 RID: 40692
		[Tooltip("How soft shadows are when far from caster.")]
		[Range(0f, 2f)]
		public float NGSS_PCSS_SOFTNESS_FAR = 1f;

		// Token: 0x04009EF5 RID: 40693
		[Header("NOISE")]
		[Tooltip("If zero = 100% noise.\nIf one = 100% dithering.\nUseful when fighting banding.")]
		[Range(0f, 1f)]
		public int NGSS_NOISE_TO_DITHERING_SCALE;

		// Token: 0x04009EF6 RID: 40694
		[Tooltip("If you set the noise scale value to something less than 1 you need to input a noise texture.\nRecommended noise textures are blue noise signals.")]
		public Texture2D NGSS_NOISE_TEXTURE;

		// Token: 0x04009EF7 RID: 40695
		[Header("DENOISER")]
		[Tooltip("Separable low pass filter that help fight artifacts and noise in shadows.\nRequires Cascaded Shadows to be enabled in the Editor Graphics Settings.")]
		public bool NGSS_DENOISER_ENABLED = true;

		// Token: 0x04009EF8 RID: 40696
		[Tooltip("How many iterations the Denoiser algorithm should do.")]
		[Range(1f, 4f)]
		public int NGSS_DENOISER_PASSES = 1;

		// Token: 0x04009EF9 RID: 40697
		[Tooltip("Overall Denoiser softness.")]
		[Range(0.01f, 1f)]
		public float NGSS_DENOISER_SOFTNESS = 1f;

		// Token: 0x04009EFA RID: 40698
		[Tooltip("The amount of shadow edges the Denoiser can tolerate during denoising.")]
		[Range(0.01f, 1f)]
		public float NGSS_DENOISER_EDGE_SOFTNESS = 1f;

		// Token: 0x04009EFB RID: 40699
		[Header("BIAS")]
		[Tooltip("This estimates receiver slope using derivatives and tries to tilt the filtering kernel along it.\nHowever, when doing it in screenspace from the depth texture can leads to shadow artifacts.\nThus it is disabled by default.")]
		public bool NGSS_RECEIVER_PLANE_BIAS;

		// Token: 0x04009EFC RID: 40700
		[Header("CASCADES")]
		[Tooltip("Blends cascades at seams intersection.\nAdditional overhead required for this option.")]
		public bool NGSS_CASCADES_BLENDING = true;

		// Token: 0x04009EFD RID: 40701
		[Tooltip("Tweak this value to adjust the blending transition between cascades.")]
		[Range(0f, 2f)]
		public float NGSS_CASCADES_BLENDING_VALUE = 1f;

		// Token: 0x04009EFE RID: 40702
		[Range(0f, 1f)]
		[Tooltip("If one, softness across cascades will be matched using splits distribution, resulting in realistic soft-ness over distance.\nIf zero the softness distribution will be based on cascade index, resulting in blurrier shadows over distance thus less realistic.")]
		public float NGSS_CASCADES_SOFTNESS_NORMALIZATION = 1f;

		// Token: 0x04009EFF RID: 40703
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _ngssDirSamplingDistanceid = Shader.PropertyToID("NGSS_DIR_SAMPLING_DISTANCE");

		// Token: 0x04009F00 RID: 40704
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _ngssTestSamplersDirid = Shader.PropertyToID("NGSS_TEST_SAMPLERS_DIR");

		// Token: 0x04009F01 RID: 40705
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _ngssFilterSamplersDirid = Shader.PropertyToID("NGSS_FILTER_SAMPLERS_DIR");

		// Token: 0x04009F02 RID: 40706
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _ngssGlobalSoftnessid = Shader.PropertyToID("NGSS_GLOBAL_SOFTNESS");

		// Token: 0x04009F03 RID: 40707
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _ngssGlobalSoftnessOptimizedid = Shader.PropertyToID("NGSS_GLOBAL_SOFTNESS_OPTIMIZED");

		// Token: 0x04009F04 RID: 40708
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _ngssOptimizedIterationsid = Shader.PropertyToID("NGSS_OPTIMIZED_ITERATIONS");

		// Token: 0x04009F05 RID: 40709
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _ngssOptimizedSamplersid = Shader.PropertyToID("NGSS_OPTIMIZED_SAMPLERS");

		// Token: 0x04009F06 RID: 40710
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _ngssNoiseToDitheringScaleDirid = Shader.PropertyToID("NGSS_NOISE_TO_DITHERING_SCALE_DIR");

		// Token: 0x04009F07 RID: 40711
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _ngssPcssFilterDirMinid = Shader.PropertyToID("NGSS_PCSS_FILTER_DIR_MIN");

		// Token: 0x04009F08 RID: 40712
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _ngssPcssFilterDirMaxid = Shader.PropertyToID("NGSS_PCSS_FILTER_DIR_MAX");

		// Token: 0x04009F09 RID: 40713
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _ngssCascadesSoftnessNormalizationid = Shader.PropertyToID("NGSS_CASCADES_SOFTNESS_NORMALIZATION");

		// Token: 0x04009F0A RID: 40714
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _ngssCascadesCountid = Shader.PropertyToID("NGSS_CASCADES_COUNT");

		// Token: 0x04009F0B RID: 40715
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _ngssCascadesSplitsid = Shader.PropertyToID("NGSS_CASCADES_SPLITS");

		// Token: 0x04009F0C RID: 40716
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _ngssCascadeBlendDistanceid = Shader.PropertyToID("NGSS_CASCADE_BLEND_DISTANCE");

		// Token: 0x04009F0D RID: 40717
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _denoiseKernelID = Shader.PropertyToID("DenoiseKernel");

		// Token: 0x04009F0E RID: 40718
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _denoiseSoftnessID = Shader.PropertyToID("DenoiseSoftness");

		// Token: 0x04009F0F RID: 40719
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _denoiseTextureID = Shader.PropertyToID("NGSS_DenoiseTexture");

		// Token: 0x04009F10 RID: 40720
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _denoiseEdgeToleranceID = Shader.PropertyToID("DenoiseEdgeTolerance");

		// Token: 0x04009F11 RID: 40721
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public CommandBuffer _computeDenoiseCB;

		// Token: 0x04009F12 RID: 40722
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public CommandBuffer _blendDenoiseCB;

		// Token: 0x04009F13 RID: 40723
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public bool _DENOISER_ENABLED;

		// Token: 0x04009F14 RID: 40724
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _DENOISER_PASSES = 1;

		// Token: 0x04009F15 RID: 40725
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public bool isSetup;

		// Token: 0x04009F16 RID: 40726
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public bool isInitialized;

		// Token: 0x04009F17 RID: 40727
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public bool isGraphicSet;

		// Token: 0x04009F18 RID: 40728
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Light _DirLight;

		// Token: 0x04009F19 RID: 40729
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Material _mMaterial;

		// Token: 0x02001B27 RID: 6951
		public enum ShadowMapResolution
		{
			// Token: 0x04009F1B RID: 40731
			UseQualitySettings = 256,
			// Token: 0x04009F1C RID: 40732
			VeryLow = 512,
			// Token: 0x04009F1D RID: 40733
			Low = 1024,
			// Token: 0x04009F1E RID: 40734
			Med = 2048,
			// Token: 0x04009F1F RID: 40735
			High = 4096,
			// Token: 0x04009F20 RID: 40736
			Ultra = 8192,
			// Token: 0x04009F21 RID: 40737
			Mega = 16384
		}
	}
}

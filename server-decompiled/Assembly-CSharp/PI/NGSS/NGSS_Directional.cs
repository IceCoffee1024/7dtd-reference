using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace PI.NGSS
{
	// Token: 0x02001B58 RID: 7000
	[RequireComponent(typeof(Light))]
	[ExecuteInEditMode]
	public class NGSS_Directional : MonoBehaviour
	{
		// Token: 0x17001A3E RID: 6718
		// (get) Token: 0x0600D20F RID: 53775 RVA: 0x004C5932 File Offset: 0x004C3B32
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

		// Token: 0x17001A3F RID: 6719
		// (get) Token: 0x0600D211 RID: 53777 RVA: 0x004C5960 File Offset: 0x004C3B60
		// (set) Token: 0x0600D210 RID: 53776 RVA: 0x004C5954 File Offset: 0x004C3B54
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

		// Token: 0x0600D212 RID: 53778 RVA: 0x004C59DC File Offset: 0x004C3BDC
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

		// Token: 0x0600D213 RID: 53779 RVA: 0x004C5A44 File Offset: 0x004C3C44
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

		// Token: 0x0600D214 RID: 53780 RVA: 0x004C5B02 File Offset: 0x004C3D02
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

		// Token: 0x0600D215 RID: 53781 RVA: 0x004C5B28 File Offset: 0x004C3D28
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

		// Token: 0x0600D216 RID: 53782 RVA: 0x004C5BAC File Offset: 0x004C3DAC
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

		// Token: 0x0600D217 RID: 53783 RVA: 0x004C5E26 File Offset: 0x004C4026
		[PublicizedFrom(EAccessModifier.Private)]
		public bool IsNotSupported()
		{
			return SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLES2;
		}

		// Token: 0x0600D218 RID: 53784 RVA: 0x004C5E30 File Offset: 0x004C4030
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

		// Token: 0x04009FBF RID: 40895
		[Header("MAIN SETTINGS")]
		public Shader denoiserShader;

		// Token: 0x04009FC0 RID: 40896
		[Tooltip("If disabled, NGSS Directional shadows replacement will be removed from Graphics settings when OnDisable is called in this component.")]
		public bool NGSS_KEEP_ONDISABLE = true;

		// Token: 0x04009FC1 RID: 40897
		[Tooltip("Check this option if you don't need to update shadows variables at runtime, only once when scene loads.\nUseful to save some CPU cycles.")]
		public bool NGSS_NO_UPDATE_ON_PLAY;

		// Token: 0x04009FC2 RID: 40898
		[Tooltip("Shadows resolution.\nUseQualitySettings = From Quality Settings, SuperLow = 512, Low = 1024, Med = 2048, High = 4096, Ultra = 8192, Mega = 16384.")]
		public NGSS_Directional.ShadowMapResolution NGSS_SHADOWS_RESOLUTION = NGSS_Directional.ShadowMapResolution.UseQualitySettings;

		// Token: 0x04009FC3 RID: 40899
		[Header("BASE SAMPLING")]
		[Tooltip("Used to test blocker search and early bail out algorithms. Keep it as low as possible, might lead to white noise if too low.\nRecommended values: Mobile = 8, Consoles & VR = 16, Desktop = 24")]
		[Range(4f, 32f)]
		public int NGSS_SAMPLING_TEST = 16;

		// Token: 0x04009FC4 RID: 40900
		[Tooltip("Number of samplers per pixel used for PCF and PCSS shadows algorithms.\nRecommended values: Mobile = 16, Consoles & VR = 32, Desktop Med = 48, Desktop High = 64, Desktop Ultra = 128")]
		[Range(8f, 128f)]
		public int NGSS_SAMPLING_FILTER = 48;

		// Token: 0x04009FC5 RID: 40901
		[Tooltip("New optimization that reduces sampling over distance. Interpolates current sampling set (TEST and FILTER) down to 4spp when reaching this distance.")]
		[Range(0f, 500f)]
		public float NGSS_SAMPLING_DISTANCE = 75f;

		// Token: 0x04009FC6 RID: 40902
		[Header("SHADOW SOFTNESS")]
		[Tooltip("Overall shadows softness.")]
		[Range(0f, 3f)]
		public float NGSS_SHADOWS_SOFTNESS = 1f;

		// Token: 0x04009FC7 RID: 40903
		[Header("PCSS")]
		[Tooltip("PCSS Requires inline sampling and SM3.5.\nProvides Area Light soft-shadows.\nDisable it if you are looking for PCF filtering (uniform soft-shadows) which runs with SM3.0.")]
		public bool NGSS_PCSS_ENABLED;

		// Token: 0x04009FC8 RID: 40904
		[Tooltip("How soft shadows are when close to caster.")]
		[Range(0f, 2f)]
		public float NGSS_PCSS_SOFTNESS_NEAR = 0.125f;

		// Token: 0x04009FC9 RID: 40905
		[Tooltip("How soft shadows are when far from caster.")]
		[Range(0f, 2f)]
		public float NGSS_PCSS_SOFTNESS_FAR = 1f;

		// Token: 0x04009FCA RID: 40906
		[Header("NOISE")]
		[Tooltip("If zero = 100% noise.\nIf one = 100% dithering.\nUseful when fighting banding.")]
		[Range(0f, 1f)]
		public int NGSS_NOISE_TO_DITHERING_SCALE;

		// Token: 0x04009FCB RID: 40907
		[Tooltip("If you set the noise scale value to something less than 1 you need to input a noise texture.\nRecommended noise textures are blue noise signals.")]
		public Texture2D NGSS_NOISE_TEXTURE;

		// Token: 0x04009FCC RID: 40908
		[Header("DENOISER")]
		[Tooltip("Separable low pass filter that help fight artifacts and noise in shadows.\nRequires Cascaded Shadows to be enabled in the Editor Graphics Settings.")]
		public bool NGSS_DENOISER_ENABLED = true;

		// Token: 0x04009FCD RID: 40909
		[Tooltip("How many iterations the Denoiser algorithm should do.")]
		[Range(1f, 4f)]
		public int NGSS_DENOISER_PASSES = 1;

		// Token: 0x04009FCE RID: 40910
		[Tooltip("Overall Denoiser softness.")]
		[Range(0.01f, 1f)]
		public float NGSS_DENOISER_SOFTNESS = 1f;

		// Token: 0x04009FCF RID: 40911
		[Tooltip("The amount of shadow edges the Denoiser can tolerate during denoising.")]
		[Range(0.01f, 1f)]
		public float NGSS_DENOISER_EDGE_SOFTNESS = 1f;

		// Token: 0x04009FD0 RID: 40912
		[Header("BIAS")]
		[Tooltip("This estimates receiver slope using derivatives and tries to tilt the filtering kernel along it.\nHowever, when doing it in screenspace from the depth texture can leads to shadow artifacts.\nThus it is disabled by default.")]
		public bool NGSS_RECEIVER_PLANE_BIAS;

		// Token: 0x04009FD1 RID: 40913
		[Header("CASCADES")]
		[Tooltip("Blends cascades at seams intersection.\nAdditional overhead required for this option.")]
		public bool NGSS_CASCADES_BLENDING = true;

		// Token: 0x04009FD2 RID: 40914
		[Tooltip("Tweak this value to adjust the blending transition between cascades.")]
		[Range(0f, 2f)]
		public float NGSS_CASCADES_BLENDING_VALUE = 1f;

		// Token: 0x04009FD3 RID: 40915
		[Range(0f, 1f)]
		[Tooltip("If one, softness across cascades will be matched using splits distribution, resulting in realistic soft-ness over distance.\nIf zero the softness distribution will be based on cascade index, resulting in blurrier shadows over distance thus less realistic.")]
		public float NGSS_CASCADES_SOFTNESS_NORMALIZATION = 1f;

		// Token: 0x04009FD4 RID: 40916
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _ngssDirSamplingDistanceid = Shader.PropertyToID("NGSS_DIR_SAMPLING_DISTANCE");

		// Token: 0x04009FD5 RID: 40917
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _ngssTestSamplersDirid = Shader.PropertyToID("NGSS_TEST_SAMPLERS_DIR");

		// Token: 0x04009FD6 RID: 40918
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _ngssFilterSamplersDirid = Shader.PropertyToID("NGSS_FILTER_SAMPLERS_DIR");

		// Token: 0x04009FD7 RID: 40919
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _ngssGlobalSoftnessid = Shader.PropertyToID("NGSS_GLOBAL_SOFTNESS");

		// Token: 0x04009FD8 RID: 40920
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _ngssGlobalSoftnessOptimizedid = Shader.PropertyToID("NGSS_GLOBAL_SOFTNESS_OPTIMIZED");

		// Token: 0x04009FD9 RID: 40921
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _ngssOptimizedIterationsid = Shader.PropertyToID("NGSS_OPTIMIZED_ITERATIONS");

		// Token: 0x04009FDA RID: 40922
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _ngssOptimizedSamplersid = Shader.PropertyToID("NGSS_OPTIMIZED_SAMPLERS");

		// Token: 0x04009FDB RID: 40923
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _ngssNoiseToDitheringScaleDirid = Shader.PropertyToID("NGSS_NOISE_TO_DITHERING_SCALE_DIR");

		// Token: 0x04009FDC RID: 40924
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _ngssPcssFilterDirMinid = Shader.PropertyToID("NGSS_PCSS_FILTER_DIR_MIN");

		// Token: 0x04009FDD RID: 40925
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _ngssPcssFilterDirMaxid = Shader.PropertyToID("NGSS_PCSS_FILTER_DIR_MAX");

		// Token: 0x04009FDE RID: 40926
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _ngssCascadesSoftnessNormalizationid = Shader.PropertyToID("NGSS_CASCADES_SOFTNESS_NORMALIZATION");

		// Token: 0x04009FDF RID: 40927
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _ngssCascadesCountid = Shader.PropertyToID("NGSS_CASCADES_COUNT");

		// Token: 0x04009FE0 RID: 40928
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _ngssCascadesSplitsid = Shader.PropertyToID("NGSS_CASCADES_SPLITS");

		// Token: 0x04009FE1 RID: 40929
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _ngssCascadeBlendDistanceid = Shader.PropertyToID("NGSS_CASCADE_BLEND_DISTANCE");

		// Token: 0x04009FE2 RID: 40930
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _denoiseKernelID = Shader.PropertyToID("DenoiseKernel");

		// Token: 0x04009FE3 RID: 40931
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _denoiseSoftnessID = Shader.PropertyToID("DenoiseSoftness");

		// Token: 0x04009FE4 RID: 40932
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _denoiseTextureID = Shader.PropertyToID("NGSS_DenoiseTexture");

		// Token: 0x04009FE5 RID: 40933
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _denoiseEdgeToleranceID = Shader.PropertyToID("DenoiseEdgeTolerance");

		// Token: 0x04009FE6 RID: 40934
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public CommandBuffer _computeDenoiseCB;

		// Token: 0x04009FE7 RID: 40935
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public CommandBuffer _blendDenoiseCB;

		// Token: 0x04009FE8 RID: 40936
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public bool _DENOISER_ENABLED;

		// Token: 0x04009FE9 RID: 40937
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _DENOISER_PASSES = 1;

		// Token: 0x04009FEA RID: 40938
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public bool isSetup;

		// Token: 0x04009FEB RID: 40939
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public bool isInitialized;

		// Token: 0x04009FEC RID: 40940
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public bool isGraphicSet;

		// Token: 0x04009FED RID: 40941
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Light _DirLight;

		// Token: 0x04009FEE RID: 40942
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Material _mMaterial;

		// Token: 0x02001B59 RID: 7001
		public enum ShadowMapResolution
		{
			// Token: 0x04009FF0 RID: 40944
			UseQualitySettings = 256,
			// Token: 0x04009FF1 RID: 40945
			VeryLow = 512,
			// Token: 0x04009FF2 RID: 40946
			Low = 1024,
			// Token: 0x04009FF3 RID: 40947
			Med = 2048,
			// Token: 0x04009FF4 RID: 40948
			High = 4096,
			// Token: 0x04009FF5 RID: 40949
			Ultra = 8192,
			// Token: 0x04009FF6 RID: 40950
			Mega = 16384
		}
	}
}

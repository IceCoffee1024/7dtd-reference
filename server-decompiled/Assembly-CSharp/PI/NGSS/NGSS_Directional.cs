using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace PI.NGSS
{
	// Token: 0x02001B89 RID: 7049
	[RequireComponent(typeof(Light))]
	[ExecuteInEditMode]
	public class NGSS_Directional : MonoBehaviour
	{
		// Token: 0x17001A86 RID: 6790
		// (get) Token: 0x0600D323 RID: 54051 RVA: 0x004C945A File Offset: 0x004C765A
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

		// Token: 0x17001A87 RID: 6791
		// (get) Token: 0x0600D325 RID: 54053 RVA: 0x004C9488 File Offset: 0x004C7688
		// (set) Token: 0x0600D324 RID: 54052 RVA: 0x004C947C File Offset: 0x004C767C
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

		// Token: 0x0600D326 RID: 54054 RVA: 0x004C9504 File Offset: 0x004C7704
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

		// Token: 0x0600D327 RID: 54055 RVA: 0x004C956C File Offset: 0x004C776C
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

		// Token: 0x0600D328 RID: 54056 RVA: 0x004C962A File Offset: 0x004C782A
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

		// Token: 0x0600D329 RID: 54057 RVA: 0x004C9650 File Offset: 0x004C7850
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

		// Token: 0x0600D32A RID: 54058 RVA: 0x004C96D4 File Offset: 0x004C78D4
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

		// Token: 0x0600D32B RID: 54059 RVA: 0x004C994E File Offset: 0x004C7B4E
		[PublicizedFrom(EAccessModifier.Private)]
		public bool IsNotSupported()
		{
			return SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLES2;
		}

		// Token: 0x0600D32C RID: 54060 RVA: 0x004C9958 File Offset: 0x004C7B58
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

		// Token: 0x0400A0A8 RID: 41128
		[Header("MAIN SETTINGS")]
		public Shader denoiserShader;

		// Token: 0x0400A0A9 RID: 41129
		[Tooltip("If disabled, NGSS Directional shadows replacement will be removed from Graphics settings when OnDisable is called in this component.")]
		public bool NGSS_KEEP_ONDISABLE = true;

		// Token: 0x0400A0AA RID: 41130
		[Tooltip("Check this option if you don't need to update shadows variables at runtime, only once when scene loads.\nUseful to save some CPU cycles.")]
		public bool NGSS_NO_UPDATE_ON_PLAY;

		// Token: 0x0400A0AB RID: 41131
		[Tooltip("Shadows resolution.\nUseQualitySettings = From Quality Settings, SuperLow = 512, Low = 1024, Med = 2048, High = 4096, Ultra = 8192, Mega = 16384.")]
		public NGSS_Directional.ShadowMapResolution NGSS_SHADOWS_RESOLUTION = NGSS_Directional.ShadowMapResolution.UseQualitySettings;

		// Token: 0x0400A0AC RID: 41132
		[Header("BASE SAMPLING")]
		[Tooltip("Used to test blocker search and early bail out algorithms. Keep it as low as possible, might lead to white noise if too low.\nRecommended values: Mobile = 8, Consoles & VR = 16, Desktop = 24")]
		[Range(4f, 32f)]
		public int NGSS_SAMPLING_TEST = 16;

		// Token: 0x0400A0AD RID: 41133
		[Tooltip("Number of samplers per pixel used for PCF and PCSS shadows algorithms.\nRecommended values: Mobile = 16, Consoles & VR = 32, Desktop Med = 48, Desktop High = 64, Desktop Ultra = 128")]
		[Range(8f, 128f)]
		public int NGSS_SAMPLING_FILTER = 48;

		// Token: 0x0400A0AE RID: 41134
		[Tooltip("New optimization that reduces sampling over distance. Interpolates current sampling set (TEST and FILTER) down to 4spp when reaching this distance.")]
		[Range(0f, 500f)]
		public float NGSS_SAMPLING_DISTANCE = 75f;

		// Token: 0x0400A0AF RID: 41135
		[Header("SHADOW SOFTNESS")]
		[Tooltip("Overall shadows softness.")]
		[Range(0f, 3f)]
		public float NGSS_SHADOWS_SOFTNESS = 1f;

		// Token: 0x0400A0B0 RID: 41136
		[Header("PCSS")]
		[Tooltip("PCSS Requires inline sampling and SM3.5.\nProvides Area Light soft-shadows.\nDisable it if you are looking for PCF filtering (uniform soft-shadows) which runs with SM3.0.")]
		public bool NGSS_PCSS_ENABLED;

		// Token: 0x0400A0B1 RID: 41137
		[Tooltip("How soft shadows are when close to caster.")]
		[Range(0f, 2f)]
		public float NGSS_PCSS_SOFTNESS_NEAR = 0.125f;

		// Token: 0x0400A0B2 RID: 41138
		[Tooltip("How soft shadows are when far from caster.")]
		[Range(0f, 2f)]
		public float NGSS_PCSS_SOFTNESS_FAR = 1f;

		// Token: 0x0400A0B3 RID: 41139
		[Header("NOISE")]
		[Tooltip("If zero = 100% noise.\nIf one = 100% dithering.\nUseful when fighting banding.")]
		[Range(0f, 1f)]
		public int NGSS_NOISE_TO_DITHERING_SCALE;

		// Token: 0x0400A0B4 RID: 41140
		[Tooltip("If you set the noise scale value to something less than 1 you need to input a noise texture.\nRecommended noise textures are blue noise signals.")]
		public Texture2D NGSS_NOISE_TEXTURE;

		// Token: 0x0400A0B5 RID: 41141
		[Header("DENOISER")]
		[Tooltip("Separable low pass filter that help fight artifacts and noise in shadows.\nRequires Cascaded Shadows to be enabled in the Editor Graphics Settings.")]
		public bool NGSS_DENOISER_ENABLED = true;

		// Token: 0x0400A0B6 RID: 41142
		[Tooltip("How many iterations the Denoiser algorithm should do.")]
		[Range(1f, 4f)]
		public int NGSS_DENOISER_PASSES = 1;

		// Token: 0x0400A0B7 RID: 41143
		[Tooltip("Overall Denoiser softness.")]
		[Range(0.01f, 1f)]
		public float NGSS_DENOISER_SOFTNESS = 1f;

		// Token: 0x0400A0B8 RID: 41144
		[Tooltip("The amount of shadow edges the Denoiser can tolerate during denoising.")]
		[Range(0.01f, 1f)]
		public float NGSS_DENOISER_EDGE_SOFTNESS = 1f;

		// Token: 0x0400A0B9 RID: 41145
		[Header("BIAS")]
		[Tooltip("This estimates receiver slope using derivatives and tries to tilt the filtering kernel along it.\nHowever, when doing it in screenspace from the depth texture can leads to shadow artifacts.\nThus it is disabled by default.")]
		public bool NGSS_RECEIVER_PLANE_BIAS;

		// Token: 0x0400A0BA RID: 41146
		[Header("CASCADES")]
		[Tooltip("Blends cascades at seams intersection.\nAdditional overhead required for this option.")]
		public bool NGSS_CASCADES_BLENDING = true;

		// Token: 0x0400A0BB RID: 41147
		[Tooltip("Tweak this value to adjust the blending transition between cascades.")]
		[Range(0f, 2f)]
		public float NGSS_CASCADES_BLENDING_VALUE = 1f;

		// Token: 0x0400A0BC RID: 41148
		[Range(0f, 1f)]
		[Tooltip("If one, softness across cascades will be matched using splits distribution, resulting in realistic soft-ness over distance.\nIf zero the softness distribution will be based on cascade index, resulting in blurrier shadows over distance thus less realistic.")]
		public float NGSS_CASCADES_SOFTNESS_NORMALIZATION = 1f;

		// Token: 0x0400A0BD RID: 41149
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _ngssDirSamplingDistanceid = Shader.PropertyToID("NGSS_DIR_SAMPLING_DISTANCE");

		// Token: 0x0400A0BE RID: 41150
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _ngssTestSamplersDirid = Shader.PropertyToID("NGSS_TEST_SAMPLERS_DIR");

		// Token: 0x0400A0BF RID: 41151
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _ngssFilterSamplersDirid = Shader.PropertyToID("NGSS_FILTER_SAMPLERS_DIR");

		// Token: 0x0400A0C0 RID: 41152
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _ngssGlobalSoftnessid = Shader.PropertyToID("NGSS_GLOBAL_SOFTNESS");

		// Token: 0x0400A0C1 RID: 41153
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _ngssGlobalSoftnessOptimizedid = Shader.PropertyToID("NGSS_GLOBAL_SOFTNESS_OPTIMIZED");

		// Token: 0x0400A0C2 RID: 41154
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _ngssOptimizedIterationsid = Shader.PropertyToID("NGSS_OPTIMIZED_ITERATIONS");

		// Token: 0x0400A0C3 RID: 41155
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _ngssOptimizedSamplersid = Shader.PropertyToID("NGSS_OPTIMIZED_SAMPLERS");

		// Token: 0x0400A0C4 RID: 41156
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _ngssNoiseToDitheringScaleDirid = Shader.PropertyToID("NGSS_NOISE_TO_DITHERING_SCALE_DIR");

		// Token: 0x0400A0C5 RID: 41157
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _ngssPcssFilterDirMinid = Shader.PropertyToID("NGSS_PCSS_FILTER_DIR_MIN");

		// Token: 0x0400A0C6 RID: 41158
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _ngssPcssFilterDirMaxid = Shader.PropertyToID("NGSS_PCSS_FILTER_DIR_MAX");

		// Token: 0x0400A0C7 RID: 41159
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _ngssCascadesSoftnessNormalizationid = Shader.PropertyToID("NGSS_CASCADES_SOFTNESS_NORMALIZATION");

		// Token: 0x0400A0C8 RID: 41160
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _ngssCascadesCountid = Shader.PropertyToID("NGSS_CASCADES_COUNT");

		// Token: 0x0400A0C9 RID: 41161
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _ngssCascadesSplitsid = Shader.PropertyToID("NGSS_CASCADES_SPLITS");

		// Token: 0x0400A0CA RID: 41162
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _ngssCascadeBlendDistanceid = Shader.PropertyToID("NGSS_CASCADE_BLEND_DISTANCE");

		// Token: 0x0400A0CB RID: 41163
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _denoiseKernelID = Shader.PropertyToID("DenoiseKernel");

		// Token: 0x0400A0CC RID: 41164
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _denoiseSoftnessID = Shader.PropertyToID("DenoiseSoftness");

		// Token: 0x0400A0CD RID: 41165
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _denoiseTextureID = Shader.PropertyToID("NGSS_DenoiseTexture");

		// Token: 0x0400A0CE RID: 41166
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _denoiseEdgeToleranceID = Shader.PropertyToID("DenoiseEdgeTolerance");

		// Token: 0x0400A0CF RID: 41167
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public CommandBuffer _computeDenoiseCB;

		// Token: 0x0400A0D0 RID: 41168
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public CommandBuffer _blendDenoiseCB;

		// Token: 0x0400A0D1 RID: 41169
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public bool _DENOISER_ENABLED;

		// Token: 0x0400A0D2 RID: 41170
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _DENOISER_PASSES = 1;

		// Token: 0x0400A0D3 RID: 41171
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public bool isSetup;

		// Token: 0x0400A0D4 RID: 41172
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public bool isInitialized;

		// Token: 0x0400A0D5 RID: 41173
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public bool isGraphicSet;

		// Token: 0x0400A0D6 RID: 41174
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Light _DirLight;

		// Token: 0x0400A0D7 RID: 41175
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Material _mMaterial;

		// Token: 0x02001B8A RID: 7050
		public enum ShadowMapResolution
		{
			// Token: 0x0400A0D9 RID: 41177
			UseQualitySettings = 256,
			// Token: 0x0400A0DA RID: 41178
			VeryLow = 512,
			// Token: 0x0400A0DB RID: 41179
			Low = 1024,
			// Token: 0x0400A0DC RID: 41180
			Med = 2048,
			// Token: 0x0400A0DD RID: 41181
			High = 4096,
			// Token: 0x0400A0DE RID: 41182
			Ultra = 8192,
			// Token: 0x0400A0DF RID: 41183
			Mega = 16384
		}
	}
}

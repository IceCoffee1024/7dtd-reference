using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace PI.NGSS
{
	// Token: 0x02001B8C RID: 7052
	[ImageEffectAllowedInSceneView]
	[ExecuteInEditMode]
	public class NGSS_FrustumShadows_7DTD : MonoBehaviour
	{
		// Token: 0x0600D33D RID: 54077 RVA: 0x004C994E File Offset: 0x004C7B4E
		[PublicizedFrom(EAccessModifier.Private)]
		public bool IsNotSupported()
		{
			return SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLES2;
		}

		// Token: 0x17001A8B RID: 6795
		// (get) Token: 0x0600D33E RID: 54078 RVA: 0x004CA804 File Offset: 0x004C8A04
		public Camera mCamera
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				if (this._mCamera == null)
				{
					this._mCamera = base.GetComponent<Camera>();
					if (this._mCamera == null)
					{
						this._mCamera = Camera.main;
					}
					if (this._mCamera == null)
					{
						Debug.LogError("NGSS Error: No MainCamera found, please provide one.", this);
						base.enabled = false;
					}
				}
				return this._mCamera;
			}
		}

		// Token: 0x17001A8C RID: 6796
		// (get) Token: 0x0600D340 RID: 54080 RVA: 0x004CA874 File Offset: 0x004C8A74
		// (set) Token: 0x0600D33F RID: 54079 RVA: 0x004CA86A File Offset: 0x004C8A6A
		public Material mMaterial
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				if (this._mMaterial == null)
				{
					if (this.frustumShadowsShader == null)
					{
						this.frustumShadowsShader = Shader.Find("Hidden/NGSS_FrustumShadows");
					}
					this._mMaterial = new Material(this.frustumShadowsShader);
					if (this._mMaterial == null)
					{
						Debug.LogWarning("NGSS Warning: can't find NGSS_FrustumShadows shader, make sure it's on your project.", this);
						base.enabled = false;
					}
				}
				return this._mMaterial;
			}
			[PublicizedFrom(EAccessModifier.Private)]
			set
			{
				this._mMaterial = value;
			}
		}

		// Token: 0x0600D341 RID: 54081 RVA: 0x004CA8E4 File Offset: 0x004C8AE4
		[PublicizedFrom(EAccessModifier.Private)]
		public void AddCommandBuffers()
		{
			if (this.computeShadowsCB == null)
			{
				this.computeShadowsCB = new CommandBuffer
				{
					name = "NGSS FrustumShadows: Compute"
				};
			}
			else
			{
				this.computeShadowsCB.Clear();
			}
			bool flag = true;
			if (this.mCamera)
			{
				CommandBuffer[] commandBuffers = this.mCamera.GetCommandBuffers((this.mCamera.actualRenderingPath == RenderingPath.DeferredShading) ? CameraEvent.BeforeLighting : CameraEvent.AfterDepthTexture);
				for (int i = 0; i < commandBuffers.Length; i++)
				{
					if (!(commandBuffers[i].name != this.computeShadowsCB.name))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					this.mCamera.AddCommandBuffer((this.mCamera.actualRenderingPath == RenderingPath.DeferredShading) ? CameraEvent.BeforeLighting : CameraEvent.AfterDepthTexture, this.computeShadowsCB);
				}
			}
		}

		// Token: 0x0600D342 RID: 54082 RVA: 0x004CA99C File Offset: 0x004C8B9C
		[PublicizedFrom(EAccessModifier.Private)]
		public void RemoveCommandBuffers()
		{
			this._mMaterial = null;
			if (this.mCamera)
			{
				this.mCamera.RemoveCommandBuffer(CameraEvent.BeforeLighting, this.computeShadowsCB);
				this.mCamera.RemoveCommandBuffer(CameraEvent.AfterDepthTexture, this.computeShadowsCB);
			}
			this._isInit = false;
		}

		// Token: 0x0600D343 RID: 54083 RVA: 0x004CA9E8 File Offset: 0x004C8BE8
		[PublicizedFrom(EAccessModifier.Private)]
		public void Init()
		{
			int scaledPixelWidth = this.mCamera.scaledPixelWidth;
			int scaledPixelHeight = this.mCamera.scaledPixelHeight;
			this.m_shadowsBlurIterations = (this.m_fastBlur ? 1 : this.m_shadowsBlurIterations);
			if (this._iterations == this.m_shadowsBlurIterations && this._downGrade == this.m_shadowsDownGrade && this._width == scaledPixelWidth && this._height == scaledPixelHeight && (this._isInit || this.mainShadowsLight == null))
			{
				return;
			}
			if (this.mCamera.actualRenderingPath == RenderingPath.VertexLit)
			{
				Debug.LogWarning("Vertex Lit Rendering Path is not supported by NGSS Contact Shadows. Please set the Rendering Path in your game camera or Graphics Settings to something else than Vertex Lit.", this);
				base.enabled = false;
				return;
			}
			if (this.mCamera.actualRenderingPath == RenderingPath.Forward)
			{
				this.mCamera.depthTextureMode |= DepthTextureMode.Depth;
			}
			this.AddCommandBuffers();
			this._width = scaledPixelWidth;
			this._height = scaledPixelHeight;
			this._downGrade = this.m_shadowsDownGrade;
			int num = Shader.PropertyToID("NGSS_ContactShadowRT1");
			int num2 = Shader.PropertyToID("NGSS_ContactShadowRT2");
			this.computeShadowsCB.GetTemporaryRT(num, scaledPixelWidth / this._downGrade, scaledPixelHeight / this._downGrade, 0, FilterMode.Bilinear, RenderTextureFormat.RG16, RenderTextureReadWrite.Linear);
			this.computeShadowsCB.GetTemporaryRT(num2, scaledPixelWidth / this._downGrade, scaledPixelHeight / this._downGrade, 0, FilterMode.Bilinear, RenderTextureFormat.RG16, RenderTextureReadWrite.Linear);
			this.computeShadowsCB.Blit(null, num, this.mMaterial, 0);
			this._iterations = this.m_shadowsBlurIterations;
			for (int i = 1; i <= this._iterations; i++)
			{
				this.computeShadowsCB.SetGlobalVector("ShadowsKernel", new Vector2(0f, (float)i));
				this.computeShadowsCB.Blit(num, num2, this.mMaterial, 1);
				this.computeShadowsCB.SetGlobalVector("ShadowsKernel", new Vector2((float)i, 0f));
				this.computeShadowsCB.Blit(num2, num, this.mMaterial, 1);
			}
			this.computeShadowsCB.SetGlobalTexture("NGSS_FrustumShadowsTexture", num);
			this.computeShadowsCB.ReleaseTemporaryRT(num);
			this.computeShadowsCB.ReleaseTemporaryRT(num2);
			this._isInit = true;
		}

		// Token: 0x0600D344 RID: 54084 RVA: 0x004CAC17 File Offset: 0x004C8E17
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

		// Token: 0x0600D345 RID: 54085 RVA: 0x004CAC3A File Offset: 0x004C8E3A
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnDisable()
		{
			Shader.SetGlobalFloat("NGSS_FRUSTUM_SHADOWS_ENABLED", 0f);
			if (this._isInit)
			{
				this.RemoveCommandBuffers();
			}
			if (this.mMaterial != null)
			{
				global::UnityEngine.Object.DestroyImmediate(this.mMaterial);
				this.mMaterial = null;
			}
		}

		// Token: 0x0600D346 RID: 54086 RVA: 0x004CAC79 File Offset: 0x004C8E79
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnApplicationQuit()
		{
			if (this._isInit)
			{
				this.RemoveCommandBuffers();
			}
		}

		// Token: 0x0600D347 RID: 54087 RVA: 0x004CAC8C File Offset: 0x004C8E8C
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnPreRender()
		{
			if (this.mainShadowsLight == null && SkyManager.SunLightT != null)
			{
				this.mainShadowsLight = SkyManager.SunLightT.GetComponent<Light>();
			}
			this.Init();
			if (!this._isInit || this.mainShadowsLight == null)
			{
				return;
			}
			if (this._currentRenderingPath != this.mCamera.actualRenderingPath)
			{
				this._currentRenderingPath = this.mCamera.actualRenderingPath;
				this.RemoveCommandBuffers();
				this.AddCommandBuffers();
			}
			Shader.SetGlobalFloat("NGSS_FRUSTUM_SHADOWS_ENABLED", 1f);
			Shader.SetGlobalFloat("NGSS_FRUSTUM_SHADOWS_OPACITY", 1f - this.mainShadowsLight.shadowStrength);
			if (this.m_Temporal)
			{
				this.m_temporalJitter = (this.m_temporalJitter + 1) % 8;
				this.mMaterial.SetFloat("TemporalJitter", (float)this.m_temporalJitter * this.m_JitterScale * 0.0002f);
			}
			else
			{
				this.mMaterial.SetFloat("TemporalJitter", 0f);
			}
			if (QualitySettings.shadowProjection == ShadowProjection.StableFit)
			{
				this.mMaterial.EnableKeyword("SHADOWS_SPLIT_SPHERES");
			}
			else
			{
				this.mMaterial.DisableKeyword("SHADOWS_SPLIT_SPHERES");
			}
			this.mMaterial.SetMatrix("WorldToView", this.mCamera.worldToCameraMatrix);
			this.mMaterial.SetVector("LightDir", this.mCamera.transform.InverseTransformDirection(-this.mainShadowsLight.transform.forward));
			this.mMaterial.SetVector("LightPosRange", new Vector4(this.mainShadowsLight.transform.position.x, this.mainShadowsLight.transform.position.y, this.mainShadowsLight.transform.position.z, this.mainShadowsLight.range * this.mainShadowsLight.range));
			this.mMaterial.SetVector("LightDirWorld", -this.mainShadowsLight.transform.forward);
			this.mMaterial.SetFloat("ShadowsEdgeTolerance", this.m_shadowsEdgeBlur);
			this.mMaterial.SetFloat("ShadowsSoftness", this.m_shadowsBlur);
			this.mMaterial.SetFloat("RayScale", this.m_rayScale);
			this.mMaterial.SetFloat("ShadowsBias", this.m_shadowsBias * 0.02f);
			this.mMaterial.SetFloat("ShadowsDistanceStart", this.m_shadowsDistanceStart - 10f);
			this.mMaterial.SetFloat("RayThickness", this.m_rayThickness);
			this.mMaterial.SetFloat("RaySamples", (float)this.m_raySamples);
			if (this.m_deferredBackfaceOptimization && this.mCamera.actualRenderingPath == RenderingPath.DeferredShading)
			{
				this.mMaterial.EnableKeyword("NGSS_DEFERRED_OPTIMIZATION");
				this.mMaterial.SetFloat("BackfaceOpacity", this.m_deferredBackfaceTranslucency);
			}
			else
			{
				this.mMaterial.DisableKeyword("NGSS_DEFERRED_OPTIMIZATION");
			}
			if (this.m_dithering)
			{
				this.mMaterial.EnableKeyword("NGSS_USE_DITHERING");
			}
			else
			{
				this.mMaterial.DisableKeyword("NGSS_USE_DITHERING");
			}
			if (this.m_fastBlur)
			{
				this.mMaterial.EnableKeyword("NGSS_FAST_BLUR");
			}
			else
			{
				this.mMaterial.DisableKeyword("NGSS_FAST_BLUR");
			}
			if (this.mainShadowsLight.type != LightType.Directional)
			{
				this.mMaterial.EnableKeyword("NGSS_USE_LOCAL_SHADOWS");
			}
			else
			{
				this.mMaterial.DisableKeyword("NGSS_USE_LOCAL_SHADOWS");
			}
			this.mMaterial.SetFloat("RayScreenScale", this.m_rayScreenScale ? 1f : 0f);
		}

		// Token: 0x0600D348 RID: 54088 RVA: 0x004CA680 File Offset: 0x004C8880
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnPostRender()
		{
			Shader.SetGlobalFloat("NGSS_FRUSTUM_SHADOWS_ENABLED", 0f);
		}

		// Token: 0x0600D349 RID: 54089 RVA: 0x004CB03F File Offset: 0x004C923F
		[PublicizedFrom(EAccessModifier.Private)]
		public void BlitXR(CommandBuffer cmd, RenderTargetIdentifier src, RenderTargetIdentifier dest, Material mat, int pass)
		{
			cmd.SetRenderTarget(dest, 0, CubemapFace.Unknown, -1);
			cmd.ClearRenderTarget(true, true, Color.clear);
			cmd.DrawMesh(this.FullScreenTriangle, Matrix4x4.identity, mat, pass);
		}

		// Token: 0x17001A8D RID: 6797
		// (get) Token: 0x0600D34A RID: 54090 RVA: 0x004CB070 File Offset: 0x004C9270
		public Mesh FullScreenTriangle
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				if (this._fullScreenTriangle)
				{
					return this._fullScreenTriangle;
				}
				this._fullScreenTriangle = new Mesh
				{
					name = "Full-Screen Triangle",
					vertices = new Vector3[]
					{
						new Vector3(-1f, -1f, 0f),
						new Vector3(-1f, 3f, 0f),
						new Vector3(3f, -1f, 0f)
					},
					triangles = new int[] { 0, 1, 2 }
				};
				this._fullScreenTriangle.UploadMeshData(true);
				return this._fullScreenTriangle;
			}
		}

		// Token: 0x0400A0FD RID: 41213
		[Header("REFERENCES")]
		public Light mainShadowsLight;

		// Token: 0x0400A0FE RID: 41214
		public Shader frustumShadowsShader;

		// Token: 0x0400A0FF RID: 41215
		[Header("SHADOWS SETTINGS")]
		[Tooltip("Poisson Noise. Randomize samples to remove repeated patterns.")]
		public bool m_dithering;

		// Token: 0x0400A100 RID: 41216
		[Tooltip("If enabled a faster separable blur will be used.\nIf disabled a slower depth aware blur will be used.")]
		public bool m_fastBlur = true;

		// Token: 0x0400A101 RID: 41217
		[Tooltip("If enabled, backfaced lit fragments will be skipped increasing performance. Requires GBuffer normals.")]
		public bool m_deferredBackfaceOptimization;

		// Token: 0x0400A102 RID: 41218
		[Range(0f, 1f)]
		[Tooltip("Set how backfaced lit fragments are shaded. Requires DeferredBackfaceOptimization to be enabled.")]
		public float m_deferredBackfaceTranslucency;

		// Token: 0x0400A103 RID: 41219
		[Tooltip("Tweak this value to remove soft-shadows leaking around edges.")]
		[Range(0.01f, 1f)]
		public float m_shadowsEdgeBlur = 0.25f;

		// Token: 0x0400A104 RID: 41220
		[Tooltip("Overall softness of the shadows.")]
		[Range(0.01f, 1f)]
		public float m_shadowsBlur = 0.5f;

		// Token: 0x0400A105 RID: 41221
		[Tooltip("Overall softness of the shadows. Higher values than 1 wont work well if FastBlur is enabled.")]
		[Range(1f, 4f)]
		public int m_shadowsBlurIterations = 1;

		// Token: 0x0400A106 RID: 41222
		[Tooltip("Rising this value will make shadows more blurry but also lower in resolution.")]
		[Range(1f, 4f)]
		public int m_shadowsDownGrade = 1;

		// Token: 0x0400A107 RID: 41223
		[Tooltip("Tweak this value if your objects display backface shadows.")]
		[Range(0f, 1f)]
		public float m_shadowsBias = 0.05f;

		// Token: 0x0400A108 RID: 41224
		[Tooltip("The distance in metters from camera where shadows start to shown.")]
		public float m_shadowsDistanceStart;

		// Token: 0x0400A109 RID: 41225
		[Header("RAY SETTINGS")]
		[Tooltip("If enabled the ray length will be scaled at screen space instead of world space. Keep it enabled for an infinite view shadows coverage. Disable it for a ContactShadows like effect. Adjust the Ray Scale property accordingly.")]
		public bool m_rayScreenScale = true;

		// Token: 0x0400A10A RID: 41226
		[Tooltip("Number of samplers between each step. The higher values produces less gaps between shadows but is more costly.")]
		[Range(16f, 128f)]
		public int m_raySamples = 64;

		// Token: 0x0400A10B RID: 41227
		[Tooltip("The higher the value, the larger the shadows ray will be.")]
		[Range(0.01f, 1f)]
		public float m_rayScale = 0.25f;

		// Token: 0x0400A10C RID: 41228
		[Tooltip("The higher the value, the ticker the shadows will look.")]
		[Range(0f, 1f)]
		public float m_rayThickness = 0.01f;

		// Token: 0x0400A10D RID: 41229
		[Header("TEMPORAL SETTINGS")]
		[Tooltip("Enable this option if you use temporal anti-aliasing in your project. Works better when Dithering is enabled.")]
		public bool m_Temporal;

		// Token: 0x0400A10E RID: 41230
		[Range(0f, 1f)]
		public float m_JitterScale = 0.5f;

		// Token: 0x0400A10F RID: 41231
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int m_temporalJitter;

		// Token: 0x0400A110 RID: 41232
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _iterations = 1;

		// Token: 0x0400A111 RID: 41233
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _downGrade = 1;

		// Token: 0x0400A112 RID: 41234
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _width;

		// Token: 0x0400A113 RID: 41235
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _height;

		// Token: 0x0400A114 RID: 41236
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public RenderingPath _currentRenderingPath;

		// Token: 0x0400A115 RID: 41237
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public CommandBuffer computeShadowsCB;

		// Token: 0x0400A116 RID: 41238
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public bool _isInit;

		// Token: 0x0400A117 RID: 41239
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Camera _mCamera;

		// Token: 0x0400A118 RID: 41240
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Material _mMaterial;

		// Token: 0x0400A119 RID: 41241
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Mesh _fullScreenTriangle;
	}
}

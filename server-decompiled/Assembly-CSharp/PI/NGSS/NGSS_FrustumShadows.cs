using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace PI.NGSS
{
	// Token: 0x02001B8B RID: 7051
	[ImageEffectAllowedInSceneView]
	[ExecuteInEditMode]
	public class NGSS_FrustumShadows : MonoBehaviour
	{
		// Token: 0x0600D32E RID: 54062 RVA: 0x004C994E File Offset: 0x004C7B4E
		[PublicizedFrom(EAccessModifier.Private)]
		public bool IsNotSupported()
		{
			return SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLES2;
		}

		// Token: 0x17001A88 RID: 6792
		// (get) Token: 0x0600D32F RID: 54063 RVA: 0x004C9E70 File Offset: 0x004C8070
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

		// Token: 0x17001A89 RID: 6793
		// (get) Token: 0x0600D331 RID: 54065 RVA: 0x004C9EE0 File Offset: 0x004C80E0
		// (set) Token: 0x0600D330 RID: 54064 RVA: 0x004C9ED6 File Offset: 0x004C80D6
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

		// Token: 0x0600D332 RID: 54066 RVA: 0x004C9F50 File Offset: 0x004C8150
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

		// Token: 0x0600D333 RID: 54067 RVA: 0x004CA008 File Offset: 0x004C8208
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

		// Token: 0x0600D334 RID: 54068 RVA: 0x004CA054 File Offset: 0x004C8254
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

		// Token: 0x0600D335 RID: 54069 RVA: 0x004CA283 File Offset: 0x004C8483
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

		// Token: 0x0600D336 RID: 54070 RVA: 0x004CA2A6 File Offset: 0x004C84A6
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

		// Token: 0x0600D337 RID: 54071 RVA: 0x004CA2E5 File Offset: 0x004C84E5
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnApplicationQuit()
		{
			if (this._isInit)
			{
				this.RemoveCommandBuffers();
			}
		}

		// Token: 0x0600D338 RID: 54072 RVA: 0x004CA2F8 File Offset: 0x004C84F8
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnPreRender()
		{
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

		// Token: 0x0600D339 RID: 54073 RVA: 0x004CA680 File Offset: 0x004C8880
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnPostRender()
		{
			Shader.SetGlobalFloat("NGSS_FRUSTUM_SHADOWS_ENABLED", 0f);
		}

		// Token: 0x0600D33A RID: 54074 RVA: 0x004CA691 File Offset: 0x004C8891
		[PublicizedFrom(EAccessModifier.Private)]
		public void BlitXR(CommandBuffer cmd, RenderTargetIdentifier src, RenderTargetIdentifier dest, Material mat, int pass)
		{
			cmd.SetRenderTarget(dest, 0, CubemapFace.Unknown, -1);
			cmd.ClearRenderTarget(true, true, Color.clear);
			cmd.DrawMesh(this.FullScreenTriangle, Matrix4x4.identity, mat, pass);
		}

		// Token: 0x17001A8A RID: 6794
		// (get) Token: 0x0600D33B RID: 54075 RVA: 0x004CA6C0 File Offset: 0x004C88C0
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

		// Token: 0x0400A0E0 RID: 41184
		[Header("REFERENCES")]
		public Light mainShadowsLight;

		// Token: 0x0400A0E1 RID: 41185
		public Shader frustumShadowsShader;

		// Token: 0x0400A0E2 RID: 41186
		[Header("SHADOWS SETTINGS")]
		[Tooltip("Poisson Noise. Randomize samples to remove repeated patterns.")]
		public bool m_dithering;

		// Token: 0x0400A0E3 RID: 41187
		[Tooltip("If enabled a faster separable blur will be used.\nIf disabled a slower depth aware blur will be used.")]
		public bool m_fastBlur = true;

		// Token: 0x0400A0E4 RID: 41188
		[Tooltip("If enabled, backfaced lit fragments will be skipped increasing performance. Requires GBuffer normals.")]
		public bool m_deferredBackfaceOptimization;

		// Token: 0x0400A0E5 RID: 41189
		[Range(0f, 1f)]
		[Tooltip("Set how backfaced lit fragments are shaded. Requires DeferredBackfaceOptimization to be enabled.")]
		public float m_deferredBackfaceTranslucency;

		// Token: 0x0400A0E6 RID: 41190
		[Tooltip("Tweak this value to remove soft-shadows leaking around edges.")]
		[Range(0.01f, 1f)]
		public float m_shadowsEdgeBlur = 0.25f;

		// Token: 0x0400A0E7 RID: 41191
		[Tooltip("Overall softness of the shadows.")]
		[Range(0.01f, 1f)]
		public float m_shadowsBlur = 0.5f;

		// Token: 0x0400A0E8 RID: 41192
		[Tooltip("Overall softness of the shadows. Higher values than 1 wont work well if FastBlur is enabled.")]
		[Range(1f, 4f)]
		public int m_shadowsBlurIterations = 1;

		// Token: 0x0400A0E9 RID: 41193
		[Tooltip("Rising this value will make shadows more blurry but also lower in resolution.")]
		[Range(1f, 4f)]
		public int m_shadowsDownGrade = 1;

		// Token: 0x0400A0EA RID: 41194
		[Tooltip("Tweak this value if your objects display backface shadows.")]
		[Range(0f, 1f)]
		public float m_shadowsBias = 0.05f;

		// Token: 0x0400A0EB RID: 41195
		[Tooltip("The distance in metters from camera where shadows start to shown.")]
		public float m_shadowsDistanceStart;

		// Token: 0x0400A0EC RID: 41196
		[Header("RAY SETTINGS")]
		[Tooltip("If enabled the ray length will be scaled at screen space instead of world space. Keep it enabled for an infinite view shadows coverage. Disable it for a ContactShadows like effect. Adjust the Ray Scale property accordingly.")]
		public bool m_rayScreenScale = true;

		// Token: 0x0400A0ED RID: 41197
		[Tooltip("Number of samplers between each step. The higher values produces less gaps between shadows but is more costly.")]
		[Range(16f, 128f)]
		public int m_raySamples = 64;

		// Token: 0x0400A0EE RID: 41198
		[Tooltip("The higher the value, the larger the shadows ray will be.")]
		[Range(0.01f, 1f)]
		public float m_rayScale = 0.25f;

		// Token: 0x0400A0EF RID: 41199
		[Tooltip("The higher the value, the ticker the shadows will look.")]
		[Range(0f, 1f)]
		public float m_rayThickness = 0.01f;

		// Token: 0x0400A0F0 RID: 41200
		[Header("TEMPORAL SETTINGS")]
		[Tooltip("Enable this option if you use temporal anti-aliasing in your project. Works better when Dithering is enabled.")]
		public bool m_Temporal;

		// Token: 0x0400A0F1 RID: 41201
		[Range(0f, 1f)]
		public float m_JitterScale = 0.5f;

		// Token: 0x0400A0F2 RID: 41202
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int m_temporalJitter;

		// Token: 0x0400A0F3 RID: 41203
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _iterations = 1;

		// Token: 0x0400A0F4 RID: 41204
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _downGrade = 1;

		// Token: 0x0400A0F5 RID: 41205
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _width;

		// Token: 0x0400A0F6 RID: 41206
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _height;

		// Token: 0x0400A0F7 RID: 41207
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public RenderingPath _currentRenderingPath;

		// Token: 0x0400A0F8 RID: 41208
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public CommandBuffer computeShadowsCB;

		// Token: 0x0400A0F9 RID: 41209
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public bool _isInit;

		// Token: 0x0400A0FA RID: 41210
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Camera _mCamera;

		// Token: 0x0400A0FB RID: 41211
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Material _mMaterial;

		// Token: 0x0400A0FC RID: 41212
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Mesh _fullScreenTriangle;
	}
}

using System;
using System.Collections.Generic;
using UniLinq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

// Token: 0x02001386 RID: 4998
public class ScreenEffects : MonoBehaviour
{
	// Token: 0x170012D6 RID: 4822
	// (get) Token: 0x06009D22 RID: 40226 RVA: 0x003B605B File Offset: 0x003B425B
	// (set) Token: 0x06009D23 RID: 40227 RVA: 0x003B6062 File Offset: 0x003B4262
	public static ScreenEffects Instance
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06009D24 RID: 40228 RVA: 0x003B606A File Offset: 0x003B426A
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.Init();
		this.cameraRef = base.GetComponent<Camera>();
	}

	// Token: 0x06009D25 RID: 40229 RVA: 0x003B607E File Offset: 0x003B427E
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.prevViewProjMatrix = GL.GetGPUProjectionMatrix(this.cameraRef.nonJitteredProjectionMatrix, false) * this.cameraRef.worldToCameraMatrix;
	}

	// Token: 0x06009D26 RID: 40230 RVA: 0x003B60A8 File Offset: 0x003B42A8
	[PublicizedFrom(EAccessModifier.Private)]
	public void Init()
	{
		if (ScreenEffects.Instance != null)
		{
			Log.Warning("ScreenEffects instance already exists!");
		}
		else
		{
			ScreenEffects.Instance = this;
		}
		Material[] array = Resources.LoadAll<Material>("ScreenEffects/");
		for (int i = 0; i < array.Length; i++)
		{
			string name = array[i].name;
			this.loadedEffects.Add(new ScreenEffects.ScreenEffect
			{
				Name = name,
				Material = global::UnityEngine.Object.Instantiate<Material>(array[i]),
				TargetIntensity = 0f,
				Intensity = 0f
			});
		}
		this.SortEffects();
		this.particles = new NativeArray<ParticleSystem.Particle>(64, Allocator.Persistent, NativeArrayOptions.ClearMemory);
	}

	// Token: 0x06009D27 RID: 40231 RVA: 0x003B6148 File Offset: 0x003B4348
	public void ResetEffects()
	{
		Material[] array = Resources.LoadAll<Material>("ScreenEffects/");
		for (int i = 0; i < array.Length; i++)
		{
			string name = array[i].name;
			for (int j = 0; j < this.loadedEffects.Count; j++)
			{
				ScreenEffects.ScreenEffect screenEffect = this.loadedEffects[j];
				if (screenEffect.Name == name)
				{
					if (screenEffect.Material != null)
					{
						global::UnityEngine.Object.Destroy(screenEffect.Material);
					}
					screenEffect.Material = global::UnityEngine.Object.Instantiate<Material>(array[i]);
				}
			}
		}
		this.SortEffects();
	}

	// Token: 0x06009D28 RID: 40232 RVA: 0x003B61DA File Offset: 0x003B43DA
	[PublicizedFrom(EAccessModifier.Private)]
	public void SortEffects()
	{
		this.loadedEffects = this.loadedEffects.OrderBy<ScreenEffects.ScreenEffect, int>((ScreenEffects.ScreenEffect se) => se.Material.renderQueue).ToList<ScreenEffects.ScreenEffect>();
	}

	// Token: 0x06009D29 RID: 40233 RVA: 0x003B6214 File Offset: 0x003B4414
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDestroy()
	{
		for (int i = 0; i < this.loadedEffects.Count; i++)
		{
			global::UnityEngine.Object.Destroy(this.loadedEffects[i].Material);
		}
		this.particles.Dispose();
		if (ScreenEffects.Instance == this)
		{
			ScreenEffects.Instance = null;
		}
	}

	// Token: 0x06009D2A RID: 40234 RVA: 0x003B626C File Offset: 0x003B446C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPreRender()
	{
		Matrix4x4 matrix4x = GL.GetGPUProjectionMatrix(this.cameraRef.nonJitteredProjectionMatrix, false) * this.cameraRef.worldToCameraMatrix;
		Shader.SetGlobalMatrix("_PrevViewProjMatrix", this.prevViewProjMatrix);
		Shader.SetGlobalMatrix("_CurrViewProjMatrix", matrix4x);
		Shader.SetGlobalMatrix("_CurrViewProjMatrix_Inverse", matrix4x.inverse);
		this.prevViewProjMatrix = matrix4x;
	}

	// Token: 0x06009D2B RID: 40235 RVA: 0x003B62D0 File Offset: 0x003B44D0
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		World world = GameManager.Instance.World;
		if (world == null)
		{
			return;
		}
		int i = 0;
		while (i < this.activeEffects.Count)
		{
			ScreenEffects.ScreenEffect screenEffect = this.activeEffects[i];
			if (screenEffect == null)
			{
				this.activeEffects.RemoveAt(i);
			}
			else
			{
				float intensity = screenEffect.Intensity;
				if (screenEffect.FadeTime <= 0f)
				{
					screenEffect.Intensity = screenEffect.TargetIntensity;
				}
				else if (screenEffect.TargetIntensity > screenEffect.Intensity)
				{
					screenEffect.Intensity = Utils.FastMin(screenEffect.Intensity + Time.deltaTime / screenEffect.FadeTime, screenEffect.TargetIntensity);
				}
				else if (screenEffect.TargetIntensity < screenEffect.Intensity)
				{
					screenEffect.Intensity = Utils.FastMax(screenEffect.Intensity - Time.deltaTime / screenEffect.FadeTime, screenEffect.TargetIntensity);
				}
				if (screenEffect.Intensity != intensity)
				{
					if (screenEffect.Name == "NightVision")
					{
						world.m_WorldEnvironment.SetNightVision(screenEffect.Intensity);
					}
					if (screenEffect.particleSystems != null)
					{
						for (int j = 0; j < screenEffect.particleSystems.Length; j++)
						{
							ParticleSystem particleSystem = screenEffect.particleSystems[j];
							int num = particleSystem.GetParticles(this.particles);
							for (int k = 0; k < num; k++)
							{
								ParticleSystem.Particle particle = this.particles[k];
								Color32 startColor = particle.startColor;
								startColor.a = (byte)(screenEffect.Intensity * 255f);
								particle.startColor = startColor;
								this.particles[k] = particle;
							}
							particleSystem.SetParticles(this.particles, num);
						}
					}
				}
				if (screenEffect.Intensity == screenEffect.TargetIntensity && screenEffect.Intensity <= 0f)
				{
					this.activeEffects.RemoveAt(i);
					if (screenEffect.particlePrefab)
					{
						global::UnityEngine.Object.Destroy(screenEffect.particlePrefab);
						screenEffect.particlePrefab = null;
					}
				}
				else
				{
					i++;
				}
			}
		}
	}

	// Token: 0x06009D2C RID: 40236 RVA: 0x003B64D0 File Offset: 0x003B46D0
	public void SetScreenEffect(string _name, float _intensity = 1f, float _fadeTime = 4f)
	{
		ScreenEffects.ScreenEffect screenEffect = this.Find(_name, this.activeEffects);
		if (screenEffect == null)
		{
			if (_intensity <= 0f)
			{
				return;
			}
			screenEffect = this.Find(_name, this.loadedEffects);
			if (screenEffect == null)
			{
				return;
			}
			int renderQueue = screenEffect.Material.renderQueue;
			int num = this.activeEffects.Count;
			for (int i = 0; i < this.activeEffects.Count; i++)
			{
				ScreenEffects.ScreenEffect screenEffect2 = this.activeEffects[i];
				if (screenEffect2 != null && renderQueue <= screenEffect2.Material.renderQueue)
				{
					num = i;
					break;
				}
			}
			this.activeEffects.Insert(num, screenEffect);
			GameObject gameObject = Resources.Load<GameObject>("ScreenEffects/" + _name);
			if (gameObject)
			{
				gameObject = global::UnityEngine.Object.Instantiate<GameObject>(gameObject);
				screenEffect.particlePrefab = gameObject;
				EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
				if (primaryPlayer)
				{
					Transform transform = gameObject.transform;
					transform.SetParent(primaryPlayer.cameraTransform, false);
					transform.SetLocalPositionAndRotation(new Vector3(0f, 0f, 0.12f), Quaternion.identity);
				}
				gameObject.GetComponentsInChildren<ParticleSystem>(this.particleSystems);
				if (this.particleSystems.Count > 0)
				{
					screenEffect.particleSystems = this.particleSystems.ToArray();
					this.particleSystems.Clear();
				}
			}
		}
		screenEffect.TargetIntensity = _intensity;
		screenEffect.FadeTime = _fadeTime;
	}

	// Token: 0x06009D2D RID: 40237 RVA: 0x003B662C File Offset: 0x003B482C
	public void DisableScreenEffects()
	{
		for (int i = 0; i < this.activeEffects.Count; i++)
		{
			this.DisableScreenEffect(this.activeEffects[i].Name);
		}
	}

	// Token: 0x06009D2E RID: 40238 RVA: 0x003B6666 File Offset: 0x003B4866
	public void DisableScreenEffect(string _name)
	{
		this.SetScreenEffect(_name, 0f, 0f);
	}

	// Token: 0x06009D2F RID: 40239 RVA: 0x003B667C File Offset: 0x003B487C
	[PublicizedFrom(EAccessModifier.Private)]
	public ScreenEffects.ScreenEffect Find(string _name, List<ScreenEffects.ScreenEffect> _list)
	{
		for (int i = 0; i < _list.Count; i++)
		{
			ScreenEffects.ScreenEffect screenEffect = _list[i];
			if (screenEffect != null && screenEffect.Name == _name)
			{
				return screenEffect;
			}
		}
		return null;
	}

	// Token: 0x040076BF RID: 30399
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cDefaultFadeTime = 4f;

	// Token: 0x040076C0 RID: 30400
	public List<ScreenEffects.ScreenEffect> loadedEffects = new List<ScreenEffects.ScreenEffect>();

	// Token: 0x040076C1 RID: 30401
	public List<ScreenEffects.ScreenEffect> activeEffects = new List<ScreenEffects.ScreenEffect>();

	// Token: 0x040076C3 RID: 30403
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<ParticleSystem> particleSystems = new List<ParticleSystem>();

	// Token: 0x040076C4 RID: 30404
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public NativeArray<ParticleSystem.Particle> particles;

	// Token: 0x040076C5 RID: 30405
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Matrix4x4 prevViewProjMatrix;

	// Token: 0x040076C6 RID: 30406
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Camera cameraRef;

	// Token: 0x02001387 RID: 4999
	public class ScreenEffect : PostProcessEffectSubRenderer
	{
		// Token: 0x06009D31 RID: 40241 RVA: 0x003B66DF File Offset: 0x003B48DF
		public override void Render(PostProcessRenderContext context)
		{
			this.Material.SetFloat("Intensity", Mathf.Clamp01(this.Intensity));
			context.command.Blit(context.source, context.destination, this.Material);
		}

		// Token: 0x040076C7 RID: 30407
		public string Name;

		// Token: 0x040076C8 RID: 30408
		public Material Material;

		// Token: 0x040076C9 RID: 30409
		public float Intensity;

		// Token: 0x040076CA RID: 30410
		public float TargetIntensity;

		// Token: 0x040076CB RID: 30411
		public float FadeTime;

		// Token: 0x040076CC RID: 30412
		public GameObject particlePrefab;

		// Token: 0x040076CD RID: 30413
		public ParticleSystem[] particleSystems;
	}
}

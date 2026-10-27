using System;
using System.Collections.Generic;
using UniLinq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

// Token: 0x02001398 RID: 5016
public class ScreenEffects : MonoBehaviour
{
	// Token: 0x170012E2 RID: 4834
	// (get) Token: 0x06009D7A RID: 40314 RVA: 0x003B845F File Offset: 0x003B665F
	// (set) Token: 0x06009D7B RID: 40315 RVA: 0x003B8466 File Offset: 0x003B6666
	public static ScreenEffects Instance
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06009D7C RID: 40316 RVA: 0x003B846E File Offset: 0x003B666E
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.Init();
		this.cameraRef = base.GetComponent<Camera>();
	}

	// Token: 0x06009D7D RID: 40317 RVA: 0x003B8482 File Offset: 0x003B6682
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.prevViewProjMatrix = GL.GetGPUProjectionMatrix(this.cameraRef.nonJitteredProjectionMatrix, false) * this.cameraRef.worldToCameraMatrix;
	}

	// Token: 0x06009D7E RID: 40318 RVA: 0x003B84AC File Offset: 0x003B66AC
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

	// Token: 0x06009D7F RID: 40319 RVA: 0x003B854C File Offset: 0x003B674C
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

	// Token: 0x06009D80 RID: 40320 RVA: 0x003B85DE File Offset: 0x003B67DE
	[PublicizedFrom(EAccessModifier.Private)]
	public void SortEffects()
	{
		this.loadedEffects = this.loadedEffects.OrderBy<ScreenEffects.ScreenEffect, int>((ScreenEffects.ScreenEffect se) => se.Material.renderQueue).ToList<ScreenEffects.ScreenEffect>();
	}

	// Token: 0x06009D81 RID: 40321 RVA: 0x003B8618 File Offset: 0x003B6818
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

	// Token: 0x06009D82 RID: 40322 RVA: 0x003B8670 File Offset: 0x003B6870
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPreRender()
	{
		Matrix4x4 matrix4x = GL.GetGPUProjectionMatrix(this.cameraRef.nonJitteredProjectionMatrix, false) * this.cameraRef.worldToCameraMatrix;
		Shader.SetGlobalMatrix("_PrevViewProjMatrix", this.prevViewProjMatrix);
		Shader.SetGlobalMatrix("_CurrViewProjMatrix", matrix4x);
		Shader.SetGlobalMatrix("_CurrViewProjMatrix_Inverse", matrix4x.inverse);
		this.prevViewProjMatrix = matrix4x;
	}

	// Token: 0x06009D83 RID: 40323 RVA: 0x003B86D4 File Offset: 0x003B68D4
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

	// Token: 0x06009D84 RID: 40324 RVA: 0x003B88D4 File Offset: 0x003B6AD4
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

	// Token: 0x06009D85 RID: 40325 RVA: 0x003B8A30 File Offset: 0x003B6C30
	public void DisableScreenEffects()
	{
		for (int i = 0; i < this.activeEffects.Count; i++)
		{
			this.DisableScreenEffect(this.activeEffects[i].Name);
		}
	}

	// Token: 0x06009D86 RID: 40326 RVA: 0x003B8A6A File Offset: 0x003B6C6A
	public void DisableScreenEffect(string _name)
	{
		this.SetScreenEffect(_name, 0f, 0f);
	}

	// Token: 0x06009D87 RID: 40327 RVA: 0x003B8A80 File Offset: 0x003B6C80
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

	// Token: 0x04007700 RID: 30464
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cDefaultFadeTime = 4f;

	// Token: 0x04007701 RID: 30465
	public List<ScreenEffects.ScreenEffect> loadedEffects = new List<ScreenEffects.ScreenEffect>();

	// Token: 0x04007702 RID: 30466
	public List<ScreenEffects.ScreenEffect> activeEffects = new List<ScreenEffects.ScreenEffect>();

	// Token: 0x04007704 RID: 30468
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<ParticleSystem> particleSystems = new List<ParticleSystem>();

	// Token: 0x04007705 RID: 30469
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public NativeArray<ParticleSystem.Particle> particles;

	// Token: 0x04007706 RID: 30470
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Matrix4x4 prevViewProjMatrix;

	// Token: 0x04007707 RID: 30471
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Camera cameraRef;

	// Token: 0x02001399 RID: 5017
	public class ScreenEffect : PostProcessEffectSubRenderer
	{
		// Token: 0x06009D89 RID: 40329 RVA: 0x003B8AE3 File Offset: 0x003B6CE3
		public override void Render(PostProcessRenderContext context)
		{
			this.Material.SetFloat("Intensity", Mathf.Clamp01(this.Intensity));
			context.command.Blit(context.source, context.destination, this.Material);
		}

		// Token: 0x04007708 RID: 30472
		public string Name;

		// Token: 0x04007709 RID: 30473
		public Material Material;

		// Token: 0x0400770A RID: 30474
		public float Intensity;

		// Token: 0x0400770B RID: 30475
		public float TargetIntensity;

		// Token: 0x0400770C RID: 30476
		public float FadeTime;

		// Token: 0x0400770D RID: 30477
		public GameObject particlePrefab;

		// Token: 0x0400770E RID: 30478
		public ParticleSystem[] particleSystems;
	}
}

using System;
using System.Collections.Generic;
using UniLinq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

// Token: 0x0200134C RID: 4940
public class ScreenEffects : MonoBehaviour
{
	// Token: 0x17001258 RID: 4696
	// (get) Token: 0x06009B7B RID: 39803 RVA: 0x003AC237 File Offset: 0x003AA437
	// (set) Token: 0x06009B7C RID: 39804 RVA: 0x003AC23E File Offset: 0x003AA43E
	public static ScreenEffects Instance
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06009B7D RID: 39805 RVA: 0x003AC246 File Offset: 0x003AA446
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.Init();
		this.cameraRef = base.GetComponent<Camera>();
	}

	// Token: 0x06009B7E RID: 39806 RVA: 0x003AC25A File Offset: 0x003AA45A
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.prevViewProjMatrix = GL.GetGPUProjectionMatrix(this.cameraRef.nonJitteredProjectionMatrix, false) * this.cameraRef.worldToCameraMatrix;
	}

	// Token: 0x06009B7F RID: 39807 RVA: 0x003AC284 File Offset: 0x003AA484
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

	// Token: 0x06009B80 RID: 39808 RVA: 0x003AC324 File Offset: 0x003AA524
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

	// Token: 0x06009B81 RID: 39809 RVA: 0x003AC3B6 File Offset: 0x003AA5B6
	[PublicizedFrom(EAccessModifier.Private)]
	public void SortEffects()
	{
		this.loadedEffects = this.loadedEffects.OrderBy<ScreenEffects.ScreenEffect, int>((ScreenEffects.ScreenEffect se) => se.Material.renderQueue).ToList<ScreenEffects.ScreenEffect>();
	}

	// Token: 0x06009B82 RID: 39810 RVA: 0x003AC3F0 File Offset: 0x003AA5F0
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

	// Token: 0x06009B83 RID: 39811 RVA: 0x003AC448 File Offset: 0x003AA648
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPreRender()
	{
		Matrix4x4 matrix4x = GL.GetGPUProjectionMatrix(this.cameraRef.nonJitteredProjectionMatrix, false) * this.cameraRef.worldToCameraMatrix;
		Shader.SetGlobalMatrix("_PrevViewProjMatrix", this.prevViewProjMatrix);
		Shader.SetGlobalMatrix("_CurrViewProjMatrix", matrix4x);
		Shader.SetGlobalMatrix("_CurrViewProjMatrix_Inverse", matrix4x.inverse);
		this.prevViewProjMatrix = matrix4x;
	}

	// Token: 0x06009B84 RID: 39812 RVA: 0x003AC4AC File Offset: 0x003AA6AC
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

	// Token: 0x06009B85 RID: 39813 RVA: 0x003AC6AC File Offset: 0x003AA8AC
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

	// Token: 0x06009B86 RID: 39814 RVA: 0x003AC808 File Offset: 0x003AAA08
	public void DisableScreenEffects()
	{
		for (int i = 0; i < this.activeEffects.Count; i++)
		{
			this.DisableScreenEffect(this.activeEffects[i].Name);
		}
	}

	// Token: 0x06009B87 RID: 39815 RVA: 0x003AC842 File Offset: 0x003AAA42
	public void DisableScreenEffect(string _name)
	{
		this.SetScreenEffect(_name, 0f, 0f);
	}

	// Token: 0x06009B88 RID: 39816 RVA: 0x003AC858 File Offset: 0x003AAA58
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

	// Token: 0x0400756C RID: 30060
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cDefaultFadeTime = 4f;

	// Token: 0x0400756D RID: 30061
	public List<ScreenEffects.ScreenEffect> loadedEffects = new List<ScreenEffects.ScreenEffect>();

	// Token: 0x0400756E RID: 30062
	public List<ScreenEffects.ScreenEffect> activeEffects = new List<ScreenEffects.ScreenEffect>();

	// Token: 0x04007570 RID: 30064
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<ParticleSystem> particleSystems = new List<ParticleSystem>();

	// Token: 0x04007571 RID: 30065
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public NativeArray<ParticleSystem.Particle> particles;

	// Token: 0x04007572 RID: 30066
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Matrix4x4 prevViewProjMatrix;

	// Token: 0x04007573 RID: 30067
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Camera cameraRef;

	// Token: 0x0200134D RID: 4941
	public class ScreenEffect : PostProcessEffectSubRenderer
	{
		// Token: 0x06009B8A RID: 39818 RVA: 0x003AC8BB File Offset: 0x003AAABB
		public override void Render(PostProcessRenderContext context)
		{
			this.Material.SetFloat("Intensity", Mathf.Clamp01(this.Intensity));
			context.command.Blit(context.source, context.destination, this.Material);
		}

		// Token: 0x04007574 RID: 30068
		public string Name;

		// Token: 0x04007575 RID: 30069
		public Material Material;

		// Token: 0x04007576 RID: 30070
		public float Intensity;

		// Token: 0x04007577 RID: 30071
		public float TargetIntensity;

		// Token: 0x04007578 RID: 30072
		public float FadeTime;

		// Token: 0x04007579 RID: 30073
		public GameObject particlePrefab;

		// Token: 0x0400757A RID: 30074
		public ParticleSystem[] particleSystems;
	}
}

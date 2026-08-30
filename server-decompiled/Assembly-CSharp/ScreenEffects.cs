using System;
using System.Collections.Generic;
using UniLinq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

// Token: 0x02001366 RID: 4966
public class ScreenEffects : MonoBehaviour
{
	// Token: 0x17001291 RID: 4753
	// (get) Token: 0x06009C76 RID: 40054 RVA: 0x003B14DF File Offset: 0x003AF6DF
	// (set) Token: 0x06009C77 RID: 40055 RVA: 0x003B14E6 File Offset: 0x003AF6E6
	public static ScreenEffects Instance
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06009C78 RID: 40056 RVA: 0x003B14EE File Offset: 0x003AF6EE
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.Init();
		this.cameraRef = base.GetComponent<Camera>();
	}

	// Token: 0x06009C79 RID: 40057 RVA: 0x003B1502 File Offset: 0x003AF702
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.prevViewProjMatrix = GL.GetGPUProjectionMatrix(this.cameraRef.nonJitteredProjectionMatrix, false) * this.cameraRef.worldToCameraMatrix;
	}

	// Token: 0x06009C7A RID: 40058 RVA: 0x003B152C File Offset: 0x003AF72C
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

	// Token: 0x06009C7B RID: 40059 RVA: 0x003B15CC File Offset: 0x003AF7CC
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

	// Token: 0x06009C7C RID: 40060 RVA: 0x003B165E File Offset: 0x003AF85E
	[PublicizedFrom(EAccessModifier.Private)]
	public void SortEffects()
	{
		this.loadedEffects = this.loadedEffects.OrderBy<ScreenEffects.ScreenEffect, int>((ScreenEffects.ScreenEffect se) => se.Material.renderQueue).ToList<ScreenEffects.ScreenEffect>();
	}

	// Token: 0x06009C7D RID: 40061 RVA: 0x003B1698 File Offset: 0x003AF898
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

	// Token: 0x06009C7E RID: 40062 RVA: 0x003B16F0 File Offset: 0x003AF8F0
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPreRender()
	{
		Matrix4x4 matrix4x = GL.GetGPUProjectionMatrix(this.cameraRef.nonJitteredProjectionMatrix, false) * this.cameraRef.worldToCameraMatrix;
		Shader.SetGlobalMatrix("_PrevViewProjMatrix", this.prevViewProjMatrix);
		Shader.SetGlobalMatrix("_CurrViewProjMatrix", matrix4x);
		Shader.SetGlobalMatrix("_CurrViewProjMatrix_Inverse", matrix4x.inverse);
		this.prevViewProjMatrix = matrix4x;
	}

	// Token: 0x06009C7F RID: 40063 RVA: 0x003B1754 File Offset: 0x003AF954
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

	// Token: 0x06009C80 RID: 40064 RVA: 0x003B1954 File Offset: 0x003AFB54
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

	// Token: 0x06009C81 RID: 40065 RVA: 0x003B1AB0 File Offset: 0x003AFCB0
	public void DisableScreenEffects()
	{
		for (int i = 0; i < this.activeEffects.Count; i++)
		{
			this.DisableScreenEffect(this.activeEffects[i].Name);
		}
	}

	// Token: 0x06009C82 RID: 40066 RVA: 0x003B1AEA File Offset: 0x003AFCEA
	public void DisableScreenEffect(string _name)
	{
		this.SetScreenEffect(_name, 0f, 0f);
	}

	// Token: 0x06009C83 RID: 40067 RVA: 0x003B1B00 File Offset: 0x003AFD00
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

	// Token: 0x04007644 RID: 30276
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cDefaultFadeTime = 4f;

	// Token: 0x04007645 RID: 30277
	public List<ScreenEffects.ScreenEffect> loadedEffects = new List<ScreenEffects.ScreenEffect>();

	// Token: 0x04007646 RID: 30278
	public List<ScreenEffects.ScreenEffect> activeEffects = new List<ScreenEffects.ScreenEffect>();

	// Token: 0x04007648 RID: 30280
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<ParticleSystem> particleSystems = new List<ParticleSystem>();

	// Token: 0x04007649 RID: 30281
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public NativeArray<ParticleSystem.Particle> particles;

	// Token: 0x0400764A RID: 30282
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Matrix4x4 prevViewProjMatrix;

	// Token: 0x0400764B RID: 30283
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Camera cameraRef;

	// Token: 0x02001367 RID: 4967
	public class ScreenEffect : PostProcessEffectSubRenderer
	{
		// Token: 0x06009C85 RID: 40069 RVA: 0x003B1B63 File Offset: 0x003AFD63
		public override void Render(PostProcessRenderContext context)
		{
			this.Material.SetFloat("Intensity", Mathf.Clamp01(this.Intensity));
			context.command.Blit(context.source, context.destination, this.Material);
		}

		// Token: 0x0400764C RID: 30284
		public string Name;

		// Token: 0x0400764D RID: 30285
		public Material Material;

		// Token: 0x0400764E RID: 30286
		public float Intensity;

		// Token: 0x0400764F RID: 30287
		public float TargetIntensity;

		// Token: 0x04007650 RID: 30288
		public float FadeTime;

		// Token: 0x04007651 RID: 30289
		public GameObject particlePrefab;

		// Token: 0x04007652 RID: 30290
		public ParticleSystem[] particleSystems;
	}
}

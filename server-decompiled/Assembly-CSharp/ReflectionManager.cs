using System;
using System.Collections.Generic;
using ShinyScreenSpaceRaytracedReflections;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02001362 RID: 4962
public class ReflectionManager
{
	// Token: 0x06009C64 RID: 40036 RVA: 0x003B0556 File Offset: 0x003AE756
	public static ReflectionManager Create(EntityPlayerLocal player)
	{
		ReflectionManager reflectionManager = new ReflectionManager();
		reflectionManager.player = player;
		reflectionManager.Init();
		return reflectionManager;
	}

	// Token: 0x06009C65 RID: 40037 RVA: 0x003B056C File Offset: 0x003AE76C
	[PublicizedFrom(EAccessModifier.Private)]
	public void Init()
	{
		global::UnityEngine.Object.Destroy(this.player.gameObject.GetComponentInChildren<PlayerReflectionProbe>().gameObject);
		this.managerObj = new GameObject("ReflectionManager");
		this.managerObj.layer = 2;
		Transform transform = this.managerObj.transform;
		this.hasCopySupport = SystemInfo.copyTextureSupport > CopyTextureSupport.None;
		this.probes = new List<ReflectionManager.Probe>();
		if (this.hasCopySupport)
		{
			for (int i = 0; i < 1; i++)
			{
				ReflectionManager.Probe probe = this.AddProbe(transform, false);
				this.probes.Add(probe);
			}
		}
		this.mainProbe = this.AddProbe(this.player.transform, true);
		int @int = GamePrefs.GetInt(EnumGamePrefs.OptionsGfxReflectQuality);
		bool @bool = GamePrefs.GetBool(EnumGamePrefs.OptionsGfxReflectShadows);
		this.ApplyProbeOptions(@int, @bool);
	}

	// Token: 0x06009C66 RID: 40038 RVA: 0x003B0634 File Offset: 0x003AE834
	public static void ApplyOptions(bool useSimple = false)
	{
		int num = GamePrefs.GetInt(EnumGamePrefs.OptionsGfxReflectQuality);
		bool flag = GamePrefs.GetBool(EnumGamePrefs.OptionsGfxReflectShadows);
		if (useSimple)
		{
			num = 0;
			flag = false;
		}
		World world = GameManager.Instance.World;
		if (world != null)
		{
			List<EntityPlayerLocal> localPlayers = world.GetLocalPlayers();
			for (int i = localPlayers.Count - 1; i >= 0; i--)
			{
				localPlayers[i].renderManager.reflectionManager.ApplyProbeOptions(num, flag);
			}
		}
	}

	// Token: 0x06009C67 RID: 40039 RVA: 0x003B06A0 File Offset: 0x003AE8A0
	public void Destroy()
	{
		global::UnityEngine.Object.Destroy(this.managerObj);
		this.probes.Clear();
		if (this.mainTex)
		{
			this.mainTex.Release();
			this.mainTex = null;
		}
		global::UnityEngine.Object.Destroy(this.mainProbe.reflectionProbe.gameObject);
		this.mainProbe = null;
		if (this.blendTex)
		{
			this.blendTex.Release();
			this.blendTex = null;
		}
		if (this.captureTex)
		{
			this.captureTex.Release();
			this.captureTex = null;
		}
	}

	// Token: 0x06009C68 RID: 40040 RVA: 0x003B073C File Offset: 0x003AE93C
	public void LightChanged(Vector3 lightPos)
	{
		ReflectionManager.Probe probe = this.mainProbe;
		if (this.probes.Count > 0)
		{
			probe = this.probes[0];
		}
		if ((lightPos - probe.worldPos).sqrMagnitude <= 225f)
		{
			probe.updateTime = 0f;
		}
	}

	// Token: 0x06009C69 RID: 40041 RVA: 0x003B0794 File Offset: 0x003AE994
	public void FrameUpdate()
	{
		if (ReflectionManager.optionsSelected.resolution == 0)
		{
			return;
		}
		int count = this.probes.Count;
		if (this.renderProbe != null)
		{
			ReflectionProbe reflectionProbe = this.renderProbe.reflectionProbe;
			if (this.renderFixTime > 0f)
			{
				this.renderFixTime -= Time.deltaTime;
				if (this.renderFixTime <= 0f)
				{
					reflectionProbe.enabled = false;
					reflectionProbe.refreshMode = ReflectionProbeRefreshMode.ViaScripting;
					this.renderProbe = null;
				}
			}
			else if (reflectionProbe.IsFinishedRendering(this.renderId))
			{
				if (this.hasCopySupport && this.renderProbe == this.blendProbe)
				{
					this.blendProbe = null;
				}
				this.renderProbe = null;
			}
			else
			{
				this.renderDuration += Time.deltaTime;
				if (this.renderDuration > 2f)
				{
					this.renderFixTime = 1f;
					reflectionProbe.refreshMode = ReflectionProbeRefreshMode.EveryFrame;
				}
			}
		}
		Vector3 position = this.player.position;
		position.y += 1.55f;
		if (this.player.IsCrouching)
		{
			position.y -= 0.2f;
		}
		Vector3 vector = this.player.GetVelocityPerSecond() * ReflectionManager.optionsSelected.playerVel;
		float num = vector.magnitude + 0.15f;
		Vector3 vector2 = position + vector;
		RaycastHit raycastHit;
		if (num > 0.2f && Physics.SphereCast(position - Origin.position, 0.2f, vector, out raycastHit, num - 0.2f, 1082195968))
		{
			vector2 = position;
		}
		ReflectionManager.Probe probe = this.mainProbe;
		if (count > 0)
		{
			this.SortProbes(vector2);
			probe = this.probes[0];
			if (probe != this.blendProbe && probe != this.renderProbe)
			{
				Graphics.CopyTexture(this.mainTex, this.blendTex);
				this.blendProbe = probe;
				this.blendPer = 0.05f;
				this.blendPos = vector2;
			}
			if (this.blendPer > 0f)
			{
				float magnitude = (vector2 - this.blendPos).magnitude;
				this.blendPos = vector2;
				float deltaTime = Time.deltaTime;
				float num2 = magnitude / 0.75f;
				num2 += 0.8333333f * ReflectionManager.optionsSelected.rateScale * deltaTime;
				num2 *= 1f - Mathf.Pow(this.blendPer, 0.7f);
				this.blendPer += num2;
				this.blendPer += ((this.renderProbe != null) ? (ReflectionManager.optionsSelected.rateRender * ReflectionManager.optionsSelected.rateScale * deltaTime) : 0f);
				if (this.blendPer < 0.95f)
				{
					ReflectionProbe.BlendCubemap(this.blendTex, this.captureTex, this.blendPer, this.mainTex);
				}
				else
				{
					ReflectionProbe.BlendCubemap(this.blendTex, this.captureTex, 1f, this.mainTex);
					this.blendPer = 0f;
				}
			}
		}
		if (this.renderProbe == null)
		{
			ReflectionManager.Probe probe2 = null;
			if (Time.time - probe.updateTime >= 8f)
			{
				probe2 = probe;
			}
			float worldLightLevelInRange = LightManager.GetWorldLightLevelInRange(probe.worldPos, 40f);
			float num3 = worldLightLevelInRange - probe.lightLevel;
			if (num3 < -0.15f || num3 > 0.15f)
			{
				probe2 = probe;
			}
			Vector3 forward = this.player.cameraTransform.forward;
			if (Vector3.Dot(forward, probe.forward) < 0.7f)
			{
				probe2 = probe;
			}
			float sqrMagnitude = (vector2 - probe.worldPos).sqrMagnitude;
			float num4 = 0.3f / ReflectionManager.optionsSelected.rateScale;
			if (sqrMagnitude >= num4 * num4)
			{
				probe2 = probe;
				if (count > 1)
				{
					probe2 = this.probes[count - 1];
				}
			}
			if (probe2 != null)
			{
				probe2.lightLevel = worldLightLevelInRange;
				probe2.updateTime = Time.time;
				probe2.worldPos = vector2;
				probe2.forward = forward;
				probe2.t.position = vector2 - Origin.position;
				ReflectionProbe reflectionProbe2 = probe2.reflectionProbe;
				reflectionProbe2.enabled = true;
				int num5 = this.renderId;
				this.renderId = reflectionProbe2.RenderProbe(this.captureTex);
				if (this.renderId == num5)
				{
					Log.Warning("{0} ReflectionManager #{1}, rid {2}, probe stuck", new object[]
					{
						GameManager.frameCount,
						this.probes.IndexOf(this.renderProbe),
						this.renderId
					});
				}
				this.renderProbe = probe2;
				this.renderDuration = 0f;
			}
		}
	}

	// Token: 0x06009C6A RID: 40042 RVA: 0x003B0C2C File Offset: 0x003AEE2C
	[PublicizedFrom(EAccessModifier.Private)]
	public void SortProbes(Vector3 pos)
	{
		for (int i = this.probes.Count - 1; i >= 0; i--)
		{
			ReflectionManager.Probe probe = this.probes[i];
			probe.distSq = (pos - probe.worldPos).sqrMagnitude;
		}
		this.probes.Sort(this.sorter);
	}

	// Token: 0x06009C6B RID: 40043 RVA: 0x003B0C8C File Offset: 0x003AEE8C
	[PublicizedFrom(EAccessModifier.Private)]
	public ReflectionManager.Probe AddProbe(Transform parentT, bool isMain)
	{
		ReflectionManager.Probe probe = new ReflectionManager.Probe();
		GameObject gameObject = new GameObject("RProbe");
		gameObject.layer = 2;
		Transform transform = gameObject.transform;
		probe.t = transform;
		transform.SetParent(parentT, false);
		ReflectionProbe reflectionProbe = gameObject.AddComponent<ReflectionProbe>();
		probe.reflectionProbe = reflectionProbe;
		reflectionProbe.enabled = false;
		reflectionProbe.mode = ReflectionProbeMode.Realtime;
		reflectionProbe.refreshMode = ReflectionProbeRefreshMode.ViaScripting;
		reflectionProbe.blendDistance = 20f;
		reflectionProbe.center = Vector3.zero;
		reflectionProbe.size = Vector3.zero;
		reflectionProbe.clearFlags = ReflectionProbeClearFlags.SolidColor;
		if (isMain)
		{
			reflectionProbe.blendDistance = 400f;
			reflectionProbe.size = new Vector3(400f, 400f, 400f);
			reflectionProbe.importance = 10;
			if (this.hasCopySupport)
			{
				reflectionProbe.mode = ReflectionProbeMode.Custom;
			}
		}
		return probe;
	}

	// Token: 0x06009C6C RID: 40044 RVA: 0x003B0D50 File Offset: 0x003AEF50
	[PublicizedFrom(EAccessModifier.Private)]
	public void ApplyProbeOptions(int quality, bool useShadows)
	{
		if (quality < 0 || quality >= ReflectionManager.optionsData.Length)
		{
			quality = ReflectionManager.optionsData.Length - 1;
		}
		ReflectionManager.optionsSelected = ReflectionManager.optionsData[quality];
		for (int i = this.probes.Count - 1; i >= 0; i--)
		{
			ReflectionManager.Probe probe = this.probes[i];
			this.ApplyProbeOptions(probe, useShadows);
		}
		this.ApplyProbeOptions(this.mainProbe, useShadows);
		if (ReflectionManager.optionsSelected.resolution > 0)
		{
			this.mainProbe.reflectionProbe.enabled = true;
		}
		bool flag = ReflectionManager.optionsSelected.resolution > 0;
		Shader.SetGlobalFloat("_ReflectionsOn", (float)(flag ? 1 : 0));
		if (!flag)
		{
			Shader.EnableKeyword("GAME_NOREFLECTION");
		}
		else
		{
			Shader.DisableKeyword("GAME_NOREFLECTION");
		}
		if (!this.mainTex || this.mainTex.width != ReflectionManager.optionsSelected.resolution)
		{
			if (this.mainTex)
			{
				this.mainTex.Release();
				this.mainTex = null;
			}
			if (flag)
			{
				this.mainTex = this.CreateTexture(false);
				this.mainTex.name = "probeMain";
				this.mainProbe.reflectionProbe.customBakedTexture = this.mainTex;
				if (!this.hasCopySupport)
				{
					this.captureTex = this.mainTex;
				}
			}
		}
		if (this.hasCopySupport)
		{
			if (!this.blendTex || this.blendTex.width != ReflectionManager.optionsSelected.resolution)
			{
				if (this.blendTex)
				{
					this.blendTex.Release();
					this.blendTex = null;
				}
				if (flag)
				{
					this.blendTex = this.CreateTexture(false);
					this.blendTex.name = "probeBlend";
				}
			}
			if (!this.captureTex || this.captureTex.width != ReflectionManager.optionsSelected.resolution)
			{
				if (this.captureTex)
				{
					this.captureTex.Release();
					this.captureTex = null;
				}
				if (flag && this.hasCopySupport)
				{
					this.captureTex = this.CreateTexture(false);
					this.captureTex.name = "probeCap";
				}
			}
		}
		this.ApplyWaterSetting();
	}

	// Token: 0x06009C6D RID: 40045 RVA: 0x003B0F84 File Offset: 0x003AF184
	[PublicizedFrom(EAccessModifier.Private)]
	public void ApplyProbeOptions(ReflectionManager.Probe probe, bool useShadows)
	{
		ReflectionProbe reflectionProbe = probe.reflectionProbe;
		reflectionProbe.enabled = false;
		if (ReflectionManager.optionsSelected.resolution == 0)
		{
			return;
		}
		reflectionProbe.nearClipPlane = 0.1f;
		reflectionProbe.farClipPlane = ReflectionManager.optionsSelected.farClip;
		reflectionProbe.shadowDistance = (useShadows ? ReflectionManager.optionsSelected.shadowDist : 0f);
		int @int = GamePrefs.GetInt(EnumGamePrefs.OptionsGfxSSReflections);
		reflectionProbe.intensity = ReflectionManager.optionsSelected.intensity * (useShadows ? 1f : 0.85f) * ((@int > 0) ? 0.91f : 1f);
		reflectionProbe.resolution = ReflectionManager.optionsSelected.resolution;
		reflectionProbe.cullingMask = ReflectionManager.optionsSelected.mask;
		if (ReflectionManager.optionsSelected.rate <= 1)
		{
			reflectionProbe.timeSlicingMode = ReflectionProbeTimeSlicingMode.IndividualFaces;
			return;
		}
		if (ReflectionManager.optionsSelected.rate <= 2)
		{
			reflectionProbe.timeSlicingMode = ReflectionProbeTimeSlicingMode.AllFacesAtOnce;
			return;
		}
		reflectionProbe.timeSlicingMode = ReflectionProbeTimeSlicingMode.NoTimeSlicing;
	}

	// Token: 0x06009C6E RID: 40046 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void ApplyWaterSetting()
	{
	}

	// Token: 0x06009C6F RID: 40047 RVA: 0x003B1070 File Offset: 0x003AF270
	public void ApplyCameraOptions(Camera camera)
	{
		int @int = GamePrefs.GetInt(EnumGamePrefs.OptionsGfxSSReflections);
		ShinySSRR component = camera.GetComponent<ShinySSRR>();
		if (component)
		{
			component.enabled = @int >= 1;
			component.jitter = 0.2f;
			switch (@int)
			{
			case 1:
				component.ApplyRaytracingPreset(RaytracingPreset.Fast);
				component.minimumBlur = 0.5f;
				break;
			case 2:
				component.ApplyRaytracingPreset(RaytracingPreset.Medium);
				component.minimumBlur = 0.35f;
				component.sampleCount = 32;
				component.maxRayLength = 16f;
				break;
			case 3:
				component.ApplyRaytracingPreset(RaytracingPreset.High);
				component.minimumBlur = 0.3f;
				break;
			case 4:
				component.ApplyRaytracingPreset(RaytracingPreset.Superb);
				component.minimumBlur = 0.2f;
				break;
			}
			component.refineThickness = false;
			component.temporalFilter = true;
		}
	}

	// Token: 0x06009C70 RID: 40048 RVA: 0x003B1140 File Offset: 0x003AF340
	[PublicizedFrom(EAccessModifier.Private)]
	public RenderTexture CreateTexture(bool autoGenMips)
	{
		RenderTextureFormat renderTextureFormat = RenderTextureFormat.ARGB32;
		RenderTexture renderTexture = new RenderTexture(new RenderTextureDescriptor(ReflectionManager.optionsSelected.resolution, ReflectionManager.optionsSelected.resolution, renderTextureFormat, 0)
		{
			dimension = TextureDimension.Cube,
			useMipMap = true,
			autoGenerateMips = autoGenMips
		});
		renderTexture.Create();
		if (!autoGenMips)
		{
			renderTexture.GenerateMips();
		}
		return renderTexture;
	}

	// Token: 0x04007619 RID: 30233
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cProbeCount = 1;

	// Token: 0x0400761A RID: 30234
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cUpdateAge = 8f;

	// Token: 0x0400761B RID: 30235
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cUpdatePlayerDistance = 0.3f;

	// Token: 0x0400761C RID: 30236
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cUpdateLightDistance = 15f;

	// Token: 0x0400761D RID: 30237
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cBlendInPerSec = 0.8333333f;

	// Token: 0x0400761E RID: 30238
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cOffsetY = 1.55f;

	// Token: 0x0400761F RID: 30239
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cIntensityNoShadowsScale = 0.85f;

	// Token: 0x04007620 RID: 30240
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x04007621 RID: 30241
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject managerObj;

	// Token: 0x04007622 RID: 30242
	[PublicizedFrom(EAccessModifier.Private)]
	public ReflectionManager.Probe mainProbe;

	// Token: 0x04007623 RID: 30243
	[PublicizedFrom(EAccessModifier.Private)]
	public RenderTexture mainTex;

	// Token: 0x04007624 RID: 30244
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ReflectionManager.Probe> probes;

	// Token: 0x04007625 RID: 30245
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasCopySupport;

	// Token: 0x04007626 RID: 30246
	[PublicizedFrom(EAccessModifier.Private)]
	public float blendPer;

	// Token: 0x04007627 RID: 30247
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 blendPos;

	// Token: 0x04007628 RID: 30248
	[PublicizedFrom(EAccessModifier.Private)]
	public ReflectionManager.Probe blendProbe;

	// Token: 0x04007629 RID: 30249
	[PublicizedFrom(EAccessModifier.Private)]
	public RenderTexture blendTex;

	// Token: 0x0400762A RID: 30250
	[PublicizedFrom(EAccessModifier.Private)]
	public RenderTexture captureTex;

	// Token: 0x0400762B RID: 30251
	[PublicizedFrom(EAccessModifier.Private)]
	public ReflectionManager.Probe renderProbe;

	// Token: 0x0400762C RID: 30252
	[PublicizedFrom(EAccessModifier.Private)]
	public float renderDuration;

	// Token: 0x0400762D RID: 30253
	[PublicizedFrom(EAccessModifier.Private)]
	public int renderId;

	// Token: 0x0400762E RID: 30254
	[PublicizedFrom(EAccessModifier.Private)]
	public float renderFixTime;

	// Token: 0x0400762F RID: 30255
	[PublicizedFrom(EAccessModifier.Private)]
	public ReflectionManager.Sorter sorter = new ReflectionManager.Sorter();

	// Token: 0x04007630 RID: 30256
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cSkyLayer = 9;

	// Token: 0x04007631 RID: 30257
	[PublicizedFrom(EAccessModifier.Private)]
	public static ReflectionManager.Options[] optionsData = new ReflectionManager.Options[]
	{
		default(ReflectionManager.Options),
		new ReflectionManager.Options
		{
			rate = 1,
			rateScale = 0.5f,
			rateRender = 2f,
			playerVel = 0.1f,
			farClip = 30f,
			shadowDist = 25f,
			resolution = 64,
			intensity = 0.55f,
			mask = 268435968
		},
		new ReflectionManager.Options
		{
			rate = 1,
			rateScale = 1f,
			rateRender = 2f,
			playerVel = 0.1f,
			farClip = 90f,
			shadowDist = 30f,
			resolution = 128,
			intensity = 0.65f,
			mask = 276824576
		},
		new ReflectionManager.Options
		{
			rate = 1,
			rateScale = 1.2f,
			rateRender = 2f,
			playerVel = 0.1f,
			farClip = 180f,
			shadowDist = 50f,
			resolution = 256,
			intensity = 0.66f,
			mask = 276824576
		},
		new ReflectionManager.Options
		{
			rate = 2,
			rateScale = 1.6f,
			rateRender = 2f,
			playerVel = 0.08f,
			farClip = 280f,
			shadowDist = 70f,
			resolution = 256,
			intensity = 0.67f,
			mask = 276824576
		},
		new ReflectionManager.Options
		{
			rate = 3,
			rateScale = 12f,
			rateRender = 5f,
			playerVel = 0.01f,
			farClip = 425f,
			shadowDist = 100f,
			resolution = 512,
			intensity = 0.68f,
			mask = 276824576
		},
		new ReflectionManager.Options
		{
			rate = 3,
			rateScale = 12f,
			rateRender = 5f,
			playerVel = 0.01f,
			farClip = 600f,
			shadowDist = 150f,
			resolution = 512,
			intensity = 0.68f,
			mask = 276824576
		}
	};

	// Token: 0x04007632 RID: 30258
	[PublicizedFrom(EAccessModifier.Private)]
	public static ReflectionManager.Options optionsSelected;

	// Token: 0x04007633 RID: 30259
	[PublicizedFrom(EAccessModifier.Private)]
	public static int[] waterTransparencyDistances = new int[]
	{
		32, 32, 32, 32, 32, 32, 36, 40, 44, 48,
		52, 56, 60
	};

	// Token: 0x02001363 RID: 4963
	public class Probe
	{
		// Token: 0x04007634 RID: 30260
		public Transform t;

		// Token: 0x04007635 RID: 30261
		public ReflectionProbe reflectionProbe;

		// Token: 0x04007636 RID: 30262
		public Vector3 worldPos;

		// Token: 0x04007637 RID: 30263
		public Vector3 forward;

		// Token: 0x04007638 RID: 30264
		public float distSq;

		// Token: 0x04007639 RID: 30265
		public float lightLevel;

		// Token: 0x0400763A RID: 30266
		public float updateTime;
	}

	// Token: 0x02001364 RID: 4964
	public class Sorter : IComparer<ReflectionManager.Probe>
	{
		// Token: 0x06009C74 RID: 40052 RVA: 0x003B14BC File Offset: 0x003AF6BC
		public int Compare(ReflectionManager.Probe _p1, ReflectionManager.Probe _p2)
		{
			if (_p1.distSq < _p2.distSq)
			{
				return -1;
			}
			if (_p1.distSq > _p2.distSq)
			{
				return 1;
			}
			return 0;
		}
	}

	// Token: 0x02001365 RID: 4965
	[PublicizedFrom(EAccessModifier.Private)]
	public struct Options
	{
		// Token: 0x0400763B RID: 30267
		public int rate;

		// Token: 0x0400763C RID: 30268
		public float rateScale;

		// Token: 0x0400763D RID: 30269
		public float rateRender;

		// Token: 0x0400763E RID: 30270
		public float playerVel;

		// Token: 0x0400763F RID: 30271
		public float farClip;

		// Token: 0x04007640 RID: 30272
		public float shadowDist;

		// Token: 0x04007641 RID: 30273
		public int resolution;

		// Token: 0x04007642 RID: 30274
		public float intensity;

		// Token: 0x04007643 RID: 30275
		public int mask;
	}
}

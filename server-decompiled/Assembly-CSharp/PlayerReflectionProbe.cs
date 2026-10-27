using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000514 RID: 1300
[RequireComponent(typeof(ReflectionProbe))]
public class PlayerReflectionProbe : MonoBehaviour
{
	// Token: 0x06002A5B RID: 10843 RVA: 0x0010BD3F File Offset: 0x00109F3F
	public static void UpdateCamera(Camera _mainCamera)
	{
		PlayerReflectionProbe.mainCamera = _mainCamera;
	}

	// Token: 0x06002A5C RID: 10844 RVA: 0x0010BD47 File Offset: 0x00109F47
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Start()
	{
		this.allReflectionProbes = base.transform.parent.GetComponentsInChildren<ReflectionProbe>();
		this.reflectionProbe = base.GetComponent<ReflectionProbe>();
		PlayerReflectionProbe.playerReflectionProbe = this;
		PlayerReflectionProbe.SetReflectionSettings(GamePrefs.GetInt(EnumGamePrefs.OptionsGfxReflectQuality), GamePrefs.GetBool(EnumGamePrefs.OptionsGfxReflectShadows));
	}

	// Token: 0x06002A5D RID: 10845 RVA: 0x0010BD87 File Offset: 0x00109F87
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Destroy()
	{
		PlayerReflectionProbe.playerReflectionProbe = null;
		this.reflectionProbe = null;
		this.allReflectionProbes = null;
	}

	// Token: 0x06002A5E RID: 10846 RVA: 0x0010BDA0 File Offset: 0x00109FA0
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Update()
	{
		if (!PlayerReflectionProbe.hasReflections || this.reflectionProbe == null || PlayerReflectionProbe.mainCamera == null)
		{
			return;
		}
		if (Time.time > this.updateEyeNearWaterTimer + this.eyeNearWaterUpdateFreq)
		{
			this.bEyeNearWaterSurface = false;
			RaycastHit raycastHit;
			if (Physics.SphereCast(new Ray(PlayerReflectionProbe.mainCamera.transform.position + Vector3.up, Vector3.down), 0.5f, out raycastHit, this.fWaterDisCheck, 16))
			{
				this.bEyeNearWaterSurface = raycastHit.distance < float.PositiveInfinity;
			}
		}
		bool flag = !this.bEyeNearWaterSurface || Time.time > this.lastTimeUpdatedProbe + this.updateProbeWhileEyeNearWaterFreq;
		flag &= Time.realtimeSinceStartup > this.nextUpdate;
		flag &= this.reflectionProbe.enabled;
		if (this.lastRenderID == -1 || this.catchUpFrameCount < 14)
		{
			for (int i = 0; i < this.allReflectionProbes.Length; i++)
			{
				this.allReflectionProbes[i].refreshMode = ReflectionProbeRefreshMode.EveryFrame;
				this.allReflectionProbes[i].timeSlicingMode = ReflectionProbeTimeSlicingMode.NoTimeSlicing;
			}
			flag = true;
		}
		else if (this.catchUpFrameCount == 14)
		{
			for (int j = 0; j < this.allReflectionProbes.Length; j++)
			{
				this.allReflectionProbes[j].refreshMode = ReflectionProbeRefreshMode.ViaScripting;
				this.allReflectionProbes[j].timeSlicingMode = this.timeSlicingMode;
			}
		}
		this.catchUpFrameCount++;
		if (flag)
		{
			for (int k = 0; k < this.allReflectionProbes.Length; k++)
			{
				this.lastRenderID = this.allReflectionProbes[k].RenderProbe();
			}
			this.lastTimeUpdatedProbe = Time.time;
			this.nextUpdate = Time.realtimeSinceStartup + 1000f / (float)this.RefreshRate * 0.001f;
		}
	}

	// Token: 0x06002A5F RID: 10847 RVA: 0x0010BF65 File Offset: 0x0010A165
	public static void SetReflectionSettings(int qualityLevel, bool bReflectedShadows)
	{
		if (!PlayerReflectionProbe.playerReflectionProbe)
		{
			return;
		}
		PlayerReflectionProbe.playerReflectionProbe.setReflectionSettings(qualityLevel, bReflectedShadows);
	}

	// Token: 0x06002A60 RID: 10848 RVA: 0x0010BF80 File Offset: 0x0010A180
	[PublicizedFrom(EAccessModifier.Private)]
	public void setReflectionSettings(int qualityLevel, bool bReflectedShadows)
	{
		if (this.reflectionProbe == null)
		{
			return;
		}
		this.lastRenderID = -1;
		this.catchUpFrameCount = 0;
		RenderSettings.reflectionBounces = 1;
		PlayerReflectionProbe.hasReflections = qualityLevel > 0;
		for (int i = 0; i < this.allReflectionProbes.Length; i++)
		{
			this.allReflectionProbes[i].enabled = PlayerReflectionProbe.hasReflections;
		}
		Shader.SetGlobalFloat("_ReflectionsOn", (float)(PlayerReflectionProbe.hasReflections ? 1 : 0));
		if (!PlayerReflectionProbe.hasReflections)
		{
			Shader.EnableKeyword("GAME_NOREFLECTION");
			return;
		}
		this.reflectionProbe.nearClipPlane = 0.1f;
		this.reflectionProbe.intensity = 1f;
		Shader.DisableKeyword("GAME_NOREFLECTION");
		switch (qualityLevel)
		{
		case 1:
		{
			this.RefreshRate = 3;
			this.reflectionProbe.farClipPlane = 30f;
			this.reflectionProbe.shadowDistance = (float)(bReflectedShadows ? 10 : 0);
			this.reflectionProbe.resolution = 128;
			this.timeSlicingMode = ReflectionProbeTimeSlicingMode.IndividualFaces;
			this.reflectionProbe.intensity = 0.5f;
			string[] array = new string[] { "ReflectionsOnly", "Terrain" };
			this.SetOptions(array);
			return;
		}
		case 2:
		{
			this.RefreshRate = 5;
			this.reflectionProbe.farClipPlane = 100f;
			this.reflectionProbe.shadowDistance = (float)(bReflectedShadows ? 20 : 0);
			this.reflectionProbe.resolution = 256;
			this.timeSlicingMode = ReflectionProbeTimeSlicingMode.IndividualFaces;
			this.reflectionProbe.intensity = 0.55f;
			string[] array2 = new string[] { "Trees", "ReflectionsOnly", "Terrain" };
			this.SetOptions(array2);
			return;
		}
		case 3:
		{
			this.RefreshRate = 15;
			this.reflectionProbe.farClipPlane = 200f;
			this.reflectionProbe.shadowDistance = (float)(bReflectedShadows ? 50 : 0);
			this.reflectionProbe.resolution = 256;
			this.timeSlicingMode = ReflectionProbeTimeSlicingMode.AllFacesAtOnce;
			this.reflectionProbe.intensity = 0.7f;
			string[] array3 = new string[] { "Trees", "ReflectionsOnly", "Terrain" };
			this.SetOptions(array3);
			return;
		}
		case 4:
		{
			this.RefreshRate = 18;
			this.reflectionProbe.farClipPlane = 280f;
			this.reflectionProbe.shadowDistance = (float)(bReflectedShadows ? 70 : 0);
			this.reflectionProbe.resolution = 256;
			this.timeSlicingMode = ReflectionProbeTimeSlicingMode.NoTimeSlicing;
			this.reflectionProbe.intensity = 0.85f;
			string[] array4 = new string[] { "Trees", "ReflectionsOnly", "Terrain" };
			this.SetOptions(array4);
			return;
		}
		case 5:
		{
			this.RefreshRate = 24;
			this.reflectionProbe.farClipPlane = 425f;
			this.reflectionProbe.shadowDistance = (float)(bReflectedShadows ? 100 : 0);
			this.reflectionProbe.resolution = 512;
			this.timeSlicingMode = ReflectionProbeTimeSlicingMode.NoTimeSlicing;
			string[] array5 = new string[] { "Trees", "ReflectionsOnly", "Terrain" };
			this.SetOptions(array5);
			return;
		}
		}
		this.RefreshRate = 30;
		this.reflectionProbe.farClipPlane = 600f;
		this.reflectionProbe.shadowDistance = (float)(bReflectedShadows ? 150 : 0);
		this.reflectionProbe.resolution = 512;
		this.timeSlicingMode = ReflectionProbeTimeSlicingMode.NoTimeSlicing;
		string[] array6 = new string[] { "Trees", "ReflectionsOnly", "Terrain" };
		this.SetOptions(array6);
	}

	// Token: 0x06002A61 RID: 10849 RVA: 0x0010C30F File Offset: 0x0010A50F
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetOptions(string[] cullingMasks)
	{
		this.reflectionProbe.cullingMask = LayerMask.GetMask(cullingMasks);
	}

	// Token: 0x04002069 RID: 8297
	public int RefreshRate = 30;

	// Token: 0x0400206A RID: 8298
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static PlayerReflectionProbe playerReflectionProbe = null;

	// Token: 0x0400206B RID: 8299
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public ReflectionProbe[] allReflectionProbes;

	// Token: 0x0400206C RID: 8300
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public ReflectionProbe reflectionProbe;

	// Token: 0x0400206D RID: 8301
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static bool hasReflections = true;

	// Token: 0x0400206E RID: 8302
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int lastRenderID = -1;

	// Token: 0x0400206F RID: 8303
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float nextUpdate;

	// Token: 0x04002070 RID: 8304
	public float fWaterDisCheck = 2f;

	// Token: 0x04002071 RID: 8305
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public ReflectionProbeTimeSlicingMode timeSlicingMode = ReflectionProbeTimeSlicingMode.IndividualFaces;

	// Token: 0x04002072 RID: 8306
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bEyeNearWaterSurface;

	// Token: 0x04002073 RID: 8307
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int raycastMask = 16;

	// Token: 0x04002074 RID: 8308
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float updateEyeNearWaterTimer;

	// Token: 0x04002075 RID: 8309
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float eyeNearWaterUpdateFreq = 1.25f;

	// Token: 0x04002076 RID: 8310
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float lastTimeUpdatedProbe;

	// Token: 0x04002077 RID: 8311
	public float updateProbeWhileEyeNearWaterFreq = 3f;

	// Token: 0x04002078 RID: 8312
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int[] viewDistantToWaterTransparencyDistance = new int[]
	{
		32, 32, 32, 32, 32, 32, 36, 40, 44, 48,
		52, 56, 60
	};

	// Token: 0x04002079 RID: 8313
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Camera mainCamera = null;

	// Token: 0x0400207A RID: 8314
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int catchUpFrameCount;

	// Token: 0x0400207B RID: 8315
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int cMaxFramesToUpdate = 14;
}

using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000512 RID: 1298
[RequireComponent(typeof(ReflectionProbe))]
public class PlayerReflectionProbe : MonoBehaviour
{
	// Token: 0x06002A54 RID: 10836 RVA: 0x0010B7EB File Offset: 0x001099EB
	public static void UpdateCamera(Camera _mainCamera)
	{
		PlayerReflectionProbe.mainCamera = _mainCamera;
	}

	// Token: 0x06002A55 RID: 10837 RVA: 0x0010B7F3 File Offset: 0x001099F3
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Start()
	{
		this.allReflectionProbes = base.transform.parent.GetComponentsInChildren<ReflectionProbe>();
		this.reflectionProbe = base.GetComponent<ReflectionProbe>();
		PlayerReflectionProbe.playerReflectionProbe = this;
		PlayerReflectionProbe.SetReflectionSettings(GamePrefs.GetInt(EnumGamePrefs.OptionsGfxReflectQuality), GamePrefs.GetBool(EnumGamePrefs.OptionsGfxReflectShadows));
	}

	// Token: 0x06002A56 RID: 10838 RVA: 0x0010B833 File Offset: 0x00109A33
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Destroy()
	{
		PlayerReflectionProbe.playerReflectionProbe = null;
		this.reflectionProbe = null;
		this.allReflectionProbes = null;
	}

	// Token: 0x06002A57 RID: 10839 RVA: 0x0010B84C File Offset: 0x00109A4C
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

	// Token: 0x06002A58 RID: 10840 RVA: 0x0010BA11 File Offset: 0x00109C11
	public static void SetReflectionSettings(int qualityLevel, bool bReflectedShadows)
	{
		if (!PlayerReflectionProbe.playerReflectionProbe)
		{
			return;
		}
		PlayerReflectionProbe.playerReflectionProbe.setReflectionSettings(qualityLevel, bReflectedShadows);
	}

	// Token: 0x06002A59 RID: 10841 RVA: 0x0010BA2C File Offset: 0x00109C2C
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

	// Token: 0x06002A5A RID: 10842 RVA: 0x0010BDBB File Offset: 0x00109FBB
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetOptions(string[] cullingMasks)
	{
		this.reflectionProbe.cullingMask = LayerMask.GetMask(cullingMasks);
	}

	// Token: 0x04002059 RID: 8281
	public int RefreshRate = 30;

	// Token: 0x0400205A RID: 8282
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static PlayerReflectionProbe playerReflectionProbe = null;

	// Token: 0x0400205B RID: 8283
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public ReflectionProbe[] allReflectionProbes;

	// Token: 0x0400205C RID: 8284
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public ReflectionProbe reflectionProbe;

	// Token: 0x0400205D RID: 8285
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static bool hasReflections = true;

	// Token: 0x0400205E RID: 8286
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int lastRenderID = -1;

	// Token: 0x0400205F RID: 8287
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float nextUpdate;

	// Token: 0x04002060 RID: 8288
	public float fWaterDisCheck = 2f;

	// Token: 0x04002061 RID: 8289
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public ReflectionProbeTimeSlicingMode timeSlicingMode = ReflectionProbeTimeSlicingMode.IndividualFaces;

	// Token: 0x04002062 RID: 8290
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bEyeNearWaterSurface;

	// Token: 0x04002063 RID: 8291
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int raycastMask = 16;

	// Token: 0x04002064 RID: 8292
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float updateEyeNearWaterTimer;

	// Token: 0x04002065 RID: 8293
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float eyeNearWaterUpdateFreq = 1.25f;

	// Token: 0x04002066 RID: 8294
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float lastTimeUpdatedProbe;

	// Token: 0x04002067 RID: 8295
	public float updateProbeWhileEyeNearWaterFreq = 3f;

	// Token: 0x04002068 RID: 8296
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int[] viewDistantToWaterTransparencyDistance = new int[]
	{
		32, 32, 32, 32, 32, 32, 36, 40, 44, 48,
		52, 56, 60
	};

	// Token: 0x04002069 RID: 8297
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Camera mainCamera = null;

	// Token: 0x0400206A RID: 8298
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int catchUpFrameCount;

	// Token: 0x0400206B RID: 8299
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int cMaxFramesToUpdate = 14;
}

using System;
using System.Runtime.CompilerServices;
using PI.NGSS;
using UnityEngine;

// Token: 0x0200139F RID: 5023
public class SkyManager : MonoBehaviour
{
	// Token: 0x170012E3 RID: 4835
	// (get) Token: 0x06009D9A RID: 40346 RVA: 0x003B8D5A File Offset: 0x003B6F5A
	public static Transform SunLightT
	{
		get
		{
			return SkyManager.sunLightT;
		}
	}

	// Token: 0x06009D9B RID: 40347 RVA: 0x003B8D61 File Offset: 0x003B6F61
	public static bool IsBloodMoonVisible()
	{
		return GameUtils.IsBloodMoonTime(new ValueTuple<int, int>(SkyManager.duskTime - 4, SkyManager.dawnTime + 2), (int)SkyManager.TimeOfDay(), SkyManager.bloodmoonDay, (int)SkyManager.dayCount);
	}

	// Token: 0x06009D9C RID: 40348 RVA: 0x003B8D8C File Offset: 0x003B6F8C
	public static float BloodMoonVisiblePercent()
	{
		int num = (int)SkyManager.dayCount;
		float num2 = SkyManager.TimeOfDay();
		if (num != SkyManager.bloodmoonDay)
		{
			return (float)((num > 1 && num == SkyManager.bloodmoonDay + 1 && num2 <= (float)(SkyManager.dawnTime + 2)) ? 1 : 0);
		}
		float num3 = (float)SkyManager.duskTime - num2;
		if (num3 < 0f)
		{
			return 1f;
		}
		float num4 = 1f - num3 / 4f;
		if (num4 >= 0f)
		{
			return num4;
		}
		return 0f;
	}

	// Token: 0x06009D9D RID: 40349 RVA: 0x003B8E01 File Offset: 0x003B7001
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		GameOptionsManager.ShadowDistanceChanged += this.OnShadowDistanceChanged;
		GameStats.OnChangedDelegates += this.OnGameStatsChanged;
	}

	// Token: 0x06009D9E RID: 40350 RVA: 0x003B8E25 File Offset: 0x003B7025
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDestroy()
	{
		GameOptionsManager.ShadowDistanceChanged -= this.OnShadowDistanceChanged;
		GameStats.OnChangedDelegates -= this.OnGameStatsChanged;
	}

	// Token: 0x06009D9F RID: 40351 RVA: 0x003B8E4C File Offset: 0x003B704C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnShadowDistanceChanged(int optionsShadowDistance)
	{
		int num;
		switch (optionsShadowDistance)
		{
		case 0:
		case 1:
		case 2:
			num = 1024;
			goto IL_0036;
		case 3:
			num = 2048;
			goto IL_0036;
		}
		num = 4096;
		IL_0036:
		SkyManager.sunLight.shadowCustomResolution = num;
		SkyManager.moonLight.shadowCustomResolution = num;
	}

	// Token: 0x06009DA0 RID: 40352 RVA: 0x003B8EA5 File Offset: 0x003B70A5
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnGameStatsChanged(EnumGameStats _gameState, object _newValue)
	{
		if (_gameState != EnumGameStats.DayLightLength)
		{
			if (_gameState == EnumGameStats.BloodMoonDay)
			{
				SkyManager.bloodmoonDay = GameStats.GetInt(EnumGameStats.BloodMoonDay);
				return;
			}
		}
		else
		{
			ValueTuple<int, int> valueTuple = GameUtils.CalcDuskDawnHours(GameStats.GetInt(EnumGameStats.DayLightLength));
			SkyManager.duskTime = valueTuple.Item1;
			SkyManager.dawnTime = valueTuple.Item2;
		}
	}

	// Token: 0x06009DA1 RID: 40353 RVA: 0x003B8EDF File Offset: 0x003B70DF
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Start()
	{
		SkyManager.Reset();
	}

	// Token: 0x06009DA2 RID: 40354 RVA: 0x003B8EE6 File Offset: 0x003B70E6
	public static void Loaded(GameObject _obj)
	{
		_obj.SetActive(true);
		_obj.transform.parent = GameManager.Instance.transform;
		SkyManager.skyManager = _obj.GetComponent<SkyManager>();
		SkyManager.Reset();
	}

	// Token: 0x06009DA3 RID: 40355 RVA: 0x003B8F14 File Offset: 0x003B7114
	public static void Cleanup()
	{
		if (SkyManager.skyManager != null)
		{
			global::UnityEngine.Object.DestroyImmediate(SkyManager.skyManager.gameObject);
		}
	}

	// Token: 0x06009DA4 RID: 40356 RVA: 0x003B8F34 File Offset: 0x003B7134
	public static void SetSkyEnabled(bool _enabled)
	{
		if (_enabled)
		{
			SkyManager.SetFogDebug(-1f, float.MinValue, float.MinValue);
			SkyManager.mainCamera.backgroundColor = Color.black;
		}
		else
		{
			SkyManager.SetFogDebug(0f, float.MinValue, float.MinValue);
			SkyManager.mainCamera.backgroundColor = new Color(0.44f, 0.48f, 0.52f);
		}
		SkyManager.atmosphereSphere.gameObject.SetActive(_enabled);
		SkyManager.cloudsSphere.gameObject.SetActive(_enabled);
	}

	// Token: 0x06009DA5 RID: 40357 RVA: 0x003B8FBB File Offset: 0x003B71BB
	public static Color GetSkyColor()
	{
		return SkyManager.SkyColor;
	}

	// Token: 0x06009DA6 RID: 40358 RVA: 0x003B8FC2 File Offset: 0x003B71C2
	public static void SetSkyColor(Color c)
	{
		SkyManager.SkyColor = c;
	}

	// Token: 0x06009DA7 RID: 40359 RVA: 0x003B8FCA File Offset: 0x003B71CA
	public static void SetGameTime(ulong _time)
	{
		SkyManager.dayCount = _time / 24000f + 1f;
		SkyManager.timeOfDay = _time;
	}

	// Token: 0x06009DA8 RID: 40360 RVA: 0x003B8FE6 File Offset: 0x003B71E6
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float TimeOfDay()
	{
		return SkyManager.timeOfDay % 24000UL / 1000f;
	}

	// Token: 0x06009DA9 RID: 40361 RVA: 0x003B8FFC File Offset: 0x003B71FC
	public static float GetTimeOfDayAsMinutes()
	{
		return SkyManager.TimeOfDay() / 24f * (float)GamePrefs.GetInt(EnumGamePrefs.DayNightLength);
	}

	// Token: 0x06009DAA RID: 40362 RVA: 0x003B9012 File Offset: 0x003B7212
	public static void SetCloudTextures(Texture _mainTex, Texture _blendTex)
	{
		SkyManager.cloudMainTex = _mainTex;
		SkyManager.cloudBlendTex = _blendTex;
	}

	// Token: 0x06009DAB RID: 40363 RVA: 0x003B9020 File Offset: 0x003B7220
	public static void SetCloudTransition(float t)
	{
		SkyManager.cloudTransition = t;
	}

	// Token: 0x06009DAC RID: 40364 RVA: 0x003B9028 File Offset: 0x003B7228
	public static Color GetFogColor()
	{
		return SkyManager.fogColor;
	}

	// Token: 0x06009DAD RID: 40365 RVA: 0x003B902F File Offset: 0x003B722F
	public static void SetFogColor(Color c)
	{
		if (SkyManager.fogDebugColor.a > 0f)
		{
			c = SkyManager.fogDebugColor;
		}
		SkyManager.fogColor = c;
	}

	// Token: 0x06009DAE RID: 40366 RVA: 0x003B904F File Offset: 0x003B724F
	public static float GetFogDensity()
	{
		return SkyManager.fogDensity;
	}

	// Token: 0x06009DAF RID: 40367 RVA: 0x003B9058 File Offset: 0x003B7258
	public static void SetFogDensity(float density)
	{
		if (SkyManager.fogDebugDensity >= 0f)
		{
			density = SkyManager.fogDebugDensity;
		}
		SkyManager.fogDensity = density;
		float num = density - 0.65f;
		if (num < 0f)
		{
			num = 0f;
		}
		SkyManager.fogLightScale = 1f - num * 1.7f;
	}

	// Token: 0x06009DB0 RID: 40368 RVA: 0x003B90A6 File Offset: 0x003B72A6
	public static float GetFogStart()
	{
		return SkyManager.fogStart;
	}

	// Token: 0x06009DB1 RID: 40369 RVA: 0x003B90AD File Offset: 0x003B72AD
	public static float GetFogEnd()
	{
		return SkyManager.fogEnd;
	}

	// Token: 0x06009DB2 RID: 40370 RVA: 0x003B90B4 File Offset: 0x003B72B4
	public static void SetFogFade(float start, float end)
	{
		float num = 1f;
		World world = GameManager.Instance.World;
		if (world != null)
		{
			num = (world.GetPrimaryPlayer().bPlayingSpawnIn ? 1f : 0.01f);
		}
		SkyManager.fogStart = Mathf.Lerp(SkyManager.fogStart, start, num);
		SkyManager.fogEnd = Mathf.Lerp(SkyManager.fogEnd, end, num);
		if (SkyManager.fogDebugDensity >= 0f)
		{
			if (SkyManager.fogDebugStart > -1000f)
			{
				SkyManager.fogStart = SkyManager.fogDebugStart;
			}
			if (SkyManager.fogDebugEnd > -1000f)
			{
				SkyManager.fogEnd = SkyManager.fogDebugEnd;
			}
		}
	}

	// Token: 0x06009DB3 RID: 40371 RVA: 0x003B9149 File Offset: 0x003B7349
	public static void SetFogDebug(float density = -1f, float start = -3.4028235E+38f, float end = -3.4028235E+38f)
	{
		SkyManager.fogDebugDensity = density;
		SkyManager.fogDebugStart = start;
		SkyManager.fogDebugEnd = end;
		SkyManager.SetFogDensity(0f);
	}

	// Token: 0x06009DB4 RID: 40372 RVA: 0x003B9167 File Offset: 0x003B7367
	public static void SetFogDebugColor(Color color = default(Color))
	{
		SkyManager.fogDebugColor = color;
		SkyManager.SetFogColor(Color.gray);
	}

	// Token: 0x06009DB5 RID: 40373 RVA: 0x003B9179 File Offset: 0x003B7379
	public static void Reset()
	{
		SkyManager.bNeedsReset = true;
	}

	// Token: 0x06009DB6 RID: 40374 RVA: 0x003B9184 File Offset: 0x003B7384
	[PublicizedFrom(EAccessModifier.Private)]
	public void ClearStatics()
	{
		SkyManager.random = null;
		SkyManager.fogLightScale = 1f;
		SkyManager.weatherLightScale = 1f;
		SkyManager.cloudTransition = 1f;
		SkyManager.cloudMainTexOld = null;
		SkyManager.cloudBlendTex = null;
		SkyManager.parent = null;
		SkyManager.sunLightT = null;
		SkyManager.sunLight = null;
		SkyManager.moonLightT = null;
		SkyManager.moonLight = null;
		SkyManager.moonSpriteT = null;
		SkyManager.mainCamera = null;
		SkyManager.moonSpriteMat = null;
		SkyManager.cloudsSphere = null;
		SkyManager.cloudsSphereMtrl = null;
		SkyManager.atmosphereSphere = null;
		SkyManager.atmosphereMtrl = null;
	}

	// Token: 0x06009DB7 RID: 40375 RVA: 0x003B920C File Offset: 0x003B740C
	[PublicizedFrom(EAccessModifier.Private)]
	public void Init()
	{
		SkyManager.random = GameRandomManager.Instance.CreateGameRandom();
		RenderSettings.fog = true;
		RenderSettings.fogMode = FogMode.Exponential;
		this.sunAxis = Vector3.forward;
		this.sunStartV = Quaternion.AngleAxis(20f, Vector3.up) * Vector3.left;
		this.moonStartV = Quaternion.AngleAxis(35f, Vector3.up) * Vector3.right;
		this.starAxis = Vector3.forward;
		if (SkyManager.parent == null)
		{
			SkyManager.parent = GameObject.Find("SkySystem(Clone)");
			if (SkyManager.parent == null)
			{
				return;
			}
		}
		if (SkyManager.sunLightT == null)
		{
			Transform transform = SkyManager.parent.transform.Find("SunLight");
			if (transform != null)
			{
				SkyManager.sunLightT = transform;
			}
		}
		if (SkyManager.sunLight == null && SkyManager.sunLightT != null)
		{
			SkyManager.sunLight = SkyManager.sunLightT.transform.GetComponent<Light>();
		}
		if (SkyManager.moonLightT == null)
		{
			SkyManager.moonLightT = SkyManager.parent.transform.Find("MoonLight");
			if (SkyManager.moonLightT)
			{
				SkyManager.moonLight = SkyManager.moonLightT.GetComponent<Light>();
			}
		}
		SkyManager.GetMaterialAndTransform("MoonSprite", ref SkyManager.moonSpriteT, ref SkyManager.moonSpriteMat);
		SkyManager.GetMaterialAndTransform("CloudsSphere", ref SkyManager.cloudsSphere, ref SkyManager.cloudsSphereMtrl);
		SkyManager.GetMaterialAndTransform("AtmosphereSphere", ref SkyManager.atmosphereSphere, ref SkyManager.atmosphereMtrl);
		MeshFilter component = SkyManager.moonSpriteT.GetComponent<MeshFilter>();
		if (component != null)
		{
			Mesh mesh = component.mesh;
			if (mesh != null)
			{
				mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 1000000f);
			}
		}
		if (SkyManager.cloudsSphereMtrl)
		{
			SkyManager.cloudsSphereMtrl.SetFloat("_CloudSpeed", this.cloudSpeed);
		}
		if (SkyManager.mainCamera == null)
		{
			this.GetMainCamera();
		}
		this.OnShadowDistanceChanged(GamePrefs.GetInt(EnumGamePrefs.OptionsGfxShadowDistance));
		SkyManager.bloodmoonDay = GameStats.GetInt(EnumGameStats.BloodMoonDay);
		ValueTuple<int, int> valueTuple = GameUtils.CalcDuskDawnHours(GameStats.GetInt(EnumGameStats.DayLightLength));
		SkyManager.duskTime = valueTuple.Item1;
		SkyManager.dawnTime = valueTuple.Item2;
	}

	// Token: 0x06009DB8 RID: 40376 RVA: 0x003B9440 File Offset: 0x003B7640
	[PublicizedFrom(EAccessModifier.Private)]
	public static void GetMaterialAndTransform(string objectName, ref Transform trans, ref Material mtrl)
	{
		Transform transform = SkyManager.parent.transform.Find(objectName);
		if (trans == null)
		{
			trans = transform;
		}
		if (mtrl == null)
		{
			MeshRenderer component = transform.GetComponent<MeshRenderer>();
			mtrl = component.material;
		}
	}

	// Token: 0x06009DB9 RID: 40377 RVA: 0x003B9484 File Offset: 0x003B7684
	[PublicizedFrom(EAccessModifier.Private)]
	public void GetMainCamera()
	{
		SkyManager.mainCamera = Camera.main;
	}

	// Token: 0x06009DBA RID: 40378 RVA: 0x003B9490 File Offset: 0x003B7690
	public static bool IsDark()
	{
		if (SkyManager.isAllTimeNight)
		{
			return true;
		}
		if (SkyManager.isAllTimeDay)
		{
			return false;
		}
		float num = SkyManager.TimeOfDay();
		return num < (float)SkyManager.dawnTime || num > (float)SkyManager.duskTime;
	}

	// Token: 0x06009DBB RID: 40379 RVA: 0x003B94C9 File Offset: 0x003B76C9
	public static float GetDawnTime()
	{
		return (float)SkyManager.dawnTime;
	}

	// Token: 0x06009DBC RID: 40380 RVA: 0x003B94D1 File Offset: 0x003B76D1
	public static float GetDawnTimeAsMinutes()
	{
		return (float)SkyManager.dawnTime / 24f * (float)GamePrefs.GetInt(EnumGamePrefs.DayNightLength);
	}

	// Token: 0x06009DBD RID: 40381 RVA: 0x003B94E8 File Offset: 0x003B76E8
	public static float GetDuskTime()
	{
		return (float)SkyManager.duskTime;
	}

	// Token: 0x06009DBE RID: 40382 RVA: 0x003B94F0 File Offset: 0x003B76F0
	public static float GetDuskTimeAsMinutes()
	{
		return (float)SkyManager.duskTime / 24f * (float)GamePrefs.GetInt(EnumGamePrefs.DayNightLength);
	}

	// Token: 0x06009DBF RID: 40383 RVA: 0x003B9508 File Offset: 0x003B7708
	[PublicizedFrom(EAccessModifier.Private)]
	public static float CalcDayPercent()
	{
		if (SkyManager.isAllTimeNight)
		{
			return 1f;
		}
		float num;
		if (SkyManager.worldRotation < 0.5f)
		{
			num = Mathf.Pow(1f - Utils.FastAbs(0.25f - SkyManager.worldRotation) * 4f, 0.6f);
			num = num * 0.68f + 0.5f;
			if (num > 1f)
			{
				num = 1f;
			}
		}
		else
		{
			num = Mathf.Pow(1f - Utils.FastAbs(0.75f - SkyManager.worldRotation) * 4f, 0.6f);
			num = 0.5f - num * 0.68f;
			if (num < 0f)
			{
				num = 0f;
			}
		}
		return num;
	}

	// Token: 0x06009DC0 RID: 40384 RVA: 0x003B95B8 File Offset: 0x003B77B8
	public static void TriggerLightning(Vector3 _position)
	{
		if (!SkyManager.triggerLightning)
		{
			SkyManager.lightningFlashes = SkyManager.random.RandomRange(2, 6);
			float num = Time.time;
			for (int i = 0; i < SkyManager.lightningFlashes; i++)
			{
				float num2 = SkyManager.random.RandomRange(0.1f, 0.21f);
				num += num2;
				SkyManager.lightningEndTimes[i] = num;
			}
			SkyManager.lightningIndex = -1;
			SkyManager.lightningDir.x = SkyManager.random.RandomFloat * 360f;
			SkyManager.lightningDir.y = (float)SkyManager.random.RandomRange(8, 20);
			SkyManager.triggerLightning = true;
		}
	}

	// Token: 0x06009DC1 RID: 40385 RVA: 0x003B9658 File Offset: 0x003B7858
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateSunMoonAngles()
	{
		if (!SkyManager.sunLight)
		{
			return;
		}
		if (!SkyManager.moonLight)
		{
			return;
		}
		if (Time.time - SkyManager.worldRotationTime >= 0.2f || SkyManager.bUpdateSunMoonNow)
		{
			bool flag = SkyManager.IsBloodMoonVisible();
			if (SkyManager.isAllTimeNight && !flag)
			{
				SkyManager.worldRotationTarget = 0.75f;
			}
			else if (SkyManager.isAllTimeDay && !flag)
			{
				SkyManager.worldRotationTarget = 0.25f;
			}
			else
			{
				SkyManager.worldRotationTime = Time.time;
				SkyManager.bUpdateSunMoonNow = false;
				float num = SkyManager.TimeOfDay();
				if (num >= (float)SkyManager.dawnTime && num < (float)SkyManager.duskTime)
				{
					SkyManager.worldRotationTarget = (num - (float)SkyManager.dawnTime) / (float)(SkyManager.duskTime - SkyManager.dawnTime);
				}
				else
				{
					float num2 = (float)(24 - SkyManager.duskTime);
					float num3 = num2 + (float)SkyManager.dawnTime;
					if (num < (float)SkyManager.dawnTime)
					{
						SkyManager.worldRotationTarget = (num2 + num) / num3;
					}
					else
					{
						SkyManager.worldRotationTarget = (num - (float)SkyManager.duskTime) / num3;
					}
					SkyManager.worldRotationTarget += 1f;
				}
				SkyManager.worldRotationTarget *= 0.5f;
				SkyManager.worldRotationTarget = Mathf.Clamp01(SkyManager.worldRotationTarget);
			}
		}
		float num4 = SkyManager.worldRotationTarget - SkyManager.worldRotation;
		float num5 = SkyManager.worldRotationTarget;
		if (num4 < -0.5f)
		{
			num5 += 1f;
		}
		else if (num4 > 0.5f)
		{
			num5 -= 1f;
		}
		SkyManager.worldRotation = Mathf.Lerp(SkyManager.worldRotation, num5, 0.05f);
		if (SkyManager.worldRotation < 0f)
		{
			SkyManager.worldRotation += 1f;
		}
		else if (SkyManager.worldRotation >= 1f)
		{
			SkyManager.worldRotation -= 1f;
		}
		SkyManager.dayPercent = SkyManager.CalcDayPercent();
		float num6 = SkyManager.worldRotation * 360f;
		SkyManager.sunDirV = Quaternion.AngleAxis(num6, this.sunAxis) * this.sunStartV;
		SkyManager.moonLightRot = Quaternion.LookRotation(Quaternion.AngleAxis(num6, this.sunAxis) * this.moonStartV);
		float num7 = SkyManager.worldRotation * 360f;
		if (SkyManager.sunIntensity >= 0.001f)
		{
			if (num7 < 14f)
			{
				num7 = 14f;
			}
			if (num7 > 166f)
			{
				num7 = 166f;
			}
			Vector3 eulerAngles = Quaternion.LookRotation(Quaternion.AngleAxis(num7, this.sunAxis) * this.sunStartV).eulerAngles;
			SkyManager.sunLightT.localEulerAngles = eulerAngles;
			SkyManager.sunLight.shadowStrength = 1f;
			SkyManager.sunLight.shadows = ((SkyManager.sunIntensity > 0f) ? LightShadows.Soft : LightShadows.None);
			SkyManager.moonLight.enabled = false;
		}
		else if (SkyManager.moonLightColor.grayscale > 0f)
		{
			if (num7 < 166f)
			{
				num7 = 166f;
			}
			if (num7 > 346f)
			{
				num7 = 346f;
			}
			Vector3 eulerAngles2 = Quaternion.LookRotation(Quaternion.AngleAxis(num7, this.sunAxis) * this.moonStartV).eulerAngles;
			SkyManager.moonLightT.localEulerAngles = eulerAngles2;
			float num8 = SkyManager.fogLightScale * SkyManager.moonBright;
			float num9 = GamePrefs.GetFloat(EnumGamePrefs.OptionsGfxBrightness) * 2f;
			num8 *= Utils.FastLerp(0.2f, 1f, num9);
			SkyManager.moonLight.intensity = num8;
			SkyManager.moonLight.color = SkyManager.moonLightColor;
			SkyManager.moonLight.shadowStrength = 1f;
			SkyManager.moonLight.shadows = ((num8 > 0f) ? LightShadows.Soft : LightShadows.None);
			SkyManager.moonLight.enabled = true;
		}
		else
		{
			SkyManager.moonLight.enabled = false;
		}
		SkyManager.sunMoonDirV = SkyManager.sunDirV;
		if (SkyManager.sunIntensity < 0.001f)
		{
			SkyManager.sunMoonDirV = SkyManager.moonLightRot * Vector3.forward;
		}
		if (!GameManager.IsDedicatedServer && SkyManager.mainCamera)
		{
			if (SkyManager.moonSpriteT)
			{
				Transform transform = SkyManager.mainCamera.transform;
				Vector3 vector = SkyManager.moonLightRot * Vector3.forward * -45000f + transform.position;
				SkyManager.moonSpriteT.SetPositionAndRotation(vector, Quaternion.LookRotation(vector, Vector3.up));
				float num10 = 6857.143f;
				if (SkyManager.IsBloodMoonVisible())
				{
					num10 *= 1.3f;
				}
				SkyManager.moonSpriteT.localScale = new Vector3(num10, num10, num10);
			}
			this.UpdateSunShaftSettings();
		}
		SkyManager.atmosphereSphere.Rotate(this.starAxis, SkyManager.worldRotation * 0.004f);
		if (this.bUpdateShaders && SkyManager.cloudsSphereMtrl)
		{
			SkyManager.cloudsSphereMtrl.SetVector("_SunDir", SkyManager.sunDirV);
			SkyManager.cloudsSphereMtrl.SetVector("_SunMoonDir", SkyManager.sunMoonDirV);
		}
	}

	// Token: 0x06009DC2 RID: 40386 RVA: 0x003B9B20 File Offset: 0x003B7D20
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateSunShaftSettings()
	{
		Vector3 vector = SkyManager.GetSunDirection();
		if (vector.y <= 0.09f)
		{
			SkyManager.sunShaftSettings.sunShaftIntensity = 0.1f + SkyManager.GetFogDensity() * 0.5f;
			SkyManager.sunShaftSettings.sunColor = SkyManager.GetSunLightColor();
			SkyManager.sunShaftSettings.sunThreshold = new Color(0.87f, 0.74f, 0.65f, 1f);
		}
		else
		{
			vector = SkyManager.GetMoonDirection();
			SkyManager.sunShaftSettings.sunShaftIntensity = 0.06f + SkyManager.GetFogDensity() * 0.85f;
			SkyManager.sunShaftSettings.sunColor = SkyManager.GetMoonLightColor();
			SkyManager.sunShaftSettings.sunThreshold = new Color(0.8f, 0.6f, 0.6f, 1f);
		}
		SkyManager.sunShaftSettings.sunPosition = vector * -100000f;
	}

	// Token: 0x06009DC3 RID: 40387 RVA: 0x003B9BF7 File Offset: 0x003B7DF7
	public static SunShaftsEffect.SunSettings GetSunShaftSettings()
	{
		return SkyManager.sunShaftSettings;
	}

	// Token: 0x06009DC4 RID: 40388 RVA: 0x003B9BFE File Offset: 0x003B7DFE
	public static float GetLuma(Color color)
	{
		return 0.2126f * color.r + 0.7152f * color.g + 0.0722f * color.b;
	}

	// Token: 0x06009DC5 RID: 40389 RVA: 0x003B9C26 File Offset: 0x003B7E26
	[PublicizedFrom(EAccessModifier.Private)]
	public bool VerifyCamera()
	{
		if (GameManager.IsDedicatedServer)
		{
			return true;
		}
		if (SkyManager.mainCamera == null)
		{
			this.GetMainCamera();
			if (SkyManager.mainCamera == null)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06009DC6 RID: 40390 RVA: 0x003B9C54 File Offset: 0x003B7E54
	[PublicizedFrom(EAccessModifier.Private)]
	public bool VerifyValidMaterials()
	{
		return !(SkyManager.atmosphereMtrl == null) && !(SkyManager.cloudsSphere == null) && !(SkyManager.atmosphereSphere == null) && !(SkyManager.moonLight == null) && !(SkyManager.sunLight == null) && !(SkyManager.moonSpriteT == null) && !(SkyManager.moonSpriteMat == null);
	}

	// Token: 0x06009DC7 RID: 40391 RVA: 0x003B9CCB File Offset: 0x003B7ECB
	[PublicizedFrom(EAccessModifier.Private)]
	public void ResetIfNeeded()
	{
		if (SkyManager.bNeedsReset)
		{
			this.ClearStatics();
			this.Init();
			SkyManager.bNeedsReset = false;
		}
	}

	// Token: 0x06009DC8 RID: 40392 RVA: 0x003B9CE8 File Offset: 0x003B7EE8
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateFogShader()
	{
		this.fogParams.x = SkyManager.GetFogStart();
		this.fogParams.y = SkyManager.GetFogEnd();
		this.fogParams.z = 1f;
		this.fogParams.w = Mathf.Pow(SkyManager.GetFogDensity(), 2f);
	}

	// Token: 0x06009DC9 RID: 40393 RVA: 0x003B9D40 File Offset: 0x003B7F40
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateShaderGlobals()
	{
		Color color = SkyManager.GetFogColor();
		float num = SkyManager.GetFogDensity();
		num *= num * num;
		RenderSettings.fogDensity = num;
		RenderSettings.fogColor = color;
		Shader.SetGlobalVector("_FogParams", new Vector4(num, this.fogParams.x, this.fogParams.y, 0f));
		Shader.SetGlobalColor("_FogColor", color.linear);
		Shader.SetGlobalVector("SunColor", SkyManager.sunLight.color);
		Shader.SetGlobalVector("FogColor", color);
		Shader.SetGlobalFloat("_HighResViewDistance", (float)GameUtils.GetViewDistance() * 16f);
		Shader.SetGlobalFloat("_DayPercent", SkyManager.dayPercent);
	}

	// Token: 0x06009DCA RID: 40394 RVA: 0x003B9DF8 File Offset: 0x003B7FF8
	public void Update()
	{
		if (SkyManager.parent == null)
		{
			this.Init();
			if (SkyManager.parent == null)
			{
				return;
			}
		}
		SkyManager.sMaxSunIntensity = this.maxSunIntensity;
		if (SkyManager.mainCamera)
		{
			float num = SkyManager.fogDensity + 0.001f;
			num *= SkyManager.fogDensity * SkyManager.fogDensity;
			SkyManager.mainCamera.farClipPlane = Utils.FastClamp(6f / num + 400f, 200f, 2800f);
		}
		int num2 = Time.frameCount & 1;
		this.bUpdateShaders = num2 == 0 || SkyManager.triggerLightning;
		if (!this.VerifyCamera())
		{
			return;
		}
		this.ResetIfNeeded();
		this.UpdateSunMoonAngles();
		if (GameManager.IsDedicatedServer)
		{
			return;
		}
		float magnitude = (SkyManager.parent.transform.position - SkyManager.mainCamera.transform.position).magnitude;
		if (magnitude < 0.001f)
		{
			this.bMovingSky = false;
			this.targetSkyPosition = SkyManager.mainCamera.transform.position;
		}
		else if (!this.bMovingSky && magnitude > 5f)
		{
			this.bMovingSky = true;
			this.targetSkyPosition = SkyManager.mainCamera.transform.position;
		}
		if (this.bMovingSky)
		{
			SkyManager.parent.transform.position = Vector3.Lerp(SkyManager.parent.transform.position, this.targetSkyPosition, Mathf.Clamp01(Time.deltaTime * 10f));
		}
		this.UpdateFogShader();
		if (!this.bUpdateShaders)
		{
			return;
		}
		if (!this.VerifyValidMaterials())
		{
			return;
		}
		SkyManager.sSunFadeHeight = this.sunFadeHeight;
		this.UpdateShaderGlobals();
		bool flag = SkyManager.IsBloodMoonVisible();
		int num3 = 0;
		Color color = new Color(1f, 0.14f, 0.05f) * 1.5f;
		if (!flag)
		{
			num3 = (int)(SkyManager.dayCount + 5.5f) % 7;
			color = Color.Lerp(Color.white, SkyManager.moonLightColor, 0.2f);
		}
		float num4 = SkyManager.sMoonPhases[num3] * 3.1415927f;
		Vector3 vector = new Vector3(-Mathf.Sin(num4), 0f, Mathf.Cos(num4));
		SkyManager.moonSpriteMat.SetVector("_LightDir", vector);
		SkyManager.moonSpriteMat.SetColor("_Color", color);
		SkyManager.moonBright = SkyManager.sMoonBrights[num3];
		float cloudThickness = WeatherManager.GetCloudThickness();
		if (cloudThickness > 45f && GamePrefs.GetFloat(EnumGamePrefs.OptionsGfxBrightness) < 0.49f)
		{
			SkyManager.moonBright *= Utils.FastLerp(1f, 0.15f, (cloudThickness - 45f) / 55f);
		}
		float num5 = ((Mathf.Pow(SkyManager.moonLightColor.grayscale, 0.45f) - 0.5f) * 0.5f + 0.5f) * SkyManager.moonBright;
		float num6 = Mathf.Max(SkyManager.sunIntensity, num5);
		if (this.bUpdateShaders)
		{
			SkyManager.atmosphereMtrl.SetColor("_SkyColor", SkyManager.GetSkyColor());
			SkyManager.atmosphereMtrl.SetFloat("_Stars", SkyManager.starIntensity);
			SkyManager.atmosphereMtrl.SetVector("_SunDir", SkyManager.sunDirV);
			SkyManager.cloudsSphereMtrl.SetFloat("_CloudTransition", SkyManager.cloudTransition);
			SkyManager.cloudsSphereMtrl.SetFloat("_LightIntensity", num6);
			SkyManager.cloudsSphereMtrl.SetColor("_SkyColor", SkyManager.GetSkyColor());
			SkyManager.cloudsSphereMtrl.SetColor("_SunColor", SkyManager.sunLight.color);
			SkyManager.cloudsSphereMtrl.SetVector("_SunDir", SkyManager.sunDirV);
			SkyManager.cloudsSphereMtrl.SetVector("_SunMoonDir", SkyManager.sunMoonDirV);
			color.a = SkyManager.moonBright * 0.4f;
			SkyManager.cloudsSphereMtrl.SetColor("_MoonColor", color);
		}
		if (SkyManager.cloudMainTex != SkyManager.cloudMainTexOld)
		{
			SkyManager.cloudsSphereMtrl.SetTexture("_CloudMainTex", SkyManager.cloudMainTex);
			SkyManager.cloudMainTexOld = SkyManager.cloudMainTex;
		}
		if (SkyManager.cloudBlendTex != SkyManager.cloudBlendTexOld)
		{
			SkyManager.cloudsSphereMtrl.SetTexture("_CloudBlendTex", SkyManager.cloudBlendTex);
			SkyManager.cloudBlendTexOld = SkyManager.cloudBlendTex;
		}
		if (SkyManager.triggerLightning)
		{
			float deltaTime = Time.deltaTime;
			Light light = SkyManager.moonLight;
			SkyManager.lightningIntensity -= 4f * deltaTime;
			if (SkyManager.lightningIndex < 0 || Time.time >= SkyManager.lightningEndTimes[SkyManager.lightningIndex])
			{
				SkyManager.lightningIndex++;
				SkyManager.lightningIntensity = 0.75f + SkyManager.random.RandomFloat * 0.25f;
				SkyManager.lightningDir.x = SkyManager.lightningDir.x + SkyManager.random.RandomRange(-20f, 20f);
				SkyManager.lightningDirVel.x = SkyManager.random.RandomRange(-50f, 50f);
				SkyManager.lightningDir.y = SkyManager.lightningDir.y + (float)SkyManager.random.RandomRange(-8, 8);
			}
			if (SkyManager.lightningIndex >= SkyManager.lightningFlashes)
			{
				SkyManager.triggerLightning = false;
				SkyManager.cloudsSphereMtrl.SetFloat("_Lightning", 0f);
			}
			else
			{
				SkyManager.lightningIntensity = Utils.FastMax(0f, SkyManager.lightningIntensity);
				SkyManager.cloudsSphereMtrl.SetFloat("_Lightning", SkyManager.lightningIntensity);
				SkyManager.lightningDir += SkyManager.lightningDirVel * deltaTime;
				float num7 = SkyManager.lightningDir.x * 0.017453292f;
				Vector3 vector2;
				vector2.x = Mathf.Sin(num7);
				vector2.y = Mathf.Sin(SkyManager.lightningDir.y * 0.017453292f) * 1.5f;
				vector2.z = Mathf.Cos(num7);
				SkyManager.cloudsSphereMtrl.SetVector("_LightningDir", vector2.normalized);
				Transform transform = GameManager.Instance.World.GetPrimaryPlayer().transform;
				Transform transform2 = light.transform;
				Vector3 vector3 = vector2 * 200f;
				vector3.y = 300f;
				transform2.position = transform.position + vector3;
				transform2.LookAt(transform);
				light.color = Color.white;
				light.intensity = SkyManager.lightningIntensity * 0.9f;
				light.shadows = LightShadows.Hard;
				light.shadowStrength = 1f;
				light.enabled = true;
			}
		}
		if (SkyManager.frustumShadows == null)
		{
			SkyManager.frustumShadows = SkyManager.mainCamera.GetComponent<NGSS_FrustumShadows_7DTD>();
		}
		if (SkyManager.frustumShadows != null)
		{
			SkyManager.frustumShadows.mainShadowsLight = ((!SkyManager.IsDark()) ? SkyManager.sunLight : SkyManager.moonLight);
		}
		SkyManager.WasBloodMoon = flag;
	}

	// Token: 0x06009DCB RID: 40395 RVA: 0x003BA499 File Offset: 0x003B8699
	public static float GetSunAngle()
	{
		return SkyManager.sunDirV.y;
	}

	// Token: 0x06009DCC RID: 40396 RVA: 0x003BA4A5 File Offset: 0x003B86A5
	public static Vector3 GetSunDirection()
	{
		return SkyManager.sunDirV;
	}

	// Token: 0x06009DCD RID: 40397 RVA: 0x003BA4AC File Offset: 0x003B86AC
	public static float GetSunPercent()
	{
		return -SkyManager.sunDirV.y;
	}

	// Token: 0x06009DCE RID: 40398 RVA: 0x003BA4B9 File Offset: 0x003B86B9
	public static Vector3 GetSunLightDirection()
	{
		if (!(SkyManager.sunLight == null))
		{
			return SkyManager.sunLightT.forward;
		}
		return Vector3.down;
	}

	// Token: 0x06009DCF RID: 40399 RVA: 0x003BA4D8 File Offset: 0x003B86D8
	public static float GetSunIntensity()
	{
		return SkyManager.sunIntensity;
	}

	// Token: 0x06009DD0 RID: 40400 RVA: 0x003BA4DF File Offset: 0x003B86DF
	public static Color GetSunLightColor()
	{
		if (SkyManager.sunLight)
		{
			return SkyManager.sunLight.color;
		}
		return Color.black;
	}

	// Token: 0x06009DD1 RID: 40401 RVA: 0x003BA4FD File Offset: 0x003B86FD
	public static void SetSunColor(Color color)
	{
		if (SkyManager.sunLight != null)
		{
			SkyManager.sunLight.color = color;
		}
	}

	// Token: 0x06009DD2 RID: 40402 RVA: 0x003BA518 File Offset: 0x003B8718
	public static void SetSunIntensity(float _intensity)
	{
		SkyManager.sunIntensity = _intensity;
		float sunAngle = SkyManager.GetSunAngle();
		if (sunAngle >= -SkyManager.sSunFadeHeight)
		{
			SkyManager.sunIntensity = -sunAngle * 10f * SkyManager.sunIntensity * (float)((sunAngle < 0f) ? 1 : 0);
		}
		SkyManager.sunIntensity = Mathf.Clamp(SkyManager.sunIntensity, 0f, SkyManager.sMaxSunIntensity);
		SkyManager.sunIntensity *= 1.5f;
		if (SkyManager.sunLight)
		{
			SkyManager.sunLight.intensity = SkyManager.sunIntensity * SkyManager.fogLightScale * SkyManager.weatherLightScale;
		}
	}

	// Token: 0x06009DD3 RID: 40403 RVA: 0x003BA5AB File Offset: 0x003B87AB
	public static void SetWeatherLightScale(float _scale)
	{
		SkyManager.weatherLightScale = _scale;
	}

	// Token: 0x06009DD4 RID: 40404 RVA: 0x003BA5B3 File Offset: 0x003B87B3
	public static float GetMoonAmbientScale(float add, float mpy)
	{
		return Utils.FastLerp(add + SkyManager.moonBright * mpy, 1f, SkyManager.dayPercent * 3.030303f);
	}

	// Token: 0x06009DD5 RID: 40405 RVA: 0x003BA5D3 File Offset: 0x003B87D3
	public static float GetMoonBrightness()
	{
		return SkyManager.moonLightColor.grayscale * SkyManager.moonBright;
	}

	// Token: 0x06009DD6 RID: 40406 RVA: 0x003BA5E5 File Offset: 0x003B87E5
	public static Color GetMoonLightColor()
	{
		return SkyManager.moonLightColor;
	}

	// Token: 0x06009DD7 RID: 40407 RVA: 0x003BA5EC File Offset: 0x003B87EC
	public static Vector3 GetMoonDirection()
	{
		return SkyManager.moonLightRot * Vector3.forward;
	}

	// Token: 0x06009DD8 RID: 40408 RVA: 0x003BA5FD File Offset: 0x003B87FD
	public static void SetMoonLightColor(Color color)
	{
		SkyManager.moonLightColor = color;
	}

	// Token: 0x06009DD9 RID: 40409 RVA: 0x003BA605 File Offset: 0x003B8805
	public static float GetWorldRotation()
	{
		return SkyManager.worldRotation;
	}

	// Token: 0x0400771D RID: 30493
	public static SkyManager skyManager;

	// Token: 0x0400771E RID: 30494
	public static float dayCount;

	// Token: 0x0400771F RID: 30495
	public static bool indoorFogOn = true;

	// Token: 0x04007720 RID: 30496
	public static bool isAllTimeDay = false;

	// Token: 0x04007721 RID: 30497
	public static bool isAllTimeNight = false;

	// Token: 0x04007722 RID: 30498
	public static Material atmosphereMtrl;

	// Token: 0x04007723 RID: 30499
	public static Transform atmosphereSphere;

	// Token: 0x04007724 RID: 30500
	public static Material cloudsSphereMtrl;

	// Token: 0x04007725 RID: 30501
	public static Transform cloudsSphere;

	// Token: 0x04007726 RID: 30502
	public static bool bUpdateSunMoonNow = false;

	// Token: 0x04007727 RID: 30503
	public static float sSunFadeHeight = 0.1f;

	// Token: 0x04007728 RID: 30504
	public static float dayPercent;

	// Token: 0x04007729 RID: 30505
	public static GameRandom random;

	// Token: 0x0400772A RID: 30506
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float sMaxSunIntensity = 0.7f;

	// Token: 0x0400772B RID: 30507
	public float maxSunIntensity = 0.7f;

	// Token: 0x0400772C RID: 30508
	public float sunHeight = 0.1f;

	// Token: 0x0400772D RID: 30509
	public float moonHeight = 0.095f;

	// Token: 0x0400772E RID: 30510
	public float sunFadeHeight = 0.1f;

	// Token: 0x0400772F RID: 30511
	public float cloudSpeed = 0.05f;

	// Token: 0x04007730 RID: 30512
	public float ShowFogDensity;

	// Token: 0x04007731 RID: 30513
	public Color ShowSkyColor;

	// Token: 0x04007732 RID: 30514
	public Color ShowFogColor;

	// Token: 0x04007733 RID: 30515
	public float maxFarClippingPlane = 1500f;

	// Token: 0x04007734 RID: 30516
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int dawnTime = 4;

	// Token: 0x04007735 RID: 30517
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int duskTime = 22;

	// Token: 0x04007736 RID: 30518
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static ulong timeOfDay;

	// Token: 0x04007737 RID: 30519
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int bloodmoonDay = 7;

	// Token: 0x04007738 RID: 30520
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float cloudTransition = 0f;

	// Token: 0x04007739 RID: 30521
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float fogDensity;

	// Token: 0x0400773A RID: 30522
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float fogLightScale;

	// Token: 0x0400773B RID: 30523
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float fogStart = 20f;

	// Token: 0x0400773C RID: 30524
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float fogEnd = 80f;

	// Token: 0x0400773D RID: 30525
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float fogDebugDensity = -1f;

	// Token: 0x0400773E RID: 30526
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float fogDebugStart;

	// Token: 0x0400773F RID: 30527
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float fogDebugEnd;

	// Token: 0x04007740 RID: 30528
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Color fogDebugColor;

	// Token: 0x04007741 RID: 30529
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float worldRotationTime = 0f;

	// Token: 0x04007742 RID: 30530
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float worldRotation = 0f;

	// Token: 0x04007743 RID: 30531
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float worldRotationTarget = 0f;

	// Token: 0x04007744 RID: 30532
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float sunIntensity = 1f;

	// Token: 0x04007745 RID: 30533
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float starIntensity = 1f;

	// Token: 0x04007746 RID: 30534
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float weatherLightScale;

	// Token: 0x04007747 RID: 30535
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static bool bNeedsReset;

	// Token: 0x04007748 RID: 30536
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Color SkyColor;

	// Token: 0x04007749 RID: 30537
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Color fogColor;

	// Token: 0x0400774A RID: 30538
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Transform sunLightT;

	// Token: 0x0400774B RID: 30539
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Light sunLight;

	// Token: 0x0400774C RID: 30540
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Vector3 sunDirV;

	// Token: 0x0400774D RID: 30541
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Vector3 sunMoonDirV;

	// Token: 0x0400774E RID: 30542
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly float[] sMoonPhases = new float[] { 0.05f, 0.35f, 0.55f, 0.7f, 1.4f, 1.63f, 1.82f };

	// Token: 0x0400774F RID: 30543
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly float[] sMoonBrights = new float[] { 1f, 0.65f, 0.45f, 0.25f, 0.4f, 0.6f, 0.9f };

	// Token: 0x04007750 RID: 30544
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Transform moonLightT;

	// Token: 0x04007751 RID: 30545
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Light moonLight;

	// Token: 0x04007752 RID: 30546
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Transform moonSpriteT;

	// Token: 0x04007753 RID: 30547
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Material moonSpriteMat;

	// Token: 0x04007754 RID: 30548
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float moonBright;

	// Token: 0x04007755 RID: 30549
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Color moonLightColor;

	// Token: 0x04007756 RID: 30550
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Quaternion moonLightRot;

	// Token: 0x04007757 RID: 30551
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Texture cloudMainTex;

	// Token: 0x04007758 RID: 30552
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Texture cloudMainTexOld;

	// Token: 0x04007759 RID: 30553
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Texture cloudBlendTex;

	// Token: 0x0400775A RID: 30554
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Texture cloudBlendTexOld;

	// Token: 0x0400775B RID: 30555
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static GameObject parent;

	// Token: 0x0400775C RID: 30556
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Camera mainCamera;

	// Token: 0x0400775D RID: 30557
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static NGSS_FrustumShadows_7DTD frustumShadows;

	// Token: 0x0400775E RID: 30558
	public static bool WasBloodMoon = false;

	// Token: 0x0400775F RID: 30559
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cWorldRotationUpdateFreq = 0.2f;

	// Token: 0x04007760 RID: 30560
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cStarRotationSpeed = 0.004f;

	// Token: 0x04007761 RID: 30561
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bUpdateShaders;

	// Token: 0x04007762 RID: 30562
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector4 fogParams;

	// Token: 0x04007763 RID: 30563
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 sunAxis;

	// Token: 0x04007764 RID: 30564
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 sunStartV;

	// Token: 0x04007765 RID: 30565
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 moonStartV;

	// Token: 0x04007766 RID: 30566
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 starAxis;

	// Token: 0x04007767 RID: 30567
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static bool triggerLightning;

	// Token: 0x04007768 RID: 30568
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int lightningFlashes;

	// Token: 0x04007769 RID: 30569
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float[] lightningEndTimes = new float[] { 1f, 2f, 3f, 4f, 5f };

	// Token: 0x0400776A RID: 30570
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int lightningIndex;

	// Token: 0x0400776B RID: 30571
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Vector2 lightningDir;

	// Token: 0x0400776C RID: 30572
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Vector2 lightningDirVel;

	// Token: 0x0400776D RID: 30573
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float lightningIntensity;

	// Token: 0x0400776E RID: 30574
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static SunShaftsEffect.SunSettings sunShaftSettings;

	// Token: 0x0400776F RID: 30575
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 targetSkyPosition;

	// Token: 0x04007770 RID: 30576
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bMovingSky = true;
}

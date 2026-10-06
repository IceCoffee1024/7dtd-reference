using System;
using System.Runtime.CompilerServices;
using PI.NGSS;
using UnityEngine;

// Token: 0x0200138D RID: 5005
public class SkyManager : MonoBehaviour
{
	// Token: 0x170012D7 RID: 4823
	// (get) Token: 0x06009D42 RID: 40258 RVA: 0x003B6956 File Offset: 0x003B4B56
	public static Transform SunLightT
	{
		get
		{
			return SkyManager.sunLightT;
		}
	}

	// Token: 0x06009D43 RID: 40259 RVA: 0x003B695D File Offset: 0x003B4B5D
	public static bool IsBloodMoonVisible()
	{
		return GameUtils.IsBloodMoonTime(new ValueTuple<int, int>(SkyManager.duskTime - 4, SkyManager.dawnTime + 2), (int)SkyManager.TimeOfDay(), SkyManager.bloodmoonDay, (int)SkyManager.dayCount);
	}

	// Token: 0x06009D44 RID: 40260 RVA: 0x003B6988 File Offset: 0x003B4B88
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

	// Token: 0x06009D45 RID: 40261 RVA: 0x003B69FD File Offset: 0x003B4BFD
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		GameOptionsManager.ShadowDistanceChanged += this.OnShadowDistanceChanged;
		GameStats.OnChangedDelegates += this.OnGameStatsChanged;
	}

	// Token: 0x06009D46 RID: 40262 RVA: 0x003B6A21 File Offset: 0x003B4C21
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDestroy()
	{
		GameOptionsManager.ShadowDistanceChanged -= this.OnShadowDistanceChanged;
		GameStats.OnChangedDelegates -= this.OnGameStatsChanged;
	}

	// Token: 0x06009D47 RID: 40263 RVA: 0x003B6A48 File Offset: 0x003B4C48
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

	// Token: 0x06009D48 RID: 40264 RVA: 0x003B6AA1 File Offset: 0x003B4CA1
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

	// Token: 0x06009D49 RID: 40265 RVA: 0x003B6ADB File Offset: 0x003B4CDB
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Start()
	{
		SkyManager.Reset();
	}

	// Token: 0x06009D4A RID: 40266 RVA: 0x003B6AE2 File Offset: 0x003B4CE2
	public static void Loaded(GameObject _obj)
	{
		_obj.SetActive(true);
		_obj.transform.parent = GameManager.Instance.transform;
		SkyManager.skyManager = _obj.GetComponent<SkyManager>();
		SkyManager.Reset();
	}

	// Token: 0x06009D4B RID: 40267 RVA: 0x003B6B10 File Offset: 0x003B4D10
	public static void Cleanup()
	{
		if (SkyManager.skyManager != null)
		{
			global::UnityEngine.Object.DestroyImmediate(SkyManager.skyManager.gameObject);
		}
	}

	// Token: 0x06009D4C RID: 40268 RVA: 0x003B6B30 File Offset: 0x003B4D30
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

	// Token: 0x06009D4D RID: 40269 RVA: 0x003B6BB7 File Offset: 0x003B4DB7
	public static Color GetSkyColor()
	{
		return SkyManager.SkyColor;
	}

	// Token: 0x06009D4E RID: 40270 RVA: 0x003B6BBE File Offset: 0x003B4DBE
	public static void SetSkyColor(Color c)
	{
		SkyManager.SkyColor = c;
	}

	// Token: 0x06009D4F RID: 40271 RVA: 0x003B6BC6 File Offset: 0x003B4DC6
	public static void SetGameTime(ulong _time)
	{
		SkyManager.dayCount = _time / 24000f + 1f;
		SkyManager.timeOfDay = _time;
	}

	// Token: 0x06009D50 RID: 40272 RVA: 0x003B6BE2 File Offset: 0x003B4DE2
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float TimeOfDay()
	{
		return SkyManager.timeOfDay % 24000UL / 1000f;
	}

	// Token: 0x06009D51 RID: 40273 RVA: 0x003B6BF8 File Offset: 0x003B4DF8
	public static float GetTimeOfDayAsMinutes()
	{
		return SkyManager.TimeOfDay() / 24f * (float)GamePrefs.GetInt(EnumGamePrefs.DayNightLength);
	}

	// Token: 0x06009D52 RID: 40274 RVA: 0x003B6C0E File Offset: 0x003B4E0E
	public static void SetCloudTextures(Texture _mainTex, Texture _blendTex)
	{
		SkyManager.cloudMainTex = _mainTex;
		SkyManager.cloudBlendTex = _blendTex;
	}

	// Token: 0x06009D53 RID: 40275 RVA: 0x003B6C1C File Offset: 0x003B4E1C
	public static void SetCloudTransition(float t)
	{
		SkyManager.cloudTransition = t;
	}

	// Token: 0x06009D54 RID: 40276 RVA: 0x003B6C24 File Offset: 0x003B4E24
	public static Color GetFogColor()
	{
		return SkyManager.fogColor;
	}

	// Token: 0x06009D55 RID: 40277 RVA: 0x003B6C2B File Offset: 0x003B4E2B
	public static void SetFogColor(Color c)
	{
		if (SkyManager.fogDebugColor.a > 0f)
		{
			c = SkyManager.fogDebugColor;
		}
		SkyManager.fogColor = c;
	}

	// Token: 0x06009D56 RID: 40278 RVA: 0x003B6C4B File Offset: 0x003B4E4B
	public static float GetFogDensity()
	{
		return SkyManager.fogDensity;
	}

	// Token: 0x06009D57 RID: 40279 RVA: 0x003B6C54 File Offset: 0x003B4E54
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

	// Token: 0x06009D58 RID: 40280 RVA: 0x003B6CA2 File Offset: 0x003B4EA2
	public static float GetFogStart()
	{
		return SkyManager.fogStart;
	}

	// Token: 0x06009D59 RID: 40281 RVA: 0x003B6CA9 File Offset: 0x003B4EA9
	public static float GetFogEnd()
	{
		return SkyManager.fogEnd;
	}

	// Token: 0x06009D5A RID: 40282 RVA: 0x003B6CB0 File Offset: 0x003B4EB0
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

	// Token: 0x06009D5B RID: 40283 RVA: 0x003B6D45 File Offset: 0x003B4F45
	public static void SetFogDebug(float density = -1f, float start = -3.4028235E+38f, float end = -3.4028235E+38f)
	{
		SkyManager.fogDebugDensity = density;
		SkyManager.fogDebugStart = start;
		SkyManager.fogDebugEnd = end;
		SkyManager.SetFogDensity(0f);
	}

	// Token: 0x06009D5C RID: 40284 RVA: 0x003B6D63 File Offset: 0x003B4F63
	public static void SetFogDebugColor(Color color = default(Color))
	{
		SkyManager.fogDebugColor = color;
		SkyManager.SetFogColor(Color.gray);
	}

	// Token: 0x06009D5D RID: 40285 RVA: 0x003B6D75 File Offset: 0x003B4F75
	public static void Reset()
	{
		SkyManager.bNeedsReset = true;
	}

	// Token: 0x06009D5E RID: 40286 RVA: 0x003B6D80 File Offset: 0x003B4F80
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

	// Token: 0x06009D5F RID: 40287 RVA: 0x003B6E08 File Offset: 0x003B5008
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

	// Token: 0x06009D60 RID: 40288 RVA: 0x003B703C File Offset: 0x003B523C
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

	// Token: 0x06009D61 RID: 40289 RVA: 0x003B7080 File Offset: 0x003B5280
	[PublicizedFrom(EAccessModifier.Private)]
	public void GetMainCamera()
	{
		SkyManager.mainCamera = Camera.main;
	}

	// Token: 0x06009D62 RID: 40290 RVA: 0x003B708C File Offset: 0x003B528C
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

	// Token: 0x06009D63 RID: 40291 RVA: 0x003B70C5 File Offset: 0x003B52C5
	public static float GetDawnTime()
	{
		return (float)SkyManager.dawnTime;
	}

	// Token: 0x06009D64 RID: 40292 RVA: 0x003B70CD File Offset: 0x003B52CD
	public static float GetDawnTimeAsMinutes()
	{
		return (float)SkyManager.dawnTime / 24f * (float)GamePrefs.GetInt(EnumGamePrefs.DayNightLength);
	}

	// Token: 0x06009D65 RID: 40293 RVA: 0x003B70E4 File Offset: 0x003B52E4
	public static float GetDuskTime()
	{
		return (float)SkyManager.duskTime;
	}

	// Token: 0x06009D66 RID: 40294 RVA: 0x003B70EC File Offset: 0x003B52EC
	public static float GetDuskTimeAsMinutes()
	{
		return (float)SkyManager.duskTime / 24f * (float)GamePrefs.GetInt(EnumGamePrefs.DayNightLength);
	}

	// Token: 0x06009D67 RID: 40295 RVA: 0x003B7104 File Offset: 0x003B5304
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

	// Token: 0x06009D68 RID: 40296 RVA: 0x003B71B4 File Offset: 0x003B53B4
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

	// Token: 0x06009D69 RID: 40297 RVA: 0x003B7254 File Offset: 0x003B5454
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

	// Token: 0x06009D6A RID: 40298 RVA: 0x003B771C File Offset: 0x003B591C
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

	// Token: 0x06009D6B RID: 40299 RVA: 0x003B77F3 File Offset: 0x003B59F3
	public static SunShaftsEffect.SunSettings GetSunShaftSettings()
	{
		return SkyManager.sunShaftSettings;
	}

	// Token: 0x06009D6C RID: 40300 RVA: 0x003B77FA File Offset: 0x003B59FA
	public static float GetLuma(Color color)
	{
		return 0.2126f * color.r + 0.7152f * color.g + 0.0722f * color.b;
	}

	// Token: 0x06009D6D RID: 40301 RVA: 0x003B7822 File Offset: 0x003B5A22
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

	// Token: 0x06009D6E RID: 40302 RVA: 0x003B7850 File Offset: 0x003B5A50
	[PublicizedFrom(EAccessModifier.Private)]
	public bool VerifyValidMaterials()
	{
		return !(SkyManager.atmosphereMtrl == null) && !(SkyManager.cloudsSphere == null) && !(SkyManager.atmosphereSphere == null) && !(SkyManager.moonLight == null) && !(SkyManager.sunLight == null) && !(SkyManager.moonSpriteT == null) && !(SkyManager.moonSpriteMat == null);
	}

	// Token: 0x06009D6F RID: 40303 RVA: 0x003B78C7 File Offset: 0x003B5AC7
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

	// Token: 0x06009D70 RID: 40304 RVA: 0x003B78E4 File Offset: 0x003B5AE4
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateFogShader()
	{
		this.fogParams.x = SkyManager.GetFogStart();
		this.fogParams.y = SkyManager.GetFogEnd();
		this.fogParams.z = 1f;
		this.fogParams.w = Mathf.Pow(SkyManager.GetFogDensity(), 2f);
	}

	// Token: 0x06009D71 RID: 40305 RVA: 0x003B793C File Offset: 0x003B5B3C
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

	// Token: 0x06009D72 RID: 40306 RVA: 0x003B79F4 File Offset: 0x003B5BF4
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

	// Token: 0x06009D73 RID: 40307 RVA: 0x003B8095 File Offset: 0x003B6295
	public static float GetSunAngle()
	{
		return SkyManager.sunDirV.y;
	}

	// Token: 0x06009D74 RID: 40308 RVA: 0x003B80A1 File Offset: 0x003B62A1
	public static Vector3 GetSunDirection()
	{
		return SkyManager.sunDirV;
	}

	// Token: 0x06009D75 RID: 40309 RVA: 0x003B80A8 File Offset: 0x003B62A8
	public static float GetSunPercent()
	{
		return -SkyManager.sunDirV.y;
	}

	// Token: 0x06009D76 RID: 40310 RVA: 0x003B80B5 File Offset: 0x003B62B5
	public static Vector3 GetSunLightDirection()
	{
		if (!(SkyManager.sunLight == null))
		{
			return SkyManager.sunLightT.forward;
		}
		return Vector3.down;
	}

	// Token: 0x06009D77 RID: 40311 RVA: 0x003B80D4 File Offset: 0x003B62D4
	public static float GetSunIntensity()
	{
		return SkyManager.sunIntensity;
	}

	// Token: 0x06009D78 RID: 40312 RVA: 0x003B80DB File Offset: 0x003B62DB
	public static Color GetSunLightColor()
	{
		if (SkyManager.sunLight)
		{
			return SkyManager.sunLight.color;
		}
		return Color.black;
	}

	// Token: 0x06009D79 RID: 40313 RVA: 0x003B80F9 File Offset: 0x003B62F9
	public static void SetSunColor(Color color)
	{
		if (SkyManager.sunLight != null)
		{
			SkyManager.sunLight.color = color;
		}
	}

	// Token: 0x06009D7A RID: 40314 RVA: 0x003B8114 File Offset: 0x003B6314
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

	// Token: 0x06009D7B RID: 40315 RVA: 0x003B81A7 File Offset: 0x003B63A7
	public static void SetWeatherLightScale(float _scale)
	{
		SkyManager.weatherLightScale = _scale;
	}

	// Token: 0x06009D7C RID: 40316 RVA: 0x003B81AF File Offset: 0x003B63AF
	public static float GetMoonAmbientScale(float add, float mpy)
	{
		return Utils.FastLerp(add + SkyManager.moonBright * mpy, 1f, SkyManager.dayPercent * 3.030303f);
	}

	// Token: 0x06009D7D RID: 40317 RVA: 0x003B81CF File Offset: 0x003B63CF
	public static float GetMoonBrightness()
	{
		return SkyManager.moonLightColor.grayscale * SkyManager.moonBright;
	}

	// Token: 0x06009D7E RID: 40318 RVA: 0x003B81E1 File Offset: 0x003B63E1
	public static Color GetMoonLightColor()
	{
		return SkyManager.moonLightColor;
	}

	// Token: 0x06009D7F RID: 40319 RVA: 0x003B81E8 File Offset: 0x003B63E8
	public static Vector3 GetMoonDirection()
	{
		return SkyManager.moonLightRot * Vector3.forward;
	}

	// Token: 0x06009D80 RID: 40320 RVA: 0x003B81F9 File Offset: 0x003B63F9
	public static void SetMoonLightColor(Color color)
	{
		SkyManager.moonLightColor = color;
	}

	// Token: 0x06009D81 RID: 40321 RVA: 0x003B8201 File Offset: 0x003B6401
	public static float GetWorldRotation()
	{
		return SkyManager.worldRotation;
	}

	// Token: 0x040076DC RID: 30428
	public static SkyManager skyManager;

	// Token: 0x040076DD RID: 30429
	public static float dayCount;

	// Token: 0x040076DE RID: 30430
	public static bool indoorFogOn = true;

	// Token: 0x040076DF RID: 30431
	public static bool isAllTimeDay = false;

	// Token: 0x040076E0 RID: 30432
	public static bool isAllTimeNight = false;

	// Token: 0x040076E1 RID: 30433
	public static Material atmosphereMtrl;

	// Token: 0x040076E2 RID: 30434
	public static Transform atmosphereSphere;

	// Token: 0x040076E3 RID: 30435
	public static Material cloudsSphereMtrl;

	// Token: 0x040076E4 RID: 30436
	public static Transform cloudsSphere;

	// Token: 0x040076E5 RID: 30437
	public static bool bUpdateSunMoonNow = false;

	// Token: 0x040076E6 RID: 30438
	public static float sSunFadeHeight = 0.1f;

	// Token: 0x040076E7 RID: 30439
	public static float dayPercent;

	// Token: 0x040076E8 RID: 30440
	public static GameRandom random;

	// Token: 0x040076E9 RID: 30441
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float sMaxSunIntensity = 0.7f;

	// Token: 0x040076EA RID: 30442
	public float maxSunIntensity = 0.7f;

	// Token: 0x040076EB RID: 30443
	public float sunHeight = 0.1f;

	// Token: 0x040076EC RID: 30444
	public float moonHeight = 0.095f;

	// Token: 0x040076ED RID: 30445
	public float sunFadeHeight = 0.1f;

	// Token: 0x040076EE RID: 30446
	public float cloudSpeed = 0.05f;

	// Token: 0x040076EF RID: 30447
	public float ShowFogDensity;

	// Token: 0x040076F0 RID: 30448
	public Color ShowSkyColor;

	// Token: 0x040076F1 RID: 30449
	public Color ShowFogColor;

	// Token: 0x040076F2 RID: 30450
	public float maxFarClippingPlane = 1500f;

	// Token: 0x040076F3 RID: 30451
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int dawnTime = 4;

	// Token: 0x040076F4 RID: 30452
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int duskTime = 22;

	// Token: 0x040076F5 RID: 30453
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static ulong timeOfDay;

	// Token: 0x040076F6 RID: 30454
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int bloodmoonDay = 7;

	// Token: 0x040076F7 RID: 30455
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float cloudTransition = 0f;

	// Token: 0x040076F8 RID: 30456
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float fogDensity;

	// Token: 0x040076F9 RID: 30457
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float fogLightScale;

	// Token: 0x040076FA RID: 30458
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float fogStart = 20f;

	// Token: 0x040076FB RID: 30459
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float fogEnd = 80f;

	// Token: 0x040076FC RID: 30460
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float fogDebugDensity = -1f;

	// Token: 0x040076FD RID: 30461
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float fogDebugStart;

	// Token: 0x040076FE RID: 30462
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float fogDebugEnd;

	// Token: 0x040076FF RID: 30463
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Color fogDebugColor;

	// Token: 0x04007700 RID: 30464
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float worldRotationTime = 0f;

	// Token: 0x04007701 RID: 30465
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float worldRotation = 0f;

	// Token: 0x04007702 RID: 30466
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float worldRotationTarget = 0f;

	// Token: 0x04007703 RID: 30467
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float sunIntensity = 1f;

	// Token: 0x04007704 RID: 30468
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float starIntensity = 1f;

	// Token: 0x04007705 RID: 30469
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float weatherLightScale;

	// Token: 0x04007706 RID: 30470
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static bool bNeedsReset;

	// Token: 0x04007707 RID: 30471
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Color SkyColor;

	// Token: 0x04007708 RID: 30472
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Color fogColor;

	// Token: 0x04007709 RID: 30473
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Transform sunLightT;

	// Token: 0x0400770A RID: 30474
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Light sunLight;

	// Token: 0x0400770B RID: 30475
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Vector3 sunDirV;

	// Token: 0x0400770C RID: 30476
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Vector3 sunMoonDirV;

	// Token: 0x0400770D RID: 30477
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly float[] sMoonPhases = new float[] { 0.05f, 0.35f, 0.55f, 0.7f, 1.4f, 1.63f, 1.82f };

	// Token: 0x0400770E RID: 30478
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly float[] sMoonBrights = new float[] { 1f, 0.65f, 0.45f, 0.25f, 0.4f, 0.6f, 0.9f };

	// Token: 0x0400770F RID: 30479
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Transform moonLightT;

	// Token: 0x04007710 RID: 30480
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Light moonLight;

	// Token: 0x04007711 RID: 30481
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Transform moonSpriteT;

	// Token: 0x04007712 RID: 30482
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Material moonSpriteMat;

	// Token: 0x04007713 RID: 30483
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float moonBright;

	// Token: 0x04007714 RID: 30484
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Color moonLightColor;

	// Token: 0x04007715 RID: 30485
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Quaternion moonLightRot;

	// Token: 0x04007716 RID: 30486
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Texture cloudMainTex;

	// Token: 0x04007717 RID: 30487
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Texture cloudMainTexOld;

	// Token: 0x04007718 RID: 30488
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Texture cloudBlendTex;

	// Token: 0x04007719 RID: 30489
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Texture cloudBlendTexOld;

	// Token: 0x0400771A RID: 30490
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static GameObject parent;

	// Token: 0x0400771B RID: 30491
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Camera mainCamera;

	// Token: 0x0400771C RID: 30492
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static NGSS_FrustumShadows_7DTD frustumShadows;

	// Token: 0x0400771D RID: 30493
	public static bool WasBloodMoon = false;

	// Token: 0x0400771E RID: 30494
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cWorldRotationUpdateFreq = 0.2f;

	// Token: 0x0400771F RID: 30495
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cStarRotationSpeed = 0.004f;

	// Token: 0x04007720 RID: 30496
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bUpdateShaders;

	// Token: 0x04007721 RID: 30497
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector4 fogParams;

	// Token: 0x04007722 RID: 30498
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 sunAxis;

	// Token: 0x04007723 RID: 30499
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 sunStartV;

	// Token: 0x04007724 RID: 30500
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 moonStartV;

	// Token: 0x04007725 RID: 30501
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 starAxis;

	// Token: 0x04007726 RID: 30502
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static bool triggerLightning;

	// Token: 0x04007727 RID: 30503
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int lightningFlashes;

	// Token: 0x04007728 RID: 30504
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float[] lightningEndTimes = new float[] { 1f, 2f, 3f, 4f, 5f };

	// Token: 0x04007729 RID: 30505
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int lightningIndex;

	// Token: 0x0400772A RID: 30506
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Vector2 lightningDir;

	// Token: 0x0400772B RID: 30507
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Vector2 lightningDirVel;

	// Token: 0x0400772C RID: 30508
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float lightningIntensity;

	// Token: 0x0400772D RID: 30509
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static SunShaftsEffect.SunSettings sunShaftSettings;

	// Token: 0x0400772E RID: 30510
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 targetSkyPosition;

	// Token: 0x0400772F RID: 30511
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bMovingSky = true;
}

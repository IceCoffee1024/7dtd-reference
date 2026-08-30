using System;
using System.Runtime.CompilerServices;
using PI.NGSS;
using UnityEngine;

// Token: 0x0200136D RID: 4973
public class SkyManager : MonoBehaviour
{
	// Token: 0x17001292 RID: 4754
	// (get) Token: 0x06009C96 RID: 40086 RVA: 0x003B1DDA File Offset: 0x003AFFDA
	public static Transform SunLightT
	{
		get
		{
			return SkyManager.sunLightT;
		}
	}

	// Token: 0x06009C97 RID: 40087 RVA: 0x003B1DE1 File Offset: 0x003AFFE1
	public static bool IsBloodMoonVisible()
	{
		return GameUtils.IsBloodMoonTime(new ValueTuple<int, int>(SkyManager.duskTime - 4, SkyManager.dawnTime + 2), (int)SkyManager.TimeOfDay(), SkyManager.bloodmoonDay, (int)SkyManager.dayCount);
	}

	// Token: 0x06009C98 RID: 40088 RVA: 0x003B1E0C File Offset: 0x003B000C
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

	// Token: 0x06009C99 RID: 40089 RVA: 0x003B1E81 File Offset: 0x003B0081
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		GameOptionsManager.ShadowDistanceChanged += this.OnShadowDistanceChanged;
		GameStats.OnChangedDelegates += this.OnGameStatsChanged;
	}

	// Token: 0x06009C9A RID: 40090 RVA: 0x003B1EA5 File Offset: 0x003B00A5
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDestroy()
	{
		GameOptionsManager.ShadowDistanceChanged -= this.OnShadowDistanceChanged;
		GameStats.OnChangedDelegates -= this.OnGameStatsChanged;
	}

	// Token: 0x06009C9B RID: 40091 RVA: 0x003B1ECC File Offset: 0x003B00CC
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

	// Token: 0x06009C9C RID: 40092 RVA: 0x003B1F25 File Offset: 0x003B0125
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

	// Token: 0x06009C9D RID: 40093 RVA: 0x003B1F5F File Offset: 0x003B015F
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Start()
	{
		SkyManager.Reset();
	}

	// Token: 0x06009C9E RID: 40094 RVA: 0x003B1F66 File Offset: 0x003B0166
	public static void Loaded(GameObject _obj)
	{
		_obj.SetActive(true);
		_obj.transform.parent = GameManager.Instance.transform;
		SkyManager.skyManager = _obj.GetComponent<SkyManager>();
		SkyManager.Reset();
	}

	// Token: 0x06009C9F RID: 40095 RVA: 0x003B1F94 File Offset: 0x003B0194
	public static void Cleanup()
	{
		if (SkyManager.skyManager != null)
		{
			global::UnityEngine.Object.DestroyImmediate(SkyManager.skyManager.gameObject);
		}
	}

	// Token: 0x06009CA0 RID: 40096 RVA: 0x003B1FB4 File Offset: 0x003B01B4
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

	// Token: 0x06009CA1 RID: 40097 RVA: 0x003B203B File Offset: 0x003B023B
	public static Color GetSkyColor()
	{
		return SkyManager.SkyColor;
	}

	// Token: 0x06009CA2 RID: 40098 RVA: 0x003B2042 File Offset: 0x003B0242
	public static void SetSkyColor(Color c)
	{
		SkyManager.SkyColor = c;
	}

	// Token: 0x06009CA3 RID: 40099 RVA: 0x003B204A File Offset: 0x003B024A
	public static void SetGameTime(ulong _time)
	{
		SkyManager.dayCount = _time / 24000f + 1f;
		SkyManager.timeOfDay = _time;
	}

	// Token: 0x06009CA4 RID: 40100 RVA: 0x003B2066 File Offset: 0x003B0266
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float TimeOfDay()
	{
		return SkyManager.timeOfDay % 24000UL / 1000f;
	}

	// Token: 0x06009CA5 RID: 40101 RVA: 0x003B207C File Offset: 0x003B027C
	public static float GetTimeOfDayAsMinutes()
	{
		return SkyManager.TimeOfDay() / 24f * (float)GamePrefs.GetInt(EnumGamePrefs.DayNightLength);
	}

	// Token: 0x06009CA6 RID: 40102 RVA: 0x003B2092 File Offset: 0x003B0292
	public static void SetCloudTextures(Texture _mainTex, Texture _blendTex)
	{
		SkyManager.cloudMainTex = _mainTex;
		SkyManager.cloudBlendTex = _blendTex;
	}

	// Token: 0x06009CA7 RID: 40103 RVA: 0x003B20A0 File Offset: 0x003B02A0
	public static void SetCloudTransition(float t)
	{
		SkyManager.cloudTransition = t;
	}

	// Token: 0x06009CA8 RID: 40104 RVA: 0x003B20A8 File Offset: 0x003B02A8
	public static Color GetFogColor()
	{
		return SkyManager.fogColor;
	}

	// Token: 0x06009CA9 RID: 40105 RVA: 0x003B20AF File Offset: 0x003B02AF
	public static void SetFogColor(Color c)
	{
		if (SkyManager.fogDebugColor.a > 0f)
		{
			c = SkyManager.fogDebugColor;
		}
		SkyManager.fogColor = c;
	}

	// Token: 0x06009CAA RID: 40106 RVA: 0x003B20CF File Offset: 0x003B02CF
	public static float GetFogDensity()
	{
		return SkyManager.fogDensity;
	}

	// Token: 0x06009CAB RID: 40107 RVA: 0x003B20D8 File Offset: 0x003B02D8
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

	// Token: 0x06009CAC RID: 40108 RVA: 0x003B2126 File Offset: 0x003B0326
	public static float GetFogStart()
	{
		return SkyManager.fogStart;
	}

	// Token: 0x06009CAD RID: 40109 RVA: 0x003B212D File Offset: 0x003B032D
	public static float GetFogEnd()
	{
		return SkyManager.fogEnd;
	}

	// Token: 0x06009CAE RID: 40110 RVA: 0x003B2134 File Offset: 0x003B0334
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

	// Token: 0x06009CAF RID: 40111 RVA: 0x003B21C9 File Offset: 0x003B03C9
	public static void SetFogDebug(float density = -1f, float start = -3.4028235E+38f, float end = -3.4028235E+38f)
	{
		SkyManager.fogDebugDensity = density;
		SkyManager.fogDebugStart = start;
		SkyManager.fogDebugEnd = end;
		SkyManager.SetFogDensity(0f);
	}

	// Token: 0x06009CB0 RID: 40112 RVA: 0x003B21E7 File Offset: 0x003B03E7
	public static void SetFogDebugColor(Color color = default(Color))
	{
		SkyManager.fogDebugColor = color;
		SkyManager.SetFogColor(Color.gray);
	}

	// Token: 0x06009CB1 RID: 40113 RVA: 0x003B21F9 File Offset: 0x003B03F9
	public static void Reset()
	{
		SkyManager.bNeedsReset = true;
	}

	// Token: 0x06009CB2 RID: 40114 RVA: 0x003B2204 File Offset: 0x003B0404
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

	// Token: 0x06009CB3 RID: 40115 RVA: 0x003B228C File Offset: 0x003B048C
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

	// Token: 0x06009CB4 RID: 40116 RVA: 0x003B24C0 File Offset: 0x003B06C0
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

	// Token: 0x06009CB5 RID: 40117 RVA: 0x003B2504 File Offset: 0x003B0704
	[PublicizedFrom(EAccessModifier.Private)]
	public void GetMainCamera()
	{
		SkyManager.mainCamera = Camera.main;
	}

	// Token: 0x06009CB6 RID: 40118 RVA: 0x003B2510 File Offset: 0x003B0710
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

	// Token: 0x06009CB7 RID: 40119 RVA: 0x003B2549 File Offset: 0x003B0749
	public static float GetDawnTime()
	{
		return (float)SkyManager.dawnTime;
	}

	// Token: 0x06009CB8 RID: 40120 RVA: 0x003B2551 File Offset: 0x003B0751
	public static float GetDawnTimeAsMinutes()
	{
		return (float)SkyManager.dawnTime / 24f * (float)GamePrefs.GetInt(EnumGamePrefs.DayNightLength);
	}

	// Token: 0x06009CB9 RID: 40121 RVA: 0x003B2568 File Offset: 0x003B0768
	public static float GetDuskTime()
	{
		return (float)SkyManager.duskTime;
	}

	// Token: 0x06009CBA RID: 40122 RVA: 0x003B2570 File Offset: 0x003B0770
	public static float GetDuskTimeAsMinutes()
	{
		return (float)SkyManager.duskTime / 24f * (float)GamePrefs.GetInt(EnumGamePrefs.DayNightLength);
	}

	// Token: 0x06009CBB RID: 40123 RVA: 0x003B2588 File Offset: 0x003B0788
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

	// Token: 0x06009CBC RID: 40124 RVA: 0x003B2638 File Offset: 0x003B0838
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

	// Token: 0x06009CBD RID: 40125 RVA: 0x003B26D8 File Offset: 0x003B08D8
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

	// Token: 0x06009CBE RID: 40126 RVA: 0x003B2BA0 File Offset: 0x003B0DA0
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

	// Token: 0x06009CBF RID: 40127 RVA: 0x003B2C77 File Offset: 0x003B0E77
	public static SunShaftsEffect.SunSettings GetSunShaftSettings()
	{
		return SkyManager.sunShaftSettings;
	}

	// Token: 0x06009CC0 RID: 40128 RVA: 0x003B2C7E File Offset: 0x003B0E7E
	public static float GetLuma(Color color)
	{
		return 0.2126f * color.r + 0.7152f * color.g + 0.0722f * color.b;
	}

	// Token: 0x06009CC1 RID: 40129 RVA: 0x003B2CA6 File Offset: 0x003B0EA6
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

	// Token: 0x06009CC2 RID: 40130 RVA: 0x003B2CD4 File Offset: 0x003B0ED4
	[PublicizedFrom(EAccessModifier.Private)]
	public bool VerifyValidMaterials()
	{
		return !(SkyManager.atmosphereMtrl == null) && !(SkyManager.cloudsSphere == null) && !(SkyManager.atmosphereSphere == null) && !(SkyManager.moonLight == null) && !(SkyManager.sunLight == null) && !(SkyManager.moonSpriteT == null) && !(SkyManager.moonSpriteMat == null);
	}

	// Token: 0x06009CC3 RID: 40131 RVA: 0x003B2D4B File Offset: 0x003B0F4B
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

	// Token: 0x06009CC4 RID: 40132 RVA: 0x003B2D68 File Offset: 0x003B0F68
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateFogShader()
	{
		this.fogParams.x = SkyManager.GetFogStart();
		this.fogParams.y = SkyManager.GetFogEnd();
		this.fogParams.z = 1f;
		this.fogParams.w = Mathf.Pow(SkyManager.GetFogDensity(), 2f);
	}

	// Token: 0x06009CC5 RID: 40133 RVA: 0x003B2DC0 File Offset: 0x003B0FC0
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

	// Token: 0x06009CC6 RID: 40134 RVA: 0x003B2E78 File Offset: 0x003B1078
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

	// Token: 0x06009CC7 RID: 40135 RVA: 0x003B3519 File Offset: 0x003B1719
	public static float GetSunAngle()
	{
		return SkyManager.sunDirV.y;
	}

	// Token: 0x06009CC8 RID: 40136 RVA: 0x003B3525 File Offset: 0x003B1725
	public static Vector3 GetSunDirection()
	{
		return SkyManager.sunDirV;
	}

	// Token: 0x06009CC9 RID: 40137 RVA: 0x003B352C File Offset: 0x003B172C
	public static float GetSunPercent()
	{
		return -SkyManager.sunDirV.y;
	}

	// Token: 0x06009CCA RID: 40138 RVA: 0x003B3539 File Offset: 0x003B1739
	public static Vector3 GetSunLightDirection()
	{
		if (!(SkyManager.sunLight == null))
		{
			return SkyManager.sunLightT.forward;
		}
		return Vector3.down;
	}

	// Token: 0x06009CCB RID: 40139 RVA: 0x003B3558 File Offset: 0x003B1758
	public static float GetSunIntensity()
	{
		return SkyManager.sunIntensity;
	}

	// Token: 0x06009CCC RID: 40140 RVA: 0x003B355F File Offset: 0x003B175F
	public static Color GetSunLightColor()
	{
		if (SkyManager.sunLight)
		{
			return SkyManager.sunLight.color;
		}
		return Color.black;
	}

	// Token: 0x06009CCD RID: 40141 RVA: 0x003B357D File Offset: 0x003B177D
	public static void SetSunColor(Color color)
	{
		if (SkyManager.sunLight != null)
		{
			SkyManager.sunLight.color = color;
		}
	}

	// Token: 0x06009CCE RID: 40142 RVA: 0x003B3598 File Offset: 0x003B1798
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

	// Token: 0x06009CCF RID: 40143 RVA: 0x003B362B File Offset: 0x003B182B
	public static void SetWeatherLightScale(float _scale)
	{
		SkyManager.weatherLightScale = _scale;
	}

	// Token: 0x06009CD0 RID: 40144 RVA: 0x003B3633 File Offset: 0x003B1833
	public static float GetMoonAmbientScale(float add, float mpy)
	{
		return Utils.FastLerp(add + SkyManager.moonBright * mpy, 1f, SkyManager.dayPercent * 3.030303f);
	}

	// Token: 0x06009CD1 RID: 40145 RVA: 0x003B3653 File Offset: 0x003B1853
	public static float GetMoonBrightness()
	{
		return SkyManager.moonLightColor.grayscale * SkyManager.moonBright;
	}

	// Token: 0x06009CD2 RID: 40146 RVA: 0x003B3665 File Offset: 0x003B1865
	public static Color GetMoonLightColor()
	{
		return SkyManager.moonLightColor;
	}

	// Token: 0x06009CD3 RID: 40147 RVA: 0x003B366C File Offset: 0x003B186C
	public static Vector3 GetMoonDirection()
	{
		return SkyManager.moonLightRot * Vector3.forward;
	}

	// Token: 0x06009CD4 RID: 40148 RVA: 0x003B367D File Offset: 0x003B187D
	public static void SetMoonLightColor(Color color)
	{
		SkyManager.moonLightColor = color;
	}

	// Token: 0x06009CD5 RID: 40149 RVA: 0x003B3685 File Offset: 0x003B1885
	public static float GetWorldRotation()
	{
		return SkyManager.worldRotation;
	}

	// Token: 0x04007661 RID: 30305
	public static SkyManager skyManager;

	// Token: 0x04007662 RID: 30306
	public static float dayCount;

	// Token: 0x04007663 RID: 30307
	public static bool indoorFogOn = true;

	// Token: 0x04007664 RID: 30308
	public static bool isAllTimeDay = false;

	// Token: 0x04007665 RID: 30309
	public static bool isAllTimeNight = false;

	// Token: 0x04007666 RID: 30310
	public static Material atmosphereMtrl;

	// Token: 0x04007667 RID: 30311
	public static Transform atmosphereSphere;

	// Token: 0x04007668 RID: 30312
	public static Material cloudsSphereMtrl;

	// Token: 0x04007669 RID: 30313
	public static Transform cloudsSphere;

	// Token: 0x0400766A RID: 30314
	public static bool bUpdateSunMoonNow = false;

	// Token: 0x0400766B RID: 30315
	public static float sSunFadeHeight = 0.1f;

	// Token: 0x0400766C RID: 30316
	public static float dayPercent;

	// Token: 0x0400766D RID: 30317
	public static GameRandom random;

	// Token: 0x0400766E RID: 30318
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float sMaxSunIntensity = 0.7f;

	// Token: 0x0400766F RID: 30319
	public float maxSunIntensity = 0.7f;

	// Token: 0x04007670 RID: 30320
	public float sunHeight = 0.1f;

	// Token: 0x04007671 RID: 30321
	public float moonHeight = 0.095f;

	// Token: 0x04007672 RID: 30322
	public float sunFadeHeight = 0.1f;

	// Token: 0x04007673 RID: 30323
	public float cloudSpeed = 0.05f;

	// Token: 0x04007674 RID: 30324
	public float ShowFogDensity;

	// Token: 0x04007675 RID: 30325
	public Color ShowSkyColor;

	// Token: 0x04007676 RID: 30326
	public Color ShowFogColor;

	// Token: 0x04007677 RID: 30327
	public float maxFarClippingPlane = 1500f;

	// Token: 0x04007678 RID: 30328
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int dawnTime = 4;

	// Token: 0x04007679 RID: 30329
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int duskTime = 22;

	// Token: 0x0400767A RID: 30330
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static ulong timeOfDay;

	// Token: 0x0400767B RID: 30331
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int bloodmoonDay = 7;

	// Token: 0x0400767C RID: 30332
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float cloudTransition = 0f;

	// Token: 0x0400767D RID: 30333
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float fogDensity;

	// Token: 0x0400767E RID: 30334
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float fogLightScale;

	// Token: 0x0400767F RID: 30335
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float fogStart = 20f;

	// Token: 0x04007680 RID: 30336
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float fogEnd = 80f;

	// Token: 0x04007681 RID: 30337
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float fogDebugDensity = -1f;

	// Token: 0x04007682 RID: 30338
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float fogDebugStart;

	// Token: 0x04007683 RID: 30339
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float fogDebugEnd;

	// Token: 0x04007684 RID: 30340
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Color fogDebugColor;

	// Token: 0x04007685 RID: 30341
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float worldRotationTime = 0f;

	// Token: 0x04007686 RID: 30342
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float worldRotation = 0f;

	// Token: 0x04007687 RID: 30343
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float worldRotationTarget = 0f;

	// Token: 0x04007688 RID: 30344
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float sunIntensity = 1f;

	// Token: 0x04007689 RID: 30345
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float starIntensity = 1f;

	// Token: 0x0400768A RID: 30346
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float weatherLightScale;

	// Token: 0x0400768B RID: 30347
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static bool bNeedsReset;

	// Token: 0x0400768C RID: 30348
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Color SkyColor;

	// Token: 0x0400768D RID: 30349
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Color fogColor;

	// Token: 0x0400768E RID: 30350
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Transform sunLightT;

	// Token: 0x0400768F RID: 30351
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Light sunLight;

	// Token: 0x04007690 RID: 30352
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Vector3 sunDirV;

	// Token: 0x04007691 RID: 30353
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Vector3 sunMoonDirV;

	// Token: 0x04007692 RID: 30354
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly float[] sMoonPhases = new float[] { 0.05f, 0.35f, 0.55f, 0.7f, 1.4f, 1.63f, 1.82f };

	// Token: 0x04007693 RID: 30355
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly float[] sMoonBrights = new float[] { 1f, 0.65f, 0.45f, 0.25f, 0.4f, 0.6f, 0.9f };

	// Token: 0x04007694 RID: 30356
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Transform moonLightT;

	// Token: 0x04007695 RID: 30357
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Light moonLight;

	// Token: 0x04007696 RID: 30358
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Transform moonSpriteT;

	// Token: 0x04007697 RID: 30359
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Material moonSpriteMat;

	// Token: 0x04007698 RID: 30360
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float moonBright;

	// Token: 0x04007699 RID: 30361
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Color moonLightColor;

	// Token: 0x0400769A RID: 30362
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Quaternion moonLightRot;

	// Token: 0x0400769B RID: 30363
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Texture cloudMainTex;

	// Token: 0x0400769C RID: 30364
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Texture cloudMainTexOld;

	// Token: 0x0400769D RID: 30365
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Texture cloudBlendTex;

	// Token: 0x0400769E RID: 30366
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Texture cloudBlendTexOld;

	// Token: 0x0400769F RID: 30367
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static GameObject parent;

	// Token: 0x040076A0 RID: 30368
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Camera mainCamera;

	// Token: 0x040076A1 RID: 30369
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static NGSS_FrustumShadows_7DTD frustumShadows;

	// Token: 0x040076A2 RID: 30370
	public static bool WasBloodMoon = false;

	// Token: 0x040076A3 RID: 30371
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cWorldRotationUpdateFreq = 0.2f;

	// Token: 0x040076A4 RID: 30372
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cStarRotationSpeed = 0.004f;

	// Token: 0x040076A5 RID: 30373
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bUpdateShaders;

	// Token: 0x040076A6 RID: 30374
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector4 fogParams;

	// Token: 0x040076A7 RID: 30375
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 sunAxis;

	// Token: 0x040076A8 RID: 30376
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 sunStartV;

	// Token: 0x040076A9 RID: 30377
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 moonStartV;

	// Token: 0x040076AA RID: 30378
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 starAxis;

	// Token: 0x040076AB RID: 30379
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static bool triggerLightning;

	// Token: 0x040076AC RID: 30380
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int lightningFlashes;

	// Token: 0x040076AD RID: 30381
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float[] lightningEndTimes = new float[] { 1f, 2f, 3f, 4f, 5f };

	// Token: 0x040076AE RID: 30382
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int lightningIndex;

	// Token: 0x040076AF RID: 30383
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Vector2 lightningDir;

	// Token: 0x040076B0 RID: 30384
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Vector2 lightningDirVel;

	// Token: 0x040076B1 RID: 30385
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float lightningIntensity;

	// Token: 0x040076B2 RID: 30386
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static SunShaftsEffect.SunSettings sunShaftSettings;

	// Token: 0x040076B3 RID: 30387
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 targetSkyPosition;

	// Token: 0x040076B4 RID: 30388
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bMovingSky = true;
}

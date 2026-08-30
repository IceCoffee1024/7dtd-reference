using System;
using System.Runtime.CompilerServices;
using PI.NGSS;
using UnityEngine;

// Token: 0x02001353 RID: 4947
public class SkyManager : MonoBehaviour
{
	// Token: 0x17001259 RID: 4697
	// (get) Token: 0x06009B9B RID: 39835 RVA: 0x003ACB32 File Offset: 0x003AAD32
	public static Transform SunLightT
	{
		get
		{
			return SkyManager.sunLightT;
		}
	}

	// Token: 0x06009B9C RID: 39836 RVA: 0x003ACB39 File Offset: 0x003AAD39
	public static bool IsBloodMoonVisible()
	{
		return GameUtils.IsBloodMoonTime(new ValueTuple<int, int>(SkyManager.duskTime - 4, SkyManager.dawnTime + 2), (int)SkyManager.TimeOfDay(), SkyManager.bloodmoonDay, (int)SkyManager.dayCount);
	}

	// Token: 0x06009B9D RID: 39837 RVA: 0x003ACB64 File Offset: 0x003AAD64
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

	// Token: 0x06009B9E RID: 39838 RVA: 0x003ACBD9 File Offset: 0x003AADD9
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		GameOptionsManager.ShadowDistanceChanged += this.OnShadowDistanceChanged;
		GameStats.OnChangedDelegates += this.OnGameStatsChanged;
	}

	// Token: 0x06009B9F RID: 39839 RVA: 0x003ACBFD File Offset: 0x003AADFD
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDestroy()
	{
		GameOptionsManager.ShadowDistanceChanged -= this.OnShadowDistanceChanged;
		GameStats.OnChangedDelegates -= this.OnGameStatsChanged;
	}

	// Token: 0x06009BA0 RID: 39840 RVA: 0x003ACC24 File Offset: 0x003AAE24
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

	// Token: 0x06009BA1 RID: 39841 RVA: 0x003ACC7D File Offset: 0x003AAE7D
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

	// Token: 0x06009BA2 RID: 39842 RVA: 0x003ACCB7 File Offset: 0x003AAEB7
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Start()
	{
		SkyManager.Reset();
	}

	// Token: 0x06009BA3 RID: 39843 RVA: 0x003ACCBE File Offset: 0x003AAEBE
	public static void Loaded(GameObject _obj)
	{
		_obj.SetActive(true);
		_obj.transform.parent = GameManager.Instance.transform;
		SkyManager.skyManager = _obj.GetComponent<SkyManager>();
		SkyManager.Reset();
	}

	// Token: 0x06009BA4 RID: 39844 RVA: 0x003ACCEC File Offset: 0x003AAEEC
	public static void Cleanup()
	{
		if (SkyManager.skyManager != null)
		{
			global::UnityEngine.Object.DestroyImmediate(SkyManager.skyManager.gameObject);
		}
	}

	// Token: 0x06009BA5 RID: 39845 RVA: 0x003ACD0C File Offset: 0x003AAF0C
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

	// Token: 0x06009BA6 RID: 39846 RVA: 0x003ACD93 File Offset: 0x003AAF93
	public static Color GetSkyColor()
	{
		return SkyManager.SkyColor;
	}

	// Token: 0x06009BA7 RID: 39847 RVA: 0x003ACD9A File Offset: 0x003AAF9A
	public static void SetSkyColor(Color c)
	{
		SkyManager.SkyColor = c;
	}

	// Token: 0x06009BA8 RID: 39848 RVA: 0x003ACDA2 File Offset: 0x003AAFA2
	public static void SetGameTime(ulong _time)
	{
		SkyManager.dayCount = _time / 24000f + 1f;
		SkyManager.timeOfDay = _time;
	}

	// Token: 0x06009BA9 RID: 39849 RVA: 0x003ACDBE File Offset: 0x003AAFBE
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float TimeOfDay()
	{
		return SkyManager.timeOfDay % 24000UL / 1000f;
	}

	// Token: 0x06009BAA RID: 39850 RVA: 0x003ACDD4 File Offset: 0x003AAFD4
	public static float GetTimeOfDayAsMinutes()
	{
		return SkyManager.TimeOfDay() / 24f * (float)GamePrefs.GetInt(EnumGamePrefs.DayNightLength);
	}

	// Token: 0x06009BAB RID: 39851 RVA: 0x003ACDEA File Offset: 0x003AAFEA
	public static void SetCloudTextures(Texture _mainTex, Texture _blendTex)
	{
		SkyManager.cloudMainTex = _mainTex;
		SkyManager.cloudBlendTex = _blendTex;
	}

	// Token: 0x06009BAC RID: 39852 RVA: 0x003ACDF8 File Offset: 0x003AAFF8
	public static void SetCloudTransition(float t)
	{
		SkyManager.cloudTransition = t;
	}

	// Token: 0x06009BAD RID: 39853 RVA: 0x003ACE00 File Offset: 0x003AB000
	public static Color GetFogColor()
	{
		return SkyManager.fogColor;
	}

	// Token: 0x06009BAE RID: 39854 RVA: 0x003ACE07 File Offset: 0x003AB007
	public static void SetFogColor(Color c)
	{
		if (SkyManager.fogDebugColor.a > 0f)
		{
			c = SkyManager.fogDebugColor;
		}
		SkyManager.fogColor = c;
	}

	// Token: 0x06009BAF RID: 39855 RVA: 0x003ACE27 File Offset: 0x003AB027
	public static float GetFogDensity()
	{
		return SkyManager.fogDensity;
	}

	// Token: 0x06009BB0 RID: 39856 RVA: 0x003ACE30 File Offset: 0x003AB030
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

	// Token: 0x06009BB1 RID: 39857 RVA: 0x003ACE7E File Offset: 0x003AB07E
	public static float GetFogStart()
	{
		return SkyManager.fogStart;
	}

	// Token: 0x06009BB2 RID: 39858 RVA: 0x003ACE85 File Offset: 0x003AB085
	public static float GetFogEnd()
	{
		return SkyManager.fogEnd;
	}

	// Token: 0x06009BB3 RID: 39859 RVA: 0x003ACE8C File Offset: 0x003AB08C
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

	// Token: 0x06009BB4 RID: 39860 RVA: 0x003ACF21 File Offset: 0x003AB121
	public static void SetFogDebug(float density = -1f, float start = -3.4028235E+38f, float end = -3.4028235E+38f)
	{
		SkyManager.fogDebugDensity = density;
		SkyManager.fogDebugStart = start;
		SkyManager.fogDebugEnd = end;
		SkyManager.SetFogDensity(0f);
	}

	// Token: 0x06009BB5 RID: 39861 RVA: 0x003ACF3F File Offset: 0x003AB13F
	public static void SetFogDebugColor(Color color = default(Color))
	{
		SkyManager.fogDebugColor = color;
		SkyManager.SetFogColor(Color.gray);
	}

	// Token: 0x06009BB6 RID: 39862 RVA: 0x003ACF51 File Offset: 0x003AB151
	public static void Reset()
	{
		SkyManager.bNeedsReset = true;
	}

	// Token: 0x06009BB7 RID: 39863 RVA: 0x003ACF5C File Offset: 0x003AB15C
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

	// Token: 0x06009BB8 RID: 39864 RVA: 0x003ACFE4 File Offset: 0x003AB1E4
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

	// Token: 0x06009BB9 RID: 39865 RVA: 0x003AD218 File Offset: 0x003AB418
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

	// Token: 0x06009BBA RID: 39866 RVA: 0x003AD25C File Offset: 0x003AB45C
	[PublicizedFrom(EAccessModifier.Private)]
	public void GetMainCamera()
	{
		SkyManager.mainCamera = Camera.main;
	}

	// Token: 0x06009BBB RID: 39867 RVA: 0x003AD268 File Offset: 0x003AB468
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

	// Token: 0x06009BBC RID: 39868 RVA: 0x003AD2A1 File Offset: 0x003AB4A1
	public static float GetDawnTime()
	{
		return (float)SkyManager.dawnTime;
	}

	// Token: 0x06009BBD RID: 39869 RVA: 0x003AD2A9 File Offset: 0x003AB4A9
	public static float GetDawnTimeAsMinutes()
	{
		return (float)SkyManager.dawnTime / 24f * (float)GamePrefs.GetInt(EnumGamePrefs.DayNightLength);
	}

	// Token: 0x06009BBE RID: 39870 RVA: 0x003AD2C0 File Offset: 0x003AB4C0
	public static float GetDuskTime()
	{
		return (float)SkyManager.duskTime;
	}

	// Token: 0x06009BBF RID: 39871 RVA: 0x003AD2C8 File Offset: 0x003AB4C8
	public static float GetDuskTimeAsMinutes()
	{
		return (float)SkyManager.duskTime / 24f * (float)GamePrefs.GetInt(EnumGamePrefs.DayNightLength);
	}

	// Token: 0x06009BC0 RID: 39872 RVA: 0x003AD2E0 File Offset: 0x003AB4E0
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

	// Token: 0x06009BC1 RID: 39873 RVA: 0x003AD390 File Offset: 0x003AB590
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

	// Token: 0x06009BC2 RID: 39874 RVA: 0x003AD430 File Offset: 0x003AB630
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

	// Token: 0x06009BC3 RID: 39875 RVA: 0x003AD8F8 File Offset: 0x003ABAF8
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

	// Token: 0x06009BC4 RID: 39876 RVA: 0x003AD9CF File Offset: 0x003ABBCF
	public static SunShaftsEffect.SunSettings GetSunShaftSettings()
	{
		return SkyManager.sunShaftSettings;
	}

	// Token: 0x06009BC5 RID: 39877 RVA: 0x003AD9D6 File Offset: 0x003ABBD6
	public static float GetLuma(Color color)
	{
		return 0.2126f * color.r + 0.7152f * color.g + 0.0722f * color.b;
	}

	// Token: 0x06009BC6 RID: 39878 RVA: 0x003AD9FE File Offset: 0x003ABBFE
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

	// Token: 0x06009BC7 RID: 39879 RVA: 0x003ADA2C File Offset: 0x003ABC2C
	[PublicizedFrom(EAccessModifier.Private)]
	public bool VerifyValidMaterials()
	{
		return !(SkyManager.atmosphereMtrl == null) && !(SkyManager.cloudsSphere == null) && !(SkyManager.atmosphereSphere == null) && !(SkyManager.moonLight == null) && !(SkyManager.sunLight == null) && !(SkyManager.moonSpriteT == null) && !(SkyManager.moonSpriteMat == null);
	}

	// Token: 0x06009BC8 RID: 39880 RVA: 0x003ADAA3 File Offset: 0x003ABCA3
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

	// Token: 0x06009BC9 RID: 39881 RVA: 0x003ADAC0 File Offset: 0x003ABCC0
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateFogShader()
	{
		this.fogParams.x = SkyManager.GetFogStart();
		this.fogParams.y = SkyManager.GetFogEnd();
		this.fogParams.z = 1f;
		this.fogParams.w = Mathf.Pow(SkyManager.GetFogDensity(), 2f);
	}

	// Token: 0x06009BCA RID: 39882 RVA: 0x003ADB18 File Offset: 0x003ABD18
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

	// Token: 0x06009BCB RID: 39883 RVA: 0x003ADBD0 File Offset: 0x003ABDD0
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

	// Token: 0x06009BCC RID: 39884 RVA: 0x003AE271 File Offset: 0x003AC471
	public static float GetSunAngle()
	{
		return SkyManager.sunDirV.y;
	}

	// Token: 0x06009BCD RID: 39885 RVA: 0x003AE27D File Offset: 0x003AC47D
	public static Vector3 GetSunDirection()
	{
		return SkyManager.sunDirV;
	}

	// Token: 0x06009BCE RID: 39886 RVA: 0x003AE284 File Offset: 0x003AC484
	public static float GetSunPercent()
	{
		return -SkyManager.sunDirV.y;
	}

	// Token: 0x06009BCF RID: 39887 RVA: 0x003AE291 File Offset: 0x003AC491
	public static Vector3 GetSunLightDirection()
	{
		if (!(SkyManager.sunLight == null))
		{
			return SkyManager.sunLightT.forward;
		}
		return Vector3.down;
	}

	// Token: 0x06009BD0 RID: 39888 RVA: 0x003AE2B0 File Offset: 0x003AC4B0
	public static float GetSunIntensity()
	{
		return SkyManager.sunIntensity;
	}

	// Token: 0x06009BD1 RID: 39889 RVA: 0x003AE2B7 File Offset: 0x003AC4B7
	public static Color GetSunLightColor()
	{
		if (SkyManager.sunLight)
		{
			return SkyManager.sunLight.color;
		}
		return Color.black;
	}

	// Token: 0x06009BD2 RID: 39890 RVA: 0x003AE2D5 File Offset: 0x003AC4D5
	public static void SetSunColor(Color color)
	{
		if (SkyManager.sunLight != null)
		{
			SkyManager.sunLight.color = color;
		}
	}

	// Token: 0x06009BD3 RID: 39891 RVA: 0x003AE2F0 File Offset: 0x003AC4F0
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

	// Token: 0x06009BD4 RID: 39892 RVA: 0x003AE383 File Offset: 0x003AC583
	public static void SetWeatherLightScale(float _scale)
	{
		SkyManager.weatherLightScale = _scale;
	}

	// Token: 0x06009BD5 RID: 39893 RVA: 0x003AE38B File Offset: 0x003AC58B
	public static float GetMoonAmbientScale(float add, float mpy)
	{
		return Utils.FastLerp(add + SkyManager.moonBright * mpy, 1f, SkyManager.dayPercent * 3.030303f);
	}

	// Token: 0x06009BD6 RID: 39894 RVA: 0x003AE3AB File Offset: 0x003AC5AB
	public static float GetMoonBrightness()
	{
		return SkyManager.moonLightColor.grayscale * SkyManager.moonBright;
	}

	// Token: 0x06009BD7 RID: 39895 RVA: 0x003AE3BD File Offset: 0x003AC5BD
	public static Color GetMoonLightColor()
	{
		return SkyManager.moonLightColor;
	}

	// Token: 0x06009BD8 RID: 39896 RVA: 0x003AE3C4 File Offset: 0x003AC5C4
	public static Vector3 GetMoonDirection()
	{
		return SkyManager.moonLightRot * Vector3.forward;
	}

	// Token: 0x06009BD9 RID: 39897 RVA: 0x003AE3D5 File Offset: 0x003AC5D5
	public static void SetMoonLightColor(Color color)
	{
		SkyManager.moonLightColor = color;
	}

	// Token: 0x06009BDA RID: 39898 RVA: 0x003AE3DD File Offset: 0x003AC5DD
	public static float GetWorldRotation()
	{
		return SkyManager.worldRotation;
	}

	// Token: 0x04007589 RID: 30089
	public static SkyManager skyManager;

	// Token: 0x0400758A RID: 30090
	public static float dayCount;

	// Token: 0x0400758B RID: 30091
	public static bool indoorFogOn = true;

	// Token: 0x0400758C RID: 30092
	public static bool isAllTimeDay = false;

	// Token: 0x0400758D RID: 30093
	public static bool isAllTimeNight = false;

	// Token: 0x0400758E RID: 30094
	public static Material atmosphereMtrl;

	// Token: 0x0400758F RID: 30095
	public static Transform atmosphereSphere;

	// Token: 0x04007590 RID: 30096
	public static Material cloudsSphereMtrl;

	// Token: 0x04007591 RID: 30097
	public static Transform cloudsSphere;

	// Token: 0x04007592 RID: 30098
	public static bool bUpdateSunMoonNow = false;

	// Token: 0x04007593 RID: 30099
	public static float sSunFadeHeight = 0.1f;

	// Token: 0x04007594 RID: 30100
	public static float dayPercent;

	// Token: 0x04007595 RID: 30101
	public static GameRandom random;

	// Token: 0x04007596 RID: 30102
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float sMaxSunIntensity = 0.7f;

	// Token: 0x04007597 RID: 30103
	public float maxSunIntensity = 0.7f;

	// Token: 0x04007598 RID: 30104
	public float sunHeight = 0.1f;

	// Token: 0x04007599 RID: 30105
	public float moonHeight = 0.095f;

	// Token: 0x0400759A RID: 30106
	public float sunFadeHeight = 0.1f;

	// Token: 0x0400759B RID: 30107
	public float cloudSpeed = 0.05f;

	// Token: 0x0400759C RID: 30108
	public float ShowFogDensity;

	// Token: 0x0400759D RID: 30109
	public Color ShowSkyColor;

	// Token: 0x0400759E RID: 30110
	public Color ShowFogColor;

	// Token: 0x0400759F RID: 30111
	public float maxFarClippingPlane = 1500f;

	// Token: 0x040075A0 RID: 30112
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int dawnTime = 4;

	// Token: 0x040075A1 RID: 30113
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int duskTime = 22;

	// Token: 0x040075A2 RID: 30114
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static ulong timeOfDay;

	// Token: 0x040075A3 RID: 30115
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int bloodmoonDay = 7;

	// Token: 0x040075A4 RID: 30116
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float cloudTransition = 0f;

	// Token: 0x040075A5 RID: 30117
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float fogDensity;

	// Token: 0x040075A6 RID: 30118
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float fogLightScale;

	// Token: 0x040075A7 RID: 30119
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float fogStart = 20f;

	// Token: 0x040075A8 RID: 30120
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float fogEnd = 80f;

	// Token: 0x040075A9 RID: 30121
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float fogDebugDensity = -1f;

	// Token: 0x040075AA RID: 30122
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float fogDebugStart;

	// Token: 0x040075AB RID: 30123
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float fogDebugEnd;

	// Token: 0x040075AC RID: 30124
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Color fogDebugColor;

	// Token: 0x040075AD RID: 30125
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float worldRotationTime = 0f;

	// Token: 0x040075AE RID: 30126
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float worldRotation = 0f;

	// Token: 0x040075AF RID: 30127
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float worldRotationTarget = 0f;

	// Token: 0x040075B0 RID: 30128
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float sunIntensity = 1f;

	// Token: 0x040075B1 RID: 30129
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float starIntensity = 1f;

	// Token: 0x040075B2 RID: 30130
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float weatherLightScale;

	// Token: 0x040075B3 RID: 30131
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static bool bNeedsReset;

	// Token: 0x040075B4 RID: 30132
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Color SkyColor;

	// Token: 0x040075B5 RID: 30133
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Color fogColor;

	// Token: 0x040075B6 RID: 30134
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Transform sunLightT;

	// Token: 0x040075B7 RID: 30135
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Light sunLight;

	// Token: 0x040075B8 RID: 30136
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Vector3 sunDirV;

	// Token: 0x040075B9 RID: 30137
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Vector3 sunMoonDirV;

	// Token: 0x040075BA RID: 30138
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly float[] sMoonPhases = new float[] { 0.05f, 0.35f, 0.55f, 0.7f, 1.4f, 1.63f, 1.82f };

	// Token: 0x040075BB RID: 30139
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly float[] sMoonBrights = new float[] { 1f, 0.65f, 0.45f, 0.25f, 0.4f, 0.6f, 0.9f };

	// Token: 0x040075BC RID: 30140
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Transform moonLightT;

	// Token: 0x040075BD RID: 30141
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Light moonLight;

	// Token: 0x040075BE RID: 30142
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Transform moonSpriteT;

	// Token: 0x040075BF RID: 30143
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Material moonSpriteMat;

	// Token: 0x040075C0 RID: 30144
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float moonBright;

	// Token: 0x040075C1 RID: 30145
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Color moonLightColor;

	// Token: 0x040075C2 RID: 30146
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Quaternion moonLightRot;

	// Token: 0x040075C3 RID: 30147
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Texture cloudMainTex;

	// Token: 0x040075C4 RID: 30148
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Texture cloudMainTexOld;

	// Token: 0x040075C5 RID: 30149
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Texture cloudBlendTex;

	// Token: 0x040075C6 RID: 30150
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Texture cloudBlendTexOld;

	// Token: 0x040075C7 RID: 30151
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static GameObject parent;

	// Token: 0x040075C8 RID: 30152
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Camera mainCamera;

	// Token: 0x040075C9 RID: 30153
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static NGSS_FrustumShadows_7DTD frustumShadows;

	// Token: 0x040075CA RID: 30154
	public static bool WasBloodMoon = false;

	// Token: 0x040075CB RID: 30155
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cWorldRotationUpdateFreq = 0.2f;

	// Token: 0x040075CC RID: 30156
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cStarRotationSpeed = 0.004f;

	// Token: 0x040075CD RID: 30157
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bUpdateShaders;

	// Token: 0x040075CE RID: 30158
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector4 fogParams;

	// Token: 0x040075CF RID: 30159
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 sunAxis;

	// Token: 0x040075D0 RID: 30160
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 sunStartV;

	// Token: 0x040075D1 RID: 30161
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 moonStartV;

	// Token: 0x040075D2 RID: 30162
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 starAxis;

	// Token: 0x040075D3 RID: 30163
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static bool triggerLightning;

	// Token: 0x040075D4 RID: 30164
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int lightningFlashes;

	// Token: 0x040075D5 RID: 30165
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float[] lightningEndTimes = new float[] { 1f, 2f, 3f, 4f, 5f };

	// Token: 0x040075D6 RID: 30166
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int lightningIndex;

	// Token: 0x040075D7 RID: 30167
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Vector2 lightningDir;

	// Token: 0x040075D8 RID: 30168
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Vector2 lightningDirVel;

	// Token: 0x040075D9 RID: 30169
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float lightningIntensity;

	// Token: 0x040075DA RID: 30170
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static SunShaftsEffect.SunSettings sunShaftSettings;

	// Token: 0x040075DB RID: 30171
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 targetSkyPosition;

	// Token: 0x040075DC RID: 30172
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bMovingSky = true;
}

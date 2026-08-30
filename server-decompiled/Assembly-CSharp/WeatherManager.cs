using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

// Token: 0x02001377 RID: 4983
public class WeatherManager : MonoBehaviour
{
	// Token: 0x06009D0E RID: 40206 RVA: 0x003B56E3 File Offset: 0x003B38E3
	public static void Init(World _world, GameObject _obj)
	{
		WeatherManager.Cleanup();
		WeatherManager.Instance = _obj.GetComponent<WeatherManager>();
		WeatherManager.Instance.Init(_world);
	}

	// Token: 0x06009D0F RID: 40207 RVA: 0x003B5700 File Offset: 0x003B3900
	[PublicizedFrom(EAccessModifier.Private)]
	public void Init(World _world)
	{
		this.world = _world;
		WeatherManager.gameRand = _world.GetGameRandom();
		string @string = GamePrefs.GetString(EnumGamePrefs.GameMode);
		this.isGameModeNormal = !GameModeEditWorld.TypeName.Equals(@string) && !GameModeCreative.TypeName.Equals(@string);
		this.InitBiomeWeather();
		this.ApplyLoad();
	}

	// Token: 0x06009D10 RID: 40208 RVA: 0x003B5758 File Offset: 0x003B3958
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitBiomeWeather()
	{
		this.biomeWeather = new List<WeatherManager.BiomeWeather>();
		foreach (KeyValuePair<uint, BiomeDefinition> keyValuePair in this.world.Biomes.GetBiomeMap())
		{
			BiomeDefinition value = keyValuePair.Value;
			if (value.weatherGroups.Count > 0)
			{
				WeatherManager.BiomeWeather biomeWeather = new WeatherManager.BiomeWeather(value);
				this.biomeWeather.Add(biomeWeather);
				biomeWeather.Reset();
			}
		}
		int count = this.biomeWeather.Count;
		this.weatherPackages = new WeatherPackage[count];
		for (int i = 0; i < count; i++)
		{
			WeatherManager.BiomeWeather biomeWeather2 = this.biomeWeather[i];
			WeatherPackage weatherPackage = new WeatherPackage();
			weatherPackage.biomeId = biomeWeather2.biomeDefinition.m_Id;
			this.weatherPackages[i] = weatherPackage;
		}
	}

	// Token: 0x06009D11 RID: 40209 RVA: 0x003B5848 File Offset: 0x003B3A48
	public static void Cleanup()
	{
		if (WeatherManager.Instance)
		{
			global::UnityEngine.Object.DestroyImmediate(WeatherManager.Instance.gameObject);
			WeatherManager.forceClouds = -1f;
			WeatherManager.forceRain = -1f;
			WeatherManager.forceSnowfall = -1f;
			WeatherManager.forceTemperature = -100f;
			WeatherManager.forceWind = -1f;
		}
	}

	// Token: 0x06009D12 RID: 40210 RVA: 0x003B58A2 File Offset: 0x003B3AA2
	public static void SetWorldTime(ulong _time)
	{
		if (WeatherManager.Instance)
		{
			int num = (int)_time;
			if (num < WeatherManager.worldTime)
			{
				WeatherManager.Instance.AdjustTimeRewind();
			}
			WeatherManager.worldTime = num;
		}
	}

	// Token: 0x06009D13 RID: 40211 RVA: 0x003B58CC File Offset: 0x003B3ACC
	[PublicizedFrom(EAccessModifier.Private)]
	public void AdjustTimeRewind()
	{
		this.weatherLastUpdateWorldTime = 0f;
		for (int i = 0; i < this.biomeWeather.Count; i++)
		{
			WeatherManager.BiomeWeather biomeWeather = this.biomeWeather[i];
			biomeWeather.stormWorldTime = 0;
			biomeWeather.stormDuration = 0;
			biomeWeather.nextRandWorldTime = 0;
		}
	}

	// Token: 0x06009D14 RID: 40212 RVA: 0x003B591A File Offset: 0x003B3B1A
	public static void Load(IBinaryReaderOrWriter _RW, int _loadSize)
	{
		if (WeatherManager.loadData == null)
		{
			WeatherManager.loadData = new MemoryStream();
		}
		WeatherManager.loadData.SetLength((long)_loadSize);
		_RW.ReadWrite(WeatherManager.loadData.GetBuffer(), 0, _loadSize);
		WeatherManager.loadData.Position = 0L;
	}

	// Token: 0x06009D15 RID: 40213 RVA: 0x003B5958 File Offset: 0x003B3B58
	public void ApplyLoad()
	{
		if (WeatherManager.loadData != null)
		{
			if (WeatherManager.loadData.Length > 0L)
			{
				using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
				{
					pooledBinaryReader.SetBaseStream(WeatherManager.loadData);
					this.ReadWriteData(pooledBinaryReader, true);
				}
			}
			WeatherManager.loadData = null;
		}
	}

	// Token: 0x06009D16 RID: 40214 RVA: 0x003B59BC File Offset: 0x003B3BBC
	public static void Save(IBinaryReaderOrWriter _RW)
	{
		if (WeatherManager.loadData != null)
		{
			_RW.ReadWrite(WeatherManager.loadData.GetBuffer(), 0, (int)WeatherManager.loadData.Length);
			return;
		}
		if (WeatherManager.Instance)
		{
			WeatherManager.Instance.ReadWriteData(_RW, false);
		}
	}

	// Token: 0x06009D17 RID: 40215 RVA: 0x003B59FC File Offset: 0x003B3BFC
	[PublicizedFrom(EAccessModifier.Private)]
	public void ReadWriteData(IBinaryReaderOrWriter _RW, bool _load)
	{
		ushort num = _RW.ReadWrite(4);
		if (!_load)
		{
			_RW.ReadWrite((byte)GamePrefs.GetInt(EnumGamePrefs.DayNightLength));
			byte b = (byte)this.biomeWeather.Count;
			_RW.ReadWrite(b);
			for (int i = 0; i < (int)b; i++)
			{
				WeatherManager.BiomeWeather biomeWeather = this.biomeWeather[i];
				_RW.ReadWrite(biomeWeather.biomeDefinition.m_Id);
				_RW.ReadWrite((byte)biomeWeather.biomeDefinition.currentWeatherGroupIndex);
				_RW.ReadWrite(biomeWeather.stormWorldTime);
				_RW.ReadWrite((short)biomeWeather.stormDuration);
				_RW.ReadWrite(biomeWeather.nextRandWorldTime);
				for (int j = 0; j < 5; j++)
				{
					_RW.ReadWrite(biomeWeather.parameters[j].value);
				}
				_RW.ReadWrite(biomeWeather.rainParam.value);
				_RW.ReadWrite(biomeWeather.snowFallParam.value);
			}
			return;
		}
		if (num < 4)
		{
			return;
		}
		if ((int)_RW.ReadWrite(0) != GamePrefs.GetInt(EnumGamePrefs.DayNightLength))
		{
			return;
		}
		int num2 = (int)_RW.ReadWrite(0);
		for (int k = 0; k < num2; k++)
		{
			int num3 = (int)_RW.ReadWrite(0);
			WeatherManager.BiomeWeather biomeWeather2 = this.FindBiomeWeather(num3);
			if (biomeWeather2 == null)
			{
				return;
			}
			int num4 = (int)_RW.ReadWrite(0);
			biomeWeather2.biomeDefinition.SetWeatherGroup(num4);
			biomeWeather2.stormWorldTime = _RW.ReadWrite(0);
			biomeWeather2.stormDuration = (int)_RW.ReadWrite(0);
			biomeWeather2.nextRandWorldTime = _RW.ReadWrite(0);
			for (int l = 0; l < 5; l++)
			{
				float num5 = _RW.ReadWrite(0f);
				biomeWeather2.parameters[l].Set(num5);
				biomeWeather2.biomeDefinition.WeatherSetValue((BiomeDefinition.Probabilities.ProbType)l, num5);
			}
			biomeWeather2.rainParam.Set(_RW.ReadWrite(0f));
			biomeWeather2.snowFallParam.Set(_RW.ReadWrite(0f));
		}
	}

	// Token: 0x06009D18 RID: 40216 RVA: 0x003B5BF8 File Offset: 0x003B3DF8
	[PublicizedFrom(EAccessModifier.Private)]
	public WeatherManager.BiomeWeather FindBiomeWeather(int _id)
	{
		for (int i = 0; i < this.biomeWeather.Count; i++)
		{
			WeatherManager.BiomeWeather biomeWeather = this.biomeWeather[i];
			if ((int)biomeWeather.biomeDefinition.m_Id == _id)
			{
				return biomeWeather;
			}
		}
		return null;
	}

	// Token: 0x06009D19 RID: 40217 RVA: 0x003B5C3C File Offset: 0x003B3E3C
	[PublicizedFrom(EAccessModifier.Private)]
	public WeatherManager.BiomeWeather FindBiomeWeather(BiomeDefinition.BiomeType _type)
	{
		for (int i = 0; i < this.biomeWeather.Count; i++)
		{
			WeatherManager.BiomeWeather biomeWeather = this.biomeWeather[i];
			if (biomeWeather.biomeDefinition.m_BiomeType == _type)
			{
				return biomeWeather;
			}
		}
		return null;
	}

	// Token: 0x06009D1A RID: 40218 RVA: 0x003B5C80 File Offset: 0x003B3E80
	public bool IsStorming(BiomeDefinition.BiomeType _type)
	{
		WeatherManager.BiomeWeather biomeWeather = WeatherManager.Instance.FindBiomeWeather(_type);
		return biomeWeather != null && biomeWeather.stormState >= 2;
	}

	// Token: 0x06009D1B RID: 40219 RVA: 0x000027FC File Offset: 0x000009FC
	public static void ClearTemperatureOffSetHeights()
	{
	}

	// Token: 0x06009D1C RID: 40220 RVA: 0x000027FC File Offset: 0x000009FC
	public static void AddTemperatureOffSetHeight(float height, float degreesOffset)
	{
	}

	// Token: 0x06009D1D RID: 40221 RVA: 0x0003D2C6 File Offset: 0x0003B4C6
	public static float SeaLevel()
	{
		return 0f;
	}

	// Token: 0x06009D1E RID: 40222 RVA: 0x003B5CAA File Offset: 0x003B3EAA
	public static float GetCloudThickness()
	{
		if (WeatherManager.forceClouds >= 0f)
		{
			return WeatherManager.forceClouds * 100f;
		}
		if (WeatherManager.currentWeather == null)
		{
			return 0f;
		}
		return WeatherManager.currentWeather.parameters[2].value;
	}

	// Token: 0x06009D1F RID: 40223 RVA: 0x0003D2C6 File Offset: 0x0003B4C6
	public static float GetTemperature()
	{
		return 0f;
	}

	// Token: 0x06009D20 RID: 40224 RVA: 0x0003D2C6 File Offset: 0x0003B4C6
	public static float GetWindSpeed()
	{
		return 0f;
	}

	// Token: 0x06009D21 RID: 40225 RVA: 0x000027FC File Offset: 0x000009FC
	public static void EntityAddedToWorld(Entity entity)
	{
	}

	// Token: 0x06009D22 RID: 40226 RVA: 0x000027FC File Offset: 0x000009FC
	public static void EntityRemovedFromWorld(Entity entity)
	{
	}

	// Token: 0x06009D23 RID: 40227 RVA: 0x003B5CE2 File Offset: 0x003B3EE2
	public float GetCurrentSnowfallPercent()
	{
		if (WeatherManager.forceSnowfall < 0f && WeatherManager.currentWeather != null)
		{
			return WeatherManager.currentWeather.snowFallParam.value;
		}
		return WeatherManager.forceSnowfall;
	}

	// Token: 0x06009D24 RID: 40228 RVA: 0x003B5D0C File Offset: 0x003B3F0C
	public float GetCurrentRainfallPercent()
	{
		if (WeatherManager.forceRain < 0f && WeatherManager.currentWeather != null)
		{
			return WeatherManager.currentWeather.rainParam.value;
		}
		return WeatherManager.forceRain;
	}

	// Token: 0x06009D25 RID: 40229 RVA: 0x003B5D38 File Offset: 0x003B3F38
	public float GetCurrentWetPercent(EntityAlive _ea)
	{
		float num = 0f;
		BiomeDefinition biomeStandingOn = _ea.biomeStandingOn;
		if (biomeStandingOn != null && biomeStandingOn.m_BiomeType == BiomeDefinition.BiomeType.Snow && biomeStandingOn.currentWeatherGroup.stormLevel >= 2)
		{
			num = 1f;
		}
		float num2 = Utils.FastMax(this.GetCurrentSnowfallPercent(), this.GetCurrentRainfallPercent());
		return Utils.FastMax(num, num2);
	}

	// Token: 0x06009D26 RID: 40230 RVA: 0x003B5D8B File Offset: 0x003B3F8B
	public float GetCurrentCloudThicknessPercent()
	{
		return WeatherManager.GetCloudThickness() * 0.01f;
	}

	// Token: 0x06009D27 RID: 40231 RVA: 0x003B5D98 File Offset: 0x003B3F98
	public float GetCurrentTemperatureValue()
	{
		return WeatherManager.GetTemperature();
	}

	// Token: 0x06009D28 RID: 40232 RVA: 0x003B5D9F File Offset: 0x003B3F9F
	public static void SetSimRandom(float _random)
	{
		WeatherManager.forceSimRandom = _random;
		if (WeatherManager.Instance)
		{
			WeatherManager.Instance.weatherLastUpdateWorldTime = 0f;
		}
	}

	// Token: 0x06009D29 RID: 40233 RVA: 0x000027FC File Offset: 0x000009FC
	public static void ReloadSpectrums()
	{
	}

	// Token: 0x06009D2A RID: 40234 RVA: 0x000027FC File Offset: 0x000009FC
	public void Start()
	{
	}

	// Token: 0x06009D2B RID: 40235 RVA: 0x000027FC File Offset: 0x000009FC
	public void PushTransitions()
	{
	}

	// Token: 0x06009D2C RID: 40236 RVA: 0x003B5DC4 File Offset: 0x003B3FC4
	[PublicizedFrom(EAccessModifier.Private)]
	public void GenerateWeatherServerFrameUpdate()
	{
		WeatherManager.inWeatherGracePeriod = WeatherManager.worldTime < 22000;
		if (WeatherManager.inWeatherGracePeriod)
		{
			this.GeneralReset();
			return;
		}
		if (this.CustomWeatherTime > 0f)
		{
			this.CustomWeatherTime -= Time.deltaTime;
			if (this.CustomWeatherTime <= 0f)
			{
				this.CustomWeatherName = "";
				this.CustomWeatherTime = -1f;
				this.GeneralReset();
				this.SetAllWeather("default");
				this.weatherLastUpdateWorldTime = (float)WeatherManager.worldTime;
				return;
			}
		}
		else
		{
			string text = this.CalcGlobalWeatherType();
			if (text != null)
			{
				this.SetAllWeather(text);
				return;
			}
			this.weatherAllName = null;
			if (Utils.FastAbs((float)WeatherManager.worldTime - this.weatherLastUpdateWorldTime) >= 5f)
			{
				this.weatherLastUpdateWorldTime = (float)WeatherManager.worldTime;
				float stormFrequency = World.StormFrequency;
				for (int i = 0; i < this.biomeWeather.Count; i++)
				{
					this.biomeWeather[i].ServerTimeUpdate(WeatherManager.worldTime, stormFrequency);
				}
			}
		}
	}

	// Token: 0x06009D2D RID: 40237 RVA: 0x003B5ECC File Offset: 0x003B40CC
	[PublicizedFrom(EAccessModifier.Private)]
	public string CalcGlobalWeatherType()
	{
		if (SkyManager.IsBloodMoonVisible())
		{
			int num = 5000;
			for (int i = 0; i < this.biomeWeather.Count; i++)
			{
				WeatherManager.BiomeWeather biomeWeather = this.biomeWeather[i];
				if (biomeWeather.stormWorldTime - WeatherManager.worldTime < num)
				{
					biomeWeather.stormWorldTime = WeatherManager.worldTime + num;
				}
			}
			return "bloodMoon";
		}
		return null;
	}

	// Token: 0x06009D2E RID: 40238 RVA: 0x003B5F2C File Offset: 0x003B412C
	public void ForceWeather(string _weatherName, float _duration)
	{
		this.CustomWeatherName = _weatherName;
		this.CustomWeatherTime = _duration;
		this.GeneralReset();
		this.SetAllWeather(_weatherName);
	}

	// Token: 0x06009D2F RID: 40239 RVA: 0x003B5F4C File Offset: 0x003B414C
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetAllWeather(string _weatherName)
	{
		if (this.weatherAllName != _weatherName)
		{
			this.weatherAllName = _weatherName;
			for (int i = 0; i < this.biomeWeather.Count; i++)
			{
				this.biomeWeather[i].SetWeather(_weatherName);
			}
		}
	}

	// Token: 0x06009D30 RID: 40240 RVA: 0x003B5F98 File Offset: 0x003B4198
	public void SetStorm(string _biomeName, int _duration)
	{
		for (int i = 0; i < this.biomeWeather.Count; i++)
		{
			WeatherManager.BiomeWeather biomeWeather = this.biomeWeather[i];
			if (_biomeName == null || biomeWeather.biomeDefinition.m_sBiomeName == _biomeName)
			{
				biomeWeather.stormWorldTime = WeatherManager.worldTime;
				biomeWeather.stormDuration = _duration;
			}
		}
	}

	// Token: 0x06009D31 RID: 40241 RVA: 0x003B5FF0 File Offset: 0x003B41F0
	public void TriggerUpdate()
	{
		this.weatherAllName = null;
		this.weatherLastUpdateWorldTime = 0f;
	}

	// Token: 0x06009D32 RID: 40242 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void GeneralReset()
	{
	}

	// Token: 0x06009D33 RID: 40243 RVA: 0x003B6004 File Offset: 0x003B4204
	public void FrameUpdate()
	{
		if (GameManager.Instance == null || SkyManager.random == null)
		{
			return;
		}
		if (this.world == null)
		{
			return;
		}
		float time = Time.time;
		bool flag = GameManager.IsDedicatedServer || SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer;
		if (flag)
		{
			this.GenerateWeatherServerFrameUpdate();
		}
		for (int i = 0; i < this.biomeWeather.Count; i++)
		{
			this.biomeWeather[i].FrameUpdate(flag);
		}
	}

	// Token: 0x06009D34 RID: 40244 RVA: 0x003B607B File Offset: 0x003B427B
	public void TriggerThunder(int _playWorldTime, Vector3 _pos)
	{
		EnvironmentAudioManager.Instance.TriggerThunder(_pos);
	}

	// Token: 0x06009D35 RID: 40245 RVA: 0x003B6088 File Offset: 0x003B4288
	public void SendPackages()
	{
		this.CalcPackages();
		NetPackageWeather package = NetPackageManager.GetPackage<NetPackageWeather>();
		package.Setup(this.weatherPackages);
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(package, true, -1, -1, -1, null, 192, false);
	}

	// Token: 0x06009D36 RID: 40246 RVA: 0x003B60CC File Offset: 0x003B42CC
	[PublicizedFrom(EAccessModifier.Private)]
	public void CalcPackages()
	{
		int count = this.biomeWeather.Count;
		for (int i = 0; i < count; i++)
		{
			WeatherManager.BiomeWeather biomeWeather = this.biomeWeather[i];
			WeatherPackage weatherPackage = this.weatherPackages[i];
			weatherPackage.biomeId = biomeWeather.biomeDefinition.m_Id;
			weatherPackage.groupIndex = (byte)biomeWeather.biomeDefinition.currentWeatherGroupIndex;
			weatherPackage.remainingSeconds = biomeWeather.remainingSeconds;
			int num = 0;
			while (num < biomeWeather.parameters.Length && num < weatherPackage.param.Length)
			{
				weatherPackage.param[num] = biomeWeather.parameterFinals[num];
				num++;
			}
		}
	}

	// Token: 0x06009D37 RID: 40247 RVA: 0x003B6170 File Offset: 0x003B4370
	public override string ToString()
	{
		string text = string.Format("#{0}", this.biomeWeather.Count);
		for (int i = 0; i < this.biomeWeather.Count; i++)
		{
			WeatherManager.BiomeWeather biomeWeather = this.biomeWeather[i];
			text = text + "\n" + biomeWeather.ToString();
		}
		return text;
	}

	// Token: 0x06009D38 RID: 40248 RVA: 0x003B61CE File Offset: 0x003B43CE
	[Conditional("DEBUG_WEATHERNET")]
	public void LogNet(string _format = "", params object[] _args)
	{
		_format = string.Format("{0} WeatherManager net {1}", GameManager.frameCount, _format);
		Log.Warning(_format, _args);
	}

	// Token: 0x0400770F RID: 30479
	public const ushort cVersion = 4;

	// Token: 0x04007710 RID: 30480
	public static WeatherManager Instance;

	// Token: 0x04007711 RID: 30481
	public const float cStormWarningDuration = 60f;

	// Token: 0x04007712 RID: 30482
	public const int BaseTemperature = 70;

	// Token: 0x04007713 RID: 30483
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cWeatherTransitionSeconds = 10f;

	// Token: 0x04007714 RID: 30484
	public List<WeatherManager.BiomeWeather> biomeWeather;

	// Token: 0x04007715 RID: 30485
	public static float forceClouds = -1f;

	// Token: 0x04007716 RID: 30486
	public static float forceRain = -1f;

	// Token: 0x04007717 RID: 30487
	public static float forceSnowfall = -1f;

	// Token: 0x04007718 RID: 30488
	public const float cForceTempDefault = -100f;

	// Token: 0x04007719 RID: 30489
	public static float forceTemperature = -100f;

	// Token: 0x0400771A RID: 30490
	public static float forceWind = -1f;

	// Token: 0x0400771B RID: 30491
	public static bool needToReUpdateWeatherSpectrums;

	// Token: 0x0400771C RID: 30492
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float forceSimRandom = -1f;

	// Token: 0x0400771D RID: 30493
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int cGracePeriodWorldTime = 22000;

	// Token: 0x0400771E RID: 30494
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int worldTime;

	// Token: 0x0400771F RID: 30495
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public World world;

	// Token: 0x04007720 RID: 30496
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static GameRandom gameRand;

	// Token: 0x04007721 RID: 30497
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isGameModeNormal;

	// Token: 0x04007722 RID: 30498
	public static bool inWeatherGracePeriod = true;

	// Token: 0x04007723 RID: 30499
	public static WeatherManager.BiomeWeather currentWeather;

	// Token: 0x04007724 RID: 30500
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public WeatherPackage[] weatherPackages;

	// Token: 0x04007725 RID: 30501
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cLightningDelayMin = 30f;

	// Token: 0x04007726 RID: 30502
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cLightningDelayMax = 60f;

	// Token: 0x04007727 RID: 30503
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float thunderDelay = 30f;

	// Token: 0x04007728 RID: 30504
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float stormFog;

	// Token: 0x04007729 RID: 30505
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int frameCount = -1;

	// Token: 0x0400772A RID: 30506
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 playerPosition;

	// Token: 0x0400772B RID: 30507
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float checkPlayerMoveTime;

	// Token: 0x0400772C RID: 30508
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static MemoryStream loadData;

	// Token: 0x0400772D RID: 30509
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string weatherAllName;

	// Token: 0x0400772E RID: 30510
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float weatherLastUpdateWorldTime;

	// Token: 0x0400772F RID: 30511
	public string CustomWeatherName = "";

	// Token: 0x04007730 RID: 30512
	public float CustomWeatherTime = -1f;

	// Token: 0x02001378 RID: 4984
	[Serializable]
	public class Param
	{
		// Token: 0x06009D3B RID: 40251 RVA: 0x003B626F File Offset: 0x003B446F
		public Param(float _value, float _step1Time = 0.25f)
		{
			this.name = "Param";
			this.value = _value;
			this.target = _value;
			this.step1Time = _step1Time;
		}

		// Token: 0x06009D3C RID: 40252 RVA: 0x003B6298 File Offset: 0x003B4498
		public void FrameUpdate()
		{
			float time = Time.time;
			if (this.value == this.target)
			{
				this.lastTime = time;
				return;
			}
			float num = time - this.lastTime;
			if (num < 0f)
			{
				this.lastTime = time;
			}
			if (num >= 0.01f)
			{
				if (num > 1f)
				{
					num = 1f;
				}
				float num2 = num / this.step1Time;
				if (this.value > this.target)
				{
					this.value -= num2;
					if (this.value < this.target)
					{
						this.value = this.target;
					}
				}
				else
				{
					this.value += num2;
					if (this.value > this.target)
					{
						this.value = this.target;
					}
				}
				this.lastTime = time;
			}
		}

		// Token: 0x06009D3D RID: 40253 RVA: 0x003B635F File Offset: 0x003B455F
		public void Clamp()
		{
			this.value = Utils.FastClamp01(this.value);
			this.target = Utils.FastClamp01(this.target);
		}

		// Token: 0x06009D3E RID: 40254 RVA: 0x003B6383 File Offset: 0x003B4583
		public void Set(float _value)
		{
			this.value = _value;
			this.target = _value;
		}

		// Token: 0x06009D3F RID: 40255 RVA: 0x003B6393 File Offset: 0x003B4593
		public void SetTarget(float _target)
		{
			if (this.target == _target)
			{
				return;
			}
			this.target = _target;
			this.lastTime = Time.time;
		}

		// Token: 0x04007731 RID: 30513
		public string name;

		// Token: 0x04007732 RID: 30514
		public float value;

		// Token: 0x04007733 RID: 30515
		public float target;

		// Token: 0x04007734 RID: 30516
		public float step1Time;

		// Token: 0x04007735 RID: 30517
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public float lastTime;
	}

	// Token: 0x02001379 RID: 4985
	[Serializable]
	public class BiomeWeather
	{
		// Token: 0x06009D40 RID: 40256 RVA: 0x003B63B4 File Offset: 0x003B45B4
		public BiomeWeather(BiomeDefinition _definition)
		{
			this.biomeDefinition = _definition;
			for (int i = 0; i < 5; i++)
			{
				this.parameters[i] = new WeatherManager.Param(0f, 0.3f);
			}
			this.parameters[0].step1Time = 0.15f;
		}

		// Token: 0x06009D41 RID: 40257 RVA: 0x003B6445 File Offset: 0x003B4645
		public float CloudThickness()
		{
			return this.parameters[2].value;
		}

		// Token: 0x06009D42 RID: 40258 RVA: 0x003B6454 File Offset: 0x003B4654
		public float FogPercent()
		{
			return this.parameters[4].value * 0.01f + WeatherManager.stormFog;
		}

		// Token: 0x06009D43 RID: 40259 RVA: 0x003B646F File Offset: 0x003B466F
		public float Wind()
		{
			return this.parameters[3].value;
		}

		// Token: 0x06009D44 RID: 40260 RVA: 0x003B6480 File Offset: 0x003B4680
		public void FrameUpdate(bool _isServer)
		{
			WeatherManager.Param[] array = this.parameters;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].FrameUpdate();
			}
			float value = this.parameters[1].value;
			float num = this.parameters[2].value;
			float num2 = this.parameters[0].value;
			if (_isServer)
			{
				this.CalcGlobalTemperature(value, num, ref num2);
				if (WeatherManager.forceClouds >= 0f)
				{
					num = WeatherManager.forceClouds * 100f;
				}
				if (WeatherManager.forceTemperature > -100f)
				{
					num2 = WeatherManager.forceTemperature;
				}
			}
			this.parameterFinals[1] = value;
			this.parameterFinals[2] = num;
			this.parameterFinals[0] = num2;
			this.parameterFinals[3] = this.parameters[3].value;
			this.parameterFinals[4] = this.parameters[4].value;
			float num3 = (value * 0.01f - 0.3f) / 0.7f;
			num3 = Utils.FastMax(0f, num3);
			this.rainParam.target = ((num2 > 32f) ? num3 : 0f);
			this.snowFallParam.target = ((num2 <= 32f) ? num3 : 0f);
			this.rainParam.FrameUpdate();
			this.snowFallParam.FrameUpdate();
			if (!_isServer)
			{
				this.stormState = this.biomeDefinition.currentWeatherGroup.stormLevel;
			}
		}

		// Token: 0x06009D45 RID: 40261 RVA: 0x003B65E0 File Offset: 0x003B47E0
		[PublicizedFrom(EAccessModifier.Private)]
		public void CalcGlobalTemperature(float _precipitation, float _cloudThickness, ref float _temperature)
		{
			float num = Utils.FastClamp01(_precipitation * 0.01f);
			_temperature += num * -5f;
			float num2 = SkyManager.GetSunPercent();
			if (num2 > 0f)
			{
				num2 *= 1f - _cloudThickness * 0.01f;
			}
			_temperature += (-num2 + 1f) * -7.5f;
		}

		// Token: 0x06009D46 RID: 40262 RVA: 0x003B6638 File Offset: 0x003B4838
		public void ParamsFrameUpdate()
		{
			for (int i = 0; i < this.parameters.Length; i++)
			{
				WeatherManager.Param param = this.parameters[i];
				param.FrameUpdate();
				this.parameterFinals[i] = param.value;
			}
			this.rainParam.FrameUpdate();
			this.snowFallParam.FrameUpdate();
		}

		// Token: 0x06009D47 RID: 40263 RVA: 0x003B668C File Offset: 0x003B488C
		public void Reset()
		{
			this.stormWorldTime = 0;
			this.nextRandWorldTime = 0;
			this.biomeDefinition.WeatherRandomize(0f);
			for (int i = 0; i < 5; i++)
			{
				this.parameters[i].Set(this.biomeDefinition.WeatherGetValue((BiomeDefinition.Probabilities.ProbType)i));
			}
			this.rainParam.Set(0f);
			this.snowFallParam.Set(0f);
		}

		// Token: 0x06009D48 RID: 40264 RVA: 0x003B66FC File Offset: 0x003B48FC
		public void ServerTimeUpdate(int _worldTime, float _freq)
		{
			if (_freq == 0f)
			{
				if (this.stormWorldTime != 2147483647)
				{
					this.stormWorldTime = int.MaxValue;
					this.nextRandWorldTime = 0;
				}
			}
			else if (this.stormWorldTime == 2147483647)
			{
				this.stormWorldTime = 0;
			}
			int num = _worldTime - this.stormWorldTime;
			if (num >= 0)
			{
				float num2 = 60f / (float)GamePrefs.GetInt(EnumGamePrefs.DayNightLength);
				int num3 = this.biomeDefinition.WeatherGetDuration("stormbuild");
				num3 = (int)((float)num3 * num2);
				int num4 = num3 - num;
				if (num4 > 0)
				{
					if (this.stormState != 1)
					{
						this.stormState = 1;
						this.SetWeather("stormbuild");
					}
					int @int = GameStats.GetInt(EnumGameStats.TimeOfDayIncPerSec);
					if (@int > 0)
					{
						this.remainingSeconds = (byte)Utils.FastMin(num4 / @int, 255);
					}
					return;
				}
				if (WeatherManager.worldTime < this.stormWorldTime + this.stormDuration)
				{
					if (this.stormState != 2)
					{
						this.stormState = 2;
						this.SetWeather("storm");
					}
					return;
				}
				this.stormState = 0;
				Vector2i vector2i;
				int num5 = this.biomeDefinition.WeatherGetDuration("storm", out vector2i);
				this.stormWorldTime = WeatherManager.worldTime + (int)((float)WeatherManager.gameRand.RandomRange(vector2i.x, vector2i.y) / _freq);
				int num6 = num5 / 8;
				num5 += WeatherManager.gameRand.RandomRange(num5 - num6, num5 + num6);
				num5 = (int)((float)num5 * num2);
				this.stormDuration = num3 + num5;
			}
			if (_worldTime >= this.nextRandWorldTime)
			{
				float num7 = WeatherManager.gameRand.RandomFloat;
				if (WeatherManager.forceSimRandom >= 0f)
				{
					num7 = WeatherManager.forceSimRandom;
				}
				this.SetWeather(_worldTime, num7);
			}
		}

		// Token: 0x06009D49 RID: 40265 RVA: 0x003B6898 File Offset: 0x003B4A98
		[PublicizedFrom(EAccessModifier.Private)]
		public void SetWeather(int _worldTime, float _rand)
		{
			if (this.biomeDefinition != null)
			{
				this.biomeDefinition.WeatherRandomize(_rand);
				for (int i = 0; i < 5; i++)
				{
					BiomeDefinition.Probabilities.ProbType probType = (BiomeDefinition.Probabilities.ProbType)i;
					this.parameters[i].target = this.biomeDefinition.WeatherGetValue(probType);
				}
				this.nextRandWorldTime = _worldTime + this.biomeDefinition.currentWeatherGroup.duration;
			}
		}

		// Token: 0x06009D4A RID: 40266 RVA: 0x003B68F8 File Offset: 0x003B4AF8
		public void SetWeather(string name)
		{
			if (this.biomeDefinition != null)
			{
				this.biomeDefinition.WeatherRandomize(name);
				for (int i = 0; i < 5; i++)
				{
					BiomeDefinition.Probabilities.ProbType probType = (BiomeDefinition.Probabilities.ProbType)i;
					this.parameters[i].target = this.biomeDefinition.WeatherGetValue(probType);
				}
				this.nextRandWorldTime = 0;
			}
		}

		// Token: 0x06009D4B RID: 40267 RVA: 0x003B6948 File Offset: 0x003B4B48
		public override string ToString()
		{
			string text = string.Format("{0}: {1}, nxtT{2}, ", this.biomeDefinition.m_sBiomeName, this.biomeDefinition.weatherName, this.nextRandWorldTime);
			for (int i = 0; i < 5; i++)
			{
				BiomeDefinition.Probabilities.ProbType probType = (BiomeDefinition.Probabilities.ProbType)i;
				text += string.Format("{0} {1}, ", probType, this.biomeDefinition.WeatherGetValue(probType));
			}
			text += string.Format("rain {0}, ", this.rainParam.value);
			text += string.Format("snow {0}, ", this.snowFallParam.value);
			return text + string.Format("storm WT {0}, dur {1}, state {2}", this.stormWorldTime, this.stormDuration, this.stormState);
		}

		// Token: 0x04007736 RID: 30518
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public const float cStep1Time = 0.15f;

		// Token: 0x04007737 RID: 30519
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public const float cPrecipitationPercentVisibleMin = 0.3f;

		// Token: 0x04007738 RID: 30520
		public BiomeDefinition biomeDefinition;

		// Token: 0x04007739 RID: 30521
		public int stormWorldTime;

		// Token: 0x0400773A RID: 30522
		public int stormDuration;

		// Token: 0x0400773B RID: 30523
		public int stormState;

		// Token: 0x0400773C RID: 30524
		public int stormIndex;

		// Token: 0x0400773D RID: 30525
		public int nextRandWorldTime;

		// Token: 0x0400773E RID: 30526
		public byte remainingSeconds;

		// Token: 0x0400773F RID: 30527
		public WeatherManager.Param[] parameters = new WeatherManager.Param[5];

		// Token: 0x04007740 RID: 30528
		public float[] parameterFinals = new float[5];

		// Token: 0x04007741 RID: 30529
		public WeatherManager.Param rainParam = new WeatherManager.Param(0f, 0.25f);

		// Token: 0x04007742 RID: 30530
		public WeatherManager.Param snowFallParam = new WeatherManager.Param(0f, 0.25f);
	}
}

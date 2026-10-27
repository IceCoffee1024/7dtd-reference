using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

// Token: 0x020013A9 RID: 5033
public class WeatherManager : MonoBehaviour
{
	// Token: 0x06009E12 RID: 40466 RVA: 0x003BC663 File Offset: 0x003BA863
	public static void Init(World _world, GameObject _obj)
	{
		WeatherManager.Cleanup();
		WeatherManager.Instance = _obj.GetComponent<WeatherManager>();
		WeatherManager.Instance.Init(_world);
	}

	// Token: 0x06009E13 RID: 40467 RVA: 0x003BC680 File Offset: 0x003BA880
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

	// Token: 0x06009E14 RID: 40468 RVA: 0x003BC6D8 File Offset: 0x003BA8D8
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

	// Token: 0x06009E15 RID: 40469 RVA: 0x003BC7C8 File Offset: 0x003BA9C8
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

	// Token: 0x06009E16 RID: 40470 RVA: 0x003BC822 File Offset: 0x003BAA22
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

	// Token: 0x06009E17 RID: 40471 RVA: 0x003BC84C File Offset: 0x003BAA4C
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

	// Token: 0x06009E18 RID: 40472 RVA: 0x003BC89A File Offset: 0x003BAA9A
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

	// Token: 0x06009E19 RID: 40473 RVA: 0x003BC8D8 File Offset: 0x003BAAD8
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

	// Token: 0x06009E1A RID: 40474 RVA: 0x003BC93C File Offset: 0x003BAB3C
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

	// Token: 0x06009E1B RID: 40475 RVA: 0x003BC97C File Offset: 0x003BAB7C
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

	// Token: 0x06009E1C RID: 40476 RVA: 0x003BCB78 File Offset: 0x003BAD78
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

	// Token: 0x06009E1D RID: 40477 RVA: 0x003BCBBC File Offset: 0x003BADBC
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

	// Token: 0x06009E1E RID: 40478 RVA: 0x003BCC00 File Offset: 0x003BAE00
	public bool IsStorming(BiomeDefinition.BiomeType _type)
	{
		WeatherManager.BiomeWeather biomeWeather = WeatherManager.Instance.FindBiomeWeather(_type);
		return biomeWeather != null && biomeWeather.stormState >= 2;
	}

	// Token: 0x06009E1F RID: 40479 RVA: 0x000027FC File Offset: 0x000009FC
	public static void ClearTemperatureOffSetHeights()
	{
	}

	// Token: 0x06009E20 RID: 40480 RVA: 0x000027FC File Offset: 0x000009FC
	public static void AddTemperatureOffSetHeight(float height, float degreesOffset)
	{
	}

	// Token: 0x06009E21 RID: 40481 RVA: 0x0003D372 File Offset: 0x0003B572
	public static float SeaLevel()
	{
		return 0f;
	}

	// Token: 0x06009E22 RID: 40482 RVA: 0x003BCC2A File Offset: 0x003BAE2A
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

	// Token: 0x06009E23 RID: 40483 RVA: 0x0003D372 File Offset: 0x0003B572
	public static float GetTemperature()
	{
		return 0f;
	}

	// Token: 0x06009E24 RID: 40484 RVA: 0x0003D372 File Offset: 0x0003B572
	public static float GetWindSpeed()
	{
		return 0f;
	}

	// Token: 0x06009E25 RID: 40485 RVA: 0x000027FC File Offset: 0x000009FC
	public static void EntityAddedToWorld(Entity entity)
	{
	}

	// Token: 0x06009E26 RID: 40486 RVA: 0x000027FC File Offset: 0x000009FC
	public static void EntityRemovedFromWorld(Entity entity)
	{
	}

	// Token: 0x06009E27 RID: 40487 RVA: 0x003BCC62 File Offset: 0x003BAE62
	public float GetCurrentSnowfallPercent()
	{
		if (WeatherManager.forceSnowfall < 0f && WeatherManager.currentWeather != null)
		{
			return WeatherManager.currentWeather.snowFallParam.value;
		}
		return WeatherManager.forceSnowfall;
	}

	// Token: 0x06009E28 RID: 40488 RVA: 0x003BCC8C File Offset: 0x003BAE8C
	public float GetCurrentRainfallPercent()
	{
		if (WeatherManager.forceRain < 0f && WeatherManager.currentWeather != null)
		{
			return WeatherManager.currentWeather.rainParam.value;
		}
		return WeatherManager.forceRain;
	}

	// Token: 0x06009E29 RID: 40489 RVA: 0x003BCCB8 File Offset: 0x003BAEB8
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

	// Token: 0x06009E2A RID: 40490 RVA: 0x003BCD0B File Offset: 0x003BAF0B
	public float GetCurrentCloudThicknessPercent()
	{
		return WeatherManager.GetCloudThickness() * 0.01f;
	}

	// Token: 0x06009E2B RID: 40491 RVA: 0x003BCD18 File Offset: 0x003BAF18
	public float GetCurrentTemperatureValue()
	{
		return WeatherManager.GetTemperature();
	}

	// Token: 0x06009E2C RID: 40492 RVA: 0x003BCD1F File Offset: 0x003BAF1F
	public static void SetSimRandom(float _random)
	{
		WeatherManager.forceSimRandom = _random;
		if (WeatherManager.Instance)
		{
			WeatherManager.Instance.weatherLastUpdateWorldTime = 0f;
		}
	}

	// Token: 0x06009E2D RID: 40493 RVA: 0x000027FC File Offset: 0x000009FC
	public static void ReloadSpectrums()
	{
	}

	// Token: 0x06009E2E RID: 40494 RVA: 0x000027FC File Offset: 0x000009FC
	public void Start()
	{
	}

	// Token: 0x06009E2F RID: 40495 RVA: 0x000027FC File Offset: 0x000009FC
	public void PushTransitions()
	{
	}

	// Token: 0x06009E30 RID: 40496 RVA: 0x003BCD44 File Offset: 0x003BAF44
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

	// Token: 0x06009E31 RID: 40497 RVA: 0x003BCE4C File Offset: 0x003BB04C
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

	// Token: 0x06009E32 RID: 40498 RVA: 0x003BCEAC File Offset: 0x003BB0AC
	public void ForceWeather(string _weatherName, float _duration)
	{
		this.CustomWeatherName = _weatherName;
		this.CustomWeatherTime = _duration;
		this.GeneralReset();
		this.SetAllWeather(_weatherName);
	}

	// Token: 0x06009E33 RID: 40499 RVA: 0x003BCECC File Offset: 0x003BB0CC
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

	// Token: 0x06009E34 RID: 40500 RVA: 0x003BCF18 File Offset: 0x003BB118
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

	// Token: 0x06009E35 RID: 40501 RVA: 0x003BCF70 File Offset: 0x003BB170
	public void TriggerUpdate()
	{
		this.weatherAllName = null;
		this.weatherLastUpdateWorldTime = 0f;
	}

	// Token: 0x06009E36 RID: 40502 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void GeneralReset()
	{
	}

	// Token: 0x06009E37 RID: 40503 RVA: 0x003BCF84 File Offset: 0x003BB184
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

	// Token: 0x06009E38 RID: 40504 RVA: 0x003BCFFB File Offset: 0x003BB1FB
	public void TriggerThunder(int _playWorldTime, Vector3 _pos)
	{
		EnvironmentAudioManager.Instance.TriggerThunder(_pos);
	}

	// Token: 0x06009E39 RID: 40505 RVA: 0x003BD008 File Offset: 0x003BB208
	public void SendPackages()
	{
		this.CalcPackages();
		NetPackageWeather package = NetPackageManager.GetPackage<NetPackageWeather>();
		package.Setup(this.weatherPackages);
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(package, true, -1, -1, -1, null, 192, false);
	}

	// Token: 0x06009E3A RID: 40506 RVA: 0x003BD04C File Offset: 0x003BB24C
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

	// Token: 0x06009E3B RID: 40507 RVA: 0x003BD0F0 File Offset: 0x003BB2F0
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

	// Token: 0x06009E3C RID: 40508 RVA: 0x003BD14E File Offset: 0x003BB34E
	[Conditional("DEBUG_WEATHERNET")]
	public void LogNet(string _format = "", params object[] _args)
	{
		_format = string.Format("{0} WeatherManager net {1}", GameManager.frameCount, _format);
		Log.Warning(_format, _args);
	}

	// Token: 0x040077CB RID: 30667
	public const ushort cVersion = 4;

	// Token: 0x040077CC RID: 30668
	public static WeatherManager Instance;

	// Token: 0x040077CD RID: 30669
	public const float cStormWarningDuration = 60f;

	// Token: 0x040077CE RID: 30670
	public const int BaseTemperature = 70;

	// Token: 0x040077CF RID: 30671
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cWeatherTransitionSeconds = 10f;

	// Token: 0x040077D0 RID: 30672
	public List<WeatherManager.BiomeWeather> biomeWeather;

	// Token: 0x040077D1 RID: 30673
	public static float forceClouds = -1f;

	// Token: 0x040077D2 RID: 30674
	public static float forceRain = -1f;

	// Token: 0x040077D3 RID: 30675
	public static float forceSnowfall = -1f;

	// Token: 0x040077D4 RID: 30676
	public const float cForceTempDefault = -100f;

	// Token: 0x040077D5 RID: 30677
	public static float forceTemperature = -100f;

	// Token: 0x040077D6 RID: 30678
	public static float forceWind = -1f;

	// Token: 0x040077D7 RID: 30679
	public static bool needToReUpdateWeatherSpectrums;

	// Token: 0x040077D8 RID: 30680
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float forceSimRandom = -1f;

	// Token: 0x040077D9 RID: 30681
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int cGracePeriodWorldTime = 22000;

	// Token: 0x040077DA RID: 30682
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int worldTime;

	// Token: 0x040077DB RID: 30683
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public World world;

	// Token: 0x040077DC RID: 30684
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static GameRandom gameRand;

	// Token: 0x040077DD RID: 30685
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isGameModeNormal;

	// Token: 0x040077DE RID: 30686
	public static bool inWeatherGracePeriod = true;

	// Token: 0x040077DF RID: 30687
	public static WeatherManager.BiomeWeather currentWeather;

	// Token: 0x040077E0 RID: 30688
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public WeatherPackage[] weatherPackages;

	// Token: 0x040077E1 RID: 30689
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cLightningDelayMin = 30f;

	// Token: 0x040077E2 RID: 30690
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cLightningDelayMax = 60f;

	// Token: 0x040077E3 RID: 30691
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float thunderDelay = 30f;

	// Token: 0x040077E4 RID: 30692
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float stormFog;

	// Token: 0x040077E5 RID: 30693
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int frameCount = -1;

	// Token: 0x040077E6 RID: 30694
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 playerPosition;

	// Token: 0x040077E7 RID: 30695
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float checkPlayerMoveTime;

	// Token: 0x040077E8 RID: 30696
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static MemoryStream loadData;

	// Token: 0x040077E9 RID: 30697
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string weatherAllName;

	// Token: 0x040077EA RID: 30698
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float weatherLastUpdateWorldTime;

	// Token: 0x040077EB RID: 30699
	public string CustomWeatherName = "";

	// Token: 0x040077EC RID: 30700
	public float CustomWeatherTime = -1f;

	// Token: 0x020013AA RID: 5034
	[Serializable]
	public class Param
	{
		// Token: 0x06009E3F RID: 40511 RVA: 0x003BD1EF File Offset: 0x003BB3EF
		public Param(float _value, float _step1Time = 0.25f)
		{
			this.name = "Param";
			this.value = _value;
			this.target = _value;
			this.step1Time = _step1Time;
		}

		// Token: 0x06009E40 RID: 40512 RVA: 0x003BD218 File Offset: 0x003BB418
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

		// Token: 0x06009E41 RID: 40513 RVA: 0x003BD2DF File Offset: 0x003BB4DF
		public void Clamp()
		{
			this.value = Utils.FastClamp01(this.value);
			this.target = Utils.FastClamp01(this.target);
		}

		// Token: 0x06009E42 RID: 40514 RVA: 0x003BD303 File Offset: 0x003BB503
		public void Set(float _value)
		{
			this.value = _value;
			this.target = _value;
		}

		// Token: 0x06009E43 RID: 40515 RVA: 0x003BD313 File Offset: 0x003BB513
		public void SetTarget(float _target)
		{
			if (this.target == _target)
			{
				return;
			}
			this.target = _target;
			this.lastTime = Time.time;
		}

		// Token: 0x040077ED RID: 30701
		public string name;

		// Token: 0x040077EE RID: 30702
		public float value;

		// Token: 0x040077EF RID: 30703
		public float target;

		// Token: 0x040077F0 RID: 30704
		public float step1Time;

		// Token: 0x040077F1 RID: 30705
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public float lastTime;
	}

	// Token: 0x020013AB RID: 5035
	[Serializable]
	public class BiomeWeather
	{
		// Token: 0x06009E44 RID: 40516 RVA: 0x003BD334 File Offset: 0x003BB534
		public BiomeWeather(BiomeDefinition _definition)
		{
			this.biomeDefinition = _definition;
			for (int i = 0; i < 5; i++)
			{
				this.parameters[i] = new WeatherManager.Param(0f, 0.3f);
			}
			this.parameters[0].step1Time = 0.15f;
		}

		// Token: 0x06009E45 RID: 40517 RVA: 0x003BD3C5 File Offset: 0x003BB5C5
		public float CloudThickness()
		{
			return this.parameters[2].value;
		}

		// Token: 0x06009E46 RID: 40518 RVA: 0x003BD3D4 File Offset: 0x003BB5D4
		public float FogPercent()
		{
			return this.parameters[4].value * 0.01f + WeatherManager.stormFog;
		}

		// Token: 0x06009E47 RID: 40519 RVA: 0x003BD3EF File Offset: 0x003BB5EF
		public float Wind()
		{
			return this.parameters[3].value;
		}

		// Token: 0x06009E48 RID: 40520 RVA: 0x003BD400 File Offset: 0x003BB600
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

		// Token: 0x06009E49 RID: 40521 RVA: 0x003BD560 File Offset: 0x003BB760
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

		// Token: 0x06009E4A RID: 40522 RVA: 0x003BD5B8 File Offset: 0x003BB7B8
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

		// Token: 0x06009E4B RID: 40523 RVA: 0x003BD60C File Offset: 0x003BB80C
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

		// Token: 0x06009E4C RID: 40524 RVA: 0x003BD67C File Offset: 0x003BB87C
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

		// Token: 0x06009E4D RID: 40525 RVA: 0x003BD818 File Offset: 0x003BBA18
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

		// Token: 0x06009E4E RID: 40526 RVA: 0x003BD878 File Offset: 0x003BBA78
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

		// Token: 0x06009E4F RID: 40527 RVA: 0x003BD8C8 File Offset: 0x003BBAC8
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

		// Token: 0x040077F2 RID: 30706
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public const float cStep1Time = 0.15f;

		// Token: 0x040077F3 RID: 30707
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public const float cPrecipitationPercentVisibleMin = 0.3f;

		// Token: 0x040077F4 RID: 30708
		public BiomeDefinition biomeDefinition;

		// Token: 0x040077F5 RID: 30709
		public int stormWorldTime;

		// Token: 0x040077F6 RID: 30710
		public int stormDuration;

		// Token: 0x040077F7 RID: 30711
		public int stormState;

		// Token: 0x040077F8 RID: 30712
		public int stormIndex;

		// Token: 0x040077F9 RID: 30713
		public int nextRandWorldTime;

		// Token: 0x040077FA RID: 30714
		public byte remainingSeconds;

		// Token: 0x040077FB RID: 30715
		public WeatherManager.Param[] parameters = new WeatherManager.Param[5];

		// Token: 0x040077FC RID: 30716
		public float[] parameterFinals = new float[5];

		// Token: 0x040077FD RID: 30717
		public WeatherManager.Param rainParam = new WeatherManager.Param(0f, 0.25f);

		// Token: 0x040077FE RID: 30718
		public WeatherManager.Param snowFallParam = new WeatherManager.Param(0f, 0.25f);
	}
}

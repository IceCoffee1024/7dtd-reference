using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

// Token: 0x02001397 RID: 5015
public class WeatherManager : MonoBehaviour
{
	// Token: 0x06009DBA RID: 40378 RVA: 0x003BA25F File Offset: 0x003B845F
	public static void Init(World _world, GameObject _obj)
	{
		WeatherManager.Cleanup();
		WeatherManager.Instance = _obj.GetComponent<WeatherManager>();
		WeatherManager.Instance.Init(_world);
	}

	// Token: 0x06009DBB RID: 40379 RVA: 0x003BA27C File Offset: 0x003B847C
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

	// Token: 0x06009DBC RID: 40380 RVA: 0x003BA2D4 File Offset: 0x003B84D4
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

	// Token: 0x06009DBD RID: 40381 RVA: 0x003BA3C4 File Offset: 0x003B85C4
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

	// Token: 0x06009DBE RID: 40382 RVA: 0x003BA41E File Offset: 0x003B861E
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

	// Token: 0x06009DBF RID: 40383 RVA: 0x003BA448 File Offset: 0x003B8648
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

	// Token: 0x06009DC0 RID: 40384 RVA: 0x003BA496 File Offset: 0x003B8696
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

	// Token: 0x06009DC1 RID: 40385 RVA: 0x003BA4D4 File Offset: 0x003B86D4
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

	// Token: 0x06009DC2 RID: 40386 RVA: 0x003BA538 File Offset: 0x003B8738
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

	// Token: 0x06009DC3 RID: 40387 RVA: 0x003BA578 File Offset: 0x003B8778
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

	// Token: 0x06009DC4 RID: 40388 RVA: 0x003BA774 File Offset: 0x003B8974
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

	// Token: 0x06009DC5 RID: 40389 RVA: 0x003BA7B8 File Offset: 0x003B89B8
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

	// Token: 0x06009DC6 RID: 40390 RVA: 0x003BA7FC File Offset: 0x003B89FC
	public bool IsStorming(BiomeDefinition.BiomeType _type)
	{
		WeatherManager.BiomeWeather biomeWeather = WeatherManager.Instance.FindBiomeWeather(_type);
		return biomeWeather != null && biomeWeather.stormState >= 2;
	}

	// Token: 0x06009DC7 RID: 40391 RVA: 0x000027FC File Offset: 0x000009FC
	public static void ClearTemperatureOffSetHeights()
	{
	}

	// Token: 0x06009DC8 RID: 40392 RVA: 0x000027FC File Offset: 0x000009FC
	public static void AddTemperatureOffSetHeight(float height, float degreesOffset)
	{
	}

	// Token: 0x06009DC9 RID: 40393 RVA: 0x0003D3B2 File Offset: 0x0003B5B2
	public static float SeaLevel()
	{
		return 0f;
	}

	// Token: 0x06009DCA RID: 40394 RVA: 0x003BA826 File Offset: 0x003B8A26
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

	// Token: 0x06009DCB RID: 40395 RVA: 0x0003D3B2 File Offset: 0x0003B5B2
	public static float GetTemperature()
	{
		return 0f;
	}

	// Token: 0x06009DCC RID: 40396 RVA: 0x0003D3B2 File Offset: 0x0003B5B2
	public static float GetWindSpeed()
	{
		return 0f;
	}

	// Token: 0x06009DCD RID: 40397 RVA: 0x000027FC File Offset: 0x000009FC
	public static void EntityAddedToWorld(Entity entity)
	{
	}

	// Token: 0x06009DCE RID: 40398 RVA: 0x000027FC File Offset: 0x000009FC
	public static void EntityRemovedFromWorld(Entity entity)
	{
	}

	// Token: 0x06009DCF RID: 40399 RVA: 0x003BA85E File Offset: 0x003B8A5E
	public float GetCurrentSnowfallPercent()
	{
		if (WeatherManager.forceSnowfall < 0f && WeatherManager.currentWeather != null)
		{
			return WeatherManager.currentWeather.snowFallParam.value;
		}
		return WeatherManager.forceSnowfall;
	}

	// Token: 0x06009DD0 RID: 40400 RVA: 0x003BA888 File Offset: 0x003B8A88
	public float GetCurrentRainfallPercent()
	{
		if (WeatherManager.forceRain < 0f && WeatherManager.currentWeather != null)
		{
			return WeatherManager.currentWeather.rainParam.value;
		}
		return WeatherManager.forceRain;
	}

	// Token: 0x06009DD1 RID: 40401 RVA: 0x003BA8B4 File Offset: 0x003B8AB4
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

	// Token: 0x06009DD2 RID: 40402 RVA: 0x003BA907 File Offset: 0x003B8B07
	public float GetCurrentCloudThicknessPercent()
	{
		return WeatherManager.GetCloudThickness() * 0.01f;
	}

	// Token: 0x06009DD3 RID: 40403 RVA: 0x003BA914 File Offset: 0x003B8B14
	public float GetCurrentTemperatureValue()
	{
		return WeatherManager.GetTemperature();
	}

	// Token: 0x06009DD4 RID: 40404 RVA: 0x003BA91B File Offset: 0x003B8B1B
	public static void SetSimRandom(float _random)
	{
		WeatherManager.forceSimRandom = _random;
		if (WeatherManager.Instance)
		{
			WeatherManager.Instance.weatherLastUpdateWorldTime = 0f;
		}
	}

	// Token: 0x06009DD5 RID: 40405 RVA: 0x000027FC File Offset: 0x000009FC
	public static void ReloadSpectrums()
	{
	}

	// Token: 0x06009DD6 RID: 40406 RVA: 0x000027FC File Offset: 0x000009FC
	public void Start()
	{
	}

	// Token: 0x06009DD7 RID: 40407 RVA: 0x000027FC File Offset: 0x000009FC
	public void PushTransitions()
	{
	}

	// Token: 0x06009DD8 RID: 40408 RVA: 0x003BA940 File Offset: 0x003B8B40
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

	// Token: 0x06009DD9 RID: 40409 RVA: 0x003BAA48 File Offset: 0x003B8C48
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

	// Token: 0x06009DDA RID: 40410 RVA: 0x003BAAA8 File Offset: 0x003B8CA8
	public void ForceWeather(string _weatherName, float _duration)
	{
		this.CustomWeatherName = _weatherName;
		this.CustomWeatherTime = _duration;
		this.GeneralReset();
		this.SetAllWeather(_weatherName);
	}

	// Token: 0x06009DDB RID: 40411 RVA: 0x003BAAC8 File Offset: 0x003B8CC8
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

	// Token: 0x06009DDC RID: 40412 RVA: 0x003BAB14 File Offset: 0x003B8D14
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

	// Token: 0x06009DDD RID: 40413 RVA: 0x003BAB6C File Offset: 0x003B8D6C
	public void TriggerUpdate()
	{
		this.weatherAllName = null;
		this.weatherLastUpdateWorldTime = 0f;
	}

	// Token: 0x06009DDE RID: 40414 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void GeneralReset()
	{
	}

	// Token: 0x06009DDF RID: 40415 RVA: 0x003BAB80 File Offset: 0x003B8D80
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

	// Token: 0x06009DE0 RID: 40416 RVA: 0x003BABF7 File Offset: 0x003B8DF7
	public void TriggerThunder(int _playWorldTime, Vector3 _pos)
	{
		EnvironmentAudioManager.Instance.TriggerThunder(_pos);
	}

	// Token: 0x06009DE1 RID: 40417 RVA: 0x003BAC04 File Offset: 0x003B8E04
	public void SendPackages()
	{
		this.CalcPackages();
		NetPackageWeather package = NetPackageManager.GetPackage<NetPackageWeather>();
		package.Setup(this.weatherPackages);
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(package, true, -1, -1, -1, null, 192, false);
	}

	// Token: 0x06009DE2 RID: 40418 RVA: 0x003BAC48 File Offset: 0x003B8E48
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

	// Token: 0x06009DE3 RID: 40419 RVA: 0x003BACEC File Offset: 0x003B8EEC
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

	// Token: 0x06009DE4 RID: 40420 RVA: 0x003BAD4A File Offset: 0x003B8F4A
	[Conditional("DEBUG_WEATHERNET")]
	public void LogNet(string _format = "", params object[] _args)
	{
		_format = string.Format("{0} WeatherManager net {1}", GameManager.frameCount, _format);
		Log.Warning(_format, _args);
	}

	// Token: 0x0400778A RID: 30602
	public const ushort cVersion = 4;

	// Token: 0x0400778B RID: 30603
	public static WeatherManager Instance;

	// Token: 0x0400778C RID: 30604
	public const float cStormWarningDuration = 60f;

	// Token: 0x0400778D RID: 30605
	public const int BaseTemperature = 70;

	// Token: 0x0400778E RID: 30606
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cWeatherTransitionSeconds = 10f;

	// Token: 0x0400778F RID: 30607
	public List<WeatherManager.BiomeWeather> biomeWeather;

	// Token: 0x04007790 RID: 30608
	public static float forceClouds = -1f;

	// Token: 0x04007791 RID: 30609
	public static float forceRain = -1f;

	// Token: 0x04007792 RID: 30610
	public static float forceSnowfall = -1f;

	// Token: 0x04007793 RID: 30611
	public const float cForceTempDefault = -100f;

	// Token: 0x04007794 RID: 30612
	public static float forceTemperature = -100f;

	// Token: 0x04007795 RID: 30613
	public static float forceWind = -1f;

	// Token: 0x04007796 RID: 30614
	public static bool needToReUpdateWeatherSpectrums;

	// Token: 0x04007797 RID: 30615
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float forceSimRandom = -1f;

	// Token: 0x04007798 RID: 30616
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int cGracePeriodWorldTime = 22000;

	// Token: 0x04007799 RID: 30617
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int worldTime;

	// Token: 0x0400779A RID: 30618
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public World world;

	// Token: 0x0400779B RID: 30619
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static GameRandom gameRand;

	// Token: 0x0400779C RID: 30620
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isGameModeNormal;

	// Token: 0x0400779D RID: 30621
	public static bool inWeatherGracePeriod = true;

	// Token: 0x0400779E RID: 30622
	public static WeatherManager.BiomeWeather currentWeather;

	// Token: 0x0400779F RID: 30623
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public WeatherPackage[] weatherPackages;

	// Token: 0x040077A0 RID: 30624
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cLightningDelayMin = 30f;

	// Token: 0x040077A1 RID: 30625
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cLightningDelayMax = 60f;

	// Token: 0x040077A2 RID: 30626
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float thunderDelay = 30f;

	// Token: 0x040077A3 RID: 30627
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float stormFog;

	// Token: 0x040077A4 RID: 30628
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int frameCount = -1;

	// Token: 0x040077A5 RID: 30629
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 playerPosition;

	// Token: 0x040077A6 RID: 30630
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float checkPlayerMoveTime;

	// Token: 0x040077A7 RID: 30631
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static MemoryStream loadData;

	// Token: 0x040077A8 RID: 30632
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string weatherAllName;

	// Token: 0x040077A9 RID: 30633
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float weatherLastUpdateWorldTime;

	// Token: 0x040077AA RID: 30634
	public string CustomWeatherName = "";

	// Token: 0x040077AB RID: 30635
	public float CustomWeatherTime = -1f;

	// Token: 0x02001398 RID: 5016
	[Serializable]
	public class Param
	{
		// Token: 0x06009DE7 RID: 40423 RVA: 0x003BADEB File Offset: 0x003B8FEB
		public Param(float _value, float _step1Time = 0.25f)
		{
			this.name = "Param";
			this.value = _value;
			this.target = _value;
			this.step1Time = _step1Time;
		}

		// Token: 0x06009DE8 RID: 40424 RVA: 0x003BAE14 File Offset: 0x003B9014
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

		// Token: 0x06009DE9 RID: 40425 RVA: 0x003BAEDB File Offset: 0x003B90DB
		public void Clamp()
		{
			this.value = Utils.FastClamp01(this.value);
			this.target = Utils.FastClamp01(this.target);
		}

		// Token: 0x06009DEA RID: 40426 RVA: 0x003BAEFF File Offset: 0x003B90FF
		public void Set(float _value)
		{
			this.value = _value;
			this.target = _value;
		}

		// Token: 0x06009DEB RID: 40427 RVA: 0x003BAF0F File Offset: 0x003B910F
		public void SetTarget(float _target)
		{
			if (this.target == _target)
			{
				return;
			}
			this.target = _target;
			this.lastTime = Time.time;
		}

		// Token: 0x040077AC RID: 30636
		public string name;

		// Token: 0x040077AD RID: 30637
		public float value;

		// Token: 0x040077AE RID: 30638
		public float target;

		// Token: 0x040077AF RID: 30639
		public float step1Time;

		// Token: 0x040077B0 RID: 30640
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public float lastTime;
	}

	// Token: 0x02001399 RID: 5017
	[Serializable]
	public class BiomeWeather
	{
		// Token: 0x06009DEC RID: 40428 RVA: 0x003BAF30 File Offset: 0x003B9130
		public BiomeWeather(BiomeDefinition _definition)
		{
			this.biomeDefinition = _definition;
			for (int i = 0; i < 5; i++)
			{
				this.parameters[i] = new WeatherManager.Param(0f, 0.3f);
			}
			this.parameters[0].step1Time = 0.15f;
		}

		// Token: 0x06009DED RID: 40429 RVA: 0x003BAFC1 File Offset: 0x003B91C1
		public float CloudThickness()
		{
			return this.parameters[2].value;
		}

		// Token: 0x06009DEE RID: 40430 RVA: 0x003BAFD0 File Offset: 0x003B91D0
		public float FogPercent()
		{
			return this.parameters[4].value * 0.01f + WeatherManager.stormFog;
		}

		// Token: 0x06009DEF RID: 40431 RVA: 0x003BAFEB File Offset: 0x003B91EB
		public float Wind()
		{
			return this.parameters[3].value;
		}

		// Token: 0x06009DF0 RID: 40432 RVA: 0x003BAFFC File Offset: 0x003B91FC
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

		// Token: 0x06009DF1 RID: 40433 RVA: 0x003BB15C File Offset: 0x003B935C
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

		// Token: 0x06009DF2 RID: 40434 RVA: 0x003BB1B4 File Offset: 0x003B93B4
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

		// Token: 0x06009DF3 RID: 40435 RVA: 0x003BB208 File Offset: 0x003B9408
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

		// Token: 0x06009DF4 RID: 40436 RVA: 0x003BB278 File Offset: 0x003B9478
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

		// Token: 0x06009DF5 RID: 40437 RVA: 0x003BB414 File Offset: 0x003B9614
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

		// Token: 0x06009DF6 RID: 40438 RVA: 0x003BB474 File Offset: 0x003B9674
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

		// Token: 0x06009DF7 RID: 40439 RVA: 0x003BB4C4 File Offset: 0x003B96C4
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

		// Token: 0x040077B1 RID: 30641
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public const float cStep1Time = 0.15f;

		// Token: 0x040077B2 RID: 30642
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public const float cPrecipitationPercentVisibleMin = 0.3f;

		// Token: 0x040077B3 RID: 30643
		public BiomeDefinition biomeDefinition;

		// Token: 0x040077B4 RID: 30644
		public int stormWorldTime;

		// Token: 0x040077B5 RID: 30645
		public int stormDuration;

		// Token: 0x040077B6 RID: 30646
		public int stormState;

		// Token: 0x040077B7 RID: 30647
		public int stormIndex;

		// Token: 0x040077B8 RID: 30648
		public int nextRandWorldTime;

		// Token: 0x040077B9 RID: 30649
		public byte remainingSeconds;

		// Token: 0x040077BA RID: 30650
		public WeatherManager.Param[] parameters = new WeatherManager.Param[5];

		// Token: 0x040077BB RID: 30651
		public float[] parameterFinals = new float[5];

		// Token: 0x040077BC RID: 30652
		public WeatherManager.Param rainParam = new WeatherManager.Param(0f, 0.25f);

		// Token: 0x040077BD RID: 30653
		public WeatherManager.Param snowFallParam = new WeatherManager.Param(0f, 0.25f);
	}
}

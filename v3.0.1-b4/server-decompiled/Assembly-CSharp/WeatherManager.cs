using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

// Token: 0x0200135D RID: 4957
public class WeatherManager : MonoBehaviour
{
	// Token: 0x06009C13 RID: 39955 RVA: 0x003B043B File Offset: 0x003AE63B
	public static void Init(World _world, GameObject _obj)
	{
		WeatherManager.Cleanup();
		WeatherManager.Instance = _obj.GetComponent<WeatherManager>();
		WeatherManager.Instance.Init(_world);
	}

	// Token: 0x06009C14 RID: 39956 RVA: 0x003B0458 File Offset: 0x003AE658
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

	// Token: 0x06009C15 RID: 39957 RVA: 0x003B04B0 File Offset: 0x003AE6B0
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

	// Token: 0x06009C16 RID: 39958 RVA: 0x003B05A0 File Offset: 0x003AE7A0
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

	// Token: 0x06009C17 RID: 39959 RVA: 0x003B05FA File Offset: 0x003AE7FA
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

	// Token: 0x06009C18 RID: 39960 RVA: 0x003B0624 File Offset: 0x003AE824
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

	// Token: 0x06009C19 RID: 39961 RVA: 0x003B0672 File Offset: 0x003AE872
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

	// Token: 0x06009C1A RID: 39962 RVA: 0x003B06B0 File Offset: 0x003AE8B0
	public void ApplyLoad()
	{
		if (WeatherManager.loadData != null)
		{
			using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
			{
				pooledBinaryReader.SetBaseStream(WeatherManager.loadData);
				this.ReadWriteData(pooledBinaryReader, true);
			}
			WeatherManager.loadData = null;
		}
	}

	// Token: 0x06009C1B RID: 39963 RVA: 0x003B0708 File Offset: 0x003AE908
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

	// Token: 0x06009C1C RID: 39964 RVA: 0x003B0748 File Offset: 0x003AE948
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

	// Token: 0x06009C1D RID: 39965 RVA: 0x003B0944 File Offset: 0x003AEB44
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

	// Token: 0x06009C1E RID: 39966 RVA: 0x003B0988 File Offset: 0x003AEB88
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

	// Token: 0x06009C1F RID: 39967 RVA: 0x003B09CC File Offset: 0x003AEBCC
	public bool IsStorming(BiomeDefinition.BiomeType _type)
	{
		WeatherManager.BiomeWeather biomeWeather = WeatherManager.Instance.FindBiomeWeather(_type);
		return biomeWeather != null && biomeWeather.stormState >= 2;
	}

	// Token: 0x06009C20 RID: 39968 RVA: 0x000027FC File Offset: 0x000009FC
	public static void ClearTemperatureOffSetHeights()
	{
	}

	// Token: 0x06009C21 RID: 39969 RVA: 0x000027FC File Offset: 0x000009FC
	public static void AddTemperatureOffSetHeight(float height, float degreesOffset)
	{
	}

	// Token: 0x06009C22 RID: 39970 RVA: 0x0003D066 File Offset: 0x0003B266
	public static float SeaLevel()
	{
		return 0f;
	}

	// Token: 0x06009C23 RID: 39971 RVA: 0x003B09F6 File Offset: 0x003AEBF6
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

	// Token: 0x06009C24 RID: 39972 RVA: 0x0003D066 File Offset: 0x0003B266
	public static float GetTemperature()
	{
		return 0f;
	}

	// Token: 0x06009C25 RID: 39973 RVA: 0x0003D066 File Offset: 0x0003B266
	public static float GetWindSpeed()
	{
		return 0f;
	}

	// Token: 0x06009C26 RID: 39974 RVA: 0x000027FC File Offset: 0x000009FC
	public static void EntityAddedToWorld(Entity entity)
	{
	}

	// Token: 0x06009C27 RID: 39975 RVA: 0x000027FC File Offset: 0x000009FC
	public static void EntityRemovedFromWorld(Entity entity)
	{
	}

	// Token: 0x06009C28 RID: 39976 RVA: 0x003B0A2E File Offset: 0x003AEC2E
	public float GetCurrentSnowfallPercent()
	{
		if (WeatherManager.forceSnowfall < 0f && WeatherManager.currentWeather != null)
		{
			return WeatherManager.currentWeather.snowFallParam.value;
		}
		return WeatherManager.forceSnowfall;
	}

	// Token: 0x06009C29 RID: 39977 RVA: 0x003B0A58 File Offset: 0x003AEC58
	public float GetCurrentRainfallPercent()
	{
		if (WeatherManager.forceRain < 0f && WeatherManager.currentWeather != null)
		{
			return WeatherManager.currentWeather.rainParam.value;
		}
		return WeatherManager.forceRain;
	}

	// Token: 0x06009C2A RID: 39978 RVA: 0x003B0A84 File Offset: 0x003AEC84
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

	// Token: 0x06009C2B RID: 39979 RVA: 0x003B0AD7 File Offset: 0x003AECD7
	public float GetCurrentCloudThicknessPercent()
	{
		return WeatherManager.GetCloudThickness() * 0.01f;
	}

	// Token: 0x06009C2C RID: 39980 RVA: 0x003B0AE4 File Offset: 0x003AECE4
	public float GetCurrentTemperatureValue()
	{
		return WeatherManager.GetTemperature();
	}

	// Token: 0x06009C2D RID: 39981 RVA: 0x003B0AEB File Offset: 0x003AECEB
	public static void SetSimRandom(float _random)
	{
		WeatherManager.forceSimRandom = _random;
		if (WeatherManager.Instance)
		{
			WeatherManager.Instance.weatherLastUpdateWorldTime = 0f;
		}
	}

	// Token: 0x06009C2E RID: 39982 RVA: 0x000027FC File Offset: 0x000009FC
	public static void ReloadSpectrums()
	{
	}

	// Token: 0x06009C2F RID: 39983 RVA: 0x000027FC File Offset: 0x000009FC
	public void Start()
	{
	}

	// Token: 0x06009C30 RID: 39984 RVA: 0x000027FC File Offset: 0x000009FC
	public void PushTransitions()
	{
	}

	// Token: 0x06009C31 RID: 39985 RVA: 0x003B0B10 File Offset: 0x003AED10
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

	// Token: 0x06009C32 RID: 39986 RVA: 0x003B0C18 File Offset: 0x003AEE18
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

	// Token: 0x06009C33 RID: 39987 RVA: 0x003B0C78 File Offset: 0x003AEE78
	public void ForceWeather(string _weatherName, float _duration)
	{
		this.CustomWeatherName = _weatherName;
		this.CustomWeatherTime = _duration;
		this.GeneralReset();
		this.SetAllWeather(_weatherName);
	}

	// Token: 0x06009C34 RID: 39988 RVA: 0x003B0C98 File Offset: 0x003AEE98
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

	// Token: 0x06009C35 RID: 39989 RVA: 0x003B0CE4 File Offset: 0x003AEEE4
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

	// Token: 0x06009C36 RID: 39990 RVA: 0x003B0D3C File Offset: 0x003AEF3C
	public void TriggerUpdate()
	{
		this.weatherAllName = null;
		this.weatherLastUpdateWorldTime = 0f;
	}

	// Token: 0x06009C37 RID: 39991 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void GeneralReset()
	{
	}

	// Token: 0x06009C38 RID: 39992 RVA: 0x003B0D50 File Offset: 0x003AEF50
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

	// Token: 0x06009C39 RID: 39993 RVA: 0x003B0DC7 File Offset: 0x003AEFC7
	public void TriggerThunder(int _playWorldTime, Vector3 _pos)
	{
		EnvironmentAudioManager.Instance.TriggerThunder(_pos);
	}

	// Token: 0x06009C3A RID: 39994 RVA: 0x003B0DD4 File Offset: 0x003AEFD4
	public void SendPackages()
	{
		this.CalcPackages();
		NetPackageWeather package = NetPackageManager.GetPackage<NetPackageWeather>();
		package.Setup(this.weatherPackages);
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(package, true, -1, -1, -1, null, 192, false);
	}

	// Token: 0x06009C3B RID: 39995 RVA: 0x003B0E18 File Offset: 0x003AF018
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

	// Token: 0x06009C3C RID: 39996 RVA: 0x003B0EBC File Offset: 0x003AF0BC
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

	// Token: 0x06009C3D RID: 39997 RVA: 0x003B0F1A File Offset: 0x003AF11A
	[Conditional("DEBUG_WEATHERNET")]
	public void LogNet(string _format = "", params object[] _args)
	{
		_format = string.Format("{0} WeatherManager net {1}", GameManager.frameCount, _format);
		Log.Warning(_format, _args);
	}

	// Token: 0x04007637 RID: 30263
	public const ushort cVersion = 4;

	// Token: 0x04007638 RID: 30264
	public static WeatherManager Instance;

	// Token: 0x04007639 RID: 30265
	public const float cStormWarningDuration = 60f;

	// Token: 0x0400763A RID: 30266
	public const int BaseTemperature = 70;

	// Token: 0x0400763B RID: 30267
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cWeatherTransitionSeconds = 10f;

	// Token: 0x0400763C RID: 30268
	public List<WeatherManager.BiomeWeather> biomeWeather;

	// Token: 0x0400763D RID: 30269
	public static float forceClouds = -1f;

	// Token: 0x0400763E RID: 30270
	public static float forceRain = -1f;

	// Token: 0x0400763F RID: 30271
	public static float forceSnowfall = -1f;

	// Token: 0x04007640 RID: 30272
	public const float cForceTempDefault = -100f;

	// Token: 0x04007641 RID: 30273
	public static float forceTemperature = -100f;

	// Token: 0x04007642 RID: 30274
	public static float forceWind = -1f;

	// Token: 0x04007643 RID: 30275
	public static bool needToReUpdateWeatherSpectrums;

	// Token: 0x04007644 RID: 30276
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float forceSimRandom = -1f;

	// Token: 0x04007645 RID: 30277
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int cGracePeriodWorldTime = 22000;

	// Token: 0x04007646 RID: 30278
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int worldTime;

	// Token: 0x04007647 RID: 30279
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public World world;

	// Token: 0x04007648 RID: 30280
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static GameRandom gameRand;

	// Token: 0x04007649 RID: 30281
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isGameModeNormal;

	// Token: 0x0400764A RID: 30282
	public static bool inWeatherGracePeriod = true;

	// Token: 0x0400764B RID: 30283
	public static WeatherManager.BiomeWeather currentWeather;

	// Token: 0x0400764C RID: 30284
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public WeatherPackage[] weatherPackages;

	// Token: 0x0400764D RID: 30285
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cLightningDelayMin = 30f;

	// Token: 0x0400764E RID: 30286
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cLightningDelayMax = 60f;

	// Token: 0x0400764F RID: 30287
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float thunderDelay = 30f;

	// Token: 0x04007650 RID: 30288
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float stormFog;

	// Token: 0x04007651 RID: 30289
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int frameCount = -1;

	// Token: 0x04007652 RID: 30290
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 playerPosition;

	// Token: 0x04007653 RID: 30291
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float checkPlayerMoveTime;

	// Token: 0x04007654 RID: 30292
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static MemoryStream loadData;

	// Token: 0x04007655 RID: 30293
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string weatherAllName;

	// Token: 0x04007656 RID: 30294
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float weatherLastUpdateWorldTime;

	// Token: 0x04007657 RID: 30295
	public string CustomWeatherName = "";

	// Token: 0x04007658 RID: 30296
	public float CustomWeatherTime = -1f;

	// Token: 0x0200135E RID: 4958
	[Serializable]
	public class Param
	{
		// Token: 0x06009C40 RID: 40000 RVA: 0x003B0FBB File Offset: 0x003AF1BB
		public Param(float _value, float _step1Time = 0.25f)
		{
			this.name = "Param";
			this.value = _value;
			this.target = _value;
			this.step1Time = _step1Time;
		}

		// Token: 0x06009C41 RID: 40001 RVA: 0x003B0FE4 File Offset: 0x003AF1E4
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

		// Token: 0x06009C42 RID: 40002 RVA: 0x003B10AB File Offset: 0x003AF2AB
		public void Clamp()
		{
			this.value = Utils.FastClamp01(this.value);
			this.target = Utils.FastClamp01(this.target);
		}

		// Token: 0x06009C43 RID: 40003 RVA: 0x003B10CF File Offset: 0x003AF2CF
		public void Set(float _value)
		{
			this.value = _value;
			this.target = _value;
		}

		// Token: 0x06009C44 RID: 40004 RVA: 0x003B10DF File Offset: 0x003AF2DF
		public void SetTarget(float _target)
		{
			if (this.target == _target)
			{
				return;
			}
			this.target = _target;
			this.lastTime = Time.time;
		}

		// Token: 0x04007659 RID: 30297
		public string name;

		// Token: 0x0400765A RID: 30298
		public float value;

		// Token: 0x0400765B RID: 30299
		public float target;

		// Token: 0x0400765C RID: 30300
		public float step1Time;

		// Token: 0x0400765D RID: 30301
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public float lastTime;
	}

	// Token: 0x0200135F RID: 4959
	[Serializable]
	public class BiomeWeather
	{
		// Token: 0x06009C45 RID: 40005 RVA: 0x003B1100 File Offset: 0x003AF300
		public BiomeWeather(BiomeDefinition _definition)
		{
			this.biomeDefinition = _definition;
			for (int i = 0; i < 5; i++)
			{
				this.parameters[i] = new WeatherManager.Param(0f, 0.3f);
			}
			this.parameters[0].step1Time = 0.15f;
		}

		// Token: 0x06009C46 RID: 40006 RVA: 0x003B1191 File Offset: 0x003AF391
		public float CloudThickness()
		{
			return this.parameters[2].value;
		}

		// Token: 0x06009C47 RID: 40007 RVA: 0x003B11A0 File Offset: 0x003AF3A0
		public float FogPercent()
		{
			return this.parameters[4].value * 0.01f + WeatherManager.stormFog;
		}

		// Token: 0x06009C48 RID: 40008 RVA: 0x003B11BB File Offset: 0x003AF3BB
		public float Wind()
		{
			return this.parameters[3].value;
		}

		// Token: 0x06009C49 RID: 40009 RVA: 0x003B11CC File Offset: 0x003AF3CC
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

		// Token: 0x06009C4A RID: 40010 RVA: 0x003B132C File Offset: 0x003AF52C
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

		// Token: 0x06009C4B RID: 40011 RVA: 0x003B1384 File Offset: 0x003AF584
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

		// Token: 0x06009C4C RID: 40012 RVA: 0x003B13D8 File Offset: 0x003AF5D8
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

		// Token: 0x06009C4D RID: 40013 RVA: 0x003B1448 File Offset: 0x003AF648
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

		// Token: 0x06009C4E RID: 40014 RVA: 0x003B15E4 File Offset: 0x003AF7E4
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

		// Token: 0x06009C4F RID: 40015 RVA: 0x003B1644 File Offset: 0x003AF844
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

		// Token: 0x06009C50 RID: 40016 RVA: 0x003B1694 File Offset: 0x003AF894
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

		// Token: 0x0400765E RID: 30302
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public const float cStep1Time = 0.15f;

		// Token: 0x0400765F RID: 30303
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public const float cPrecipitationPercentVisibleMin = 0.3f;

		// Token: 0x04007660 RID: 30304
		public BiomeDefinition biomeDefinition;

		// Token: 0x04007661 RID: 30305
		public int stormWorldTime;

		// Token: 0x04007662 RID: 30306
		public int stormDuration;

		// Token: 0x04007663 RID: 30307
		public int stormState;

		// Token: 0x04007664 RID: 30308
		public int stormIndex;

		// Token: 0x04007665 RID: 30309
		public int nextRandWorldTime;

		// Token: 0x04007666 RID: 30310
		public byte remainingSeconds;

		// Token: 0x04007667 RID: 30311
		public WeatherManager.Param[] parameters = new WeatherManager.Param[5];

		// Token: 0x04007668 RID: 30312
		public float[] parameterFinals = new float[5];

		// Token: 0x04007669 RID: 30313
		public WeatherManager.Param rainParam = new WeatherManager.Param(0f, 0.25f);

		// Token: 0x0400766A RID: 30314
		public WeatherManager.Param snowFallParam = new WeatherManager.Param(0f, 0.25f);
	}
}

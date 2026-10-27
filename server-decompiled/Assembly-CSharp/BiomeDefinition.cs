using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000C14 RID: 3092
public class BiomeDefinition
{
	// Token: 0x06005DC6 RID: 24006 RVA: 0x00249996 File Offset: 0x00247B96
	public static string LocalizedBiomeName(BiomeDefinition.BiomeType _biomeType)
	{
		return Localization.Get("biome_" + _biomeType.ToStringCached<BiomeDefinition.BiomeType>(), false, null);
	}

	// Token: 0x06005DC7 RID: 24007 RVA: 0x002499B0 File Offset: 0x00247BB0
	public BiomeDefinition(byte _id, byte _subId, string _name, uint _color, int _radiationLevel, string _buff)
	{
		this.m_Id = _id;
		this.m_BiomeType = (BiomeDefinition.BiomeType)(Enum.IsDefined(typeof(BiomeDefinition.BiomeType), (int)this.m_Id) ? this.m_Id : 0);
		this.subId = _subId;
		this.m_sBiomeName = _name;
		this.LocalizedName = Localization.Get("biome_" + _name, false, null);
		this.m_SpectrumName = this.m_sBiomeName;
		this.m_uiColor = _color;
		this.m_RadiationLevel = _radiationLevel;
		this.m_Layers = new List<BiomeLayer>();
		this.m_DecoBlocks = new List<BiomeBlockDecoration>();
		this.m_DistantDecoBlocks = new List<BiomeBlockDecoration>();
		this.m_DecoPrefabs = new List<BiomePrefabDecoration>();
		this.m_DecoBluffs = new List<BiomeBluffDecoration>();
		this.Buff = _buff;
		this.InitWeather();
	}

	// Token: 0x06005DC8 RID: 24008 RVA: 0x00249ADF File Offset: 0x00247CDF
	public void AddLayer(BiomeLayer _layer)
	{
		this.m_Layers.Add(_layer);
		this.TotalLayerDepth += _layer.m_Depth;
	}

	// Token: 0x06005DC9 RID: 24009 RVA: 0x00249B00 File Offset: 0x00247D00
	public void AddDecoBlock(BiomeBlockDecoration _deco)
	{
		this.m_DecoBlocks.Add(_deco);
		if (Block.BlocksLoaded)
		{
			Block block = _deco.blockValues[0].Block;
			if (block != null && block.IsDistantDecoration)
			{
				this.m_DistantDecoBlocks.Add(_deco);
			}
		}
	}

	// Token: 0x06005DCA RID: 24010 RVA: 0x00249B49 File Offset: 0x00247D49
	public void AddDecoPrefab(BiomePrefabDecoration _deco)
	{
		this.m_DecoPrefabs.Add(_deco);
	}

	// Token: 0x06005DCB RID: 24011 RVA: 0x00249B57 File Offset: 0x00247D57
	public void AddBluff(BiomeBluffDecoration _deco)
	{
		this.m_DecoBluffs.Add(_deco);
	}

	// Token: 0x06005DCC RID: 24012 RVA: 0x00249B65 File Offset: 0x00247D65
	public void AddReplacement(int _sourceId, int _targetId)
	{
		this.Replacements[_sourceId] = _targetId;
	}

	// Token: 0x06005DCD RID: 24013 RVA: 0x00249B74 File Offset: 0x00247D74
	public void addSubBiome(BiomeDefinition _subbiome)
	{
		this.subbiomes.Add(_subbiome);
	}

	// Token: 0x06005DCE RID: 24014 RVA: 0x00249B82 File Offset: 0x00247D82
	public override bool Equals(object obj)
	{
		return obj is BiomeDefinition && ((BiomeDefinition)obj).m_Id == this.m_Id;
	}

	// Token: 0x06005DCF RID: 24015 RVA: 0x00249BA1 File Offset: 0x00247DA1
	public override int GetHashCode()
	{
		return (int)this.m_Id;
	}

	// Token: 0x06005DD0 RID: 24016 RVA: 0x00249BA9 File Offset: 0x00247DA9
	public override string ToString()
	{
		return this.m_sBiomeName;
	}

	// Token: 0x06005DD1 RID: 24017 RVA: 0x00249BB1 File Offset: 0x00247DB1
	public static uint GetBiomeColor(BiomeDefinition.BiomeType _type)
	{
		return BiomeDefinition.BiomeColors[(int)_type];
	}

	// Token: 0x06005DD2 RID: 24018 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitWeather()
	{
	}

	// Token: 0x06005DD3 RID: 24019 RVA: 0x00249BBC File Offset: 0x00247DBC
	public BiomeDefinition.WeatherGroup AddWeatherGroup(string _name, float _prob, float _duration, Vector2 _delay, string _buffName)
	{
		BiomeDefinition.WeatherGroup weatherGroup = new BiomeDefinition.WeatherGroup();
		weatherGroup.name = _name;
		weatherGroup.stormLevel = ((!_name.StartsWith("storm")) ? 0 : (_name.Contains("build") ? 1 : 2));
		weatherGroup.prob = _prob;
		weatherGroup.duration = (int)(_duration * 1000f);
		weatherGroup.delay.x = (int)(_delay.x * 1000f);
		weatherGroup.delay.y = (int)(_delay.y * 1000f);
		if (!string.IsNullOrEmpty(_buffName))
		{
			weatherGroup.buffName = _buffName;
		}
		this.weatherGroups.Add(weatherGroup);
		return weatherGroup;
	}

	// Token: 0x06005DD4 RID: 24020 RVA: 0x00249C64 File Offset: 0x00247E64
	public void SetupWeather()
	{
		float num = 0f;
		for (int i = 0; i < this.weatherGroups.Count; i++)
		{
			BiomeDefinition.WeatherGroup weatherGroup = this.weatherGroups[i];
			num += weatherGroup.prob;
			weatherGroup.probabilities.Normalize();
		}
		num += 1E-06f;
		for (int j = 0; j < this.weatherGroups.Count; j++)
		{
			this.weatherGroups[j].prob /= num;
		}
	}

	// Token: 0x06005DD5 RID: 24021 RVA: 0x00249CE5 File Offset: 0x00247EE5
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float WeatherGetValue(BiomeDefinition.Probabilities.ProbType _type)
	{
		return this.weatherValues[(int)_type];
	}

	// Token: 0x06005DD6 RID: 24022 RVA: 0x00249CEF File Offset: 0x00247EEF
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void WeatherSetValue(BiomeDefinition.Probabilities.ProbType _type, float _value)
	{
		this.weatherValues[(int)_type] = _value;
	}

	// Token: 0x06005DD7 RID: 24023 RVA: 0x00249CFC File Offset: 0x00247EFC
	public void WeatherRandomize(float _rand)
	{
		float num = 0f;
		for (int i = 0; i < this.weatherGroups.Count; i++)
		{
			BiomeDefinition.WeatherGroup weatherGroup = this.weatherGroups[i];
			num += weatherGroup.prob;
			if (_rand < num)
			{
				this.SelectWeatherGroup(i);
				return;
			}
		}
	}

	// Token: 0x06005DD8 RID: 24024 RVA: 0x00249D48 File Offset: 0x00247F48
	public bool WeatherRandomize(string _weatherGroup)
	{
		int num = this.FindWeatherGroupIndex(_weatherGroup);
		if (num >= 0)
		{
			this.SelectWeatherGroup(num);
			return true;
		}
		return false;
	}

	// Token: 0x06005DD9 RID: 24025 RVA: 0x00249D6C File Offset: 0x00247F6C
	public int WeatherGetDuration(string _weatherGroup)
	{
		BiomeDefinition.WeatherGroup weatherGroup = this.FindWeatherGroup(_weatherGroup);
		if (weatherGroup != null)
		{
			return weatherGroup.duration;
		}
		return 0;
	}

	// Token: 0x06005DDA RID: 24026 RVA: 0x00249D8C File Offset: 0x00247F8C
	public int WeatherGetDuration(string _weatherGroup, out Vector2i _delay)
	{
		BiomeDefinition.WeatherGroup weatherGroup = this.FindWeatherGroup(_weatherGroup);
		if (weatherGroup != null)
		{
			_delay = weatherGroup.delay;
			return weatherGroup.duration;
		}
		_delay = Vector2i.zero;
		return 0;
	}

	// Token: 0x06005DDB RID: 24027 RVA: 0x00249DC4 File Offset: 0x00247FC4
	[PublicizedFrom(EAccessModifier.Private)]
	public BiomeDefinition.WeatherGroup FindWeatherGroup(string _name)
	{
		for (int i = 0; i < this.weatherGroups.Count; i++)
		{
			BiomeDefinition.WeatherGroup weatherGroup = this.weatherGroups[i];
			if (weatherGroup.name == _name)
			{
				return weatherGroup;
			}
		}
		return null;
	}

	// Token: 0x06005DDC RID: 24028 RVA: 0x00249E08 File Offset: 0x00248008
	[PublicizedFrom(EAccessModifier.Private)]
	public int FindWeatherGroupIndex(string _name)
	{
		for (int i = 0; i < this.weatherGroups.Count; i++)
		{
			if (this.weatherGroups[i].name == _name)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06005DDD RID: 24029 RVA: 0x00249E48 File Offset: 0x00248048
	[PublicizedFrom(EAccessModifier.Private)]
	public void SelectWeatherGroup(int _index)
	{
		BiomeDefinition.WeatherGroup weatherGroup = this.weatherGroups[_index];
		this.weatherName = weatherGroup.name;
		this.weatherSpectrum = weatherGroup.spectrum;
		this.currentWeatherGroupIndex = _index;
		this.currentWeatherGroup = weatherGroup;
		for (int i = 0; i < 5; i++)
		{
			float randomValue = weatherGroup.probabilities.GetRandomValue((BiomeDefinition.Probabilities.ProbType)i);
			this.weatherValues[i] = randomValue;
		}
	}

	// Token: 0x06005DDE RID: 24030 RVA: 0x00249EAC File Offset: 0x002480AC
	public void SetWeatherGroup(int _index)
	{
		this.currentWeatherGroupIndex = _index;
		BiomeDefinition.WeatherGroup weatherGroup = this.weatherGroups[_index];
		this.currentWeatherGroup = weatherGroup;
		this.weatherSpectrum = weatherGroup.spectrum;
	}

	// Token: 0x04004892 RID: 18578
	public float currentPlayerIntensity;

	// Token: 0x04004893 RID: 18579
	public const string BiomeNameLocalizationPrefix = "biome_";

	// Token: 0x04004894 RID: 18580
	public static string[] BiomeNames = new string[]
	{
		"any", "snow", "forest", "pine_forest", "plains", "desert", "water", "radiated", "wasteland", "burnt_forest",
		"city", "city_wasteland", "wasteland_hub", "caveFloor", "caveCeiling"
	};

	// Token: 0x04004895 RID: 18581
	public static uint[] BiomeColors = new uint[]
	{
		0U, 16777215U, 0U, 16384U, 0U, 16770167U, 25599U, 0U, 16754688U, 12189951U,
		8421504U, 12632256U, 10526880U, 0U, 0U
	};

	// Token: 0x04004896 RID: 18582
	public readonly byte m_Id;

	// Token: 0x04004897 RID: 18583
	public readonly BiomeDefinition.BiomeType m_BiomeType;

	// Token: 0x04004898 RID: 18584
	public byte subId;

	// Token: 0x04004899 RID: 18585
	public readonly string m_sBiomeName;

	// Token: 0x0400489A RID: 18586
	public readonly string LocalizedName;

	// Token: 0x0400489B RID: 18587
	public uint m_uiColor;

	// Token: 0x0400489C RID: 18588
	public string m_SpectrumName;

	// Token: 0x0400489D RID: 18589
	public static Dictionary<string, byte> nameToId;

	// Token: 0x0400489E RID: 18590
	public int m_RadiationLevel;

	// Token: 0x0400489F RID: 18591
	public int Difficulty = 1;

	// Token: 0x040048A0 RID: 18592
	public List<BiomeLayer> m_Layers;

	// Token: 0x040048A1 RID: 18593
	public List<BiomeBlockDecoration> m_DecoBlocks;

	// Token: 0x040048A2 RID: 18594
	public List<BiomeBlockDecoration> m_DistantDecoBlocks;

	// Token: 0x040048A3 RID: 18595
	public List<BiomePrefabDecoration> m_DecoPrefabs;

	// Token: 0x040048A4 RID: 18596
	public List<BiomeBluffDecoration> m_DecoBluffs;

	// Token: 0x040048A5 RID: 18597
	public List<BiomeDefinition.WeatherGroup> weatherGroups = new List<BiomeDefinition.WeatherGroup>();

	// Token: 0x040048A6 RID: 18598
	public string weatherName;

	// Token: 0x040048A7 RID: 18599
	public int currentWeatherGroupIndex;

	// Token: 0x040048A8 RID: 18600
	public BiomeDefinition.WeatherGroup currentWeatherGroup;

	// Token: 0x040048A9 RID: 18601
	public SpectrumWeatherType weatherSpectrum;

	// Token: 0x040048AA RID: 18602
	[PublicizedFrom(EAccessModifier.Private)]
	public float[] weatherValues = new float[5];

	// Token: 0x040048AB RID: 18603
	public int TotalLayerDepth;

	// Token: 0x040048AC RID: 18604
	public List<BiomeDefinition> subbiomes = new List<BiomeDefinition>();

	// Token: 0x040048AD RID: 18605
	public float noiseFreq = 0.03f;

	// Token: 0x040048AE RID: 18606
	public float noiseMin = 0.2f;

	// Token: 0x040048AF RID: 18607
	public float noiseMax = 1f;

	// Token: 0x040048B0 RID: 18608
	public Vector2 noiseOffset;

	// Token: 0x040048B1 RID: 18609
	public Dictionary<int, int> Replacements = new Dictionary<int, int>();

	// Token: 0x040048B2 RID: 18610
	public float GameStageMod;

	// Token: 0x040048B3 RID: 18611
	public float GameStageBonus;

	// Token: 0x040048B4 RID: 18612
	public float LootStageMod;

	// Token: 0x040048B5 RID: 18613
	public float LootStageBonus;

	// Token: 0x040048B6 RID: 18614
	public int LootStageMin = -1;

	// Token: 0x040048B7 RID: 18615
	public int LootStageMax = -1;

	// Token: 0x040048B8 RID: 18616
	public string Buff;

	// Token: 0x02000C15 RID: 3093
	public enum BiomeType
	{
		// Token: 0x040048BA RID: 18618
		Any,
		// Token: 0x040048BB RID: 18619
		Snow,
		// Token: 0x040048BC RID: 18620
		Forest,
		// Token: 0x040048BD RID: 18621
		PineForest,
		// Token: 0x040048BE RID: 18622
		Plains,
		// Token: 0x040048BF RID: 18623
		Desert,
		// Token: 0x040048C0 RID: 18624
		Water,
		// Token: 0x040048C1 RID: 18625
		Radiated,
		// Token: 0x040048C2 RID: 18626
		Wasteland,
		// Token: 0x040048C3 RID: 18627
		burnt_forest,
		// Token: 0x040048C4 RID: 18628
		city,
		// Token: 0x040048C5 RID: 18629
		city_wasteland,
		// Token: 0x040048C6 RID: 18630
		wasteland_hub,
		// Token: 0x040048C7 RID: 18631
		caveFloor,
		// Token: 0x040048C8 RID: 18632
		caveCeiling
	}

	// Token: 0x02000C16 RID: 3094
	public class Probabilities
	{
		// Token: 0x06005DE0 RID: 24032 RVA: 0x00249F90 File Offset: 0x00248190
		public Probabilities()
		{
			this.probabilities = new List<Vector3>[5];
			for (int i = 0; i < 5; i++)
			{
				this.probabilities[i] = new List<Vector3>();
			}
		}

		// Token: 0x06005DE1 RID: 24033 RVA: 0x00249FC8 File Offset: 0x002481C8
		public void AddProbability(BiomeDefinition.Probabilities.ProbType _type, Vector2 _range, float _probability)
		{
			this.probabilities[(int)_type].Add(new Vector3(_range.x, _range.y, _probability));
		}

		// Token: 0x06005DE2 RID: 24034 RVA: 0x00249FF8 File Offset: 0x002481F8
		public Vector2 CalcMinMaxPossibleValue(BiomeDefinition.Probabilities.ProbType type)
		{
			Vector2 vector = new Vector2(float.MaxValue, float.MinValue);
			List<Vector3> list = this.probabilities[(int)type];
			for (int i = 0; i < list.Count; i++)
			{
				Vector3 vector2 = list[i];
				if (vector2.x < vector.x)
				{
					vector.x = vector2.x;
				}
				if (vector2.y > vector.y)
				{
					vector.y = vector2.y;
				}
			}
			return vector;
		}

		// Token: 0x06005DE3 RID: 24035 RVA: 0x0024A070 File Offset: 0x00248270
		public float GetRandomValue(BiomeDefinition.Probabilities.ProbType _type)
		{
			GameRandom gameRandom = GameManager.Instance.World.GetGameRandom();
			float randomFloat = gameRandom.RandomFloat;
			float num = 0f;
			List<Vector3> list = this.probabilities[(int)_type];
			for (int i = 0; i < list.Count; i++)
			{
				Vector3 vector = list[i];
				num += vector.z;
				if (randomFloat < num)
				{
					float randomFloat2 = gameRandom.RandomFloat;
					return vector.x * randomFloat2 + vector.y * (1f - randomFloat2);
				}
			}
			return 0f;
		}

		// Token: 0x06005DE4 RID: 24036 RVA: 0x0024A0FC File Offset: 0x002482FC
		public void Normalize()
		{
			for (int i = 0; i < 5; i++)
			{
				List<Vector3> list = this.probabilities[i];
				float num = 0f;
				for (int j = 0; j < list.Count; j++)
				{
					num += list[j].z;
				}
				for (int k = 0; k < list.Count; k++)
				{
					Vector3 vector = list[k];
					vector.z /= num;
					list[k] = vector;
				}
			}
		}

		// Token: 0x040048C9 RID: 18633
		public const int ProbTypeCount = 5;

		// Token: 0x040048CA RID: 18634
		[PublicizedFrom(EAccessModifier.Private)]
		public List<Vector3>[] probabilities;

		// Token: 0x02000C17 RID: 3095
		public enum ProbType
		{
			// Token: 0x040048CC RID: 18636
			Temperature,
			// Token: 0x040048CD RID: 18637
			Precipitation,
			// Token: 0x040048CE RID: 18638
			CloudThickness,
			// Token: 0x040048CF RID: 18639
			Wind,
			// Token: 0x040048D0 RID: 18640
			Fog,
			// Token: 0x040048D1 RID: 18641
			Count
		}
	}

	// Token: 0x02000C18 RID: 3096
	public class WeatherGroup
	{
		// Token: 0x06005DE5 RID: 24037 RVA: 0x0024A179 File Offset: 0x00248379
		public void AddProbability(BiomeDefinition.Probabilities.ProbType _type, Vector2 _range, float _probability)
		{
			this.probabilities.AddProbability(_type, _range, _probability);
		}

		// Token: 0x040048D2 RID: 18642
		public string name;

		// Token: 0x040048D3 RID: 18643
		public int stormLevel;

		// Token: 0x040048D4 RID: 18644
		public float prob;

		// Token: 0x040048D5 RID: 18645
		public int duration;

		// Token: 0x040048D6 RID: 18646
		public Vector2i delay;

		// Token: 0x040048D7 RID: 18647
		public string buffName;

		// Token: 0x040048D8 RID: 18648
		public SpectrumWeatherType spectrum;

		// Token: 0x040048D9 RID: 18649
		public BiomeDefinition.Probabilities probabilities = new BiomeDefinition.Probabilities();
	}
}

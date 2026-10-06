using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000C09 RID: 3081
public class BiomeDefinition
{
	// Token: 0x06005DA1 RID: 23969 RVA: 0x0024846E File Offset: 0x0024666E
	public static string LocalizedBiomeName(BiomeDefinition.BiomeType _biomeType)
	{
		return Localization.Get("biome_" + _biomeType.ToStringCached<BiomeDefinition.BiomeType>(), false, null);
	}

	// Token: 0x06005DA2 RID: 23970 RVA: 0x00248488 File Offset: 0x00246688
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

	// Token: 0x06005DA3 RID: 23971 RVA: 0x002485B7 File Offset: 0x002467B7
	public void AddLayer(BiomeLayer _layer)
	{
		this.m_Layers.Add(_layer);
		this.TotalLayerDepth += _layer.m_Depth;
	}

	// Token: 0x06005DA4 RID: 23972 RVA: 0x002485D8 File Offset: 0x002467D8
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

	// Token: 0x06005DA5 RID: 23973 RVA: 0x00248621 File Offset: 0x00246821
	public void AddDecoPrefab(BiomePrefabDecoration _deco)
	{
		this.m_DecoPrefabs.Add(_deco);
	}

	// Token: 0x06005DA6 RID: 23974 RVA: 0x0024862F File Offset: 0x0024682F
	public void AddBluff(BiomeBluffDecoration _deco)
	{
		this.m_DecoBluffs.Add(_deco);
	}

	// Token: 0x06005DA7 RID: 23975 RVA: 0x0024863D File Offset: 0x0024683D
	public void AddReplacement(int _sourceId, int _targetId)
	{
		this.Replacements[_sourceId] = _targetId;
	}

	// Token: 0x06005DA8 RID: 23976 RVA: 0x0024864C File Offset: 0x0024684C
	public void addSubBiome(BiomeDefinition _subbiome)
	{
		this.subbiomes.Add(_subbiome);
	}

	// Token: 0x06005DA9 RID: 23977 RVA: 0x0024865A File Offset: 0x0024685A
	public override bool Equals(object obj)
	{
		return obj is BiomeDefinition && ((BiomeDefinition)obj).m_Id == this.m_Id;
	}

	// Token: 0x06005DAA RID: 23978 RVA: 0x00248679 File Offset: 0x00246879
	public override int GetHashCode()
	{
		return (int)this.m_Id;
	}

	// Token: 0x06005DAB RID: 23979 RVA: 0x00248681 File Offset: 0x00246881
	public override string ToString()
	{
		return this.m_sBiomeName;
	}

	// Token: 0x06005DAC RID: 23980 RVA: 0x00248689 File Offset: 0x00246889
	public static uint GetBiomeColor(BiomeDefinition.BiomeType _type)
	{
		return BiomeDefinition.BiomeColors[(int)_type];
	}

	// Token: 0x06005DAD RID: 23981 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitWeather()
	{
	}

	// Token: 0x06005DAE RID: 23982 RVA: 0x00248694 File Offset: 0x00246894
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

	// Token: 0x06005DAF RID: 23983 RVA: 0x0024873C File Offset: 0x0024693C
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

	// Token: 0x06005DB0 RID: 23984 RVA: 0x002487BD File Offset: 0x002469BD
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float WeatherGetValue(BiomeDefinition.Probabilities.ProbType _type)
	{
		return this.weatherValues[(int)_type];
	}

	// Token: 0x06005DB1 RID: 23985 RVA: 0x002487C7 File Offset: 0x002469C7
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void WeatherSetValue(BiomeDefinition.Probabilities.ProbType _type, float _value)
	{
		this.weatherValues[(int)_type] = _value;
	}

	// Token: 0x06005DB2 RID: 23986 RVA: 0x002487D4 File Offset: 0x002469D4
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

	// Token: 0x06005DB3 RID: 23987 RVA: 0x00248820 File Offset: 0x00246A20
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

	// Token: 0x06005DB4 RID: 23988 RVA: 0x00248844 File Offset: 0x00246A44
	public int WeatherGetDuration(string _weatherGroup)
	{
		BiomeDefinition.WeatherGroup weatherGroup = this.FindWeatherGroup(_weatherGroup);
		if (weatherGroup != null)
		{
			return weatherGroup.duration;
		}
		return 0;
	}

	// Token: 0x06005DB5 RID: 23989 RVA: 0x00248864 File Offset: 0x00246A64
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

	// Token: 0x06005DB6 RID: 23990 RVA: 0x0024889C File Offset: 0x00246A9C
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

	// Token: 0x06005DB7 RID: 23991 RVA: 0x002488E0 File Offset: 0x00246AE0
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

	// Token: 0x06005DB8 RID: 23992 RVA: 0x00248920 File Offset: 0x00246B20
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

	// Token: 0x06005DB9 RID: 23993 RVA: 0x00248984 File Offset: 0x00246B84
	public void SetWeatherGroup(int _index)
	{
		this.currentWeatherGroupIndex = _index;
		BiomeDefinition.WeatherGroup weatherGroup = this.weatherGroups[_index];
		this.currentWeatherGroup = weatherGroup;
		this.weatherSpectrum = weatherGroup.spectrum;
	}

	// Token: 0x0400486D RID: 18541
	public float currentPlayerIntensity;

	// Token: 0x0400486E RID: 18542
	public const string BiomeNameLocalizationPrefix = "biome_";

	// Token: 0x0400486F RID: 18543
	public static string[] BiomeNames = new string[]
	{
		"any", "snow", "forest", "pine_forest", "plains", "desert", "water", "radiated", "wasteland", "burnt_forest",
		"city", "city_wasteland", "wasteland_hub", "caveFloor", "caveCeiling"
	};

	// Token: 0x04004870 RID: 18544
	public static uint[] BiomeColors = new uint[]
	{
		0U, 16777215U, 0U, 16384U, 0U, 16770167U, 25599U, 0U, 16754688U, 12189951U,
		8421504U, 12632256U, 10526880U, 0U, 0U
	};

	// Token: 0x04004871 RID: 18545
	public readonly byte m_Id;

	// Token: 0x04004872 RID: 18546
	public readonly BiomeDefinition.BiomeType m_BiomeType;

	// Token: 0x04004873 RID: 18547
	public byte subId;

	// Token: 0x04004874 RID: 18548
	public readonly string m_sBiomeName;

	// Token: 0x04004875 RID: 18549
	public readonly string LocalizedName;

	// Token: 0x04004876 RID: 18550
	public uint m_uiColor;

	// Token: 0x04004877 RID: 18551
	public string m_SpectrumName;

	// Token: 0x04004878 RID: 18552
	public static Dictionary<string, byte> nameToId;

	// Token: 0x04004879 RID: 18553
	public int m_RadiationLevel;

	// Token: 0x0400487A RID: 18554
	public int Difficulty = 1;

	// Token: 0x0400487B RID: 18555
	public List<BiomeLayer> m_Layers;

	// Token: 0x0400487C RID: 18556
	public List<BiomeBlockDecoration> m_DecoBlocks;

	// Token: 0x0400487D RID: 18557
	public List<BiomeBlockDecoration> m_DistantDecoBlocks;

	// Token: 0x0400487E RID: 18558
	public List<BiomePrefabDecoration> m_DecoPrefabs;

	// Token: 0x0400487F RID: 18559
	public List<BiomeBluffDecoration> m_DecoBluffs;

	// Token: 0x04004880 RID: 18560
	public List<BiomeDefinition.WeatherGroup> weatherGroups = new List<BiomeDefinition.WeatherGroup>();

	// Token: 0x04004881 RID: 18561
	public string weatherName;

	// Token: 0x04004882 RID: 18562
	public int currentWeatherGroupIndex;

	// Token: 0x04004883 RID: 18563
	public BiomeDefinition.WeatherGroup currentWeatherGroup;

	// Token: 0x04004884 RID: 18564
	public SpectrumWeatherType weatherSpectrum;

	// Token: 0x04004885 RID: 18565
	[PublicizedFrom(EAccessModifier.Private)]
	public float[] weatherValues = new float[5];

	// Token: 0x04004886 RID: 18566
	public int TotalLayerDepth;

	// Token: 0x04004887 RID: 18567
	public List<BiomeDefinition> subbiomes = new List<BiomeDefinition>();

	// Token: 0x04004888 RID: 18568
	public float noiseFreq = 0.03f;

	// Token: 0x04004889 RID: 18569
	public float noiseMin = 0.2f;

	// Token: 0x0400488A RID: 18570
	public float noiseMax = 1f;

	// Token: 0x0400488B RID: 18571
	public Vector2 noiseOffset;

	// Token: 0x0400488C RID: 18572
	public Dictionary<int, int> Replacements = new Dictionary<int, int>();

	// Token: 0x0400488D RID: 18573
	public float GameStageMod;

	// Token: 0x0400488E RID: 18574
	public float GameStageBonus;

	// Token: 0x0400488F RID: 18575
	public float LootStageMod;

	// Token: 0x04004890 RID: 18576
	public float LootStageBonus;

	// Token: 0x04004891 RID: 18577
	public int LootStageMin = -1;

	// Token: 0x04004892 RID: 18578
	public int LootStageMax = -1;

	// Token: 0x04004893 RID: 18579
	public string Buff;

	// Token: 0x02000C0A RID: 3082
	public enum BiomeType
	{
		// Token: 0x04004895 RID: 18581
		Any,
		// Token: 0x04004896 RID: 18582
		Snow,
		// Token: 0x04004897 RID: 18583
		Forest,
		// Token: 0x04004898 RID: 18584
		PineForest,
		// Token: 0x04004899 RID: 18585
		Plains,
		// Token: 0x0400489A RID: 18586
		Desert,
		// Token: 0x0400489B RID: 18587
		Water,
		// Token: 0x0400489C RID: 18588
		Radiated,
		// Token: 0x0400489D RID: 18589
		Wasteland,
		// Token: 0x0400489E RID: 18590
		burnt_forest,
		// Token: 0x0400489F RID: 18591
		city,
		// Token: 0x040048A0 RID: 18592
		city_wasteland,
		// Token: 0x040048A1 RID: 18593
		wasteland_hub,
		// Token: 0x040048A2 RID: 18594
		caveFloor,
		// Token: 0x040048A3 RID: 18595
		caveCeiling
	}

	// Token: 0x02000C0B RID: 3083
	public class Probabilities
	{
		// Token: 0x06005DBB RID: 23995 RVA: 0x00248A68 File Offset: 0x00246C68
		public Probabilities()
		{
			this.probabilities = new List<Vector3>[5];
			for (int i = 0; i < 5; i++)
			{
				this.probabilities[i] = new List<Vector3>();
			}
		}

		// Token: 0x06005DBC RID: 23996 RVA: 0x00248AA0 File Offset: 0x00246CA0
		public void AddProbability(BiomeDefinition.Probabilities.ProbType _type, Vector2 _range, float _probability)
		{
			this.probabilities[(int)_type].Add(new Vector3(_range.x, _range.y, _probability));
		}

		// Token: 0x06005DBD RID: 23997 RVA: 0x00248AD0 File Offset: 0x00246CD0
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

		// Token: 0x06005DBE RID: 23998 RVA: 0x00248B48 File Offset: 0x00246D48
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

		// Token: 0x06005DBF RID: 23999 RVA: 0x00248BD4 File Offset: 0x00246DD4
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

		// Token: 0x040048A4 RID: 18596
		public const int ProbTypeCount = 5;

		// Token: 0x040048A5 RID: 18597
		[PublicizedFrom(EAccessModifier.Private)]
		public List<Vector3>[] probabilities;

		// Token: 0x02000C0C RID: 3084
		public enum ProbType
		{
			// Token: 0x040048A7 RID: 18599
			Temperature,
			// Token: 0x040048A8 RID: 18600
			Precipitation,
			// Token: 0x040048A9 RID: 18601
			CloudThickness,
			// Token: 0x040048AA RID: 18602
			Wind,
			// Token: 0x040048AB RID: 18603
			Fog,
			// Token: 0x040048AC RID: 18604
			Count
		}
	}

	// Token: 0x02000C0D RID: 3085
	public class WeatherGroup
	{
		// Token: 0x06005DC0 RID: 24000 RVA: 0x00248C51 File Offset: 0x00246E51
		public void AddProbability(BiomeDefinition.Probabilities.ProbType _type, Vector2 _range, float _probability)
		{
			this.probabilities.AddProbability(_type, _range, _probability);
		}

		// Token: 0x040048AD RID: 18605
		public string name;

		// Token: 0x040048AE RID: 18606
		public int stormLevel;

		// Token: 0x040048AF RID: 18607
		public float prob;

		// Token: 0x040048B0 RID: 18608
		public int duration;

		// Token: 0x040048B1 RID: 18609
		public Vector2i delay;

		// Token: 0x040048B2 RID: 18610
		public string buffName;

		// Token: 0x040048B3 RID: 18611
		public SpectrumWeatherType spectrum;

		// Token: 0x040048B4 RID: 18612
		public BiomeDefinition.Probabilities probabilities = new BiomeDefinition.Probabilities();
	}
}

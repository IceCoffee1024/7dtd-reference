using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000BE5 RID: 3045
public class BiomeDefinition
{
	// Token: 0x06005CFE RID: 23806 RVA: 0x002423CE File Offset: 0x002405CE
	public static string LocalizedBiomeName(BiomeDefinition.BiomeType _biomeType)
	{
		return Localization.Get("biome_" + _biomeType.ToStringCached<BiomeDefinition.BiomeType>(), false, null);
	}

	// Token: 0x06005CFF RID: 23807 RVA: 0x002423E8 File Offset: 0x002405E8
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

	// Token: 0x06005D00 RID: 23808 RVA: 0x00242517 File Offset: 0x00240717
	public void AddLayer(BiomeLayer _layer)
	{
		this.m_Layers.Add(_layer);
		this.TotalLayerDepth += _layer.m_Depth;
	}

	// Token: 0x06005D01 RID: 23809 RVA: 0x00242538 File Offset: 0x00240738
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

	// Token: 0x06005D02 RID: 23810 RVA: 0x00242581 File Offset: 0x00240781
	public void AddDecoPrefab(BiomePrefabDecoration _deco)
	{
		this.m_DecoPrefabs.Add(_deco);
	}

	// Token: 0x06005D03 RID: 23811 RVA: 0x0024258F File Offset: 0x0024078F
	public void AddBluff(BiomeBluffDecoration _deco)
	{
		this.m_DecoBluffs.Add(_deco);
	}

	// Token: 0x06005D04 RID: 23812 RVA: 0x0024259D File Offset: 0x0024079D
	public void AddReplacement(int _sourceId, int _targetId)
	{
		this.Replacements[_sourceId] = _targetId;
	}

	// Token: 0x06005D05 RID: 23813 RVA: 0x002425AC File Offset: 0x002407AC
	public void addSubBiome(BiomeDefinition _subbiome)
	{
		this.subbiomes.Add(_subbiome);
	}

	// Token: 0x06005D06 RID: 23814 RVA: 0x002425BA File Offset: 0x002407BA
	public override bool Equals(object obj)
	{
		return obj is BiomeDefinition && ((BiomeDefinition)obj).m_Id == this.m_Id;
	}

	// Token: 0x06005D07 RID: 23815 RVA: 0x002425D9 File Offset: 0x002407D9
	public override int GetHashCode()
	{
		return (int)this.m_Id;
	}

	// Token: 0x06005D08 RID: 23816 RVA: 0x002425E1 File Offset: 0x002407E1
	public override string ToString()
	{
		return this.m_sBiomeName;
	}

	// Token: 0x06005D09 RID: 23817 RVA: 0x002425E9 File Offset: 0x002407E9
	public static uint GetBiomeColor(BiomeDefinition.BiomeType _type)
	{
		return BiomeDefinition.BiomeColors[(int)_type];
	}

	// Token: 0x06005D0A RID: 23818 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitWeather()
	{
	}

	// Token: 0x06005D0B RID: 23819 RVA: 0x002425F4 File Offset: 0x002407F4
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

	// Token: 0x06005D0C RID: 23820 RVA: 0x0024269C File Offset: 0x0024089C
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

	// Token: 0x06005D0D RID: 23821 RVA: 0x0024271D File Offset: 0x0024091D
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float WeatherGetValue(BiomeDefinition.Probabilities.ProbType _type)
	{
		return this.weatherValues[(int)_type];
	}

	// Token: 0x06005D0E RID: 23822 RVA: 0x00242727 File Offset: 0x00240927
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void WeatherSetValue(BiomeDefinition.Probabilities.ProbType _type, float _value)
	{
		this.weatherValues[(int)_type] = _value;
	}

	// Token: 0x06005D0F RID: 23823 RVA: 0x00242734 File Offset: 0x00240934
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

	// Token: 0x06005D10 RID: 23824 RVA: 0x00242780 File Offset: 0x00240980
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

	// Token: 0x06005D11 RID: 23825 RVA: 0x002427A4 File Offset: 0x002409A4
	public int WeatherGetDuration(string _weatherGroup)
	{
		BiomeDefinition.WeatherGroup weatherGroup = this.FindWeatherGroup(_weatherGroup);
		if (weatherGroup != null)
		{
			return weatherGroup.duration;
		}
		return 0;
	}

	// Token: 0x06005D12 RID: 23826 RVA: 0x002427C4 File Offset: 0x002409C4
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

	// Token: 0x06005D13 RID: 23827 RVA: 0x002427FC File Offset: 0x002409FC
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

	// Token: 0x06005D14 RID: 23828 RVA: 0x00242840 File Offset: 0x00240A40
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

	// Token: 0x06005D15 RID: 23829 RVA: 0x00242880 File Offset: 0x00240A80
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

	// Token: 0x06005D16 RID: 23830 RVA: 0x002428E4 File Offset: 0x00240AE4
	public void SetWeatherGroup(int _index)
	{
		this.currentWeatherGroupIndex = _index;
		BiomeDefinition.WeatherGroup weatherGroup = this.weatherGroups[_index];
		this.currentWeatherGroup = weatherGroup;
		this.weatherSpectrum = weatherGroup.spectrum;
	}

	// Token: 0x04004792 RID: 18322
	public float currentPlayerIntensity;

	// Token: 0x04004793 RID: 18323
	public const string BiomeNameLocalizationPrefix = "biome_";

	// Token: 0x04004794 RID: 18324
	public static string[] BiomeNames = new string[]
	{
		"any", "snow", "forest", "pine_forest", "plains", "desert", "water", "radiated", "wasteland", "burnt_forest",
		"city", "city_wasteland", "wasteland_hub", "caveFloor", "caveCeiling"
	};

	// Token: 0x04004795 RID: 18325
	public static uint[] BiomeColors = new uint[]
	{
		0U, 16777215U, 0U, 16384U, 0U, 16770167U, 25599U, 0U, 16754688U, 12189951U,
		8421504U, 12632256U, 10526880U, 0U, 0U
	};

	// Token: 0x04004796 RID: 18326
	public readonly byte m_Id;

	// Token: 0x04004797 RID: 18327
	public readonly BiomeDefinition.BiomeType m_BiomeType;

	// Token: 0x04004798 RID: 18328
	public byte subId;

	// Token: 0x04004799 RID: 18329
	public readonly string m_sBiomeName;

	// Token: 0x0400479A RID: 18330
	public readonly string LocalizedName;

	// Token: 0x0400479B RID: 18331
	public uint m_uiColor;

	// Token: 0x0400479C RID: 18332
	public string m_SpectrumName;

	// Token: 0x0400479D RID: 18333
	public static Dictionary<string, byte> nameToId;

	// Token: 0x0400479E RID: 18334
	public int m_RadiationLevel;

	// Token: 0x0400479F RID: 18335
	public int Difficulty = 1;

	// Token: 0x040047A0 RID: 18336
	public List<BiomeLayer> m_Layers;

	// Token: 0x040047A1 RID: 18337
	public List<BiomeBlockDecoration> m_DecoBlocks;

	// Token: 0x040047A2 RID: 18338
	public List<BiomeBlockDecoration> m_DistantDecoBlocks;

	// Token: 0x040047A3 RID: 18339
	public List<BiomePrefabDecoration> m_DecoPrefabs;

	// Token: 0x040047A4 RID: 18340
	public List<BiomeBluffDecoration> m_DecoBluffs;

	// Token: 0x040047A5 RID: 18341
	public List<BiomeDefinition.WeatherGroup> weatherGroups = new List<BiomeDefinition.WeatherGroup>();

	// Token: 0x040047A6 RID: 18342
	public string weatherName;

	// Token: 0x040047A7 RID: 18343
	public int currentWeatherGroupIndex;

	// Token: 0x040047A8 RID: 18344
	public BiomeDefinition.WeatherGroup currentWeatherGroup;

	// Token: 0x040047A9 RID: 18345
	public SpectrumWeatherType weatherSpectrum;

	// Token: 0x040047AA RID: 18346
	[PublicizedFrom(EAccessModifier.Private)]
	public float[] weatherValues = new float[5];

	// Token: 0x040047AB RID: 18347
	public int TotalLayerDepth;

	// Token: 0x040047AC RID: 18348
	public List<BiomeDefinition> subbiomes = new List<BiomeDefinition>();

	// Token: 0x040047AD RID: 18349
	public float noiseFreq = 0.03f;

	// Token: 0x040047AE RID: 18350
	public float noiseMin = 0.2f;

	// Token: 0x040047AF RID: 18351
	public float noiseMax = 1f;

	// Token: 0x040047B0 RID: 18352
	public Vector2 noiseOffset;

	// Token: 0x040047B1 RID: 18353
	public Dictionary<int, int> Replacements = new Dictionary<int, int>();

	// Token: 0x040047B2 RID: 18354
	public float GameStageMod;

	// Token: 0x040047B3 RID: 18355
	public float GameStageBonus;

	// Token: 0x040047B4 RID: 18356
	public float LootStageMod;

	// Token: 0x040047B5 RID: 18357
	public float LootStageBonus;

	// Token: 0x040047B6 RID: 18358
	public int LootStageMin = -1;

	// Token: 0x040047B7 RID: 18359
	public int LootStageMax = -1;

	// Token: 0x040047B8 RID: 18360
	public string Buff;

	// Token: 0x02000BE6 RID: 3046
	public enum BiomeType
	{
		// Token: 0x040047BA RID: 18362
		Any,
		// Token: 0x040047BB RID: 18363
		Snow,
		// Token: 0x040047BC RID: 18364
		Forest,
		// Token: 0x040047BD RID: 18365
		PineForest,
		// Token: 0x040047BE RID: 18366
		Plains,
		// Token: 0x040047BF RID: 18367
		Desert,
		// Token: 0x040047C0 RID: 18368
		Water,
		// Token: 0x040047C1 RID: 18369
		Radiated,
		// Token: 0x040047C2 RID: 18370
		Wasteland,
		// Token: 0x040047C3 RID: 18371
		burnt_forest,
		// Token: 0x040047C4 RID: 18372
		city,
		// Token: 0x040047C5 RID: 18373
		city_wasteland,
		// Token: 0x040047C6 RID: 18374
		wasteland_hub,
		// Token: 0x040047C7 RID: 18375
		caveFloor,
		// Token: 0x040047C8 RID: 18376
		caveCeiling
	}

	// Token: 0x02000BE7 RID: 3047
	public class Probabilities
	{
		// Token: 0x06005D18 RID: 23832 RVA: 0x002429C8 File Offset: 0x00240BC8
		public Probabilities()
		{
			this.probabilities = new List<Vector3>[5];
			for (int i = 0; i < 5; i++)
			{
				this.probabilities[i] = new List<Vector3>();
			}
		}

		// Token: 0x06005D19 RID: 23833 RVA: 0x00242A00 File Offset: 0x00240C00
		public void AddProbability(BiomeDefinition.Probabilities.ProbType _type, Vector2 _range, float _probability)
		{
			this.probabilities[(int)_type].Add(new Vector3(_range.x, _range.y, _probability));
		}

		// Token: 0x06005D1A RID: 23834 RVA: 0x00242A30 File Offset: 0x00240C30
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

		// Token: 0x06005D1B RID: 23835 RVA: 0x00242AA8 File Offset: 0x00240CA8
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

		// Token: 0x06005D1C RID: 23836 RVA: 0x00242B34 File Offset: 0x00240D34
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

		// Token: 0x040047C9 RID: 18377
		public const int ProbTypeCount = 5;

		// Token: 0x040047CA RID: 18378
		[PublicizedFrom(EAccessModifier.Private)]
		public List<Vector3>[] probabilities;

		// Token: 0x02000BE8 RID: 3048
		public enum ProbType
		{
			// Token: 0x040047CC RID: 18380
			Temperature,
			// Token: 0x040047CD RID: 18381
			Precipitation,
			// Token: 0x040047CE RID: 18382
			CloudThickness,
			// Token: 0x040047CF RID: 18383
			Wind,
			// Token: 0x040047D0 RID: 18384
			Fog,
			// Token: 0x040047D1 RID: 18385
			Count
		}
	}

	// Token: 0x02000BE9 RID: 3049
	public class WeatherGroup
	{
		// Token: 0x06005D1D RID: 23837 RVA: 0x00242BB1 File Offset: 0x00240DB1
		public void AddProbability(BiomeDefinition.Probabilities.ProbType _type, Vector2 _range, float _probability)
		{
			this.probabilities.AddProbability(_type, _range, _probability);
		}

		// Token: 0x040047D2 RID: 18386
		public string name;

		// Token: 0x040047D3 RID: 18387
		public int stormLevel;

		// Token: 0x040047D4 RID: 18388
		public float prob;

		// Token: 0x040047D5 RID: 18389
		public int duration;

		// Token: 0x040047D6 RID: 18390
		public Vector2i delay;

		// Token: 0x040047D7 RID: 18391
		public string buffName;

		// Token: 0x040047D8 RID: 18392
		public SpectrumWeatherType spectrum;

		// Token: 0x040047D9 RID: 18393
		public BiomeDefinition.Probabilities probabilities = new BiomeDefinition.Probabilities();
	}
}

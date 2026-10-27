using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UniLinq;
using UnityEngine;

namespace WorldGenerationEngineFinal
{
	// Token: 0x02001779 RID: 6009
	public class PrefabManager
	{
		// Token: 0x0600BA8A RID: 47754 RVA: 0x0044D32C File Offset: 0x0044B52C
		public PrefabManager(WorldBuilder _worldBuilder)
		{
			this.worldBuilder = _worldBuilder;
		}

		// Token: 0x0600BA8B RID: 47755 RVA: 0x0044D367 File Offset: 0x0044B567
		public void LoadPrefabs()
		{
			this.prefabManagerData.LoadPrefabs();
		}

		// Token: 0x0600BA8C RID: 47756 RVA: 0x0044D374 File Offset: 0x0044B574
		public void ShufflePrefabData(int _seed)
		{
			this.prefabManagerData.ShufflePrefabData(_seed);
		}

		// Token: 0x0600BA8D RID: 47757 RVA: 0x0044D382 File Offset: 0x0044B582
		public void Clear()
		{
			this.StreetTilesUsed.Clear();
		}

		// Token: 0x0600BA8E RID: 47758 RVA: 0x0044D38F File Offset: 0x0044B58F
		public void ClearDisplayed()
		{
			this.UsedPrefabsWorld.Clear();
			this.WorldUsedPrefabNames.Clear();
		}

		// Token: 0x0600BA8F RID: 47759 RVA: 0x0044D3A7 File Offset: 0x0044B5A7
		public void Cleanup()
		{
			this.prefabManagerData.Cleanup();
			this.ClearDisplayed();
		}

		// Token: 0x0600BA90 RID: 47760 RVA: 0x0044D3BC File Offset: 0x0044B5BC
		public static bool isSizeValid(PrefabData prefab, Vector2i minSize, Vector2i maxSize)
		{
			return (maxSize == default(Vector2i) || (prefab.size.x <= maxSize.x && prefab.size.z <= maxSize.y) || (prefab.size.z <= maxSize.x && prefab.size.x <= maxSize.y)) && (minSize == default(Vector2i) || (prefab.size.x >= minSize.x && prefab.size.z >= minSize.y) || (prefab.size.z >= minSize.x && prefab.size.x >= minSize.y));
		}

		// Token: 0x0600BA91 RID: 47761 RVA: 0x0044D48C File Offset: 0x0044B68C
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isThemeValid(PrefabData prefab, Vector2i prefabPos, List<PrefabDataInstance> prefabInstances, int distance)
		{
			if (prefab.ThemeTags.IsEmpty)
			{
				return true;
			}
			prefabPos.x -= this.worldBuilder.WorldSize / 2;
			prefabPos.y -= this.worldBuilder.WorldSize / 2;
			int num = distance * distance;
			foreach (PrefabDataInstance prefabDataInstance in prefabInstances)
			{
				if (!prefabDataInstance.prefab.ThemeTags.IsEmpty && prefabDataInstance.prefab.ThemeTags.Test_AnySet(prefab.ThemeTags) && Vector2i.DistanceSqr(prefabDataInstance.CenterXZ, prefabPos) < (float)num)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600BA92 RID: 47762 RVA: 0x0044D55C File Offset: 0x0044B75C
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isNameValid(PrefabData prefab, Vector2i prefabPos, List<PrefabDataInstance> prefabInstances, int distance)
		{
			prefabPos.x -= this.worldBuilder.WorldSize / 2;
			prefabPos.y -= this.worldBuilder.WorldSize / 2;
			int num = distance * distance;
			foreach (PrefabDataInstance prefabDataInstance in prefabInstances)
			{
				if (!(prefabDataInstance.prefab.Name != prefab.Name) && Vector2i.DistanceSqr(prefabDataInstance.CenterXZ, prefabPos) < (float)num)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600BA93 RID: 47763 RVA: 0x0044D608 File Offset: 0x0044B808
		public PrefabData GetPrefabWithDistrict(District _district, FastTags<TagGroup.Poi> _markerTags, Vector2i minSize, Vector2i maxSize, Vector2i center, float densityPointsLeft, float _distanceScale)
		{
			bool flag = !_district.tag.IsEmpty;
			bool flag2 = !_markerTags.IsEmpty;
			PrefabData prefabData = null;
			float num = float.MinValue;
			int worldSizeDistDiv = this.worldBuilder.WorldSizeDistDiv;
			for (int i = 0; i < this.prefabManagerData.prefabDataList.Count; i++)
			{
				PrefabData prefabData2 = this.prefabManagerData.prefabDataList[i];
				if (prefabData2.DensityScore <= densityPointsLeft && !prefabData2.Tags.Test_AnySet(this.prefabManagerData.PartsAndTilesTags) && (!flag || prefabData2.Tags.Test_AllSet(_district.tag)))
				{
					if (flag2)
					{
						if (!prefabData2.Tags.Test_AnySet(_markerTags))
						{
							goto IL_0149;
						}
					}
					else if (prefabData2.Tags.Test_AnySet(this.prefabManagerData.HasTags))
					{
						goto IL_0149;
					}
					if (PrefabManager.isSizeValid(prefabData2, minSize, maxSize))
					{
						int num2 = prefabData2.ThemeRepeatDistance;
						if (prefabData2.ThemeTags.Test_AnySet(this.prefabManagerData.TraderTags))
						{
							num2 /= worldSizeDistDiv;
						}
						if (this.isThemeValid(prefabData2, center, this.UsedPrefabsWorld, num2) && (_distanceScale <= 0f || this.isNameValid(prefabData2, center, this.UsedPrefabsWorld, (int)((float)prefabData2.DuplicateRepeatDistance * _distanceScale))))
						{
							float scoreForPrefab = this.getScoreForPrefab(prefabData2, center);
							if (scoreForPrefab > num)
							{
								num = scoreForPrefab;
								prefabData = prefabData2;
							}
						}
					}
				}
				IL_0149:;
			}
			return prefabData;
		}

		// Token: 0x0600BA94 RID: 47764 RVA: 0x0044D77C File Offset: 0x0044B97C
		public PrefabData GetWildernessPrefab(FastTags<TagGroup.Poi> _withoutTags, FastTags<TagGroup.Poi> _markerTags, Vector2i minSize = default(Vector2i), Vector2i maxSize = default(Vector2i), Vector2i center = default(Vector2i), bool _isRetry = false)
		{
			PrefabData prefabData = null;
			float num = float.MinValue;
			for (int i = 0; i < this.prefabManagerData.prefabDataList.Count; i++)
			{
				PrefabData prefabData2 = this.prefabManagerData.prefabDataList[i];
				if (!prefabData2.Tags.Test_AnySet(this.prefabManagerData.PartsAndTilesTags) && (prefabData2.Tags.Test_AnySet(this.prefabManagerData.WildernessTags) || prefabData2.Tags.Test_AnySet(this.prefabManagerData.TraderTags)) && (_markerTags.IsEmpty || prefabData2.Tags.Test_AnySet(_markerTags) || prefabData2.ThemeTags.Test_AnySet(_markerTags)) && PrefabManager.isSizeValid(prefabData2, minSize, maxSize) && this.isThemeValid(prefabData2, center, this.UsedPrefabsWorld, prefabData2.ThemeRepeatDistance) && (_isRetry || this.isNameValid(prefabData2, center, this.UsedPrefabsWorld, prefabData2.DuplicateRepeatDistance)))
				{
					float scoreForPrefab = this.getScoreForPrefab(prefabData2, center);
					if (scoreForPrefab > num)
					{
						num = scoreForPrefab;
						prefabData = prefabData2;
					}
				}
			}
			if (prefabData == null && !_isRetry)
			{
				return this.GetWildernessPrefab(_withoutTags, _markerTags, minSize, maxSize, center, true);
			}
			return prefabData;
		}

		// Token: 0x0600BA95 RID: 47765 RVA: 0x0044D8A0 File Offset: 0x0044BAA0
		[PublicizedFrom(EAccessModifier.Private)]
		public static int getRandomVal(int min, int maxExclusive, int seed)
		{
			GameRandom gameRandom = GameRandomManager.Instance.CreateGameRandom(seed);
			int num = gameRandom.RandomRange(min, maxExclusive);
			GameRandomManager.Instance.FreeGameRandom(gameRandom);
			return num;
		}

		// Token: 0x0600BA96 RID: 47766 RVA: 0x0044D8CC File Offset: 0x0044BACC
		[PublicizedFrom(EAccessModifier.Private)]
		public static float getRandomVal(float min, float max, int seed)
		{
			GameRandom gameRandom = GameRandomManager.Instance.CreateGameRandom(seed);
			float num = gameRandom.RandomRange(min, max);
			GameRandomManager.Instance.FreeGameRandom(gameRandom);
			return num;
		}

		// Token: 0x0600BA97 RID: 47767 RVA: 0x0044D8F8 File Offset: 0x0044BAF8
		public PrefabData GetPrefabByName(string _lowerCaseName)
		{
			PrefabData prefabData;
			if (!this.prefabManagerData.AllPrefabDatas.TryGetValue(_lowerCaseName.ToLower(), out prefabData))
			{
				return null;
			}
			return prefabData;
		}

		// Token: 0x0600BA98 RID: 47768 RVA: 0x0044D924 File Offset: 0x0044BB24
		public PrefabData GetStreetTile(string _lowerCaseName, Vector2i centerPoint, bool useExactString = false)
		{
			GameRandom rnd = GameRandomManager.Instance.CreateGameRandom(this.worldBuilder.Seed + (centerPoint.x + centerPoint.x * centerPoint.y * centerPoint.y));
			string text = this.prefabManagerData.AllPrefabDatas.Keys.Where<string>(delegate(string c)
			{
				Vector2i vector2i;
				int num;
				return ((useExactString && c.Equals(_lowerCaseName)) || (!useExactString && c.StartsWith(_lowerCaseName))) && (!PrefabManagerStatic.TileMinMaxCounts.TryGetValue(c, out vector2i) || !this.StreetTilesUsed.TryGetValue(c, out num) || num < vector2i.y);
			}).OrderByDescending<string, float>(delegate(string c)
			{
				Vector2i vector2i2;
				return (float)(PrefabManagerStatic.TileMinMaxCounts.TryGetValue(c, out vector2i2) ? vector2i2.x : 0) + rnd.RandomRange(0f, 1f);
			}).FirstOrDefault<string>();
			GameRandomManager.Instance.FreeGameRandom(rnd);
			if (text == null)
			{
				Log.Warning("Tile starting with " + _lowerCaseName + " not found!");
				return null;
			}
			return this.prefabManagerData.AllPrefabDatas[text];
		}

		// Token: 0x0600BA99 RID: 47769 RVA: 0x0044D9FC File Offset: 0x0044BBFC
		public bool SavePrefabData(Stream _stream)
		{
			bool flag;
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.CreateXmlDeclaration();
				XmlElement xmlElement = xmlDocument.AddXmlElement("prefabs");
				for (int i = 0; i < this.UsedPrefabsWorld.Count; i++)
				{
					PrefabDataInstance prefabDataInstance = this.UsedPrefabsWorld[i];
					if (prefabDataInstance != null)
					{
						string text = "";
						if (prefabDataInstance.prefab != null && prefabDataInstance.prefab.location.Type != PathAbstractions.EAbstractedLocationType.None)
						{
							text = prefabDataInstance.prefab.location.Name;
						}
						else if (prefabDataInstance.location.Type != PathAbstractions.EAbstractedLocationType.None)
						{
							text = prefabDataInstance.location.Name;
						}
						xmlElement.AddXmlElement("decoration").SetAttrib("type", "model").SetAttrib("name", text)
							.SetAttrib("position", prefabDataInstance.boundingBoxPosition.ToStringNoBlanks())
							.SetAttrib("rotation", prefabDataInstance.rotation.ToString());
					}
				}
				xmlDocument.Save(_stream);
				flag = true;
			}
			catch (Exception ex)
			{
				Log.Exception(ex);
				flag = false;
			}
			return flag;
		}

		// Token: 0x0600BA9A RID: 47770 RVA: 0x0044DB1C File Offset: 0x0044BD1C
		public void GetPrefabsAround(Vector3 _position, float _distance, Dictionary<int, PrefabDataInstance> _prefabs)
		{
			for (int i = 0; i < this.UsedPrefabsWorld.Count; i++)
			{
				PrefabDataInstance prefabDataInstance = this.UsedPrefabsWorld[i];
				_prefabs[this.UsedPrefabsWorld[i].id] = this.UsedPrefabsWorld[i];
			}
		}

		// Token: 0x0600BA9B RID: 47771 RVA: 0x0044DB70 File Offset: 0x0044BD70
		[PublicizedFrom(EAccessModifier.Private)]
		public float getScoreForPrefab(PrefabData prefab, Vector2i center)
		{
			float num = 1f;
			float num2 = 1f;
			FastTags<TagGroup.Poi> fastTags = FastTags<TagGroup.Poi>.Parse(this.worldBuilder.GetBiome(center).ToString());
			PrefabManager.POIWeightData poiweightData = null;
			for (int i = 0; i < PrefabManagerStatic.prefabWeightData.Count; i++)
			{
				PrefabManager.POIWeightData poiweightData2 = PrefabManagerStatic.prefabWeightData[i];
				bool flag = poiweightData2.PartialPOIName.Length > 0 && prefab.Name.Contains(poiweightData2.PartialPOIName, StringComparison.OrdinalIgnoreCase);
				if (flag && !poiweightData2.BiomeTags.IsEmpty && !poiweightData2.BiomeTags.Test_AnySet(fastTags))
				{
					return float.MinValue;
				}
				if (flag || (!poiweightData2.Tags.IsEmpty && ((!prefab.Tags.IsEmpty && prefab.Tags.Test_AnySet(poiweightData2.Tags)) || (!prefab.ThemeTags.IsEmpty && prefab.ThemeTags.Test_AnySet(poiweightData2.Tags)))))
				{
					poiweightData = poiweightData2;
					break;
				}
			}
			if (poiweightData != null)
			{
				num2 = poiweightData.Weight;
				num += poiweightData.Bias;
				int num4;
				int num3 = (this.WorldUsedPrefabNames.TryGetValue(prefab.Name, out num4) ? num4 : 0);
				if (num3 < poiweightData.MinCount)
				{
					num += (float)(poiweightData.MinCount - num3);
				}
				int num5;
				if (this.WorldUsedPrefabNames.TryGetValue(prefab.Name, out num5) && num5 >= poiweightData.MaxCount)
				{
					num2 = 0f;
				}
			}
			num += (float)prefab.DifficultyTier / 5f;
			int num6;
			if (this.WorldUsedPrefabNames.TryGetValue(prefab.Name, out num6))
			{
				num /= (float)num6 + 1f;
			}
			return num * num2;
		}

		// Token: 0x0600BA9C RID: 47772 RVA: 0x0044DD28 File Offset: 0x0044BF28
		public void AddUsedPrefab(string prefabName)
		{
			int num;
			if (this.WorldUsedPrefabNames.TryGetValue(prefabName, out num))
			{
				this.WorldUsedPrefabNames[prefabName] = num + 1;
				return;
			}
			this.WorldUsedPrefabNames.Add(prefabName, 1);
		}

		// Token: 0x0600BA9D RID: 47773 RVA: 0x0044DD62 File Offset: 0x0044BF62
		public void AddUsedPrefabWorld(int townshipID, PrefabDataInstance pdi)
		{
			this.UsedPrefabsWorld.Add(pdi);
			this.AddUsedPrefab(pdi.prefab.Name);
		}

		// Token: 0x04008B46 RID: 35654
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly WorldBuilder worldBuilder;

		// Token: 0x04008B47 RID: 35655
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly PrefabManagerData prefabManagerData = new PrefabManagerData();

		// Token: 0x04008B48 RID: 35656
		public readonly Dictionary<string, int> StreetTilesUsed = new Dictionary<string, int>();

		// Token: 0x04008B49 RID: 35657
		public readonly List<PrefabDataInstance> UsedPrefabsWorld = new List<PrefabDataInstance>();

		// Token: 0x04008B4A RID: 35658
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<string, int> WorldUsedPrefabNames = new Dictionary<string, int>();

		// Token: 0x04008B4B RID: 35659
		public int PrefabInstanceId;

		// Token: 0x0200177A RID: 6010
		public class POIWeightData
		{
			// Token: 0x0600BA9E RID: 47774 RVA: 0x0044DD84 File Offset: 0x0044BF84
			public POIWeightData(string _partialPOIName, FastTags<TagGroup.Poi> _tags, FastTags<TagGroup.Poi> _biomeTags, float _weight, float _bias, int minCount, int maxCount)
			{
				this.PartialPOIName = _partialPOIName.ToLower();
				this.Tags = _tags;
				this.BiomeTags = _biomeTags;
				this.Weight = _weight;
				this.Bias = _bias;
				this.MinCount = minCount;
				this.MaxCount = maxCount;
			}

			// Token: 0x04008B4C RID: 35660
			public string PartialPOIName;

			// Token: 0x04008B4D RID: 35661
			public FastTags<TagGroup.Poi> Tags;

			// Token: 0x04008B4E RID: 35662
			public FastTags<TagGroup.Poi> BiomeTags;

			// Token: 0x04008B4F RID: 35663
			public float Weight;

			// Token: 0x04008B50 RID: 35664
			public float Bias;

			// Token: 0x04008B51 RID: 35665
			public int MinCount;

			// Token: 0x04008B52 RID: 35666
			public int MaxCount;
		}
	}
}

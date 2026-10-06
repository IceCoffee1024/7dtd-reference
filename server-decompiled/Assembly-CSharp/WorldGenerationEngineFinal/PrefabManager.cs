using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UniLinq;
using UnityEngine;

namespace WorldGenerationEngineFinal
{
	// Token: 0x02001748 RID: 5960
	public class PrefabManager
	{
		// Token: 0x0600B976 RID: 47478 RVA: 0x00449980 File Offset: 0x00447B80
		public PrefabManager(WorldBuilder _worldBuilder)
		{
			this.worldBuilder = _worldBuilder;
		}

		// Token: 0x0600B977 RID: 47479 RVA: 0x004499BB File Offset: 0x00447BBB
		public void LoadPrefabs()
		{
			this.prefabManagerData.LoadPrefabs();
		}

		// Token: 0x0600B978 RID: 47480 RVA: 0x004499C8 File Offset: 0x00447BC8
		public void ShufflePrefabData(int _seed)
		{
			this.prefabManagerData.ShufflePrefabData(_seed);
		}

		// Token: 0x0600B979 RID: 47481 RVA: 0x004499D6 File Offset: 0x00447BD6
		public void Clear()
		{
			this.StreetTilesUsed.Clear();
		}

		// Token: 0x0600B97A RID: 47482 RVA: 0x004499E3 File Offset: 0x00447BE3
		public void ClearDisplayed()
		{
			this.UsedPrefabsWorld.Clear();
			this.WorldUsedPrefabNames.Clear();
		}

		// Token: 0x0600B97B RID: 47483 RVA: 0x004499FB File Offset: 0x00447BFB
		public void Cleanup()
		{
			this.prefabManagerData.Cleanup();
			this.ClearDisplayed();
		}

		// Token: 0x0600B97C RID: 47484 RVA: 0x00449A10 File Offset: 0x00447C10
		public static bool isSizeValid(PrefabData prefab, Vector2i minSize, Vector2i maxSize)
		{
			return (maxSize == default(Vector2i) || (prefab.size.x <= maxSize.x && prefab.size.z <= maxSize.y) || (prefab.size.z <= maxSize.x && prefab.size.x <= maxSize.y)) && (minSize == default(Vector2i) || (prefab.size.x >= minSize.x && prefab.size.z >= minSize.y) || (prefab.size.z >= minSize.x && prefab.size.x >= minSize.y));
		}

		// Token: 0x0600B97D RID: 47485 RVA: 0x00449AE0 File Offset: 0x00447CE0
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

		// Token: 0x0600B97E RID: 47486 RVA: 0x00449BB0 File Offset: 0x00447DB0
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

		// Token: 0x0600B97F RID: 47487 RVA: 0x00449C5C File Offset: 0x00447E5C
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

		// Token: 0x0600B980 RID: 47488 RVA: 0x00449DD0 File Offset: 0x00447FD0
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

		// Token: 0x0600B981 RID: 47489 RVA: 0x00449EF4 File Offset: 0x004480F4
		[PublicizedFrom(EAccessModifier.Private)]
		public static int getRandomVal(int min, int maxExclusive, int seed)
		{
			GameRandom gameRandom = GameRandomManager.Instance.CreateGameRandom(seed);
			int num = gameRandom.RandomRange(min, maxExclusive);
			GameRandomManager.Instance.FreeGameRandom(gameRandom);
			return num;
		}

		// Token: 0x0600B982 RID: 47490 RVA: 0x00449F20 File Offset: 0x00448120
		[PublicizedFrom(EAccessModifier.Private)]
		public static float getRandomVal(float min, float max, int seed)
		{
			GameRandom gameRandom = GameRandomManager.Instance.CreateGameRandom(seed);
			float num = gameRandom.RandomRange(min, max);
			GameRandomManager.Instance.FreeGameRandom(gameRandom);
			return num;
		}

		// Token: 0x0600B983 RID: 47491 RVA: 0x00449F4C File Offset: 0x0044814C
		public PrefabData GetPrefabByName(string _lowerCaseName)
		{
			PrefabData prefabData;
			if (!this.prefabManagerData.AllPrefabDatas.TryGetValue(_lowerCaseName.ToLower(), out prefabData))
			{
				return null;
			}
			return prefabData;
		}

		// Token: 0x0600B984 RID: 47492 RVA: 0x00449F78 File Offset: 0x00448178
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

		// Token: 0x0600B985 RID: 47493 RVA: 0x0044A050 File Offset: 0x00448250
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

		// Token: 0x0600B986 RID: 47494 RVA: 0x0044A170 File Offset: 0x00448370
		public void GetPrefabsAround(Vector3 _position, float _distance, Dictionary<int, PrefabDataInstance> _prefabs)
		{
			for (int i = 0; i < this.UsedPrefabsWorld.Count; i++)
			{
				PrefabDataInstance prefabDataInstance = this.UsedPrefabsWorld[i];
				_prefabs[this.UsedPrefabsWorld[i].id] = this.UsedPrefabsWorld[i];
			}
		}

		// Token: 0x0600B987 RID: 47495 RVA: 0x0044A1C4 File Offset: 0x004483C4
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

		// Token: 0x0600B988 RID: 47496 RVA: 0x0044A37C File Offset: 0x0044857C
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

		// Token: 0x0600B989 RID: 47497 RVA: 0x0044A3B6 File Offset: 0x004485B6
		public void AddUsedPrefabWorld(int townshipID, PrefabDataInstance pdi)
		{
			this.UsedPrefabsWorld.Add(pdi);
			this.AddUsedPrefab(pdi.prefab.Name);
		}

		// Token: 0x04008A5D RID: 35421
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly WorldBuilder worldBuilder;

		// Token: 0x04008A5E RID: 35422
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly PrefabManagerData prefabManagerData = new PrefabManagerData();

		// Token: 0x04008A5F RID: 35423
		public readonly Dictionary<string, int> StreetTilesUsed = new Dictionary<string, int>();

		// Token: 0x04008A60 RID: 35424
		public readonly List<PrefabDataInstance> UsedPrefabsWorld = new List<PrefabDataInstance>();

		// Token: 0x04008A61 RID: 35425
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<string, int> WorldUsedPrefabNames = new Dictionary<string, int>();

		// Token: 0x04008A62 RID: 35426
		public int PrefabInstanceId;

		// Token: 0x02001749 RID: 5961
		public class POIWeightData
		{
			// Token: 0x0600B98A RID: 47498 RVA: 0x0044A3D8 File Offset: 0x004485D8
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

			// Token: 0x04008A63 RID: 35427
			public string PartialPOIName;

			// Token: 0x04008A64 RID: 35428
			public FastTags<TagGroup.Poi> Tags;

			// Token: 0x04008A65 RID: 35429
			public FastTags<TagGroup.Poi> BiomeTags;

			// Token: 0x04008A66 RID: 35430
			public float Weight;

			// Token: 0x04008A67 RID: 35431
			public float Bias;

			// Token: 0x04008A68 RID: 35432
			public int MinCount;

			// Token: 0x04008A69 RID: 35433
			public int MaxCount;
		}
	}
}

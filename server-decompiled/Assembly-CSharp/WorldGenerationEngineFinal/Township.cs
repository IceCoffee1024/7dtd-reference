using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace WorldGenerationEngineFinal
{
	// Token: 0x0200173B RID: 5947
	public class Township
	{
		// Token: 0x0600B8FD RID: 47357 RVA: 0x0044CAF0 File Offset: 0x0044ACF0
		public Township(WorldBuilder _worldBuilder, TownshipData _townshipData)
		{
			this.worldBuilder = _worldBuilder;
			this.townshipData = _townshipData;
		}

		// Token: 0x0600B8FE RID: 47358 RVA: 0x0044CB48 File Offset: 0x0044AD48
		public void Cleanup()
		{
			GameRandomManager.Instance.FreeGameRandom(this.rand);
		}

		// Token: 0x1700167A RID: 5754
		// (get) Token: 0x0600B8FF RID: 47359 RVA: 0x0044CB5A File Offset: 0x0044AD5A
		public TownshipData Data
		{
			get
			{
				return this.townshipData;
			}
		}

		// Token: 0x0600B900 RID: 47360 RVA: 0x0044CB62 File Offset: 0x0044AD62
		public string GetTypeName()
		{
			return this.townshipData.Name;
		}

		// Token: 0x0600B901 RID: 47361 RVA: 0x0044CB6F File Offset: 0x0044AD6F
		public bool IsBig()
		{
			return this.townshipData.Name == "citybig";
		}

		// Token: 0x0600B902 RID: 47362 RVA: 0x0044CB86 File Offset: 0x0044AD86
		public bool IsRoadside()
		{
			return this.townshipData.Category == TownshipData.eCategory.Roadside;
		}

		// Token: 0x0600B903 RID: 47363 RVA: 0x0044CB96 File Offset: 0x0044AD96
		public bool IsRural()
		{
			return this.townshipData.Category == TownshipData.eCategory.Rural;
		}

		// Token: 0x0600B904 RID: 47364 RVA: 0x0044CBA6 File Offset: 0x0044ADA6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsWilderness()
		{
			return this.townshipData.Category == TownshipData.eCategory.Wilderness;
		}

		// Token: 0x0600B905 RID: 47365 RVA: 0x0044CBB6 File Offset: 0x0044ADB6
		public void SortGatewaysClockwise()
		{
			this.Gateways.Sort(delegate(StreetTile _t1, StreetTile _t2)
			{
				float num = Mathf.Atan2((float)(_t1.GridPosition.y - this.GridCenter.y), (float)(_t1.GridPosition.x - this.GridCenter.x));
				float num2 = Mathf.Atan2((float)(_t2.GridPosition.y - this.GridCenter.y), (float)(_t2.GridPosition.x - this.GridCenter.x));
				return num.CompareTo(num2);
			});
		}

		// Token: 0x0600B906 RID: 47366 RVA: 0x0044CBD0 File Offset: 0x0044ADD0
		public void CleanupStreets()
		{
			if (this.Streets == null || this.Streets.Count == 0)
			{
				Log.Error("CleanupStreets none!");
				return;
			}
			this.rand = GameRandomManager.Instance.CreateGameRandom(this.worldBuilder.Seed + this.ID + this.Streets.Count);
			int num = int.MaxValue;
			int num2 = int.MaxValue;
			int num3 = int.MinValue;
			int num4 = int.MinValue;
			foreach (StreetTile streetTile in this.Streets.Values)
			{
				num = Utils.FastMin(num, streetTile.WorldPosition.x);
				num2 = Utils.FastMin(num2, streetTile.WorldPosition.y);
				num3 = Utils.FastMax(num3, streetTile.WorldPositionMax.x);
				num4 = Utils.FastMax(num4, streetTile.WorldPositionMax.y);
			}
			this.Area = new Rect((float)num, (float)num2, (float)(num3 - num), (float)(num4 - num2));
			this.BufferArea = new Rect(this.Area.xMin - 150f, this.Area.yMin - 150f, this.Area.width + 300f, this.Area.height + 300f);
		}

		// Token: 0x0600B907 RID: 47367 RVA: 0x0044CD3C File Offset: 0x0044AF3C
		public void SpawnPrefabs()
		{
			foreach (StreetTile streetTile in this.Streets.Values)
			{
				if (streetTile == null)
				{
					Log.Error("WorldTileData is null, this shouldn't happen!");
				}
				else
				{
					streetTile.SpawnPrefabs();
				}
			}
			this.Prefabs.Clear();
		}

		// Token: 0x0600B908 RID: 47368 RVA: 0x0044CDB0 File Offset: 0x0044AFB0
		public StreetTile CalcCenterStreetTile()
		{
			if (this.centerMostTile != null)
			{
				return this.centerMostTile;
			}
			Vector2i vector2i = Vector2i.zero;
			foreach (StreetTile streetTile in this.Streets.Values)
			{
				vector2i += streetTile.GridPosition;
			}
			vector2i /= this.Streets.Count;
			int num = int.MaxValue;
			StreetTile streetTile2 = null;
			foreach (StreetTile streetTile3 in this.Streets.Values)
			{
				int num2 = Vector2i.DistanceSqrInt(vector2i, streetTile3.GridPosition);
				if (num2 < num)
				{
					num = num2;
					streetTile2 = streetTile3;
				}
			}
			this.centerMostTile = streetTile2;
			float num3 = 0f;
			Vector2i worldPositionCenter = this.centerMostTile.WorldPositionCenter;
			for (int i = -50; i <= 50; i += 50)
			{
				Vector2i vector2i2;
				vector2i2.y = worldPositionCenter.y + i;
				for (int j = -50; j <= 50; j += 50)
				{
					vector2i2.x = worldPositionCenter.x + j;
					num3 += this.worldBuilder.GetHeight(vector2i2);
				}
			}
			this.Height = Mathf.CeilToInt(num3 / 9f);
			this.Height += 3;
			if (this.Streets.Count > 2 && this.Height < 130)
			{
				if (this.BiomeType == BiomeType.snow)
				{
					this.Height += 25;
				}
				else if (this.BiomeType == BiomeType.wasteland)
				{
					this.Height += 12;
				}
			}
			return streetTile2;
		}

		// Token: 0x0600B909 RID: 47369 RVA: 0x0044CF7C File Offset: 0x0044B17C
		public void AddToUsedPOIList(string name)
		{
			this.worldBuilder.PrefabManager.AddUsedPrefab(name);
		}

		// Token: 0x0600B90A RID: 47370 RVA: 0x0044CF90 File Offset: 0x0044B190
		public List<Vector2i> GetUnusedTownExits(int _gatewayUnusedMax = 4)
		{
			this.list.Clear();
			if (this.townshipData.SpawnGateway)
			{
				using (List<StreetTile>.Enumerator enumerator = this.Gateways.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						StreetTile streetTile = enumerator.Current;
						if (streetTile.UsedExitList.Count <= _gatewayUnusedMax)
						{
							foreach (Vector2i vector2i in streetTile.GetHighwayExits(true))
							{
								this.list.Add(vector2i);
							}
						}
					}
					goto IL_0102;
				}
			}
			foreach (StreetTile streetTile2 in this.Streets.Values)
			{
				foreach (Vector2i vector2i2 in streetTile2.GetHighwayExits(false))
				{
					this.list.Add(vector2i2);
				}
			}
			IL_0102:
			return this.list;
		}

		// Token: 0x0600B90B RID: 47371 RVA: 0x0044D0DC File Offset: 0x0044B2DC
		public void AddPrefab(PrefabDataInstance pdi)
		{
			this.Prefabs.Add(pdi);
			this.worldBuilder.PrefabManager.AddUsedPrefabWorld(this.ID, pdi);
		}

		// Token: 0x0600B90C RID: 47372 RVA: 0x0044D104 File Offset: 0x0044B304
		[PublicizedFrom(EAccessModifier.Private)]
		public bool[] GetNeighborExits(StreetTile current)
		{
			bool[] array = new bool[4];
			int num = -1;
			foreach (StreetTile streetTile in current.GetNeighbors())
			{
				num++;
				if (streetTile != null && streetTile.District != null && streetTile.Township != null)
				{
					bool flag = current.District.name == "highway";
					bool flag2 = current.District.name == "gateway";
					bool flag3 = streetTile.District.name == "highway";
					bool flag4 = streetTile.District.name == "gateway";
					if ((streetTile.Township == current.Township || ((flag || flag4 || flag2 || flag3) && (!flag2 || flag3) && (!flag || flag4))) && (streetTile.HasExitTo(current) || (flag && flag3)))
					{
						array[num] = true;
					}
				}
			}
			return array;
		}

		// Token: 0x0600B90D RID: 47373 RVA: 0x0044D200 File Offset: 0x0044B400
		[PublicizedFrom(EAccessModifier.Private)]
		public int GetNeighborCount(Vector2i current)
		{
			int num = 0;
			for (int i = 0; i < this.worldBuilder.TownshipShared.dir4way.Length; i++)
			{
				Vector2i vector2i = current + this.worldBuilder.TownshipShared.dir4way[i];
				if (this.Streets.ContainsKey(vector2i))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600B90E RID: 47374 RVA: 0x0044D25C File Offset: 0x0044B45C
		public override string ToString()
		{
			return string.Format("Township {0}, {1}, {2}", this.townshipData.Name, this.townshipData.Category, this.ID);
		}

		// Token: 0x04008A3E RID: 35390
		[PublicizedFrom(EAccessModifier.Private)]
		public const int BUFFER_DISTANCE = 300;

		// Token: 0x04008A3F RID: 35391
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly WorldBuilder worldBuilder;

		// Token: 0x04008A40 RID: 35392
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly TownshipData townshipData;

		// Token: 0x04008A41 RID: 35393
		public int ID;

		// Token: 0x04008A42 RID: 35394
		public BiomeType BiomeType;

		// Token: 0x04008A43 RID: 35395
		public Rect Area;

		// Token: 0x04008A44 RID: 35396
		public Rect BufferArea;

		// Token: 0x04008A45 RID: 35397
		[PublicizedFrom(EAccessModifier.Private)]
		public StreetTile commercialCap;

		// Token: 0x04008A46 RID: 35398
		[PublicizedFrom(EAccessModifier.Private)]
		public StreetTile ruralCap;

		// Token: 0x04008A47 RID: 35399
		public Vector2i GridCenter;

		// Token: 0x04008A48 RID: 35400
		public int Height;

		// Token: 0x04008A49 RID: 35401
		[PublicizedFrom(EAccessModifier.Private)]
		public StreetTile centerMostTile;

		// Token: 0x04008A4A RID: 35402
		public Dictionary<Vector2i, StreetTile> Streets = new Dictionary<Vector2i, StreetTile>();

		// Token: 0x04008A4B RID: 35403
		public List<PrefabDataInstance> Prefabs = new List<PrefabDataInstance>();

		// Token: 0x04008A4C RID: 35404
		public List<StreetTile> Gateways = new List<StreetTile>();

		// Token: 0x04008A4D RID: 35405
		public Dictionary<Township, int> TownshipConnectionCounts = new Dictionary<Township, int>();

		// Token: 0x04008A4E RID: 35406
		public GameRandom rand;

		// Token: 0x04008A4F RID: 35407
		[PublicizedFrom(EAccessModifier.Private)]
		public List<Vector2i> list = new List<Vector2i>();
	}
}

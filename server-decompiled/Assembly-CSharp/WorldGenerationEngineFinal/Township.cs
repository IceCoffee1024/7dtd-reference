using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace WorldGenerationEngineFinal
{
	// Token: 0x02001767 RID: 5991
	public class Township
	{
		// Token: 0x0600BA54 RID: 47700 RVA: 0x00451010 File Offset: 0x0044F210
		public Township(WorldBuilder _worldBuilder, TownshipData _townshipData)
		{
			this.worldBuilder = _worldBuilder;
			this.townshipData = _townshipData;
		}

		// Token: 0x0600BA55 RID: 47701 RVA: 0x00451068 File Offset: 0x0044F268
		public void Cleanup()
		{
			GameRandomManager.Instance.FreeGameRandom(this.rand);
		}

		// Token: 0x1700171C RID: 5916
		// (get) Token: 0x0600BA56 RID: 47702 RVA: 0x0045107A File Offset: 0x0044F27A
		public TownshipData Data
		{
			get
			{
				return this.townshipData;
			}
		}

		// Token: 0x0600BA57 RID: 47703 RVA: 0x00451082 File Offset: 0x0044F282
		public string GetTypeName()
		{
			return this.townshipData.Name;
		}

		// Token: 0x0600BA58 RID: 47704 RVA: 0x0045108F File Offset: 0x0044F28F
		public bool IsBig()
		{
			return this.townshipData.Name == "citybig";
		}

		// Token: 0x0600BA59 RID: 47705 RVA: 0x004510A6 File Offset: 0x0044F2A6
		public bool IsRoadside()
		{
			return this.townshipData.Category == TownshipData.eCategory.Roadside;
		}

		// Token: 0x0600BA5A RID: 47706 RVA: 0x004510B6 File Offset: 0x0044F2B6
		public bool IsRural()
		{
			return this.townshipData.Category == TownshipData.eCategory.Rural;
		}

		// Token: 0x0600BA5B RID: 47707 RVA: 0x004510C6 File Offset: 0x0044F2C6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsWilderness()
		{
			return this.townshipData.Category == TownshipData.eCategory.Wilderness;
		}

		// Token: 0x0600BA5C RID: 47708 RVA: 0x004510D6 File Offset: 0x0044F2D6
		public void SortGatewaysClockwise()
		{
			this.Gateways.Sort(delegate(StreetTile _t1, StreetTile _t2)
			{
				float num = Mathf.Atan2((float)(_t1.GridPosition.y - this.GridCenter.y), (float)(_t1.GridPosition.x - this.GridCenter.x));
				float num2 = Mathf.Atan2((float)(_t2.GridPosition.y - this.GridCenter.y), (float)(_t2.GridPosition.x - this.GridCenter.x));
				return num.CompareTo(num2);
			});
		}

		// Token: 0x0600BA5D RID: 47709 RVA: 0x004510F0 File Offset: 0x0044F2F0
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

		// Token: 0x0600BA5E RID: 47710 RVA: 0x0045125C File Offset: 0x0044F45C
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

		// Token: 0x0600BA5F RID: 47711 RVA: 0x004512D0 File Offset: 0x0044F4D0
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

		// Token: 0x0600BA60 RID: 47712 RVA: 0x0045149C File Offset: 0x0044F69C
		public void AddToUsedPOIList(string name)
		{
			this.worldBuilder.PrefabManager.AddUsedPrefab(name);
		}

		// Token: 0x0600BA61 RID: 47713 RVA: 0x004514B0 File Offset: 0x0044F6B0
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

		// Token: 0x0600BA62 RID: 47714 RVA: 0x004515FC File Offset: 0x0044F7FC
		public void AddPrefab(PrefabDataInstance pdi)
		{
			this.Prefabs.Add(pdi);
			this.worldBuilder.PrefabManager.AddUsedPrefabWorld(this.ID, pdi);
		}

		// Token: 0x0600BA63 RID: 47715 RVA: 0x00451624 File Offset: 0x0044F824
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

		// Token: 0x0600BA64 RID: 47716 RVA: 0x00451720 File Offset: 0x0044F920
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

		// Token: 0x0600BA65 RID: 47717 RVA: 0x0045177C File Offset: 0x0044F97C
		public override string ToString()
		{
			return string.Format("Township {0}, {1}, {2}", this.townshipData.Name, this.townshipData.Category, this.ID);
		}

		// Token: 0x04008B03 RID: 35587
		[PublicizedFrom(EAccessModifier.Private)]
		public const int BUFFER_DISTANCE = 300;

		// Token: 0x04008B04 RID: 35588
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly WorldBuilder worldBuilder;

		// Token: 0x04008B05 RID: 35589
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly TownshipData townshipData;

		// Token: 0x04008B06 RID: 35590
		public int ID;

		// Token: 0x04008B07 RID: 35591
		public BiomeType BiomeType;

		// Token: 0x04008B08 RID: 35592
		public Rect Area;

		// Token: 0x04008B09 RID: 35593
		public Rect BufferArea;

		// Token: 0x04008B0A RID: 35594
		[PublicizedFrom(EAccessModifier.Private)]
		public StreetTile commercialCap;

		// Token: 0x04008B0B RID: 35595
		[PublicizedFrom(EAccessModifier.Private)]
		public StreetTile ruralCap;

		// Token: 0x04008B0C RID: 35596
		public Vector2i GridCenter;

		// Token: 0x04008B0D RID: 35597
		public int Height;

		// Token: 0x04008B0E RID: 35598
		[PublicizedFrom(EAccessModifier.Private)]
		public StreetTile centerMostTile;

		// Token: 0x04008B0F RID: 35599
		public Dictionary<Vector2i, StreetTile> Streets = new Dictionary<Vector2i, StreetTile>();

		// Token: 0x04008B10 RID: 35600
		public List<PrefabDataInstance> Prefabs = new List<PrefabDataInstance>();

		// Token: 0x04008B11 RID: 35601
		public List<StreetTile> Gateways = new List<StreetTile>();

		// Token: 0x04008B12 RID: 35602
		public Dictionary<Township, int> TownshipConnectionCounts = new Dictionary<Township, int>();

		// Token: 0x04008B13 RID: 35603
		public GameRandom rand;

		// Token: 0x04008B14 RID: 35604
		[PublicizedFrom(EAccessModifier.Private)]
		public List<Vector2i> list = new List<Vector2i>();
	}
}

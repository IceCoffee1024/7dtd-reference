using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace WorldGenerationEngineFinal
{
	// Token: 0x02001798 RID: 6040
	public class Township
	{
		// Token: 0x0600BB68 RID: 47976 RVA: 0x004549BC File Offset: 0x00452BBC
		public Township(WorldBuilder _worldBuilder, TownshipData _townshipData)
		{
			this.worldBuilder = _worldBuilder;
			this.townshipData = _townshipData;
		}

		// Token: 0x0600BB69 RID: 47977 RVA: 0x00454A14 File Offset: 0x00452C14
		public void Cleanup()
		{
			GameRandomManager.Instance.FreeGameRandom(this.rand);
		}

		// Token: 0x17001764 RID: 5988
		// (get) Token: 0x0600BB6A RID: 47978 RVA: 0x00454A26 File Offset: 0x00452C26
		public TownshipData Data
		{
			get
			{
				return this.townshipData;
			}
		}

		// Token: 0x0600BB6B RID: 47979 RVA: 0x00454A2E File Offset: 0x00452C2E
		public string GetTypeName()
		{
			return this.townshipData.Name;
		}

		// Token: 0x0600BB6C RID: 47980 RVA: 0x00454A3B File Offset: 0x00452C3B
		public bool IsBig()
		{
			return this.townshipData.Name == "citybig";
		}

		// Token: 0x0600BB6D RID: 47981 RVA: 0x00454A52 File Offset: 0x00452C52
		public bool IsRoadside()
		{
			return this.townshipData.Category == TownshipData.eCategory.Roadside;
		}

		// Token: 0x0600BB6E RID: 47982 RVA: 0x00454A62 File Offset: 0x00452C62
		public bool IsRural()
		{
			return this.townshipData.Category == TownshipData.eCategory.Rural;
		}

		// Token: 0x0600BB6F RID: 47983 RVA: 0x00454A72 File Offset: 0x00452C72
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsWilderness()
		{
			return this.townshipData.Category == TownshipData.eCategory.Wilderness;
		}

		// Token: 0x0600BB70 RID: 47984 RVA: 0x00454A82 File Offset: 0x00452C82
		public void SortGatewaysClockwise()
		{
			this.Gateways.Sort(delegate(StreetTile _t1, StreetTile _t2)
			{
				float num = Mathf.Atan2((float)(_t1.GridPosition.y - this.GridCenter.y), (float)(_t1.GridPosition.x - this.GridCenter.x));
				float num2 = Mathf.Atan2((float)(_t2.GridPosition.y - this.GridCenter.y), (float)(_t2.GridPosition.x - this.GridCenter.x));
				return num.CompareTo(num2);
			});
		}

		// Token: 0x0600BB71 RID: 47985 RVA: 0x00454A9C File Offset: 0x00452C9C
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

		// Token: 0x0600BB72 RID: 47986 RVA: 0x00454C08 File Offset: 0x00452E08
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

		// Token: 0x0600BB73 RID: 47987 RVA: 0x00454C7C File Offset: 0x00452E7C
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

		// Token: 0x0600BB74 RID: 47988 RVA: 0x00454E48 File Offset: 0x00453048
		public void AddToUsedPOIList(string name)
		{
			this.worldBuilder.PrefabManager.AddUsedPrefab(name);
		}

		// Token: 0x0600BB75 RID: 47989 RVA: 0x00454E5C File Offset: 0x0045305C
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

		// Token: 0x0600BB76 RID: 47990 RVA: 0x00454FA8 File Offset: 0x004531A8
		public void AddPrefab(PrefabDataInstance pdi)
		{
			this.Prefabs.Add(pdi);
			this.worldBuilder.PrefabManager.AddUsedPrefabWorld(this.ID, pdi);
		}

		// Token: 0x0600BB77 RID: 47991 RVA: 0x00454FD0 File Offset: 0x004531D0
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

		// Token: 0x0600BB78 RID: 47992 RVA: 0x004550CC File Offset: 0x004532CC
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

		// Token: 0x0600BB79 RID: 47993 RVA: 0x00455128 File Offset: 0x00453328
		public override string ToString()
		{
			return string.Format("Township {0}, {1}, {2}", this.townshipData.Name, this.townshipData.Category, this.ID);
		}

		// Token: 0x04008BEC RID: 35820
		[PublicizedFrom(EAccessModifier.Private)]
		public const int BUFFER_DISTANCE = 300;

		// Token: 0x04008BED RID: 35821
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly WorldBuilder worldBuilder;

		// Token: 0x04008BEE RID: 35822
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly TownshipData townshipData;

		// Token: 0x04008BEF RID: 35823
		public int ID;

		// Token: 0x04008BF0 RID: 35824
		public BiomeType BiomeType;

		// Token: 0x04008BF1 RID: 35825
		public Rect Area;

		// Token: 0x04008BF2 RID: 35826
		public Rect BufferArea;

		// Token: 0x04008BF3 RID: 35827
		[PublicizedFrom(EAccessModifier.Private)]
		public StreetTile commercialCap;

		// Token: 0x04008BF4 RID: 35828
		[PublicizedFrom(EAccessModifier.Private)]
		public StreetTile ruralCap;

		// Token: 0x04008BF5 RID: 35829
		public Vector2i GridCenter;

		// Token: 0x04008BF6 RID: 35830
		public int Height;

		// Token: 0x04008BF7 RID: 35831
		[PublicizedFrom(EAccessModifier.Private)]
		public StreetTile centerMostTile;

		// Token: 0x04008BF8 RID: 35832
		public Dictionary<Vector2i, StreetTile> Streets = new Dictionary<Vector2i, StreetTile>();

		// Token: 0x04008BF9 RID: 35833
		public List<PrefabDataInstance> Prefabs = new List<PrefabDataInstance>();

		// Token: 0x04008BFA RID: 35834
		public List<StreetTile> Gateways = new List<StreetTile>();

		// Token: 0x04008BFB RID: 35835
		public Dictionary<Township, int> TownshipConnectionCounts = new Dictionary<Township, int>();

		// Token: 0x04008BFC RID: 35836
		public GameRandom rand;

		// Token: 0x04008BFD RID: 35837
		[PublicizedFrom(EAccessModifier.Private)]
		public List<Vector2i> list = new List<Vector2i>();
	}
}

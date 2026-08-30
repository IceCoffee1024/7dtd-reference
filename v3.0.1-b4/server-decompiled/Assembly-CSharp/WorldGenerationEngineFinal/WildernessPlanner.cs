using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

namespace WorldGenerationEngineFinal
{
	// Token: 0x02001717 RID: 5911
	public class WildernessPlanner
	{
		// Token: 0x0600B78F RID: 46991 RVA: 0x004475F7 File Offset: 0x004457F7
		public WildernessPlanner(WorldBuilder _worldBuilder)
		{
			this.worldBuilder = _worldBuilder;
		}

		// Token: 0x0600B790 RID: 46992 RVA: 0x00447611 File Offset: 0x00445811
		public void AddPathInfo(Vector2i _startPos, float _pathRadius)
		{
			this.WildernessPathInfos.Add(new WildernessPlanner.WildernessPathInfo(new Vector2i(_startPos), _pathRadius));
		}

		// Token: 0x0600B791 RID: 46993 RVA: 0x00447630 File Offset: 0x00445830
		public void Plan(DynamicProperties thisWorldProperties, int worldSeed)
		{
			MicroStopwatch microStopwatch = new MicroStopwatch(true);
			this.WildernessPathInfos.Clear();
			int num = 0;
			List<StreetTile> list = new List<StreetTile>(200);
			int num2 = worldSeed + 409651;
			GameRandom gameRandom = GameRandomManager.Instance.CreateGameRandom(num2);
			for (int i = 0; i < 5; i++)
			{
				int num3 = this.worldBuilder.GetCount(((BiomeType)i).ToString() + "_wilderness", this.worldBuilder.Wilderness, null);
				if (num3 < 0)
				{
					num3 = this.worldBuilder.GetCount("wilderness", this.worldBuilder.Wilderness, null);
				}
				int j = num3;
				if (j < 0)
				{
					j = 200;
					Log.Warning("No wilderness settings in rwgmixer for this world size, using default count of {0}", new object[] { j });
				}
				int num4 = j;
				List<StreetTile> unusedWildernessTiles = this.GetUnusedWildernessTiles((BiomeType)i);
				while (j > 0)
				{
					this.GetUnusedWildernessTiles(unusedWildernessTiles, list);
					if (list.Count == 0)
					{
						break;
					}
					this.worldBuilder.SetTaskMessage(string.Format(this.worldBuilder.messageWildernessPOIs, Mathf.FloorToInt(100f * (1f - (float)j / (float)num4))));
					for (int k = 0; k < 20; k++)
					{
						StreetTile streetTile = list[WildernessPlanner.GetLowBiasedRandom(gameRandom, list.Count)];
						if (!streetTile.Used && streetTile.SpawnPrefabs())
						{
							streetTile.Used = true;
							break;
						}
						num++;
					}
					j--;
				}
			}
			GameRandomManager.Instance.FreeGameRandom(gameRandom);
			Log.Out(string.Format("WildernessPlanner Plan {0} prefabs spawned, in {1}, retries {2}, r={3:x}", new object[]
			{
				this.worldBuilder.WildernessPrefabCount,
				(float)microStopwatch.ElapsedMilliseconds * 0.001f,
				num,
				Rand.Instance.PeekSample()
			}));
		}

		// Token: 0x0600B792 RID: 46994 RVA: 0x00447821 File Offset: 0x00445A21
		[PublicizedFrom(EAccessModifier.Private)]
		public static int GetLowBiasedRandom(GameRandom rnd, int max)
		{
			float randomFloat = rnd.RandomFloat;
			return (int)(randomFloat * randomFloat * (float)max);
		}

		// Token: 0x0600B793 RID: 46995 RVA: 0x00447830 File Offset: 0x00445A30
		[PublicizedFrom(EAccessModifier.Private)]
		public List<StreetTile> GetUnusedWildernessTiles(BiomeType _biome)
		{
			return (from StreetTile st in this.worldBuilder.StreetTileMap
				where !st.OverlapsRadiation && !st.AllIsWater && (st.District == null || st.District.name == "wilderness") && !st.Used && st.BiomeType == _biome
				select st).ToList<StreetTile>();
		}

		// Token: 0x0600B794 RID: 46996 RVA: 0x00447870 File Offset: 0x00445A70
		[PublicizedFrom(EAccessModifier.Private)]
		public void GetUnusedWildernessTiles(List<StreetTile> _list, List<StreetTile> _resultList)
		{
			IOrderedEnumerable<StreetTile> orderedEnumerable = from StreetTile st in _list
				where !st.Used
				orderby this.distanceFromClosestTownship(st) descending
				select st;
			_resultList.Clear();
			_resultList.AddRange(orderedEnumerable);
		}

		// Token: 0x0600B795 RID: 46997 RVA: 0x004478C8 File Offset: 0x00445AC8
		[PublicizedFrom(EAccessModifier.Private)]
		public int distanceFromClosestTownship(StreetTile st)
		{
			int num = int.MaxValue;
			foreach (Township township in this.worldBuilder.Townships)
			{
				int num2 = Vector2i.DistanceSqrInt(st.WorldPositionCenter, this.worldBuilder.GetStreetTileGrid(township.GridCenter).WorldPositionCenter);
				if (num2 < num)
				{
					num = num2;
				}
			}
			return num;
		}

		// Token: 0x0600B796 RID: 46998 RVA: 0x00447948 File Offset: 0x00445B48
		[PublicizedFrom(EAccessModifier.Private)]
		public static float distanceSqr(Vector2 pointA, Vector2 pointB)
		{
			Vector2 vector = pointA - pointB;
			return vector.x * vector.x + vector.y * vector.y;
		}

		// Token: 0x04008938 RID: 35128
		[PublicizedFrom(EAccessModifier.Private)]
		public const int cWildernessSpawnTries = 20;

		// Token: 0x04008939 RID: 35129
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly WorldBuilder worldBuilder;

		// Token: 0x0400893A RID: 35130
		public readonly List<WildernessPlanner.WildernessPathInfo> WildernessPathInfos = new List<WildernessPlanner.WildernessPathInfo>();

		// Token: 0x02001718 RID: 5912
		public class WildernessPathInfo
		{
			// Token: 0x0600B798 RID: 47000 RVA: 0x00447981 File Offset: 0x00445B81
			public WildernessPathInfo(Vector2i _startPos, float _pathRadius)
			{
				this.Position = _startPos;
				this.PathRadius = _pathRadius;
			}

			// Token: 0x0400893B RID: 35131
			public Vector2i Position;

			// Token: 0x0400893C RID: 35132
			public float PathRadius;

			// Token: 0x0400893D RID: 35133
			public Path Path;

			// Token: 0x0400893E RID: 35134
			public float highwayDistance;

			// Token: 0x0400893F RID: 35135
			public Vector2 highwayPoint;
		}
	}
}

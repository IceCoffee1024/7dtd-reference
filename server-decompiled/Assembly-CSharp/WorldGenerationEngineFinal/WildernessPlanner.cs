using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

namespace WorldGenerationEngineFinal
{
	// Token: 0x02001769 RID: 5993
	public class WildernessPlanner
	{
		// Token: 0x0600BA69 RID: 47721 RVA: 0x0045188F File Offset: 0x0044FA8F
		public WildernessPlanner(WorldBuilder _worldBuilder)
		{
			this.worldBuilder = _worldBuilder;
		}

		// Token: 0x0600BA6A RID: 47722 RVA: 0x004518A9 File Offset: 0x0044FAA9
		public void AddPathInfo(Vector2i _startPos, float _pathRadius)
		{
			this.WildernessPathInfos.Add(new WildernessPlanner.WildernessPathInfo(new Vector2i(_startPos), _pathRadius));
		}

		// Token: 0x0600BA6B RID: 47723 RVA: 0x004518C8 File Offset: 0x0044FAC8
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

		// Token: 0x0600BA6C RID: 47724 RVA: 0x00451AB9 File Offset: 0x0044FCB9
		[PublicizedFrom(EAccessModifier.Private)]
		public static int GetLowBiasedRandom(GameRandom rnd, int max)
		{
			float randomFloat = rnd.RandomFloat;
			return (int)(randomFloat * randomFloat * (float)max);
		}

		// Token: 0x0600BA6D RID: 47725 RVA: 0x00451AC8 File Offset: 0x0044FCC8
		[PublicizedFrom(EAccessModifier.Private)]
		public List<StreetTile> GetUnusedWildernessTiles(BiomeType _biome)
		{
			return (from StreetTile st in this.worldBuilder.StreetTileMap
				where !st.OverlapsRadiation && !st.AllIsWater && (st.District == null || st.District.name == "wilderness") && !st.Used && st.BiomeType == _biome
				select st).ToList<StreetTile>();
		}

		// Token: 0x0600BA6E RID: 47726 RVA: 0x00451B08 File Offset: 0x0044FD08
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

		// Token: 0x0600BA6F RID: 47727 RVA: 0x00451B60 File Offset: 0x0044FD60
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

		// Token: 0x0600BA70 RID: 47728 RVA: 0x00451BE0 File Offset: 0x0044FDE0
		[PublicizedFrom(EAccessModifier.Private)]
		public static float distanceSqr(Vector2 pointA, Vector2 pointB)
		{
			Vector2 vector = pointA - pointB;
			return vector.x * vector.x + vector.y * vector.y;
		}

		// Token: 0x04008B19 RID: 35609
		[PublicizedFrom(EAccessModifier.Private)]
		public const int cWildernessSpawnTries = 20;

		// Token: 0x04008B1A RID: 35610
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly WorldBuilder worldBuilder;

		// Token: 0x04008B1B RID: 35611
		public readonly List<WildernessPlanner.WildernessPathInfo> WildernessPathInfos = new List<WildernessPlanner.WildernessPathInfo>();

		// Token: 0x0200176A RID: 5994
		public class WildernessPathInfo
		{
			// Token: 0x0600BA72 RID: 47730 RVA: 0x00451C19 File Offset: 0x0044FE19
			public WildernessPathInfo(Vector2i _startPos, float _pathRadius)
			{
				this.Position = _startPos;
				this.PathRadius = _pathRadius;
			}

			// Token: 0x04008B1C RID: 35612
			public Vector2i Position;

			// Token: 0x04008B1D RID: 35613
			public float PathRadius;

			// Token: 0x04008B1E RID: 35614
			public Path Path;

			// Token: 0x04008B1F RID: 35615
			public float highwayDistance;

			// Token: 0x04008B20 RID: 35616
			public Vector2 highwayPoint;
		}
	}
}

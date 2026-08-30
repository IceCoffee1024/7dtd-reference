using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

namespace WorldGenerationEngineFinal
{
	// Token: 0x0200173D RID: 5949
	public class WildernessPlanner
	{
		// Token: 0x0600B912 RID: 47378 RVA: 0x0044D36F File Offset: 0x0044B56F
		public WildernessPlanner(WorldBuilder _worldBuilder)
		{
			this.worldBuilder = _worldBuilder;
		}

		// Token: 0x0600B913 RID: 47379 RVA: 0x0044D389 File Offset: 0x0044B589
		public void AddPathInfo(Vector2i _startPos, float _pathRadius)
		{
			this.WildernessPathInfos.Add(new WildernessPlanner.WildernessPathInfo(new Vector2i(_startPos), _pathRadius));
		}

		// Token: 0x0600B914 RID: 47380 RVA: 0x0044D3A8 File Offset: 0x0044B5A8
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

		// Token: 0x0600B915 RID: 47381 RVA: 0x0044D599 File Offset: 0x0044B799
		[PublicizedFrom(EAccessModifier.Private)]
		public static int GetLowBiasedRandom(GameRandom rnd, int max)
		{
			float randomFloat = rnd.RandomFloat;
			return (int)(randomFloat * randomFloat * (float)max);
		}

		// Token: 0x0600B916 RID: 47382 RVA: 0x0044D5A8 File Offset: 0x0044B7A8
		[PublicizedFrom(EAccessModifier.Private)]
		public List<StreetTile> GetUnusedWildernessTiles(BiomeType _biome)
		{
			return (from StreetTile st in this.worldBuilder.StreetTileMap
				where !st.OverlapsRadiation && !st.AllIsWater && (st.District == null || st.District.name == "wilderness") && !st.Used && st.BiomeType == _biome
				select st).ToList<StreetTile>();
		}

		// Token: 0x0600B917 RID: 47383 RVA: 0x0044D5E8 File Offset: 0x0044B7E8
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

		// Token: 0x0600B918 RID: 47384 RVA: 0x0044D640 File Offset: 0x0044B840
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

		// Token: 0x0600B919 RID: 47385 RVA: 0x0044D6C0 File Offset: 0x0044B8C0
		[PublicizedFrom(EAccessModifier.Private)]
		public static float distanceSqr(Vector2 pointA, Vector2 pointB)
		{
			Vector2 vector = pointA - pointB;
			return vector.x * vector.x + vector.y * vector.y;
		}

		// Token: 0x04008A54 RID: 35412
		[PublicizedFrom(EAccessModifier.Private)]
		public const int cWildernessSpawnTries = 20;

		// Token: 0x04008A55 RID: 35413
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly WorldBuilder worldBuilder;

		// Token: 0x04008A56 RID: 35414
		public readonly List<WildernessPlanner.WildernessPathInfo> WildernessPathInfos = new List<WildernessPlanner.WildernessPathInfo>();

		// Token: 0x0200173E RID: 5950
		public class WildernessPathInfo
		{
			// Token: 0x0600B91B RID: 47387 RVA: 0x0044D6F9 File Offset: 0x0044B8F9
			public WildernessPathInfo(Vector2i _startPos, float _pathRadius)
			{
				this.Position = _startPos;
				this.PathRadius = _pathRadius;
			}

			// Token: 0x04008A57 RID: 35415
			public Vector2i Position;

			// Token: 0x04008A58 RID: 35416
			public float PathRadius;

			// Token: 0x04008A59 RID: 35417
			public Path Path;

			// Token: 0x04008A5A RID: 35418
			public float highwayDistance;

			// Token: 0x04008A5B RID: 35419
			public Vector2 highwayPoint;
		}
	}
}

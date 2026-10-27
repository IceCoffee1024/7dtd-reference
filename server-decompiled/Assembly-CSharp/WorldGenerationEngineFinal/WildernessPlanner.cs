using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

namespace WorldGenerationEngineFinal
{
	// Token: 0x0200179A RID: 6042
	public class WildernessPlanner
	{
		// Token: 0x0600BB7D RID: 47997 RVA: 0x0045523B File Offset: 0x0045343B
		public WildernessPlanner(WorldBuilder _worldBuilder)
		{
			this.worldBuilder = _worldBuilder;
		}

		// Token: 0x0600BB7E RID: 47998 RVA: 0x00455255 File Offset: 0x00453455
		public void AddPathInfo(Vector2i _startPos, float _pathRadius)
		{
			this.WildernessPathInfos.Add(new WildernessPlanner.WildernessPathInfo(new Vector2i(_startPos), _pathRadius));
		}

		// Token: 0x0600BB7F RID: 47999 RVA: 0x00455274 File Offset: 0x00453474
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

		// Token: 0x0600BB80 RID: 48000 RVA: 0x00455465 File Offset: 0x00453665
		[PublicizedFrom(EAccessModifier.Private)]
		public static int GetLowBiasedRandom(GameRandom rnd, int max)
		{
			float randomFloat = rnd.RandomFloat;
			return (int)(randomFloat * randomFloat * (float)max);
		}

		// Token: 0x0600BB81 RID: 48001 RVA: 0x00455474 File Offset: 0x00453674
		[PublicizedFrom(EAccessModifier.Private)]
		public List<StreetTile> GetUnusedWildernessTiles(BiomeType _biome)
		{
			return (from StreetTile st in this.worldBuilder.StreetTileMap
				where !st.OverlapsRadiation && !st.AllIsWater && (st.District == null || st.District.name == "wilderness") && !st.Used && st.BiomeType == _biome
				select st).ToList<StreetTile>();
		}

		// Token: 0x0600BB82 RID: 48002 RVA: 0x004554B4 File Offset: 0x004536B4
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

		// Token: 0x0600BB83 RID: 48003 RVA: 0x0045550C File Offset: 0x0045370C
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

		// Token: 0x0600BB84 RID: 48004 RVA: 0x0045558C File Offset: 0x0045378C
		[PublicizedFrom(EAccessModifier.Private)]
		public static float distanceSqr(Vector2 pointA, Vector2 pointB)
		{
			Vector2 vector = pointA - pointB;
			return vector.x * vector.x + vector.y * vector.y;
		}

		// Token: 0x04008C02 RID: 35842
		[PublicizedFrom(EAccessModifier.Private)]
		public const int cWildernessSpawnTries = 20;

		// Token: 0x04008C03 RID: 35843
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly WorldBuilder worldBuilder;

		// Token: 0x04008C04 RID: 35844
		public readonly List<WildernessPlanner.WildernessPathInfo> WildernessPathInfos = new List<WildernessPlanner.WildernessPathInfo>();

		// Token: 0x0200179B RID: 6043
		public class WildernessPathInfo
		{
			// Token: 0x0600BB86 RID: 48006 RVA: 0x004555C5 File Offset: 0x004537C5
			public WildernessPathInfo(Vector2i _startPos, float _pathRadius)
			{
				this.Position = _startPos;
				this.PathRadius = _pathRadius;
			}

			// Token: 0x04008C05 RID: 35845
			public Vector2i Position;

			// Token: 0x04008C06 RID: 35846
			public float PathRadius;

			// Token: 0x04008C07 RID: 35847
			public Path Path;

			// Token: 0x04008C08 RID: 35848
			public float highwayDistance;

			// Token: 0x04008C09 RID: 35849
			public Vector2 highwayPoint;
		}
	}
}

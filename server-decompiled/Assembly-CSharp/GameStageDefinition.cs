using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000AD7 RID: 2775
public sealed class GameStageDefinition
{
	// Token: 0x060052CE RID: 21198 RVA: 0x001FAD13 File Offset: 0x001F8F13
	public GameStageDefinition(string _name)
	{
		this.name = _name;
	}

	// Token: 0x060052CF RID: 21199 RVA: 0x001FAD2D File Offset: 0x001F8F2D
	public static void AddGameStage(GameStageDefinition gameStage)
	{
		gameStage.SortStages();
		GameStageDefinition.gameStages.Add(gameStage.name, gameStage);
	}

	// Token: 0x060052D0 RID: 21200 RVA: 0x001FAD46 File Offset: 0x001F8F46
	public static GameStageDefinition GetGameStage(string name)
	{
		return GameStageDefinition.gameStages[name];
	}

	// Token: 0x060052D1 RID: 21201 RVA: 0x001FAD53 File Offset: 0x001F8F53
	public static bool TryGetGameStage(string name, out GameStageDefinition definition)
	{
		return GameStageDefinition.gameStages.TryGetValue(name, out definition);
	}

	// Token: 0x060052D2 RID: 21202 RVA: 0x001FAD61 File Offset: 0x001F8F61
	public static void Clear()
	{
		GameStageDefinition.gameStages.Clear();
	}

	// Token: 0x060052D3 RID: 21203 RVA: 0x001FAD6D File Offset: 0x001F8F6D
	public void AddStage(GameStageDefinition.Stage stage)
	{
		this.stages.Add(stage);
	}

	// Token: 0x060052D4 RID: 21204 RVA: 0x001FAD7B File Offset: 0x001F8F7B
	public void SortStages()
	{
		this.stages.Sort((GameStageDefinition.Stage x, GameStageDefinition.Stage y) => x.stageNum.CompareTo(y.stageNum));
	}

	// Token: 0x060052D5 RID: 21205 RVA: 0x001FADA8 File Offset: 0x001F8FA8
	public GameStageDefinition.Stage GetStage(int stage)
	{
		if (this.stages.Count < 1)
		{
			return null;
		}
		if (stage < this.stages[0].stageNum)
		{
			return null;
		}
		int num = GameStageDefinition.GetBoundIndex<GameStageDefinition.Stage>(this.stages, (GameStageDefinition.Stage s) => s.stageNum <= stage);
		num = Mathf.Clamp(num, 0, this.stages.Count - 1);
		return this.stages[num];
	}

	// Token: 0x060052D6 RID: 21206 RVA: 0x001FAE28 File Offset: 0x001F9028
	public static int GetBoundIndex<T>(IList<T> sortedList, Func<T, bool> f)
	{
		int num = 0;
		int num2 = sortedList.Count;
		while (num + 1 < num2)
		{
			int num3 = (num + num2) / 2;
			if (f(sortedList[num3]))
			{
				num = num3;
			}
			else
			{
				num2 = num3;
			}
		}
		if (num2 < sortedList.Count && f(sortedList[num2]))
		{
			num = num2;
		}
		return num;
	}

	// Token: 0x060052D7 RID: 21207 RVA: 0x001FAE7C File Offset: 0x001F907C
	public static int CalcPartyLevel(List<int> playerGameStages)
	{
		float num = 0f;
		playerGameStages.Sort();
		float num2 = GameStageDefinition.StartingWeight;
		for (int i = playerGameStages.Count - 1; i >= 0; i--)
		{
			num += (float)playerGameStages[i] * num2;
			num2 *= GameStageDefinition.DiminishingReturns;
		}
		return Mathf.FloorToInt(num);
	}

	// Token: 0x060052D8 RID: 21208 RVA: 0x001FAECC File Offset: 0x001F90CC
	public static int CalcGameStageAround(EntityPlayer player)
	{
		List<EntityPlayer> list = new List<EntityPlayer>();
		player.world.GetPlayersAround(player.position, 100f, list);
		List<int> list2 = new List<int>();
		for (int i = 0; i < list.Count; i++)
		{
			EntityPlayer entityPlayer = list[i];
			if (entityPlayer.prefab == player.prefab)
			{
				list2.Add(entityPlayer.gameStage);
			}
		}
		return GameStageDefinition.CalcPartyLevel(list2);
	}

	// Token: 0x04004087 RID: 16519
	public static float DifficultyBonus = 1f;

	// Token: 0x04004088 RID: 16520
	public static float StartingWeight = 1f;

	// Token: 0x04004089 RID: 16521
	public static float DiminishingReturns = 0.5f;

	// Token: 0x0400408A RID: 16522
	public static long DaysAliveChangeWhenKilled = 2L;

	// Token: 0x0400408B RID: 16523
	public static int LootBonusEvery;

	// Token: 0x0400408C RID: 16524
	public static int LootBonusMaxCount;

	// Token: 0x0400408D RID: 16525
	public static float LootBonusScale;

	// Token: 0x0400408E RID: 16526
	public static int LootWanderingBonusEvery;

	// Token: 0x0400408F RID: 16527
	public static float LootWanderingBonusScale;

	// Token: 0x04004090 RID: 16528
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<string, GameStageDefinition> gameStages = new Dictionary<string, GameStageDefinition>();

	// Token: 0x04004091 RID: 16529
	public string name;

	// Token: 0x04004092 RID: 16530
	[PublicizedFrom(EAccessModifier.Private)]
	public List<GameStageDefinition.Stage> stages = new List<GameStageDefinition.Stage>();

	// Token: 0x02000AD8 RID: 2776
	public class Stage
	{
		// Token: 0x060052DA RID: 21210 RVA: 0x001FAF66 File Offset: 0x001F9166
		public Stage(int _stageNum)
		{
			this.stageNum = _stageNum;
		}

		// Token: 0x060052DB RID: 21211 RVA: 0x001FAF80 File Offset: 0x001F9180
		public void AddSpawnGroup(GameStageDefinition.SpawnGroup spawn)
		{
			this.spawnGroups.Add(spawn);
		}

		// Token: 0x060052DC RID: 21212 RVA: 0x001FAF8E File Offset: 0x001F918E
		public GameStageDefinition.SpawnGroup GetSpawnGroup(int index)
		{
			if (index >= 0 && index < this.spawnGroups.Count)
			{
				return this.spawnGroups[index];
			}
			return null;
		}

		// Token: 0x170008F6 RID: 2294
		// (get) Token: 0x060052DD RID: 21213 RVA: 0x001FAFB0 File Offset: 0x001F91B0
		public int Count
		{
			get
			{
				return this.spawnGroups.Count;
			}
		}

		// Token: 0x04004093 RID: 16531
		public readonly int stageNum;

		// Token: 0x04004094 RID: 16532
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<GameStageDefinition.SpawnGroup> spawnGroups = new List<GameStageDefinition.SpawnGroup>();
	}

	// Token: 0x02000AD9 RID: 2777
	public class SpawnGroup
	{
		// Token: 0x060052DE RID: 21214 RVA: 0x001FAFBD File Offset: 0x001F91BD
		public SpawnGroup(string _groupName, int _spawnCount, int _maxAlive, int _interval, int _duration)
		{
			this.groupName = _groupName;
			this.spawnCount = (ushort)_spawnCount;
			this.maxAlive = (ushort)_maxAlive;
			this.interval = (ushort)_interval;
			this.duration = (ushort)_duration;
		}

		// Token: 0x04004095 RID: 16533
		public readonly string groupName;

		// Token: 0x04004096 RID: 16534
		public readonly ushort spawnCount;

		// Token: 0x04004097 RID: 16535
		public readonly ushort maxAlive;

		// Token: 0x04004098 RID: 16536
		public readonly ushort interval;

		// Token: 0x04004099 RID: 16537
		public readonly ushort duration;
	}
}

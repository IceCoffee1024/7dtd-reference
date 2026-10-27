using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B06 RID: 2822
public sealed class GameStageDefinition
{
	// Token: 0x06005394 RID: 21396 RVA: 0x00202167 File Offset: 0x00200367
	public GameStageDefinition(string _name)
	{
		this.name = _name;
	}

	// Token: 0x06005395 RID: 21397 RVA: 0x00202181 File Offset: 0x00200381
	public static void AddGameStage(GameStageDefinition gameStage)
	{
		gameStage.SortStages();
		GameStageDefinition.gameStages.Add(gameStage.name, gameStage);
	}

	// Token: 0x06005396 RID: 21398 RVA: 0x0020219A File Offset: 0x0020039A
	public static GameStageDefinition GetGameStage(string name)
	{
		return GameStageDefinition.gameStages[name];
	}

	// Token: 0x06005397 RID: 21399 RVA: 0x002021A7 File Offset: 0x002003A7
	public static bool TryGetGameStage(string name, out GameStageDefinition definition)
	{
		return GameStageDefinition.gameStages.TryGetValue(name, out definition);
	}

	// Token: 0x06005398 RID: 21400 RVA: 0x002021B5 File Offset: 0x002003B5
	public static void Clear()
	{
		GameStageDefinition.gameStages.Clear();
	}

	// Token: 0x06005399 RID: 21401 RVA: 0x002021C1 File Offset: 0x002003C1
	public void AddStage(GameStageDefinition.Stage stage)
	{
		this.stages.Add(stage);
	}

	// Token: 0x0600539A RID: 21402 RVA: 0x002021CF File Offset: 0x002003CF
	public void SortStages()
	{
		this.stages.Sort((GameStageDefinition.Stage x, GameStageDefinition.Stage y) => x.stageNum.CompareTo(y.stageNum));
	}

	// Token: 0x0600539B RID: 21403 RVA: 0x002021FC File Offset: 0x002003FC
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

	// Token: 0x0600539C RID: 21404 RVA: 0x0020227C File Offset: 0x0020047C
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

	// Token: 0x0600539D RID: 21405 RVA: 0x002022D0 File Offset: 0x002004D0
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

	// Token: 0x0600539E RID: 21406 RVA: 0x00202320 File Offset: 0x00200520
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

	// Token: 0x0400417F RID: 16767
	public static float DifficultyBonus = 1f;

	// Token: 0x04004180 RID: 16768
	public static float StartingWeight = 1f;

	// Token: 0x04004181 RID: 16769
	public static float DiminishingReturns = 0.5f;

	// Token: 0x04004182 RID: 16770
	public static long DaysAliveChangeWhenKilled = 2L;

	// Token: 0x04004183 RID: 16771
	public static int LootBonusEvery;

	// Token: 0x04004184 RID: 16772
	public static int LootBonusMaxCount;

	// Token: 0x04004185 RID: 16773
	public static float LootBonusScale;

	// Token: 0x04004186 RID: 16774
	public static int LootWanderingBonusEvery;

	// Token: 0x04004187 RID: 16775
	public static float LootWanderingBonusScale;

	// Token: 0x04004188 RID: 16776
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<string, GameStageDefinition> gameStages = new Dictionary<string, GameStageDefinition>();

	// Token: 0x04004189 RID: 16777
	public string name;

	// Token: 0x0400418A RID: 16778
	[PublicizedFrom(EAccessModifier.Private)]
	public List<GameStageDefinition.Stage> stages = new List<GameStageDefinition.Stage>();

	// Token: 0x02000B07 RID: 2823
	public class Stage
	{
		// Token: 0x060053A0 RID: 21408 RVA: 0x002023BA File Offset: 0x002005BA
		public Stage(int _stageNum)
		{
			this.stageNum = _stageNum;
		}

		// Token: 0x060053A1 RID: 21409 RVA: 0x002023D4 File Offset: 0x002005D4
		public void AddSpawnGroup(GameStageDefinition.SpawnGroup spawn)
		{
			this.spawnGroups.Add(spawn);
		}

		// Token: 0x060053A2 RID: 21410 RVA: 0x002023E2 File Offset: 0x002005E2
		public GameStageDefinition.SpawnGroup GetSpawnGroup(int index)
		{
			if (index >= 0 && index < this.spawnGroups.Count)
			{
				return this.spawnGroups[index];
			}
			return null;
		}

		// Token: 0x17000930 RID: 2352
		// (get) Token: 0x060053A3 RID: 21411 RVA: 0x00202404 File Offset: 0x00200604
		public int Count
		{
			get
			{
				return this.spawnGroups.Count;
			}
		}

		// Token: 0x0400418B RID: 16779
		public readonly int stageNum;

		// Token: 0x0400418C RID: 16780
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<GameStageDefinition.SpawnGroup> spawnGroups = new List<GameStageDefinition.SpawnGroup>();
	}

	// Token: 0x02000B08 RID: 2824
	public class SpawnGroup
	{
		// Token: 0x060053A4 RID: 21412 RVA: 0x00202411 File Offset: 0x00200611
		public SpawnGroup(string _groupName, int _spawnCount, int _maxAlive, int _interval, int _duration)
		{
			this.groupName = _groupName;
			this.spawnCount = (ushort)_spawnCount;
			this.maxAlive = (ushort)_maxAlive;
			this.interval = (ushort)_interval;
			this.duration = (ushort)_duration;
		}

		// Token: 0x0400418D RID: 16781
		public readonly string groupName;

		// Token: 0x0400418E RID: 16782
		public readonly ushort spawnCount;

		// Token: 0x0400418F RID: 16783
		public readonly ushort maxAlive;

		// Token: 0x04004190 RID: 16784
		public readonly ushort interval;

		// Token: 0x04004191 RID: 16785
		public readonly ushort duration;
	}
}

using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000AFB RID: 2811
public sealed class GameStageDefinition
{
	// Token: 0x0600536F RID: 21359 RVA: 0x00200BE3 File Offset: 0x001FEDE3
	public GameStageDefinition(string _name)
	{
		this.name = _name;
	}

	// Token: 0x06005370 RID: 21360 RVA: 0x00200BFD File Offset: 0x001FEDFD
	public static void AddGameStage(GameStageDefinition gameStage)
	{
		gameStage.SortStages();
		GameStageDefinition.gameStages.Add(gameStage.name, gameStage);
	}

	// Token: 0x06005371 RID: 21361 RVA: 0x00200C16 File Offset: 0x001FEE16
	public static GameStageDefinition GetGameStage(string name)
	{
		return GameStageDefinition.gameStages[name];
	}

	// Token: 0x06005372 RID: 21362 RVA: 0x00200C23 File Offset: 0x001FEE23
	public static bool TryGetGameStage(string name, out GameStageDefinition definition)
	{
		return GameStageDefinition.gameStages.TryGetValue(name, out definition);
	}

	// Token: 0x06005373 RID: 21363 RVA: 0x00200C31 File Offset: 0x001FEE31
	public static void Clear()
	{
		GameStageDefinition.gameStages.Clear();
	}

	// Token: 0x06005374 RID: 21364 RVA: 0x00200C3D File Offset: 0x001FEE3D
	public void AddStage(GameStageDefinition.Stage stage)
	{
		this.stages.Add(stage);
	}

	// Token: 0x06005375 RID: 21365 RVA: 0x00200C4B File Offset: 0x001FEE4B
	public void SortStages()
	{
		this.stages.Sort((GameStageDefinition.Stage x, GameStageDefinition.Stage y) => x.stageNum.CompareTo(y.stageNum));
	}

	// Token: 0x06005376 RID: 21366 RVA: 0x00200C78 File Offset: 0x001FEE78
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

	// Token: 0x06005377 RID: 21367 RVA: 0x00200CF8 File Offset: 0x001FEEF8
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

	// Token: 0x06005378 RID: 21368 RVA: 0x00200D4C File Offset: 0x001FEF4C
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

	// Token: 0x06005379 RID: 21369 RVA: 0x00200D9C File Offset: 0x001FEF9C
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

	// Token: 0x0400415A RID: 16730
	public static float DifficultyBonus = 1f;

	// Token: 0x0400415B RID: 16731
	public static float StartingWeight = 1f;

	// Token: 0x0400415C RID: 16732
	public static float DiminishingReturns = 0.5f;

	// Token: 0x0400415D RID: 16733
	public static long DaysAliveChangeWhenKilled = 2L;

	// Token: 0x0400415E RID: 16734
	public static int LootBonusEvery;

	// Token: 0x0400415F RID: 16735
	public static int LootBonusMaxCount;

	// Token: 0x04004160 RID: 16736
	public static float LootBonusScale;

	// Token: 0x04004161 RID: 16737
	public static int LootWanderingBonusEvery;

	// Token: 0x04004162 RID: 16738
	public static float LootWanderingBonusScale;

	// Token: 0x04004163 RID: 16739
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<string, GameStageDefinition> gameStages = new Dictionary<string, GameStageDefinition>();

	// Token: 0x04004164 RID: 16740
	public string name;

	// Token: 0x04004165 RID: 16741
	[PublicizedFrom(EAccessModifier.Private)]
	public List<GameStageDefinition.Stage> stages = new List<GameStageDefinition.Stage>();

	// Token: 0x02000AFC RID: 2812
	public class Stage
	{
		// Token: 0x0600537B RID: 21371 RVA: 0x00200E36 File Offset: 0x001FF036
		public Stage(int _stageNum)
		{
			this.stageNum = _stageNum;
		}

		// Token: 0x0600537C RID: 21372 RVA: 0x00200E50 File Offset: 0x001FF050
		public void AddSpawnGroup(GameStageDefinition.SpawnGroup spawn)
		{
			this.spawnGroups.Add(spawn);
		}

		// Token: 0x0600537D RID: 21373 RVA: 0x00200E5E File Offset: 0x001FF05E
		public GameStageDefinition.SpawnGroup GetSpawnGroup(int index)
		{
			if (index >= 0 && index < this.spawnGroups.Count)
			{
				return this.spawnGroups[index];
			}
			return null;
		}

		// Token: 0x1700092A RID: 2346
		// (get) Token: 0x0600537E RID: 21374 RVA: 0x00200E80 File Offset: 0x001FF080
		public int Count
		{
			get
			{
				return this.spawnGroups.Count;
			}
		}

		// Token: 0x04004166 RID: 16742
		public readonly int stageNum;

		// Token: 0x04004167 RID: 16743
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<GameStageDefinition.SpawnGroup> spawnGroups = new List<GameStageDefinition.SpawnGroup>();
	}

	// Token: 0x02000AFD RID: 2813
	public class SpawnGroup
	{
		// Token: 0x0600537F RID: 21375 RVA: 0x00200E8D File Offset: 0x001FF08D
		public SpawnGroup(string _groupName, int _spawnCount, int _maxAlive, int _interval, int _duration)
		{
			this.groupName = _groupName;
			this.spawnCount = (ushort)_spawnCount;
			this.maxAlive = (ushort)_maxAlive;
			this.interval = (ushort)_interval;
			this.duration = (ushort)_duration;
		}

		// Token: 0x04004168 RID: 16744
		public readonly string groupName;

		// Token: 0x04004169 RID: 16745
		public readonly ushort spawnCount;

		// Token: 0x0400416A RID: 16746
		public readonly ushort maxAlive;

		// Token: 0x0400416B RID: 16747
		public readonly ushort interval;

		// Token: 0x0400416C RID: 16748
		public readonly ushort duration;
	}
}

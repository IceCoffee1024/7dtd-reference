using System;
using System.Globalization;
using UnityEngine;

// Token: 0x02000AD6 RID: 2774
public class EntitySpawnerClass
{
	// Token: 0x060052CA RID: 21194 RVA: 0x001FA7C0 File Offset: 0x001F89C0
	public void Init()
	{
		if (!this.Properties.Values.ContainsKey(EntitySpawnerClass.PropEntityGroupName))
		{
			throw new Exception(string.Concat(new string[]
			{
				"Mandatory property '",
				EntitySpawnerClass.PropEntityGroupName,
				"' missing in entityspawnerclass '",
				this.name,
				"'"
			}));
		}
		this.entityGroupName = this.Properties.Values[EntitySpawnerClass.PropEntityGroupName];
		if (!EntityGroups.list.ContainsKey(this.entityGroupName))
		{
			throw new Exception("Entity spawner '" + this.name + "' contains invalid group " + this.entityGroupName);
		}
		if (this.Properties.Values.ContainsKey(EntitySpawnerClass.PropStartSound))
		{
			this.startSound = this.Properties.Values[EntitySpawnerClass.PropStartSound];
		}
		if (this.Properties.Values.ContainsKey(EntitySpawnerClass.PropStartText))
		{
			this.startText = this.Properties.Values[EntitySpawnerClass.PropStartText];
		}
		this.spawnAtTimeOfDay = EDaytime.Any;
		if (this.Properties.Values.ContainsKey(EntitySpawnerClass.PropTime))
		{
			this.spawnAtTimeOfDay = EnumUtils.Parse<EDaytime>(this.Properties.Values[EntitySpawnerClass.PropTime], false);
		}
		this.delayBetweenSpawns = 0f;
		if (this.Properties.Values.ContainsKey(EntitySpawnerClass.PropDelayBetweenSpawns))
		{
			this.delayBetweenSpawns = StringParsers.ParseFloat(this.Properties.Values[EntitySpawnerClass.PropDelayBetweenSpawns], 0, -1, NumberStyles.Any);
		}
		this.totalAlive = 1;
		if (this.Properties.Values.ContainsKey(EntitySpawnerClass.PropTotalAlive))
		{
			this.totalAlive = int.Parse(this.Properties.Values[EntitySpawnerClass.PropTotalAlive]);
		}
		this.totalPerWaveMin = 1;
		this.totalPerWaveMax = 1;
		if (this.Properties.Values.ContainsKey(EntitySpawnerClass.PropTotalPerWave))
		{
			StringParsers.ParseMinMaxCount(this.Properties.Values[EntitySpawnerClass.PropTotalPerWave], out this.totalPerWaveMin, out this.totalPerWaveMax);
		}
		this.delayToNextWave = 1f;
		if (this.Properties.Values.ContainsKey(EntitySpawnerClass.PropDelayToNextWave))
		{
			this.delayToNextWave = StringParsers.ParseFloat(this.Properties.Values[EntitySpawnerClass.PropDelayToNextWave], 0, -1, NumberStyles.Any);
		}
		this.bAttackPlayerImmediately = false;
		if (this.Properties.Values.ContainsKey(EntitySpawnerClass.PropAttackPlayerAtOnce))
		{
			this.bAttackPlayerImmediately = StringParsers.ParseBool(this.Properties.Values[EntitySpawnerClass.PropAttackPlayerAtOnce], 0, -1, true);
		}
		if (this.Properties.Values.ContainsKey(EntitySpawnerClass.PropNumberOfWaves))
		{
			this.numberOfWaves = int.Parse(this.Properties.Values[EntitySpawnerClass.PropNumberOfWaves]);
		}
		this.bTerritorial = false;
		if (this.Properties.Values.ContainsKey(EntitySpawnerClass.PropTerritorial))
		{
			this.bTerritorial = StringParsers.ParseBool(this.Properties.Values[EntitySpawnerClass.PropTerritorial], 0, -1, true);
		}
		this.territorialRange = 10;
		if (this.Properties.Values.ContainsKey(EntitySpawnerClass.PropTerritorialRange))
		{
			this.territorialRange = int.Parse(this.Properties.Values[EntitySpawnerClass.PropTerritorialRange]);
		}
		this.bSpawnOnGround = true;
		if (this.Properties.Values.ContainsKey(EntitySpawnerClass.PropSpawnOnGround))
		{
			this.bSpawnOnGround = StringParsers.ParseBool(this.Properties.Values[EntitySpawnerClass.PropSpawnOnGround], 0, -1, true);
		}
		this.bIgnoreTrigger = false;
		if (this.Properties.Values.ContainsKey(EntitySpawnerClass.PropIgnoreTrigger))
		{
			this.bIgnoreTrigger = StringParsers.ParseBool(this.Properties.Values[EntitySpawnerClass.PropIgnoreTrigger], 0, -1, true);
		}
		this.bPropResetToday = true;
		if (this.Properties.Values.ContainsKey(EntitySpawnerClass.PropResetToday))
		{
			this.bPropResetToday = StringParsers.ParseBool(this.Properties.Values[EntitySpawnerClass.PropResetToday], 0, -1, true);
		}
		this.daysToRespawnIfPlayerLeft = 0;
		if (this.Properties.Values.ContainsKey(EntitySpawnerClass.PropDaysToRespawnIfPlayerLeft))
		{
			this.daysToRespawnIfPlayerLeft = Mathf.RoundToInt(StringParsers.ParseFloat(this.Properties.Values[EntitySpawnerClass.PropDaysToRespawnIfPlayerLeft], 0, -1, NumberStyles.Any));
		}
		if (EntitySpawnerClass.DefaultClassName == null)
		{
			EntitySpawnerClass.DefaultClassName = this;
		}
	}

	// Token: 0x060052CB RID: 21195 RVA: 0x001FAC3C File Offset: 0x001F8E3C
	public static void Cleanup()
	{
		EntitySpawnerClass.list.Clear();
	}

	// Token: 0x04004062 RID: 16482
	public static string PropStartSound = "StartSound";

	// Token: 0x04004063 RID: 16483
	public static string PropStartText = "StartText";

	// Token: 0x04004064 RID: 16484
	public static string PropEntityGroupName = "EntityGroupName";

	// Token: 0x04004065 RID: 16485
	public static string PropTime = "Time";

	// Token: 0x04004066 RID: 16486
	public static string PropDelayBetweenSpawns = "DelayBetweenSpawns";

	// Token: 0x04004067 RID: 16487
	public static string PropTotalAlive = "TotalAlive";

	// Token: 0x04004068 RID: 16488
	public static string PropTotalPerWave = "TotalPerWave";

	// Token: 0x04004069 RID: 16489
	public static string PropDelayToNextWave = "DelayToNextWave";

	// Token: 0x0400406A RID: 16490
	public static string PropAttackPlayerAtOnce = "AttackPlayerAtOnce";

	// Token: 0x0400406B RID: 16491
	public static string PropNumberOfWaves = "NumberOfWaves";

	// Token: 0x0400406C RID: 16492
	public static string PropTerritorial = "Territorial";

	// Token: 0x0400406D RID: 16493
	public static string PropTerritorialRange = "TerritorialRange";

	// Token: 0x0400406E RID: 16494
	public static string PropSpawnOnGround = "SpawnOnGround";

	// Token: 0x0400406F RID: 16495
	public static string PropIgnoreTrigger = "IgnoreTrigger";

	// Token: 0x04004070 RID: 16496
	public static string PropResetToday = "ResetToday";

	// Token: 0x04004071 RID: 16497
	public static string PropDaysToRespawnIfPlayerLeft = "DaysToRespawnIfPlayerLeft";

	// Token: 0x04004072 RID: 16498
	public static DictionarySave<string, EntitySpawnerClassForDay> list = new DictionarySave<string, EntitySpawnerClassForDay>();

	// Token: 0x04004073 RID: 16499
	public static EntitySpawnerClass DefaultClassName;

	// Token: 0x04004074 RID: 16500
	public DynamicProperties Properties = new DynamicProperties();

	// Token: 0x04004075 RID: 16501
	public string name;

	// Token: 0x04004076 RID: 16502
	public string entityGroupName;

	// Token: 0x04004077 RID: 16503
	public EDaytime spawnAtTimeOfDay;

	// Token: 0x04004078 RID: 16504
	public float delayBetweenSpawns;

	// Token: 0x04004079 RID: 16505
	public int totalAlive;

	// Token: 0x0400407A RID: 16506
	public float delayToNextWave;

	// Token: 0x0400407B RID: 16507
	public int totalPerWaveMin;

	// Token: 0x0400407C RID: 16508
	public int totalPerWaveMax;

	// Token: 0x0400407D RID: 16509
	public int numberOfWaves;

	// Token: 0x0400407E RID: 16510
	public bool bAttackPlayerImmediately;

	// Token: 0x0400407F RID: 16511
	public bool bSpawnOnGround;

	// Token: 0x04004080 RID: 16512
	public bool bIgnoreTrigger;

	// Token: 0x04004081 RID: 16513
	public bool bTerritorial;

	// Token: 0x04004082 RID: 16514
	public int territorialRange;

	// Token: 0x04004083 RID: 16515
	public bool bPropResetToday;

	// Token: 0x04004084 RID: 16516
	public int daysToRespawnIfPlayerLeft;

	// Token: 0x04004085 RID: 16517
	public string startSound;

	// Token: 0x04004086 RID: 16518
	public string startText;
}

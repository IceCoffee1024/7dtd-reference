using System;
using System.Globalization;
using UnityEngine;

// Token: 0x02000AFA RID: 2810
public class EntitySpawnerClass
{
	// Token: 0x0600536B RID: 21355 RVA: 0x00200690 File Offset: 0x001FE890
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

	// Token: 0x0600536C RID: 21356 RVA: 0x00200B0C File Offset: 0x001FED0C
	public static void Cleanup()
	{
		EntitySpawnerClass.list.Clear();
	}

	// Token: 0x04004135 RID: 16693
	public static string PropStartSound = "StartSound";

	// Token: 0x04004136 RID: 16694
	public static string PropStartText = "StartText";

	// Token: 0x04004137 RID: 16695
	public static string PropEntityGroupName = "EntityGroupName";

	// Token: 0x04004138 RID: 16696
	public static string PropTime = "Time";

	// Token: 0x04004139 RID: 16697
	public static string PropDelayBetweenSpawns = "DelayBetweenSpawns";

	// Token: 0x0400413A RID: 16698
	public static string PropTotalAlive = "TotalAlive";

	// Token: 0x0400413B RID: 16699
	public static string PropTotalPerWave = "TotalPerWave";

	// Token: 0x0400413C RID: 16700
	public static string PropDelayToNextWave = "DelayToNextWave";

	// Token: 0x0400413D RID: 16701
	public static string PropAttackPlayerAtOnce = "AttackPlayerAtOnce";

	// Token: 0x0400413E RID: 16702
	public static string PropNumberOfWaves = "NumberOfWaves";

	// Token: 0x0400413F RID: 16703
	public static string PropTerritorial = "Territorial";

	// Token: 0x04004140 RID: 16704
	public static string PropTerritorialRange = "TerritorialRange";

	// Token: 0x04004141 RID: 16705
	public static string PropSpawnOnGround = "SpawnOnGround";

	// Token: 0x04004142 RID: 16706
	public static string PropIgnoreTrigger = "IgnoreTrigger";

	// Token: 0x04004143 RID: 16707
	public static string PropResetToday = "ResetToday";

	// Token: 0x04004144 RID: 16708
	public static string PropDaysToRespawnIfPlayerLeft = "DaysToRespawnIfPlayerLeft";

	// Token: 0x04004145 RID: 16709
	public static DictionarySave<string, EntitySpawnerClassForDay> list = new DictionarySave<string, EntitySpawnerClassForDay>();

	// Token: 0x04004146 RID: 16710
	public static EntitySpawnerClass DefaultClassName;

	// Token: 0x04004147 RID: 16711
	public DynamicProperties Properties = new DynamicProperties();

	// Token: 0x04004148 RID: 16712
	public string name;

	// Token: 0x04004149 RID: 16713
	public string entityGroupName;

	// Token: 0x0400414A RID: 16714
	public EDaytime spawnAtTimeOfDay;

	// Token: 0x0400414B RID: 16715
	public float delayBetweenSpawns;

	// Token: 0x0400414C RID: 16716
	public int totalAlive;

	// Token: 0x0400414D RID: 16717
	public float delayToNextWave;

	// Token: 0x0400414E RID: 16718
	public int totalPerWaveMin;

	// Token: 0x0400414F RID: 16719
	public int totalPerWaveMax;

	// Token: 0x04004150 RID: 16720
	public int numberOfWaves;

	// Token: 0x04004151 RID: 16721
	public bool bAttackPlayerImmediately;

	// Token: 0x04004152 RID: 16722
	public bool bSpawnOnGround;

	// Token: 0x04004153 RID: 16723
	public bool bIgnoreTrigger;

	// Token: 0x04004154 RID: 16724
	public bool bTerritorial;

	// Token: 0x04004155 RID: 16725
	public int territorialRange;

	// Token: 0x04004156 RID: 16726
	public bool bPropResetToday;

	// Token: 0x04004157 RID: 16727
	public int daysToRespawnIfPlayerLeft;

	// Token: 0x04004158 RID: 16728
	public string startSound;

	// Token: 0x04004159 RID: 16729
	public string startText;
}

using System;
using System.Globalization;
using UnityEngine;

// Token: 0x02000B05 RID: 2821
public class EntitySpawnerClass
{
	// Token: 0x06005390 RID: 21392 RVA: 0x00201C14 File Offset: 0x001FFE14
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

	// Token: 0x06005391 RID: 21393 RVA: 0x00202090 File Offset: 0x00200290
	public static void Cleanup()
	{
		EntitySpawnerClass.list.Clear();
	}

	// Token: 0x0400415A RID: 16730
	public static string PropStartSound = "StartSound";

	// Token: 0x0400415B RID: 16731
	public static string PropStartText = "StartText";

	// Token: 0x0400415C RID: 16732
	public static string PropEntityGroupName = "EntityGroupName";

	// Token: 0x0400415D RID: 16733
	public static string PropTime = "Time";

	// Token: 0x0400415E RID: 16734
	public static string PropDelayBetweenSpawns = "DelayBetweenSpawns";

	// Token: 0x0400415F RID: 16735
	public static string PropTotalAlive = "TotalAlive";

	// Token: 0x04004160 RID: 16736
	public static string PropTotalPerWave = "TotalPerWave";

	// Token: 0x04004161 RID: 16737
	public static string PropDelayToNextWave = "DelayToNextWave";

	// Token: 0x04004162 RID: 16738
	public static string PropAttackPlayerAtOnce = "AttackPlayerAtOnce";

	// Token: 0x04004163 RID: 16739
	public static string PropNumberOfWaves = "NumberOfWaves";

	// Token: 0x04004164 RID: 16740
	public static string PropTerritorial = "Territorial";

	// Token: 0x04004165 RID: 16741
	public static string PropTerritorialRange = "TerritorialRange";

	// Token: 0x04004166 RID: 16742
	public static string PropSpawnOnGround = "SpawnOnGround";

	// Token: 0x04004167 RID: 16743
	public static string PropIgnoreTrigger = "IgnoreTrigger";

	// Token: 0x04004168 RID: 16744
	public static string PropResetToday = "ResetToday";

	// Token: 0x04004169 RID: 16745
	public static string PropDaysToRespawnIfPlayerLeft = "DaysToRespawnIfPlayerLeft";

	// Token: 0x0400416A RID: 16746
	public static DictionarySave<string, EntitySpawnerClassForDay> list = new DictionarySave<string, EntitySpawnerClassForDay>();

	// Token: 0x0400416B RID: 16747
	public static EntitySpawnerClass DefaultClassName;

	// Token: 0x0400416C RID: 16748
	public DynamicProperties Properties = new DynamicProperties();

	// Token: 0x0400416D RID: 16749
	public string name;

	// Token: 0x0400416E RID: 16750
	public string entityGroupName;

	// Token: 0x0400416F RID: 16751
	public EDaytime spawnAtTimeOfDay;

	// Token: 0x04004170 RID: 16752
	public float delayBetweenSpawns;

	// Token: 0x04004171 RID: 16753
	public int totalAlive;

	// Token: 0x04004172 RID: 16754
	public float delayToNextWave;

	// Token: 0x04004173 RID: 16755
	public int totalPerWaveMin;

	// Token: 0x04004174 RID: 16756
	public int totalPerWaveMax;

	// Token: 0x04004175 RID: 16757
	public int numberOfWaves;

	// Token: 0x04004176 RID: 16758
	public bool bAttackPlayerImmediately;

	// Token: 0x04004177 RID: 16759
	public bool bSpawnOnGround;

	// Token: 0x04004178 RID: 16760
	public bool bIgnoreTrigger;

	// Token: 0x04004179 RID: 16761
	public bool bTerritorial;

	// Token: 0x0400417A RID: 16762
	public int territorialRange;

	// Token: 0x0400417B RID: 16763
	public bool bPropResetToday;

	// Token: 0x0400417C RID: 16764
	public int daysToRespawnIfPlayerLeft;

	// Token: 0x0400417D RID: 16765
	public string startSound;

	// Token: 0x0400417E RID: 16766
	public string startText;
}

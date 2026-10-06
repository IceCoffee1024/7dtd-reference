using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000973 RID: 2419
[Preserve]
public class QuestActionSpawnGSEnemy : BaseQuestAction
{
	// Token: 0x0600464E RID: 17998 RVA: 0x001BED02 File Offset: 0x001BCF02
	public override void PerformAction(Quest ownerQuest)
	{
		if (!GameStats.GetBool(EnumGameStats.EnemySpawnMode))
		{
			return;
		}
		this.HandleSpawnEnemies(ownerQuest);
	}

	// Token: 0x0600464F RID: 17999 RVA: 0x001BED18 File Offset: 0x001BCF18
	public void HandleSpawnEnemies(Quest ownerQuest)
	{
		if (this.Value != null && this.Value != "" && !int.TryParse(this.Value, out this.count) && this.Value.Contains("-"))
		{
			string[] array = this.Value.Split('-', StringSplitOptions.None);
			int num = Convert.ToInt32(array[0]);
			int num2 = Convert.ToInt32(array[1]);
			World world = GameManager.Instance.World;
			this.count = world.GetGameRandom().RandomRange(num, num2);
		}
		GameManager.Instance.StartCoroutine(this.SpawnEnemies(ownerQuest));
	}

	// Token: 0x06004650 RID: 18000 RVA: 0x001BEDB3 File Offset: 0x001BCFB3
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator SpawnEnemies(Quest ownerQuest)
	{
		EntityPlayerLocal player = ownerQuest.OwnerJournal.OwnerPlayer;
		int lastClassId = 0;
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			World world = GameManager.Instance.World;
			GameStageDefinition def = GameStageDefinition.GetGameStage(this.ID);
			int num;
			for (int i = 0; i < this.count; i = num + 1)
			{
				yield return new WaitForSeconds(0.5f);
				int randomFromGroup = EntityGroups.GetRandomFromGroup(def.GetStage(player.PartyGameStage).GetSpawnGroup(0).groupName, ref lastClassId, null);
				if (randomFromGroup != 0)
				{
					QuestActionSpawnGSEnemy.SpawnQuestEntity(randomFromGroup, -1, player);
				}
				num = i;
			}
			def = null;
		}
		else
		{
			int num;
			for (int i = 0; i < this.count; i = num + 1)
			{
				yield return new WaitForSeconds(0.5f);
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageQuestEntitySpawn>().Setup(this.ID, player.entityId), false);
				num = i;
			}
		}
		yield break;
	}

	// Token: 0x06004651 RID: 18001 RVA: 0x001BEDCC File Offset: 0x001BCFCC
	public static void SpawnQuestEntity(int spawnedEntityID, int entityIDQuestHolder, EntityPlayer player = null)
	{
		World world = GameManager.Instance.World;
		if (player == null)
		{
			player = world.GetEntity(entityIDQuestHolder) as EntityPlayer;
		}
		Vector3 vector = new Vector3(world.GetGameRandom().RandomFloat * 2f + -1f, 0f, world.GetGameRandom().RandomFloat * 2f + -1f);
		vector.Normalize();
		float num = world.GetGameRandom().RandomFloat * 12f + 12f;
		Vector3 vector2 = player.position + vector * num;
		Vector3 vector3 = new Vector3(0f, player.transform.eulerAngles.y + 180f, 0f);
		float num2 = (float)GameManager.Instance.World.GetHeight((int)vector2.x, (int)vector2.z);
		float num3 = (float)GameManager.Instance.World.GetTerrainHeight((int)vector2.x, (int)vector2.z);
		vector2.y = (num2 + num3) / 2f + 1.5f;
		Entity entity = EntityFactory.CreateEntity(spawnedEntityID, vector2, vector3);
		entity.SetSpawnerSource(EnumSpawnerSource.Dynamic);
		GameManager.Instance.World.SpawnEntityInWorld(entity);
		(entity as EntityAlive).SetAttackTarget(player, 200);
	}

	// Token: 0x06004652 RID: 18002 RVA: 0x001BEF20 File Offset: 0x001BD120
	public override BaseQuestAction Clone()
	{
		QuestActionSpawnGSEnemy questActionSpawnGSEnemy = new QuestActionSpawnGSEnemy();
		base.CopyValues(questActionSpawnGSEnemy);
		return questActionSpawnGSEnemy;
	}

	// Token: 0x06004653 RID: 18003 RVA: 0x001BEF3C File Offset: 0x001BD13C
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		if (properties.Values.ContainsKey(QuestActionSpawnGSEnemy.PropGameStageID))
		{
			this.ID = properties.Values[QuestActionSpawnGSEnemy.PropGameStageID];
		}
		if (properties.Values.ContainsKey(QuestActionSpawnGSEnemy.PropCount))
		{
			this.Value = properties.Values[QuestActionSpawnGSEnemy.PropCount];
		}
	}

	// Token: 0x040038D8 RID: 14552
	[PublicizedFrom(EAccessModifier.Private)]
	public int count = 1;

	// Token: 0x040038D9 RID: 14553
	public static string PropGameStageID = "gamestage_list";

	// Token: 0x040038DA RID: 14554
	public static string PropCount = "count";
}

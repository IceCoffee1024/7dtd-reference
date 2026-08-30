using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200096B RID: 2411
[Preserve]
public class QuestActionSpawnEnemy : BaseQuestAction
{
	// Token: 0x060046A0 RID: 18080 RVA: 0x001BE3DC File Offset: 0x001BC5DC
	public override void SetupAction()
	{
		string[] array = this.ID.Split(',', StringSplitOptions.None);
		for (int i = 0; i < array.Length; i++)
		{
			foreach (KeyValuePair<int, EntityClass> keyValuePair in EntityClass.list.Dict)
			{
				if (keyValuePair.Value.entityClassName == array[i])
				{
					this.entityIDs.Add(keyValuePair.Key);
					if (this.entityIDs.Count == array.Length)
					{
						break;
					}
				}
			}
		}
	}

	// Token: 0x060046A1 RID: 18081 RVA: 0x001BE484 File Offset: 0x001BC684
	public override void PerformAction(Quest ownerQuest)
	{
		if (!GameStats.GetBool(EnumGameStats.EnemySpawnMode))
		{
			return;
		}
		this.HandleSpawnEnemies(ownerQuest);
	}

	// Token: 0x060046A2 RID: 18082 RVA: 0x001BE498 File Offset: 0x001BC698
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

	// Token: 0x060046A3 RID: 18083 RVA: 0x001BE533 File Offset: 0x001BC733
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator SpawnEnemies(Quest ownerQuest)
	{
		EntityPlayerLocal player = ownerQuest.OwnerJournal.OwnerPlayer;
		int num2;
		for (int i = 0; i < this.count; i = num2 + 1)
		{
			yield return new WaitForSeconds(0.5f);
			World world = GameManager.Instance.World;
			int num = this.entityIDs[world.GetGameRandom().RandomRange(this.entityIDs.Count)];
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				QuestActionSpawnEnemy.SpawnQuestEntity(num, -1, player);
			}
			else
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageQuestEntitySpawn>().Setup(num, player.entityId), false);
			}
			num2 = i;
		}
		yield break;
	}

	// Token: 0x060046A4 RID: 18084 RVA: 0x001BE54C File Offset: 0x001BC74C
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

	// Token: 0x060046A5 RID: 18085 RVA: 0x001BE6A0 File Offset: 0x001BC8A0
	public override BaseQuestAction Clone()
	{
		QuestActionSpawnEnemy questActionSpawnEnemy = new QuestActionSpawnEnemy();
		base.CopyValues(questActionSpawnEnemy);
		questActionSpawnEnemy.entityIDs.AddRange(this.entityIDs);
		return questActionSpawnEnemy;
	}

	// Token: 0x040038D4 RID: 14548
	[PublicizedFrom(EAccessModifier.Private)]
	public List<int> entityIDs = new List<int>();

	// Token: 0x040038D5 RID: 14549
	[PublicizedFrom(EAccessModifier.Private)]
	public int count = 1;
}

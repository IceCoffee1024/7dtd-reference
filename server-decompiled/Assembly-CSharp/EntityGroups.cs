using System;
using System.Collections.Generic;

// Token: 0x02000AEF RID: 2799
public class EntityGroups
{
	// Token: 0x0600533C RID: 21308 RVA: 0x001FF434 File Offset: 0x001FD634
	public static int GetRandomEntityFromGroupMaxTier(string _sEntityGroupName, EntityClass.EntityTierTypes maxTier, ref int lastClassId, bool isEnemy = false, bool isAnimal = false, GameRandom random = null)
	{
		List<SEntityClassAndProb> list = EntityGroups.list[_sEntityGroupName];
		if (random == null)
		{
			random = GameManager.Instance.World.GetGameRandom();
		}
		int num = -1;
		if (EntityGroups.workingGroupList == null)
		{
			EntityGroups.workingGroupList = new List<SEntityClassAndProb>();
		}
		else
		{
			EntityGroups.workingGroupList.Clear();
		}
		foreach (SEntityClassAndProb sentityClassAndProb in list)
		{
			EntityClass entityClass = EntityClass.GetEntityClass(sentityClassAndProb.entityClassId);
			if (entityClass == null)
			{
				Log.Error(string.Format("EntityGroup GetRandomEnemyFromGroupMaxTier: unknown type ({0})", sentityClassAndProb.entityClassId));
				return -1;
			}
			EntityClass entityClassWithinMaxTier = EntityClass.GetEntityClassWithinMaxTier(entityClass, maxTier);
			if (entityClassWithinMaxTier != null)
			{
				bool flag = isEnemy && !entityClassWithinMaxTier.bIsEnemyEntity;
				bool flag2 = isAnimal && !entityClassWithinMaxTier.bIsAnimalEntity;
				if (!flag && !flag2)
				{
					EntityGroups.workingGroupList.Add(new SEntityClassAndProb
					{
						entityClassId = EntityClass.GetId(entityClassWithinMaxTier.entityClassName),
						prob = sentityClassAndProb.prob
					});
				}
			}
		}
		if (EntityGroups.workingGroupList.Count == 0)
		{
			return -1;
		}
		EntityGroups.NormalizeWorkingList(EntityGroups.workingGroupList);
		for (int i = 0; i < 3; i++)
		{
			num = EntityGroups.GetRandomFromGroupList(EntityGroups.workingGroupList, random);
			if (num != lastClassId)
			{
				lastClassId = num;
				break;
			}
		}
		return num;
	}

	// Token: 0x0600533D RID: 21309 RVA: 0x001FF5A0 File Offset: 0x001FD7A0
	[PublicizedFrom(EAccessModifier.Private)]
	public static void NormalizeWorkingList(List<SEntityClassAndProb> list)
	{
		float num = 0f;
		for (int i = 0; i < list.Count; i++)
		{
			num += list[i].prob;
		}
		if (num <= 0f)
		{
			return;
		}
		for (int j = 0; j < list.Count; j++)
		{
			SEntityClassAndProb sentityClassAndProb = list[j];
			sentityClassAndProb.prob /= num;
			list[j] = sentityClassAndProb;
		}
	}

	// Token: 0x0600533E RID: 21310 RVA: 0x001FF60C File Offset: 0x001FD80C
	public static int GetRandomFromGroup(string _sEntityGroupName, ref int lastClassId, GameRandom random = null)
	{
		List<SEntityClassAndProb> list = EntityGroups.list[_sEntityGroupName];
		if (random == null)
		{
			random = GameManager.Instance.World.GetGameRandom();
		}
		int num = 0;
		for (int i = 0; i < 3; i++)
		{
			num = EntityGroups.GetRandomFromGroupList(list, random);
			if (num != lastClassId)
			{
				lastClassId = num;
				break;
			}
		}
		return num;
	}

	// Token: 0x0600533F RID: 21311 RVA: 0x001FF65C File Offset: 0x001FD85C
	[PublicizedFrom(EAccessModifier.Private)]
	public static int GetRandomFromGroupList(List<SEntityClassAndProb> grpList, GameRandom random)
	{
		float randomFloat = random.RandomFloat;
		float num = 0f;
		for (int i = 0; i < grpList.Count; i++)
		{
			SEntityClassAndProb sentityClassAndProb = grpList[i];
			num += sentityClassAndProb.prob;
			if (randomFloat <= num && sentityClassAndProb.prob > 0f)
			{
				return sentityClassAndProb.entityClassId;
			}
		}
		return -1;
	}

	// Token: 0x06005340 RID: 21312 RVA: 0x001FF6B4 File Offset: 0x001FD8B4
	public static bool IsEnemyGroup(string _sEntityGroupName)
	{
		List<SEntityClassAndProb> list = EntityGroups.list[_sEntityGroupName];
		return list != null && list.Count >= 1 && EntityClass.list[list[0].entityClassId].bIsEnemyEntity;
	}

	// Token: 0x06005341 RID: 21313 RVA: 0x001FF6F8 File Offset: 0x001FD8F8
	public static void Normalize(string _sEntityGroupName, float totalp)
	{
		List<SEntityClassAndProb> list = EntityGroups.list[_sEntityGroupName];
		for (int i = 0; i < list.Count; i++)
		{
			SEntityClassAndProb sentityClassAndProb = list[i];
			sentityClassAndProb.prob /= totalp;
			list[i] = sentityClassAndProb;
		}
	}

	// Token: 0x06005342 RID: 21314 RVA: 0x001FF73E File Offset: 0x001FD93E
	public static void Cleanup()
	{
		if (EntityGroups.list != null)
		{
			EntityGroups.list.Clear();
		}
	}

	// Token: 0x0400410F RID: 16655
	public static DictionarySave<string, List<SEntityClassAndProb>> list = new DictionarySave<string, List<SEntityClassAndProb>>();

	// Token: 0x04004110 RID: 16656
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<SEntityClassAndProb> workingGroupList;
}

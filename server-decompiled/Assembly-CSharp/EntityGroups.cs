using System;
using System.Collections.Generic;

// Token: 0x02000AFA RID: 2810
public class EntityGroups
{
	// Token: 0x06005361 RID: 21345 RVA: 0x002009B8 File Offset: 0x001FEBB8
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

	// Token: 0x06005362 RID: 21346 RVA: 0x00200B24 File Offset: 0x001FED24
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

	// Token: 0x06005363 RID: 21347 RVA: 0x00200B90 File Offset: 0x001FED90
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

	// Token: 0x06005364 RID: 21348 RVA: 0x00200BE0 File Offset: 0x001FEDE0
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

	// Token: 0x06005365 RID: 21349 RVA: 0x00200C38 File Offset: 0x001FEE38
	public static bool IsEnemyGroup(string _sEntityGroupName)
	{
		List<SEntityClassAndProb> list = EntityGroups.list[_sEntityGroupName];
		return list != null && list.Count >= 1 && EntityClass.list[list[0].entityClassId].bIsEnemyEntity;
	}

	// Token: 0x06005366 RID: 21350 RVA: 0x00200C7C File Offset: 0x001FEE7C
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

	// Token: 0x06005367 RID: 21351 RVA: 0x00200CC2 File Offset: 0x001FEEC2
	public static void Cleanup()
	{
		if (EntityGroups.list != null)
		{
			EntityGroups.list.Clear();
		}
	}

	// Token: 0x04004134 RID: 16692
	public static DictionarySave<string, List<SEntityClassAndProb>> list = new DictionarySave<string, List<SEntityClassAndProb>>();

	// Token: 0x04004135 RID: 16693
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<SEntityClassAndProb> workingGroupList;
}

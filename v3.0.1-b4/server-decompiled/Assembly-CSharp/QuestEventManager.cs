using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Challenges;
using Quests;
using UnityEngine;

// Token: 0x020009CA RID: 2506
public class QuestEventManager
{
	// Token: 0x170007ED RID: 2029
	// (get) Token: 0x06004A19 RID: 18969 RVA: 0x001CE003 File Offset: 0x001CC203
	public static QuestEventManager Current
	{
		get
		{
			if (QuestEventManager.instance == null)
			{
				QuestEventManager.instance = new QuestEventManager();
			}
			return QuestEventManager.instance;
		}
	}

	// Token: 0x170007EE RID: 2030
	// (get) Token: 0x06004A1A RID: 18970 RVA: 0x001CE01B File Offset: 0x001CC21B
	public static bool HasInstance
	{
		get
		{
			return QuestEventManager.instance != null;
		}
	}

	// Token: 0x06004A1B RID: 18971 RVA: 0x001CE028 File Offset: 0x001CC228
	public void SetupTraderPrefabList(TraderArea area)
	{
		if (!this.TraderPrefabList.ContainsKey(area))
		{
			Vector3 vector = area.Position.ToVector3();
			List<PrefabInstance> list = new List<PrefabInstance>();
			GameManager.Instance.GetDynamicPrefabDecorator().GetPOIPrefabs(list);
			List<QuestEventManager.PrefabListData> list2 = new List<QuestEventManager.PrefabListData>();
			QuestEventManager.PrefabListData prefabListData = new QuestEventManager.PrefabListData();
			QuestEventManager.PrefabListData prefabListData2 = new QuestEventManager.PrefabListData();
			QuestEventManager.PrefabListData prefabListData3 = new QuestEventManager.PrefabListData();
			list2.Add(prefabListData);
			list2.Add(prefabListData2);
			list2.Add(prefabListData3);
			for (int i = 0; i < list.Count; i++)
			{
				float num = Vector3.Distance(vector, list[i].boundingBoxPosition);
				if (num <= 500f)
				{
					prefabListData.AddPOI(list[i]);
				}
				else if (num <= 1500f)
				{
					prefabListData2.AddPOI(list[i]);
				}
				else
				{
					prefabListData3.AddPOI(list[i]);
				}
			}
			this.TraderPrefabList.Add(area, list2);
		}
	}

	// Token: 0x06004A1C RID: 18972 RVA: 0x001CE118 File Offset: 0x001CC318
	public List<PrefabInstance> GetPrefabsForTrader(TraderArea traderArea, int difficulty, int index, GameRandom gameRandom)
	{
		if (traderArea == null)
		{
			return null;
		}
		if (!this.TraderPrefabList.ContainsKey(traderArea))
		{
			this.SetupTraderPrefabList(traderArea);
		}
		QuestEventManager.PrefabListData prefabListData = this.TraderPrefabList[traderArea][index];
		if (gameRandom != null)
		{
			prefabListData.ShuffleDifficulty(difficulty, gameRandom);
		}
		List<PrefabInstance> list;
		if (prefabListData.TierData.TryGetValue(difficulty, out list))
		{
			return list;
		}
		return null;
	}

	// Token: 0x06004A1D RID: 18973 RVA: 0x001CE174 File Offset: 0x001CC374
	public int GetTraderPoiCount(TraderArea traderArea, int difficulty, int index)
	{
		int num = 0;
		if (traderArea == null)
		{
			return 0;
		}
		if (!this.TraderPrefabList.ContainsKey(traderArea))
		{
			this.SetupTraderPrefabList(traderArea);
		}
		List<PrefabInstance> list;
		if (this.TraderPrefabList[traderArea][index].TierData.TryGetValue(difficulty, out list))
		{
			num = list.Count;
		}
		return num;
	}

	// Token: 0x06004A1E RID: 18974 RVA: 0x001CE1C8 File Offset: 0x001CC3C8
	public List<PrefabInstance> GetPrefabsByDifficultyTier(int difficulty)
	{
		if (this.tierPrefabList.Count == 0)
		{
			List<PrefabInstance> list = new List<PrefabInstance>();
			GameManager.Instance.GetDynamicPrefabDecorator().GetPOIPrefabs(list);
			for (int i = 0; i < list.Count; i++)
			{
				if (!this.tierPrefabList.ContainsKey((int)list[i].prefab.DifficultyTier))
				{
					this.tierPrefabList.Add((int)list[i].prefab.DifficultyTier, new List<PrefabInstance>());
				}
				this.tierPrefabList[(int)list[i].prefab.DifficultyTier].Add(list[i]);
			}
		}
		if (this.tierPrefabList.ContainsKey(difficulty))
		{
			return this.tierPrefabList[difficulty];
		}
		return null;
	}

	// Token: 0x06004A1F RID: 18975 RVA: 0x001CE290 File Offset: 0x001CC490
	[PublicizedFrom(EAccessModifier.Private)]
	public QuestEventManager()
	{
	}

	// Token: 0x14000052 RID: 82
	// (add) Token: 0x06004A20 RID: 18976 RVA: 0x001CE340 File Offset: 0x001CC540
	// (remove) Token: 0x06004A21 RID: 18977 RVA: 0x001CE378 File Offset: 0x001CC578
	public event QuestEvent_BlockEvent BlockActivate;

	// Token: 0x06004A22 RID: 18978 RVA: 0x001CE3AD File Offset: 0x001CC5AD
	public void BlockActivated(string blockName, Vector3i blockPos)
	{
		if (this.BlockActivate != null)
		{
			this.BlockActivate(blockName, blockPos);
		}
	}

	// Token: 0x14000053 RID: 83
	// (add) Token: 0x06004A23 RID: 18979 RVA: 0x001CE3C4 File Offset: 0x001CC5C4
	// (remove) Token: 0x06004A24 RID: 18980 RVA: 0x001CE3FC File Offset: 0x001CC5FC
	public event QuestEvent_BlockChangedEvent BlockChange;

	// Token: 0x06004A25 RID: 18981 RVA: 0x001CE431 File Offset: 0x001CC631
	public void BlockChanged(Block blockOld, Block blockNew, Vector3i blockPos)
	{
		if (this.BlockChange != null)
		{
			this.BlockChange(blockOld, blockNew, blockPos);
		}
	}

	// Token: 0x14000054 RID: 84
	// (add) Token: 0x06004A26 RID: 18982 RVA: 0x001CE44C File Offset: 0x001CC64C
	// (remove) Token: 0x06004A27 RID: 18983 RVA: 0x001CE484 File Offset: 0x001CC684
	public event QuestEvent_BlockDestroyEvent BlockDestroy;

	// Token: 0x06004A28 RID: 18984 RVA: 0x001CE4BC File Offset: 0x001CC6BC
	public void BlockDestroyed(Block block, BlockValueRef bvRef, Entity byEntity = null)
	{
		Vector3i vector3i;
		if (!bvRef.TryGetBlockPos(out vector3i))
		{
			return;
		}
		if (this.BlockDestroy != null)
		{
			this.BlockDestroy(block, vector3i);
		}
		if (block.AllowBlockTriggers && byEntity)
		{
			EntityPlayer entityPlayer = byEntity as EntityPlayer;
			if (!entityPlayer)
			{
				entityPlayer = byEntity.world.GetClosestPlayer(byEntity, 500f, false);
			}
			if (entityPlayer)
			{
				BlockValue blockValue = default(BlockValue);
				blockValue.type = block.blockID;
				block.HandleTrigger(entityPlayer, entityPlayer.world, vector3i, blockValue);
			}
		}
	}

	// Token: 0x14000055 RID: 85
	// (add) Token: 0x06004A29 RID: 18985 RVA: 0x001CE548 File Offset: 0x001CC748
	// (remove) Token: 0x06004A2A RID: 18986 RVA: 0x001CE580 File Offset: 0x001CC780
	public event QuestEvent_BlockEvent BlockPickup;

	// Token: 0x06004A2B RID: 18987 RVA: 0x001CE5B5 File Offset: 0x001CC7B5
	public void BlockPickedUp(string blockName, Vector3i blockPos)
	{
		if (this.BlockPickup != null)
		{
			this.BlockPickup(blockName, blockPos);
		}
	}

	// Token: 0x14000056 RID: 86
	// (add) Token: 0x06004A2C RID: 18988 RVA: 0x001CE5CC File Offset: 0x001CC7CC
	// (remove) Token: 0x06004A2D RID: 18989 RVA: 0x001CE604 File Offset: 0x001CC804
	public event QuestEvent_BlockEvent BlockPlace;

	// Token: 0x06004A2E RID: 18990 RVA: 0x001CE639 File Offset: 0x001CC839
	public void BlockPlaced(string blockName, Vector3i blockPos)
	{
		if (this.BlockPlace != null)
		{
			this.BlockPlace(blockName, blockPos);
		}
	}

	// Token: 0x14000057 RID: 87
	// (add) Token: 0x06004A2F RID: 18991 RVA: 0x001CE650 File Offset: 0x001CC850
	// (remove) Token: 0x06004A30 RID: 18992 RVA: 0x001CE688 File Offset: 0x001CC888
	public event QuestEvent_BlockEvent BlockUpgrade;

	// Token: 0x06004A31 RID: 18993 RVA: 0x001CE6BD File Offset: 0x001CC8BD
	public void BlockUpgraded(string blockName, Vector3i blockPos)
	{
		if (this.BlockUpgrade != null)
		{
			this.BlockUpgrade(blockName, blockPos);
		}
	}

	// Token: 0x14000058 RID: 88
	// (add) Token: 0x06004A32 RID: 18994 RVA: 0x001CE6D4 File Offset: 0x001CC8D4
	// (remove) Token: 0x06004A33 RID: 18995 RVA: 0x001CE70C File Offset: 0x001CC90C
	public event QuestEvent_ItemStackActionEvent AddItem;

	// Token: 0x06004A34 RID: 18996 RVA: 0x001CE741 File Offset: 0x001CC941
	public void ItemAdded(ItemStack newStack)
	{
		if (this.AddItem != null)
		{
			this.AddItem(newStack);
		}
	}

	// Token: 0x14000059 RID: 89
	// (add) Token: 0x06004A35 RID: 18997 RVA: 0x001CE758 File Offset: 0x001CC958
	// (remove) Token: 0x06004A36 RID: 18998 RVA: 0x001CE790 File Offset: 0x001CC990
	public event QuestEvent_HarvestStackActionEvent HarvestItem;

	// Token: 0x06004A37 RID: 18999 RVA: 0x001CE7C5 File Offset: 0x001CC9C5
	public void HarvestedItem(ItemValue heldItem, ItemStack newStack, BlockValue bv)
	{
		if (this.HarvestItem != null)
		{
			this.HarvestItem(heldItem, newStack, bv);
		}
	}

	// Token: 0x1400005A RID: 90
	// (add) Token: 0x06004A38 RID: 19000 RVA: 0x001CE7E0 File Offset: 0x001CC9E0
	// (remove) Token: 0x06004A39 RID: 19001 RVA: 0x001CE818 File Offset: 0x001CCA18
	public event QuestEvent_ItemStackActionEvent AssembleItem;

	// Token: 0x06004A3A RID: 19002 RVA: 0x001CE84D File Offset: 0x001CCA4D
	public void AssembledItem(ItemStack newStack)
	{
		if (this.AssembleItem != null)
		{
			this.AssembleItem(newStack);
		}
	}

	// Token: 0x1400005B RID: 91
	// (add) Token: 0x06004A3B RID: 19003 RVA: 0x001CE864 File Offset: 0x001CCA64
	// (remove) Token: 0x06004A3C RID: 19004 RVA: 0x001CE89C File Offset: 0x001CCA9C
	public event QuestEvent_ItemStackActionEvent CraftItem;

	// Token: 0x06004A3D RID: 19005 RVA: 0x001CE8D1 File Offset: 0x001CCAD1
	public void CraftedItem(ItemStack newStack)
	{
		if (this.CraftItem != null)
		{
			this.CraftItem(newStack);
		}
	}

	// Token: 0x1400005C RID: 92
	// (add) Token: 0x06004A3E RID: 19006 RVA: 0x001CE8E8 File Offset: 0x001CCAE8
	// (remove) Token: 0x06004A3F RID: 19007 RVA: 0x001CE920 File Offset: 0x001CCB20
	public event QuestEvent_ItemStackActionEvent ExchangeFromItem;

	// Token: 0x06004A40 RID: 19008 RVA: 0x001CE955 File Offset: 0x001CCB55
	public void ExchangedFromItem(ItemStack newStack)
	{
		if (this.ExchangeFromItem != null)
		{
			this.ExchangeFromItem(newStack);
		}
	}

	// Token: 0x1400005D RID: 93
	// (add) Token: 0x06004A41 RID: 19009 RVA: 0x001CE96C File Offset: 0x001CCB6C
	// (remove) Token: 0x06004A42 RID: 19010 RVA: 0x001CE9A4 File Offset: 0x001CCBA4
	public event QuestEvent_ItemStackActionEvent ScrapItem;

	// Token: 0x06004A43 RID: 19011 RVA: 0x001CE9D9 File Offset: 0x001CCBD9
	public void ScrappedItem(ItemStack newStack)
	{
		if (this.ScrapItem != null)
		{
			this.ScrapItem(newStack);
		}
	}

	// Token: 0x1400005E RID: 94
	// (add) Token: 0x06004A44 RID: 19012 RVA: 0x001CE9F0 File Offset: 0x001CCBF0
	// (remove) Token: 0x06004A45 RID: 19013 RVA: 0x001CEA28 File Offset: 0x001CCC28
	public event QuestEvent_ItemValueActionEvent RepairItem;

	// Token: 0x06004A46 RID: 19014 RVA: 0x001CEA5D File Offset: 0x001CCC5D
	public void RepairedItem(ItemValue newValue)
	{
		if (this.RepairItem != null)
		{
			this.RepairItem(newValue);
		}
	}

	// Token: 0x1400005F RID: 95
	// (add) Token: 0x06004A47 RID: 19015 RVA: 0x001CEA74 File Offset: 0x001CCC74
	// (remove) Token: 0x06004A48 RID: 19016 RVA: 0x001CEAAC File Offset: 0x001CCCAC
	public event QuestEvent_SkillPointSpent SkillPointSpent;

	// Token: 0x14000060 RID: 96
	// (add) Token: 0x06004A49 RID: 19017 RVA: 0x001CEAE4 File Offset: 0x001CCCE4
	// (remove) Token: 0x06004A4A RID: 19018 RVA: 0x001CEB1C File Offset: 0x001CCD1C
	public event QuestEvent_ItemValueActionEvent HoldItem;

	// Token: 0x06004A4B RID: 19019 RVA: 0x001CEB51 File Offset: 0x001CCD51
	public void HeldItem(ItemValue newValue)
	{
		if (this.HoldItem != null)
		{
			this.HoldItem(newValue);
		}
	}

	// Token: 0x14000061 RID: 97
	// (add) Token: 0x06004A4C RID: 19020 RVA: 0x001CEB68 File Offset: 0x001CCD68
	// (remove) Token: 0x06004A4D RID: 19021 RVA: 0x001CEBA0 File Offset: 0x001CCDA0
	public event QuestEvent_ItemValueActionEvent WearItem;

	// Token: 0x06004A4E RID: 19022 RVA: 0x001CEBD5 File Offset: 0x001CCDD5
	public void WoreItem(ItemValue newValue)
	{
		if (this.WearItem != null)
		{
			this.WearItem(newValue);
		}
	}

	// Token: 0x06004A4F RID: 19023 RVA: 0x001CEBEB File Offset: 0x001CCDEB
	public void SpendSkillPoint(ProgressionValue skill)
	{
		if (this.SkillPointSpent != null)
		{
			this.SkillPointSpent(skill.ProgressionClass.Name);
		}
	}

	// Token: 0x14000062 RID: 98
	// (add) Token: 0x06004A50 RID: 19024 RVA: 0x001CEC0C File Offset: 0x001CCE0C
	// (remove) Token: 0x06004A51 RID: 19025 RVA: 0x001CEC44 File Offset: 0x001CCE44
	public event QuestEvent_WindowChanged WindowChanged;

	// Token: 0x06004A52 RID: 19026 RVA: 0x001CEC79 File Offset: 0x001CCE79
	public void ChangedWindow(string windowName)
	{
		if (this.WindowChanged != null)
		{
			this.WindowChanged(windowName);
		}
	}

	// Token: 0x14000063 RID: 99
	// (add) Token: 0x06004A53 RID: 19027 RVA: 0x001CEC90 File Offset: 0x001CCE90
	// (remove) Token: 0x06004A54 RID: 19028 RVA: 0x001CECC8 File Offset: 0x001CCEC8
	public event QuestEvent_OpenContainer ContainerOpened;

	// Token: 0x06004A55 RID: 19029 RVA: 0x001CECFD File Offset: 0x001CCEFD
	public void OpenedContainer(Vector3i containerLocation, ITileEntityLootable tileEntity)
	{
		if (this.ContainerOpened != null)
		{
			this.ContainerOpened(containerLocation, tileEntity);
		}
	}

	// Token: 0x14000064 RID: 100
	// (add) Token: 0x06004A56 RID: 19030 RVA: 0x001CED14 File Offset: 0x001CCF14
	// (remove) Token: 0x06004A57 RID: 19031 RVA: 0x001CED4C File Offset: 0x001CCF4C
	public event QuestEvent_OpenContainer ContainerClosed;

	// Token: 0x06004A58 RID: 19032 RVA: 0x001CED81 File Offset: 0x001CCF81
	public void ClosedContainer(Vector3i containerLocation, ITileEntityLootable tileEntity)
	{
		if (this.ContainerClosed != null)
		{
			this.ContainerClosed(containerLocation, tileEntity);
		}
	}

	// Token: 0x14000065 RID: 101
	// (add) Token: 0x06004A59 RID: 19033 RVA: 0x001CED98 File Offset: 0x001CCF98
	// (remove) Token: 0x06004A5A RID: 19034 RVA: 0x001CEDD0 File Offset: 0x001CCFD0
	public event QuestEvent_EntityKillEvent EntityKill;

	// Token: 0x06004A5B RID: 19035 RVA: 0x001CEE05 File Offset: 0x001CD005
	public void EntityKilled(EntityAlive killedBy, EntityAlive killedEntity)
	{
		if (this.EntityKill != null && killedBy != null && killedEntity != null)
		{
			this.EntityKill(killedBy, killedEntity);
		}
	}

	// Token: 0x14000066 RID: 102
	// (add) Token: 0x06004A5C RID: 19036 RVA: 0x001CEE30 File Offset: 0x001CD030
	// (remove) Token: 0x06004A5D RID: 19037 RVA: 0x001CEE68 File Offset: 0x001CD068
	public event QuestEvent_NPCInteracted NPCInteract;

	// Token: 0x06004A5E RID: 19038 RVA: 0x001CEE9D File Offset: 0x001CD09D
	public void NPCInteracted(EntityNPC entityNPC)
	{
		if (this.NPCInteract != null)
		{
			this.NPCInteract(entityNPC);
		}
	}

	// Token: 0x14000067 RID: 103
	// (add) Token: 0x06004A5F RID: 19039 RVA: 0x001CEEB4 File Offset: 0x001CD0B4
	// (remove) Token: 0x06004A60 RID: 19040 RVA: 0x001CEEEC File Offset: 0x001CD0EC
	public event QuestEvent_NPCInteracted NPCMeet;

	// Token: 0x06004A61 RID: 19041 RVA: 0x001CEF21 File Offset: 0x001CD121
	public void NPCMet(EntityNPC entityNPC)
	{
		if (this.NPCMeet != null)
		{
			this.NPCMeet(entityNPC);
		}
	}

	// Token: 0x14000068 RID: 104
	// (add) Token: 0x06004A62 RID: 19042 RVA: 0x001CEF38 File Offset: 0x001CD138
	// (remove) Token: 0x06004A63 RID: 19043 RVA: 0x001CEF70 File Offset: 0x001CD170
	public event QuestEvent_SleepersCleared SleepersCleared;

	// Token: 0x06004A64 RID: 19044 RVA: 0x001CEFA5 File Offset: 0x001CD1A5
	public void ClearedSleepers(Vector3 prefabPos)
	{
		if (this.SleepersCleared != null)
		{
			this.SleepersCleared(prefabPos);
		}
	}

	// Token: 0x14000069 RID: 105
	// (add) Token: 0x06004A65 RID: 19045 RVA: 0x001CEFBC File Offset: 0x001CD1BC
	// (remove) Token: 0x06004A66 RID: 19046 RVA: 0x001CEFF4 File Offset: 0x001CD1F4
	public event QuestEvent_Explosion ExplosionDetected;

	// Token: 0x06004A67 RID: 19047 RVA: 0x001CF029 File Offset: 0x001CD229
	public void DetectedExplosion(Vector3 explosionPos, int entityID, float blockDamage)
	{
		if (this.ExplosionDetected != null)
		{
			this.ExplosionDetected(explosionPos, entityID, blockDamage);
		}
	}

	// Token: 0x1400006A RID: 106
	// (add) Token: 0x06004A68 RID: 19048 RVA: 0x001CF044 File Offset: 0x001CD244
	// (remove) Token: 0x06004A69 RID: 19049 RVA: 0x001CF07C File Offset: 0x001CD27C
	public event QuestEvent_PurchaseEvent BuyItems;

	// Token: 0x1400006B RID: 107
	// (add) Token: 0x06004A6A RID: 19050 RVA: 0x001CF0B4 File Offset: 0x001CD2B4
	// (remove) Token: 0x06004A6B RID: 19051 RVA: 0x001CF0EC File Offset: 0x001CD2EC
	public event QuestEvent_PurchaseEvent SellItems;

	// Token: 0x06004A6C RID: 19052 RVA: 0x001CF121 File Offset: 0x001CD321
	public void BoughtItems(string traderName, int itemCount)
	{
		if (this.BuyItems != null)
		{
			this.BuyItems(traderName, itemCount);
		}
	}

	// Token: 0x06004A6D RID: 19053 RVA: 0x001CF138 File Offset: 0x001CD338
	public void SoldItems(string traderName, int itemCount)
	{
		if (this.SellItems != null)
		{
			this.SellItems(traderName, itemCount);
		}
	}

	// Token: 0x1400006C RID: 108
	// (add) Token: 0x06004A6E RID: 19054 RVA: 0x001CF150 File Offset: 0x001CD350
	// (remove) Token: 0x06004A6F RID: 19055 RVA: 0x001CF188 File Offset: 0x001CD388
	public event QuestEvent_ChallengeCompleteEvent ChallengeComplete;

	// Token: 0x06004A70 RID: 19056 RVA: 0x001CF1BD File Offset: 0x001CD3BD
	public void ChallengeCompleted(ChallengeClass challenge, bool isRedeemed)
	{
		if (this.ChallengeComplete != null)
		{
			this.ChallengeComplete(challenge, isRedeemed);
		}
	}

	// Token: 0x1400006D RID: 109
	// (add) Token: 0x06004A71 RID: 19057 RVA: 0x001CF1D4 File Offset: 0x001CD3D4
	// (remove) Token: 0x06004A72 RID: 19058 RVA: 0x001CF20C File Offset: 0x001CD40C
	public event QuestEvent_TwitchEvent TwitchEventReceive;

	// Token: 0x06004A73 RID: 19059 RVA: 0x001CF241 File Offset: 0x001CD441
	public void TwitchEventReceived(TwitchObjectiveTypes actionType, string param)
	{
		if (this.TwitchEventReceive != null)
		{
			this.TwitchEventReceive(actionType, param);
		}
	}

	// Token: 0x1400006E RID: 110
	// (add) Token: 0x06004A74 RID: 19060 RVA: 0x001CF258 File Offset: 0x001CD458
	// (remove) Token: 0x06004A75 RID: 19061 RVA: 0x001CF290 File Offset: 0x001CD490
	public event QuestEvent_QuestCompleteEvent QuestComplete;

	// Token: 0x06004A76 RID: 19062 RVA: 0x001CF2C5 File Offset: 0x001CD4C5
	public void QuestCompleted(FastTags<TagGroup.Global> questTags, QuestClass questClass)
	{
		if (this.QuestComplete != null)
		{
			this.QuestComplete(questTags, questClass);
		}
	}

	// Token: 0x1400006F RID: 111
	// (add) Token: 0x06004A77 RID: 19063 RVA: 0x001CF2DC File Offset: 0x001CD4DC
	// (remove) Token: 0x06004A78 RID: 19064 RVA: 0x001CF314 File Offset: 0x001CD514
	public event QuestEvent_ChallengeAwardCredit ChallengeAwardCredit;

	// Token: 0x06004A79 RID: 19065 RVA: 0x001CF349 File Offset: 0x001CD549
	public void ChallengeAwardCredited(string challengeStat, int creditAmount)
	{
		if (this.ChallengeAwardCredit != null)
		{
			this.ChallengeAwardCredit(challengeStat, creditAmount);
		}
	}

	// Token: 0x14000070 RID: 112
	// (add) Token: 0x06004A7A RID: 19066 RVA: 0x001CF360 File Offset: 0x001CD560
	// (remove) Token: 0x06004A7B RID: 19067 RVA: 0x001CF398 File Offset: 0x001CD598
	public event QuestEvent_ChallengeAwardCredit QuestAwardCredit;

	// Token: 0x06004A7C RID: 19068 RVA: 0x001CF3CD File Offset: 0x001CD5CD
	public void QuestAwardCredited(string stat, int creditAmount)
	{
		if (this.QuestAwardCredit != null)
		{
			this.QuestAwardCredit(stat, creditAmount);
		}
	}

	// Token: 0x14000071 RID: 113
	// (add) Token: 0x06004A7D RID: 19069 RVA: 0x001CF3E4 File Offset: 0x001CD5E4
	// (remove) Token: 0x06004A7E RID: 19070 RVA: 0x001CF41C File Offset: 0x001CD61C
	public event QuestEvent_BiomeEvent BiomeEnter;

	// Token: 0x06004A7F RID: 19071 RVA: 0x001CF451 File Offset: 0x001CD651
	public void BiomeEntered(BiomeDefinition biomeDef)
	{
		if (this.BiomeEnter != null)
		{
			this.BiomeEnter(biomeDef);
		}
	}

	// Token: 0x14000072 RID: 114
	// (add) Token: 0x06004A80 RID: 19072 RVA: 0x001CF468 File Offset: 0x001CD668
	// (remove) Token: 0x06004A81 RID: 19073 RVA: 0x001CF4A0 File Offset: 0x001CD6A0
	public event QuestEvent_ItemValueActionEvent UseItem;

	// Token: 0x06004A82 RID: 19074 RVA: 0x001CF4D5 File Offset: 0x001CD6D5
	public void UsedItem(ItemValue newValue)
	{
		if (this.UseItem != null)
		{
			this.UseItem(newValue);
		}
	}

	// Token: 0x14000073 RID: 115
	// (add) Token: 0x06004A83 RID: 19075 RVA: 0x001CF4EC File Offset: 0x001CD6EC
	// (remove) Token: 0x06004A84 RID: 19076 RVA: 0x001CF524 File Offset: 0x001CD724
	public event QuestEvent_FloatEvent TimeSurvive;

	// Token: 0x06004A85 RID: 19077 RVA: 0x001CF559 File Offset: 0x001CD759
	public void TimeSurvived(float time)
	{
		if (this.TimeSurvive != null)
		{
			this.TimeSurvive(time);
		}
	}

	// Token: 0x14000074 RID: 116
	// (add) Token: 0x06004A86 RID: 19078 RVA: 0x001CF570 File Offset: 0x001CD770
	// (remove) Token: 0x06004A87 RID: 19079 RVA: 0x001CF5A8 File Offset: 0x001CD7A8
	public event QuestEvent_Event BloodMoonSurvive;

	// Token: 0x06004A88 RID: 19080 RVA: 0x001CF5DD File Offset: 0x001CD7DD
	public void BloodMoonSurvived()
	{
		if (this.BloodMoonSurvive != null)
		{
			this.BloodMoonSurvive();
		}
	}

	// Token: 0x06004A89 RID: 19081 RVA: 0x001CF5F4 File Offset: 0x001CD7F4
	public void Update()
	{
		ObjectiveRallyPoint.SetupFlags(this.objectivesToUpdate);
		float deltaTime = Time.deltaTime;
		for (int i = 0; i < this.objectivesToUpdate.Count; i++)
		{
			this.objectivesToUpdate[i].HandleUpdate(deltaTime);
		}
		for (int j = 0; j < this.challengeObjectivesToUpdate.Count; j++)
		{
			this.challengeObjectivesToUpdate[j].HandleUpdate(deltaTime);
		}
		for (int k = this.questTrackersToUpdate.Count - 1; k >= 0; k--)
		{
			if (!this.questTrackersToUpdate[k].Update(deltaTime))
			{
				this.questTrackersToUpdate.RemoveAt(k);
			}
		}
		if (this.challengeTrackerToUpdate != null && !this.challengeTrackerToUpdate.Update(deltaTime))
		{
			this.challengeTrackerToUpdate = null;
		}
		foreach (KeyValuePair<Vector3, SleeperEventData> keyValuePair in this.SleeperVolumeUpdateDictionary)
		{
			if (keyValuePair.Value.Update())
			{
				this.removeSleeperDataList.Add(keyValuePair.Value.position);
			}
		}
		for (int l = 0; l < this.removeSleeperDataList.Count; l++)
		{
			this.SleeperVolumeUpdateDictionary.Remove(this.removeSleeperDataList[l]);
		}
		this.removeSleeperDataList.Clear();
	}

	// Token: 0x06004A8A RID: 19082 RVA: 0x001CF75C File Offset: 0x001CD95C
	public void HandlePlayerDisconnect(EntityPlayer player)
	{
		for (int i = 0; i < player.QuestJournal.quests.Count; i++)
		{
			Quest quest = player.QuestJournal.quests[i];
			if (quest.CurrentState == Quest.QuestState.InProgress)
			{
				quest.HandleUnlockPOI(player);
				this.FinishTreasureQuest(quest.QuestCode, player);
			}
		}
	}

	// Token: 0x06004A8B RID: 19083 RVA: 0x001CF7B4 File Offset: 0x001CD9B4
	public void HandleAllPlayersDisconnect()
	{
		foreach (int num in this.TreasureQuestDictionary.Keys)
		{
			this.TreasureQuestDictionary[num].Remove();
		}
		this.TreasureQuestDictionary.Clear();
	}

	// Token: 0x06004A8C RID: 19084 RVA: 0x001CF824 File Offset: 0x001CDA24
	[PublicizedFrom(EAccessModifier.Internal)]
	public void AddTraderResetQuestsForPlayer(int playerID, int traderID)
	{
		if (!this.ForceResetQuestTrader.ContainsKey(playerID))
		{
			this.ForceResetQuestTrader.Add(playerID, traderID);
			return;
		}
		this.ForceResetQuestTrader[playerID] = traderID;
	}

	// Token: 0x06004A8D RID: 19085 RVA: 0x001CF84F File Offset: 0x001CDA4F
	public void ClearTraderResetQuestsForPlayer(int playerID)
	{
		if (this.ForceResetQuestTrader.ContainsKey(playerID))
		{
			this.ForceResetQuestTrader.Remove(playerID);
		}
	}

	// Token: 0x06004A8E RID: 19086 RVA: 0x001CF86C File Offset: 0x001CDA6C
	[PublicizedFrom(EAccessModifier.Private)]
	public bool CheckResetQuestTrader(int playerEntityID, int npcEntityID)
	{
		if (!this.ForceResetQuestTrader.ContainsKey(playerEntityID))
		{
			return false;
		}
		Log.Out(string.Format("CheckResetQuestTrader {0}", this.ForceResetQuestTrader[playerEntityID] == npcEntityID));
		return this.ForceResetQuestTrader[playerEntityID] == npcEntityID;
	}

	// Token: 0x06004A8F RID: 19087 RVA: 0x001CF8BB File Offset: 0x001CDABB
	public void AddObjectiveToBeUpdated(BaseObjective obj)
	{
		if (!this.objectivesToUpdate.Contains(obj))
		{
			this.objectivesToUpdate.Add(obj);
		}
	}

	// Token: 0x06004A90 RID: 19088 RVA: 0x001CF8D7 File Offset: 0x001CDAD7
	public void RemoveObjectiveToBeUpdated(BaseObjective obj)
	{
		if (this.objectivesToUpdate.Contains(obj))
		{
			this.objectivesToUpdate.Remove(obj);
		}
	}

	// Token: 0x06004A91 RID: 19089 RVA: 0x001CF8F4 File Offset: 0x001CDAF4
	public void AddObjectiveToBeUpdated(BaseChallengeObjective obj)
	{
		if (!this.challengeObjectivesToUpdate.Contains(obj))
		{
			this.challengeObjectivesToUpdate.Add(obj);
		}
	}

	// Token: 0x06004A92 RID: 19090 RVA: 0x001CF910 File Offset: 0x001CDB10
	public void RemoveObjectiveToBeUpdated(BaseChallengeObjective obj)
	{
		if (this.challengeObjectivesToUpdate.Contains(obj))
		{
			this.challengeObjectivesToUpdate.Remove(obj);
		}
	}

	// Token: 0x06004A93 RID: 19091 RVA: 0x001CF92D File Offset: 0x001CDB2D
	public void AddTrackerToBeUpdated(TrackingHandler track)
	{
		if (!this.questTrackersToUpdate.Contains(track))
		{
			this.questTrackersToUpdate.Add(track);
		}
	}

	// Token: 0x06004A94 RID: 19092 RVA: 0x001CF949 File Offset: 0x001CDB49
	public void RemoveTrackerToBeUpdated(TrackingHandler track)
	{
		if (this.questTrackersToUpdate.Contains(track))
		{
			this.questTrackersToUpdate.Remove(track);
		}
	}

	// Token: 0x06004A95 RID: 19093 RVA: 0x001CF966 File Offset: 0x001CDB66
	public void AddTrackerToBeUpdated(ChallengeTrackingHandler track)
	{
		this.challengeTrackerToUpdate = track;
	}

	// Token: 0x06004A96 RID: 19094 RVA: 0x001CF96F File Offset: 0x001CDB6F
	public void RemoveTrackerToBeUpdated(ChallengeTrackingHandler track)
	{
		this.challengeTrackerToUpdate = null;
	}

	// Token: 0x14000075 RID: 117
	// (add) Token: 0x06004A97 RID: 19095 RVA: 0x001CF978 File Offset: 0x001CDB78
	// (remove) Token: 0x06004A98 RID: 19096 RVA: 0x001CF9B0 File Offset: 0x001CDBB0
	public event QuestEvent_SleeperVolumePositionChanged SleeperVolumePositionAdd;

	// Token: 0x14000076 RID: 118
	// (add) Token: 0x06004A99 RID: 19097 RVA: 0x001CF9E8 File Offset: 0x001CDBE8
	// (remove) Token: 0x06004A9A RID: 19098 RVA: 0x001CFA20 File Offset: 0x001CDC20
	public event QuestEvent_SleeperVolumePositionChanged SleeperVolumePositionRemove;

	// Token: 0x06004A9B RID: 19099 RVA: 0x001CFA55 File Offset: 0x001CDC55
	public void SleeperVolumePositionAdded(Vector3 pos)
	{
		if (this.SleeperVolumePositionAdd != null)
		{
			this.SleeperVolumePositionAdd(pos);
		}
	}

	// Token: 0x06004A9C RID: 19100 RVA: 0x001CFA6B File Offset: 0x001CDC6B
	public void SleeperVolumePositionRemoved(Vector3 pos)
	{
		if (this.SleeperVolumePositionRemove != null)
		{
			this.SleeperVolumePositionRemove(pos);
		}
	}

	// Token: 0x06004A9D RID: 19101 RVA: 0x001CFA81 File Offset: 0x001CDC81
	public void AddSleeperVolumeLocation(Vector3 newLocation)
	{
		this.SleeperVolumeLocationList.Add(newLocation);
	}

	// Token: 0x06004A9E RID: 19102 RVA: 0x001CFA90 File Offset: 0x001CDC90
	public void SubscribeToUpdateEvent(int entityID, Vector3 prefabPos)
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			if (!this.SleeperVolumeUpdateDictionary.ContainsKey(prefabPos))
			{
				SleeperEventData sleeperEventData = new SleeperEventData();
				sleeperEventData.SetupData(prefabPos);
				this.SleeperVolumeUpdateDictionary.Add(prefabPos, sleeperEventData);
			}
			SleeperEventData sleeperEventData2 = this.SleeperVolumeUpdateDictionary[prefabPos];
			this.removeSleeperDataList.Remove(prefabPos);
			if (!sleeperEventData2.EntityList.Contains(entityID))
			{
				sleeperEventData2.EntityList.Add(entityID);
				return;
			}
		}
		else
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageQuestEvent>().Setup(NetPackageQuestEvent.QuestEventTypes.ClearSleeper, entityID, prefabPos, true), false);
		}
	}

	// Token: 0x06004A9F RID: 19103 RVA: 0x001CFB20 File Offset: 0x001CDD20
	public void UnSubscribeToUpdateEvent(int entityID, Vector3 prefabPos)
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			if (!this.SleeperVolumeUpdateDictionary.ContainsKey(prefabPos))
			{
				return;
			}
			SleeperEventData sleeperEventData = this.SleeperVolumeUpdateDictionary[prefabPos];
			if (!sleeperEventData.EntityList.Contains(entityID))
			{
				return;
			}
			sleeperEventData.EntityList.Remove(entityID);
			if (sleeperEventData.EntityList.Count == 0)
			{
				this.removeSleeperDataList.Add(prefabPos);
			}
			using (List<SleeperVolume>.Enumerator enumerator = sleeperEventData.SleeperVolumes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SleeperVolume sleeperVolume = enumerator.Current;
					QuestEventManager.Current.SleeperVolumePositionRemoved(sleeperVolume.Center);
				}
				return;
			}
		}
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageQuestEvent>().Setup(NetPackageQuestEvent.QuestEventTypes.ClearSleeper, entityID, prefabPos, false), false);
	}

	// Token: 0x06004AA0 RID: 19104 RVA: 0x001CFBF8 File Offset: 0x001CDDF8
	public IEnumerator QuestLockPOI(int entityID, QuestClass questClass, Vector3 prefabPos, FastTags<TagGroup.Global> questTags, int[] sharedWithList)
	{
		List<PrefabInstance> prefabsFromWorldPosInside = GameManager.Instance.GetDynamicPrefabDecorator().GetPrefabsFromWorldPosInside(prefabPos, questTags);
		for (int i = 0; i < prefabsFromWorldPosInside.Count; i++)
		{
			PrefabInstance prefabInstance = prefabsFromWorldPosInside[i];
			if (prefabInstance.prefab.GetQuestTag(questTags) && (prefabInstance.lockInstance == null || prefabInstance.lockInstance.CheckQuestLock()))
			{
				prefabInstance.lockInstance = new QuestLockInstance(entityID);
				if (sharedWithList != null)
				{
					prefabInstance.lockInstance.AddQuesters(sharedWithList);
				}
			}
		}
		yield return GameManager.Instance.World.ResetPOIS(prefabsFromWorldPosInside, questTags, entityID, sharedWithList, questClass);
		yield return null;
		if (GameManager.Instance.World.GetPrimaryPlayerId() != entityID)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageQuestEvent>().Setup(NetPackageQuestEvent.QuestEventTypes.POILocked, entityID), false, entityID, -1, -1, null, 192, false);
		}
		yield break;
	}

	// Token: 0x06004AA1 RID: 19105 RVA: 0x001CFC28 File Offset: 0x001CDE28
	public void QuestUnlockPOI(int entityID, Vector3 prefabPos)
	{
		PrefabInstance prefabFromWorldPos = GameManager.Instance.GetDynamicPrefabDecorator().GetPrefabFromWorldPos((int)prefabPos.x, (int)prefabPos.z);
		if (prefabFromWorldPos.lockInstance != null)
		{
			prefabFromWorldPos.lockInstance.RemoveQuester(entityID);
		}
	}

	// Token: 0x06004AA2 RID: 19106 RVA: 0x001CFC68 File Offset: 0x001CDE68
	public QuestEventManager.POILockoutReasonTypes CheckForPOILockouts(int entityId, Vector2 prefabPos, out ulong extraData)
	{
		World world = GameManager.Instance.World;
		PrefabInstance prefabFromWorldPos = GameManager.Instance.GetDynamicPrefabDecorator().GetPrefabFromWorldPos((int)prefabPos.x, (int)prefabPos.y);
		Rect rect = new Rect((float)prefabFromWorldPos.boundingBoxPosition.x, (float)prefabFromWorldPos.boundingBoxPosition.z, (float)prefabFromWorldPos.boundingBoxSize.x, (float)prefabFromWorldPos.boundingBoxSize.z);
		if (prefabFromWorldPos.lockInstance != null && prefabFromWorldPos.lockInstance.CheckQuestLock())
		{
			prefabFromWorldPos.lockInstance = null;
		}
		if (prefabFromWorldPos.lockInstance != null)
		{
			extraData = prefabFromWorldPos.lockInstance.LockedOutUntil;
			return QuestEventManager.POILockoutReasonTypes.QuestLock;
		}
		extraData = 0UL;
		EntityPlayer entityPlayer = (EntityPlayer)world.GetEntity(entityId);
		if (entityPlayer != null)
		{
			for (int i = 0; i < world.Players.list.Count; i++)
			{
				Vector3 position = world.Players.list[i].GetPosition();
				EntityPlayer entityPlayer2 = world.Players.list[i];
				if (entityPlayer != entityPlayer2 && (!entityPlayer.IsInParty() || !entityPlayer.Party.MemberList.Contains(entityPlayer2)) && rect.Contains(new Vector2(position.x, position.z)))
				{
					return QuestEventManager.POILockoutReasonTypes.PlayerInside;
				}
			}
		}
		GameUtils.EPlayerHomeType eplayerHomeType = prefabFromWorldPos.CheckForAnyPlayerHome(world);
		if (eplayerHomeType == GameUtils.EPlayerHomeType.Landclaim)
		{
			return QuestEventManager.POILockoutReasonTypes.LandClaim;
		}
		if (eplayerHomeType == GameUtils.EPlayerHomeType.Bedroll)
		{
			return QuestEventManager.POILockoutReasonTypes.Bedroll;
		}
		return QuestEventManager.POILockoutReasonTypes.None;
	}

	// Token: 0x06004AA3 RID: 19107 RVA: 0x001CFDD0 File Offset: 0x001CDFD0
	public void SetupRepairForMP(List<Vector3i> repairBlockList, List<bool> repairStates, World _world, Vector3 prefabPos)
	{
		PrefabInstance prefabFromWorldPos = GameManager.Instance.GetDynamicPrefabDecorator().GetPrefabFromWorldPos((int)prefabPos.x, (int)prefabPos.z);
		Vector3i vector3i = new Vector3i(prefabPos);
		Vector3i size = prefabFromWorldPos.prefab.size;
		int num = World.toChunkXZ(vector3i.x - 1);
		int num2 = World.toChunkXZ(vector3i.x + size.x + 1);
		int num3 = World.toChunkXZ(vector3i.z - 1);
		int num4 = World.toChunkXZ(vector3i.z + size.z + 1);
		repairBlockList.Clear();
		repairStates.Clear();
		Rect rect = new Rect((float)vector3i.x, (float)vector3i.z, (float)size.x, (float)size.z);
		for (int i = num; i <= num2; i++)
		{
			for (int j = num3; j <= num4; j++)
			{
				Chunk chunk = _world.GetChunkSync(i, j) as Chunk;
				if (chunk != null)
				{
					List<Vector3i> list = chunk.IndexedBlocks[Constants.cQuestRestorePowerIndexName];
					if (list != null)
					{
						for (int k = 0; k < list.Count; k++)
						{
							BlockValue block = chunk.GetBlock(list[k]);
							if (!block.ischild)
							{
								Vector3i vector3i2 = chunk.ToWorldPos(list[k]);
								if (rect.Contains(new Vector2((float)vector3i2.x, (float)vector3i2.z)))
								{
									repairStates.Add(!block.Block.UpgradeBlock.isair);
									repairBlockList.Add(vector3i2);
								}
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06004AA4 RID: 19108 RVA: 0x001CFF68 File Offset: 0x001CE168
	public void SetupActivateForMP(int entityID, int questCode, string completeEvent, List<Vector3i> activateBlockList, World _world, Vector3 prefabPos, string indexName, int[] sharedWithList)
	{
		PrefabInstance prefabFromWorldPos = GameManager.Instance.GetDynamicPrefabDecorator().GetPrefabFromWorldPos((int)prefabPos.x, (int)prefabPos.z);
		Vector3i vector3i = new Vector3i(prefabPos);
		Vector3i size = prefabFromWorldPos.prefab.size;
		EntityPlayer entityPlayer = _world.GetEntity(entityID) as EntityPlayer;
		int num = World.toChunkXZ(vector3i.x - 1);
		int num2 = World.toChunkXZ(vector3i.x + size.x + 1);
		int num3 = World.toChunkXZ(vector3i.z - 1);
		int num4 = World.toChunkXZ(vector3i.z + size.z + 1);
		activateBlockList.Clear();
		Rect rect = new Rect((float)vector3i.x, (float)vector3i.z, (float)size.x, (float)size.z);
		new BlockChangeInfo();
		List<BlockChangeInfo> list = new List<BlockChangeInfo>();
		for (int i = num; i <= num2; i++)
		{
			for (int j = num3; j <= num4; j++)
			{
				Chunk chunk = _world.GetChunkSync(i, j) as Chunk;
				if (chunk != null)
				{
					List<Vector3i> list2 = chunk.IndexedBlocks[indexName];
					if (list2 != null)
					{
						for (int k = 0; k < list2.Count; k++)
						{
							BlockValue block = chunk.GetBlock(list2[k]);
							if (!block.ischild)
							{
								Vector3i vector3i2 = chunk.ToWorldPos(list2[k]);
								if (rect.Contains(new Vector2((float)vector3i2.x, (float)vector3i2.z)))
								{
									activateBlockList.Add(vector3i2);
									if (block.Block is BlockQuestActivate)
									{
										(block.Block as BlockQuestActivate).SetupForQuest(_world, chunk, vector3i2, block, list);
									}
								}
							}
						}
					}
				}
			}
		}
		if (entityPlayer is EntityPlayerLocal)
		{
			entityPlayer.QuestJournal.HandleRestorePowerReceived(prefabPos, activateBlockList);
		}
		else
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageQuestEvent>().Setup(NetPackageQuestEvent.QuestEventTypes.SetupRestorePower, entityPlayer.entityId, questCode, completeEvent, prefabPos, activateBlockList), false, entityPlayer.entityId, -1, -1, null, 192, false);
		}
		QuestEventManager.Current.AddRestorePowerQuest(questCode, entityID, new Vector3i(prefabPos), completeEvent);
		if (entityPlayer.IsInParty() && sharedWithList != null)
		{
			Party party = entityPlayer.Party;
			for (int l = 0; l < sharedWithList.Length; l++)
			{
				EntityPlayer entityPlayer2 = _world.GetEntity(sharedWithList[l]) as EntityPlayer;
				if (entityPlayer2 is EntityPlayerLocal)
				{
					entityPlayer2.QuestJournal.HandleRestorePowerReceived(prefabPos, activateBlockList);
				}
				else
				{
					SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageQuestEvent>().Setup(NetPackageQuestEvent.QuestEventTypes.SetupRestorePower, entityPlayer2.entityId, questCode, completeEvent, prefabPos, activateBlockList), false, entityPlayer2.entityId, -1, -1, null, 192, false);
				}
				QuestEventManager.Current.AddRestorePowerQuest(questCode, sharedWithList[l], new Vector3i(prefabPos), completeEvent);
			}
		}
		if (list.Count > 0)
		{
			GameManager.Instance.StartCoroutine(this.UpdateBlocks(list));
		}
		GameEventManager.Current.HandleAction("quest_poi_lights_off", null, entityPlayer, false, vector3i, "", "", false, true, "", null, null);
	}

	// Token: 0x06004AA5 RID: 19109 RVA: 0x001D0290 File Offset: 0x001CE490
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEnumerator UpdateBlocks(List<BlockChangeInfo> blockChanges)
	{
		yield return new WaitForSeconds(1f);
		if (GameManager.Instance != null && GameManager.Instance.World != null)
		{
			GameManager.Instance.World.SetBlocksRPC(blockChanges);
		}
		yield break;
	}

	// Token: 0x06004AA6 RID: 19110 RVA: 0x001D02A0 File Offset: 0x001CE4A0
	public void SetupFetchForMP(int entityID, Vector3 prefabPos, ObjectiveFetchFromContainer.FetchModeTypes fetchMode, int[] sharedWithList)
	{
		PrefabInstance prefabFromWorldPos = GameManager.Instance.GetDynamicPrefabDecorator().GetPrefabFromWorldPos((int)prefabPos.x, (int)prefabPos.z);
		this.HandleContainerPositions(GameManager.Instance.World, entityID, new Vector3i(prefabPos), prefabFromWorldPos.prefab.size, fetchMode, sharedWithList);
	}

	// Token: 0x06004AA7 RID: 19111 RVA: 0x001D02F0 File Offset: 0x001CE4F0
	public void HandleContainerPositions(World _world, int _entityID, Vector3i _prefabPosition, Vector3i _prefabSize, ObjectiveFetchFromContainer.FetchModeTypes fetchMode, int[] sharedWithList)
	{
		int num = World.toChunkXZ(_prefabPosition.x - 1);
		int num2 = World.toChunkXZ(_prefabPosition.x + _prefabSize.x + 1);
		int num3 = World.toChunkXZ(_prefabPosition.z - 1);
		int num4 = World.toChunkXZ(_prefabPosition.z + _prefabSize.z + 1);
		List<Vector3i> list = new List<Vector3i>();
		Rect rect = new Rect((float)_prefabPosition.x, (float)_prefabPosition.z, (float)_prefabSize.x, (float)_prefabSize.z);
		for (int i = num; i <= num2; i++)
		{
			for (int j = num3; j <= num4; j++)
			{
				Chunk chunk = _world.GetChunkSync(i, j) as Chunk;
				if (chunk != null)
				{
					List<Vector3i> list2 = chunk.IndexedBlocks[Constants.cQuestLootFetchContainerIndexName];
					if (list2 != null)
					{
						for (int k = 0; k < list2.Count; k++)
						{
							if (!chunk.GetBlock(list2[k]).ischild)
							{
								Vector3i vector3i = chunk.ToWorldPos(list2[k]);
								if (rect.Contains(new Vector2((float)vector3i.x, (float)vector3i.z)))
								{
									list.Add(vector3i);
								}
							}
						}
					}
				}
			}
		}
		if (list.Count == 0)
		{
			Log.Error("Valid container not found for fetch loot.");
			return;
		}
		List<int> list3 = new List<int>();
		EntityPlayer entityPlayer = _world.GetEntity(_entityID) as EntityPlayer;
		Quest.PositionDataTypes positionDataTypes = ((fetchMode == ObjectiveFetchFromContainer.FetchModeTypes.Standard) ? Quest.PositionDataTypes.FetchContainer : Quest.PositionDataTypes.HiddenCache);
		int num5 = _world.GetGameRandom().RandomRange(list.Count);
		if (entityPlayer is EntityPlayerLocal)
		{
			entityPlayer.QuestJournal.SetActivePositionData(positionDataTypes, list[num5]);
		}
		else
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageQuestEvent>().Setup(NetPackageQuestEvent.QuestEventTypes.SetupFetch, _entityID, list[num5].ToVector3(), fetchMode), false, -1, -1, -1, null, 192, false);
		}
		list3.Add(num5);
		if (entityPlayer.IsInParty() && sharedWithList != null)
		{
			Party party = entityPlayer.Party;
			for (int l = 0; l < sharedWithList.Length; l++)
			{
				entityPlayer = _world.GetEntity(sharedWithList[l]) as EntityPlayer;
				num5 = _world.GetGameRandom().RandomRange(list.Count);
				if (entityPlayer is EntityPlayerLocal)
				{
					entityPlayer.QuestJournal.SetActivePositionData(positionDataTypes, list[num5]);
				}
				else
				{
					SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageQuestEvent>().Setup(NetPackageQuestEvent.QuestEventTypes.SetupFetch, entityPlayer.entityId, list[num5].ToVector3(), fetchMode), false, -1, -1, -1, null, 192, false);
				}
				if (!list3.Contains(num5))
				{
					list3.Add(num5);
				}
			}
		}
		List<BlockChangeInfo> list4 = new List<BlockChangeInfo>();
		GameRandom gameRandom = GameManager.Instance.World.GetGameRandom();
		for (int m = 0; m < list.Count; m++)
		{
			if (!list3.Contains(m))
			{
				Chunk chunk2 = (Chunk)_world.GetChunkFromWorldPos(list[m]);
				BlockValue blockValue = BlockPlaceholderMap.Instance.Replace(Block.GetBlockValue("cntQuestRandomLootHelper", false), gameRandom, chunk2, list[m].x, 0, list[m].z, FastTags<TagGroup.Global>.none, false, true);
				list4.Add(new BlockChangeInfo(list[m], blockValue));
			}
		}
		if (list4.Count > 0)
		{
			GameManager.Instance.StartCoroutine(this.UpdateBlocks(list4));
		}
	}

	// Token: 0x06004AA8 RID: 19112 RVA: 0x001D0678 File Offset: 0x001CE878
	public void Cleanup()
	{
		this.BlockPickup = null;
		this.BlockPlace = null;
		this.BlockUpgrade = null;
		this.AddItem = null;
		this.AssembleItem = null;
		this.CraftItem = null;
		this.ExchangeFromItem = null;
		this.ScrapItem = null;
		this.RepairItem = null;
		this.SkillPointSpent = null;
		this.WearItem = null;
		this.WindowChanged = null;
		this.ContainerOpened = null;
		this.EntityKill = null;
		this.HarvestItem = null;
		this.SellItems = null;
		this.BuyItems = null;
		this.ExplosionDetected = null;
		this.ChallengeComplete = null;
		this.BiomeEnter = null;
		this.UseItem = null;
		this.TimeSurvive = null;
		this.BloodMoonSurvive = null;
		this.objectivesToUpdate = null;
		this.npcQuestData.Clear();
		this.npcQuestData = null;
		this.questTierRewards.Clear();
		this.questTierRewards = null;
		QuestEventManager.instance = null;
	}

	// Token: 0x06004AA9 RID: 19113 RVA: 0x001D0758 File Offset: 0x001CE958
	public void SetupQuestList(EntityTrader npc, int playerEntityID, List<Quest> questList)
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
		{
			return;
		}
		if (!this.npcQuestData.ContainsKey(npc.entityId))
		{
			this.npcQuestData.Add(npc.entityId, new NPCQuestData());
		}
		if (!this.npcQuestData[npc.entityId].PlayerQuestList.ContainsKey(playerEntityID))
		{
			this.npcQuestData[npc.entityId].PlayerQuestList.Add(playerEntityID, new NPCQuestData.PlayerQuestData(questList));
		}
		else
		{
			this.npcQuestData[npc.entityId].PlayerQuestList[playerEntityID].QuestList = questList;
		}
		if (!(GameManager.Instance.World.GetEntity(playerEntityID) is EntityPlayerLocal))
		{
			NetPackageNPCQuestList.SendQuestPacketsToPlayer(npc, playerEntityID);
		}
	}

	// Token: 0x06004AAA RID: 19114 RVA: 0x001D0820 File Offset: 0x001CEA20
	public List<Quest> GetQuestList(World world, int npcEntityID, int playerEntityID)
	{
		if (this.npcQuestData.ContainsKey(npcEntityID))
		{
			NPCQuestData npcquestData = this.npcQuestData[npcEntityID];
			if (npcquestData.PlayerQuestList.ContainsKey(playerEntityID))
			{
				NPCQuestData.PlayerQuestData playerQuestData = npcquestData.PlayerQuestList[playerEntityID];
				if (QuestEventManager.Current.CheckResetQuestTrader(playerEntityID, npcEntityID))
				{
					playerQuestData.QuestList.Clear();
					playerQuestData.QuestList = null;
					QuestEventManager.Current.ClearTraderResetQuestsForPlayer(playerEntityID);
				}
				else if ((int)(world.GetWorldTime() - playerQuestData.LastUpdate) > 24000)
				{
					playerQuestData.QuestList.Clear();
					playerQuestData.QuestList = null;
				}
				return playerQuestData.QuestList;
			}
		}
		return null;
	}

	// Token: 0x06004AAB RID: 19115 RVA: 0x001D08C2 File Offset: 0x001CEAC2
	public void ClearQuestList(int npcEntityID)
	{
		if (this.npcQuestData.ContainsKey(npcEntityID))
		{
			this.npcQuestData[npcEntityID].PlayerQuestList.Clear();
		}
	}

	// Token: 0x06004AAC RID: 19116 RVA: 0x001D08E8 File Offset: 0x001CEAE8
	public void ClearQuestListForPlayer(int npcEntityID, int playerID)
	{
		if (this.npcQuestData.ContainsKey(npcEntityID))
		{
			NPCQuestData npcquestData = this.npcQuestData[npcEntityID];
			if (npcquestData.PlayerQuestList.ContainsKey(playerID))
			{
				npcquestData.PlayerQuestList.Remove(playerID);
			}
		}
	}

	// Token: 0x06004AAD RID: 19117 RVA: 0x001D092B File Offset: 0x001CEB2B
	public void AddQuestTierReward(QuestTierReward reward)
	{
		if (this.questTierRewards == null)
		{
			this.questTierRewards = new List<QuestTierReward>();
		}
		this.questTierRewards.Add(reward);
	}

	// Token: 0x06004AAE RID: 19118 RVA: 0x001D094C File Offset: 0x001CEB4C
	public void HandleNewCompletedQuest(EntityPlayer player, byte questFaction, int completedQuestTier, bool addsToTierComplete)
	{
		if (addsToTierComplete)
		{
			int currentFactionTier = player.QuestJournal.GetCurrentFactionTier(questFaction, 0, true);
			int currentFactionTier2 = player.QuestJournal.GetCurrentFactionTier(questFaction, completedQuestTier, true);
			if (currentFactionTier != currentFactionTier2)
			{
				for (int i = 0; i < this.questTierRewards.Count; i++)
				{
					if (this.questTierRewards[i].Tier == currentFactionTier2)
					{
						this.questTierRewards[i].GiveRewards(player);
					}
				}
			}
		}
	}

	// Token: 0x06004AAF RID: 19119 RVA: 0x001D09BC File Offset: 0x001CEBBC
	public void HandleRallyMarkerActivate(EntityPlayerLocal _player, Vector3i blockPos, BlockValue blockValue)
	{
		Quest quest = _player.QuestJournal.HasQuestAtRallyPosition(blockPos.ToVector3(), true);
		if (quest != null)
		{
			Action action = delegate
			{
				QuestEventManager.Current.BlockActivated(blockValue.Block.GetBlockName(), blockPos);
			};
			if (_player.IsInParty())
			{
				List<EntityPlayer> sharedWithListNotInRange = quest.GetSharedWithListNotInRange();
				if (sharedWithListNotInRange != null && sharedWithListNotInRange.Count > 0)
				{
					StringBuilder stringBuilder = new StringBuilder();
					for (int i = 0; i < sharedWithListNotInRange.Count; i++)
					{
						stringBuilder.Append(sharedWithListNotInRange[i].PlayerDisplayName);
						if (i < sharedWithListNotInRange.Count - 1)
						{
							stringBuilder.Append(", ");
						}
					}
					XUiC_MessageBoxWindowGroup.ShowOkCancel(_player.PlayerUI.xui, "Rally Activate", string.Format(Localization.Get("xuiQuestRallyOutOfRange", false, null), stringBuilder.ToString().Trim(',')), "", action, null, false, true, false);
					return;
				}
				action();
				return;
			}
			else
			{
				action();
			}
		}
	}

	// Token: 0x06004AB0 RID: 19120 RVA: 0x001D0AC4 File Offset: 0x001CECC4
	public void AddTreasureQuest(int _questCode, int _entityID, int _blocksPerReduction, Vector3i _position, Vector3 _treasureOffset)
	{
		if (!this.TreasureQuestDictionary.ContainsKey(_questCode))
		{
			TreasureQuestData treasureQuestData = new TreasureQuestData(_questCode, _entityID, _blocksPerReduction, _position, _treasureOffset);
			this.TreasureQuestDictionary.Add(_questCode, treasureQuestData);
		}
	}

	// Token: 0x06004AB1 RID: 19121 RVA: 0x001D0AF9 File Offset: 0x001CECF9
	public void SetTreasureContainerPosition(int _questCode, Vector3i _updatedPosition)
	{
		if (this.TreasureQuestDictionary.ContainsKey(_questCode))
		{
			this.TreasureQuestDictionary[_questCode].UpdatePosition(_updatedPosition);
		}
	}

	// Token: 0x06004AB2 RID: 19122 RVA: 0x001D0B1C File Offset: 0x001CED1C
	public bool GetTreasureContainerPosition(int _questCode, float _distance, int _offset, float _treasureRadius, Vector3 _startPosition, int _entityID, bool _useNearby, int _currentBlocksPerReduction, out int _blocksPerReduction, out Vector3i _position, out Vector3 _treasureOffset)
	{
		_position = Vector3i.zero;
		_treasureOffset = Vector3.zero;
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageQuestTreasurePoint>().Setup(_questCode, _distance, _offset, _treasureRadius, _startPosition, _entityID, _useNearby, _currentBlocksPerReduction), false);
			_position = Vector3i.zero;
			_treasureOffset = Vector3.zero;
			_blocksPerReduction = _currentBlocksPerReduction;
			return true;
		}
		if (this.TreasureQuestDictionary.ContainsKey(_questCode))
		{
			_position = this.TreasureQuestDictionary[_questCode].Position;
			_treasureOffset = this.TreasureQuestDictionary[_questCode].TreasureOffset;
			this.TreasureQuestDictionary[_questCode].AddSharedQuester(_entityID, _currentBlocksPerReduction);
			_blocksPerReduction = this.TreasureQuestDictionary[_questCode].BlocksPerReduction;
			return true;
		}
		_blocksPerReduction = _currentBlocksPerReduction;
		float num = _distance + 25f;
		for (float num2 = _distance; num2 < num; num2 += 2.5f)
		{
			for (int i = 0; i < 5; i++)
			{
				if (ObjectiveTreasureChest.CalculateTreasurePoint(_startPosition, num2, _offset, _treasureRadius - 1f, _useNearby, out _position, out _treasureOffset))
				{
					this.AddTreasureQuest(_questCode, _entityID, _blocksPerReduction, _position, _treasureOffset);
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06004AB3 RID: 19123 RVA: 0x001D0C60 File Offset: 0x001CEE60
	public void UpdateTreasureBlocksPerReduction(int _questCode, int _newBlocksPerReduction)
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			if (this.TreasureQuestDictionary.ContainsKey(_questCode))
			{
				this.TreasureQuestDictionary[_questCode].SendBlocksPerReductionUpdate(_newBlocksPerReduction);
				return;
			}
		}
		else
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageQuestTreasurePoint>().Setup(_questCode, _newBlocksPerReduction), false);
		}
	}

	// Token: 0x06004AB4 RID: 19124 RVA: 0x001D0CB4 File Offset: 0x001CEEB4
	public void FinishTreasureQuest(int _questCode, EntityPlayer _player)
	{
		TreasureQuestData treasureQuestData;
		if (this.TreasureQuestDictionary.TryGetValue(_questCode, out treasureQuestData))
		{
			treasureQuestData.RemoveSharedQuester(_player);
			ChunkProviderGenerateWorld chunkProviderGenerateWorld = GameManager.Instance.World.ChunkCache.ChunkProvider as ChunkProviderGenerateWorld;
			if (chunkProviderGenerateWorld != null)
			{
				Debug.Log(string.Format("[FinishTreasureQuest] Requesting reset at world position: {0}", treasureQuestData.Position));
				Vector2i vector2i = World.toChunkXZ(treasureQuestData.Position);
				for (int i = vector2i.x - 1; i <= vector2i.x + 1; i++)
				{
					for (int j = vector2i.y - 1; j <= vector2i.y + 1; j++)
					{
						long num = WorldChunkCache.MakeChunkKey(i, j);
						chunkProviderGenerateWorld.RequestChunkReset(num);
					}
				}
			}
		}
	}

	// Token: 0x06004AB5 RID: 19125 RVA: 0x001D0D68 File Offset: 0x001CEF68
	public void AddRestorePowerQuest(int _questCode, int _entityID, Vector3i _position, string _completeEvent)
	{
		if (!this.BlockActivateQuestDictionary.ContainsKey(_questCode))
		{
			RestorePowerQuestData restorePowerQuestData = new RestorePowerQuestData(_questCode, _entityID, _position, _completeEvent);
			this.BlockActivateQuestDictionary.Add(_questCode, restorePowerQuestData);
			return;
		}
		this.BlockActivateQuestDictionary[_questCode].AddSharedQuester(_entityID);
	}

	// Token: 0x06004AB6 RID: 19126 RVA: 0x001D0DAE File Offset: 0x001CEFAE
	public void FinishManagedQuest(int _questCode, EntityPlayer _player)
	{
		if (this.BlockActivateQuestDictionary.ContainsKey(_questCode))
		{
			this.BlockActivateQuestDictionary[_questCode].RemoveSharedQuester(_player);
		}
	}

	// Token: 0x04003A88 RID: 14984
	[PublicizedFrom(EAccessModifier.Private)]
	public static QuestEventManager instance = null;

	// Token: 0x04003A89 RID: 14985
	[PublicizedFrom(EAccessModifier.Private)]
	public List<BaseObjective> objectivesToUpdate = new List<BaseObjective>();

	// Token: 0x04003A8A RID: 14986
	[PublicizedFrom(EAccessModifier.Private)]
	public List<BaseChallengeObjective> challengeObjectivesToUpdate = new List<BaseChallengeObjective>();

	// Token: 0x04003A8B RID: 14987
	[PublicizedFrom(EAccessModifier.Private)]
	public List<TrackingHandler> questTrackersToUpdate = new List<TrackingHandler>();

	// Token: 0x04003A8C RID: 14988
	[PublicizedFrom(EAccessModifier.Private)]
	public ChallengeTrackingHandler challengeTrackerToUpdate;

	// Token: 0x04003A8D RID: 14989
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Vector3> removeSleeperDataList = new List<Vector3>();

	// Token: 0x04003A8E RID: 14990
	public Dictionary<int, NPCQuestData> npcQuestData = new Dictionary<int, NPCQuestData>();

	// Token: 0x04003A8F RID: 14991
	public List<QuestTierReward> questTierRewards = new List<QuestTierReward>();

	// Token: 0x04003A90 RID: 14992
	public Dictionary<Vector3, SleeperEventData> SleeperVolumeUpdateDictionary = new Dictionary<Vector3, SleeperEventData>();

	// Token: 0x04003A91 RID: 14993
	public List<Vector3> SleeperVolumeLocationList = new List<Vector3>();

	// Token: 0x04003A92 RID: 14994
	public Dictionary<int, TreasureQuestData> TreasureQuestDictionary = new Dictionary<int, TreasureQuestData>();

	// Token: 0x04003A93 RID: 14995
	public Dictionary<int, RestorePowerQuestData> BlockActivateQuestDictionary = new Dictionary<int, RestorePowerQuestData>();

	// Token: 0x04003A94 RID: 14996
	public Dictionary<int, List<PrefabInstance>> tierPrefabList = new Dictionary<int, List<PrefabInstance>>();

	// Token: 0x04003A95 RID: 14997
	public Dictionary<TraderArea, List<QuestEventManager.PrefabListData>> TraderPrefabList = new Dictionary<TraderArea, List<QuestEventManager.PrefabListData>>();

	// Token: 0x04003A96 RID: 14998
	public Rect QuestBounds;

	// Token: 0x04003A97 RID: 14999
	public List<Vector3i> ActiveQuestBlocks = new List<Vector3i>();

	// Token: 0x04003A98 RID: 15000
	public Dictionary<int, int> ForceResetQuestTrader = new Dictionary<int, int>();

	// Token: 0x04003A99 RID: 15001
	public static FastTags<TagGroup.Global> manualResetTag = FastTags<TagGroup.Global>.Parse("manual");

	// Token: 0x04003A9A RID: 15002
	public static FastTags<TagGroup.Global> traderTag = FastTags<TagGroup.Global>.Parse("trader");

	// Token: 0x04003A9B RID: 15003
	public static FastTags<TagGroup.Global> clearTag = FastTags<TagGroup.Global>.Parse("clear");

	// Token: 0x04003A9C RID: 15004
	public static FastTags<TagGroup.Global> treasureTag = FastTags<TagGroup.Global>.Parse("treasure");

	// Token: 0x04003A9D RID: 15005
	public static FastTags<TagGroup.Global> fetchTag = FastTags<TagGroup.Global>.Parse("fetch");

	// Token: 0x04003A9E RID: 15006
	public static FastTags<TagGroup.Global> craftingTag = FastTags<TagGroup.Global>.Parse("crafting");

	// Token: 0x04003A9F RID: 15007
	public static FastTags<TagGroup.Global> restorePowerTag = FastTags<TagGroup.Global>.Parse("restore_power");

	// Token: 0x04003AA0 RID: 15008
	public static FastTags<TagGroup.Global> infestedTag = FastTags<TagGroup.Global>.Parse("infested");

	// Token: 0x04003AA1 RID: 15009
	public static FastTags<TagGroup.Global> banditTag = FastTags<TagGroup.Global>.Parse("bandit");

	// Token: 0x04003AA2 RID: 15010
	public static FastTags<TagGroup.Global> allQuestTags = FastTags<TagGroup.Global>.CombineTags(FastTags<TagGroup.Global>.CombineTags(QuestEventManager.traderTag, QuestEventManager.clearTag, QuestEventManager.treasureTag, QuestEventManager.fetchTag), FastTags<TagGroup.Global>.CombineTags(QuestEventManager.craftingTag, QuestEventManager.restorePowerTag), FastTags<TagGroup.Global>.CombineTags(QuestEventManager.infestedTag, QuestEventManager.banditTag));

	// Token: 0x04003AA3 RID: 15011
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cTreasurePointAttempts = 5;

	// Token: 0x04003AA4 RID: 15012
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cTreasurePointDistanceAdd = 2.5f;

	// Token: 0x04003AA5 RID: 15013
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cTreasurePointMaxDistanceAdd = 25f;

	// Token: 0x020009CB RID: 2507
	public enum POILockoutReasonTypes
	{
		// Token: 0x04003ACC RID: 15052
		None,
		// Token: 0x04003ACD RID: 15053
		PlayerInside,
		// Token: 0x04003ACE RID: 15054
		Bedroll,
		// Token: 0x04003ACF RID: 15055
		LandClaim,
		// Token: 0x04003AD0 RID: 15056
		QuestLock
	}

	// Token: 0x020009CC RID: 2508
	public class PrefabListData
	{
		// Token: 0x06004AB8 RID: 19128 RVA: 0x001D0EAC File Offset: 0x001CF0AC
		public void AddPOI(PrefabInstance poi)
		{
			int difficultyTier = (int)poi.prefab.DifficultyTier;
			if (!this.TierData.ContainsKey(difficultyTier))
			{
				this.TierData.Add(difficultyTier, new List<PrefabInstance>());
			}
			this.TierData[difficultyTier].Add(poi);
		}

		// Token: 0x06004AB9 RID: 19129 RVA: 0x001D0EF8 File Offset: 0x001CF0F8
		public void ShuffleDifficulty(int difficulty, GameRandom gameRandom)
		{
			List<PrefabInstance> list;
			if (this.TierData.TryGetValue(difficulty, out list))
			{
				list.Shuffle<PrefabInstance>(gameRandom);
			}
		}

		// Token: 0x04003AD1 RID: 15057
		public Dictionary<int, List<PrefabInstance>> TierData = new Dictionary<int, List<PrefabInstance>>();
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Challenges;
using Quests;
using UnityEngine;

// Token: 0x020009DB RID: 2523
public class QuestEventManager
{
	// Token: 0x170007FF RID: 2047
	// (get) Token: 0x06004A9F RID: 19103 RVA: 0x001D07EB File Offset: 0x001CE9EB
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

	// Token: 0x17000800 RID: 2048
	// (get) Token: 0x06004AA0 RID: 19104 RVA: 0x001D0803 File Offset: 0x001CEA03
	public static bool HasInstance
	{
		get
		{
			return QuestEventManager.instance != null;
		}
	}

	// Token: 0x06004AA1 RID: 19105 RVA: 0x001D0810 File Offset: 0x001CEA10
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

	// Token: 0x06004AA2 RID: 19106 RVA: 0x001D0900 File Offset: 0x001CEB00
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

	// Token: 0x06004AA3 RID: 19107 RVA: 0x001D095C File Offset: 0x001CEB5C
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

	// Token: 0x06004AA4 RID: 19108 RVA: 0x001D09B0 File Offset: 0x001CEBB0
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

	// Token: 0x06004AA5 RID: 19109 RVA: 0x001D0A78 File Offset: 0x001CEC78
	[PublicizedFrom(EAccessModifier.Private)]
	public QuestEventManager()
	{
	}

	// Token: 0x14000052 RID: 82
	// (add) Token: 0x06004AA6 RID: 19110 RVA: 0x001D0B28 File Offset: 0x001CED28
	// (remove) Token: 0x06004AA7 RID: 19111 RVA: 0x001D0B60 File Offset: 0x001CED60
	public event QuestEvent_BlockEvent BlockActivate;

	// Token: 0x06004AA8 RID: 19112 RVA: 0x001D0B95 File Offset: 0x001CED95
	public void BlockActivated(string blockName, Vector3i blockPos)
	{
		if (this.BlockActivate != null)
		{
			this.BlockActivate(blockName, blockPos);
		}
	}

	// Token: 0x14000053 RID: 83
	// (add) Token: 0x06004AA9 RID: 19113 RVA: 0x001D0BAC File Offset: 0x001CEDAC
	// (remove) Token: 0x06004AAA RID: 19114 RVA: 0x001D0BE4 File Offset: 0x001CEDE4
	public event QuestEvent_BlockChangedEvent BlockChange;

	// Token: 0x06004AAB RID: 19115 RVA: 0x001D0C19 File Offset: 0x001CEE19
	public void BlockChanged(Block blockOld, Block blockNew, Vector3i blockPos)
	{
		if (this.BlockChange != null)
		{
			this.BlockChange(blockOld, blockNew, blockPos);
		}
	}

	// Token: 0x14000054 RID: 84
	// (add) Token: 0x06004AAC RID: 19116 RVA: 0x001D0C34 File Offset: 0x001CEE34
	// (remove) Token: 0x06004AAD RID: 19117 RVA: 0x001D0C6C File Offset: 0x001CEE6C
	public event QuestEvent_BlockDestroyEvent BlockDestroy;

	// Token: 0x06004AAE RID: 19118 RVA: 0x001D0CA4 File Offset: 0x001CEEA4
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
	// (add) Token: 0x06004AAF RID: 19119 RVA: 0x001D0D30 File Offset: 0x001CEF30
	// (remove) Token: 0x06004AB0 RID: 19120 RVA: 0x001D0D68 File Offset: 0x001CEF68
	public event QuestEvent_BlockEvent BlockPickup;

	// Token: 0x06004AB1 RID: 19121 RVA: 0x001D0D9D File Offset: 0x001CEF9D
	public void BlockPickedUp(string blockName, Vector3i blockPos)
	{
		if (this.BlockPickup != null)
		{
			this.BlockPickup(blockName, blockPos);
		}
	}

	// Token: 0x14000056 RID: 86
	// (add) Token: 0x06004AB2 RID: 19122 RVA: 0x001D0DB4 File Offset: 0x001CEFB4
	// (remove) Token: 0x06004AB3 RID: 19123 RVA: 0x001D0DEC File Offset: 0x001CEFEC
	public event QuestEvent_BlockEvent BlockPlace;

	// Token: 0x06004AB4 RID: 19124 RVA: 0x001D0E21 File Offset: 0x001CF021
	public void BlockPlaced(string blockName, Vector3i blockPos)
	{
		if (this.BlockPlace != null)
		{
			this.BlockPlace(blockName, blockPos);
		}
	}

	// Token: 0x14000057 RID: 87
	// (add) Token: 0x06004AB5 RID: 19125 RVA: 0x001D0E38 File Offset: 0x001CF038
	// (remove) Token: 0x06004AB6 RID: 19126 RVA: 0x001D0E70 File Offset: 0x001CF070
	public event QuestEvent_BlockEvent BlockUpgrade;

	// Token: 0x06004AB7 RID: 19127 RVA: 0x001D0EA5 File Offset: 0x001CF0A5
	public void BlockUpgraded(string blockName, Vector3i blockPos)
	{
		if (this.BlockUpgrade != null)
		{
			this.BlockUpgrade(blockName, blockPos);
		}
	}

	// Token: 0x14000058 RID: 88
	// (add) Token: 0x06004AB8 RID: 19128 RVA: 0x001D0EBC File Offset: 0x001CF0BC
	// (remove) Token: 0x06004AB9 RID: 19129 RVA: 0x001D0EF4 File Offset: 0x001CF0F4
	public event QuestEvent_ItemStackActionEvent AddItem;

	// Token: 0x06004ABA RID: 19130 RVA: 0x001D0F29 File Offset: 0x001CF129
	public void ItemAdded(ItemStack newStack)
	{
		if (this.AddItem != null)
		{
			this.AddItem(newStack);
		}
	}

	// Token: 0x14000059 RID: 89
	// (add) Token: 0x06004ABB RID: 19131 RVA: 0x001D0F40 File Offset: 0x001CF140
	// (remove) Token: 0x06004ABC RID: 19132 RVA: 0x001D0F78 File Offset: 0x001CF178
	public event QuestEvent_HarvestStackActionEvent HarvestItem;

	// Token: 0x06004ABD RID: 19133 RVA: 0x001D0FAD File Offset: 0x001CF1AD
	public void HarvestedItem(ItemValue heldItem, ItemStack newStack, BlockValue bv)
	{
		if (this.HarvestItem != null)
		{
			this.HarvestItem(heldItem, newStack, bv);
		}
	}

	// Token: 0x1400005A RID: 90
	// (add) Token: 0x06004ABE RID: 19134 RVA: 0x001D0FC8 File Offset: 0x001CF1C8
	// (remove) Token: 0x06004ABF RID: 19135 RVA: 0x001D1000 File Offset: 0x001CF200
	public event QuestEvent_ItemStackActionEvent AssembleItem;

	// Token: 0x06004AC0 RID: 19136 RVA: 0x001D1035 File Offset: 0x001CF235
	public void AssembledItem(ItemStack newStack)
	{
		if (this.AssembleItem != null)
		{
			this.AssembleItem(newStack);
		}
	}

	// Token: 0x1400005B RID: 91
	// (add) Token: 0x06004AC1 RID: 19137 RVA: 0x001D104C File Offset: 0x001CF24C
	// (remove) Token: 0x06004AC2 RID: 19138 RVA: 0x001D1084 File Offset: 0x001CF284
	public event QuestEvent_ItemStackActionEvent CraftItem;

	// Token: 0x06004AC3 RID: 19139 RVA: 0x001D10B9 File Offset: 0x001CF2B9
	public void CraftedItem(ItemStack newStack)
	{
		if (this.CraftItem != null)
		{
			this.CraftItem(newStack);
		}
	}

	// Token: 0x1400005C RID: 92
	// (add) Token: 0x06004AC4 RID: 19140 RVA: 0x001D10D0 File Offset: 0x001CF2D0
	// (remove) Token: 0x06004AC5 RID: 19141 RVA: 0x001D1108 File Offset: 0x001CF308
	public event QuestEvent_ItemStackActionEvent ExchangeFromItem;

	// Token: 0x06004AC6 RID: 19142 RVA: 0x001D113D File Offset: 0x001CF33D
	public void ExchangedFromItem(ItemStack newStack)
	{
		if (this.ExchangeFromItem != null)
		{
			this.ExchangeFromItem(newStack);
		}
	}

	// Token: 0x1400005D RID: 93
	// (add) Token: 0x06004AC7 RID: 19143 RVA: 0x001D1154 File Offset: 0x001CF354
	// (remove) Token: 0x06004AC8 RID: 19144 RVA: 0x001D118C File Offset: 0x001CF38C
	public event QuestEvent_ItemStackActionEvent ScrapItem;

	// Token: 0x06004AC9 RID: 19145 RVA: 0x001D11C1 File Offset: 0x001CF3C1
	public void ScrappedItem(ItemStack newStack)
	{
		if (this.ScrapItem != null)
		{
			this.ScrapItem(newStack);
		}
	}

	// Token: 0x1400005E RID: 94
	// (add) Token: 0x06004ACA RID: 19146 RVA: 0x001D11D8 File Offset: 0x001CF3D8
	// (remove) Token: 0x06004ACB RID: 19147 RVA: 0x001D1210 File Offset: 0x001CF410
	public event QuestEvent_ItemValueActionEvent RepairItem;

	// Token: 0x06004ACC RID: 19148 RVA: 0x001D1245 File Offset: 0x001CF445
	public void RepairedItem(ItemValue newValue)
	{
		if (this.RepairItem != null)
		{
			this.RepairItem(newValue);
		}
	}

	// Token: 0x1400005F RID: 95
	// (add) Token: 0x06004ACD RID: 19149 RVA: 0x001D125C File Offset: 0x001CF45C
	// (remove) Token: 0x06004ACE RID: 19150 RVA: 0x001D1294 File Offset: 0x001CF494
	public event QuestEvent_SkillPointSpent SkillPointSpent;

	// Token: 0x14000060 RID: 96
	// (add) Token: 0x06004ACF RID: 19151 RVA: 0x001D12CC File Offset: 0x001CF4CC
	// (remove) Token: 0x06004AD0 RID: 19152 RVA: 0x001D1304 File Offset: 0x001CF504
	public event QuestEvent_ItemValueActionEvent HoldItem;

	// Token: 0x06004AD1 RID: 19153 RVA: 0x001D1339 File Offset: 0x001CF539
	public void HeldItem(ItemValue newValue)
	{
		if (this.HoldItem != null)
		{
			this.HoldItem(newValue);
		}
	}

	// Token: 0x14000061 RID: 97
	// (add) Token: 0x06004AD2 RID: 19154 RVA: 0x001D1350 File Offset: 0x001CF550
	// (remove) Token: 0x06004AD3 RID: 19155 RVA: 0x001D1388 File Offset: 0x001CF588
	public event QuestEvent_ItemValueActionEvent WearItem;

	// Token: 0x06004AD4 RID: 19156 RVA: 0x001D13BD File Offset: 0x001CF5BD
	public void WoreItem(ItemValue newValue)
	{
		if (this.WearItem != null)
		{
			this.WearItem(newValue);
		}
	}

	// Token: 0x06004AD5 RID: 19157 RVA: 0x001D13D3 File Offset: 0x001CF5D3
	public void SpendSkillPoint(ProgressionValue skill)
	{
		if (this.SkillPointSpent != null)
		{
			this.SkillPointSpent(skill.ProgressionClass.Name);
		}
	}

	// Token: 0x14000062 RID: 98
	// (add) Token: 0x06004AD6 RID: 19158 RVA: 0x001D13F4 File Offset: 0x001CF5F4
	// (remove) Token: 0x06004AD7 RID: 19159 RVA: 0x001D142C File Offset: 0x001CF62C
	public event QuestEvent_WindowChanged WindowChanged;

	// Token: 0x06004AD8 RID: 19160 RVA: 0x001D1461 File Offset: 0x001CF661
	public void ChangedWindow(string windowName)
	{
		if (this.WindowChanged != null)
		{
			this.WindowChanged(windowName);
		}
	}

	// Token: 0x14000063 RID: 99
	// (add) Token: 0x06004AD9 RID: 19161 RVA: 0x001D1478 File Offset: 0x001CF678
	// (remove) Token: 0x06004ADA RID: 19162 RVA: 0x001D14B0 File Offset: 0x001CF6B0
	public event QuestEvent_OpenContainer ContainerOpened;

	// Token: 0x06004ADB RID: 19163 RVA: 0x001D14E5 File Offset: 0x001CF6E5
	public void OpenedContainer(Vector3i containerLocation, ITileEntityLootable tileEntity)
	{
		if (this.ContainerOpened != null)
		{
			this.ContainerOpened(containerLocation, tileEntity);
		}
	}

	// Token: 0x14000064 RID: 100
	// (add) Token: 0x06004ADC RID: 19164 RVA: 0x001D14FC File Offset: 0x001CF6FC
	// (remove) Token: 0x06004ADD RID: 19165 RVA: 0x001D1534 File Offset: 0x001CF734
	public event QuestEvent_OpenContainer ContainerClosed;

	// Token: 0x06004ADE RID: 19166 RVA: 0x001D1569 File Offset: 0x001CF769
	public void ClosedContainer(Vector3i containerLocation, ITileEntityLootable tileEntity)
	{
		if (this.ContainerClosed != null)
		{
			this.ContainerClosed(containerLocation, tileEntity);
		}
	}

	// Token: 0x14000065 RID: 101
	// (add) Token: 0x06004ADF RID: 19167 RVA: 0x001D1580 File Offset: 0x001CF780
	// (remove) Token: 0x06004AE0 RID: 19168 RVA: 0x001D15B8 File Offset: 0x001CF7B8
	public event QuestEvent_EntityKillEvent EntityKill;

	// Token: 0x06004AE1 RID: 19169 RVA: 0x001D15ED File Offset: 0x001CF7ED
	public void EntityKilled(EntityAlive killedBy, EntityAlive killedEntity)
	{
		if (this.EntityKill != null && killedBy != null && killedEntity != null)
		{
			this.EntityKill(killedBy, killedEntity);
		}
	}

	// Token: 0x14000066 RID: 102
	// (add) Token: 0x06004AE2 RID: 19170 RVA: 0x001D1618 File Offset: 0x001CF818
	// (remove) Token: 0x06004AE3 RID: 19171 RVA: 0x001D1650 File Offset: 0x001CF850
	public event QuestEvent_NPCInteracted NPCInteract;

	// Token: 0x06004AE4 RID: 19172 RVA: 0x001D1685 File Offset: 0x001CF885
	public void NPCInteracted(EntityNPC entityNPC)
	{
		if (this.NPCInteract != null)
		{
			this.NPCInteract(entityNPC);
		}
	}

	// Token: 0x14000067 RID: 103
	// (add) Token: 0x06004AE5 RID: 19173 RVA: 0x001D169C File Offset: 0x001CF89C
	// (remove) Token: 0x06004AE6 RID: 19174 RVA: 0x001D16D4 File Offset: 0x001CF8D4
	public event QuestEvent_NPCInteracted NPCMeet;

	// Token: 0x06004AE7 RID: 19175 RVA: 0x001D1709 File Offset: 0x001CF909
	public void NPCMet(EntityNPC entityNPC)
	{
		if (this.NPCMeet != null)
		{
			this.NPCMeet(entityNPC);
		}
	}

	// Token: 0x14000068 RID: 104
	// (add) Token: 0x06004AE8 RID: 19176 RVA: 0x001D1720 File Offset: 0x001CF920
	// (remove) Token: 0x06004AE9 RID: 19177 RVA: 0x001D1758 File Offset: 0x001CF958
	public event QuestEvent_SleepersCleared SleepersCleared;

	// Token: 0x06004AEA RID: 19178 RVA: 0x001D178D File Offset: 0x001CF98D
	public void ClearedSleepers(Vector3 prefabPos)
	{
		if (this.SleepersCleared != null)
		{
			this.SleepersCleared(prefabPos);
		}
	}

	// Token: 0x14000069 RID: 105
	// (add) Token: 0x06004AEB RID: 19179 RVA: 0x001D17A4 File Offset: 0x001CF9A4
	// (remove) Token: 0x06004AEC RID: 19180 RVA: 0x001D17DC File Offset: 0x001CF9DC
	public event QuestEvent_Explosion ExplosionDetected;

	// Token: 0x06004AED RID: 19181 RVA: 0x001D1811 File Offset: 0x001CFA11
	public void DetectedExplosion(Vector3 explosionPos, int entityID, float blockDamage)
	{
		if (this.ExplosionDetected != null)
		{
			this.ExplosionDetected(explosionPos, entityID, blockDamage);
		}
	}

	// Token: 0x1400006A RID: 106
	// (add) Token: 0x06004AEE RID: 19182 RVA: 0x001D182C File Offset: 0x001CFA2C
	// (remove) Token: 0x06004AEF RID: 19183 RVA: 0x001D1864 File Offset: 0x001CFA64
	public event QuestEvent_PurchaseEvent BuyItems;

	// Token: 0x1400006B RID: 107
	// (add) Token: 0x06004AF0 RID: 19184 RVA: 0x001D189C File Offset: 0x001CFA9C
	// (remove) Token: 0x06004AF1 RID: 19185 RVA: 0x001D18D4 File Offset: 0x001CFAD4
	public event QuestEvent_PurchaseEvent SellItems;

	// Token: 0x06004AF2 RID: 19186 RVA: 0x001D1909 File Offset: 0x001CFB09
	public void BoughtItems(string traderName, int itemCount)
	{
		if (this.BuyItems != null)
		{
			this.BuyItems(traderName, itemCount);
		}
	}

	// Token: 0x06004AF3 RID: 19187 RVA: 0x001D1920 File Offset: 0x001CFB20
	public void SoldItems(string traderName, int itemCount)
	{
		if (this.SellItems != null)
		{
			this.SellItems(traderName, itemCount);
		}
	}

	// Token: 0x1400006C RID: 108
	// (add) Token: 0x06004AF4 RID: 19188 RVA: 0x001D1938 File Offset: 0x001CFB38
	// (remove) Token: 0x06004AF5 RID: 19189 RVA: 0x001D1970 File Offset: 0x001CFB70
	public event QuestEvent_ChallengeCompleteEvent ChallengeComplete;

	// Token: 0x06004AF6 RID: 19190 RVA: 0x001D19A5 File Offset: 0x001CFBA5
	public void ChallengeCompleted(ChallengeClass challenge, bool isRedeemed)
	{
		if (this.ChallengeComplete != null)
		{
			this.ChallengeComplete(challenge, isRedeemed);
		}
	}

	// Token: 0x1400006D RID: 109
	// (add) Token: 0x06004AF7 RID: 19191 RVA: 0x001D19BC File Offset: 0x001CFBBC
	// (remove) Token: 0x06004AF8 RID: 19192 RVA: 0x001D19F4 File Offset: 0x001CFBF4
	public event QuestEvent_TwitchEvent TwitchEventReceive;

	// Token: 0x06004AF9 RID: 19193 RVA: 0x001D1A29 File Offset: 0x001CFC29
	public void TwitchEventReceived(TwitchObjectiveTypes actionType, string param)
	{
		if (this.TwitchEventReceive != null)
		{
			this.TwitchEventReceive(actionType, param);
		}
	}

	// Token: 0x1400006E RID: 110
	// (add) Token: 0x06004AFA RID: 19194 RVA: 0x001D1A40 File Offset: 0x001CFC40
	// (remove) Token: 0x06004AFB RID: 19195 RVA: 0x001D1A78 File Offset: 0x001CFC78
	public event QuestEvent_QuestCompleteEvent QuestComplete;

	// Token: 0x06004AFC RID: 19196 RVA: 0x001D1AAD File Offset: 0x001CFCAD
	public void QuestCompleted(FastTags<TagGroup.Global> questTags, QuestClass questClass)
	{
		if (this.QuestComplete != null)
		{
			this.QuestComplete(questTags, questClass);
		}
	}

	// Token: 0x1400006F RID: 111
	// (add) Token: 0x06004AFD RID: 19197 RVA: 0x001D1AC4 File Offset: 0x001CFCC4
	// (remove) Token: 0x06004AFE RID: 19198 RVA: 0x001D1AFC File Offset: 0x001CFCFC
	public event QuestEvent_ChallengeAwardCredit ChallengeAwardCredit;

	// Token: 0x06004AFF RID: 19199 RVA: 0x001D1B31 File Offset: 0x001CFD31
	public void ChallengeAwardCredited(string challengeStat, int creditAmount)
	{
		if (this.ChallengeAwardCredit != null)
		{
			this.ChallengeAwardCredit(challengeStat, creditAmount);
		}
	}

	// Token: 0x14000070 RID: 112
	// (add) Token: 0x06004B00 RID: 19200 RVA: 0x001D1B48 File Offset: 0x001CFD48
	// (remove) Token: 0x06004B01 RID: 19201 RVA: 0x001D1B80 File Offset: 0x001CFD80
	public event QuestEvent_ChallengeAwardCredit QuestAwardCredit;

	// Token: 0x06004B02 RID: 19202 RVA: 0x001D1BB5 File Offset: 0x001CFDB5
	public void QuestAwardCredited(string stat, int creditAmount)
	{
		if (this.QuestAwardCredit != null)
		{
			this.QuestAwardCredit(stat, creditAmount);
		}
	}

	// Token: 0x14000071 RID: 113
	// (add) Token: 0x06004B03 RID: 19203 RVA: 0x001D1BCC File Offset: 0x001CFDCC
	// (remove) Token: 0x06004B04 RID: 19204 RVA: 0x001D1C04 File Offset: 0x001CFE04
	public event QuestEvent_BiomeEvent BiomeEnter;

	// Token: 0x06004B05 RID: 19205 RVA: 0x001D1C39 File Offset: 0x001CFE39
	public void BiomeEntered(BiomeDefinition biomeDef)
	{
		if (this.BiomeEnter != null)
		{
			this.BiomeEnter(biomeDef);
		}
	}

	// Token: 0x14000072 RID: 114
	// (add) Token: 0x06004B06 RID: 19206 RVA: 0x001D1C50 File Offset: 0x001CFE50
	// (remove) Token: 0x06004B07 RID: 19207 RVA: 0x001D1C88 File Offset: 0x001CFE88
	public event QuestEvent_ItemValueActionEvent UseItem;

	// Token: 0x06004B08 RID: 19208 RVA: 0x001D1CBD File Offset: 0x001CFEBD
	public void UsedItem(ItemValue newValue)
	{
		if (this.UseItem != null)
		{
			this.UseItem(newValue);
		}
	}

	// Token: 0x14000073 RID: 115
	// (add) Token: 0x06004B09 RID: 19209 RVA: 0x001D1CD4 File Offset: 0x001CFED4
	// (remove) Token: 0x06004B0A RID: 19210 RVA: 0x001D1D0C File Offset: 0x001CFF0C
	public event QuestEvent_FloatEvent TimeSurvive;

	// Token: 0x06004B0B RID: 19211 RVA: 0x001D1D41 File Offset: 0x001CFF41
	public void TimeSurvived(float time)
	{
		if (this.TimeSurvive != null)
		{
			this.TimeSurvive(time);
		}
	}

	// Token: 0x14000074 RID: 116
	// (add) Token: 0x06004B0C RID: 19212 RVA: 0x001D1D58 File Offset: 0x001CFF58
	// (remove) Token: 0x06004B0D RID: 19213 RVA: 0x001D1D90 File Offset: 0x001CFF90
	public event QuestEvent_Event BloodMoonSurvive;

	// Token: 0x06004B0E RID: 19214 RVA: 0x001D1DC5 File Offset: 0x001CFFC5
	public void BloodMoonSurvived()
	{
		if (this.BloodMoonSurvive != null)
		{
			this.BloodMoonSurvive();
		}
	}

	// Token: 0x06004B0F RID: 19215 RVA: 0x001D1DDC File Offset: 0x001CFFDC
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

	// Token: 0x06004B10 RID: 19216 RVA: 0x001D1F44 File Offset: 0x001D0144
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

	// Token: 0x06004B11 RID: 19217 RVA: 0x001D1F9C File Offset: 0x001D019C
	public void HandleAllPlayersDisconnect()
	{
		foreach (int num in this.TreasureQuestDictionary.Keys)
		{
			this.TreasureQuestDictionary[num].Remove();
		}
		this.TreasureQuestDictionary.Clear();
	}

	// Token: 0x06004B12 RID: 19218 RVA: 0x001D200C File Offset: 0x001D020C
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

	// Token: 0x06004B13 RID: 19219 RVA: 0x001D2037 File Offset: 0x001D0237
	public void ClearTraderResetQuestsForPlayer(int playerID)
	{
		if (this.ForceResetQuestTrader.ContainsKey(playerID))
		{
			this.ForceResetQuestTrader.Remove(playerID);
		}
	}

	// Token: 0x06004B14 RID: 19220 RVA: 0x001D2054 File Offset: 0x001D0254
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

	// Token: 0x06004B15 RID: 19221 RVA: 0x001D20A3 File Offset: 0x001D02A3
	public void AddObjectiveToBeUpdated(BaseObjective obj)
	{
		if (!this.objectivesToUpdate.Contains(obj))
		{
			this.objectivesToUpdate.Add(obj);
		}
	}

	// Token: 0x06004B16 RID: 19222 RVA: 0x001D20BF File Offset: 0x001D02BF
	public void RemoveObjectiveToBeUpdated(BaseObjective obj)
	{
		if (this.objectivesToUpdate.Contains(obj))
		{
			this.objectivesToUpdate.Remove(obj);
		}
	}

	// Token: 0x06004B17 RID: 19223 RVA: 0x001D20DC File Offset: 0x001D02DC
	public void AddObjectiveToBeUpdated(BaseChallengeObjective obj)
	{
		if (!this.challengeObjectivesToUpdate.Contains(obj))
		{
			this.challengeObjectivesToUpdate.Add(obj);
		}
	}

	// Token: 0x06004B18 RID: 19224 RVA: 0x001D20F8 File Offset: 0x001D02F8
	public void RemoveObjectiveToBeUpdated(BaseChallengeObjective obj)
	{
		if (this.challengeObjectivesToUpdate.Contains(obj))
		{
			this.challengeObjectivesToUpdate.Remove(obj);
		}
	}

	// Token: 0x06004B19 RID: 19225 RVA: 0x001D2115 File Offset: 0x001D0315
	public void AddTrackerToBeUpdated(TrackingHandler track)
	{
		if (!this.questTrackersToUpdate.Contains(track))
		{
			this.questTrackersToUpdate.Add(track);
		}
	}

	// Token: 0x06004B1A RID: 19226 RVA: 0x001D2131 File Offset: 0x001D0331
	public void RemoveTrackerToBeUpdated(TrackingHandler track)
	{
		if (this.questTrackersToUpdate.Contains(track))
		{
			this.questTrackersToUpdate.Remove(track);
		}
	}

	// Token: 0x06004B1B RID: 19227 RVA: 0x001D214E File Offset: 0x001D034E
	public void AddTrackerToBeUpdated(ChallengeTrackingHandler track)
	{
		this.challengeTrackerToUpdate = track;
	}

	// Token: 0x06004B1C RID: 19228 RVA: 0x001D2157 File Offset: 0x001D0357
	public void RemoveTrackerToBeUpdated(ChallengeTrackingHandler track)
	{
		this.challengeTrackerToUpdate = null;
	}

	// Token: 0x14000075 RID: 117
	// (add) Token: 0x06004B1D RID: 19229 RVA: 0x001D2160 File Offset: 0x001D0360
	// (remove) Token: 0x06004B1E RID: 19230 RVA: 0x001D2198 File Offset: 0x001D0398
	public event QuestEvent_SleeperVolumePositionChanged SleeperVolumePositionAdd;

	// Token: 0x14000076 RID: 118
	// (add) Token: 0x06004B1F RID: 19231 RVA: 0x001D21D0 File Offset: 0x001D03D0
	// (remove) Token: 0x06004B20 RID: 19232 RVA: 0x001D2208 File Offset: 0x001D0408
	public event QuestEvent_SleeperVolumePositionChanged SleeperVolumePositionRemove;

	// Token: 0x06004B21 RID: 19233 RVA: 0x001D223D File Offset: 0x001D043D
	public void SleeperVolumePositionAdded(Vector3 pos)
	{
		if (this.SleeperVolumePositionAdd != null)
		{
			this.SleeperVolumePositionAdd(pos);
		}
	}

	// Token: 0x06004B22 RID: 19234 RVA: 0x001D2253 File Offset: 0x001D0453
	public void SleeperVolumePositionRemoved(Vector3 pos)
	{
		if (this.SleeperVolumePositionRemove != null)
		{
			this.SleeperVolumePositionRemove(pos);
		}
	}

	// Token: 0x06004B23 RID: 19235 RVA: 0x001D2269 File Offset: 0x001D0469
	public void AddSleeperVolumeLocation(Vector3 newLocation)
	{
		this.SleeperVolumeLocationList.Add(newLocation);
	}

	// Token: 0x06004B24 RID: 19236 RVA: 0x001D2278 File Offset: 0x001D0478
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

	// Token: 0x06004B25 RID: 19237 RVA: 0x001D2308 File Offset: 0x001D0508
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

	// Token: 0x06004B26 RID: 19238 RVA: 0x001D23E0 File Offset: 0x001D05E0
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

	// Token: 0x06004B27 RID: 19239 RVA: 0x001D2410 File Offset: 0x001D0610
	public void QuestUnlockPOI(int entityID, Vector3 prefabPos)
	{
		PrefabInstance prefabFromWorldPos = GameManager.Instance.GetDynamicPrefabDecorator().GetPrefabFromWorldPos((int)prefabPos.x, (int)prefabPos.z);
		if (prefabFromWorldPos.lockInstance != null)
		{
			prefabFromWorldPos.lockInstance.RemoveQuester(entityID);
		}
	}

	// Token: 0x06004B28 RID: 19240 RVA: 0x001D2450 File Offset: 0x001D0650
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

	// Token: 0x06004B29 RID: 19241 RVA: 0x001D25B8 File Offset: 0x001D07B8
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

	// Token: 0x06004B2A RID: 19242 RVA: 0x001D2750 File Offset: 0x001D0950
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

	// Token: 0x06004B2B RID: 19243 RVA: 0x001D2A78 File Offset: 0x001D0C78
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

	// Token: 0x06004B2C RID: 19244 RVA: 0x001D2A88 File Offset: 0x001D0C88
	public void SetupFetchForMP(int entityID, Vector3 prefabPos, ObjectiveFetchFromContainer.FetchModeTypes fetchMode, int[] sharedWithList)
	{
		PrefabInstance prefabFromWorldPos = GameManager.Instance.GetDynamicPrefabDecorator().GetPrefabFromWorldPos((int)prefabPos.x, (int)prefabPos.z);
		this.HandleContainerPositions(GameManager.Instance.World, entityID, new Vector3i(prefabPos), prefabFromWorldPos.prefab.size, fetchMode, sharedWithList);
	}

	// Token: 0x06004B2D RID: 19245 RVA: 0x001D2AD8 File Offset: 0x001D0CD8
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

	// Token: 0x06004B2E RID: 19246 RVA: 0x001D2E60 File Offset: 0x001D1060
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

	// Token: 0x06004B2F RID: 19247 RVA: 0x001D2F40 File Offset: 0x001D1140
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

	// Token: 0x06004B30 RID: 19248 RVA: 0x001D3008 File Offset: 0x001D1208
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

	// Token: 0x06004B31 RID: 19249 RVA: 0x001D30AA File Offset: 0x001D12AA
	public void ClearQuestList(int npcEntityID)
	{
		if (this.npcQuestData.ContainsKey(npcEntityID))
		{
			this.npcQuestData[npcEntityID].PlayerQuestList.Clear();
		}
	}

	// Token: 0x06004B32 RID: 19250 RVA: 0x001D30D0 File Offset: 0x001D12D0
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

	// Token: 0x06004B33 RID: 19251 RVA: 0x001D3113 File Offset: 0x001D1313
	public void AddQuestTierReward(QuestTierReward reward)
	{
		if (this.questTierRewards == null)
		{
			this.questTierRewards = new List<QuestTierReward>();
		}
		this.questTierRewards.Add(reward);
	}

	// Token: 0x06004B34 RID: 19252 RVA: 0x001D3134 File Offset: 0x001D1334
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

	// Token: 0x06004B35 RID: 19253 RVA: 0x001D31A4 File Offset: 0x001D13A4
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

	// Token: 0x06004B36 RID: 19254 RVA: 0x001D32AC File Offset: 0x001D14AC
	public void AddTreasureQuest(int _questCode, int _entityID, int _blocksPerReduction, Vector3i _position, Vector3 _treasureOffset)
	{
		if (!this.TreasureQuestDictionary.ContainsKey(_questCode))
		{
			TreasureQuestData treasureQuestData = new TreasureQuestData(_questCode, _entityID, _blocksPerReduction, _position, _treasureOffset);
			this.TreasureQuestDictionary.Add(_questCode, treasureQuestData);
		}
	}

	// Token: 0x06004B37 RID: 19255 RVA: 0x001D32E1 File Offset: 0x001D14E1
	public void SetTreasureContainerPosition(int _questCode, Vector3i _updatedPosition)
	{
		if (this.TreasureQuestDictionary.ContainsKey(_questCode))
		{
			this.TreasureQuestDictionary[_questCode].UpdatePosition(_updatedPosition);
		}
	}

	// Token: 0x06004B38 RID: 19256 RVA: 0x001D3304 File Offset: 0x001D1504
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

	// Token: 0x06004B39 RID: 19257 RVA: 0x001D3448 File Offset: 0x001D1648
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

	// Token: 0x06004B3A RID: 19258 RVA: 0x001D349C File Offset: 0x001D169C
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

	// Token: 0x06004B3B RID: 19259 RVA: 0x001D3550 File Offset: 0x001D1750
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

	// Token: 0x06004B3C RID: 19260 RVA: 0x001D3596 File Offset: 0x001D1796
	public void FinishManagedQuest(int _questCode, EntityPlayer _player)
	{
		if (this.BlockActivateQuestDictionary.ContainsKey(_questCode))
		{
			this.BlockActivateQuestDictionary[_questCode].RemoveSharedQuester(_player);
		}
	}

	// Token: 0x04003B19 RID: 15129
	[PublicizedFrom(EAccessModifier.Private)]
	public static QuestEventManager instance = null;

	// Token: 0x04003B1A RID: 15130
	[PublicizedFrom(EAccessModifier.Private)]
	public List<BaseObjective> objectivesToUpdate = new List<BaseObjective>();

	// Token: 0x04003B1B RID: 15131
	[PublicizedFrom(EAccessModifier.Private)]
	public List<BaseChallengeObjective> challengeObjectivesToUpdate = new List<BaseChallengeObjective>();

	// Token: 0x04003B1C RID: 15132
	[PublicizedFrom(EAccessModifier.Private)]
	public List<TrackingHandler> questTrackersToUpdate = new List<TrackingHandler>();

	// Token: 0x04003B1D RID: 15133
	[PublicizedFrom(EAccessModifier.Private)]
	public ChallengeTrackingHandler challengeTrackerToUpdate;

	// Token: 0x04003B1E RID: 15134
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Vector3> removeSleeperDataList = new List<Vector3>();

	// Token: 0x04003B1F RID: 15135
	public Dictionary<int, NPCQuestData> npcQuestData = new Dictionary<int, NPCQuestData>();

	// Token: 0x04003B20 RID: 15136
	public List<QuestTierReward> questTierRewards = new List<QuestTierReward>();

	// Token: 0x04003B21 RID: 15137
	public Dictionary<Vector3, SleeperEventData> SleeperVolumeUpdateDictionary = new Dictionary<Vector3, SleeperEventData>();

	// Token: 0x04003B22 RID: 15138
	public List<Vector3> SleeperVolumeLocationList = new List<Vector3>();

	// Token: 0x04003B23 RID: 15139
	public Dictionary<int, TreasureQuestData> TreasureQuestDictionary = new Dictionary<int, TreasureQuestData>();

	// Token: 0x04003B24 RID: 15140
	public Dictionary<int, RestorePowerQuestData> BlockActivateQuestDictionary = new Dictionary<int, RestorePowerQuestData>();

	// Token: 0x04003B25 RID: 15141
	public Dictionary<int, List<PrefabInstance>> tierPrefabList = new Dictionary<int, List<PrefabInstance>>();

	// Token: 0x04003B26 RID: 15142
	public Dictionary<TraderArea, List<QuestEventManager.PrefabListData>> TraderPrefabList = new Dictionary<TraderArea, List<QuestEventManager.PrefabListData>>();

	// Token: 0x04003B27 RID: 15143
	public Rect QuestBounds;

	// Token: 0x04003B28 RID: 15144
	public List<Vector3i> ActiveQuestBlocks = new List<Vector3i>();

	// Token: 0x04003B29 RID: 15145
	public Dictionary<int, int> ForceResetQuestTrader = new Dictionary<int, int>();

	// Token: 0x04003B2A RID: 15146
	public static FastTags<TagGroup.Global> manualResetTag = FastTags<TagGroup.Global>.Parse("manual");

	// Token: 0x04003B2B RID: 15147
	public static FastTags<TagGroup.Global> traderTag = FastTags<TagGroup.Global>.Parse("trader");

	// Token: 0x04003B2C RID: 15148
	public static FastTags<TagGroup.Global> clearTag = FastTags<TagGroup.Global>.Parse("clear");

	// Token: 0x04003B2D RID: 15149
	public static FastTags<TagGroup.Global> treasureTag = FastTags<TagGroup.Global>.Parse("treasure");

	// Token: 0x04003B2E RID: 15150
	public static FastTags<TagGroup.Global> fetchTag = FastTags<TagGroup.Global>.Parse("fetch");

	// Token: 0x04003B2F RID: 15151
	public static FastTags<TagGroup.Global> craftingTag = FastTags<TagGroup.Global>.Parse("crafting");

	// Token: 0x04003B30 RID: 15152
	public static FastTags<TagGroup.Global> restorePowerTag = FastTags<TagGroup.Global>.Parse("restore_power");

	// Token: 0x04003B31 RID: 15153
	public static FastTags<TagGroup.Global> infestedTag = FastTags<TagGroup.Global>.Parse("infested");

	// Token: 0x04003B32 RID: 15154
	public static FastTags<TagGroup.Global> banditTag = FastTags<TagGroup.Global>.Parse("bandit");

	// Token: 0x04003B33 RID: 15155
	public static FastTags<TagGroup.Global> allQuestTags = FastTags<TagGroup.Global>.CombineTags(FastTags<TagGroup.Global>.CombineTags(QuestEventManager.traderTag, QuestEventManager.clearTag, QuestEventManager.treasureTag, QuestEventManager.fetchTag), FastTags<TagGroup.Global>.CombineTags(QuestEventManager.craftingTag, QuestEventManager.restorePowerTag), FastTags<TagGroup.Global>.CombineTags(QuestEventManager.infestedTag, QuestEventManager.banditTag));

	// Token: 0x04003B34 RID: 15156
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cTreasurePointAttempts = 5;

	// Token: 0x04003B35 RID: 15157
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cTreasurePointDistanceAdd = 2.5f;

	// Token: 0x04003B36 RID: 15158
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cTreasurePointMaxDistanceAdd = 25f;

	// Token: 0x020009DC RID: 2524
	public enum POILockoutReasonTypes
	{
		// Token: 0x04003B5D RID: 15197
		None,
		// Token: 0x04003B5E RID: 15198
		PlayerInside,
		// Token: 0x04003B5F RID: 15199
		Bedroll,
		// Token: 0x04003B60 RID: 15200
		LandClaim,
		// Token: 0x04003B61 RID: 15201
		QuestLock
	}

	// Token: 0x020009DD RID: 2525
	public class PrefabListData
	{
		// Token: 0x06004B3E RID: 19262 RVA: 0x001D3694 File Offset: 0x001D1894
		public void AddPOI(PrefabInstance poi)
		{
			int difficultyTier = (int)poi.prefab.DifficultyTier;
			if (!this.TierData.ContainsKey(difficultyTier))
			{
				this.TierData.Add(difficultyTier, new List<PrefabInstance>());
			}
			this.TierData[difficultyTier].Add(poi);
		}

		// Token: 0x06004B3F RID: 19263 RVA: 0x001D36E0 File Offset: 0x001D18E0
		public void ShuffleDifficulty(int difficulty, GameRandom gameRandom)
		{
			List<PrefabInstance> list;
			if (this.TierData.TryGetValue(difficulty, out list))
			{
				list.Shuffle<PrefabInstance>(gameRandom);
			}
		}

		// Token: 0x04003B62 RID: 15202
		public Dictionary<int, List<PrefabInstance>> TierData = new Dictionary<int, List<PrefabInstance>>();
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Challenges;
using Quests;
using UnityEngine;

// Token: 0x020009EF RID: 2543
public class QuestEventManager
{
	// Token: 0x17000825 RID: 2085
	// (get) Token: 0x06004A91 RID: 19089 RVA: 0x001D2B03 File Offset: 0x001D0D03
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

	// Token: 0x17000826 RID: 2086
	// (get) Token: 0x06004A92 RID: 19090 RVA: 0x001D2B1B File Offset: 0x001D0D1B
	public static bool HasInstance
	{
		get
		{
			return QuestEventManager.instance != null;
		}
	}

	// Token: 0x06004A93 RID: 19091 RVA: 0x001D2B28 File Offset: 0x001D0D28
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

	// Token: 0x06004A94 RID: 19092 RVA: 0x001D2C18 File Offset: 0x001D0E18
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

	// Token: 0x06004A95 RID: 19093 RVA: 0x001D2C74 File Offset: 0x001D0E74
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

	// Token: 0x06004A96 RID: 19094 RVA: 0x001D2CC8 File Offset: 0x001D0EC8
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

	// Token: 0x06004A97 RID: 19095 RVA: 0x001D2D90 File Offset: 0x001D0F90
	[PublicizedFrom(EAccessModifier.Private)]
	public QuestEventManager()
	{
	}

	// Token: 0x14000053 RID: 83
	// (add) Token: 0x06004A98 RID: 19096 RVA: 0x001D2E40 File Offset: 0x001D1040
	// (remove) Token: 0x06004A99 RID: 19097 RVA: 0x001D2E78 File Offset: 0x001D1078
	public event QuestEvent_BlockEvent BlockActivate;

	// Token: 0x06004A9A RID: 19098 RVA: 0x001D2EAD File Offset: 0x001D10AD
	public void BlockActivated(string blockName, Vector3i blockPos)
	{
		if (this.BlockActivate != null)
		{
			this.BlockActivate(blockName, blockPos);
		}
	}

	// Token: 0x14000054 RID: 84
	// (add) Token: 0x06004A9B RID: 19099 RVA: 0x001D2EC4 File Offset: 0x001D10C4
	// (remove) Token: 0x06004A9C RID: 19100 RVA: 0x001D2EFC File Offset: 0x001D10FC
	public event QuestEvent_BlockChangedEvent BlockChange;

	// Token: 0x06004A9D RID: 19101 RVA: 0x001D2F31 File Offset: 0x001D1131
	public void BlockChanged(Block blockOld, Block blockNew, Vector3i blockPos)
	{
		if (this.BlockChange != null)
		{
			this.BlockChange(blockOld, blockNew, blockPos);
		}
	}

	// Token: 0x14000055 RID: 85
	// (add) Token: 0x06004A9E RID: 19102 RVA: 0x001D2F4C File Offset: 0x001D114C
	// (remove) Token: 0x06004A9F RID: 19103 RVA: 0x001D2F84 File Offset: 0x001D1184
	public event QuestEvent_BlockDestroyEvent BlockDestroy;

	// Token: 0x06004AA0 RID: 19104 RVA: 0x001D2FBC File Offset: 0x001D11BC
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

	// Token: 0x14000056 RID: 86
	// (add) Token: 0x06004AA1 RID: 19105 RVA: 0x001D3048 File Offset: 0x001D1248
	// (remove) Token: 0x06004AA2 RID: 19106 RVA: 0x001D3080 File Offset: 0x001D1280
	public event QuestEvent_BlockEvent BlockPickup;

	// Token: 0x06004AA3 RID: 19107 RVA: 0x001D30B5 File Offset: 0x001D12B5
	public void BlockPickedUp(string blockName, Vector3i blockPos)
	{
		if (this.BlockPickup != null)
		{
			this.BlockPickup(blockName, blockPos);
		}
	}

	// Token: 0x14000057 RID: 87
	// (add) Token: 0x06004AA4 RID: 19108 RVA: 0x001D30CC File Offset: 0x001D12CC
	// (remove) Token: 0x06004AA5 RID: 19109 RVA: 0x001D3104 File Offset: 0x001D1304
	public event QuestEvent_BlockEvent BlockPlace;

	// Token: 0x06004AA6 RID: 19110 RVA: 0x001D3139 File Offset: 0x001D1339
	public void BlockPlaced(string blockName, Vector3i blockPos)
	{
		if (this.BlockPlace != null)
		{
			this.BlockPlace(blockName, blockPos);
		}
	}

	// Token: 0x14000058 RID: 88
	// (add) Token: 0x06004AA7 RID: 19111 RVA: 0x001D3150 File Offset: 0x001D1350
	// (remove) Token: 0x06004AA8 RID: 19112 RVA: 0x001D3188 File Offset: 0x001D1388
	public event QuestEvent_BlockEvent BlockUpgrade;

	// Token: 0x06004AA9 RID: 19113 RVA: 0x001D31BD File Offset: 0x001D13BD
	public void BlockUpgraded(string blockName, Vector3i blockPos)
	{
		if (this.BlockUpgrade != null)
		{
			this.BlockUpgrade(blockName, blockPos);
		}
	}

	// Token: 0x14000059 RID: 89
	// (add) Token: 0x06004AAA RID: 19114 RVA: 0x001D31D4 File Offset: 0x001D13D4
	// (remove) Token: 0x06004AAB RID: 19115 RVA: 0x001D320C File Offset: 0x001D140C
	public event QuestEvent_ItemStackActionEvent AddItem;

	// Token: 0x06004AAC RID: 19116 RVA: 0x001D3241 File Offset: 0x001D1441
	public void ItemAdded(ItemStack newStack)
	{
		if (this.AddItem != null)
		{
			this.AddItem(newStack);
		}
	}

	// Token: 0x1400005A RID: 90
	// (add) Token: 0x06004AAD RID: 19117 RVA: 0x001D3258 File Offset: 0x001D1458
	// (remove) Token: 0x06004AAE RID: 19118 RVA: 0x001D3290 File Offset: 0x001D1490
	public event QuestEvent_HarvestStackActionEvent HarvestItem;

	// Token: 0x06004AAF RID: 19119 RVA: 0x001D32C5 File Offset: 0x001D14C5
	public void HarvestedItem(ItemValue heldItem, ItemStack newStack, BlockValue bv)
	{
		if (this.HarvestItem != null)
		{
			this.HarvestItem(heldItem, newStack, bv);
		}
	}

	// Token: 0x1400005B RID: 91
	// (add) Token: 0x06004AB0 RID: 19120 RVA: 0x001D32E0 File Offset: 0x001D14E0
	// (remove) Token: 0x06004AB1 RID: 19121 RVA: 0x001D3318 File Offset: 0x001D1518
	public event QuestEvent_ItemStackActionEvent AssembleItem;

	// Token: 0x06004AB2 RID: 19122 RVA: 0x001D334D File Offset: 0x001D154D
	public void AssembledItem(ItemStack newStack)
	{
		if (this.AssembleItem != null)
		{
			this.AssembleItem(newStack);
		}
	}

	// Token: 0x1400005C RID: 92
	// (add) Token: 0x06004AB3 RID: 19123 RVA: 0x001D3364 File Offset: 0x001D1564
	// (remove) Token: 0x06004AB4 RID: 19124 RVA: 0x001D339C File Offset: 0x001D159C
	public event QuestEvent_ItemStackActionEvent CraftItem;

	// Token: 0x06004AB5 RID: 19125 RVA: 0x001D33D1 File Offset: 0x001D15D1
	public void CraftedItem(ItemStack newStack)
	{
		if (this.CraftItem != null)
		{
			this.CraftItem(newStack);
		}
	}

	// Token: 0x1400005D RID: 93
	// (add) Token: 0x06004AB6 RID: 19126 RVA: 0x001D33E8 File Offset: 0x001D15E8
	// (remove) Token: 0x06004AB7 RID: 19127 RVA: 0x001D3420 File Offset: 0x001D1620
	public event QuestEvent_ItemStackActionEvent ExchangeFromItem;

	// Token: 0x06004AB8 RID: 19128 RVA: 0x001D3455 File Offset: 0x001D1655
	public void ExchangedFromItem(ItemStack newStack)
	{
		if (this.ExchangeFromItem != null)
		{
			this.ExchangeFromItem(newStack);
		}
	}

	// Token: 0x1400005E RID: 94
	// (add) Token: 0x06004AB9 RID: 19129 RVA: 0x001D346C File Offset: 0x001D166C
	// (remove) Token: 0x06004ABA RID: 19130 RVA: 0x001D34A4 File Offset: 0x001D16A4
	public event QuestEvent_ItemStackActionEvent ScrapItem;

	// Token: 0x06004ABB RID: 19131 RVA: 0x001D34D9 File Offset: 0x001D16D9
	public void ScrappedItem(ItemStack newStack)
	{
		if (this.ScrapItem != null)
		{
			this.ScrapItem(newStack);
		}
	}

	// Token: 0x1400005F RID: 95
	// (add) Token: 0x06004ABC RID: 19132 RVA: 0x001D34F0 File Offset: 0x001D16F0
	// (remove) Token: 0x06004ABD RID: 19133 RVA: 0x001D3528 File Offset: 0x001D1728
	public event QuestEvent_ItemValueActionEvent RepairItem;

	// Token: 0x06004ABE RID: 19134 RVA: 0x001D355D File Offset: 0x001D175D
	public void RepairedItem(ItemValue newValue)
	{
		if (this.RepairItem != null)
		{
			this.RepairItem(newValue);
		}
	}

	// Token: 0x14000060 RID: 96
	// (add) Token: 0x06004ABF RID: 19135 RVA: 0x001D3574 File Offset: 0x001D1774
	// (remove) Token: 0x06004AC0 RID: 19136 RVA: 0x001D35AC File Offset: 0x001D17AC
	public event QuestEvent_SkillPointSpent SkillPointSpent;

	// Token: 0x14000061 RID: 97
	// (add) Token: 0x06004AC1 RID: 19137 RVA: 0x001D35E4 File Offset: 0x001D17E4
	// (remove) Token: 0x06004AC2 RID: 19138 RVA: 0x001D361C File Offset: 0x001D181C
	public event QuestEvent_ItemValueActionEvent HoldItem;

	// Token: 0x06004AC3 RID: 19139 RVA: 0x001D3651 File Offset: 0x001D1851
	public void HeldItem(ItemValue newValue)
	{
		if (this.HoldItem != null)
		{
			this.HoldItem(newValue);
		}
	}

	// Token: 0x14000062 RID: 98
	// (add) Token: 0x06004AC4 RID: 19140 RVA: 0x001D3668 File Offset: 0x001D1868
	// (remove) Token: 0x06004AC5 RID: 19141 RVA: 0x001D36A0 File Offset: 0x001D18A0
	public event QuestEvent_ItemValueActionEvent WearItem;

	// Token: 0x06004AC6 RID: 19142 RVA: 0x001D36D5 File Offset: 0x001D18D5
	public void WoreItem(ItemValue newValue)
	{
		if (this.WearItem != null)
		{
			this.WearItem(newValue);
		}
	}

	// Token: 0x06004AC7 RID: 19143 RVA: 0x001D36EB File Offset: 0x001D18EB
	public void SpendSkillPoint(ProgressionValue skill)
	{
		if (this.SkillPointSpent != null)
		{
			this.SkillPointSpent(skill.ProgressionClass.Name);
		}
	}

	// Token: 0x14000063 RID: 99
	// (add) Token: 0x06004AC8 RID: 19144 RVA: 0x001D370C File Offset: 0x001D190C
	// (remove) Token: 0x06004AC9 RID: 19145 RVA: 0x001D3744 File Offset: 0x001D1944
	public event QuestEvent_WindowChanged WindowChanged;

	// Token: 0x06004ACA RID: 19146 RVA: 0x001D3779 File Offset: 0x001D1979
	public void ChangedWindow(string windowName)
	{
		if (this.WindowChanged != null)
		{
			this.WindowChanged(windowName);
		}
	}

	// Token: 0x14000064 RID: 100
	// (add) Token: 0x06004ACB RID: 19147 RVA: 0x001D3790 File Offset: 0x001D1990
	// (remove) Token: 0x06004ACC RID: 19148 RVA: 0x001D37C8 File Offset: 0x001D19C8
	public event QuestEvent_ContainerOpened ContainerOpened;

	// Token: 0x06004ACD RID: 19149 RVA: 0x001D37FD File Offset: 0x001D19FD
	public void OpenedContainer(Vector3i containerLocation, TEFeatureStorage tileEntity, bool firstTimeTouched)
	{
		if (this.ContainerOpened != null)
		{
			this.ContainerOpened(containerLocation, tileEntity, firstTimeTouched);
		}
	}

	// Token: 0x14000065 RID: 101
	// (add) Token: 0x06004ACE RID: 19150 RVA: 0x001D3818 File Offset: 0x001D1A18
	// (remove) Token: 0x06004ACF RID: 19151 RVA: 0x001D3850 File Offset: 0x001D1A50
	public event QuestEvent_OpenContainer ContainerClosed;

	// Token: 0x06004AD0 RID: 19152 RVA: 0x001D3885 File Offset: 0x001D1A85
	public void ClosedContainer(Vector3i containerLocation, TEFeatureStorage tileEntity)
	{
		if (this.ContainerClosed != null)
		{
			this.ContainerClosed(containerLocation, tileEntity);
		}
	}

	// Token: 0x14000066 RID: 102
	// (add) Token: 0x06004AD1 RID: 19153 RVA: 0x001D389C File Offset: 0x001D1A9C
	// (remove) Token: 0x06004AD2 RID: 19154 RVA: 0x001D38D4 File Offset: 0x001D1AD4
	public event QuestEvent_EntityKillEvent EntityKill;

	// Token: 0x06004AD3 RID: 19155 RVA: 0x001D3909 File Offset: 0x001D1B09
	public void EntityKilled(EntityAlive killedBy, EntityAlive killedEntity)
	{
		if (this.EntityKill != null && killedBy != null && killedEntity != null)
		{
			this.EntityKill(killedBy, killedEntity);
		}
	}

	// Token: 0x14000067 RID: 103
	// (add) Token: 0x06004AD4 RID: 19156 RVA: 0x001D3934 File Offset: 0x001D1B34
	// (remove) Token: 0x06004AD5 RID: 19157 RVA: 0x001D396C File Offset: 0x001D1B6C
	public event QuestEvent_NPCInteracted NPCInteract;

	// Token: 0x06004AD6 RID: 19158 RVA: 0x001D39A1 File Offset: 0x001D1BA1
	public void NPCInteracted(EntityNPC entityNPC)
	{
		if (this.NPCInteract != null)
		{
			this.NPCInteract(entityNPC);
		}
	}

	// Token: 0x14000068 RID: 104
	// (add) Token: 0x06004AD7 RID: 19159 RVA: 0x001D39B8 File Offset: 0x001D1BB8
	// (remove) Token: 0x06004AD8 RID: 19160 RVA: 0x001D39F0 File Offset: 0x001D1BF0
	public event QuestEvent_NPCInteracted NPCMeet;

	// Token: 0x06004AD9 RID: 19161 RVA: 0x001D3A25 File Offset: 0x001D1C25
	public void NPCMet(EntityNPC entityNPC)
	{
		if (this.NPCMeet != null)
		{
			this.NPCMeet(entityNPC);
		}
	}

	// Token: 0x14000069 RID: 105
	// (add) Token: 0x06004ADA RID: 19162 RVA: 0x001D3A3C File Offset: 0x001D1C3C
	// (remove) Token: 0x06004ADB RID: 19163 RVA: 0x001D3A74 File Offset: 0x001D1C74
	public event QuestEvent_SleepersCleared SleepersCleared;

	// Token: 0x06004ADC RID: 19164 RVA: 0x001D3AA9 File Offset: 0x001D1CA9
	public void ClearedSleepers(Vector3 prefabPos)
	{
		if (this.SleepersCleared != null)
		{
			this.SleepersCleared(prefabPos);
		}
	}

	// Token: 0x1400006A RID: 106
	// (add) Token: 0x06004ADD RID: 19165 RVA: 0x001D3AC0 File Offset: 0x001D1CC0
	// (remove) Token: 0x06004ADE RID: 19166 RVA: 0x001D3AF8 File Offset: 0x001D1CF8
	public event QuestEvent_Explosion ExplosionDetected;

	// Token: 0x06004ADF RID: 19167 RVA: 0x001D3B2D File Offset: 0x001D1D2D
	public void DetectedExplosion(Vector3 explosionPos, int entityID, float blockDamage)
	{
		if (this.ExplosionDetected != null)
		{
			this.ExplosionDetected(explosionPos, entityID, blockDamage);
		}
	}

	// Token: 0x1400006B RID: 107
	// (add) Token: 0x06004AE0 RID: 19168 RVA: 0x001D3B48 File Offset: 0x001D1D48
	// (remove) Token: 0x06004AE1 RID: 19169 RVA: 0x001D3B80 File Offset: 0x001D1D80
	public event QuestEvent_PurchaseEvent BuyItems;

	// Token: 0x1400006C RID: 108
	// (add) Token: 0x06004AE2 RID: 19170 RVA: 0x001D3BB8 File Offset: 0x001D1DB8
	// (remove) Token: 0x06004AE3 RID: 19171 RVA: 0x001D3BF0 File Offset: 0x001D1DF0
	public event QuestEvent_PurchaseEvent SellItems;

	// Token: 0x06004AE4 RID: 19172 RVA: 0x001D3C25 File Offset: 0x001D1E25
	public void BoughtItems(string traderName, int itemCount)
	{
		if (this.BuyItems != null)
		{
			this.BuyItems(traderName, itemCount);
		}
	}

	// Token: 0x06004AE5 RID: 19173 RVA: 0x001D3C3C File Offset: 0x001D1E3C
	public void SoldItems(string traderName, int itemCount)
	{
		if (this.SellItems != null)
		{
			this.SellItems(traderName, itemCount);
		}
	}

	// Token: 0x1400006D RID: 109
	// (add) Token: 0x06004AE6 RID: 19174 RVA: 0x001D3C54 File Offset: 0x001D1E54
	// (remove) Token: 0x06004AE7 RID: 19175 RVA: 0x001D3C8C File Offset: 0x001D1E8C
	public event QuestEvent_ChallengeCompleteEvent ChallengeComplete;

	// Token: 0x06004AE8 RID: 19176 RVA: 0x001D3CC1 File Offset: 0x001D1EC1
	public void ChallengeCompleted(ChallengeClass challenge, bool isRedeemed)
	{
		if (this.ChallengeComplete != null)
		{
			this.ChallengeComplete(challenge, isRedeemed);
		}
	}

	// Token: 0x1400006E RID: 110
	// (add) Token: 0x06004AE9 RID: 19177 RVA: 0x001D3CD8 File Offset: 0x001D1ED8
	// (remove) Token: 0x06004AEA RID: 19178 RVA: 0x001D3D10 File Offset: 0x001D1F10
	public event QuestEvent_TwitchEvent TwitchEventReceive;

	// Token: 0x06004AEB RID: 19179 RVA: 0x001D3D45 File Offset: 0x001D1F45
	public void TwitchEventReceived(TwitchObjectiveTypes actionType, string param)
	{
		if (this.TwitchEventReceive != null)
		{
			this.TwitchEventReceive(actionType, param);
		}
	}

	// Token: 0x1400006F RID: 111
	// (add) Token: 0x06004AEC RID: 19180 RVA: 0x001D3D5C File Offset: 0x001D1F5C
	// (remove) Token: 0x06004AED RID: 19181 RVA: 0x001D3D94 File Offset: 0x001D1F94
	public event QuestEvent_QuestCompleteEvent QuestComplete;

	// Token: 0x06004AEE RID: 19182 RVA: 0x001D3DC9 File Offset: 0x001D1FC9
	public void QuestCompleted(FastTags<TagGroup.Global> questTags, QuestClass questClass)
	{
		if (this.QuestComplete != null)
		{
			this.QuestComplete(questTags, questClass);
		}
	}

	// Token: 0x14000070 RID: 112
	// (add) Token: 0x06004AEF RID: 19183 RVA: 0x001D3DE0 File Offset: 0x001D1FE0
	// (remove) Token: 0x06004AF0 RID: 19184 RVA: 0x001D3E18 File Offset: 0x001D2018
	public event QuestEvent_ChallengeAwardCredit ChallengeAwardCredit;

	// Token: 0x06004AF1 RID: 19185 RVA: 0x001D3E4D File Offset: 0x001D204D
	public void ChallengeAwardCredited(string challengeStat, int creditAmount)
	{
		if (this.ChallengeAwardCredit != null)
		{
			this.ChallengeAwardCredit(challengeStat, creditAmount);
		}
	}

	// Token: 0x14000071 RID: 113
	// (add) Token: 0x06004AF2 RID: 19186 RVA: 0x001D3E64 File Offset: 0x001D2064
	// (remove) Token: 0x06004AF3 RID: 19187 RVA: 0x001D3E9C File Offset: 0x001D209C
	public event QuestEvent_ChallengeAwardCredit QuestAwardCredit;

	// Token: 0x06004AF4 RID: 19188 RVA: 0x001D3ED1 File Offset: 0x001D20D1
	public void QuestAwardCredited(string stat, int creditAmount)
	{
		if (this.QuestAwardCredit != null)
		{
			this.QuestAwardCredit(stat, creditAmount);
		}
	}

	// Token: 0x14000072 RID: 114
	// (add) Token: 0x06004AF5 RID: 19189 RVA: 0x001D3EE8 File Offset: 0x001D20E8
	// (remove) Token: 0x06004AF6 RID: 19190 RVA: 0x001D3F20 File Offset: 0x001D2120
	public event QuestEvent_BiomeEvent BiomeEnter;

	// Token: 0x06004AF7 RID: 19191 RVA: 0x001D3F55 File Offset: 0x001D2155
	public void BiomeEntered(BiomeDefinition biomeDef)
	{
		if (this.BiomeEnter != null)
		{
			this.BiomeEnter(biomeDef);
		}
	}

	// Token: 0x14000073 RID: 115
	// (add) Token: 0x06004AF8 RID: 19192 RVA: 0x001D3F6C File Offset: 0x001D216C
	// (remove) Token: 0x06004AF9 RID: 19193 RVA: 0x001D3FA4 File Offset: 0x001D21A4
	public event QuestEvent_ItemValueActionEvent UseItem;

	// Token: 0x06004AFA RID: 19194 RVA: 0x001D3FD9 File Offset: 0x001D21D9
	public void UsedItem(ItemValue newValue)
	{
		if (this.UseItem != null)
		{
			this.UseItem(newValue);
		}
	}

	// Token: 0x14000074 RID: 116
	// (add) Token: 0x06004AFB RID: 19195 RVA: 0x001D3FF0 File Offset: 0x001D21F0
	// (remove) Token: 0x06004AFC RID: 19196 RVA: 0x001D4028 File Offset: 0x001D2228
	public event QuestEvent_FloatEvent TimeSurvive;

	// Token: 0x06004AFD RID: 19197 RVA: 0x001D405D File Offset: 0x001D225D
	public void TimeSurvived(float time)
	{
		if (this.TimeSurvive != null)
		{
			this.TimeSurvive(time);
		}
	}

	// Token: 0x14000075 RID: 117
	// (add) Token: 0x06004AFE RID: 19198 RVA: 0x001D4074 File Offset: 0x001D2274
	// (remove) Token: 0x06004AFF RID: 19199 RVA: 0x001D40AC File Offset: 0x001D22AC
	public event QuestEvent_Event BloodMoonSurvive;

	// Token: 0x06004B00 RID: 19200 RVA: 0x001D40E1 File Offset: 0x001D22E1
	public void BloodMoonSurvived()
	{
		if (this.BloodMoonSurvive != null)
		{
			this.BloodMoonSurvive();
		}
	}

	// Token: 0x06004B01 RID: 19201 RVA: 0x001D40F6 File Offset: 0x001D22F6
	public void ForceUpdate()
	{
		this.Update();
	}

	// Token: 0x06004B02 RID: 19202 RVA: 0x001D4100 File Offset: 0x001D2300
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

	// Token: 0x06004B03 RID: 19203 RVA: 0x001D4268 File Offset: 0x001D2468
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

	// Token: 0x06004B04 RID: 19204 RVA: 0x001D42C0 File Offset: 0x001D24C0
	public void HandleAllPlayersDisconnect()
	{
		foreach (int num in this.TreasureQuestDictionary.Keys)
		{
			this.TreasureQuestDictionary[num].Remove();
		}
		this.TreasureQuestDictionary.Clear();
	}

	// Token: 0x06004B05 RID: 19205 RVA: 0x001D4330 File Offset: 0x001D2530
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

	// Token: 0x06004B06 RID: 19206 RVA: 0x001D435B File Offset: 0x001D255B
	public void ClearTraderResetQuestsForPlayer(int playerID)
	{
		if (this.ForceResetQuestTrader.ContainsKey(playerID))
		{
			this.ForceResetQuestTrader.Remove(playerID);
		}
	}

	// Token: 0x06004B07 RID: 19207 RVA: 0x001D4378 File Offset: 0x001D2578
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

	// Token: 0x06004B08 RID: 19208 RVA: 0x001D43C7 File Offset: 0x001D25C7
	public void AddObjectiveToBeUpdated(BaseObjective obj)
	{
		if (!this.objectivesToUpdate.Contains(obj))
		{
			this.objectivesToUpdate.Add(obj);
		}
	}

	// Token: 0x06004B09 RID: 19209 RVA: 0x001D43E3 File Offset: 0x001D25E3
	public void RemoveObjectiveToBeUpdated(BaseObjective obj)
	{
		if (this.objectivesToUpdate.Contains(obj))
		{
			this.objectivesToUpdate.Remove(obj);
		}
	}

	// Token: 0x06004B0A RID: 19210 RVA: 0x001D4400 File Offset: 0x001D2600
	public void AddObjectiveToBeUpdated(BaseChallengeObjective obj)
	{
		if (!this.challengeObjectivesToUpdate.Contains(obj))
		{
			this.challengeObjectivesToUpdate.Add(obj);
		}
	}

	// Token: 0x06004B0B RID: 19211 RVA: 0x001D441C File Offset: 0x001D261C
	public void RemoveObjectiveToBeUpdated(BaseChallengeObjective obj)
	{
		if (this.challengeObjectivesToUpdate.Contains(obj))
		{
			this.challengeObjectivesToUpdate.Remove(obj);
		}
	}

	// Token: 0x06004B0C RID: 19212 RVA: 0x001D4439 File Offset: 0x001D2639
	public void AddTrackerToBeUpdated(TrackingHandler track)
	{
		if (!this.questTrackersToUpdate.Contains(track))
		{
			this.questTrackersToUpdate.Add(track);
		}
	}

	// Token: 0x06004B0D RID: 19213 RVA: 0x001D4455 File Offset: 0x001D2655
	public void RemoveTrackerToBeUpdated(TrackingHandler track)
	{
		if (this.questTrackersToUpdate.Contains(track))
		{
			this.questTrackersToUpdate.Remove(track);
		}
	}

	// Token: 0x06004B0E RID: 19214 RVA: 0x001D4472 File Offset: 0x001D2672
	public void AddTrackerToBeUpdated(ChallengeTrackingHandler track)
	{
		this.challengeTrackerToUpdate = track;
	}

	// Token: 0x06004B0F RID: 19215 RVA: 0x001D447B File Offset: 0x001D267B
	public void RemoveTrackerToBeUpdated(ChallengeTrackingHandler track)
	{
		this.challengeTrackerToUpdate = null;
	}

	// Token: 0x14000076 RID: 118
	// (add) Token: 0x06004B10 RID: 19216 RVA: 0x001D4484 File Offset: 0x001D2684
	// (remove) Token: 0x06004B11 RID: 19217 RVA: 0x001D44BC File Offset: 0x001D26BC
	public event QuestEvent_SleeperVolumePositionChanged SleeperVolumePositionAdd;

	// Token: 0x14000077 RID: 119
	// (add) Token: 0x06004B12 RID: 19218 RVA: 0x001D44F4 File Offset: 0x001D26F4
	// (remove) Token: 0x06004B13 RID: 19219 RVA: 0x001D452C File Offset: 0x001D272C
	public event QuestEvent_SleeperVolumePositionChanged SleeperVolumePositionRemove;

	// Token: 0x06004B14 RID: 19220 RVA: 0x001D4561 File Offset: 0x001D2761
	public void SleeperVolumePositionAdded(Vector3 pos)
	{
		if (this.SleeperVolumePositionAdd != null)
		{
			this.SleeperVolumePositionAdd(pos);
		}
	}

	// Token: 0x06004B15 RID: 19221 RVA: 0x001D4577 File Offset: 0x001D2777
	public void SleeperVolumePositionRemoved(Vector3 pos)
	{
		if (this.SleeperVolumePositionRemove != null)
		{
			this.SleeperVolumePositionRemove(pos);
		}
	}

	// Token: 0x06004B16 RID: 19222 RVA: 0x001D458D File Offset: 0x001D278D
	public void AddSleeperVolumeLocation(Vector3 newLocation)
	{
		this.SleeperVolumeLocationList.Add(newLocation);
	}

	// Token: 0x06004B17 RID: 19223 RVA: 0x001D459C File Offset: 0x001D279C
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

	// Token: 0x06004B18 RID: 19224 RVA: 0x001D462C File Offset: 0x001D282C
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

	// Token: 0x06004B19 RID: 19225 RVA: 0x001D4704 File Offset: 0x001D2904
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

	// Token: 0x06004B1A RID: 19226 RVA: 0x001D4734 File Offset: 0x001D2934
	public void QuestUnlockPOI(int entityID, Vector3 prefabPos)
	{
		PrefabInstance prefabFromWorldPos = GameManager.Instance.GetDynamicPrefabDecorator().GetPrefabFromWorldPos((int)prefabPos.x, (int)prefabPos.z);
		if (prefabFromWorldPos.lockInstance != null)
		{
			prefabFromWorldPos.lockInstance.RemoveQuester(entityID);
		}
	}

	// Token: 0x06004B1B RID: 19227 RVA: 0x001D4774 File Offset: 0x001D2974
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

	// Token: 0x06004B1C RID: 19228 RVA: 0x001D48DC File Offset: 0x001D2ADC
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

	// Token: 0x06004B1D RID: 19229 RVA: 0x001D4A74 File Offset: 0x001D2C74
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

	// Token: 0x06004B1E RID: 19230 RVA: 0x001D4D9C File Offset: 0x001D2F9C
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

	// Token: 0x06004B1F RID: 19231 RVA: 0x001D4DAC File Offset: 0x001D2FAC
	public void SetupFetchForMP(int entityID, Vector3 prefabPos, ObjectiveFetchFromContainer.FetchModeTypes fetchMode, int[] sharedWithList)
	{
		PrefabInstance prefabFromWorldPos = GameManager.Instance.GetDynamicPrefabDecorator().GetPrefabFromWorldPos((int)prefabPos.x, (int)prefabPos.z);
		this.HandleContainerPositions(GameManager.Instance.World, entityID, new Vector3i(prefabPos), prefabFromWorldPos.prefab.size, fetchMode, sharedWithList);
	}

	// Token: 0x06004B20 RID: 19232 RVA: 0x001D4DFC File Offset: 0x001D2FFC
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

	// Token: 0x06004B21 RID: 19233 RVA: 0x001D5184 File Offset: 0x001D3384
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

	// Token: 0x06004B22 RID: 19234 RVA: 0x001D5264 File Offset: 0x001D3464
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

	// Token: 0x06004B23 RID: 19235 RVA: 0x001D532C File Offset: 0x001D352C
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

	// Token: 0x06004B24 RID: 19236 RVA: 0x001D53CE File Offset: 0x001D35CE
	public void ClearQuestList(int npcEntityID)
	{
		if (this.npcQuestData.ContainsKey(npcEntityID))
		{
			this.npcQuestData[npcEntityID].PlayerQuestList.Clear();
		}
	}

	// Token: 0x06004B25 RID: 19237 RVA: 0x001D53F4 File Offset: 0x001D35F4
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

	// Token: 0x06004B26 RID: 19238 RVA: 0x001D5437 File Offset: 0x001D3637
	public void AddQuestTierReward(QuestTierReward reward)
	{
		if (this.questTierRewards == null)
		{
			this.questTierRewards = new List<QuestTierReward>();
		}
		this.questTierRewards.Add(reward);
	}

	// Token: 0x06004B27 RID: 19239 RVA: 0x001D5458 File Offset: 0x001D3658
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

	// Token: 0x06004B28 RID: 19240 RVA: 0x001D54C8 File Offset: 0x001D36C8
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

	// Token: 0x06004B29 RID: 19241 RVA: 0x001D55D0 File Offset: 0x001D37D0
	public void AddTreasureQuest(int _questCode, int _entityID, int _blocksPerReduction, Vector3i _position, Vector3 _treasureOffset)
	{
		if (!this.TreasureQuestDictionary.ContainsKey(_questCode))
		{
			TreasureQuestData treasureQuestData = new TreasureQuestData(_questCode, _entityID, _blocksPerReduction, _position, _treasureOffset);
			this.TreasureQuestDictionary.Add(_questCode, treasureQuestData);
		}
	}

	// Token: 0x06004B2A RID: 19242 RVA: 0x001D5605 File Offset: 0x001D3805
	public void SetTreasureContainerPosition(int _questCode, Vector3i _updatedPosition)
	{
		if (this.TreasureQuestDictionary.ContainsKey(_questCode))
		{
			this.TreasureQuestDictionary[_questCode].UpdatePosition(_updatedPosition);
		}
	}

	// Token: 0x06004B2B RID: 19243 RVA: 0x001D5628 File Offset: 0x001D3828
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

	// Token: 0x06004B2C RID: 19244 RVA: 0x001D576C File Offset: 0x001D396C
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

	// Token: 0x06004B2D RID: 19245 RVA: 0x001D57C0 File Offset: 0x001D39C0
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

	// Token: 0x06004B2E RID: 19246 RVA: 0x001D5874 File Offset: 0x001D3A74
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

	// Token: 0x06004B2F RID: 19247 RVA: 0x001D58BA File Offset: 0x001D3ABA
	public void FinishManagedQuest(int _questCode, EntityPlayer _player)
	{
		if (this.BlockActivateQuestDictionary.ContainsKey(_questCode))
		{
			this.BlockActivateQuestDictionary[_questCode].RemoveSharedQuester(_player);
		}
	}

	// Token: 0x04003B46 RID: 15174
	[PublicizedFrom(EAccessModifier.Private)]
	public static QuestEventManager instance = null;

	// Token: 0x04003B47 RID: 15175
	[PublicizedFrom(EAccessModifier.Private)]
	public List<BaseObjective> objectivesToUpdate = new List<BaseObjective>();

	// Token: 0x04003B48 RID: 15176
	[PublicizedFrom(EAccessModifier.Private)]
	public List<BaseChallengeObjective> challengeObjectivesToUpdate = new List<BaseChallengeObjective>();

	// Token: 0x04003B49 RID: 15177
	[PublicizedFrom(EAccessModifier.Private)]
	public List<TrackingHandler> questTrackersToUpdate = new List<TrackingHandler>();

	// Token: 0x04003B4A RID: 15178
	[PublicizedFrom(EAccessModifier.Private)]
	public ChallengeTrackingHandler challengeTrackerToUpdate;

	// Token: 0x04003B4B RID: 15179
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Vector3> removeSleeperDataList = new List<Vector3>();

	// Token: 0x04003B4C RID: 15180
	public Dictionary<int, NPCQuestData> npcQuestData = new Dictionary<int, NPCQuestData>();

	// Token: 0x04003B4D RID: 15181
	public List<QuestTierReward> questTierRewards = new List<QuestTierReward>();

	// Token: 0x04003B4E RID: 15182
	public Dictionary<Vector3, SleeperEventData> SleeperVolumeUpdateDictionary = new Dictionary<Vector3, SleeperEventData>();

	// Token: 0x04003B4F RID: 15183
	public List<Vector3> SleeperVolumeLocationList = new List<Vector3>();

	// Token: 0x04003B50 RID: 15184
	public Dictionary<int, TreasureQuestData> TreasureQuestDictionary = new Dictionary<int, TreasureQuestData>();

	// Token: 0x04003B51 RID: 15185
	public Dictionary<int, RestorePowerQuestData> BlockActivateQuestDictionary = new Dictionary<int, RestorePowerQuestData>();

	// Token: 0x04003B52 RID: 15186
	public Dictionary<int, List<PrefabInstance>> tierPrefabList = new Dictionary<int, List<PrefabInstance>>();

	// Token: 0x04003B53 RID: 15187
	public Dictionary<TraderArea, List<QuestEventManager.PrefabListData>> TraderPrefabList = new Dictionary<TraderArea, List<QuestEventManager.PrefabListData>>();

	// Token: 0x04003B54 RID: 15188
	public Rect QuestBounds;

	// Token: 0x04003B55 RID: 15189
	public List<Vector3i> ActiveQuestBlocks = new List<Vector3i>();

	// Token: 0x04003B56 RID: 15190
	public Dictionary<int, int> ForceResetQuestTrader = new Dictionary<int, int>();

	// Token: 0x04003B57 RID: 15191
	public static FastTags<TagGroup.Global> manualResetTag = FastTags<TagGroup.Global>.Parse("manual");

	// Token: 0x04003B58 RID: 15192
	public static FastTags<TagGroup.Global> traderTag = FastTags<TagGroup.Global>.Parse("trader");

	// Token: 0x04003B59 RID: 15193
	public static FastTags<TagGroup.Global> clearTag = FastTags<TagGroup.Global>.Parse("clear");

	// Token: 0x04003B5A RID: 15194
	public static FastTags<TagGroup.Global> treasureTag = FastTags<TagGroup.Global>.Parse("treasure");

	// Token: 0x04003B5B RID: 15195
	public static FastTags<TagGroup.Global> fetchTag = FastTags<TagGroup.Global>.Parse("fetch");

	// Token: 0x04003B5C RID: 15196
	public static FastTags<TagGroup.Global> craftingTag = FastTags<TagGroup.Global>.Parse("crafting");

	// Token: 0x04003B5D RID: 15197
	public static FastTags<TagGroup.Global> restorePowerTag = FastTags<TagGroup.Global>.Parse("restore_power");

	// Token: 0x04003B5E RID: 15198
	public static FastTags<TagGroup.Global> infestedTag = FastTags<TagGroup.Global>.Parse("infested");

	// Token: 0x04003B5F RID: 15199
	public static FastTags<TagGroup.Global> banditTag = FastTags<TagGroup.Global>.Parse("bandit");

	// Token: 0x04003B60 RID: 15200
	public static FastTags<TagGroup.Global> allQuestTags = FastTags<TagGroup.Global>.CombineTags(FastTags<TagGroup.Global>.CombineTags(QuestEventManager.traderTag, QuestEventManager.clearTag, QuestEventManager.treasureTag, QuestEventManager.fetchTag), FastTags<TagGroup.Global>.CombineTags(QuestEventManager.craftingTag, QuestEventManager.restorePowerTag), FastTags<TagGroup.Global>.CombineTags(QuestEventManager.infestedTag, QuestEventManager.banditTag));

	// Token: 0x04003B61 RID: 15201
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cTreasurePointAttempts = 5;

	// Token: 0x04003B62 RID: 15202
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cTreasurePointDistanceAdd = 2.5f;

	// Token: 0x04003B63 RID: 15203
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cTreasurePointMaxDistanceAdd = 25f;

	// Token: 0x020009F0 RID: 2544
	public enum POILockoutReasonTypes
	{
		// Token: 0x04003B8A RID: 15242
		None,
		// Token: 0x04003B8B RID: 15243
		PlayerInside,
		// Token: 0x04003B8C RID: 15244
		Bedroll,
		// Token: 0x04003B8D RID: 15245
		LandClaim,
		// Token: 0x04003B8E RID: 15246
		QuestLock
	}

	// Token: 0x020009F1 RID: 2545
	public class PrefabListData
	{
		// Token: 0x06004B31 RID: 19249 RVA: 0x001D59B8 File Offset: 0x001D3BB8
		public void AddPOI(PrefabInstance poi)
		{
			int difficultyTier = (int)poi.prefab.DifficultyTier;
			if (!this.TierData.ContainsKey(difficultyTier))
			{
				this.TierData.Add(difficultyTier, new List<PrefabInstance>());
			}
			this.TierData[difficultyTier].Add(poi);
		}

		// Token: 0x06004B32 RID: 19250 RVA: 0x001D5A04 File Offset: 0x001D3C04
		public void ShuffleDifficulty(int difficulty, GameRandom gameRandom)
		{
			List<PrefabInstance> list;
			if (this.TierData.TryGetValue(difficulty, out list))
			{
				list.Shuffle<PrefabInstance>(gameRandom);
			}
		}

		// Token: 0x04003B8F RID: 15247
		public Dictionary<int, List<PrefabInstance>> TierData = new Dictionary<int, List<PrefabInstance>>();
	}
}

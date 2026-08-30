using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000997 RID: 2455
[Preserve]
public class ObjectiveFetchFromTreasure : BaseObjective
{
	// Token: 0x1700079E RID: 1950
	// (get) Token: 0x06004817 RID: 18455 RVA: 0x00010E62 File Offset: 0x0000F062
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Boolean;
		}
	}

	// Token: 0x06004818 RID: 18456 RVA: 0x001C307C File Offset: 0x001C127C
	[PublicizedFrom(EAccessModifier.Private)]
	public void RemoveFetchItems()
	{
		if (this.expectedItemClass == null)
		{
			this.SetupExpectedItem();
		}
		XUi xui = LocalPlayerUI.GetUIForPlayer(base.OwnerQuest.OwnerJournal.OwnerPlayer).xui;
		XUiM_PlayerInventory playerInventory = xui.PlayerInventory;
		int num = 1;
		int num2 = 1;
		num2 -= playerInventory.Backpack.DecItem(this.expectedItem, num2, false, null);
		if (num2 > 0)
		{
			playerInventory.Toolbelt.DecItem(this.expectedItem, num2, false, null);
		}
		if (num != num2)
		{
			xui.CollectedItemList.RemoveItemStack(new ItemStack(this.expectedItem.Clone(), num - num2));
		}
	}

	// Token: 0x06004819 RID: 18457 RVA: 0x001C310F File Offset: 0x001C130F
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveFetchContainer_keyword", false, null);
		if (this.expectedItemClass == null)
		{
			this.SetupExpectedItem();
		}
	}

	// Token: 0x0600481A RID: 18458 RVA: 0x001C3134 File Offset: 0x001C1334
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetupExpectedItem()
	{
		if (base.OwnerQuest.QuestCode == 0)
		{
			base.OwnerQuest.SetupQuestCode();
		}
		this.expectedItemClass = ItemClass.GetItemClass(ObjectiveFetchFromTreasure.questItemClassID, false);
		int id = this.expectedItemClass.Id;
		ushort num = StringParsers.ParseUInt16(this.ID, 0, -1, NumberStyles.Integer);
		this.expectedItemClass = ItemClassQuest.GetItemQuestById(num);
		this.expectedItem = new ItemValue(id, false);
		this.expectedItem.Seed = num;
		this.expectedItem.Meta = base.OwnerQuest.QuestCode;
		this.itemCount = 1;
	}

	// Token: 0x0600481B RID: 18459 RVA: 0x001C31C7 File Offset: 0x001C13C7
	public override void SetupDisplay()
	{
		base.Description = string.Format(this.keyword, this.expectedItemClass.GetLocalizedItemName());
		this.StatusText = "";
	}

	// Token: 0x0600481C RID: 18460 RVA: 0x001C31F0 File Offset: 0x001C13F0
	public override void HandleCompleted()
	{
		base.HandleCompleted();
		this.RemoveFetchItems();
	}

	// Token: 0x0600481D RID: 18461 RVA: 0x001C31FE File Offset: 0x001C13FE
	public override void HandlePhaseCompleted()
	{
		base.HandlePhaseCompleted();
	}

	// Token: 0x0600481E RID: 18462 RVA: 0x001C3206 File Offset: 0x001C1406
	public override void HandleFailed()
	{
		base.HandleFailed();
		this.RemoveFetchItems();
	}

	// Token: 0x0600481F RID: 18463 RVA: 0x001C3214 File Offset: 0x001C1414
	public override void AddHooks()
	{
		LocalPlayerUI.GetUIForPlayer(base.OwnerQuest.OwnerJournal.OwnerPlayer);
		XUiM_PlayerInventory playerInventory = LocalPlayerUI.GetUIForPlayer(base.OwnerQuest.OwnerJournal.OwnerPlayer).xui.PlayerInventory;
		playerInventory.Backpack.OnBackpackItemsChangedInternal += this.Backpack_OnBackpackItemsChangedInternal;
		playerInventory.Toolbelt.OnToolbeltItemsChangedInternal += this.Toolbelt_OnToolbeltItemsChangedInternal;
		QuestEventManager.Current.ContainerOpened += this.Current_ContainerOpened;
		QuestEventManager.Current.ContainerClosed += this.Current_ContainerClosed;
		QuestEventManager.Current.BlockChange += this.Current_BlockChange;
		this.Refresh();
	}

	// Token: 0x06004820 RID: 18464 RVA: 0x001C32CC File Offset: 0x001C14CC
	public override void RemoveObjectives()
	{
		QuestEventManager.Current.ContainerOpened -= this.Current_ContainerOpened;
		QuestEventManager.Current.ContainerClosed -= this.Current_ContainerClosed;
		QuestEventManager.Current.BlockChange -= this.Current_BlockChange;
	}

	// Token: 0x06004821 RID: 18465 RVA: 0x001C331C File Offset: 0x001C151C
	public override void RemoveHooks()
	{
		base.OwnerQuest.RemoveMapObject();
		XUiM_PlayerInventory playerInventory = LocalPlayerUI.GetUIForPlayer(base.OwnerQuest.OwnerJournal.OwnerPlayer).xui.PlayerInventory;
		if (playerInventory != null)
		{
			playerInventory.Backpack.OnBackpackItemsChangedInternal -= this.Backpack_OnBackpackItemsChangedInternal;
			playerInventory.Toolbelt.OnToolbeltItemsChangedInternal -= this.Toolbelt_OnToolbeltItemsChangedInternal;
		}
	}

	// Token: 0x06004822 RID: 18466 RVA: 0x001C3388 File Offset: 0x001C1588
	[PublicizedFrom(EAccessModifier.Private)]
	public void Backpack_OnBackpackItemsChangedInternal()
	{
		LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(base.OwnerQuest.OwnerJournal.OwnerPlayer);
		if (base.Complete || uiforPlayer.xui.PlayerInventory == null)
		{
			return;
		}
		this.Refresh();
	}

	// Token: 0x06004823 RID: 18467 RVA: 0x001C33C8 File Offset: 0x001C15C8
	[PublicizedFrom(EAccessModifier.Private)]
	public void Toolbelt_OnToolbeltItemsChangedInternal()
	{
		LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(base.OwnerQuest.OwnerJournal.OwnerPlayer);
		if (base.Complete || uiforPlayer.xui.PlayerInventory == null)
		{
			return;
		}
		this.Refresh();
	}

	// Token: 0x06004824 RID: 18468 RVA: 0x001C3408 File Offset: 0x001C1608
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_ContainerOpened(Vector3i containerLocation, ITileEntityLootable lootTE)
	{
		Vector3 zero = Vector3.zero;
		base.OwnerQuest.GetPositionData(out zero, Quest.PositionDataTypes.TreasurePoint);
		if ((float)containerLocation.x != zero.x || (float)containerLocation.z != zero.z)
		{
			return;
		}
		if (this.GetItemCount() >= 1)
		{
			return;
		}
		if (GameManager.Instance.World.GetBlock(containerLocation).Block.GetBlockName() == this.containerName && lootTE != null && !lootTE.HasItem(this.expectedItem))
		{
			this.hasOpened = true;
			lootTE.AddItem(new ItemStack(this.expectedItem, 1));
		}
	}

	// Token: 0x06004825 RID: 18469 RVA: 0x001C34A8 File Offset: 0x001C16A8
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_ContainerClosed(Vector3i containerLocation, ITileEntityLootable lootTE)
	{
		if (GameManager.Instance.World.GetBlock(containerLocation).Block.GetBlockName() == this.containerName && lootTE != null)
		{
			lootTE.RemoveItem(this.expectedItem);
			lootTE.SetModified();
			if (base.Complete)
			{
				if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
				{
					QuestEventManager.Current.FinishTreasureQuest(base.OwnerQuest.QuestCode, base.OwnerQuest.OwnerJournal.OwnerPlayer);
					return;
				}
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageQuestObjectiveUpdate>().Setup(NetPackageQuestObjectiveUpdate.QuestObjectiveEventTypes.TreasureComplete, base.OwnerQuest.OwnerJournal.OwnerPlayer.entityId, base.OwnerQuest.QuestCode), false);
			}
		}
	}

	// Token: 0x06004826 RID: 18470 RVA: 0x001C356C File Offset: 0x001C176C
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_BlockChange(Block blockOld, Block blockNew, Vector3i blockPos)
	{
		if (base.Complete || !this.hasOpened)
		{
			return;
		}
		Vector3 vector;
		base.OwnerQuest.GetPositionData(out vector, Quest.PositionDataTypes.TreasurePoint);
		this.containerPos = new Vector3i(vector);
		if (blockPos != this.containerPos)
		{
			return;
		}
		Chunk chunk = GameManager.Instance.World.GetChunkFromWorldPos(blockPos) as Chunk;
		if (chunk != null && chunk.IsDisplayed)
		{
			string blockName = blockNew.GetBlockName();
			if (blockName != this.containerName && blockName != this.altContainerName)
			{
				base.OwnerQuest.CloseQuest(Quest.QuestState.Failed, null);
			}
		}
	}

	// Token: 0x06004827 RID: 18471 RVA: 0x001C3608 File Offset: 0x001C1808
	[PublicizedFrom(EAccessModifier.Private)]
	public int GetItemCount()
	{
		XUiM_PlayerInventory playerInventory = LocalPlayerUI.GetUIForPlayer(base.OwnerQuest.OwnerJournal.OwnerPlayer).xui.PlayerInventory;
		this.expectedItem.Meta = base.OwnerQuest.QuestCode;
		return playerInventory.Backpack.GetItemCount(this.expectedItem, -1, base.OwnerQuest.QuestCode, true) + playerInventory.Toolbelt.GetItemCount(this.expectedItem, false, -1, base.OwnerQuest.QuestCode, true);
	}

	// Token: 0x06004828 RID: 18472 RVA: 0x001C368C File Offset: 0x001C188C
	public override void Refresh()
	{
		if (base.Complete)
		{
			return;
		}
		this.currentCount = this.GetItemCount();
		if (this.currentCount > 1)
		{
			this.currentCount = 1;
		}
		this.SetupDisplay();
		if (this.currentCount != (int)base.CurrentValue)
		{
			base.CurrentValue = (byte)this.currentCount;
		}
		base.Complete = this.currentCount >= this.itemCount && base.OwnerQuest.CheckRequirements();
		if (base.Complete)
		{
			base.OwnerQuest.RefreshQuestCompletion(QuestClass.CompletionTypes.AutoComplete, null, true, null);
			this.RemoveHooks();
		}
	}

	// Token: 0x06004829 RID: 18473 RVA: 0x001C3720 File Offset: 0x001C1920
	public override BaseObjective Clone()
	{
		ObjectiveFetchFromTreasure objectiveFetchFromTreasure = new ObjectiveFetchFromTreasure();
		this.CopyValues(objectiveFetchFromTreasure);
		return objectiveFetchFromTreasure;
	}

	// Token: 0x0600482A RID: 18474 RVA: 0x001C373B File Offset: 0x001C193B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CopyValues(BaseObjective objective)
	{
		base.CopyValues(objective);
		ObjectiveFetchFromTreasure objectiveFetchFromTreasure = (ObjectiveFetchFromTreasure)objective;
		objectiveFetchFromTreasure.containerName = this.containerName;
		objectiveFetchFromTreasure.hasOpened = this.hasOpened;
		objectiveFetchFromTreasure.containerName = this.containerName;
		objectiveFetchFromTreasure.altContainerName = this.altContainerName;
	}

	// Token: 0x0600482B RID: 18475 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool SetLocation(Vector3 pos, Vector3 size)
	{
		return true;
	}

	// Token: 0x0600482C RID: 18476 RVA: 0x001C377C File Offset: 0x001C197C
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		if (properties.Values.ContainsKey(ObjectiveFetchFromTreasure.PropQuestItemID))
		{
			this.ID = properties.Values[ObjectiveFetchFromTreasure.PropQuestItemID];
		}
		if (properties.Values.ContainsKey(ObjectiveFetchFromTreasure.PropItemCount))
		{
			this.Value = properties.Values[ObjectiveFetchFromTreasure.PropItemCount];
		}
		if (properties.Values.ContainsKey(ObjectiveFetchFromTreasure.PropBlock))
		{
			this.containerName = properties.Values[ObjectiveFetchFromTreasure.PropBlock];
		}
		if (properties.Values.ContainsKey(ObjectiveFetchFromTreasure.PropAltBlock))
		{
			this.altContainerName = properties.Values[ObjectiveFetchFromTreasure.PropAltBlock];
		}
	}

	// Token: 0x04003994 RID: 14740
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue expectedItem = ItemValue.None;

	// Token: 0x04003995 RID: 14741
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass expectedItemClass;

	// Token: 0x04003996 RID: 14742
	[PublicizedFrom(EAccessModifier.Private)]
	public int itemCount;

	// Token: 0x04003997 RID: 14743
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentCount;

	// Token: 0x04003998 RID: 14744
	[PublicizedFrom(EAccessModifier.Private)]
	public string containerName = "";

	// Token: 0x04003999 RID: 14745
	[PublicizedFrom(EAccessModifier.Private)]
	public string altContainerName = "";

	// Token: 0x0400399A RID: 14746
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasOpened;

	// Token: 0x0400399B RID: 14747
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i containerPos = Vector3i.zero;

	// Token: 0x0400399C RID: 14748
	public static string questItemClassID = "questItem";

	// Token: 0x0400399D RID: 14749
	public static string PropQuestItemID = "quest_item_ID";

	// Token: 0x0400399E RID: 14750
	public static string PropItemCount = "item_count";

	// Token: 0x0400399F RID: 14751
	public static string PropBlock = "block";

	// Token: 0x040039A0 RID: 14752
	public static string PropAltBlock = "alt_block";
}

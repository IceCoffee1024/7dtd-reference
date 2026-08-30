using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000986 RID: 2438
[Preserve]
public class ObjectiveFetchFromTreasure : BaseObjective
{
	// Token: 0x1700078C RID: 1932
	// (get) Token: 0x06004791 RID: 18321 RVA: 0x00010E62 File Offset: 0x0000F062
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Boolean;
		}
	}

	// Token: 0x06004792 RID: 18322 RVA: 0x001C0888 File Offset: 0x001BEA88
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

	// Token: 0x06004793 RID: 18323 RVA: 0x001C091B File Offset: 0x001BEB1B
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveFetchContainer_keyword", false, null);
		if (this.expectedItemClass == null)
		{
			this.SetupExpectedItem();
		}
	}

	// Token: 0x06004794 RID: 18324 RVA: 0x001C0940 File Offset: 0x001BEB40
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

	// Token: 0x06004795 RID: 18325 RVA: 0x001C09D3 File Offset: 0x001BEBD3
	public override void SetupDisplay()
	{
		base.Description = string.Format(this.keyword, this.expectedItemClass.GetLocalizedItemName());
		this.StatusText = "";
	}

	// Token: 0x06004796 RID: 18326 RVA: 0x001C09FC File Offset: 0x001BEBFC
	public override void HandleCompleted()
	{
		base.HandleCompleted();
		this.RemoveFetchItems();
	}

	// Token: 0x06004797 RID: 18327 RVA: 0x001C0A0A File Offset: 0x001BEC0A
	public override void HandlePhaseCompleted()
	{
		base.HandlePhaseCompleted();
	}

	// Token: 0x06004798 RID: 18328 RVA: 0x001C0A12 File Offset: 0x001BEC12
	public override void HandleFailed()
	{
		base.HandleFailed();
		this.RemoveFetchItems();
	}

	// Token: 0x06004799 RID: 18329 RVA: 0x001C0A20 File Offset: 0x001BEC20
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

	// Token: 0x0600479A RID: 18330 RVA: 0x001C0AD8 File Offset: 0x001BECD8
	public override void RemoveObjectives()
	{
		QuestEventManager.Current.ContainerOpened -= this.Current_ContainerOpened;
		QuestEventManager.Current.ContainerClosed -= this.Current_ContainerClosed;
		QuestEventManager.Current.BlockChange -= this.Current_BlockChange;
	}

	// Token: 0x0600479B RID: 18331 RVA: 0x001C0B28 File Offset: 0x001BED28
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

	// Token: 0x0600479C RID: 18332 RVA: 0x001C0B94 File Offset: 0x001BED94
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

	// Token: 0x0600479D RID: 18333 RVA: 0x001C0BD4 File Offset: 0x001BEDD4
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

	// Token: 0x0600479E RID: 18334 RVA: 0x001C0C14 File Offset: 0x001BEE14
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

	// Token: 0x0600479F RID: 18335 RVA: 0x001C0CB4 File Offset: 0x001BEEB4
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

	// Token: 0x060047A0 RID: 18336 RVA: 0x001C0D78 File Offset: 0x001BEF78
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

	// Token: 0x060047A1 RID: 18337 RVA: 0x001C0E14 File Offset: 0x001BF014
	[PublicizedFrom(EAccessModifier.Private)]
	public int GetItemCount()
	{
		XUiM_PlayerInventory playerInventory = LocalPlayerUI.GetUIForPlayer(base.OwnerQuest.OwnerJournal.OwnerPlayer).xui.PlayerInventory;
		this.expectedItem.Meta = base.OwnerQuest.QuestCode;
		return playerInventory.Backpack.GetItemCount(this.expectedItem, -1, base.OwnerQuest.QuestCode, true) + playerInventory.Toolbelt.GetItemCount(this.expectedItem, false, -1, base.OwnerQuest.QuestCode, true);
	}

	// Token: 0x060047A2 RID: 18338 RVA: 0x001C0E98 File Offset: 0x001BF098
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

	// Token: 0x060047A3 RID: 18339 RVA: 0x001C0F2C File Offset: 0x001BF12C
	public override BaseObjective Clone()
	{
		ObjectiveFetchFromTreasure objectiveFetchFromTreasure = new ObjectiveFetchFromTreasure();
		this.CopyValues(objectiveFetchFromTreasure);
		return objectiveFetchFromTreasure;
	}

	// Token: 0x060047A4 RID: 18340 RVA: 0x001C0F47 File Offset: 0x001BF147
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

	// Token: 0x060047A5 RID: 18341 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool SetLocation(Vector3 pos, Vector3 size)
	{
		return true;
	}

	// Token: 0x060047A6 RID: 18342 RVA: 0x001C0F88 File Offset: 0x001BF188
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

	// Token: 0x04003903 RID: 14595
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue expectedItem = ItemValue.None;

	// Token: 0x04003904 RID: 14596
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass expectedItemClass;

	// Token: 0x04003905 RID: 14597
	[PublicizedFrom(EAccessModifier.Private)]
	public int itemCount;

	// Token: 0x04003906 RID: 14598
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentCount;

	// Token: 0x04003907 RID: 14599
	[PublicizedFrom(EAccessModifier.Private)]
	public string containerName = "";

	// Token: 0x04003908 RID: 14600
	[PublicizedFrom(EAccessModifier.Private)]
	public string altContainerName = "";

	// Token: 0x04003909 RID: 14601
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasOpened;

	// Token: 0x0400390A RID: 14602
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i containerPos = Vector3i.zero;

	// Token: 0x0400390B RID: 14603
	public static string questItemClassID = "questItem";

	// Token: 0x0400390C RID: 14604
	public static string PropQuestItemID = "quest_item_ID";

	// Token: 0x0400390D RID: 14605
	public static string PropItemCount = "item_count";

	// Token: 0x0400390E RID: 14606
	public static string PropBlock = "block";

	// Token: 0x0400390F RID: 14607
	public static string PropAltBlock = "alt_block";
}

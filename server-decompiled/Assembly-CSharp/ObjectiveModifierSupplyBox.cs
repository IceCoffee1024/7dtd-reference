using System;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x0200097C RID: 2428
[Preserve]
public class ObjectiveModifierSupplyBox : BaseObjectiveModifier
{
	// Token: 0x060046FF RID: 18175 RVA: 0x001BF3CE File Offset: 0x001BD5CE
	public override void AddHooks()
	{
		QuestEventManager.Current.ContainerOpened += this.Current_ContainerOpened;
		QuestEventManager.Current.ContainerClosed += this.Current_ContainerClosed;
	}

	// Token: 0x06004700 RID: 18176 RVA: 0x001BF3FC File Offset: 0x001BD5FC
	public override void RemoveHooks()
	{
		QuestEventManager.Current.ContainerOpened -= this.Current_ContainerOpened;
		QuestEventManager.Current.ContainerClosed -= this.Current_ContainerClosed;
	}

	// Token: 0x06004701 RID: 18177 RVA: 0x001BF42C File Offset: 0x001BD62C
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_ContainerOpened(Vector3i containerLocation, ITileEntityLootable tileEntity)
	{
		if (this.expectedItemClass == null)
		{
			this.SetupExpectedItem();
		}
		int num = this.GetItemCount();
		if (num >= this.itemCount)
		{
			return;
		}
		if (tileEntity.blockValue.Block.GetBlockName() == this.defaultContainer && !tileEntity.HasItem(this.expectedItem))
		{
			tileEntity.AddItem(new ItemStack(this.expectedItem, this.itemCount - num));
		}
	}

	// Token: 0x06004702 RID: 18178 RVA: 0x001BF4A0 File Offset: 0x001BD6A0
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_ContainerClosed(Vector3i containerLocation, ITileEntityLootable tileEntity)
	{
		if (tileEntity.blockValue.Block.GetBlockName() == this.defaultContainer)
		{
			tileEntity.RemoveItem(this.expectedItem);
			tileEntity.SetModified();
		}
	}

	// Token: 0x06004703 RID: 18179 RVA: 0x001BF4E0 File Offset: 0x001BD6E0
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		if (properties.Values.ContainsKey(ObjectiveModifierSupplyBox.PropExpectedItemClassID))
		{
			this.expectedItemClassID = properties.Values[ObjectiveModifierSupplyBox.PropExpectedItemClassID];
		}
		if (properties.Values.ContainsKey(ObjectiveModifierSupplyBox.PropQuestItemID))
		{
			this.expectedQuestItemID = properties.Values[ObjectiveModifierSupplyBox.PropQuestItemID];
		}
		if (properties.Values.ContainsKey(ObjectiveModifierSupplyBox.PropItemCount))
		{
			this.itemCount = StringParsers.ParseSInt32(properties.Values[ObjectiveModifierSupplyBox.PropItemCount], 0, -1, NumberStyles.Integer);
		}
		if (properties.Values.ContainsKey(ObjectiveModifierSupplyBox.PropDefaultContainer))
		{
			this.defaultContainer = properties.Values[ObjectiveModifierSupplyBox.PropDefaultContainer];
		}
	}

	// Token: 0x06004704 RID: 18180 RVA: 0x001BF59C File Offset: 0x001BD79C
	[PublicizedFrom(EAccessModifier.Protected)]
	public void SetupExpectedItem()
	{
		if (base.OwnerObjective.OwnerQuest.QuestCode == 0)
		{
			base.OwnerObjective.OwnerQuest.SetupQuestCode();
		}
		this.expectedItemClass = ItemClass.GetItemClass(this.expectedItemClassID, false);
		this.expectedItem = new ItemValue(this.expectedItemClass.Id, false);
		if (this.expectedItemClass is ItemClassQuest)
		{
			ushort num = StringParsers.ParseUInt16(this.expectedQuestItemID, 0, -1, NumberStyles.Integer);
			this.expectedItemClass = ItemClassQuest.GetItemQuestById(num);
			this.expectedItem.Seed = num;
		}
		this.expectedItem.Meta = base.OwnerObjective.OwnerQuest.QuestCode;
	}

	// Token: 0x06004705 RID: 18181 RVA: 0x001BF644 File Offset: 0x001BD844
	[PublicizedFrom(EAccessModifier.Protected)]
	public void RemoveFetchItems()
	{
		if (this.expectedItemClass == null)
		{
			this.SetupExpectedItem();
		}
		XUi xui = LocalPlayerUI.GetUIForPlayer(base.OwnerObjective.OwnerQuest.OwnerJournal.OwnerPlayer).xui;
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
			ItemStack itemStack = new ItemStack(this.expectedItem.Clone(), num - num2);
			xui.CollectedItemList.AddRemoveItemQueueEntry(itemStack);
		}
	}

	// Token: 0x06004706 RID: 18182 RVA: 0x001BF6E0 File Offset: 0x001BD8E0
	[PublicizedFrom(EAccessModifier.Protected)]
	public int GetItemCount()
	{
		XUiM_PlayerInventory playerInventory = LocalPlayerUI.GetUIForPlayer(base.OwnerObjective.OwnerQuest.OwnerJournal.OwnerPlayer).xui.PlayerInventory;
		this.expectedItem.Meta = base.OwnerObjective.OwnerQuest.QuestCode;
		return playerInventory.Backpack.GetItemCount(this.expectedItem, -1, base.OwnerObjective.OwnerQuest.QuestCode, true) + playerInventory.Toolbelt.GetItemCount(this.expectedItem, false, -1, base.OwnerObjective.OwnerQuest.QuestCode, true);
	}

	// Token: 0x06004707 RID: 18183 RVA: 0x001BF775 File Offset: 0x001BD975
	public override BaseObjectiveModifier Clone()
	{
		return new ObjectiveModifierSupplyBox
		{
			expectedItemClassID = this.expectedItemClassID,
			expectedQuestItemID = this.expectedQuestItemID,
			itemCount = this.itemCount,
			defaultContainer = this.defaultContainer
		};
	}

	// Token: 0x04003900 RID: 14592
	[PublicizedFrom(EAccessModifier.Protected)]
	public ItemValue expectedItem = ItemValue.None;

	// Token: 0x04003901 RID: 14593
	[PublicizedFrom(EAccessModifier.Protected)]
	public ItemClass expectedItemClass;

	// Token: 0x04003902 RID: 14594
	[PublicizedFrom(EAccessModifier.Protected)]
	public int currentCount;

	// Token: 0x04003903 RID: 14595
	[PublicizedFrom(EAccessModifier.Protected)]
	public int itemCount;

	// Token: 0x04003904 RID: 14596
	[PublicizedFrom(EAccessModifier.Protected)]
	public string defaultContainer = "";

	// Token: 0x04003905 RID: 14597
	[PublicizedFrom(EAccessModifier.Protected)]
	public string expectedItemClassID = "questItem";

	// Token: 0x04003906 RID: 14598
	[PublicizedFrom(EAccessModifier.Protected)]
	public string expectedQuestItemID = "";

	// Token: 0x04003907 RID: 14599
	public static string PropExpectedItemClassID = "item_class";

	// Token: 0x04003908 RID: 14600
	public static string PropQuestItemID = "quest_item_ID";

	// Token: 0x04003909 RID: 14601
	public static string PropItemCount = "item_count";

	// Token: 0x0400390A RID: 14602
	public static string PropDefaultContainer = "container";
}

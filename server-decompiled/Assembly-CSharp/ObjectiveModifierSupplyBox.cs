using System;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x0200098C RID: 2444
[Preserve]
public class ObjectiveModifierSupplyBox : BaseObjectiveModifier
{
	// Token: 0x060046C3 RID: 18115 RVA: 0x001C095A File Offset: 0x001BEB5A
	public override void AddHooks()
	{
		QuestEventManager.Current.ContainerOpened += this.Current_ContainerOpened;
		QuestEventManager.Current.ContainerClosed += this.Current_ContainerClosed;
	}

	// Token: 0x060046C4 RID: 18116 RVA: 0x001C0988 File Offset: 0x001BEB88
	public override void RemoveHooks()
	{
		QuestEventManager.Current.ContainerOpened -= this.Current_ContainerOpened;
		QuestEventManager.Current.ContainerClosed -= this.Current_ContainerClosed;
	}

	// Token: 0x060046C5 RID: 18117 RVA: 0x001C09B8 File Offset: 0x001BEBB8
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_ContainerOpened(Vector3i containerLocation, TEFeatureStorage tileEntity, bool firstTimeTouched)
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

	// Token: 0x060046C6 RID: 18118 RVA: 0x001C0A2C File Offset: 0x001BEC2C
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_ContainerClosed(Vector3i containerLocation, TEFeatureStorage tileEntity)
	{
		if (tileEntity.blockValue.Block.GetBlockName() == this.defaultContainer)
		{
			tileEntity.RemoveItem(this.expectedItem);
			tileEntity.SetModified();
		}
	}

	// Token: 0x060046C7 RID: 18119 RVA: 0x001C0A6C File Offset: 0x001BEC6C
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

	// Token: 0x060046C8 RID: 18120 RVA: 0x001C0B28 File Offset: 0x001BED28
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

	// Token: 0x060046C9 RID: 18121 RVA: 0x001C0BD0 File Offset: 0x001BEDD0
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

	// Token: 0x060046CA RID: 18122 RVA: 0x001C0C6C File Offset: 0x001BEE6C
	[PublicizedFrom(EAccessModifier.Protected)]
	public int GetItemCount()
	{
		XUiM_PlayerInventory playerInventory = LocalPlayerUI.GetUIForPlayer(base.OwnerObjective.OwnerQuest.OwnerJournal.OwnerPlayer).xui.PlayerInventory;
		this.expectedItem.Meta = base.OwnerObjective.OwnerQuest.QuestCode;
		return playerInventory.Backpack.GetItemCount(this.expectedItem, -1, base.OwnerObjective.OwnerQuest.QuestCode, true) + playerInventory.Toolbelt.GetItemCount(this.expectedItem, false, -1, base.OwnerObjective.OwnerQuest.QuestCode, true);
	}

	// Token: 0x060046CB RID: 18123 RVA: 0x001C0D01 File Offset: 0x001BEF01
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

	// Token: 0x0400391C RID: 14620
	[PublicizedFrom(EAccessModifier.Protected)]
	public ItemValue expectedItem = ItemValue.None;

	// Token: 0x0400391D RID: 14621
	[PublicizedFrom(EAccessModifier.Protected)]
	public ItemClass expectedItemClass;

	// Token: 0x0400391E RID: 14622
	[PublicizedFrom(EAccessModifier.Protected)]
	public int currentCount;

	// Token: 0x0400391F RID: 14623
	[PublicizedFrom(EAccessModifier.Protected)]
	public int itemCount;

	// Token: 0x04003920 RID: 14624
	[PublicizedFrom(EAccessModifier.Protected)]
	public string defaultContainer = "";

	// Token: 0x04003921 RID: 14625
	[PublicizedFrom(EAccessModifier.Protected)]
	public string expectedItemClassID = "questItem";

	// Token: 0x04003922 RID: 14626
	[PublicizedFrom(EAccessModifier.Protected)]
	public string expectedQuestItemID = "";

	// Token: 0x04003923 RID: 14627
	public static string PropExpectedItemClassID = "item_class";

	// Token: 0x04003924 RID: 14628
	public static string PropQuestItemID = "quest_item_ID";

	// Token: 0x04003925 RID: 14629
	public static string PropItemCount = "item_count";

	// Token: 0x04003926 RID: 14630
	public static string PropDefaultContainer = "container";
}

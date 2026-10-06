using System;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x02000982 RID: 2434
[Preserve]
public class ObjectiveModifierSupplyBox : BaseObjectiveModifier
{
	// Token: 0x060046A0 RID: 18080 RVA: 0x001BF8D6 File Offset: 0x001BDAD6
	public override void AddHooks()
	{
		QuestEventManager.Current.ContainerOpened += this.Current_ContainerOpened;
		QuestEventManager.Current.ContainerClosed += this.Current_ContainerClosed;
	}

	// Token: 0x060046A1 RID: 18081 RVA: 0x001BF904 File Offset: 0x001BDB04
	public override void RemoveHooks()
	{
		QuestEventManager.Current.ContainerOpened -= this.Current_ContainerOpened;
		QuestEventManager.Current.ContainerClosed -= this.Current_ContainerClosed;
	}

	// Token: 0x060046A2 RID: 18082 RVA: 0x001BF934 File Offset: 0x001BDB34
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

	// Token: 0x060046A3 RID: 18083 RVA: 0x001BF9A8 File Offset: 0x001BDBA8
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_ContainerClosed(Vector3i containerLocation, TEFeatureStorage tileEntity)
	{
		if (tileEntity.blockValue.Block.GetBlockName() == this.defaultContainer)
		{
			tileEntity.RemoveItem(this.expectedItem);
			tileEntity.SetModified();
		}
	}

	// Token: 0x060046A4 RID: 18084 RVA: 0x001BF9E8 File Offset: 0x001BDBE8
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

	// Token: 0x060046A5 RID: 18085 RVA: 0x001BFAA4 File Offset: 0x001BDCA4
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

	// Token: 0x060046A6 RID: 18086 RVA: 0x001BFB4C File Offset: 0x001BDD4C
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

	// Token: 0x060046A7 RID: 18087 RVA: 0x001BFBE8 File Offset: 0x001BDDE8
	[PublicizedFrom(EAccessModifier.Protected)]
	public int GetItemCount()
	{
		XUiM_PlayerInventory playerInventory = LocalPlayerUI.GetUIForPlayer(base.OwnerObjective.OwnerQuest.OwnerJournal.OwnerPlayer).xui.PlayerInventory;
		this.expectedItem.Meta = base.OwnerObjective.OwnerQuest.QuestCode;
		return playerInventory.Backpack.GetItemCount(this.expectedItem, -1, base.OwnerObjective.OwnerQuest.QuestCode, true) + playerInventory.Toolbelt.GetItemCount(this.expectedItem, false, -1, base.OwnerObjective.OwnerQuest.QuestCode, true);
	}

	// Token: 0x060046A8 RID: 18088 RVA: 0x001BFC7D File Offset: 0x001BDE7D
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

	// Token: 0x040038FC RID: 14588
	[PublicizedFrom(EAccessModifier.Protected)]
	public ItemValue expectedItem = ItemValue.None;

	// Token: 0x040038FD RID: 14589
	[PublicizedFrom(EAccessModifier.Protected)]
	public ItemClass expectedItemClass;

	// Token: 0x040038FE RID: 14590
	[PublicizedFrom(EAccessModifier.Protected)]
	public int currentCount;

	// Token: 0x040038FF RID: 14591
	[PublicizedFrom(EAccessModifier.Protected)]
	public int itemCount;

	// Token: 0x04003900 RID: 14592
	[PublicizedFrom(EAccessModifier.Protected)]
	public string defaultContainer = "";

	// Token: 0x04003901 RID: 14593
	[PublicizedFrom(EAccessModifier.Protected)]
	public string expectedItemClassID = "questItem";

	// Token: 0x04003902 RID: 14594
	[PublicizedFrom(EAccessModifier.Protected)]
	public string expectedQuestItemID = "";

	// Token: 0x04003903 RID: 14595
	public static string PropExpectedItemClassID = "item_class";

	// Token: 0x04003904 RID: 14596
	public static string PropQuestItemID = "quest_item_ID";

	// Token: 0x04003905 RID: 14597
	public static string PropItemCount = "item_count";

	// Token: 0x04003906 RID: 14598
	public static string PropDefaultContainer = "container";
}

using System;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x0200096B RID: 2411
[Preserve]
public class ObjectiveModifierSupplyBox : BaseObjectiveModifier
{
	// Token: 0x06004679 RID: 18041 RVA: 0x001BCBDA File Offset: 0x001BADDA
	public override void AddHooks()
	{
		QuestEventManager.Current.ContainerOpened += this.Current_ContainerOpened;
		QuestEventManager.Current.ContainerClosed += this.Current_ContainerClosed;
	}

	// Token: 0x0600467A RID: 18042 RVA: 0x001BCC08 File Offset: 0x001BAE08
	public override void RemoveHooks()
	{
		QuestEventManager.Current.ContainerOpened -= this.Current_ContainerOpened;
		QuestEventManager.Current.ContainerClosed -= this.Current_ContainerClosed;
	}

	// Token: 0x0600467B RID: 18043 RVA: 0x001BCC38 File Offset: 0x001BAE38
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

	// Token: 0x0600467C RID: 18044 RVA: 0x001BCCAC File Offset: 0x001BAEAC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_ContainerClosed(Vector3i containerLocation, ITileEntityLootable tileEntity)
	{
		if (tileEntity.blockValue.Block.GetBlockName() == this.defaultContainer)
		{
			tileEntity.RemoveItem(this.expectedItem);
			tileEntity.SetModified();
		}
	}

	// Token: 0x0600467D RID: 18045 RVA: 0x001BCCEC File Offset: 0x001BAEEC
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

	// Token: 0x0600467E RID: 18046 RVA: 0x001BCDA8 File Offset: 0x001BAFA8
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

	// Token: 0x0600467F RID: 18047 RVA: 0x001BCE50 File Offset: 0x001BB050
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

	// Token: 0x06004680 RID: 18048 RVA: 0x001BCEEC File Offset: 0x001BB0EC
	[PublicizedFrom(EAccessModifier.Protected)]
	public int GetItemCount()
	{
		XUiM_PlayerInventory playerInventory = LocalPlayerUI.GetUIForPlayer(base.OwnerObjective.OwnerQuest.OwnerJournal.OwnerPlayer).xui.PlayerInventory;
		this.expectedItem.Meta = base.OwnerObjective.OwnerQuest.QuestCode;
		return playerInventory.Backpack.GetItemCount(this.expectedItem, -1, base.OwnerObjective.OwnerQuest.QuestCode, true) + playerInventory.Toolbelt.GetItemCount(this.expectedItem, false, -1, base.OwnerObjective.OwnerQuest.QuestCode, true);
	}

	// Token: 0x06004681 RID: 18049 RVA: 0x001BCF81 File Offset: 0x001BB181
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

	// Token: 0x0400386F RID: 14447
	[PublicizedFrom(EAccessModifier.Protected)]
	public ItemValue expectedItem = ItemValue.None;

	// Token: 0x04003870 RID: 14448
	[PublicizedFrom(EAccessModifier.Protected)]
	public ItemClass expectedItemClass;

	// Token: 0x04003871 RID: 14449
	[PublicizedFrom(EAccessModifier.Protected)]
	public int currentCount;

	// Token: 0x04003872 RID: 14450
	[PublicizedFrom(EAccessModifier.Protected)]
	public int itemCount;

	// Token: 0x04003873 RID: 14451
	[PublicizedFrom(EAccessModifier.Protected)]
	public string defaultContainer = "";

	// Token: 0x04003874 RID: 14452
	[PublicizedFrom(EAccessModifier.Protected)]
	public string expectedItemClassID = "questItem";

	// Token: 0x04003875 RID: 14453
	[PublicizedFrom(EAccessModifier.Protected)]
	public string expectedQuestItemID = "";

	// Token: 0x04003876 RID: 14454
	public static string PropExpectedItemClassID = "item_class";

	// Token: 0x04003877 RID: 14455
	public static string PropQuestItemID = "quest_item_ID";

	// Token: 0x04003878 RID: 14456
	public static string PropItemCount = "item_count";

	// Token: 0x04003879 RID: 14457
	public static string PropDefaultContainer = "container";
}

using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x02000996 RID: 2454
[Preserve]
public class ObjectiveBaseFetchContainer : BaseObjective
{
	// Token: 0x170007B3 RID: 1971
	// (get) Token: 0x0600472D RID: 18221 RVA: 0x001C1AE8 File Offset: 0x001BFCE8
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			if (base.CurrentValue != 3)
			{
				return BaseObjective.ObjectiveValueTypes.Number;
			}
			return BaseObjective.ObjectiveValueTypes.Boolean;
		}
	}

	// Token: 0x0600472E RID: 18222 RVA: 0x001C1AF8 File Offset: 0x001BFCF8
	[PublicizedFrom(EAccessModifier.Protected)]
	public void RemoveFetchItems()
	{
		if (base.CurrentValue != 3)
		{
			return;
		}
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
			ItemStack itemStack = new ItemStack(this.expectedItem.Clone(), num - num2);
			xui.CollectedItemList.AddRemoveItemQueueEntry(itemStack);
		}
	}

	// Token: 0x0600472F RID: 18223 RVA: 0x001C1B99 File Offset: 0x001BFD99
	public override void SetupQuestTag()
	{
		base.OwnerQuest.AddQuestTag(QuestEventManager.fetchTag);
	}

	// Token: 0x06004730 RID: 18224 RVA: 0x001C1BAB File Offset: 0x001BFDAB
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveFetchContainer_keyword", false, null);
		if (this.expectedItemClass == null)
		{
			this.SetupExpectedItem();
		}
		this.SetupQuestTag();
	}

	// Token: 0x06004731 RID: 18225 RVA: 0x001C1BD4 File Offset: 0x001BFDD4
	[PublicizedFrom(EAccessModifier.Protected)]
	public void SetupExpectedItem()
	{
		if (base.OwnerQuest.QuestCode == 0)
		{
			base.OwnerQuest.SetupQuestCode();
		}
		this.expectedItemClass = ItemClass.GetItemClass(this.questItemClassID, false);
		this.expectedItem = new ItemValue(this.expectedItemClass.Id, false);
		if (this.expectedItemClass is ItemClassQuest)
		{
			ushort num = StringParsers.ParseUInt16(this.ID, 0, -1, NumberStyles.Integer);
			this.expectedItemClass = ItemClassQuest.GetItemQuestById(num);
			this.expectedItem.Seed = num;
		}
		this.expectedItem.Meta = base.OwnerQuest.QuestCode;
	}

	// Token: 0x06004732 RID: 18226 RVA: 0x001C1C6C File Offset: 0x001BFE6C
	public override void HandleCompleted()
	{
		base.HandleCompleted();
		this.RemoveFetchItems();
	}

	// Token: 0x06004733 RID: 18227 RVA: 0x001C1C7A File Offset: 0x001BFE7A
	public override void HandleFailed()
	{
		base.HandleFailed();
		this.RemoveFetchItems();
	}

	// Token: 0x06004734 RID: 18228 RVA: 0x001C1C88 File Offset: 0x001BFE88
	public override void ResetObjective()
	{
		this.RemoveFetchItems();
	}

	// Token: 0x06004735 RID: 18229 RVA: 0x001C1C90 File Offset: 0x001BFE90
	[PublicizedFrom(EAccessModifier.Protected)]
	public int GetItemCount(int _expectedMeta = -2)
	{
		XUiM_PlayerInventory playerInventory = LocalPlayerUI.GetUIForPlayer(base.OwnerQuest.OwnerJournal.OwnerPlayer).xui.PlayerInventory;
		if (_expectedMeta == -2)
		{
			_expectedMeta = base.OwnerQuest.QuestCode;
		}
		this.expectedItem.Meta = _expectedMeta;
		return playerInventory.Backpack.GetItemCount(this.expectedItem, -1, _expectedMeta, true) + playerInventory.Toolbelt.GetItemCount(this.expectedItem, false, -1, _expectedMeta, true);
	}

	// Token: 0x06004736 RID: 18230 RVA: 0x001C1D05 File Offset: 0x001BFF05
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CopyValues(BaseObjective objective)
	{
		base.CopyValues(objective);
		((ObjectiveBaseFetchContainer)objective).questItemClassID = this.questItemClassID;
	}

	// Token: 0x06004737 RID: 18231 RVA: 0x001C1D20 File Offset: 0x001BFF20
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		if (properties.Values.ContainsKey(ObjectiveBaseFetchContainer.PropQuestItemClass))
		{
			this.questItemClassID = properties.Values[ObjectiveBaseFetchContainer.PropQuestItemClass];
		}
		if (properties.Values.ContainsKey(ObjectiveBaseFetchContainer.PropQuestItemID))
		{
			this.ID = properties.Values[ObjectiveBaseFetchContainer.PropQuestItemID];
		}
		if (properties.Values.ContainsKey(ObjectiveBaseFetchContainer.PropItemCount))
		{
			this.Value = properties.Values[ObjectiveBaseFetchContainer.PropItemCount];
		}
		if (properties.Values.ContainsKey(ObjectiveBaseFetchContainer.PropDefaultContainer))
		{
			this.defaultContainer = properties.Values[ObjectiveBaseFetchContainer.PropDefaultContainer];
		}
	}

	// Token: 0x06004738 RID: 18232 RVA: 0x001C1DD4 File Offset: 0x001BFFD4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
		parameters.Add("expected_item_id", this.expectedItem.ItemClass.Name);
	}

	// Token: 0x04003974 RID: 14708
	[PublicizedFrom(EAccessModifier.Protected)]
	public ItemValue expectedItem = ItemValue.None;

	// Token: 0x04003975 RID: 14709
	[PublicizedFrom(EAccessModifier.Protected)]
	public ItemClass expectedItemClass;

	// Token: 0x04003976 RID: 14710
	[PublicizedFrom(EAccessModifier.Protected)]
	public float distance;

	// Token: 0x04003977 RID: 14711
	[PublicizedFrom(EAccessModifier.Protected)]
	public int currentCount;

	// Token: 0x04003978 RID: 14712
	[PublicizedFrom(EAccessModifier.Protected)]
	public string defaultContainer = "";

	// Token: 0x04003979 RID: 14713
	public string questItemClassID = "questItem";

	// Token: 0x0400397A RID: 14714
	public static string PropQuestItemClass = "quest_item";

	// Token: 0x0400397B RID: 14715
	public static string PropQuestItemID = "quest_item_ID";

	// Token: 0x0400397C RID: 14716
	public static string PropItemCount = "item_count";

	// Token: 0x0400397D RID: 14717
	public static string PropDefaultContainer = "default_container";

	// Token: 0x0400397E RID: 14718
	[PublicizedFrom(EAccessModifier.Protected)]
	public World world;
}

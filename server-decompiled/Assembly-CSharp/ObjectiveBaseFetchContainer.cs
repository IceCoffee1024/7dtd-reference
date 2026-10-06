using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x0200098C RID: 2444
[Preserve]
public class ObjectiveBaseFetchContainer : BaseObjective
{
	// Token: 0x170007AD RID: 1965
	// (get) Token: 0x0600470A RID: 18186 RVA: 0x001C0A64 File Offset: 0x001BEC64
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

	// Token: 0x0600470B RID: 18187 RVA: 0x001C0A74 File Offset: 0x001BEC74
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

	// Token: 0x0600470C RID: 18188 RVA: 0x001C0B15 File Offset: 0x001BED15
	public override void SetupQuestTag()
	{
		base.OwnerQuest.AddQuestTag(QuestEventManager.fetchTag);
	}

	// Token: 0x0600470D RID: 18189 RVA: 0x001C0B27 File Offset: 0x001BED27
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveFetchContainer_keyword", false, null);
		if (this.expectedItemClass == null)
		{
			this.SetupExpectedItem();
		}
		this.SetupQuestTag();
	}

	// Token: 0x0600470E RID: 18190 RVA: 0x001C0B50 File Offset: 0x001BED50
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

	// Token: 0x0600470F RID: 18191 RVA: 0x001C0BE8 File Offset: 0x001BEDE8
	public override void HandleCompleted()
	{
		base.HandleCompleted();
		this.RemoveFetchItems();
	}

	// Token: 0x06004710 RID: 18192 RVA: 0x001C0BF6 File Offset: 0x001BEDF6
	public override void HandleFailed()
	{
		base.HandleFailed();
		this.RemoveFetchItems();
	}

	// Token: 0x06004711 RID: 18193 RVA: 0x001C0C04 File Offset: 0x001BEE04
	public override void ResetObjective()
	{
		this.RemoveFetchItems();
	}

	// Token: 0x06004712 RID: 18194 RVA: 0x001C0C0C File Offset: 0x001BEE0C
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

	// Token: 0x06004713 RID: 18195 RVA: 0x001C0C81 File Offset: 0x001BEE81
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CopyValues(BaseObjective objective)
	{
		base.CopyValues(objective);
		((ObjectiveBaseFetchContainer)objective).questItemClassID = this.questItemClassID;
	}

	// Token: 0x06004714 RID: 18196 RVA: 0x001C0C9C File Offset: 0x001BEE9C
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

	// Token: 0x06004715 RID: 18197 RVA: 0x001C0D50 File Offset: 0x001BEF50
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
		parameters.Add("expected_item_id", this.expectedItem.ItemClass.Name);
	}

	// Token: 0x04003954 RID: 14676
	[PublicizedFrom(EAccessModifier.Protected)]
	public ItemValue expectedItem = ItemValue.None;

	// Token: 0x04003955 RID: 14677
	[PublicizedFrom(EAccessModifier.Protected)]
	public ItemClass expectedItemClass;

	// Token: 0x04003956 RID: 14678
	[PublicizedFrom(EAccessModifier.Protected)]
	public float distance;

	// Token: 0x04003957 RID: 14679
	[PublicizedFrom(EAccessModifier.Protected)]
	public int currentCount;

	// Token: 0x04003958 RID: 14680
	[PublicizedFrom(EAccessModifier.Protected)]
	public string defaultContainer = "";

	// Token: 0x04003959 RID: 14681
	public string questItemClassID = "questItem";

	// Token: 0x0400395A RID: 14682
	public static string PropQuestItemClass = "quest_item";

	// Token: 0x0400395B RID: 14683
	public static string PropQuestItemID = "quest_item_ID";

	// Token: 0x0400395C RID: 14684
	public static string PropItemCount = "item_count";

	// Token: 0x0400395D RID: 14685
	public static string PropDefaultContainer = "default_container";

	// Token: 0x0400395E RID: 14686
	[PublicizedFrom(EAccessModifier.Protected)]
	public World world;
}

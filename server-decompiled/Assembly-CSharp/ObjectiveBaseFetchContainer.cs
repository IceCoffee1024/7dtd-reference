using System;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x02000986 RID: 2438
[Preserve]
public class ObjectiveBaseFetchContainer : BaseObjective
{
	// Token: 0x1700078D RID: 1933
	// (get) Token: 0x06004766 RID: 18278 RVA: 0x001C04BE File Offset: 0x001BE6BE
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

	// Token: 0x06004767 RID: 18279 RVA: 0x001C04CC File Offset: 0x001BE6CC
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

	// Token: 0x06004768 RID: 18280 RVA: 0x001C056D File Offset: 0x001BE76D
	public override void SetupQuestTag()
	{
		base.OwnerQuest.AddQuestTag(QuestEventManager.fetchTag);
	}

	// Token: 0x06004769 RID: 18281 RVA: 0x001C057F File Offset: 0x001BE77F
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveFetchContainer_keyword", false, null);
		if (this.expectedItemClass == null)
		{
			this.SetupExpectedItem();
		}
		this.SetupQuestTag();
	}

	// Token: 0x0600476A RID: 18282 RVA: 0x001C05A8 File Offset: 0x001BE7A8
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

	// Token: 0x0600476B RID: 18283 RVA: 0x001C0640 File Offset: 0x001BE840
	public override void HandleCompleted()
	{
		base.HandleCompleted();
		this.RemoveFetchItems();
	}

	// Token: 0x0600476C RID: 18284 RVA: 0x001C064E File Offset: 0x001BE84E
	public override void HandleFailed()
	{
		base.HandleFailed();
		this.RemoveFetchItems();
	}

	// Token: 0x0600476D RID: 18285 RVA: 0x001C065C File Offset: 0x001BE85C
	public override void ResetObjective()
	{
		this.RemoveFetchItems();
	}

	// Token: 0x0600476E RID: 18286 RVA: 0x001C0664 File Offset: 0x001BE864
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

	// Token: 0x0600476F RID: 18287 RVA: 0x001C06D9 File Offset: 0x001BE8D9
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CopyValues(BaseObjective objective)
	{
		base.CopyValues(objective);
		((ObjectiveBaseFetchContainer)objective).questItemClassID = this.questItemClassID;
	}

	// Token: 0x06004770 RID: 18288 RVA: 0x001C06F4 File Offset: 0x001BE8F4
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

	// Token: 0x04003958 RID: 14680
	[PublicizedFrom(EAccessModifier.Protected)]
	public ItemValue expectedItem = ItemValue.None;

	// Token: 0x04003959 RID: 14681
	[PublicizedFrom(EAccessModifier.Protected)]
	public ItemClass expectedItemClass;

	// Token: 0x0400395A RID: 14682
	[PublicizedFrom(EAccessModifier.Protected)]
	public float distance;

	// Token: 0x0400395B RID: 14683
	[PublicizedFrom(EAccessModifier.Protected)]
	public int currentCount;

	// Token: 0x0400395C RID: 14684
	[PublicizedFrom(EAccessModifier.Protected)]
	public string defaultContainer = "";

	// Token: 0x0400395D RID: 14685
	public string questItemClassID = "questItem";

	// Token: 0x0400395E RID: 14686
	public static string PropQuestItemClass = "quest_item";

	// Token: 0x0400395F RID: 14687
	public static string PropQuestItemID = "quest_item_ID";

	// Token: 0x04003960 RID: 14688
	public static string PropItemCount = "item_count";

	// Token: 0x04003961 RID: 14689
	public static string PropDefaultContainer = "default_container";

	// Token: 0x04003962 RID: 14690
	[PublicizedFrom(EAccessModifier.Protected)]
	public World world;
}

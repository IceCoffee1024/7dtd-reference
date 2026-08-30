using System;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x02000975 RID: 2421
[Preserve]
public class ObjectiveBaseFetchContainer : BaseObjective
{
	// Token: 0x1700077B RID: 1915
	// (get) Token: 0x060046E0 RID: 18144 RVA: 0x001BDCCA File Offset: 0x001BBECA
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

	// Token: 0x060046E1 RID: 18145 RVA: 0x001BDCD8 File Offset: 0x001BBED8
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

	// Token: 0x060046E2 RID: 18146 RVA: 0x001BDD79 File Offset: 0x001BBF79
	public override void SetupQuestTag()
	{
		base.OwnerQuest.AddQuestTag(QuestEventManager.fetchTag);
	}

	// Token: 0x060046E3 RID: 18147 RVA: 0x001BDD8B File Offset: 0x001BBF8B
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveFetchContainer_keyword", false, null);
		if (this.expectedItemClass == null)
		{
			this.SetupExpectedItem();
		}
		this.SetupQuestTag();
	}

	// Token: 0x060046E4 RID: 18148 RVA: 0x001BDDB4 File Offset: 0x001BBFB4
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

	// Token: 0x060046E5 RID: 18149 RVA: 0x001BDE4C File Offset: 0x001BC04C
	public override void HandleCompleted()
	{
		base.HandleCompleted();
		this.RemoveFetchItems();
	}

	// Token: 0x060046E6 RID: 18150 RVA: 0x001BDE5A File Offset: 0x001BC05A
	public override void HandleFailed()
	{
		base.HandleFailed();
		this.RemoveFetchItems();
	}

	// Token: 0x060046E7 RID: 18151 RVA: 0x001BDE68 File Offset: 0x001BC068
	public override void ResetObjective()
	{
		this.RemoveFetchItems();
	}

	// Token: 0x060046E8 RID: 18152 RVA: 0x001BDE70 File Offset: 0x001BC070
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

	// Token: 0x060046E9 RID: 18153 RVA: 0x001BDEE5 File Offset: 0x001BC0E5
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CopyValues(BaseObjective objective)
	{
		base.CopyValues(objective);
		((ObjectiveBaseFetchContainer)objective).questItemClassID = this.questItemClassID;
	}

	// Token: 0x060046EA RID: 18154 RVA: 0x001BDF00 File Offset: 0x001BC100
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

	// Token: 0x040038C7 RID: 14535
	[PublicizedFrom(EAccessModifier.Protected)]
	public ItemValue expectedItem = ItemValue.None;

	// Token: 0x040038C8 RID: 14536
	[PublicizedFrom(EAccessModifier.Protected)]
	public ItemClass expectedItemClass;

	// Token: 0x040038C9 RID: 14537
	[PublicizedFrom(EAccessModifier.Protected)]
	public float distance;

	// Token: 0x040038CA RID: 14538
	[PublicizedFrom(EAccessModifier.Protected)]
	public int currentCount;

	// Token: 0x040038CB RID: 14539
	[PublicizedFrom(EAccessModifier.Protected)]
	public string defaultContainer = "";

	// Token: 0x040038CC RID: 14540
	public string questItemClassID = "questItem";

	// Token: 0x040038CD RID: 14541
	public static string PropQuestItemClass = "quest_item";

	// Token: 0x040038CE RID: 14542
	public static string PropQuestItemID = "quest_item_ID";

	// Token: 0x040038CF RID: 14543
	public static string PropItemCount = "item_count";

	// Token: 0x040038D0 RID: 14544
	public static string PropDefaultContainer = "default_container";

	// Token: 0x040038D1 RID: 14545
	[PublicizedFrom(EAccessModifier.Protected)]
	public World world;
}

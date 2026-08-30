using System;
using UnityEngine.Scripting;

// Token: 0x0200098F RID: 2447
[Preserve]
public class ObjectiveCraft : BaseObjective
{
	// Token: 0x17000795 RID: 1941
	// (get) Token: 0x060047BC RID: 18364 RVA: 0x00020021 File Offset: 0x0001E221
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x060047BD RID: 18365 RVA: 0x001C18E3 File Offset: 0x001BFAE3
	public override void SetupQuestTag()
	{
		base.OwnerQuest.AddQuestTag(QuestEventManager.craftingTag);
	}

	// Token: 0x060047BE RID: 18366 RVA: 0x001C18F8 File Offset: 0x001BFAF8
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveCraft_keyword", false, null);
		this.expectedItem = ItemClass.GetItem(this.ID, false);
		this.expectedItemClass = ItemClass.GetItemClass(this.ID, false);
		this.itemCount = Convert.ToInt32(this.Value);
	}

	// Token: 0x060047BF RID: 18367 RVA: 0x001C194C File Offset: 0x001BFB4C
	public override void SetupDisplay()
	{
		string text = ((this.ID != "" && this.ID != null) ? Localization.Get(this.ID, false, null) : "Any Item");
		base.Description = string.Format(this.keyword, text);
		this.StatusText = string.Format("{0}/{1}", base.CurrentValue, this.itemCount);
	}

	// Token: 0x060047C0 RID: 18368 RVA: 0x001C19C0 File Offset: 0x001BFBC0
	public override void AddHooks()
	{
		QuestEventManager.Current.CraftItem -= this.Current_CraftItem;
		QuestEventManager.Current.CraftItem += this.Current_CraftItem;
	}

	// Token: 0x060047C1 RID: 18369 RVA: 0x001C19EE File Offset: 0x001BFBEE
	public override void RemoveHooks()
	{
		QuestEventManager.Current.CraftItem -= this.Current_CraftItem;
	}

	// Token: 0x060047C2 RID: 18370 RVA: 0x001C1A08 File Offset: 0x001BFC08
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_CraftItem(ItemStack stack)
	{
		if (base.Complete)
		{
			return;
		}
		if (stack.itemValue.type == this.expectedItem.type && base.OwnerQuest.CheckRequirements())
		{
			base.CurrentValue += (byte)stack.count;
			this.Refresh();
		}
	}

	// Token: 0x060047C3 RID: 18371 RVA: 0x001C1A5E File Offset: 0x001BFC5E
	public override void Refresh()
	{
		if (base.Complete)
		{
			return;
		}
		base.Complete = (int)base.CurrentValue >= this.itemCount;
		if (base.Complete)
		{
			base.OwnerQuest.RefreshQuestCompletion(QuestClass.CompletionTypes.AutoComplete, null, true, null);
		}
	}

	// Token: 0x060047C4 RID: 18372 RVA: 0x001C1A98 File Offset: 0x001BFC98
	public override BaseObjective Clone()
	{
		ObjectiveCraft objectiveCraft = new ObjectiveCraft();
		this.CopyValues(objectiveCraft);
		return objectiveCraft;
	}

	// Token: 0x060047C5 RID: 18373 RVA: 0x001C1AB4 File Offset: 0x001BFCB4
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		if (properties.Values.ContainsKey(ObjectiveCraft.PropItem))
		{
			this.ID = properties.Values[ObjectiveCraft.PropItem];
		}
		if (properties.Values.ContainsKey(ObjectiveCraft.PropCount))
		{
			this.Value = properties.Values[ObjectiveCraft.PropCount];
		}
	}

	// Token: 0x04003976 RID: 14710
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue expectedItem = ItemValue.None;

	// Token: 0x04003977 RID: 14711
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass expectedItemClass;

	// Token: 0x04003978 RID: 14712
	[PublicizedFrom(EAccessModifier.Private)]
	public int itemCount;

	// Token: 0x04003979 RID: 14713
	public static string PropItem = "item";

	// Token: 0x0400397A RID: 14714
	public static string PropCount = "count";
}

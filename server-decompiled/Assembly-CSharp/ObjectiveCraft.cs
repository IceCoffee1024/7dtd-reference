using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000995 RID: 2453
[Preserve]
public class ObjectiveCraft : BaseObjective
{
	// Token: 0x170007B5 RID: 1973
	// (get) Token: 0x06004767 RID: 18279 RVA: 0x00020119 File Offset: 0x0001E319
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x06004768 RID: 18280 RVA: 0x001C1F77 File Offset: 0x001C0177
	public override void SetupQuestTag()
	{
		base.OwnerQuest.AddQuestTag(QuestEventManager.craftingTag);
	}

	// Token: 0x06004769 RID: 18281 RVA: 0x001C1F8C File Offset: 0x001C018C
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveCraft_keyword", false, null);
		this.expectedItem = ItemClass.GetItem(this.ID, false);
		this.expectedItemClass = ItemClass.GetItemClass(this.ID, false);
		this.itemCount = Convert.ToInt32(this.Value);
	}

	// Token: 0x0600476A RID: 18282 RVA: 0x001C1FE0 File Offset: 0x001C01E0
	public override void SetupDisplay()
	{
		string text = ((this.ID != "" && this.ID != null) ? Localization.Get(this.ID, false, null) : "Any Item");
		base.Description = string.Format(this.keyword, text);
		this.StatusText = string.Format("{0}/{1}", base.CurrentValue, this.itemCount);
	}

	// Token: 0x0600476B RID: 18283 RVA: 0x001C2054 File Offset: 0x001C0254
	public override void AddHooks()
	{
		QuestEventManager.Current.CraftItem -= this.Current_CraftItem;
		QuestEventManager.Current.CraftItem += this.Current_CraftItem;
	}

	// Token: 0x0600476C RID: 18284 RVA: 0x001C2082 File Offset: 0x001C0282
	public override void RemoveHooks()
	{
		QuestEventManager.Current.CraftItem -= this.Current_CraftItem;
	}

	// Token: 0x0600476D RID: 18285 RVA: 0x001C209C File Offset: 0x001C029C
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

	// Token: 0x0600476E RID: 18286 RVA: 0x001C20F2 File Offset: 0x001C02F2
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

	// Token: 0x0600476F RID: 18287 RVA: 0x001C212C File Offset: 0x001C032C
	public override BaseObjective Clone()
	{
		ObjectiveCraft objectiveCraft = new ObjectiveCraft();
		this.CopyValues(objectiveCraft);
		return objectiveCraft;
	}

	// Token: 0x06004770 RID: 18288 RVA: 0x001C2148 File Offset: 0x001C0348
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

	// Token: 0x06004771 RID: 18289 RVA: 0x001C21AC File Offset: 0x001C03AC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
		parameters.Add("expected_item_id", this.expectedItem.ItemClass.Name);
		parameters.Add("count", this.itemCount);
	}

	// Token: 0x04003972 RID: 14706
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue expectedItem = ItemValue.None;

	// Token: 0x04003973 RID: 14707
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass expectedItemClass;

	// Token: 0x04003974 RID: 14708
	[PublicizedFrom(EAccessModifier.Private)]
	public int itemCount;

	// Token: 0x04003975 RID: 14709
	public static string PropItem = "item";

	// Token: 0x04003976 RID: 14710
	public static string PropCount = "count";
}

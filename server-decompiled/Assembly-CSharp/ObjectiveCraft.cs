using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200099F RID: 2463
[Preserve]
public class ObjectiveCraft : BaseObjective
{
	// Token: 0x170007BB RID: 1979
	// (get) Token: 0x0600478A RID: 18314 RVA: 0x00020119 File Offset: 0x0001E319
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x0600478B RID: 18315 RVA: 0x001C2FFB File Offset: 0x001C11FB
	public override void SetupQuestTag()
	{
		base.OwnerQuest.AddQuestTag(QuestEventManager.craftingTag);
	}

	// Token: 0x0600478C RID: 18316 RVA: 0x001C3010 File Offset: 0x001C1210
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveCraft_keyword", false, null);
		this.expectedItem = ItemClass.GetItem(this.ID, false);
		this.expectedItemClass = ItemClass.GetItemClass(this.ID, false);
		this.itemCount = Convert.ToInt32(this.Value);
	}

	// Token: 0x0600478D RID: 18317 RVA: 0x001C3064 File Offset: 0x001C1264
	public override void SetupDisplay()
	{
		string text = ((this.ID != "" && this.ID != null) ? Localization.Get(this.ID, false, null) : "Any Item");
		base.Description = string.Format(this.keyword, text);
		this.StatusText = string.Format("{0}/{1}", base.CurrentValue, this.itemCount);
	}

	// Token: 0x0600478E RID: 18318 RVA: 0x001C30D8 File Offset: 0x001C12D8
	public override void AddHooks()
	{
		QuestEventManager.Current.CraftItem -= this.Current_CraftItem;
		QuestEventManager.Current.CraftItem += this.Current_CraftItem;
	}

	// Token: 0x0600478F RID: 18319 RVA: 0x001C3106 File Offset: 0x001C1306
	public override void RemoveHooks()
	{
		QuestEventManager.Current.CraftItem -= this.Current_CraftItem;
	}

	// Token: 0x06004790 RID: 18320 RVA: 0x001C3120 File Offset: 0x001C1320
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

	// Token: 0x06004791 RID: 18321 RVA: 0x001C3176 File Offset: 0x001C1376
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

	// Token: 0x06004792 RID: 18322 RVA: 0x001C31B0 File Offset: 0x001C13B0
	public override BaseObjective Clone()
	{
		ObjectiveCraft objectiveCraft = new ObjectiveCraft();
		this.CopyValues(objectiveCraft);
		return objectiveCraft;
	}

	// Token: 0x06004793 RID: 18323 RVA: 0x001C31CC File Offset: 0x001C13CC
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

	// Token: 0x06004794 RID: 18324 RVA: 0x001C3230 File Offset: 0x001C1430
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
		parameters.Add("expected_item_id", this.expectedItem.ItemClass.Name);
		parameters.Add("count", this.itemCount);
	}

	// Token: 0x04003992 RID: 14738
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue expectedItem = ItemValue.None;

	// Token: 0x04003993 RID: 14739
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass expectedItemClass;

	// Token: 0x04003994 RID: 14740
	[PublicizedFrom(EAccessModifier.Private)]
	public int itemCount;

	// Token: 0x04003995 RID: 14741
	public static string PropItem = "item";

	// Token: 0x04003996 RID: 14742
	public static string PropCount = "count";
}

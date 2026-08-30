using System;
using UnityEngine.Scripting;

// Token: 0x0200097E RID: 2430
[Preserve]
public class ObjectiveCraft : BaseObjective
{
	// Token: 0x17000783 RID: 1923
	// (get) Token: 0x06004736 RID: 18230 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x06004737 RID: 18231 RVA: 0x001BF0EF File Offset: 0x001BD2EF
	public override void SetupQuestTag()
	{
		base.OwnerQuest.AddQuestTag(QuestEventManager.craftingTag);
	}

	// Token: 0x06004738 RID: 18232 RVA: 0x001BF104 File Offset: 0x001BD304
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveCraft_keyword", false, null);
		this.expectedItem = ItemClass.GetItem(this.ID, false);
		this.expectedItemClass = ItemClass.GetItemClass(this.ID, false);
		this.itemCount = Convert.ToInt32(this.Value);
	}

	// Token: 0x06004739 RID: 18233 RVA: 0x001BF158 File Offset: 0x001BD358
	public override void SetupDisplay()
	{
		string text = ((this.ID != "" && this.ID != null) ? Localization.Get(this.ID, false, null) : "Any Item");
		base.Description = string.Format(this.keyword, text);
		this.StatusText = string.Format("{0}/{1}", base.CurrentValue, this.itemCount);
	}

	// Token: 0x0600473A RID: 18234 RVA: 0x001BF1CC File Offset: 0x001BD3CC
	public override void AddHooks()
	{
		QuestEventManager.Current.CraftItem -= this.Current_CraftItem;
		QuestEventManager.Current.CraftItem += this.Current_CraftItem;
	}

	// Token: 0x0600473B RID: 18235 RVA: 0x001BF1FA File Offset: 0x001BD3FA
	public override void RemoveHooks()
	{
		QuestEventManager.Current.CraftItem -= this.Current_CraftItem;
	}

	// Token: 0x0600473C RID: 18236 RVA: 0x001BF214 File Offset: 0x001BD414
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

	// Token: 0x0600473D RID: 18237 RVA: 0x001BF26A File Offset: 0x001BD46A
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

	// Token: 0x0600473E RID: 18238 RVA: 0x001BF2A4 File Offset: 0x001BD4A4
	public override BaseObjective Clone()
	{
		ObjectiveCraft objectiveCraft = new ObjectiveCraft();
		this.CopyValues(objectiveCraft);
		return objectiveCraft;
	}

	// Token: 0x0600473F RID: 18239 RVA: 0x001BF2C0 File Offset: 0x001BD4C0
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

	// Token: 0x040038E5 RID: 14565
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue expectedItem = ItemValue.None;

	// Token: 0x040038E6 RID: 14566
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass expectedItemClass;

	// Token: 0x040038E7 RID: 14567
	[PublicizedFrom(EAccessModifier.Private)]
	public int itemCount;

	// Token: 0x040038E8 RID: 14568
	public static string PropItem = "item";

	// Token: 0x040038E9 RID: 14569
	public static string PropCount = "count";
}

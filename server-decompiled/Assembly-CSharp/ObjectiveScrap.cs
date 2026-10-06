using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020009B4 RID: 2484
[Preserve]
public class ObjectiveScrap : BaseObjective
{
	// Token: 0x170007E0 RID: 2016
	// (get) Token: 0x060048CB RID: 18635 RVA: 0x00020119 File Offset: 0x0001E319
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x060048CC RID: 18636 RVA: 0x001C9600 File Offset: 0x001C7800
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveScrap_keyword", false, null);
		this.expectedItem = ItemClass.GetItem(this.ID, false);
		this.expectedItemClass = ItemClass.GetItemClass(this.ID, false);
		this.scrapCount = Convert.ToInt32(this.Value);
	}

	// Token: 0x060048CD RID: 18637 RVA: 0x001C9654 File Offset: 0x001C7854
	public override void SetupDisplay()
	{
		base.Description = string.Format(this.keyword, this.expectedItemClass.GetLocalizedItemName());
		this.StatusText = string.Format("{0}/{1}", base.CurrentValue, this.scrapCount);
	}

	// Token: 0x060048CE RID: 18638 RVA: 0x001C96A3 File Offset: 0x001C78A3
	public override void AddHooks()
	{
		QuestEventManager.Current.ScrapItem += this.Current_ScrapItem;
	}

	// Token: 0x060048CF RID: 18639 RVA: 0x001C96BB File Offset: 0x001C78BB
	public override void RemoveHooks()
	{
		QuestEventManager.Current.ScrapItem -= this.Current_ScrapItem;
	}

	// Token: 0x060048D0 RID: 18640 RVA: 0x001C96D4 File Offset: 0x001C78D4
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_ScrapItem(ItemStack stack)
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

	// Token: 0x060048D1 RID: 18641 RVA: 0x001C972A File Offset: 0x001C792A
	public override void Refresh()
	{
		if (base.Complete)
		{
			return;
		}
		base.Complete = (int)base.CurrentValue >= this.scrapCount;
		if (base.Complete)
		{
			base.OwnerQuest.RefreshQuestCompletion(QuestClass.CompletionTypes.AutoComplete, null, true, null);
		}
	}

	// Token: 0x060048D2 RID: 18642 RVA: 0x001C9764 File Offset: 0x001C7964
	public override BaseObjective Clone()
	{
		ObjectiveScrap objectiveScrap = new ObjectiveScrap();
		this.CopyValues(objectiveScrap);
		return objectiveScrap;
	}

	// Token: 0x060048D3 RID: 18643 RVA: 0x001C977F File Offset: 0x001C797F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
		parameters.Add("expected_item_id", this.expectedItem.ItemClass.Name);
		parameters.Add("count", this.scrapCount);
	}

	// Token: 0x04003A1D RID: 14877
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue expectedItem = ItemValue.None;

	// Token: 0x04003A1E RID: 14878
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass expectedItemClass;

	// Token: 0x04003A1F RID: 14879
	[PublicizedFrom(EAccessModifier.Private)]
	public int scrapCount;
}

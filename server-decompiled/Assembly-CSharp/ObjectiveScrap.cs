using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020009BE RID: 2494
[Preserve]
public class ObjectiveScrap : BaseObjective
{
	// Token: 0x170007E6 RID: 2022
	// (get) Token: 0x060048EE RID: 18670 RVA: 0x00020119 File Offset: 0x0001E319
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x060048EF RID: 18671 RVA: 0x001CA6D4 File Offset: 0x001C88D4
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveScrap_keyword", false, null);
		this.expectedItem = ItemClass.GetItem(this.ID, false);
		this.expectedItemClass = ItemClass.GetItemClass(this.ID, false);
		this.scrapCount = Convert.ToInt32(this.Value);
	}

	// Token: 0x060048F0 RID: 18672 RVA: 0x001CA728 File Offset: 0x001C8928
	public override void SetupDisplay()
	{
		base.Description = string.Format(this.keyword, this.expectedItemClass.GetLocalizedItemName());
		this.StatusText = string.Format("{0}/{1}", base.CurrentValue, this.scrapCount);
	}

	// Token: 0x060048F1 RID: 18673 RVA: 0x001CA777 File Offset: 0x001C8977
	public override void AddHooks()
	{
		QuestEventManager.Current.ScrapItem += this.Current_ScrapItem;
	}

	// Token: 0x060048F2 RID: 18674 RVA: 0x001CA78F File Offset: 0x001C898F
	public override void RemoveHooks()
	{
		QuestEventManager.Current.ScrapItem -= this.Current_ScrapItem;
	}

	// Token: 0x060048F3 RID: 18675 RVA: 0x001CA7A8 File Offset: 0x001C89A8
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

	// Token: 0x060048F4 RID: 18676 RVA: 0x001CA7FE File Offset: 0x001C89FE
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

	// Token: 0x060048F5 RID: 18677 RVA: 0x001CA838 File Offset: 0x001C8A38
	public override BaseObjective Clone()
	{
		ObjectiveScrap objectiveScrap = new ObjectiveScrap();
		this.CopyValues(objectiveScrap);
		return objectiveScrap;
	}

	// Token: 0x060048F6 RID: 18678 RVA: 0x001CA853 File Offset: 0x001C8A53
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
		parameters.Add("expected_item_id", this.expectedItem.ItemClass.Name);
		parameters.Add("count", this.scrapCount);
	}

	// Token: 0x04003A3D RID: 14909
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue expectedItem = ItemValue.None;

	// Token: 0x04003A3E RID: 14910
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass expectedItemClass;

	// Token: 0x04003A3F RID: 14911
	[PublicizedFrom(EAccessModifier.Private)]
	public int scrapCount;
}

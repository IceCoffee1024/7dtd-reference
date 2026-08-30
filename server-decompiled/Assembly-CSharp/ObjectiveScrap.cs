using System;
using UnityEngine.Scripting;

// Token: 0x0200099D RID: 2461
[Preserve]
public class ObjectiveScrap : BaseObjective
{
	// Token: 0x170007AE RID: 1966
	// (get) Token: 0x0600488B RID: 18571 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x0600488C RID: 18572 RVA: 0x001C6444 File Offset: 0x001C4644
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveScrap_keyword", false, null);
		this.expectedItem = ItemClass.GetItem(this.ID, false);
		this.expectedItemClass = ItemClass.GetItemClass(this.ID, false);
		this.scrapCount = Convert.ToInt32(this.Value);
	}

	// Token: 0x0600488D RID: 18573 RVA: 0x001C6498 File Offset: 0x001C4698
	public override void SetupDisplay()
	{
		base.Description = string.Format(this.keyword, this.expectedItemClass.GetLocalizedItemName());
		this.StatusText = string.Format("{0}/{1}", base.CurrentValue, this.scrapCount);
	}

	// Token: 0x0600488E RID: 18574 RVA: 0x001C64E7 File Offset: 0x001C46E7
	public override void AddHooks()
	{
		QuestEventManager.Current.ScrapItem += this.Current_ScrapItem;
	}

	// Token: 0x0600488F RID: 18575 RVA: 0x001C64FF File Offset: 0x001C46FF
	public override void RemoveHooks()
	{
		QuestEventManager.Current.ScrapItem -= this.Current_ScrapItem;
	}

	// Token: 0x06004890 RID: 18576 RVA: 0x001C6518 File Offset: 0x001C4718
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

	// Token: 0x06004891 RID: 18577 RVA: 0x001C656E File Offset: 0x001C476E
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

	// Token: 0x06004892 RID: 18578 RVA: 0x001C65A8 File Offset: 0x001C47A8
	public override BaseObjective Clone()
	{
		ObjectiveScrap objectiveScrap = new ObjectiveScrap();
		this.CopyValues(objectiveScrap);
		return objectiveScrap;
	}

	// Token: 0x0400398F RID: 14735
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue expectedItem = ItemValue.None;

	// Token: 0x04003990 RID: 14736
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass expectedItemClass;

	// Token: 0x04003991 RID: 14737
	[PublicizedFrom(EAccessModifier.Private)]
	public int scrapCount;
}

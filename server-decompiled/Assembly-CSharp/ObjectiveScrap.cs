using System;
using UnityEngine.Scripting;

// Token: 0x020009AE RID: 2478
[Preserve]
public class ObjectiveScrap : BaseObjective
{
	// Token: 0x170007C0 RID: 1984
	// (get) Token: 0x06004911 RID: 18705 RVA: 0x00020021 File Offset: 0x0001E221
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x06004912 RID: 18706 RVA: 0x001C8C38 File Offset: 0x001C6E38
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveScrap_keyword", false, null);
		this.expectedItem = ItemClass.GetItem(this.ID, false);
		this.expectedItemClass = ItemClass.GetItemClass(this.ID, false);
		this.scrapCount = Convert.ToInt32(this.Value);
	}

	// Token: 0x06004913 RID: 18707 RVA: 0x001C8C8C File Offset: 0x001C6E8C
	public override void SetupDisplay()
	{
		base.Description = string.Format(this.keyword, this.expectedItemClass.GetLocalizedItemName());
		this.StatusText = string.Format("{0}/{1}", base.CurrentValue, this.scrapCount);
	}

	// Token: 0x06004914 RID: 18708 RVA: 0x001C8CDB File Offset: 0x001C6EDB
	public override void AddHooks()
	{
		QuestEventManager.Current.ScrapItem += this.Current_ScrapItem;
	}

	// Token: 0x06004915 RID: 18709 RVA: 0x001C8CF3 File Offset: 0x001C6EF3
	public override void RemoveHooks()
	{
		QuestEventManager.Current.ScrapItem -= this.Current_ScrapItem;
	}

	// Token: 0x06004916 RID: 18710 RVA: 0x001C8D0C File Offset: 0x001C6F0C
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

	// Token: 0x06004917 RID: 18711 RVA: 0x001C8D62 File Offset: 0x001C6F62
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

	// Token: 0x06004918 RID: 18712 RVA: 0x001C8D9C File Offset: 0x001C6F9C
	public override BaseObjective Clone()
	{
		ObjectiveScrap objectiveScrap = new ObjectiveScrap();
		this.CopyValues(objectiveScrap);
		return objectiveScrap;
	}

	// Token: 0x04003A20 RID: 14880
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue expectedItem = ItemValue.None;

	// Token: 0x04003A21 RID: 14881
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass expectedItemClass;

	// Token: 0x04003A22 RID: 14882
	[PublicizedFrom(EAccessModifier.Private)]
	public int scrapCount;
}

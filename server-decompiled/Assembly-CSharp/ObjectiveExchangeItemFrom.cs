using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000997 RID: 2455
[Preserve]
public class ObjectiveExchangeItemFrom : BaseObjective
{
	// Token: 0x170007B8 RID: 1976
	// (get) Token: 0x06004782 RID: 18306 RVA: 0x00020119 File Offset: 0x0001E319
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x06004783 RID: 18307 RVA: 0x001C2634 File Offset: 0x001C0834
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveExchangeItemFrom_keyword", false, null);
		this.expectedItem = ItemClass.GetItem(this.ID, false);
		this.expectedItemClass = ItemClass.GetItemClass(this.ID, false);
		this.exchangeCount = Convert.ToInt32(this.Value);
	}

	// Token: 0x06004784 RID: 18308 RVA: 0x001C2688 File Offset: 0x001C0888
	public override void SetupDisplay()
	{
		base.Description = string.Format(this.keyword, this.expectedItemClass.GetLocalizedItemName());
		this.StatusText = string.Format("{0}/{1}", base.CurrentValue, this.exchangeCount);
	}

	// Token: 0x06004785 RID: 18309 RVA: 0x001C26D7 File Offset: 0x001C08D7
	public override void AddHooks()
	{
		QuestEventManager.Current.ExchangeFromItem += this.Current_ExchangeItem;
	}

	// Token: 0x06004786 RID: 18310 RVA: 0x001C26EF File Offset: 0x001C08EF
	public override void RemoveHooks()
	{
		QuestEventManager.Current.ExchangeFromItem -= this.Current_ExchangeItem;
	}

	// Token: 0x06004787 RID: 18311 RVA: 0x001C2708 File Offset: 0x001C0908
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_ExchangeItem(ItemStack itemStack)
	{
		if (base.Complete)
		{
			return;
		}
		if (itemStack.itemValue.type == this.expectedItem.type && base.OwnerQuest.CheckRequirements())
		{
			base.CurrentValue += (byte)itemStack.count;
			this.Refresh();
		}
	}

	// Token: 0x06004788 RID: 18312 RVA: 0x001C275E File Offset: 0x001C095E
	public override void Refresh()
	{
		if (base.Complete)
		{
			return;
		}
		base.Complete = (int)base.CurrentValue >= this.exchangeCount;
		if (base.Complete)
		{
			base.OwnerQuest.RefreshQuestCompletion(QuestClass.CompletionTypes.AutoComplete, null, true, null);
		}
	}

	// Token: 0x06004789 RID: 18313 RVA: 0x001C2798 File Offset: 0x001C0998
	public override BaseObjective Clone()
	{
		ObjectiveExchangeItemFrom objectiveExchangeItemFrom = new ObjectiveExchangeItemFrom();
		this.CopyValues(objectiveExchangeItemFrom);
		return objectiveExchangeItemFrom;
	}

	// Token: 0x0600478A RID: 18314 RVA: 0x001C27B3 File Offset: 0x001C09B3
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
		parameters.Add("expected_item_id", this.expectedItem.ItemClass.Name);
		parameters.Add("count", this.exchangeCount);
	}

	// Token: 0x0400397F RID: 14719
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue expectedItem = ItemValue.None;

	// Token: 0x04003980 RID: 14720
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass expectedItemClass;

	// Token: 0x04003981 RID: 14721
	[PublicizedFrom(EAccessModifier.Private)]
	public int exchangeCount;
}

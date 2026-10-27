using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020009A1 RID: 2465
[Preserve]
public class ObjectiveExchangeItemFrom : BaseObjective
{
	// Token: 0x170007BE RID: 1982
	// (get) Token: 0x060047A5 RID: 18341 RVA: 0x00020119 File Offset: 0x0001E319
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x060047A6 RID: 18342 RVA: 0x001C36B8 File Offset: 0x001C18B8
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveExchangeItemFrom_keyword", false, null);
		this.expectedItem = ItemClass.GetItem(this.ID, false);
		this.expectedItemClass = ItemClass.GetItemClass(this.ID, false);
		this.exchangeCount = Convert.ToInt32(this.Value);
	}

	// Token: 0x060047A7 RID: 18343 RVA: 0x001C370C File Offset: 0x001C190C
	public override void SetupDisplay()
	{
		base.Description = string.Format(this.keyword, this.expectedItemClass.GetLocalizedItemName());
		this.StatusText = string.Format("{0}/{1}", base.CurrentValue, this.exchangeCount);
	}

	// Token: 0x060047A8 RID: 18344 RVA: 0x001C375B File Offset: 0x001C195B
	public override void AddHooks()
	{
		QuestEventManager.Current.ExchangeFromItem += this.Current_ExchangeItem;
	}

	// Token: 0x060047A9 RID: 18345 RVA: 0x001C3773 File Offset: 0x001C1973
	public override void RemoveHooks()
	{
		QuestEventManager.Current.ExchangeFromItem -= this.Current_ExchangeItem;
	}

	// Token: 0x060047AA RID: 18346 RVA: 0x001C378C File Offset: 0x001C198C
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

	// Token: 0x060047AB RID: 18347 RVA: 0x001C37E2 File Offset: 0x001C19E2
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

	// Token: 0x060047AC RID: 18348 RVA: 0x001C381C File Offset: 0x001C1A1C
	public override BaseObjective Clone()
	{
		ObjectiveExchangeItemFrom objectiveExchangeItemFrom = new ObjectiveExchangeItemFrom();
		this.CopyValues(objectiveExchangeItemFrom);
		return objectiveExchangeItemFrom;
	}

	// Token: 0x060047AD RID: 18349 RVA: 0x001C3837 File Offset: 0x001C1A37
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
		parameters.Add("expected_item_id", this.expectedItem.ItemClass.Name);
		parameters.Add("count", this.exchangeCount);
	}

	// Token: 0x0400399F RID: 14751
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue expectedItem = ItemValue.None;

	// Token: 0x040039A0 RID: 14752
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass expectedItemClass;

	// Token: 0x040039A1 RID: 14753
	[PublicizedFrom(EAccessModifier.Private)]
	public int exchangeCount;
}

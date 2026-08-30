using System;
using UnityEngine.Scripting;

// Token: 0x02000991 RID: 2449
[Preserve]
public class ObjectiveExchangeItemFrom : BaseObjective
{
	// Token: 0x17000798 RID: 1944
	// (get) Token: 0x060047D5 RID: 18389 RVA: 0x00020021 File Offset: 0x0001E221
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x060047D6 RID: 18390 RVA: 0x001C1F18 File Offset: 0x001C0118
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveExchangeItemFrom_keyword", false, null);
		this.expectedItem = ItemClass.GetItem(this.ID, false);
		this.expectedItemClass = ItemClass.GetItemClass(this.ID, false);
		this.exchangeCount = Convert.ToInt32(this.Value);
	}

	// Token: 0x060047D7 RID: 18391 RVA: 0x001C1F6C File Offset: 0x001C016C
	public override void SetupDisplay()
	{
		base.Description = string.Format(this.keyword, this.expectedItemClass.GetLocalizedItemName());
		this.StatusText = string.Format("{0}/{1}", base.CurrentValue, this.exchangeCount);
	}

	// Token: 0x060047D8 RID: 18392 RVA: 0x001C1FBB File Offset: 0x001C01BB
	public override void AddHooks()
	{
		QuestEventManager.Current.ExchangeFromItem += this.Current_ExchangeItem;
	}

	// Token: 0x060047D9 RID: 18393 RVA: 0x001C1FD3 File Offset: 0x001C01D3
	public override void RemoveHooks()
	{
		QuestEventManager.Current.ExchangeFromItem -= this.Current_ExchangeItem;
	}

	// Token: 0x060047DA RID: 18394 RVA: 0x001C1FEC File Offset: 0x001C01EC
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

	// Token: 0x060047DB RID: 18395 RVA: 0x001C2042 File Offset: 0x001C0242
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

	// Token: 0x060047DC RID: 18396 RVA: 0x001C207C File Offset: 0x001C027C
	public override BaseObjective Clone()
	{
		ObjectiveExchangeItemFrom objectiveExchangeItemFrom = new ObjectiveExchangeItemFrom();
		this.CopyValues(objectiveExchangeItemFrom);
		return objectiveExchangeItemFrom;
	}

	// Token: 0x04003983 RID: 14723
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue expectedItem = ItemValue.None;

	// Token: 0x04003984 RID: 14724
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass expectedItemClass;

	// Token: 0x04003985 RID: 14725
	[PublicizedFrom(EAccessModifier.Private)]
	public int exchangeCount;
}

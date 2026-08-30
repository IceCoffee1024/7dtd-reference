using System;
using UnityEngine.Scripting;

// Token: 0x02000980 RID: 2432
[Preserve]
public class ObjectiveExchangeItemFrom : BaseObjective
{
	// Token: 0x17000786 RID: 1926
	// (get) Token: 0x0600474F RID: 18255 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x06004750 RID: 18256 RVA: 0x001BF724 File Offset: 0x001BD924
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveExchangeItemFrom_keyword", false, null);
		this.expectedItem = ItemClass.GetItem(this.ID, false);
		this.expectedItemClass = ItemClass.GetItemClass(this.ID, false);
		this.exchangeCount = Convert.ToInt32(this.Value);
	}

	// Token: 0x06004751 RID: 18257 RVA: 0x001BF778 File Offset: 0x001BD978
	public override void SetupDisplay()
	{
		base.Description = string.Format(this.keyword, this.expectedItemClass.GetLocalizedItemName());
		this.StatusText = string.Format("{0}/{1}", base.CurrentValue, this.exchangeCount);
	}

	// Token: 0x06004752 RID: 18258 RVA: 0x001BF7C7 File Offset: 0x001BD9C7
	public override void AddHooks()
	{
		QuestEventManager.Current.ExchangeFromItem += this.Current_ExchangeItem;
	}

	// Token: 0x06004753 RID: 18259 RVA: 0x001BF7DF File Offset: 0x001BD9DF
	public override void RemoveHooks()
	{
		QuestEventManager.Current.ExchangeFromItem -= this.Current_ExchangeItem;
	}

	// Token: 0x06004754 RID: 18260 RVA: 0x001BF7F8 File Offset: 0x001BD9F8
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

	// Token: 0x06004755 RID: 18261 RVA: 0x001BF84E File Offset: 0x001BDA4E
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

	// Token: 0x06004756 RID: 18262 RVA: 0x001BF888 File Offset: 0x001BDA88
	public override BaseObjective Clone()
	{
		ObjectiveExchangeItemFrom objectiveExchangeItemFrom = new ObjectiveExchangeItemFrom();
		this.CopyValues(objectiveExchangeItemFrom);
		return objectiveExchangeItemFrom;
	}

	// Token: 0x040038F2 RID: 14578
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue expectedItem = ItemValue.None;

	// Token: 0x040038F3 RID: 14579
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass expectedItemClass;

	// Token: 0x040038F4 RID: 14580
	[PublicizedFrom(EAccessModifier.Private)]
	public int exchangeCount;
}

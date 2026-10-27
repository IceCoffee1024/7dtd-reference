using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x0200196D RID: 6509
	[Preserve]
	public class ChallengeObjectiveTrader : BaseChallengeObjective
	{
		// Token: 0x17001977 RID: 6519
		// (get) Token: 0x0600C8A0 RID: 51360 RVA: 0x001B39BC File Offset: 0x001B1BBC
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Trader;
			}
		}

		// Token: 0x17001978 RID: 6520
		// (get) Token: 0x0600C8A1 RID: 51361 RVA: 0x00496054 File Offset: 0x00494254
		public override string DescriptionText
		{
			get
			{
				if (string.IsNullOrEmpty(this.TraderName))
				{
					if (!this.BuyItems)
					{
						return Localization.Get("challengeObjectiveSellItems", false, null);
					}
					return Localization.Get("challengeObjectiveBuyItems", false, null);
				}
				else
				{
					if (!this.BuyItems)
					{
						return string.Format(Localization.Get("challengeObjectiveSellItemsTo", false, null), Localization.Get(this.TraderName, false, null));
					}
					return string.Format(Localization.Get("challengeObjectiveBuyItemsFrom", false, null), Localization.Get(this.TraderName, false, null));
				}
			}
		}

		// Token: 0x0600C8A2 RID: 51362 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Init()
		{
		}

		// Token: 0x0600C8A3 RID: 51363 RVA: 0x004960D5 File Offset: 0x004942D5
		public override void HandleAddHooks()
		{
			if (this.BuyItems)
			{
				QuestEventManager.Current.BuyItems += this.Current_BuyItems;
				return;
			}
			QuestEventManager.Current.SellItems += this.Current_SellItems;
		}

		// Token: 0x0600C8A4 RID: 51364 RVA: 0x0049610C File Offset: 0x0049430C
		public override void HandleRemoveHooks()
		{
			if (this.BuyItems)
			{
				QuestEventManager.Current.BuyItems -= this.Current_BuyItems;
				return;
			}
			QuestEventManager.Current.SellItems -= this.Current_SellItems;
		}

		// Token: 0x0600C8A5 RID: 51365 RVA: 0x00496144 File Offset: 0x00494344
		[PublicizedFrom(EAccessModifier.Private)]
		public void Current_BuyItems(string traderName, int itemCounts)
		{
			if (this.CheckBaseRequirements())
			{
				return;
			}
			if (this.TraderName == "" || traderName == this.TraderName)
			{
				base.Current += itemCounts;
				if (base.Current >= this.MaxCount)
				{
					base.Current = this.MaxCount;
					this.CheckObjectiveComplete(true);
				}
			}
		}

		// Token: 0x0600C8A6 RID: 51366 RVA: 0x004961AC File Offset: 0x004943AC
		[PublicizedFrom(EAccessModifier.Private)]
		public void Current_SellItems(string traderName, int itemCounts)
		{
			if (this.CheckBaseRequirements())
			{
				return;
			}
			if (this.TraderName == "" || traderName == this.TraderName)
			{
				base.Current += itemCounts;
				if (base.Current >= this.MaxCount)
				{
					base.Current = this.MaxCount;
					this.CheckObjectiveComplete(true);
				}
			}
		}

		// Token: 0x0600C8A7 RID: 51367 RVA: 0x00496214 File Offset: 0x00494414
		public override void ParseElement(XElement e)
		{
			base.ParseElement(e);
			if (e.HasAttribute("is_buy"))
			{
				this.BuyItems = StringParsers.ParseBool(e.GetAttribute("is_buy"), 0, -1, true);
			}
			if (e.HasAttribute("trader_name"))
			{
				this.TraderName = e.GetAttribute("trader_name");
			}
		}

		// Token: 0x0600C8A8 RID: 51368 RVA: 0x00496280 File Offset: 0x00494480
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveTrader
			{
				BuyItems = this.BuyItems,
				TraderName = this.TraderName
			};
		}

		// Token: 0x04009722 RID: 38690
		[PublicizedFrom(EAccessModifier.Private)]
		public bool BuyItems;

		// Token: 0x04009723 RID: 38691
		[PublicizedFrom(EAccessModifier.Private)]
		public string TraderName = "";
	}
}

using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001910 RID: 6416
	[Preserve]
	public class ChallengeObjectiveTrader : BaseChallengeObjective
	{
		// Token: 0x1700188D RID: 6285
		// (get) Token: 0x0600C634 RID: 50740 RVA: 0x001B24E4 File Offset: 0x001B06E4
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Trader;
			}
		}

		// Token: 0x1700188E RID: 6286
		// (get) Token: 0x0600C635 RID: 50741 RVA: 0x0048E174 File Offset: 0x0048C374
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

		// Token: 0x0600C636 RID: 50742 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Init()
		{
		}

		// Token: 0x0600C637 RID: 50743 RVA: 0x0048E1F5 File Offset: 0x0048C3F5
		public override void HandleAddHooks()
		{
			if (this.BuyItems)
			{
				QuestEventManager.Current.BuyItems += this.Current_BuyItems;
				return;
			}
			QuestEventManager.Current.SellItems += this.Current_SellItems;
		}

		// Token: 0x0600C638 RID: 50744 RVA: 0x0048E22C File Offset: 0x0048C42C
		public override void HandleRemoveHooks()
		{
			if (this.BuyItems)
			{
				QuestEventManager.Current.BuyItems -= this.Current_BuyItems;
				return;
			}
			QuestEventManager.Current.SellItems -= this.Current_SellItems;
		}

		// Token: 0x0600C639 RID: 50745 RVA: 0x0048E264 File Offset: 0x0048C464
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

		// Token: 0x0600C63A RID: 50746 RVA: 0x0048E2CC File Offset: 0x0048C4CC
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

		// Token: 0x0600C63B RID: 50747 RVA: 0x0048E334 File Offset: 0x0048C534
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

		// Token: 0x0600C63C RID: 50748 RVA: 0x0048E3A0 File Offset: 0x0048C5A0
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveTrader
			{
				BuyItems = this.BuyItems,
				TraderName = this.TraderName
			};
		}

		// Token: 0x04009573 RID: 38259
		[PublicizedFrom(EAccessModifier.Private)]
		public bool BuyItems;

		// Token: 0x04009574 RID: 38260
		[PublicizedFrom(EAccessModifier.Private)]
		public string TraderName = "";
	}
}

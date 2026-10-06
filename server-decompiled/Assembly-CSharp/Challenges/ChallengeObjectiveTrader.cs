using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x0200193C RID: 6460
	[Preserve]
	public class ChallengeObjectiveTrader : BaseChallengeObjective
	{
		// Token: 0x1700192F RID: 6447
		// (get) Token: 0x0600C78C RID: 51084 RVA: 0x001B2938 File Offset: 0x001B0B38
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Trader;
			}
		}

		// Token: 0x17001930 RID: 6448
		// (get) Token: 0x0600C78D RID: 51085 RVA: 0x00492660 File Offset: 0x00490860
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

		// Token: 0x0600C78E RID: 51086 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Init()
		{
		}

		// Token: 0x0600C78F RID: 51087 RVA: 0x004926E1 File Offset: 0x004908E1
		public override void HandleAddHooks()
		{
			if (this.BuyItems)
			{
				QuestEventManager.Current.BuyItems += this.Current_BuyItems;
				return;
			}
			QuestEventManager.Current.SellItems += this.Current_SellItems;
		}

		// Token: 0x0600C790 RID: 51088 RVA: 0x00492718 File Offset: 0x00490918
		public override void HandleRemoveHooks()
		{
			if (this.BuyItems)
			{
				QuestEventManager.Current.BuyItems -= this.Current_BuyItems;
				return;
			}
			QuestEventManager.Current.SellItems -= this.Current_SellItems;
		}

		// Token: 0x0600C791 RID: 51089 RVA: 0x00492750 File Offset: 0x00490950
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

		// Token: 0x0600C792 RID: 51090 RVA: 0x004927B8 File Offset: 0x004909B8
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

		// Token: 0x0600C793 RID: 51091 RVA: 0x00492820 File Offset: 0x00490A20
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

		// Token: 0x0600C794 RID: 51092 RVA: 0x0049288C File Offset: 0x00490A8C
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveTrader
			{
				BuyItems = this.BuyItems,
				TraderName = this.TraderName
			};
		}

		// Token: 0x04009639 RID: 38457
		[PublicizedFrom(EAccessModifier.Private)]
		public bool BuyItems;

		// Token: 0x0400963A RID: 38458
		[PublicizedFrom(EAccessModifier.Private)]
		public string TraderName = "";
	}
}

using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x020018EA RID: 6378
	[Preserve]
	public class ChallengeObjectiveTrader : BaseChallengeObjective
	{
		// Token: 0x1700181B RID: 6171
		// (get) Token: 0x0600C4AD RID: 50349 RVA: 0x001AFF0C File Offset: 0x001AE10C
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Trader;
			}
		}

		// Token: 0x1700181C RID: 6172
		// (get) Token: 0x0600C4AE RID: 50350 RVA: 0x00487E28 File Offset: 0x00486028
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

		// Token: 0x0600C4AF RID: 50351 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Init()
		{
		}

		// Token: 0x0600C4B0 RID: 50352 RVA: 0x00487EA9 File Offset: 0x004860A9
		public override void HandleAddHooks()
		{
			if (this.BuyItems)
			{
				QuestEventManager.Current.BuyItems += this.Current_BuyItems;
				return;
			}
			QuestEventManager.Current.SellItems += this.Current_SellItems;
		}

		// Token: 0x0600C4B1 RID: 50353 RVA: 0x00487EE0 File Offset: 0x004860E0
		public override void HandleRemoveHooks()
		{
			if (this.BuyItems)
			{
				QuestEventManager.Current.BuyItems -= this.Current_BuyItems;
				return;
			}
			QuestEventManager.Current.SellItems -= this.Current_SellItems;
		}

		// Token: 0x0600C4B2 RID: 50354 RVA: 0x00487F18 File Offset: 0x00486118
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

		// Token: 0x0600C4B3 RID: 50355 RVA: 0x00487F80 File Offset: 0x00486180
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

		// Token: 0x0600C4B4 RID: 50356 RVA: 0x00487FE8 File Offset: 0x004861E8
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

		// Token: 0x0600C4B5 RID: 50357 RVA: 0x00488054 File Offset: 0x00486254
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveTrader
			{
				BuyItems = this.BuyItems,
				TraderName = this.TraderName
			};
		}

		// Token: 0x0400944A RID: 37962
		[PublicizedFrom(EAccessModifier.Private)]
		public bool BuyItems;

		// Token: 0x0400944B RID: 37963
		[PublicizedFrom(EAccessModifier.Private)]
		public string TraderName = "";
	}
}

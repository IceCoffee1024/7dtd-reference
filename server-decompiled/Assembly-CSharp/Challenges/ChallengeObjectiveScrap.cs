using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001969 RID: 6505
	[Preserve]
	public class ChallengeObjectiveScrap : BaseChallengeObjective
	{
		// Token: 0x1700196A RID: 6506
		// (get) Token: 0x0600C873 RID: 51315 RVA: 0x000FAEF6 File Offset: 0x000F90F6
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Scrap;
			}
		}

		// Token: 0x1700196B RID: 6507
		// (get) Token: 0x0600C874 RID: 51316 RVA: 0x004959DC File Offset: 0x00493BDC
		public override string DescriptionText
		{
			get
			{
				string text = ((this.expectedItemClass != null) ? this.expectedItemClass.GetLocalizedItemName() : Localization.Get("xuiItems", false, null));
				return Localization.Get("challengeObjectiveScrap", false, null) + " " + text + ":";
			}
		}

		// Token: 0x0600C875 RID: 51317 RVA: 0x00495A27 File Offset: 0x00493C27
		public override void Init()
		{
			this.expectedItem = ItemClass.GetItem(this.itemClassID, false);
			this.expectedItemClass = ItemClass.GetItemClass(this.itemClassID, false);
		}

		// Token: 0x0600C876 RID: 51318 RVA: 0x00495A4D File Offset: 0x00493C4D
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.ScrapItem += this.Current_ScrapItem;
		}

		// Token: 0x0600C877 RID: 51319 RVA: 0x00495A68 File Offset: 0x00493C68
		[PublicizedFrom(EAccessModifier.Private)]
		public void Current_ScrapItem(ItemStack stack)
		{
			if (this.CheckBaseRequirements())
			{
				return;
			}
			if (this.expectedItemClass == null || stack.itemValue.type == this.expectedItem.type)
			{
				base.Current += stack.count;
				this.CheckObjectiveComplete(true);
			}
		}

		// Token: 0x0600C878 RID: 51320 RVA: 0x00495AB9 File Offset: 0x00493CB9
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.ScrapItem -= this.Current_ScrapItem;
		}

		// Token: 0x0600C879 RID: 51321 RVA: 0x00495AD1 File Offset: 0x00493CD1
		public override void ParseElement(XElement e)
		{
			base.ParseElement(e);
			if (e.HasAttribute("item"))
			{
				this.itemClassID = e.GetAttribute("item");
			}
		}

		// Token: 0x0600C87A RID: 51322 RVA: 0x00495B02 File Offset: 0x00493D02
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveScrap
			{
				itemClassID = this.itemClassID,
				expectedItem = this.expectedItem,
				expectedItemClass = this.expectedItemClass
			};
		}

		// Token: 0x04009719 RID: 38681
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemValue expectedItem = ItemValue.None;

		// Token: 0x0400971A RID: 38682
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemClass expectedItemClass;

		// Token: 0x0400971B RID: 38683
		public string itemClassID = "";
	}
}

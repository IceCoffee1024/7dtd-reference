using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x020018E6 RID: 6374
	[Preserve]
	public class ChallengeObjectiveScrap : BaseChallengeObjective
	{
		// Token: 0x1700180E RID: 6158
		// (get) Token: 0x0600C480 RID: 50304 RVA: 0x000F8E8A File Offset: 0x000F708A
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Scrap;
			}
		}

		// Token: 0x1700180F RID: 6159
		// (get) Token: 0x0600C481 RID: 50305 RVA: 0x004877B0 File Offset: 0x004859B0
		public override string DescriptionText
		{
			get
			{
				string text = ((this.expectedItemClass != null) ? this.expectedItemClass.GetLocalizedItemName() : Localization.Get("xuiItems", false, null));
				return Localization.Get("challengeObjectiveScrap", false, null) + " " + text + ":";
			}
		}

		// Token: 0x0600C482 RID: 50306 RVA: 0x004877FB File Offset: 0x004859FB
		public override void Init()
		{
			this.expectedItem = ItemClass.GetItem(this.itemClassID, false);
			this.expectedItemClass = ItemClass.GetItemClass(this.itemClassID, false);
		}

		// Token: 0x0600C483 RID: 50307 RVA: 0x00487821 File Offset: 0x00485A21
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.ScrapItem += this.Current_ScrapItem;
		}

		// Token: 0x0600C484 RID: 50308 RVA: 0x0048783C File Offset: 0x00485A3C
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

		// Token: 0x0600C485 RID: 50309 RVA: 0x0048788D File Offset: 0x00485A8D
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.ScrapItem -= this.Current_ScrapItem;
		}

		// Token: 0x0600C486 RID: 50310 RVA: 0x004878A5 File Offset: 0x00485AA5
		public override void ParseElement(XElement e)
		{
			base.ParseElement(e);
			if (e.HasAttribute("item"))
			{
				this.itemClassID = e.GetAttribute("item");
			}
		}

		// Token: 0x0600C487 RID: 50311 RVA: 0x004878D6 File Offset: 0x00485AD6
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveScrap
			{
				itemClassID = this.itemClassID,
				expectedItem = this.expectedItem,
				expectedItemClass = this.expectedItemClass
			};
		}

		// Token: 0x04009441 RID: 37953
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemValue expectedItem = ItemValue.None;

		// Token: 0x04009442 RID: 37954
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemClass expectedItemClass;

		// Token: 0x04009443 RID: 37955
		public string itemClassID = "";
	}
}

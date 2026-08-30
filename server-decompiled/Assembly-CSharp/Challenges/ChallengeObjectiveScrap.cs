using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x0200190C RID: 6412
	[Preserve]
	public class ChallengeObjectiveScrap : BaseChallengeObjective
	{
		// Token: 0x17001880 RID: 6272
		// (get) Token: 0x0600C607 RID: 50695 RVA: 0x000F9E96 File Offset: 0x000F8096
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Scrap;
			}
		}

		// Token: 0x17001881 RID: 6273
		// (get) Token: 0x0600C608 RID: 50696 RVA: 0x0048DAFC File Offset: 0x0048BCFC
		public override string DescriptionText
		{
			get
			{
				string text = ((this.expectedItemClass != null) ? this.expectedItemClass.GetLocalizedItemName() : Localization.Get("xuiItems", false, null));
				return Localization.Get("challengeObjectiveScrap", false, null) + " " + text + ":";
			}
		}

		// Token: 0x0600C609 RID: 50697 RVA: 0x0048DB47 File Offset: 0x0048BD47
		public override void Init()
		{
			this.expectedItem = ItemClass.GetItem(this.itemClassID, false);
			this.expectedItemClass = ItemClass.GetItemClass(this.itemClassID, false);
		}

		// Token: 0x0600C60A RID: 50698 RVA: 0x0048DB6D File Offset: 0x0048BD6D
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.ScrapItem += this.Current_ScrapItem;
		}

		// Token: 0x0600C60B RID: 50699 RVA: 0x0048DB88 File Offset: 0x0048BD88
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

		// Token: 0x0600C60C RID: 50700 RVA: 0x0048DBD9 File Offset: 0x0048BDD9
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.ScrapItem -= this.Current_ScrapItem;
		}

		// Token: 0x0600C60D RID: 50701 RVA: 0x0048DBF1 File Offset: 0x0048BDF1
		public override void ParseElement(XElement e)
		{
			base.ParseElement(e);
			if (e.HasAttribute("item"))
			{
				this.itemClassID = e.GetAttribute("item");
			}
		}

		// Token: 0x0600C60E RID: 50702 RVA: 0x0048DC22 File Offset: 0x0048BE22
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveScrap
			{
				itemClassID = this.itemClassID,
				expectedItem = this.expectedItem,
				expectedItemClass = this.expectedItemClass
			};
		}

		// Token: 0x0400956A RID: 38250
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemValue expectedItem = ItemValue.None;

		// Token: 0x0400956B RID: 38251
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemClass expectedItemClass;

		// Token: 0x0400956C RID: 38252
		public string itemClassID = "";
	}
}

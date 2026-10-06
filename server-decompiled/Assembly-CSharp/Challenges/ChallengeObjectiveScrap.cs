using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001938 RID: 6456
	[Preserve]
	public class ChallengeObjectiveScrap : BaseChallengeObjective
	{
		// Token: 0x17001922 RID: 6434
		// (get) Token: 0x0600C75F RID: 51039 RVA: 0x000FA9BE File Offset: 0x000F8BBE
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Scrap;
			}
		}

		// Token: 0x17001923 RID: 6435
		// (get) Token: 0x0600C760 RID: 51040 RVA: 0x00491FE8 File Offset: 0x004901E8
		public override string DescriptionText
		{
			get
			{
				string text = ((this.expectedItemClass != null) ? this.expectedItemClass.GetLocalizedItemName() : Localization.Get("xuiItems", false, null));
				return Localization.Get("challengeObjectiveScrap", false, null) + " " + text + ":";
			}
		}

		// Token: 0x0600C761 RID: 51041 RVA: 0x00492033 File Offset: 0x00490233
		public override void Init()
		{
			this.expectedItem = ItemClass.GetItem(this.itemClassID, false);
			this.expectedItemClass = ItemClass.GetItemClass(this.itemClassID, false);
		}

		// Token: 0x0600C762 RID: 51042 RVA: 0x00492059 File Offset: 0x00490259
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.ScrapItem += this.Current_ScrapItem;
		}

		// Token: 0x0600C763 RID: 51043 RVA: 0x00492074 File Offset: 0x00490274
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

		// Token: 0x0600C764 RID: 51044 RVA: 0x004920C5 File Offset: 0x004902C5
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.ScrapItem -= this.Current_ScrapItem;
		}

		// Token: 0x0600C765 RID: 51045 RVA: 0x004920DD File Offset: 0x004902DD
		public override void ParseElement(XElement e)
		{
			base.ParseElement(e);
			if (e.HasAttribute("item"))
			{
				this.itemClassID = e.GetAttribute("item");
			}
		}

		// Token: 0x0600C766 RID: 51046 RVA: 0x0049210E File Offset: 0x0049030E
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveScrap
			{
				itemClassID = this.itemClassID,
				expectedItem = this.expectedItem,
				expectedItemClass = this.expectedItemClass
			};
		}

		// Token: 0x04009630 RID: 38448
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemValue expectedItem = ItemValue.None;

		// Token: 0x04009631 RID: 38449
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemClass expectedItemClass;

		// Token: 0x04009632 RID: 38450
		public string itemClassID = "";
	}
}

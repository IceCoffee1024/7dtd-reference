using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001914 RID: 6420
	[Preserve]
	public class ChallengeObjectiveWear : BaseChallengeObjective
	{
		// Token: 0x17001894 RID: 6292
		// (get) Token: 0x0600C650 RID: 50768 RVA: 0x0017E246 File Offset: 0x0017C446
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Wear;
			}
		}

		// Token: 0x17001895 RID: 6293
		// (get) Token: 0x0600C651 RID: 50769 RVA: 0x0048E868 File Offset: 0x0048CA68
		public override string DescriptionText
		{
			get
			{
				if (this.expectedItemClass == null)
				{
					return Localization.Get("challengeObjectiveWear", false, null) + " " + this.wearName + ":";
				}
				return Localization.Get("challengeObjectiveWear", false, null) + " " + this.expectedItemClass.GetLocalizedItemName() + ":";
			}
		}

		// Token: 0x0600C652 RID: 50770 RVA: 0x0048E8C5 File Offset: 0x0048CAC5
		public override void Init()
		{
			this.expectedItem = ItemClass.GetItem(this.itemClassID, false);
			this.expectedItemClass = ItemClass.GetItemClass(this.itemClassID, false);
		}

		// Token: 0x0600C653 RID: 50771 RVA: 0x0048E8EC File Offset: 0x0048CAEC
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.WearItem -= this.Current_WearItem;
			XUi xui = LocalPlayerUI.GetUIForPlayer(this.Owner.Owner.Player).xui;
			XUiM_PlayerInventory playerInventory = xui.PlayerInventory;
			if (xui.PlayerEquipment.IsWearing(this.expectedItem))
			{
				base.Current = this.MaxCount;
				this.CheckObjectiveComplete(true);
				return;
			}
			QuestEventManager.Current.WearItem += this.Current_WearItem;
		}

		// Token: 0x0600C654 RID: 50772 RVA: 0x0048E96D File Offset: 0x0048CB6D
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.WearItem -= this.Current_WearItem;
		}

		// Token: 0x0600C655 RID: 50773 RVA: 0x0048E988 File Offset: 0x0048CB88
		[PublicizedFrom(EAccessModifier.Private)]
		public void Current_WearItem(ItemValue itemValue)
		{
			if (this.CheckBaseRequirements())
			{
				return;
			}
			if (!this.armorTags.IsEmpty && !this.armorTags.Test_AnySet(itemValue.ItemClass.ItemTags))
			{
				return;
			}
			if (this.itemClassID != "" && this.expectedItem.type != itemValue.type)
			{
				return;
			}
			base.Current = this.MaxCount;
			this.CheckObjectiveComplete(true);
		}

		// Token: 0x0600C656 RID: 50774 RVA: 0x0048EA00 File Offset: 0x0048CC00
		public override void ParseElement(XElement e)
		{
			base.ParseElement(e);
			if (e.HasAttribute("item"))
			{
				this.itemClassID = e.GetAttribute("item");
			}
			if (e.HasAttribute("tags"))
			{
				this.armorTags = FastTags<TagGroup.Global>.Parse(e.GetAttribute("tags"));
			}
			if (e.HasAttribute("wear_name_key"))
			{
				this.wearName = Localization.Get(e.GetAttribute("wear_name_key"), false, null);
				return;
			}
			if (e.HasAttribute("wear_name"))
			{
				this.wearName = e.GetAttribute("wear_name");
			}
		}

		// Token: 0x0600C657 RID: 50775 RVA: 0x0048EAC4 File Offset: 0x0048CCC4
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveWear
			{
				itemClassID = this.itemClassID,
				armorTags = this.armorTags,
				expectedItem = this.expectedItem,
				expectedItemClass = this.expectedItemClass,
				wearName = this.wearName
			};
		}

		// Token: 0x04009586 RID: 38278
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemValue expectedItem = ItemValue.None;

		// Token: 0x04009587 RID: 38279
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemClass expectedItemClass;

		// Token: 0x04009588 RID: 38280
		public string itemClassID = "";

		// Token: 0x04009589 RID: 38281
		[PublicizedFrom(EAccessModifier.Private)]
		public string wearName = "";

		// Token: 0x0400958A RID: 38282
		[PublicizedFrom(EAccessModifier.Private)]
		public FastTags<TagGroup.Global> armorTags;
	}
}

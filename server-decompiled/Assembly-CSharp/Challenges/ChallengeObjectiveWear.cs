using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001940 RID: 6464
	[Preserve]
	public class ChallengeObjectiveWear : BaseChallengeObjective
	{
		// Token: 0x17001936 RID: 6454
		// (get) Token: 0x0600C7A8 RID: 51112 RVA: 0x0026C7C8 File Offset: 0x0026A9C8
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Wear;
			}
		}

		// Token: 0x17001937 RID: 6455
		// (get) Token: 0x0600C7A9 RID: 51113 RVA: 0x00492D54 File Offset: 0x00490F54
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

		// Token: 0x0600C7AA RID: 51114 RVA: 0x00492DB1 File Offset: 0x00490FB1
		public override void Init()
		{
			this.expectedItem = ItemClass.GetItem(this.itemClassID, false);
			this.expectedItemClass = ItemClass.GetItemClass(this.itemClassID, false);
		}

		// Token: 0x0600C7AB RID: 51115 RVA: 0x00492DD8 File Offset: 0x00490FD8
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

		// Token: 0x0600C7AC RID: 51116 RVA: 0x00492E59 File Offset: 0x00491059
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.WearItem -= this.Current_WearItem;
		}

		// Token: 0x0600C7AD RID: 51117 RVA: 0x00492E74 File Offset: 0x00491074
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

		// Token: 0x0600C7AE RID: 51118 RVA: 0x00492EEC File Offset: 0x004910EC
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

		// Token: 0x0600C7AF RID: 51119 RVA: 0x00492FB0 File Offset: 0x004911B0
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

		// Token: 0x0400964C RID: 38476
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemValue expectedItem = ItemValue.None;

		// Token: 0x0400964D RID: 38477
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemClass expectedItemClass;

		// Token: 0x0400964E RID: 38478
		public string itemClassID = "";

		// Token: 0x0400964F RID: 38479
		[PublicizedFrom(EAccessModifier.Private)]
		public string wearName = "";

		// Token: 0x04009650 RID: 38480
		[PublicizedFrom(EAccessModifier.Private)]
		public FastTags<TagGroup.Global> armorTags;
	}
}

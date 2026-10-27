using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001971 RID: 6513
	[Preserve]
	public class ChallengeObjectiveWear : BaseChallengeObjective
	{
		// Token: 0x1700197E RID: 6526
		// (get) Token: 0x0600C8BC RID: 51388 RVA: 0x0026DCF0 File Offset: 0x0026BEF0
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Wear;
			}
		}

		// Token: 0x1700197F RID: 6527
		// (get) Token: 0x0600C8BD RID: 51389 RVA: 0x00496748 File Offset: 0x00494948
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

		// Token: 0x0600C8BE RID: 51390 RVA: 0x004967A5 File Offset: 0x004949A5
		public override void Init()
		{
			this.expectedItem = ItemClass.GetItem(this.itemClassID, false);
			this.expectedItemClass = ItemClass.GetItemClass(this.itemClassID, false);
		}

		// Token: 0x0600C8BF RID: 51391 RVA: 0x004967CC File Offset: 0x004949CC
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

		// Token: 0x0600C8C0 RID: 51392 RVA: 0x0049684D File Offset: 0x00494A4D
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.WearItem -= this.Current_WearItem;
		}

		// Token: 0x0600C8C1 RID: 51393 RVA: 0x00496868 File Offset: 0x00494A68
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

		// Token: 0x0600C8C2 RID: 51394 RVA: 0x004968E0 File Offset: 0x00494AE0
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

		// Token: 0x0600C8C3 RID: 51395 RVA: 0x004969A4 File Offset: 0x00494BA4
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

		// Token: 0x04009735 RID: 38709
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemValue expectedItem = ItemValue.None;

		// Token: 0x04009736 RID: 38710
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemClass expectedItemClass;

		// Token: 0x04009737 RID: 38711
		public string itemClassID = "";

		// Token: 0x04009738 RID: 38712
		[PublicizedFrom(EAccessModifier.Private)]
		public string wearName = "";

		// Token: 0x04009739 RID: 38713
		[PublicizedFrom(EAccessModifier.Private)]
		public FastTags<TagGroup.Global> armorTags;
	}
}

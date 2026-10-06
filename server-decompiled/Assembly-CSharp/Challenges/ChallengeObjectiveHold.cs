using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001932 RID: 6450
	[Preserve]
	public class ChallengeObjectiveHold : BaseChallengeObjective
	{
		// Token: 0x17001916 RID: 6422
		// (get) Token: 0x0600C729 RID: 50985 RVA: 0x000814A2 File Offset: 0x0007F6A2
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Hold;
			}
		}

		// Token: 0x17001917 RID: 6423
		// (get) Token: 0x0600C72A RID: 50986 RVA: 0x004913DC File Offset: 0x0048F5DC
		public override string DescriptionText
		{
			get
			{
				return Localization.Get("challengeObjectiveHold", false, null) + " " + Localization.Get(this.itemClassList[0], false, null) + ":";
			}
		}

		// Token: 0x0600C72B RID: 50987 RVA: 0x00491408 File Offset: 0x0048F608
		public override void Init()
		{
			this.itemClassList = this.itemClassID.Split(',', StringSplitOptions.None);
			this.expectedItemClass = ItemClass.GetItemClass(this.itemClassList[0], false);
		}

		// Token: 0x0600C72C RID: 50988 RVA: 0x00491432 File Offset: 0x0048F632
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.HoldItem -= this.Current_HoldItem;
			QuestEventManager.Current.HoldItem += this.Current_HoldItem;
		}

		// Token: 0x0600C72D RID: 50989 RVA: 0x00491460 File Offset: 0x0048F660
		[PublicizedFrom(EAccessModifier.Private)]
		public void Current_HoldItem(ItemValue itemValue)
		{
			if (itemValue.ItemClass != null && this.itemClassList.ContainsCaseInsensitive(itemValue.ItemClass.Name))
			{
				base.Current = this.MaxCount;
			}
			else
			{
				base.Current = 0;
			}
			this.CheckObjectiveComplete(true);
		}

		// Token: 0x0600C72E RID: 50990 RVA: 0x0049149F File Offset: 0x0048F69F
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.HoldItem -= this.Current_HoldItem;
		}

		// Token: 0x0600C72F RID: 50991 RVA: 0x004914B8 File Offset: 0x0048F6B8
		public override bool HandleCheckStatus()
		{
			ItemClass holdingItem = this.Owner.Owner.Player.inventory.holdingItem;
			if (holdingItem != null)
			{
				base.Current = (this.itemClassList.ContainsCaseInsensitive(holdingItem.Name) ? this.MaxCount : 0);
			}
			base.Complete = this.CheckObjectiveComplete(false);
			return base.Complete;
		}

		// Token: 0x0600C730 RID: 50992 RVA: 0x00491518 File Offset: 0x0048F718
		public override void ParseElement(XElement e)
		{
			base.ParseElement(e);
			if (e.HasAttribute("item"))
			{
				this.itemClassID = e.GetAttribute("item");
			}
		}

		// Token: 0x0600C731 RID: 50993 RVA: 0x00491549 File Offset: 0x0048F749
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveHold
			{
				itemClassID = this.itemClassID,
				expectedItem = this.expectedItem,
				expectedItemClass = this.expectedItemClass
			};
		}

		// Token: 0x0400961C RID: 38428
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemValue expectedItem = ItemValue.None;

		// Token: 0x0400961D RID: 38429
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemClass expectedItemClass;

		// Token: 0x0400961E RID: 38430
		public string itemClassID = "";

		// Token: 0x0400961F RID: 38431
		[PublicizedFrom(EAccessModifier.Private)]
		public string[] itemClassList;
	}
}

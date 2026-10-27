using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001963 RID: 6499
	[Preserve]
	public class ChallengeObjectiveHold : BaseChallengeObjective
	{
		// Token: 0x1700195E RID: 6494
		// (get) Token: 0x0600C83D RID: 51261 RVA: 0x000816C6 File Offset: 0x0007F8C6
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Hold;
			}
		}

		// Token: 0x1700195F RID: 6495
		// (get) Token: 0x0600C83E RID: 51262 RVA: 0x00494DD0 File Offset: 0x00492FD0
		public override string DescriptionText
		{
			get
			{
				return Localization.Get("challengeObjectiveHold", false, null) + " " + Localization.Get(this.itemClassList[0], false, null) + ":";
			}
		}

		// Token: 0x0600C83F RID: 51263 RVA: 0x00494DFC File Offset: 0x00492FFC
		public override void Init()
		{
			this.itemClassList = this.itemClassID.Split(',', StringSplitOptions.None);
			this.expectedItemClass = ItemClass.GetItemClass(this.itemClassList[0], false);
		}

		// Token: 0x0600C840 RID: 51264 RVA: 0x00494E26 File Offset: 0x00493026
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.HoldItem -= this.Current_HoldItem;
			QuestEventManager.Current.HoldItem += this.Current_HoldItem;
		}

		// Token: 0x0600C841 RID: 51265 RVA: 0x00494E54 File Offset: 0x00493054
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

		// Token: 0x0600C842 RID: 51266 RVA: 0x00494E93 File Offset: 0x00493093
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.HoldItem -= this.Current_HoldItem;
		}

		// Token: 0x0600C843 RID: 51267 RVA: 0x00494EAC File Offset: 0x004930AC
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

		// Token: 0x0600C844 RID: 51268 RVA: 0x00494F0C File Offset: 0x0049310C
		public override void ParseElement(XElement e)
		{
			base.ParseElement(e);
			if (e.HasAttribute("item"))
			{
				this.itemClassID = e.GetAttribute("item");
			}
		}

		// Token: 0x0600C845 RID: 51269 RVA: 0x00494F3D File Offset: 0x0049313D
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveHold
			{
				itemClassID = this.itemClassID,
				expectedItem = this.expectedItem,
				expectedItemClass = this.expectedItemClass
			};
		}

		// Token: 0x04009705 RID: 38661
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemValue expectedItem = ItemValue.None;

		// Token: 0x04009706 RID: 38662
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemClass expectedItemClass;

		// Token: 0x04009707 RID: 38663
		public string itemClassID = "";

		// Token: 0x04009708 RID: 38664
		[PublicizedFrom(EAccessModifier.Private)]
		public string[] itemClassList;
	}
}

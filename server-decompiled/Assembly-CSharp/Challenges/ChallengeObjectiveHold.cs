using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001906 RID: 6406
	[Preserve]
	public class ChallengeObjectiveHold : BaseChallengeObjective
	{
		// Token: 0x17001874 RID: 6260
		// (get) Token: 0x0600C5D1 RID: 50641 RVA: 0x000811EE File Offset: 0x0007F3EE
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Hold;
			}
		}

		// Token: 0x17001875 RID: 6261
		// (get) Token: 0x0600C5D2 RID: 50642 RVA: 0x0048CEEC File Offset: 0x0048B0EC
		public override string DescriptionText
		{
			get
			{
				return Localization.Get("challengeObjectiveHold", false, null) + " " + Localization.Get(this.itemClassList[0], false, null) + ":";
			}
		}

		// Token: 0x0600C5D3 RID: 50643 RVA: 0x0048CF18 File Offset: 0x0048B118
		public override void Init()
		{
			this.itemClassList = this.itemClassID.Split(',', StringSplitOptions.None);
			this.expectedItemClass = ItemClass.GetItemClass(this.itemClassList[0], false);
		}

		// Token: 0x0600C5D4 RID: 50644 RVA: 0x0048CF42 File Offset: 0x0048B142
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.HoldItem -= this.Current_HoldItem;
			QuestEventManager.Current.HoldItem += this.Current_HoldItem;
		}

		// Token: 0x0600C5D5 RID: 50645 RVA: 0x0048CF70 File Offset: 0x0048B170
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

		// Token: 0x0600C5D6 RID: 50646 RVA: 0x0048CFAF File Offset: 0x0048B1AF
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.HoldItem -= this.Current_HoldItem;
		}

		// Token: 0x0600C5D7 RID: 50647 RVA: 0x0048CFC8 File Offset: 0x0048B1C8
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

		// Token: 0x0600C5D8 RID: 50648 RVA: 0x0048D028 File Offset: 0x0048B228
		public override void ParseElement(XElement e)
		{
			base.ParseElement(e);
			if (e.HasAttribute("item"))
			{
				this.itemClassID = e.GetAttribute("item");
			}
		}

		// Token: 0x0600C5D9 RID: 50649 RVA: 0x0048D059 File Offset: 0x0048B259
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveHold
			{
				itemClassID = this.itemClassID,
				expectedItem = this.expectedItem,
				expectedItemClass = this.expectedItemClass
			};
		}

		// Token: 0x04009556 RID: 38230
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemValue expectedItem = ItemValue.None;

		// Token: 0x04009557 RID: 38231
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemClass expectedItemClass;

		// Token: 0x04009558 RID: 38232
		public string itemClassID = "";

		// Token: 0x04009559 RID: 38233
		[PublicizedFrom(EAccessModifier.Private)]
		public string[] itemClassList;
	}
}

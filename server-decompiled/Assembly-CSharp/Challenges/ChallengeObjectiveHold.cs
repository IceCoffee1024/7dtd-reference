using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x020018E0 RID: 6368
	[Preserve]
	public class ChallengeObjectiveHold : BaseChallengeObjective
	{
		// Token: 0x17001802 RID: 6146
		// (get) Token: 0x0600C44A RID: 50250 RVA: 0x00080B3A File Offset: 0x0007ED3A
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Hold;
			}
		}

		// Token: 0x17001803 RID: 6147
		// (get) Token: 0x0600C44B RID: 50251 RVA: 0x00486BA0 File Offset: 0x00484DA0
		public override string DescriptionText
		{
			get
			{
				return Localization.Get("challengeObjectiveHold", false, null) + " " + Localization.Get(this.itemClassList[0], false, null) + ":";
			}
		}

		// Token: 0x0600C44C RID: 50252 RVA: 0x00486BCC File Offset: 0x00484DCC
		public override void Init()
		{
			this.itemClassList = this.itemClassID.Split(',', StringSplitOptions.None);
			this.expectedItemClass = ItemClass.GetItemClass(this.itemClassList[0], false);
		}

		// Token: 0x0600C44D RID: 50253 RVA: 0x00486BF6 File Offset: 0x00484DF6
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.HoldItem -= this.Current_HoldItem;
			QuestEventManager.Current.HoldItem += this.Current_HoldItem;
		}

		// Token: 0x0600C44E RID: 50254 RVA: 0x00486C24 File Offset: 0x00484E24
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

		// Token: 0x0600C44F RID: 50255 RVA: 0x00486C63 File Offset: 0x00484E63
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.HoldItem -= this.Current_HoldItem;
		}

		// Token: 0x0600C450 RID: 50256 RVA: 0x00486C7C File Offset: 0x00484E7C
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

		// Token: 0x0600C451 RID: 50257 RVA: 0x00486CDC File Offset: 0x00484EDC
		public override void ParseElement(XElement e)
		{
			base.ParseElement(e);
			if (e.HasAttribute("item"))
			{
				this.itemClassID = e.GetAttribute("item");
			}
		}

		// Token: 0x0600C452 RID: 50258 RVA: 0x00486D0D File Offset: 0x00484F0D
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveHold
			{
				itemClassID = this.itemClassID,
				expectedItem = this.expectedItem,
				expectedItemClass = this.expectedItemClass
			};
		}

		// Token: 0x0400942D RID: 37933
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemValue expectedItem = ItemValue.None;

		// Token: 0x0400942E RID: 37934
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemClass expectedItemClass;

		// Token: 0x0400942F RID: 37935
		public string itemClassID = "";

		// Token: 0x04009430 RID: 37936
		[PublicizedFrom(EAccessModifier.Private)]
		public string[] itemClassList;
	}
}

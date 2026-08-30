using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001904 RID: 6404
	[Preserve]
	public class ChallengeObjectiveHarvest : ChallengeBaseTrackedItemObjective
	{
		// Token: 0x17001870 RID: 6256
		// (get) Token: 0x0600C5B7 RID: 50615 RVA: 0x00131719 File Offset: 0x0012F919
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Harvest;
			}
		}

		// Token: 0x17001871 RID: 6257
		// (get) Token: 0x0600C5B8 RID: 50616 RVA: 0x0048C634 File Offset: 0x0048A834
		public override string DescriptionText
		{
			get
			{
				return Localization.Get("challengeObjectiveHarvest", false, null) + " " + this.expectedItemClass.GetLocalizedItemName() + ":";
			}
		}

		// Token: 0x0600C5B9 RID: 50617 RVA: 0x0048C65C File Offset: 0x0048A85C
		public override void Init()
		{
			this.expectedItem = ItemClass.GetItem(this.itemClassID, false);
			this.expectedItemClass = ItemClass.GetItemClass(this.itemClassID, false);
			this.expectedHeldClass = ItemClass.GetItemClass(this.heldItemClassID, false);
		}

		// Token: 0x0600C5BA RID: 50618 RVA: 0x0048C694 File Offset: 0x0048A894
		public override void HandleOnCreated()
		{
			base.HandleOnCreated();
			this.CreateRequirements();
		}

		// Token: 0x0600C5BB RID: 50619 RVA: 0x0048C6A2 File Offset: 0x0048A8A2
		[PublicizedFrom(EAccessModifier.Private)]
		public void CreateRequirements()
		{
			if (!this.ShowRequirements)
			{
				return;
			}
			if (!this.requireHeld)
			{
				return;
			}
			this.Owner.SetRequirementGroup(new RequirementObjectiveGroupHold(this.heldItemClassID));
		}

		// Token: 0x0600C5BC RID: 50620 RVA: 0x0048C6CC File Offset: 0x0048A8CC
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.HarvestItem -= this.Current_HarvestItem;
			QuestEventManager.Current.HarvestItem += this.Current_HarvestItem;
			base.HandleAddHooks();
		}

		// Token: 0x0600C5BD RID: 50621 RVA: 0x0048C700 File Offset: 0x0048A900
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.HarvestItem -= this.Current_HarvestItem;
		}

		// Token: 0x0600C5BE RID: 50622 RVA: 0x0048C718 File Offset: 0x0048A918
		[PublicizedFrom(EAccessModifier.Private)]
		public void Current_HarvestItem(ItemValue held, ItemStack stack, BlockValue bv)
		{
			if (this.CheckBaseRequirements())
			{
				return;
			}
			if (held.ItemClass == this.expectedHeldClass || !this.requireHeld)
			{
				if (bv.isair && this.isBlock)
				{
					return;
				}
				if ((!this.isBlock || this.blockTag.IsEmpty || bv.Block.HasAnyFastTags(this.blockTag)) && stack.itemValue.type == this.expectedItem.type)
				{
					if (base.Current + stack.count > this.MaxCount)
					{
						base.Current = this.MaxCount;
					}
					else
					{
						base.Current += stack.count;
					}
					this.CheckObjectiveComplete(true);
					if (base.Complete && this.IsTracking && this.trackingEntry != null)
					{
						this.trackingEntry.RemoveHooks();
					}
				}
			}
		}

		// Token: 0x0600C5BF RID: 50623 RVA: 0x0048C7FC File Offset: 0x0048A9FC
		public override void HandleTrackingStarted()
		{
			base.HandleTrackingStarted();
			if (this.trackingEntry != null)
			{
				this.Owner.AddTrackingEntry(this.trackingEntry);
				this.trackingEntry.TrackingHelper = this.Owner.TrackingHandler;
				this.trackingEntry.AddHooks();
			}
		}

		// Token: 0x0600C5C0 RID: 50624 RVA: 0x0048B895 File Offset: 0x00489A95
		public override void HandleTrackingEnded()
		{
			base.HandleTrackingEnded();
			if (this.trackingEntry != null)
			{
				this.trackingEntry.RemoveHooks();
				this.Owner.RemoveTrackingEntry(this.trackingEntry);
			}
		}

		// Token: 0x0600C5C1 RID: 50625 RVA: 0x0048C84C File Offset: 0x0048AA4C
		public override void ParseElement(XElement e)
		{
			base.ParseElement(e);
			if (e.HasAttribute("block_tag"))
			{
				this.blockTag = FastTags<TagGroup.Global>.Parse(e.GetAttribute("block_tag"));
			}
			if (e.HasAttribute("held"))
			{
				this.heldItemClassID = e.GetAttribute("held");
			}
			if (e.HasAttribute("is_block"))
			{
				this.isBlock = StringParsers.ParseBool(e.GetAttribute("is_block"), 0, -1, true);
			}
			if (e.HasAttribute("required_held"))
			{
				this.requireHeld = StringParsers.ParseBool(e.GetAttribute("required_held"), 0, -1, true);
			}
		}

		// Token: 0x0600C5C2 RID: 50626 RVA: 0x0048C918 File Offset: 0x0048AB18
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveHarvest
			{
				itemClassID = this.itemClassID,
				heldItemClassID = this.heldItemClassID,
				overrideTrackerIndexName = this.overrideTrackerIndexName,
				expectedItem = this.expectedItem,
				expectedItemClass = this.expectedItemClass,
				expectedHeldClass = this.expectedHeldClass,
				requireHeld = this.requireHeld,
				blockTag = this.blockTag,
				isBlock = this.isBlock
			};
		}

		// Token: 0x04009548 RID: 38216
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemClass expectedHeldClass;

		// Token: 0x04009549 RID: 38217
		[PublicizedFrom(EAccessModifier.Private)]
		public string heldItemClassID = "";

		// Token: 0x0400954A RID: 38218
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isBlock = true;

		// Token: 0x0400954B RID: 38219
		[PublicizedFrom(EAccessModifier.Private)]
		public bool requireHeld;

		// Token: 0x0400954C RID: 38220
		[PublicizedFrom(EAccessModifier.Private)]
		public FastTags<TagGroup.Global> blockTag = FastTags<TagGroup.Global>.none;
	}
}

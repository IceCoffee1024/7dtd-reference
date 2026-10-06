using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001931 RID: 6449
	[Preserve]
	public class ChallengeObjectiveHarvestByTag : ChallengeBaseTrackedItemObjective
	{
		// Token: 0x17001914 RID: 6420
		// (get) Token: 0x0600C71C RID: 50972 RVA: 0x001321B1 File Offset: 0x001303B1
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Harvest;
			}
		}

		// Token: 0x17001915 RID: 6421
		// (get) Token: 0x0600C71D RID: 50973 RVA: 0x00490EAB File Offset: 0x0048F0AB
		public override string DescriptionText
		{
			get
			{
				return Localization.Get("challengeObjectiveHarvest", false, null) + " " + Localization.Get(this.targetName, false, null) + ":";
			}
		}

		// Token: 0x0600C71E RID: 50974 RVA: 0x00490ED5 File Offset: 0x0048F0D5
		public override void Init()
		{
			this.harvestTags = FastTags<TagGroup.Global>.Parse(this.harvestTag);
			this.expectedHeldClass = ItemClass.GetItemClass(this.heldItemClassID, false);
		}

		// Token: 0x0600C71F RID: 50975 RVA: 0x00490EFA File Offset: 0x0048F0FA
		public override void HandleOnCreated()
		{
			base.HandleOnCreated();
			this.CreateRequirements();
		}

		// Token: 0x0600C720 RID: 50976 RVA: 0x00490F08 File Offset: 0x0048F108
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

		// Token: 0x0600C721 RID: 50977 RVA: 0x00490F34 File Offset: 0x0048F134
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.HarvestItem -= this.Current_HarvestItem;
			QuestEventManager.Current.HarvestItem += this.Current_HarvestItem;
			base.HandleAddHooks();
			if (this.overrideTrackerIndexName != null && this.overrideTrackerIndexName != null && this.trackingEntry == null && !this.disableTracking)
			{
				this.trackingEntry = new TrackingEntry
				{
					TrackedItem = this.expectedItemClass,
					Owner = this,
					blockIndexName = this.overrideTrackerIndexName,
					navObjectName = ((this.overrideNavObject != "") ? this.overrideNavObject : "quest_resource"),
					trackDistance = this.trackDistance
				};
				this.trackingEntry.TrackingHelper = this.Owner.GetTrackingHelper();
			}
		}

		// Token: 0x0600C722 RID: 50978 RVA: 0x0049100A File Offset: 0x0048F20A
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.HarvestItem -= this.Current_HarvestItem;
		}

		// Token: 0x0600C723 RID: 50979 RVA: 0x00491024 File Offset: 0x0048F224
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
				if ((!this.isBlock || this.blockTag.IsEmpty || bv.Block.HasAnyFastTags(this.blockTag)) && stack.itemValue.ItemClass.ItemTags.Test_AnySet(this.harvestTags))
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

		// Token: 0x0600C724 RID: 50980 RVA: 0x0049110C File Offset: 0x0048F30C
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

		// Token: 0x0600C725 RID: 50981 RVA: 0x0048FD85 File Offset: 0x0048DF85
		public override void HandleTrackingEnded()
		{
			base.HandleTrackingEnded();
			if (this.trackingEntry != null)
			{
				this.trackingEntry.RemoveHooks();
				this.Owner.RemoveTrackingEntry(this.trackingEntry);
			}
		}

		// Token: 0x0600C726 RID: 50982 RVA: 0x0049115C File Offset: 0x0048F35C
		public override void ParseElement(XElement e)
		{
			base.ParseElement(e);
			if (e.HasAttribute("block_tag"))
			{
				this.blockTag = FastTags<TagGroup.Global>.Parse(e.GetAttribute("block_tag"));
			}
			if (e.HasAttribute("harvest_tags"))
			{
				this.harvestTag = e.GetAttribute("harvest_tags");
			}
			if (e.HasAttribute("target_name_key"))
			{
				this.targetName = Localization.Get(e.GetAttribute("target_name_key"), false, null);
			}
			else if (e.HasAttribute("target_name"))
			{
				this.targetName = e.GetAttribute("target_name");
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
			if (e.HasAttribute("override_nav_object"))
			{
				this.overrideNavObject = e.GetAttribute("override_nav_object");
			}
		}

		// Token: 0x0600C727 RID: 50983 RVA: 0x004912D0 File Offset: 0x0048F4D0
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveHarvestByTag
			{
				itemClassID = this.itemClassID,
				heldItemClassID = this.heldItemClassID,
				overrideTrackerIndexName = this.overrideTrackerIndexName,
				expectedItem = this.expectedItem,
				expectedItemClass = this.expectedItemClass,
				expectedHeldClass = this.expectedHeldClass,
				requireHeld = this.requireHeld,
				blockTag = this.blockTag,
				isBlock = this.isBlock,
				harvestTag = this.harvestTag,
				harvestTags = this.harvestTags,
				targetName = this.targetName,
				overrideNavObject = this.overrideNavObject
			};
		}

		// Token: 0x04009613 RID: 38419
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemClass expectedHeldClass;

		// Token: 0x04009614 RID: 38420
		[PublicizedFrom(EAccessModifier.Private)]
		public string heldItemClassID = "";

		// Token: 0x04009615 RID: 38421
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isBlock = true;

		// Token: 0x04009616 RID: 38422
		[PublicizedFrom(EAccessModifier.Private)]
		public bool requireHeld;

		// Token: 0x04009617 RID: 38423
		[PublicizedFrom(EAccessModifier.Private)]
		public FastTags<TagGroup.Global> blockTag = FastTags<TagGroup.Global>.none;

		// Token: 0x04009618 RID: 38424
		[PublicizedFrom(EAccessModifier.Private)]
		public string harvestTag = "";

		// Token: 0x04009619 RID: 38425
		[PublicizedFrom(EAccessModifier.Private)]
		public FastTags<TagGroup.Global> harvestTags = FastTags<TagGroup.Global>.none;

		// Token: 0x0400961A RID: 38426
		[PublicizedFrom(EAccessModifier.Private)]
		public string targetName = "";

		// Token: 0x0400961B RID: 38427
		[PublicizedFrom(EAccessModifier.Private)]
		public string overrideNavObject = "";
	}
}

using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001962 RID: 6498
	[Preserve]
	public class ChallengeObjectiveHarvestByTag : ChallengeBaseTrackedItemObjective
	{
		// Token: 0x1700195C RID: 6492
		// (get) Token: 0x0600C830 RID: 51248 RVA: 0x00132DA5 File Offset: 0x00130FA5
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Harvest;
			}
		}

		// Token: 0x1700195D RID: 6493
		// (get) Token: 0x0600C831 RID: 51249 RVA: 0x0049489F File Offset: 0x00492A9F
		public override string DescriptionText
		{
			get
			{
				return Localization.Get("challengeObjectiveHarvest", false, null) + " " + Localization.Get(this.targetName, false, null) + ":";
			}
		}

		// Token: 0x0600C832 RID: 51250 RVA: 0x004948C9 File Offset: 0x00492AC9
		public override void Init()
		{
			this.harvestTags = FastTags<TagGroup.Global>.Parse(this.harvestTag);
			this.expectedHeldClass = ItemClass.GetItemClass(this.heldItemClassID, false);
		}

		// Token: 0x0600C833 RID: 51251 RVA: 0x004948EE File Offset: 0x00492AEE
		public override void HandleOnCreated()
		{
			base.HandleOnCreated();
			this.CreateRequirements();
		}

		// Token: 0x0600C834 RID: 51252 RVA: 0x004948FC File Offset: 0x00492AFC
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

		// Token: 0x0600C835 RID: 51253 RVA: 0x00494928 File Offset: 0x00492B28
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

		// Token: 0x0600C836 RID: 51254 RVA: 0x004949FE File Offset: 0x00492BFE
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.HarvestItem -= this.Current_HarvestItem;
		}

		// Token: 0x0600C837 RID: 51255 RVA: 0x00494A18 File Offset: 0x00492C18
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

		// Token: 0x0600C838 RID: 51256 RVA: 0x00494B00 File Offset: 0x00492D00
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

		// Token: 0x0600C839 RID: 51257 RVA: 0x00493779 File Offset: 0x00491979
		public override void HandleTrackingEnded()
		{
			base.HandleTrackingEnded();
			if (this.trackingEntry != null)
			{
				this.trackingEntry.RemoveHooks();
				this.Owner.RemoveTrackingEntry(this.trackingEntry);
			}
		}

		// Token: 0x0600C83A RID: 51258 RVA: 0x00494B50 File Offset: 0x00492D50
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

		// Token: 0x0600C83B RID: 51259 RVA: 0x00494CC4 File Offset: 0x00492EC4
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

		// Token: 0x040096FC RID: 38652
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemClass expectedHeldClass;

		// Token: 0x040096FD RID: 38653
		[PublicizedFrom(EAccessModifier.Private)]
		public string heldItemClassID = "";

		// Token: 0x040096FE RID: 38654
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isBlock = true;

		// Token: 0x040096FF RID: 38655
		[PublicizedFrom(EAccessModifier.Private)]
		public bool requireHeld;

		// Token: 0x04009700 RID: 38656
		[PublicizedFrom(EAccessModifier.Private)]
		public FastTags<TagGroup.Global> blockTag = FastTags<TagGroup.Global>.none;

		// Token: 0x04009701 RID: 38657
		[PublicizedFrom(EAccessModifier.Private)]
		public string harvestTag = "";

		// Token: 0x04009702 RID: 38658
		[PublicizedFrom(EAccessModifier.Private)]
		public FastTags<TagGroup.Global> harvestTags = FastTags<TagGroup.Global>.none;

		// Token: 0x04009703 RID: 38659
		[PublicizedFrom(EAccessModifier.Private)]
		public string targetName = "";

		// Token: 0x04009704 RID: 38660
		[PublicizedFrom(EAccessModifier.Private)]
		public string overrideNavObject = "";
	}
}

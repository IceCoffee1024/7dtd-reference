using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001905 RID: 6405
	[Preserve]
	public class ChallengeObjectiveHarvestByTag : ChallengeBaseTrackedItemObjective
	{
		// Token: 0x17001872 RID: 6258
		// (get) Token: 0x0600C5C4 RID: 50628 RVA: 0x00131719 File Offset: 0x0012F919
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Harvest;
			}
		}

		// Token: 0x17001873 RID: 6259
		// (get) Token: 0x0600C5C5 RID: 50629 RVA: 0x0048C9BB File Offset: 0x0048ABBB
		public override string DescriptionText
		{
			get
			{
				return Localization.Get("challengeObjectiveHarvest", false, null) + " " + Localization.Get(this.targetName, false, null) + ":";
			}
		}

		// Token: 0x0600C5C6 RID: 50630 RVA: 0x0048C9E5 File Offset: 0x0048ABE5
		public override void Init()
		{
			this.harvestTags = FastTags<TagGroup.Global>.Parse(this.harvestTag);
			this.expectedHeldClass = ItemClass.GetItemClass(this.heldItemClassID, false);
		}

		// Token: 0x0600C5C7 RID: 50631 RVA: 0x0048CA0A File Offset: 0x0048AC0A
		public override void HandleOnCreated()
		{
			base.HandleOnCreated();
			this.CreateRequirements();
		}

		// Token: 0x0600C5C8 RID: 50632 RVA: 0x0048CA18 File Offset: 0x0048AC18
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

		// Token: 0x0600C5C9 RID: 50633 RVA: 0x0048CA44 File Offset: 0x0048AC44
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

		// Token: 0x0600C5CA RID: 50634 RVA: 0x0048CB1A File Offset: 0x0048AD1A
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.HarvestItem -= this.Current_HarvestItem;
		}

		// Token: 0x0600C5CB RID: 50635 RVA: 0x0048CB34 File Offset: 0x0048AD34
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

		// Token: 0x0600C5CC RID: 50636 RVA: 0x0048CC1C File Offset: 0x0048AE1C
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

		// Token: 0x0600C5CD RID: 50637 RVA: 0x0048B895 File Offset: 0x00489A95
		public override void HandleTrackingEnded()
		{
			base.HandleTrackingEnded();
			if (this.trackingEntry != null)
			{
				this.trackingEntry.RemoveHooks();
				this.Owner.RemoveTrackingEntry(this.trackingEntry);
			}
		}

		// Token: 0x0600C5CE RID: 50638 RVA: 0x0048CC6C File Offset: 0x0048AE6C
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

		// Token: 0x0600C5CF RID: 50639 RVA: 0x0048CDE0 File Offset: 0x0048AFE0
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

		// Token: 0x0400954D RID: 38221
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemClass expectedHeldClass;

		// Token: 0x0400954E RID: 38222
		[PublicizedFrom(EAccessModifier.Private)]
		public string heldItemClassID = "";

		// Token: 0x0400954F RID: 38223
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isBlock = true;

		// Token: 0x04009550 RID: 38224
		[PublicizedFrom(EAccessModifier.Private)]
		public bool requireHeld;

		// Token: 0x04009551 RID: 38225
		[PublicizedFrom(EAccessModifier.Private)]
		public FastTags<TagGroup.Global> blockTag = FastTags<TagGroup.Global>.none;

		// Token: 0x04009552 RID: 38226
		[PublicizedFrom(EAccessModifier.Private)]
		public string harvestTag = "";

		// Token: 0x04009553 RID: 38227
		[PublicizedFrom(EAccessModifier.Private)]
		public FastTags<TagGroup.Global> harvestTags = FastTags<TagGroup.Global>.none;

		// Token: 0x04009554 RID: 38228
		[PublicizedFrom(EAccessModifier.Private)]
		public string targetName = "";

		// Token: 0x04009555 RID: 38229
		[PublicizedFrom(EAccessModifier.Private)]
		public string overrideNavObject = "";
	}
}

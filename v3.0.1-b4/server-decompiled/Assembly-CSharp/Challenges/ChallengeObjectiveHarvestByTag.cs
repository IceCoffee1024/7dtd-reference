using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x020018DF RID: 6367
	[Preserve]
	public class ChallengeObjectiveHarvestByTag : ChallengeBaseTrackedItemObjective
	{
		// Token: 0x17001800 RID: 6144
		// (get) Token: 0x0600C43D RID: 50237 RVA: 0x001306A9 File Offset: 0x0012E8A9
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Harvest;
			}
		}

		// Token: 0x17001801 RID: 6145
		// (get) Token: 0x0600C43E RID: 50238 RVA: 0x0048666F File Offset: 0x0048486F
		public override string DescriptionText
		{
			get
			{
				return Localization.Get("challengeObjectiveHarvest", false, null) + " " + Localization.Get(this.targetName, false, null) + ":";
			}
		}

		// Token: 0x0600C43F RID: 50239 RVA: 0x00486699 File Offset: 0x00484899
		public override void Init()
		{
			this.harvestTags = FastTags<TagGroup.Global>.Parse(this.harvestTag);
			this.expectedHeldClass = ItemClass.GetItemClass(this.heldItemClassID, false);
		}

		// Token: 0x0600C440 RID: 50240 RVA: 0x004866BE File Offset: 0x004848BE
		public override void HandleOnCreated()
		{
			base.HandleOnCreated();
			this.CreateRequirements();
		}

		// Token: 0x0600C441 RID: 50241 RVA: 0x004866CC File Offset: 0x004848CC
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

		// Token: 0x0600C442 RID: 50242 RVA: 0x004866F8 File Offset: 0x004848F8
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

		// Token: 0x0600C443 RID: 50243 RVA: 0x004867CE File Offset: 0x004849CE
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.HarvestItem -= this.Current_HarvestItem;
		}

		// Token: 0x0600C444 RID: 50244 RVA: 0x004867E8 File Offset: 0x004849E8
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

		// Token: 0x0600C445 RID: 50245 RVA: 0x004868D0 File Offset: 0x00484AD0
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

		// Token: 0x0600C446 RID: 50246 RVA: 0x0048554D File Offset: 0x0048374D
		public override void HandleTrackingEnded()
		{
			base.HandleTrackingEnded();
			if (this.trackingEntry != null)
			{
				this.trackingEntry.RemoveHooks();
				this.Owner.RemoveTrackingEntry(this.trackingEntry);
			}
		}

		// Token: 0x0600C447 RID: 50247 RVA: 0x00486920 File Offset: 0x00484B20
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

		// Token: 0x0600C448 RID: 50248 RVA: 0x00486A94 File Offset: 0x00484C94
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

		// Token: 0x04009424 RID: 37924
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemClass expectedHeldClass;

		// Token: 0x04009425 RID: 37925
		[PublicizedFrom(EAccessModifier.Private)]
		public string heldItemClassID = "";

		// Token: 0x04009426 RID: 37926
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isBlock = true;

		// Token: 0x04009427 RID: 37927
		[PublicizedFrom(EAccessModifier.Private)]
		public bool requireHeld;

		// Token: 0x04009428 RID: 37928
		[PublicizedFrom(EAccessModifier.Private)]
		public FastTags<TagGroup.Global> blockTag = FastTags<TagGroup.Global>.none;

		// Token: 0x04009429 RID: 37929
		[PublicizedFrom(EAccessModifier.Private)]
		public string harvestTag = "";

		// Token: 0x0400942A RID: 37930
		[PublicizedFrom(EAccessModifier.Private)]
		public FastTags<TagGroup.Global> harvestTags = FastTags<TagGroup.Global>.none;

		// Token: 0x0400942B RID: 37931
		[PublicizedFrom(EAccessModifier.Private)]
		public string targetName = "";

		// Token: 0x0400942C RID: 37932
		[PublicizedFrom(EAccessModifier.Private)]
		public string overrideNavObject = "";
	}
}

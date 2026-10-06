using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001930 RID: 6448
	[Preserve]
	public class ChallengeObjectiveHarvest : ChallengeBaseTrackedItemObjective
	{
		// Token: 0x17001912 RID: 6418
		// (get) Token: 0x0600C70F RID: 50959 RVA: 0x001321B1 File Offset: 0x001303B1
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Harvest;
			}
		}

		// Token: 0x17001913 RID: 6419
		// (get) Token: 0x0600C710 RID: 50960 RVA: 0x00490B24 File Offset: 0x0048ED24
		public override string DescriptionText
		{
			get
			{
				return Localization.Get("challengeObjectiveHarvest", false, null) + " " + this.expectedItemClass.GetLocalizedItemName() + ":";
			}
		}

		// Token: 0x0600C711 RID: 50961 RVA: 0x00490B4C File Offset: 0x0048ED4C
		public override void Init()
		{
			this.expectedItem = ItemClass.GetItem(this.itemClassID, false);
			this.expectedItemClass = ItemClass.GetItemClass(this.itemClassID, false);
			this.expectedHeldClass = ItemClass.GetItemClass(this.heldItemClassID, false);
		}

		// Token: 0x0600C712 RID: 50962 RVA: 0x00490B84 File Offset: 0x0048ED84
		public override void HandleOnCreated()
		{
			base.HandleOnCreated();
			this.CreateRequirements();
		}

		// Token: 0x0600C713 RID: 50963 RVA: 0x00490B92 File Offset: 0x0048ED92
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

		// Token: 0x0600C714 RID: 50964 RVA: 0x00490BBC File Offset: 0x0048EDBC
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.HarvestItem -= this.Current_HarvestItem;
			QuestEventManager.Current.HarvestItem += this.Current_HarvestItem;
			base.HandleAddHooks();
		}

		// Token: 0x0600C715 RID: 50965 RVA: 0x00490BF0 File Offset: 0x0048EDF0
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.HarvestItem -= this.Current_HarvestItem;
		}

		// Token: 0x0600C716 RID: 50966 RVA: 0x00490C08 File Offset: 0x0048EE08
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

		// Token: 0x0600C717 RID: 50967 RVA: 0x00490CEC File Offset: 0x0048EEEC
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

		// Token: 0x0600C718 RID: 50968 RVA: 0x0048FD85 File Offset: 0x0048DF85
		public override void HandleTrackingEnded()
		{
			base.HandleTrackingEnded();
			if (this.trackingEntry != null)
			{
				this.trackingEntry.RemoveHooks();
				this.Owner.RemoveTrackingEntry(this.trackingEntry);
			}
		}

		// Token: 0x0600C719 RID: 50969 RVA: 0x00490D3C File Offset: 0x0048EF3C
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

		// Token: 0x0600C71A RID: 50970 RVA: 0x00490E08 File Offset: 0x0048F008
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

		// Token: 0x0400960E RID: 38414
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemClass expectedHeldClass;

		// Token: 0x0400960F RID: 38415
		[PublicizedFrom(EAccessModifier.Private)]
		public string heldItemClassID = "";

		// Token: 0x04009610 RID: 38416
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isBlock = true;

		// Token: 0x04009611 RID: 38417
		[PublicizedFrom(EAccessModifier.Private)]
		public bool requireHeld;

		// Token: 0x04009612 RID: 38418
		[PublicizedFrom(EAccessModifier.Private)]
		public FastTags<TagGroup.Global> blockTag = FastTags<TagGroup.Global>.none;
	}
}

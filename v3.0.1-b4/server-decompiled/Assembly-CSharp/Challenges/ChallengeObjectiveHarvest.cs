using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x020018DE RID: 6366
	[Preserve]
	public class ChallengeObjectiveHarvest : ChallengeBaseTrackedItemObjective
	{
		// Token: 0x170017FE RID: 6142
		// (get) Token: 0x0600C430 RID: 50224 RVA: 0x001306A9 File Offset: 0x0012E8A9
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Harvest;
			}
		}

		// Token: 0x170017FF RID: 6143
		// (get) Token: 0x0600C431 RID: 50225 RVA: 0x004862E8 File Offset: 0x004844E8
		public override string DescriptionText
		{
			get
			{
				return Localization.Get("challengeObjectiveHarvest", false, null) + " " + this.expectedItemClass.GetLocalizedItemName() + ":";
			}
		}

		// Token: 0x0600C432 RID: 50226 RVA: 0x00486310 File Offset: 0x00484510
		public override void Init()
		{
			this.expectedItem = ItemClass.GetItem(this.itemClassID, false);
			this.expectedItemClass = ItemClass.GetItemClass(this.itemClassID, false);
			this.expectedHeldClass = ItemClass.GetItemClass(this.heldItemClassID, false);
		}

		// Token: 0x0600C433 RID: 50227 RVA: 0x00486348 File Offset: 0x00484548
		public override void HandleOnCreated()
		{
			base.HandleOnCreated();
			this.CreateRequirements();
		}

		// Token: 0x0600C434 RID: 50228 RVA: 0x00486356 File Offset: 0x00484556
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

		// Token: 0x0600C435 RID: 50229 RVA: 0x00486380 File Offset: 0x00484580
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.HarvestItem -= this.Current_HarvestItem;
			QuestEventManager.Current.HarvestItem += this.Current_HarvestItem;
			base.HandleAddHooks();
		}

		// Token: 0x0600C436 RID: 50230 RVA: 0x004863B4 File Offset: 0x004845B4
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.HarvestItem -= this.Current_HarvestItem;
		}

		// Token: 0x0600C437 RID: 50231 RVA: 0x004863CC File Offset: 0x004845CC
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

		// Token: 0x0600C438 RID: 50232 RVA: 0x004864B0 File Offset: 0x004846B0
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

		// Token: 0x0600C439 RID: 50233 RVA: 0x0048554D File Offset: 0x0048374D
		public override void HandleTrackingEnded()
		{
			base.HandleTrackingEnded();
			if (this.trackingEntry != null)
			{
				this.trackingEntry.RemoveHooks();
				this.Owner.RemoveTrackingEntry(this.trackingEntry);
			}
		}

		// Token: 0x0600C43A RID: 50234 RVA: 0x00486500 File Offset: 0x00484700
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

		// Token: 0x0600C43B RID: 50235 RVA: 0x004865CC File Offset: 0x004847CC
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

		// Token: 0x0400941F RID: 37919
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemClass expectedHeldClass;

		// Token: 0x04009420 RID: 37920
		[PublicizedFrom(EAccessModifier.Private)]
		public string heldItemClassID = "";

		// Token: 0x04009421 RID: 37921
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isBlock = true;

		// Token: 0x04009422 RID: 37922
		[PublicizedFrom(EAccessModifier.Private)]
		public bool requireHeld;

		// Token: 0x04009423 RID: 37923
		[PublicizedFrom(EAccessModifier.Private)]
		public FastTags<TagGroup.Global> blockTag = FastTags<TagGroup.Global>.none;
	}
}

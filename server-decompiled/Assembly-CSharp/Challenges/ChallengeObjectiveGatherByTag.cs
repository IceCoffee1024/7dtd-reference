using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001902 RID: 6402
	[Preserve]
	public class ChallengeObjectiveGatherByTag : ChallengeBaseTrackedItemObjective
	{
		// Token: 0x1700186B RID: 6251
		// (get) Token: 0x0600C59B RID: 50587 RVA: 0x0048BB6D File Offset: 0x00489D6D
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.GatherByTag;
			}
		}

		// Token: 0x1700186C RID: 6252
		// (get) Token: 0x0600C59C RID: 50588 RVA: 0x0048BB71 File Offset: 0x00489D71
		public override string DescriptionText
		{
			get
			{
				return Localization.Get("challengeObjectiveGather", false, null) + " " + Localization.Get(this.targetName, false, null) + ":";
			}
		}

		// Token: 0x0600C59D RID: 50589 RVA: 0x0048BB9B File Offset: 0x00489D9B
		public override void Init()
		{
			this.gatherTags = FastTags<TagGroup.Global>.Parse(this.gatherTag);
		}

		// Token: 0x0600C59E RID: 50590 RVA: 0x0048BBB0 File Offset: 0x00489DB0
		public override void HandleAddHooks()
		{
			EntityPlayerLocal player = this.Owner.Owner.Player;
			XUiM_PlayerInventory playerInventory = LocalPlayerUI.GetUIForPlayer(this.Owner.Owner.Player).xui.PlayerInventory;
			playerInventory.Backpack.OnBackpackItemsChangedInternal -= this.ItemsChangedInternal;
			playerInventory.Toolbelt.OnToolbeltItemsChangedInternal -= this.ItemsChangedInternal;
			playerInventory.Backpack.OnBackpackItemsChangedInternal += this.ItemsChangedInternal;
			playerInventory.Toolbelt.OnToolbeltItemsChangedInternal += this.ItemsChangedInternal;
			player.DragAndDropItemChanged -= this.ItemsChangedInternal;
			player.DragAndDropItemChanged += this.ItemsChangedInternal;
			base.HandleAddHooks();
			this.ItemsChangedInternal();
			if (this.IsRequirement && this.trackingEntry != null)
			{
				this.Owner.AddTrackingEntry(this.trackingEntry);
				this.trackingEntry.TrackingHelper = this.Owner.TrackingHandler;
				this.trackingEntry.AddHooks();
			}
		}

		// Token: 0x0600C59F RID: 50591 RVA: 0x0048BCB8 File Offset: 0x00489EB8
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

		// Token: 0x0600C5A0 RID: 50592 RVA: 0x0048B895 File Offset: 0x00489A95
		public override void HandleTrackingEnded()
		{
			base.HandleTrackingEnded();
			if (this.trackingEntry != null)
			{
				this.trackingEntry.RemoveHooks();
				this.Owner.RemoveTrackingEntry(this.trackingEntry);
			}
		}

		// Token: 0x0600C5A1 RID: 50593 RVA: 0x0048BD05 File Offset: 0x00489F05
		public override bool CheckObjectiveComplete(bool handleComplete = true)
		{
			if (this.CheckForNeededItem())
			{
				base.Complete = true;
				base.Current = this.MaxCount;
				if (handleComplete)
				{
					this.Owner.HandleComplete(true, false);
				}
				return true;
			}
			base.Complete = false;
			return base.CheckObjectiveComplete(handleComplete);
		}

		// Token: 0x0600C5A2 RID: 50594 RVA: 0x0048BD44 File Offset: 0x00489F44
		[PublicizedFrom(EAccessModifier.Private)]
		public void ItemsChangedInternal()
		{
			if (this.CheckBaseRequirements())
			{
				return;
			}
			if (this.CheckObjectiveComplete(true))
			{
				if (this.IsTracking && this.trackingEntry != null)
				{
					this.trackingEntry.RemoveHooks();
				}
				if (this.IsRequirement)
				{
					this.Parent.CheckPrerequisites();
					return;
				}
			}
			else if (this.IsTracking && this.trackingEntry != null)
			{
				this.trackingEntry.AddHooks();
			}
		}

		// Token: 0x0600C5A3 RID: 50595 RVA: 0x0048B969 File Offset: 0x00489B69
		public override void UpdateStatus()
		{
			base.UpdateStatus();
			if (base.Complete)
			{
				if (this.trackingEntry != null)
				{
					this.trackingEntry.RemoveHooks();
					return;
				}
			}
			else if (this.trackingEntry != null)
			{
				this.trackingEntry.AddHooks();
			}
		}

		// Token: 0x0600C5A4 RID: 50596 RVA: 0x0048BDB0 File Offset: 0x00489FB0
		public override void HandleRemoveHooks()
		{
			EntityPlayerLocal player = this.Owner.Owner.Player;
			if (player == null)
			{
				return;
			}
			LocalPlayerUI.GetUIForPlayer(player);
			XUiM_PlayerInventory playerInventory = LocalPlayerUI.GetUIForPlayer(player).xui.PlayerInventory;
			playerInventory.Backpack.OnBackpackItemsChangedInternal -= this.ItemsChangedInternal;
			playerInventory.Toolbelt.OnToolbeltItemsChangedInternal -= this.ItemsChangedInternal;
			player.DragAndDropItemChanged -= this.ItemsChangedInternal;
			if (this.IsRequirement && this.trackingEntry != null)
			{
				this.trackingEntry.RemoveHooks();
				this.Owner.RemoveTrackingEntry(this.trackingEntry);
			}
		}

		// Token: 0x0600C5A5 RID: 50597 RVA: 0x0048BE5C File Offset: 0x0048A05C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleUpdatingCurrent()
		{
			base.HandleUpdatingCurrent();
			XUiM_PlayerInventory playerInventory = LocalPlayerUI.GetUIForPlayer(this.Owner.Owner.Player).xui.PlayerInventory;
			int num = playerInventory.Backpack.GetItemCount(this.gatherTags, -1, -1, true);
			num += playerInventory.Toolbelt.GetItemCount(this.gatherTags, -1, -1, true);
			if (num > this.MaxCount)
			{
				num = this.MaxCount;
			}
			if (this.current != num)
			{
				base.Current = num;
			}
		}

		// Token: 0x0600C5A6 RID: 50598 RVA: 0x0048BEDC File Offset: 0x0048A0DC
		[PublicizedFrom(EAccessModifier.Private)]
		public bool CheckForNeededItem()
		{
			XUiM_PlayerInventory playerInventory = LocalPlayerUI.GetUIForPlayer(this.Owner.Owner.Player).xui.PlayerInventory;
			return playerInventory.Backpack.GetItemCount(this.gatherTags, -1, -1, true) + playerInventory.Toolbelt.GetItemCount(this.gatherTags, -1, -1, true) >= this.MaxCount;
		}

		// Token: 0x0600C5A7 RID: 50599 RVA: 0x0048BF40 File Offset: 0x0048A140
		public override void ParseElement(XElement e)
		{
			base.ParseElement(e);
			if (e.HasAttribute("gather_tags"))
			{
				this.gatherTag = e.GetAttribute("gather_tags");
			}
			if (e.HasAttribute("target_name_key"))
			{
				this.targetName = Localization.Get(e.GetAttribute("target_name_key"), false, null);
				return;
			}
			if (e.HasAttribute("target_name"))
			{
				this.targetName = e.GetAttribute("target_name");
			}
		}

		// Token: 0x0600C5A8 RID: 50600 RVA: 0x0048BFD4 File Offset: 0x0048A1D4
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveGatherByTag
			{
				gatherTag = this.gatherTag,
				gatherTags = this.gatherTags,
				trackingEntry = this.trackingEntry,
				targetName = this.targetName
			};
		}

		// Token: 0x0400953E RID: 38206
		[PublicizedFrom(EAccessModifier.Private)]
		public string gatherTag = "";

		// Token: 0x0400953F RID: 38207
		[PublicizedFrom(EAccessModifier.Private)]
		public FastTags<TagGroup.Global> gatherTags;

		// Token: 0x04009540 RID: 38208
		[PublicizedFrom(EAccessModifier.Private)]
		public string targetName = "";

		// Token: 0x04009541 RID: 38209
		public BaseRequirementObjectiveGroup Parent;
	}
}

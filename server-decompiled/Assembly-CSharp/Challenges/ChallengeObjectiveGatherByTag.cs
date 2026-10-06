using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x0200192E RID: 6446
	[Preserve]
	public class ChallengeObjectiveGatherByTag : ChallengeBaseTrackedItemObjective
	{
		// Token: 0x1700190D RID: 6413
		// (get) Token: 0x0600C6F3 RID: 50931 RVA: 0x0049005D File Offset: 0x0048E25D
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.GatherByTag;
			}
		}

		// Token: 0x1700190E RID: 6414
		// (get) Token: 0x0600C6F4 RID: 50932 RVA: 0x00490061 File Offset: 0x0048E261
		public override string DescriptionText
		{
			get
			{
				return Localization.Get("challengeObjectiveGather", false, null) + " " + Localization.Get(this.targetName, false, null) + ":";
			}
		}

		// Token: 0x0600C6F5 RID: 50933 RVA: 0x0049008B File Offset: 0x0048E28B
		public override void Init()
		{
			this.gatherTags = FastTags<TagGroup.Global>.Parse(this.gatherTag);
		}

		// Token: 0x0600C6F6 RID: 50934 RVA: 0x004900A0 File Offset: 0x0048E2A0
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

		// Token: 0x0600C6F7 RID: 50935 RVA: 0x004901A8 File Offset: 0x0048E3A8
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

		// Token: 0x0600C6F8 RID: 50936 RVA: 0x0048FD85 File Offset: 0x0048DF85
		public override void HandleTrackingEnded()
		{
			base.HandleTrackingEnded();
			if (this.trackingEntry != null)
			{
				this.trackingEntry.RemoveHooks();
				this.Owner.RemoveTrackingEntry(this.trackingEntry);
			}
		}

		// Token: 0x0600C6F9 RID: 50937 RVA: 0x004901F5 File Offset: 0x0048E3F5
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

		// Token: 0x0600C6FA RID: 50938 RVA: 0x00490234 File Offset: 0x0048E434
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

		// Token: 0x0600C6FB RID: 50939 RVA: 0x0048FE59 File Offset: 0x0048E059
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

		// Token: 0x0600C6FC RID: 50940 RVA: 0x004902A0 File Offset: 0x0048E4A0
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

		// Token: 0x0600C6FD RID: 50941 RVA: 0x0049034C File Offset: 0x0048E54C
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

		// Token: 0x0600C6FE RID: 50942 RVA: 0x004903CC File Offset: 0x0048E5CC
		[PublicizedFrom(EAccessModifier.Private)]
		public bool CheckForNeededItem()
		{
			XUiM_PlayerInventory playerInventory = LocalPlayerUI.GetUIForPlayer(this.Owner.Owner.Player).xui.PlayerInventory;
			return playerInventory.Backpack.GetItemCount(this.gatherTags, -1, -1, true) + playerInventory.Toolbelt.GetItemCount(this.gatherTags, -1, -1, true) >= this.MaxCount;
		}

		// Token: 0x0600C6FF RID: 50943 RVA: 0x00490430 File Offset: 0x0048E630
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

		// Token: 0x0600C700 RID: 50944 RVA: 0x004904C4 File Offset: 0x0048E6C4
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

		// Token: 0x04009604 RID: 38404
		[PublicizedFrom(EAccessModifier.Private)]
		public string gatherTag = "";

		// Token: 0x04009605 RID: 38405
		[PublicizedFrom(EAccessModifier.Private)]
		public FastTags<TagGroup.Global> gatherTags;

		// Token: 0x04009606 RID: 38406
		[PublicizedFrom(EAccessModifier.Private)]
		public string targetName = "";

		// Token: 0x04009607 RID: 38407
		public BaseRequirementObjectiveGroup Parent;
	}
}

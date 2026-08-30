using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x020018DC RID: 6364
	[Preserve]
	public class ChallengeObjectiveGatherByTag : ChallengeBaseTrackedItemObjective
	{
		// Token: 0x170017F9 RID: 6137
		// (get) Token: 0x0600C414 RID: 50196 RVA: 0x00485825 File Offset: 0x00483A25
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.GatherByTag;
			}
		}

		// Token: 0x170017FA RID: 6138
		// (get) Token: 0x0600C415 RID: 50197 RVA: 0x00485829 File Offset: 0x00483A29
		public override string DescriptionText
		{
			get
			{
				return Localization.Get("challengeObjectiveGather", false, null) + " " + Localization.Get(this.targetName, false, null) + ":";
			}
		}

		// Token: 0x0600C416 RID: 50198 RVA: 0x00485853 File Offset: 0x00483A53
		public override void Init()
		{
			this.gatherTags = FastTags<TagGroup.Global>.Parse(this.gatherTag);
		}

		// Token: 0x0600C417 RID: 50199 RVA: 0x00485868 File Offset: 0x00483A68
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

		// Token: 0x0600C418 RID: 50200 RVA: 0x00485970 File Offset: 0x00483B70
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

		// Token: 0x0600C419 RID: 50201 RVA: 0x0048554D File Offset: 0x0048374D
		public override void HandleTrackingEnded()
		{
			base.HandleTrackingEnded();
			if (this.trackingEntry != null)
			{
				this.trackingEntry.RemoveHooks();
				this.Owner.RemoveTrackingEntry(this.trackingEntry);
			}
		}

		// Token: 0x0600C41A RID: 50202 RVA: 0x004859BD File Offset: 0x00483BBD
		public override bool CheckObjectiveComplete(bool handleComplete = true)
		{
			if (this.CheckForNeededItem())
			{
				base.Complete = true;
				base.Current = this.MaxCount;
				if (handleComplete)
				{
					this.Owner.HandleComplete(true);
				}
				return true;
			}
			base.Complete = false;
			return base.CheckObjectiveComplete(handleComplete);
		}

		// Token: 0x0600C41B RID: 50203 RVA: 0x004859FC File Offset: 0x00483BFC
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

		// Token: 0x0600C41C RID: 50204 RVA: 0x00485621 File Offset: 0x00483821
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

		// Token: 0x0600C41D RID: 50205 RVA: 0x00485A68 File Offset: 0x00483C68
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

		// Token: 0x0600C41E RID: 50206 RVA: 0x00485B14 File Offset: 0x00483D14
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

		// Token: 0x0600C41F RID: 50207 RVA: 0x00485B94 File Offset: 0x00483D94
		[PublicizedFrom(EAccessModifier.Private)]
		public bool CheckForNeededItem()
		{
			XUiM_PlayerInventory playerInventory = LocalPlayerUI.GetUIForPlayer(this.Owner.Owner.Player).xui.PlayerInventory;
			return playerInventory.Backpack.GetItemCount(this.gatherTags, -1, -1, true) + playerInventory.Toolbelt.GetItemCount(this.gatherTags, -1, -1, true) >= this.MaxCount;
		}

		// Token: 0x0600C420 RID: 50208 RVA: 0x00485BF8 File Offset: 0x00483DF8
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

		// Token: 0x0600C421 RID: 50209 RVA: 0x00485C8C File Offset: 0x00483E8C
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

		// Token: 0x04009415 RID: 37909
		[PublicizedFrom(EAccessModifier.Private)]
		public string gatherTag = "";

		// Token: 0x04009416 RID: 37910
		[PublicizedFrom(EAccessModifier.Private)]
		public FastTags<TagGroup.Global> gatherTags;

		// Token: 0x04009417 RID: 37911
		[PublicizedFrom(EAccessModifier.Private)]
		public string targetName = "";

		// Token: 0x04009418 RID: 37912
		public BaseRequirementObjectiveGroup Parent;
	}
}

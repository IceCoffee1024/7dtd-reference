using System;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001901 RID: 6401
	[Preserve]
	public class ChallengeObjectiveGather : ChallengeBaseTrackedItemObjective
	{
		// Token: 0x17001869 RID: 6249
		// (get) Token: 0x0600C58D RID: 50573 RVA: 0x000601DD File Offset: 0x0005E3DD
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Gather;
			}
		}

		// Token: 0x1700186A RID: 6250
		// (get) Token: 0x0600C58E RID: 50574 RVA: 0x0048B71D File Offset: 0x0048991D
		public override string DescriptionText
		{
			get
			{
				return Localization.Get("challengeObjectiveGather", false, null) + " " + this.expectedItemClass.GetLocalizedItemName();
			}
		}

		// Token: 0x0600C58F RID: 50575 RVA: 0x0048A102 File Offset: 0x00488302
		public override void Init()
		{
			this.expectedItem = ItemClass.GetItem(this.itemClassID, false);
			this.expectedItemClass = ItemClass.GetItemClass(this.itemClassID, false);
		}

		// Token: 0x0600C590 RID: 50576 RVA: 0x0048B740 File Offset: 0x00489940
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

		// Token: 0x0600C591 RID: 50577 RVA: 0x0048B848 File Offset: 0x00489A48
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

		// Token: 0x0600C592 RID: 50578 RVA: 0x0048B895 File Offset: 0x00489A95
		public override void HandleTrackingEnded()
		{
			base.HandleTrackingEnded();
			if (this.trackingEntry != null)
			{
				this.trackingEntry.RemoveHooks();
				this.Owner.RemoveTrackingEntry(this.trackingEntry);
			}
		}

		// Token: 0x0600C593 RID: 50579 RVA: 0x0048B8C1 File Offset: 0x00489AC1
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

		// Token: 0x0600C594 RID: 50580 RVA: 0x0048B900 File Offset: 0x00489B00
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

		// Token: 0x0600C595 RID: 50581 RVA: 0x0048B969 File Offset: 0x00489B69
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

		// Token: 0x0600C596 RID: 50582 RVA: 0x0048B9A0 File Offset: 0x00489BA0
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

		// Token: 0x0600C597 RID: 50583 RVA: 0x0048BA4C File Offset: 0x00489C4C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleUpdatingCurrent()
		{
			base.HandleUpdatingCurrent();
			XUiM_PlayerInventory playerInventory = LocalPlayerUI.GetUIForPlayer(this.Owner.Owner.Player).xui.PlayerInventory;
			int num = playerInventory.Backpack.GetItemCount(this.expectedItem, -1, -1, true);
			num += playerInventory.Toolbelt.GetItemCount(this.expectedItem, false, -1, -1, true);
			if (num > this.MaxCount)
			{
				num = this.MaxCount;
			}
			if (this.current != num)
			{
				base.Current = num;
			}
		}

		// Token: 0x0600C598 RID: 50584 RVA: 0x0048BACC File Offset: 0x00489CCC
		[PublicizedFrom(EAccessModifier.Private)]
		public bool CheckForNeededItem()
		{
			XUiM_PlayerInventory playerInventory = LocalPlayerUI.GetUIForPlayer(this.Owner.Owner.Player).xui.PlayerInventory;
			return playerInventory.Backpack.GetItemCount(this.expectedItem, -1, -1, true) + playerInventory.Toolbelt.GetItemCount(this.expectedItem, false, -1, -1, true) >= this.MaxCount;
		}

		// Token: 0x0600C599 RID: 50585 RVA: 0x0048BB2E File Offset: 0x00489D2E
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveGather
			{
				itemClassID = this.itemClassID,
				expectedItem = this.expectedItem,
				expectedItemClass = this.expectedItemClass,
				trackingEntry = this.trackingEntry
			};
		}

		// Token: 0x0400953D RID: 38205
		public BaseRequirementObjectiveGroup Parent;
	}
}

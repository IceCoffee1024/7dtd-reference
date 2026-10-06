using System;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x0200192D RID: 6445
	[Preserve]
	public class ChallengeObjectiveGather : ChallengeBaseTrackedItemObjective
	{
		// Token: 0x1700190B RID: 6411
		// (get) Token: 0x0600C6E5 RID: 50917 RVA: 0x000601CD File Offset: 0x0005E3CD
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Gather;
			}
		}

		// Token: 0x1700190C RID: 6412
		// (get) Token: 0x0600C6E6 RID: 50918 RVA: 0x0048FC0D File Offset: 0x0048DE0D
		public override string DescriptionText
		{
			get
			{
				return Localization.Get("challengeObjectiveGather", false, null) + " " + this.expectedItemClass.GetLocalizedItemName();
			}
		}

		// Token: 0x0600C6E7 RID: 50919 RVA: 0x0048E5F2 File Offset: 0x0048C7F2
		public override void Init()
		{
			this.expectedItem = ItemClass.GetItem(this.itemClassID, false);
			this.expectedItemClass = ItemClass.GetItemClass(this.itemClassID, false);
		}

		// Token: 0x0600C6E8 RID: 50920 RVA: 0x0048FC30 File Offset: 0x0048DE30
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

		// Token: 0x0600C6E9 RID: 50921 RVA: 0x0048FD38 File Offset: 0x0048DF38
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

		// Token: 0x0600C6EA RID: 50922 RVA: 0x0048FD85 File Offset: 0x0048DF85
		public override void HandleTrackingEnded()
		{
			base.HandleTrackingEnded();
			if (this.trackingEntry != null)
			{
				this.trackingEntry.RemoveHooks();
				this.Owner.RemoveTrackingEntry(this.trackingEntry);
			}
		}

		// Token: 0x0600C6EB RID: 50923 RVA: 0x0048FDB1 File Offset: 0x0048DFB1
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

		// Token: 0x0600C6EC RID: 50924 RVA: 0x0048FDF0 File Offset: 0x0048DFF0
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

		// Token: 0x0600C6ED RID: 50925 RVA: 0x0048FE59 File Offset: 0x0048E059
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

		// Token: 0x0600C6EE RID: 50926 RVA: 0x0048FE90 File Offset: 0x0048E090
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

		// Token: 0x0600C6EF RID: 50927 RVA: 0x0048FF3C File Offset: 0x0048E13C
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

		// Token: 0x0600C6F0 RID: 50928 RVA: 0x0048FFBC File Offset: 0x0048E1BC
		[PublicizedFrom(EAccessModifier.Private)]
		public bool CheckForNeededItem()
		{
			XUiM_PlayerInventory playerInventory = LocalPlayerUI.GetUIForPlayer(this.Owner.Owner.Player).xui.PlayerInventory;
			return playerInventory.Backpack.GetItemCount(this.expectedItem, -1, -1, true) + playerInventory.Toolbelt.GetItemCount(this.expectedItem, false, -1, -1, true) >= this.MaxCount;
		}

		// Token: 0x0600C6F1 RID: 50929 RVA: 0x0049001E File Offset: 0x0048E21E
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

		// Token: 0x04009603 RID: 38403
		public BaseRequirementObjectiveGroup Parent;
	}
}

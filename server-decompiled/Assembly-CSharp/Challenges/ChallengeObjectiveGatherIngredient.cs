using System;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x0200192F RID: 6447
	[Preserve]
	public class ChallengeObjectiveGatherIngredient : ChallengeBaseTrackedItemObjective
	{
		// Token: 0x1700190F RID: 6415
		// (get) Token: 0x0600C702 RID: 50946 RVA: 0x000814D1 File Offset: 0x0007F6D1
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.GatherIngredient;
			}
		}

		// Token: 0x17001910 RID: 6416
		// (get) Token: 0x0600C703 RID: 50947 RVA: 0x0048FC0D File Offset: 0x0048DE0D
		public override string DescriptionText
		{
			get
			{
				return Localization.Get("challengeObjectiveGather", false, null) + " " + this.expectedItemClass.GetLocalizedItemName();
			}
		}

		// Token: 0x17001911 RID: 6417
		// (get) Token: 0x0600C704 RID: 50948 RVA: 0x0049051C File Offset: 0x0048E71C
		public override string StatusText
		{
			get
			{
				int num = Math.Max(0, this.MaxCount - this.currentNeededCount);
				if (base.Complete)
				{
					return string.Format("{0}/{1}", num, num);
				}
				return string.Format("{0}/{1}", this.current, num);
			}
		}

		// Token: 0x0600C705 RID: 50949 RVA: 0x00490577 File Offset: 0x0048E777
		public override void Init()
		{
			this.expectedItem = this.itemRecipe.ingredients[this.IngredientIndex].itemValue;
			this.expectedItemClass = this.expectedItem.ItemClass;
		}

		// Token: 0x0600C706 RID: 50950 RVA: 0x004905AC File Offset: 0x0048E7AC
		public override void HandleAddHooks()
		{
			EntityPlayerLocal player = this.Owner.Owner.Player;
			LocalPlayerUI.GetUIForPlayer(this.Owner.Owner.Player);
			XUiM_PlayerInventory playerInventory = LocalPlayerUI.GetUIForPlayer(player).xui.PlayerInventory;
			playerInventory.Backpack.OnBackpackItemsChangedInternal -= this.ItemsChangedInternal;
			playerInventory.Toolbelt.OnToolbeltItemsChangedInternal -= this.ItemsChangedInternal;
			playerInventory.Backpack.OnBackpackItemsChangedInternal += this.ItemsChangedInternal;
			playerInventory.Toolbelt.OnToolbeltItemsChangedInternal += this.ItemsChangedInternal;
			player.DragAndDropItemChanged += this.ItemsChangedInternal;
			base.HandleAddHooks();
			if (this.trackingEntry != null)
			{
				this.Owner.AddTrackingEntry(this.trackingEntry);
				this.trackingEntry.TrackingHelper = this.Owner.TrackingHandler;
				this.trackingEntry.AddHooks();
			}
		}

		// Token: 0x0600C707 RID: 50951 RVA: 0x0049069B File Offset: 0x0048E89B
		public override bool CheckObjectiveComplete(bool handleComplete = true)
		{
			if (this.CheckForNeededItem())
			{
				base.Current = this.MaxCount;
				base.Complete = true;
				if (handleComplete)
				{
					this.Owner.HandleComplete(true, false);
				}
				return true;
			}
			base.Complete = false;
			return base.CheckObjectiveComplete(handleComplete);
		}

		// Token: 0x0600C708 RID: 50952 RVA: 0x004906D8 File Offset: 0x0048E8D8
		[PublicizedFrom(EAccessModifier.Private)]
		public void ItemsChangedInternal()
		{
			if (this.CheckObjectiveComplete(true))
			{
				if (this.trackingEntry != null)
				{
					this.trackingEntry.RemoveHooks();
				}
				this.Parent.CheckPrerequisites();
				return;
			}
			if (this.trackingEntry != null)
			{
				this.trackingEntry.AddHooks();
			}
		}

		// Token: 0x0600C709 RID: 50953 RVA: 0x0048FE59 File Offset: 0x0048E059
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

		// Token: 0x0600C70A RID: 50954 RVA: 0x00490718 File Offset: 0x0048E918
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
			if (this.trackingEntry != null)
			{
				this.trackingEntry.RemoveHooks();
				this.Owner.RemoveTrackingEntry(this.trackingEntry);
			}
		}

		// Token: 0x0600C70B RID: 50955 RVA: 0x004907BC File Offset: 0x0048E9BC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleUpdatingCurrent()
		{
			base.HandleUpdatingCurrent();
			int num = this.itemRecipe.ingredients[this.IngredientIndex].count;
			ItemValue itemValue = new ItemValue(this.itemRecipe.itemValueType, false);
			if (this.itemRecipe.UseIngredientModifier)
			{
				num = (int)EffectManager.GetValue(PassiveEffects.CraftingIngredientCount, null, (float)num, this.Owner.Owner.Player, this.itemRecipe, FastTags<TagGroup.Global>.Parse(this.expectedItemClass.GetItemName()), true, true, true, true, true, itemValue.HasQuality ? 1 : 0, true, false);
				if (num > 0)
				{
					num = (int)((float)num * XUiM_Recipes.GetCraftingInputModifier(this.itemRecipe));
					num = Utils.FastMax(1, num);
				}
			}
			LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(this.Owner.Owner.Player);
			CraftingData craftingData = uiforPlayer.xui.GetCraftingData();
			XUiM_PlayerInventory playerInventory = uiforPlayer.xui.PlayerInventory;
			RecipeQueueItem[] recipeQueueItems = craftingData.RecipeQueueItems;
			int num2 = 0;
			if (recipeQueueItems != null)
			{
				foreach (RecipeQueueItem recipeQueueItem in recipeQueueItems)
				{
					if (recipeQueueItem.Recipe != null && recipeQueueItem.Recipe.itemValueType == this.itemRecipe.itemValueType)
					{
						num2 += recipeQueueItem.Recipe.count * (int)recipeQueueItem.Multiplier;
					}
				}
			}
			num2 += playerInventory.Backpack.GetItemCount(itemValue, -1, -1, true);
			num2 += playerInventory.Toolbelt.GetItemCount(itemValue, false, -1, -1, true);
			int num3 = this.IngredientCount * Math.Max(0, this.NeededCount - num2);
			int num4 = playerInventory.Backpack.GetItemCount(this.expectedItem, -1, -1, true);
			num4 += playerInventory.Toolbelt.GetItemCount(this.expectedItem, false, -1, -1, true);
			if (num4 > num3)
			{
				num4 = num3;
			}
			if (this.current != num4)
			{
				base.Current = num4;
			}
		}

		// Token: 0x0600C70C RID: 50956 RVA: 0x0049098C File Offset: 0x0048EB8C
		[PublicizedFrom(EAccessModifier.Private)]
		public bool CheckForNeededItem()
		{
			LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(this.Owner.Owner.Player);
			XUiM_PlayerInventory playerInventory = uiforPlayer.xui.PlayerInventory;
			ItemValue itemValue = new ItemValue(this.itemRecipe.itemValueType, false);
			RecipeQueueItem[] recipeQueueItems = uiforPlayer.xui.GetCraftingData().RecipeQueueItems;
			int num = playerInventory.Backpack.GetItemCount(this.expectedItem, -1, -1, true);
			num += playerInventory.Toolbelt.GetItemCount(this.expectedItem, false, -1, -1, true);
			this.currentNeededCount = 0;
			this.currentNeededCount = playerInventory.Backpack.GetItemCount(itemValue, -1, -1, true);
			this.currentNeededCount += playerInventory.Toolbelt.GetItemCount(itemValue, false, -1, -1, true);
			int num2 = 0;
			if (recipeQueueItems != null)
			{
				foreach (RecipeQueueItem recipeQueueItem in recipeQueueItems)
				{
					if (recipeQueueItem.Recipe != null && recipeQueueItem.Recipe.itemValueType == this.itemRecipe.itemValueType)
					{
						num2 += recipeQueueItem.Recipe.count * (int)recipeQueueItem.Multiplier;
					}
				}
			}
			return num >= this.IngredientCount * Math.Max(0, this.NeededCount - (this.currentNeededCount + num2));
		}

		// Token: 0x0600C70D RID: 50957 RVA: 0x00490AC0 File Offset: 0x0048ECC0
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveGatherIngredient
			{
				itemRecipe = this.itemRecipe,
				IngredientIndex = this.IngredientIndex,
				expectedItem = this.expectedItem,
				expectedItemClass = this.expectedItemClass,
				NeededCount = this.NeededCount
			};
		}

		// Token: 0x04009608 RID: 38408
		public Recipe itemRecipe;

		// Token: 0x04009609 RID: 38409
		public int IngredientIndex = -1;

		// Token: 0x0400960A RID: 38410
		public int IngredientCount = -1;

		// Token: 0x0400960B RID: 38411
		public int NeededCount;

		// Token: 0x0400960C RID: 38412
		public int currentNeededCount;

		// Token: 0x0400960D RID: 38413
		public BaseRequirementObjectiveGroup Parent;
	}
}

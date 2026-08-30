using System;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x020018DD RID: 6365
	[Preserve]
	public class ChallengeObjectiveGatherIngredient : ChallengeBaseTrackedItemObjective
	{
		// Token: 0x170017FB RID: 6139
		// (get) Token: 0x0600C423 RID: 50211 RVA: 0x00080B69 File Offset: 0x0007ED69
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.GatherIngredient;
			}
		}

		// Token: 0x170017FC RID: 6140
		// (get) Token: 0x0600C424 RID: 50212 RVA: 0x004853D5 File Offset: 0x004835D5
		public override string DescriptionText
		{
			get
			{
				return Localization.Get("challengeObjectiveGather", false, null) + " " + this.expectedItemClass.GetLocalizedItemName();
			}
		}

		// Token: 0x170017FD RID: 6141
		// (get) Token: 0x0600C425 RID: 50213 RVA: 0x00485CE4 File Offset: 0x00483EE4
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

		// Token: 0x0600C426 RID: 50214 RVA: 0x00485D3F File Offset: 0x00483F3F
		public override void Init()
		{
			this.expectedItem = this.itemRecipe.ingredients[this.IngredientIndex].itemValue;
			this.expectedItemClass = this.expectedItem.ItemClass;
		}

		// Token: 0x0600C427 RID: 50215 RVA: 0x00485D74 File Offset: 0x00483F74
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

		// Token: 0x0600C428 RID: 50216 RVA: 0x00485E63 File Offset: 0x00484063
		public override bool CheckObjectiveComplete(bool handleComplete = true)
		{
			if (this.CheckForNeededItem())
			{
				base.Current = this.MaxCount;
				base.Complete = true;
				if (handleComplete)
				{
					this.Owner.HandleComplete(true);
				}
				return true;
			}
			base.Complete = false;
			return base.CheckObjectiveComplete(handleComplete);
		}

		// Token: 0x0600C429 RID: 50217 RVA: 0x00485E9F File Offset: 0x0048409F
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

		// Token: 0x0600C42A RID: 50218 RVA: 0x00485621 File Offset: 0x00483821
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

		// Token: 0x0600C42B RID: 50219 RVA: 0x00485EDC File Offset: 0x004840DC
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

		// Token: 0x0600C42C RID: 50220 RVA: 0x00485F80 File Offset: 0x00484180
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

		// Token: 0x0600C42D RID: 50221 RVA: 0x00486150 File Offset: 0x00484350
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

		// Token: 0x0600C42E RID: 50222 RVA: 0x00486284 File Offset: 0x00484484
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

		// Token: 0x04009419 RID: 37913
		public Recipe itemRecipe;

		// Token: 0x0400941A RID: 37914
		public int IngredientIndex = -1;

		// Token: 0x0400941B RID: 37915
		public int IngredientCount = -1;

		// Token: 0x0400941C RID: 37916
		public int NeededCount;

		// Token: 0x0400941D RID: 37917
		public int currentNeededCount;

		// Token: 0x0400941E RID: 37918
		public BaseRequirementObjectiveGroup Parent;
	}
}

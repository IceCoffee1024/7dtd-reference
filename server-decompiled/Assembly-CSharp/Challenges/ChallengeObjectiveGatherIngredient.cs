using System;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001960 RID: 6496
	[Preserve]
	public class ChallengeObjectiveGatherIngredient : ChallengeBaseTrackedItemObjective
	{
		// Token: 0x17001957 RID: 6487
		// (get) Token: 0x0600C816 RID: 51222 RVA: 0x000816F5 File Offset: 0x0007F8F5
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.GatherIngredient;
			}
		}

		// Token: 0x17001958 RID: 6488
		// (get) Token: 0x0600C817 RID: 51223 RVA: 0x00493601 File Offset: 0x00491801
		public override string DescriptionText
		{
			get
			{
				return Localization.Get("challengeObjectiveGather", false, null) + " " + this.expectedItemClass.GetLocalizedItemName();
			}
		}

		// Token: 0x17001959 RID: 6489
		// (get) Token: 0x0600C818 RID: 51224 RVA: 0x00493F10 File Offset: 0x00492110
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

		// Token: 0x0600C819 RID: 51225 RVA: 0x00493F6B File Offset: 0x0049216B
		public override void Init()
		{
			this.expectedItem = this.itemRecipe.ingredients[this.IngredientIndex].itemValue;
			this.expectedItemClass = this.expectedItem.ItemClass;
		}

		// Token: 0x0600C81A RID: 51226 RVA: 0x00493FA0 File Offset: 0x004921A0
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

		// Token: 0x0600C81B RID: 51227 RVA: 0x0049408F File Offset: 0x0049228F
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

		// Token: 0x0600C81C RID: 51228 RVA: 0x004940CC File Offset: 0x004922CC
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

		// Token: 0x0600C81D RID: 51229 RVA: 0x0049384D File Offset: 0x00491A4D
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

		// Token: 0x0600C81E RID: 51230 RVA: 0x0049410C File Offset: 0x0049230C
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

		// Token: 0x0600C81F RID: 51231 RVA: 0x004941B0 File Offset: 0x004923B0
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

		// Token: 0x0600C820 RID: 51232 RVA: 0x00494380 File Offset: 0x00492580
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

		// Token: 0x0600C821 RID: 51233 RVA: 0x004944B4 File Offset: 0x004926B4
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

		// Token: 0x040096F1 RID: 38641
		public Recipe itemRecipe;

		// Token: 0x040096F2 RID: 38642
		public int IngredientIndex = -1;

		// Token: 0x040096F3 RID: 38643
		public int IngredientCount = -1;

		// Token: 0x040096F4 RID: 38644
		public int NeededCount;

		// Token: 0x040096F5 RID: 38645
		public int currentNeededCount;

		// Token: 0x040096F6 RID: 38646
		public BaseRequirementObjectiveGroup Parent;
	}
}

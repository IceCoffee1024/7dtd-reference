using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001918 RID: 6424
	[Preserve]
	public class RequirementObjectiveGroupBlockUpgrade : BaseRequirementObjectiveGroup
	{
		// Token: 0x0600C67C RID: 50812 RVA: 0x0048F13A File Offset: 0x0048D33A
		public RequirementObjectiveGroupBlockUpgrade(string itemID, string neededResourceID, int neededResourceCount)
		{
			this.ItemID = itemID;
			this.NeededResourceID = neededResourceID;
			this.NeededResourceCount = neededResourceCount;
		}

		// Token: 0x0600C67D RID: 50813 RVA: 0x0048F174 File Offset: 0x0048D374
		public override void CreateRequirements()
		{
			if (this.PhaseList == null)
			{
				this.PhaseList = new List<RequirementGroupPhase>();
			}
			this.ResourceRecipe = CraftingManager.GetRecipe(this.NeededResourceID);
			RequirementGroupPhase requirementGroupPhase;
			if (this.ResourceRecipe == null || (this.ResourceRecipe != null && this.ResourceRecipe.ingredients.Count == 0))
			{
				requirementGroupPhase = new RequirementGroupPhase();
				ChallengeObjectiveGather challengeObjectiveGather = new ChallengeObjectiveGather();
				challengeObjectiveGather.Owner = this.Owner;
				challengeObjectiveGather.IsRequirement = true;
				challengeObjectiveGather.Parent = this;
				challengeObjectiveGather.SetupItem(this.NeededResourceID);
				challengeObjectiveGather.MaxCount = this.NeededResourceCount;
				challengeObjectiveGather.Init();
				requirementGroupPhase.AddChallengeObjective(challengeObjectiveGather);
				this.PhaseList.Add(requirementGroupPhase);
			}
			else
			{
				requirementGroupPhase = this.AddIngredientGatheringReqs();
				if (requirementGroupPhase != null)
				{
					this.PhaseList.Add(requirementGroupPhase);
					requirementGroupPhase = new RequirementGroupPhase();
					ChallengeObjectiveCraft challengeObjectiveCraft = new ChallengeObjectiveCraft();
					challengeObjectiveCraft.Owner = this.Owner;
					challengeObjectiveCraft.SetupItem(this.NeededResourceID);
					challengeObjectiveCraft.IsRequirement = true;
					challengeObjectiveCraft.MaxCount = this.NeededResourceCount;
					challengeObjectiveCraft.Init();
					requirementGroupPhase.AddChallengeObjective(challengeObjectiveCraft);
					this.PhaseList.Add(requirementGroupPhase);
				}
			}
			requirementGroupPhase = new RequirementGroupPhase();
			ChallengeObjectiveHold challengeObjectiveHold = new ChallengeObjectiveHold();
			challengeObjectiveHold.Owner = this.Owner;
			challengeObjectiveHold.itemClassID = this.ItemID;
			challengeObjectiveHold.IsRequirement = true;
			challengeObjectiveHold.MaxCount = 1;
			challengeObjectiveHold.Init();
			requirementGroupPhase.AddChallengeObjective(challengeObjectiveHold);
			this.PhaseList.Add(requirementGroupPhase);
		}

		// Token: 0x0600C67E RID: 50814 RVA: 0x0048F2D8 File Offset: 0x0048D4D8
		[PublicizedFrom(EAccessModifier.Private)]
		public RequirementGroupPhase AddIngredientGatheringReqs()
		{
			Recipe recipe = CraftingManager.GetRecipe(this.NeededResourceID);
			if (recipe == null)
			{
				return null;
			}
			RequirementGroupPhase requirementGroupPhase = new RequirementGroupPhase();
			int num = (recipe.GetOutputItemClass().HasQuality ? 1 : 0);
			for (int i = 0; i < recipe.ingredients.Count; i++)
			{
				int num2 = recipe.ingredients[i].count;
				if (recipe.UseIngredientModifier)
				{
					num2 = (int)EffectManager.GetValue(PassiveEffects.CraftingIngredientCount, null, (float)num2, this.Owner.Owner.Player, recipe, FastTags<TagGroup.Global>.Parse(recipe.ingredients[i].itemValue.ItemClass.GetItemName()), true, true, true, true, true, num, true, false);
					if (num2 > 0)
					{
						num2 = (int)((float)num2 * XUiM_Recipes.GetCraftingInputModifier(recipe));
						if (XUiM_Recipes.CraftingInputModifier > 0f)
						{
							num2 = Utils.FastMax(1, num2);
						}
					}
				}
				if (num2 != 0)
				{
					ChallengeObjectiveGatherIngredient challengeObjectiveGatherIngredient = new ChallengeObjectiveGatherIngredient();
					challengeObjectiveGatherIngredient.Owner = this.Owner;
					challengeObjectiveGatherIngredient.Parent = this;
					challengeObjectiveGatherIngredient.IsRequirement = true;
					challengeObjectiveGatherIngredient.itemRecipe = recipe;
					challengeObjectiveGatherIngredient.IngredientIndex = i;
					challengeObjectiveGatherIngredient.IngredientCount = num2;
					challengeObjectiveGatherIngredient.NeededCount = this.NeededResourceCount;
					challengeObjectiveGatherIngredient.Init();
					challengeObjectiveGatherIngredient.MaxCount = num2 * this.NeededResourceCount;
					requirementGroupPhase.AddChallengeObjective(challengeObjectiveGatherIngredient);
				}
			}
			return requirementGroupPhase;
		}

		// Token: 0x0600C67F RID: 50815 RVA: 0x0048F428 File Offset: 0x0048D628
		public override bool HasPrerequisiteCondition()
		{
			EntityPlayerLocal player = this.Owner.Owner.Player;
			LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(this.Owner.Owner.Player);
			XUiM_PlayerInventory playerInventory = uiforPlayer.xui.PlayerInventory;
			ItemClass holdingItem = this.Owner.Owner.Player.inventory.holdingItem;
			return !playerInventory.HasItem(new ItemStack(ItemClass.GetItem(this.NeededResourceID, false), this.NeededResourceCount)) || (!playerInventory.HasItem(ItemClass.GetItem(this.ItemID, false)) && !this.CheckDragDropItem(uiforPlayer.xui.DragAndDropWindow.CurrentStack)) || holdingItem.Name != this.ItemID;
		}

		// Token: 0x0600C680 RID: 50816 RVA: 0x0048F4E1 File Offset: 0x0048D6E1
		[PublicizedFrom(EAccessModifier.Private)]
		public bool CheckResourceDragDropItem(ItemStack stack)
		{
			return !stack.IsEmpty() && stack.itemValue.ItemClass.GetItemName() == this.NeededResourceID && stack.count >= this.NeededResourceCount;
		}

		// Token: 0x0600C681 RID: 50817 RVA: 0x0048F51D File Offset: 0x0048D71D
		[PublicizedFrom(EAccessModifier.Private)]
		public bool CheckDragDropItem(ItemStack stack)
		{
			return !stack.IsEmpty() && stack.itemValue.ItemClass.GetItemName() == this.ItemID;
		}

		// Token: 0x0600C682 RID: 50818 RVA: 0x0048F544 File Offset: 0x0048D744
		[PublicizedFrom(EAccessModifier.Protected)]
		public override bool CheckPhaseStatus(int index)
		{
			EntityPlayerLocal player = this.Owner.Owner.Player;
			LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(this.Owner.Owner.Player);
			XUiM_PlayerInventory playerInventory = LocalPlayerUI.GetUIForPlayer(player).xui.PlayerInventory;
			ItemClass holdingItem = this.Owner.Owner.Player.inventory.holdingItem;
			if (playerInventory.HasItem(ItemClass.GetItem(this.ItemID, false)) && playerInventory.HasItem(new ItemStack(ItemClass.GetItem(this.NeededResourceID, false), this.NeededResourceCount)) && holdingItem.Name == this.ItemID)
			{
				return false;
			}
			if (this.ResourceRecipe == null || (this.ResourceRecipe != null && this.ResourceRecipe.ingredients.Count == 0))
			{
				if (index == 0)
				{
					return !playerInventory.HasItem(new ItemStack(ItemClass.GetItem(this.NeededResourceID, false), this.NeededResourceCount)) && !this.CheckResourceDragDropItem(uiforPlayer.xui.DragAndDropWindow.CurrentStack);
				}
				if (index == 1)
				{
					return (playerInventory.HasItem(new ItemStack(ItemClass.GetItem(this.NeededResourceID, false), this.NeededResourceCount)) && !playerInventory.HasItem(ItemClass.GetItem(this.ItemID, false)) && !this.CheckDragDropItem(uiforPlayer.xui.DragAndDropWindow.CurrentStack)) || holdingItem.Name != this.ItemID;
				}
			}
			else
			{
				if (index <= 1)
				{
					return !playerInventory.HasItem(new ItemStack(ItemClass.GetItem(this.NeededResourceID, false), this.NeededResourceCount)) && !this.CheckResourceDragDropItem(uiforPlayer.xui.DragAndDropWindow.CurrentStack);
				}
				if (index == 2)
				{
					return (playerInventory.HasItem(new ItemStack(ItemClass.GetItem(this.NeededResourceID, false), this.NeededResourceCount)) && !playerInventory.HasItem(ItemClass.GetItem(this.ItemID, false)) && !this.CheckDragDropItem(uiforPlayer.xui.DragAndDropWindow.CurrentStack)) || holdingItem.Name != this.ItemID;
				}
			}
			return true;
		}

		// Token: 0x0600C683 RID: 50819 RVA: 0x0048F760 File Offset: 0x0048D960
		public override BaseRequirementObjectiveGroup Clone()
		{
			return new RequirementObjectiveGroupBlockUpgrade(this.ItemID, this.NeededResourceID, this.NeededResourceCount);
		}

		// Token: 0x04009594 RID: 38292
		public string ItemID = "";

		// Token: 0x04009595 RID: 38293
		public string NeededResourceID = "";

		// Token: 0x04009596 RID: 38294
		public int NeededResourceCount = 1;

		// Token: 0x04009597 RID: 38295
		public Recipe ResourceRecipe;
	}
}

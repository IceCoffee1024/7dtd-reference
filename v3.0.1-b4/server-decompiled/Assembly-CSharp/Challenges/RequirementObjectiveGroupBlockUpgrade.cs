using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x020018F2 RID: 6386
	[Preserve]
	public class RequirementObjectiveGroupBlockUpgrade : BaseRequirementObjectiveGroup
	{
		// Token: 0x0600C4F5 RID: 50421 RVA: 0x00488DEE File Offset: 0x00486FEE
		public RequirementObjectiveGroupBlockUpgrade(string itemID, string neededResourceID, int neededResourceCount)
		{
			this.ItemID = itemID;
			this.NeededResourceID = neededResourceID;
			this.NeededResourceCount = neededResourceCount;
		}

		// Token: 0x0600C4F6 RID: 50422 RVA: 0x00488E28 File Offset: 0x00487028
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

		// Token: 0x0600C4F7 RID: 50423 RVA: 0x00488F8C File Offset: 0x0048718C
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

		// Token: 0x0600C4F8 RID: 50424 RVA: 0x004890DC File Offset: 0x004872DC
		public override bool HasPrerequisiteCondition()
		{
			EntityPlayerLocal player = this.Owner.Owner.Player;
			LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(this.Owner.Owner.Player);
			XUiM_PlayerInventory playerInventory = uiforPlayer.xui.PlayerInventory;
			ItemClass holdingItem = this.Owner.Owner.Player.inventory.holdingItem;
			return !playerInventory.HasItem(new ItemStack(ItemClass.GetItem(this.NeededResourceID, false), this.NeededResourceCount)) || (!playerInventory.HasItem(ItemClass.GetItem(this.ItemID, false)) && !this.CheckDragDropItem(uiforPlayer.xui.DragAndDropWindow.CurrentStack)) || holdingItem.Name != this.ItemID;
		}

		// Token: 0x0600C4F9 RID: 50425 RVA: 0x00489195 File Offset: 0x00487395
		[PublicizedFrom(EAccessModifier.Private)]
		public bool CheckResourceDragDropItem(ItemStack stack)
		{
			return !stack.IsEmpty() && stack.itemValue.ItemClass.GetItemName() == this.NeededResourceID && stack.count >= this.NeededResourceCount;
		}

		// Token: 0x0600C4FA RID: 50426 RVA: 0x004891D1 File Offset: 0x004873D1
		[PublicizedFrom(EAccessModifier.Private)]
		public bool CheckDragDropItem(ItemStack stack)
		{
			return !stack.IsEmpty() && stack.itemValue.ItemClass.GetItemName() == this.ItemID;
		}

		// Token: 0x0600C4FB RID: 50427 RVA: 0x004891F8 File Offset: 0x004873F8
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

		// Token: 0x0600C4FC RID: 50428 RVA: 0x00489414 File Offset: 0x00487614
		public override BaseRequirementObjectiveGroup Clone()
		{
			return new RequirementObjectiveGroupBlockUpgrade(this.ItemID, this.NeededResourceID, this.NeededResourceCount);
		}

		// Token: 0x0400946B RID: 37995
		public string ItemID = "";

		// Token: 0x0400946C RID: 37996
		public string NeededResourceID = "";

		// Token: 0x0400946D RID: 37997
		public int NeededResourceCount = 1;

		// Token: 0x0400946E RID: 37998
		public Recipe ResourceRecipe;
	}
}

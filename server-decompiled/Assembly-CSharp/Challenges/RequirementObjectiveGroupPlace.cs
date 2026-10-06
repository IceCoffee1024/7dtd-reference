using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001948 RID: 6472
	[Preserve]
	public class RequirementObjectiveGroupPlace : BaseRequirementObjectiveGroup
	{
		// Token: 0x0600C7E8 RID: 51176 RVA: 0x00494128 File Offset: 0x00492328
		public RequirementObjectiveGroupPlace(string itemID)
		{
			this.ItemID = itemID;
		}

		// Token: 0x0600C7E9 RID: 51177 RVA: 0x00494144 File Offset: 0x00492344
		public override void CreateRequirements()
		{
			if (this.PhaseList == null)
			{
				this.PhaseList = new List<RequirementGroupPhase>();
			}
			this.PhaseList.Add(this.AddIngredientGatheringReqs());
			RequirementGroupPhase requirementGroupPhase = new RequirementGroupPhase();
			ChallengeObjectiveCraft challengeObjectiveCraft = new ChallengeObjectiveCraft();
			challengeObjectiveCraft.Owner = this.Owner;
			challengeObjectiveCraft.SetupItem(this.ItemID);
			challengeObjectiveCraft.IsRequirement = true;
			challengeObjectiveCraft.MaxCount = 1;
			challengeObjectiveCraft.Init();
			requirementGroupPhase.AddChallengeObjective(challengeObjectiveCraft);
			this.PhaseList.Add(requirementGroupPhase);
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

		// Token: 0x0600C7EA RID: 51178 RVA: 0x0049420C File Offset: 0x0049240C
		[PublicizedFrom(EAccessModifier.Private)]
		public RequirementGroupPhase AddIngredientGatheringReqs()
		{
			Recipe recipe = CraftingManager.GetRecipe(this.ItemID);
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
					challengeObjectiveGatherIngredient.NeededCount = 1;
					challengeObjectiveGatherIngredient.MaxCount = num2;
					challengeObjectiveGatherIngredient.Init();
					requirementGroupPhase.AddChallengeObjective(challengeObjectiveGatherIngredient);
				}
			}
			return requirementGroupPhase;
		}

		// Token: 0x0600C7EB RID: 51179 RVA: 0x00494348 File Offset: 0x00492548
		public override bool HasPrerequisiteCondition()
		{
			EntityPlayerLocal player = this.Owner.Owner.Player;
			LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(this.Owner.Owner.Player);
			XUiM_PlayerInventory playerInventory = uiforPlayer.xui.PlayerInventory;
			ItemClass holdingItem = this.Owner.Owner.Player.inventory.holdingItem;
			return (!playerInventory.HasItem(ItemClass.GetItem(this.ItemID, false)) && !this.CheckDragDropItem(uiforPlayer.xui.DragAndDropWindow.CurrentStack, this.ItemID)) || holdingItem.Name != this.ItemID;
		}

		// Token: 0x0600C7EC RID: 51180 RVA: 0x004943E6 File Offset: 0x004925E6
		[PublicizedFrom(EAccessModifier.Private)]
		public bool CheckDragDropItem(ItemStack stack, string itemID)
		{
			return !stack.IsEmpty() && stack.itemValue.ItemClass.GetItemName() == itemID;
		}

		// Token: 0x0600C7ED RID: 51181 RVA: 0x00494408 File Offset: 0x00492608
		[PublicizedFrom(EAccessModifier.Protected)]
		public override bool CheckPhaseStatus(int index)
		{
			EntityPlayerLocal player = this.Owner.Owner.Player;
			LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(this.Owner.Owner.Player);
			XUiM_PlayerInventory playerInventory = LocalPlayerUI.GetUIForPlayer(player).xui.PlayerInventory;
			ItemClass holdingItem = this.Owner.Owner.Player.inventory.holdingItem;
			if (playerInventory.HasItem(ItemClass.GetItem(this.ItemID, false)) && holdingItem.Name == this.ItemID)
			{
				return false;
			}
			if (index > 1)
			{
				return index != 2 || ((playerInventory.HasItem(ItemClass.GetItem(this.ItemID, false)) || this.CheckDragDropItem(uiforPlayer.xui.DragAndDropWindow.CurrentStack, this.ItemID)) && holdingItem.Name != this.ItemID);
			}
			return !playerInventory.HasItem(ItemClass.GetItem(this.ItemID, false)) && !this.CheckDragDropItem(uiforPlayer.xui.DragAndDropWindow.CurrentStack, this.ItemID);
		}

		// Token: 0x0600C7EE RID: 51182 RVA: 0x00494516 File Offset: 0x00492716
		public override BaseRequirementObjectiveGroup Clone()
		{
			return new RequirementObjectiveGroupPlace(this.ItemID);
		}

		// Token: 0x04009664 RID: 38500
		public string ItemID = "";
	}
}

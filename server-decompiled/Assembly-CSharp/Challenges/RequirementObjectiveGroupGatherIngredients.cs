using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x0200191A RID: 6426
	[Preserve]
	public class RequirementObjectiveGroupGatherIngredients : BaseRequirementObjectiveGroup
	{
		// Token: 0x0600C688 RID: 50824 RVA: 0x0048F968 File Offset: 0x0048DB68
		public RequirementObjectiveGroupGatherIngredients(string itemID)
		{
			this.ItemID = itemID;
			this.itemRecipe = CraftingManager.GetRecipe(itemID);
		}

		// Token: 0x0600C689 RID: 50825 RVA: 0x0048F990 File Offset: 0x0048DB90
		public override void CreateRequirements()
		{
			if (this.PhaseList == null)
			{
				this.PhaseList = new List<RequirementGroupPhase>();
			}
			RequirementGroupPhase requirementGroupPhase = new RequirementGroupPhase();
			int num = (this.itemRecipe.GetOutputItemClass().HasQuality ? 1 : 0);
			for (int i = 0; i < this.itemRecipe.ingredients.Count; i++)
			{
				int num2 = this.itemRecipe.ingredients[i].count;
				if (this.itemRecipe.UseIngredientModifier)
				{
					num2 = (int)EffectManager.GetValue(PassiveEffects.CraftingIngredientCount, null, (float)num2, this.Owner.Owner.Player, this.itemRecipe, FastTags<TagGroup.Global>.Parse(this.itemRecipe.ingredients[i].itemValue.ItemClass.GetItemName()), true, true, true, true, true, num, true, false);
					if (num2 > 0)
					{
						num2 = (int)((float)num2 * XUiM_Recipes.GetCraftingInputModifier(this.itemRecipe));
						if (XUiM_Recipes.CraftingInputModifier > 0f)
						{
							num2 = Utils.FastMax(1, num2);
						}
					}
				}
				if (num2 != 0)
				{
					ChallengeObjectiveGatherIngredient challengeObjectiveGatherIngredient = new ChallengeObjectiveGatherIngredient();
					challengeObjectiveGatherIngredient.Parent = this;
					challengeObjectiveGatherIngredient.Owner = this.Owner;
					challengeObjectiveGatherIngredient.IsRequirement = true;
					challengeObjectiveGatherIngredient.itemRecipe = this.itemRecipe;
					challengeObjectiveGatherIngredient.IngredientIndex = i;
					challengeObjectiveGatherIngredient.IngredientCount = num2;
					challengeObjectiveGatherIngredient.NeededCount = ((this.CraftObj == null) ? 1 : this.CraftObj.MaxCount);
					challengeObjectiveGatherIngredient.MaxCount = num2 * challengeObjectiveGatherIngredient.NeededCount;
					challengeObjectiveGatherIngredient.Init();
					requirementGroupPhase.AddChallengeObjective(challengeObjectiveGatherIngredient);
				}
			}
			this.PhaseList.Add(requirementGroupPhase);
		}

		// Token: 0x0600C68A RID: 50826 RVA: 0x00020021 File Offset: 0x0001E221
		public override bool HasPrerequisiteCondition()
		{
			return true;
		}

		// Token: 0x0600C68B RID: 50827 RVA: 0x0048FB18 File Offset: 0x0048DD18
		public override BaseRequirementObjectiveGroup Clone()
		{
			return new RequirementObjectiveGroupGatherIngredients(this.ItemID);
		}

		// Token: 0x0400959A RID: 38298
		public string ItemID = "";

		// Token: 0x0400959B RID: 38299
		[PublicizedFrom(EAccessModifier.Private)]
		public Recipe itemRecipe;

		// Token: 0x0400959C RID: 38300
		public ChallengeObjectiveCraft CraftObj;
	}
}

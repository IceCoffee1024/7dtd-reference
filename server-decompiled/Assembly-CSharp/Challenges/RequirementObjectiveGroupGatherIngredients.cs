using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001977 RID: 6519
	[Preserve]
	public class RequirementObjectiveGroupGatherIngredients : BaseRequirementObjectiveGroup
	{
		// Token: 0x0600C8F4 RID: 51444 RVA: 0x00497848 File Offset: 0x00495A48
		public RequirementObjectiveGroupGatherIngredients(string itemID)
		{
			this.ItemID = itemID;
			this.itemRecipe = CraftingManager.GetRecipe(itemID);
		}

		// Token: 0x0600C8F5 RID: 51445 RVA: 0x00497870 File Offset: 0x00495A70
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

		// Token: 0x0600C8F6 RID: 51446 RVA: 0x00020119 File Offset: 0x0001E319
		public override bool HasPrerequisiteCondition()
		{
			return true;
		}

		// Token: 0x0600C8F7 RID: 51447 RVA: 0x004979F8 File Offset: 0x00495BF8
		public override BaseRequirementObjectiveGroup Clone()
		{
			return new RequirementObjectiveGroupGatherIngredients(this.ItemID);
		}

		// Token: 0x04009749 RID: 38729
		public string ItemID = "";

		// Token: 0x0400974A RID: 38730
		[PublicizedFrom(EAccessModifier.Private)]
		public Recipe itemRecipe;

		// Token: 0x0400974B RID: 38731
		public ChallengeObjectiveCraft CraftObj;
	}
}

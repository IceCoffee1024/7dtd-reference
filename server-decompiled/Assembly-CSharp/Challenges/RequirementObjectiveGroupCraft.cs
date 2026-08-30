using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001919 RID: 6425
	[Preserve]
	public class RequirementObjectiveGroupCraft : BaseRequirementObjectiveGroup
	{
		// Token: 0x0600C684 RID: 50820 RVA: 0x0048F779 File Offset: 0x0048D979
		public RequirementObjectiveGroupCraft(string itemID)
		{
			this.ItemID = itemID;
			this.ItemRecipe = CraftingManager.GetRecipe(itemID);
		}

		// Token: 0x0600C685 RID: 50821 RVA: 0x0048F7A0 File Offset: 0x0048D9A0
		public override void CreateRequirements()
		{
			if (this.PhaseList == null)
			{
				this.PhaseList = new List<RequirementGroupPhase>();
			}
			RequirementGroupPhase requirementGroupPhase = new RequirementGroupPhase();
			ChallengeObjectiveCraft challengeObjectiveCraft = new ChallengeObjectiveCraft();
			challengeObjectiveCraft.Owner = this.Owner;
			challengeObjectiveCraft.SetupItem(this.ItemID);
			challengeObjectiveCraft.IsRequirement = true;
			challengeObjectiveCraft.MaxCount = 1;
			challengeObjectiveCraft.Init();
			requirementGroupPhase.AddChallengeObjective(challengeObjectiveCraft);
			this.PhaseList.Add(requirementGroupPhase);
		}

		// Token: 0x0600C686 RID: 50822 RVA: 0x0048F80C File Offset: 0x0048DA0C
		public override bool HasPrerequisiteCondition()
		{
			EntityPlayerLocal player = this.Owner.Owner.Player;
			LocalPlayerUI.GetUIForPlayer(this.Owner.Owner.Player);
			XUiM_PlayerInventory playerInventory = LocalPlayerUI.GetUIForPlayer(player).xui.PlayerInventory;
			int num = (this.ItemRecipe.GetOutputItemClass().HasQuality ? 1 : 0);
			for (int i = 0; i < this.ItemRecipe.ingredients.Count; i++)
			{
				ItemStack itemStack = this.ItemRecipe.ingredients[i].Clone();
				if (this.ItemRecipe.UseIngredientModifier)
				{
					itemStack.count = (int)EffectManager.GetValue(PassiveEffects.CraftingIngredientCount, null, (float)itemStack.count, this.Owner.Owner.Player, this.ItemRecipe, FastTags<TagGroup.Global>.Parse(itemStack.itemValue.ItemClass.GetItemName()), true, true, true, true, true, num, true, false);
					if (itemStack.count > 0)
					{
						itemStack.count = (int)((float)itemStack.count * XUiM_Recipes.GetCraftingInputModifier(this.ItemRecipe));
						if (XUiM_Recipes.CraftingInputModifier > 0f)
						{
							itemStack.count = Utils.FastMax(1, itemStack.count);
						}
					}
				}
				if (itemStack.count != 0 && !playerInventory.HasItem(itemStack))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600C687 RID: 50823 RVA: 0x0048F94F File Offset: 0x0048DB4F
		public override BaseRequirementObjectiveGroup Clone()
		{
			return new RequirementObjectiveGroupCraft(this.ItemID)
			{
				ItemRecipe = this.ItemRecipe
			};
		}

		// Token: 0x04009598 RID: 38296
		public string ItemID = "";

		// Token: 0x04009599 RID: 38297
		public Recipe ItemRecipe;
	}
}

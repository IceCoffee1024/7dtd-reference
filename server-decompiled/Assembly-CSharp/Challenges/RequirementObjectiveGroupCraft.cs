using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001945 RID: 6469
	[Preserve]
	public class RequirementObjectiveGroupCraft : BaseRequirementObjectiveGroup
	{
		// Token: 0x0600C7DC RID: 51164 RVA: 0x00493C65 File Offset: 0x00491E65
		public RequirementObjectiveGroupCraft(string itemID)
		{
			this.ItemID = itemID;
			this.ItemRecipe = CraftingManager.GetRecipe(itemID);
		}

		// Token: 0x0600C7DD RID: 51165 RVA: 0x00493C8C File Offset: 0x00491E8C
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

		// Token: 0x0600C7DE RID: 51166 RVA: 0x00493CF8 File Offset: 0x00491EF8
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

		// Token: 0x0600C7DF RID: 51167 RVA: 0x00493E3B File Offset: 0x0049203B
		public override BaseRequirementObjectiveGroup Clone()
		{
			return new RequirementObjectiveGroupCraft(this.ItemID)
			{
				ItemRecipe = this.ItemRecipe
			};
		}

		// Token: 0x0400965E RID: 38494
		public string ItemID = "";

		// Token: 0x0400965F RID: 38495
		public Recipe ItemRecipe;
	}
}

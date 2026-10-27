using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001976 RID: 6518
	[Preserve]
	public class RequirementObjectiveGroupCraft : BaseRequirementObjectiveGroup
	{
		// Token: 0x0600C8F0 RID: 51440 RVA: 0x00497659 File Offset: 0x00495859
		public RequirementObjectiveGroupCraft(string itemID)
		{
			this.ItemID = itemID;
			this.ItemRecipe = CraftingManager.GetRecipe(itemID);
		}

		// Token: 0x0600C8F1 RID: 51441 RVA: 0x00497680 File Offset: 0x00495880
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

		// Token: 0x0600C8F2 RID: 51442 RVA: 0x004976EC File Offset: 0x004958EC
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

		// Token: 0x0600C8F3 RID: 51443 RVA: 0x0049782F File Offset: 0x00495A2F
		public override BaseRequirementObjectiveGroup Clone()
		{
			return new RequirementObjectiveGroupCraft(this.ItemID)
			{
				ItemRecipe = this.ItemRecipe
			};
		}

		// Token: 0x04009747 RID: 38727
		public string ItemID = "";

		// Token: 0x04009748 RID: 38728
		public Recipe ItemRecipe;
	}
}

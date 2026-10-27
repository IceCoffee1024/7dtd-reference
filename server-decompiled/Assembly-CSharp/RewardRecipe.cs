using System;
using UnityEngine.Scripting;

// Token: 0x02000A08 RID: 2568
[Preserve]
public class RewardRecipe : BaseReward
{
	// Token: 0x06004C06 RID: 19462 RVA: 0x001DA16E File Offset: 0x001D836E
	public override void SetupReward()
	{
		base.Description = Localization.Get("RewardRecipe_keyword", false, null);
		base.ValueText = base.ID;
		base.Icon = "ui_game_symbol_hammer";
	}

	// Token: 0x06004C07 RID: 19463 RVA: 0x001DA19C File Offset: 0x001D839C
	public override void GiveReward(EntityPlayer player)
	{
		LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(player as EntityPlayerLocal);
		if (!XUiM_Recipes.GetRecipeIsUnlocked(uiforPlayer.xui, base.ID))
		{
			CraftingManager.UnlockRecipe(base.ID, uiforPlayer.entityPlayer);
		}
	}

	// Token: 0x06004C08 RID: 19464 RVA: 0x001DA1DC File Offset: 0x001D83DC
	public override BaseReward Clone()
	{
		RewardRecipe rewardRecipe = new RewardRecipe();
		base.CopyValues(rewardRecipe);
		return rewardRecipe;
	}

	// Token: 0x06004C09 RID: 19465 RVA: 0x001DA1F7 File Offset: 0x001D83F7
	public override void SetupGlobalRewardSettings()
	{
		CraftingManager.LockRecipe(base.ID, CraftingManager.RecipeLockTypes.Quest);
	}
}

using System;
using UnityEngine.Scripting;

// Token: 0x020009F4 RID: 2548
[Preserve]
public class RewardRecipe : BaseReward
{
	// Token: 0x06004C13 RID: 19475 RVA: 0x001D7DD6 File Offset: 0x001D5FD6
	public override void SetupReward()
	{
		base.Description = Localization.Get("RewardRecipe_keyword", false, null);
		base.ValueText = base.ID;
		base.Icon = "ui_game_symbol_hammer";
	}

	// Token: 0x06004C14 RID: 19476 RVA: 0x001D7E04 File Offset: 0x001D6004
	public override void GiveReward(EntityPlayer player)
	{
		LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(player as EntityPlayerLocal);
		if (!XUiM_Recipes.GetRecipeIsUnlocked(uiforPlayer.xui, base.ID))
		{
			CraftingManager.UnlockRecipe(base.ID, uiforPlayer.entityPlayer);
		}
	}

	// Token: 0x06004C15 RID: 19477 RVA: 0x001D7E44 File Offset: 0x001D6044
	public override BaseReward Clone()
	{
		RewardRecipe rewardRecipe = new RewardRecipe();
		base.CopyValues(rewardRecipe);
		return rewardRecipe;
	}

	// Token: 0x06004C16 RID: 19478 RVA: 0x001D7E5F File Offset: 0x001D605F
	public override void SetupGlobalRewardSettings()
	{
		CraftingManager.LockRecipe(base.ID, CraftingManager.RecipeLockTypes.Quest);
	}
}

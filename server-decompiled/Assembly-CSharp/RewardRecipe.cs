using System;
using UnityEngine.Scripting;

// Token: 0x020009FE RID: 2558
[Preserve]
public class RewardRecipe : BaseReward
{
	// Token: 0x06004BE2 RID: 19426 RVA: 0x001D9086 File Offset: 0x001D7286
	public override void SetupReward()
	{
		base.Description = Localization.Get("RewardRecipe_keyword", false, null);
		base.ValueText = base.ID;
		base.Icon = "ui_game_symbol_hammer";
	}

	// Token: 0x06004BE3 RID: 19427 RVA: 0x001D90B4 File Offset: 0x001D72B4
	public override void GiveReward(EntityPlayer player)
	{
		LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(player as EntityPlayerLocal);
		if (!XUiM_Recipes.GetRecipeIsUnlocked(uiforPlayer.xui, base.ID))
		{
			CraftingManager.UnlockRecipe(base.ID, uiforPlayer.entityPlayer);
		}
	}

	// Token: 0x06004BE4 RID: 19428 RVA: 0x001D90F4 File Offset: 0x001D72F4
	public override BaseReward Clone()
	{
		RewardRecipe rewardRecipe = new RewardRecipe();
		base.CopyValues(rewardRecipe);
		return rewardRecipe;
	}

	// Token: 0x06004BE5 RID: 19429 RVA: 0x001D910F File Offset: 0x001D730F
	public override void SetupGlobalRewardSettings()
	{
		CraftingManager.LockRecipe(base.ID, CraftingManager.RecipeLockTypes.Quest);
	}
}

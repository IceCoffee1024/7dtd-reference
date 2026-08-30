using System;
using UnityEngine.Scripting;

// Token: 0x020009E3 RID: 2531
[Preserve]
public class RewardRecipe : BaseReward
{
	// Token: 0x06004B8D RID: 19341 RVA: 0x001D55EE File Offset: 0x001D37EE
	public override void SetupReward()
	{
		base.Description = Localization.Get("RewardRecipe_keyword", false, null);
		base.ValueText = base.ID;
		base.Icon = "ui_game_symbol_hammer";
	}

	// Token: 0x06004B8E RID: 19342 RVA: 0x001D561C File Offset: 0x001D381C
	public override void GiveReward(EntityPlayer player)
	{
		LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(player as EntityPlayerLocal);
		if (!XUiM_Recipes.GetRecipeIsUnlocked(uiforPlayer.xui, base.ID))
		{
			CraftingManager.UnlockRecipe(base.ID, uiforPlayer.entityPlayer);
		}
	}

	// Token: 0x06004B8F RID: 19343 RVA: 0x001D565C File Offset: 0x001D385C
	public override BaseReward Clone()
	{
		RewardRecipe rewardRecipe = new RewardRecipe();
		base.CopyValues(rewardRecipe);
		return rewardRecipe;
	}

	// Token: 0x06004B90 RID: 19344 RVA: 0x001D5677 File Offset: 0x001D3877
	public override void SetupGlobalRewardSettings()
	{
		CraftingManager.LockRecipe(base.ID, CraftingManager.RecipeLockTypes.Quest);
	}
}

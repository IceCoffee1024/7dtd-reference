using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000D77 RID: 3447
[Preserve]
public class ItemActionEntryShowPerk : BaseItemActionEntry
{
	// Token: 0x06006839 RID: 26681 RVA: 0x00295BF8 File Offset: 0x00293DF8
	public ItemActionEntryShowPerk(XUiController _controller, RecipeUnlockData _unlockData)
		: base(_controller, "OVERRIDDEN BELOW", "OVERRIDDEN BELOW", BaseItemActionEntry.GamepadShortCut.DPadUp, "crafting/craft_click_craft", "ui/ui_denied")
	{
		this.unlockData = _unlockData;
		switch (_unlockData.UnlockType)
		{
		case RecipeUnlockData.UnlockTypes.Perk:
			base.ActionName = Localization.Get("xuiPerk", false, null);
			base.IconName = "ui_game_symbol_skills";
			return;
		case RecipeUnlockData.UnlockTypes.Book:
			base.ActionName = Localization.Get("xuiBook", false, null);
			base.IconName = "ui_game_symbol_book";
			return;
		case RecipeUnlockData.UnlockTypes.Skill:
			base.ActionName = Localization.Get("RewardSkill_keyword", false, null);
			base.IconName = "ui_game_symbol_hammer";
			return;
		case RecipeUnlockData.UnlockTypes.Schematic:
			base.ActionName = Localization.Get("xuiItem", false, null);
			base.IconName = "ui_game_symbol_hammer";
			return;
		default:
			return;
		}
	}

	// Token: 0x0600683A RID: 26682 RVA: 0x002941E6 File Offset: 0x002923E6
	public override void RefreshEnabled()
	{
		base.Enabled = true;
	}

	// Token: 0x0600683B RID: 26683 RVA: 0x00295CC0 File Offset: 0x00293EC0
	public override void OnActivated()
	{
		XUi xui = base.ItemController.xui;
		xui.playerUI.windowManager.Close(XUiC_LootWindowGroup.ID);
		List<XUiC_SkillList> childrenByType = xui.GetChildrenByType<XUiC_SkillList>();
		XUiC_SkillList xuiC_SkillList = null;
		for (int i = 0; i < childrenByType.Count; i++)
		{
			if (childrenByType[i].WindowGroup != null && childrenByType[i].WindowGroup.isShowing)
			{
				xuiC_SkillList = childrenByType[i];
				break;
			}
		}
		if (xuiC_SkillList == null)
		{
			XUiC_WindowSelector.OpenSelectorAndWindow(xui.playerUI.entityPlayer, "skills");
			xuiC_SkillList = xui.GetChildByType<XUiC_SkillList>();
		}
		if (xuiC_SkillList != null)
		{
			xuiC_SkillList.SetSelectedByUnlockData(this.unlockData);
		}
	}

	// Token: 0x0400504B RID: 20555
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly RecipeUnlockData unlockData;
}

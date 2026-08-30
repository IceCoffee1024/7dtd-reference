using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000D66 RID: 3430
[Preserve]
public class ItemActionEntryShowPerk : BaseItemActionEntry
{
	// Token: 0x06006832 RID: 26674 RVA: 0x00292648 File Offset: 0x00290848
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

	// Token: 0x06006833 RID: 26675 RVA: 0x00290C6A File Offset: 0x0028EE6A
	public override void RefreshEnabled()
	{
		base.Enabled = true;
	}

	// Token: 0x06006834 RID: 26676 RVA: 0x00292710 File Offset: 0x00290910
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

	// Token: 0x0400500C RID: 20492
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly RecipeUnlockData unlockData;
}

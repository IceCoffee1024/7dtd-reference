using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000D53 RID: 3411
[Preserve]
public class ItemActionEntryShowPerk : BaseItemActionEntry
{
	// Token: 0x060067A3 RID: 26531 RVA: 0x0028F6B4 File Offset: 0x0028D8B4
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

	// Token: 0x060067A4 RID: 26532 RVA: 0x0028DCCE File Offset: 0x0028BECE
	public override void RefreshEnabled()
	{
		base.Enabled = true;
	}

	// Token: 0x060067A5 RID: 26533 RVA: 0x0028F77C File Offset: 0x0028D97C
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

	// Token: 0x04004F6E RID: 20334
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly RecipeUnlockData unlockData;
}

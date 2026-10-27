using System;
using UnityEngine.Scripting;

// Token: 0x02000FAE RID: 4014
[Preserve]
public class XUiC_MapSubPopupList : XUiController
{
	// Token: 0x06007E26 RID: 32294 RVA: 0x002FF294 File Offset: 0x002FD494
	public override void Init()
	{
		base.Init();
		for (int i = 0; i < this.children.Count; i++)
		{
			XUiController xuiController = this.children[i].Children[0];
			if (xuiController is XUiC_MapSubPopupEntry)
			{
				XUiC_MapSubPopupEntry xuiC_MapSubPopupEntry = (XUiC_MapSubPopupEntry)xuiController;
				xuiC_MapSubPopupEntry.SetIndex(i);
				xuiC_MapSubPopupEntry.SetSpriteName(XUiC_MapSubPopupList.sprites[i % XUiC_MapSubPopupList.sprites.Length]);
			}
		}
	}

	// Token: 0x06007E27 RID: 32295 RVA: 0x002FF300 File Offset: 0x002FD500
	[PublicizedFrom(EAccessModifier.Internal)]
	public void ResetList()
	{
		for (int i = 0; i < this.children.Count; i++)
		{
			XUiController xuiController = this.children[i].Children[0];
			if (xuiController is XUiC_MapSubPopupEntry)
			{
				((XUiC_MapSubPopupEntry)xuiController).Reset();
			}
		}
		this.children[0].SelectCursorElement(true, false);
	}

	// Token: 0x04005D99 RID: 23961
	[PublicizedFrom(EAccessModifier.Private)]
	public static string[] sprites = new string[]
	{
		"ui_game_symbol_map_cave", "ui_game_symbol_map_cabin", "ui_game_symbol_map_campsite", "ui_game_symbol_map_city", "ui_game_symbol_map_fortress", "ui_game_symbol_map_civil", "ui_game_symbol_map_house", "ui_game_symbol_map_town", "ui_game_symbol_map_trader", "ui_game_symbol_x",
		"ui_game_symbol_book", "ui_game_symbol_coin", "ui_game_symbol_safe", "ui_game_symbol_treasure", "ui_game_symbol_radial_number_1", "ui_game_symbol_radial_number_2", "ui_game_symbol_radial_number_3", "ui_game_symbol_radial_number_4", "ui_game_symbol_radial_number_5", "ui_game_symbol_check"
	};
}

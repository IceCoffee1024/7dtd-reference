using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F99 RID: 3993
[Preserve]
public class XUiC_MapPopupEntry : XUiController
{
	// Token: 0x06007DC0 RID: 32192 RVA: 0x002FCF6C File Offset: 0x002FB16C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnHovered(bool _isOver)
	{
		XUiV_Sprite xuiV_Sprite = (XUiV_Sprite)base.GetChildById("background").ViewComponent;
		if (xuiV_Sprite != null)
		{
			xuiV_Sprite.Color = (_isOver ? new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue) : new Color32(64, 64, 64, byte.MaxValue));
			xuiV_Sprite.SpriteName = (_isOver ? "ui_game_select_row" : "menu_empty");
		}
		base.OnHovered(_isOver);
	}
}

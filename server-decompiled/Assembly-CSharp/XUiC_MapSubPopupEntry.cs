using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FAD RID: 4013
[Preserve]
public class XUiC_MapSubPopupEntry : XUiController
{
	// Token: 0x06007E1D RID: 32285 RVA: 0x002FF0E0 File Offset: 0x002FD2E0
	public override void Init()
	{
		base.Init();
		base.OnPress += this.onPressed;
		base.OnVisiblity += this.XUiC_MapSubPopupEntry_OnVisiblity;
	}

	// Token: 0x06007E1E RID: 32286 RVA: 0x002FF10C File Offset: 0x002FD30C
	[PublicizedFrom(EAccessModifier.Private)]
	public void XUiC_MapSubPopupEntry_OnVisiblity(XUiController _sender, bool _visibleSelf, bool _visibleInScene)
	{
		this.select(false);
	}

	// Token: 0x06007E1F RID: 32287 RVA: 0x002FF115 File Offset: 0x002FD315
	public void SetIndex(int _idx)
	{
		this.index = _idx;
	}

	// Token: 0x06007E20 RID: 32288 RVA: 0x002FF120 File Offset: 0x002FD320
	public void SetSpriteName(string _s)
	{
		this.spriteName = _s;
		for (int i = 0; i < base.Parent.Children.Count; i++)
		{
			XUiView viewComponent = base.Parent.Children[i].ViewComponent;
			if (viewComponent.ID.EqualsCaseInsensitive("icon"))
			{
				((XUiV_Sprite)viewComponent).SpriteName = _s;
			}
		}
	}

	// Token: 0x06007E21 RID: 32289 RVA: 0x002FF184 File Offset: 0x002FD384
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnHovered(bool _isOver)
	{
		this.select(_isOver);
	}

	// Token: 0x06007E22 RID: 32290 RVA: 0x002FF190 File Offset: 0x002FD390
	[PublicizedFrom(EAccessModifier.Private)]
	public void onPressed(XUiController _sender, int _mouseButton)
	{
		this.select(true);
		((XUiC_MapArea)this.xui.GetWindow("mapArea").Controller).OnWaypointEntryChosen(this.spriteName);
		XUiC_MapEnterWaypoint childByType = this.xui.GetWindow("mapAreaEnterWaypointName").Controller.GetChildByType<XUiC_MapEnterWaypoint>();
		int num = this.index / 10;
		int num2 = this.index % 10;
		Vector2i vector2i = this.xui.GetWindow("mapAreaChooseWaypoint").Position + new Vector2i(52 * (num + 1), num2 * -43);
		childByType.Show(vector2i);
	}

	// Token: 0x06007E23 RID: 32291 RVA: 0x002FF228 File Offset: 0x002FD428
	[PublicizedFrom(EAccessModifier.Private)]
	public void select(bool _bSelect)
	{
		XUiV_Sprite xuiV_Sprite = (XUiV_Sprite)this.viewComponent;
		if (xuiV_Sprite != null)
		{
			xuiV_Sprite.Color = (_bSelect ? new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue) : new Color32(64, 64, 64, byte.MaxValue));
			xuiV_Sprite.SpriteName = (_bSelect ? "ui_game_select_row" : "menu_empty");
		}
	}

	// Token: 0x06007E24 RID: 32292 RVA: 0x002FF10C File Offset: 0x002FD30C
	public void Reset()
	{
		this.select(false);
	}

	// Token: 0x04005D97 RID: 23959
	[PublicizedFrom(EAccessModifier.Private)]
	public int index;

	// Token: 0x04005D98 RID: 23960
	[PublicizedFrom(EAccessModifier.Private)]
	public string spriteName;
}

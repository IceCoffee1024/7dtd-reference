using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F9D RID: 3997
[Preserve]
public class XUiC_MapSubPopupEntry : XUiController
{
	// Token: 0x06007DCE RID: 32206 RVA: 0x002FD5C4 File Offset: 0x002FB7C4
	public override void Init()
	{
		base.Init();
		base.OnPress += this.onPressed;
		base.OnVisiblity += this.XUiC_MapSubPopupEntry_OnVisiblity;
	}

	// Token: 0x06007DCF RID: 32207 RVA: 0x002FD5F0 File Offset: 0x002FB7F0
	[PublicizedFrom(EAccessModifier.Private)]
	public void XUiC_MapSubPopupEntry_OnVisiblity(XUiController _sender, bool _visibleSelf, bool _visibleInScene)
	{
		this.select(false);
	}

	// Token: 0x06007DD0 RID: 32208 RVA: 0x002FD5F9 File Offset: 0x002FB7F9
	public void SetIndex(int _idx)
	{
		this.index = _idx;
	}

	// Token: 0x06007DD1 RID: 32209 RVA: 0x002FD604 File Offset: 0x002FB804
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

	// Token: 0x06007DD2 RID: 32210 RVA: 0x002FD668 File Offset: 0x002FB868
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnHovered(bool _isOver)
	{
		this.select(_isOver);
	}

	// Token: 0x06007DD3 RID: 32211 RVA: 0x002FD674 File Offset: 0x002FB874
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

	// Token: 0x06007DD4 RID: 32212 RVA: 0x002FD70C File Offset: 0x002FB90C
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

	// Token: 0x06007DD5 RID: 32213 RVA: 0x002FD5F0 File Offset: 0x002FB7F0
	public void Reset()
	{
		this.select(false);
	}

	// Token: 0x04005D5C RID: 23900
	[PublicizedFrom(EAccessModifier.Private)]
	public int index;

	// Token: 0x04005D5D RID: 23901
	[PublicizedFrom(EAccessModifier.Private)]
	public string spriteName;
}

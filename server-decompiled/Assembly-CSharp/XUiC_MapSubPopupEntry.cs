using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F75 RID: 3957
[Preserve]
public class XUiC_MapSubPopupEntry : XUiController
{
	// Token: 0x06007CCC RID: 31948 RVA: 0x002F5D4C File Offset: 0x002F3F4C
	public override void Init()
	{
		base.Init();
		base.OnPress += this.onPressed;
		base.OnVisiblity += this.XUiC_MapSubPopupEntry_OnVisiblity;
	}

	// Token: 0x06007CCD RID: 31949 RVA: 0x002F5D78 File Offset: 0x002F3F78
	[PublicizedFrom(EAccessModifier.Private)]
	public void XUiC_MapSubPopupEntry_OnVisiblity(XUiController _sender, bool _visibleSelf, bool _visibleInScene)
	{
		this.select(false);
	}

	// Token: 0x06007CCE RID: 31950 RVA: 0x002F5D81 File Offset: 0x002F3F81
	public void SetIndex(int _idx)
	{
		this.index = _idx;
	}

	// Token: 0x06007CCF RID: 31951 RVA: 0x002F5D8C File Offset: 0x002F3F8C
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

	// Token: 0x06007CD0 RID: 31952 RVA: 0x002F5DF0 File Offset: 0x002F3FF0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnHovered(bool _isOver)
	{
		this.select(_isOver);
	}

	// Token: 0x06007CD1 RID: 31953 RVA: 0x002F5DFC File Offset: 0x002F3FFC
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

	// Token: 0x06007CD2 RID: 31954 RVA: 0x002F5E94 File Offset: 0x002F4094
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

	// Token: 0x06007CD3 RID: 31955 RVA: 0x002F5D78 File Offset: 0x002F3F78
	public void Reset()
	{
		this.select(false);
	}

	// Token: 0x04005C6C RID: 23660
	[PublicizedFrom(EAccessModifier.Private)]
	public int index;

	// Token: 0x04005C6D RID: 23661
	[PublicizedFrom(EAccessModifier.Private)]
	public string spriteName;
}

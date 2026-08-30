using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F8E RID: 3982
[Preserve]
public class XUiC_MapSubPopupEntry : XUiController
{
	// Token: 0x06007DBE RID: 32190 RVA: 0x002FA520 File Offset: 0x002F8720
	public override void Init()
	{
		base.Init();
		base.OnPress += this.onPressed;
		base.OnVisiblity += this.XUiC_MapSubPopupEntry_OnVisiblity;
	}

	// Token: 0x06007DBF RID: 32191 RVA: 0x002FA54C File Offset: 0x002F874C
	[PublicizedFrom(EAccessModifier.Private)]
	public void XUiC_MapSubPopupEntry_OnVisiblity(XUiController _sender, bool _visibleSelf, bool _visibleInScene)
	{
		this.select(false);
	}

	// Token: 0x06007DC0 RID: 32192 RVA: 0x002FA555 File Offset: 0x002F8755
	public void SetIndex(int _idx)
	{
		this.index = _idx;
	}

	// Token: 0x06007DC1 RID: 32193 RVA: 0x002FA560 File Offset: 0x002F8760
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

	// Token: 0x06007DC2 RID: 32194 RVA: 0x002FA5C4 File Offset: 0x002F87C4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnHovered(bool _isOver)
	{
		this.select(_isOver);
	}

	// Token: 0x06007DC3 RID: 32195 RVA: 0x002FA5D0 File Offset: 0x002F87D0
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

	// Token: 0x06007DC4 RID: 32196 RVA: 0x002FA668 File Offset: 0x002F8868
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

	// Token: 0x06007DC5 RID: 32197 RVA: 0x002FA54C File Offset: 0x002F874C
	public void Reset()
	{
		this.select(false);
	}

	// Token: 0x04005D33 RID: 23859
	[PublicizedFrom(EAccessModifier.Private)]
	public int index;

	// Token: 0x04005D34 RID: 23860
	[PublicizedFrom(EAccessModifier.Private)]
	public string spriteName;
}

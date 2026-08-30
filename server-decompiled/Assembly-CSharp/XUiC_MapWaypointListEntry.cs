using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F94 RID: 3988
[Preserve]
public class XUiC_MapWaypointListEntry : XUiController
{
	// Token: 0x17000EB5 RID: 3765
	// (get) Token: 0x06007DE8 RID: 32232 RVA: 0x002FB497 File Offset: 0x002F9697
	// (set) Token: 0x06007DE9 RID: 32233 RVA: 0x002FB4A0 File Offset: 0x002F96A0
	public Waypoint Waypoint
	{
		get
		{
			return this._waypoint;
		}
		set
		{
			this._waypoint = value;
			this.Background.IsNavigatable = (this.Background.IsSnappable = this._waypoint != null);
		}
	}

	// Token: 0x17000EB6 RID: 3766
	// (get) Token: 0x06007DEA RID: 32234 RVA: 0x002FB4D6 File Offset: 0x002F96D6
	// (set) Token: 0x06007DEB RID: 32235 RVA: 0x002FB4DE File Offset: 0x002F96DE
	public new bool Selected
	{
		get
		{
			return this.m_bSelected;
		}
		set
		{
			this.m_bSelected = value;
			this.updateSelected(false);
		}
	}

	// Token: 0x06007DEC RID: 32236 RVA: 0x002FB4F0 File Offset: 0x002F96F0
	public override void Init()
	{
		base.Init();
		this.waypointList = (XUiC_MapWaypointList)base.Parent.GetChildById("waypointList");
		this.Background = (XUiV_Sprite)base.GetChildById("Background").ViewComponent;
		this.Sprite = (XUiV_Sprite)base.GetChildById("Icon").ViewComponent;
		this.Tracking = (XUiV_Sprite)base.GetChildById("Tracking").ViewComponent;
		this.Name = (XUiV_Label)base.GetChildById("Name").ViewComponent;
		this.Distance = (XUiV_Label)base.GetChildById("Distance").ViewComponent;
		this.Background.Controller.OnHover += this.Controller_OnHover;
		this.Background.Controller.OnPress += this.Controller_OnPress;
		this.Background.Controller.OnScroll += this.Controller_OnScroll;
		this.Background.IsNavigatable = (this.Background.IsSnappable = false);
	}

	// Token: 0x06007DED RID: 32237 RVA: 0x002FB613 File Offset: 0x002F9813
	[PublicizedFrom(EAccessModifier.Private)]
	public void Controller_OnScroll(XUiController _sender, float _delta)
	{
		if (_delta > 0f)
		{
			XUiC_Paging pager = this.waypointList.pager;
			if (pager == null)
			{
				return;
			}
			pager.PageDown();
			return;
		}
		else
		{
			XUiC_Paging pager2 = this.waypointList.pager;
			if (pager2 == null)
			{
				return;
			}
			pager2.PageUp();
			return;
		}
	}

	// Token: 0x06007DEE RID: 32238 RVA: 0x002FB64A File Offset: 0x002F984A
	[PublicizedFrom(EAccessModifier.Private)]
	public void Controller_OnHover(XUiController _sender, bool _isOver)
	{
		this.updateSelected(this.Waypoint != null && _isOver);
	}

	// Token: 0x06007DEF RID: 32239 RVA: 0x002FB660 File Offset: 0x002F9860
	[PublicizedFrom(EAccessModifier.Private)]
	public void Controller_OnPress(XUiController _sender, int _mouseButton)
	{
		if (this.Waypoint == null)
		{
			this.Selected = false;
			return;
		}
		for (int i = 0; i < base.Parent.Children.Count; i++)
		{
			if (base.Parent.Children[i] is XUiC_MapWaypointListEntry)
			{
				((XUiC_MapWaypointListEntry)base.Parent.Children[i]).Selected = false;
			}
		}
		this.waypointList.SelectedWaypoint = this.Waypoint;
		this.waypointList.SelectedWaypointEntry = this;
		this.Selected = true;
		if (InputUtils.ShiftKeyPressed && this.Waypoint != null)
		{
			this.waypointList.TrackedWaypoint = this.Waypoint;
			this.Waypoint.hiddenOnCompass = false;
			this.Waypoint.navObject.hiddenOnCompass = false;
			this.waypointList.UpdateWaypointsList(this.waypointList.SelectedWaypointEntry.Waypoint);
		}
	}

	// Token: 0x06007DF0 RID: 32240 RVA: 0x002FB748 File Offset: 0x002F9948
	public override void OnOpen()
	{
		base.OnOpen();
		this.Tracking.IsVisible = this.Waypoint != null && this.Waypoint.bTracked;
	}

	// Token: 0x06007DF1 RID: 32241 RVA: 0x002FB774 File Offset: 0x002F9974
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateSelected(bool _bHover)
	{
		XUiV_Sprite background = this.Background;
		if (background != null)
		{
			if (this.m_bSelected)
			{
				background.Color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
				background.SpriteName = "ui_game_select_row";
			}
			else if (_bHover)
			{
				background.Color = new Color32(96, 96, 96, byte.MaxValue);
				background.SpriteName = "menu_empty";
			}
			else
			{
				background.Color = new Color32(64, 64, 64, byte.MaxValue);
				background.SpriteName = "menu_empty";
			}
		}
		this.Tracking.IsVisible = this.Waypoint != null && this.Waypoint.bTracked;
	}

	// Token: 0x04005D47 RID: 23879
	public int Index;

	// Token: 0x04005D48 RID: 23880
	public XUiV_Sprite Background;

	// Token: 0x04005D49 RID: 23881
	public XUiV_Sprite Sprite;

	// Token: 0x04005D4A RID: 23882
	public XUiV_Label Name;

	// Token: 0x04005D4B RID: 23883
	public XUiV_Label Distance;

	// Token: 0x04005D4C RID: 23884
	public XUiV_Sprite Tracking;

	// Token: 0x04005D4D RID: 23885
	[PublicizedFrom(EAccessModifier.Private)]
	public Waypoint _waypoint;

	// Token: 0x04005D4E RID: 23886
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_bSelected;

	// Token: 0x04005D4F RID: 23887
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_MapWaypointList waypointList;
}

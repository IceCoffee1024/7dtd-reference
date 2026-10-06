using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FA3 RID: 4003
[Preserve]
public class XUiC_MapWaypointListEntry : XUiController
{
	// Token: 0x17000EDC RID: 3804
	// (get) Token: 0x06007DF8 RID: 32248 RVA: 0x002FE53B File Offset: 0x002FC73B
	// (set) Token: 0x06007DF9 RID: 32249 RVA: 0x002FE544 File Offset: 0x002FC744
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

	// Token: 0x17000EDD RID: 3805
	// (get) Token: 0x06007DFA RID: 32250 RVA: 0x002FE57A File Offset: 0x002FC77A
	// (set) Token: 0x06007DFB RID: 32251 RVA: 0x002FE582 File Offset: 0x002FC782
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

	// Token: 0x06007DFC RID: 32252 RVA: 0x002FE594 File Offset: 0x002FC794
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

	// Token: 0x06007DFD RID: 32253 RVA: 0x002FE6B7 File Offset: 0x002FC8B7
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

	// Token: 0x06007DFE RID: 32254 RVA: 0x002FE6EE File Offset: 0x002FC8EE
	[PublicizedFrom(EAccessModifier.Private)]
	public void Controller_OnHover(XUiController _sender, bool _isOver)
	{
		this.updateSelected(this.Waypoint != null && _isOver);
	}

	// Token: 0x06007DFF RID: 32255 RVA: 0x002FE704 File Offset: 0x002FC904
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

	// Token: 0x06007E00 RID: 32256 RVA: 0x002FE7EC File Offset: 0x002FC9EC
	public override void OnOpen()
	{
		base.OnOpen();
		this.Tracking.IsVisible = this.Waypoint != null && this.Waypoint.bTracked;
	}

	// Token: 0x06007E01 RID: 32257 RVA: 0x002FE818 File Offset: 0x002FCA18
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

	// Token: 0x04005D70 RID: 23920
	public int Index;

	// Token: 0x04005D71 RID: 23921
	public XUiV_Sprite Background;

	// Token: 0x04005D72 RID: 23922
	public XUiV_Sprite Sprite;

	// Token: 0x04005D73 RID: 23923
	public XUiV_Label Name;

	// Token: 0x04005D74 RID: 23924
	public XUiV_Label Distance;

	// Token: 0x04005D75 RID: 23925
	public XUiV_Sprite Tracking;

	// Token: 0x04005D76 RID: 23926
	[PublicizedFrom(EAccessModifier.Private)]
	public Waypoint _waypoint;

	// Token: 0x04005D77 RID: 23927
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_bSelected;

	// Token: 0x04005D78 RID: 23928
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_MapWaypointList waypointList;
}

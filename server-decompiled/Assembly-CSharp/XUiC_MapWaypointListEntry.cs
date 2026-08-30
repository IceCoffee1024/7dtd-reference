using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F7B RID: 3963
[Preserve]
public class XUiC_MapWaypointListEntry : XUiController
{
	// Token: 0x17000E7C RID: 3708
	// (get) Token: 0x06007CF6 RID: 31990 RVA: 0x002F6CC3 File Offset: 0x002F4EC3
	// (set) Token: 0x06007CF7 RID: 31991 RVA: 0x002F6CCC File Offset: 0x002F4ECC
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

	// Token: 0x17000E7D RID: 3709
	// (get) Token: 0x06007CF8 RID: 31992 RVA: 0x002F6D02 File Offset: 0x002F4F02
	// (set) Token: 0x06007CF9 RID: 31993 RVA: 0x002F6D0A File Offset: 0x002F4F0A
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

	// Token: 0x06007CFA RID: 31994 RVA: 0x002F6D1C File Offset: 0x002F4F1C
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

	// Token: 0x06007CFB RID: 31995 RVA: 0x002F6E3F File Offset: 0x002F503F
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

	// Token: 0x06007CFC RID: 31996 RVA: 0x002F6E76 File Offset: 0x002F5076
	[PublicizedFrom(EAccessModifier.Private)]
	public void Controller_OnHover(XUiController _sender, bool _isOver)
	{
		this.updateSelected(this.Waypoint != null && _isOver);
	}

	// Token: 0x06007CFD RID: 31997 RVA: 0x002F6E8C File Offset: 0x002F508C
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

	// Token: 0x06007CFE RID: 31998 RVA: 0x002F6F74 File Offset: 0x002F5174
	public override void OnOpen()
	{
		base.OnOpen();
		this.Tracking.IsVisible = this.Waypoint != null && this.Waypoint.bTracked;
	}

	// Token: 0x06007CFF RID: 31999 RVA: 0x002F6FA0 File Offset: 0x002F51A0
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

	// Token: 0x04005C80 RID: 23680
	public int Index;

	// Token: 0x04005C81 RID: 23681
	public XUiV_Sprite Background;

	// Token: 0x04005C82 RID: 23682
	public XUiV_Sprite Sprite;

	// Token: 0x04005C83 RID: 23683
	public XUiV_Label Name;

	// Token: 0x04005C84 RID: 23684
	public XUiV_Label Distance;

	// Token: 0x04005C85 RID: 23685
	public XUiV_Sprite Tracking;

	// Token: 0x04005C86 RID: 23686
	[PublicizedFrom(EAccessModifier.Private)]
	public Waypoint _waypoint;

	// Token: 0x04005C87 RID: 23687
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_bSelected;

	// Token: 0x04005C88 RID: 23688
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_MapWaypointList waypointList;
}

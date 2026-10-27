using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FB3 RID: 4019
[Preserve]
public class XUiC_MapWaypointListEntry : XUiController
{
	// Token: 0x17000EE7 RID: 3815
	// (get) Token: 0x06007E47 RID: 32327 RVA: 0x00300057 File Offset: 0x002FE257
	// (set) Token: 0x06007E48 RID: 32328 RVA: 0x00300060 File Offset: 0x002FE260
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

	// Token: 0x17000EE8 RID: 3816
	// (get) Token: 0x06007E49 RID: 32329 RVA: 0x00300096 File Offset: 0x002FE296
	// (set) Token: 0x06007E4A RID: 32330 RVA: 0x0030009E File Offset: 0x002FE29E
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

	// Token: 0x06007E4B RID: 32331 RVA: 0x003000B0 File Offset: 0x002FE2B0
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

	// Token: 0x06007E4C RID: 32332 RVA: 0x003001D3 File Offset: 0x002FE3D3
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

	// Token: 0x06007E4D RID: 32333 RVA: 0x0030020A File Offset: 0x002FE40A
	[PublicizedFrom(EAccessModifier.Private)]
	public void Controller_OnHover(XUiController _sender, bool _isOver)
	{
		this.updateSelected(this.Waypoint != null && _isOver);
	}

	// Token: 0x06007E4E RID: 32334 RVA: 0x00300220 File Offset: 0x002FE420
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

	// Token: 0x06007E4F RID: 32335 RVA: 0x00300308 File Offset: 0x002FE508
	public override void OnOpen()
	{
		base.OnOpen();
		this.Tracking.IsVisible = this.Waypoint != null && this.Waypoint.bTracked;
	}

	// Token: 0x06007E50 RID: 32336 RVA: 0x00300334 File Offset: 0x002FE534
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

	// Token: 0x04005DAB RID: 23979
	public int Index;

	// Token: 0x04005DAC RID: 23980
	public XUiV_Sprite Background;

	// Token: 0x04005DAD RID: 23981
	public XUiV_Sprite Sprite;

	// Token: 0x04005DAE RID: 23982
	public XUiV_Label Name;

	// Token: 0x04005DAF RID: 23983
	public XUiV_Label Distance;

	// Token: 0x04005DB0 RID: 23984
	public XUiV_Sprite Tracking;

	// Token: 0x04005DB1 RID: 23985
	[PublicizedFrom(EAccessModifier.Private)]
	public Waypoint _waypoint;

	// Token: 0x04005DB2 RID: 23986
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_bSelected;

	// Token: 0x04005DB3 RID: 23987
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_MapWaypointList waypointList;
}

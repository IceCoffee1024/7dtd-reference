using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F89 RID: 3977
[Preserve]
public class XUiC_MapInvitesListEntry : XUiController
{
	// Token: 0x17000EB1 RID: 3761
	// (get) Token: 0x06007DA6 RID: 32166 RVA: 0x002F9B33 File Offset: 0x002F7D33
	// (set) Token: 0x06007DA7 RID: 32167 RVA: 0x002F9B3C File Offset: 0x002F7D3C
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

	// Token: 0x17000EB2 RID: 3762
	// (get) Token: 0x06007DA8 RID: 32168 RVA: 0x002F9B72 File Offset: 0x002F7D72
	// (set) Token: 0x06007DA9 RID: 32169 RVA: 0x002F9B7C File Offset: 0x002F7D7C
	public new bool Selected
	{
		get
		{
			return this.m_bSelected;
		}
		set
		{
			if (this.m_bSelected && !value && this.Waypoint != null && this.Waypoint.navObject != null && this.Waypoint.navObject.NavObjectClass.NavObjectClassName == "waypoint_invite")
			{
				NavObjectManager.Instance.UnRegisterNavObject(this.Waypoint.navObject);
			}
			if (!this.m_bSelected && value)
			{
				this.Waypoint.navObject = NavObjectManager.Instance.RegisterNavObject("waypoint_invite", this.Waypoint.pos.ToVector3(), this.Waypoint.icon, false, -1, null);
				this.Waypoint.navObject.IsActive = false;
				this.Waypoint.navObject.name = GeneratedTextManager.GetDisplayTextImmediately(this.Waypoint.name, false, GeneratedTextManager.TextFilteringMode.Filter, GeneratedTextManager.BbCodeSupportMode.SupportedAndAddEscapes);
				this.Waypoint.navObject.usingLocalizationId = this.Waypoint.bUsingLocalizationId;
			}
			this.m_bSelected = value;
			this.updateSelected(false);
		}
	}

	// Token: 0x06007DAA RID: 32170 RVA: 0x002F9C84 File Offset: 0x002F7E84
	[PublicizedFrom(EAccessModifier.Private)]
	public int makeKey()
	{
		return int.MaxValue - this.Index;
	}

	// Token: 0x06007DAB RID: 32171 RVA: 0x002F9C94 File Offset: 0x002F7E94
	public override void Init()
	{
		base.Init();
		this.waypointList = (XUiC_MapInvitesList)base.Parent.GetChildById("invitesList");
		this.Background = (XUiV_Sprite)base.GetChildById("Background").ViewComponent;
		this.Sprite = (XUiV_Sprite)base.GetChildById("Icon").ViewComponent;
		this.Name = (XUiV_Label)base.GetChildById("Name").ViewComponent;
		this.Distance = (XUiV_Label)base.GetChildById("Distance").ViewComponent;
		this.Background.Controller.OnHover += this.Controller_OnHover;
		this.Background.Controller.OnPress += this.Controller_OnPress;
		this.Background.IsNavigatable = (this.Background.IsSnappable = false);
	}

	// Token: 0x06007DAC RID: 32172 RVA: 0x002F9D80 File Offset: 0x002F7F80
	[PublicizedFrom(EAccessModifier.Private)]
	public void Controller_OnHover(XUiController _sender, bool _isOver)
	{
		this.updateSelected(this.Waypoint != null && _isOver);
	}

	// Token: 0x06007DAD RID: 32173 RVA: 0x002F9D94 File Offset: 0x002F7F94
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
			if (base.Parent.Children[i] is XUiC_MapInvitesListEntry)
			{
				((XUiC_MapInvitesListEntry)base.Parent.Children[i]).Selected = false;
			}
		}
		this.waypointList.SelectedInvite = this.Waypoint;
		this.waypointList.SelectedInviteEntry = this;
		this.Selected = true;
	}

	// Token: 0x06007DAE RID: 32174 RVA: 0x002F9E24 File Offset: 0x002F8024
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
				return;
			}
			if (_bHover)
			{
				background.Color = new Color32(96, 96, 96, byte.MaxValue);
				background.SpriteName = "menu_empty";
				return;
			}
			background.Color = new Color32(64, 64, 64, byte.MaxValue);
			background.SpriteName = "menu_empty";
		}
	}

	// Token: 0x04005D22 RID: 23842
	public int Index;

	// Token: 0x04005D23 RID: 23843
	public XUiV_Sprite Background;

	// Token: 0x04005D24 RID: 23844
	public XUiV_Sprite Sprite;

	// Token: 0x04005D25 RID: 23845
	public XUiV_Label Name;

	// Token: 0x04005D26 RID: 23846
	public XUiV_Label Distance;

	// Token: 0x04005D27 RID: 23847
	public Action RefreshNameAction;

	// Token: 0x04005D28 RID: 23848
	[PublicizedFrom(EAccessModifier.Private)]
	public Waypoint _waypoint;

	// Token: 0x04005D29 RID: 23849
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_bSelected;

	// Token: 0x04005D2A RID: 23850
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_MapInvitesList waypointList;
}

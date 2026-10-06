using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F98 RID: 3992
[Preserve]
public class XUiC_MapInvitesListEntry : XUiController
{
	// Token: 0x17000ED8 RID: 3800
	// (get) Token: 0x06007DB6 RID: 32182 RVA: 0x002FCBD7 File Offset: 0x002FADD7
	// (set) Token: 0x06007DB7 RID: 32183 RVA: 0x002FCBE0 File Offset: 0x002FADE0
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

	// Token: 0x17000ED9 RID: 3801
	// (get) Token: 0x06007DB8 RID: 32184 RVA: 0x002FCC16 File Offset: 0x002FAE16
	// (set) Token: 0x06007DB9 RID: 32185 RVA: 0x002FCC20 File Offset: 0x002FAE20
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

	// Token: 0x06007DBA RID: 32186 RVA: 0x002FCD28 File Offset: 0x002FAF28
	[PublicizedFrom(EAccessModifier.Private)]
	public int makeKey()
	{
		return int.MaxValue - this.Index;
	}

	// Token: 0x06007DBB RID: 32187 RVA: 0x002FCD38 File Offset: 0x002FAF38
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

	// Token: 0x06007DBC RID: 32188 RVA: 0x002FCE24 File Offset: 0x002FB024
	[PublicizedFrom(EAccessModifier.Private)]
	public void Controller_OnHover(XUiController _sender, bool _isOver)
	{
		this.updateSelected(this.Waypoint != null && _isOver);
	}

	// Token: 0x06007DBD RID: 32189 RVA: 0x002FCE38 File Offset: 0x002FB038
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

	// Token: 0x06007DBE RID: 32190 RVA: 0x002FCEC8 File Offset: 0x002FB0C8
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

	// Token: 0x04005D4B RID: 23883
	public int Index;

	// Token: 0x04005D4C RID: 23884
	public XUiV_Sprite Background;

	// Token: 0x04005D4D RID: 23885
	public XUiV_Sprite Sprite;

	// Token: 0x04005D4E RID: 23886
	public XUiV_Label Name;

	// Token: 0x04005D4F RID: 23887
	public XUiV_Label Distance;

	// Token: 0x04005D50 RID: 23888
	public Action RefreshNameAction;

	// Token: 0x04005D51 RID: 23889
	[PublicizedFrom(EAccessModifier.Private)]
	public Waypoint _waypoint;

	// Token: 0x04005D52 RID: 23890
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_bSelected;

	// Token: 0x04005D53 RID: 23891
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_MapInvitesList waypointList;
}

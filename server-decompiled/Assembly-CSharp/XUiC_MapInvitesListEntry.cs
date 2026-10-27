using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FA8 RID: 4008
[Preserve]
public class XUiC_MapInvitesListEntry : XUiController
{
	// Token: 0x17000EE3 RID: 3811
	// (get) Token: 0x06007E05 RID: 32261 RVA: 0x002FE6F3 File Offset: 0x002FC8F3
	// (set) Token: 0x06007E06 RID: 32262 RVA: 0x002FE6FC File Offset: 0x002FC8FC
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

	// Token: 0x17000EE4 RID: 3812
	// (get) Token: 0x06007E07 RID: 32263 RVA: 0x002FE732 File Offset: 0x002FC932
	// (set) Token: 0x06007E08 RID: 32264 RVA: 0x002FE73C File Offset: 0x002FC93C
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

	// Token: 0x06007E09 RID: 32265 RVA: 0x002FE844 File Offset: 0x002FCA44
	[PublicizedFrom(EAccessModifier.Private)]
	public int makeKey()
	{
		return int.MaxValue - this.Index;
	}

	// Token: 0x06007E0A RID: 32266 RVA: 0x002FE854 File Offset: 0x002FCA54
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

	// Token: 0x06007E0B RID: 32267 RVA: 0x002FE940 File Offset: 0x002FCB40
	[PublicizedFrom(EAccessModifier.Private)]
	public void Controller_OnHover(XUiController _sender, bool _isOver)
	{
		this.updateSelected(this.Waypoint != null && _isOver);
	}

	// Token: 0x06007E0C RID: 32268 RVA: 0x002FE954 File Offset: 0x002FCB54
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

	// Token: 0x06007E0D RID: 32269 RVA: 0x002FE9E4 File Offset: 0x002FCBE4
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

	// Token: 0x04005D86 RID: 23942
	public int Index;

	// Token: 0x04005D87 RID: 23943
	public XUiV_Sprite Background;

	// Token: 0x04005D88 RID: 23944
	public XUiV_Sprite Sprite;

	// Token: 0x04005D89 RID: 23945
	public XUiV_Label Name;

	// Token: 0x04005D8A RID: 23946
	public XUiV_Label Distance;

	// Token: 0x04005D8B RID: 23947
	public Action RefreshNameAction;

	// Token: 0x04005D8C RID: 23948
	[PublicizedFrom(EAccessModifier.Private)]
	public Waypoint _waypoint;

	// Token: 0x04005D8D RID: 23949
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_bSelected;

	// Token: 0x04005D8E RID: 23950
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_MapInvitesList waypointList;
}

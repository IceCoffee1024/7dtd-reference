using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F70 RID: 3952
[Preserve]
public class XUiC_MapInvitesListEntry : XUiController
{
	// Token: 0x17000E78 RID: 3704
	// (get) Token: 0x06007CB4 RID: 31924 RVA: 0x002F535F File Offset: 0x002F355F
	// (set) Token: 0x06007CB5 RID: 31925 RVA: 0x002F5368 File Offset: 0x002F3568
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

	// Token: 0x17000E79 RID: 3705
	// (get) Token: 0x06007CB6 RID: 31926 RVA: 0x002F539E File Offset: 0x002F359E
	// (set) Token: 0x06007CB7 RID: 31927 RVA: 0x002F53A8 File Offset: 0x002F35A8
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

	// Token: 0x06007CB8 RID: 31928 RVA: 0x002F54B0 File Offset: 0x002F36B0
	[PublicizedFrom(EAccessModifier.Private)]
	public int makeKey()
	{
		return int.MaxValue - this.Index;
	}

	// Token: 0x06007CB9 RID: 31929 RVA: 0x002F54C0 File Offset: 0x002F36C0
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

	// Token: 0x06007CBA RID: 31930 RVA: 0x002F55AC File Offset: 0x002F37AC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Controller_OnHover(XUiController _sender, bool _isOver)
	{
		this.updateSelected(this.Waypoint != null && _isOver);
	}

	// Token: 0x06007CBB RID: 31931 RVA: 0x002F55C0 File Offset: 0x002F37C0
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

	// Token: 0x06007CBC RID: 31932 RVA: 0x002F5650 File Offset: 0x002F3850
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

	// Token: 0x04005C5B RID: 23643
	public int Index;

	// Token: 0x04005C5C RID: 23644
	public XUiV_Sprite Background;

	// Token: 0x04005C5D RID: 23645
	public XUiV_Sprite Sprite;

	// Token: 0x04005C5E RID: 23646
	public XUiV_Label Name;

	// Token: 0x04005C5F RID: 23647
	public XUiV_Label Distance;

	// Token: 0x04005C60 RID: 23648
	public Action RefreshNameAction;

	// Token: 0x04005C61 RID: 23649
	[PublicizedFrom(EAccessModifier.Private)]
	public Waypoint _waypoint;

	// Token: 0x04005C62 RID: 23650
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_bSelected;

	// Token: 0x04005C63 RID: 23651
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_MapInvitesList waypointList;
}

using System;
using System.Collections.Generic;
using Audio;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F94 RID: 3988
[Preserve]
public class XUiC_MapInvitesList : XUiController
{
	// Token: 0x17000ED7 RID: 3799
	// (get) Token: 0x06007DA5 RID: 32165 RVA: 0x002FC580 File Offset: 0x002FA780
	// (set) Token: 0x06007DA6 RID: 32166 RVA: 0x002FC588 File Offset: 0x002FA788
	public Waypoint SelectedInvite
	{
		get
		{
			return this.selectedInvite;
		}
		set
		{
			this.selectedInvite = value;
		}
	}

	// Token: 0x06007DA7 RID: 32167 RVA: 0x002FC594 File Offset: 0x002FA794
	public override void Init()
	{
		base.Init();
		this.waypointSetBtn = base.Parent.Parent.GetChildById("waypointSetBtn");
		this.waypointSetBtn.OnPress += this.onInviteAddToWaypoints;
		this.waypointShowOnMapBtn = base.Parent.Parent.GetChildById("showOnMapBtn");
		this.waypointShowOnMapBtn.OnPress += this.onInviteShowOnMapPressed;
		this.waypointRemoveBtn = base.Parent.Parent.GetChildById("waypointRemoveBtn");
		this.waypointRemoveBtn.OnPress += this.onInviteRemovePressed;
		this.waypointReportBtn = base.Parent.Parent.GetChildById("waypointReportBtn");
		this.waypointReportBtn.OnPress += this.onReportWaypointPressed;
		this.list = (XUiV_Grid)base.GetChildById("invitesList").ViewComponent;
	}

	// Token: 0x06007DA8 RID: 32168 RVA: 0x002FC68A File Offset: 0x002FA88A
	public override void OnOpen()
	{
		base.OnOpen();
		this.UpdateInvitesList();
	}

	// Token: 0x06007DA9 RID: 32169 RVA: 0x002FC698 File Offset: 0x002FA898
	public void UpdateInvitesList()
	{
		EntityPlayer entityPlayer = this.xui.playerUI.entityPlayer;
		List<Waypoint> waypointInvites = entityPlayer.WaypointInvites;
		int num = 0;
		foreach (XUiController xuiController in this.children)
		{
			XUiC_MapInvitesListEntry uiEntry = xuiController as XUiC_MapInvitesListEntry;
			if (uiEntry != null)
			{
				if (num >= waypointInvites.Count)
				{
					uiEntry.Index = num;
					uiEntry.Sprite.SpriteName = string.Empty;
					uiEntry.Waypoint = null;
					uiEntry.Name.Text = string.Empty;
					uiEntry.Selected = false;
					uiEntry.Background.SoundPlayOnClick = false;
					uiEntry.Distance.Text = string.Empty;
				}
				else
				{
					Waypoint waypoint = waypointInvites[num];
					uiEntry.Index = num;
					uiEntry.Sprite.SpriteName = waypoint.icon;
					uiEntry.Waypoint = waypoint;
					if (waypoint.bIsAutoWaypoint)
					{
						uiEntry.Name.Text = Localization.Get(waypoint.name.Text, false, null);
					}
					else
					{
						GeneratedTextManager.GetDisplayText(waypoint.name, delegate(string _filtered)
						{
							uiEntry.Name.Text = _filtered;
						}, true, false, GeneratedTextManager.TextFilteringMode.FilterWithSafeString, GeneratedTextManager.BbCodeSupportMode.SupportedAndAddEscapes);
					}
					uiEntry.Selected = false;
					uiEntry.Background.SoundPlayOnClick = true;
					Vector3 vector = waypoint.pos.ToVector3();
					Vector3 position = entityPlayer.GetPosition();
					vector.y = 0f;
					position.y = 0f;
					float magnitude = (vector - position).magnitude;
					uiEntry.Distance.Text = ValueDisplayFormatters.Distance(magnitude);
				}
				num++;
			}
		}
	}

	// Token: 0x06007DAA RID: 32170 RVA: 0x002FC8C4 File Offset: 0x002FAAC4
	[PublicizedFrom(EAccessModifier.Private)]
	public void onInviteAddToWaypoints(XUiController _sender, int _mouseButton)
	{
		if (this.SelectedInvite != null)
		{
			EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
			entityPlayer.WaypointInvites.Remove(this.SelectedInvite);
			entityPlayer.Waypoints.Collection.Add(this.SelectedInvite);
			this.SelectedInviteEntry.Selected = false;
			this.SelectedInvite.navObject = NavObjectManager.Instance.RegisterNavObject("waypoint", this.SelectedInvite.pos.ToVector3(), this.SelectedInvite.icon, false, -1, null);
			this.SelectedInvite.navObject.IsActive = false;
			this.SelectedInvite.navObject.usingLocalizationId = this.SelectedInvite.bUsingLocalizationId;
			GeneratedTextManager.GetDisplayText(this.SelectedInvite.name, delegate(string _filtered)
			{
				this.SelectedInvite.navObject.name = _filtered;
			}, true, false, GeneratedTextManager.TextFilteringMode.FilterWithSafeString, GeneratedTextManager.BbCodeSupportMode.SupportedAndAddEscapes);
			XUiV_Window window = this.xui.GetWindow("mapTracking");
			if (window != null && window.IsVisible)
			{
				((XUiC_MapWaypointList)window.Controller.GetChildById("waypointList")).UpdateWaypointsList(null);
			}
			this.SelectedInvite = null;
			this.SelectedInviteEntry = null;
			this.UpdateInvitesList();
		}
	}

	// Token: 0x06007DAB RID: 32171 RVA: 0x002FC9EE File Offset: 0x002FABEE
	[PublicizedFrom(EAccessModifier.Private)]
	public void onInviteShowOnMapPressed(XUiController _sender, int _mouseButton)
	{
		if (this.SelectedInvite != null)
		{
			((XUiC_MapArea)this.xui.GetWindow("mapArea").Controller).PositionMapAt(this.SelectedInvite.pos.ToVector3());
		}
	}

	// Token: 0x06007DAC RID: 32172 RVA: 0x002FCA28 File Offset: 0x002FAC28
	[PublicizedFrom(EAccessModifier.Private)]
	public void onInviteRemovePressed(XUiController _sender, int _mouseButton)
	{
		EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
		if (this.SelectedInvite != null)
		{
			entityPlayer.WaypointInvites.Remove(this.SelectedInvite);
			this.SelectedInviteEntry.Selected = false;
			this.SelectedInvite = null;
			this.SelectedInviteEntry = null;
			this.UpdateInvitesList();
			Manager.PlayInsidePlayerHead("ui_waypoint_delete", -1, 0f, false, false);
			return;
		}
		Manager.PlayInsidePlayerHead("ui_denied", -1, 0f, false, false);
	}

	// Token: 0x06007DAD RID: 32173 RVA: 0x002FCAA8 File Offset: 0x002FACA8
	[PublicizedFrom(EAccessModifier.Private)]
	public void onReportWaypointPressed(XUiController _sender, int _mouseButton)
	{
		EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
		if (this.SelectedInvite != null && !this.SelectedInvite.bIsAutoWaypoint && PlatformManager.MultiPlatform.PlayerReporting != null)
		{
			PersistentPlayerData ppData = GameManager.Instance.persistentPlayers.GetPlayerDataFromEntityID(this.SelectedInvite.InviterEntityId);
			if (ppData != null)
			{
				GeneratedTextManager.GetDisplayText(this.SelectedInvite.name, delegate(string _filtered)
				{
					ThreadManager.AddSingleTaskMainThread("OpenReportWindow", delegate(object _)
					{
						XUiC_ReportPlayer.Open(ppData.PlayerData, EnumReportCategory.VerbalAbuse, string.Format(Localization.Get("xuiReportOffensiveTextMessage", false, null), _filtered));
					}, null);
				}, true, false, GeneratedTextManager.TextFilteringMode.Filter, GeneratedTextManager.BbCodeSupportMode.SupportedAndAddEscapes);
			}
			return;
		}
		Manager.PlayInsidePlayerHead("ui_denied", -1, 0f, false, false);
	}

	// Token: 0x04005D40 RID: 23872
	[PublicizedFrom(EAccessModifier.Private)]
	public Waypoint selectedInvite;

	// Token: 0x04005D41 RID: 23873
	public XUiC_MapInvitesListEntry SelectedInviteEntry;

	// Token: 0x04005D42 RID: 23874
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Grid list;

	// Token: 0x04005D43 RID: 23875
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController waypointSetBtn;

	// Token: 0x04005D44 RID: 23876
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController waypointShowOnMapBtn;

	// Token: 0x04005D45 RID: 23877
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController waypointRemoveBtn;

	// Token: 0x04005D46 RID: 23878
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController waypointReportBtn;
}

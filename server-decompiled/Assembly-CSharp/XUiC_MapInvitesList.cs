using System;
using System.Collections.Generic;
using Audio;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FA4 RID: 4004
[Preserve]
public class XUiC_MapInvitesList : XUiController
{
	// Token: 0x17000EE2 RID: 3810
	// (get) Token: 0x06007DF4 RID: 32244 RVA: 0x002FE09C File Offset: 0x002FC29C
	// (set) Token: 0x06007DF5 RID: 32245 RVA: 0x002FE0A4 File Offset: 0x002FC2A4
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

	// Token: 0x06007DF6 RID: 32246 RVA: 0x002FE0B0 File Offset: 0x002FC2B0
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

	// Token: 0x06007DF7 RID: 32247 RVA: 0x002FE1A6 File Offset: 0x002FC3A6
	public override void OnOpen()
	{
		base.OnOpen();
		this.UpdateInvitesList();
	}

	// Token: 0x06007DF8 RID: 32248 RVA: 0x002FE1B4 File Offset: 0x002FC3B4
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

	// Token: 0x06007DF9 RID: 32249 RVA: 0x002FE3E0 File Offset: 0x002FC5E0
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

	// Token: 0x06007DFA RID: 32250 RVA: 0x002FE50A File Offset: 0x002FC70A
	[PublicizedFrom(EAccessModifier.Private)]
	public void onInviteShowOnMapPressed(XUiController _sender, int _mouseButton)
	{
		if (this.SelectedInvite != null)
		{
			((XUiC_MapArea)this.xui.GetWindow("mapArea").Controller).PositionMapAt(this.SelectedInvite.pos.ToVector3());
		}
	}

	// Token: 0x06007DFB RID: 32251 RVA: 0x002FE544 File Offset: 0x002FC744
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

	// Token: 0x06007DFC RID: 32252 RVA: 0x002FE5C4 File Offset: 0x002FC7C4
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

	// Token: 0x04005D7B RID: 23931
	[PublicizedFrom(EAccessModifier.Private)]
	public Waypoint selectedInvite;

	// Token: 0x04005D7C RID: 23932
	public XUiC_MapInvitesListEntry SelectedInviteEntry;

	// Token: 0x04005D7D RID: 23933
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Grid list;

	// Token: 0x04005D7E RID: 23934
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController waypointSetBtn;

	// Token: 0x04005D7F RID: 23935
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController waypointShowOnMapBtn;

	// Token: 0x04005D80 RID: 23936
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController waypointRemoveBtn;

	// Token: 0x04005D81 RID: 23937
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController waypointReportBtn;
}

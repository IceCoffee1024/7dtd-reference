using System;
using System.Collections.Generic;
using Audio;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F6C RID: 3948
[Preserve]
public class XUiC_MapInvitesList : XUiController
{
	// Token: 0x17000E77 RID: 3703
	// (get) Token: 0x06007CA3 RID: 31907 RVA: 0x002F4D08 File Offset: 0x002F2F08
	// (set) Token: 0x06007CA4 RID: 31908 RVA: 0x002F4D10 File Offset: 0x002F2F10
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

	// Token: 0x06007CA5 RID: 31909 RVA: 0x002F4D1C File Offset: 0x002F2F1C
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

	// Token: 0x06007CA6 RID: 31910 RVA: 0x002F4E12 File Offset: 0x002F3012
	public override void OnOpen()
	{
		base.OnOpen();
		this.UpdateInvitesList();
	}

	// Token: 0x06007CA7 RID: 31911 RVA: 0x002F4E20 File Offset: 0x002F3020
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

	// Token: 0x06007CA8 RID: 31912 RVA: 0x002F504C File Offset: 0x002F324C
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

	// Token: 0x06007CA9 RID: 31913 RVA: 0x002F5176 File Offset: 0x002F3376
	[PublicizedFrom(EAccessModifier.Private)]
	public void onInviteShowOnMapPressed(XUiController _sender, int _mouseButton)
	{
		if (this.SelectedInvite != null)
		{
			((XUiC_MapArea)this.xui.GetWindow("mapArea").Controller).PositionMapAt(this.SelectedInvite.pos.ToVector3());
		}
	}

	// Token: 0x06007CAA RID: 31914 RVA: 0x002F51B0 File Offset: 0x002F33B0
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

	// Token: 0x06007CAB RID: 31915 RVA: 0x002F5230 File Offset: 0x002F3430
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

	// Token: 0x04005C50 RID: 23632
	[PublicizedFrom(EAccessModifier.Private)]
	public Waypoint selectedInvite;

	// Token: 0x04005C51 RID: 23633
	public XUiC_MapInvitesListEntry SelectedInviteEntry;

	// Token: 0x04005C52 RID: 23634
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Grid list;

	// Token: 0x04005C53 RID: 23635
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController waypointSetBtn;

	// Token: 0x04005C54 RID: 23636
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController waypointShowOnMapBtn;

	// Token: 0x04005C55 RID: 23637
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController waypointRemoveBtn;

	// Token: 0x04005C56 RID: 23638
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController waypointReportBtn;
}

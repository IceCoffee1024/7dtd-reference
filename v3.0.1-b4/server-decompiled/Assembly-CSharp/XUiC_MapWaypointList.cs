using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F78 RID: 3960
[Preserve]
public class XUiC_MapWaypointList : XUiController
{
	// Token: 0x17000E7A RID: 3706
	// (get) Token: 0x06007CDC RID: 31964 RVA: 0x002F60B7 File Offset: 0x002F42B7
	// (set) Token: 0x06007CDD RID: 31965 RVA: 0x002F60C0 File Offset: 0x002F42C0
	public Waypoint TrackedWaypoint
	{
		get
		{
			return this.trackedWaypoint;
		}
		set
		{
			if (this.trackedWaypoint != null)
			{
				this.trackedWaypoint.bTracked = false;
				this.trackedWaypoint.navObject.IsActive = this.trackedWaypoint.bTracked;
			}
			this.trackedWaypoint = value;
			if (this.trackedWaypoint != null)
			{
				this.trackedWaypoint.bTracked = true;
				this.trackedWaypoint.navObject.IsActive = this.trackedWaypoint.bTracked;
			}
		}
	}

	// Token: 0x17000E7B RID: 3707
	// (get) Token: 0x06007CDE RID: 31966 RVA: 0x002F6132 File Offset: 0x002F4332
	// (set) Token: 0x06007CDF RID: 31967 RVA: 0x002F613A File Offset: 0x002F433A
	public Waypoint SelectedWaypoint
	{
		get
		{
			return this.selectedWaypoint;
		}
		set
		{
			this.selectedWaypoint = value;
		}
	}

	// Token: 0x06007CE0 RID: 31968 RVA: 0x002F6144 File Offset: 0x002F4344
	public override void Init()
	{
		base.Init();
		this.list = (XUiV_Grid)base.GetChildById("waypointList").ViewComponent;
		this.cCountWaypointsPerPage = this.list.Columns * this.list.Rows;
		this.pager = base.Parent.GetChildByType<XUiC_Paging>();
		if (this.pager != null)
		{
			this.pager.OnPageChanged += delegate
			{
				this.currentPage = this.pager.CurrentPageNumber;
				this.UpdateWaypointsList(null);
				if (this.SelectedWaypointEntry != null)
				{
					this.SelectedWaypointEntry.Selected = false;
				}
			};
		}
		this.trackBtn = base.Parent.GetChildById("trackBtn");
		this.trackBtn.OnPress += this.onTrackWaypointPressed;
		this.showOnMapBtn = base.Parent.GetChildById("showOnMapBtn");
		this.showOnMapBtn.OnPress += this.onShowOnMapPressed;
		this.waypointRemoveBtn = base.Parent.GetChildById("waypointRemoveBtn");
		this.waypointRemoveBtn.OnPress += this.onWaypointRemovePressed;
		this.inviteBtn = base.Parent.GetChildById("inviteBtn");
		this.inviteBtn.OnPress += this.onInvitePressed;
		this.txtInputFilter = (XUiC_TextInput)base.Parent.GetChildById("searchInput");
		this.txtInputFilter.Text = string.Empty;
		this.txtInputFilter.OnChangeHandler += this.waypointFilterOnChangeHandler;
		this.txtInputFilter.OnSubmitHandler += this.waypointFilerOnSubmitHandler;
		this.xui.GetWindow("mapTrackingPopup").Controller.GetChildById("inviteFriends").OnPress += this.onInviteFriendsPressed;
		this.xui.GetWindow("mapTrackingPopup").Controller.GetChildById("inviteEveryone").OnPress += this.onInviteEveryonePressed;
	}

	// Token: 0x06007CE1 RID: 31969 RVA: 0x002F632C File Offset: 0x002F452C
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.SelectedWaypointEntry != null)
		{
			this.SelectedWaypoint = null;
			this.SelectedWaypointEntry.Selected = false;
		}
		this.currentPage = 0;
		this.filterString = this.txtInputFilter.Text;
		this.GetTrackedWaypoint();
		this.UpdateWaypointsList(null);
	}

	// Token: 0x06007CE2 RID: 31970 RVA: 0x002F6380 File Offset: 0x002F4580
	[PublicizedFrom(EAccessModifier.Private)]
	public void GetTrackedWaypoint()
	{
		EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
		bool flag = false;
		for (int i = 0; i < entityPlayer.Waypoints.Collection.list.Count; i++)
		{
			Waypoint waypoint = entityPlayer.Waypoints.Collection.list[i];
			if (waypoint.bTracked)
			{
				if (!flag)
				{
					this.TrackedWaypoint = waypoint;
					flag = true;
				}
				else
				{
					waypoint.bTracked = false;
					waypoint.navObject.IsActive = false;
				}
			}
		}
	}

	// Token: 0x06007CE3 RID: 31971 RVA: 0x002F6400 File Offset: 0x002F4600
	public void UpdateWaypointsList(Waypoint _selectThisWaypoint = null)
	{
		if (this.pager == null)
		{
			this.updateWaypointsNextUpdate = true;
			return;
		}
		for (int i = 0; i < this.cCountWaypointsPerPage; i++)
		{
			XUiC_MapWaypointListEntry xuiC_MapWaypointListEntry = (XUiC_MapWaypointListEntry)this.children[i];
			if (xuiC_MapWaypointListEntry != null)
			{
				xuiC_MapWaypointListEntry.Index = i;
				xuiC_MapWaypointListEntry.Sprite.SpriteName = string.Empty;
				xuiC_MapWaypointListEntry.Name.Text = string.Empty;
				xuiC_MapWaypointListEntry.Distance.Text = string.Empty;
				xuiC_MapWaypointListEntry.Waypoint = null;
				xuiC_MapWaypointListEntry.Selected = false;
				xuiC_MapWaypointListEntry.Background.Enabled = false;
			}
		}
		EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
		List<Waypoint> list = new List<Waypoint>();
		foreach (Waypoint waypoint in entityPlayer.Waypoints.Collection.list)
		{
			if (!waypoint.HiddenOnMap)
			{
				list.Add(waypoint);
			}
		}
		list.Sort(new XUiC_MapWaypointList.WaypointSorter(entityPlayer));
		if (this.txtInputFilter.Text != null && this.txtInputFilter.Text != string.Empty)
		{
			for (int j = 0; j < list.Count; j++)
			{
				if (!list[j].name.Text.ContainsCaseInsensitive(this.txtInputFilter.Text))
				{
					list.RemoveAt(j);
					j--;
					if (j < 0)
					{
						j = 0;
					}
				}
			}
			if (this.filterString != this.txtInputFilter.Text)
			{
				this.currentPage = 0;
				this.filterString = this.txtInputFilter.Text;
			}
		}
		XUiC_Paging xuiC_Paging = this.pager;
		if (xuiC_Paging != null)
		{
			xuiC_Paging.SetLastPageByElementsAndPageLength(list.Count, this.cCountWaypointsPerPage);
		}
		XUiC_Paging xuiC_Paging2 = this.pager;
		if (xuiC_Paging2 != null)
		{
			xuiC_Paging2.SetPage(this.currentPage);
		}
		int num = 0;
		for (int k = 0; k < this.cCountWaypointsPerPage; k++)
		{
			int num2 = k + this.cCountWaypointsPerPage * this.currentPage;
			if (num2 >= list.Count)
			{
				break;
			}
			XUiC_MapWaypointListEntry waypointEntry = (XUiC_MapWaypointListEntry)this.children[num];
			if (waypointEntry != null && (this.txtInputFilter.Text == null || !(this.txtInputFilter.Text != string.Empty) || list[num2].name.Text.ContainsCaseInsensitive(this.txtInputFilter.Text)))
			{
				waypointEntry.Background.Enabled = true;
				waypointEntry.Index = k;
				waypointEntry.Sprite.SpriteName = list[num2].icon;
				waypointEntry.Waypoint = list[num2];
				if (waypointEntry.Waypoint.bIsAutoWaypoint || waypointEntry.Waypoint.bUsingLocalizationId)
				{
					waypointEntry.Name.Text = Localization.Get(waypointEntry.Waypoint.name.Text, false, null);
				}
				else
				{
					GeneratedTextManager.GetDisplayText(waypointEntry.Waypoint.name, delegate(string _filtered)
					{
						waypointEntry.Name.Text = _filtered;
					}, true, false, GeneratedTextManager.TextFilteringMode.FilterWithSafeString, GeneratedTextManager.BbCodeSupportMode.SupportedAndAddEscapes);
				}
				waypointEntry.Selected = _selectThisWaypoint != null && _selectThisWaypoint.Equals(list[num2]);
				Vector3 vector = list[num2].pos.ToVector3();
				Vector3 position = entityPlayer.GetPosition();
				vector.y = 0f;
				position.y = 0f;
				float num3 = (vector - position).magnitude;
				string text = "m";
				if (num3 >= 1000f)
				{
					num3 /= 1000f;
					text = "km";
				}
				waypointEntry.Distance.Text = string.Format("{0} {1}", num3.ToCultureInvariantString("0.0"), text);
				if (_selectThisWaypoint != null && _selectThisWaypoint.Equals(list[num2]))
				{
					this.SelectedWaypointEntry = waypointEntry;
				}
				num++;
			}
		}
	}

	// Token: 0x06007CE4 RID: 31972 RVA: 0x002F6850 File Offset: 0x002F4A50
	public void SelectWaypoint(Waypoint _w)
	{
		EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
		List<Waypoint> list = new List<Waypoint>(entityPlayer.Waypoints.Collection.list);
		list.Sort(new XUiC_MapWaypointList.WaypointSorter(entityPlayer));
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].Equals(_w))
			{
				this.currentPage = i / this.cCountWaypointsPerPage;
				this.UpdateWaypointsList(_w);
				this.SelectedWaypoint = _w;
				return;
			}
		}
	}

	// Token: 0x06007CE5 RID: 31973 RVA: 0x002F68D0 File Offset: 0x002F4AD0
	public void SelectWaypoint(NavObject _nav)
	{
		EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
		List<Waypoint> list = new List<Waypoint>(entityPlayer.Waypoints.Collection.list);
		list.Sort(new XUiC_MapWaypointList.WaypointSorter(entityPlayer));
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].navObject.Equals(_nav))
			{
				this.currentPage = i / this.cCountWaypointsPerPage;
				this.UpdateWaypointsList(list[i]);
				this.SelectedWaypoint = list[i];
				return;
			}
		}
	}

	// Token: 0x06007CE6 RID: 31974 RVA: 0x002F6960 File Offset: 0x002F4B60
	[PublicizedFrom(EAccessModifier.Private)]
	public void onTrackWaypointPressed(XUiController _sender, int _mouseButton)
	{
		Waypoint waypoint = this.GetSelectedWaypoint();
		if (waypoint != null && this.SelectedWaypointEntry != null)
		{
			if (this.TrackedWaypoint == waypoint)
			{
				this.TrackedWaypoint = null;
			}
			else
			{
				this.TrackedWaypoint = waypoint;
				this.trackedWaypoint.hiddenOnCompass = false;
				this.trackedWaypoint.navObject.hiddenOnCompass = false;
			}
			this.UpdateWaypointsList(this.SelectedWaypointEntry.Waypoint);
		}
	}

	// Token: 0x06007CE7 RID: 31975 RVA: 0x002F69C6 File Offset: 0x002F4BC6
	[PublicizedFrom(EAccessModifier.Private)]
	public void onInvitePressed(XUiController _sender, int _mouseButton)
	{
		if (this.selectedWaypoint != null)
		{
			this.xui.GetWindow("mapTrackingPopup").IsVisible = true;
		}
	}

	// Token: 0x06007CE8 RID: 31976 RVA: 0x002F69E8 File Offset: 0x002F4BE8
	[PublicizedFrom(EAccessModifier.Private)]
	public void onShowOnMapPressed(XUiController _sender, int _mouseButton)
	{
		Waypoint waypoint = this.GetSelectedWaypoint();
		if (waypoint != null)
		{
			((XUiC_MapArea)this.xui.GetWindow("mapArea").Controller).PositionMapAt(waypoint.pos.ToVector3());
		}
	}

	// Token: 0x06007CE9 RID: 31977 RVA: 0x002F6A2C File Offset: 0x002F4C2C
	[PublicizedFrom(EAccessModifier.Private)]
	public void onWaypointRemovePressed(XUiController _sender, int _mouseButton)
	{
		EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
		Waypoint waypoint = this.GetSelectedWaypoint();
		if (waypoint != null && (waypoint.lastKnownPositionEntityId == -1 || waypoint.bIsAutoWaypoint))
		{
			entityPlayer.Waypoints.Collection.Remove(waypoint);
			NavObjectManager.Instance.UnRegisterNavObject(waypoint.navObject);
			this.UpdateWaypointsList(null);
			this.SelectedWaypoint = null;
			if (this.SelectedWaypointEntry != null)
			{
				this.SelectedWaypointEntry.Selected = false;
			}
			Manager.PlayInsidePlayerHead("ui_waypoint_delete", -1, 0f, false, false);
			return;
		}
		Manager.PlayInsidePlayerHead("ui_denied", -1, 0f, false, false);
	}

	// Token: 0x06007CEA RID: 31978 RVA: 0x002F6ACD File Offset: 0x002F4CCD
	public Waypoint GetSelectedWaypoint()
	{
		return this.SelectedWaypoint;
	}

	// Token: 0x06007CEB RID: 31979 RVA: 0x002F6AD8 File Offset: 0x002F4CD8
	[PublicizedFrom(EAccessModifier.Private)]
	public void onInviteFriendsPressed(XUiController _sender, int _mouseButton)
	{
		if (this.SelectedWaypoint != null)
		{
			EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
			GameManager.Instance.WaypointInviteServer(this.SelectedWaypoint, EnumWaypointInviteMode.Friends, entityPlayer.entityId);
			this.xui.GetWindow("mapTrackingPopup").IsVisible = false;
			GameManager.ShowTooltip(entityPlayer, string.Format(Localization.Get("tooltipInviteFriends", false, null), this.SelectedWaypoint.navObject.DisplayName), false, false, 0f);
		}
	}

	// Token: 0x06007CEC RID: 31980 RVA: 0x002F6B5C File Offset: 0x002F4D5C
	[PublicizedFrom(EAccessModifier.Private)]
	public void onInviteEveryonePressed(XUiController _sender, int _mouseButton)
	{
		if (this.SelectedWaypoint != null)
		{
			EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
			GameManager.Instance.WaypointInviteServer(this.SelectedWaypoint, EnumWaypointInviteMode.Everyone, entityPlayer.entityId);
			this.xui.GetWindow("mapTrackingPopup").IsVisible = false;
			GameManager.ShowTooltip(entityPlayer, string.Format(Localization.Get("tooltipInviteEveryone", false, null), this.SelectedWaypoint.navObject.DisplayName), false, false, 0f);
		}
	}

	// Token: 0x06007CED RID: 31981 RVA: 0x002F6BDD File Offset: 0x002F4DDD
	[PublicizedFrom(EAccessModifier.Private)]
	public void waypointFilerOnSubmitHandler(XUiController _sender, string _text)
	{
		this.UpdateWaypointsList(null);
	}

	// Token: 0x06007CEE RID: 31982 RVA: 0x002F6BDD File Offset: 0x002F4DDD
	[PublicizedFrom(EAccessModifier.Private)]
	public void waypointFilterOnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.UpdateWaypointsList(null);
	}

	// Token: 0x06007CEF RID: 31983 RVA: 0x002F6BE6 File Offset: 0x002F4DE6
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.updateWaypointsNextUpdate)
		{
			this.updateWaypointsNextUpdate = false;
			this.UpdateWaypointsList(null);
		}
	}

	// Token: 0x04005C70 RID: 23664
	[PublicizedFrom(EAccessModifier.Private)]
	public Waypoint trackedWaypoint;

	// Token: 0x04005C71 RID: 23665
	[PublicizedFrom(EAccessModifier.Private)]
	public Waypoint selectedWaypoint;

	// Token: 0x04005C72 RID: 23666
	public XUiC_MapWaypointListEntry SelectedWaypointEntry;

	// Token: 0x04005C73 RID: 23667
	[PublicizedFrom(EAccessModifier.Private)]
	public int cCountWaypointsPerPage = 8;

	// Token: 0x04005C74 RID: 23668
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Grid list;

	// Token: 0x04005C75 RID: 23669
	public XUiC_Paging pager;

	// Token: 0x04005C76 RID: 23670
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentPage;

	// Token: 0x04005C77 RID: 23671
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtInputFilter;

	// Token: 0x04005C78 RID: 23672
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController trackBtn;

	// Token: 0x04005C79 RID: 23673
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController showOnMapBtn;

	// Token: 0x04005C7A RID: 23674
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController waypointRemoveBtn;

	// Token: 0x04005C7B RID: 23675
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController inviteBtn;

	// Token: 0x04005C7C RID: 23676
	[PublicizedFrom(EAccessModifier.Private)]
	public string filterString;

	// Token: 0x04005C7D RID: 23677
	[PublicizedFrom(EAccessModifier.Private)]
	public bool updateWaypointsNextUpdate;

	// Token: 0x02000F79 RID: 3961
	public class WaypointSorter : IComparer<Waypoint>
	{
		// Token: 0x06007CF2 RID: 31986 RVA: 0x002F6C42 File Offset: 0x002F4E42
		public WaypointSorter(EntityPlayerLocal _localPlayer)
		{
			this.localPlayerPos = _localPlayer.GetPosition();
		}

		// Token: 0x06007CF3 RID: 31987 RVA: 0x002F6C58 File Offset: 0x002F4E58
		public int Compare(Waypoint _w1, Waypoint _w2)
		{
			float sqrMagnitude = (_w1.pos.ToVector3() - this.localPlayerPos).sqrMagnitude;
			float sqrMagnitude2 = (_w2.pos.ToVector3() - this.localPlayerPos).sqrMagnitude;
			if (sqrMagnitude < sqrMagnitude2)
			{
				return -1;
			}
			if (sqrMagnitude > sqrMagnitude2)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x04005C7E RID: 23678
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3 localPlayerPos;
	}
}

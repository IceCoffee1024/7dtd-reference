using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F91 RID: 3985
[Preserve]
public class XUiC_MapWaypointList : XUiController
{
	// Token: 0x17000EB3 RID: 3763
	// (get) Token: 0x06007DCE RID: 32206 RVA: 0x002FA88B File Offset: 0x002F8A8B
	// (set) Token: 0x06007DCF RID: 32207 RVA: 0x002FA894 File Offset: 0x002F8A94
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

	// Token: 0x17000EB4 RID: 3764
	// (get) Token: 0x06007DD0 RID: 32208 RVA: 0x002FA906 File Offset: 0x002F8B06
	// (set) Token: 0x06007DD1 RID: 32209 RVA: 0x002FA90E File Offset: 0x002F8B0E
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

	// Token: 0x06007DD2 RID: 32210 RVA: 0x002FA918 File Offset: 0x002F8B18
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

	// Token: 0x06007DD3 RID: 32211 RVA: 0x002FAB00 File Offset: 0x002F8D00
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

	// Token: 0x06007DD4 RID: 32212 RVA: 0x002FAB54 File Offset: 0x002F8D54
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

	// Token: 0x06007DD5 RID: 32213 RVA: 0x002FABD4 File Offset: 0x002F8DD4
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

	// Token: 0x06007DD6 RID: 32214 RVA: 0x002FB024 File Offset: 0x002F9224
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

	// Token: 0x06007DD7 RID: 32215 RVA: 0x002FB0A4 File Offset: 0x002F92A4
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

	// Token: 0x06007DD8 RID: 32216 RVA: 0x002FB134 File Offset: 0x002F9334
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

	// Token: 0x06007DD9 RID: 32217 RVA: 0x002FB19A File Offset: 0x002F939A
	[PublicizedFrom(EAccessModifier.Private)]
	public void onInvitePressed(XUiController _sender, int _mouseButton)
	{
		if (this.selectedWaypoint != null)
		{
			this.xui.GetWindow("mapTrackingPopup").IsVisible = true;
		}
	}

	// Token: 0x06007DDA RID: 32218 RVA: 0x002FB1BC File Offset: 0x002F93BC
	[PublicizedFrom(EAccessModifier.Private)]
	public void onShowOnMapPressed(XUiController _sender, int _mouseButton)
	{
		Waypoint waypoint = this.GetSelectedWaypoint();
		if (waypoint != null)
		{
			((XUiC_MapArea)this.xui.GetWindow("mapArea").Controller).PositionMapAt(waypoint.pos.ToVector3());
		}
	}

	// Token: 0x06007DDB RID: 32219 RVA: 0x002FB200 File Offset: 0x002F9400
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

	// Token: 0x06007DDC RID: 32220 RVA: 0x002FB2A1 File Offset: 0x002F94A1
	public Waypoint GetSelectedWaypoint()
	{
		return this.SelectedWaypoint;
	}

	// Token: 0x06007DDD RID: 32221 RVA: 0x002FB2AC File Offset: 0x002F94AC
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

	// Token: 0x06007DDE RID: 32222 RVA: 0x002FB330 File Offset: 0x002F9530
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

	// Token: 0x06007DDF RID: 32223 RVA: 0x002FB3B1 File Offset: 0x002F95B1
	[PublicizedFrom(EAccessModifier.Private)]
	public void waypointFilerOnSubmitHandler(XUiController _sender, string _text)
	{
		this.UpdateWaypointsList(null);
	}

	// Token: 0x06007DE0 RID: 32224 RVA: 0x002FB3B1 File Offset: 0x002F95B1
	[PublicizedFrom(EAccessModifier.Private)]
	public void waypointFilterOnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.UpdateWaypointsList(null);
	}

	// Token: 0x06007DE1 RID: 32225 RVA: 0x002FB3BA File Offset: 0x002F95BA
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.updateWaypointsNextUpdate)
		{
			this.updateWaypointsNextUpdate = false;
			this.UpdateWaypointsList(null);
		}
	}

	// Token: 0x04005D37 RID: 23863
	[PublicizedFrom(EAccessModifier.Private)]
	public Waypoint trackedWaypoint;

	// Token: 0x04005D38 RID: 23864
	[PublicizedFrom(EAccessModifier.Private)]
	public Waypoint selectedWaypoint;

	// Token: 0x04005D39 RID: 23865
	public XUiC_MapWaypointListEntry SelectedWaypointEntry;

	// Token: 0x04005D3A RID: 23866
	[PublicizedFrom(EAccessModifier.Private)]
	public int cCountWaypointsPerPage = 8;

	// Token: 0x04005D3B RID: 23867
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Grid list;

	// Token: 0x04005D3C RID: 23868
	public XUiC_Paging pager;

	// Token: 0x04005D3D RID: 23869
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentPage;

	// Token: 0x04005D3E RID: 23870
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtInputFilter;

	// Token: 0x04005D3F RID: 23871
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController trackBtn;

	// Token: 0x04005D40 RID: 23872
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController showOnMapBtn;

	// Token: 0x04005D41 RID: 23873
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController waypointRemoveBtn;

	// Token: 0x04005D42 RID: 23874
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController inviteBtn;

	// Token: 0x04005D43 RID: 23875
	[PublicizedFrom(EAccessModifier.Private)]
	public string filterString;

	// Token: 0x04005D44 RID: 23876
	[PublicizedFrom(EAccessModifier.Private)]
	public bool updateWaypointsNextUpdate;

	// Token: 0x02000F92 RID: 3986
	public class WaypointSorter : IComparer<Waypoint>
	{
		// Token: 0x06007DE4 RID: 32228 RVA: 0x002FB416 File Offset: 0x002F9616
		public WaypointSorter(EntityPlayerLocal _localPlayer)
		{
			this.localPlayerPos = _localPlayer.GetPosition();
		}

		// Token: 0x06007DE5 RID: 32229 RVA: 0x002FB42C File Offset: 0x002F962C
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

		// Token: 0x04005D45 RID: 23877
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3 localPlayerPos;
	}
}

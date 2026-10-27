using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FB0 RID: 4016
[Preserve]
public class XUiC_MapWaypointList : XUiController
{
	// Token: 0x17000EE5 RID: 3813
	// (get) Token: 0x06007E2D RID: 32301 RVA: 0x002FF44B File Offset: 0x002FD64B
	// (set) Token: 0x06007E2E RID: 32302 RVA: 0x002FF454 File Offset: 0x002FD654
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

	// Token: 0x17000EE6 RID: 3814
	// (get) Token: 0x06007E2F RID: 32303 RVA: 0x002FF4C6 File Offset: 0x002FD6C6
	// (set) Token: 0x06007E30 RID: 32304 RVA: 0x002FF4CE File Offset: 0x002FD6CE
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

	// Token: 0x06007E31 RID: 32305 RVA: 0x002FF4D8 File Offset: 0x002FD6D8
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

	// Token: 0x06007E32 RID: 32306 RVA: 0x002FF6C0 File Offset: 0x002FD8C0
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

	// Token: 0x06007E33 RID: 32307 RVA: 0x002FF714 File Offset: 0x002FD914
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

	// Token: 0x06007E34 RID: 32308 RVA: 0x002FF794 File Offset: 0x002FD994
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

	// Token: 0x06007E35 RID: 32309 RVA: 0x002FFBE4 File Offset: 0x002FDDE4
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

	// Token: 0x06007E36 RID: 32310 RVA: 0x002FFC64 File Offset: 0x002FDE64
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

	// Token: 0x06007E37 RID: 32311 RVA: 0x002FFCF4 File Offset: 0x002FDEF4
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

	// Token: 0x06007E38 RID: 32312 RVA: 0x002FFD5A File Offset: 0x002FDF5A
	[PublicizedFrom(EAccessModifier.Private)]
	public void onInvitePressed(XUiController _sender, int _mouseButton)
	{
		if (this.selectedWaypoint != null)
		{
			this.xui.GetWindow("mapTrackingPopup").IsVisible = true;
		}
	}

	// Token: 0x06007E39 RID: 32313 RVA: 0x002FFD7C File Offset: 0x002FDF7C
	[PublicizedFrom(EAccessModifier.Private)]
	public void onShowOnMapPressed(XUiController _sender, int _mouseButton)
	{
		Waypoint waypoint = this.GetSelectedWaypoint();
		if (waypoint != null)
		{
			((XUiC_MapArea)this.xui.GetWindow("mapArea").Controller).PositionMapAt(waypoint.pos.ToVector3());
		}
	}

	// Token: 0x06007E3A RID: 32314 RVA: 0x002FFDC0 File Offset: 0x002FDFC0
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

	// Token: 0x06007E3B RID: 32315 RVA: 0x002FFE61 File Offset: 0x002FE061
	public Waypoint GetSelectedWaypoint()
	{
		return this.SelectedWaypoint;
	}

	// Token: 0x06007E3C RID: 32316 RVA: 0x002FFE6C File Offset: 0x002FE06C
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

	// Token: 0x06007E3D RID: 32317 RVA: 0x002FFEF0 File Offset: 0x002FE0F0
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

	// Token: 0x06007E3E RID: 32318 RVA: 0x002FFF71 File Offset: 0x002FE171
	[PublicizedFrom(EAccessModifier.Private)]
	public void waypointFilerOnSubmitHandler(XUiController _sender, string _text)
	{
		this.UpdateWaypointsList(null);
	}

	// Token: 0x06007E3F RID: 32319 RVA: 0x002FFF71 File Offset: 0x002FE171
	[PublicizedFrom(EAccessModifier.Private)]
	public void waypointFilterOnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.UpdateWaypointsList(null);
	}

	// Token: 0x06007E40 RID: 32320 RVA: 0x002FFF7A File Offset: 0x002FE17A
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.updateWaypointsNextUpdate)
		{
			this.updateWaypointsNextUpdate = false;
			this.UpdateWaypointsList(null);
		}
	}

	// Token: 0x04005D9B RID: 23963
	[PublicizedFrom(EAccessModifier.Private)]
	public Waypoint trackedWaypoint;

	// Token: 0x04005D9C RID: 23964
	[PublicizedFrom(EAccessModifier.Private)]
	public Waypoint selectedWaypoint;

	// Token: 0x04005D9D RID: 23965
	public XUiC_MapWaypointListEntry SelectedWaypointEntry;

	// Token: 0x04005D9E RID: 23966
	[PublicizedFrom(EAccessModifier.Private)]
	public int cCountWaypointsPerPage = 8;

	// Token: 0x04005D9F RID: 23967
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Grid list;

	// Token: 0x04005DA0 RID: 23968
	public XUiC_Paging pager;

	// Token: 0x04005DA1 RID: 23969
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentPage;

	// Token: 0x04005DA2 RID: 23970
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtInputFilter;

	// Token: 0x04005DA3 RID: 23971
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController trackBtn;

	// Token: 0x04005DA4 RID: 23972
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController showOnMapBtn;

	// Token: 0x04005DA5 RID: 23973
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController waypointRemoveBtn;

	// Token: 0x04005DA6 RID: 23974
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController inviteBtn;

	// Token: 0x04005DA7 RID: 23975
	[PublicizedFrom(EAccessModifier.Private)]
	public string filterString;

	// Token: 0x04005DA8 RID: 23976
	[PublicizedFrom(EAccessModifier.Private)]
	public bool updateWaypointsNextUpdate;

	// Token: 0x02000FB1 RID: 4017
	public class WaypointSorter : IComparer<Waypoint>
	{
		// Token: 0x06007E43 RID: 32323 RVA: 0x002FFFD6 File Offset: 0x002FE1D6
		public WaypointSorter(EntityPlayerLocal _localPlayer)
		{
			this.localPlayerPos = _localPlayer.GetPosition();
		}

		// Token: 0x06007E44 RID: 32324 RVA: 0x002FFFEC File Offset: 0x002FE1EC
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

		// Token: 0x04005DA9 RID: 23977
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3 localPlayerPos;
	}
}

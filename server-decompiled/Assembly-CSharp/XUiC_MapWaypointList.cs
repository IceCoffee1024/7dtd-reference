using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FA0 RID: 4000
[Preserve]
public class XUiC_MapWaypointList : XUiController
{
	// Token: 0x17000EDA RID: 3802
	// (get) Token: 0x06007DDE RID: 32222 RVA: 0x002FD92F File Offset: 0x002FBB2F
	// (set) Token: 0x06007DDF RID: 32223 RVA: 0x002FD938 File Offset: 0x002FBB38
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

	// Token: 0x17000EDB RID: 3803
	// (get) Token: 0x06007DE0 RID: 32224 RVA: 0x002FD9AA File Offset: 0x002FBBAA
	// (set) Token: 0x06007DE1 RID: 32225 RVA: 0x002FD9B2 File Offset: 0x002FBBB2
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

	// Token: 0x06007DE2 RID: 32226 RVA: 0x002FD9BC File Offset: 0x002FBBBC
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

	// Token: 0x06007DE3 RID: 32227 RVA: 0x002FDBA4 File Offset: 0x002FBDA4
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

	// Token: 0x06007DE4 RID: 32228 RVA: 0x002FDBF8 File Offset: 0x002FBDF8
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

	// Token: 0x06007DE5 RID: 32229 RVA: 0x002FDC78 File Offset: 0x002FBE78
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

	// Token: 0x06007DE6 RID: 32230 RVA: 0x002FE0C8 File Offset: 0x002FC2C8
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

	// Token: 0x06007DE7 RID: 32231 RVA: 0x002FE148 File Offset: 0x002FC348
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

	// Token: 0x06007DE8 RID: 32232 RVA: 0x002FE1D8 File Offset: 0x002FC3D8
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

	// Token: 0x06007DE9 RID: 32233 RVA: 0x002FE23E File Offset: 0x002FC43E
	[PublicizedFrom(EAccessModifier.Private)]
	public void onInvitePressed(XUiController _sender, int _mouseButton)
	{
		if (this.selectedWaypoint != null)
		{
			this.xui.GetWindow("mapTrackingPopup").IsVisible = true;
		}
	}

	// Token: 0x06007DEA RID: 32234 RVA: 0x002FE260 File Offset: 0x002FC460
	[PublicizedFrom(EAccessModifier.Private)]
	public void onShowOnMapPressed(XUiController _sender, int _mouseButton)
	{
		Waypoint waypoint = this.GetSelectedWaypoint();
		if (waypoint != null)
		{
			((XUiC_MapArea)this.xui.GetWindow("mapArea").Controller).PositionMapAt(waypoint.pos.ToVector3());
		}
	}

	// Token: 0x06007DEB RID: 32235 RVA: 0x002FE2A4 File Offset: 0x002FC4A4
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

	// Token: 0x06007DEC RID: 32236 RVA: 0x002FE345 File Offset: 0x002FC545
	public Waypoint GetSelectedWaypoint()
	{
		return this.SelectedWaypoint;
	}

	// Token: 0x06007DED RID: 32237 RVA: 0x002FE350 File Offset: 0x002FC550
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

	// Token: 0x06007DEE RID: 32238 RVA: 0x002FE3D4 File Offset: 0x002FC5D4
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

	// Token: 0x06007DEF RID: 32239 RVA: 0x002FE455 File Offset: 0x002FC655
	[PublicizedFrom(EAccessModifier.Private)]
	public void waypointFilerOnSubmitHandler(XUiController _sender, string _text)
	{
		this.UpdateWaypointsList(null);
	}

	// Token: 0x06007DF0 RID: 32240 RVA: 0x002FE455 File Offset: 0x002FC655
	[PublicizedFrom(EAccessModifier.Private)]
	public void waypointFilterOnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.UpdateWaypointsList(null);
	}

	// Token: 0x06007DF1 RID: 32241 RVA: 0x002FE45E File Offset: 0x002FC65E
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.updateWaypointsNextUpdate)
		{
			this.updateWaypointsNextUpdate = false;
			this.UpdateWaypointsList(null);
		}
	}

	// Token: 0x04005D60 RID: 23904
	[PublicizedFrom(EAccessModifier.Private)]
	public Waypoint trackedWaypoint;

	// Token: 0x04005D61 RID: 23905
	[PublicizedFrom(EAccessModifier.Private)]
	public Waypoint selectedWaypoint;

	// Token: 0x04005D62 RID: 23906
	public XUiC_MapWaypointListEntry SelectedWaypointEntry;

	// Token: 0x04005D63 RID: 23907
	[PublicizedFrom(EAccessModifier.Private)]
	public int cCountWaypointsPerPage = 8;

	// Token: 0x04005D64 RID: 23908
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Grid list;

	// Token: 0x04005D65 RID: 23909
	public XUiC_Paging pager;

	// Token: 0x04005D66 RID: 23910
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentPage;

	// Token: 0x04005D67 RID: 23911
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtInputFilter;

	// Token: 0x04005D68 RID: 23912
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController trackBtn;

	// Token: 0x04005D69 RID: 23913
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController showOnMapBtn;

	// Token: 0x04005D6A RID: 23914
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController waypointRemoveBtn;

	// Token: 0x04005D6B RID: 23915
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController inviteBtn;

	// Token: 0x04005D6C RID: 23916
	[PublicizedFrom(EAccessModifier.Private)]
	public string filterString;

	// Token: 0x04005D6D RID: 23917
	[PublicizedFrom(EAccessModifier.Private)]
	public bool updateWaypointsNextUpdate;

	// Token: 0x02000FA1 RID: 4001
	public class WaypointSorter : IComparer<Waypoint>
	{
		// Token: 0x06007DF4 RID: 32244 RVA: 0x002FE4BA File Offset: 0x002FC6BA
		public WaypointSorter(EntityPlayerLocal _localPlayer)
		{
			this.localPlayerPos = _localPlayer.GetPosition();
		}

		// Token: 0x06007DF5 RID: 32245 RVA: 0x002FE4D0 File Offset: 0x002FC6D0
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

		// Token: 0x04005D6E RID: 23918
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3 localPlayerPos;
	}
}

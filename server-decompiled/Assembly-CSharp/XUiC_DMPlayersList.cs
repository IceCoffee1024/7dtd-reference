using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000D82 RID: 3458
[Preserve]
public class XUiC_DMPlayersList : XUiC_DMBaseList<XUiC_DMPlayersList.ListEntry>
{
	// Token: 0x17000B28 RID: 2856
	// (get) Token: 0x060068F0 RID: 26864 RVA: 0x002969AE File Offset: 0x00294BAE
	public bool HasBlockedPlayers
	{
		get
		{
			return this.BlockedPlayerCount > 0;
		}
	}

	// Token: 0x17000B29 RID: 2857
	// (get) Token: 0x060068F1 RID: 26865 RVA: 0x002969B9 File Offset: 0x00294BB9
	public int BlockedPlayerCount
	{
		get
		{
			return this.blockedPlayers.Count;
		}
	}

	// Token: 0x17000B2A RID: 2858
	// (get) Token: 0x060068F2 RID: 26866 RVA: 0x002969C6 File Offset: 0x00294BC6
	public IEnumerable<SaveInfoProvider.PlayerEntryInfo> BlockedPlayers
	{
		get
		{
			return this.blockedPlayers;
		}
	}

	// Token: 0x060068F3 RID: 26867 RVA: 0x002969D0 File Offset: 0x00294BD0
	public override void Init()
	{
		base.Init();
		this.loadingView.IsVisible = false;
		base.GetChildrenById("btnProfile", this.profileButtons);
		foreach (XUiController xuiController in this.profileButtons)
		{
			xuiController.OnPress += this.ProfileButtonOnPress;
			xuiController.OnHover += base.childElementHovered;
		}
	}

	// Token: 0x060068F4 RID: 26868 RVA: 0x00296A64 File Offset: 0x00294C64
	[PublicizedFrom(EAccessModifier.Private)]
	public void ProfileButtonOnPress(XUiController _sender, int _mouseButton)
	{
		for (int i = 0; i < this.profileButtons.Count; i++)
		{
			if (_sender == this.profileButtons[i])
			{
				this.showProfileForEntry(i);
			}
		}
	}

	// Token: 0x060068F5 RID: 26869 RVA: 0x00296AA0 File Offset: 0x00294CA0
	[PublicizedFrom(EAccessModifier.Private)]
	public void showProfileForEntry(int _index)
	{
		if (_index < 0 || _index >= this.listEntryControllers.Length)
		{
			Log.Error(string.Format("ProfileButton index out of range. Index: {0}", _index));
			return;
		}
		XUiC_DMPlayersList.ListEntry entry = this.listEntryControllers[_index].GetEntry();
		if (entry == null)
		{
			Log.Error("ProfileButton pressed for empty entry");
			return;
		}
		if (entry.NativeUserId == null)
		{
			Log.Error("ProfileButton pressed for null user id");
			return;
		}
		PlatformManager.MultiPlatform.User.ShowProfile(entry.NativeUserId);
	}

	// Token: 0x060068F6 RID: 26870 RVA: 0x000027FC File Offset: 0x000009FC
	public override void RebuildList(bool _resetFilter = false)
	{
	}

	// Token: 0x060068F7 RID: 26871 RVA: 0x00296B18 File Offset: 0x00294D18
	public void RebuildList(IReadOnlyCollection<SaveInfoProvider.PlayerEntryInfo> _playerEntryInfos, bool _resetFilter = false)
	{
		this.ClearList(false);
		foreach (SaveInfoProvider.PlayerEntryInfo playerEntryInfo in _playerEntryInfos)
		{
			IPlatformUserBlockedData platformUserBlockedData;
			if (playerEntryInfo.PlatformUserData != null && playerEntryInfo.PlatformUserData.Blocked.TryGetValue(EBlockType.Play, out platformUserBlockedData) && platformUserBlockedData.State != EUserBlockState.NotBlocked)
			{
				this.blockedPlayers.Add(playerEntryInfo);
			}
			else
			{
				this.allEntries.Add(new XUiC_DMPlayersList.ListEntry(playerEntryInfo));
			}
		}
		if (this.lblPlayerLimit != null)
		{
			this.lblPlayerLimit.Text = string.Format("{0}/{1}", this.allEntries.Count + this.BlockedPlayerCount, 100);
		}
		this.loadingView.IsVisible = false;
		base.RebuildList(_resetFilter);
	}

	// Token: 0x060068F8 RID: 26872 RVA: 0x00296BF4 File Offset: 0x00294DF4
	public void ClearList(bool _resetFilter = false)
	{
		this.allEntries.Clear();
		if (this.lblPlayerLimit != null)
		{
			this.lblPlayerLimit.Text = string.Empty;
		}
		this.blockedPlayers.Clear();
		base.RebuildList(_resetFilter);
	}

	// Token: 0x060068F9 RID: 26873 RVA: 0x00296C2B File Offset: 0x00294E2B
	public void ShowLoading()
	{
		this.loadingView.IsVisible = true;
	}

	// Token: 0x040050A4 RID: 20644
	[XuiBindComponent("lblPlayerLimit", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Label lblPlayerLimit;

	// Token: 0x040050A5 RID: 20645
	[XuiBindComponent("loadingOverlay", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiView loadingView;

	// Token: 0x040050A6 RID: 20646
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiController> profileButtons = new List<XUiController>();

	// Token: 0x040050A7 RID: 20647
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<SaveInfoProvider.PlayerEntryInfo> blockedPlayers = new List<SaveInfoProvider.PlayerEntryInfo>();

	// Token: 0x02000D83 RID: 3459
	[Preserve]
	public class ListEntry : XUiListEntry<XUiC_DMPlayersList.ListEntry>
	{
		// Token: 0x060068FB RID: 26875 RVA: 0x00296C58 File Offset: 0x00294E58
		public ListEntry(SaveInfoProvider.PlayerEntryInfo _playerEntryInfo)
		{
			this.PlayerEntryInfo = _playerEntryInfo;
			this.ID = _playerEntryInfo.Id;
			this.CachedName = _playerEntryInfo.CachedName;
			IPlatformUserData platformUserData = _playerEntryInfo.PlatformUserData;
			this.PlayerName = ((platformUserData != null) ? platformUserData.Name : null);
			this.Platform = _playerEntryInfo.PlatformName;
			this.SaveSize = _playerEntryInfo.Size;
			this.LastPlayed = _playerEntryInfo.LastPlayed;
			this.PlayerLevel = _playerEntryInfo.PlayerLevel;
			this.DistanceWalked = _playerEntryInfo.DistanceWalked;
			this.NativeUserId = _playerEntryInfo.NativeUserId;
		}

		// Token: 0x060068FC RID: 26876 RVA: 0x00296CEA File Offset: 0x00294EEA
		public bool CanShowProfile()
		{
			return this.NativeUserId != null && PlatformManager.MultiPlatform.User.CanShowProfile(this.NativeUserId);
		}

		// Token: 0x060068FD RID: 26877 RVA: 0x00296D0B File Offset: 0x00294F0B
		public override int CompareTo(XUiC_DMPlayersList.ListEntry _otherEntry)
		{
			if (_otherEntry != null)
			{
				return this.PlayerEntryInfo.CompareTo(_otherEntry.PlayerEntryInfo);
			}
			return 1;
		}

		// Token: 0x060068FE RID: 26878 RVA: 0x00296D23 File Offset: 0x00294F23
		public override bool MatchesSearch(string _searchString)
		{
			return (this.PlayerName ?? this.CachedName).ContainsCaseInsensitive(_searchString);
		}

		// Token: 0x040050A8 RID: 20648
		public readonly string ID;

		// Token: 0x040050A9 RID: 20649
		public readonly string CachedName;

		// Token: 0x040050AA RID: 20650
		public readonly string PlayerName;

		// Token: 0x040050AB RID: 20651
		public readonly string Platform;

		// Token: 0x040050AC RID: 20652
		public readonly DateTime LastPlayed;

		// Token: 0x040050AD RID: 20653
		public readonly int PlayerLevel;

		// Token: 0x040050AE RID: 20654
		public readonly float DistanceWalked;

		// Token: 0x040050AF RID: 20655
		public readonly long SaveSize;

		// Token: 0x040050B0 RID: 20656
		public readonly PlatformUserIdentifierAbs NativeUserId;

		// Token: 0x040050B1 RID: 20657
		public readonly SaveInfoProvider.PlayerEntryInfo PlayerEntryInfo;
	}

	// Token: 0x02000D84 RID: 3460
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_DMPlayersList.ListEntry>.XUiC_ListEntry
	{
		// Token: 0x060068FF RID: 26879 RVA: 0x00296D3B File Offset: 0x00294F3B
		[XuiXmlBinding("savename")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingSaveName()
		{
			XUiC_DMPlayersList.ListEntry entryData = this.entryData;
			string text;
			if ((text = ((entryData != null) ? entryData.PlayerName : null)) == null)
			{
				XUiC_DMPlayersList.ListEntry entryData2 = this.entryData;
				text = ((entryData2 != null) ? entryData2.CachedName : null) ?? "";
			}
			return text;
		}

		// Token: 0x06006900 RID: 26880 RVA: 0x00296D6E File Offset: 0x00294F6E
		[XuiXmlBinding("platform")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingPlatform()
		{
			XUiC_DMPlayersList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.Platform : null) ?? "";
		}

		// Token: 0x06006901 RID: 26881 RVA: 0x00296D8C File Offset: 0x00294F8C
		[XuiXmlBinding("lastplayed")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingLastPlayed()
		{
			XUiC_DMPlayersList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.LastPlayed.ToString("yyyy-MM-dd HH:mm") : null) ?? "";
		}

		// Token: 0x06006902 RID: 26882 RVA: 0x00296DC4 File Offset: 0x00294FC4
		[XuiXmlBinding("lastplayedinfo")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingLastPlayedInfo()
		{
			if (this.entryData == null)
			{
				return "";
			}
			int num = (int)(DateTime.Now - this.entryData.LastPlayed).TotalDays;
			return string.Format("{0} {1}", num, Localization.Get("xuiDmDaysAgo", false, null));
		}

		// Token: 0x06006903 RID: 26883 RVA: 0x00296E1C File Offset: 0x0029501C
		[XuiXmlBinding("level")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingLevel()
		{
			if (this.entryData == null)
			{
				return "";
			}
			if (this.entryData.PlayerLevel >= 1)
			{
				return string.Format("{0} {1}", Localization.Get("xuiLevel", false, null), this.entryData.PlayerLevel);
			}
			return "-";
		}

		// Token: 0x06006904 RID: 26884 RVA: 0x00296E74 File Offset: 0x00295074
		[XuiXmlBinding("distance")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingDistance()
		{
			if (this.entryData == null)
			{
				return "";
			}
			if (this.entryData.PlayerLevel >= 1)
			{
				return string.Format("{0} {1}", (int)(this.entryData.DistanceWalked / 1000f), Localization.Get("xuiKMTravelled", false, null));
			}
			return "-";
		}

		// Token: 0x06006905 RID: 26885 RVA: 0x00296ED0 File Offset: 0x002950D0
		[XuiXmlBinding("canShowProfile")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingCanShowProfile()
		{
			XUiC_DMPlayersList.ListEntry entryData = this.entryData;
			return entryData != null && entryData.CanShowProfile();
		}
	}
}

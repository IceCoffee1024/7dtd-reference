using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000D91 RID: 3473
[Preserve]
public class XUiC_DMPlayersList : XUiC_DMBaseList<XUiC_DMPlayersList.ListEntry>
{
	// Token: 0x17000B43 RID: 2883
	// (get) Token: 0x060068E8 RID: 26856 RVA: 0x00299D3A File Offset: 0x00297F3A
	public bool HasBlockedPlayers
	{
		get
		{
			return this.BlockedPlayerCount > 0;
		}
	}

	// Token: 0x17000B44 RID: 2884
	// (get) Token: 0x060068E9 RID: 26857 RVA: 0x00299D45 File Offset: 0x00297F45
	public int BlockedPlayerCount
	{
		get
		{
			return this.blockedPlayers.Count;
		}
	}

	// Token: 0x17000B45 RID: 2885
	// (get) Token: 0x060068EA RID: 26858 RVA: 0x00299D52 File Offset: 0x00297F52
	public IEnumerable<SaveInfoProvider.PlayerEntryInfo> BlockedPlayers
	{
		get
		{
			return this.blockedPlayers;
		}
	}

	// Token: 0x060068EB RID: 26859 RVA: 0x00299D5C File Offset: 0x00297F5C
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

	// Token: 0x060068EC RID: 26860 RVA: 0x00299DF0 File Offset: 0x00297FF0
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

	// Token: 0x060068ED RID: 26861 RVA: 0x00299E2C File Offset: 0x0029802C
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

	// Token: 0x060068EE RID: 26862 RVA: 0x000027FC File Offset: 0x000009FC
	public override void RebuildList(bool _resetFilter = false)
	{
	}

	// Token: 0x060068EF RID: 26863 RVA: 0x00299EA4 File Offset: 0x002980A4
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

	// Token: 0x060068F0 RID: 26864 RVA: 0x00299F80 File Offset: 0x00298180
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

	// Token: 0x060068F1 RID: 26865 RVA: 0x00299FB7 File Offset: 0x002981B7
	public void ShowLoading()
	{
		this.loadingView.IsVisible = true;
	}

	// Token: 0x040050DB RID: 20699
	[XuiBindComponent("lblPlayerLimit", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Label lblPlayerLimit;

	// Token: 0x040050DC RID: 20700
	[XuiBindComponent("loadingOverlay", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiView loadingView;

	// Token: 0x040050DD RID: 20701
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiController> profileButtons = new List<XUiController>();

	// Token: 0x040050DE RID: 20702
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<SaveInfoProvider.PlayerEntryInfo> blockedPlayers = new List<SaveInfoProvider.PlayerEntryInfo>();

	// Token: 0x02000D92 RID: 3474
	[Preserve]
	public class ListEntry : XUiListEntry<XUiC_DMPlayersList.ListEntry>
	{
		// Token: 0x060068F3 RID: 26867 RVA: 0x00299FE4 File Offset: 0x002981E4
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

		// Token: 0x060068F4 RID: 26868 RVA: 0x0029A076 File Offset: 0x00298276
		public bool CanShowProfile()
		{
			return this.NativeUserId != null && PlatformManager.MultiPlatform.User.CanShowProfile(this.NativeUserId);
		}

		// Token: 0x060068F5 RID: 26869 RVA: 0x0029A097 File Offset: 0x00298297
		public override int CompareTo(XUiC_DMPlayersList.ListEntry _otherEntry)
		{
			if (_otherEntry != null)
			{
				return this.PlayerEntryInfo.CompareTo(_otherEntry.PlayerEntryInfo);
			}
			return 1;
		}

		// Token: 0x060068F6 RID: 26870 RVA: 0x0029A0AF File Offset: 0x002982AF
		public override bool MatchesSearch(string _searchString)
		{
			return (this.PlayerName ?? this.CachedName).ContainsCaseInsensitive(_searchString);
		}

		// Token: 0x040050DF RID: 20703
		public readonly string ID;

		// Token: 0x040050E0 RID: 20704
		public readonly string CachedName;

		// Token: 0x040050E1 RID: 20705
		public readonly string PlayerName;

		// Token: 0x040050E2 RID: 20706
		public readonly string Platform;

		// Token: 0x040050E3 RID: 20707
		public readonly DateTime LastPlayed;

		// Token: 0x040050E4 RID: 20708
		public readonly int PlayerLevel;

		// Token: 0x040050E5 RID: 20709
		public readonly float DistanceWalked;

		// Token: 0x040050E6 RID: 20710
		public readonly long SaveSize;

		// Token: 0x040050E7 RID: 20711
		public readonly PlatformUserIdentifierAbs NativeUserId;

		// Token: 0x040050E8 RID: 20712
		public readonly SaveInfoProvider.PlayerEntryInfo PlayerEntryInfo;
	}

	// Token: 0x02000D93 RID: 3475
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_DMPlayersList.ListEntry>.XUiC_ListEntry
	{
		// Token: 0x060068F7 RID: 26871 RVA: 0x0029A0C7 File Offset: 0x002982C7
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

		// Token: 0x060068F8 RID: 26872 RVA: 0x0029A0FA File Offset: 0x002982FA
		[XuiXmlBinding("platform")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingPlatform()
		{
			XUiC_DMPlayersList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.Platform : null) ?? "";
		}

		// Token: 0x060068F9 RID: 26873 RVA: 0x0029A118 File Offset: 0x00298318
		[XuiXmlBinding("lastplayed")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingLastPlayed()
		{
			XUiC_DMPlayersList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.LastPlayed.ToString("yyyy-MM-dd HH:mm") : null) ?? "";
		}

		// Token: 0x060068FA RID: 26874 RVA: 0x0029A150 File Offset: 0x00298350
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

		// Token: 0x060068FB RID: 26875 RVA: 0x0029A1A8 File Offset: 0x002983A8
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

		// Token: 0x060068FC RID: 26876 RVA: 0x0029A200 File Offset: 0x00298400
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

		// Token: 0x060068FD RID: 26877 RVA: 0x0029A25C File Offset: 0x0029845C
		[XuiXmlBinding("canShowProfile")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingCanShowProfile()
		{
			XUiC_DMPlayersList.ListEntry entryData = this.entryData;
			return entryData != null && entryData.CanShowProfile();
		}
	}
}

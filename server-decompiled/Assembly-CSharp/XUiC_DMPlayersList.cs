using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000D6E RID: 3438
[Preserve]
public class XUiC_DMPlayersList : XUiC_DMBaseList<XUiC_DMPlayersList.ListEntry>
{
	// Token: 0x17000B10 RID: 2832
	// (get) Token: 0x06006854 RID: 26708 RVA: 0x0029385A File Offset: 0x00291A5A
	public bool HasBlockedPlayers
	{
		get
		{
			return this.BlockedPlayerCount > 0;
		}
	}

	// Token: 0x17000B11 RID: 2833
	// (get) Token: 0x06006855 RID: 26709 RVA: 0x00293865 File Offset: 0x00291A65
	public int BlockedPlayerCount
	{
		get
		{
			return this.blockedPlayers.Count;
		}
	}

	// Token: 0x17000B12 RID: 2834
	// (get) Token: 0x06006856 RID: 26710 RVA: 0x00293872 File Offset: 0x00291A72
	public IEnumerable<SaveInfoProvider.PlayerEntryInfo> BlockedPlayers
	{
		get
		{
			return this.blockedPlayers;
		}
	}

	// Token: 0x06006857 RID: 26711 RVA: 0x0029387C File Offset: 0x00291A7C
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

	// Token: 0x06006858 RID: 26712 RVA: 0x00293910 File Offset: 0x00291B10
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

	// Token: 0x06006859 RID: 26713 RVA: 0x0029394C File Offset: 0x00291B4C
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

	// Token: 0x0600685A RID: 26714 RVA: 0x000027FC File Offset: 0x000009FC
	public override void RebuildList(bool _resetFilter = false)
	{
	}

	// Token: 0x0600685B RID: 26715 RVA: 0x002939C4 File Offset: 0x00291BC4
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

	// Token: 0x0600685C RID: 26716 RVA: 0x00293AA0 File Offset: 0x00291CA0
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

	// Token: 0x0600685D RID: 26717 RVA: 0x00293AD7 File Offset: 0x00291CD7
	public void ShowLoading()
	{
		this.loadingView.IsVisible = true;
	}

	// Token: 0x04005002 RID: 20482
	[XuiBindComponent("lblPlayerLimit", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Label lblPlayerLimit;

	// Token: 0x04005003 RID: 20483
	[XuiBindComponent("loadingOverlay", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiView loadingView;

	// Token: 0x04005004 RID: 20484
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiController> profileButtons = new List<XUiController>();

	// Token: 0x04005005 RID: 20485
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<SaveInfoProvider.PlayerEntryInfo> blockedPlayers = new List<SaveInfoProvider.PlayerEntryInfo>();

	// Token: 0x02000D6F RID: 3439
	[Preserve]
	public class ListEntry : XUiListEntry<XUiC_DMPlayersList.ListEntry>
	{
		// Token: 0x0600685F RID: 26719 RVA: 0x00293B04 File Offset: 0x00291D04
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

		// Token: 0x06006860 RID: 26720 RVA: 0x00293B96 File Offset: 0x00291D96
		public bool CanShowProfile()
		{
			return this.NativeUserId != null && PlatformManager.MultiPlatform.User.CanShowProfile(this.NativeUserId);
		}

		// Token: 0x06006861 RID: 26721 RVA: 0x00293BB7 File Offset: 0x00291DB7
		public override int CompareTo(XUiC_DMPlayersList.ListEntry _otherEntry)
		{
			if (_otherEntry != null)
			{
				return this.PlayerEntryInfo.CompareTo(_otherEntry.PlayerEntryInfo);
			}
			return 1;
		}

		// Token: 0x06006862 RID: 26722 RVA: 0x00293BCF File Offset: 0x00291DCF
		public override bool MatchesSearch(string _searchString)
		{
			return (this.PlayerName ?? this.CachedName).ContainsCaseInsensitive(_searchString);
		}

		// Token: 0x04005006 RID: 20486
		public readonly string ID;

		// Token: 0x04005007 RID: 20487
		public readonly string CachedName;

		// Token: 0x04005008 RID: 20488
		public readonly string PlayerName;

		// Token: 0x04005009 RID: 20489
		public readonly string Platform;

		// Token: 0x0400500A RID: 20490
		public readonly DateTime LastPlayed;

		// Token: 0x0400500B RID: 20491
		public readonly int PlayerLevel;

		// Token: 0x0400500C RID: 20492
		public readonly float DistanceWalked;

		// Token: 0x0400500D RID: 20493
		public readonly long SaveSize;

		// Token: 0x0400500E RID: 20494
		public readonly PlatformUserIdentifierAbs NativeUserId;

		// Token: 0x0400500F RID: 20495
		public readonly SaveInfoProvider.PlayerEntryInfo PlayerEntryInfo;
	}

	// Token: 0x02000D70 RID: 3440
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_DMPlayersList.ListEntry>.XUiC_ListEntry
	{
		// Token: 0x06006863 RID: 26723 RVA: 0x00293BE7 File Offset: 0x00291DE7
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

		// Token: 0x06006864 RID: 26724 RVA: 0x00293C1A File Offset: 0x00291E1A
		[XuiXmlBinding("platform")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingPlatform()
		{
			XUiC_DMPlayersList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.Platform : null) ?? "";
		}

		// Token: 0x06006865 RID: 26725 RVA: 0x00293C38 File Offset: 0x00291E38
		[XuiXmlBinding("lastplayed")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingLastPlayed()
		{
			XUiC_DMPlayersList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.LastPlayed.ToString("yyyy-MM-dd HH:mm") : null) ?? "";
		}

		// Token: 0x06006866 RID: 26726 RVA: 0x00293C70 File Offset: 0x00291E70
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

		// Token: 0x06006867 RID: 26727 RVA: 0x00293CC8 File Offset: 0x00291EC8
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

		// Token: 0x06006868 RID: 26728 RVA: 0x00293D20 File Offset: 0x00291F20
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

		// Token: 0x06006869 RID: 26729 RVA: 0x00293D7C File Offset: 0x00291F7C
		[XuiXmlBinding("canShowProfile")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingCanShowProfile()
		{
			XUiC_DMPlayersList.ListEntry entryData = this.entryData;
			return entryData != null && entryData.CanShowProfile();
		}
	}
}

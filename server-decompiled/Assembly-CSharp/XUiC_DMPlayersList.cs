using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000D9C RID: 3484
[Preserve]
public class XUiC_DMPlayersList : XUiC_DMBaseList<XUiC_DMPlayersList.ListEntry>
{
	// Token: 0x17000B49 RID: 2889
	// (get) Token: 0x0600690D RID: 26893 RVA: 0x0029B262 File Offset: 0x00299462
	public bool HasBlockedPlayers
	{
		get
		{
			return this.BlockedPlayerCount > 0;
		}
	}

	// Token: 0x17000B4A RID: 2890
	// (get) Token: 0x0600690E RID: 26894 RVA: 0x0029B26D File Offset: 0x0029946D
	public int BlockedPlayerCount
	{
		get
		{
			return this.blockedPlayers.Count;
		}
	}

	// Token: 0x17000B4B RID: 2891
	// (get) Token: 0x0600690F RID: 26895 RVA: 0x0029B27A File Offset: 0x0029947A
	public IEnumerable<SaveInfoProvider.PlayerEntryInfo> BlockedPlayers
	{
		get
		{
			return this.blockedPlayers;
		}
	}

	// Token: 0x06006910 RID: 26896 RVA: 0x0029B284 File Offset: 0x00299484
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

	// Token: 0x06006911 RID: 26897 RVA: 0x0029B318 File Offset: 0x00299518
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

	// Token: 0x06006912 RID: 26898 RVA: 0x0029B354 File Offset: 0x00299554
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

	// Token: 0x06006913 RID: 26899 RVA: 0x000027FC File Offset: 0x000009FC
	public override void RebuildList(bool _resetFilter = false)
	{
	}

	// Token: 0x06006914 RID: 26900 RVA: 0x0029B3CC File Offset: 0x002995CC
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

	// Token: 0x06006915 RID: 26901 RVA: 0x0029B4A8 File Offset: 0x002996A8
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

	// Token: 0x06006916 RID: 26902 RVA: 0x0029B4DF File Offset: 0x002996DF
	public void ShowLoading()
	{
		this.loadingView.IsVisible = true;
	}

	// Token: 0x04005100 RID: 20736
	[XuiBindComponent("lblPlayerLimit", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Label lblPlayerLimit;

	// Token: 0x04005101 RID: 20737
	[XuiBindComponent("loadingOverlay", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiView loadingView;

	// Token: 0x04005102 RID: 20738
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiController> profileButtons = new List<XUiController>();

	// Token: 0x04005103 RID: 20739
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<SaveInfoProvider.PlayerEntryInfo> blockedPlayers = new List<SaveInfoProvider.PlayerEntryInfo>();

	// Token: 0x02000D9D RID: 3485
	[Preserve]
	public class ListEntry : XUiListEntry<XUiC_DMPlayersList.ListEntry>
	{
		// Token: 0x06006918 RID: 26904 RVA: 0x0029B50C File Offset: 0x0029970C
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

		// Token: 0x06006919 RID: 26905 RVA: 0x0029B59E File Offset: 0x0029979E
		public bool CanShowProfile()
		{
			return this.NativeUserId != null && PlatformManager.MultiPlatform.User.CanShowProfile(this.NativeUserId);
		}

		// Token: 0x0600691A RID: 26906 RVA: 0x0029B5BF File Offset: 0x002997BF
		public override int CompareTo(XUiC_DMPlayersList.ListEntry _otherEntry)
		{
			if (_otherEntry != null)
			{
				return this.PlayerEntryInfo.CompareTo(_otherEntry.PlayerEntryInfo);
			}
			return 1;
		}

		// Token: 0x0600691B RID: 26907 RVA: 0x0029B5D7 File Offset: 0x002997D7
		public override bool MatchesSearch(string _searchString)
		{
			return (this.PlayerName ?? this.CachedName).ContainsCaseInsensitive(_searchString);
		}

		// Token: 0x04005104 RID: 20740
		public readonly string ID;

		// Token: 0x04005105 RID: 20741
		public readonly string CachedName;

		// Token: 0x04005106 RID: 20742
		public readonly string PlayerName;

		// Token: 0x04005107 RID: 20743
		public readonly string Platform;

		// Token: 0x04005108 RID: 20744
		public readonly DateTime LastPlayed;

		// Token: 0x04005109 RID: 20745
		public readonly int PlayerLevel;

		// Token: 0x0400510A RID: 20746
		public readonly float DistanceWalked;

		// Token: 0x0400510B RID: 20747
		public readonly long SaveSize;

		// Token: 0x0400510C RID: 20748
		public readonly PlatformUserIdentifierAbs NativeUserId;

		// Token: 0x0400510D RID: 20749
		public readonly SaveInfoProvider.PlayerEntryInfo PlayerEntryInfo;
	}

	// Token: 0x02000D9E RID: 3486
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_DMPlayersList.ListEntry>.XUiC_ListEntry
	{
		// Token: 0x0600691C RID: 26908 RVA: 0x0029B5EF File Offset: 0x002997EF
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

		// Token: 0x0600691D RID: 26909 RVA: 0x0029B622 File Offset: 0x00299822
		[XuiXmlBinding("platform")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingPlatform()
		{
			XUiC_DMPlayersList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.Platform : null) ?? "";
		}

		// Token: 0x0600691E RID: 26910 RVA: 0x0029B640 File Offset: 0x00299840
		[XuiXmlBinding("lastplayed")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingLastPlayed()
		{
			XUiC_DMPlayersList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.LastPlayed.ToString("yyyy-MM-dd HH:mm") : null) ?? "";
		}

		// Token: 0x0600691F RID: 26911 RVA: 0x0029B678 File Offset: 0x00299878
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

		// Token: 0x06006920 RID: 26912 RVA: 0x0029B6D0 File Offset: 0x002998D0
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

		// Token: 0x06006921 RID: 26913 RVA: 0x0029B728 File Offset: 0x00299928
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

		// Token: 0x06006922 RID: 26914 RVA: 0x0029B784 File Offset: 0x00299984
		[XuiXmlBinding("canShowProfile")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingCanShowProfile()
		{
			XUiC_DMPlayersList.ListEntry entryData = this.entryData;
			return entryData != null && entryData.CanShowProfile();
		}
	}
}

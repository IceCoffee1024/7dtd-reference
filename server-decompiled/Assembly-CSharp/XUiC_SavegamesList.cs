using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000E44 RID: 3652
[Preserve]
public class XUiC_SavegamesList : XUiC_List<XUiC_SavegamesList.ListEntry>
{
	// Token: 0x0600705B RID: 28763 RVA: 0x002B3B43 File Offset: 0x002B1D43
	public override void OnOpen()
	{
		base.OnOpen();
		this.RebuildList(false);
	}

	// Token: 0x0600705C RID: 28764 RVA: 0x002B3B52 File Offset: 0x002B1D52
	public override void RebuildList(bool _resetFilter = false)
	{
		this.allEntries.Clear();
		GameIO.GetPlayerSaves(new GameIO.FoundSave(this.addSaveToEntries), false);
		this.allEntries.Sort();
		base.RebuildList(_resetFilter);
	}

	// Token: 0x0600705D RID: 28765 RVA: 0x002B3B84 File Offset: 0x002B1D84
	public bool SelectByName(string _name)
	{
		if (!string.IsNullOrEmpty(_name))
		{
			for (int i = 0; i < this.filteredEntries.Count; i++)
			{
				if (this.filteredEntries[i].SaveName.Equals(_name, StringComparison.OrdinalIgnoreCase))
				{
					base.SelectedEntryIndex = i;
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600705E RID: 28766 RVA: 0x002B3BD4 File Offset: 0x002B1DD4
	public bool TrySelectEntry(string _worldName, string _saveName, UserDataStorageType _storageType)
	{
		if (this.filteredEntries == null)
		{
			Log.Error("filteredEntries is null");
			return false;
		}
		for (int i = 0; i < this.filteredEntries.Count; i++)
		{
			XUiC_SavegamesList.ListEntry listEntry = this.filteredEntries[i];
			if (listEntry.WorldName.EqualsCaseInsensitive(_worldName) && listEntry.SaveName.EqualsCaseInsensitive(_saveName) && listEntry.StorageType == _storageType)
			{
				base.SelectedEntryIndex = i;
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600705F RID: 28767 RVA: 0x002B3C47 File Offset: 0x002B1E47
	[PublicizedFrom(EAccessModifier.Private)]
	public void addSaveToEntries(UserDataStorageType _storage, string _saveName, string _worldName, DateTime _lastSaved, WorldState _worldState, bool _isArchived)
	{
		this.allEntries.Add(new XUiC_SavegamesList.ListEntry(_storage, _saveName, _worldName, _lastSaved, _worldState));
	}

	// Token: 0x02000E45 RID: 3653
	[Preserve]
	public class ListEntry : XUiListEntry<XUiC_SavegamesList.ListEntry>
	{
		// Token: 0x17000CB3 RID: 3251
		// (get) Token: 0x06007061 RID: 28769 RVA: 0x002B3C68 File Offset: 0x002B1E68
		public GameMode GameMode
		{
			get
			{
				return GameMode.GetGameModeForId(this.WorldState.activeGameMode);
			}
		}

		// Token: 0x06007062 RID: 28770 RVA: 0x002B3C7C File Offset: 0x002B1E7C
		public ListEntry(UserDataStorageType _storage, string _saveName, string _worldName, DateTime _lastSaved, WorldState _worldState)
		{
			this.StorageType = _storage;
			this.SaveName = _saveName;
			this.WorldName = _worldName;
			this.LastSaved = _lastSaved;
			this.WorldState = _worldState;
			this.WorldExists = PathAbstractions.Contextual.DoesWorldExist(_worldName);
			this.SaveVersion = this.WorldState.gameVersion;
			this.VersionComparison = XUiC_SavegamesList.ListEntry.checkSaveCompatibility(this.SaveVersion);
			XUiC_SavegamesList.ListEntry.ESaveCompatibility versionComparison = this.VersionComparison;
			this.Playable = versionComparison == XUiC_SavegamesList.ListEntry.ESaveCompatibility.SameVersion || versionComparison == XUiC_SavegamesList.ListEntry.ESaveCompatibility.NewerMinor || versionComparison == XUiC_SavegamesList.ListEntry.ESaveCompatibility.Compatible;
			if (PlatformManager.MultiPlatform.UserDataRoaming.IsRoamingOptional)
			{
				this.DisplaySaveName = this.SaveName + " [808080][i](" + this.StorageType.LocalizedName() + ")[/i][-]";
				return;
			}
			this.DisplaySaveName = this.SaveName;
		}

		// Token: 0x06007063 RID: 28771 RVA: 0x002B3D48 File Offset: 0x002B1F48
		[PublicizedFrom(EAccessModifier.Private)]
		public static XUiC_SavegamesList.ListEntry.ESaveCompatibility checkSaveCompatibility(VersionInformation _saveVersion)
		{
			VersionInformation cVersionInformation = Constants.cVersionInformation;
			if (_saveVersion.ReleaseType < cVersionInformation.ReleaseType)
			{
				return XUiC_SavegamesList.ListEntry.ESaveCompatibility.OldVersion;
			}
			if (_saveVersion.ReleaseType > cVersionInformation.ReleaseType)
			{
				return XUiC_SavegamesList.ListEntry.ESaveCompatibility.FutureVersion;
			}
			if (_saveVersion.Major > cVersionInformation.Major)
			{
				return XUiC_SavegamesList.ListEntry.ESaveCompatibility.FutureVersion;
			}
			if (_saveVersion.Major == cVersionInformation.Major)
			{
				if (_saveVersion.Minor == cVersionInformation.Minor)
				{
					return XUiC_SavegamesList.ListEntry.ESaveCompatibility.SameVersion;
				}
				if (_saveVersion.Minor > cVersionInformation.Minor)
				{
					return XUiC_SavegamesList.ListEntry.ESaveCompatibility.NewerMinor;
				}
				return XUiC_SavegamesList.ListEntry.ESaveCompatibility.Compatible;
			}
			else
			{
				if (_saveVersion.Major < 2)
				{
					return XUiC_SavegamesList.ListEntry.ESaveCompatibility.OldVersion;
				}
				return XUiC_SavegamesList.ListEntry.ESaveCompatibility.Compatible;
			}
		}

		// Token: 0x06007064 RID: 28772 RVA: 0x002B3DC7 File Offset: 0x002B1FC7
		public string GetSaveDir()
		{
			return GameIO.GetSaveGameDir(this.WorldName, this.SaveName, this.StorageType);
		}

		// Token: 0x06007065 RID: 28773 RVA: 0x002B3DE0 File Offset: 0x002B1FE0
		public override int CompareTo(XUiC_SavegamesList.ListEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return -1;
			}
			return -1 * this.LastSaved.CompareTo(_otherEntry.LastSaved);
		}

		// Token: 0x06007066 RID: 28774 RVA: 0x002B3E08 File Offset: 0x002B2008
		public override bool MatchesSearch(string _searchString)
		{
			return this.SaveName.ContainsCaseInsensitive(_searchString);
		}

		// Token: 0x040054DC RID: 21724
		public readonly UserDataStorageType StorageType;

		// Token: 0x040054DD RID: 21725
		public readonly string SaveName;

		// Token: 0x040054DE RID: 21726
		public readonly string DisplaySaveName;

		// Token: 0x040054DF RID: 21727
		public readonly string WorldName;

		// Token: 0x040054E0 RID: 21728
		public readonly DateTime LastSaved;

		// Token: 0x040054E1 RID: 21729
		public readonly WorldState WorldState;

		// Token: 0x040054E2 RID: 21730
		public readonly bool WorldExists;

		// Token: 0x040054E3 RID: 21731
		public readonly VersionInformation SaveVersion;

		// Token: 0x040054E4 RID: 21732
		public readonly XUiC_SavegamesList.ListEntry.ESaveCompatibility VersionComparison;

		// Token: 0x040054E5 RID: 21733
		public readonly bool Playable;

		// Token: 0x02000E46 RID: 3654
		public enum ESaveCompatibility
		{
			// Token: 0x040054E7 RID: 21735
			SameVersion,
			// Token: 0x040054E8 RID: 21736
			NewerMinor,
			// Token: 0x040054E9 RID: 21737
			Compatible,
			// Token: 0x040054EA RID: 21738
			FutureVersion,
			// Token: 0x040054EB RID: 21739
			OldVersion
		}
	}

	// Token: 0x02000E47 RID: 3655
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_SavegamesList.ListEntry>.XUiC_ListEntry
	{
		// Token: 0x06007067 RID: 28775 RVA: 0x002B3E18 File Offset: 0x002B2018
		public override void Init()
		{
			base.Init();
			XUiController childById = base.GetChildById("Version");
			childById.OnScroll += delegate(XUiController _, float _delta)
			{
				base.Scrolled(_delta);
			};
			childById.OnPress += delegate(XUiController _, int _button)
			{
				this.Pressed(_button);
			};
			childById.OnDoubleClick += delegate(XUiController _, int _button)
			{
				base.DoubleClicked(_button);
			};
			childById.OnHover += delegate(XUiController _, bool _over)
			{
				base.ForceHovered = _over;
				base.Hovered(_over);
			};
			XUiController childById2 = base.GetChildById("World");
			childById2.OnScroll += delegate(XUiController _, float _delta)
			{
				base.Scrolled(_delta);
			};
			childById2.OnPress += delegate(XUiController _, int _button)
			{
				this.Pressed(_button);
			};
			childById2.OnDoubleClick += delegate(XUiController _, int _button)
			{
				base.DoubleClicked(_button);
			};
			childById2.OnHover += delegate(XUiController _, bool _over)
			{
				base.ForceHovered = _over;
				base.Hovered(_over);
			};
		}

		// Token: 0x06007068 RID: 28776 RVA: 0x002B3ECF File Offset: 0x002B20CF
		[XuiXmlBinding("savename")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingSaveName()
		{
			XUiC_SavegamesList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.DisplaySaveName : null) ?? "";
		}

		// Token: 0x06007069 RID: 28777 RVA: 0x002B3EEC File Offset: 0x002B20EC
		[XuiXmlBinding("worldname")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingWorldName()
		{
			XUiC_SavegamesList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.WorldName : null) ?? "";
		}

		// Token: 0x0600706A RID: 28778 RVA: 0x002B3F09 File Offset: 0x002B2109
		[XuiXmlBinding("worldtooltip")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingWorldTooltip()
		{
			if (this.entryData != null && !this.entryData.WorldExists)
			{
				return Localization.Get("xuiSavegameWorldNotFound", false, null);
			}
			return "";
		}

		// Token: 0x0600706B RID: 28779 RVA: 0x002B3F34 File Offset: 0x002B2134
		[XuiXmlBinding("mode")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingMode()
		{
			if (this.entryData == null)
			{
				return "";
			}
			GameMode gameMode = this.entryData.GameMode;
			if (gameMode != null)
			{
				return Localization.Get(gameMode.GetName(), false, null);
			}
			return "-Unknown-";
		}

		// Token: 0x0600706C RID: 28780 RVA: 0x002B3F74 File Offset: 0x002B2174
		[XuiXmlBinding("version")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingVersion()
		{
			if (this.entryData == null)
			{
				return "";
			}
			WorldState worldState = this.entryData.WorldState;
			if (worldState.gameVersion.Major < 0)
			{
				return worldState.gameVersionString;
			}
			return worldState.gameVersion.ShortString;
		}

		// Token: 0x0600706D RID: 28781 RVA: 0x002B3FBC File Offset: 0x002B21BC
		[XuiXmlBinding("versiontooltip")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingVersionTooltip()
		{
			if (this.entryData == null)
			{
				return "";
			}
			string text;
			switch (this.entryData.VersionComparison)
			{
			case XUiC_SavegamesList.ListEntry.ESaveCompatibility.SameVersion:
				text = "";
				break;
			case XUiC_SavegamesList.ListEntry.ESaveCompatibility.NewerMinor:
				text = Localization.Get("xuiSavegameNewerMinor", false, null);
				break;
			case XUiC_SavegamesList.ListEntry.ESaveCompatibility.Compatible:
				text = Localization.Get("xuiSavegameOlderMinor", false, null);
				break;
			case XUiC_SavegamesList.ListEntry.ESaveCompatibility.FutureVersion:
				text = Localization.Get("xuiSavegameFutureMajor", false, null);
				break;
			case XUiC_SavegamesList.ListEntry.ESaveCompatibility.OldVersion:
				text = Localization.Get("xuiSavegameOldVersion", false, null);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			return text;
		}

		// Token: 0x0600706E RID: 28782 RVA: 0x002B404C File Offset: 0x002B224C
		[XuiXmlBinding("lastplayed")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingLastPlayed()
		{
			XUiC_SavegamesList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.LastSaved.ToString("yyyy-MM-dd HH:mm") : null) ?? "";
		}

		// Token: 0x0600706F RID: 28783 RVA: 0x002B4081 File Offset: 0x002B2281
		[XuiXmlBinding("usesdatalimit")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingUsesDataLimit()
		{
			return this.entryData != null && PlatformManager.MultiPlatform.UserDataRoaming.IsRoamingOptional && this.entryData.StorageType.UsesDataLimit();
		}

		// Token: 0x06007070 RID: 28784 RVA: 0x002B40AE File Offset: 0x002B22AE
		[XuiXmlBinding("worldstate")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingWorldState()
		{
			if (this.entryData == null)
			{
				return "";
			}
			if (!this.entryData.WorldExists)
			{
				return "missing";
			}
			return "ok";
		}

		// Token: 0x06007071 RID: 28785 RVA: 0x002B40D8 File Offset: 0x002B22D8
		[XuiXmlBinding("versionstate")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingVersionState()
		{
			if (this.entryData == null)
			{
				return "";
			}
			string text;
			switch (this.entryData.VersionComparison)
			{
			case XUiC_SavegamesList.ListEntry.ESaveCompatibility.SameVersion:
				text = "same";
				break;
			case XUiC_SavegamesList.ListEntry.ESaveCompatibility.NewerMinor:
				text = "compatible";
				break;
			case XUiC_SavegamesList.ListEntry.ESaveCompatibility.Compatible:
				text = "compatible";
				break;
			case XUiC_SavegamesList.ListEntry.ESaveCompatibility.FutureVersion:
				text = "incompatible";
				break;
			case XUiC_SavegamesList.ListEntry.ESaveCompatibility.OldVersion:
				text = "incompatible";
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			return text;
		}
	}
}

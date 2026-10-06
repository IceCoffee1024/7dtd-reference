using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000E53 RID: 3667
[Preserve]
public class XUiC_SavegamesList : XUiC_List<XUiC_SavegamesList.ListEntry>
{
	// Token: 0x06007053 RID: 28755 RVA: 0x002B6EF3 File Offset: 0x002B50F3
	public override void OnOpen()
	{
		base.OnOpen();
		this.RebuildList(false);
	}

	// Token: 0x06007054 RID: 28756 RVA: 0x002B6F02 File Offset: 0x002B5102
	public override void RebuildList(bool _resetFilter = false)
	{
		this.allEntries.Clear();
		GameIO.GetPlayerSaves(new GameIO.FoundSave(this.addSaveToEntries), false);
		this.allEntries.Sort();
		base.RebuildList(_resetFilter);
	}

	// Token: 0x06007055 RID: 28757 RVA: 0x002B6F34 File Offset: 0x002B5134
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

	// Token: 0x06007056 RID: 28758 RVA: 0x002B6F84 File Offset: 0x002B5184
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

	// Token: 0x06007057 RID: 28759 RVA: 0x002B6FF7 File Offset: 0x002B51F7
	[PublicizedFrom(EAccessModifier.Private)]
	public void addSaveToEntries(UserDataStorageType _storage, string _saveName, string _worldName, DateTime _lastSaved, WorldState _worldState, bool _isArchived)
	{
		this.allEntries.Add(new XUiC_SavegamesList.ListEntry(_storage, _saveName, _worldName, _lastSaved, _worldState));
	}

	// Token: 0x02000E54 RID: 3668
	[Preserve]
	public class ListEntry : XUiListEntry<XUiC_SavegamesList.ListEntry>
	{
		// Token: 0x17000CCE RID: 3278
		// (get) Token: 0x06007059 RID: 28761 RVA: 0x002B7018 File Offset: 0x002B5218
		public GameMode GameMode
		{
			get
			{
				return GameMode.GetGameModeForId(this.WorldState.activeGameMode);
			}
		}

		// Token: 0x0600705A RID: 28762 RVA: 0x002B702C File Offset: 0x002B522C
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

		// Token: 0x0600705B RID: 28763 RVA: 0x002B70F8 File Offset: 0x002B52F8
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

		// Token: 0x0600705C RID: 28764 RVA: 0x002B7177 File Offset: 0x002B5377
		public string GetSaveDir()
		{
			return GameIO.GetSaveGameDir(this.WorldName, this.SaveName, this.StorageType);
		}

		// Token: 0x0600705D RID: 28765 RVA: 0x002B7190 File Offset: 0x002B5390
		public override int CompareTo(XUiC_SavegamesList.ListEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return -1;
			}
			return -1 * this.LastSaved.CompareTo(_otherEntry.LastSaved);
		}

		// Token: 0x0600705E RID: 28766 RVA: 0x002B71B8 File Offset: 0x002B53B8
		public override bool MatchesSearch(string _searchString)
		{
			return this.SaveName.ContainsCaseInsensitive(_searchString);
		}

		// Token: 0x04005513 RID: 21779
		public readonly UserDataStorageType StorageType;

		// Token: 0x04005514 RID: 21780
		public readonly string SaveName;

		// Token: 0x04005515 RID: 21781
		public readonly string DisplaySaveName;

		// Token: 0x04005516 RID: 21782
		public readonly string WorldName;

		// Token: 0x04005517 RID: 21783
		public readonly DateTime LastSaved;

		// Token: 0x04005518 RID: 21784
		public readonly WorldState WorldState;

		// Token: 0x04005519 RID: 21785
		public readonly bool WorldExists;

		// Token: 0x0400551A RID: 21786
		public readonly VersionInformation SaveVersion;

		// Token: 0x0400551B RID: 21787
		public readonly XUiC_SavegamesList.ListEntry.ESaveCompatibility VersionComparison;

		// Token: 0x0400551C RID: 21788
		public readonly bool Playable;

		// Token: 0x02000E55 RID: 3669
		public enum ESaveCompatibility
		{
			// Token: 0x0400551E RID: 21790
			SameVersion,
			// Token: 0x0400551F RID: 21791
			NewerMinor,
			// Token: 0x04005520 RID: 21792
			Compatible,
			// Token: 0x04005521 RID: 21793
			FutureVersion,
			// Token: 0x04005522 RID: 21794
			OldVersion
		}
	}

	// Token: 0x02000E56 RID: 3670
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_SavegamesList.ListEntry>.XUiC_ListEntry
	{
		// Token: 0x0600705F RID: 28767 RVA: 0x002B71C8 File Offset: 0x002B53C8
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

		// Token: 0x06007060 RID: 28768 RVA: 0x002B727F File Offset: 0x002B547F
		[XuiXmlBinding("savename")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingSaveName()
		{
			XUiC_SavegamesList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.DisplaySaveName : null) ?? "";
		}

		// Token: 0x06007061 RID: 28769 RVA: 0x002B729C File Offset: 0x002B549C
		[XuiXmlBinding("worldname")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingWorldName()
		{
			XUiC_SavegamesList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.WorldName : null) ?? "";
		}

		// Token: 0x06007062 RID: 28770 RVA: 0x002B72B9 File Offset: 0x002B54B9
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

		// Token: 0x06007063 RID: 28771 RVA: 0x002B72E4 File Offset: 0x002B54E4
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

		// Token: 0x06007064 RID: 28772 RVA: 0x002B7324 File Offset: 0x002B5524
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

		// Token: 0x06007065 RID: 28773 RVA: 0x002B736C File Offset: 0x002B556C
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

		// Token: 0x06007066 RID: 28774 RVA: 0x002B73FC File Offset: 0x002B55FC
		[XuiXmlBinding("lastplayed")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingLastPlayed()
		{
			XUiC_SavegamesList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.LastSaved.ToString("yyyy-MM-dd HH:mm") : null) ?? "";
		}

		// Token: 0x06007067 RID: 28775 RVA: 0x002B7431 File Offset: 0x002B5631
		[XuiXmlBinding("usesdatalimit")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingUsesDataLimit()
		{
			return this.entryData != null && PlatformManager.MultiPlatform.UserDataRoaming.IsRoamingOptional && this.entryData.StorageType.UsesDataLimit();
		}

		// Token: 0x06007068 RID: 28776 RVA: 0x002B745E File Offset: 0x002B565E
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

		// Token: 0x06007069 RID: 28777 RVA: 0x002B7488 File Offset: 0x002B5688
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

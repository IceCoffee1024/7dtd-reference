using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000E2F RID: 3631
[Preserve]
public class XUiC_SavegamesList : XUiC_List<XUiC_SavegamesList.ListEntry>
{
	// Token: 0x06006FBB RID: 28603 RVA: 0x002B061F File Offset: 0x002AE81F
	public override void OnOpen()
	{
		base.OnOpen();
		this.RebuildList(false);
	}

	// Token: 0x06006FBC RID: 28604 RVA: 0x002B062E File Offset: 0x002AE82E
	public override void RebuildList(bool _resetFilter = false)
	{
		this.allEntries.Clear();
		GameIO.GetPlayerSaves(new GameIO.FoundSave(this.addSaveToEntries), false);
		this.allEntries.Sort();
		base.RebuildList(_resetFilter);
	}

	// Token: 0x06006FBD RID: 28605 RVA: 0x002B0660 File Offset: 0x002AE860
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

	// Token: 0x06006FBE RID: 28606 RVA: 0x002B06B0 File Offset: 0x002AE8B0
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

	// Token: 0x06006FBF RID: 28607 RVA: 0x002B0723 File Offset: 0x002AE923
	[PublicizedFrom(EAccessModifier.Private)]
	public void addSaveToEntries(UserDataStorageType _storage, string _saveName, string _worldName, DateTime _lastSaved, WorldState _worldState, bool _isArchived)
	{
		this.allEntries.Add(new XUiC_SavegamesList.ListEntry(_storage, _saveName, _worldName, _lastSaved, _worldState));
	}

	// Token: 0x02000E30 RID: 3632
	[Preserve]
	public class ListEntry : XUiListEntry<XUiC_SavegamesList.ListEntry>
	{
		// Token: 0x17000C9A RID: 3226
		// (get) Token: 0x06006FC1 RID: 28609 RVA: 0x002B0744 File Offset: 0x002AE944
		public GameMode GameMode
		{
			get
			{
				return GameMode.GetGameModeForId(this.WorldState.activeGameMode);
			}
		}

		// Token: 0x06006FC2 RID: 28610 RVA: 0x002B0758 File Offset: 0x002AE958
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

		// Token: 0x06006FC3 RID: 28611 RVA: 0x002B0824 File Offset: 0x002AEA24
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

		// Token: 0x06006FC4 RID: 28612 RVA: 0x002B08A3 File Offset: 0x002AEAA3
		public string GetSaveDir()
		{
			return GameIO.GetSaveGameDir(this.WorldName, this.SaveName, this.StorageType);
		}

		// Token: 0x06006FC5 RID: 28613 RVA: 0x002B08BC File Offset: 0x002AEABC
		public override int CompareTo(XUiC_SavegamesList.ListEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return -1;
			}
			return -1 * this.LastSaved.CompareTo(_otherEntry.LastSaved);
		}

		// Token: 0x06006FC6 RID: 28614 RVA: 0x002B08E4 File Offset: 0x002AEAE4
		public override bool MatchesSearch(string _searchString)
		{
			return this.SaveName.ContainsCaseInsensitive(_searchString);
		}

		// Token: 0x04005438 RID: 21560
		public readonly UserDataStorageType StorageType;

		// Token: 0x04005439 RID: 21561
		public readonly string SaveName;

		// Token: 0x0400543A RID: 21562
		public readonly string DisplaySaveName;

		// Token: 0x0400543B RID: 21563
		public readonly string WorldName;

		// Token: 0x0400543C RID: 21564
		public readonly DateTime LastSaved;

		// Token: 0x0400543D RID: 21565
		public readonly WorldState WorldState;

		// Token: 0x0400543E RID: 21566
		public readonly bool WorldExists;

		// Token: 0x0400543F RID: 21567
		public readonly VersionInformation SaveVersion;

		// Token: 0x04005440 RID: 21568
		public readonly XUiC_SavegamesList.ListEntry.ESaveCompatibility VersionComparison;

		// Token: 0x04005441 RID: 21569
		public readonly bool Playable;

		// Token: 0x02000E31 RID: 3633
		public enum ESaveCompatibility
		{
			// Token: 0x04005443 RID: 21571
			SameVersion,
			// Token: 0x04005444 RID: 21572
			NewerMinor,
			// Token: 0x04005445 RID: 21573
			Compatible,
			// Token: 0x04005446 RID: 21574
			FutureVersion,
			// Token: 0x04005447 RID: 21575
			OldVersion
		}
	}

	// Token: 0x02000E32 RID: 3634
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_SavegamesList.ListEntry>.XUiC_ListEntry
	{
		// Token: 0x06006FC7 RID: 28615 RVA: 0x002B08F4 File Offset: 0x002AEAF4
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

		// Token: 0x06006FC8 RID: 28616 RVA: 0x002B09AB File Offset: 0x002AEBAB
		[XuiXmlBinding("savename")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingSaveName()
		{
			XUiC_SavegamesList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.DisplaySaveName : null) ?? "";
		}

		// Token: 0x06006FC9 RID: 28617 RVA: 0x002B09C8 File Offset: 0x002AEBC8
		[XuiXmlBinding("worldname")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingWorldName()
		{
			XUiC_SavegamesList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.WorldName : null) ?? "";
		}

		// Token: 0x06006FCA RID: 28618 RVA: 0x002B09E5 File Offset: 0x002AEBE5
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

		// Token: 0x06006FCB RID: 28619 RVA: 0x002B0A10 File Offset: 0x002AEC10
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

		// Token: 0x06006FCC RID: 28620 RVA: 0x002B0A50 File Offset: 0x002AEC50
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

		// Token: 0x06006FCD RID: 28621 RVA: 0x002B0A98 File Offset: 0x002AEC98
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

		// Token: 0x06006FCE RID: 28622 RVA: 0x002B0B28 File Offset: 0x002AED28
		[XuiXmlBinding("lastplayed")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingLastPlayed()
		{
			XUiC_SavegamesList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.LastSaved.ToString("yyyy-MM-dd HH:mm") : null) ?? "";
		}

		// Token: 0x06006FCF RID: 28623 RVA: 0x002B0B5D File Offset: 0x002AED5D
		[XuiXmlBinding("usesdatalimit")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingUsesDataLimit()
		{
			return this.entryData != null && PlatformManager.MultiPlatform.UserDataRoaming.IsRoamingOptional && this.entryData.StorageType.UsesDataLimit();
		}

		// Token: 0x06006FD0 RID: 28624 RVA: 0x002B0B8A File Offset: 0x002AED8A
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

		// Token: 0x06006FD1 RID: 28625 RVA: 0x002B0BB4 File Offset: 0x002AEDB4
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

using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000E5E RID: 3678
[Preserve]
public class XUiC_SavegamesList : XUiC_List<XUiC_SavegamesList.ListEntry>
{
	// Token: 0x06007078 RID: 28792 RVA: 0x002B845F File Offset: 0x002B665F
	public override void OnOpen()
	{
		base.OnOpen();
		this.RebuildList(false);
	}

	// Token: 0x06007079 RID: 28793 RVA: 0x002B846E File Offset: 0x002B666E
	public override void RebuildList(bool _resetFilter = false)
	{
		this.allEntries.Clear();
		GameIO.GetPlayerSaves(new GameIO.FoundSave(this.addSaveToEntries), false);
		this.allEntries.Sort();
		base.RebuildList(_resetFilter);
	}

	// Token: 0x0600707A RID: 28794 RVA: 0x002B84A0 File Offset: 0x002B66A0
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

	// Token: 0x0600707B RID: 28795 RVA: 0x002B84F0 File Offset: 0x002B66F0
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

	// Token: 0x0600707C RID: 28796 RVA: 0x002B8563 File Offset: 0x002B6763
	[PublicizedFrom(EAccessModifier.Private)]
	public void addSaveToEntries(UserDataStorageType _storage, string _saveName, string _worldName, DateTime _lastSaved, WorldState _worldState, bool _isArchived)
	{
		this.allEntries.Add(new XUiC_SavegamesList.ListEntry(_storage, _saveName, _worldName, _lastSaved, _worldState));
	}

	// Token: 0x02000E5F RID: 3679
	[Preserve]
	public class ListEntry : XUiListEntry<XUiC_SavegamesList.ListEntry>
	{
		// Token: 0x17000CD4 RID: 3284
		// (get) Token: 0x0600707E RID: 28798 RVA: 0x002B8584 File Offset: 0x002B6784
		public GameMode GameMode
		{
			get
			{
				return GameMode.GetGameModeForId(this.WorldState.activeGameMode);
			}
		}

		// Token: 0x0600707F RID: 28799 RVA: 0x002B8598 File Offset: 0x002B6798
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

		// Token: 0x06007080 RID: 28800 RVA: 0x002B8664 File Offset: 0x002B6864
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

		// Token: 0x06007081 RID: 28801 RVA: 0x002B86E3 File Offset: 0x002B68E3
		public string GetSaveDir()
		{
			return GameIO.GetSaveGameDir(this.WorldName, this.SaveName, this.StorageType);
		}

		// Token: 0x06007082 RID: 28802 RVA: 0x002B86FC File Offset: 0x002B68FC
		public override int CompareTo(XUiC_SavegamesList.ListEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return -1;
			}
			return -1 * this.LastSaved.CompareTo(_otherEntry.LastSaved);
		}

		// Token: 0x06007083 RID: 28803 RVA: 0x002B8724 File Offset: 0x002B6924
		public override bool MatchesSearch(string _searchString)
		{
			return this.SaveName.ContainsCaseInsensitive(_searchString);
		}

		// Token: 0x04005538 RID: 21816
		public readonly UserDataStorageType StorageType;

		// Token: 0x04005539 RID: 21817
		public readonly string SaveName;

		// Token: 0x0400553A RID: 21818
		public readonly string DisplaySaveName;

		// Token: 0x0400553B RID: 21819
		public readonly string WorldName;

		// Token: 0x0400553C RID: 21820
		public readonly DateTime LastSaved;

		// Token: 0x0400553D RID: 21821
		public readonly WorldState WorldState;

		// Token: 0x0400553E RID: 21822
		public readonly bool WorldExists;

		// Token: 0x0400553F RID: 21823
		public readonly VersionInformation SaveVersion;

		// Token: 0x04005540 RID: 21824
		public readonly XUiC_SavegamesList.ListEntry.ESaveCompatibility VersionComparison;

		// Token: 0x04005541 RID: 21825
		public readonly bool Playable;

		// Token: 0x02000E60 RID: 3680
		public enum ESaveCompatibility
		{
			// Token: 0x04005543 RID: 21827
			SameVersion,
			// Token: 0x04005544 RID: 21828
			NewerMinor,
			// Token: 0x04005545 RID: 21829
			Compatible,
			// Token: 0x04005546 RID: 21830
			FutureVersion,
			// Token: 0x04005547 RID: 21831
			OldVersion
		}
	}

	// Token: 0x02000E61 RID: 3681
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_SavegamesList.ListEntry>.XUiC_ListEntry
	{
		// Token: 0x06007084 RID: 28804 RVA: 0x002B8734 File Offset: 0x002B6934
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

		// Token: 0x06007085 RID: 28805 RVA: 0x002B87EB File Offset: 0x002B69EB
		[XuiXmlBinding("savename")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingSaveName()
		{
			XUiC_SavegamesList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.DisplaySaveName : null) ?? "";
		}

		// Token: 0x06007086 RID: 28806 RVA: 0x002B8808 File Offset: 0x002B6A08
		[XuiXmlBinding("worldname")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingWorldName()
		{
			XUiC_SavegamesList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.WorldName : null) ?? "";
		}

		// Token: 0x06007087 RID: 28807 RVA: 0x002B8825 File Offset: 0x002B6A25
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

		// Token: 0x06007088 RID: 28808 RVA: 0x002B8850 File Offset: 0x002B6A50
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

		// Token: 0x06007089 RID: 28809 RVA: 0x002B8890 File Offset: 0x002B6A90
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

		// Token: 0x0600708A RID: 28810 RVA: 0x002B88D8 File Offset: 0x002B6AD8
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

		// Token: 0x0600708B RID: 28811 RVA: 0x002B8968 File Offset: 0x002B6B68
		[XuiXmlBinding("lastplayed")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingLastPlayed()
		{
			XUiC_SavegamesList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.LastSaved.ToString("yyyy-MM-dd HH:mm") : null) ?? "";
		}

		// Token: 0x0600708C RID: 28812 RVA: 0x002B899D File Offset: 0x002B6B9D
		[XuiXmlBinding("usesdatalimit")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingUsesDataLimit()
		{
			return this.entryData != null && PlatformManager.MultiPlatform.UserDataRoaming.IsRoamingOptional && this.entryData.StorageType.UsesDataLimit();
		}

		// Token: 0x0600708D RID: 28813 RVA: 0x002B89CA File Offset: 0x002B6BCA
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

		// Token: 0x0600708E RID: 28814 RVA: 0x002B89F4 File Offset: 0x002B6BF4
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

using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000E4D RID: 3661
[Preserve]
public class XUiC_WorldList : XUiC_List<XUiC_WorldList.WorldListEntry>
{
	// Token: 0x060070E6 RID: 28902 RVA: 0x002B57BC File Offset: 0x002B39BC
	[PublicizedFrom(EAccessModifier.Private)]
	public long getWorldMemory(XUiC_WorldList.WorldListEntry _worldEntry)
	{
		PathAbstractions.AbstractedLocation location = _worldEntry.Location;
		if (!this.worldDataSizeCache.ContainsKey(location))
		{
			this.worldDataSizeCache.Add(location, GameIO.GetDirectorySize(location.FullPath, true));
		}
		return this.worldDataSizeCache[location];
	}

	// Token: 0x060070E7 RID: 28903 RVA: 0x002B5803 File Offset: 0x002B3A03
	public override void OnOpen()
	{
		base.OnOpen();
		this.worldDataSizeCache.Clear();
		this.RebuildList(false);
	}

	// Token: 0x060070E8 RID: 28904 RVA: 0x002B581D File Offset: 0x002B3A1D
	public override void OnClose()
	{
		base.OnClose();
		this.worldDataSizeCache.Clear();
	}

	// Token: 0x060070E9 RID: 28905 RVA: 0x002B5830 File Offset: 0x002B3A30
	public override void RebuildList(bool _resetFilter = false)
	{
		this.worldDataSizeCache.Clear();
		this.allEntries.Clear();
		foreach (PathAbstractions.AbstractedLocation abstractedLocation in PathAbstractions.WorldsSearchPaths.GetAvailablePathsList(null, false, null))
		{
			if (!XUiC_WorldList.forbiddenWorlds.ContainsWithComparer(abstractedLocation.Name, StringComparer.OrdinalIgnoreCase) && (this.userDataStorageTypeFilter.Count <= 0 || this.userDataStorageTypeFilter.Contains(abstractedLocation.StorageType)))
			{
				GameUtils.WorldInfo worldInfo = GameUtils.WorldInfo.LoadWorldInfo(abstractedLocation);
				if (worldInfo != null)
				{
					this.allEntries.Add(new XUiC_WorldList.WorldListEntry(abstractedLocation, GameIO.IsWorldGenerated(abstractedLocation.Name, abstractedLocation.StorageType), worldInfo.GameVersionCreated));
				}
			}
		}
		this.allEntries.Sort();
		base.RebuildList(_resetFilter);
	}

	// Token: 0x060070EA RID: 28906 RVA: 0x002B5920 File Offset: 0x002B3B20
	public void SetUserDataStorageTypeFilter(params UserDataStorageType[] _storageTypes)
	{
		this.userDataStorageTypeFilter.Clear();
		foreach (UserDataStorageType userDataStorageType in _storageTypes)
		{
			this.userDataStorageTypeFilter.Add(userDataStorageType);
		}
		this.filteredEntries.Clear();
		this.FilterResults(this.previousMatch);
		this.RefreshView(false, true);
	}

	// Token: 0x060070EB RID: 28907 RVA: 0x002B5978 File Offset: 0x002B3B78
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void FilterResults(string _textMatch)
	{
		base.FilterResults(_textMatch);
		if (this.userDataStorageTypeFilter.Count == 0)
		{
			return;
		}
		for (int i = this.filteredEntries.Count - 1; i >= 0; i--)
		{
			if (!this.userDataStorageTypeFilter.Contains(this.filteredEntries[i].Location.StorageType))
			{
				this.filteredEntries.RemoveAt(i);
				i--;
			}
		}
	}

	// Token: 0x060070EC RID: 28908 RVA: 0x002B59E8 File Offset: 0x002B3BE8
	public bool SelectByName(string _name)
	{
		if (string.IsNullOrEmpty(_name))
		{
			return false;
		}
		for (int i = 0; i < this.filteredEntries.Count; i++)
		{
			if (this.filteredEntries[i].Location.Name.Equals(_name, StringComparison.OrdinalIgnoreCase))
			{
				base.SelectedEntryIndex = i;
				return true;
			}
		}
		return false;
	}

	// Token: 0x040054F1 RID: 21745
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<string> forbiddenWorlds = new List<string> { "Empty", "Playtesting" };

	// Token: 0x040054F2 RID: 21746
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<UserDataStorageType> userDataStorageTypeFilter = new List<UserDataStorageType>();

	// Token: 0x040054F3 RID: 21747
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<PathAbstractions.AbstractedLocation, long> worldDataSizeCache = new Dictionary<PathAbstractions.AbstractedLocation, long>();

	// Token: 0x02000E4E RID: 3662
	[Preserve]
	public class WorldListEntry : XUiListEntry<XUiC_WorldList.WorldListEntry>
	{
		// Token: 0x060070EF RID: 28911 RVA: 0x002B5A7E File Offset: 0x002B3C7E
		public WorldListEntry(PathAbstractions.AbstractedLocation _location, bool _generatedWorld, VersionInformation _version)
		{
			this.Location = _location;
			this.GeneratedWorld = _generatedWorld;
			this.Version = _version;
			this.VersionComparison = this.Version.CompareToRunningBuild();
		}

		// Token: 0x060070F0 RID: 28912 RVA: 0x002B5AAC File Offset: 0x002B3CAC
		public override int CompareTo(XUiC_WorldList.WorldListEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return -1;
			}
			return this.Location.CompareTo(_otherEntry.Location);
		}

		// Token: 0x060070F1 RID: 28913 RVA: 0x002B5AC4 File Offset: 0x002B3CC4
		public override bool MatchesSearch(string _searchString)
		{
			return this.Location.Name.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x040054F4 RID: 21748
		public readonly PathAbstractions.AbstractedLocation Location;

		// Token: 0x040054F5 RID: 21749
		public readonly bool GeneratedWorld;

		// Token: 0x040054F6 RID: 21750
		public readonly VersionInformation Version;

		// Token: 0x040054F7 RID: 21751
		public readonly VersionInformation.EVersionComparisonResult VersionComparison;
	}

	// Token: 0x02000E4F RID: 3663
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_WorldList.WorldListEntry>.XUiC_ListEntry
	{
		// Token: 0x060070F2 RID: 28914 RVA: 0x002B5ADE File Offset: 0x002B3CDE
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			if (this.entryData != null)
			{
				return this.entryData.Location.Name;
			}
			return "";
		}

		// Token: 0x060070F3 RID: 28915 RVA: 0x002B5AFE File Offset: 0x002B3CFE
		[XuiXmlBinding("location")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingLocation()
		{
			if (this.entryData == null)
			{
				return "";
			}
			return this.entryData.Location.GetLocationTypeDisplayString();
		}

		// Token: 0x060070F4 RID: 28916 RVA: 0x002B5B20 File Offset: 0x002B3D20
		[XuiXmlBinding("version")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingVersion()
		{
			if (this.entryData == null)
			{
				return "";
			}
			if (!this.entryData.Version.IsValid)
			{
				return VersionInformation.EGameReleaseType.V.ToString() + " " + 3.ToString();
			}
			return this.entryData.Version.LongStringNoBuild;
		}

		// Token: 0x060070F5 RID: 28917 RVA: 0x002B5B80 File Offset: 0x002B3D80
		[XuiXmlBinding("versionvalid")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingValidVersion()
		{
			return this.entryData == null || !this.entryData.Version.IsValid || this.entryData.Version.Major == 3;
		}

		// Token: 0x060070F6 RID: 28918 RVA: 0x002B5BB3 File Offset: 0x002B3DB3
		[XuiXmlBinding("worldsize")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingWorldSize()
		{
			if (this.entryData != null)
			{
				return ValueDisplayFormatters.MemoryMiB(this.parentList.getWorldMemory(this.entryData));
			}
			return "";
		}

		// Token: 0x060070F7 RID: 28919 RVA: 0x002B5BDC File Offset: 0x002B3DDC
		[XuiXmlBinding("versionstate")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingVersionState()
		{
			if (this.entryData == null)
			{
				return "";
			}
			if (!this.entryData.Version.IsValid)
			{
				return "same";
			}
			string text;
			switch (this.entryData.VersionComparison)
			{
			case VersionInformation.EVersionComparisonResult.SameBuild:
				text = "same";
				break;
			case VersionInformation.EVersionComparisonResult.SameMinor:
				text = "same";
				break;
			case VersionInformation.EVersionComparisonResult.NewerMinor:
				text = "compatible";
				break;
			case VersionInformation.EVersionComparisonResult.OlderMinor:
				text = "compatible";
				break;
			default:
				text = "incompatible";
				break;
			}
			return text;
		}

		// Token: 0x040054F8 RID: 21752
		[XuiBindParent(true)]
		[PublicizedFrom(EAccessModifier.Private)]
		public XUiC_WorldList parentList;
	}
}

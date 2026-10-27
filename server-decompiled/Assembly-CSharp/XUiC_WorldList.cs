using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000E7C RID: 3708
[Preserve]
public class XUiC_WorldList : XUiC_List<XUiC_WorldList.WorldListEntry>
{
	// Token: 0x060071A3 RID: 29091 RVA: 0x002BD5FC File Offset: 0x002BB7FC
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

	// Token: 0x060071A4 RID: 29092 RVA: 0x002BD643 File Offset: 0x002BB843
	public override void OnOpen()
	{
		base.OnOpen();
		this.worldDataSizeCache.Clear();
		this.RebuildList(false);
	}

	// Token: 0x060071A5 RID: 29093 RVA: 0x002BD65D File Offset: 0x002BB85D
	public override void OnClose()
	{
		base.OnClose();
		this.worldDataSizeCache.Clear();
	}

	// Token: 0x060071A6 RID: 29094 RVA: 0x002BD670 File Offset: 0x002BB870
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

	// Token: 0x060071A7 RID: 29095 RVA: 0x002BD760 File Offset: 0x002BB960
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

	// Token: 0x060071A8 RID: 29096 RVA: 0x002BD7B8 File Offset: 0x002BB9B8
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

	// Token: 0x060071A9 RID: 29097 RVA: 0x002BD828 File Offset: 0x002BBA28
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

	// Token: 0x040055F1 RID: 22001
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<string> forbiddenWorlds = new List<string> { "Empty", "Playtesting" };

	// Token: 0x040055F2 RID: 22002
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<UserDataStorageType> userDataStorageTypeFilter = new List<UserDataStorageType>();

	// Token: 0x040055F3 RID: 22003
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<PathAbstractions.AbstractedLocation, long> worldDataSizeCache = new Dictionary<PathAbstractions.AbstractedLocation, long>();

	// Token: 0x02000E7D RID: 3709
	[Preserve]
	public class WorldListEntry : XUiListEntry<XUiC_WorldList.WorldListEntry>
	{
		// Token: 0x060071AC RID: 29100 RVA: 0x002BD8BE File Offset: 0x002BBABE
		public WorldListEntry(PathAbstractions.AbstractedLocation _location, bool _generatedWorld, VersionInformation _version)
		{
			this.Location = _location;
			this.GeneratedWorld = _generatedWorld;
			this.Version = _version;
			this.VersionComparison = this.Version.CompareToRunningBuild();
		}

		// Token: 0x060071AD RID: 29101 RVA: 0x002BD8EC File Offset: 0x002BBAEC
		public override int CompareTo(XUiC_WorldList.WorldListEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return -1;
			}
			return this.Location.CompareTo(_otherEntry.Location);
		}

		// Token: 0x060071AE RID: 29102 RVA: 0x002BD904 File Offset: 0x002BBB04
		public override bool MatchesSearch(string _searchString)
		{
			return this.Location.Name.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x040055F4 RID: 22004
		public readonly PathAbstractions.AbstractedLocation Location;

		// Token: 0x040055F5 RID: 22005
		public readonly bool GeneratedWorld;

		// Token: 0x040055F6 RID: 22006
		public readonly VersionInformation Version;

		// Token: 0x040055F7 RID: 22007
		public readonly VersionInformation.EVersionComparisonResult VersionComparison;
	}

	// Token: 0x02000E7E RID: 3710
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_WorldList.WorldListEntry>.XUiC_ListEntry
	{
		// Token: 0x060071AF RID: 29103 RVA: 0x002BD91E File Offset: 0x002BBB1E
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

		// Token: 0x060071B0 RID: 29104 RVA: 0x002BD93E File Offset: 0x002BBB3E
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

		// Token: 0x060071B1 RID: 29105 RVA: 0x002BD960 File Offset: 0x002BBB60
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

		// Token: 0x060071B2 RID: 29106 RVA: 0x002BD9C0 File Offset: 0x002BBBC0
		[XuiXmlBinding("versionvalid")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingValidVersion()
		{
			return this.entryData == null || !this.entryData.Version.IsValid || this.entryData.Version.Major == 3;
		}

		// Token: 0x060071B3 RID: 29107 RVA: 0x002BD9F3 File Offset: 0x002BBBF3
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

		// Token: 0x060071B4 RID: 29108 RVA: 0x002BDA1C File Offset: 0x002BBC1C
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

		// Token: 0x040055F8 RID: 22008
		[XuiBindParent(true)]
		[PublicizedFrom(EAccessModifier.Private)]
		public XUiC_WorldList parentList;
	}
}

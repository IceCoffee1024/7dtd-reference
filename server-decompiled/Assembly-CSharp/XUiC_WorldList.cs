using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000E71 RID: 3697
[Preserve]
public class XUiC_WorldList : XUiC_List<XUiC_WorldList.WorldListEntry>
{
	// Token: 0x0600717E RID: 29054 RVA: 0x002BC090 File Offset: 0x002BA290
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

	// Token: 0x0600717F RID: 29055 RVA: 0x002BC0D7 File Offset: 0x002BA2D7
	public override void OnOpen()
	{
		base.OnOpen();
		this.worldDataSizeCache.Clear();
		this.RebuildList(false);
	}

	// Token: 0x06007180 RID: 29056 RVA: 0x002BC0F1 File Offset: 0x002BA2F1
	public override void OnClose()
	{
		base.OnClose();
		this.worldDataSizeCache.Clear();
	}

	// Token: 0x06007181 RID: 29057 RVA: 0x002BC104 File Offset: 0x002BA304
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

	// Token: 0x06007182 RID: 29058 RVA: 0x002BC1F4 File Offset: 0x002BA3F4
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

	// Token: 0x06007183 RID: 29059 RVA: 0x002BC24C File Offset: 0x002BA44C
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

	// Token: 0x06007184 RID: 29060 RVA: 0x002BC2BC File Offset: 0x002BA4BC
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

	// Token: 0x040055CC RID: 21964
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<string> forbiddenWorlds = new List<string> { "Empty", "Playtesting" };

	// Token: 0x040055CD RID: 21965
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<UserDataStorageType> userDataStorageTypeFilter = new List<UserDataStorageType>();

	// Token: 0x040055CE RID: 21966
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<PathAbstractions.AbstractedLocation, long> worldDataSizeCache = new Dictionary<PathAbstractions.AbstractedLocation, long>();

	// Token: 0x02000E72 RID: 3698
	[Preserve]
	public class WorldListEntry : XUiListEntry<XUiC_WorldList.WorldListEntry>
	{
		// Token: 0x06007187 RID: 29063 RVA: 0x002BC352 File Offset: 0x002BA552
		public WorldListEntry(PathAbstractions.AbstractedLocation _location, bool _generatedWorld, VersionInformation _version)
		{
			this.Location = _location;
			this.GeneratedWorld = _generatedWorld;
			this.Version = _version;
			this.VersionComparison = this.Version.CompareToRunningBuild();
		}

		// Token: 0x06007188 RID: 29064 RVA: 0x002BC380 File Offset: 0x002BA580
		public override int CompareTo(XUiC_WorldList.WorldListEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return -1;
			}
			return this.Location.CompareTo(_otherEntry.Location);
		}

		// Token: 0x06007189 RID: 29065 RVA: 0x002BC398 File Offset: 0x002BA598
		public override bool MatchesSearch(string _searchString)
		{
			return this.Location.Name.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x040055CF RID: 21967
		public readonly PathAbstractions.AbstractedLocation Location;

		// Token: 0x040055D0 RID: 21968
		public readonly bool GeneratedWorld;

		// Token: 0x040055D1 RID: 21969
		public readonly VersionInformation Version;

		// Token: 0x040055D2 RID: 21970
		public readonly VersionInformation.EVersionComparisonResult VersionComparison;
	}

	// Token: 0x02000E73 RID: 3699
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_WorldList.WorldListEntry>.XUiC_ListEntry
	{
		// Token: 0x0600718A RID: 29066 RVA: 0x002BC3B2 File Offset: 0x002BA5B2
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

		// Token: 0x0600718B RID: 29067 RVA: 0x002BC3D2 File Offset: 0x002BA5D2
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

		// Token: 0x0600718C RID: 29068 RVA: 0x002BC3F4 File Offset: 0x002BA5F4
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

		// Token: 0x0600718D RID: 29069 RVA: 0x002BC454 File Offset: 0x002BA654
		[XuiXmlBinding("versionvalid")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingValidVersion()
		{
			return this.entryData == null || !this.entryData.Version.IsValid || this.entryData.Version.Major == 3;
		}

		// Token: 0x0600718E RID: 29070 RVA: 0x002BC487 File Offset: 0x002BA687
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

		// Token: 0x0600718F RID: 29071 RVA: 0x002BC4B0 File Offset: 0x002BA6B0
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

		// Token: 0x040055D3 RID: 21971
		[XuiBindParent(true)]
		[PublicizedFrom(EAccessModifier.Private)]
		public XUiC_WorldList parentList;
	}
}

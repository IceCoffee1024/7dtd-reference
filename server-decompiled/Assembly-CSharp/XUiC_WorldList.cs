using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000E62 RID: 3682
[Preserve]
public class XUiC_WorldList : XUiC_List<XUiC_WorldList.WorldListEntry>
{
	// Token: 0x06007186 RID: 29062 RVA: 0x002B8CE0 File Offset: 0x002B6EE0
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

	// Token: 0x06007187 RID: 29063 RVA: 0x002B8D27 File Offset: 0x002B6F27
	public override void OnOpen()
	{
		base.OnOpen();
		this.worldDataSizeCache.Clear();
		this.RebuildList(false);
	}

	// Token: 0x06007188 RID: 29064 RVA: 0x002B8D41 File Offset: 0x002B6F41
	public override void OnClose()
	{
		base.OnClose();
		this.worldDataSizeCache.Clear();
	}

	// Token: 0x06007189 RID: 29065 RVA: 0x002B8D54 File Offset: 0x002B6F54
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

	// Token: 0x0600718A RID: 29066 RVA: 0x002B8E44 File Offset: 0x002B7044
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

	// Token: 0x0600718B RID: 29067 RVA: 0x002B8E9C File Offset: 0x002B709C
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

	// Token: 0x0600718C RID: 29068 RVA: 0x002B8F0C File Offset: 0x002B710C
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

	// Token: 0x04005595 RID: 21909
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<string> forbiddenWorlds = new List<string> { "Empty", "Playtesting" };

	// Token: 0x04005596 RID: 21910
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<UserDataStorageType> userDataStorageTypeFilter = new List<UserDataStorageType>();

	// Token: 0x04005597 RID: 21911
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<PathAbstractions.AbstractedLocation, long> worldDataSizeCache = new Dictionary<PathAbstractions.AbstractedLocation, long>();

	// Token: 0x02000E63 RID: 3683
	[Preserve]
	public class WorldListEntry : XUiListEntry<XUiC_WorldList.WorldListEntry>
	{
		// Token: 0x0600718F RID: 29071 RVA: 0x002B8FA2 File Offset: 0x002B71A2
		public WorldListEntry(PathAbstractions.AbstractedLocation _location, bool _generatedWorld, VersionInformation _version)
		{
			this.Location = _location;
			this.GeneratedWorld = _generatedWorld;
			this.Version = _version;
			this.VersionComparison = this.Version.CompareToRunningBuild();
		}

		// Token: 0x06007190 RID: 29072 RVA: 0x002B8FD0 File Offset: 0x002B71D0
		public override int CompareTo(XUiC_WorldList.WorldListEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return -1;
			}
			return this.Location.CompareTo(_otherEntry.Location);
		}

		// Token: 0x06007191 RID: 29073 RVA: 0x002B8FE8 File Offset: 0x002B71E8
		public override bool MatchesSearch(string _searchString)
		{
			return this.Location.Name.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x04005598 RID: 21912
		public readonly PathAbstractions.AbstractedLocation Location;

		// Token: 0x04005599 RID: 21913
		public readonly bool GeneratedWorld;

		// Token: 0x0400559A RID: 21914
		public readonly VersionInformation Version;

		// Token: 0x0400559B RID: 21915
		public readonly VersionInformation.EVersionComparisonResult VersionComparison;
	}

	// Token: 0x02000E64 RID: 3684
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_WorldList.WorldListEntry>.XUiC_ListEntry
	{
		// Token: 0x06007192 RID: 29074 RVA: 0x002B9002 File Offset: 0x002B7202
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

		// Token: 0x06007193 RID: 29075 RVA: 0x002B9022 File Offset: 0x002B7222
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

		// Token: 0x06007194 RID: 29076 RVA: 0x002B9044 File Offset: 0x002B7244
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

		// Token: 0x06007195 RID: 29077 RVA: 0x002B90A4 File Offset: 0x002B72A4
		[XuiXmlBinding("versionvalid")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingValidVersion()
		{
			return this.entryData == null || !this.entryData.Version.IsValid || this.entryData.Version.Major == 3;
		}

		// Token: 0x06007196 RID: 29078 RVA: 0x002B90D7 File Offset: 0x002B72D7
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

		// Token: 0x06007197 RID: 29079 RVA: 0x002B9100 File Offset: 0x002B7300
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

		// Token: 0x0400559C RID: 21916
		[XuiBindParent(true)]
		[PublicizedFrom(EAccessModifier.Private)]
		public XUiC_WorldList parentList;
	}
}

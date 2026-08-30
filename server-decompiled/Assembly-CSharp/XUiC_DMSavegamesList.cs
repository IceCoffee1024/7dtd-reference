using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000D71 RID: 3441
[Preserve]
public class XUiC_DMSavegamesList : XUiC_DMBaseList<XUiC_DMSavegamesList.ListEntry>
{
	// Token: 0x17000B13 RID: 2835
	// (get) Token: 0x0600686B RID: 26731 RVA: 0x00293D97 File Offset: 0x00291F97
	// (set) Token: 0x0600686C RID: 26732 RVA: 0x00293D9F File Offset: 0x00291F9F
	[XuiXmlAttribute("matching_version_color", false)]
	public string MatchingVersionColor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000B14 RID: 2836
	// (get) Token: 0x0600686D RID: 26733 RVA: 0x00293DA8 File Offset: 0x00291FA8
	// (set) Token: 0x0600686E RID: 26734 RVA: 0x00293DB0 File Offset: 0x00291FB0
	[XuiXmlAttribute("compatible_version_color", false)]
	public string CompatibleVersionColor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000B15 RID: 2837
	// (get) Token: 0x0600686F RID: 26735 RVA: 0x00293DB9 File Offset: 0x00291FB9
	// (set) Token: 0x06006870 RID: 26736 RVA: 0x00293DC1 File Offset: 0x00291FC1
	[XuiXmlAttribute("incompatible_version_color", false)]
	public string IncompatibleVersionColor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06006871 RID: 26737 RVA: 0x000027FC File Offset: 0x000009FC
	public override void RebuildList(bool _resetFilter = false)
	{
	}

	// Token: 0x06006872 RID: 26738 RVA: 0x00293DCC File Offset: 0x00291FCC
	public void RebuildList(IReadOnlyCollection<SaveInfoProvider.SaveEntryInfo> _saveEntryInfos, bool _resetFilter = false)
	{
		this.allEntries.Clear();
		foreach (SaveInfoProvider.SaveEntryInfo saveEntryInfo in _saveEntryInfos)
		{
			this.allEntries.Add(new XUiC_DMSavegamesList.ListEntry(saveEntryInfo, this.MatchingVersionColor, this.CompatibleVersionColor, this.IncompatibleVersionColor));
		}
		this.allEntries.Sort();
		base.RebuildList(_resetFilter);
	}

	// Token: 0x06006873 RID: 26739 RVA: 0x00293E50 File Offset: 0x00292050
	public void ClearList()
	{
		this.allEntries.Clear();
		this.RebuildList(true);
	}

	// Token: 0x06006874 RID: 26740 RVA: 0x00293E64 File Offset: 0x00292064
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

	// Token: 0x06006875 RID: 26741 RVA: 0x00293EB3 File Offset: 0x002920B3
	public void SetWorldFilter(string _worldKey)
	{
		this.worldFilter = _worldKey;
		this.filteredEntries.Clear();
		this.FilterResults(this.previousMatch);
		this.RefreshView(false, true);
	}

	// Token: 0x06006876 RID: 26742 RVA: 0x00293EDC File Offset: 0x002920DC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void FilterResults(string _textMatch)
	{
		base.FilterResults(_textMatch);
		if (this.worldFilter == null)
		{
			return;
		}
		for (int i = 0; i < this.filteredEntries.Count; i++)
		{
			if (this.filteredEntries[i].WorldKey != this.worldFilter)
			{
				this.filteredEntries.RemoveAt(i);
				i--;
			}
		}
	}

	// Token: 0x06006877 RID: 26743 RVA: 0x00293F3D File Offset: 0x0029213D
	public IEnumerable<XUiC_DMSavegamesList.ListEntry> GetSavesInWorld(string _worldKey)
	{
		if (string.IsNullOrEmpty(_worldKey))
		{
			yield break;
		}
		int num;
		for (int i = 0; i < this.allEntries.Count; i = num + 1)
		{
			if (this.allEntries[i].WorldKey == _worldKey)
			{
				yield return this.allEntries[i];
			}
			num = i;
		}
		yield break;
	}

	// Token: 0x04005013 RID: 20499
	[PublicizedFrom(EAccessModifier.Private)]
	public string worldFilter;

	// Token: 0x02000D72 RID: 3442
	[Preserve]
	public class ListEntry : XUiListEntry<XUiC_DMSavegamesList.ListEntry>
	{
		// Token: 0x06006879 RID: 26745 RVA: 0x00293F5C File Offset: 0x0029215C
		public ListEntry(SaveInfoProvider.SaveEntryInfo _saveEntryInfo, string _matchingColor = "255,255,255", string _compatibleColor = "255,255,255", string _incompatibleColor = "255,255,255")
		{
			this.SaveEntryInfo = _saveEntryInfo;
			this.SaveName = _saveEntryInfo.Name;
			this.DisplayName = _saveEntryInfo.DisplayName;
			this.WorldKey = _saveEntryInfo.WorldEntry.WorldKey;
			this.SaveDirectory = _saveEntryInfo.SaveDir;
			this.LastSaved = _saveEntryInfo.LastSaved;
			this.Version = _saveEntryInfo.Version;
			VersionInformation version = this.Version;
			this.VersionComparison = ((version != null) ? version.CompareToRunningBuild() : VersionInformation.EVersionComparisonResult.SameBuild);
			this.MatchingColor = _matchingColor;
			this.CompatibleColor = _compatibleColor;
			this.IncompatibleColor = _incompatibleColor;
		}

		// Token: 0x0600687A RID: 26746 RVA: 0x00293FF1 File Offset: 0x002921F1
		public override int CompareTo(XUiC_DMSavegamesList.ListEntry _otherEntry)
		{
			if (_otherEntry != null)
			{
				return this.SaveEntryInfo.CompareTo(_otherEntry.SaveEntryInfo);
			}
			return 1;
		}

		// Token: 0x0600687B RID: 26747 RVA: 0x00294009 File Offset: 0x00292209
		public override bool MatchesSearch(string _searchString)
		{
			return this.SaveName.ContainsCaseInsensitive(_searchString);
		}

		// Token: 0x04005014 RID: 20500
		public readonly string SaveName;

		// Token: 0x04005015 RID: 20501
		public readonly string DisplayName;

		// Token: 0x04005016 RID: 20502
		public readonly string WorldKey;

		// Token: 0x04005017 RID: 20503
		public readonly string SaveDirectory;

		// Token: 0x04005018 RID: 20504
		public readonly DateTime LastSaved;

		// Token: 0x04005019 RID: 20505
		public readonly VersionInformation Version;

		// Token: 0x0400501A RID: 20506
		public readonly VersionInformation.EVersionComparisonResult VersionComparison;

		// Token: 0x0400501B RID: 20507
		public readonly SaveInfoProvider.SaveEntryInfo SaveEntryInfo;

		// Token: 0x0400501C RID: 20508
		public readonly string MatchingColor;

		// Token: 0x0400501D RID: 20509
		public readonly string CompatibleColor;

		// Token: 0x0400501E RID: 20510
		public readonly string IncompatibleColor;
	}

	// Token: 0x02000D73 RID: 3443
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_DMSavegamesList.ListEntry>.XUiC_ListEntry
	{
		// Token: 0x0600687C RID: 26748 RVA: 0x00294017 File Offset: 0x00292217
		[XuiXmlBinding("savename")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingSaveName()
		{
			XUiC_DMSavegamesList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.DisplayName : null) ?? "";
		}

		// Token: 0x0600687D RID: 26749 RVA: 0x00294034 File Offset: 0x00292234
		[XuiXmlBinding("worldname")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingWorldName()
		{
			XUiC_DMSavegamesList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.WorldKey : null) ?? "";
		}

		// Token: 0x0600687E RID: 26750 RVA: 0x00294054 File Offset: 0x00292254
		[XuiXmlBinding("version")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingVersion()
		{
			XUiC_DMSavegamesList.ListEntry entryData = this.entryData;
			if (((entryData != null) ? entryData.Version : null) == null)
			{
				return "";
			}
			if (this.entryData.Version.Major < 0)
			{
				return Constants.cVersionInformation.LongStringNoBuild;
			}
			return this.entryData.Version.LongStringNoBuild;
		}

		// Token: 0x0600687F RID: 26751 RVA: 0x002940AC File Offset: 0x002922AC
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
			case VersionInformation.EVersionComparisonResult.SameBuild:
				text = "";
				break;
			case VersionInformation.EVersionComparisonResult.SameMinor:
				text = "";
				break;
			case VersionInformation.EVersionComparisonResult.NewerMinor:
				text = Localization.Get("xuiSavegameNewerMinor", false, null);
				break;
			case VersionInformation.EVersionComparisonResult.OlderMinor:
				text = Localization.Get("xuiSavegameOlderMinor", false, null);
				break;
			default:
				text = Localization.Get("xuiSavegameDifferentMajor", false, null);
				break;
			}
			return text;
		}

		// Token: 0x06006880 RID: 26752 RVA: 0x00294128 File Offset: 0x00292328
		[XuiXmlBinding("lastplayed")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingLastPlayed()
		{
			XUiC_DMSavegamesList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.LastSaved.ToString("yyyy-MM-dd HH:mm") : null) ?? "";
		}

		// Token: 0x06006881 RID: 26753 RVA: 0x00294160 File Offset: 0x00292360
		[XuiXmlBinding("lastplayedinfo")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingLastPlayedInfo()
		{
			if (this.entryData == null)
			{
				return "";
			}
			if (this.entryData.SaveEntryInfo.SizeInfo.IsArchived)
			{
				return "[fabc02ff]" + Localization.Get("xuiDmArchivedLabel", false, null) + "[-]";
			}
			int num = (int)(DateTime.Now - this.entryData.LastSaved).TotalDays;
			return string.Format("[ffffff88]{0} {1}[-]", num, Localization.Get("xuiDmDaysAgo", false, null));
		}

		// Token: 0x06006882 RID: 26754 RVA: 0x002941EC File Offset: 0x002923EC
		[XuiXmlBinding("savesize")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingSaveSize()
		{
			if (this.entryData == null)
			{
				return "";
			}
			string text = (this.entryData.SaveEntryInfo.SizeInfo.IsArchived ? "fabc02ff" : "ffffffbb");
			return string.Concat(new string[]
			{
				"[",
				text,
				"]",
				ValueDisplayFormatters.MemoryMiB(this.entryData.SaveEntryInfo.SizeInfo.ReportedSize),
				"[-]"
			});
		}

		// Token: 0x06006883 RID: 26755 RVA: 0x0029426F File Offset: 0x0029246F
		[XuiXmlBinding("nameColor")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingNameColor()
		{
			if (this.entryData != null)
			{
				return "255,255,255";
			}
			return "0,0,0,0";
		}

		// Token: 0x06006884 RID: 26756 RVA: 0x0029426F File Offset: 0x0029246F
		[XuiXmlBinding("saveSizeColor")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingSaveSizeColor()
		{
			if (this.entryData != null)
			{
				return "255,255,255";
			}
			return "0,0,0,0";
		}

		// Token: 0x06006885 RID: 26757 RVA: 0x00294284 File Offset: 0x00292484
		[XuiXmlBinding("versioncolor")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingVersionColor()
		{
			if (this.entryData == null)
			{
				return "0,0,0,0";
			}
			string text;
			switch (this.entryData.VersionComparison)
			{
			case VersionInformation.EVersionComparisonResult.SameBuild:
				text = this.entryData.MatchingColor;
				break;
			case VersionInformation.EVersionComparisonResult.SameMinor:
				text = this.entryData.MatchingColor;
				break;
			case VersionInformation.EVersionComparisonResult.NewerMinor:
				text = this.entryData.CompatibleColor;
				break;
			case VersionInformation.EVersionComparisonResult.OlderMinor:
				text = this.entryData.CompatibleColor;
				break;
			default:
				text = this.entryData.IncompatibleColor;
				break;
			}
			return text;
		}

		// Token: 0x06006886 RID: 26758 RVA: 0x00294308 File Offset: 0x00292508
		[XuiXmlBinding("entrycolor")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingEntryColor()
		{
			if (this.entryData == null)
			{
				return "0,0,0,0";
			}
			string text;
			switch (this.entryData.VersionComparison)
			{
			case VersionInformation.EVersionComparisonResult.SameBuild:
				text = this.entryData.MatchingColor;
				break;
			case VersionInformation.EVersionComparisonResult.SameMinor:
				text = this.entryData.MatchingColor;
				break;
			case VersionInformation.EVersionComparisonResult.NewerMinor:
				text = this.entryData.CompatibleColor;
				break;
			case VersionInformation.EVersionComparisonResult.OlderMinor:
				text = this.entryData.CompatibleColor;
				break;
			default:
				text = this.entryData.IncompatibleColor;
				break;
			}
			return text;
		}
	}
}

using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000D94 RID: 3476
[Preserve]
public class XUiC_DMSavegamesList : XUiC_DMBaseList<XUiC_DMSavegamesList.ListEntry>
{
	// Token: 0x17000B46 RID: 2886
	// (get) Token: 0x060068FF RID: 26879 RVA: 0x0029A277 File Offset: 0x00298477
	// (set) Token: 0x06006900 RID: 26880 RVA: 0x0029A27F File Offset: 0x0029847F
	[XuiXmlAttribute("matching_version_color", false)]
	public string MatchingVersionColor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000B47 RID: 2887
	// (get) Token: 0x06006901 RID: 26881 RVA: 0x0029A288 File Offset: 0x00298488
	// (set) Token: 0x06006902 RID: 26882 RVA: 0x0029A290 File Offset: 0x00298490
	[XuiXmlAttribute("compatible_version_color", false)]
	public string CompatibleVersionColor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000B48 RID: 2888
	// (get) Token: 0x06006903 RID: 26883 RVA: 0x0029A299 File Offset: 0x00298499
	// (set) Token: 0x06006904 RID: 26884 RVA: 0x0029A2A1 File Offset: 0x002984A1
	[XuiXmlAttribute("incompatible_version_color", false)]
	public string IncompatibleVersionColor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06006905 RID: 26885 RVA: 0x000027FC File Offset: 0x000009FC
	public override void RebuildList(bool _resetFilter = false)
	{
	}

	// Token: 0x06006906 RID: 26886 RVA: 0x0029A2AC File Offset: 0x002984AC
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

	// Token: 0x06006907 RID: 26887 RVA: 0x0029A330 File Offset: 0x00298530
	public void ClearList()
	{
		this.allEntries.Clear();
		this.RebuildList(true);
	}

	// Token: 0x06006908 RID: 26888 RVA: 0x0029A344 File Offset: 0x00298544
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

	// Token: 0x06006909 RID: 26889 RVA: 0x0029A393 File Offset: 0x00298593
	public void SetWorldFilter(string _worldKey)
	{
		this.worldFilter = _worldKey;
		this.filteredEntries.Clear();
		this.FilterResults(this.previousMatch);
		this.RefreshView(false, true);
	}

	// Token: 0x0600690A RID: 26890 RVA: 0x0029A3BC File Offset: 0x002985BC
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

	// Token: 0x0600690B RID: 26891 RVA: 0x0029A41D File Offset: 0x0029861D
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

	// Token: 0x040050EC RID: 20716
	[PublicizedFrom(EAccessModifier.Private)]
	public string worldFilter;

	// Token: 0x02000D95 RID: 3477
	[Preserve]
	public class ListEntry : XUiListEntry<XUiC_DMSavegamesList.ListEntry>
	{
		// Token: 0x0600690D RID: 26893 RVA: 0x0029A43C File Offset: 0x0029863C
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

		// Token: 0x0600690E RID: 26894 RVA: 0x0029A4D1 File Offset: 0x002986D1
		public override int CompareTo(XUiC_DMSavegamesList.ListEntry _otherEntry)
		{
			if (_otherEntry != null)
			{
				return this.SaveEntryInfo.CompareTo(_otherEntry.SaveEntryInfo);
			}
			return 1;
		}

		// Token: 0x0600690F RID: 26895 RVA: 0x0029A4E9 File Offset: 0x002986E9
		public override bool MatchesSearch(string _searchString)
		{
			return this.SaveName.ContainsCaseInsensitive(_searchString);
		}

		// Token: 0x040050ED RID: 20717
		public readonly string SaveName;

		// Token: 0x040050EE RID: 20718
		public readonly string DisplayName;

		// Token: 0x040050EF RID: 20719
		public readonly string WorldKey;

		// Token: 0x040050F0 RID: 20720
		public readonly string SaveDirectory;

		// Token: 0x040050F1 RID: 20721
		public readonly DateTime LastSaved;

		// Token: 0x040050F2 RID: 20722
		public readonly VersionInformation Version;

		// Token: 0x040050F3 RID: 20723
		public readonly VersionInformation.EVersionComparisonResult VersionComparison;

		// Token: 0x040050F4 RID: 20724
		public readonly SaveInfoProvider.SaveEntryInfo SaveEntryInfo;

		// Token: 0x040050F5 RID: 20725
		public readonly string MatchingColor;

		// Token: 0x040050F6 RID: 20726
		public readonly string CompatibleColor;

		// Token: 0x040050F7 RID: 20727
		public readonly string IncompatibleColor;
	}

	// Token: 0x02000D96 RID: 3478
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_DMSavegamesList.ListEntry>.XUiC_ListEntry
	{
		// Token: 0x06006910 RID: 26896 RVA: 0x0029A4F7 File Offset: 0x002986F7
		[XuiXmlBinding("savename")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingSaveName()
		{
			XUiC_DMSavegamesList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.DisplayName : null) ?? "";
		}

		// Token: 0x06006911 RID: 26897 RVA: 0x0029A514 File Offset: 0x00298714
		[XuiXmlBinding("worldname")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingWorldName()
		{
			XUiC_DMSavegamesList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.WorldKey : null) ?? "";
		}

		// Token: 0x06006912 RID: 26898 RVA: 0x0029A534 File Offset: 0x00298734
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

		// Token: 0x06006913 RID: 26899 RVA: 0x0029A58C File Offset: 0x0029878C
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

		// Token: 0x06006914 RID: 26900 RVA: 0x0029A608 File Offset: 0x00298808
		[XuiXmlBinding("lastplayed")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingLastPlayed()
		{
			XUiC_DMSavegamesList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.LastSaved.ToString("yyyy-MM-dd HH:mm") : null) ?? "";
		}

		// Token: 0x06006915 RID: 26901 RVA: 0x0029A640 File Offset: 0x00298840
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

		// Token: 0x06006916 RID: 26902 RVA: 0x0029A6CC File Offset: 0x002988CC
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

		// Token: 0x06006917 RID: 26903 RVA: 0x0029A74F File Offset: 0x0029894F
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

		// Token: 0x06006918 RID: 26904 RVA: 0x0029A74F File Offset: 0x0029894F
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

		// Token: 0x06006919 RID: 26905 RVA: 0x0029A764 File Offset: 0x00298964
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

		// Token: 0x0600691A RID: 26906 RVA: 0x0029A7E8 File Offset: 0x002989E8
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

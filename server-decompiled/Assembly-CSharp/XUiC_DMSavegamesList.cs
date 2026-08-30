using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000D85 RID: 3461
[Preserve]
public class XUiC_DMSavegamesList : XUiC_DMBaseList<XUiC_DMSavegamesList.ListEntry>
{
	// Token: 0x17000B2B RID: 2859
	// (get) Token: 0x06006907 RID: 26887 RVA: 0x00296EEB File Offset: 0x002950EB
	// (set) Token: 0x06006908 RID: 26888 RVA: 0x00296EF3 File Offset: 0x002950F3
	[XuiXmlAttribute("matching_version_color", false)]
	public string MatchingVersionColor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000B2C RID: 2860
	// (get) Token: 0x06006909 RID: 26889 RVA: 0x00296EFC File Offset: 0x002950FC
	// (set) Token: 0x0600690A RID: 26890 RVA: 0x00296F04 File Offset: 0x00295104
	[XuiXmlAttribute("compatible_version_color", false)]
	public string CompatibleVersionColor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000B2D RID: 2861
	// (get) Token: 0x0600690B RID: 26891 RVA: 0x00296F0D File Offset: 0x0029510D
	// (set) Token: 0x0600690C RID: 26892 RVA: 0x00296F15 File Offset: 0x00295115
	[XuiXmlAttribute("incompatible_version_color", false)]
	public string IncompatibleVersionColor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x0600690D RID: 26893 RVA: 0x000027FC File Offset: 0x000009FC
	public override void RebuildList(bool _resetFilter = false)
	{
	}

	// Token: 0x0600690E RID: 26894 RVA: 0x00296F20 File Offset: 0x00295120
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

	// Token: 0x0600690F RID: 26895 RVA: 0x00296FA4 File Offset: 0x002951A4
	public void ClearList()
	{
		this.allEntries.Clear();
		this.RebuildList(true);
	}

	// Token: 0x06006910 RID: 26896 RVA: 0x00296FB8 File Offset: 0x002951B8
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

	// Token: 0x06006911 RID: 26897 RVA: 0x00297007 File Offset: 0x00295207
	public void SetWorldFilter(string _worldKey)
	{
		this.worldFilter = _worldKey;
		this.filteredEntries.Clear();
		this.FilterResults(this.previousMatch);
		this.RefreshView(false, true);
	}

	// Token: 0x06006912 RID: 26898 RVA: 0x00297030 File Offset: 0x00295230
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

	// Token: 0x06006913 RID: 26899 RVA: 0x00297091 File Offset: 0x00295291
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

	// Token: 0x040050B5 RID: 20661
	[PublicizedFrom(EAccessModifier.Private)]
	public string worldFilter;

	// Token: 0x02000D86 RID: 3462
	[Preserve]
	public class ListEntry : XUiListEntry<XUiC_DMSavegamesList.ListEntry>
	{
		// Token: 0x06006915 RID: 26901 RVA: 0x002970B0 File Offset: 0x002952B0
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

		// Token: 0x06006916 RID: 26902 RVA: 0x00297145 File Offset: 0x00295345
		public override int CompareTo(XUiC_DMSavegamesList.ListEntry _otherEntry)
		{
			if (_otherEntry != null)
			{
				return this.SaveEntryInfo.CompareTo(_otherEntry.SaveEntryInfo);
			}
			return 1;
		}

		// Token: 0x06006917 RID: 26903 RVA: 0x0029715D File Offset: 0x0029535D
		public override bool MatchesSearch(string _searchString)
		{
			return this.SaveName.ContainsCaseInsensitive(_searchString);
		}

		// Token: 0x040050B6 RID: 20662
		public readonly string SaveName;

		// Token: 0x040050B7 RID: 20663
		public readonly string DisplayName;

		// Token: 0x040050B8 RID: 20664
		public readonly string WorldKey;

		// Token: 0x040050B9 RID: 20665
		public readonly string SaveDirectory;

		// Token: 0x040050BA RID: 20666
		public readonly DateTime LastSaved;

		// Token: 0x040050BB RID: 20667
		public readonly VersionInformation Version;

		// Token: 0x040050BC RID: 20668
		public readonly VersionInformation.EVersionComparisonResult VersionComparison;

		// Token: 0x040050BD RID: 20669
		public readonly SaveInfoProvider.SaveEntryInfo SaveEntryInfo;

		// Token: 0x040050BE RID: 20670
		public readonly string MatchingColor;

		// Token: 0x040050BF RID: 20671
		public readonly string CompatibleColor;

		// Token: 0x040050C0 RID: 20672
		public readonly string IncompatibleColor;
	}

	// Token: 0x02000D87 RID: 3463
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_DMSavegamesList.ListEntry>.XUiC_ListEntry
	{
		// Token: 0x06006918 RID: 26904 RVA: 0x0029716B File Offset: 0x0029536B
		[XuiXmlBinding("savename")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingSaveName()
		{
			XUiC_DMSavegamesList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.DisplayName : null) ?? "";
		}

		// Token: 0x06006919 RID: 26905 RVA: 0x00297188 File Offset: 0x00295388
		[XuiXmlBinding("worldname")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingWorldName()
		{
			XUiC_DMSavegamesList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.WorldKey : null) ?? "";
		}

		// Token: 0x0600691A RID: 26906 RVA: 0x002971A8 File Offset: 0x002953A8
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

		// Token: 0x0600691B RID: 26907 RVA: 0x00297200 File Offset: 0x00295400
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

		// Token: 0x0600691C RID: 26908 RVA: 0x0029727C File Offset: 0x0029547C
		[XuiXmlBinding("lastplayed")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingLastPlayed()
		{
			XUiC_DMSavegamesList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.LastSaved.ToString("yyyy-MM-dd HH:mm") : null) ?? "";
		}

		// Token: 0x0600691D RID: 26909 RVA: 0x002972B4 File Offset: 0x002954B4
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

		// Token: 0x0600691E RID: 26910 RVA: 0x00297340 File Offset: 0x00295540
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

		// Token: 0x0600691F RID: 26911 RVA: 0x002973C3 File Offset: 0x002955C3
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

		// Token: 0x06006920 RID: 26912 RVA: 0x002973C3 File Offset: 0x002955C3
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

		// Token: 0x06006921 RID: 26913 RVA: 0x002973D8 File Offset: 0x002955D8
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

		// Token: 0x06006922 RID: 26914 RVA: 0x0029745C File Offset: 0x0029565C
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

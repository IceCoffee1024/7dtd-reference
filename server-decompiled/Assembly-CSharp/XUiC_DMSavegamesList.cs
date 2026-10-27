using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000D9F RID: 3487
[Preserve]
public class XUiC_DMSavegamesList : XUiC_DMBaseList<XUiC_DMSavegamesList.ListEntry>
{
	// Token: 0x17000B4C RID: 2892
	// (get) Token: 0x06006924 RID: 26916 RVA: 0x0029B79F File Offset: 0x0029999F
	// (set) Token: 0x06006925 RID: 26917 RVA: 0x0029B7A7 File Offset: 0x002999A7
	[XuiXmlAttribute("matching_version_color", false)]
	public string MatchingVersionColor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000B4D RID: 2893
	// (get) Token: 0x06006926 RID: 26918 RVA: 0x0029B7B0 File Offset: 0x002999B0
	// (set) Token: 0x06006927 RID: 26919 RVA: 0x0029B7B8 File Offset: 0x002999B8
	[XuiXmlAttribute("compatible_version_color", false)]
	public string CompatibleVersionColor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000B4E RID: 2894
	// (get) Token: 0x06006928 RID: 26920 RVA: 0x0029B7C1 File Offset: 0x002999C1
	// (set) Token: 0x06006929 RID: 26921 RVA: 0x0029B7C9 File Offset: 0x002999C9
	[XuiXmlAttribute("incompatible_version_color", false)]
	public string IncompatibleVersionColor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x0600692A RID: 26922 RVA: 0x000027FC File Offset: 0x000009FC
	public override void RebuildList(bool _resetFilter = false)
	{
	}

	// Token: 0x0600692B RID: 26923 RVA: 0x0029B7D4 File Offset: 0x002999D4
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

	// Token: 0x0600692C RID: 26924 RVA: 0x0029B858 File Offset: 0x00299A58
	public void ClearList()
	{
		this.allEntries.Clear();
		this.RebuildList(true);
	}

	// Token: 0x0600692D RID: 26925 RVA: 0x0029B86C File Offset: 0x00299A6C
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

	// Token: 0x0600692E RID: 26926 RVA: 0x0029B8BB File Offset: 0x00299ABB
	public void SetWorldFilter(string _worldKey)
	{
		this.worldFilter = _worldKey;
		this.filteredEntries.Clear();
		this.FilterResults(this.previousMatch);
		this.RefreshView(false, true);
	}

	// Token: 0x0600692F RID: 26927 RVA: 0x0029B8E4 File Offset: 0x00299AE4
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

	// Token: 0x06006930 RID: 26928 RVA: 0x0029B945 File Offset: 0x00299B45
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

	// Token: 0x04005111 RID: 20753
	[PublicizedFrom(EAccessModifier.Private)]
	public string worldFilter;

	// Token: 0x02000DA0 RID: 3488
	[Preserve]
	public class ListEntry : XUiListEntry<XUiC_DMSavegamesList.ListEntry>
	{
		// Token: 0x06006932 RID: 26930 RVA: 0x0029B964 File Offset: 0x00299B64
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

		// Token: 0x06006933 RID: 26931 RVA: 0x0029B9F9 File Offset: 0x00299BF9
		public override int CompareTo(XUiC_DMSavegamesList.ListEntry _otherEntry)
		{
			if (_otherEntry != null)
			{
				return this.SaveEntryInfo.CompareTo(_otherEntry.SaveEntryInfo);
			}
			return 1;
		}

		// Token: 0x06006934 RID: 26932 RVA: 0x0029BA11 File Offset: 0x00299C11
		public override bool MatchesSearch(string _searchString)
		{
			return this.SaveName.ContainsCaseInsensitive(_searchString);
		}

		// Token: 0x04005112 RID: 20754
		public readonly string SaveName;

		// Token: 0x04005113 RID: 20755
		public readonly string DisplayName;

		// Token: 0x04005114 RID: 20756
		public readonly string WorldKey;

		// Token: 0x04005115 RID: 20757
		public readonly string SaveDirectory;

		// Token: 0x04005116 RID: 20758
		public readonly DateTime LastSaved;

		// Token: 0x04005117 RID: 20759
		public readonly VersionInformation Version;

		// Token: 0x04005118 RID: 20760
		public readonly VersionInformation.EVersionComparisonResult VersionComparison;

		// Token: 0x04005119 RID: 20761
		public readonly SaveInfoProvider.SaveEntryInfo SaveEntryInfo;

		// Token: 0x0400511A RID: 20762
		public readonly string MatchingColor;

		// Token: 0x0400511B RID: 20763
		public readonly string CompatibleColor;

		// Token: 0x0400511C RID: 20764
		public readonly string IncompatibleColor;
	}

	// Token: 0x02000DA1 RID: 3489
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_DMSavegamesList.ListEntry>.XUiC_ListEntry
	{
		// Token: 0x06006935 RID: 26933 RVA: 0x0029BA1F File Offset: 0x00299C1F
		[XuiXmlBinding("savename")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingSaveName()
		{
			XUiC_DMSavegamesList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.DisplayName : null) ?? "";
		}

		// Token: 0x06006936 RID: 26934 RVA: 0x0029BA3C File Offset: 0x00299C3C
		[XuiXmlBinding("worldname")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingWorldName()
		{
			XUiC_DMSavegamesList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.WorldKey : null) ?? "";
		}

		// Token: 0x06006937 RID: 26935 RVA: 0x0029BA5C File Offset: 0x00299C5C
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

		// Token: 0x06006938 RID: 26936 RVA: 0x0029BAB4 File Offset: 0x00299CB4
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

		// Token: 0x06006939 RID: 26937 RVA: 0x0029BB30 File Offset: 0x00299D30
		[XuiXmlBinding("lastplayed")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingLastPlayed()
		{
			XUiC_DMSavegamesList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.LastSaved.ToString("yyyy-MM-dd HH:mm") : null) ?? "";
		}

		// Token: 0x0600693A RID: 26938 RVA: 0x0029BB68 File Offset: 0x00299D68
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

		// Token: 0x0600693B RID: 26939 RVA: 0x0029BBF4 File Offset: 0x00299DF4
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

		// Token: 0x0600693C RID: 26940 RVA: 0x0029BC77 File Offset: 0x00299E77
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

		// Token: 0x0600693D RID: 26941 RVA: 0x0029BC77 File Offset: 0x00299E77
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

		// Token: 0x0600693E RID: 26942 RVA: 0x0029BC8C File Offset: 0x00299E8C
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

		// Token: 0x0600693F RID: 26943 RVA: 0x0029BD10 File Offset: 0x00299F10
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

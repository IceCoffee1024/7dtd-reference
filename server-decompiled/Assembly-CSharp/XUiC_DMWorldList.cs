using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000DA3 RID: 3491
[Preserve]
public class XUiC_DMWorldList : XUiC_DMBaseList<XUiC_DMWorldList.ListEntry>
{
	// Token: 0x17000B51 RID: 2897
	// (get) Token: 0x06006949 RID: 26953 RVA: 0x0029BECB File Offset: 0x0029A0CB
	// (set) Token: 0x0600694A RID: 26954 RVA: 0x0029BED3 File Offset: 0x0029A0D3
	[XuiXmlAttribute("matching_version_color", false)]
	public string MatchingVersionColor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000B52 RID: 2898
	// (get) Token: 0x0600694B RID: 26955 RVA: 0x0029BEDC File Offset: 0x0029A0DC
	// (set) Token: 0x0600694C RID: 26956 RVA: 0x0029BEE4 File Offset: 0x0029A0E4
	[XuiXmlAttribute("compatible_version_color", false)]
	public string CompatibleVersionColor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000B53 RID: 2899
	// (get) Token: 0x0600694D RID: 26957 RVA: 0x0029BEED File Offset: 0x0029A0ED
	// (set) Token: 0x0600694E RID: 26958 RVA: 0x0029BEF5 File Offset: 0x0029A0F5
	[XuiXmlAttribute("incompatible_version_color", false)]
	public string IncompatibleVersionColor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x0600694F RID: 26959 RVA: 0x000027FC File Offset: 0x000009FC
	public override void RebuildList(bool _resetFilter = false)
	{
	}

	// Token: 0x06006950 RID: 26960 RVA: 0x0029BF00 File Offset: 0x0029A100
	public void RebuildList(IReadOnlyCollection<SaveInfoProvider.WorldEntryInfo> _worldEntryInfos, bool _resetFilter = false)
	{
		this.allEntries.Clear();
		foreach (SaveInfoProvider.WorldEntryInfo worldEntryInfo in _worldEntryInfos)
		{
			this.allEntries.Add(new XUiC_DMWorldList.ListEntry(worldEntryInfo, this.MatchingVersionColor, this.CompatibleVersionColor, this.IncompatibleVersionColor));
		}
		this.allEntries.Sort();
		base.RebuildList(_resetFilter);
	}

	// Token: 0x06006951 RID: 26961 RVA: 0x0029BF84 File Offset: 0x0029A184
	public void ClearList()
	{
		this.allEntries.Clear();
		this.RebuildList(true);
	}

	// Token: 0x06006952 RID: 26962 RVA: 0x0029BF98 File Offset: 0x0029A198
	public bool SelectByKey(string _key)
	{
		if (string.IsNullOrEmpty(_key))
		{
			return false;
		}
		for (int i = 0; i < this.filteredEntries.Count; i++)
		{
			if (this.filteredEntries[i].Key.Equals(_key, StringComparison.OrdinalIgnoreCase))
			{
				base.SelectedEntryIndex = i;
				return true;
			}
		}
		return false;
	}

	// Token: 0x06006953 RID: 26963 RVA: 0x0029BFE9 File Offset: 0x0029A1E9
	public void UpdateHiddenEntryVisibility()
	{
		this.filteredEntries.Clear();
		this.FilterResults(this.previousMatch);
	}

	// Token: 0x06006954 RID: 26964 RVA: 0x0029C004 File Offset: 0x0029A204
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void FilterResults(string _textMatch)
	{
		base.FilterResults(_textMatch);
		for (int i = 0; i < this.filteredEntries.Count; i++)
		{
			XUiC_DMWorldList.ListEntry listEntry = this.filteredEntries[i];
			if (listEntry.HideIfEmpty && listEntry.SaveDataSizeTotal == 0L)
			{
				this.filteredEntries.RemoveAt(i);
				i--;
			}
		}
	}

	// Token: 0x02000DA4 RID: 3492
	[Preserve]
	public class ListEntry : XUiListEntry<XUiC_DMWorldList.ListEntry>
	{
		// Token: 0x06006956 RID: 26966 RVA: 0x0029C064 File Offset: 0x0029A264
		public ListEntry(SaveInfoProvider.WorldEntryInfo _worldEntryInfo, string _matchingColor, string _compatibleColor, string _incompatibleColor)
		{
			this.WorldEntryInfo = _worldEntryInfo;
			this.Key = _worldEntryInfo.WorldKey;
			this.Name = _worldEntryInfo.Name;
			this.DisplayName = _worldEntryInfo.DisplayName;
			this.Type = _worldEntryInfo.Type;
			this.Location = _worldEntryInfo.Location;
			this.Deletable = _worldEntryInfo.Deletable;
			this.Moveable = _worldEntryInfo.Moveable;
			this.WorldDataSize = _worldEntryInfo.WorldDataSize;
			this.Version = _worldEntryInfo.Version;
			VersionInformation version = this.Version;
			this.VersionComparison = ((version != null) ? version.CompareToRunningBuild() : VersionInformation.EVersionComparisonResult.SameBuild);
			this.SaveDataSizeTotal = _worldEntryInfo.SaveDataSizeTotal;
			this.SaveDataSizeForLimit = _worldEntryInfo.SaveDataSizeForLimit;
			this.SaveDataCount = _worldEntryInfo.SaveDataCount;
			this.UsesDataLimit = _worldEntryInfo.UsesDataLimit;
			this.MatchingColor = _matchingColor;
			this.CompatibleColor = _compatibleColor;
			this.IncompatibleColor = _incompatibleColor;
			this.HideIfEmpty = _worldEntryInfo.HideIfEmpty;
		}

		// Token: 0x06006957 RID: 26967 RVA: 0x0029C154 File Offset: 0x0029A354
		public override int CompareTo(XUiC_DMWorldList.ListEntry _otherEntry)
		{
			if (_otherEntry != null)
			{
				return this.WorldEntryInfo.CompareTo(_otherEntry.WorldEntryInfo);
			}
			return 1;
		}

		// Token: 0x06006958 RID: 26968 RVA: 0x0029C16C File Offset: 0x0029A36C
		public override bool MatchesSearch(string _searchString)
		{
			return this.Name.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x04005127 RID: 20775
		public readonly string Key;

		// Token: 0x04005128 RID: 20776
		public readonly string Name;

		// Token: 0x04005129 RID: 20777
		public readonly string DisplayName;

		// Token: 0x0400512A RID: 20778
		public readonly string Type;

		// Token: 0x0400512B RID: 20779
		public readonly PathAbstractions.AbstractedLocation Location;

		// Token: 0x0400512C RID: 20780
		public readonly bool Deletable;

		// Token: 0x0400512D RID: 20781
		public readonly bool Moveable;

		// Token: 0x0400512E RID: 20782
		public readonly long WorldDataSize;

		// Token: 0x0400512F RID: 20783
		public readonly VersionInformation Version;

		// Token: 0x04005130 RID: 20784
		public readonly VersionInformation.EVersionComparisonResult VersionComparison;

		// Token: 0x04005131 RID: 20785
		public readonly long SaveDataSizeTotal;

		// Token: 0x04005132 RID: 20786
		public readonly long SaveDataSizeForLimit;

		// Token: 0x04005133 RID: 20787
		public readonly int SaveDataCount;

		// Token: 0x04005134 RID: 20788
		public readonly bool HideIfEmpty;

		// Token: 0x04005135 RID: 20789
		public readonly SaveInfoProvider.WorldEntryInfo WorldEntryInfo;

		// Token: 0x04005136 RID: 20790
		public readonly bool UsesDataLimit;

		// Token: 0x04005137 RID: 20791
		public readonly string MatchingColor;

		// Token: 0x04005138 RID: 20792
		public readonly string CompatibleColor;

		// Token: 0x04005139 RID: 20793
		public readonly string IncompatibleColor;
	}

	// Token: 0x02000DA5 RID: 3493
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_DMWorldList.ListEntry>.XUiC_ListEntry
	{
		// Token: 0x06006959 RID: 26969 RVA: 0x0029C181 File Offset: 0x0029A381
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_DMWorldList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.DisplayName : null) ?? "";
		}

		// Token: 0x0600695A RID: 26970 RVA: 0x0029C19E File Offset: 0x0029A39E
		[XuiXmlBinding("type")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingType()
		{
			XUiC_DMWorldList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.Type : null) ?? "";
		}

		// Token: 0x0600695B RID: 26971 RVA: 0x0029C1BB File Offset: 0x0029A3BB
		[XuiXmlBinding("worldDataSize")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingWorldDataSize()
		{
			if (this.entryData == null)
			{
				return "";
			}
			if (!this.entryData.Deletable)
			{
				return "-";
			}
			return ValueDisplayFormatters.MemoryMiB(this.entryData.WorldDataSize);
		}

		// Token: 0x0600695C RID: 26972 RVA: 0x0029C1EE File Offset: 0x0029A3EE
		[XuiXmlBinding("saveDataSize")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingSaveDataSize()
		{
			if (this.entryData != null)
			{
				return ValueDisplayFormatters.MemoryMiB(this.entryData.SaveDataSizeTotal);
			}
			return "";
		}

		// Token: 0x0600695D RID: 26973 RVA: 0x0029C20E File Offset: 0x0029A40E
		[XuiXmlBinding("saveDataCount")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingSaveDataCount()
		{
			if (this.entryData != null)
			{
				return string.Format("{0} {1}", this.entryData.SaveDataCount, Localization.Get("xuiDmSaves", false, null));
			}
			return "";
		}

		// Token: 0x0600695E RID: 26974 RVA: 0x0029A7DA File Offset: 0x002989DA
		[XuiXmlBinding("isRoamingOptional")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingIsRoamingOptional()
		{
			return PlatformManager.MultiPlatform.UserDataRoaming.IsRoamingOptional;
		}

		// Token: 0x0600695F RID: 26975 RVA: 0x0029C244 File Offset: 0x0029A444
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

		// Token: 0x06006960 RID: 26976 RVA: 0x0029C25C File Offset: 0x0029A45C
		[XuiXmlBinding("totalDataSizeForLimit")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingtTotalDataSizeForLimit()
		{
			if (this.entryData != null)
			{
				return ValueDisplayFormatters.MemoryMiB(this.entryData.UsesDataLimit ? (this.entryData.WorldDataSize + this.entryData.SaveDataSizeForLimit) : this.entryData.SaveDataSizeForLimit);
			}
			return "";
		}

		// Token: 0x06006961 RID: 26977 RVA: 0x0029C2B0 File Offset: 0x0029A4B0
		[XuiXmlBinding("totalDataSizeNotLimited")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingTotalDataSizeNotLimited()
		{
			if (this.entryData == null)
			{
				return "";
			}
			long num = this.entryData.SaveDataSizeTotal - this.entryData.SaveDataSizeForLimit;
			return ValueDisplayFormatters.MemoryMiB((!this.entryData.UsesDataLimit) ? (this.entryData.WorldDataSize + num) : num);
		}

		// Token: 0x06006962 RID: 26978 RVA: 0x0029C308 File Offset: 0x0029A508
		[XuiXmlBinding("totalDataSize")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingTotalDataSize()
		{
			if (this.entryData != null)
			{
				return ValueDisplayFormatters.MemoryMiB(this.entryData.UsesDataLimit ? (this.entryData.WorldDataSize + this.entryData.SaveDataSizeTotal) : this.entryData.SaveDataSizeTotal);
			}
			return "";
		}

		// Token: 0x06006963 RID: 26979 RVA: 0x0029C35C File Offset: 0x0029A55C
		[XuiXmlBinding("versioncolor")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingVersioncolor()
		{
			if (this.entryData == null)
			{
				return "0,0,0,0";
			}
			if (this.entryData.Version == null)
			{
				return this.entryData.IncompatibleColor;
			}
			if (this.entryData.Version.Major < 0)
			{
				return this.entryData.MatchingColor;
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

		// Token: 0x06006964 RID: 26980 RVA: 0x0029C418 File Offset: 0x0029A618
		[XuiXmlBinding("version")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingVersion()
		{
			XUiC_DMWorldList.ListEntry entryData = this.entryData;
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

		// Token: 0x06006965 RID: 26981 RVA: 0x0029C470 File Offset: 0x0029A670
		[XuiXmlBinding("versiontooltip")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingVersionTooltip()
		{
			if (this.entryData == null)
			{
				return "";
			}
			if (this.entryData.Version == null || this.entryData.Version.Major < 0)
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
	}
}

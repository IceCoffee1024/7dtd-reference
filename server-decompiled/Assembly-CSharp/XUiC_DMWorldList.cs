using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000D75 RID: 3445
[Preserve]
public class XUiC_DMWorldList : XUiC_DMBaseList<XUiC_DMWorldList.ListEntry>
{
	// Token: 0x17000B18 RID: 2840
	// (get) Token: 0x06006890 RID: 26768 RVA: 0x002944C3 File Offset: 0x002926C3
	// (set) Token: 0x06006891 RID: 26769 RVA: 0x002944CB File Offset: 0x002926CB
	[XuiXmlAttribute("matching_version_color", false)]
	public string MatchingVersionColor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000B19 RID: 2841
	// (get) Token: 0x06006892 RID: 26770 RVA: 0x002944D4 File Offset: 0x002926D4
	// (set) Token: 0x06006893 RID: 26771 RVA: 0x002944DC File Offset: 0x002926DC
	[XuiXmlAttribute("compatible_version_color", false)]
	public string CompatibleVersionColor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000B1A RID: 2842
	// (get) Token: 0x06006894 RID: 26772 RVA: 0x002944E5 File Offset: 0x002926E5
	// (set) Token: 0x06006895 RID: 26773 RVA: 0x002944ED File Offset: 0x002926ED
	[XuiXmlAttribute("incompatible_version_color", false)]
	public string IncompatibleVersionColor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06006896 RID: 26774 RVA: 0x000027FC File Offset: 0x000009FC
	public override void RebuildList(bool _resetFilter = false)
	{
	}

	// Token: 0x06006897 RID: 26775 RVA: 0x002944F8 File Offset: 0x002926F8
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

	// Token: 0x06006898 RID: 26776 RVA: 0x0029457C File Offset: 0x0029277C
	public void ClearList()
	{
		this.allEntries.Clear();
		this.RebuildList(true);
	}

	// Token: 0x06006899 RID: 26777 RVA: 0x00294590 File Offset: 0x00292790
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

	// Token: 0x0600689A RID: 26778 RVA: 0x002945E1 File Offset: 0x002927E1
	public void UpdateHiddenEntryVisibility()
	{
		this.filteredEntries.Clear();
		this.FilterResults(this.previousMatch);
	}

	// Token: 0x0600689B RID: 26779 RVA: 0x002945FC File Offset: 0x002927FC
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

	// Token: 0x02000D76 RID: 3446
	[Preserve]
	public class ListEntry : XUiListEntry<XUiC_DMWorldList.ListEntry>
	{
		// Token: 0x0600689D RID: 26781 RVA: 0x0029465C File Offset: 0x0029285C
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

		// Token: 0x0600689E RID: 26782 RVA: 0x0029474C File Offset: 0x0029294C
		public override int CompareTo(XUiC_DMWorldList.ListEntry _otherEntry)
		{
			if (_otherEntry != null)
			{
				return this.WorldEntryInfo.CompareTo(_otherEntry.WorldEntryInfo);
			}
			return 1;
		}

		// Token: 0x0600689F RID: 26783 RVA: 0x00294764 File Offset: 0x00292964
		public override bool MatchesSearch(string _searchString)
		{
			return this.Name.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x04005029 RID: 20521
		public readonly string Key;

		// Token: 0x0400502A RID: 20522
		public readonly string Name;

		// Token: 0x0400502B RID: 20523
		public readonly string DisplayName;

		// Token: 0x0400502C RID: 20524
		public readonly string Type;

		// Token: 0x0400502D RID: 20525
		public readonly PathAbstractions.AbstractedLocation Location;

		// Token: 0x0400502E RID: 20526
		public readonly bool Deletable;

		// Token: 0x0400502F RID: 20527
		public readonly bool Moveable;

		// Token: 0x04005030 RID: 20528
		public readonly long WorldDataSize;

		// Token: 0x04005031 RID: 20529
		public readonly VersionInformation Version;

		// Token: 0x04005032 RID: 20530
		public readonly VersionInformation.EVersionComparisonResult VersionComparison;

		// Token: 0x04005033 RID: 20531
		public readonly long SaveDataSizeTotal;

		// Token: 0x04005034 RID: 20532
		public readonly long SaveDataSizeForLimit;

		// Token: 0x04005035 RID: 20533
		public readonly int SaveDataCount;

		// Token: 0x04005036 RID: 20534
		public readonly bool HideIfEmpty;

		// Token: 0x04005037 RID: 20535
		public readonly SaveInfoProvider.WorldEntryInfo WorldEntryInfo;

		// Token: 0x04005038 RID: 20536
		public readonly bool UsesDataLimit;

		// Token: 0x04005039 RID: 20537
		public readonly string MatchingColor;

		// Token: 0x0400503A RID: 20538
		public readonly string CompatibleColor;

		// Token: 0x0400503B RID: 20539
		public readonly string IncompatibleColor;
	}

	// Token: 0x02000D77 RID: 3447
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_DMWorldList.ListEntry>.XUiC_ListEntry
	{
		// Token: 0x060068A0 RID: 26784 RVA: 0x00294779 File Offset: 0x00292979
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_DMWorldList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.DisplayName : null) ?? "";
		}

		// Token: 0x060068A1 RID: 26785 RVA: 0x00294796 File Offset: 0x00292996
		[XuiXmlBinding("type")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingType()
		{
			XUiC_DMWorldList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.Type : null) ?? "";
		}

		// Token: 0x060068A2 RID: 26786 RVA: 0x002947B3 File Offset: 0x002929B3
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

		// Token: 0x060068A3 RID: 26787 RVA: 0x002947E6 File Offset: 0x002929E6
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

		// Token: 0x060068A4 RID: 26788 RVA: 0x00294806 File Offset: 0x00292A06
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

		// Token: 0x060068A5 RID: 26789 RVA: 0x00292DD2 File Offset: 0x00290FD2
		[XuiXmlBinding("isRoamingOptional")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingIsRoamingOptional()
		{
			return PlatformManager.MultiPlatform.UserDataRoaming.IsRoamingOptional;
		}

		// Token: 0x060068A6 RID: 26790 RVA: 0x0029483C File Offset: 0x00292A3C
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

		// Token: 0x060068A7 RID: 26791 RVA: 0x00294854 File Offset: 0x00292A54
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

		// Token: 0x060068A8 RID: 26792 RVA: 0x002948A8 File Offset: 0x00292AA8
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

		// Token: 0x060068A9 RID: 26793 RVA: 0x00294900 File Offset: 0x00292B00
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

		// Token: 0x060068AA RID: 26794 RVA: 0x00294954 File Offset: 0x00292B54
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

		// Token: 0x060068AB RID: 26795 RVA: 0x00294A10 File Offset: 0x00292C10
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

		// Token: 0x060068AC RID: 26796 RVA: 0x00294A68 File Offset: 0x00292C68
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

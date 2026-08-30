using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000D89 RID: 3465
[Preserve]
public class XUiC_DMWorldList : XUiC_DMBaseList<XUiC_DMWorldList.ListEntry>
{
	// Token: 0x17000B30 RID: 2864
	// (get) Token: 0x0600692C RID: 26924 RVA: 0x00297617 File Offset: 0x00295817
	// (set) Token: 0x0600692D RID: 26925 RVA: 0x0029761F File Offset: 0x0029581F
	[XuiXmlAttribute("matching_version_color", false)]
	public string MatchingVersionColor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000B31 RID: 2865
	// (get) Token: 0x0600692E RID: 26926 RVA: 0x00297628 File Offset: 0x00295828
	// (set) Token: 0x0600692F RID: 26927 RVA: 0x00297630 File Offset: 0x00295830
	[XuiXmlAttribute("compatible_version_color", false)]
	public string CompatibleVersionColor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000B32 RID: 2866
	// (get) Token: 0x06006930 RID: 26928 RVA: 0x00297639 File Offset: 0x00295839
	// (set) Token: 0x06006931 RID: 26929 RVA: 0x00297641 File Offset: 0x00295841
	[XuiXmlAttribute("incompatible_version_color", false)]
	public string IncompatibleVersionColor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06006932 RID: 26930 RVA: 0x000027FC File Offset: 0x000009FC
	public override void RebuildList(bool _resetFilter = false)
	{
	}

	// Token: 0x06006933 RID: 26931 RVA: 0x0029764C File Offset: 0x0029584C
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

	// Token: 0x06006934 RID: 26932 RVA: 0x002976D0 File Offset: 0x002958D0
	public void ClearList()
	{
		this.allEntries.Clear();
		this.RebuildList(true);
	}

	// Token: 0x06006935 RID: 26933 RVA: 0x002976E4 File Offset: 0x002958E4
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

	// Token: 0x06006936 RID: 26934 RVA: 0x00297735 File Offset: 0x00295935
	public void UpdateHiddenEntryVisibility()
	{
		this.filteredEntries.Clear();
		this.FilterResults(this.previousMatch);
	}

	// Token: 0x06006937 RID: 26935 RVA: 0x00297750 File Offset: 0x00295950
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

	// Token: 0x02000D8A RID: 3466
	[Preserve]
	public class ListEntry : XUiListEntry<XUiC_DMWorldList.ListEntry>
	{
		// Token: 0x06006939 RID: 26937 RVA: 0x002977B0 File Offset: 0x002959B0
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

		// Token: 0x0600693A RID: 26938 RVA: 0x002978A0 File Offset: 0x00295AA0
		public override int CompareTo(XUiC_DMWorldList.ListEntry _otherEntry)
		{
			if (_otherEntry != null)
			{
				return this.WorldEntryInfo.CompareTo(_otherEntry.WorldEntryInfo);
			}
			return 1;
		}

		// Token: 0x0600693B RID: 26939 RVA: 0x002978B8 File Offset: 0x00295AB8
		public override bool MatchesSearch(string _searchString)
		{
			return this.Name.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x040050CB RID: 20683
		public readonly string Key;

		// Token: 0x040050CC RID: 20684
		public readonly string Name;

		// Token: 0x040050CD RID: 20685
		public readonly string DisplayName;

		// Token: 0x040050CE RID: 20686
		public readonly string Type;

		// Token: 0x040050CF RID: 20687
		public readonly PathAbstractions.AbstractedLocation Location;

		// Token: 0x040050D0 RID: 20688
		public readonly bool Deletable;

		// Token: 0x040050D1 RID: 20689
		public readonly bool Moveable;

		// Token: 0x040050D2 RID: 20690
		public readonly long WorldDataSize;

		// Token: 0x040050D3 RID: 20691
		public readonly VersionInformation Version;

		// Token: 0x040050D4 RID: 20692
		public readonly VersionInformation.EVersionComparisonResult VersionComparison;

		// Token: 0x040050D5 RID: 20693
		public readonly long SaveDataSizeTotal;

		// Token: 0x040050D6 RID: 20694
		public readonly long SaveDataSizeForLimit;

		// Token: 0x040050D7 RID: 20695
		public readonly int SaveDataCount;

		// Token: 0x040050D8 RID: 20696
		public readonly bool HideIfEmpty;

		// Token: 0x040050D9 RID: 20697
		public readonly SaveInfoProvider.WorldEntryInfo WorldEntryInfo;

		// Token: 0x040050DA RID: 20698
		public readonly bool UsesDataLimit;

		// Token: 0x040050DB RID: 20699
		public readonly string MatchingColor;

		// Token: 0x040050DC RID: 20700
		public readonly string CompatibleColor;

		// Token: 0x040050DD RID: 20701
		public readonly string IncompatibleColor;
	}

	// Token: 0x02000D8B RID: 3467
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_DMWorldList.ListEntry>.XUiC_ListEntry
	{
		// Token: 0x0600693C RID: 26940 RVA: 0x002978CD File Offset: 0x00295ACD
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_DMWorldList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.DisplayName : null) ?? "";
		}

		// Token: 0x0600693D RID: 26941 RVA: 0x002978EA File Offset: 0x00295AEA
		[XuiXmlBinding("type")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingType()
		{
			XUiC_DMWorldList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.Type : null) ?? "";
		}

		// Token: 0x0600693E RID: 26942 RVA: 0x00297907 File Offset: 0x00295B07
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

		// Token: 0x0600693F RID: 26943 RVA: 0x0029793A File Offset: 0x00295B3A
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

		// Token: 0x06006940 RID: 26944 RVA: 0x0029795A File Offset: 0x00295B5A
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

		// Token: 0x06006941 RID: 26945 RVA: 0x00295F26 File Offset: 0x00294126
		[XuiXmlBinding("isRoamingOptional")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingIsRoamingOptional()
		{
			return PlatformManager.MultiPlatform.UserDataRoaming.IsRoamingOptional;
		}

		// Token: 0x06006942 RID: 26946 RVA: 0x00297990 File Offset: 0x00295B90
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

		// Token: 0x06006943 RID: 26947 RVA: 0x002979A8 File Offset: 0x00295BA8
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

		// Token: 0x06006944 RID: 26948 RVA: 0x002979FC File Offset: 0x00295BFC
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

		// Token: 0x06006945 RID: 26949 RVA: 0x00297A54 File Offset: 0x00295C54
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

		// Token: 0x06006946 RID: 26950 RVA: 0x00297AA8 File Offset: 0x00295CA8
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

		// Token: 0x06006947 RID: 26951 RVA: 0x00297B64 File Offset: 0x00295D64
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

		// Token: 0x06006948 RID: 26952 RVA: 0x00297BBC File Offset: 0x00295DBC
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

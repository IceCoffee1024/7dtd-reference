using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000D98 RID: 3480
[Preserve]
public class XUiC_DMWorldList : XUiC_DMBaseList<XUiC_DMWorldList.ListEntry>
{
	// Token: 0x17000B4B RID: 2891
	// (get) Token: 0x06006924 RID: 26916 RVA: 0x0029A9A3 File Offset: 0x00298BA3
	// (set) Token: 0x06006925 RID: 26917 RVA: 0x0029A9AB File Offset: 0x00298BAB
	[XuiXmlAttribute("matching_version_color", false)]
	public string MatchingVersionColor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000B4C RID: 2892
	// (get) Token: 0x06006926 RID: 26918 RVA: 0x0029A9B4 File Offset: 0x00298BB4
	// (set) Token: 0x06006927 RID: 26919 RVA: 0x0029A9BC File Offset: 0x00298BBC
	[XuiXmlAttribute("compatible_version_color", false)]
	public string CompatibleVersionColor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000B4D RID: 2893
	// (get) Token: 0x06006928 RID: 26920 RVA: 0x0029A9C5 File Offset: 0x00298BC5
	// (set) Token: 0x06006929 RID: 26921 RVA: 0x0029A9CD File Offset: 0x00298BCD
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

	// Token: 0x0600692B RID: 26923 RVA: 0x0029A9D8 File Offset: 0x00298BD8
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

	// Token: 0x0600692C RID: 26924 RVA: 0x0029AA5C File Offset: 0x00298C5C
	public void ClearList()
	{
		this.allEntries.Clear();
		this.RebuildList(true);
	}

	// Token: 0x0600692D RID: 26925 RVA: 0x0029AA70 File Offset: 0x00298C70
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

	// Token: 0x0600692E RID: 26926 RVA: 0x0029AAC1 File Offset: 0x00298CC1
	public void UpdateHiddenEntryVisibility()
	{
		this.filteredEntries.Clear();
		this.FilterResults(this.previousMatch);
	}

	// Token: 0x0600692F RID: 26927 RVA: 0x0029AADC File Offset: 0x00298CDC
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

	// Token: 0x02000D99 RID: 3481
	[Preserve]
	public class ListEntry : XUiListEntry<XUiC_DMWorldList.ListEntry>
	{
		// Token: 0x06006931 RID: 26929 RVA: 0x0029AB3C File Offset: 0x00298D3C
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

		// Token: 0x06006932 RID: 26930 RVA: 0x0029AC2C File Offset: 0x00298E2C
		public override int CompareTo(XUiC_DMWorldList.ListEntry _otherEntry)
		{
			if (_otherEntry != null)
			{
				return this.WorldEntryInfo.CompareTo(_otherEntry.WorldEntryInfo);
			}
			return 1;
		}

		// Token: 0x06006933 RID: 26931 RVA: 0x0029AC44 File Offset: 0x00298E44
		public override bool MatchesSearch(string _searchString)
		{
			return this.Name.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x04005102 RID: 20738
		public readonly string Key;

		// Token: 0x04005103 RID: 20739
		public readonly string Name;

		// Token: 0x04005104 RID: 20740
		public readonly string DisplayName;

		// Token: 0x04005105 RID: 20741
		public readonly string Type;

		// Token: 0x04005106 RID: 20742
		public readonly PathAbstractions.AbstractedLocation Location;

		// Token: 0x04005107 RID: 20743
		public readonly bool Deletable;

		// Token: 0x04005108 RID: 20744
		public readonly bool Moveable;

		// Token: 0x04005109 RID: 20745
		public readonly long WorldDataSize;

		// Token: 0x0400510A RID: 20746
		public readonly VersionInformation Version;

		// Token: 0x0400510B RID: 20747
		public readonly VersionInformation.EVersionComparisonResult VersionComparison;

		// Token: 0x0400510C RID: 20748
		public readonly long SaveDataSizeTotal;

		// Token: 0x0400510D RID: 20749
		public readonly long SaveDataSizeForLimit;

		// Token: 0x0400510E RID: 20750
		public readonly int SaveDataCount;

		// Token: 0x0400510F RID: 20751
		public readonly bool HideIfEmpty;

		// Token: 0x04005110 RID: 20752
		public readonly SaveInfoProvider.WorldEntryInfo WorldEntryInfo;

		// Token: 0x04005111 RID: 20753
		public readonly bool UsesDataLimit;

		// Token: 0x04005112 RID: 20754
		public readonly string MatchingColor;

		// Token: 0x04005113 RID: 20755
		public readonly string CompatibleColor;

		// Token: 0x04005114 RID: 20756
		public readonly string IncompatibleColor;
	}

	// Token: 0x02000D9A RID: 3482
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_DMWorldList.ListEntry>.XUiC_ListEntry
	{
		// Token: 0x06006934 RID: 26932 RVA: 0x0029AC59 File Offset: 0x00298E59
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_DMWorldList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.DisplayName : null) ?? "";
		}

		// Token: 0x06006935 RID: 26933 RVA: 0x0029AC76 File Offset: 0x00298E76
		[XuiXmlBinding("type")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingType()
		{
			XUiC_DMWorldList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.Type : null) ?? "";
		}

		// Token: 0x06006936 RID: 26934 RVA: 0x0029AC93 File Offset: 0x00298E93
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

		// Token: 0x06006937 RID: 26935 RVA: 0x0029ACC6 File Offset: 0x00298EC6
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

		// Token: 0x06006938 RID: 26936 RVA: 0x0029ACE6 File Offset: 0x00298EE6
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

		// Token: 0x06006939 RID: 26937 RVA: 0x002992B2 File Offset: 0x002974B2
		[XuiXmlBinding("isRoamingOptional")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingIsRoamingOptional()
		{
			return PlatformManager.MultiPlatform.UserDataRoaming.IsRoamingOptional;
		}

		// Token: 0x0600693A RID: 26938 RVA: 0x0029AD1C File Offset: 0x00298F1C
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

		// Token: 0x0600693B RID: 26939 RVA: 0x0029AD34 File Offset: 0x00298F34
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

		// Token: 0x0600693C RID: 26940 RVA: 0x0029AD88 File Offset: 0x00298F88
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

		// Token: 0x0600693D RID: 26941 RVA: 0x0029ADE0 File Offset: 0x00298FE0
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

		// Token: 0x0600693E RID: 26942 RVA: 0x0029AE34 File Offset: 0x00299034
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

		// Token: 0x0600693F RID: 26943 RVA: 0x0029AEF0 File Offset: 0x002990F0
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

		// Token: 0x06006940 RID: 26944 RVA: 0x0029AF48 File Offset: 0x00299148
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

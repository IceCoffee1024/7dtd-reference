using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000DF1 RID: 3569
[Preserve]
public class XUiC_BugReportSavesList : XUiC_List<XUiC_BugReportSavesList.ListEntry>
{
	// Token: 0x06006D3E RID: 27966 RVA: 0x000027FC File Offset: 0x000009FC
	public override void RebuildList(bool _resetFilter = false)
	{
	}

	// Token: 0x06006D3F RID: 27967 RVA: 0x002A755C File Offset: 0x002A575C
	public void RebuildList(IReadOnlyCollection<SaveInfoProvider.SaveEntryInfo> _saveEntryInfos, bool _resetFilter = false)
	{
		this.allEntries.Clear();
		foreach (SaveInfoProvider.SaveEntryInfo saveEntryInfo in _saveEntryInfos)
		{
			this.allEntries.Add(new XUiC_BugReportSavesList.ListEntry(saveEntryInfo));
		}
		this.allEntries.Sort();
		base.RebuildList(_resetFilter);
	}

	// Token: 0x02000DF2 RID: 3570
	[Preserve]
	public class ListEntry : XUiListEntry<XUiC_BugReportSavesList.ListEntry>
	{
		// Token: 0x06006D41 RID: 27969 RVA: 0x002A75D4 File Offset: 0x002A57D4
		public ListEntry(SaveInfoProvider.SaveEntryInfo _saveEntryInfo)
		{
			this.SaveEntryInfo = _saveEntryInfo;
			this.SaveName = _saveEntryInfo.Name;
			this.SaveDisplayName = _saveEntryInfo.DisplayName;
			this.WorldKey = _saveEntryInfo.WorldEntry.WorldKey;
			this.SaveDirectory = _saveEntryInfo.SaveDir;
			this.LastSaved = _saveEntryInfo.LastSaved;
		}

		// Token: 0x06006D42 RID: 27970 RVA: 0x002A762F File Offset: 0x002A582F
		public override int CompareTo(XUiC_BugReportSavesList.ListEntry _otherEntry)
		{
			if (_otherEntry != null)
			{
				return this.SaveEntryInfo.CompareTo(_otherEntry.SaveEntryInfo);
			}
			return 1;
		}

		// Token: 0x06006D43 RID: 27971 RVA: 0x002A7647 File Offset: 0x002A5847
		public override bool MatchesSearch(string _searchString)
		{
			return this.SaveName.ContainsCaseInsensitive(_searchString);
		}

		// Token: 0x04005348 RID: 21320
		public readonly string SaveName;

		// Token: 0x04005349 RID: 21321
		public readonly string SaveDisplayName;

		// Token: 0x0400534A RID: 21322
		public readonly string WorldKey;

		// Token: 0x0400534B RID: 21323
		public readonly string SaveDirectory;

		// Token: 0x0400534C RID: 21324
		public readonly DateTime LastSaved;

		// Token: 0x0400534D RID: 21325
		public readonly SaveInfoProvider.SaveEntryInfo SaveEntryInfo;
	}

	// Token: 0x02000DF3 RID: 3571
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_BugReportSavesList.ListEntry>.XUiC_ListEntry
	{
		// Token: 0x06006D44 RID: 27972 RVA: 0x002A7655 File Offset: 0x002A5855
		[XuiXmlBinding("savename")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingSaveName()
		{
			XUiC_BugReportSavesList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.SaveDisplayName : null) ?? "";
		}

		// Token: 0x06006D45 RID: 27973 RVA: 0x002A7672 File Offset: 0x002A5872
		[XuiXmlBinding("saveusesdatalimit")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingSaveUsesDataLimit()
		{
			return this.entryData != null && PlatformManager.MultiPlatform.UserDataRoaming.IsRoamingOptional && this.entryData.SaveEntryInfo.StorageType.UsesDataLimit();
		}

		// Token: 0x06006D46 RID: 27974 RVA: 0x002A76A4 File Offset: 0x002A58A4
		[XuiXmlBinding("worldname")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingWorldName()
		{
			XUiC_BugReportSavesList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.SaveEntryInfo.WorldEntry.Name : null) ?? "";
		}

		// Token: 0x06006D47 RID: 27975 RVA: 0x002A76CB File Offset: 0x002A58CB
		[XuiXmlBinding("worldusesdatalimit")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingWorldUsesDataLimit()
		{
			return this.entryData != null && PlatformManager.MultiPlatform.UserDataRoaming.IsRoamingOptional && this.entryData.SaveEntryInfo.WorldEntry.Location.StorageType.UsesDataLimit();
		}

		// Token: 0x06006D48 RID: 27976 RVA: 0x002A7708 File Offset: 0x002A5908
		[XuiXmlBinding("lastplayed")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingLastPlayed()
		{
			XUiC_BugReportSavesList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.LastSaved.ToString("yyyy-MM-dd HH:mm") : null) ?? "";
		}
	}
}

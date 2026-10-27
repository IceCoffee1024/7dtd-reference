using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000E0B RID: 3595
[Preserve]
public class XUiC_BugReportSavesList : XUiC_List<XUiC_BugReportSavesList.ListEntry>
{
	// Token: 0x06006D5B RID: 27995 RVA: 0x000027FC File Offset: 0x000009FC
	public override void RebuildList(bool _resetFilter = false)
	{
	}

	// Token: 0x06006D5C RID: 27996 RVA: 0x002ABE14 File Offset: 0x002AA014
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

	// Token: 0x02000E0C RID: 3596
	[Preserve]
	public class ListEntry : XUiListEntry<XUiC_BugReportSavesList.ListEntry>
	{
		// Token: 0x06006D5E RID: 27998 RVA: 0x002ABE8C File Offset: 0x002AA08C
		public ListEntry(SaveInfoProvider.SaveEntryInfo _saveEntryInfo)
		{
			this.SaveEntryInfo = _saveEntryInfo;
			this.SaveName = _saveEntryInfo.Name;
			this.SaveDisplayName = _saveEntryInfo.DisplayName;
			this.WorldKey = _saveEntryInfo.WorldEntry.WorldKey;
			this.SaveDirectory = _saveEntryInfo.SaveDir;
			this.LastSaved = _saveEntryInfo.LastSaved;
		}

		// Token: 0x06006D5F RID: 27999 RVA: 0x002ABEE7 File Offset: 0x002AA0E7
		public override int CompareTo(XUiC_BugReportSavesList.ListEntry _otherEntry)
		{
			if (_otherEntry != null)
			{
				return this.SaveEntryInfo.CompareTo(_otherEntry.SaveEntryInfo);
			}
			return 1;
		}

		// Token: 0x06006D60 RID: 28000 RVA: 0x002ABEFF File Offset: 0x002AA0FF
		public override bool MatchesSearch(string _searchString)
		{
			return this.SaveName.ContainsCaseInsensitive(_searchString);
		}

		// Token: 0x040053A4 RID: 21412
		public readonly string SaveName;

		// Token: 0x040053A5 RID: 21413
		public readonly string SaveDisplayName;

		// Token: 0x040053A6 RID: 21414
		public readonly string WorldKey;

		// Token: 0x040053A7 RID: 21415
		public readonly string SaveDirectory;

		// Token: 0x040053A8 RID: 21416
		public readonly DateTime LastSaved;

		// Token: 0x040053A9 RID: 21417
		public readonly SaveInfoProvider.SaveEntryInfo SaveEntryInfo;
	}

	// Token: 0x02000E0D RID: 3597
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_BugReportSavesList.ListEntry>.XUiC_ListEntry
	{
		// Token: 0x06006D61 RID: 28001 RVA: 0x002ABF0D File Offset: 0x002AA10D
		[XuiXmlBinding("savename")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingSaveName()
		{
			XUiC_BugReportSavesList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.SaveDisplayName : null) ?? "";
		}

		// Token: 0x06006D62 RID: 28002 RVA: 0x002ABF2A File Offset: 0x002AA12A
		[XuiXmlBinding("saveusesdatalimit")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingSaveUsesDataLimit()
		{
			return this.entryData != null && PlatformManager.MultiPlatform.UserDataRoaming.IsRoamingOptional && this.entryData.SaveEntryInfo.StorageType.UsesDataLimit();
		}

		// Token: 0x06006D63 RID: 28003 RVA: 0x002ABF5C File Offset: 0x002AA15C
		[XuiXmlBinding("worldname")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingWorldName()
		{
			XUiC_BugReportSavesList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.SaveEntryInfo.WorldEntry.Name : null) ?? "";
		}

		// Token: 0x06006D64 RID: 28004 RVA: 0x002ABF83 File Offset: 0x002AA183
		[XuiXmlBinding("worldusesdatalimit")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingWorldUsesDataLimit()
		{
			return this.entryData != null && PlatformManager.MultiPlatform.UserDataRoaming.IsRoamingOptional && this.entryData.SaveEntryInfo.WorldEntry.Location.StorageType.UsesDataLimit();
		}

		// Token: 0x06006D65 RID: 28005 RVA: 0x002ABFC0 File Offset: 0x002AA1C0
		[XuiXmlBinding("lastplayed")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingLastPlayed()
		{
			XUiC_BugReportSavesList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.LastSaved.ToString("yyyy-MM-dd HH:mm") : null) ?? "";
		}
	}
}

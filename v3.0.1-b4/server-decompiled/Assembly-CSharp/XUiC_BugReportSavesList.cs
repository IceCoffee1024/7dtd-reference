using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000DDC RID: 3548
[Preserve]
public class XUiC_BugReportSavesList : XUiC_List<XUiC_BugReportSavesList.ListEntry>
{
	// Token: 0x06006C9F RID: 27807 RVA: 0x000027FC File Offset: 0x000009FC
	public override void RebuildList(bool _resetFilter = false)
	{
	}

	// Token: 0x06006CA0 RID: 27808 RVA: 0x002A40C8 File Offset: 0x002A22C8
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

	// Token: 0x02000DDD RID: 3549
	[Preserve]
	public class ListEntry : XUiListEntry<XUiC_BugReportSavesList.ListEntry>
	{
		// Token: 0x06006CA2 RID: 27810 RVA: 0x002A4140 File Offset: 0x002A2340
		public ListEntry(SaveInfoProvider.SaveEntryInfo _saveEntryInfo)
		{
			this.SaveEntryInfo = _saveEntryInfo;
			this.SaveName = _saveEntryInfo.Name;
			this.SaveDisplayName = _saveEntryInfo.DisplayName;
			this.WorldKey = _saveEntryInfo.WorldEntry.WorldKey;
			this.SaveDirectory = _saveEntryInfo.SaveDir;
			this.LastSaved = _saveEntryInfo.LastSaved;
		}

		// Token: 0x06006CA3 RID: 27811 RVA: 0x002A419B File Offset: 0x002A239B
		public override int CompareTo(XUiC_BugReportSavesList.ListEntry _otherEntry)
		{
			if (_otherEntry != null)
			{
				return this.SaveEntryInfo.CompareTo(_otherEntry.SaveEntryInfo);
			}
			return 1;
		}

		// Token: 0x06006CA4 RID: 27812 RVA: 0x002A41B3 File Offset: 0x002A23B3
		public override bool MatchesSearch(string _searchString)
		{
			return this.SaveName.ContainsCaseInsensitive(_searchString);
		}

		// Token: 0x040052A5 RID: 21157
		public readonly string SaveName;

		// Token: 0x040052A6 RID: 21158
		public readonly string SaveDisplayName;

		// Token: 0x040052A7 RID: 21159
		public readonly string WorldKey;

		// Token: 0x040052A8 RID: 21160
		public readonly string SaveDirectory;

		// Token: 0x040052A9 RID: 21161
		public readonly DateTime LastSaved;

		// Token: 0x040052AA RID: 21162
		public readonly SaveInfoProvider.SaveEntryInfo SaveEntryInfo;
	}

	// Token: 0x02000DDE RID: 3550
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_BugReportSavesList.ListEntry>.XUiC_ListEntry
	{
		// Token: 0x06006CA5 RID: 27813 RVA: 0x002A41C1 File Offset: 0x002A23C1
		[XuiXmlBinding("savename")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingSaveName()
		{
			XUiC_BugReportSavesList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.SaveDisplayName : null) ?? "";
		}

		// Token: 0x06006CA6 RID: 27814 RVA: 0x002A41DE File Offset: 0x002A23DE
		[XuiXmlBinding("saveusesdatalimit")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingSaveUsesDataLimit()
		{
			return this.entryData != null && PlatformManager.MultiPlatform.UserDataRoaming.IsRoamingOptional && this.entryData.SaveEntryInfo.StorageType.UsesDataLimit();
		}

		// Token: 0x06006CA7 RID: 27815 RVA: 0x002A4210 File Offset: 0x002A2410
		[XuiXmlBinding("worldname")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingWorldName()
		{
			XUiC_BugReportSavesList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.SaveEntryInfo.WorldEntry.Name : null) ?? "";
		}

		// Token: 0x06006CA8 RID: 27816 RVA: 0x002A4237 File Offset: 0x002A2437
		[XuiXmlBinding("worldusesdatalimit")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingWorldUsesDataLimit()
		{
			return this.entryData != null && PlatformManager.MultiPlatform.UserDataRoaming.IsRoamingOptional && this.entryData.SaveEntryInfo.WorldEntry.Location.StorageType.UsesDataLimit();
		}

		// Token: 0x06006CA9 RID: 27817 RVA: 0x002A4274 File Offset: 0x002A2474
		[XuiXmlBinding("lastplayed")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingLastPlayed()
		{
			XUiC_BugReportSavesList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.LastSaved.ToString("yyyy-MM-dd HH:mm") : null) ?? "";
		}
	}
}

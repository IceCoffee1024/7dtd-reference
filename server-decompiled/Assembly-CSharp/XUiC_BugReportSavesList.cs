using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000E00 RID: 3584
[Preserve]
public class XUiC_BugReportSavesList : XUiC_List<XUiC_BugReportSavesList.ListEntry>
{
	// Token: 0x06006D36 RID: 27958 RVA: 0x000027FC File Offset: 0x000009FC
	public override void RebuildList(bool _resetFilter = false)
	{
	}

	// Token: 0x06006D37 RID: 27959 RVA: 0x002AA8EC File Offset: 0x002A8AEC
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

	// Token: 0x02000E01 RID: 3585
	[Preserve]
	public class ListEntry : XUiListEntry<XUiC_BugReportSavesList.ListEntry>
	{
		// Token: 0x06006D39 RID: 27961 RVA: 0x002AA964 File Offset: 0x002A8B64
		public ListEntry(SaveInfoProvider.SaveEntryInfo _saveEntryInfo)
		{
			this.SaveEntryInfo = _saveEntryInfo;
			this.SaveName = _saveEntryInfo.Name;
			this.SaveDisplayName = _saveEntryInfo.DisplayName;
			this.WorldKey = _saveEntryInfo.WorldEntry.WorldKey;
			this.SaveDirectory = _saveEntryInfo.SaveDir;
			this.LastSaved = _saveEntryInfo.LastSaved;
		}

		// Token: 0x06006D3A RID: 27962 RVA: 0x002AA9BF File Offset: 0x002A8BBF
		public override int CompareTo(XUiC_BugReportSavesList.ListEntry _otherEntry)
		{
			if (_otherEntry != null)
			{
				return this.SaveEntryInfo.CompareTo(_otherEntry.SaveEntryInfo);
			}
			return 1;
		}

		// Token: 0x06006D3B RID: 27963 RVA: 0x002AA9D7 File Offset: 0x002A8BD7
		public override bool MatchesSearch(string _searchString)
		{
			return this.SaveName.ContainsCaseInsensitive(_searchString);
		}

		// Token: 0x0400537F RID: 21375
		public readonly string SaveName;

		// Token: 0x04005380 RID: 21376
		public readonly string SaveDisplayName;

		// Token: 0x04005381 RID: 21377
		public readonly string WorldKey;

		// Token: 0x04005382 RID: 21378
		public readonly string SaveDirectory;

		// Token: 0x04005383 RID: 21379
		public readonly DateTime LastSaved;

		// Token: 0x04005384 RID: 21380
		public readonly SaveInfoProvider.SaveEntryInfo SaveEntryInfo;
	}

	// Token: 0x02000E02 RID: 3586
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_BugReportSavesList.ListEntry>.XUiC_ListEntry
	{
		// Token: 0x06006D3C RID: 27964 RVA: 0x002AA9E5 File Offset: 0x002A8BE5
		[XuiXmlBinding("savename")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingSaveName()
		{
			XUiC_BugReportSavesList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.SaveDisplayName : null) ?? "";
		}

		// Token: 0x06006D3D RID: 27965 RVA: 0x002AAA02 File Offset: 0x002A8C02
		[XuiXmlBinding("saveusesdatalimit")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingSaveUsesDataLimit()
		{
			return this.entryData != null && PlatformManager.MultiPlatform.UserDataRoaming.IsRoamingOptional && this.entryData.SaveEntryInfo.StorageType.UsesDataLimit();
		}

		// Token: 0x06006D3E RID: 27966 RVA: 0x002AAA34 File Offset: 0x002A8C34
		[XuiXmlBinding("worldname")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingWorldName()
		{
			XUiC_BugReportSavesList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.SaveEntryInfo.WorldEntry.Name : null) ?? "";
		}

		// Token: 0x06006D3F RID: 27967 RVA: 0x002AAA5B File Offset: 0x002A8C5B
		[XuiXmlBinding("worldusesdatalimit")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingWorldUsesDataLimit()
		{
			return this.entryData != null && PlatformManager.MultiPlatform.UserDataRoaming.IsRoamingOptional && this.entryData.SaveEntryInfo.WorldEntry.Location.StorageType.UsesDataLimit();
		}

		// Token: 0x06006D40 RID: 27968 RVA: 0x002AAA98 File Offset: 0x002A8C98
		[XuiXmlBinding("lastplayed")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingLastPlayed()
		{
			XUiC_BugReportSavesList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.LastSaved.ToString("yyyy-MM-dd HH:mm") : null) ?? "";
		}
	}
}

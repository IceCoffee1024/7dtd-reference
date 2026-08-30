using System;
using System.IO;
using UnityEngine.Scripting;

// Token: 0x02000FC7 RID: 4039
[Preserve]
public class XUiC_PrefabFolderList : XUiC_List<XUiC_PrefabFolderList.PrefabFolderEntry>
{
	// Token: 0x17000EE3 RID: 3811
	// (get) Token: 0x06007FA6 RID: 32678 RVA: 0x003064B5 File Offset: 0x003046B5
	// (set) Token: 0x06007FA7 RID: 32679 RVA: 0x003064BD File Offset: 0x003046BD
	public PathAbstractions.EAbstractedLocationType LocationType
	{
		get
		{
			return this.locationType;
		}
		set
		{
			if (value == this.locationType)
			{
				return;
			}
			this.locationType = value;
			this.RebuildList(false);
		}
	}

	// Token: 0x17000EE4 RID: 3812
	// (get) Token: 0x06007FA8 RID: 32680 RVA: 0x003064D7 File Offset: 0x003046D7
	// (set) Token: 0x06007FA9 RID: 32681 RVA: 0x003064DF File Offset: 0x003046DF
	public Mod Mod
	{
		get
		{
			return this.mod;
		}
		set
		{
			if (value == this.mod)
			{
				return;
			}
			this.mod = value;
			this.RebuildList(false);
		}
	}

	// Token: 0x06007FAA RID: 32682 RVA: 0x003064F9 File Offset: 0x003046F9
	public override void OnOpen()
	{
		base.OnOpen();
		this.RebuildList(false);
	}

	// Token: 0x06007FAB RID: 32683 RVA: 0x00306508 File Offset: 0x00304708
	public override void RebuildList(bool _resetFilter = false)
	{
		this.allEntries.Clear();
		string basePath = PathAbstractions.PrefabsSearchPaths.GetBasePath(this.locationType, this.mod);
		if (SdDirectory.Exists(basePath))
		{
			foreach (string text in SdDirectory.GetDirectories(basePath))
			{
				string fileName = Path.GetFileName(text);
				this.allEntries.Add(new XUiC_PrefabFolderList.PrefabFolderEntry(fileName, fileName, text));
			}
			this.allEntries.Sort();
		}
		this.allEntries.Insert(0, new XUiC_PrefabFolderList.PrefabFolderEntry("<Root>", "", basePath));
		base.RebuildList(_resetFilter);
		base.SelectedEntryIndex = 0;
	}

	// Token: 0x06007FAC RID: 32684 RVA: 0x003065AC File Offset: 0x003047AC
	public bool SelectByName(string _name)
	{
		if (string.IsNullOrEmpty(_name))
		{
			return false;
		}
		for (int i = 0; i < this.filteredEntries.Count; i++)
		{
			if (this.filteredEntries[i].Name.Equals(_name, StringComparison.OrdinalIgnoreCase))
			{
				base.SelectedEntryIndex = i;
				return true;
			}
		}
		return false;
	}

	// Token: 0x04005EA8 RID: 24232
	[PublicizedFrom(EAccessModifier.Private)]
	public PathAbstractions.EAbstractedLocationType locationType = PathAbstractions.EAbstractedLocationType.GameData;

	// Token: 0x04005EA9 RID: 24233
	[PublicizedFrom(EAccessModifier.Private)]
	public Mod mod;

	// Token: 0x02000FC8 RID: 4040
	public class PrefabFolderEntry : XUiListEntry<XUiC_PrefabFolderList.PrefabFolderEntry>
	{
		// Token: 0x06007FAE RID: 32686 RVA: 0x0030660C File Offset: 0x0030480C
		public PrefabFolderEntry(string _name, string _relativePath, string _absolutePath)
		{
			this.Name = _name;
			this.RelativePath = _relativePath;
			this.AbsolutePath = _absolutePath;
		}

		// Token: 0x06007FAF RID: 32687 RVA: 0x00306629 File Offset: 0x00304829
		public override int CompareTo(XUiC_PrefabFolderList.PrefabFolderEntry _otherEntry)
		{
			return string.Compare(this.Name, _otherEntry.Name, StringComparison.Ordinal);
		}

		// Token: 0x06007FB0 RID: 32688 RVA: 0x0030663D File Offset: 0x0030483D
		public override bool MatchesSearch(string _searchString)
		{
			return this.Name.ContainsCaseInsensitive(_searchString);
		}

		// Token: 0x04005EAA RID: 24234
		public readonly string Name;

		// Token: 0x04005EAB RID: 24235
		public readonly string RelativePath;

		// Token: 0x04005EAC RID: 24236
		public readonly string AbsolutePath;
	}

	// Token: 0x02000FC9 RID: 4041
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_PrefabFolderList.PrefabFolderEntry>.XUiC_ListEntry
	{
		// Token: 0x06007FB1 RID: 32689 RVA: 0x0030664B File Offset: 0x0030484B
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_PrefabFolderList.PrefabFolderEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.Name : null) ?? "";
		}
	}
}

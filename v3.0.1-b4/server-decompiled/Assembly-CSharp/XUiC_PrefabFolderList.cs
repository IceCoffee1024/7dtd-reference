using System;
using System.IO;
using UnityEngine.Scripting;

// Token: 0x02000FAE RID: 4014
[Preserve]
public class XUiC_PrefabFolderList : XUiC_List<XUiC_PrefabFolderList.PrefabFolderEntry>
{
	// Token: 0x17000EAA RID: 3754
	// (get) Token: 0x06007EB4 RID: 32436 RVA: 0x00301CE1 File Offset: 0x002FFEE1
	// (set) Token: 0x06007EB5 RID: 32437 RVA: 0x00301CE9 File Offset: 0x002FFEE9
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

	// Token: 0x17000EAB RID: 3755
	// (get) Token: 0x06007EB6 RID: 32438 RVA: 0x00301D03 File Offset: 0x002FFF03
	// (set) Token: 0x06007EB7 RID: 32439 RVA: 0x00301D0B File Offset: 0x002FFF0B
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

	// Token: 0x06007EB8 RID: 32440 RVA: 0x00301D25 File Offset: 0x002FFF25
	public override void OnOpen()
	{
		base.OnOpen();
		this.RebuildList(false);
	}

	// Token: 0x06007EB9 RID: 32441 RVA: 0x00301D34 File Offset: 0x002FFF34
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

	// Token: 0x06007EBA RID: 32442 RVA: 0x00301DD8 File Offset: 0x002FFFD8
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

	// Token: 0x04005DE1 RID: 24033
	[PublicizedFrom(EAccessModifier.Private)]
	public PathAbstractions.EAbstractedLocationType locationType = PathAbstractions.EAbstractedLocationType.GameData;

	// Token: 0x04005DE2 RID: 24034
	[PublicizedFrom(EAccessModifier.Private)]
	public Mod mod;

	// Token: 0x02000FAF RID: 4015
	public class PrefabFolderEntry : XUiListEntry<XUiC_PrefabFolderList.PrefabFolderEntry>
	{
		// Token: 0x06007EBC RID: 32444 RVA: 0x00301E38 File Offset: 0x00300038
		public PrefabFolderEntry(string _name, string _relativePath, string _absolutePath)
		{
			this.Name = _name;
			this.RelativePath = _relativePath;
			this.AbsolutePath = _absolutePath;
		}

		// Token: 0x06007EBD RID: 32445 RVA: 0x00301E55 File Offset: 0x00300055
		public override int CompareTo(XUiC_PrefabFolderList.PrefabFolderEntry _otherEntry)
		{
			return string.Compare(this.Name, _otherEntry.Name, StringComparison.Ordinal);
		}

		// Token: 0x06007EBE RID: 32446 RVA: 0x00301E69 File Offset: 0x00300069
		public override bool MatchesSearch(string _searchString)
		{
			return this.Name.ContainsCaseInsensitive(_searchString);
		}

		// Token: 0x04005DE3 RID: 24035
		public readonly string Name;

		// Token: 0x04005DE4 RID: 24036
		public readonly string RelativePath;

		// Token: 0x04005DE5 RID: 24037
		public readonly string AbsolutePath;
	}

	// Token: 0x02000FB0 RID: 4016
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_PrefabFolderList.PrefabFolderEntry>.XUiC_ListEntry
	{
		// Token: 0x06007EBF RID: 32447 RVA: 0x00301E77 File Offset: 0x00300077
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_PrefabFolderList.PrefabFolderEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.Name : null) ?? "";
		}
	}
}

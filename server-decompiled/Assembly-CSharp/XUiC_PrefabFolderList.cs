using System;
using System.IO;
using UnityEngine.Scripting;

// Token: 0x02000FD6 RID: 4054
[Preserve]
public class XUiC_PrefabFolderList : XUiC_List<XUiC_PrefabFolderList.PrefabFolderEntry>
{
	// Token: 0x17000F0A RID: 3850
	// (get) Token: 0x06007FB5 RID: 32693 RVA: 0x0030948D File Offset: 0x0030768D
	// (set) Token: 0x06007FB6 RID: 32694 RVA: 0x00309495 File Offset: 0x00307695
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

	// Token: 0x17000F0B RID: 3851
	// (get) Token: 0x06007FB7 RID: 32695 RVA: 0x003094AF File Offset: 0x003076AF
	// (set) Token: 0x06007FB8 RID: 32696 RVA: 0x003094B7 File Offset: 0x003076B7
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

	// Token: 0x06007FB9 RID: 32697 RVA: 0x003094D1 File Offset: 0x003076D1
	public override void OnOpen()
	{
		base.OnOpen();
		this.RebuildList(false);
	}

	// Token: 0x06007FBA RID: 32698 RVA: 0x003094E0 File Offset: 0x003076E0
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

	// Token: 0x06007FBB RID: 32699 RVA: 0x00309584 File Offset: 0x00307784
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

	// Token: 0x04005ED1 RID: 24273
	[PublicizedFrom(EAccessModifier.Private)]
	public PathAbstractions.EAbstractedLocationType locationType = PathAbstractions.EAbstractedLocationType.GameData;

	// Token: 0x04005ED2 RID: 24274
	[PublicizedFrom(EAccessModifier.Private)]
	public Mod mod;

	// Token: 0x02000FD7 RID: 4055
	public class PrefabFolderEntry : XUiListEntry<XUiC_PrefabFolderList.PrefabFolderEntry>
	{
		// Token: 0x06007FBD RID: 32701 RVA: 0x003095E4 File Offset: 0x003077E4
		public PrefabFolderEntry(string _name, string _relativePath, string _absolutePath)
		{
			this.Name = _name;
			this.RelativePath = _relativePath;
			this.AbsolutePath = _absolutePath;
		}

		// Token: 0x06007FBE RID: 32702 RVA: 0x00309601 File Offset: 0x00307801
		public override int CompareTo(XUiC_PrefabFolderList.PrefabFolderEntry _otherEntry)
		{
			return string.Compare(this.Name, _otherEntry.Name, StringComparison.Ordinal);
		}

		// Token: 0x06007FBF RID: 32703 RVA: 0x00309615 File Offset: 0x00307815
		public override bool MatchesSearch(string _searchString)
		{
			return this.Name.ContainsCaseInsensitive(_searchString);
		}

		// Token: 0x04005ED3 RID: 24275
		public readonly string Name;

		// Token: 0x04005ED4 RID: 24276
		public readonly string RelativePath;

		// Token: 0x04005ED5 RID: 24277
		public readonly string AbsolutePath;
	}

	// Token: 0x02000FD8 RID: 4056
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_PrefabFolderList.PrefabFolderEntry>.XUiC_ListEntry
	{
		// Token: 0x06007FC0 RID: 32704 RVA: 0x00309623 File Offset: 0x00307823
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_PrefabFolderList.PrefabFolderEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.Name : null) ?? "";
		}
	}
}

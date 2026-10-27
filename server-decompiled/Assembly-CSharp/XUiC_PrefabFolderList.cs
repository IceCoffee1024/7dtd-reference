using System;
using System.IO;
using UnityEngine.Scripting;

// Token: 0x02000FE6 RID: 4070
[Preserve]
public class XUiC_PrefabFolderList : XUiC_List<XUiC_PrefabFolderList.PrefabFolderEntry>
{
	// Token: 0x17000F15 RID: 3861
	// (get) Token: 0x06008004 RID: 32772 RVA: 0x0030AFA9 File Offset: 0x003091A9
	// (set) Token: 0x06008005 RID: 32773 RVA: 0x0030AFB1 File Offset: 0x003091B1
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

	// Token: 0x17000F16 RID: 3862
	// (get) Token: 0x06008006 RID: 32774 RVA: 0x0030AFCB File Offset: 0x003091CB
	// (set) Token: 0x06008007 RID: 32775 RVA: 0x0030AFD3 File Offset: 0x003091D3
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

	// Token: 0x06008008 RID: 32776 RVA: 0x0030AFED File Offset: 0x003091ED
	public override void OnOpen()
	{
		base.OnOpen();
		this.RebuildList(false);
	}

	// Token: 0x06008009 RID: 32777 RVA: 0x0030AFFC File Offset: 0x003091FC
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

	// Token: 0x0600800A RID: 32778 RVA: 0x0030B0A0 File Offset: 0x003092A0
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

	// Token: 0x04005F0C RID: 24332
	[PublicizedFrom(EAccessModifier.Private)]
	public PathAbstractions.EAbstractedLocationType locationType = PathAbstractions.EAbstractedLocationType.GameData;

	// Token: 0x04005F0D RID: 24333
	[PublicizedFrom(EAccessModifier.Private)]
	public Mod mod;

	// Token: 0x02000FE7 RID: 4071
	public class PrefabFolderEntry : XUiListEntry<XUiC_PrefabFolderList.PrefabFolderEntry>
	{
		// Token: 0x0600800C RID: 32780 RVA: 0x0030B100 File Offset: 0x00309300
		public PrefabFolderEntry(string _name, string _relativePath, string _absolutePath)
		{
			this.Name = _name;
			this.RelativePath = _relativePath;
			this.AbsolutePath = _absolutePath;
		}

		// Token: 0x0600800D RID: 32781 RVA: 0x0030B11D File Offset: 0x0030931D
		public override int CompareTo(XUiC_PrefabFolderList.PrefabFolderEntry _otherEntry)
		{
			return string.Compare(this.Name, _otherEntry.Name, StringComparison.Ordinal);
		}

		// Token: 0x0600800E RID: 32782 RVA: 0x0030B131 File Offset: 0x00309331
		public override bool MatchesSearch(string _searchString)
		{
			return this.Name.ContainsCaseInsensitive(_searchString);
		}

		// Token: 0x04005F0E RID: 24334
		public readonly string Name;

		// Token: 0x04005F0F RID: 24335
		public readonly string RelativePath;

		// Token: 0x04005F10 RID: 24336
		public readonly string AbsolutePath;
	}

	// Token: 0x02000FE8 RID: 4072
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_PrefabFolderList.PrefabFolderEntry>.XUiC_ListEntry
	{
		// Token: 0x0600800F RID: 32783 RVA: 0x0030B13F File Offset: 0x0030933F
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_PrefabFolderList.PrefabFolderEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.Name : null) ?? "";
		}
	}
}

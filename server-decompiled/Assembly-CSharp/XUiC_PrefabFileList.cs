using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000FD3 RID: 4051
[Preserve]
public class XUiC_PrefabFileList : XUiC_List<XUiC_PrefabFileList.PrefabFileEntry>
{
	// Token: 0x06007FAA RID: 32682 RVA: 0x0030921C File Offset: 0x0030741C
	public override void RebuildList(bool _resetFilter = false)
	{
		this.allEntries.Clear();
		this.prefabSearchList.Clear();
		PrefabEditModeManager.Instance.FindPrefabs(this.groupFilter, this.prefabSearchList);
		foreach (PathAbstractions.AbstractedLocation abstractedLocation in this.prefabSearchList)
		{
			this.allEntries.Add(new XUiC_PrefabFileList.PrefabFileEntry(abstractedLocation));
		}
		this.allEntries.Sort();
		base.RebuildList(_resetFilter);
	}

	// Token: 0x06007FAB RID: 32683 RVA: 0x003092B8 File Offset: 0x003074B8
	public void SetGroupFilter(string _filter)
	{
		this.groupFilter = _filter;
		this.RebuildList(true);
	}

	// Token: 0x06007FAC RID: 32684 RVA: 0x003092C8 File Offset: 0x003074C8
	public bool SelectByName(string _name)
	{
		if (!string.IsNullOrEmpty(_name))
		{
			for (int i = 0; i < this.filteredEntries.Count; i++)
			{
				if (this.filteredEntries[i].location.Name.EqualsCaseInsensitive(_name))
				{
					base.SelectedEntryIndex = i;
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06007FAD RID: 32685 RVA: 0x0030931C File Offset: 0x0030751C
	public bool SelectByLocation(PathAbstractions.AbstractedLocation _location)
	{
		if (_location.Type == PathAbstractions.EAbstractedLocationType.None)
		{
			return false;
		}
		for (int i = 0; i < this.filteredEntries.Count; i++)
		{
			if (this.filteredEntries[i].location == _location)
			{
				base.SelectedEntryIndex = i;
				return true;
			}
		}
		return false;
	}

	// Token: 0x04005ECE RID: 24270
	[PublicizedFrom(EAccessModifier.Private)]
	public string groupFilter;

	// Token: 0x04005ECF RID: 24271
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<PathAbstractions.AbstractedLocation> prefabSearchList = new List<PathAbstractions.AbstractedLocation>();

	// Token: 0x02000FD4 RID: 4052
	public class PrefabFileEntry : XUiListEntry<XUiC_PrefabFileList.PrefabFileEntry>
	{
		// Token: 0x06007FAF RID: 32687 RVA: 0x00309380 File Offset: 0x00307580
		public PrefabFileEntry(PathAbstractions.AbstractedLocation _location)
		{
			this.location = _location;
		}

		// Token: 0x06007FB0 RID: 32688 RVA: 0x0030938F File Offset: 0x0030758F
		public override int CompareTo(XUiC_PrefabFileList.PrefabFileEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return 1;
			}
			return this.location.CompareTo(_otherEntry.location);
		}

		// Token: 0x06007FB1 RID: 32689 RVA: 0x003093A7 File Offset: 0x003075A7
		public override bool MatchesSearch(string _searchString)
		{
			return this.location.Name.ContainsCaseInsensitive(_searchString);
		}

		// Token: 0x04005ED0 RID: 24272
		public readonly PathAbstractions.AbstractedLocation location;
	}

	// Token: 0x02000FD5 RID: 4053
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_PrefabFileList.PrefabFileEntry>.XUiC_ListEntry
	{
		// Token: 0x06007FB2 RID: 32690 RVA: 0x003093BC File Offset: 0x003075BC
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			if (this.entryData == null)
			{
				return "";
			}
			PathAbstractions.EAbstractedLocationType type = this.entryData.location.Type;
			string text;
			if (type != PathAbstractions.EAbstractedLocationType.Mods)
			{
				if (type != PathAbstractions.EAbstractedLocationType.GameData)
				{
					text = " (from " + this.entryData.location.Type.ToStringCached<PathAbstractions.EAbstractedLocationType>() + ")";
				}
				else
				{
					text = "";
				}
			}
			else
			{
				text = " (Mod: " + this.entryData.location.ContainingMod.Name + ")";
			}
			string text2 = text;
			return this.entryData.location.Name + text2;
		}

		// Token: 0x06007FB3 RID: 32691 RVA: 0x0030945E File Offset: 0x0030765E
		[XuiXmlBinding("localizedname")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingLocalizedName()
		{
			if (this.entryData != null)
			{
				return Localization.Get(this.entryData.location.Name, false, null);
			}
			return "";
		}
	}
}

using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000FAB RID: 4011
[Preserve]
public class XUiC_PrefabFileList : XUiC_List<XUiC_PrefabFileList.PrefabFileEntry>
{
	// Token: 0x06007EA9 RID: 32425 RVA: 0x00301A70 File Offset: 0x002FFC70
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

	// Token: 0x06007EAA RID: 32426 RVA: 0x00301B0C File Offset: 0x002FFD0C
	public void SetGroupFilter(string _filter)
	{
		this.groupFilter = _filter;
		this.RebuildList(true);
	}

	// Token: 0x06007EAB RID: 32427 RVA: 0x00301B1C File Offset: 0x002FFD1C
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

	// Token: 0x06007EAC RID: 32428 RVA: 0x00301B70 File Offset: 0x002FFD70
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

	// Token: 0x04005DDE RID: 24030
	[PublicizedFrom(EAccessModifier.Private)]
	public string groupFilter;

	// Token: 0x04005DDF RID: 24031
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<PathAbstractions.AbstractedLocation> prefabSearchList = new List<PathAbstractions.AbstractedLocation>();

	// Token: 0x02000FAC RID: 4012
	public class PrefabFileEntry : XUiListEntry<XUiC_PrefabFileList.PrefabFileEntry>
	{
		// Token: 0x06007EAE RID: 32430 RVA: 0x00301BD4 File Offset: 0x002FFDD4
		public PrefabFileEntry(PathAbstractions.AbstractedLocation _location)
		{
			this.location = _location;
		}

		// Token: 0x06007EAF RID: 32431 RVA: 0x00301BE3 File Offset: 0x002FFDE3
		public override int CompareTo(XUiC_PrefabFileList.PrefabFileEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return 1;
			}
			return this.location.CompareTo(_otherEntry.location);
		}

		// Token: 0x06007EB0 RID: 32432 RVA: 0x00301BFB File Offset: 0x002FFDFB
		public override bool MatchesSearch(string _searchString)
		{
			return this.location.Name.ContainsCaseInsensitive(_searchString);
		}

		// Token: 0x04005DE0 RID: 24032
		public readonly PathAbstractions.AbstractedLocation location;
	}

	// Token: 0x02000FAD RID: 4013
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_PrefabFileList.PrefabFileEntry>.XUiC_ListEntry
	{
		// Token: 0x06007EB1 RID: 32433 RVA: 0x00301C10 File Offset: 0x002FFE10
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

		// Token: 0x06007EB2 RID: 32434 RVA: 0x00301CB2 File Offset: 0x002FFEB2
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

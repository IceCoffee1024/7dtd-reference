using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000FE3 RID: 4067
[Preserve]
public class XUiC_PrefabFileList : XUiC_List<XUiC_PrefabFileList.PrefabFileEntry>
{
	// Token: 0x06007FF9 RID: 32761 RVA: 0x0030AD38 File Offset: 0x00308F38
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

	// Token: 0x06007FFA RID: 32762 RVA: 0x0030ADD4 File Offset: 0x00308FD4
	public void SetGroupFilter(string _filter)
	{
		this.groupFilter = _filter;
		this.RebuildList(true);
	}

	// Token: 0x06007FFB RID: 32763 RVA: 0x0030ADE4 File Offset: 0x00308FE4
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

	// Token: 0x06007FFC RID: 32764 RVA: 0x0030AE38 File Offset: 0x00309038
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

	// Token: 0x04005F09 RID: 24329
	[PublicizedFrom(EAccessModifier.Private)]
	public string groupFilter;

	// Token: 0x04005F0A RID: 24330
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<PathAbstractions.AbstractedLocation> prefabSearchList = new List<PathAbstractions.AbstractedLocation>();

	// Token: 0x02000FE4 RID: 4068
	public class PrefabFileEntry : XUiListEntry<XUiC_PrefabFileList.PrefabFileEntry>
	{
		// Token: 0x06007FFE RID: 32766 RVA: 0x0030AE9C File Offset: 0x0030909C
		public PrefabFileEntry(PathAbstractions.AbstractedLocation _location)
		{
			this.location = _location;
		}

		// Token: 0x06007FFF RID: 32767 RVA: 0x0030AEAB File Offset: 0x003090AB
		public override int CompareTo(XUiC_PrefabFileList.PrefabFileEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return 1;
			}
			return this.location.CompareTo(_otherEntry.location);
		}

		// Token: 0x06008000 RID: 32768 RVA: 0x0030AEC3 File Offset: 0x003090C3
		public override bool MatchesSearch(string _searchString)
		{
			return this.location.Name.ContainsCaseInsensitive(_searchString);
		}

		// Token: 0x04005F0B RID: 24331
		public readonly PathAbstractions.AbstractedLocation location;
	}

	// Token: 0x02000FE5 RID: 4069
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_PrefabFileList.PrefabFileEntry>.XUiC_ListEntry
	{
		// Token: 0x06008001 RID: 32769 RVA: 0x0030AED8 File Offset: 0x003090D8
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

		// Token: 0x06008002 RID: 32770 RVA: 0x0030AF7A File Offset: 0x0030917A
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

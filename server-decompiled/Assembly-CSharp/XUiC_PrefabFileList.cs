using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000FC4 RID: 4036
[Preserve]
public class XUiC_PrefabFileList : XUiC_List<XUiC_PrefabFileList.PrefabFileEntry>
{
	// Token: 0x06007F9B RID: 32667 RVA: 0x00306244 File Offset: 0x00304444
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

	// Token: 0x06007F9C RID: 32668 RVA: 0x003062E0 File Offset: 0x003044E0
	public void SetGroupFilter(string _filter)
	{
		this.groupFilter = _filter;
		this.RebuildList(true);
	}

	// Token: 0x06007F9D RID: 32669 RVA: 0x003062F0 File Offset: 0x003044F0
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

	// Token: 0x06007F9E RID: 32670 RVA: 0x00306344 File Offset: 0x00304544
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

	// Token: 0x04005EA5 RID: 24229
	[PublicizedFrom(EAccessModifier.Private)]
	public string groupFilter;

	// Token: 0x04005EA6 RID: 24230
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<PathAbstractions.AbstractedLocation> prefabSearchList = new List<PathAbstractions.AbstractedLocation>();

	// Token: 0x02000FC5 RID: 4037
	public class PrefabFileEntry : XUiListEntry<XUiC_PrefabFileList.PrefabFileEntry>
	{
		// Token: 0x06007FA0 RID: 32672 RVA: 0x003063A8 File Offset: 0x003045A8
		public PrefabFileEntry(PathAbstractions.AbstractedLocation _location)
		{
			this.location = _location;
		}

		// Token: 0x06007FA1 RID: 32673 RVA: 0x003063B7 File Offset: 0x003045B7
		public override int CompareTo(XUiC_PrefabFileList.PrefabFileEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return 1;
			}
			return this.location.CompareTo(_otherEntry.location);
		}

		// Token: 0x06007FA2 RID: 32674 RVA: 0x003063CF File Offset: 0x003045CF
		public override bool MatchesSearch(string _searchString)
		{
			return this.location.Name.ContainsCaseInsensitive(_searchString);
		}

		// Token: 0x04005EA7 RID: 24231
		public readonly PathAbstractions.AbstractedLocation location;
	}

	// Token: 0x02000FC6 RID: 4038
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_PrefabFileList.PrefabFileEntry>.XUiC_ListEntry
	{
		// Token: 0x06007FA3 RID: 32675 RVA: 0x003063E4 File Offset: 0x003045E4
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

		// Token: 0x06007FA4 RID: 32676 RVA: 0x00306486 File Offset: 0x00304686
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

using System;
using UniLinq;
using UnityEngine.Scripting;

// Token: 0x02000E4B RID: 3659
[Preserve]
public class XUiC_ProfilesList : XUiC_List<XUiC_ProfilesList.ListEntry>
{
	// Token: 0x06006FD3 RID: 28627 RVA: 0x002B5F72 File Offset: 0x002B4172
	public override void OnOpen()
	{
		base.OnOpen();
		this.RebuildList(false);
	}

	// Token: 0x06006FD4 RID: 28628 RVA: 0x002B5F84 File Offset: 0x002B4184
	public override void RebuildList(bool _resetFilter = false)
	{
		this.allEntries.Clear();
		foreach (string text in Archetype.s_Archetypes.Keys)
		{
			if (text != "BaseMale" && text != "BaseFemale")
			{
				this.allEntries.Add(new XUiC_ProfilesList.ListEntry(text, true));
			}
		}
		foreach (string text2 in (from s in ProfileSDF.GetProfiles()
			where Archetype.GetArchetype(s) == null && ProfileSDF.GetArchetype(s) != null && (ProfileSDF.GetArchetype(s).Equals("BaseMale") || ProfileSDF.GetArchetype(s).Equals("BaseFemale"))
			select s).ToArray<string>())
		{
			this.allEntries.Add(new XUiC_ProfilesList.ListEntry(text2, false));
		}
		this.allEntries.Sort();
		base.RebuildList(_resetFilter);
		this.SelectByName(ProfileSDF.CurrentProfileName());
	}

	// Token: 0x06006FD5 RID: 28629 RVA: 0x002B6080 File Offset: 0x002B4280
	public bool SelectByName(string _name)
	{
		if (!string.IsNullOrEmpty(_name))
		{
			for (int i = 0; i < this.filteredEntries.Count; i++)
			{
				if (this.filteredEntries[i].Name.Equals(_name, StringComparison.OrdinalIgnoreCase))
				{
					base.SelectedEntryIndex = i;
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x02000E4C RID: 3660
	[Preserve]
	public class ListEntry : XUiListEntry<XUiC_ProfilesList.ListEntry>
	{
		// Token: 0x06006FD7 RID: 28631 RVA: 0x002B60D7 File Offset: 0x002B42D7
		public ListEntry(string _name, bool _isArchetype)
		{
			this.Name = _name;
			this.isArchetype = _isArchetype;
		}

		// Token: 0x06006FD8 RID: 28632 RVA: 0x002B60F0 File Offset: 0x002B42F0
		public override int CompareTo(XUiC_ProfilesList.ListEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return 1;
			}
			int num = -this.isArchetype.CompareTo(_otherEntry.isArchetype);
			if (num != 0)
			{
				return num;
			}
			return string.Compare(this.Name, _otherEntry.Name, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06006FD9 RID: 28633 RVA: 0x002B612F File Offset: 0x002B432F
		public override bool MatchesSearch(string _searchString)
		{
			return this.Name.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x040054E1 RID: 21729
		public readonly string Name;

		// Token: 0x040054E2 RID: 21730
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool isArchetype;
	}

	// Token: 0x02000E4D RID: 3661
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_ProfilesList.ListEntry>.XUiC_ListEntry
	{
		// Token: 0x06006FDA RID: 28634 RVA: 0x002B6144 File Offset: 0x002B4344
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_ProfilesList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.Name : null) ?? "";
		}
	}
}

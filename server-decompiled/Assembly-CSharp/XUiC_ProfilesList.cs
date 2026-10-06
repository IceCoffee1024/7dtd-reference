using System;
using UniLinq;
using UnityEngine.Scripting;

// Token: 0x02000E40 RID: 3648
[Preserve]
public class XUiC_ProfilesList : XUiC_List<XUiC_ProfilesList.ListEntry>
{
	// Token: 0x06006FAE RID: 28590 RVA: 0x002B4A06 File Offset: 0x002B2C06
	public override void OnOpen()
	{
		base.OnOpen();
		this.RebuildList(false);
	}

	// Token: 0x06006FAF RID: 28591 RVA: 0x002B4A18 File Offset: 0x002B2C18
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

	// Token: 0x06006FB0 RID: 28592 RVA: 0x002B4B14 File Offset: 0x002B2D14
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

	// Token: 0x02000E41 RID: 3649
	[Preserve]
	public class ListEntry : XUiListEntry<XUiC_ProfilesList.ListEntry>
	{
		// Token: 0x06006FB2 RID: 28594 RVA: 0x002B4B6B File Offset: 0x002B2D6B
		public ListEntry(string _name, bool _isArchetype)
		{
			this.Name = _name;
			this.isArchetype = _isArchetype;
		}

		// Token: 0x06006FB3 RID: 28595 RVA: 0x002B4B84 File Offset: 0x002B2D84
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

		// Token: 0x06006FB4 RID: 28596 RVA: 0x002B4BC3 File Offset: 0x002B2DC3
		public override bool MatchesSearch(string _searchString)
		{
			return this.Name.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x040054BC RID: 21692
		public readonly string Name;

		// Token: 0x040054BD RID: 21693
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool isArchetype;
	}

	// Token: 0x02000E42 RID: 3650
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_ProfilesList.ListEntry>.XUiC_ListEntry
	{
		// Token: 0x06006FB5 RID: 28597 RVA: 0x002B4BD8 File Offset: 0x002B2DD8
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_ProfilesList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.Name : null) ?? "";
		}
	}
}

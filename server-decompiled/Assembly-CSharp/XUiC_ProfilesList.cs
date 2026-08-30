using System;
using UniLinq;
using UnityEngine.Scripting;

// Token: 0x02000E31 RID: 3633
[Preserve]
public class XUiC_ProfilesList : XUiC_List<XUiC_ProfilesList.ListEntry>
{
	// Token: 0x06006FB6 RID: 28598 RVA: 0x002B1656 File Offset: 0x002AF856
	public override void OnOpen()
	{
		base.OnOpen();
		this.RebuildList(false);
	}

	// Token: 0x06006FB7 RID: 28599 RVA: 0x002B1668 File Offset: 0x002AF868
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

	// Token: 0x06006FB8 RID: 28600 RVA: 0x002B1764 File Offset: 0x002AF964
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

	// Token: 0x02000E32 RID: 3634
	[Preserve]
	public class ListEntry : XUiListEntry<XUiC_ProfilesList.ListEntry>
	{
		// Token: 0x06006FBA RID: 28602 RVA: 0x002B17BB File Offset: 0x002AF9BB
		public ListEntry(string _name, bool _isArchetype)
		{
			this.Name = _name;
			this.isArchetype = _isArchetype;
		}

		// Token: 0x06006FBB RID: 28603 RVA: 0x002B17D4 File Offset: 0x002AF9D4
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

		// Token: 0x06006FBC RID: 28604 RVA: 0x002B1813 File Offset: 0x002AFA13
		public override bool MatchesSearch(string _searchString)
		{
			return this.Name.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x04005485 RID: 21637
		public readonly string Name;

		// Token: 0x04005486 RID: 21638
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool isArchetype;
	}

	// Token: 0x02000E33 RID: 3635
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_ProfilesList.ListEntry>.XUiC_ListEntry
	{
		// Token: 0x06006FBD RID: 28605 RVA: 0x002B1828 File Offset: 0x002AFA28
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_ProfilesList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.Name : null) ?? "";
		}
	}
}

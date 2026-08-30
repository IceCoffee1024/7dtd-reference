using System;
using UniLinq;
using UnityEngine.Scripting;

// Token: 0x02000E1C RID: 3612
[Preserve]
public class XUiC_ProfilesList : XUiC_List<XUiC_ProfilesList.ListEntry>
{
	// Token: 0x06006F16 RID: 28438 RVA: 0x002AE135 File Offset: 0x002AC335
	public override void OnOpen()
	{
		base.OnOpen();
		this.RebuildList(false);
	}

	// Token: 0x06006F17 RID: 28439 RVA: 0x002AE144 File Offset: 0x002AC344
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

	// Token: 0x06006F18 RID: 28440 RVA: 0x002AE240 File Offset: 0x002AC440
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

	// Token: 0x02000E1D RID: 3613
	[Preserve]
	public class ListEntry : XUiListEntry<XUiC_ProfilesList.ListEntry>
	{
		// Token: 0x06006F1A RID: 28442 RVA: 0x002AE297 File Offset: 0x002AC497
		public ListEntry(string _name, bool _isArchetype)
		{
			this.Name = _name;
			this.isArchetype = _isArchetype;
		}

		// Token: 0x06006F1B RID: 28443 RVA: 0x002AE2B0 File Offset: 0x002AC4B0
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

		// Token: 0x06006F1C RID: 28444 RVA: 0x002AE2EF File Offset: 0x002AC4EF
		public override bool MatchesSearch(string _searchString)
		{
			return this.Name.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x040053E1 RID: 21473
		public readonly string Name;

		// Token: 0x040053E2 RID: 21474
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool isArchetype;
	}

	// Token: 0x02000E1E RID: 3614
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_ProfilesList.ListEntry>.XUiC_ListEntry
	{
		// Token: 0x06006F1D RID: 28445 RVA: 0x002AE304 File Offset: 0x002AC504
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_ProfilesList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.Name : null) ?? "";
		}
	}
}

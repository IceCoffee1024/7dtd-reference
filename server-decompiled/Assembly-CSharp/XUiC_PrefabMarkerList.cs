using System;
using PrefabVolumes;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FDE RID: 4062
[Preserve]
public class XUiC_PrefabMarkerList : XUiC_List<XUiC_PrefabMarkerList.PrefabMarkerEntry>
{
	// Token: 0x06007FEA RID: 32746 RVA: 0x0030A658 File Offset: 0x00308858
	public override void OnOpen()
	{
		base.OnOpen();
		this.RebuildList(false);
	}

	// Token: 0x06007FEB RID: 32747 RVA: 0x0030A668 File Offset: 0x00308868
	public override void RebuildList(bool _resetFilter = false)
	{
		if (PrefabEditModeManager.Instance.VoxelPrefab == null)
		{
			return;
		}
		this.allEntries.Clear();
		foreach (SelectionBox selectionBox in POIMarkerToolManager.GetRegisteredBoxes())
		{
			this.allEntries.Add(new XUiC_PrefabMarkerList.PrefabMarkerEntry(this, selectionBox));
		}
		this.allEntries.Sort();
		base.RebuildList(_resetFilter);
	}

	// Token: 0x06007FEC RID: 32748 RVA: 0x0030A6EC File Offset: 0x003088EC
	public bool SelectByMarker(Marker _marker)
	{
		if (_marker == null)
		{
			return false;
		}
		for (int i = 0; i < this.filteredEntries.Count; i++)
		{
			if (this.filteredEntries[i].Marker == _marker)
			{
				base.SelectedEntryIndex = i;
				return true;
			}
		}
		return false;
	}

	// Token: 0x02000FDF RID: 4063
	[Preserve]
	public class PrefabMarkerEntry : XUiListEntry<XUiC_PrefabMarkerList.PrefabMarkerEntry>
	{
		// Token: 0x17000F0C RID: 3852
		// (get) Token: 0x06007FEE RID: 32750 RVA: 0x0030A73A File Offset: 0x0030893A
		public Marker Marker
		{
			get
			{
				return this.Box.UserData as Marker;
			}
		}

		// Token: 0x06007FEF RID: 32751 RVA: 0x0030A74C File Offset: 0x0030894C
		public PrefabMarkerEntry(XUiC_PrefabMarkerList _parentList, SelectionBox _box)
		{
			this.parentList = _parentList;
			this.Box = _box;
		}

		// Token: 0x06007FF0 RID: 32752 RVA: 0x0030A764 File Offset: 0x00308964
		public override int CompareTo(XUiC_PrefabMarkerList.PrefabMarkerEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return 1;
			}
			int num = string.Compare(this.Marker.GroupName, _otherEntry.Marker.GroupName, StringComparison.OrdinalIgnoreCase);
			if (num != 0)
			{
				return num;
			}
			return string.Compare(this.Box.name, _otherEntry.Box.name, StringComparison.Ordinal);
		}

		// Token: 0x06007FF1 RID: 32753 RVA: 0x0030A7B4 File Offset: 0x003089B4
		public override bool MatchesSearch(string _searchString)
		{
			return this.Marker.GroupName.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x04005EE8 RID: 24296
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiC_PrefabMarkerList parentList;

		// Token: 0x04005EE9 RID: 24297
		public readonly SelectionBox Box;

		// Token: 0x04005EEA RID: 24298
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly CachedStringFormatterXuiRgbaColor colorFormatter = new CachedStringFormatterXuiRgbaColor();
	}

	// Token: 0x02000FE0 RID: 4064
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_PrefabMarkerList.PrefabMarkerEntry>.XUiC_ListEntry
	{
		// Token: 0x06007FF3 RID: 32755 RVA: 0x0030A7DA File Offset: 0x003089DA
		[XuiXmlBinding("groupname")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingGroupName()
		{
			XUiC_PrefabMarkerList.PrefabMarkerEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.Marker.GroupName : null) ?? "";
		}

		// Token: 0x06007FF4 RID: 32756 RVA: 0x0030A7FC File Offset: 0x003089FC
		[XuiXmlBinding("markertype")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingMarkerType()
		{
			XUiC_PrefabMarkerList.PrefabMarkerEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.Marker.MarkerType.ToString() : null) ?? "";
		}

		// Token: 0x06007FF5 RID: 32757 RVA: 0x0030A838 File Offset: 0x00308A38
		[XuiXmlBinding("groupcolor")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingGroupColor()
		{
			if (this.entryData == null)
			{
				return "0,0,0,0";
			}
			Color groupColor = this.entryData.Marker.GroupColor;
			groupColor.a = 1f;
			return groupColor.ToXuiColorString();
		}

		// Token: 0x06007FF6 RID: 32758 RVA: 0x0030A87C File Offset: 0x00308A7C
		[XuiXmlBinding("markersize")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingMarkerSize()
		{
			if (this.entryData == null)
			{
				return "";
			}
			int num = Marker.MarkerSizes.IndexOf(this.entryData.Marker.size);
			if (num >= 0)
			{
				return ((Marker.MarkerSize)num).ToString();
			}
			return Marker.MarkerSize.Custom.ToString();
		}
	}
}

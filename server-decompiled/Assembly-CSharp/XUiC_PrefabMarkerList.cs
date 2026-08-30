using System;
using PrefabVolumes;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FCF RID: 4047
[Preserve]
public class XUiC_PrefabMarkerList : XUiC_List<XUiC_PrefabMarkerList.PrefabMarkerEntry>
{
	// Token: 0x06007FDB RID: 32731 RVA: 0x00307680 File Offset: 0x00305880
	public override void OnOpen()
	{
		base.OnOpen();
		this.RebuildList(false);
	}

	// Token: 0x06007FDC RID: 32732 RVA: 0x00307690 File Offset: 0x00305890
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

	// Token: 0x06007FDD RID: 32733 RVA: 0x00307714 File Offset: 0x00305914
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

	// Token: 0x02000FD0 RID: 4048
	[Preserve]
	public class PrefabMarkerEntry : XUiListEntry<XUiC_PrefabMarkerList.PrefabMarkerEntry>
	{
		// Token: 0x17000EE5 RID: 3813
		// (get) Token: 0x06007FDF RID: 32735 RVA: 0x00307762 File Offset: 0x00305962
		public Marker Marker
		{
			get
			{
				return this.Box.UserData as Marker;
			}
		}

		// Token: 0x06007FE0 RID: 32736 RVA: 0x00307774 File Offset: 0x00305974
		public PrefabMarkerEntry(XUiC_PrefabMarkerList _parentList, SelectionBox _box)
		{
			this.parentList = _parentList;
			this.Box = _box;
		}

		// Token: 0x06007FE1 RID: 32737 RVA: 0x0030778C File Offset: 0x0030598C
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

		// Token: 0x06007FE2 RID: 32738 RVA: 0x003077DC File Offset: 0x003059DC
		public override bool MatchesSearch(string _searchString)
		{
			return this.Marker.GroupName.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x04005EBF RID: 24255
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiC_PrefabMarkerList parentList;

		// Token: 0x04005EC0 RID: 24256
		public readonly SelectionBox Box;

		// Token: 0x04005EC1 RID: 24257
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly CachedStringFormatterXuiRgbaColor colorFormatter = new CachedStringFormatterXuiRgbaColor();
	}

	// Token: 0x02000FD1 RID: 4049
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_PrefabMarkerList.PrefabMarkerEntry>.XUiC_ListEntry
	{
		// Token: 0x06007FE4 RID: 32740 RVA: 0x00307802 File Offset: 0x00305A02
		[XuiXmlBinding("groupname")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingGroupName()
		{
			XUiC_PrefabMarkerList.PrefabMarkerEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.Marker.GroupName : null) ?? "";
		}

		// Token: 0x06007FE5 RID: 32741 RVA: 0x00307824 File Offset: 0x00305A24
		[XuiXmlBinding("markertype")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingMarkerType()
		{
			XUiC_PrefabMarkerList.PrefabMarkerEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.Marker.MarkerType.ToString() : null) ?? "";
		}

		// Token: 0x06007FE6 RID: 32742 RVA: 0x00307860 File Offset: 0x00305A60
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

		// Token: 0x06007FE7 RID: 32743 RVA: 0x003078A4 File Offset: 0x00305AA4
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

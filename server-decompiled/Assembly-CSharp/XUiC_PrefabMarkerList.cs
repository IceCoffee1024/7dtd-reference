using System;
using PrefabVolumes;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FEE RID: 4078
[Preserve]
public class XUiC_PrefabMarkerList : XUiC_List<XUiC_PrefabMarkerList.PrefabMarkerEntry>
{
	// Token: 0x06008039 RID: 32825 RVA: 0x0030C174 File Offset: 0x0030A374
	public override void OnOpen()
	{
		base.OnOpen();
		this.RebuildList(false);
	}

	// Token: 0x0600803A RID: 32826 RVA: 0x0030C184 File Offset: 0x0030A384
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

	// Token: 0x0600803B RID: 32827 RVA: 0x0030C208 File Offset: 0x0030A408
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

	// Token: 0x02000FEF RID: 4079
	[Preserve]
	public class PrefabMarkerEntry : XUiListEntry<XUiC_PrefabMarkerList.PrefabMarkerEntry>
	{
		// Token: 0x17000F17 RID: 3863
		// (get) Token: 0x0600803D RID: 32829 RVA: 0x0030C256 File Offset: 0x0030A456
		public Marker Marker
		{
			get
			{
				return this.Box.UserData as Marker;
			}
		}

		// Token: 0x0600803E RID: 32830 RVA: 0x0030C268 File Offset: 0x0030A468
		public PrefabMarkerEntry(XUiC_PrefabMarkerList _parentList, SelectionBox _box)
		{
			this.parentList = _parentList;
			this.Box = _box;
		}

		// Token: 0x0600803F RID: 32831 RVA: 0x0030C280 File Offset: 0x0030A480
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

		// Token: 0x06008040 RID: 32832 RVA: 0x0030C2D0 File Offset: 0x0030A4D0
		public override bool MatchesSearch(string _searchString)
		{
			return this.Marker.GroupName.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x04005F23 RID: 24355
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiC_PrefabMarkerList parentList;

		// Token: 0x04005F24 RID: 24356
		public readonly SelectionBox Box;

		// Token: 0x04005F25 RID: 24357
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly CachedStringFormatterXuiRgbaColor colorFormatter = new CachedStringFormatterXuiRgbaColor();
	}

	// Token: 0x02000FF0 RID: 4080
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_PrefabMarkerList.PrefabMarkerEntry>.XUiC_ListEntry
	{
		// Token: 0x06008042 RID: 32834 RVA: 0x0030C2F6 File Offset: 0x0030A4F6
		[XuiXmlBinding("groupname")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingGroupName()
		{
			XUiC_PrefabMarkerList.PrefabMarkerEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.Marker.GroupName : null) ?? "";
		}

		// Token: 0x06008043 RID: 32835 RVA: 0x0030C318 File Offset: 0x0030A518
		[XuiXmlBinding("markertype")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingMarkerType()
		{
			XUiC_PrefabMarkerList.PrefabMarkerEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.Marker.MarkerType.ToString() : null) ?? "";
		}

		// Token: 0x06008044 RID: 32836 RVA: 0x0030C354 File Offset: 0x0030A554
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

		// Token: 0x06008045 RID: 32837 RVA: 0x0030C398 File Offset: 0x0030A598
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

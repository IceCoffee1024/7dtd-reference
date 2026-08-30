using System;
using PrefabVolumes;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FB6 RID: 4022
[Preserve]
public class XUiC_PrefabMarkerList : XUiC_List<XUiC_PrefabMarkerList.PrefabMarkerEntry>
{
	// Token: 0x06007EE9 RID: 32489 RVA: 0x00302EAC File Offset: 0x003010AC
	public override void OnOpen()
	{
		base.OnOpen();
		this.RebuildList(false);
	}

	// Token: 0x06007EEA RID: 32490 RVA: 0x00302EBC File Offset: 0x003010BC
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

	// Token: 0x06007EEB RID: 32491 RVA: 0x00302F40 File Offset: 0x00301140
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

	// Token: 0x02000FB7 RID: 4023
	[Preserve]
	public class PrefabMarkerEntry : XUiListEntry<XUiC_PrefabMarkerList.PrefabMarkerEntry>
	{
		// Token: 0x17000EAC RID: 3756
		// (get) Token: 0x06007EED RID: 32493 RVA: 0x00302F8E File Offset: 0x0030118E
		public Marker Marker
		{
			get
			{
				return this.Box.UserData as Marker;
			}
		}

		// Token: 0x06007EEE RID: 32494 RVA: 0x00302FA0 File Offset: 0x003011A0
		public PrefabMarkerEntry(XUiC_PrefabMarkerList _parentList, SelectionBox _box)
		{
			this.parentList = _parentList;
			this.Box = _box;
		}

		// Token: 0x06007EEF RID: 32495 RVA: 0x00302FB8 File Offset: 0x003011B8
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

		// Token: 0x06007EF0 RID: 32496 RVA: 0x00303008 File Offset: 0x00301208
		public override bool MatchesSearch(string _searchString)
		{
			return this.Marker.GroupName.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x04005DF8 RID: 24056
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiC_PrefabMarkerList parentList;

		// Token: 0x04005DF9 RID: 24057
		public readonly SelectionBox Box;

		// Token: 0x04005DFA RID: 24058
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly CachedStringFormatterXuiRgbaColor colorFormatter = new CachedStringFormatterXuiRgbaColor();
	}

	// Token: 0x02000FB8 RID: 4024
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_PrefabMarkerList.PrefabMarkerEntry>.XUiC_ListEntry
	{
		// Token: 0x06007EF2 RID: 32498 RVA: 0x0030302E File Offset: 0x0030122E
		[XuiXmlBinding("groupname")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingGroupName()
		{
			XUiC_PrefabMarkerList.PrefabMarkerEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.Marker.GroupName : null) ?? "";
		}

		// Token: 0x06007EF3 RID: 32499 RVA: 0x00303050 File Offset: 0x00301250
		[XuiXmlBinding("markertype")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingMarkerType()
		{
			XUiC_PrefabMarkerList.PrefabMarkerEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.Marker.MarkerType.ToString() : null) ?? "";
		}

		// Token: 0x06007EF4 RID: 32500 RVA: 0x0030308C File Offset: 0x0030128C
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

		// Token: 0x06007EF5 RID: 32501 RVA: 0x003030D0 File Offset: 0x003012D0
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

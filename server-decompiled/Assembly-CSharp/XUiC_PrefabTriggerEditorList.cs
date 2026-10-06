using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000FE6 RID: 4070
[Preserve]
public class XUiC_PrefabTriggerEditorList : XUiC_List<XUiC_PrefabTriggerEditorList.PrefabTriggerEntry>
{
	// Token: 0x06008018 RID: 32792 RVA: 0x0030B095 File Offset: 0x00309295
	public override void OnOpen()
	{
		base.OnOpen();
		this.RebuildList(false);
	}

	// Token: 0x06008019 RID: 32793 RVA: 0x0030B0A4 File Offset: 0x003092A4
	public override void RebuildList(bool _resetFilter = false)
	{
		this.allEntries.Clear();
		Func<List<byte>> getCurrentPrefabTriggerLayers = this.GetCurrentPrefabTriggerLayers;
		List<byte> list = ((getCurrentPrefabTriggerLayers != null) ? getCurrentPrefabTriggerLayers() : null);
		if (list != null)
		{
			foreach (byte b in list)
			{
				this.allEntries.Add(new XUiC_PrefabTriggerEditorList.PrefabTriggerEntry(this, b));
			}
		}
		this.allEntries.Sort();
		base.RebuildList(_resetFilter);
	}

	// Token: 0x04005EF5 RID: 24309
	public Func<List<byte>> GetCurrentPrefabTriggerLayers;

	// Token: 0x04005EF6 RID: 24310
	public Func<List<byte>> GetCurrentTriggerIndicesList;

	// Token: 0x02000FE7 RID: 4071
	[Preserve]
	public class PrefabTriggerEntry : XUiListEntry<XUiC_PrefabTriggerEditorList.PrefabTriggerEntry>
	{
		// Token: 0x0600801B RID: 32795 RVA: 0x0030B138 File Offset: 0x00309338
		public PrefabTriggerEntry(XUiC_PrefabTriggerEditorList _parentList, byte _triggerLayer)
		{
			this.parentList = _parentList;
			this.TriggerLayer = _triggerLayer;
			this.name = _triggerLayer.ToString();
		}

		// Token: 0x0600801C RID: 32796 RVA: 0x0030B15C File Offset: 0x0030935C
		public override int CompareTo(XUiC_PrefabTriggerEditorList.PrefabTriggerEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return 1;
			}
			return this.TriggerLayer.CompareTo(_otherEntry.TriggerLayer);
		}

		// Token: 0x0600801D RID: 32797 RVA: 0x0030B184 File Offset: 0x00309384
		public bool isSelected()
		{
			Func<List<byte>> getCurrentTriggerIndicesList = this.parentList.GetCurrentTriggerIndicesList;
			List<byte> list = ((getCurrentTriggerIndicesList != null) ? getCurrentTriggerIndicesList() : null);
			return list != null && list.Contains(this.TriggerLayer);
		}

		// Token: 0x0600801E RID: 32798 RVA: 0x0030B1BA File Offset: 0x003093BA
		public override bool MatchesSearch(string _searchString)
		{
			return this.name.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x04005EF7 RID: 24311
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiC_PrefabTriggerEditorList parentList;

		// Token: 0x04005EF8 RID: 24312
		public readonly string name;

		// Token: 0x04005EF9 RID: 24313
		public readonly byte TriggerLayer;
	}

	// Token: 0x02000FE8 RID: 4072
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_PrefabTriggerEditorList.PrefabTriggerEntry>.XUiC_ListEntry
	{
		// Token: 0x0600801F RID: 32799 RVA: 0x0030B1CF File Offset: 0x003093CF
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_PrefabTriggerEditorList.PrefabTriggerEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.name : null) ?? "";
		}

		// Token: 0x06008020 RID: 32800 RVA: 0x0030B1EC File Offset: 0x003093EC
		[XuiXmlBinding("selected")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingSelected()
		{
			XUiC_PrefabTriggerEditorList.PrefabTriggerEntry entryData = this.entryData;
			return entryData != null && entryData.isSelected();
		}
	}
}

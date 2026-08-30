using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000FD7 RID: 4055
[Preserve]
public class XUiC_PrefabTriggerEditorList : XUiC_List<XUiC_PrefabTriggerEditorList.PrefabTriggerEntry>
{
	// Token: 0x06008009 RID: 32777 RVA: 0x003080BD File Offset: 0x003062BD
	public override void OnOpen()
	{
		base.OnOpen();
		this.RebuildList(false);
	}

	// Token: 0x0600800A RID: 32778 RVA: 0x003080CC File Offset: 0x003062CC
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

	// Token: 0x04005ECC RID: 24268
	public Func<List<byte>> GetCurrentPrefabTriggerLayers;

	// Token: 0x04005ECD RID: 24269
	public Func<List<byte>> GetCurrentTriggerIndicesList;

	// Token: 0x02000FD8 RID: 4056
	[Preserve]
	public class PrefabTriggerEntry : XUiListEntry<XUiC_PrefabTriggerEditorList.PrefabTriggerEntry>
	{
		// Token: 0x0600800C RID: 32780 RVA: 0x00308160 File Offset: 0x00306360
		public PrefabTriggerEntry(XUiC_PrefabTriggerEditorList _parentList, byte _triggerLayer)
		{
			this.parentList = _parentList;
			this.TriggerLayer = _triggerLayer;
			this.name = _triggerLayer.ToString();
		}

		// Token: 0x0600800D RID: 32781 RVA: 0x00308184 File Offset: 0x00306384
		public override int CompareTo(XUiC_PrefabTriggerEditorList.PrefabTriggerEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return 1;
			}
			return this.TriggerLayer.CompareTo(_otherEntry.TriggerLayer);
		}

		// Token: 0x0600800E RID: 32782 RVA: 0x003081AC File Offset: 0x003063AC
		public bool isSelected()
		{
			Func<List<byte>> getCurrentTriggerIndicesList = this.parentList.GetCurrentTriggerIndicesList;
			List<byte> list = ((getCurrentTriggerIndicesList != null) ? getCurrentTriggerIndicesList() : null);
			return list != null && list.Contains(this.TriggerLayer);
		}

		// Token: 0x0600800F RID: 32783 RVA: 0x003081E2 File Offset: 0x003063E2
		public override bool MatchesSearch(string _searchString)
		{
			return this.name.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x04005ECE RID: 24270
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiC_PrefabTriggerEditorList parentList;

		// Token: 0x04005ECF RID: 24271
		public readonly string name;

		// Token: 0x04005ED0 RID: 24272
		public readonly byte TriggerLayer;
	}

	// Token: 0x02000FD9 RID: 4057
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_PrefabTriggerEditorList.PrefabTriggerEntry>.XUiC_ListEntry
	{
		// Token: 0x06008010 RID: 32784 RVA: 0x003081F7 File Offset: 0x003063F7
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_PrefabTriggerEditorList.PrefabTriggerEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.name : null) ?? "";
		}

		// Token: 0x06008011 RID: 32785 RVA: 0x00308214 File Offset: 0x00306414
		[XuiXmlBinding("selected")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingSelected()
		{
			XUiC_PrefabTriggerEditorList.PrefabTriggerEntry entryData = this.entryData;
			return entryData != null && entryData.isSelected();
		}
	}
}

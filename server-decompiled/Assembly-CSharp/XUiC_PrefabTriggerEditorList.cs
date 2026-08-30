using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000FBE RID: 4030
[Preserve]
public class XUiC_PrefabTriggerEditorList : XUiC_List<XUiC_PrefabTriggerEditorList.PrefabTriggerEntry>
{
	// Token: 0x06007F17 RID: 32535 RVA: 0x003038E9 File Offset: 0x00301AE9
	public override void OnOpen()
	{
		base.OnOpen();
		this.RebuildList(false);
	}

	// Token: 0x06007F18 RID: 32536 RVA: 0x003038F8 File Offset: 0x00301AF8
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

	// Token: 0x04005E05 RID: 24069
	public Func<List<byte>> GetCurrentPrefabTriggerLayers;

	// Token: 0x04005E06 RID: 24070
	public Func<List<byte>> GetCurrentTriggerIndicesList;

	// Token: 0x02000FBF RID: 4031
	[Preserve]
	public class PrefabTriggerEntry : XUiListEntry<XUiC_PrefabTriggerEditorList.PrefabTriggerEntry>
	{
		// Token: 0x06007F1A RID: 32538 RVA: 0x0030398C File Offset: 0x00301B8C
		public PrefabTriggerEntry(XUiC_PrefabTriggerEditorList _parentList, byte _triggerLayer)
		{
			this.parentList = _parentList;
			this.TriggerLayer = _triggerLayer;
			this.name = _triggerLayer.ToString();
		}

		// Token: 0x06007F1B RID: 32539 RVA: 0x003039B0 File Offset: 0x00301BB0
		public override int CompareTo(XUiC_PrefabTriggerEditorList.PrefabTriggerEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return 1;
			}
			return this.TriggerLayer.CompareTo(_otherEntry.TriggerLayer);
		}

		// Token: 0x06007F1C RID: 32540 RVA: 0x003039D8 File Offset: 0x00301BD8
		public bool isSelected()
		{
			Func<List<byte>> getCurrentTriggerIndicesList = this.parentList.GetCurrentTriggerIndicesList;
			List<byte> list = ((getCurrentTriggerIndicesList != null) ? getCurrentTriggerIndicesList() : null);
			return list != null && list.Contains(this.TriggerLayer);
		}

		// Token: 0x06007F1D RID: 32541 RVA: 0x00303A0E File Offset: 0x00301C0E
		public override bool MatchesSearch(string _searchString)
		{
			return this.name.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x04005E07 RID: 24071
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiC_PrefabTriggerEditorList parentList;

		// Token: 0x04005E08 RID: 24072
		public readonly string name;

		// Token: 0x04005E09 RID: 24073
		public readonly byte TriggerLayer;
	}

	// Token: 0x02000FC0 RID: 4032
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_PrefabTriggerEditorList.PrefabTriggerEntry>.XUiC_ListEntry
	{
		// Token: 0x06007F1E RID: 32542 RVA: 0x00303A23 File Offset: 0x00301C23
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_PrefabTriggerEditorList.PrefabTriggerEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.name : null) ?? "";
		}

		// Token: 0x06007F1F RID: 32543 RVA: 0x00303A40 File Offset: 0x00301C40
		[XuiXmlBinding("selected")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingSelected()
		{
			XUiC_PrefabTriggerEditorList.PrefabTriggerEntry entryData = this.entryData;
			return entryData != null && entryData.isSelected();
		}
	}
}

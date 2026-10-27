using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000FF6 RID: 4086
[Preserve]
public class XUiC_PrefabTriggerEditorList : XUiC_List<XUiC_PrefabTriggerEditorList.PrefabTriggerEntry>
{
	// Token: 0x06008067 RID: 32871 RVA: 0x0030CBB1 File Offset: 0x0030ADB1
	public override void OnOpen()
	{
		base.OnOpen();
		this.RebuildList(false);
	}

	// Token: 0x06008068 RID: 32872 RVA: 0x0030CBC0 File Offset: 0x0030ADC0
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

	// Token: 0x04005F30 RID: 24368
	public Func<List<byte>> GetCurrentPrefabTriggerLayers;

	// Token: 0x04005F31 RID: 24369
	public Func<List<byte>> GetCurrentTriggerIndicesList;

	// Token: 0x02000FF7 RID: 4087
	[Preserve]
	public class PrefabTriggerEntry : XUiListEntry<XUiC_PrefabTriggerEditorList.PrefabTriggerEntry>
	{
		// Token: 0x0600806A RID: 32874 RVA: 0x0030CC54 File Offset: 0x0030AE54
		public PrefabTriggerEntry(XUiC_PrefabTriggerEditorList _parentList, byte _triggerLayer)
		{
			this.parentList = _parentList;
			this.TriggerLayer = _triggerLayer;
			this.name = _triggerLayer.ToString();
		}

		// Token: 0x0600806B RID: 32875 RVA: 0x0030CC78 File Offset: 0x0030AE78
		public override int CompareTo(XUiC_PrefabTriggerEditorList.PrefabTriggerEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return 1;
			}
			return this.TriggerLayer.CompareTo(_otherEntry.TriggerLayer);
		}

		// Token: 0x0600806C RID: 32876 RVA: 0x0030CCA0 File Offset: 0x0030AEA0
		public bool isSelected()
		{
			Func<List<byte>> getCurrentTriggerIndicesList = this.parentList.GetCurrentTriggerIndicesList;
			List<byte> list = ((getCurrentTriggerIndicesList != null) ? getCurrentTriggerIndicesList() : null);
			return list != null && list.Contains(this.TriggerLayer);
		}

		// Token: 0x0600806D RID: 32877 RVA: 0x0030CCD6 File Offset: 0x0030AED6
		public override bool MatchesSearch(string _searchString)
		{
			return this.name.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x04005F32 RID: 24370
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiC_PrefabTriggerEditorList parentList;

		// Token: 0x04005F33 RID: 24371
		public readonly string name;

		// Token: 0x04005F34 RID: 24372
		public readonly byte TriggerLayer;
	}

	// Token: 0x02000FF8 RID: 4088
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_PrefabTriggerEditorList.PrefabTriggerEntry>.XUiC_ListEntry
	{
		// Token: 0x0600806E RID: 32878 RVA: 0x0030CCEB File Offset: 0x0030AEEB
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_PrefabTriggerEditorList.PrefabTriggerEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.name : null) ?? "";
		}

		// Token: 0x0600806F RID: 32879 RVA: 0x0030CD08 File Offset: 0x0030AF08
		[XuiXmlBinding("selected")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingSelected()
		{
			XUiC_PrefabTriggerEditorList.PrefabTriggerEntry entryData = this.entryData;
			return entryData != null && entryData.isSelected();
		}
	}
}

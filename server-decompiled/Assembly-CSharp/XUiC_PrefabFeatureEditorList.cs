using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000FC0 RID: 4032
[Preserve]
public abstract class XUiC_PrefabFeatureEditorList : XUiC_List<XUiC_PrefabFeatureEditorList.FeatureEntry>
{
	// Token: 0x140000D8 RID: 216
	// (add) Token: 0x06007F81 RID: 32641 RVA: 0x00305F1C File Offset: 0x0030411C
	// (remove) Token: 0x06007F82 RID: 32642 RVA: 0x00305F54 File Offset: 0x00304154
	public event XUiC_PrefabFeatureEditorList.FeatureChangedDelegate FeatureChanged;

	// Token: 0x06007F83 RID: 32643
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract bool FeatureEnabled(string _featureName);

	// Token: 0x06007F84 RID: 32644 RVA: 0x00305F8C File Offset: 0x0030418C
	public override void Init()
	{
		base.Init();
		base.SelectionChanged += this.FeatureListSelectionChanged;
		this.addInput = base.GetChildById("addInput") as XUiC_TextInput;
		if (this.addInput != null)
		{
			this.addInput.OnSubmitHandler += this.OnAddInputSubmit;
		}
		XUiController childById = base.GetChildById("addButton");
		if (childById != null)
		{
			childById.OnPress += this.HandleAddEntry;
		}
	}

	// Token: 0x06007F85 RID: 32645 RVA: 0x00306007 File Offset: 0x00304207
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleAddEntry(XUiController _sender, int _mouseButton)
	{
		this.OnAddFeaturePressed(this.addInput.Text);
	}

	// Token: 0x06007F86 RID: 32646 RVA: 0x00306007 File Offset: 0x00304207
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnAddInputSubmit(XUiController _sender, string _text)
	{
		this.OnAddFeaturePressed(this.addInput.Text);
	}

	// Token: 0x06007F87 RID: 32647
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void AddNewFeature(string _featureName);

	// Token: 0x06007F88 RID: 32648 RVA: 0x0030601C File Offset: 0x0030421C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnAddFeaturePressed(string _s)
	{
		_s = _s.Trim();
		if (!this.validGroupName(_s))
		{
			return;
		}
		this.AddNewFeature(_s);
		this.RebuildList(false);
		this.addInput.Text = string.Empty;
		XUiC_PrefabFeatureEditorList.FeatureChangedDelegate featureChanged = this.FeatureChanged;
		if (featureChanged == null)
		{
			return;
		}
		featureChanged(this, _s, true);
	}

	// Token: 0x06007F89 RID: 32649 RVA: 0x0030606C File Offset: 0x0030426C
	[PublicizedFrom(EAccessModifier.Private)]
	public bool validGroupName(string _s)
	{
		_s = _s.Trim();
		return _s.Length > 0 && _s.IndexOf(",", StringComparison.OrdinalIgnoreCase) < 0 && !this.groupsResult.ContainsCaseInsensitive(_s);
	}

	// Token: 0x06007F8A RID: 32650
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void ToggleFeature(string _featureName);

	// Token: 0x06007F8B RID: 32651 RVA: 0x003060A0 File Offset: 0x003042A0
	[PublicizedFrom(EAccessModifier.Private)]
	public void FeatureListSelectionChanged(XUiC_List<XUiC_PrefabFeatureEditorList.FeatureEntry> _list, XUiC_PrefabFeatureEditorList.FeatureEntry _previousEntry, XUiC_PrefabFeatureEditorList.FeatureEntry _newEntry)
	{
		if (_newEntry == null)
		{
			return;
		}
		string name = _newEntry.Name;
		this.ToggleFeature(name);
		_newEntry.UiDirty = true;
		XUiC_PrefabFeatureEditorList.FeatureChangedDelegate featureChanged = this.FeatureChanged;
		if (featureChanged == null)
		{
			return;
		}
		featureChanged(this, name, this.FeatureEnabled(name));
	}

	// Token: 0x06007F8C RID: 32652 RVA: 0x003060DF File Offset: 0x003042DF
	public override void OnOpen()
	{
		base.OnOpen();
		this.RebuildList(false);
	}

	// Token: 0x06007F8D RID: 32653
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void GetSupportedFeatures();

	// Token: 0x06007F8E RID: 32654 RVA: 0x003060F0 File Offset: 0x003042F0
	public override void RebuildList(bool _resetFilter = false)
	{
		this.allEntries.Clear();
		this.groupsResult.Clear();
		this.GetSupportedFeatures();
		foreach (string text in this.groupsResult)
		{
			this.allEntries.Add(new XUiC_PrefabFeatureEditorList.FeatureEntry(this, text));
		}
		this.allEntries.Sort();
		base.RebuildList(_resetFilter);
	}

	// Token: 0x06007F8F RID: 32655 RVA: 0x0030617C File Offset: 0x0030437C
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_PrefabFeatureEditorList()
	{
	}

	// Token: 0x04005E9F RID: 24223
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput addInput;

	// Token: 0x04005EA0 RID: 24224
	public Prefab EditPrefab;

	// Token: 0x04005EA1 RID: 24225
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly List<string> groupsResult = new List<string>();

	// Token: 0x02000FC1 RID: 4033
	// (Invoke) Token: 0x06007F91 RID: 32657
	public delegate void FeatureChangedDelegate(XUiC_PrefabFeatureEditorList _list, string _featureName, bool _selected);

	// Token: 0x02000FC2 RID: 4034
	[Preserve]
	public class FeatureEntry : XUiListEntry<XUiC_PrefabFeatureEditorList.FeatureEntry>
	{
		// Token: 0x06007F94 RID: 32660 RVA: 0x0030618F File Offset: 0x0030438F
		public FeatureEntry(XUiC_PrefabFeatureEditorList _parentList, string _name)
		{
			this.parentList = _parentList;
			this.Name = _name;
		}

		// Token: 0x06007F95 RID: 32661 RVA: 0x003061A5 File Offset: 0x003043A5
		public override int CompareTo(XUiC_PrefabFeatureEditorList.FeatureEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return 1;
			}
			return string.Compare(this.Name, _otherEntry.Name, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06007F96 RID: 32662 RVA: 0x003061BE File Offset: 0x003043BE
		public override bool MatchesSearch(string _searchString)
		{
			return this.Name.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x04005EA2 RID: 24226
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiC_PrefabFeatureEditorList parentList;

		// Token: 0x04005EA3 RID: 24227
		public readonly string Name;
	}

	// Token: 0x02000FC3 RID: 4035
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_PrefabFeatureEditorList.FeatureEntry>.XUiC_ListEntry
	{
		// Token: 0x06007F97 RID: 32663 RVA: 0x003061D3 File Offset: 0x003043D3
		public override void Init()
		{
			base.Init();
			this.parentList = (XUiC_PrefabFeatureEditorList)this.list;
		}

		// Token: 0x06007F98 RID: 32664 RVA: 0x003061EC File Offset: 0x003043EC
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_PrefabFeatureEditorList.FeatureEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.Name : null) ?? "";
		}

		// Token: 0x06007F99 RID: 32665 RVA: 0x00306209 File Offset: 0x00304409
		[XuiXmlBinding("selected")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingSelected()
		{
			return this.entryData != null && this.parentList.EditPrefab != null && this.parentList.FeatureEnabled(this.entryData.Name);
		}

		// Token: 0x04005EA4 RID: 24228
		[PublicizedFrom(EAccessModifier.Private)]
		public XUiC_PrefabFeatureEditorList parentList;
	}
}

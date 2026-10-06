using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000FCF RID: 4047
[Preserve]
public abstract class XUiC_PrefabFeatureEditorList : XUiC_List<XUiC_PrefabFeatureEditorList.FeatureEntry>
{
	// Token: 0x140000D7 RID: 215
	// (add) Token: 0x06007F90 RID: 32656 RVA: 0x00308EF4 File Offset: 0x003070F4
	// (remove) Token: 0x06007F91 RID: 32657 RVA: 0x00308F2C File Offset: 0x0030712C
	public event XUiC_PrefabFeatureEditorList.FeatureChangedDelegate FeatureChanged;

	// Token: 0x06007F92 RID: 32658
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract bool FeatureEnabled(string _featureName);

	// Token: 0x06007F93 RID: 32659 RVA: 0x00308F64 File Offset: 0x00307164
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

	// Token: 0x06007F94 RID: 32660 RVA: 0x00308FDF File Offset: 0x003071DF
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleAddEntry(XUiController _sender, int _mouseButton)
	{
		this.OnAddFeaturePressed(this.addInput.Text);
	}

	// Token: 0x06007F95 RID: 32661 RVA: 0x00308FDF File Offset: 0x003071DF
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnAddInputSubmit(XUiController _sender, string _text)
	{
		this.OnAddFeaturePressed(this.addInput.Text);
	}

	// Token: 0x06007F96 RID: 32662
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void AddNewFeature(string _featureName);

	// Token: 0x06007F97 RID: 32663 RVA: 0x00308FF4 File Offset: 0x003071F4
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

	// Token: 0x06007F98 RID: 32664 RVA: 0x00309044 File Offset: 0x00307244
	[PublicizedFrom(EAccessModifier.Private)]
	public bool validGroupName(string _s)
	{
		_s = _s.Trim();
		return _s.Length > 0 && _s.IndexOf(",", StringComparison.OrdinalIgnoreCase) < 0 && !this.groupsResult.ContainsCaseInsensitive(_s);
	}

	// Token: 0x06007F99 RID: 32665
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void ToggleFeature(string _featureName);

	// Token: 0x06007F9A RID: 32666 RVA: 0x00309078 File Offset: 0x00307278
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

	// Token: 0x06007F9B RID: 32667 RVA: 0x003090B7 File Offset: 0x003072B7
	public override void OnOpen()
	{
		base.OnOpen();
		this.RebuildList(false);
	}

	// Token: 0x06007F9C RID: 32668
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void GetSupportedFeatures();

	// Token: 0x06007F9D RID: 32669 RVA: 0x003090C8 File Offset: 0x003072C8
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

	// Token: 0x06007F9E RID: 32670 RVA: 0x00309154 File Offset: 0x00307354
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_PrefabFeatureEditorList()
	{
	}

	// Token: 0x04005EC8 RID: 24264
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput addInput;

	// Token: 0x04005EC9 RID: 24265
	public Prefab EditPrefab;

	// Token: 0x04005ECA RID: 24266
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly List<string> groupsResult = new List<string>();

	// Token: 0x02000FD0 RID: 4048
	// (Invoke) Token: 0x06007FA0 RID: 32672
	public delegate void FeatureChangedDelegate(XUiC_PrefabFeatureEditorList _list, string _featureName, bool _selected);

	// Token: 0x02000FD1 RID: 4049
	[Preserve]
	public class FeatureEntry : XUiListEntry<XUiC_PrefabFeatureEditorList.FeatureEntry>
	{
		// Token: 0x06007FA3 RID: 32675 RVA: 0x00309167 File Offset: 0x00307367
		public FeatureEntry(XUiC_PrefabFeatureEditorList _parentList, string _name)
		{
			this.parentList = _parentList;
			this.Name = _name;
		}

		// Token: 0x06007FA4 RID: 32676 RVA: 0x0030917D File Offset: 0x0030737D
		public override int CompareTo(XUiC_PrefabFeatureEditorList.FeatureEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return 1;
			}
			return string.Compare(this.Name, _otherEntry.Name, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06007FA5 RID: 32677 RVA: 0x00309196 File Offset: 0x00307396
		public override bool MatchesSearch(string _searchString)
		{
			return this.Name.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x04005ECB RID: 24267
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiC_PrefabFeatureEditorList parentList;

		// Token: 0x04005ECC RID: 24268
		public readonly string Name;
	}

	// Token: 0x02000FD2 RID: 4050
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_PrefabFeatureEditorList.FeatureEntry>.XUiC_ListEntry
	{
		// Token: 0x06007FA6 RID: 32678 RVA: 0x003091AB File Offset: 0x003073AB
		public override void Init()
		{
			base.Init();
			this.parentList = (XUiC_PrefabFeatureEditorList)this.list;
		}

		// Token: 0x06007FA7 RID: 32679 RVA: 0x003091C4 File Offset: 0x003073C4
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_PrefabFeatureEditorList.FeatureEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.Name : null) ?? "";
		}

		// Token: 0x06007FA8 RID: 32680 RVA: 0x003091E1 File Offset: 0x003073E1
		[XuiXmlBinding("selected")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingSelected()
		{
			return this.entryData != null && this.parentList.EditPrefab != null && this.parentList.FeatureEnabled(this.entryData.Name);
		}

		// Token: 0x04005ECD RID: 24269
		[PublicizedFrom(EAccessModifier.Private)]
		public XUiC_PrefabFeatureEditorList parentList;
	}
}

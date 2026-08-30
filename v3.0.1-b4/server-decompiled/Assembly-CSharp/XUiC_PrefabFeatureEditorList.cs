using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000FA7 RID: 4007
[Preserve]
public abstract class XUiC_PrefabFeatureEditorList : XUiC_List<XUiC_PrefabFeatureEditorList.FeatureEntry>
{
	// Token: 0x140000D7 RID: 215
	// (add) Token: 0x06007E8F RID: 32399 RVA: 0x00301748 File Offset: 0x002FF948
	// (remove) Token: 0x06007E90 RID: 32400 RVA: 0x00301780 File Offset: 0x002FF980
	public event XUiC_PrefabFeatureEditorList.FeatureChangedDelegate FeatureChanged;

	// Token: 0x06007E91 RID: 32401
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract bool FeatureEnabled(string _featureName);

	// Token: 0x06007E92 RID: 32402 RVA: 0x003017B8 File Offset: 0x002FF9B8
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

	// Token: 0x06007E93 RID: 32403 RVA: 0x00301833 File Offset: 0x002FFA33
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleAddEntry(XUiController _sender, int _mouseButton)
	{
		this.OnAddFeaturePressed(this.addInput.Text);
	}

	// Token: 0x06007E94 RID: 32404 RVA: 0x00301833 File Offset: 0x002FFA33
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnAddInputSubmit(XUiController _sender, string _text)
	{
		this.OnAddFeaturePressed(this.addInput.Text);
	}

	// Token: 0x06007E95 RID: 32405
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void AddNewFeature(string _featureName);

	// Token: 0x06007E96 RID: 32406 RVA: 0x00301848 File Offset: 0x002FFA48
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

	// Token: 0x06007E97 RID: 32407 RVA: 0x00301898 File Offset: 0x002FFA98
	[PublicizedFrom(EAccessModifier.Private)]
	public bool validGroupName(string _s)
	{
		_s = _s.Trim();
		return _s.Length > 0 && _s.IndexOf(",", StringComparison.OrdinalIgnoreCase) < 0 && !this.groupsResult.ContainsCaseInsensitive(_s);
	}

	// Token: 0x06007E98 RID: 32408
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void ToggleFeature(string _featureName);

	// Token: 0x06007E99 RID: 32409 RVA: 0x003018CC File Offset: 0x002FFACC
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

	// Token: 0x06007E9A RID: 32410 RVA: 0x0030190B File Offset: 0x002FFB0B
	public override void OnOpen()
	{
		base.OnOpen();
		this.RebuildList(false);
	}

	// Token: 0x06007E9B RID: 32411
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void GetSupportedFeatures();

	// Token: 0x06007E9C RID: 32412 RVA: 0x0030191C File Offset: 0x002FFB1C
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

	// Token: 0x06007E9D RID: 32413 RVA: 0x003019A8 File Offset: 0x002FFBA8
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_PrefabFeatureEditorList()
	{
	}

	// Token: 0x04005DD8 RID: 24024
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput addInput;

	// Token: 0x04005DD9 RID: 24025
	public Prefab EditPrefab;

	// Token: 0x04005DDA RID: 24026
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly List<string> groupsResult = new List<string>();

	// Token: 0x02000FA8 RID: 4008
	// (Invoke) Token: 0x06007E9F RID: 32415
	public delegate void FeatureChangedDelegate(XUiC_PrefabFeatureEditorList _list, string _featureName, bool _selected);

	// Token: 0x02000FA9 RID: 4009
	[Preserve]
	public class FeatureEntry : XUiListEntry<XUiC_PrefabFeatureEditorList.FeatureEntry>
	{
		// Token: 0x06007EA2 RID: 32418 RVA: 0x003019BB File Offset: 0x002FFBBB
		public FeatureEntry(XUiC_PrefabFeatureEditorList _parentList, string _name)
		{
			this.parentList = _parentList;
			this.Name = _name;
		}

		// Token: 0x06007EA3 RID: 32419 RVA: 0x003019D1 File Offset: 0x002FFBD1
		public override int CompareTo(XUiC_PrefabFeatureEditorList.FeatureEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return 1;
			}
			return string.Compare(this.Name, _otherEntry.Name, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06007EA4 RID: 32420 RVA: 0x003019EA File Offset: 0x002FFBEA
		public override bool MatchesSearch(string _searchString)
		{
			return this.Name.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x04005DDB RID: 24027
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiC_PrefabFeatureEditorList parentList;

		// Token: 0x04005DDC RID: 24028
		public readonly string Name;
	}

	// Token: 0x02000FAA RID: 4010
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_PrefabFeatureEditorList.FeatureEntry>.XUiC_ListEntry
	{
		// Token: 0x06007EA5 RID: 32421 RVA: 0x003019FF File Offset: 0x002FFBFF
		public override void Init()
		{
			base.Init();
			this.parentList = (XUiC_PrefabFeatureEditorList)this.list;
		}

		// Token: 0x06007EA6 RID: 32422 RVA: 0x00301A18 File Offset: 0x002FFC18
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_PrefabFeatureEditorList.FeatureEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.Name : null) ?? "";
		}

		// Token: 0x06007EA7 RID: 32423 RVA: 0x00301A35 File Offset: 0x002FFC35
		[XuiXmlBinding("selected")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingSelected()
		{
			return this.entryData != null && this.parentList.EditPrefab != null && this.parentList.FeatureEnabled(this.entryData.Name);
		}

		// Token: 0x04005DDD RID: 24029
		[PublicizedFrom(EAccessModifier.Private)]
		public XUiC_PrefabFeatureEditorList parentList;
	}
}

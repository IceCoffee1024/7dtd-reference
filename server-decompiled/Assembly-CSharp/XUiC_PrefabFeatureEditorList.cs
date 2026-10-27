using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000FDF RID: 4063
[Preserve]
public abstract class XUiC_PrefabFeatureEditorList : XUiC_List<XUiC_PrefabFeatureEditorList.FeatureEntry>
{
	// Token: 0x140000D7 RID: 215
	// (add) Token: 0x06007FDF RID: 32735 RVA: 0x0030AA10 File Offset: 0x00308C10
	// (remove) Token: 0x06007FE0 RID: 32736 RVA: 0x0030AA48 File Offset: 0x00308C48
	public event XUiC_PrefabFeatureEditorList.FeatureChangedDelegate FeatureChanged;

	// Token: 0x06007FE1 RID: 32737
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract bool FeatureEnabled(string _featureName);

	// Token: 0x06007FE2 RID: 32738 RVA: 0x0030AA80 File Offset: 0x00308C80
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

	// Token: 0x06007FE3 RID: 32739 RVA: 0x0030AAFB File Offset: 0x00308CFB
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleAddEntry(XUiController _sender, int _mouseButton)
	{
		this.OnAddFeaturePressed(this.addInput.Text);
	}

	// Token: 0x06007FE4 RID: 32740 RVA: 0x0030AAFB File Offset: 0x00308CFB
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnAddInputSubmit(XUiController _sender, string _text)
	{
		this.OnAddFeaturePressed(this.addInput.Text);
	}

	// Token: 0x06007FE5 RID: 32741
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void AddNewFeature(string _featureName);

	// Token: 0x06007FE6 RID: 32742 RVA: 0x0030AB10 File Offset: 0x00308D10
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

	// Token: 0x06007FE7 RID: 32743 RVA: 0x0030AB60 File Offset: 0x00308D60
	[PublicizedFrom(EAccessModifier.Private)]
	public bool validGroupName(string _s)
	{
		_s = _s.Trim();
		return _s.Length > 0 && _s.IndexOf(",", StringComparison.OrdinalIgnoreCase) < 0 && !this.groupsResult.ContainsCaseInsensitive(_s);
	}

	// Token: 0x06007FE8 RID: 32744
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void ToggleFeature(string _featureName);

	// Token: 0x06007FE9 RID: 32745 RVA: 0x0030AB94 File Offset: 0x00308D94
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

	// Token: 0x06007FEA RID: 32746 RVA: 0x0030ABD3 File Offset: 0x00308DD3
	public override void OnOpen()
	{
		base.OnOpen();
		this.RebuildList(false);
	}

	// Token: 0x06007FEB RID: 32747
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void GetSupportedFeatures();

	// Token: 0x06007FEC RID: 32748 RVA: 0x0030ABE4 File Offset: 0x00308DE4
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

	// Token: 0x06007FED RID: 32749 RVA: 0x0030AC70 File Offset: 0x00308E70
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_PrefabFeatureEditorList()
	{
	}

	// Token: 0x04005F03 RID: 24323
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput addInput;

	// Token: 0x04005F04 RID: 24324
	public Prefab EditPrefab;

	// Token: 0x04005F05 RID: 24325
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly List<string> groupsResult = new List<string>();

	// Token: 0x02000FE0 RID: 4064
	// (Invoke) Token: 0x06007FEF RID: 32751
	public delegate void FeatureChangedDelegate(XUiC_PrefabFeatureEditorList _list, string _featureName, bool _selected);

	// Token: 0x02000FE1 RID: 4065
	[Preserve]
	public class FeatureEntry : XUiListEntry<XUiC_PrefabFeatureEditorList.FeatureEntry>
	{
		// Token: 0x06007FF2 RID: 32754 RVA: 0x0030AC83 File Offset: 0x00308E83
		public FeatureEntry(XUiC_PrefabFeatureEditorList _parentList, string _name)
		{
			this.parentList = _parentList;
			this.Name = _name;
		}

		// Token: 0x06007FF3 RID: 32755 RVA: 0x0030AC99 File Offset: 0x00308E99
		public override int CompareTo(XUiC_PrefabFeatureEditorList.FeatureEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return 1;
			}
			return string.Compare(this.Name, _otherEntry.Name, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06007FF4 RID: 32756 RVA: 0x0030ACB2 File Offset: 0x00308EB2
		public override bool MatchesSearch(string _searchString)
		{
			return this.Name.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x04005F06 RID: 24326
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiC_PrefabFeatureEditorList parentList;

		// Token: 0x04005F07 RID: 24327
		public readonly string Name;
	}

	// Token: 0x02000FE2 RID: 4066
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_PrefabFeatureEditorList.FeatureEntry>.XUiC_ListEntry
	{
		// Token: 0x06007FF5 RID: 32757 RVA: 0x0030ACC7 File Offset: 0x00308EC7
		public override void Init()
		{
			base.Init();
			this.parentList = (XUiC_PrefabFeatureEditorList)this.list;
		}

		// Token: 0x06007FF6 RID: 32758 RVA: 0x0030ACE0 File Offset: 0x00308EE0
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_PrefabFeatureEditorList.FeatureEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.Name : null) ?? "";
		}

		// Token: 0x06007FF7 RID: 32759 RVA: 0x0030ACFD File Offset: 0x00308EFD
		[XuiXmlBinding("selected")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingSelected()
		{
			return this.entryData != null && this.parentList.EditPrefab != null && this.parentList.FeatureEnabled(this.entryData.Name);
		}

		// Token: 0x04005F08 RID: 24328
		[PublicizedFrom(EAccessModifier.Private)]
		public XUiC_PrefabFeatureEditorList parentList;
	}
}

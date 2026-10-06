using System;
using SandboxOptions;
using UnityEngine.Scripting;

// Token: 0x02000E4E RID: 3662
[Preserve]
public class XUiC_SandboxPresetSelector : XUiController
{
	// Token: 0x1400009E RID: 158
	// (add) Token: 0x06007014 RID: 28692 RVA: 0x002B63E8 File Offset: 0x002B45E8
	// (remove) Token: 0x06007015 RID: 28693 RVA: 0x002B6420 File Offset: 0x002B4620
	public event SandboxPresetSelectionChangedDelegate SandboxPresetSelectionChanged;

	// Token: 0x17000CBC RID: 3260
	// (get) Token: 0x06007016 RID: 28694 RVA: 0x002B6455 File Offset: 0x002B4655
	[XuiXmlBinding("description")]
	public string Description
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_ComboBoxList<SandboxPresetInfo> xuiC_ComboBoxList = this.cbxPreset;
			return ((xuiC_ComboBoxList != null) ? xuiC_ComboBoxList.Value.Description : null) ?? "";
		}
	}

	// Token: 0x17000CBD RID: 3261
	// (get) Token: 0x06007017 RID: 28695 RVA: 0x002B6477 File Offset: 0x002B4677
	[XuiXmlBinding("icon")]
	public string Icon
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_ComboBoxList<SandboxPresetInfo> xuiC_ComboBoxList = this.cbxPreset;
			return ((xuiC_ComboBoxList != null) ? xuiC_ComboBoxList.Value.Icon : null) ?? "";
		}
	}

	// Token: 0x17000CBE RID: 3262
	// (get) Token: 0x06007018 RID: 28696 RVA: 0x002B6499 File Offset: 0x002B4699
	[XuiXmlBinding("difficulty")]
	public int Difficulty
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_ComboBoxList<SandboxPresetInfo> xuiC_ComboBoxList = this.cbxPreset;
			if (xuiC_ComboBoxList == null)
			{
				return 0;
			}
			return xuiC_ComboBoxList.Value.Difficulty;
		}
	}

	// Token: 0x17000CBF RID: 3263
	// (get) Token: 0x06007019 RID: 28697 RVA: 0x002B64B1 File Offset: 0x002B46B1
	[XuiXmlBinding("userpreset")]
	public bool IsUserPreset
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_ComboBoxList<SandboxPresetInfo> xuiC_ComboBoxList = this.cbxPreset;
			return xuiC_ComboBoxList != null && xuiC_ComboBoxList.Value.IsUserPreset;
		}
	}

	// Token: 0x17000CC0 RID: 3264
	// (get) Token: 0x0600701A RID: 28698 RVA: 0x002B64C9 File Offset: 0x002B46C9
	[XuiXmlBinding("custompreset")]
	public bool IsCustomPreset
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_ComboBoxList<SandboxPresetInfo> xuiC_ComboBoxList = this.cbxPreset;
			return xuiC_ComboBoxList != null && xuiC_ComboBoxList.Value.IsCustomPreset;
		}
	}

	// Token: 0x17000CC1 RID: 3265
	// (get) Token: 0x0600701B RID: 28699 RVA: 0x002B64E1 File Offset: 0x002B46E1
	public SandboxPresetInfo SelectedPreset
	{
		get
		{
			return this.cbxPreset.Value;
		}
	}

	// Token: 0x0600701C RID: 28700 RVA: 0x002B64F0 File Offset: 0x002B46F0
	[PublicizedFrom(EAccessModifier.Private)]
	public void updatePresetGroups(bool _allowCustom = false)
	{
		string text = this.cbxPresetGroups.Value.InternalName ?? "";
		this.cbxPresetGroups.Elements.Clear();
		foreach (string text2 in this.sandboxManager.GetAllPresetGroups())
		{
			if (!(text2 == "Custom") && this.sandboxManager.GetPresetsForGroup(text2))
			{
				SandboxPresetGroupData sandboxPresetGroupData = new SandboxPresetGroupData(text2, Localization.Get(text2, false, null));
				this.cbxPresetGroups.Elements.Add(sandboxPresetGroupData);
			}
		}
		if (_allowCustom)
		{
			this.cbxPresetGroups.Elements.Add(new SandboxPresetGroupData("Custom", Localization.Get("Custom", false, null)));
		}
		this.selectGroupByName(text);
	}

	// Token: 0x0600701D RID: 28701 RVA: 0x002B65D8 File Offset: 0x002B47D8
	[PublicizedFrom(EAccessModifier.Private)]
	public void updatePresetsForSelectedGroup(string _selectPreset = null, bool _forceChangedCallback = false)
	{
		string text = this.cbxPresetGroups.Value.InternalName ?? "";
		SandboxPresetInfo oldSelectedPreset = this.cbxPreset.Value;
		this.cbxPreset.Elements.Clear();
		foreach (SandboxOptionPreset sandboxOptionPreset in this.sandboxManager.SandboxPresets)
		{
			if (!(sandboxOptionPreset.Group != text))
			{
				this.cbxPreset.Elements.Add(new SandboxPresetInfo(sandboxOptionPreset));
			}
		}
		if (_selectPreset != null)
		{
			this.cbxPreset.SelectedIndex = this.cbxPreset.Elements.FindIndex((SandboxPresetInfo _data) => _data.InternalName == _selectPreset);
		}
		else
		{
			this.cbxPreset.SelectedIndex = this.cbxPreset.Elements.FindIndex((SandboxPresetInfo _data) => _data.Equals(oldSelectedPreset));
		}
		if (_forceChangedCallback || !this.cbxPreset.Value.Equals(oldSelectedPreset))
		{
			this.CbxPreset_OnValueChanged(this, oldSelectedPreset, this.cbxPreset.Value);
		}
	}

	// Token: 0x0600701E RID: 28702 RVA: 0x002B6724 File Offset: 0x002B4924
	[XuiBindEvent("OnValueChanged", "cbxPresetGroups")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxPresetGroups_OnValueChanged(XUiController _sender, SandboxPresetGroupData _oldValue, SandboxPresetGroupData _newValue)
	{
		this.updatePresetsForSelectedGroup(null, false);
		this.IsDirty = true;
	}

	// Token: 0x0600701F RID: 28703 RVA: 0x002B6735 File Offset: 0x002B4935
	[XuiBindEvent("OnValueChanged", "cbxPreset")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxPreset_OnValueChanged(XUiController _sender, SandboxPresetInfo _oldValue, SandboxPresetInfo _newValue)
	{
		this.IsDirty = true;
		SandboxPresetSelectionChangedDelegate sandboxPresetSelectionChanged = this.SandboxPresetSelectionChanged;
		if (sandboxPresetSelectionChanged == null)
		{
			return;
		}
		sandboxPresetSelectionChanged(_oldValue, _newValue);
	}

	// Token: 0x06007020 RID: 28704 RVA: 0x002B6750 File Offset: 0x002B4950
	[PublicizedFrom(EAccessModifier.Private)]
	public void selectGroupByName(string _groupName)
	{
		this.cbxPresetGroups.SelectedIndex = this.cbxPresetGroups.Elements.FindIndex((SandboxPresetGroupData _data) => _data.InternalName == _groupName);
	}

	// Token: 0x06007021 RID: 28705 RVA: 0x002B6794 File Offset: 0x002B4994
	public void SelectPresetByName(string _presetName, string _tryForceGroup = null, bool _forceChangedCallback = false)
	{
		SandboxOptionPreset sandboxOptionPreset = this.sandboxManager.GetPreset(_presetName);
		string text;
		if (sandboxOptionPreset != null)
		{
			text = sandboxOptionPreset.Group;
		}
		else
		{
			sandboxOptionPreset = this.sandboxManager.GetDefaultPreset();
			text = _tryForceGroup ?? sandboxOptionPreset.Group;
		}
		this.updatePresetGroups(sandboxOptionPreset.IsCustomPreset);
		this.selectGroupByName(text);
		this.updatePresetsForSelectedGroup(sandboxOptionPreset.Name, _forceChangedCallback);
	}

	// Token: 0x06007022 RID: 28706 RVA: 0x002B67F2 File Offset: 0x002B49F2
	public override void OnOpen()
	{
		base.OnOpen();
		this.updatePresetGroups(false);
		this.updatePresetsForSelectedGroup(null, false);
	}

	// Token: 0x06007023 RID: 28707 RVA: 0x00291183 File Offset: 0x0028F383
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x040054F4 RID: 21748
	[XuiBindComponent("cbxPresetGroup", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<SandboxPresetGroupData> cbxPresetGroups;

	// Token: 0x040054F5 RID: 21749
	[XuiBindComponent("cbxPreset", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<SandboxPresetInfo> cbxPreset;

	// Token: 0x040054F6 RID: 21750
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SandboxOptionManager sandboxManager = SandboxOptionManager.Current;

	// Token: 0x040054F7 RID: 21751
	[PublicizedFrom(EAccessModifier.Private)]
	public string sandBoxPresetDescription;

	// Token: 0x040054F8 RID: 21752
	[PublicizedFrom(EAccessModifier.Private)]
	public string sandBoxValuesTemplate;
}

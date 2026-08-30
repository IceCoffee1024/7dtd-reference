using System;
using SandboxOptions;
using UnityEngine.Scripting;

// Token: 0x02000E3F RID: 3647
[Preserve]
public class XUiC_SandboxPresetSelector : XUiController
{
	// Token: 0x1400009F RID: 159
	// (add) Token: 0x0600701C RID: 28700 RVA: 0x002B3038 File Offset: 0x002B1238
	// (remove) Token: 0x0600701D RID: 28701 RVA: 0x002B3070 File Offset: 0x002B1270
	public event SandboxPresetSelectionChangedDelegate SandboxPresetSelectionChanged;

	// Token: 0x17000CA1 RID: 3233
	// (get) Token: 0x0600701E RID: 28702 RVA: 0x002B30A5 File Offset: 0x002B12A5
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

	// Token: 0x17000CA2 RID: 3234
	// (get) Token: 0x0600701F RID: 28703 RVA: 0x002B30C7 File Offset: 0x002B12C7
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

	// Token: 0x17000CA3 RID: 3235
	// (get) Token: 0x06007020 RID: 28704 RVA: 0x002B30E9 File Offset: 0x002B12E9
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

	// Token: 0x17000CA4 RID: 3236
	// (get) Token: 0x06007021 RID: 28705 RVA: 0x002B3101 File Offset: 0x002B1301
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

	// Token: 0x17000CA5 RID: 3237
	// (get) Token: 0x06007022 RID: 28706 RVA: 0x002B3119 File Offset: 0x002B1319
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

	// Token: 0x17000CA6 RID: 3238
	// (get) Token: 0x06007023 RID: 28707 RVA: 0x002B3131 File Offset: 0x002B1331
	public SandboxPresetInfo SelectedPreset
	{
		get
		{
			return this.cbxPreset.Value;
		}
	}

	// Token: 0x06007024 RID: 28708 RVA: 0x002B3140 File Offset: 0x002B1340
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

	// Token: 0x06007025 RID: 28709 RVA: 0x002B3228 File Offset: 0x002B1428
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

	// Token: 0x06007026 RID: 28710 RVA: 0x002B3374 File Offset: 0x002B1574
	[XuiBindEvent("OnValueChanged", "cbxPresetGroups")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxPresetGroups_OnValueChanged(XUiController _sender, SandboxPresetGroupData _oldValue, SandboxPresetGroupData _newValue)
	{
		this.updatePresetsForSelectedGroup(null, false);
		this.IsDirty = true;
	}

	// Token: 0x06007027 RID: 28711 RVA: 0x002B3385 File Offset: 0x002B1585
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

	// Token: 0x06007028 RID: 28712 RVA: 0x002B33A0 File Offset: 0x002B15A0
	[PublicizedFrom(EAccessModifier.Private)]
	public void selectGroupByName(string _groupName)
	{
		this.cbxPresetGroups.SelectedIndex = this.cbxPresetGroups.Elements.FindIndex((SandboxPresetGroupData _data) => _data.InternalName == _groupName);
	}

	// Token: 0x06007029 RID: 28713 RVA: 0x002B33E4 File Offset: 0x002B15E4
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

	// Token: 0x0600702A RID: 28714 RVA: 0x002B3442 File Offset: 0x002B1642
	public override void OnOpen()
	{
		base.OnOpen();
		this.updatePresetGroups(false);
		this.updatePresetsForSelectedGroup(null, false);
	}

	// Token: 0x0600702B RID: 28715 RVA: 0x0028DC1B File Offset: 0x0028BE1B
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x040054BD RID: 21693
	[XuiBindComponent("cbxPresetGroup", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<SandboxPresetGroupData> cbxPresetGroups;

	// Token: 0x040054BE RID: 21694
	[XuiBindComponent("cbxPreset", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<SandboxPresetInfo> cbxPreset;

	// Token: 0x040054BF RID: 21695
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SandboxOptionManager sandboxManager = SandboxOptionManager.Current;

	// Token: 0x040054C0 RID: 21696
	[PublicizedFrom(EAccessModifier.Private)]
	public string sandBoxPresetDescription;

	// Token: 0x040054C1 RID: 21697
	[PublicizedFrom(EAccessModifier.Private)]
	public string sandBoxValuesTemplate;
}

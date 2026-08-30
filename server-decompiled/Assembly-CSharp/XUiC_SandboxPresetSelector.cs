using System;
using SandboxOptions;
using UnityEngine.Scripting;

// Token: 0x02000E2A RID: 3626
[Preserve]
public class XUiC_SandboxPresetSelector : XUiController
{
	// Token: 0x1400009E RID: 158
	// (add) Token: 0x06006F7C RID: 28540 RVA: 0x002AFB14 File Offset: 0x002ADD14
	// (remove) Token: 0x06006F7D RID: 28541 RVA: 0x002AFB4C File Offset: 0x002ADD4C
	public event SandboxPresetSelectionChangedDelegate SandboxPresetSelectionChanged;

	// Token: 0x17000C88 RID: 3208
	// (get) Token: 0x06006F7E RID: 28542 RVA: 0x002AFB81 File Offset: 0x002ADD81
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

	// Token: 0x17000C89 RID: 3209
	// (get) Token: 0x06006F7F RID: 28543 RVA: 0x002AFBA3 File Offset: 0x002ADDA3
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

	// Token: 0x17000C8A RID: 3210
	// (get) Token: 0x06006F80 RID: 28544 RVA: 0x002AFBC5 File Offset: 0x002ADDC5
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

	// Token: 0x17000C8B RID: 3211
	// (get) Token: 0x06006F81 RID: 28545 RVA: 0x002AFBDD File Offset: 0x002ADDDD
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

	// Token: 0x17000C8C RID: 3212
	// (get) Token: 0x06006F82 RID: 28546 RVA: 0x002AFBF5 File Offset: 0x002ADDF5
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

	// Token: 0x17000C8D RID: 3213
	// (get) Token: 0x06006F83 RID: 28547 RVA: 0x002AFC0D File Offset: 0x002ADE0D
	public SandboxPresetInfo SelectedPreset
	{
		get
		{
			return this.cbxPreset.Value;
		}
	}

	// Token: 0x06006F84 RID: 28548 RVA: 0x002AFC1C File Offset: 0x002ADE1C
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

	// Token: 0x06006F85 RID: 28549 RVA: 0x002AFD04 File Offset: 0x002ADF04
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

	// Token: 0x06006F86 RID: 28550 RVA: 0x002AFE50 File Offset: 0x002AE050
	[XuiBindEvent("OnValueChanged", "cbxPresetGroups")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxPresetGroups_OnValueChanged(XUiController _sender, SandboxPresetGroupData _oldValue, SandboxPresetGroupData _newValue)
	{
		this.updatePresetsForSelectedGroup(null, false);
		this.IsDirty = true;
	}

	// Token: 0x06006F87 RID: 28551 RVA: 0x002AFE61 File Offset: 0x002AE061
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

	// Token: 0x06006F88 RID: 28552 RVA: 0x002AFE7C File Offset: 0x002AE07C
	[PublicizedFrom(EAccessModifier.Private)]
	public void selectGroupByName(string _groupName)
	{
		this.cbxPresetGroups.SelectedIndex = this.cbxPresetGroups.Elements.FindIndex((SandboxPresetGroupData _data) => _data.InternalName == _groupName);
	}

	// Token: 0x06006F89 RID: 28553 RVA: 0x002AFEC0 File Offset: 0x002AE0C0
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

	// Token: 0x06006F8A RID: 28554 RVA: 0x002AFF1E File Offset: 0x002AE11E
	public override void OnOpen()
	{
		base.OnOpen();
		this.updatePresetGroups(false);
		this.updatePresetsForSelectedGroup(null, false);
	}

	// Token: 0x06006F8B RID: 28555 RVA: 0x0028AD37 File Offset: 0x00288F37
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x04005419 RID: 21529
	[XuiBindComponent("cbxPresetGroup", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<SandboxPresetGroupData> cbxPresetGroups;

	// Token: 0x0400541A RID: 21530
	[XuiBindComponent("cbxPreset", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<SandboxPresetInfo> cbxPreset;

	// Token: 0x0400541B RID: 21531
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SandboxOptionManager sandboxManager = SandboxOptionManager.Current;

	// Token: 0x0400541C RID: 21532
	[PublicizedFrom(EAccessModifier.Private)]
	public string sandBoxPresetDescription;

	// Token: 0x0400541D RID: 21533
	[PublicizedFrom(EAccessModifier.Private)]
	public string sandBoxValuesTemplate;
}

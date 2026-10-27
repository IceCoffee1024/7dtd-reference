using System;
using SandboxOptions;
using UnityEngine.Scripting;

// Token: 0x02000E59 RID: 3673
[Preserve]
public class XUiC_SandboxPresetSelector : XUiController
{
	// Token: 0x1400009E RID: 158
	// (add) Token: 0x06007039 RID: 28729 RVA: 0x002B7954 File Offset: 0x002B5B54
	// (remove) Token: 0x0600703A RID: 28730 RVA: 0x002B798C File Offset: 0x002B5B8C
	public event SandboxPresetSelectionChangedDelegate SandboxPresetSelectionChanged;

	// Token: 0x17000CC2 RID: 3266
	// (get) Token: 0x0600703B RID: 28731 RVA: 0x002B79C1 File Offset: 0x002B5BC1
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

	// Token: 0x17000CC3 RID: 3267
	// (get) Token: 0x0600703C RID: 28732 RVA: 0x002B79E3 File Offset: 0x002B5BE3
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

	// Token: 0x17000CC4 RID: 3268
	// (get) Token: 0x0600703D RID: 28733 RVA: 0x002B7A05 File Offset: 0x002B5C05
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

	// Token: 0x17000CC5 RID: 3269
	// (get) Token: 0x0600703E RID: 28734 RVA: 0x002B7A1D File Offset: 0x002B5C1D
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

	// Token: 0x17000CC6 RID: 3270
	// (get) Token: 0x0600703F RID: 28735 RVA: 0x002B7A35 File Offset: 0x002B5C35
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

	// Token: 0x17000CC7 RID: 3271
	// (get) Token: 0x06007040 RID: 28736 RVA: 0x002B7A4D File Offset: 0x002B5C4D
	public SandboxPresetInfo SelectedPreset
	{
		get
		{
			return this.cbxPreset.Value;
		}
	}

	// Token: 0x06007041 RID: 28737 RVA: 0x002B7A5C File Offset: 0x002B5C5C
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

	// Token: 0x06007042 RID: 28738 RVA: 0x002B7B44 File Offset: 0x002B5D44
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

	// Token: 0x06007043 RID: 28739 RVA: 0x002B7C90 File Offset: 0x002B5E90
	[XuiBindEvent("OnValueChanged", "cbxPresetGroups")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxPresetGroups_OnValueChanged(XUiController _sender, SandboxPresetGroupData _oldValue, SandboxPresetGroupData _newValue)
	{
		this.updatePresetsForSelectedGroup(null, false);
		this.IsDirty = true;
	}

	// Token: 0x06007044 RID: 28740 RVA: 0x002B7CA1 File Offset: 0x002B5EA1
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

	// Token: 0x06007045 RID: 28741 RVA: 0x002B7CBC File Offset: 0x002B5EBC
	[PublicizedFrom(EAccessModifier.Private)]
	public void selectGroupByName(string _groupName)
	{
		this.cbxPresetGroups.SelectedIndex = this.cbxPresetGroups.Elements.FindIndex((SandboxPresetGroupData _data) => _data.InternalName == _groupName);
	}

	// Token: 0x06007046 RID: 28742 RVA: 0x002B7D00 File Offset: 0x002B5F00
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

	// Token: 0x06007047 RID: 28743 RVA: 0x002B7D5E File Offset: 0x002B5F5E
	public override void OnOpen()
	{
		base.OnOpen();
		this.updatePresetGroups(false);
		this.updatePresetsForSelectedGroup(null, false);
	}

	// Token: 0x06007048 RID: 28744 RVA: 0x002926AB File Offset: 0x002908AB
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x04005519 RID: 21785
	[XuiBindComponent("cbxPresetGroup", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<SandboxPresetGroupData> cbxPresetGroups;

	// Token: 0x0400551A RID: 21786
	[XuiBindComponent("cbxPreset", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<SandboxPresetInfo> cbxPreset;

	// Token: 0x0400551B RID: 21787
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SandboxOptionManager sandboxManager = SandboxOptionManager.Current;

	// Token: 0x0400551C RID: 21788
	[PublicizedFrom(EAccessModifier.Private)]
	public string sandBoxPresetDescription;

	// Token: 0x0400551D RID: 21789
	[PublicizedFrom(EAccessModifier.Private)]
	public string sandBoxValuesTemplate;
}

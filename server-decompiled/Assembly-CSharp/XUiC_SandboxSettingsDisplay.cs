using System;
using System.Collections.Generic;
using System.Text;
using SandboxOptions;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E5C RID: 3676
[Preserve]
public class XUiC_SandboxSettingsDisplay : XUiController
{
	// Token: 0x17000CC8 RID: 3272
	// (get) Token: 0x0600704F RID: 28751 RVA: 0x002B7DBD File Offset: 0x002B5FBD
	// (set) Token: 0x06007050 RID: 28752 RVA: 0x002B7DC5 File Offset: 0x002B5FC5
	[XuiXmlAttribute("pause_game", false)]
	public bool PauseGame
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000CC9 RID: 3273
	// (get) Token: 0x06007051 RID: 28753 RVA: 0x002B7DCE File Offset: 0x002B5FCE
	// (set) Token: 0x06007052 RID: 28754 RVA: 0x002B7DD6 File Offset: 0x002B5FD6
	[XuiXmlAttribute("sandbox_values_columns", false)]
	public int SandboxValuesColumns
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.sandboxValuesColumns;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.sandboxValuesColumns = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000CCA RID: 3274
	// (get) Token: 0x06007053 RID: 28755 RVA: 0x002B7DE6 File Offset: 0x002B5FE6
	// (set) Token: 0x06007054 RID: 28756 RVA: 0x002B7DF0 File Offset: 0x002B5FF0
	[XuiXmlAttribute("sandbox_values_template", false)]
	public string SandBoxValuesTemplate
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.sandBoxValuesTemplate;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (value == null)
			{
				this.sandBoxValuesTemplate = "";
				this.IsDirty = true;
				return;
			}
			this.sandBoxValuesTemplate = value.Replace("%0", "{0}").Replace("%1", "{1}");
			this.IsDirty = true;
		}
	}

	// Token: 0x17000CCB RID: 3275
	// (get) Token: 0x06007055 RID: 28757 RVA: 0x002B7E3F File Offset: 0x002B603F
	// (set) Token: 0x06007056 RID: 28758 RVA: 0x002B7E48 File Offset: 0x002B6048
	[XuiXmlAttribute("sandbox_values_template_defaults", false)]
	public string SandBoxValuesTemplateDefaults
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.sandBoxValuesTemplateDefaults;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (value == null)
			{
				this.sandBoxValuesTemplateDefaults = "";
				this.IsDirty = true;
				return;
			}
			this.sandBoxValuesTemplateDefaults = value.Replace("%0", "{0}").Replace("%1", "{1}");
			this.IsDirty = true;
		}
	}

	// Token: 0x17000CCC RID: 3276
	// (get) Token: 0x06007057 RID: 28759 RVA: 0x002B7E97 File Offset: 0x002B6097
	// (set) Token: 0x06007058 RID: 28760 RVA: 0x002B7EA8 File Offset: 0x002B60A8
	[XuiXmlAttribute("sandboxcode", false)]
	[XuiXmlBinding("sandboxcode")]
	public string SandboxCode
	{
		get
		{
			return this.sandboxCode ?? "";
		}
		set
		{
			if (string.Equals(value, this.sandboxCode))
			{
				return;
			}
			this.sandboxName = null;
			this.sandboxCode = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000CCD RID: 3277
	// (get) Token: 0x06007059 RID: 28761 RVA: 0x002B7ECE File Offset: 0x002B60CE
	// (set) Token: 0x0600705A RID: 28762 RVA: 0x002B7EDF File Offset: 0x002B60DF
	[XuiXmlAttribute("sandboxname", false)]
	[XuiXmlBinding("sandboxname")]
	public string SandboxName
	{
		get
		{
			return this.sandboxName ?? "";
		}
		set
		{
			if (string.Equals(value, this.sandboxName))
			{
				return;
			}
			this.sandboxCode = null;
			this.sandboxName = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000CCE RID: 3278
	// (get) Token: 0x0600705B RID: 28763 RVA: 0x002B7F05 File Offset: 0x002B6105
	// (set) Token: 0x0600705C RID: 28764 RVA: 0x002B7F0D File Offset: 0x002B610D
	[XuiXmlAttribute("descriptiontemplate", false)]
	public string SandboxDescriptionTemplate { get; set; }

	// Token: 0x17000CCF RID: 3279
	// (get) Token: 0x0600705D RID: 28765 RVA: 0x002B7F16 File Offset: 0x002B6116
	// (set) Token: 0x0600705E RID: 28766 RVA: 0x002B7F1E File Offset: 0x002B611E
	[XuiXmlBinding("sandboxcodevalid")]
	public bool Valid
	{
		get
		{
			return this.valid;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (this.valid == value)
			{
				return;
			}
			this.valid = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000CD0 RID: 3280
	// (get) Token: 0x0600705F RID: 28767 RVA: 0x002B7F38 File Offset: 0x002B6138
	[XuiXmlBinding("sandboxvalues")]
	public Dictionary<string, string> SandboxValues
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
	} = new Dictionary<string, string>();

	// Token: 0x06007060 RID: 28768 RVA: 0x002B7F40 File Offset: 0x002B6140
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.RefreshBindings();
		if (this.IsDirty)
		{
			this.IsDirty = false;
			this.updateSandboxData();
		}
	}

	// Token: 0x06007061 RID: 28769 RVA: 0x002B7F64 File Offset: 0x002B6164
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateSandboxData()
	{
		this.SandboxValues.Clear();
		if (this.sandboxName == null && this.sandboxCode == null)
		{
			return;
		}
		SandboxOptionPreset sandboxOptionPreset;
		if (this.sandboxName != null)
		{
			sandboxOptionPreset = SandboxOptionManager.Current.GetPreset(this.sandboxName);
			this.sandboxCode = sandboxOptionPreset.SandboxCode;
		}
		else
		{
			sandboxOptionPreset = new SandboxOptionPreset();
			this.Valid = this.sandboxManager.LoadOptionsFromCode(this.SandboxCode, sandboxOptionPreset);
			if (!this.valid)
			{
				return;
			}
		}
		List<ValueTuple<string, string, bool>> list;
		if (!this.sandboxManager.GetChangedPresetOptions(sandboxOptionPreset, out list))
		{
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < this.SandboxValuesColumns; i++)
		{
			for (int j = 0; j < list.Count; j++)
			{
				int num = i + j * this.SandboxValuesColumns;
				if (num >= list.Count)
				{
					break;
				}
				stringBuilder.AppendFormat(list[num].Item3 ? this.SandBoxValuesTemplateDefaults : this.SandBoxValuesTemplate, list[num].Item1, list[num].Item2);
			}
			this.SandboxValues[(i + 1).ToString()] = stringBuilder.ToString();
			stringBuilder.Clear();
		}
		ThreadManager.RunTaskAfterFrames(delegate
		{
			if (this.sandboxPresetValuesScroll != null)
			{
				this.sandboxPresetValuesScroll.ResetPosition();
				return;
			}
			if (this.parentScroll != null)
			{
				this.parentScroll.ResetPosition();
			}
		}, 3);
	}

	// Token: 0x06007062 RID: 28770 RVA: 0x002B80A9 File Offset: 0x002B62A9
	[XuiBindEvent("OnPress", "btnCopyCode")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnCopyCode_OnPress(XUiController _sender, int _mouseButton)
	{
		GUIUtility.systemCopyBuffer = this.SandboxCode;
	}

	// Token: 0x06007063 RID: 28771 RVA: 0x002B80B6 File Offset: 0x002B62B6
	[XuiBindEvent("OnPress", "btnSaveAsPreset")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnSaveAsPreset_OnPress(XUiController _sender, int _mouseButton)
	{
		XUiC_SandboxSettingsSaveAsPreset.Open(LocalPlayerUI.primaryUI.xui, this.SandboxCode, this.SandboxDescriptionTemplate, null);
	}

	// Token: 0x06007064 RID: 28772 RVA: 0x002B80D4 File Offset: 0x002B62D4
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.PauseGame)
		{
			GameManager.Instance.Pause(true);
		}
	}

	// Token: 0x06007065 RID: 28773 RVA: 0x002B80EF File Offset: 0x002B62EF
	public override void OnClose()
	{
		base.OnOpen();
		if (this.PauseGame)
		{
			GameManager.Instance.Pause(false);
		}
	}

	// Token: 0x04005521 RID: 21793
	[XuiBindComponent("sandboxPresetValues.", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_ScrollView sandboxPresetValuesScroll;

	// Token: 0x04005522 RID: 21794
	[XuiBindParent(false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_ScrollView parentScroll;

	// Token: 0x04005523 RID: 21795
	[XuiBindComponent("btnCopyCode", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCopyCode;

	// Token: 0x04005524 RID: 21796
	[XuiBindComponent("btnSaveAsPreset", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnSaveAsPreset;

	// Token: 0x04005525 RID: 21797
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SandboxOptionManager sandboxManager = SandboxOptionManager.Current;

	// Token: 0x04005526 RID: 21798
	[PublicizedFrom(EAccessModifier.Private)]
	public int sandboxValuesColumns;

	// Token: 0x04005527 RID: 21799
	[PublicizedFrom(EAccessModifier.Private)]
	public string sandBoxValuesTemplate;

	// Token: 0x04005528 RID: 21800
	[PublicizedFrom(EAccessModifier.Private)]
	public string sandBoxValuesTemplateDefaults;

	// Token: 0x0400552A RID: 21802
	[PublicizedFrom(EAccessModifier.Private)]
	public string sandboxCode;

	// Token: 0x0400552B RID: 21803
	[PublicizedFrom(EAccessModifier.Private)]
	public string sandboxName;

	// Token: 0x0400552D RID: 21805
	[PublicizedFrom(EAccessModifier.Private)]
	public bool valid;
}

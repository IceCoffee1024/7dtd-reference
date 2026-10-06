using System;
using System.Collections.Generic;
using System.Text;
using SandboxOptions;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E51 RID: 3665
[Preserve]
public class XUiC_SandboxSettingsDisplay : XUiController
{
	// Token: 0x17000CC2 RID: 3266
	// (get) Token: 0x0600702A RID: 28714 RVA: 0x002B6851 File Offset: 0x002B4A51
	// (set) Token: 0x0600702B RID: 28715 RVA: 0x002B6859 File Offset: 0x002B4A59
	[XuiXmlAttribute("pause_game", false)]
	public bool PauseGame
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000CC3 RID: 3267
	// (get) Token: 0x0600702C RID: 28716 RVA: 0x002B6862 File Offset: 0x002B4A62
	// (set) Token: 0x0600702D RID: 28717 RVA: 0x002B686A File Offset: 0x002B4A6A
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

	// Token: 0x17000CC4 RID: 3268
	// (get) Token: 0x0600702E RID: 28718 RVA: 0x002B687A File Offset: 0x002B4A7A
	// (set) Token: 0x0600702F RID: 28719 RVA: 0x002B6884 File Offset: 0x002B4A84
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

	// Token: 0x17000CC5 RID: 3269
	// (get) Token: 0x06007030 RID: 28720 RVA: 0x002B68D3 File Offset: 0x002B4AD3
	// (set) Token: 0x06007031 RID: 28721 RVA: 0x002B68DC File Offset: 0x002B4ADC
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

	// Token: 0x17000CC6 RID: 3270
	// (get) Token: 0x06007032 RID: 28722 RVA: 0x002B692B File Offset: 0x002B4B2B
	// (set) Token: 0x06007033 RID: 28723 RVA: 0x002B693C File Offset: 0x002B4B3C
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

	// Token: 0x17000CC7 RID: 3271
	// (get) Token: 0x06007034 RID: 28724 RVA: 0x002B6962 File Offset: 0x002B4B62
	// (set) Token: 0x06007035 RID: 28725 RVA: 0x002B6973 File Offset: 0x002B4B73
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

	// Token: 0x17000CC8 RID: 3272
	// (get) Token: 0x06007036 RID: 28726 RVA: 0x002B6999 File Offset: 0x002B4B99
	// (set) Token: 0x06007037 RID: 28727 RVA: 0x002B69A1 File Offset: 0x002B4BA1
	[XuiXmlAttribute("descriptiontemplate", false)]
	public string SandboxDescriptionTemplate { get; set; }

	// Token: 0x17000CC9 RID: 3273
	// (get) Token: 0x06007038 RID: 28728 RVA: 0x002B69AA File Offset: 0x002B4BAA
	// (set) Token: 0x06007039 RID: 28729 RVA: 0x002B69B2 File Offset: 0x002B4BB2
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

	// Token: 0x17000CCA RID: 3274
	// (get) Token: 0x0600703A RID: 28730 RVA: 0x002B69CC File Offset: 0x002B4BCC
	[XuiXmlBinding("sandboxvalues")]
	public Dictionary<string, string> SandboxValues
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
	} = new Dictionary<string, string>();

	// Token: 0x0600703B RID: 28731 RVA: 0x002B69D4 File Offset: 0x002B4BD4
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

	// Token: 0x0600703C RID: 28732 RVA: 0x002B69F8 File Offset: 0x002B4BF8
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

	// Token: 0x0600703D RID: 28733 RVA: 0x002B6B3D File Offset: 0x002B4D3D
	[XuiBindEvent("OnPress", "btnCopyCode")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnCopyCode_OnPress(XUiController _sender, int _mouseButton)
	{
		GUIUtility.systemCopyBuffer = this.SandboxCode;
	}

	// Token: 0x0600703E RID: 28734 RVA: 0x002B6B4A File Offset: 0x002B4D4A
	[XuiBindEvent("OnPress", "btnSaveAsPreset")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnSaveAsPreset_OnPress(XUiController _sender, int _mouseButton)
	{
		XUiC_SandboxSettingsSaveAsPreset.Open(LocalPlayerUI.primaryUI.xui, this.SandboxCode, this.SandboxDescriptionTemplate, null);
	}

	// Token: 0x0600703F RID: 28735 RVA: 0x002B6B68 File Offset: 0x002B4D68
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.PauseGame)
		{
			GameManager.Instance.Pause(true);
		}
	}

	// Token: 0x06007040 RID: 28736 RVA: 0x002B6B83 File Offset: 0x002B4D83
	public override void OnClose()
	{
		base.OnOpen();
		if (this.PauseGame)
		{
			GameManager.Instance.Pause(false);
		}
	}

	// Token: 0x040054FC RID: 21756
	[XuiBindComponent("sandboxPresetValues.", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_ScrollView sandboxPresetValuesScroll;

	// Token: 0x040054FD RID: 21757
	[XuiBindParent(false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_ScrollView parentScroll;

	// Token: 0x040054FE RID: 21758
	[XuiBindComponent("btnCopyCode", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCopyCode;

	// Token: 0x040054FF RID: 21759
	[XuiBindComponent("btnSaveAsPreset", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnSaveAsPreset;

	// Token: 0x04005500 RID: 21760
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SandboxOptionManager sandboxManager = SandboxOptionManager.Current;

	// Token: 0x04005501 RID: 21761
	[PublicizedFrom(EAccessModifier.Private)]
	public int sandboxValuesColumns;

	// Token: 0x04005502 RID: 21762
	[PublicizedFrom(EAccessModifier.Private)]
	public string sandBoxValuesTemplate;

	// Token: 0x04005503 RID: 21763
	[PublicizedFrom(EAccessModifier.Private)]
	public string sandBoxValuesTemplateDefaults;

	// Token: 0x04005505 RID: 21765
	[PublicizedFrom(EAccessModifier.Private)]
	public string sandboxCode;

	// Token: 0x04005506 RID: 21766
	[PublicizedFrom(EAccessModifier.Private)]
	public string sandboxName;

	// Token: 0x04005508 RID: 21768
	[PublicizedFrom(EAccessModifier.Private)]
	public bool valid;
}

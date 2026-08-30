using System;
using System.Collections.Generic;
using System.Text;
using SandboxOptions;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E42 RID: 3650
[Preserve]
public class XUiC_SandboxSettingsDisplay : XUiController
{
	// Token: 0x17000CA7 RID: 3239
	// (get) Token: 0x06007032 RID: 28722 RVA: 0x002B34A1 File Offset: 0x002B16A1
	// (set) Token: 0x06007033 RID: 28723 RVA: 0x002B34A9 File Offset: 0x002B16A9
	[XuiXmlAttribute("pause_game", false)]
	public bool PauseGame
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000CA8 RID: 3240
	// (get) Token: 0x06007034 RID: 28724 RVA: 0x002B34B2 File Offset: 0x002B16B2
	// (set) Token: 0x06007035 RID: 28725 RVA: 0x002B34BA File Offset: 0x002B16BA
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

	// Token: 0x17000CA9 RID: 3241
	// (get) Token: 0x06007036 RID: 28726 RVA: 0x002B34CA File Offset: 0x002B16CA
	// (set) Token: 0x06007037 RID: 28727 RVA: 0x002B34D4 File Offset: 0x002B16D4
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

	// Token: 0x17000CAA RID: 3242
	// (get) Token: 0x06007038 RID: 28728 RVA: 0x002B3523 File Offset: 0x002B1723
	// (set) Token: 0x06007039 RID: 28729 RVA: 0x002B352C File Offset: 0x002B172C
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

	// Token: 0x17000CAB RID: 3243
	// (get) Token: 0x0600703A RID: 28730 RVA: 0x002B357B File Offset: 0x002B177B
	// (set) Token: 0x0600703B RID: 28731 RVA: 0x002B358C File Offset: 0x002B178C
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

	// Token: 0x17000CAC RID: 3244
	// (get) Token: 0x0600703C RID: 28732 RVA: 0x002B35B2 File Offset: 0x002B17B2
	// (set) Token: 0x0600703D RID: 28733 RVA: 0x002B35C3 File Offset: 0x002B17C3
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

	// Token: 0x17000CAD RID: 3245
	// (get) Token: 0x0600703E RID: 28734 RVA: 0x002B35E9 File Offset: 0x002B17E9
	// (set) Token: 0x0600703F RID: 28735 RVA: 0x002B35F1 File Offset: 0x002B17F1
	[XuiXmlAttribute("descriptiontemplate", false)]
	public string SandboxDescriptionTemplate { get; set; }

	// Token: 0x17000CAE RID: 3246
	// (get) Token: 0x06007040 RID: 28736 RVA: 0x002B35FA File Offset: 0x002B17FA
	// (set) Token: 0x06007041 RID: 28737 RVA: 0x002B3602 File Offset: 0x002B1802
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

	// Token: 0x17000CAF RID: 3247
	// (get) Token: 0x06007042 RID: 28738 RVA: 0x002B361C File Offset: 0x002B181C
	[XuiXmlBinding("sandboxvalues")]
	public Dictionary<string, string> SandboxValues
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
	} = new Dictionary<string, string>();

	// Token: 0x06007043 RID: 28739 RVA: 0x002B3624 File Offset: 0x002B1824
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

	// Token: 0x06007044 RID: 28740 RVA: 0x002B3648 File Offset: 0x002B1848
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

	// Token: 0x06007045 RID: 28741 RVA: 0x002B378D File Offset: 0x002B198D
	[XuiBindEvent("OnPress", "btnCopyCode")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnCopyCode_OnPress(XUiController _sender, int _mouseButton)
	{
		GUIUtility.systemCopyBuffer = this.SandboxCode;
	}

	// Token: 0x06007046 RID: 28742 RVA: 0x002B379A File Offset: 0x002B199A
	[XuiBindEvent("OnPress", "btnSaveAsPreset")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnSaveAsPreset_OnPress(XUiController _sender, int _mouseButton)
	{
		XUiC_SandboxSettingsSaveAsPreset.Open(LocalPlayerUI.primaryUI.xui, this.SandboxCode, this.SandboxDescriptionTemplate, null);
	}

	// Token: 0x06007047 RID: 28743 RVA: 0x002B37B8 File Offset: 0x002B19B8
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.PauseGame)
		{
			GameManager.Instance.Pause(true);
		}
	}

	// Token: 0x06007048 RID: 28744 RVA: 0x002B37D3 File Offset: 0x002B19D3
	public override void OnClose()
	{
		base.OnOpen();
		if (this.PauseGame)
		{
			GameManager.Instance.Pause(false);
		}
	}

	// Token: 0x040054C5 RID: 21701
	[XuiBindComponent("sandboxPresetValues.", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_ScrollView sandboxPresetValuesScroll;

	// Token: 0x040054C6 RID: 21702
	[XuiBindParent(false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_ScrollView parentScroll;

	// Token: 0x040054C7 RID: 21703
	[XuiBindComponent("btnCopyCode", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCopyCode;

	// Token: 0x040054C8 RID: 21704
	[XuiBindComponent("btnSaveAsPreset", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnSaveAsPreset;

	// Token: 0x040054C9 RID: 21705
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SandboxOptionManager sandboxManager = SandboxOptionManager.Current;

	// Token: 0x040054CA RID: 21706
	[PublicizedFrom(EAccessModifier.Private)]
	public int sandboxValuesColumns;

	// Token: 0x040054CB RID: 21707
	[PublicizedFrom(EAccessModifier.Private)]
	public string sandBoxValuesTemplate;

	// Token: 0x040054CC RID: 21708
	[PublicizedFrom(EAccessModifier.Private)]
	public string sandBoxValuesTemplateDefaults;

	// Token: 0x040054CE RID: 21710
	[PublicizedFrom(EAccessModifier.Private)]
	public string sandboxCode;

	// Token: 0x040054CF RID: 21711
	[PublicizedFrom(EAccessModifier.Private)]
	public string sandboxName;

	// Token: 0x040054D1 RID: 21713
	[PublicizedFrom(EAccessModifier.Private)]
	public bool valid;
}

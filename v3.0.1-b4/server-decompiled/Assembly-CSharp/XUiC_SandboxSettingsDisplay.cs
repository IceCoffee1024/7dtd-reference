using System;
using System.Collections.Generic;
using System.Text;
using SandboxOptions;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E2D RID: 3629
[Preserve]
public class XUiC_SandboxSettingsDisplay : XUiController
{
	// Token: 0x17000C8E RID: 3214
	// (get) Token: 0x06006F92 RID: 28562 RVA: 0x002AFF7D File Offset: 0x002AE17D
	// (set) Token: 0x06006F93 RID: 28563 RVA: 0x002AFF85 File Offset: 0x002AE185
	[XuiXmlAttribute("pause_game", false)]
	public bool PauseGame
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000C8F RID: 3215
	// (get) Token: 0x06006F94 RID: 28564 RVA: 0x002AFF8E File Offset: 0x002AE18E
	// (set) Token: 0x06006F95 RID: 28565 RVA: 0x002AFF96 File Offset: 0x002AE196
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

	// Token: 0x17000C90 RID: 3216
	// (get) Token: 0x06006F96 RID: 28566 RVA: 0x002AFFA6 File Offset: 0x002AE1A6
	// (set) Token: 0x06006F97 RID: 28567 RVA: 0x002AFFB0 File Offset: 0x002AE1B0
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

	// Token: 0x17000C91 RID: 3217
	// (get) Token: 0x06006F98 RID: 28568 RVA: 0x002AFFFF File Offset: 0x002AE1FF
	// (set) Token: 0x06006F99 RID: 28569 RVA: 0x002B0008 File Offset: 0x002AE208
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

	// Token: 0x17000C92 RID: 3218
	// (get) Token: 0x06006F9A RID: 28570 RVA: 0x002B0057 File Offset: 0x002AE257
	// (set) Token: 0x06006F9B RID: 28571 RVA: 0x002B0068 File Offset: 0x002AE268
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

	// Token: 0x17000C93 RID: 3219
	// (get) Token: 0x06006F9C RID: 28572 RVA: 0x002B008E File Offset: 0x002AE28E
	// (set) Token: 0x06006F9D RID: 28573 RVA: 0x002B009F File Offset: 0x002AE29F
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

	// Token: 0x17000C94 RID: 3220
	// (get) Token: 0x06006F9E RID: 28574 RVA: 0x002B00C5 File Offset: 0x002AE2C5
	// (set) Token: 0x06006F9F RID: 28575 RVA: 0x002B00CD File Offset: 0x002AE2CD
	[XuiXmlAttribute("descriptiontemplate", false)]
	public string SandboxDescriptionTemplate { get; set; }

	// Token: 0x17000C95 RID: 3221
	// (get) Token: 0x06006FA0 RID: 28576 RVA: 0x002B00D6 File Offset: 0x002AE2D6
	// (set) Token: 0x06006FA1 RID: 28577 RVA: 0x002B00DE File Offset: 0x002AE2DE
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

	// Token: 0x17000C96 RID: 3222
	// (get) Token: 0x06006FA2 RID: 28578 RVA: 0x002B00F8 File Offset: 0x002AE2F8
	[XuiXmlBinding("sandboxvalues")]
	public Dictionary<string, string> SandboxValues
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
	} = new Dictionary<string, string>();

	// Token: 0x06006FA3 RID: 28579 RVA: 0x002B0100 File Offset: 0x002AE300
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

	// Token: 0x06006FA4 RID: 28580 RVA: 0x002B0124 File Offset: 0x002AE324
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

	// Token: 0x06006FA5 RID: 28581 RVA: 0x002B0269 File Offset: 0x002AE469
	[XuiBindEvent("OnPress", "btnCopyCode")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnCopyCode_OnPress(XUiController _sender, int _mouseButton)
	{
		GUIUtility.systemCopyBuffer = this.SandboxCode;
	}

	// Token: 0x06006FA6 RID: 28582 RVA: 0x002B0276 File Offset: 0x002AE476
	[XuiBindEvent("OnPress", "btnSaveAsPreset")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnSaveAsPreset_OnPress(XUiController _sender, int _mouseButton)
	{
		XUiC_SandboxSettingsSaveAsPreset.Open(LocalPlayerUI.primaryUI.xui, this.SandboxCode, this.SandboxDescriptionTemplate, null);
	}

	// Token: 0x06006FA7 RID: 28583 RVA: 0x002B0294 File Offset: 0x002AE494
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.PauseGame)
		{
			GameManager.Instance.Pause(true);
		}
	}

	// Token: 0x06006FA8 RID: 28584 RVA: 0x002B02AF File Offset: 0x002AE4AF
	public override void OnClose()
	{
		base.OnOpen();
		if (this.PauseGame)
		{
			GameManager.Instance.Pause(false);
		}
	}

	// Token: 0x04005421 RID: 21537
	[XuiBindComponent("sandboxPresetValues.", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_ScrollView sandboxPresetValuesScroll;

	// Token: 0x04005422 RID: 21538
	[XuiBindParent(false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_ScrollView parentScroll;

	// Token: 0x04005423 RID: 21539
	[XuiBindComponent("btnCopyCode", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCopyCode;

	// Token: 0x04005424 RID: 21540
	[XuiBindComponent("btnSaveAsPreset", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnSaveAsPreset;

	// Token: 0x04005425 RID: 21541
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SandboxOptionManager sandboxManager = SandboxOptionManager.Current;

	// Token: 0x04005426 RID: 21542
	[PublicizedFrom(EAccessModifier.Private)]
	public int sandboxValuesColumns;

	// Token: 0x04005427 RID: 21543
	[PublicizedFrom(EAccessModifier.Private)]
	public string sandBoxValuesTemplate;

	// Token: 0x04005428 RID: 21544
	[PublicizedFrom(EAccessModifier.Private)]
	public string sandBoxValuesTemplateDefaults;

	// Token: 0x0400542A RID: 21546
	[PublicizedFrom(EAccessModifier.Private)]
	public string sandboxCode;

	// Token: 0x0400542B RID: 21547
	[PublicizedFrom(EAccessModifier.Private)]
	public string sandboxName;

	// Token: 0x0400542D RID: 21549
	[PublicizedFrom(EAccessModifier.Private)]
	public bool valid;
}

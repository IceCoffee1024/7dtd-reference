using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000E4E RID: 3662
[Preserve]
public class XUiC_WorldEditorCreateWorld : XUiController
{
	// Token: 0x17000CBD RID: 3261
	// (get) Token: 0x060070B2 RID: 28850 RVA: 0x002B54D7 File Offset: 0x002B36D7
	[XuiXmlBinding("minworldsize")]
	public int MinWorldSize
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return 1024;
		}
	}

	// Token: 0x17000CBE RID: 3262
	// (get) Token: 0x060070B3 RID: 28851 RVA: 0x002B54DE File Offset: 0x002B36DE
	[XuiXmlBinding("maxworldsize")]
	public int MaxWorldSize
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return 16384;
		}
	}

	// Token: 0x17000CBF RID: 3263
	// (get) Token: 0x060070B4 RID: 28852 RVA: 0x002B54E5 File Offset: 0x002B36E5
	[XuiXmlBinding("iscustomsize")]
	public bool IsCustomSize
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_ComboBoxList<int> xuiC_ComboBoxList = this.cmbSize;
			return xuiC_ComboBoxList != null && xuiC_ComboBoxList.Value == -1;
		}
	}

	// Token: 0x17000CC0 RID: 3264
	// (get) Token: 0x060070B5 RID: 28853 RVA: 0x002B54FC File Offset: 0x002B36FC
	public int NewWorldSize
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (!this.IsCustomSize)
			{
				XUiC_ComboBoxList<int> xuiC_ComboBoxList = this.cmbSize;
				if (xuiC_ComboBoxList == null)
				{
					return -1;
				}
				return xuiC_ComboBoxList.Value;
			}
			else
			{
				int num;
				if (this.txtSize != null && int.TryParse(this.txtSize.Text, out num))
				{
					return num;
				}
				return -1;
			}
		}
	}

	// Token: 0x17000CC1 RID: 3265
	// (get) Token: 0x060070B6 RID: 28854 RVA: 0x002B5544 File Offset: 0x002B3744
	[XuiXmlBinding("customsizevalid")]
	public bool CustomSizeValid
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			int num;
			return this.txtSize != null && int.TryParse(this.txtSize.Text, out num) && num >= this.MinWorldSize && num <= this.MaxWorldSize && num % this.MinWorldSize == 0;
		}
	}

	// Token: 0x17000CC2 RID: 3266
	// (get) Token: 0x060070B7 RID: 28855 RVA: 0x002B558C File Offset: 0x002B378C
	[XuiXmlBinding("customnamevalid")]
	public bool CustomNameValid
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.txtName != null && this.txtName.Text.Trim().Length > 0 && !SdDirectory.Exists(GameIO.GetGameDir("Data/Worlds") + "/" + this.txtName.Text);
		}
	}

	// Token: 0x17000CC3 RID: 3267
	// (get) Token: 0x060070B8 RID: 28856 RVA: 0x002B55E2 File Offset: 0x002B37E2
	[XuiXmlBinding("startable")]
	public bool Startable
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CustomNameValid && (!this.IsCustomSize || this.CustomSizeValid);
		}
	}

	// Token: 0x060070B9 RID: 28857 RVA: 0x002B55FE File Offset: 0x002B37FE
	public override void Init()
	{
		base.Init();
		this.txtSize.UIInputController.OnScroll += delegate(XUiController _sender, float _args)
		{
			this.cmbSize.ScrollEvent(_sender, _args);
		};
	}

	// Token: 0x060070BA RID: 28858 RVA: 0x002B5624 File Offset: 0x002B3824
	public override void OnOpen()
	{
		base.OnOpen();
		this.txtName.Text = GamePrefs.GetString(EnumGamePrefs.CreateLevelName);
		int num;
		if (int.TryParse(GamePrefs.GetString(EnumGamePrefs.CreateLevelDim), out num))
		{
			if (this.cmbSize.Elements.Contains(num))
			{
				this.cmbSize.Value = num;
			}
			else
			{
				this.cmbSize.SelectedIndex = 0;
				this.txtSize.Text = num.ToString();
			}
		}
		else
		{
			this.txtSize.Text = "";
		}
		this.IsDirty = true;
	}

	// Token: 0x060070BB RID: 28859 RVA: 0x002B56AF File Offset: 0x002B38AF
	public override void OnClose()
	{
		base.OnClose();
		this.owner = null;
	}

	// Token: 0x060070BC RID: 28860 RVA: 0x002B56C0 File Offset: 0x002B38C0
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
		if (XUiUtils.HotkeysAllowedFor(this.viewComponent ?? this.children[0].ViewComponent, false))
		{
			if (this.xui.playerUI.playerInput.PermanentActions.Cancel.WasReleased)
			{
				this.BtnCancel_OnPressed(this, 0);
			}
			if (this.xui.playerUI.playerInput.GUIActions.Apply.WasReleased)
			{
				this.BtnConfirm_OnPressed(this, 0);
			}
		}
	}

	// Token: 0x060070BD RID: 28861 RVA: 0x0008A715 File Offset: 0x00088915
	[XuiBindEvent("OnChangeHandler", "txtName")]
	[XuiBindEvent("OnChangeHandler", "txtSize")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void TxtInput_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.IsDirty = true;
	}

	// Token: 0x060070BE RID: 28862 RVA: 0x002B5750 File Offset: 0x002B3950
	[XuiBindEvent("OnValueChanged", "cmbSize")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void CmbSize_OnValueChanged(XUiController _sender, int _oldValue, int _newValue)
	{
		if (_newValue < 0 && PlatformManager.NativePlatform.Input.CurrentInputStyle == PlayerInputManager.InputStyle.Keyboard)
		{
			this.txtSize.SetSelected(true, true);
		}
		this.IsDirty = true;
	}

	// Token: 0x060070BF RID: 28863 RVA: 0x00089495 File Offset: 0x00087695
	[XuiBindEvent("OnPress", "btnCancel")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCancel_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x060070C0 RID: 28864 RVA: 0x002B577C File Offset: 0x002B397C
	[XuiBindEvent("OnPress", "btnConfirm")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnConfirm_OnPressed(XUiController _sender, int _mouseButton)
	{
		if (!this.btnConfirm.ViewComponent.Enabled)
		{
			return;
		}
		new GameModeEditWorld().ResetGamePrefs();
		string text = this.txtName.Text.Trim();
		int newWorldSize = this.NewWorldSize;
		GamePrefs.Set(EnumGamePrefs.GameWorld, text);
		GamePrefs.Set(EnumGamePrefs.CreateLevelName, text);
		GamePrefs.Set(EnumGamePrefs.CreateLevelDim, newWorldSize.ToString());
		MicroStopwatch microStopwatch = new MicroStopwatch(true);
		MicroStopwatch microStopwatch2 = new MicroStopwatch(true);
		WorldStaticData.Cleanup(null);
		Log.Out(string.Format("WSD.Cleanup took {0} ms", microStopwatch2.ElapsedMilliseconds));
		microStopwatch2.ResetAndRestart();
		WorldStaticData.Reset(null);
		Log.Out(string.Format("WSD.Reset took {0} ms", microStopwatch2.ElapsedMilliseconds));
		microStopwatch2.ResetAndRestart();
		PathAbstractions.AbstractedLocation abstractedLocation = XUiC_WorldEditor.LocationForNewWorld(text);
		GameUtils.CreateEmptyFlatLevel(text, abstractedLocation, newWorldSize, 60);
		Log.Out(string.Format("Creating empty world took {0} ms", microStopwatch.ElapsedMilliseconds));
		XUiC_WorldEditor xuiC_WorldEditor = this.owner;
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		xuiC_WorldEditor.StartEditor();
	}

	// Token: 0x060070C1 RID: 28865 RVA: 0x002B5888 File Offset: 0x002B3A88
	public static void Open(XUiC_WorldEditor _owner)
	{
		XUiC_WorldEditorCreateWorld childByType = _owner.xui.GetChildByType<XUiC_WorldEditorCreateWorld>();
		childByType.owner = _owner;
		_owner.xui.playerUI.windowManager.Open(childByType.windowGroup, false);
	}

	// Token: 0x04005515 RID: 21781
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WorldEditor owner;

	// Token: 0x04005516 RID: 21782
	[XuiBindComponent("btnConfirm", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnConfirm;

	// Token: 0x04005517 RID: 21783
	[XuiBindComponent("btnCancel", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCancel;

	// Token: 0x04005518 RID: 21784
	[XuiBindComponent("txtName", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtName;

	// Token: 0x04005519 RID: 21785
	[XuiBindComponent("txtSize", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtSize;

	// Token: 0x0400551A RID: 21786
	[XuiBindComponent("cmbSize", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<int> cmbSize;
}

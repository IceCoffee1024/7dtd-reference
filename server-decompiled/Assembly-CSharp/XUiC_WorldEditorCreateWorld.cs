using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000E5D RID: 3677
[Preserve]
public class XUiC_WorldEditorCreateWorld : XUiController
{
	// Token: 0x17000CD8 RID: 3288
	// (get) Token: 0x060070AA RID: 28842 RVA: 0x002B8887 File Offset: 0x002B6A87
	[XuiXmlBinding("minworldsize")]
	public int MinWorldSize
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return 1024;
		}
	}

	// Token: 0x17000CD9 RID: 3289
	// (get) Token: 0x060070AB RID: 28843 RVA: 0x002B888E File Offset: 0x002B6A8E
	[XuiXmlBinding("maxworldsize")]
	public int MaxWorldSize
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return 16384;
		}
	}

	// Token: 0x17000CDA RID: 3290
	// (get) Token: 0x060070AC RID: 28844 RVA: 0x002B8895 File Offset: 0x002B6A95
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

	// Token: 0x17000CDB RID: 3291
	// (get) Token: 0x060070AD RID: 28845 RVA: 0x002B88AC File Offset: 0x002B6AAC
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

	// Token: 0x17000CDC RID: 3292
	// (get) Token: 0x060070AE RID: 28846 RVA: 0x002B88F4 File Offset: 0x002B6AF4
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

	// Token: 0x17000CDD RID: 3293
	// (get) Token: 0x060070AF RID: 28847 RVA: 0x002B893C File Offset: 0x002B6B3C
	[XuiXmlBinding("customnamevalid")]
	public bool CustomNameValid
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.txtName != null && this.txtName.Text.Trim().Length > 0 && !SdDirectory.Exists(GameIO.GetGameDir("Data/Worlds") + "/" + this.txtName.Text);
		}
	}

	// Token: 0x17000CDE RID: 3294
	// (get) Token: 0x060070B0 RID: 28848 RVA: 0x002B8992 File Offset: 0x002B6B92
	[XuiXmlBinding("startable")]
	public bool Startable
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CustomNameValid && (!this.IsCustomSize || this.CustomSizeValid);
		}
	}

	// Token: 0x060070B1 RID: 28849 RVA: 0x002B89AE File Offset: 0x002B6BAE
	public override void Init()
	{
		base.Init();
		this.txtSize.UIInputController.OnScroll += delegate(XUiController _sender, float _args)
		{
			this.cmbSize.ScrollEvent(_sender, _args);
		};
	}

	// Token: 0x060070B2 RID: 28850 RVA: 0x002B89D4 File Offset: 0x002B6BD4
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

	// Token: 0x060070B3 RID: 28851 RVA: 0x002B8A5F File Offset: 0x002B6C5F
	public override void OnClose()
	{
		base.OnClose();
		this.owner = null;
	}

	// Token: 0x060070B4 RID: 28852 RVA: 0x002B8A70 File Offset: 0x002B6C70
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

	// Token: 0x060070B5 RID: 28853 RVA: 0x0008A995 File Offset: 0x00088B95
	[XuiBindEvent("OnChangeHandler", "txtName")]
	[XuiBindEvent("OnChangeHandler", "txtSize")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void TxtInput_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.IsDirty = true;
	}

	// Token: 0x060070B6 RID: 28854 RVA: 0x002B8B00 File Offset: 0x002B6D00
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

	// Token: 0x060070B7 RID: 28855 RVA: 0x00089715 File Offset: 0x00087915
	[XuiBindEvent("OnPress", "btnCancel")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCancel_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x060070B8 RID: 28856 RVA: 0x002B8B2C File Offset: 0x002B6D2C
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

	// Token: 0x060070B9 RID: 28857 RVA: 0x002B8C38 File Offset: 0x002B6E38
	public static void Open(XUiC_WorldEditor _owner)
	{
		XUiC_WorldEditorCreateWorld childByType = _owner.xui.GetChildByType<XUiC_WorldEditorCreateWorld>();
		childByType.owner = _owner;
		_owner.xui.playerUI.windowManager.Open(childByType.windowGroup, false);
	}

	// Token: 0x0400554C RID: 21836
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WorldEditor owner;

	// Token: 0x0400554D RID: 21837
	[XuiBindComponent("btnConfirm", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnConfirm;

	// Token: 0x0400554E RID: 21838
	[XuiBindComponent("btnCancel", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCancel;

	// Token: 0x0400554F RID: 21839
	[XuiBindComponent("txtName", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtName;

	// Token: 0x04005550 RID: 21840
	[XuiBindComponent("txtSize", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtSize;

	// Token: 0x04005551 RID: 21841
	[XuiBindComponent("cmbSize", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<int> cmbSize;
}

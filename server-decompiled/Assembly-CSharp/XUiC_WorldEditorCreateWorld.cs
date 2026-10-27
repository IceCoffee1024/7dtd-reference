using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000E68 RID: 3688
[Preserve]
public class XUiC_WorldEditorCreateWorld : XUiController
{
	// Token: 0x17000CDE RID: 3294
	// (get) Token: 0x060070CF RID: 28879 RVA: 0x002B9DF3 File Offset: 0x002B7FF3
	[XuiXmlBinding("minworldsize")]
	public int MinWorldSize
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return 1024;
		}
	}

	// Token: 0x17000CDF RID: 3295
	// (get) Token: 0x060070D0 RID: 28880 RVA: 0x002B9DFA File Offset: 0x002B7FFA
	[XuiXmlBinding("maxworldsize")]
	public int MaxWorldSize
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return 16384;
		}
	}

	// Token: 0x17000CE0 RID: 3296
	// (get) Token: 0x060070D1 RID: 28881 RVA: 0x002B9E01 File Offset: 0x002B8001
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

	// Token: 0x17000CE1 RID: 3297
	// (get) Token: 0x060070D2 RID: 28882 RVA: 0x002B9E18 File Offset: 0x002B8018
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

	// Token: 0x17000CE2 RID: 3298
	// (get) Token: 0x060070D3 RID: 28883 RVA: 0x002B9E60 File Offset: 0x002B8060
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

	// Token: 0x17000CE3 RID: 3299
	// (get) Token: 0x060070D4 RID: 28884 RVA: 0x002B9EA8 File Offset: 0x002B80A8
	[XuiXmlBinding("customnamevalid")]
	public bool CustomNameValid
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.txtName != null && this.txtName.Text.Trim().Length > 0 && !SdDirectory.Exists(GameIO.GetGameDir("Data/Worlds") + "/" + this.txtName.Text);
		}
	}

	// Token: 0x17000CE4 RID: 3300
	// (get) Token: 0x060070D5 RID: 28885 RVA: 0x002B9EFE File Offset: 0x002B80FE
	[XuiXmlBinding("startable")]
	public bool Startable
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CustomNameValid && (!this.IsCustomSize || this.CustomSizeValid);
		}
	}

	// Token: 0x060070D6 RID: 28886 RVA: 0x002B9F1A File Offset: 0x002B811A
	public override void Init()
	{
		base.Init();
		this.txtSize.UIInputController.OnScroll += delegate(XUiController _sender, float _args)
		{
			this.cmbSize.ScrollEvent(_sender, _args);
		};
	}

	// Token: 0x060070D7 RID: 28887 RVA: 0x002B9F40 File Offset: 0x002B8140
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

	// Token: 0x060070D8 RID: 28888 RVA: 0x002B9FCB File Offset: 0x002B81CB
	public override void OnClose()
	{
		base.OnClose();
		this.owner = null;
	}

	// Token: 0x060070D9 RID: 28889 RVA: 0x002B9FDC File Offset: 0x002B81DC
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

	// Token: 0x060070DA RID: 28890 RVA: 0x0008ABC9 File Offset: 0x00088DC9
	[XuiBindEvent("OnChangeHandler", "txtName")]
	[XuiBindEvent("OnChangeHandler", "txtSize")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void TxtInput_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.IsDirty = true;
	}

	// Token: 0x060070DB RID: 28891 RVA: 0x002BA06C File Offset: 0x002B826C
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

	// Token: 0x060070DC RID: 28892 RVA: 0x00089949 File Offset: 0x00087B49
	[XuiBindEvent("OnPress", "btnCancel")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCancel_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x060070DD RID: 28893 RVA: 0x002BA098 File Offset: 0x002B8298
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

	// Token: 0x060070DE RID: 28894 RVA: 0x002BA1A4 File Offset: 0x002B83A4
	public static void Open(XUiC_WorldEditor _owner)
	{
		XUiC_WorldEditorCreateWorld childByType = _owner.xui.GetChildByType<XUiC_WorldEditorCreateWorld>();
		childByType.owner = _owner;
		_owner.xui.playerUI.windowManager.Open(childByType.windowGroup, false);
	}

	// Token: 0x04005571 RID: 21873
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WorldEditor owner;

	// Token: 0x04005572 RID: 21874
	[XuiBindComponent("btnConfirm", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnConfirm;

	// Token: 0x04005573 RID: 21875
	[XuiBindComponent("btnCancel", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCancel;

	// Token: 0x04005574 RID: 21876
	[XuiBindComponent("txtName", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtName;

	// Token: 0x04005575 RID: 21877
	[XuiBindComponent("txtSize", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtSize;

	// Token: 0x04005576 RID: 21878
	[XuiBindComponent("cmbSize", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<int> cmbSize;
}

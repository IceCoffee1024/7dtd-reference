using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000E39 RID: 3641
[Preserve]
public class XUiC_WorldEditorCreateWorld : XUiController
{
	// Token: 0x17000CA4 RID: 3236
	// (get) Token: 0x06007012 RID: 28690 RVA: 0x002B1FB3 File Offset: 0x002B01B3
	[XuiXmlBinding("minworldsize")]
	public int MinWorldSize
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return 1024;
		}
	}

	// Token: 0x17000CA5 RID: 3237
	// (get) Token: 0x06007013 RID: 28691 RVA: 0x002B1FBA File Offset: 0x002B01BA
	[XuiXmlBinding("maxworldsize")]
	public int MaxWorldSize
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return 16384;
		}
	}

	// Token: 0x17000CA6 RID: 3238
	// (get) Token: 0x06007014 RID: 28692 RVA: 0x002B1FC1 File Offset: 0x002B01C1
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

	// Token: 0x17000CA7 RID: 3239
	// (get) Token: 0x06007015 RID: 28693 RVA: 0x002B1FD8 File Offset: 0x002B01D8
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

	// Token: 0x17000CA8 RID: 3240
	// (get) Token: 0x06007016 RID: 28694 RVA: 0x002B2020 File Offset: 0x002B0220
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

	// Token: 0x17000CA9 RID: 3241
	// (get) Token: 0x06007017 RID: 28695 RVA: 0x002B2068 File Offset: 0x002B0268
	[XuiXmlBinding("customnamevalid")]
	public bool CustomNameValid
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.txtName != null && this.txtName.Text.Trim().Length > 0 && !SdDirectory.Exists(GameIO.GetGameDir("Data/Worlds") + "/" + this.txtName.Text);
		}
	}

	// Token: 0x17000CAA RID: 3242
	// (get) Token: 0x06007018 RID: 28696 RVA: 0x002B20BE File Offset: 0x002B02BE
	[XuiXmlBinding("startable")]
	public bool Startable
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CustomNameValid && (!this.IsCustomSize || this.CustomSizeValid);
		}
	}

	// Token: 0x06007019 RID: 28697 RVA: 0x002B20DA File Offset: 0x002B02DA
	public override void Init()
	{
		base.Init();
		this.txtSize.UIInputController.OnScroll += delegate(XUiController _sender, float _args)
		{
			this.cmbSize.ScrollEvent(_sender, _args);
		};
	}

	// Token: 0x0600701A RID: 28698 RVA: 0x002B2100 File Offset: 0x002B0300
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

	// Token: 0x0600701B RID: 28699 RVA: 0x002B218B File Offset: 0x002B038B
	public override void OnClose()
	{
		base.OnClose();
		this.owner = null;
	}

	// Token: 0x0600701C RID: 28700 RVA: 0x002B219C File Offset: 0x002B039C
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

	// Token: 0x0600701D RID: 28701 RVA: 0x0008A069 File Offset: 0x00088269
	[XuiBindEvent("OnChangeHandler", "txtName")]
	[XuiBindEvent("OnChangeHandler", "txtSize")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void TxtInput_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.IsDirty = true;
	}

	// Token: 0x0600701E RID: 28702 RVA: 0x002B222C File Offset: 0x002B042C
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

	// Token: 0x0600701F RID: 28703 RVA: 0x00088DF5 File Offset: 0x00086FF5
	[XuiBindEvent("OnPress", "btnCancel")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCancel_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x06007020 RID: 28704 RVA: 0x002B2258 File Offset: 0x002B0458
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

	// Token: 0x06007021 RID: 28705 RVA: 0x002B2364 File Offset: 0x002B0564
	public static void Open(XUiC_WorldEditor _owner)
	{
		XUiC_WorldEditorCreateWorld childByType = _owner.xui.GetChildByType<XUiC_WorldEditorCreateWorld>();
		childByType.owner = _owner;
		_owner.xui.playerUI.windowManager.Open(childByType.windowGroup, false);
	}

	// Token: 0x04005471 RID: 21617
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WorldEditor owner;

	// Token: 0x04005472 RID: 21618
	[XuiBindComponent("btnConfirm", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnConfirm;

	// Token: 0x04005473 RID: 21619
	[XuiBindComponent("btnCancel", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCancel;

	// Token: 0x04005474 RID: 21620
	[XuiBindComponent("txtName", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtName;

	// Token: 0x04005475 RID: 21621
	[XuiBindComponent("txtSize", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtSize;

	// Token: 0x04005476 RID: 21622
	[XuiBindComponent("cmbSize", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<int> cmbSize;
}

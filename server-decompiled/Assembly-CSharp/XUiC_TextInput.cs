using System;
using System.Collections;
using System.Collections.Generic;
using InControl;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000EB2 RID: 3762
[global::UnityEngine.Scripting.Preserve]
public class XUiC_TextInput : XUiController
{
	// Token: 0x140000B7 RID: 183
	// (add) Token: 0x06007493 RID: 29843 RVA: 0x002C38CC File Offset: 0x002C1ACC
	// (remove) Token: 0x06007494 RID: 29844 RVA: 0x002C3904 File Offset: 0x002C1B04
	public event XUiEvent_InputOnSubmitEventHandler OnSubmitHandler;

	// Token: 0x140000B8 RID: 184
	// (add) Token: 0x06007495 RID: 29845 RVA: 0x002C393C File Offset: 0x002C1B3C
	// (remove) Token: 0x06007496 RID: 29846 RVA: 0x002C3974 File Offset: 0x002C1B74
	public event XUiEvent_InputOnChangedEventHandler OnChangeHandler;

	// Token: 0x140000B9 RID: 185
	// (add) Token: 0x06007497 RID: 29847 RVA: 0x002C39AC File Offset: 0x002C1BAC
	// (remove) Token: 0x06007498 RID: 29848 RVA: 0x002C39E4 File Offset: 0x002C1BE4
	public event XUiEvent_InputOnAbortedEventHandler OnInputAbortedHandler;

	// Token: 0x140000BA RID: 186
	// (add) Token: 0x06007499 RID: 29849 RVA: 0x002C3A1C File Offset: 0x002C1C1C
	// (remove) Token: 0x0600749A RID: 29850 RVA: 0x002C3A54 File Offset: 0x002C1C54
	public event XUiEvent_InputOnSelectedEventHandler OnInputSelectedHandler;

	// Token: 0x140000BB RID: 187
	// (add) Token: 0x0600749B RID: 29851 RVA: 0x002C3A8C File Offset: 0x002C1C8C
	// (remove) Token: 0x0600749C RID: 29852 RVA: 0x002C3AC4 File Offset: 0x002C1CC4
	public event XUiEvent_InputOnErrorEventHandler OnInputErrorHandler;

	// Token: 0x140000BC RID: 188
	// (add) Token: 0x0600749D RID: 29853 RVA: 0x002C3AFC File Offset: 0x002C1CFC
	// (remove) Token: 0x0600749E RID: 29854 RVA: 0x002C3B34 File Offset: 0x002C1D34
	public event UIInput.OnClipboard OnClipboardHandler;

	// Token: 0x17000DC2 RID: 3522
	// (get) Token: 0x0600749F RID: 29855 RVA: 0x002C3B69 File Offset: 0x002C1D69
	// (set) Token: 0x060074A0 RID: 29856 RVA: 0x002C3B71 File Offset: 0x002C1D71
	[XuiXmlAttribute("value", false)]
	public string InitialValue
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000DC3 RID: 3523
	// (get) Token: 0x060074A1 RID: 29857 RVA: 0x002C3B7A File Offset: 0x002C1D7A
	// (set) Token: 0x060074A2 RID: 29858 RVA: 0x002C3B82 File Offset: 0x002C1D82
	[XuiXmlAttribute("caret_color", false)]
	public Color CaretColor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000DC4 RID: 3524
	// (get) Token: 0x060074A3 RID: 29859 RVA: 0x002C3B8B File Offset: 0x002C1D8B
	// (set) Token: 0x060074A4 RID: 29860 RVA: 0x002C3B93 File Offset: 0x002C1D93
	[XuiXmlAttribute("selection_color", false)]
	public Color SelectionColor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000DC5 RID: 3525
	// (get) Token: 0x060074A5 RID: 29861 RVA: 0x002C3B9C File Offset: 0x002C1D9C
	// (set) Token: 0x060074A6 RID: 29862 RVA: 0x002C3BA4 File Offset: 0x002C1DA4
	[XuiXmlAttribute("validation", false)]
	public UIInput.Validation Validation
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000DC6 RID: 3526
	// (get) Token: 0x060074A7 RID: 29863 RVA: 0x002C3BAD File Offset: 0x002C1DAD
	// (set) Token: 0x060074A8 RID: 29864 RVA: 0x002C3BB5 File Offset: 0x002C1DB5
	[XuiXmlAttribute("hide_input", false)]
	public bool HideInput
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000DC7 RID: 3527
	// (get) Token: 0x060074A9 RID: 29865 RVA: 0x002C3BBE File Offset: 0x002C1DBE
	// (set) Token: 0x060074AA RID: 29866 RVA: 0x002C3BC6 File Offset: 0x002C1DC6
	[XuiXmlAttribute("on_return", false)]
	public UIInput.OnReturnKey OnReturnKey
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = UIInput.OnReturnKey.Submit;

	// Token: 0x17000DC8 RID: 3528
	// (get) Token: 0x060074AB RID: 29867 RVA: 0x002C3BCF File Offset: 0x002C1DCF
	// (set) Token: 0x060074AC RID: 29868 RVA: 0x002C3BD7 File Offset: 0x002C1DD7
	[XuiXmlAttribute("character_limit", false)]
	public int CharacterLimit
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000DC9 RID: 3529
	// (get) Token: 0x060074AD RID: 29869 RVA: 0x002C3BE0 File Offset: 0x002C1DE0
	// (set) Token: 0x060074AE RID: 29870 RVA: 0x002C3BE8 File Offset: 0x002C1DE8
	[XuiXmlAttribute("input_type", false)]
	public UIInput.KeyboardType InputType
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000DCA RID: 3530
	// (get) Token: 0x060074AF RID: 29871 RVA: 0x002C3BF1 File Offset: 0x002C1DF1
	// (set) Token: 0x060074B0 RID: 29872 RVA: 0x002C3BF9 File Offset: 0x002C1DF9
	[XuiXmlAttribute("ignore_up_down_keys", false)]
	public bool IgnoreUpDownKeys
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000DCB RID: 3531
	// (get) Token: 0x060074B1 RID: 29873 RVA: 0x002C3C02 File Offset: 0x002C1E02
	// (set) Token: 0x060074B2 RID: 29874 RVA: 0x002C3C0A File Offset: 0x002C1E0A
	[XuiXmlAttribute("search_field", false)]
	public bool IsSearchField
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000DCC RID: 3532
	// (get) Token: 0x060074B3 RID: 29875 RVA: 0x002C3C13 File Offset: 0x002C1E13
	// (set) Token: 0x060074B4 RID: 29876 RVA: 0x002C3C1B File Offset: 0x002C1E1B
	[XuiXmlAttribute("clear_button", false)]
	[XuiXmlBinding("clearbutton")]
	public bool HasClearButton
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000DCD RID: 3533
	// (get) Token: 0x060074B5 RID: 29877 RVA: 0x002C3C24 File Offset: 0x002C1E24
	// (set) Token: 0x060074B6 RID: 29878 RVA: 0x002C3C2C File Offset: 0x002C1E2C
	[XuiXmlAttribute("password_field", false)]
	[XuiXmlBinding("passwordfield")]
	public bool IsPasswordField
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000DCE RID: 3534
	// (get) Token: 0x060074B7 RID: 29879 RVA: 0x002C3C35 File Offset: 0x002C1E35
	// (set) Token: 0x060074B8 RID: 29880 RVA: 0x002C3C3D File Offset: 0x002C1E3D
	[XuiXmlAttribute("focus_on_open", false)]
	public bool FocusOnOpen
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000DCF RID: 3535
	// (get) Token: 0x060074B9 RID: 29881 RVA: 0x002C3C46 File Offset: 0x002C1E46
	// (set) Token: 0x060074BA RID: 29882 RVA: 0x002C3C4E File Offset: 0x002C1E4E
	[XuiXmlAttribute("open_vk_on_open", false)]
	public bool OpenVkOnOpen
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000DD0 RID: 3536
	// (get) Token: 0x060074BB RID: 29883 RVA: 0x002C3C57 File Offset: 0x002C1E57
	// (set) Token: 0x060074BC RID: 29884 RVA: 0x002C3C5F File Offset: 0x002C1E5F
	[XuiXmlAttribute("clear_on_open", false)]
	public bool ClearOnOpen
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000DD1 RID: 3537
	// (get) Token: 0x060074BD RID: 29885 RVA: 0x002C3C68 File Offset: 0x002C1E68
	// (set) Token: 0x060074BE RID: 29886 RVA: 0x002C3C70 File Offset: 0x002C1E70
	[XuiXmlAttribute("vk_propagates_events", false)]
	public bool VkPropagatesEvents
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = true;

	// Token: 0x17000DD2 RID: 3538
	// (get) Token: 0x060074BF RID: 29887 RVA: 0x002C3C79 File Offset: 0x002C1E79
	// (set) Token: 0x060074C0 RID: 29888 RVA: 0x002C3C81 File Offset: 0x002C1E81
	[XuiXmlAttribute("close_group_on_tab", false)]
	public bool CloseGroupOnTab
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x060074C1 RID: 29889 RVA: 0x002C3C8A File Offset: 0x002C1E8A
	public static void SelectCurrentSearchField(LocalPlayerUI _playerUi)
	{
		if (XUiC_TextInput.currentSearchField != null && !_playerUi.windowManager.IsInputActive() && XUiC_TextInput.currentSearchField.viewComponent.IsActiveInHierarchy)
		{
			XUiC_TextInput.currentSearchField.SetSelected(true, false);
		}
	}

	// Token: 0x17000DD3 RID: 3539
	// (get) Token: 0x060074C2 RID: 29890 RVA: 0x002C3CBD File Offset: 0x002C1EBD
	public UIInput UIInput
	{
		get
		{
			return this.uiInput;
		}
	}

	// Token: 0x17000DD4 RID: 3540
	// (get) Token: 0x060074C3 RID: 29891 RVA: 0x002C3CC5 File Offset: 0x002C1EC5
	public XUiController UIInputController
	{
		get
		{
			return this.uiInputController;
		}
	}

	// Token: 0x17000DD5 RID: 3541
	// (get) Token: 0x060074C4 RID: 29892 RVA: 0x002C3CCD File Offset: 0x002C1ECD
	// (set) Token: 0x060074C5 RID: 29893 RVA: 0x002C3CEA File Offset: 0x002C1EEA
	public string Text
	{
		get
		{
			UIInput uiinput = this.uiInput;
			return ((uiinput != null) ? uiinput.value : null) ?? "";
		}
		set
		{
			this.textChangeFromCode = true;
			this.uiInput.value = value;
			this.textChangeFromCode = false;
			this.uiInput.UpdateLabel();
		}
	}

	// Token: 0x17000DD6 RID: 3542
	// (get) Token: 0x060074C6 RID: 29894 RVA: 0x002C3D11 File Offset: 0x002C1F11
	// (set) Token: 0x060074C7 RID: 29895 RVA: 0x002C3D1C File Offset: 0x002C1F1C
	public XUiC_TextInput SelectOnTab
	{
		get
		{
			return this.selectOnTab;
		}
		set
		{
			if (this.selectOnTab == value)
			{
				return;
			}
			this.selectOnTab = value;
			if (value != null)
			{
				UIKeyNavigation uikeyNavigation = this.uiInputController.ViewComponent.UiTransform.gameObject.AddMissingComponent<UIKeyNavigation>();
				uikeyNavigation.constraint = UIKeyNavigation.Constraint.Explicit;
				uikeyNavigation.onTab = value.uiInput.gameObject;
				return;
			}
			UIKeyNavigation component = this.uiInputController.ViewComponent.UiTransform.gameObject.GetComponent<UIKeyNavigation>();
			if (component != null)
			{
				global::UnityEngine.Object.Destroy(component);
			}
		}
	}

	// Token: 0x17000DD7 RID: 3543
	// (get) Token: 0x060074C8 RID: 29896 RVA: 0x002C3D99 File Offset: 0x002C1F99
	// (set) Token: 0x060074C9 RID: 29897 RVA: 0x002C3DA1 File Offset: 0x002C1FA1
	[XuiXmlAttribute("active_text_color", false)]
	public Color ActiveTextColor
	{
		get
		{
			return this.activeTextColor;
		}
		set
		{
			if (value == this.activeTextColor)
			{
				return;
			}
			this.activeTextColor = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000DD8 RID: 3544
	// (get) Token: 0x060074CA RID: 29898 RVA: 0x002C3DC0 File Offset: 0x002C1FC0
	// (set) Token: 0x060074CB RID: 29899 RVA: 0x002C3DC8 File Offset: 0x002C1FC8
	[XuiXmlAttribute("input_enabled", false)]
	public bool Enabled
	{
		get
		{
			return this.enabled;
		}
		set
		{
			if (value == this.enabled)
			{
				return;
			}
			this.enabled = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000DD9 RID: 3545
	// (get) Token: 0x060074CC RID: 29900 RVA: 0x002C3DE2 File Offset: 0x002C1FE2
	// (set) Token: 0x060074CD RID: 29901 RVA: 0x002C3DEF File Offset: 0x002C1FEF
	public bool SupportBbCode
	{
		get
		{
			return this.label.SupportBbCode;
		}
		set
		{
			this.label.SupportBbCode = value;
		}
	}

	// Token: 0x17000DDA RID: 3546
	// (get) Token: 0x060074CE RID: 29902 RVA: 0x002C3DFD File Offset: 0x002C1FFD
	public bool IsSelected
	{
		get
		{
			return this.uiInput.isSelected;
		}
	}

	// Token: 0x17000DDB RID: 3547
	// (get) Token: 0x060074CF RID: 29903 RVA: 0x002C3E0A File Offset: 0x002C200A
	[XuiXmlBinding("showpassword")]
	public bool ShowPassword
	{
		get
		{
			return this.displayType == UIInput.InputType.Standard;
		}
	}

	// Token: 0x060074D0 RID: 29904 RVA: 0x002C3E15 File Offset: 0x002C2015
	[XuiXmlAttribute("virtual_keyboard_prompt", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeVirtualKeyboardPrompt(string _value)
	{
		this.virtKeyboardPrompt = Localization.Get(_value, false, null);
	}

	// Token: 0x17000DDC RID: 3548
	// (get) Token: 0x060074D1 RID: 29905 RVA: 0x002C3E25 File Offset: 0x002C2025
	[XuiXmlBinding("txt_empty")]
	public bool IsEmpty
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.Text.Length == 0;
		}
	}

	// Token: 0x060074D2 RID: 29906 RVA: 0x002C3E35 File Offset: 0x002C2035
	[XuiXmlAttribute("select_on_tab", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeSelectOnTab(string _value)
	{
		if (string.IsNullOrEmpty(_value))
		{
			return;
		}
		this.selectOnTabString = _value;
		this.selectOnTabSetFromXML = true;
		this.IsDirty = true;
	}

	// Token: 0x060074D3 RID: 29907 RVA: 0x002C3E58 File Offset: 0x002C2058
	public override void Init()
	{
		base.Init();
		this.uiInputController = this.label.Controller;
		this.uiInput = this.uiInputController.ViewComponent.UiTransform.gameObject.AddComponent<UIInput>();
		if (this.IgnoreUpDownKeys)
		{
			this.uiInput.onDownArrow = delegate
			{
			};
			this.uiInput.onUpArrow = delegate
			{
			};
		}
		XUiController childById = base.GetChildById("btnShowPassword");
		if (childById != null)
		{
			childById.OnPress += this.BtnShowPassword_OnPress;
		}
		XUiController childById2 = base.GetChildById("btnClearInput");
		if (childById2 != null)
		{
			childById2.OnPress += this.BtnClearInput_OnPress;
		}
		EventDelegate.Add(this.uiInput.onSubmit, new EventDelegate.Callback(this.OnSubmit));
		EventDelegate.Add(this.uiInput.onChange, new EventDelegate.Callback(this.OnChange));
		this.uiInput.onClipboard += this.OnClipboard;
		this.label.BindToUiInput(this.uiInput);
		this.uiInput.value = this.InitialValue ?? "";
		this.uiInput.activeTextColor = this.activeTextColor;
		this.uiInput.caretColor = this.CaretColor;
		this.uiInput.selectionColor = this.SelectionColor;
		this.uiInput.inputType = this.displayType;
		this.uiInput.validation = this.Validation;
		this.uiInput.hideInput = this.HideInput;
		this.uiInput.onReturnKey = this.OnReturnKey;
		this.uiInput.characterLimit = this.CharacterLimit;
		this.uiInput.keyboardType = this.InputType;
		this.uiInputController.OnSelect += this.inputFieldSelected;
	}

	// Token: 0x060074D4 RID: 29908 RVA: 0x002C4068 File Offset: 0x002C2268
	[XuiBindEvent("OnPress", "scrollArea")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void ScrollArea_OnPress(XUiController _sender, int _mouseButton)
	{
		this.SelectOrVirtualKeyboard(true);
	}

	// Token: 0x060074D5 RID: 29909 RVA: 0x002C4071 File Offset: 0x002C2271
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnShowPassword_OnPress(XUiController _sender, int _mouseButton)
	{
		this.displayType = ((this.displayType == UIInput.InputType.Password) ? UIInput.InputType.Standard : UIInput.InputType.Password);
		this.IsDirty = true;
	}

	// Token: 0x060074D6 RID: 29910 RVA: 0x002C408D File Offset: 0x002C228D
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnClearInput_OnPress(XUiController _sender, int _mouseButton)
	{
		this.Text = "";
		this.IsDirty = true;
	}

	// Token: 0x060074D7 RID: 29911 RVA: 0x002C40A4 File Offset: 0x002C22A4
	[PublicizedFrom(EAccessModifier.Private)]
	public void showVirtualKeyboard()
	{
		if (!this.Enabled)
		{
			return;
		}
		IVirtualKeyboard virtualKeyboard = PlatformManager.NativePlatform.VirtualKeyboard;
		string text;
		if (virtualKeyboard == null)
		{
			text = Localization.Get("ttPlatformHasNoVirtualKeyboard", false, null);
		}
		else
		{
			uint num = (uint)((this.CharacterLimit <= 0) ? 200 : this.CharacterLimit);
			text = virtualKeyboard.Open(this.virtKeyboardPrompt, this.uiInput.value, new Action<bool, string>(this.OnTextReceived), this.displayType, this.OnReturnKey == UIInput.OnReturnKey.NewLine, num);
		}
		if (text == null)
		{
			return;
		}
		GameManager instance = GameManager.Instance;
		EntityPlayerLocal entityPlayerLocal;
		if (instance == null)
		{
			entityPlayerLocal = null;
		}
		else
		{
			World world = instance.World;
			entityPlayerLocal = ((world != null) ? world.GetPrimaryPlayer() : null);
		}
		GameManager.ShowTooltip(entityPlayerLocal, "[BB0000]" + text, false, false, 0f);
		XUiEvent_InputOnErrorEventHandler onInputErrorHandler = this.OnInputErrorHandler;
		if (onInputErrorHandler == null)
		{
			return;
		}
		onInputErrorHandler(this, text);
	}

	// Token: 0x060074D8 RID: 29912 RVA: 0x002C416C File Offset: 0x002C236C
	[PublicizedFrom(EAccessModifier.Private)]
	public void inputFieldSelected(XUiController _sender, bool _selected)
	{
		if (_selected)
		{
			ThreadManager.StartCoroutine(this.removeSelection());
			return;
		}
		XUiEvent_InputOnSelectedEventHandler onInputSelectedHandler = this.OnInputSelectedHandler;
		if (onInputSelectedHandler == null)
		{
			return;
		}
		onInputSelectedHandler(this, false);
	}

	// Token: 0x060074D9 RID: 29913 RVA: 0x002C4190 File Offset: 0x002C2390
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator removeSelection()
	{
		yield return null;
		if (PlatformManager.NativePlatform.Input.CurrentInputStyle != PlayerInputManager.InputStyle.Keyboard)
		{
			this.SetSelected(false, false);
			this.showVirtualKeyboard();
		}
		XUiEvent_InputOnSelectedEventHandler onInputSelectedHandler = this.OnInputSelectedHandler;
		if (onInputSelectedHandler != null)
		{
			onInputSelectedHandler(this, true);
		}
		yield break;
	}

	// Token: 0x060074DA RID: 29914 RVA: 0x002C41A0 File Offset: 0x002C23A0
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnTextReceived(bool _success, string _text)
	{
		if (!this.isOpen)
		{
			return;
		}
		this.Text = _text;
		if (this.VkPropagatesEvents)
		{
			if (_success)
			{
				if (this.OnSubmitHandler != null)
				{
					this.OnSubmitHandler(this, _text);
				}
				else if (this.OnChangeHandler != null)
				{
					this.OnChangeHandler(this, _text, false);
				}
			}
			else if (this.OnInputAbortedHandler != null)
			{
				this.OnInputAbortedHandler(this);
			}
		}
		this.uiInput.RemoveFocus();
	}

	// Token: 0x060074DB RID: 29915 RVA: 0x002C4218 File Offset: 0x002C2418
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnSubmit()
	{
		string value = UIInput.current.value;
		this.uiInput.RemoveFocus();
		if (this.OnSubmitHandler != null)
		{
			ThreadManager.StartCoroutine(this.delaySubmitHandler(value));
		}
	}

	// Token: 0x060074DC RID: 29916 RVA: 0x002C4250 File Offset: 0x002C2450
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator delaySubmitHandler(string _input)
	{
		yield return null;
		XUiEvent_InputOnSubmitEventHandler onSubmitHandler = this.OnSubmitHandler;
		if (onSubmitHandler != null)
		{
			onSubmitHandler(this, _input);
		}
		yield break;
	}

	// Token: 0x060074DD RID: 29917 RVA: 0x002C4266 File Offset: 0x002C2466
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnChange()
	{
		if (this.windowGroup.isShowing)
		{
			this.updateScrollPositionToCaret();
		}
		this.IsDirty = true;
		XUiEvent_InputOnChangedEventHandler onChangeHandler = this.OnChangeHandler;
		if (onChangeHandler == null)
		{
			return;
		}
		onChangeHandler(this, UIInput.current.value, this.textChangeFromCode);
	}

	// Token: 0x060074DE RID: 29918 RVA: 0x002C42A4 File Offset: 0x002C24A4
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateScrollPositionToCaret()
	{
		if (this.scrollView == null)
		{
			return;
		}
		UITexture caret = this.uiInput.caret;
		if (!(caret == null) && caret.enabled)
		{
			List<Vector3> verts = caret.geometry.verts;
			for (int i = 0; i < verts.Count; i++)
			{
				if (i >= this.caretWorldCorners.Count)
				{
					this.caretWorldCorners.Add(Vector3.zero);
				}
				this.caretWorldCorners[i] = caret.transform.TransformPoint(verts[i]);
			}
			this.scrollView.MakeVisible(this.caretWorldCorners);
			return;
		}
		XUiV_ScrollView xuiV_ScrollView = this.scrollView;
		if (xuiV_ScrollView == null)
		{
			return;
		}
		xuiV_ScrollView.UpdatePosition();
	}

	// Token: 0x060074DF RID: 29919 RVA: 0x002C4352 File Offset: 0x002C2552
	public void TriggerOnChangeHandler(bool _changeFromCode = false)
	{
		XUiEvent_InputOnChangedEventHandler onChangeHandler = this.OnChangeHandler;
		if (onChangeHandler == null)
		{
			return;
		}
		onChangeHandler(this, this.uiInput.value, _changeFromCode);
	}

	// Token: 0x060074E0 RID: 29920 RVA: 0x002C4371 File Offset: 0x002C2571
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnClipboard(UIInput.ClipboardAction _actionType, string _oldText, int _selStart, int _selEnd, string _actionResultText)
	{
		UIInput.OnClipboard onClipboardHandler = this.OnClipboardHandler;
		if (onClipboardHandler == null)
		{
			return;
		}
		onClipboardHandler(_actionType, _oldText, _selStart, _selEnd, _actionResultText);
	}

	// Token: 0x060074E1 RID: 29921 RVA: 0x002C438C File Offset: 0x002C258C
	public override void OnOpen()
	{
		base.OnOpen();
		this.label.Text = this.uiInput.value;
		this.IsDirty = true;
		this.isOpen = true;
		this.openCompleted = false;
		this.removeFocusOnOpen = true;
		if (this.IsPasswordField)
		{
			this.displayType = UIInput.InputType.Password;
			this.uiInput.UpdateLabel();
		}
		if (this.IsSearchField)
		{
			XUiC_TextInput.currentSearchField = this;
		}
		if (this.ClearOnOpen)
		{
			this.Text = "";
		}
		if (this.FocusOnOpen)
		{
			this.SetSelected(true, true);
		}
		if (this.OpenVkOnOpen)
		{
			this.SelectOrVirtualKeyboard(true);
		}
	}

	// Token: 0x060074E2 RID: 29922 RVA: 0x002C442B File Offset: 0x002C262B
	public override void OnClose()
	{
		this.SetSelected(false, false);
		base.OnClose();
		this.isOpen = false;
		if (XUiC_TextInput.currentSearchField == this)
		{
			XUiC_TextInput.currentSearchField = null;
		}
	}

	// Token: 0x060074E3 RID: 29923 RVA: 0x002C4450 File Offset: 0x002C2650
	[XuiBindEvent("OnVisiblity", null)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void visibilityChanged(XUiController _sender, bool _visibleSelf, bool _visibleInScene)
	{
		if (!_visibleInScene)
		{
			this.SetSelected(false, false);
			GameObject selectedObject = UICamera.selectedObject;
		}
	}

	// Token: 0x060074E4 RID: 29924 RVA: 0x002C4464 File Offset: 0x002C2664
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.xui.playerUI.CursorController.GetMouseButton(UICamera.MouseButton.LeftButton) && this.uiInput.isSelected && !this.uiInputController.ViewComponent.UiTransformIsHovered)
		{
			this.uiInput.isSelected = false;
		}
		if (this.CloseGroupOnTab && this.uiInput.isSelected && this.selectOnTab == null)
		{
			PlayerAction inventory = this.xui.playerUI.playerInput.PermanentActions.Inventory;
			KeyBindingSource keyBindingSource = inventory.GetBindingOfType(false) as KeyBindingSource;
			if (inventory.WasPressed && keyBindingSource != null && keyBindingSource.Control == XUiC_TextInput.tabCombo)
			{
				ThreadManager.StartCoroutine(this.closeOnTabLater());
			}
		}
		if (this.IsDirty)
		{
			if (!this.Enabled)
			{
				this.uiInput.isSelected = false;
			}
			this.uiInput.enabled = this.Enabled;
			this.uiInput.inputType = this.displayType;
			this.uiInput.activeTextColor = this.activeTextColor;
			this.uiInput.UpdateLabel();
			if (!this.openCompleted && this.removeFocusOnOpen)
			{
				this.uiInput.RemoveFocus();
			}
			this.parseSelectOnTabName();
			this.IsDirty = false;
			base.RefreshBindings();
			if (!this.openCompleted)
			{
				this.openCompleted = true;
			}
		}
	}

	// Token: 0x060074E5 RID: 29925 RVA: 0x002C45C7 File Offset: 0x002C27C7
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator closeOnTabLater()
	{
		PlayerAction inventoryAction = this.xui.playerUI.playerInput.PermanentActions.Inventory;
		KeyBindingSource keyBindingSource = inventoryAction.GetBindingOfType(false) as KeyBindingSource;
		if (keyBindingSource == null || keyBindingSource.Control != XUiC_TextInput.tabCombo)
		{
			yield break;
		}
		while (inventoryAction.IsPressed)
		{
			yield return null;
		}
		if (this.CloseGroupOnTab && this.uiInput.isSelected && this.selectOnTab == null)
		{
			this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		}
		yield break;
	}

	// Token: 0x060074E6 RID: 29926 RVA: 0x002C45D6 File Offset: 0x002C27D6
	public void SetSelected(bool _selected = true, bool _delayed = false)
	{
		ThreadManager.StartCoroutine(this.setSelectedDelayed(_selected, _delayed));
	}

	// Token: 0x060074E7 RID: 29927 RVA: 0x002C45E6 File Offset: 0x002C27E6
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator setSelectedDelayed(bool _selected, bool _delayed)
	{
		if (_delayed)
		{
			yield return null;
		}
		if (!_selected || PlatformManager.NativePlatform.Input.CurrentInputStyle == PlayerInputManager.InputStyle.Keyboard)
		{
			this.uiInput.isSelected = _selected;
			if (!this.openCompleted)
			{
				this.removeFocusOnOpen = false;
			}
		}
		yield break;
	}

	// Token: 0x060074E8 RID: 29928 RVA: 0x002C4603 File Offset: 0x002C2803
	public void SelectOrVirtualKeyboard(bool _delayed = false)
	{
		if (PlatformManager.NativePlatform.Input.CurrentInputStyle == PlayerInputManager.InputStyle.Keyboard)
		{
			this.SetSelected(true, _delayed);
			return;
		}
		this.showVirtualKeyboard();
	}

	// Token: 0x060074E9 RID: 29929 RVA: 0x002C4628 File Offset: 0x002C2828
	[PublicizedFrom(EAccessModifier.Protected)]
	public void parseSelectOnTabName()
	{
		if (!this.selectOnTabSetFromXML)
		{
			return;
		}
		XUiC_TextInput xuiC_TextInput = this.selectOnTab;
		if (((xuiC_TextInput != null) ? xuiC_TextInput.ViewComponent.ID : null) == this.selectOnTabString)
		{
			return;
		}
		XUiView xuiView = XUiUtils.FindHierarchyClosestView(this, this.selectOnTabString);
		XUiC_TextInput xuiC_TextInput2 = ((xuiView != null) ? xuiView.Controller : null) as XUiC_TextInput;
		if (xuiC_TextInput2 != null)
		{
			this.SelectOnTab = xuiC_TextInput2;
		}
	}

	// Token: 0x040056DE RID: 22238
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Label label;

	// Token: 0x040056DF RID: 22239
	[XuiBindComponent("scrollarea", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiController scrollArea;

	// Token: 0x040056E0 RID: 22240
	[XuiBindComponent(false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_ScrollView scrollView;

	// Token: 0x040056E1 RID: 22241
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController uiInputController;

	// Token: 0x040056E2 RID: 22242
	[PublicizedFrom(EAccessModifier.Private)]
	public UIInput uiInput;

	// Token: 0x040056E3 RID: 22243
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput selectOnTab;

	// Token: 0x040056E4 RID: 22244
	[PublicizedFrom(EAccessModifier.Private)]
	public string selectOnTabString;

	// Token: 0x040056E5 RID: 22245
	[PublicizedFrom(EAccessModifier.Private)]
	public bool selectOnTabSetFromXML;

	// Token: 0x040056E7 RID: 22247
	[PublicizedFrom(EAccessModifier.Private)]
	public Color activeTextColor;

	// Token: 0x040056EA RID: 22250
	[PublicizedFrom(EAccessModifier.Private)]
	public UIInput.InputType displayType;

	// Token: 0x040056F1 RID: 22257
	[PublicizedFrom(EAccessModifier.Private)]
	public string virtKeyboardPrompt = "";

	// Token: 0x040056F2 RID: 22258
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOpen;

	// Token: 0x040056F3 RID: 22259
	[PublicizedFrom(EAccessModifier.Private)]
	public bool openCompleted;

	// Token: 0x040056F4 RID: 22260
	[PublicizedFrom(EAccessModifier.Private)]
	public bool removeFocusOnOpen = true;

	// Token: 0x040056FC RID: 22268
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly KeyCombo tabCombo = new KeyCombo(new Key[] { Key.Tab });

	// Token: 0x040056FE RID: 22270
	[PublicizedFrom(EAccessModifier.Private)]
	public bool textChangeFromCode;

	// Token: 0x040056FF RID: 22271
	[PublicizedFrom(EAccessModifier.Private)]
	public static XUiC_TextInput currentSearchField;

	// Token: 0x04005700 RID: 22272
	[PublicizedFrom(EAccessModifier.Private)]
	public bool enabled = true;

	// Token: 0x04005701 RID: 22273
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<Vector3> caretWorldCorners = new List<Vector3>(4);
}

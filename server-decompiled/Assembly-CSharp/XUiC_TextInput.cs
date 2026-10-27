using System;
using System.Collections;
using System.Collections.Generic;
using InControl;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000EBD RID: 3773
[global::UnityEngine.Scripting.Preserve]
public class XUiC_TextInput : XUiController
{
	// Token: 0x140000B7 RID: 183
	// (add) Token: 0x060074B8 RID: 29880 RVA: 0x002C4E38 File Offset: 0x002C3038
	// (remove) Token: 0x060074B9 RID: 29881 RVA: 0x002C4E70 File Offset: 0x002C3070
	public event XUiEvent_InputOnSubmitEventHandler OnSubmitHandler;

	// Token: 0x140000B8 RID: 184
	// (add) Token: 0x060074BA RID: 29882 RVA: 0x002C4EA8 File Offset: 0x002C30A8
	// (remove) Token: 0x060074BB RID: 29883 RVA: 0x002C4EE0 File Offset: 0x002C30E0
	public event XUiEvent_InputOnChangedEventHandler OnChangeHandler;

	// Token: 0x140000B9 RID: 185
	// (add) Token: 0x060074BC RID: 29884 RVA: 0x002C4F18 File Offset: 0x002C3118
	// (remove) Token: 0x060074BD RID: 29885 RVA: 0x002C4F50 File Offset: 0x002C3150
	public event XUiEvent_InputOnAbortedEventHandler OnInputAbortedHandler;

	// Token: 0x140000BA RID: 186
	// (add) Token: 0x060074BE RID: 29886 RVA: 0x002C4F88 File Offset: 0x002C3188
	// (remove) Token: 0x060074BF RID: 29887 RVA: 0x002C4FC0 File Offset: 0x002C31C0
	public event XUiEvent_InputOnSelectedEventHandler OnInputSelectedHandler;

	// Token: 0x140000BB RID: 187
	// (add) Token: 0x060074C0 RID: 29888 RVA: 0x002C4FF8 File Offset: 0x002C31F8
	// (remove) Token: 0x060074C1 RID: 29889 RVA: 0x002C5030 File Offset: 0x002C3230
	public event XUiEvent_InputOnErrorEventHandler OnInputErrorHandler;

	// Token: 0x140000BC RID: 188
	// (add) Token: 0x060074C2 RID: 29890 RVA: 0x002C5068 File Offset: 0x002C3268
	// (remove) Token: 0x060074C3 RID: 29891 RVA: 0x002C50A0 File Offset: 0x002C32A0
	public event UIInput.OnClipboard OnClipboardHandler;

	// Token: 0x17000DC8 RID: 3528
	// (get) Token: 0x060074C4 RID: 29892 RVA: 0x002C50D5 File Offset: 0x002C32D5
	// (set) Token: 0x060074C5 RID: 29893 RVA: 0x002C50DD File Offset: 0x002C32DD
	[XuiXmlAttribute("value", false)]
	public string InitialValue
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000DC9 RID: 3529
	// (get) Token: 0x060074C6 RID: 29894 RVA: 0x002C50E6 File Offset: 0x002C32E6
	// (set) Token: 0x060074C7 RID: 29895 RVA: 0x002C50EE File Offset: 0x002C32EE
	[XuiXmlAttribute("caret_color", false)]
	public Color CaretColor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000DCA RID: 3530
	// (get) Token: 0x060074C8 RID: 29896 RVA: 0x002C50F7 File Offset: 0x002C32F7
	// (set) Token: 0x060074C9 RID: 29897 RVA: 0x002C50FF File Offset: 0x002C32FF
	[XuiXmlAttribute("selection_color", false)]
	public Color SelectionColor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000DCB RID: 3531
	// (get) Token: 0x060074CA RID: 29898 RVA: 0x002C5108 File Offset: 0x002C3308
	// (set) Token: 0x060074CB RID: 29899 RVA: 0x002C5110 File Offset: 0x002C3310
	[XuiXmlAttribute("validation", false)]
	public UIInput.Validation Validation
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000DCC RID: 3532
	// (get) Token: 0x060074CC RID: 29900 RVA: 0x002C5119 File Offset: 0x002C3319
	// (set) Token: 0x060074CD RID: 29901 RVA: 0x002C5121 File Offset: 0x002C3321
	[XuiXmlAttribute("hide_input", false)]
	public bool HideInput
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000DCD RID: 3533
	// (get) Token: 0x060074CE RID: 29902 RVA: 0x002C512A File Offset: 0x002C332A
	// (set) Token: 0x060074CF RID: 29903 RVA: 0x002C5132 File Offset: 0x002C3332
	[XuiXmlAttribute("on_return", false)]
	public UIInput.OnReturnKey OnReturnKey
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = UIInput.OnReturnKey.Submit;

	// Token: 0x17000DCE RID: 3534
	// (get) Token: 0x060074D0 RID: 29904 RVA: 0x002C513B File Offset: 0x002C333B
	// (set) Token: 0x060074D1 RID: 29905 RVA: 0x002C5143 File Offset: 0x002C3343
	[XuiXmlAttribute("character_limit", false)]
	public int CharacterLimit
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000DCF RID: 3535
	// (get) Token: 0x060074D2 RID: 29906 RVA: 0x002C514C File Offset: 0x002C334C
	// (set) Token: 0x060074D3 RID: 29907 RVA: 0x002C5154 File Offset: 0x002C3354
	[XuiXmlAttribute("input_type", false)]
	public UIInput.KeyboardType InputType
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000DD0 RID: 3536
	// (get) Token: 0x060074D4 RID: 29908 RVA: 0x002C515D File Offset: 0x002C335D
	// (set) Token: 0x060074D5 RID: 29909 RVA: 0x002C5165 File Offset: 0x002C3365
	[XuiXmlAttribute("ignore_up_down_keys", false)]
	public bool IgnoreUpDownKeys
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000DD1 RID: 3537
	// (get) Token: 0x060074D6 RID: 29910 RVA: 0x002C516E File Offset: 0x002C336E
	// (set) Token: 0x060074D7 RID: 29911 RVA: 0x002C5176 File Offset: 0x002C3376
	[XuiXmlAttribute("search_field", false)]
	public bool IsSearchField
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000DD2 RID: 3538
	// (get) Token: 0x060074D8 RID: 29912 RVA: 0x002C517F File Offset: 0x002C337F
	// (set) Token: 0x060074D9 RID: 29913 RVA: 0x002C5187 File Offset: 0x002C3387
	[XuiXmlAttribute("clear_button", false)]
	[XuiXmlBinding("clearbutton")]
	public bool HasClearButton
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000DD3 RID: 3539
	// (get) Token: 0x060074DA RID: 29914 RVA: 0x002C5190 File Offset: 0x002C3390
	// (set) Token: 0x060074DB RID: 29915 RVA: 0x002C5198 File Offset: 0x002C3398
	[XuiXmlAttribute("password_field", false)]
	[XuiXmlBinding("passwordfield")]
	public bool IsPasswordField
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000DD4 RID: 3540
	// (get) Token: 0x060074DC RID: 29916 RVA: 0x002C51A1 File Offset: 0x002C33A1
	// (set) Token: 0x060074DD RID: 29917 RVA: 0x002C51A9 File Offset: 0x002C33A9
	[XuiXmlAttribute("focus_on_open", false)]
	public bool FocusOnOpen
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000DD5 RID: 3541
	// (get) Token: 0x060074DE RID: 29918 RVA: 0x002C51B2 File Offset: 0x002C33B2
	// (set) Token: 0x060074DF RID: 29919 RVA: 0x002C51BA File Offset: 0x002C33BA
	[XuiXmlAttribute("open_vk_on_open", false)]
	public bool OpenVkOnOpen
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000DD6 RID: 3542
	// (get) Token: 0x060074E0 RID: 29920 RVA: 0x002C51C3 File Offset: 0x002C33C3
	// (set) Token: 0x060074E1 RID: 29921 RVA: 0x002C51CB File Offset: 0x002C33CB
	[XuiXmlAttribute("clear_on_open", false)]
	public bool ClearOnOpen
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000DD7 RID: 3543
	// (get) Token: 0x060074E2 RID: 29922 RVA: 0x002C51D4 File Offset: 0x002C33D4
	// (set) Token: 0x060074E3 RID: 29923 RVA: 0x002C51DC File Offset: 0x002C33DC
	[XuiXmlAttribute("vk_propagates_events", false)]
	public bool VkPropagatesEvents
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = true;

	// Token: 0x17000DD8 RID: 3544
	// (get) Token: 0x060074E4 RID: 29924 RVA: 0x002C51E5 File Offset: 0x002C33E5
	// (set) Token: 0x060074E5 RID: 29925 RVA: 0x002C51ED File Offset: 0x002C33ED
	[XuiXmlAttribute("close_group_on_tab", false)]
	public bool CloseGroupOnTab
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x060074E6 RID: 29926 RVA: 0x002C51F6 File Offset: 0x002C33F6
	public static void SelectCurrentSearchField(LocalPlayerUI _playerUi)
	{
		if (XUiC_TextInput.currentSearchField != null && !_playerUi.windowManager.IsInputActive() && XUiC_TextInput.currentSearchField.viewComponent.IsActiveInHierarchy)
		{
			XUiC_TextInput.currentSearchField.SetSelected(true, false);
		}
	}

	// Token: 0x17000DD9 RID: 3545
	// (get) Token: 0x060074E7 RID: 29927 RVA: 0x002C5229 File Offset: 0x002C3429
	public UIInput UIInput
	{
		get
		{
			return this.uiInput;
		}
	}

	// Token: 0x17000DDA RID: 3546
	// (get) Token: 0x060074E8 RID: 29928 RVA: 0x002C5231 File Offset: 0x002C3431
	public XUiController UIInputController
	{
		get
		{
			return this.uiInputController;
		}
	}

	// Token: 0x17000DDB RID: 3547
	// (get) Token: 0x060074E9 RID: 29929 RVA: 0x002C5239 File Offset: 0x002C3439
	// (set) Token: 0x060074EA RID: 29930 RVA: 0x002C5256 File Offset: 0x002C3456
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

	// Token: 0x17000DDC RID: 3548
	// (get) Token: 0x060074EB RID: 29931 RVA: 0x002C527D File Offset: 0x002C347D
	// (set) Token: 0x060074EC RID: 29932 RVA: 0x002C5288 File Offset: 0x002C3488
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

	// Token: 0x17000DDD RID: 3549
	// (get) Token: 0x060074ED RID: 29933 RVA: 0x002C5305 File Offset: 0x002C3505
	// (set) Token: 0x060074EE RID: 29934 RVA: 0x002C530D File Offset: 0x002C350D
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

	// Token: 0x17000DDE RID: 3550
	// (get) Token: 0x060074EF RID: 29935 RVA: 0x002C532C File Offset: 0x002C352C
	// (set) Token: 0x060074F0 RID: 29936 RVA: 0x002C5334 File Offset: 0x002C3534
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

	// Token: 0x17000DDF RID: 3551
	// (get) Token: 0x060074F1 RID: 29937 RVA: 0x002C534E File Offset: 0x002C354E
	// (set) Token: 0x060074F2 RID: 29938 RVA: 0x002C535B File Offset: 0x002C355B
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

	// Token: 0x17000DE0 RID: 3552
	// (get) Token: 0x060074F3 RID: 29939 RVA: 0x002C5369 File Offset: 0x002C3569
	public bool IsSelected
	{
		get
		{
			return this.uiInput.isSelected;
		}
	}

	// Token: 0x17000DE1 RID: 3553
	// (get) Token: 0x060074F4 RID: 29940 RVA: 0x002C5376 File Offset: 0x002C3576
	[XuiXmlBinding("showpassword")]
	public bool ShowPassword
	{
		get
		{
			return this.displayType == UIInput.InputType.Standard;
		}
	}

	// Token: 0x060074F5 RID: 29941 RVA: 0x002C5381 File Offset: 0x002C3581
	[XuiXmlAttribute("virtual_keyboard_prompt", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeVirtualKeyboardPrompt(string _value)
	{
		this.virtKeyboardPrompt = Localization.Get(_value, false, null);
	}

	// Token: 0x17000DE2 RID: 3554
	// (get) Token: 0x060074F6 RID: 29942 RVA: 0x002C5391 File Offset: 0x002C3591
	[XuiXmlBinding("txt_empty")]
	public bool IsEmpty
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.Text.Length == 0;
		}
	}

	// Token: 0x060074F7 RID: 29943 RVA: 0x002C53A1 File Offset: 0x002C35A1
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

	// Token: 0x060074F8 RID: 29944 RVA: 0x002C53C4 File Offset: 0x002C35C4
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

	// Token: 0x060074F9 RID: 29945 RVA: 0x002C55D4 File Offset: 0x002C37D4
	[XuiBindEvent("OnPress", "scrollArea")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void ScrollArea_OnPress(XUiController _sender, int _mouseButton)
	{
		this.SelectOrVirtualKeyboard(true);
	}

	// Token: 0x060074FA RID: 29946 RVA: 0x002C55DD File Offset: 0x002C37DD
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnShowPassword_OnPress(XUiController _sender, int _mouseButton)
	{
		this.displayType = ((this.displayType == UIInput.InputType.Password) ? UIInput.InputType.Standard : UIInput.InputType.Password);
		this.IsDirty = true;
	}

	// Token: 0x060074FB RID: 29947 RVA: 0x002C55F9 File Offset: 0x002C37F9
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnClearInput_OnPress(XUiController _sender, int _mouseButton)
	{
		this.Text = "";
		this.IsDirty = true;
	}

	// Token: 0x060074FC RID: 29948 RVA: 0x002C5610 File Offset: 0x002C3810
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

	// Token: 0x060074FD RID: 29949 RVA: 0x002C56D8 File Offset: 0x002C38D8
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

	// Token: 0x060074FE RID: 29950 RVA: 0x002C56FC File Offset: 0x002C38FC
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

	// Token: 0x060074FF RID: 29951 RVA: 0x002C570C File Offset: 0x002C390C
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

	// Token: 0x06007500 RID: 29952 RVA: 0x002C5784 File Offset: 0x002C3984
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

	// Token: 0x06007501 RID: 29953 RVA: 0x002C57BC File Offset: 0x002C39BC
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

	// Token: 0x06007502 RID: 29954 RVA: 0x002C57D2 File Offset: 0x002C39D2
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

	// Token: 0x06007503 RID: 29955 RVA: 0x002C5810 File Offset: 0x002C3A10
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

	// Token: 0x06007504 RID: 29956 RVA: 0x002C58BE File Offset: 0x002C3ABE
	public void TriggerOnChangeHandler(bool _changeFromCode = false)
	{
		XUiEvent_InputOnChangedEventHandler onChangeHandler = this.OnChangeHandler;
		if (onChangeHandler == null)
		{
			return;
		}
		onChangeHandler(this, this.uiInput.value, _changeFromCode);
	}

	// Token: 0x06007505 RID: 29957 RVA: 0x002C58DD File Offset: 0x002C3ADD
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

	// Token: 0x06007506 RID: 29958 RVA: 0x002C58F8 File Offset: 0x002C3AF8
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

	// Token: 0x06007507 RID: 29959 RVA: 0x002C5997 File Offset: 0x002C3B97
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

	// Token: 0x06007508 RID: 29960 RVA: 0x002C59BC File Offset: 0x002C3BBC
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

	// Token: 0x06007509 RID: 29961 RVA: 0x002C59D0 File Offset: 0x002C3BD0
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

	// Token: 0x0600750A RID: 29962 RVA: 0x002C5B33 File Offset: 0x002C3D33
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

	// Token: 0x0600750B RID: 29963 RVA: 0x002C5B42 File Offset: 0x002C3D42
	public void SetSelected(bool _selected = true, bool _delayed = false)
	{
		ThreadManager.StartCoroutine(this.setSelectedDelayed(_selected, _delayed));
	}

	// Token: 0x0600750C RID: 29964 RVA: 0x002C5B52 File Offset: 0x002C3D52
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

	// Token: 0x0600750D RID: 29965 RVA: 0x002C5B6F File Offset: 0x002C3D6F
	public void SelectOrVirtualKeyboard(bool _delayed = false)
	{
		if (PlatformManager.NativePlatform.Input.CurrentInputStyle == PlayerInputManager.InputStyle.Keyboard)
		{
			this.SetSelected(true, _delayed);
			return;
		}
		this.showVirtualKeyboard();
	}

	// Token: 0x0600750E RID: 29966 RVA: 0x002C5B94 File Offset: 0x002C3D94
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

	// Token: 0x04005703 RID: 22275
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Label label;

	// Token: 0x04005704 RID: 22276
	[XuiBindComponent("scrollarea", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiController scrollArea;

	// Token: 0x04005705 RID: 22277
	[XuiBindComponent(false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_ScrollView scrollView;

	// Token: 0x04005706 RID: 22278
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController uiInputController;

	// Token: 0x04005707 RID: 22279
	[PublicizedFrom(EAccessModifier.Private)]
	public UIInput uiInput;

	// Token: 0x04005708 RID: 22280
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput selectOnTab;

	// Token: 0x04005709 RID: 22281
	[PublicizedFrom(EAccessModifier.Private)]
	public string selectOnTabString;

	// Token: 0x0400570A RID: 22282
	[PublicizedFrom(EAccessModifier.Private)]
	public bool selectOnTabSetFromXML;

	// Token: 0x0400570C RID: 22284
	[PublicizedFrom(EAccessModifier.Private)]
	public Color activeTextColor;

	// Token: 0x0400570F RID: 22287
	[PublicizedFrom(EAccessModifier.Private)]
	public UIInput.InputType displayType;

	// Token: 0x04005716 RID: 22294
	[PublicizedFrom(EAccessModifier.Private)]
	public string virtKeyboardPrompt = "";

	// Token: 0x04005717 RID: 22295
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOpen;

	// Token: 0x04005718 RID: 22296
	[PublicizedFrom(EAccessModifier.Private)]
	public bool openCompleted;

	// Token: 0x04005719 RID: 22297
	[PublicizedFrom(EAccessModifier.Private)]
	public bool removeFocusOnOpen = true;

	// Token: 0x04005721 RID: 22305
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly KeyCombo tabCombo = new KeyCombo(new Key[] { Key.Tab });

	// Token: 0x04005723 RID: 22307
	[PublicizedFrom(EAccessModifier.Private)]
	public bool textChangeFromCode;

	// Token: 0x04005724 RID: 22308
	[PublicizedFrom(EAccessModifier.Private)]
	public static XUiC_TextInput currentSearchField;

	// Token: 0x04005725 RID: 22309
	[PublicizedFrom(EAccessModifier.Private)]
	public bool enabled = true;

	// Token: 0x04005726 RID: 22310
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<Vector3> caretWorldCorners = new List<Vector3>(4);
}

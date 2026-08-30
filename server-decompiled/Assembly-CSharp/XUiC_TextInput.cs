using System;
using System.Collections;
using System.Collections.Generic;
using InControl;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000EA3 RID: 3747
[global::UnityEngine.Scripting.Preserve]
public class XUiC_TextInput : XUiController
{
	// Token: 0x140000B8 RID: 184
	// (add) Token: 0x0600749B RID: 29851 RVA: 0x002C0514 File Offset: 0x002BE714
	// (remove) Token: 0x0600749C RID: 29852 RVA: 0x002C054C File Offset: 0x002BE74C
	public event XUiEvent_InputOnSubmitEventHandler OnSubmitHandler;

	// Token: 0x140000B9 RID: 185
	// (add) Token: 0x0600749D RID: 29853 RVA: 0x002C0584 File Offset: 0x002BE784
	// (remove) Token: 0x0600749E RID: 29854 RVA: 0x002C05BC File Offset: 0x002BE7BC
	public event XUiEvent_InputOnChangedEventHandler OnChangeHandler;

	// Token: 0x140000BA RID: 186
	// (add) Token: 0x0600749F RID: 29855 RVA: 0x002C05F4 File Offset: 0x002BE7F4
	// (remove) Token: 0x060074A0 RID: 29856 RVA: 0x002C062C File Offset: 0x002BE82C
	public event XUiEvent_InputOnAbortedEventHandler OnInputAbortedHandler;

	// Token: 0x140000BB RID: 187
	// (add) Token: 0x060074A1 RID: 29857 RVA: 0x002C0664 File Offset: 0x002BE864
	// (remove) Token: 0x060074A2 RID: 29858 RVA: 0x002C069C File Offset: 0x002BE89C
	public event XUiEvent_InputOnSelectedEventHandler OnInputSelectedHandler;

	// Token: 0x140000BC RID: 188
	// (add) Token: 0x060074A3 RID: 29859 RVA: 0x002C06D4 File Offset: 0x002BE8D4
	// (remove) Token: 0x060074A4 RID: 29860 RVA: 0x002C070C File Offset: 0x002BE90C
	public event XUiEvent_InputOnErrorEventHandler OnInputErrorHandler;

	// Token: 0x140000BD RID: 189
	// (add) Token: 0x060074A5 RID: 29861 RVA: 0x002C0744 File Offset: 0x002BE944
	// (remove) Token: 0x060074A6 RID: 29862 RVA: 0x002C077C File Offset: 0x002BE97C
	public event UIInput.OnClipboard OnClipboardHandler;

	// Token: 0x17000DA7 RID: 3495
	// (get) Token: 0x060074A7 RID: 29863 RVA: 0x002C07B1 File Offset: 0x002BE9B1
	// (set) Token: 0x060074A8 RID: 29864 RVA: 0x002C07B9 File Offset: 0x002BE9B9
	[XuiXmlAttribute("value", false)]
	public string InitialValue
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000DA8 RID: 3496
	// (get) Token: 0x060074A9 RID: 29865 RVA: 0x002C07C2 File Offset: 0x002BE9C2
	// (set) Token: 0x060074AA RID: 29866 RVA: 0x002C07CA File Offset: 0x002BE9CA
	[XuiXmlAttribute("caret_color", false)]
	public Color CaretColor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000DA9 RID: 3497
	// (get) Token: 0x060074AB RID: 29867 RVA: 0x002C07D3 File Offset: 0x002BE9D3
	// (set) Token: 0x060074AC RID: 29868 RVA: 0x002C07DB File Offset: 0x002BE9DB
	[XuiXmlAttribute("selection_color", false)]
	public Color SelectionColor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000DAA RID: 3498
	// (get) Token: 0x060074AD RID: 29869 RVA: 0x002C07E4 File Offset: 0x002BE9E4
	// (set) Token: 0x060074AE RID: 29870 RVA: 0x002C07EC File Offset: 0x002BE9EC
	[XuiXmlAttribute("validation", false)]
	public UIInput.Validation Validation
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000DAB RID: 3499
	// (get) Token: 0x060074AF RID: 29871 RVA: 0x002C07F5 File Offset: 0x002BE9F5
	// (set) Token: 0x060074B0 RID: 29872 RVA: 0x002C07FD File Offset: 0x002BE9FD
	[XuiXmlAttribute("hide_input", false)]
	public bool HideInput
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000DAC RID: 3500
	// (get) Token: 0x060074B1 RID: 29873 RVA: 0x002C0806 File Offset: 0x002BEA06
	// (set) Token: 0x060074B2 RID: 29874 RVA: 0x002C080E File Offset: 0x002BEA0E
	[XuiXmlAttribute("on_return", false)]
	public UIInput.OnReturnKey OnReturnKey
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = UIInput.OnReturnKey.Submit;

	// Token: 0x17000DAD RID: 3501
	// (get) Token: 0x060074B3 RID: 29875 RVA: 0x002C0817 File Offset: 0x002BEA17
	// (set) Token: 0x060074B4 RID: 29876 RVA: 0x002C081F File Offset: 0x002BEA1F
	[XuiXmlAttribute("character_limit", false)]
	public int CharacterLimit
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000DAE RID: 3502
	// (get) Token: 0x060074B5 RID: 29877 RVA: 0x002C0828 File Offset: 0x002BEA28
	// (set) Token: 0x060074B6 RID: 29878 RVA: 0x002C0830 File Offset: 0x002BEA30
	[XuiXmlAttribute("input_type", false)]
	public UIInput.KeyboardType InputType
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000DAF RID: 3503
	// (get) Token: 0x060074B7 RID: 29879 RVA: 0x002C0839 File Offset: 0x002BEA39
	// (set) Token: 0x060074B8 RID: 29880 RVA: 0x002C0841 File Offset: 0x002BEA41
	[XuiXmlAttribute("ignore_up_down_keys", false)]
	public bool IgnoreUpDownKeys
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000DB0 RID: 3504
	// (get) Token: 0x060074B9 RID: 29881 RVA: 0x002C084A File Offset: 0x002BEA4A
	// (set) Token: 0x060074BA RID: 29882 RVA: 0x002C0852 File Offset: 0x002BEA52
	[XuiXmlAttribute("search_field", false)]
	public bool IsSearchField
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000DB1 RID: 3505
	// (get) Token: 0x060074BB RID: 29883 RVA: 0x002C085B File Offset: 0x002BEA5B
	// (set) Token: 0x060074BC RID: 29884 RVA: 0x002C0863 File Offset: 0x002BEA63
	[XuiXmlAttribute("clear_button", false)]
	[XuiXmlBinding("clearbutton")]
	public bool HasClearButton
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000DB2 RID: 3506
	// (get) Token: 0x060074BD RID: 29885 RVA: 0x002C086C File Offset: 0x002BEA6C
	// (set) Token: 0x060074BE RID: 29886 RVA: 0x002C0874 File Offset: 0x002BEA74
	[XuiXmlAttribute("password_field", false)]
	[XuiXmlBinding("passwordfield")]
	public bool IsPasswordField
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000DB3 RID: 3507
	// (get) Token: 0x060074BF RID: 29887 RVA: 0x002C087D File Offset: 0x002BEA7D
	// (set) Token: 0x060074C0 RID: 29888 RVA: 0x002C0885 File Offset: 0x002BEA85
	[XuiXmlAttribute("focus_on_open", false)]
	public bool FocusOnOpen
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000DB4 RID: 3508
	// (get) Token: 0x060074C1 RID: 29889 RVA: 0x002C088E File Offset: 0x002BEA8E
	// (set) Token: 0x060074C2 RID: 29890 RVA: 0x002C0896 File Offset: 0x002BEA96
	[XuiXmlAttribute("open_vk_on_open", false)]
	public bool OpenVkOnOpen
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000DB5 RID: 3509
	// (get) Token: 0x060074C3 RID: 29891 RVA: 0x002C089F File Offset: 0x002BEA9F
	// (set) Token: 0x060074C4 RID: 29892 RVA: 0x002C08A7 File Offset: 0x002BEAA7
	[XuiXmlAttribute("clear_on_open", false)]
	public bool ClearOnOpen
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000DB6 RID: 3510
	// (get) Token: 0x060074C5 RID: 29893 RVA: 0x002C08B0 File Offset: 0x002BEAB0
	// (set) Token: 0x060074C6 RID: 29894 RVA: 0x002C08B8 File Offset: 0x002BEAB8
	[XuiXmlAttribute("vk_propagates_events", false)]
	public bool VkPropagatesEvents
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = true;

	// Token: 0x17000DB7 RID: 3511
	// (get) Token: 0x060074C7 RID: 29895 RVA: 0x002C08C1 File Offset: 0x002BEAC1
	// (set) Token: 0x060074C8 RID: 29896 RVA: 0x002C08C9 File Offset: 0x002BEAC9
	[XuiXmlAttribute("close_group_on_tab", false)]
	public bool CloseGroupOnTab
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x060074C9 RID: 29897 RVA: 0x002C08D2 File Offset: 0x002BEAD2
	public static void SelectCurrentSearchField(LocalPlayerUI _playerUi)
	{
		if (XUiC_TextInput.currentSearchField != null && !_playerUi.windowManager.IsInputActive() && XUiC_TextInput.currentSearchField.viewComponent.IsActiveInHierarchy)
		{
			XUiC_TextInput.currentSearchField.SetSelected(true, false);
		}
	}

	// Token: 0x17000DB8 RID: 3512
	// (get) Token: 0x060074CA RID: 29898 RVA: 0x002C0905 File Offset: 0x002BEB05
	public UIInput UIInput
	{
		get
		{
			return this.uiInput;
		}
	}

	// Token: 0x17000DB9 RID: 3513
	// (get) Token: 0x060074CB RID: 29899 RVA: 0x002C090D File Offset: 0x002BEB0D
	public XUiController UIInputController
	{
		get
		{
			return this.uiInputController;
		}
	}

	// Token: 0x17000DBA RID: 3514
	// (get) Token: 0x060074CC RID: 29900 RVA: 0x002C0915 File Offset: 0x002BEB15
	// (set) Token: 0x060074CD RID: 29901 RVA: 0x002C0932 File Offset: 0x002BEB32
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

	// Token: 0x17000DBB RID: 3515
	// (get) Token: 0x060074CE RID: 29902 RVA: 0x002C0959 File Offset: 0x002BEB59
	// (set) Token: 0x060074CF RID: 29903 RVA: 0x002C0964 File Offset: 0x002BEB64
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

	// Token: 0x17000DBC RID: 3516
	// (get) Token: 0x060074D0 RID: 29904 RVA: 0x002C09E1 File Offset: 0x002BEBE1
	// (set) Token: 0x060074D1 RID: 29905 RVA: 0x002C09E9 File Offset: 0x002BEBE9
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

	// Token: 0x17000DBD RID: 3517
	// (get) Token: 0x060074D2 RID: 29906 RVA: 0x002C0A08 File Offset: 0x002BEC08
	// (set) Token: 0x060074D3 RID: 29907 RVA: 0x002C0A10 File Offset: 0x002BEC10
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

	// Token: 0x17000DBE RID: 3518
	// (get) Token: 0x060074D4 RID: 29908 RVA: 0x002C0A2A File Offset: 0x002BEC2A
	// (set) Token: 0x060074D5 RID: 29909 RVA: 0x002C0A37 File Offset: 0x002BEC37
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

	// Token: 0x17000DBF RID: 3519
	// (get) Token: 0x060074D6 RID: 29910 RVA: 0x002C0A45 File Offset: 0x002BEC45
	public bool IsSelected
	{
		get
		{
			return this.uiInput.isSelected;
		}
	}

	// Token: 0x17000DC0 RID: 3520
	// (get) Token: 0x060074D7 RID: 29911 RVA: 0x002C0A52 File Offset: 0x002BEC52
	[XuiXmlBinding("showpassword")]
	public bool ShowPassword
	{
		get
		{
			return this.displayType == UIInput.InputType.Standard;
		}
	}

	// Token: 0x060074D8 RID: 29912 RVA: 0x002C0A5D File Offset: 0x002BEC5D
	[XuiXmlAttribute("virtual_keyboard_prompt", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeVirtualKeyboardPrompt(string _value)
	{
		this.virtKeyboardPrompt = Localization.Get(_value, false, null);
	}

	// Token: 0x17000DC1 RID: 3521
	// (get) Token: 0x060074D9 RID: 29913 RVA: 0x002C0A6D File Offset: 0x002BEC6D
	[XuiXmlBinding("txt_empty")]
	public bool IsEmpty
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.Text.Length == 0;
		}
	}

	// Token: 0x060074DA RID: 29914 RVA: 0x002C0A7D File Offset: 0x002BEC7D
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

	// Token: 0x060074DB RID: 29915 RVA: 0x002C0AA0 File Offset: 0x002BECA0
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

	// Token: 0x060074DC RID: 29916 RVA: 0x002C0CB0 File Offset: 0x002BEEB0
	[XuiBindEvent("OnPress", "scrollArea")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void ScrollArea_OnPress(XUiController _sender, int _mouseButton)
	{
		this.SelectOrVirtualKeyboard(true);
	}

	// Token: 0x060074DD RID: 29917 RVA: 0x002C0CB9 File Offset: 0x002BEEB9
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnShowPassword_OnPress(XUiController _sender, int _mouseButton)
	{
		this.displayType = ((this.displayType == UIInput.InputType.Password) ? UIInput.InputType.Standard : UIInput.InputType.Password);
		this.IsDirty = true;
	}

	// Token: 0x060074DE RID: 29918 RVA: 0x002C0CD5 File Offset: 0x002BEED5
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnClearInput_OnPress(XUiController _sender, int _mouseButton)
	{
		this.Text = "";
		this.IsDirty = true;
	}

	// Token: 0x060074DF RID: 29919 RVA: 0x002C0CEC File Offset: 0x002BEEEC
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

	// Token: 0x060074E0 RID: 29920 RVA: 0x002C0DB4 File Offset: 0x002BEFB4
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

	// Token: 0x060074E1 RID: 29921 RVA: 0x002C0DD8 File Offset: 0x002BEFD8
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

	// Token: 0x060074E2 RID: 29922 RVA: 0x002C0DE8 File Offset: 0x002BEFE8
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

	// Token: 0x060074E3 RID: 29923 RVA: 0x002C0E60 File Offset: 0x002BF060
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

	// Token: 0x060074E4 RID: 29924 RVA: 0x002C0E98 File Offset: 0x002BF098
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

	// Token: 0x060074E5 RID: 29925 RVA: 0x002C0EAE File Offset: 0x002BF0AE
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

	// Token: 0x060074E6 RID: 29926 RVA: 0x002C0EEC File Offset: 0x002BF0EC
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

	// Token: 0x060074E7 RID: 29927 RVA: 0x002C0F9A File Offset: 0x002BF19A
	public void TriggerOnChangeHandler(bool _changeFromCode = false)
	{
		XUiEvent_InputOnChangedEventHandler onChangeHandler = this.OnChangeHandler;
		if (onChangeHandler == null)
		{
			return;
		}
		onChangeHandler(this, this.uiInput.value, _changeFromCode);
	}

	// Token: 0x060074E8 RID: 29928 RVA: 0x002C0FB9 File Offset: 0x002BF1B9
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

	// Token: 0x060074E9 RID: 29929 RVA: 0x002C0FD4 File Offset: 0x002BF1D4
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

	// Token: 0x060074EA RID: 29930 RVA: 0x002C1073 File Offset: 0x002BF273
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

	// Token: 0x060074EB RID: 29931 RVA: 0x002C1098 File Offset: 0x002BF298
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

	// Token: 0x060074EC RID: 29932 RVA: 0x002C10AC File Offset: 0x002BF2AC
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

	// Token: 0x060074ED RID: 29933 RVA: 0x002C120F File Offset: 0x002BF40F
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

	// Token: 0x060074EE RID: 29934 RVA: 0x002C121E File Offset: 0x002BF41E
	public void SetSelected(bool _selected = true, bool _delayed = false)
	{
		ThreadManager.StartCoroutine(this.setSelectedDelayed(_selected, _delayed));
	}

	// Token: 0x060074EF RID: 29935 RVA: 0x002C122E File Offset: 0x002BF42E
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

	// Token: 0x060074F0 RID: 29936 RVA: 0x002C124B File Offset: 0x002BF44B
	public void SelectOrVirtualKeyboard(bool _delayed = false)
	{
		if (PlatformManager.NativePlatform.Input.CurrentInputStyle == PlayerInputManager.InputStyle.Keyboard)
		{
			this.SetSelected(true, _delayed);
			return;
		}
		this.showVirtualKeyboard();
	}

	// Token: 0x060074F1 RID: 29937 RVA: 0x002C1270 File Offset: 0x002BF470
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

	// Token: 0x040056A7 RID: 22183
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Label label;

	// Token: 0x040056A8 RID: 22184
	[XuiBindComponent("scrollarea", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiController scrollArea;

	// Token: 0x040056A9 RID: 22185
	[XuiBindComponent(false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_ScrollView scrollView;

	// Token: 0x040056AA RID: 22186
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController uiInputController;

	// Token: 0x040056AB RID: 22187
	[PublicizedFrom(EAccessModifier.Private)]
	public UIInput uiInput;

	// Token: 0x040056AC RID: 22188
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput selectOnTab;

	// Token: 0x040056AD RID: 22189
	[PublicizedFrom(EAccessModifier.Private)]
	public string selectOnTabString;

	// Token: 0x040056AE RID: 22190
	[PublicizedFrom(EAccessModifier.Private)]
	public bool selectOnTabSetFromXML;

	// Token: 0x040056B0 RID: 22192
	[PublicizedFrom(EAccessModifier.Private)]
	public Color activeTextColor;

	// Token: 0x040056B3 RID: 22195
	[PublicizedFrom(EAccessModifier.Private)]
	public UIInput.InputType displayType;

	// Token: 0x040056BA RID: 22202
	[PublicizedFrom(EAccessModifier.Private)]
	public string virtKeyboardPrompt = "";

	// Token: 0x040056BB RID: 22203
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOpen;

	// Token: 0x040056BC RID: 22204
	[PublicizedFrom(EAccessModifier.Private)]
	public bool openCompleted;

	// Token: 0x040056BD RID: 22205
	[PublicizedFrom(EAccessModifier.Private)]
	public bool removeFocusOnOpen = true;

	// Token: 0x040056C5 RID: 22213
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly KeyCombo tabCombo = new KeyCombo(new Key[] { Key.Tab });

	// Token: 0x040056C7 RID: 22215
	[PublicizedFrom(EAccessModifier.Private)]
	public bool textChangeFromCode;

	// Token: 0x040056C8 RID: 22216
	[PublicizedFrom(EAccessModifier.Private)]
	public static XUiC_TextInput currentSearchField;

	// Token: 0x040056C9 RID: 22217
	[PublicizedFrom(EAccessModifier.Private)]
	public bool enabled = true;

	// Token: 0x040056CA RID: 22218
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<Vector3> caretWorldCorners = new List<Vector3>(4);
}

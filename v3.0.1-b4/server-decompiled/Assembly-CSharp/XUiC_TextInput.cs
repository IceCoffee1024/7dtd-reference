using System;
using System.Collections;
using System.Collections.Generic;
using InControl;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E8E RID: 3726
[global::UnityEngine.Scripting.Preserve]
public class XUiC_TextInput : XUiController
{
	// Token: 0x140000B7 RID: 183
	// (add) Token: 0x060073FB RID: 29691 RVA: 0x002BCFE8 File Offset: 0x002BB1E8
	// (remove) Token: 0x060073FC RID: 29692 RVA: 0x002BD020 File Offset: 0x002BB220
	public event XUiEvent_InputOnSubmitEventHandler OnSubmitHandler;

	// Token: 0x140000B8 RID: 184
	// (add) Token: 0x060073FD RID: 29693 RVA: 0x002BD058 File Offset: 0x002BB258
	// (remove) Token: 0x060073FE RID: 29694 RVA: 0x002BD090 File Offset: 0x002BB290
	public event XUiEvent_InputOnChangedEventHandler OnChangeHandler;

	// Token: 0x140000B9 RID: 185
	// (add) Token: 0x060073FF RID: 29695 RVA: 0x002BD0C8 File Offset: 0x002BB2C8
	// (remove) Token: 0x06007400 RID: 29696 RVA: 0x002BD100 File Offset: 0x002BB300
	public event XUiEvent_InputOnAbortedEventHandler OnInputAbortedHandler;

	// Token: 0x140000BA RID: 186
	// (add) Token: 0x06007401 RID: 29697 RVA: 0x002BD138 File Offset: 0x002BB338
	// (remove) Token: 0x06007402 RID: 29698 RVA: 0x002BD170 File Offset: 0x002BB370
	public event XUiEvent_InputOnSelectedEventHandler OnInputSelectedHandler;

	// Token: 0x140000BB RID: 187
	// (add) Token: 0x06007403 RID: 29699 RVA: 0x002BD1A8 File Offset: 0x002BB3A8
	// (remove) Token: 0x06007404 RID: 29700 RVA: 0x002BD1E0 File Offset: 0x002BB3E0
	public event XUiEvent_InputOnErrorEventHandler OnInputErrorHandler;

	// Token: 0x140000BC RID: 188
	// (add) Token: 0x06007405 RID: 29701 RVA: 0x002BD218 File Offset: 0x002BB418
	// (remove) Token: 0x06007406 RID: 29702 RVA: 0x002BD250 File Offset: 0x002BB450
	public event UIInput.OnClipboard OnClipboardHandler;

	// Token: 0x17000D8E RID: 3470
	// (get) Token: 0x06007407 RID: 29703 RVA: 0x002BD285 File Offset: 0x002BB485
	// (set) Token: 0x06007408 RID: 29704 RVA: 0x002BD28D File Offset: 0x002BB48D
	[XuiXmlAttribute("value", false)]
	public string InitialValue
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000D8F RID: 3471
	// (get) Token: 0x06007409 RID: 29705 RVA: 0x002BD296 File Offset: 0x002BB496
	// (set) Token: 0x0600740A RID: 29706 RVA: 0x002BD29E File Offset: 0x002BB49E
	[XuiXmlAttribute("caret_color", false)]
	public Color CaretColor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000D90 RID: 3472
	// (get) Token: 0x0600740B RID: 29707 RVA: 0x002BD2A7 File Offset: 0x002BB4A7
	// (set) Token: 0x0600740C RID: 29708 RVA: 0x002BD2AF File Offset: 0x002BB4AF
	[XuiXmlAttribute("selection_color", false)]
	public Color SelectionColor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000D91 RID: 3473
	// (get) Token: 0x0600740D RID: 29709 RVA: 0x002BD2B8 File Offset: 0x002BB4B8
	// (set) Token: 0x0600740E RID: 29710 RVA: 0x002BD2C0 File Offset: 0x002BB4C0
	[XuiXmlAttribute("validation", false)]
	public UIInput.Validation Validation
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000D92 RID: 3474
	// (get) Token: 0x0600740F RID: 29711 RVA: 0x002BD2C9 File Offset: 0x002BB4C9
	// (set) Token: 0x06007410 RID: 29712 RVA: 0x002BD2D1 File Offset: 0x002BB4D1
	[XuiXmlAttribute("hide_input", false)]
	public bool HideInput
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000D93 RID: 3475
	// (get) Token: 0x06007411 RID: 29713 RVA: 0x002BD2DA File Offset: 0x002BB4DA
	// (set) Token: 0x06007412 RID: 29714 RVA: 0x002BD2E2 File Offset: 0x002BB4E2
	[XuiXmlAttribute("on_return", false)]
	public UIInput.OnReturnKey OnReturnKey
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = UIInput.OnReturnKey.Submit;

	// Token: 0x17000D94 RID: 3476
	// (get) Token: 0x06007413 RID: 29715 RVA: 0x002BD2EB File Offset: 0x002BB4EB
	// (set) Token: 0x06007414 RID: 29716 RVA: 0x002BD2F3 File Offset: 0x002BB4F3
	[XuiXmlAttribute("character_limit", false)]
	public int CharacterLimit
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000D95 RID: 3477
	// (get) Token: 0x06007415 RID: 29717 RVA: 0x002BD2FC File Offset: 0x002BB4FC
	// (set) Token: 0x06007416 RID: 29718 RVA: 0x002BD304 File Offset: 0x002BB504
	[XuiXmlAttribute("input_type", false)]
	public UIInput.KeyboardType InputType
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000D96 RID: 3478
	// (get) Token: 0x06007417 RID: 29719 RVA: 0x002BD30D File Offset: 0x002BB50D
	// (set) Token: 0x06007418 RID: 29720 RVA: 0x002BD315 File Offset: 0x002BB515
	[XuiXmlAttribute("ignore_up_down_keys", false)]
	public bool IgnoreUpDownKeys
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000D97 RID: 3479
	// (get) Token: 0x06007419 RID: 29721 RVA: 0x002BD31E File Offset: 0x002BB51E
	// (set) Token: 0x0600741A RID: 29722 RVA: 0x002BD326 File Offset: 0x002BB526
	[XuiXmlAttribute("search_field", false)]
	public bool IsSearchField
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000D98 RID: 3480
	// (get) Token: 0x0600741B RID: 29723 RVA: 0x002BD32F File Offset: 0x002BB52F
	// (set) Token: 0x0600741C RID: 29724 RVA: 0x002BD337 File Offset: 0x002BB537
	[XuiXmlAttribute("clear_button", false)]
	[XuiXmlBinding("clearbutton")]
	public bool HasClearButton
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000D99 RID: 3481
	// (get) Token: 0x0600741D RID: 29725 RVA: 0x002BD340 File Offset: 0x002BB540
	// (set) Token: 0x0600741E RID: 29726 RVA: 0x002BD348 File Offset: 0x002BB548
	[XuiXmlAttribute("password_field", false)]
	[XuiXmlBinding("passwordfield")]
	public bool IsPasswordField
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000D9A RID: 3482
	// (get) Token: 0x0600741F RID: 29727 RVA: 0x002BD351 File Offset: 0x002BB551
	// (set) Token: 0x06007420 RID: 29728 RVA: 0x002BD359 File Offset: 0x002BB559
	[XuiXmlAttribute("focus_on_open", false)]
	public bool FocusOnOpen
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000D9B RID: 3483
	// (get) Token: 0x06007421 RID: 29729 RVA: 0x002BD362 File Offset: 0x002BB562
	// (set) Token: 0x06007422 RID: 29730 RVA: 0x002BD36A File Offset: 0x002BB56A
	[XuiXmlAttribute("open_vk_on_open", false)]
	public bool OpenVkOnOpen
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000D9C RID: 3484
	// (get) Token: 0x06007423 RID: 29731 RVA: 0x002BD373 File Offset: 0x002BB573
	// (set) Token: 0x06007424 RID: 29732 RVA: 0x002BD37B File Offset: 0x002BB57B
	[XuiXmlAttribute("clear_on_open", false)]
	public bool ClearOnOpen
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000D9D RID: 3485
	// (get) Token: 0x06007425 RID: 29733 RVA: 0x002BD384 File Offset: 0x002BB584
	// (set) Token: 0x06007426 RID: 29734 RVA: 0x002BD38C File Offset: 0x002BB58C
	[XuiXmlAttribute("vk_propagates_events", false)]
	public bool VkPropagatesEvents
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = true;

	// Token: 0x17000D9E RID: 3486
	// (get) Token: 0x06007427 RID: 29735 RVA: 0x002BD395 File Offset: 0x002BB595
	// (set) Token: 0x06007428 RID: 29736 RVA: 0x002BD39D File Offset: 0x002BB59D
	[XuiXmlAttribute("close_group_on_tab", false)]
	public bool CloseGroupOnTab
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06007429 RID: 29737 RVA: 0x002BD3A6 File Offset: 0x002BB5A6
	public static void SelectCurrentSearchField(LocalPlayerUI _playerUi)
	{
		if (XUiC_TextInput.currentSearchField != null && !_playerUi.windowManager.IsInputActive() && XUiC_TextInput.currentSearchField.viewComponent.IsActiveInHierarchy)
		{
			XUiC_TextInput.currentSearchField.SetSelected(true, false);
		}
	}

	// Token: 0x17000D9F RID: 3487
	// (get) Token: 0x0600742A RID: 29738 RVA: 0x002BD3D9 File Offset: 0x002BB5D9
	public UIInput UIInput
	{
		get
		{
			return this.uiInput;
		}
	}

	// Token: 0x17000DA0 RID: 3488
	// (get) Token: 0x0600742B RID: 29739 RVA: 0x002BD3E1 File Offset: 0x002BB5E1
	public XUiController UIInputController
	{
		get
		{
			return this.uiInputController;
		}
	}

	// Token: 0x17000DA1 RID: 3489
	// (get) Token: 0x0600742C RID: 29740 RVA: 0x002BD3E9 File Offset: 0x002BB5E9
	// (set) Token: 0x0600742D RID: 29741 RVA: 0x002BD406 File Offset: 0x002BB606
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

	// Token: 0x17000DA2 RID: 3490
	// (get) Token: 0x0600742E RID: 29742 RVA: 0x002BD42D File Offset: 0x002BB62D
	// (set) Token: 0x0600742F RID: 29743 RVA: 0x002BD438 File Offset: 0x002BB638
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

	// Token: 0x17000DA3 RID: 3491
	// (get) Token: 0x06007430 RID: 29744 RVA: 0x002BD4B5 File Offset: 0x002BB6B5
	// (set) Token: 0x06007431 RID: 29745 RVA: 0x002BD4BD File Offset: 0x002BB6BD
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

	// Token: 0x17000DA4 RID: 3492
	// (get) Token: 0x06007432 RID: 29746 RVA: 0x002BD4DC File Offset: 0x002BB6DC
	// (set) Token: 0x06007433 RID: 29747 RVA: 0x002BD4E4 File Offset: 0x002BB6E4
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

	// Token: 0x17000DA5 RID: 3493
	// (get) Token: 0x06007434 RID: 29748 RVA: 0x002BD4FE File Offset: 0x002BB6FE
	// (set) Token: 0x06007435 RID: 29749 RVA: 0x002BD50B File Offset: 0x002BB70B
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

	// Token: 0x17000DA6 RID: 3494
	// (get) Token: 0x06007436 RID: 29750 RVA: 0x002BD519 File Offset: 0x002BB719
	public bool IsSelected
	{
		get
		{
			return this.uiInput.isSelected;
		}
	}

	// Token: 0x17000DA7 RID: 3495
	// (get) Token: 0x06007437 RID: 29751 RVA: 0x002BD526 File Offset: 0x002BB726
	[XuiXmlBinding("showpassword")]
	public bool ShowPassword
	{
		get
		{
			return this.displayType == UIInput.InputType.Standard;
		}
	}

	// Token: 0x06007438 RID: 29752 RVA: 0x002BD531 File Offset: 0x002BB731
	[XuiXmlAttribute("virtual_keyboard_prompt", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeVirtualKeyboardPrompt(string _value)
	{
		this.virtKeyboardPrompt = Localization.Get(_value, false, null);
	}

	// Token: 0x17000DA8 RID: 3496
	// (get) Token: 0x06007439 RID: 29753 RVA: 0x002BD541 File Offset: 0x002BB741
	[XuiXmlBinding("txt_empty")]
	public bool IsEmpty
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.Text.Length == 0;
		}
	}

	// Token: 0x0600743A RID: 29754 RVA: 0x002BD551 File Offset: 0x002BB751
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

	// Token: 0x0600743B RID: 29755 RVA: 0x002BD574 File Offset: 0x002BB774
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

	// Token: 0x0600743C RID: 29756 RVA: 0x002BD784 File Offset: 0x002BB984
	[XuiBindEvent("OnPress", "scrollArea")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void ScrollArea_OnPress(XUiController _sender, int _mouseButton)
	{
		this.SelectOrVirtualKeyboard(true);
	}

	// Token: 0x0600743D RID: 29757 RVA: 0x002BD78D File Offset: 0x002BB98D
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnShowPassword_OnPress(XUiController _sender, int _mouseButton)
	{
		this.displayType = ((this.displayType == UIInput.InputType.Password) ? UIInput.InputType.Standard : UIInput.InputType.Password);
		this.IsDirty = true;
	}

	// Token: 0x0600743E RID: 29758 RVA: 0x002BD7A9 File Offset: 0x002BB9A9
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnClearInput_OnPress(XUiController _sender, int _mouseButton)
	{
		this.Text = "";
		this.IsDirty = true;
	}

	// Token: 0x0600743F RID: 29759 RVA: 0x002BD7C0 File Offset: 0x002BB9C0
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

	// Token: 0x06007440 RID: 29760 RVA: 0x002BD888 File Offset: 0x002BBA88
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

	// Token: 0x06007441 RID: 29761 RVA: 0x002BD8AC File Offset: 0x002BBAAC
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

	// Token: 0x06007442 RID: 29762 RVA: 0x002BD8BC File Offset: 0x002BBABC
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

	// Token: 0x06007443 RID: 29763 RVA: 0x002BD934 File Offset: 0x002BBB34
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

	// Token: 0x06007444 RID: 29764 RVA: 0x002BD96C File Offset: 0x002BBB6C
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

	// Token: 0x06007445 RID: 29765 RVA: 0x002BD982 File Offset: 0x002BBB82
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

	// Token: 0x06007446 RID: 29766 RVA: 0x002BD9C0 File Offset: 0x002BBBC0
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

	// Token: 0x06007447 RID: 29767 RVA: 0x002BDA6E File Offset: 0x002BBC6E
	public void TriggerOnChangeHandler(bool _changeFromCode = false)
	{
		XUiEvent_InputOnChangedEventHandler onChangeHandler = this.OnChangeHandler;
		if (onChangeHandler == null)
		{
			return;
		}
		onChangeHandler(this, this.uiInput.value, _changeFromCode);
	}

	// Token: 0x06007448 RID: 29768 RVA: 0x002BDA8D File Offset: 0x002BBC8D
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

	// Token: 0x06007449 RID: 29769 RVA: 0x002BDAA8 File Offset: 0x002BBCA8
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

	// Token: 0x0600744A RID: 29770 RVA: 0x002BDB47 File Offset: 0x002BBD47
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

	// Token: 0x0600744B RID: 29771 RVA: 0x002BDB6C File Offset: 0x002BBD6C
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

	// Token: 0x0600744C RID: 29772 RVA: 0x002BDB80 File Offset: 0x002BBD80
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

	// Token: 0x0600744D RID: 29773 RVA: 0x002BDCE3 File Offset: 0x002BBEE3
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

	// Token: 0x0600744E RID: 29774 RVA: 0x002BDCF2 File Offset: 0x002BBEF2
	public void SetSelected(bool _selected = true, bool _delayed = false)
	{
		ThreadManager.StartCoroutine(this.setSelectedDelayed(_selected, _delayed));
	}

	// Token: 0x0600744F RID: 29775 RVA: 0x002BDD02 File Offset: 0x002BBF02
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

	// Token: 0x06007450 RID: 29776 RVA: 0x002BDD1F File Offset: 0x002BBF1F
	public void SelectOrVirtualKeyboard(bool _delayed = false)
	{
		if (PlatformManager.NativePlatform.Input.CurrentInputStyle == PlayerInputManager.InputStyle.Keyboard)
		{
			this.SetSelected(true, _delayed);
			return;
		}
		this.showVirtualKeyboard();
	}

	// Token: 0x06007451 RID: 29777 RVA: 0x002BDD44 File Offset: 0x002BBF44
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

	// Token: 0x04005603 RID: 22019
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Label label;

	// Token: 0x04005604 RID: 22020
	[XuiBindComponent("scrollarea", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiController scrollArea;

	// Token: 0x04005605 RID: 22021
	[XuiBindComponent(false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_ScrollView scrollView;

	// Token: 0x04005606 RID: 22022
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController uiInputController;

	// Token: 0x04005607 RID: 22023
	[PublicizedFrom(EAccessModifier.Private)]
	public UIInput uiInput;

	// Token: 0x04005608 RID: 22024
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput selectOnTab;

	// Token: 0x04005609 RID: 22025
	[PublicizedFrom(EAccessModifier.Private)]
	public string selectOnTabString;

	// Token: 0x0400560A RID: 22026
	[PublicizedFrom(EAccessModifier.Private)]
	public bool selectOnTabSetFromXML;

	// Token: 0x0400560C RID: 22028
	[PublicizedFrom(EAccessModifier.Private)]
	public Color activeTextColor;

	// Token: 0x0400560F RID: 22031
	[PublicizedFrom(EAccessModifier.Private)]
	public UIInput.InputType displayType;

	// Token: 0x04005616 RID: 22038
	[PublicizedFrom(EAccessModifier.Private)]
	public string virtKeyboardPrompt = "";

	// Token: 0x04005617 RID: 22039
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOpen;

	// Token: 0x04005618 RID: 22040
	[PublicizedFrom(EAccessModifier.Private)]
	public bool openCompleted;

	// Token: 0x04005619 RID: 22041
	[PublicizedFrom(EAccessModifier.Private)]
	public bool removeFocusOnOpen = true;

	// Token: 0x04005621 RID: 22049
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly KeyCombo tabCombo = new KeyCombo(new Key[] { Key.Tab });

	// Token: 0x04005623 RID: 22051
	[PublicizedFrom(EAccessModifier.Private)]
	public bool textChangeFromCode;

	// Token: 0x04005624 RID: 22052
	[PublicizedFrom(EAccessModifier.Private)]
	public static XUiC_TextInput currentSearchField;

	// Token: 0x04005625 RID: 22053
	[PublicizedFrom(EAccessModifier.Private)]
	public bool enabled = true;

	// Token: 0x04005626 RID: 22054
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<Vector3> caretWorldCorners = new List<Vector3>(4);
}

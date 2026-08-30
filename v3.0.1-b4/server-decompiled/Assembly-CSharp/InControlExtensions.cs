using System;
using System.Collections.Generic;
using System.Text;
using InControl;
using Platform;

// Token: 0x02000550 RID: 1360
public static class InControlExtensions
{
	// Token: 0x06002C67 RID: 11367 RVA: 0x00119014 File Offset: 0x00117214
	public static string GetBindingString(this PlayerAction _action, bool _forController, PlayerInputManager.InputStyle _inputStyle = PlayerInputManager.InputStyle.Undefined, XUiUtils.EmptyBindingStyle _emptyStyle = XUiUtils.EmptyBindingStyle.LocalizedNone, XUiUtils.DisplayStyle _displayStyle = XUiUtils.DisplayStyle.Plain, bool _isCustomDisplayStyle = false, string _customDisplayStyleString = null)
	{
		if (_action != null)
		{
			string name = _action.Name;
			if (!(name == "GUI Action Up"))
			{
				if (!(name == "GUI Action Down"))
				{
					if (!(name == "GUI Action Left"))
					{
						if (name == "GUI Action Right")
						{
							if (_forController)
							{
								return LocalPlayerUI.primaryUI.playerInput.GUIActions.Cancel.GetBindingOfType(true).GetBindingSourceString(_inputStyle, _emptyStyle, _displayStyle, _isCustomDisplayStyle, _customDisplayStyleString);
							}
							return LocalPlayerUI.primaryUI.playerInput.GUIActions.DPad_Right.GetBindingOfType(false).GetBindingSourceString(_inputStyle, _emptyStyle, _displayStyle, _isCustomDisplayStyle, _customDisplayStyleString);
						}
					}
					else
					{
						if (_forController)
						{
							return LocalPlayerUI.primaryUI.playerInput.GUIActions.HalfStack.GetBindingOfType(true).GetBindingSourceString(_inputStyle, _emptyStyle, _displayStyle, _isCustomDisplayStyle, _customDisplayStyleString);
						}
						return LocalPlayerUI.primaryUI.playerInput.GUIActions.DPad_Left.GetBindingOfType(false).GetBindingSourceString(_inputStyle, _emptyStyle, _displayStyle, _isCustomDisplayStyle, _customDisplayStyleString);
					}
				}
				else
				{
					if (_forController)
					{
						return LocalPlayerUI.primaryUI.playerInput.GUIActions.Submit.GetBindingOfType(true).GetBindingSourceString(_inputStyle, _emptyStyle, _displayStyle, _isCustomDisplayStyle, _customDisplayStyleString);
					}
					return LocalPlayerUI.primaryUI.playerInput.GUIActions.DPad_Down.GetBindingOfType(false).GetBindingSourceString(_inputStyle, _emptyStyle, _displayStyle, _isCustomDisplayStyle, _customDisplayStyleString);
				}
			}
			else
			{
				if (_forController)
				{
					return LocalPlayerUI.primaryUI.playerInput.GUIActions.Inspect.GetBindingOfType(true).GetBindingSourceString(_inputStyle, _emptyStyle, _displayStyle, _isCustomDisplayStyle, _customDisplayStyleString);
				}
				return LocalPlayerUI.primaryUI.playerInput.GUIActions.DPad_Up.GetBindingOfType(false).GetBindingSourceString(_inputStyle, _emptyStyle, _displayStyle, _isCustomDisplayStyle, _customDisplayStyleString);
			}
		}
		return _action.GetBindingOfType(_forController).GetBindingSourceString(_inputStyle, _emptyStyle, _displayStyle, _isCustomDisplayStyle, _customDisplayStyleString);
	}

	// Token: 0x06002C68 RID: 11368 RVA: 0x001191D0 File Offset: 0x001173D0
	[PublicizedFrom(EAccessModifier.Private)]
	public static string GetBindingSourceString(this BindingSource _bs, PlayerInputManager.InputStyle _inputStyle = PlayerInputManager.InputStyle.Undefined, XUiUtils.EmptyBindingStyle _emptyStyle = XUiUtils.EmptyBindingStyle.LocalizedNone, XUiUtils.DisplayStyle _displayStyle = XUiUtils.DisplayStyle.Plain, bool _isCustomDisplayStyle = false, string _customDisplayStyleString = null)
	{
		string text;
		if (_bs == null)
		{
			switch (_emptyStyle)
			{
			case XUiUtils.EmptyBindingStyle.EmptyString:
				text = string.Empty;
				break;
			case XUiUtils.EmptyBindingStyle.NullString:
				text = null;
				break;
			case XUiUtils.EmptyBindingStyle.LocalizedUnbound:
				text = InControlExtensions.TryLocalizeButtonName("Unbound");
				break;
			case XUiUtils.EmptyBindingStyle.LocalizedNone:
				text = InControlExtensions.TryLocalizeButtonName("None");
				break;
			default:
				throw new ArgumentOutOfRangeException("_emptyStyle", _emptyStyle, null);
			}
			return text;
		}
		switch (_bs.BindingSourceType)
		{
		case BindingSourceType.DeviceBindingSource:
			text = InControlExtensions.GetGamepadSourceString(((DeviceBindingSource)_bs).Control);
			break;
		case BindingSourceType.KeyBindingSource:
			text = InControlExtensions.GetKeyboardSourceString((KeyBindingSource)_bs, _displayStyle, _isCustomDisplayStyle, _customDisplayStyleString);
			break;
		case BindingSourceType.MouseBindingSource:
			text = InControlExtensions.GetMouseSourceString(((MouseBindingSource)_bs).Control);
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		return text;
	}

	// Token: 0x06002C69 RID: 11369 RVA: 0x00119298 File Offset: 0x00117498
	[PublicizedFrom(EAccessModifier.Private)]
	public static string GetKeyboardSourceString(KeyBindingSource _kbs, XUiUtils.DisplayStyle _displayStyle, bool _isCustomDisplayStyle, string _customDisplayStyleString)
	{
		KeyCombo control = _kbs.Control;
		InControlExtensions.bindingToString.Clear();
		for (int i = 0; i < control.IncludeCount; i++)
		{
			if (i > 0)
			{
				InControlExtensions.bindingToString.Append(" + ");
			}
			string localizedName = control.GetInclude(i).GetLocalizedName();
			if (_isCustomDisplayStyle)
			{
				InControlExtensions.bindingToString.Append(_customDisplayStyleString.Replace("###", localizedName));
			}
			else
			{
				switch (_displayStyle)
				{
				case XUiUtils.DisplayStyle.Plain:
					InControlExtensions.bindingToString.Append(localizedName);
					break;
				case XUiUtils.DisplayStyle.KeyboardWithAngleBrackets:
					InControlExtensions.bindingToString.Append("<");
					InControlExtensions.bindingToString.Append(localizedName);
					InControlExtensions.bindingToString.Append(">");
					break;
				case XUiUtils.DisplayStyle.KeyboardWithParentheses:
					InControlExtensions.bindingToString.Append("( ");
					InControlExtensions.bindingToString.Append(localizedName);
					InControlExtensions.bindingToString.Append(" )");
					break;
				case XUiUtils.DisplayStyle.KeyboardWithBrackets:
					InControlExtensions.bindingToString.Append("[");
					InControlExtensions.bindingToString.Append(localizedName);
					InControlExtensions.bindingToString.Append("]");
					break;
				default:
					throw new ArgumentOutOfRangeException("_displayStyle");
				}
			}
		}
		return InControlExtensions.bindingToString.ToString();
	}

	// Token: 0x06002C6A RID: 11370 RVA: 0x001193DC File Offset: 0x001175DC
	[PublicizedFrom(EAccessModifier.Private)]
	public static string GetMouseSourceString(Mouse _control)
	{
		string text;
		switch (_control)
		{
		case Mouse.LeftButton:
			text = "[sp=Mouse_LeftButton_Large]";
			break;
		case Mouse.RightButton:
			text = "[sp=Mouse_RightButton_Large]";
			break;
		case Mouse.MiddleButton:
			text = "[sp=Mouse_MiddleButton_Large]";
			break;
		default:
			text = _control.GetLocalizedName();
			break;
		}
		return text;
	}

	// Token: 0x06002C6B RID: 11371 RVA: 0x00119420 File Offset: 0x00117620
	public static string GetGamepadSourceString(InputControlType _control)
	{
		PlayerInputManager.InputStyle inputStyle = PlayerInputManager.InputStyleFromSelectedIconStyle();
		InputControlType inputControlType;
		switch (_control)
		{
		case InputControlType.Back:
		case InputControlType.Select:
		case InputControlType.View:
		case InputControlType.Minus:
			break;
		case InputControlType.Start:
		case InputControlType.Options:
		case InputControlType.Menu:
		case InputControlType.Plus:
			if (inputStyle != PlayerInputManager.InputStyle.PS4)
			{
				if (inputStyle != PlayerInputManager.InputStyle.XB1)
				{
					inputControlType = _control;
				}
				else
				{
					inputControlType = InputControlType.Menu;
				}
			}
			else
			{
				inputControlType = InputControlType.Options;
			}
			_control = inputControlType;
			goto IL_008C;
		case InputControlType.System:
		case InputControlType.Pause:
		case InputControlType.Share:
		case InputControlType.Home:
		case InputControlType.Power:
		case InputControlType.Capture:
		case InputControlType.Assistant:
			goto IL_008C;
		default:
			if (_control != InputControlType.TouchPadButton)
			{
				goto IL_008C;
			}
			break;
		}
		if (inputStyle != PlayerInputManager.InputStyle.PS4)
		{
			if (inputStyle != PlayerInputManager.InputStyle.XB1)
			{
				inputControlType = _control;
			}
			else
			{
				inputControlType = InputControlType.Back;
			}
		}
		else
		{
			inputControlType = InputControlType.TouchPadButton;
		}
		_control = inputControlType;
		IL_008C:
		string text;
		if (inputStyle != PlayerInputManager.InputStyle.PS4)
		{
			if (inputStyle != PlayerInputManager.InputStyle.XB1)
			{
				text = "[sp={_control.ToStringCached ()}]";
			}
			else
			{
				text = "[sp=XB_Button_" + _control.ToStringCached<InputControlType>() + "]";
			}
		}
		else
		{
			text = "[sp=PS5_Button_" + _control.ToStringCached<InputControlType>() + "]";
		}
		return text;
	}

	// Token: 0x06002C6C RID: 11372 RVA: 0x001194FA File Offset: 0x001176FA
	public static string GetBlankDPadSourceString()
	{
		if (PlayerInputManager.InputStyleFromSelectedIconStyle() == PlayerInputManager.InputStyle.PS4)
		{
			return "[sp=PS5_Button_DPadBlank]";
		}
		return "[sp=XB_Button_DPadBlank]";
	}

	// Token: 0x06002C6D RID: 11373 RVA: 0x00119510 File Offset: 0x00117710
	[PublicizedFrom(EAccessModifier.Private)]
	public static string GetLocalizedName(this Key _key)
	{
		string name = UnityKeyboardProvider.KeyMappings[(int)_key].Name;
		string text = "inpButton" + name.Replace(" ", "");
		string text2 = Localization.Get(text, false, null);
		if (text2 != text)
		{
			return text2;
		}
		return name;
	}

	// Token: 0x06002C6E RID: 11374 RVA: 0x00119560 File Offset: 0x00117760
	[PublicizedFrom(EAccessModifier.Private)]
	public static string GetLocalizedName(this Mouse _key)
	{
		string text = "inpButton" + _key.ToStringCached<Mouse>();
		string text2 = Localization.Get(text, false, null);
		if (text2 != text)
		{
			return text2;
		}
		return _key.ToStringCached<Mouse>();
	}

	// Token: 0x06002C6F RID: 11375 RVA: 0x00119598 File Offset: 0x00117798
	public static string TryLocalizeButtonName(string _buttonName)
	{
		string text = "inpButton" + _buttonName.Replace(" ", "");
		string text2 = Localization.Get(text, false, null);
		if (text2 != text)
		{
			return text2;
		}
		return _buttonName;
	}

	// Token: 0x06002C70 RID: 11376 RVA: 0x001195D8 File Offset: 0x001177D8
	public static void UnbindBindingsOfType(this PlayerAction _action, bool _controller)
	{
		foreach (BindingSource bindingSource in _action.Bindings)
		{
			if (_controller == (bindingSource.BindingSourceType == BindingSourceType.DeviceBindingSource))
			{
				_action.RemoveBinding(bindingSource);
			}
		}
	}

	// Token: 0x06002C71 RID: 11377 RVA: 0x00119634 File Offset: 0x00117834
	public static void UnbindBindingsOfType(this PlayerAction _action, BindingSourceType _bindingType)
	{
		foreach (BindingSource bindingSource in _action.Bindings)
		{
			if (bindingSource.BindingSourceType == _bindingType)
			{
				_action.RemoveBinding(bindingSource);
			}
		}
	}

	// Token: 0x06002C72 RID: 11378 RVA: 0x0011968C File Offset: 0x0011788C
	public static PlayerAction BindingUsed(this PlayerActionSet _actionSet, BindingSource _binding)
	{
		if (_binding == null)
		{
			return null;
		}
		int count = _actionSet.Actions.Count;
		for (int i = 0; i < count; i++)
		{
			if (_actionSet.Actions[i].HasBinding(_binding))
			{
				return _actionSet.Actions[i];
			}
		}
		return null;
	}

	// Token: 0x06002C73 RID: 11379 RVA: 0x001196E0 File Offset: 0x001178E0
	public static BindingSource GetBindingOfType(this PlayerAction _action, bool _forController = false)
	{
		if (_action == null)
		{
			return null;
		}
		foreach (BindingSource bindingSource in _action.Bindings)
		{
			bool flag = BindingSourceType.KeyBindingSource == bindingSource.BindingSourceType || BindingSourceType.MouseBindingSource == bindingSource.BindingSourceType;
			if (_forController != flag)
			{
				return bindingSource;
			}
		}
		return null;
	}

	// Token: 0x06002C74 RID: 11380 RVA: 0x0011974C File Offset: 0x0011794C
	public static string GetNameForControlType(this InputDeviceProfile _profile, InputControlType _controlType)
	{
		foreach (InputControlMapping inputControlMapping in _profile.AnalogMappings)
		{
			if (inputControlMapping.Target == _controlType)
			{
				return inputControlMapping.Target.ToStringCached<InputControlType>();
			}
		}
		foreach (InputControlMapping inputControlMapping2 in _profile.ButtonMappings)
		{
			if (inputControlMapping2.Target == _controlType)
			{
				return inputControlMapping2.Target.ToStringCached<InputControlType>();
			}
		}
		return null;
	}

	// Token: 0x06002C75 RID: 11381 RVA: 0x001197B8 File Offset: 0x001179B8
	public static void GetBoundAction(this InputControlType _controlType, PlayerActionSet[] _actionSets, IList<PlayerAction> _result)
	{
		_result.Clear();
		if (_actionSets == null || _actionSets.Length == 0)
		{
			return;
		}
		for (int i = 0; i < _actionSets.Length; i++)
		{
			foreach (PlayerAction playerAction in _actionSets[i].Actions)
			{
				foreach (BindingSource bindingSource in playerAction.UnfilteredBindings)
				{
					if (bindingSource.BindingSourceType == BindingSourceType.DeviceBindingSource && ((DeviceBindingSource)bindingSource).Control == _controlType)
					{
						_result.Add(playerAction);
					}
				}
			}
		}
	}

	// Token: 0x04002218 RID: 8728
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly StringBuilder bindingToString = new StringBuilder();
}

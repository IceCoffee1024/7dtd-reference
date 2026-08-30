using System;
using System.Collections.Generic;
using System.Text;
using InControl;
using Platform;

// Token: 0x02000555 RID: 1365
public static class InControlExtensions
{
	// Token: 0x06002C91 RID: 11409 RVA: 0x0011A088 File Offset: 0x00118288
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

	// Token: 0x06002C92 RID: 11410 RVA: 0x0011A244 File Offset: 0x00118444
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

	// Token: 0x06002C93 RID: 11411 RVA: 0x0011A30C File Offset: 0x0011850C
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

	// Token: 0x06002C94 RID: 11412 RVA: 0x0011A450 File Offset: 0x00118650
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

	// Token: 0x06002C95 RID: 11413 RVA: 0x0011A494 File Offset: 0x00118694
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

	// Token: 0x06002C96 RID: 11414 RVA: 0x0011A56E File Offset: 0x0011876E
	public static string GetBlankDPadSourceString()
	{
		if (PlayerInputManager.InputStyleFromSelectedIconStyle() == PlayerInputManager.InputStyle.PS4)
		{
			return "[sp=PS5_Button_DPadBlank]";
		}
		return "[sp=XB_Button_DPadBlank]";
	}

	// Token: 0x06002C97 RID: 11415 RVA: 0x0011A584 File Offset: 0x00118784
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

	// Token: 0x06002C98 RID: 11416 RVA: 0x0011A5D4 File Offset: 0x001187D4
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

	// Token: 0x06002C99 RID: 11417 RVA: 0x0011A60C File Offset: 0x0011880C
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

	// Token: 0x06002C9A RID: 11418 RVA: 0x0011A64C File Offset: 0x0011884C
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

	// Token: 0x06002C9B RID: 11419 RVA: 0x0011A6A8 File Offset: 0x001188A8
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

	// Token: 0x06002C9C RID: 11420 RVA: 0x0011A700 File Offset: 0x00118900
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

	// Token: 0x06002C9D RID: 11421 RVA: 0x0011A754 File Offset: 0x00118954
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

	// Token: 0x06002C9E RID: 11422 RVA: 0x0011A7C0 File Offset: 0x001189C0
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

	// Token: 0x06002C9F RID: 11423 RVA: 0x0011A82C File Offset: 0x00118A2C
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

	// Token: 0x0400224A RID: 8778
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly StringBuilder bindingToString = new StringBuilder();
}

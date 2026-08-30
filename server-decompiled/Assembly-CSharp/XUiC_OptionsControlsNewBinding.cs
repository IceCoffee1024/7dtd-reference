using System;
using System.Collections;
using System.Collections.Generic;
using InControl;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000DA8 RID: 3496
[UnityEngine.Scripting.Preserve]
public class XUiC_OptionsControlsNewBinding : XUiController
{
	// Token: 0x06006ABC RID: 27324 RVA: 0x0029BF08 File Offset: 0x0029A108
	public override void Init()
	{
		base.Init();
		XUiC_OptionsControlsNewBinding.id = base.WindowGroup.Id;
		((XUiC_SimpleButton)base.GetChildById("btnCancel")).OnPressed += this.BtnCancel_OnPressed;
		((XUiC_SimpleButton)base.GetChildById("btnNewBinding")).OnPressed += this.BtnNewBinding_OnPressed;
		((XUiC_SimpleButton)base.GetChildById("btnGrabBinding")).OnPressed += this.BtnGrabBinding_OnPressed;
	}

	// Token: 0x06006ABD RID: 27325 RVA: 0x00089495 File Offset: 0x00087695
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCancel_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x06006ABE RID: 27326 RVA: 0x0029BF8E File Offset: 0x0029A18E
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnNewBinding_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.startGetBinding();
	}

	// Token: 0x06006ABF RID: 27327 RVA: 0x0029BF98 File Offset: 0x0029A198
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnGrabBinding_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.conflictingAction.ClearInputState();
		this.conflictingAction.RemoveBinding(this.binding);
		this.action.UnbindBindingsOfType(this.forController);
		this.action.AddBinding(this.binding);
		ThreadManager.StartCoroutine(this.closeNextFrame());
	}

	// Token: 0x06006AC0 RID: 27328 RVA: 0x0029BFF0 File Offset: 0x0029A1F0
	[PublicizedFrom(EAccessModifier.Private)]
	public void startGetBinding()
	{
		this.xui.playerUI.windowManager.IsInputLocked = true;
		InputUtils.EnableAllPlayerActions(false);
		GameManager.Instance.SetCursorEnabledOverride(true, false);
		this.rectInUse.IsVisible = false;
		this.conflictingAction = null;
		this.binding = null;
		this.action.Owner.ListenOptions.IncludeUnknownControllers = false;
		this.action.Owner.ListenOptions.IncludeMouseButtons = !this.forController;
		this.action.Owner.ListenOptions.IncludeMouseScrollWheel = !this.forController;
		this.action.Owner.ListenOptions.IncludeKeys = true;
		this.action.Owner.ListenOptions.OnBindingFound = new Func<PlayerAction, BindingSource, bool>(this.onBindingReceived);
		this.action.ListenForBinding();
	}

	// Token: 0x06006AC1 RID: 27329 RVA: 0x0029C0D3 File Offset: 0x0029A2D3
	[PublicizedFrom(EAccessModifier.Private)]
	public void stoppedGetBinding(bool _alsoStopListening = true)
	{
		if (_alsoStopListening)
		{
			this.action.StopListeningForBinding();
		}
		InputUtils.EnableAllPlayerActions(true);
		this.xui.playerUI.windowManager.IsInputLocked = false;
		GameManager.Instance.SetCursorEnabledOverride(false, false);
	}

	// Token: 0x06006AC2 RID: 27330 RVA: 0x0029C10C File Offset: 0x0029A30C
	[PublicizedFrom(EAccessModifier.Private)]
	public bool onBindingReceived(PlayerAction _action, BindingSource _binding)
	{
		if (this.bindingAbortActions.Contains(_binding))
		{
			Log.Out("Abort action pressed, aborting listening for new binding for {0}", new object[] { _action.Name });
			this.stoppedGetBinding(true);
			ThreadManager.StartCoroutine(this.closeNextFrame());
			return false;
		}
		if (this.forController && (_binding is KeyBindingSource || _binding is MouseBindingSource))
		{
			Log.Out("Cannot accept key or mouse for controller binding");
			return false;
		}
		if (!this.forController && _binding is DeviceBindingSource)
		{
			Log.Out("Cannot accept device binding source for keyboard/mouse binding");
			return false;
		}
		foreach (BindingSource bindingSource in this.bindingForbidden)
		{
			if (_binding == bindingSource)
			{
				Log.Out("Binding {0} not allowed", new object[] { _binding.Name });
				return false;
			}
		}
		if (this.forController != (_binding.BindingSourceType == BindingSourceType.DeviceBindingSource))
		{
			Log.Out("New binding ({0}) doesn't match expected input device type ({1})", new object[]
			{
				_binding.BindingSourceType.ToStringCached<BindingSourceType>(),
				this.forController ? BindingSourceType.DeviceBindingSource.ToStringCached<BindingSourceType>() : BindingSourceType.KeyBindingSource.ToStringCached<BindingSourceType>()
			});
			return false;
		}
		if (_action.HasBinding(_binding))
		{
			Log.Out("Binding {0} already bound to the current action {1}", new object[] { _binding.Name, _action.Name });
			this.stoppedGetBinding(true);
			ThreadManager.StartCoroutine(this.closeNextFrame());
			return false;
		}
		if (this.alreadyBound(_binding, _action))
		{
			Log.Out("Binding {0} already bound to the action {1}", new object[]
			{
				_binding.Name,
				this.conflictingAction.Name
			});
			this.stoppedGetBinding(true);
			return false;
		}
		_action.UnbindBindingsOfType(this.forController);
		this.stoppedGetBinding(false);
		ThreadManager.StartCoroutine(this.closeNextFrame());
		return true;
	}

	// Token: 0x06006AC3 RID: 27331 RVA: 0x0029C2B8 File Offset: 0x0029A4B8
	[PublicizedFrom(EAccessModifier.Private)]
	public bool alreadyBound(BindingSource _binding, PlayerAction _selfAction)
	{
		PlayerAction playerAction = _selfAction.Owner.BindingUsed(_binding);
		if (playerAction == null && _selfAction.Owner.UserData != null)
		{
			PlayerActionsBase[] bindingsConflictWithSet = ((PlayerActionData.ActionSetUserData)_selfAction.Owner.UserData).bindingsConflictWithSet;
			for (int i = 0; i < bindingsConflictWithSet.Length; i++)
			{
				playerAction = bindingsConflictWithSet[i].BindingUsed(_binding);
				if (playerAction != null)
				{
					break;
				}
			}
		}
		if (playerAction == null)
		{
			return false;
		}
		PlayerActionData.ActionUserData actionUserData = (PlayerActionData.ActionUserData)_selfAction.UserData;
		PlayerActionData.ActionUserData actionUserData2 = (PlayerActionData.ActionUserData)playerAction.UserData;
		if (actionUserData.allowMultipleBindings || actionUserData2.allowMultipleBindings)
		{
			PlayerActionSet playerActionSet = this.xui.playerUI.playerInput;
			if (!this.xui.playerUI.playerInput.Actions.Contains(_selfAction) || !this.xui.playerUI.playerInput.Actions.Contains(playerAction))
			{
				if (this.xui.playerUI.playerInput.GUIActions.Actions.Contains(_selfAction) && this.xui.playerUI.playerInput.GUIActions.Actions.Contains(playerAction))
				{
					playerActionSet = this.xui.playerUI.playerInput.GUIActions;
				}
				else if (this.xui.playerUI.playerInput.VehicleActions.Actions.Contains(_selfAction) && this.xui.playerUI.playerInput.VehicleActions.Actions.Contains(playerAction))
				{
					playerActionSet = this.xui.playerUI.playerInput.VehicleActions;
				}
				else
				{
					if (!this.xui.playerUI.playerInput.PermanentActions.Actions.Contains(_selfAction) || !this.xui.playerUI.playerInput.PermanentActions.Actions.Contains(playerAction))
					{
						return false;
					}
					playerActionSet = this.xui.playerUI.playerInput.PermanentActions;
				}
			}
			bool flag = false;
			foreach (PlayerAction playerAction2 in playerActionSet.Actions)
			{
				if (playerAction2 != playerAction && playerAction2 != _selfAction && playerAction2.Bindings.Contains(_binding))
				{
					flag = true;
					playerAction = playerAction2;
					break;
				}
			}
			if (!flag)
			{
				return false;
			}
		}
		this.rectInUse.IsVisible = true;
		this.binding = playerAction.GetBindingOfType(this.forController);
		this.conflictingAction = playerAction;
		if (this.forController)
		{
			this.lblInUseBy.Text = string.Format(Localization.Get("xuiNewBindingConflictingAction_Controller", false, null), playerAction.GetBindingString(true, PlatformManager.NativePlatform.Input.CurrentControllerInputStyle, XUiUtils.EmptyBindingStyle.LocalizedNone, XUiUtils.DisplayStyle.Plain, false, null), ((PlayerActionData.ActionUserData)playerAction.UserData).LocalizedName);
		}
		else
		{
			this.lblInUseBy.Text = string.Format(Localization.Get("xuiNewBindingConflictingAction", false, null), playerAction.GetBindingString(this.forController, PlayerInputManager.InputStyle.Undefined, XUiUtils.EmptyBindingStyle.LocalizedNone, XUiUtils.DisplayStyle.Plain, false, null), ((PlayerActionData.ActionUserData)playerAction.UserData).LocalizedName);
		}
		base.GetChildById("btnNewBinding").SelectCursorElement(true, false);
		this.lblInUseBy.ToolTip = ((PlayerActionData.ActionUserData)playerAction.UserData).LocalizedDescription;
		return true;
	}

	// Token: 0x06006AC4 RID: 27332 RVA: 0x0029C5F8 File Offset: 0x0029A7F8
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator closeNextFrame()
	{
		yield return null;
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		yield break;
	}

	// Token: 0x06006AC5 RID: 27333 RVA: 0x0029C607 File Offset: 0x0029A807
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator startNextFrame()
	{
		yield return null;
		this.startGetBinding();
		yield break;
	}

	// Token: 0x06006AC6 RID: 27334 RVA: 0x0029C618 File Offset: 0x0029A818
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.forController)
		{
			this.lblAction.Text = string.Format(Localization.Get("xuiNewBindingCurrent_Controller", false, null), ((PlayerActionData.ActionUserData)this.action.UserData).LocalizedName, this.action.GetBindingString(true, PlayerInputManager.InputStyleFromSelectedIconStyle(), XUiUtils.EmptyBindingStyle.LocalizedNone, XUiUtils.DisplayStyle.Plain, false, null));
		}
		else
		{
			this.lblAction.Text = string.Format(Localization.Get("xuiNewBindingCurrent", false, null), ((PlayerActionData.ActionUserData)this.action.UserData).LocalizedName, this.action.GetBindingString(this.forController, PlayerInputManager.InputStyle.Undefined, XUiUtils.EmptyBindingStyle.LocalizedNone, XUiUtils.DisplayStyle.Plain, false, null));
		}
		PlayerInputManager.InputStyle inputStyle = PlayerInputManager.InputStyleFromSelectedIconStyle();
		string text2;
		if (this.forController)
		{
			string text = ((inputStyle == PlayerInputManager.InputStyle.PS4) ? "[sp=PS5_Button_Options] / ESC" : "[sp=XB_Button_Back] / ESC");
			text2 = string.Format(Localization.Get("xuiNewBindingAbort_Controller", false, null), text);
		}
		else
		{
			string text3 = ((inputStyle == PlayerInputManager.InputStyle.PS4) ? "ESC / [sp=PS5_Button_Options]" : "ESC / [sp=XB_Button_Back]");
			text2 = string.Format(Localization.Get("xuiNewBindingAbort_Controller", false, null), text3);
		}
		this.lblAbort.Text = text2;
		ThreadManager.StartCoroutine(this.startNextFrame());
	}

	// Token: 0x06006AC7 RID: 27335 RVA: 0x0029C734 File Offset: 0x0029A934
	public static void GetNewBinding(XUi _xuiInstance, PlayerAction _action, bool _forController = false)
	{
		XUiC_OptionsControlsNewBinding childByType = _xuiInstance.FindWindowGroupByName(XUiC_OptionsControlsNewBinding.id).GetChildByType<XUiC_OptionsControlsNewBinding>();
		childByType.action = _action;
		childByType.forController = _forController;
		_xuiInstance.playerUI.windowManager.Open(childByType.WindowGroup, false);
	}

	// Token: 0x04005166 RID: 20838
	[PublicizedFrom(EAccessModifier.Private)]
	public static string id;

	// Token: 0x04005167 RID: 20839
	[XuiBindComponent("forAction", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Label lblAction;

	// Token: 0x04005168 RID: 20840
	[XuiBindComponent("newBindingAbort", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Label lblAbort;

	// Token: 0x04005169 RID: 20841
	[XuiBindComponent("inUse", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Rect rectInUse;

	// Token: 0x0400516A RID: 20842
	[XuiBindComponent("inUseBy", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Label lblInUseBy;

	// Token: 0x0400516B RID: 20843
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerAction action;

	// Token: 0x0400516C RID: 20844
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerAction conflictingAction;

	// Token: 0x0400516D RID: 20845
	[PublicizedFrom(EAccessModifier.Private)]
	public BindingSource binding;

	// Token: 0x0400516E RID: 20846
	[PublicizedFrom(EAccessModifier.Private)]
	public bool forController;

	// Token: 0x0400516F RID: 20847
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<BindingSource> bindingAbortActions = new List<BindingSource>
	{
		new DeviceBindingSource(InputControlType.Back),
		new DeviceBindingSource(InputControlType.Options),
		new DeviceBindingSource(InputControlType.View),
		new DeviceBindingSource(InputControlType.Minus),
		new KeyBindingSource(new Key[] { Key.Escape })
	};

	// Token: 0x04005170 RID: 20848
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly BindingSource[] bindingForbidden = new BindingSource[]
	{
		new KeyBindingSource(new Key[] { Key.F1 }),
		new KeyBindingSource(new Key[] { Key.F2 }),
		new KeyBindingSource(new Key[] { Key.F3 }),
		new KeyBindingSource(new Key[] { Key.F4 }),
		new KeyBindingSource(new Key[] { Key.F5 }),
		new KeyBindingSource(new Key[] { Key.F6 }),
		new KeyBindingSource(new Key[] { Key.F7 }),
		new KeyBindingSource(new Key[] { Key.F8 }),
		new KeyBindingSource(new Key[] { Key.F9 }),
		new KeyBindingSource(new Key[] { Key.F10 }),
		new KeyBindingSource(new Key[] { Key.F11 }),
		new KeyBindingSource(new Key[] { Key.F12 }),
		new DeviceBindingSource(InputControlType.Start),
		new DeviceBindingSource(InputControlType.Back),
		new DeviceBindingSource(InputControlType.LeftStickUp),
		new DeviceBindingSource(InputControlType.LeftStickDown),
		new DeviceBindingSource(InputControlType.LeftStickLeft),
		new DeviceBindingSource(InputControlType.LeftStickRight),
		new DeviceBindingSource(InputControlType.RightStickUp),
		new DeviceBindingSource(InputControlType.RightStickDown),
		new DeviceBindingSource(InputControlType.RightStickLeft),
		new DeviceBindingSource(InputControlType.RightStickRight),
		new DeviceBindingSource(InputControlType.Share),
		new DeviceBindingSource(InputControlType.Menu),
		new DeviceBindingSource(InputControlType.View),
		new DeviceBindingSource(InputControlType.Options),
		new DeviceBindingSource(InputControlType.Plus),
		new DeviceBindingSource(InputControlType.Minus),
		new DeviceBindingSource(InputControlType.TouchPadButton),
		new DeviceBindingSource(InputControlType.Select),
		new DeviceBindingSource(InputControlType.LeftStickY),
		new DeviceBindingSource(InputControlType.LeftStickX),
		new DeviceBindingSource(InputControlType.RightStickY),
		new DeviceBindingSource(InputControlType.RightStickX),
		new DeviceBindingSource(InputControlType.Create),
		new DeviceBindingSource(InputControlType.Guide),
		new DeviceBindingSource(InputControlType.Home),
		new DeviceBindingSource(InputControlType.Mute),
		new DeviceBindingSource(InputControlType.Capture)
	};
}

using System;
using System.Collections.Generic;
using InControl;
using Platform;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02001206 RID: 4614
public class GUIWindowManager : MonoBehaviour
{
	// Token: 0x06009332 RID: 37682 RVA: 0x0037B038 File Offset: 0x00379238
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Awake()
	{
		this.nguiWindowManager = base.GetComponent<NGUIWindowManager>();
		this.playerUI = base.GetComponent<LocalPlayerUI>();
	}

	// Token: 0x06009333 RID: 37683 RVA: 0x0037B054 File Offset: 0x00379254
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnGUI()
	{
		for (int i = 0; i < this.windowsToOpen.Count; i++)
		{
			GUIWindow guiwindow = this.windowsToOpen[i];
			guiwindow.isShowing = true;
			this.openWindows.Add(guiwindow);
			this.topmostWindow = guiwindow;
		}
		this.windowsToOpen.Clear();
		this.modalWindow = null;
		for (int j = 0; j < this.openWindows.Count; j++)
		{
			GUIWindow guiwindow2 = this.openWindows[j];
			if (guiwindow2.isModal)
			{
				this.modalWindow = guiwindow2;
				break;
			}
		}
		this.cursorWindowOpen = false;
		for (int k = 0; k < this.openWindows.Count; k++)
		{
			GUIWindow guiwindow3 = this.openWindows[k];
			if (guiwindow3.isShowing)
			{
				this.cursorWindowOpen |= guiwindow3.alwaysUsesMouseCursor;
				GUI.matrix = Matrix4x4.identity;
				guiwindow3.OnGUI();
			}
		}
		GUI.enabled = true;
		List<GUIWindow> list = this.windowsToRemove;
		list.Clear();
		for (int l = 0; l < this.openWindows.Count; l++)
		{
			GUIWindow guiwindow4 = this.openWindows[l];
			if (!guiwindow4.isShowing)
			{
				list.Add(guiwindow4);
				this.topmostWindow = ((this.openWindows.Count > 0) ? this.openWindows[this.openWindows.Count - 1] : null);
			}
		}
		for (int m = 0; m < list.Count; m++)
		{
			GUIWindow guiwindow5 = list[m];
			this.openWindows.Remove(guiwindow5);
		}
	}

	// Token: 0x06009334 RID: 37684 RVA: 0x0037B1F0 File Offset: 0x003793F0
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Update()
	{
		for (int i = 0; i < this.openWindows.Count; i++)
		{
			this.openWindows[i].Update();
		}
		if (this.lastActionClicked.Count != 0)
		{
			for (int j = 0; j < this.lastActionClicked.Count; j++)
			{
				NGuiAction nguiAction = this.lastActionClicked[j];
				PlayerAction hotkey = nguiAction.GetHotkey();
				if (hotkey != null && hotkey.WasReleased)
				{
					nguiAction.OnRelease();
					this.actionsToClear.Add(nguiAction);
				}
			}
			for (int k = 0; k < this.actionsToClear.Count; k++)
			{
				NGuiAction nguiAction2 = this.actionsToClear[k];
				this.lastActionClicked.Remove(nguiAction2);
			}
			this.actionsToClear.Clear();
		}
		if (this.IsInputActive())
		{
			if (this.playerUI.playerInput != null && this.playerUI.playerInput.PermanentActions.Cancel.WasPressed && UIInput.selection != null)
			{
				UIInput.selection.RemoveFocus();
			}
			return;
		}
		if (!this.IsInputLocked)
		{
			List<NGuiAction> list = this.actionsForGlobalHotkeys;
			list.Clear();
			for (int l = 0; l < this.globalActions.Count; l++)
			{
				PlayerAction hotkey2 = this.globalActions[l].GetHotkey();
				if (hotkey2 != null && (((this.globalActions[l].KeyMode & NGuiAction.EnumKeyMode.FireOnRelease) == NGuiAction.EnumKeyMode.FireOnRelease && hotkey2.WasReleased) || ((this.globalActions[l].KeyMode & NGuiAction.EnumKeyMode.FireOnPress) == NGuiAction.EnumKeyMode.FireOnPress && hotkey2.WasPressed) || ((this.globalActions[l].KeyMode & NGuiAction.EnumKeyMode.FireOnRepeat) == NGuiAction.EnumKeyMode.FireOnRepeat && hotkey2.WasRepeated)))
				{
					list.Add(this.globalActions[l]);
				}
			}
			if (list.Count > 0)
			{
				for (int m = 0; m < list.Count; m++)
				{
					NGuiAction nguiAction3 = list[m];
					nguiAction3.OnClick();
					this.lastActionClicked.Add(nguiAction3);
				}
				list.Clear();
			}
			if (this.playerUI.playerInput != null && this.playerUI.playerInput.PermanentActions.Cancel.WasPressed && !this.IsWindowOpen("popupGroup") && this.modalWindow != null && this.modalWindow.isEscClosable)
			{
				this.CloseAllOpenModalWindows(null, true);
			}
		}
	}

	// Token: 0x06009335 RID: 37685 RVA: 0x0037B468 File Offset: 0x00379668
	public bool IsInputActive()
	{
		return (UIInput.selection != null && UIInput.selection.gameObject.activeInHierarchy) || (this.topmostWindow != null && this.topmostWindow.isInputActive) || this.IsUGUIInputActive() || GUIWindowConsole.IsOpen();
	}

	// Token: 0x06009336 RID: 37686 RVA: 0x0037B4B8 File Offset: 0x003796B8
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsUGUIInputActive()
	{
		EventSystem current = EventSystem.current;
		if (current == null)
		{
			return false;
		}
		GameObject currentSelectedGameObject = current.currentSelectedGameObject;
		if (currentSelectedGameObject == null)
		{
			return false;
		}
		InputField inputField;
		if (currentSelectedGameObject.TryGetComponent<InputField>(out inputField))
		{
			return inputField.isFocused;
		}
		TMP_InputField tmp_InputField;
		return currentSelectedGameObject.TryGetComponent<TMP_InputField>(out tmp_InputField) && tmp_InputField.isFocused;
	}

	// Token: 0x06009337 RID: 37687 RVA: 0x0037B50B File Offset: 0x0037970B
	public bool IsKeyShortcutsAllowed()
	{
		return !this.IsInputLocked && (PlatformManager.NativePlatform.Input.CurrentInputStyle != PlayerInputManager.InputStyle.Keyboard || !this.IsInputActive());
	}

	// Token: 0x06009338 RID: 37688 RVA: 0x0037B534 File Offset: 0x00379734
	public void Add(GUIWindow _window)
	{
		this.nameToWindowMap.Add(_window.Id, _window);
		_window.windowManager = this;
		if (this.nguiWindowManager == null)
		{
			this.nguiWindowManager = base.GetComponent<NGUIWindowManager>();
		}
		if (this.playerUI == null)
		{
			this.playerUI = base.GetComponent<LocalPlayerUI>();
		}
		_window.playerUI = this.playerUI;
	}

	// Token: 0x06009339 RID: 37689 RVA: 0x0037B59A File Offset: 0x0037979A
	public void Remove(GUIWindow _w)
	{
		if (_w.isShowing)
		{
			this.Close(_w, false);
		}
		_w.Cleanup();
		this.nameToWindowMap.Remove(_w.Id);
	}

	// Token: 0x0600933A RID: 37690 RVA: 0x0037B5C4 File Offset: 0x003797C4
	public GUIWindow GetWindow(string _windowName)
	{
		GUIWindow guiwindow;
		if (!this.nameToWindowMap.TryGetValue(_windowName, out guiwindow))
		{
			Log.Warning("GUIWindowManager.GetWindow: Window \"{0}\" unknown!", new object[] { _windowName });
			return null;
		}
		return guiwindow;
	}

	// Token: 0x0600933B RID: 37691 RVA: 0x0037B5F8 File Offset: 0x003797F8
	public bool TryGetWindow(string _windowName, out GUIWindow _window)
	{
		return this.nameToWindowMap.TryGetValue(_windowName, out _window);
	}

	// Token: 0x0600933C RID: 37692 RVA: 0x0037B607 File Offset: 0x00379807
	public T GetWindow<T>(string _windowName) where T : GUIWindow
	{
		return (T)((object)this.nameToWindowMap[_windowName]);
	}

	// Token: 0x0600933D RID: 37693 RVA: 0x0037B61C File Offset: 0x0037981C
	public void SwitchVisible(string _windowName, bool _modal = true)
	{
		GUIWindow guiwindow;
		if (this.nameToWindowMap.TryGetValue(_windowName, out guiwindow))
		{
			this.SwitchVisible(guiwindow, _modal);
		}
	}

	// Token: 0x0600933E RID: 37694 RVA: 0x0037B641 File Offset: 0x00379841
	public void SwitchVisible(GUIWindow _guiWindow, bool _modal = true)
	{
		if ((!_modal || _guiWindow.isModal) && _guiWindow.isShowing)
		{
			this.Close(_guiWindow, false);
			return;
		}
		this.Open(_guiWindow, _modal);
	}

	// Token: 0x0600933F RID: 37695 RVA: 0x0037B668 File Offset: 0x00379868
	public bool CloseAllOpenModalWindows(string _exceptWindowName)
	{
		GUIWindow guiwindow;
		this.nameToWindowMap.TryGetValue(_exceptWindowName, out guiwindow);
		return this.CloseAllOpenModalWindows(guiwindow, false);
	}

	// Token: 0x06009340 RID: 37696 RVA: 0x0037B68C File Offset: 0x0037988C
	public bool CloseAllOpenModalWindows(GUIWindow _exceptWindow = null, bool _fromEsc = false)
	{
		bool flag = false;
		for (int i = 0; i < this.openWindows.Count; i++)
		{
			GUIWindow guiwindow = this.openWindows[i];
			if (guiwindow.isModal && (_exceptWindow == null || _exceptWindow != guiwindow))
			{
				this.Close(guiwindow, _fromEsc);
				flag = true;
			}
		}
		if (this.playerUI.CursorController != null)
		{
			if (this.playerUI.CursorController.navigationTarget != null)
			{
				this.playerUI.CursorController.navigationTarget.Controller.Hovered(false);
			}
			this.playerUI.CursorController.SetNavigationTarget(null);
			this.playerUI.CursorController.SetNavigationLockView(null, null);
		}
		return flag;
	}

	// Token: 0x06009341 RID: 37697 RVA: 0x0037B73C File Offset: 0x0037993C
	public void Open(string _windowName, bool _bModal)
	{
		GUIWindow guiwindow;
		if (!this.nameToWindowMap.TryGetValue(_windowName, out guiwindow))
		{
			Log.Warning("GUIWindowManager.Open: Window \"{0}\" unknown!", new object[] { _windowName });
			Log.Out("Trace: " + StackTraceUtility.ExtractStackTrace());
			return;
		}
		this.openInternal(guiwindow, _bModal, false);
	}

	// Token: 0x06009342 RID: 37698 RVA: 0x0037B78C File Offset: 0x0037998C
	public void Open(string _windowName, bool _bModal, bool _bIsNotEscClosable)
	{
		GUIWindow guiwindow;
		if (!this.nameToWindowMap.TryGetValue(_windowName, out guiwindow))
		{
			Log.Warning("GUIWindowManager.Open: Window \"{0}\" unknown!", new object[] { _windowName });
			Log.Out("Trace: " + StackTraceUtility.ExtractStackTrace());
			return;
		}
		this.openInternal(guiwindow, _bModal, _bIsNotEscClosable);
	}

	// Token: 0x06009343 RID: 37699 RVA: 0x0037B7DB File Offset: 0x003799DB
	public void Open(GUIWindow _w, bool _bModal)
	{
		this.openInternal(_w, _bModal, false);
	}

	// Token: 0x06009344 RID: 37700 RVA: 0x0037B7E6 File Offset: 0x003799E6
	public void Open(GUIWindow _w, bool _bModal, bool _bIsNotEscClosable)
	{
		this.openInternal(_w, _bModal, _bIsNotEscClosable);
	}

	// Token: 0x06009345 RID: 37701 RVA: 0x0037B7F4 File Offset: 0x003799F4
	[PublicizedFrom(EAccessModifier.Private)]
	public void openInternal(GUIWindow _w, bool _bModal, bool _bIsNotEscClosable)
	{
		if (_w == null)
		{
			return;
		}
		if (this.IsFullHUDDisabled())
		{
			return;
		}
		if (this.IsWindowOpen(_w))
		{
			return;
		}
		QuestEventManager.Current.ChangedWindow(_w.Id);
		if (_bModal)
		{
			this.CloseAllOpenModalWindows(null, false);
			int i = 0;
			while (i < this.windowsToOpen.Count)
			{
				GUIWindow guiwindow = this.windowsToOpen[i];
				if (guiwindow.isModal && guiwindow != _w)
				{
					this.windowsToOpen.Remove(guiwindow);
					guiwindow.OnClose();
					if (guiwindow.HasActionSet())
					{
						this.DisableWindowActionSet(guiwindow);
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
		}
		_w.isModal = _bModal;
		if (_w.isShowing)
		{
			return;
		}
		_w.windowManager = this;
		_w.isEscClosable = !_bIsNotEscClosable;
		bool flag = _w.isShowing || this.windowsToOpen.Contains(_w);
		if (!this.openWindows.Contains(_w))
		{
			this.windowsToOpen.Add(_w);
		}
		else
		{
			_w.isShowing = true;
		}
		if (!flag)
		{
			_w.OnOpen();
		}
		if (_w.HasActionSet() && !_w.bActionSetEnabled && (_w.isShowing || this.windowsToOpen.Contains(_w)))
		{
			this.EnableWindowActionSet(_w);
		}
	}

	// Token: 0x06009346 RID: 37702 RVA: 0x0037B918 File Offset: 0x00379B18
	public bool IsWindowOpen(string _wdwID)
	{
		GUIWindow guiwindow;
		return this.nameToWindowMap.TryGetValue(_wdwID, out guiwindow) && this.IsWindowOpen(guiwindow);
	}

	// Token: 0x06009347 RID: 37703 RVA: 0x0037B93E File Offset: 0x00379B3E
	public bool IsWindowOpen(GUIWindow _window)
	{
		return _window != null && (_window.isShowing || this.windowsToOpen.Contains(_window));
	}

	// Token: 0x06009348 RID: 37704 RVA: 0x0037B95B File Offset: 0x00379B5B
	public bool IsModalWindowOpen()
	{
		return this.modalWindow != null;
	}

	// Token: 0x06009349 RID: 37705 RVA: 0x0037B966 File Offset: 0x00379B66
	public GUIWindow GetModalWindow()
	{
		return this.modalWindow;
	}

	// Token: 0x0600934A RID: 37706 RVA: 0x0037B96E File Offset: 0x00379B6E
	public bool IsCursorWindowOpen()
	{
		return this.cursorWindowOpen;
	}

	// Token: 0x0600934B RID: 37707 RVA: 0x0037B978 File Offset: 0x00379B78
	public void Close(string _windowName)
	{
		GUIWindow guiwindow;
		if (this.nameToWindowMap.TryGetValue(_windowName, out guiwindow))
		{
			this.Close(guiwindow, false);
		}
	}

	// Token: 0x0600934C RID: 37708 RVA: 0x0037B9A0 File Offset: 0x00379BA0
	public void Close(GUIWindow _w, bool _fromEsc = false)
	{
		if (_w == null)
		{
			return;
		}
		if (_w.isShowing)
		{
			_w.isShowing = false;
			_w.OnClose();
			if (_fromEsc && !string.IsNullOrEmpty(_w.openWindowOnEsc))
			{
				this.Open(_w.openWindowOnEsc, _w.isModal);
			}
		}
		else if (this.windowsToOpen.Contains(_w))
		{
			this.windowsToOpen.Remove(_w);
			_w.OnClose();
		}
		if (_w.bActionSetEnabled)
		{
			this.DisableWindowActionSet(_w);
		}
	}

	// Token: 0x0600934D RID: 37709 RVA: 0x0037BA1A File Offset: 0x00379C1A
	[PublicizedFrom(EAccessModifier.Private)]
	public void DisableWindowActionSet(GUIWindow _w)
	{
		if (_w.playerUI != null && _w.playerUI.ActionSetManager != null)
		{
			_w.playerUI.ActionSetManager.Pop(_w);
			_w.bActionSetEnabled = false;
		}
	}

	// Token: 0x0600934E RID: 37710 RVA: 0x0037BA4F File Offset: 0x00379C4F
	[PublicizedFrom(EAccessModifier.Private)]
	public void EnableWindowActionSet(GUIWindow _w)
	{
		if (_w.playerUI != null && _w.playerUI.ActionSetManager != null)
		{
			_w.playerUI.ActionSetManager.Push(_w);
			_w.bActionSetEnabled = true;
		}
	}

	// Token: 0x0600934F RID: 37711 RVA: 0x0037BA84 File Offset: 0x00379C84
	public void ResetActionSets()
	{
		ActionSetManager actionSetManager = this.playerUI.ActionSetManager;
		if (actionSetManager == null)
		{
			return;
		}
		actionSetManager.Reset();
		actionSetManager.Push(this.playerUI.playerInput);
		foreach (GUIWindow guiwindow in this.openWindows)
		{
			if (guiwindow.isShowing && guiwindow.HasActionSet())
			{
				this.EnableWindowActionSet(guiwindow);
			}
		}
		foreach (GUIWindow guiwindow2 in this.windowsToOpen)
		{
			if (guiwindow2.bActionSetEnabled)
			{
				this.EnableWindowActionSet(guiwindow2);
			}
		}
	}

	// Token: 0x06009350 RID: 37712 RVA: 0x0037BB58 File Offset: 0x00379D58
	public void RemoveGlobalAction(NGuiAction _action)
	{
		this.globalActions.Remove(_action);
	}

	// Token: 0x06009351 RID: 37713 RVA: 0x0037BB67 File Offset: 0x00379D67
	public void AddGlobalAction(NGuiAction _action)
	{
		this.globalActions.Add(_action);
	}

	// Token: 0x06009352 RID: 37714 RVA: 0x0037BB75 File Offset: 0x00379D75
	public bool IsHUDEnabled()
	{
		return this.bHUDEnabled == GUIWindowManager.HudEnabledStates.Enabled;
	}

	// Token: 0x06009353 RID: 37715 RVA: 0x0037BB80 File Offset: 0x00379D80
	public bool IsHUDPartialHidden()
	{
		return this.bHUDEnabled == GUIWindowManager.HudEnabledStates.PartialHide;
	}

	// Token: 0x06009354 RID: 37716 RVA: 0x0037BB8B File Offset: 0x00379D8B
	public bool IsFullHUDDisabled()
	{
		return this.bHUDEnabled == GUIWindowManager.HudEnabledStates.FullHide;
	}

	// Token: 0x06009355 RID: 37717 RVA: 0x0037BB96 File Offset: 0x00379D96
	public void ToggleHUDEnabled()
	{
		this.bHUDEnabled = this.bHUDEnabled.CycleEnum(false, true);
		this.SetHUDEnabled(this.bHUDEnabled);
	}

	// Token: 0x06009356 RID: 37718 RVA: 0x0037BBB7 File Offset: 0x00379DB7
	public void TempHUDDisable()
	{
		this.bTempEnabled = this.bHUDEnabled;
		this.bHUDEnabled = GUIWindowManager.HudEnabledStates.FullHide;
		this.SetHUDEnabled(this.bHUDEnabled);
	}

	// Token: 0x06009357 RID: 37719 RVA: 0x0037BBD8 File Offset: 0x00379DD8
	public void ReEnableHUD()
	{
		this.bHUDEnabled = this.bTempEnabled;
		this.SetHUDEnabled(this.bHUDEnabled);
	}

	// Token: 0x06009358 RID: 37720 RVA: 0x0037BBF4 File Offset: 0x00379DF4
	public void SetHUDEnabled(GUIWindowManager.HudEnabledStates _hudState)
	{
		this.bHUDEnabled = _hudState;
		if (_hudState <= GUIWindowManager.HudEnabledStates.PartialHide)
		{
			this.nguiWindowManager.ShowAll(true);
			this.playerUI.xui.transform.gameObject.SetActive(true);
			return;
		}
		if (_hudState != GUIWindowManager.HudEnabledStates.FullHide)
		{
			return;
		}
		this.nguiWindowManager.ShowAll(false);
		this.playerUI.xui.transform.gameObject.SetActive(false);
	}

	// Token: 0x04006E3A RID: 28218
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<GUIWindow> windowsToOpen = new List<GUIWindow>();

	// Token: 0x04006E3B RID: 28219
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<GUIWindow> windowsToRemove = new List<GUIWindow>();

	// Token: 0x04006E3C RID: 28220
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<GUIWindow> openWindows = new List<GUIWindow>();

	// Token: 0x04006E3D RID: 28221
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly Dictionary<string, GUIWindow> nameToWindowMap = new CaseInsensitiveStringDictionary<GUIWindow>();

	// Token: 0x04006E3E RID: 28222
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GUIWindow topmostWindow;

	// Token: 0x04006E3F RID: 28223
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GUIWindow modalWindow;

	// Token: 0x04006E40 RID: 28224
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool cursorWindowOpen;

	// Token: 0x04006E41 RID: 28225
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<NGuiAction> globalActions = new List<NGuiAction>();

	// Token: 0x04006E42 RID: 28226
	public bool IsInputLocked;

	// Token: 0x04006E43 RID: 28227
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public NGUIWindowManager nguiWindowManager;

	// Token: 0x04006E44 RID: 28228
	public LocalPlayerUI playerUI;

	// Token: 0x04006E45 RID: 28229
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<NGuiAction> lastActionClicked = new List<NGuiAction>();

	// Token: 0x04006E46 RID: 28230
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<NGuiAction> actionsToClear = new List<NGuiAction>();

	// Token: 0x04006E47 RID: 28231
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<NGuiAction> actionsForGlobalHotkeys = new List<NGuiAction>();

	// Token: 0x04006E48 RID: 28232
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GUIWindowManager.HudEnabledStates bHUDEnabled;

	// Token: 0x04006E49 RID: 28233
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GUIWindowManager.HudEnabledStates bTempEnabled;

	// Token: 0x02001207 RID: 4615
	public enum HudEnabledStates
	{
		// Token: 0x04006E4B RID: 28235
		Enabled,
		// Token: 0x04006E4C RID: 28236
		PartialHide,
		// Token: 0x04006E4D RID: 28237
		FullHide
	}
}

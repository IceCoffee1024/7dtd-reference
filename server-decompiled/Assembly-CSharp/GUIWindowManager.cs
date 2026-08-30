using System;
using System.Collections.Generic;
using InControl;
using Platform;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02001222 RID: 4642
public class GUIWindowManager : MonoBehaviour
{
	// Token: 0x06009436 RID: 37942 RVA: 0x00380314 File Offset: 0x0037E514
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Awake()
	{
		this.nguiWindowManager = base.GetComponent<NGUIWindowManager>();
		this.playerUI = base.GetComponent<LocalPlayerUI>();
	}

	// Token: 0x06009437 RID: 37943 RVA: 0x00380330 File Offset: 0x0037E530
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

	// Token: 0x06009438 RID: 37944 RVA: 0x003804CC File Offset: 0x0037E6CC
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

	// Token: 0x06009439 RID: 37945 RVA: 0x00380744 File Offset: 0x0037E944
	public bool IsInputActive()
	{
		return (UIInput.selection != null && UIInput.selection.gameObject.activeInHierarchy) || (this.topmostWindow != null && this.topmostWindow.isInputActive) || this.IsUGUIInputActive() || GUIWindowConsole.IsOpen();
	}

	// Token: 0x0600943A RID: 37946 RVA: 0x00380794 File Offset: 0x0037E994
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

	// Token: 0x0600943B RID: 37947 RVA: 0x003807E7 File Offset: 0x0037E9E7
	public bool IsKeyShortcutsAllowed()
	{
		return !this.IsInputLocked && (PlatformManager.NativePlatform.Input.CurrentInputStyle != PlayerInputManager.InputStyle.Keyboard || !this.IsInputActive());
	}

	// Token: 0x0600943C RID: 37948 RVA: 0x00380810 File Offset: 0x0037EA10
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

	// Token: 0x0600943D RID: 37949 RVA: 0x00380876 File Offset: 0x0037EA76
	public void Remove(GUIWindow _w)
	{
		if (_w.isShowing)
		{
			this.Close(_w, false);
		}
		_w.Cleanup();
		this.nameToWindowMap.Remove(_w.Id);
	}

	// Token: 0x0600943E RID: 37950 RVA: 0x003808A0 File Offset: 0x0037EAA0
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

	// Token: 0x0600943F RID: 37951 RVA: 0x003808D4 File Offset: 0x0037EAD4
	public bool TryGetWindow(string _windowName, out GUIWindow _window)
	{
		return this.nameToWindowMap.TryGetValue(_windowName, out _window);
	}

	// Token: 0x06009440 RID: 37952 RVA: 0x003808E3 File Offset: 0x0037EAE3
	public T GetWindow<T>(string _windowName) where T : GUIWindow
	{
		return (T)((object)this.nameToWindowMap[_windowName]);
	}

	// Token: 0x06009441 RID: 37953 RVA: 0x003808F8 File Offset: 0x0037EAF8
	public void SwitchVisible(string _windowName, bool _modal = true)
	{
		GUIWindow guiwindow;
		if (this.nameToWindowMap.TryGetValue(_windowName, out guiwindow))
		{
			this.SwitchVisible(guiwindow, _modal);
		}
	}

	// Token: 0x06009442 RID: 37954 RVA: 0x0038091D File Offset: 0x0037EB1D
	public void SwitchVisible(GUIWindow _guiWindow, bool _modal = true)
	{
		if ((!_modal || _guiWindow.isModal) && _guiWindow.isShowing)
		{
			this.Close(_guiWindow, false);
			return;
		}
		this.Open(_guiWindow, _modal);
	}

	// Token: 0x06009443 RID: 37955 RVA: 0x00380944 File Offset: 0x0037EB44
	public bool CloseAllOpenModalWindows(string _exceptWindowName)
	{
		GUIWindow guiwindow;
		this.nameToWindowMap.TryGetValue(_exceptWindowName, out guiwindow);
		return this.CloseAllOpenModalWindows(guiwindow, false);
	}

	// Token: 0x06009444 RID: 37956 RVA: 0x00380968 File Offset: 0x0037EB68
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

	// Token: 0x06009445 RID: 37957 RVA: 0x00380A18 File Offset: 0x0037EC18
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

	// Token: 0x06009446 RID: 37958 RVA: 0x00380A68 File Offset: 0x0037EC68
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

	// Token: 0x06009447 RID: 37959 RVA: 0x00380AB7 File Offset: 0x0037ECB7
	public void Open(GUIWindow _w, bool _bModal)
	{
		this.openInternal(_w, _bModal, false);
	}

	// Token: 0x06009448 RID: 37960 RVA: 0x00380AC2 File Offset: 0x0037ECC2
	public void Open(GUIWindow _w, bool _bModal, bool _bIsNotEscClosable)
	{
		this.openInternal(_w, _bModal, _bIsNotEscClosable);
	}

	// Token: 0x06009449 RID: 37961 RVA: 0x00380AD0 File Offset: 0x0037ECD0
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

	// Token: 0x0600944A RID: 37962 RVA: 0x00380BF4 File Offset: 0x0037EDF4
	public bool IsWindowOpen(string _wdwID)
	{
		GUIWindow guiwindow;
		return this.nameToWindowMap.TryGetValue(_wdwID, out guiwindow) && this.IsWindowOpen(guiwindow);
	}

	// Token: 0x0600944B RID: 37963 RVA: 0x00380C1A File Offset: 0x0037EE1A
	public bool IsWindowOpen(GUIWindow _window)
	{
		return _window != null && (_window.isShowing || this.windowsToOpen.Contains(_window));
	}

	// Token: 0x0600944C RID: 37964 RVA: 0x00380C37 File Offset: 0x0037EE37
	public bool IsModalWindowOpen()
	{
		return this.modalWindow != null;
	}

	// Token: 0x0600944D RID: 37965 RVA: 0x00380C42 File Offset: 0x0037EE42
	public GUIWindow GetModalWindow()
	{
		return this.modalWindow;
	}

	// Token: 0x0600944E RID: 37966 RVA: 0x00380C4A File Offset: 0x0037EE4A
	public bool IsCursorWindowOpen()
	{
		return this.cursorWindowOpen;
	}

	// Token: 0x0600944F RID: 37967 RVA: 0x00380C54 File Offset: 0x0037EE54
	public void Close(string _windowName)
	{
		GUIWindow guiwindow;
		if (this.nameToWindowMap.TryGetValue(_windowName, out guiwindow))
		{
			this.Close(guiwindow, false);
		}
	}

	// Token: 0x06009450 RID: 37968 RVA: 0x00380C7C File Offset: 0x0037EE7C
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

	// Token: 0x06009451 RID: 37969 RVA: 0x00380CF6 File Offset: 0x0037EEF6
	[PublicizedFrom(EAccessModifier.Private)]
	public void DisableWindowActionSet(GUIWindow _w)
	{
		if (_w.playerUI != null && _w.playerUI.ActionSetManager != null)
		{
			_w.playerUI.ActionSetManager.Pop(_w);
			_w.bActionSetEnabled = false;
		}
	}

	// Token: 0x06009452 RID: 37970 RVA: 0x00380D2B File Offset: 0x0037EF2B
	[PublicizedFrom(EAccessModifier.Private)]
	public void EnableWindowActionSet(GUIWindow _w)
	{
		if (_w.playerUI != null && _w.playerUI.ActionSetManager != null)
		{
			_w.playerUI.ActionSetManager.Push(_w);
			_w.bActionSetEnabled = true;
		}
	}

	// Token: 0x06009453 RID: 37971 RVA: 0x00380D60 File Offset: 0x0037EF60
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

	// Token: 0x06009454 RID: 37972 RVA: 0x00380E34 File Offset: 0x0037F034
	public void RemoveGlobalAction(NGuiAction _action)
	{
		this.globalActions.Remove(_action);
	}

	// Token: 0x06009455 RID: 37973 RVA: 0x00380E43 File Offset: 0x0037F043
	public void AddGlobalAction(NGuiAction _action)
	{
		this.globalActions.Add(_action);
	}

	// Token: 0x06009456 RID: 37974 RVA: 0x00380E51 File Offset: 0x0037F051
	public bool IsHUDEnabled()
	{
		return this.bHUDEnabled == GUIWindowManager.HudEnabledStates.Enabled;
	}

	// Token: 0x06009457 RID: 37975 RVA: 0x00380E5C File Offset: 0x0037F05C
	public bool IsHUDPartialHidden()
	{
		return this.bHUDEnabled == GUIWindowManager.HudEnabledStates.PartialHide;
	}

	// Token: 0x06009458 RID: 37976 RVA: 0x00380E67 File Offset: 0x0037F067
	public bool IsFullHUDDisabled()
	{
		return this.bHUDEnabled == GUIWindowManager.HudEnabledStates.FullHide;
	}

	// Token: 0x06009459 RID: 37977 RVA: 0x00380E72 File Offset: 0x0037F072
	public void ToggleHUDEnabled()
	{
		this.bHUDEnabled = this.bHUDEnabled.CycleEnum(false, true);
		this.SetHUDEnabled(this.bHUDEnabled);
	}

	// Token: 0x0600945A RID: 37978 RVA: 0x00380E93 File Offset: 0x0037F093
	public void TempHUDDisable()
	{
		this.bTempEnabled = this.bHUDEnabled;
		this.bHUDEnabled = GUIWindowManager.HudEnabledStates.FullHide;
		this.SetHUDEnabled(this.bHUDEnabled);
	}

	// Token: 0x0600945B RID: 37979 RVA: 0x00380EB4 File Offset: 0x0037F0B4
	public void ReEnableHUD()
	{
		this.bHUDEnabled = this.bTempEnabled;
		this.SetHUDEnabled(this.bHUDEnabled);
	}

	// Token: 0x0600945C RID: 37980 RVA: 0x00380ED0 File Offset: 0x0037F0D0
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

	// Token: 0x04006F13 RID: 28435
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<GUIWindow> windowsToOpen = new List<GUIWindow>();

	// Token: 0x04006F14 RID: 28436
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<GUIWindow> windowsToRemove = new List<GUIWindow>();

	// Token: 0x04006F15 RID: 28437
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<GUIWindow> openWindows = new List<GUIWindow>();

	// Token: 0x04006F16 RID: 28438
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly Dictionary<string, GUIWindow> nameToWindowMap = new CaseInsensitiveStringDictionary<GUIWindow>();

	// Token: 0x04006F17 RID: 28439
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GUIWindow topmostWindow;

	// Token: 0x04006F18 RID: 28440
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GUIWindow modalWindow;

	// Token: 0x04006F19 RID: 28441
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool cursorWindowOpen;

	// Token: 0x04006F1A RID: 28442
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<NGuiAction> globalActions = new List<NGuiAction>();

	// Token: 0x04006F1B RID: 28443
	public bool IsInputLocked;

	// Token: 0x04006F1C RID: 28444
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public NGUIWindowManager nguiWindowManager;

	// Token: 0x04006F1D RID: 28445
	public LocalPlayerUI playerUI;

	// Token: 0x04006F1E RID: 28446
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<NGuiAction> lastActionClicked = new List<NGuiAction>();

	// Token: 0x04006F1F RID: 28447
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<NGuiAction> actionsToClear = new List<NGuiAction>();

	// Token: 0x04006F20 RID: 28448
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<NGuiAction> actionsForGlobalHotkeys = new List<NGuiAction>();

	// Token: 0x04006F21 RID: 28449
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GUIWindowManager.HudEnabledStates bHUDEnabled;

	// Token: 0x04006F22 RID: 28450
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GUIWindowManager.HudEnabledStates bTempEnabled;

	// Token: 0x02001223 RID: 4643
	public enum HudEnabledStates
	{
		// Token: 0x04006F24 RID: 28452
		Enabled,
		// Token: 0x04006F25 RID: 28453
		PartialHide,
		// Token: 0x04006F26 RID: 28454
		FullHide
	}
}

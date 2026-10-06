using System;
using System.Collections.Generic;
using InControl;
using Platform;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200123A RID: 4666
public class GUIWindowManager : MonoBehaviour
{
	// Token: 0x060094C4 RID: 38084 RVA: 0x00384B5C File Offset: 0x00382D5C
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Awake()
	{
		this.nguiWindowManager = base.GetComponent<NGUIWindowManager>();
		this.playerUI = base.GetComponent<LocalPlayerUI>();
	}

	// Token: 0x060094C5 RID: 38085 RVA: 0x00384B78 File Offset: 0x00382D78
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

	// Token: 0x060094C6 RID: 38086 RVA: 0x00384D14 File Offset: 0x00382F14
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

	// Token: 0x060094C7 RID: 38087 RVA: 0x00384F8C File Offset: 0x0038318C
	public bool IsInputActive()
	{
		return (UIInput.selection != null && UIInput.selection.gameObject.activeInHierarchy) || (this.topmostWindow != null && this.topmostWindow.isInputActive) || this.IsUGUIInputActive() || GUIWindowConsole.IsOpen();
	}

	// Token: 0x060094C8 RID: 38088 RVA: 0x00384FDC File Offset: 0x003831DC
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

	// Token: 0x060094C9 RID: 38089 RVA: 0x0038502F File Offset: 0x0038322F
	public bool IsKeyShortcutsAllowed()
	{
		return !this.IsInputLocked && (PlatformManager.NativePlatform.Input.CurrentInputStyle != PlayerInputManager.InputStyle.Keyboard || !this.IsInputActive());
	}

	// Token: 0x060094CA RID: 38090 RVA: 0x00385058 File Offset: 0x00383258
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

	// Token: 0x060094CB RID: 38091 RVA: 0x003850BE File Offset: 0x003832BE
	public void Remove(GUIWindow _w)
	{
		if (_w.isShowing)
		{
			this.Close(_w, false);
		}
		_w.Cleanup();
		this.nameToWindowMap.Remove(_w.Id);
	}

	// Token: 0x060094CC RID: 38092 RVA: 0x003850E8 File Offset: 0x003832E8
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

	// Token: 0x060094CD RID: 38093 RVA: 0x0038511C File Offset: 0x0038331C
	public bool TryGetWindow(string _windowName, out GUIWindow _window)
	{
		return this.nameToWindowMap.TryGetValue(_windowName, out _window);
	}

	// Token: 0x060094CE RID: 38094 RVA: 0x0038512B File Offset: 0x0038332B
	public T GetWindow<T>(string _windowName) where T : GUIWindow
	{
		return (T)((object)this.nameToWindowMap[_windowName]);
	}

	// Token: 0x060094CF RID: 38095 RVA: 0x00385140 File Offset: 0x00383340
	public void SwitchVisible(string _windowName, bool _modal = true)
	{
		GUIWindow guiwindow;
		if (this.nameToWindowMap.TryGetValue(_windowName, out guiwindow))
		{
			this.SwitchVisible(guiwindow, _modal);
		}
	}

	// Token: 0x060094D0 RID: 38096 RVA: 0x00385165 File Offset: 0x00383365
	public void SwitchVisible(GUIWindow _guiWindow, bool _modal = true)
	{
		if ((!_modal || _guiWindow.isModal) && _guiWindow.isShowing)
		{
			this.Close(_guiWindow, false);
			return;
		}
		this.Open(_guiWindow, _modal);
	}

	// Token: 0x060094D1 RID: 38097 RVA: 0x0038518C File Offset: 0x0038338C
	public bool CloseAllOpenModalWindows(string _exceptWindowName)
	{
		GUIWindow guiwindow;
		this.nameToWindowMap.TryGetValue(_exceptWindowName, out guiwindow);
		return this.CloseAllOpenModalWindows(guiwindow, false);
	}

	// Token: 0x060094D2 RID: 38098 RVA: 0x003851B0 File Offset: 0x003833B0
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

	// Token: 0x060094D3 RID: 38099 RVA: 0x00385260 File Offset: 0x00383460
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

	// Token: 0x060094D4 RID: 38100 RVA: 0x003852B0 File Offset: 0x003834B0
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

	// Token: 0x060094D5 RID: 38101 RVA: 0x003852FF File Offset: 0x003834FF
	public void Open(GUIWindow _w, bool _bModal)
	{
		this.openInternal(_w, _bModal, false);
	}

	// Token: 0x060094D6 RID: 38102 RVA: 0x0038530A File Offset: 0x0038350A
	public void Open(GUIWindow _w, bool _bModal, bool _bIsNotEscClosable)
	{
		this.openInternal(_w, _bModal, _bIsNotEscClosable);
	}

	// Token: 0x060094D7 RID: 38103 RVA: 0x00385318 File Offset: 0x00383518
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

	// Token: 0x060094D8 RID: 38104 RVA: 0x0038543C File Offset: 0x0038363C
	public bool IsWindowOpen(string _wdwID)
	{
		GUIWindow guiwindow;
		return this.nameToWindowMap.TryGetValue(_wdwID, out guiwindow) && this.IsWindowOpen(guiwindow);
	}

	// Token: 0x060094D9 RID: 38105 RVA: 0x00385462 File Offset: 0x00383662
	public bool IsWindowOpen(GUIWindow _window)
	{
		return _window != null && (_window.isShowing || this.windowsToOpen.Contains(_window));
	}

	// Token: 0x060094DA RID: 38106 RVA: 0x0038547F File Offset: 0x0038367F
	public bool IsModalWindowOpen()
	{
		return this.modalWindow != null;
	}

	// Token: 0x060094DB RID: 38107 RVA: 0x0038548A File Offset: 0x0038368A
	public GUIWindow GetModalWindow()
	{
		return this.modalWindow;
	}

	// Token: 0x060094DC RID: 38108 RVA: 0x00385492 File Offset: 0x00383692
	public bool IsCursorWindowOpen()
	{
		return this.cursorWindowOpen;
	}

	// Token: 0x060094DD RID: 38109 RVA: 0x0038549C File Offset: 0x0038369C
	public void Close(string _windowName)
	{
		GUIWindow guiwindow;
		if (this.nameToWindowMap.TryGetValue(_windowName, out guiwindow))
		{
			this.Close(guiwindow, false);
		}
	}

	// Token: 0x060094DE RID: 38110 RVA: 0x003854C4 File Offset: 0x003836C4
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

	// Token: 0x060094DF RID: 38111 RVA: 0x0038553E File Offset: 0x0038373E
	[PublicizedFrom(EAccessModifier.Private)]
	public void DisableWindowActionSet(GUIWindow _w)
	{
		if (_w.playerUI != null && _w.playerUI.ActionSetManager != null)
		{
			_w.playerUI.ActionSetManager.Pop(_w);
			_w.bActionSetEnabled = false;
		}
	}

	// Token: 0x060094E0 RID: 38112 RVA: 0x00385573 File Offset: 0x00383773
	[PublicizedFrom(EAccessModifier.Private)]
	public void EnableWindowActionSet(GUIWindow _w)
	{
		if (_w.playerUI != null && _w.playerUI.ActionSetManager != null)
		{
			_w.playerUI.ActionSetManager.Push(_w);
			_w.bActionSetEnabled = true;
		}
	}

	// Token: 0x060094E1 RID: 38113 RVA: 0x003855A8 File Offset: 0x003837A8
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

	// Token: 0x060094E2 RID: 38114 RVA: 0x0038567C File Offset: 0x0038387C
	public void RemoveGlobalAction(NGuiAction _action)
	{
		this.globalActions.Remove(_action);
	}

	// Token: 0x060094E3 RID: 38115 RVA: 0x0038568B File Offset: 0x0038388B
	public void AddGlobalAction(NGuiAction _action)
	{
		this.globalActions.Add(_action);
	}

	// Token: 0x060094E4 RID: 38116 RVA: 0x00385699 File Offset: 0x00383899
	public bool IsHUDEnabled()
	{
		return this.bHUDEnabled == GUIWindowManager.HudEnabledStates.Enabled;
	}

	// Token: 0x060094E5 RID: 38117 RVA: 0x003856A4 File Offset: 0x003838A4
	public bool IsHUDPartialHidden()
	{
		return this.bHUDEnabled == GUIWindowManager.HudEnabledStates.PartialHide;
	}

	// Token: 0x060094E6 RID: 38118 RVA: 0x003856AF File Offset: 0x003838AF
	public bool IsFullHUDDisabled()
	{
		return this.bHUDEnabled == GUIWindowManager.HudEnabledStates.FullHide;
	}

	// Token: 0x060094E7 RID: 38119 RVA: 0x003856BA File Offset: 0x003838BA
	public void ToggleHUDEnabled()
	{
		this.bHUDEnabled = this.bHUDEnabled.CycleEnum(false, true);
		this.SetHUDEnabled(this.bHUDEnabled);
	}

	// Token: 0x060094E8 RID: 38120 RVA: 0x003856DB File Offset: 0x003838DB
	public void TempHUDDisable()
	{
		this.bTempEnabled = this.bHUDEnabled;
		this.bHUDEnabled = GUIWindowManager.HudEnabledStates.FullHide;
		this.SetHUDEnabled(this.bHUDEnabled);
	}

	// Token: 0x060094E9 RID: 38121 RVA: 0x003856FC File Offset: 0x003838FC
	public void ReEnableHUD()
	{
		this.bHUDEnabled = this.bTempEnabled;
		this.SetHUDEnabled(this.bHUDEnabled);
	}

	// Token: 0x060094EA RID: 38122 RVA: 0x00385718 File Offset: 0x00383918
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

	// Token: 0x04006F7D RID: 28541
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<GUIWindow> windowsToOpen = new List<GUIWindow>();

	// Token: 0x04006F7E RID: 28542
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<GUIWindow> windowsToRemove = new List<GUIWindow>();

	// Token: 0x04006F7F RID: 28543
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<GUIWindow> openWindows = new List<GUIWindow>();

	// Token: 0x04006F80 RID: 28544
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly Dictionary<string, GUIWindow> nameToWindowMap = new CaseInsensitiveStringDictionary<GUIWindow>();

	// Token: 0x04006F81 RID: 28545
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GUIWindow topmostWindow;

	// Token: 0x04006F82 RID: 28546
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GUIWindow modalWindow;

	// Token: 0x04006F83 RID: 28547
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool cursorWindowOpen;

	// Token: 0x04006F84 RID: 28548
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<NGuiAction> globalActions = new List<NGuiAction>();

	// Token: 0x04006F85 RID: 28549
	public bool IsInputLocked;

	// Token: 0x04006F86 RID: 28550
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public NGUIWindowManager nguiWindowManager;

	// Token: 0x04006F87 RID: 28551
	public LocalPlayerUI playerUI;

	// Token: 0x04006F88 RID: 28552
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<NGuiAction> lastActionClicked = new List<NGuiAction>();

	// Token: 0x04006F89 RID: 28553
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<NGuiAction> actionsToClear = new List<NGuiAction>();

	// Token: 0x04006F8A RID: 28554
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<NGuiAction> actionsForGlobalHotkeys = new List<NGuiAction>();

	// Token: 0x04006F8B RID: 28555
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GUIWindowManager.HudEnabledStates bHUDEnabled;

	// Token: 0x04006F8C RID: 28556
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GUIWindowManager.HudEnabledStates bTempEnabled;

	// Token: 0x0200123B RID: 4667
	public enum HudEnabledStates
	{
		// Token: 0x04006F8E RID: 28558
		Enabled,
		// Token: 0x04006F8F RID: 28559
		PartialHide,
		// Token: 0x04006F90 RID: 28560
		FullHide
	}
}

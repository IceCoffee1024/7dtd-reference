using System;
using System.Collections.Generic;
using InControl;
using Platform;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200124C RID: 4684
public class GUIWindowManager : MonoBehaviour
{
	// Token: 0x0600951B RID: 38171 RVA: 0x00386F58 File Offset: 0x00385158
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Awake()
	{
		this.nguiWindowManager = base.GetComponent<NGUIWindowManager>();
		this.playerUI = base.GetComponent<LocalPlayerUI>();
	}

	// Token: 0x0600951C RID: 38172 RVA: 0x00386F74 File Offset: 0x00385174
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

	// Token: 0x0600951D RID: 38173 RVA: 0x00387110 File Offset: 0x00385310
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

	// Token: 0x0600951E RID: 38174 RVA: 0x00387388 File Offset: 0x00385588
	public bool IsInputActive()
	{
		return (UIInput.selection != null && UIInput.selection.gameObject.activeInHierarchy) || (this.topmostWindow != null && this.topmostWindow.isInputActive) || this.IsUGUIInputActive() || GUIWindowConsole.IsOpen();
	}

	// Token: 0x0600951F RID: 38175 RVA: 0x003873D8 File Offset: 0x003855D8
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

	// Token: 0x06009520 RID: 38176 RVA: 0x0038742B File Offset: 0x0038562B
	public bool IsKeyShortcutsAllowed()
	{
		return !this.IsInputLocked && (PlatformManager.NativePlatform.Input.CurrentInputStyle != PlayerInputManager.InputStyle.Keyboard || !this.IsInputActive());
	}

	// Token: 0x06009521 RID: 38177 RVA: 0x00387454 File Offset: 0x00385654
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

	// Token: 0x06009522 RID: 38178 RVA: 0x003874BA File Offset: 0x003856BA
	public void Remove(GUIWindow _w)
	{
		if (_w.isShowing)
		{
			this.Close(_w, false);
		}
		_w.Cleanup();
		this.nameToWindowMap.Remove(_w.Id);
	}

	// Token: 0x06009523 RID: 38179 RVA: 0x003874E4 File Offset: 0x003856E4
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

	// Token: 0x06009524 RID: 38180 RVA: 0x00387518 File Offset: 0x00385718
	public bool TryGetWindow(string _windowName, out GUIWindow _window)
	{
		return this.nameToWindowMap.TryGetValue(_windowName, out _window);
	}

	// Token: 0x06009525 RID: 38181 RVA: 0x00387527 File Offset: 0x00385727
	public T GetWindow<T>(string _windowName) where T : GUIWindow
	{
		return (T)((object)this.nameToWindowMap[_windowName]);
	}

	// Token: 0x06009526 RID: 38182 RVA: 0x0038753C File Offset: 0x0038573C
	public void SwitchVisible(string _windowName, bool _modal = true)
	{
		GUIWindow guiwindow;
		if (this.nameToWindowMap.TryGetValue(_windowName, out guiwindow))
		{
			this.SwitchVisible(guiwindow, _modal);
		}
	}

	// Token: 0x06009527 RID: 38183 RVA: 0x00387561 File Offset: 0x00385761
	public void SwitchVisible(GUIWindow _guiWindow, bool _modal = true)
	{
		if ((!_modal || _guiWindow.isModal) && _guiWindow.isShowing)
		{
			this.Close(_guiWindow, false);
			return;
		}
		this.Open(_guiWindow, _modal);
	}

	// Token: 0x06009528 RID: 38184 RVA: 0x00387588 File Offset: 0x00385788
	public bool CloseAllOpenModalWindows(string _exceptWindowName)
	{
		GUIWindow guiwindow;
		this.nameToWindowMap.TryGetValue(_exceptWindowName, out guiwindow);
		return this.CloseAllOpenModalWindows(guiwindow, false);
	}

	// Token: 0x06009529 RID: 38185 RVA: 0x003875AC File Offset: 0x003857AC
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

	// Token: 0x0600952A RID: 38186 RVA: 0x0038765C File Offset: 0x0038585C
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

	// Token: 0x0600952B RID: 38187 RVA: 0x003876AC File Offset: 0x003858AC
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

	// Token: 0x0600952C RID: 38188 RVA: 0x003876FB File Offset: 0x003858FB
	public void Open(GUIWindow _w, bool _bModal)
	{
		this.openInternal(_w, _bModal, false);
	}

	// Token: 0x0600952D RID: 38189 RVA: 0x00387706 File Offset: 0x00385906
	public void Open(GUIWindow _w, bool _bModal, bool _bIsNotEscClosable)
	{
		this.openInternal(_w, _bModal, _bIsNotEscClosable);
	}

	// Token: 0x0600952E RID: 38190 RVA: 0x00387714 File Offset: 0x00385914
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

	// Token: 0x0600952F RID: 38191 RVA: 0x00387838 File Offset: 0x00385A38
	public bool IsWindowOpen(string _wdwID)
	{
		GUIWindow guiwindow;
		return this.nameToWindowMap.TryGetValue(_wdwID, out guiwindow) && this.IsWindowOpen(guiwindow);
	}

	// Token: 0x06009530 RID: 38192 RVA: 0x0038785E File Offset: 0x00385A5E
	public bool IsWindowOpen(GUIWindow _window)
	{
		return _window != null && (_window.isShowing || this.windowsToOpen.Contains(_window));
	}

	// Token: 0x06009531 RID: 38193 RVA: 0x0038787B File Offset: 0x00385A7B
	public bool IsModalWindowOpen()
	{
		return this.modalWindow != null;
	}

	// Token: 0x06009532 RID: 38194 RVA: 0x00387886 File Offset: 0x00385A86
	public GUIWindow GetModalWindow()
	{
		return this.modalWindow;
	}

	// Token: 0x06009533 RID: 38195 RVA: 0x0038788E File Offset: 0x00385A8E
	public bool IsCursorWindowOpen()
	{
		return this.cursorWindowOpen;
	}

	// Token: 0x06009534 RID: 38196 RVA: 0x00387898 File Offset: 0x00385A98
	public void Close(string _windowName)
	{
		GUIWindow guiwindow;
		if (this.nameToWindowMap.TryGetValue(_windowName, out guiwindow))
		{
			this.Close(guiwindow, false);
		}
	}

	// Token: 0x06009535 RID: 38197 RVA: 0x003878C0 File Offset: 0x00385AC0
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

	// Token: 0x06009536 RID: 38198 RVA: 0x0038793A File Offset: 0x00385B3A
	[PublicizedFrom(EAccessModifier.Private)]
	public void DisableWindowActionSet(GUIWindow _w)
	{
		if (_w.playerUI != null && _w.playerUI.ActionSetManager != null)
		{
			_w.playerUI.ActionSetManager.Pop(_w);
			_w.bActionSetEnabled = false;
		}
	}

	// Token: 0x06009537 RID: 38199 RVA: 0x0038796F File Offset: 0x00385B6F
	[PublicizedFrom(EAccessModifier.Private)]
	public void EnableWindowActionSet(GUIWindow _w)
	{
		if (_w.playerUI != null && _w.playerUI.ActionSetManager != null)
		{
			_w.playerUI.ActionSetManager.Push(_w);
			_w.bActionSetEnabled = true;
		}
	}

	// Token: 0x06009538 RID: 38200 RVA: 0x003879A4 File Offset: 0x00385BA4
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

	// Token: 0x06009539 RID: 38201 RVA: 0x00387A78 File Offset: 0x00385C78
	public void RemoveGlobalAction(NGuiAction _action)
	{
		this.globalActions.Remove(_action);
	}

	// Token: 0x0600953A RID: 38202 RVA: 0x00387A87 File Offset: 0x00385C87
	public void AddGlobalAction(NGuiAction _action)
	{
		this.globalActions.Add(_action);
	}

	// Token: 0x0600953B RID: 38203 RVA: 0x00387A95 File Offset: 0x00385C95
	public bool IsHUDEnabled()
	{
		return this.bHUDEnabled == GUIWindowManager.HudEnabledStates.Enabled;
	}

	// Token: 0x0600953C RID: 38204 RVA: 0x00387AA0 File Offset: 0x00385CA0
	public bool IsHUDPartialHidden()
	{
		return this.bHUDEnabled == GUIWindowManager.HudEnabledStates.PartialHide;
	}

	// Token: 0x0600953D RID: 38205 RVA: 0x00387AAB File Offset: 0x00385CAB
	public bool IsFullHUDDisabled()
	{
		return this.bHUDEnabled == GUIWindowManager.HudEnabledStates.FullHide;
	}

	// Token: 0x0600953E RID: 38206 RVA: 0x00387AB6 File Offset: 0x00385CB6
	public void ToggleHUDEnabled()
	{
		this.bHUDEnabled = this.bHUDEnabled.CycleEnum(false, true);
		this.SetHUDEnabled(this.bHUDEnabled);
	}

	// Token: 0x0600953F RID: 38207 RVA: 0x00387AD7 File Offset: 0x00385CD7
	public void TempHUDDisable()
	{
		this.bTempEnabled = this.bHUDEnabled;
		this.bHUDEnabled = GUIWindowManager.HudEnabledStates.FullHide;
		this.SetHUDEnabled(this.bHUDEnabled);
	}

	// Token: 0x06009540 RID: 38208 RVA: 0x00387AF8 File Offset: 0x00385CF8
	public void ReEnableHUD()
	{
		this.bHUDEnabled = this.bTempEnabled;
		this.SetHUDEnabled(this.bHUDEnabled);
	}

	// Token: 0x06009541 RID: 38209 RVA: 0x00387B14 File Offset: 0x00385D14
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

	// Token: 0x04006FBE RID: 28606
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<GUIWindow> windowsToOpen = new List<GUIWindow>();

	// Token: 0x04006FBF RID: 28607
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<GUIWindow> windowsToRemove = new List<GUIWindow>();

	// Token: 0x04006FC0 RID: 28608
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<GUIWindow> openWindows = new List<GUIWindow>();

	// Token: 0x04006FC1 RID: 28609
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly Dictionary<string, GUIWindow> nameToWindowMap = new CaseInsensitiveStringDictionary<GUIWindow>();

	// Token: 0x04006FC2 RID: 28610
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GUIWindow topmostWindow;

	// Token: 0x04006FC3 RID: 28611
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GUIWindow modalWindow;

	// Token: 0x04006FC4 RID: 28612
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool cursorWindowOpen;

	// Token: 0x04006FC5 RID: 28613
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<NGuiAction> globalActions = new List<NGuiAction>();

	// Token: 0x04006FC6 RID: 28614
	public bool IsInputLocked;

	// Token: 0x04006FC7 RID: 28615
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public NGUIWindowManager nguiWindowManager;

	// Token: 0x04006FC8 RID: 28616
	public LocalPlayerUI playerUI;

	// Token: 0x04006FC9 RID: 28617
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<NGuiAction> lastActionClicked = new List<NGuiAction>();

	// Token: 0x04006FCA RID: 28618
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<NGuiAction> actionsToClear = new List<NGuiAction>();

	// Token: 0x04006FCB RID: 28619
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<NGuiAction> actionsForGlobalHotkeys = new List<NGuiAction>();

	// Token: 0x04006FCC RID: 28620
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GUIWindowManager.HudEnabledStates bHUDEnabled;

	// Token: 0x04006FCD RID: 28621
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GUIWindowManager.HudEnabledStates bTempEnabled;

	// Token: 0x0200124D RID: 4685
	public enum HudEnabledStates
	{
		// Token: 0x04006FCF RID: 28623
		Enabled,
		// Token: 0x04006FD0 RID: 28624
		PartialHide,
		// Token: 0x04006FD1 RID: 28625
		FullHide
	}
}

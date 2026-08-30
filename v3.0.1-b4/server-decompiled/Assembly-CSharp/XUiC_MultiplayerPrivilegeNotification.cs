using System;
using System.Collections;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E01 RID: 3585
[Preserve]
public class XUiC_MultiplayerPrivilegeNotification : XUiController
{
	// Token: 0x06006DF6 RID: 28150 RVA: 0x002AA1F0 File Offset: 0x002A83F0
	[XuiBindEvent("OnPress", "btnCancel")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCancel_OnPressed(XUiController _sender, int _mouseButton)
	{
		if (this.cancellationToken != null)
		{
			this.cancellationToken.Cancel();
		}
		else
		{
			this.closeWindow(false);
		}
		this.btnCancel.ViewComponent.Enabled = false;
	}

	// Token: 0x06006DF7 RID: 28151 RVA: 0x002AA21F File Offset: 0x002A841F
	[XuiBindEvent("OnPress", "btnClose")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnClose_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.closeWindow(false);
	}

	// Token: 0x06006DF8 RID: 28152 RVA: 0x002AA228 File Offset: 0x002A8428
	public override void OnClose()
	{
		CoroutineCancellationToken coroutineCancellationToken = this.cancellationToken;
		if (coroutineCancellationToken != null)
		{
			coroutineCancellationToken.Cancel();
		}
		this.lblResolvingPrivileges.IsVisible = false;
		this.lblInvalidPrivileges.IsVisible = false;
		this.btnCancel.ViewComponent.IsVisible = false;
		this.btnClose.ViewComponent.IsVisible = false;
		base.OnClose();
		if (XUiC_ProgressWindow.IsWindowOpen())
		{
			this.closeWindow(false);
		}
	}

	// Token: 0x06006DF9 RID: 28153 RVA: 0x002AA294 File Offset: 0x002A8494
	[PublicizedFrom(EAccessModifier.Private)]
	public bool resolvePrivilegesWithDialog(EUserPerms _permissions, Action<bool> _resolutionComplete, float _delayDisplay = -1f, bool _usingProgressWindow = false, Action _cancellationCleanupAction = null)
	{
		if (this.resolving)
		{
			return false;
		}
		if (_permissions == (EUserPerms)0)
		{
			Log.Error("No privileges specified.");
			return false;
		}
		this.resolving = true;
		this.cancellationToken = new CoroutineCancellationToken();
		this.cancellationCleanupAction = _cancellationCleanupAction;
		if (_usingProgressWindow)
		{
			string text = Localization.Get("lblResolvingPrivileges", false, null) + "\n\n[FFFFFF]" + Utils.GetCancellationMessage();
			XUiC_ProgressWindow.Open(this.xui.playerUI, text, delegate
			{
				this.cancellationToken.Cancel();
			}, true, true, false);
		}
		else
		{
			this.xui.playerUI.windowManager.Open(this.windowGroup, false);
			this.setContentVisibility(false);
			this.btnCancel.ViewComponent.Enabled = true;
			this.btnCancel.ViewComponent.IsVisible = true;
			this.btnClose.ViewComponent.Enabled = false;
			this.btnClose.ViewComponent.IsVisible = false;
			this.lblResolvingPrivileges.IsVisible = true;
			this.lblInvalidPrivileges.IsVisible = false;
		}
		if (_delayDisplay < 0f)
		{
			_delayDisplay = this.DefaultPlatformDelay;
		}
		ThreadManager.StartCoroutine(this.delayPanelVisibility(_delayDisplay));
		ThreadManager.StartCoroutine(this.resolvePrivilegesCoroutine(_permissions, _resolutionComplete, _usingProgressWindow));
		return true;
	}

	// Token: 0x06006DFA RID: 28154 RVA: 0x002AA3C3 File Offset: 0x002A85C3
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator delayPanelVisibility(float _delay)
	{
		yield return new WaitForSeconds(_delay);
		if (this.resolving)
		{
			this.setContentVisibility(true);
		}
		yield break;
	}

	// Token: 0x17000C35 RID: 3125
	// (get) Token: 0x06006DFB RID: 28155 RVA: 0x002AA3DC File Offset: 0x002A85DC
	public float DefaultPlatformDelay
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return 0.2f;
		}
	}

	// Token: 0x06006DFC RID: 28156 RVA: 0x002AA3F0 File Offset: 0x002A85F0
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator resolvePrivilegesCoroutine(EUserPerms _permissions, Action<bool> _resolutionComplete, bool _usingProgressWindow)
	{
		if (_permissions != (EUserPerms)0)
		{
			yield return PermissionsManager.ResolvePermissions(_permissions, true, this.cancellationToken);
		}
		this.resolving = false;
		CoroutineCancellationToken coroutineCancellationToken = this.cancellationToken;
		if (coroutineCancellationToken != null && coroutineCancellationToken.IsCancelled())
		{
			Action action = this.cancellationCleanupAction;
			if (action != null)
			{
				action();
			}
			this.closeWindow(false);
			yield break;
		}
		this.eulaAccepted = GameManager.HasAcceptedLatestEula();
		bool flag = this.eulaAccepted && (PermissionsManager.GetPermissions(PermissionsManager.PermissionSources.All) & _permissions) == _permissions;
		if (_usingProgressWindow)
		{
			if (flag)
			{
				this.closeWindow(true);
			}
			else
			{
				string text = ((!this.eulaAccepted) ? Localization.Get("uiPermissionsEula", false, null) : (PermissionsManager.GetPermissionDenyReason(_permissions, PermissionsManager.PermissionSources.All) ?? Localization.Get("lblInvalidPrivileges", false, null)));
				text = text + "\n\n[FFFFFF]" + Utils.GetCancellationMessage();
				XUiC_ProgressWindow.Open(this.xui.playerUI, text, delegate
				{
					this.closeWindow(false);
				}, true, true, false);
			}
		}
		else if (flag)
		{
			this.closeWindow(true);
		}
		else
		{
			this.lblResolvingPrivileges.IsVisible = false;
			string text2;
			if (!this.eulaAccepted)
			{
				text2 = Localization.Get("uiPermissionsEula", false, null);
			}
			else
			{
				text2 = PermissionsManager.GetPermissionDenyReason(_permissions, PermissionsManager.PermissionSources.All);
			}
			if (!string.IsNullOrEmpty(text2))
			{
				this.btnCancel.ViewComponent.Enabled = false;
				this.btnCancel.ViewComponent.IsVisible = false;
				this.btnClose.ViewComponent.Enabled = true;
				this.btnClose.ViewComponent.IsVisible = true;
				this.lblInvalidPrivileges.Text = text2;
				this.lblInvalidPrivileges.IsVisible = true;
				this.setContentVisibility(true);
			}
			else
			{
				this.closeWindow(false);
			}
		}
		if (_resolutionComplete != null)
		{
			_resolutionComplete(flag);
		}
		yield break;
	}

	// Token: 0x06006DFD RID: 28157 RVA: 0x002AA414 File Offset: 0x002A8614
	[PublicizedFrom(EAccessModifier.Private)]
	public void closeWindow(bool _success)
	{
		if (!XUiC_ProgressWindow.IsWindowOpen())
		{
			this.xui.playerUI.windowManager.Close(this.windowGroup, false);
			return;
		}
		XUiC_ProgressWindow.Close(this.xui.playerUI);
		if (_success || GameManager.Instance.gameStateManager.IsGameStarted())
		{
			return;
		}
		this.xui.playerUI.windowManager.Open(XUiC_MainMenu.ID, true);
		if (!GameManager.HasAcceptedLatestEula())
		{
			XUiC_EulaWindow.Open(this.xui, false);
		}
	}

	// Token: 0x06006DFE RID: 28158 RVA: 0x002AA498 File Offset: 0x002A8698
	[PublicizedFrom(EAccessModifier.Private)]
	public void setContentVisibility(bool _visible)
	{
		this.header.IsVisible = _visible;
		this.content.IsVisible = _visible;
		this.buttons.IsVisible = _visible;
		if (_visible)
		{
			if (this.btnClose.ViewComponent.IsVisible)
			{
				this.xui.playerUI.CursorController.SetNavigationLockView(this.buttons, this.btnClose.ViewComponent);
				this.btnClose.SelectCursorElement(true, false);
				return;
			}
			if (this.btnCancel.ViewComponent.IsVisible)
			{
				this.xui.playerUI.CursorController.SetNavigationLockView(this.buttons, this.btnCancel.ViewComponent);
				this.btnCancel.SelectCursorElement(true, false);
				return;
			}
		}
		else
		{
			this.xui.playerUI.CursorController.SetNavigationLockView(null, null);
		}
	}

	// Token: 0x06006DFF RID: 28159 RVA: 0x002AA574 File Offset: 0x002A8774
	public static bool ResolvePrivilegesWithDialog(EUserPerms _permissions, Action<bool> _resolutionComplete, float _delayDisplay = -1f, bool _usingProgressWindow = false, Action _cancellationCleanupAction = null)
	{
		return LocalPlayerUI.primaryUI.xui.GetChildByType<XUiC_MultiplayerPrivilegeNotification>().resolvePrivilegesWithDialog(_permissions, _resolutionComplete, _delayDisplay, _usingProgressWindow, _cancellationCleanupAction);
	}

	// Token: 0x06006E00 RID: 28160 RVA: 0x002AA590 File Offset: 0x002A8790
	public static void Close()
	{
		LocalPlayerUI primaryUI = LocalPlayerUI.primaryUI;
		primaryUI.windowManager.Close(primaryUI.xui.GetChildByType<XUiC_MultiplayerPrivilegeNotification>().windowGroup, false);
	}

	// Token: 0x0400535D RID: 21341
	[XuiBindComponent("btnCancel", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCancel;

	// Token: 0x0400535E RID: 21342
	[XuiBindComponent("btnClose", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnClose;

	// Token: 0x0400535F RID: 21343
	[XuiBindComponent("lblResolvingPrivileges", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Label lblResolvingPrivileges;

	// Token: 0x04005360 RID: 21344
	[XuiBindComponent("lblInvalidPrivileges", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Label lblInvalidPrivileges;

	// Token: 0x04005361 RID: 21345
	[XuiBindComponent("header", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiView header;

	// Token: 0x04005362 RID: 21346
	[XuiBindComponent("content", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiView content;

	// Token: 0x04005363 RID: 21347
	[XuiBindComponent("buttons", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiView buttons;

	// Token: 0x04005364 RID: 21348
	[PublicizedFrom(EAccessModifier.Private)]
	public bool resolving;

	// Token: 0x04005365 RID: 21349
	[PublicizedFrom(EAccessModifier.Private)]
	public bool eulaAccepted;

	// Token: 0x04005366 RID: 21350
	[PublicizedFrom(EAccessModifier.Private)]
	public CoroutineCancellationToken cancellationToken;

	// Token: 0x04005367 RID: 21351
	[PublicizedFrom(EAccessModifier.Private)]
	public Action cancellationCleanupAction;
}

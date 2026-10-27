using System;
using System.Collections;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E30 RID: 3632
[Preserve]
public class XUiC_MultiplayerPrivilegeNotification : XUiController
{
	// Token: 0x06006EB3 RID: 28339 RVA: 0x002B2018 File Offset: 0x002B0218
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

	// Token: 0x06006EB4 RID: 28340 RVA: 0x002B2047 File Offset: 0x002B0247
	[XuiBindEvent("OnPress", "btnClose")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnClose_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.closeWindow(false);
	}

	// Token: 0x06006EB5 RID: 28341 RVA: 0x002B2050 File Offset: 0x002B0250
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

	// Token: 0x06006EB6 RID: 28342 RVA: 0x002B20BC File Offset: 0x002B02BC
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

	// Token: 0x06006EB7 RID: 28343 RVA: 0x002B21EB File Offset: 0x002B03EB
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

	// Token: 0x17000C6F RID: 3183
	// (get) Token: 0x06006EB8 RID: 28344 RVA: 0x002B2204 File Offset: 0x002B0404
	public float DefaultPlatformDelay
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return 0.2f;
		}
	}

	// Token: 0x06006EB9 RID: 28345 RVA: 0x002B2218 File Offset: 0x002B0418
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

	// Token: 0x06006EBA RID: 28346 RVA: 0x002B223C File Offset: 0x002B043C
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

	// Token: 0x06006EBB RID: 28347 RVA: 0x002B22C0 File Offset: 0x002B04C0
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

	// Token: 0x06006EBC RID: 28348 RVA: 0x002B239C File Offset: 0x002B059C
	public static bool ResolvePrivilegesWithDialog(EUserPerms _permissions, Action<bool> _resolutionComplete, float _delayDisplay = -1f, bool _usingProgressWindow = false, Action _cancellationCleanupAction = null)
	{
		return LocalPlayerUI.primaryUI.xui.GetChildByType<XUiC_MultiplayerPrivilegeNotification>().resolvePrivilegesWithDialog(_permissions, _resolutionComplete, _delayDisplay, _usingProgressWindow, _cancellationCleanupAction);
	}

	// Token: 0x06006EBD RID: 28349 RVA: 0x002B23B8 File Offset: 0x002B05B8
	public static void Close()
	{
		LocalPlayerUI primaryUI = LocalPlayerUI.primaryUI;
		primaryUI.windowManager.Close(primaryUI.xui.GetChildByType<XUiC_MultiplayerPrivilegeNotification>().windowGroup, false);
	}

	// Token: 0x0400545C RID: 21596
	[XuiBindComponent("btnCancel", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCancel;

	// Token: 0x0400545D RID: 21597
	[XuiBindComponent("btnClose", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnClose;

	// Token: 0x0400545E RID: 21598
	[XuiBindComponent("lblResolvingPrivileges", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Label lblResolvingPrivileges;

	// Token: 0x0400545F RID: 21599
	[XuiBindComponent("lblInvalidPrivileges", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Label lblInvalidPrivileges;

	// Token: 0x04005460 RID: 21600
	[XuiBindComponent("header", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiView header;

	// Token: 0x04005461 RID: 21601
	[XuiBindComponent("content", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiView content;

	// Token: 0x04005462 RID: 21602
	[XuiBindComponent("buttons", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiView buttons;

	// Token: 0x04005463 RID: 21603
	[PublicizedFrom(EAccessModifier.Private)]
	public bool resolving;

	// Token: 0x04005464 RID: 21604
	[PublicizedFrom(EAccessModifier.Private)]
	public bool eulaAccepted;

	// Token: 0x04005465 RID: 21605
	[PublicizedFrom(EAccessModifier.Private)]
	public CoroutineCancellationToken cancellationToken;

	// Token: 0x04005466 RID: 21606
	[PublicizedFrom(EAccessModifier.Private)]
	public Action cancellationCleanupAction;
}

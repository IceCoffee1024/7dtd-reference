using System;
using UnityEngine.Scripting;

// Token: 0x02000D3D RID: 3389
[Preserve]
public class XUiC_ProgressWindow : XUiController
{
	// Token: 0x17000AF3 RID: 2803
	// (set) Token: 0x0600673E RID: 26430 RVA: 0x0028BFC0 File Offset: 0x0028A1C0
	public string ProgressText
	{
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.lblProgress.Text = value;
		}
	}

	// Token: 0x17000AF4 RID: 2804
	// (get) Token: 0x0600673F RID: 26431 RVA: 0x0028BFCE File Offset: 0x0028A1CE
	// (set) Token: 0x06006740 RID: 26432 RVA: 0x0028BFD6 File Offset: 0x0028A1D6
	[XuiXmlBinding("use_shadow")]
	public bool UseShadow
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.useShadow;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (this.useShadow == value)
			{
				return;
			}
			this.IsDirty = true;
			this.useShadow = value;
		}
	}

	// Token: 0x06006741 RID: 26433 RVA: 0x0028BFF0 File Offset: 0x0028A1F0
	public override void Init()
	{
		base.Init();
		XUiC_ProgressWindow.ID = base.WindowGroup.Id;
	}

	// Token: 0x06006742 RID: 26434 RVA: 0x0028C008 File Offset: 0x0028A208
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.escapeDelegate != null && this.xui.playerUI.playerInput != null && (this.xui.playerUI.playerInput.Menu.WasPressed || this.xui.playerUI.playerInput.PermanentActions.Cancel.WasPressed))
		{
			this.escapeDelegate();
		}
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
	}

	// Token: 0x06006743 RID: 26435 RVA: 0x0028C094 File Offset: 0x0028A294
	public override void OnOpen()
	{
		base.OnOpen();
		((XUiV_Window)base.ViewComponent).ForceVisible(1f);
		this.xui.playerUI.CursorController.SetNavigationTarget(null);
		this.xui.playerUI.CursorController.Locked = true;
	}

	// Token: 0x06006744 RID: 26436 RVA: 0x0028C0E8 File Offset: 0x0028A2E8
	public override void OnClose()
	{
		base.OnClose();
		this.escapeDelegate = null;
		this.xui.playerUI.CursorController.Locked = false;
	}

	// Token: 0x06006745 RID: 26437 RVA: 0x0028C110 File Offset: 0x0028A310
	public static bool IsWindowOpen()
	{
		XUiC_ProgressWindow childByType = LocalPlayerUI.primaryUI.xui.FindWindowGroupByName(XUiC_ProgressWindow.ID).GetChildByType<XUiC_ProgressWindow>();
		return childByType != null && childByType.IsOpen;
	}

	// Token: 0x06006746 RID: 26438 RVA: 0x0028C144 File Offset: 0x0028A344
	public static void Open(LocalPlayerUI _playerUi, string _text, Action _escDelegate = null, bool _modal = true, bool _notEscClosable = true, bool _useShadow = false)
	{
		if (_playerUi != null && _playerUi.xui != null)
		{
			XUiC_ProgressWindow childByType = _playerUi.xui.FindWindowGroupByName(XUiC_ProgressWindow.ID).GetChildByType<XUiC_ProgressWindow>();
			childByType.ProgressText = _text;
			_playerUi.windowManager.Open(XUiC_ProgressWindow.ID, _modal, _notEscClosable);
			childByType.escapeDelegate = _escDelegate;
			childByType.UseShadow = _useShadow;
		}
	}

	// Token: 0x06006747 RID: 26439 RVA: 0x0028C1A5 File Offset: 0x0028A3A5
	public static void Close(LocalPlayerUI _playerUi)
	{
		if (_playerUi != null && _playerUi.xui != null)
		{
			_playerUi.windowManager.Close(XUiC_ProgressWindow.ID);
		}
	}

	// Token: 0x06006748 RID: 26440 RVA: 0x0028C1D0 File Offset: 0x0028A3D0
	public static void SetText(LocalPlayerUI _playerUi, string _text, bool _clearEscDelegate = true)
	{
		if (_playerUi != null && _playerUi.xui != null)
		{
			XUiC_ProgressWindow childByType = _playerUi.xui.FindWindowGroupByName(XUiC_ProgressWindow.ID).GetChildByType<XUiC_ProgressWindow>();
			childByType.ProgressText = _text;
			if (_clearEscDelegate)
			{
				childByType.escapeDelegate = null;
			}
		}
	}

	// Token: 0x06006749 RID: 26441 RVA: 0x0028C21B File Offset: 0x0028A41B
	public static void SetEscDelegate(LocalPlayerUI _playerUi, Action _escapeDelegate)
	{
		_playerUi.xui.FindWindowGroupByName(XUiC_ProgressWindow.ID).GetChildByType<XUiC_ProgressWindow>().escapeDelegate = _escapeDelegate;
	}

	// Token: 0x04004F32 RID: 20274
	public static string ID = "";

	// Token: 0x04004F33 RID: 20275
	[PublicizedFrom(EAccessModifier.Private)]
	public bool useShadow;

	// Token: 0x04004F34 RID: 20276
	[PublicizedFrom(EAccessModifier.Private)]
	public Action escapeDelegate;

	// Token: 0x04004F35 RID: 20277
	[XuiBindComponent("lblProgress", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Label lblProgress;
}

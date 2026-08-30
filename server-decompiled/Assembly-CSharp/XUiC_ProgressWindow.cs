using System;
using UnityEngine.Scripting;

// Token: 0x02000D4F RID: 3407
[Preserve]
public class XUiC_ProgressWindow : XUiController
{
	// Token: 0x17000B06 RID: 2822
	// (set) Token: 0x060067CA RID: 26570 RVA: 0x0028EEA4 File Offset: 0x0028D0A4
	public string ProgressText
	{
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.lblProgress.Text = value;
		}
	}

	// Token: 0x17000B07 RID: 2823
	// (get) Token: 0x060067CB RID: 26571 RVA: 0x0028EEB2 File Offset: 0x0028D0B2
	// (set) Token: 0x060067CC RID: 26572 RVA: 0x0028EEBA File Offset: 0x0028D0BA
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

	// Token: 0x060067CD RID: 26573 RVA: 0x0028EED4 File Offset: 0x0028D0D4
	public override void Init()
	{
		base.Init();
		XUiC_ProgressWindow.ID = base.WindowGroup.Id;
	}

	// Token: 0x060067CE RID: 26574 RVA: 0x0028EEEC File Offset: 0x0028D0EC
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

	// Token: 0x060067CF RID: 26575 RVA: 0x0028EF78 File Offset: 0x0028D178
	public override void OnOpen()
	{
		base.OnOpen();
		((XUiV_Window)base.ViewComponent).ForceVisible(1f);
		this.xui.playerUI.CursorController.SetNavigationTarget(null);
		this.xui.playerUI.CursorController.Locked = true;
	}

	// Token: 0x060067D0 RID: 26576 RVA: 0x0028EFCC File Offset: 0x0028D1CC
	public override void OnClose()
	{
		base.OnClose();
		this.escapeDelegate = null;
		this.xui.playerUI.CursorController.Locked = false;
	}

	// Token: 0x060067D1 RID: 26577 RVA: 0x0028EFF4 File Offset: 0x0028D1F4
	public static bool IsWindowOpen()
	{
		XUiC_ProgressWindow childByType = LocalPlayerUI.primaryUI.xui.FindWindowGroupByName(XUiC_ProgressWindow.ID).GetChildByType<XUiC_ProgressWindow>();
		return childByType != null && childByType.IsOpen;
	}

	// Token: 0x060067D2 RID: 26578 RVA: 0x0028F028 File Offset: 0x0028D228
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

	// Token: 0x060067D3 RID: 26579 RVA: 0x0028F089 File Offset: 0x0028D289
	public static void Close(LocalPlayerUI _playerUi)
	{
		if (_playerUi != null && _playerUi.xui != null)
		{
			_playerUi.windowManager.Close(XUiC_ProgressWindow.ID);
		}
	}

	// Token: 0x060067D4 RID: 26580 RVA: 0x0028F0B4 File Offset: 0x0028D2B4
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

	// Token: 0x060067D5 RID: 26581 RVA: 0x0028F0FF File Offset: 0x0028D2FF
	public static void SetEscDelegate(LocalPlayerUI _playerUi, Action _escapeDelegate)
	{
		_playerUi.xui.FindWindowGroupByName(XUiC_ProgressWindow.ID).GetChildByType<XUiC_ProgressWindow>().escapeDelegate = _escapeDelegate;
	}

	// Token: 0x04004FD0 RID: 20432
	public static string ID = "";

	// Token: 0x04004FD1 RID: 20433
	[PublicizedFrom(EAccessModifier.Private)]
	public bool useShadow;

	// Token: 0x04004FD2 RID: 20434
	[PublicizedFrom(EAccessModifier.Private)]
	public Action escapeDelegate;

	// Token: 0x04004FD3 RID: 20435
	[XuiBindComponent("lblProgress", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Label lblProgress;
}

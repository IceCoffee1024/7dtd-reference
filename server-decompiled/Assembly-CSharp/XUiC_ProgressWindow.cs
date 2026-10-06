using System;
using UnityEngine.Scripting;

// Token: 0x02000D5F RID: 3423
[Preserve]
public class XUiC_ProgressWindow : XUiController
{
	// Token: 0x17000B1E RID: 2846
	// (set) Token: 0x060067C4 RID: 26564 RVA: 0x0029240C File Offset: 0x0029060C
	public string ProgressText
	{
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.lblProgress.Text = value;
		}
	}

	// Token: 0x17000B1F RID: 2847
	// (get) Token: 0x060067C5 RID: 26565 RVA: 0x0029241A File Offset: 0x0029061A
	// (set) Token: 0x060067C6 RID: 26566 RVA: 0x00292422 File Offset: 0x00290622
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

	// Token: 0x060067C7 RID: 26567 RVA: 0x0029243C File Offset: 0x0029063C
	public override void Init()
	{
		base.Init();
		XUiC_ProgressWindow.ID = base.WindowGroup.Id;
	}

	// Token: 0x060067C8 RID: 26568 RVA: 0x00292454 File Offset: 0x00290654
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

	// Token: 0x060067C9 RID: 26569 RVA: 0x002924E0 File Offset: 0x002906E0
	public override void OnOpen()
	{
		base.OnOpen();
		((XUiV_Window)base.ViewComponent).ForceVisible(1f);
		this.xui.playerUI.CursorController.SetNavigationTarget(null);
		this.xui.playerUI.CursorController.Locked = true;
	}

	// Token: 0x060067CA RID: 26570 RVA: 0x00292534 File Offset: 0x00290734
	public override void OnClose()
	{
		base.OnClose();
		this.escapeDelegate = null;
		this.xui.playerUI.CursorController.Locked = false;
	}

	// Token: 0x060067CB RID: 26571 RVA: 0x0029255C File Offset: 0x0029075C
	public static bool IsWindowOpen()
	{
		XUiC_ProgressWindow childByType = LocalPlayerUI.primaryUI.xui.FindWindowGroupByName(XUiC_ProgressWindow.ID).GetChildByType<XUiC_ProgressWindow>();
		return childByType != null && childByType.IsOpen;
	}

	// Token: 0x060067CC RID: 26572 RVA: 0x00292590 File Offset: 0x00290790
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

	// Token: 0x060067CD RID: 26573 RVA: 0x002925F1 File Offset: 0x002907F1
	public static void Close(LocalPlayerUI _playerUi)
	{
		if (_playerUi != null && _playerUi.xui != null)
		{
			_playerUi.windowManager.Close(XUiC_ProgressWindow.ID);
		}
	}

	// Token: 0x060067CE RID: 26574 RVA: 0x0029261C File Offset: 0x0029081C
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

	// Token: 0x060067CF RID: 26575 RVA: 0x00292667 File Offset: 0x00290867
	public static void SetEscDelegate(LocalPlayerUI _playerUi, Action _escapeDelegate)
	{
		_playerUi.xui.FindWindowGroupByName(XUiC_ProgressWindow.ID).GetChildByType<XUiC_ProgressWindow>().escapeDelegate = _escapeDelegate;
	}

	// Token: 0x0400500F RID: 20495
	public static string ID = "";

	// Token: 0x04005010 RID: 20496
	[PublicizedFrom(EAccessModifier.Private)]
	public bool useShadow;

	// Token: 0x04005011 RID: 20497
	[PublicizedFrom(EAccessModifier.Private)]
	public Action escapeDelegate;

	// Token: 0x04005012 RID: 20498
	[XuiBindComponent("lblProgress", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Label lblProgress;
}

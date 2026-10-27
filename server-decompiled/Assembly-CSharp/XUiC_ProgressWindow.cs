using System;
using UnityEngine.Scripting;

// Token: 0x02000D6A RID: 3434
[Preserve]
public class XUiC_ProgressWindow : XUiController
{
	// Token: 0x17000B24 RID: 2852
	// (set) Token: 0x060067E9 RID: 26601 RVA: 0x00293934 File Offset: 0x00291B34
	public string ProgressText
	{
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.lblProgress.Text = value;
		}
	}

	// Token: 0x17000B25 RID: 2853
	// (get) Token: 0x060067EA RID: 26602 RVA: 0x00293942 File Offset: 0x00291B42
	// (set) Token: 0x060067EB RID: 26603 RVA: 0x0029394A File Offset: 0x00291B4A
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

	// Token: 0x060067EC RID: 26604 RVA: 0x00293964 File Offset: 0x00291B64
	public override void Init()
	{
		base.Init();
		XUiC_ProgressWindow.ID = base.WindowGroup.Id;
	}

	// Token: 0x060067ED RID: 26605 RVA: 0x0029397C File Offset: 0x00291B7C
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

	// Token: 0x060067EE RID: 26606 RVA: 0x00293A08 File Offset: 0x00291C08
	public override void OnOpen()
	{
		base.OnOpen();
		((XUiV_Window)base.ViewComponent).ForceVisible(1f);
		this.xui.playerUI.CursorController.SetNavigationTarget(null);
		this.xui.playerUI.CursorController.Locked = true;
	}

	// Token: 0x060067EF RID: 26607 RVA: 0x00293A5C File Offset: 0x00291C5C
	public override void OnClose()
	{
		base.OnClose();
		this.escapeDelegate = null;
		this.xui.playerUI.CursorController.Locked = false;
	}

	// Token: 0x060067F0 RID: 26608 RVA: 0x00293A84 File Offset: 0x00291C84
	public static bool IsWindowOpen()
	{
		XUiC_ProgressWindow childByType = LocalPlayerUI.primaryUI.xui.FindWindowGroupByName(XUiC_ProgressWindow.ID).GetChildByType<XUiC_ProgressWindow>();
		return childByType != null && childByType.IsOpen;
	}

	// Token: 0x060067F1 RID: 26609 RVA: 0x00293AB8 File Offset: 0x00291CB8
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

	// Token: 0x060067F2 RID: 26610 RVA: 0x00293B19 File Offset: 0x00291D19
	public static void Close(LocalPlayerUI _playerUi)
	{
		if (_playerUi != null && _playerUi.xui != null)
		{
			_playerUi.windowManager.Close(XUiC_ProgressWindow.ID);
		}
	}

	// Token: 0x060067F3 RID: 26611 RVA: 0x00293B44 File Offset: 0x00291D44
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

	// Token: 0x060067F4 RID: 26612 RVA: 0x00293B8F File Offset: 0x00291D8F
	public static void SetEscDelegate(LocalPlayerUI _playerUi, Action _escapeDelegate)
	{
		_playerUi.xui.FindWindowGroupByName(XUiC_ProgressWindow.ID).GetChildByType<XUiC_ProgressWindow>().escapeDelegate = _escapeDelegate;
	}

	// Token: 0x04005034 RID: 20532
	public static string ID = "";

	// Token: 0x04005035 RID: 20533
	[PublicizedFrom(EAccessModifier.Private)]
	public bool useShadow;

	// Token: 0x04005036 RID: 20534
	[PublicizedFrom(EAccessModifier.Private)]
	public Action escapeDelegate;

	// Token: 0x04005037 RID: 20535
	[XuiBindComponent("lblProgress", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Label lblProgress;
}

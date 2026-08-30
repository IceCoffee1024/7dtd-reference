using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000E04 RID: 3588
[Preserve]
public class XUiC_DlcWindowNew : XUiController
{
	// Token: 0x06006DE5 RID: 28133 RVA: 0x002AA9EA File Offset: 0x002A8BEA
	public override void Init()
	{
		base.Init();
		XUiC_DlcWindowNew.ID = base.WindowGroup.Id;
		base.registerForInputStyleChanges();
	}

	// Token: 0x06006DE6 RID: 28134 RVA: 0x002AAA08 File Offset: 0x002A8C08
	[XuiBindEvent("ListEntryClicked", "dlcList")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void entryClicked(XUiC_List<XUiC_DlcList.DlcEntry> _list, XUiC_DlcList.DlcEntry _entry)
	{
		EntitlementManager.Instance.OpenStore(_entry.DlcSet, delegate(EntitlementSetEnum _)
		{
			this.dlcList.RebuildList(false);
		});
	}

	// Token: 0x06006DE7 RID: 28135 RVA: 0x002AAA26 File Offset: 0x002A8C26
	public override void OnOpen()
	{
		base.OnOpen();
		this.windowGroup.openWindowOnEsc = XUiC_MainMenu.ID;
		this.updatePagingButtonIcons(PlatformManager.NativePlatform.Input.CurrentInputStyle);
	}

	// Token: 0x06006DE8 RID: 28136 RVA: 0x002AA8C8 File Offset: 0x002A8AC8
	[XuiBindEvent("OnPress", "btnBack")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnBack_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, true);
	}

	// Token: 0x06006DE9 RID: 28137 RVA: 0x002AAA54 File Offset: 0x002A8C54
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (XUiUtils.HotkeysAllowedFor(this.viewComponent, false))
		{
			XUi.HandlePaging(this.xui, delegate
			{
				this.dlcList.ChangePage(1);
				return true;
			}, delegate
			{
				this.dlcList.ChangePage(-1);
				return true;
			}, false);
			if (this.xui.playerUI.playerInput.GUIActions.Submit.WasReleased && !this.xui.playerUI.CursorController.CursorModeActive)
			{
				int currentBaseIndex = this.dlcList.CurrentBaseIndex;
				int pageLength = this.dlcList.PageLength;
				int num = currentBaseIndex + pageLength / 2;
				XUiC_DlcList.DlcEntry entryByIndex = this.dlcList.GetEntryByIndex(num);
				this.entryClicked(this.dlcList, entryByIndex);
			}
		}
	}

	// Token: 0x06006DEA RID: 28138 RVA: 0x002AAB0B File Offset: 0x002A8D0B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void inputStyleChanged(PlayerInputManager.InputStyle _oldStyle, PlayerInputManager.InputStyle _newStyle)
	{
		this.updatePagingButtonIcons(_newStyle);
	}

	// Token: 0x06006DEB RID: 28139 RVA: 0x002AAB14 File Offset: 0x002A8D14
	[PublicizedFrom(EAccessModifier.Private)]
	public void updatePagingButtonIcons(PlayerInputManager.InputStyle _inputStyle)
	{
		bool flag = _inputStyle != PlayerInputManager.InputStyle.Keyboard && this.dlcList.AllEntries().Count > this.dlcList.PageLength;
		base.GetChildById("LB_Icon").ViewComponent.IsVisible = (base.GetChildById("RB_Icon").ViewComponent.IsVisible = flag);
	}

	// Token: 0x040053A6 RID: 21414
	public static string ID = "";

	// Token: 0x040053A7 RID: 21415
	[XuiBindComponent("btnBack", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnBack;

	// Token: 0x040053A8 RID: 21416
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_DlcList dlcList;
}

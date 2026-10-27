using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000E1E RID: 3614
[Preserve]
public class XUiC_DlcWindowNew : XUiController
{
	// Token: 0x06006E02 RID: 28162 RVA: 0x002AF2FE File Offset: 0x002AD4FE
	public override void Init()
	{
		base.Init();
		XUiC_DlcWindowNew.ID = base.WindowGroup.Id;
		base.registerForInputStyleChanges();
	}

	// Token: 0x06006E03 RID: 28163 RVA: 0x002AF31C File Offset: 0x002AD51C
	[XuiBindEvent("ListEntryClicked", "dlcList")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void entryClicked(XUiC_List<XUiC_DlcList.DlcEntry> _list, XUiC_DlcList.DlcEntry _entry)
	{
		EntitlementManager.Instance.OpenStore(_entry.DlcSet, delegate(EntitlementSetEnum _)
		{
			this.dlcList.RebuildList(false);
		});
	}

	// Token: 0x06006E04 RID: 28164 RVA: 0x002AF33A File Offset: 0x002AD53A
	public override void OnOpen()
	{
		base.OnOpen();
		this.windowGroup.openWindowOnEsc = XUiC_MainMenu.ID;
		this.updatePagingButtonIcons(PlatformManager.NativePlatform.Input.CurrentInputStyle);
	}

	// Token: 0x06006E05 RID: 28165 RVA: 0x002AF1DC File Offset: 0x002AD3DC
	[XuiBindEvent("OnPress", "btnBack")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnBack_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, true);
	}

	// Token: 0x06006E06 RID: 28166 RVA: 0x002AF368 File Offset: 0x002AD568
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

	// Token: 0x06006E07 RID: 28167 RVA: 0x002AF41F File Offset: 0x002AD61F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void inputStyleChanged(PlayerInputManager.InputStyle _oldStyle, PlayerInputManager.InputStyle _newStyle)
	{
		this.updatePagingButtonIcons(_newStyle);
	}

	// Token: 0x06006E08 RID: 28168 RVA: 0x002AF428 File Offset: 0x002AD628
	[PublicizedFrom(EAccessModifier.Private)]
	public void updatePagingButtonIcons(PlayerInputManager.InputStyle _inputStyle)
	{
		bool flag = _inputStyle != PlayerInputManager.InputStyle.Keyboard && this.dlcList.AllEntries().Count > this.dlcList.PageLength;
		base.GetChildById("LB_Icon").ViewComponent.IsVisible = (base.GetChildById("RB_Icon").ViewComponent.IsVisible = flag);
	}

	// Token: 0x04005402 RID: 21506
	public static string ID = "";

	// Token: 0x04005403 RID: 21507
	[XuiBindComponent("btnBack", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnBack;

	// Token: 0x04005404 RID: 21508
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_DlcList dlcList;
}

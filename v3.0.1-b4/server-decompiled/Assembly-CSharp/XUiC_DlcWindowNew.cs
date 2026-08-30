using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000DEF RID: 3567
[Preserve]
public class XUiC_DlcWindowNew : XUiController
{
	// Token: 0x06006D46 RID: 27974 RVA: 0x002A7526 File Offset: 0x002A5726
	public override void Init()
	{
		base.Init();
		XUiC_DlcWindowNew.ID = base.WindowGroup.Id;
		base.registerForInputStyleChanges();
	}

	// Token: 0x06006D47 RID: 27975 RVA: 0x002A7544 File Offset: 0x002A5744
	[XuiBindEvent("ListEntryClicked", "dlcList")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void entryClicked(XUiC_List<XUiC_DlcList.DlcEntry> _list, XUiC_DlcList.DlcEntry _entry)
	{
		EntitlementManager.Instance.OpenStore(_entry.DlcSet, delegate(EntitlementSetEnum _)
		{
			this.dlcList.RebuildList(false);
		});
	}

	// Token: 0x06006D48 RID: 27976 RVA: 0x002A7562 File Offset: 0x002A5762
	public override void OnOpen()
	{
		base.OnOpen();
		this.windowGroup.openWindowOnEsc = XUiC_MainMenu.ID;
		this.updatePagingButtonIcons(PlatformManager.NativePlatform.Input.CurrentInputStyle);
	}

	// Token: 0x06006D49 RID: 27977 RVA: 0x002A7404 File Offset: 0x002A5604
	[XuiBindEvent("OnPress", "btnBack")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnBack_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, true);
	}

	// Token: 0x06006D4A RID: 27978 RVA: 0x002A7590 File Offset: 0x002A5790
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

	// Token: 0x06006D4B RID: 27979 RVA: 0x002A7647 File Offset: 0x002A5847
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void inputStyleChanged(PlayerInputManager.InputStyle _oldStyle, PlayerInputManager.InputStyle _newStyle)
	{
		this.updatePagingButtonIcons(_newStyle);
	}

	// Token: 0x06006D4C RID: 27980 RVA: 0x002A7650 File Offset: 0x002A5850
	[PublicizedFrom(EAccessModifier.Private)]
	public void updatePagingButtonIcons(PlayerInputManager.InputStyle _inputStyle)
	{
		bool flag = _inputStyle != PlayerInputManager.InputStyle.Keyboard && this.dlcList.AllEntries().Count > this.dlcList.PageLength;
		base.GetChildById("LB_Icon").ViewComponent.IsVisible = (base.GetChildById("RB_Icon").ViewComponent.IsVisible = flag);
	}

	// Token: 0x04005303 RID: 21251
	public static string ID = "";

	// Token: 0x04005304 RID: 21252
	[XuiBindComponent("btnBack", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnBack;

	// Token: 0x04005305 RID: 21253
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_DlcList dlcList;
}

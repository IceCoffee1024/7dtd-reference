using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000E13 RID: 3603
[Preserve]
public class XUiC_DlcWindowNew : XUiController
{
	// Token: 0x06006DDD RID: 28125 RVA: 0x002ADD92 File Offset: 0x002ABF92
	public override void Init()
	{
		base.Init();
		XUiC_DlcWindowNew.ID = base.WindowGroup.Id;
		base.registerForInputStyleChanges();
	}

	// Token: 0x06006DDE RID: 28126 RVA: 0x002ADDB0 File Offset: 0x002ABFB0
	[XuiBindEvent("ListEntryClicked", "dlcList")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void entryClicked(XUiC_List<XUiC_DlcList.DlcEntry> _list, XUiC_DlcList.DlcEntry _entry)
	{
		EntitlementManager.Instance.OpenStore(_entry.DlcSet, delegate(EntitlementSetEnum _)
		{
			this.dlcList.RebuildList(false);
		});
	}

	// Token: 0x06006DDF RID: 28127 RVA: 0x002ADDCE File Offset: 0x002ABFCE
	public override void OnOpen()
	{
		base.OnOpen();
		this.windowGroup.openWindowOnEsc = XUiC_MainMenu.ID;
		this.updatePagingButtonIcons(PlatformManager.NativePlatform.Input.CurrentInputStyle);
	}

	// Token: 0x06006DE0 RID: 28128 RVA: 0x002ADC70 File Offset: 0x002ABE70
	[XuiBindEvent("OnPress", "btnBack")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnBack_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, true);
	}

	// Token: 0x06006DE1 RID: 28129 RVA: 0x002ADDFC File Offset: 0x002ABFFC
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

	// Token: 0x06006DE2 RID: 28130 RVA: 0x002ADEB3 File Offset: 0x002AC0B3
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void inputStyleChanged(PlayerInputManager.InputStyle _oldStyle, PlayerInputManager.InputStyle _newStyle)
	{
		this.updatePagingButtonIcons(_newStyle);
	}

	// Token: 0x06006DE3 RID: 28131 RVA: 0x002ADEBC File Offset: 0x002AC0BC
	[PublicizedFrom(EAccessModifier.Private)]
	public void updatePagingButtonIcons(PlayerInputManager.InputStyle _inputStyle)
	{
		bool flag = _inputStyle != PlayerInputManager.InputStyle.Keyboard && this.dlcList.AllEntries().Count > this.dlcList.PageLength;
		base.GetChildById("LB_Icon").ViewComponent.IsVisible = (base.GetChildById("RB_Icon").ViewComponent.IsVisible = flag);
	}

	// Token: 0x040053DD RID: 21469
	public static string ID = "";

	// Token: 0x040053DE RID: 21470
	[XuiBindComponent("btnBack", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnBack;

	// Token: 0x040053DF RID: 21471
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_DlcList dlcList;
}

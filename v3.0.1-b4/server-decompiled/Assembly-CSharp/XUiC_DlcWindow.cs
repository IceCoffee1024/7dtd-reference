using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000DEE RID: 3566
[Preserve]
public class XUiC_DlcWindow : XUiController
{
	// Token: 0x06006D3C RID: 27964 RVA: 0x002A739B File Offset: 0x002A559B
	public override void Init()
	{
		base.Init();
		XUiC_DlcWindow.ID = base.WindowGroup.Id;
		base.registerForInputStyleChanges();
	}

	// Token: 0x06006D3D RID: 27965 RVA: 0x002A73B9 File Offset: 0x002A55B9
	[XuiBindEvent("ListEntryClicked", "dlcList")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void entryClicked(XUiC_List<XUiC_DlcList.DlcEntry> _list, XUiC_DlcList.DlcEntry _entry)
	{
		EntitlementManager.Instance.OpenStore(_entry.DlcSet, delegate(EntitlementSetEnum _)
		{
			this.dlcList.RebuildList(false);
		});
	}

	// Token: 0x06006D3E RID: 27966 RVA: 0x002A73D7 File Offset: 0x002A55D7
	public override void OnOpen()
	{
		base.OnOpen();
		this.windowGroup.openWindowOnEsc = XUiC_MainMenu.ID;
		this.updatePagingButtonIcons(PlatformManager.NativePlatform.Input.CurrentInputStyle);
	}

	// Token: 0x06006D3F RID: 27967 RVA: 0x002A7404 File Offset: 0x002A5604
	[XuiBindEvent("OnPress", "btnBack")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnBack_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, true);
	}

	// Token: 0x06006D40 RID: 27968 RVA: 0x002A7424 File Offset: 0x002A5624
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (XUiUtils.HotkeysAllowedFor(this.viewComponent, false))
		{
			if (this.xui.playerUI.playerInput.GUIActions.WindowPagingRight.WasReleased)
			{
				this.dlcList.ChangePage(1);
				return;
			}
			if (this.xui.playerUI.playerInput.GUIActions.WindowPagingLeft.WasReleased)
			{
				this.dlcList.ChangePage(-1);
			}
		}
	}

	// Token: 0x06006D41 RID: 27969 RVA: 0x002A74A1 File Offset: 0x002A56A1
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void inputStyleChanged(PlayerInputManager.InputStyle _oldStyle, PlayerInputManager.InputStyle _newStyle)
	{
		this.updatePagingButtonIcons(_newStyle);
	}

	// Token: 0x06006D42 RID: 27970 RVA: 0x002A74AC File Offset: 0x002A56AC
	[PublicizedFrom(EAccessModifier.Private)]
	public void updatePagingButtonIcons(PlayerInputManager.InputStyle _inputStyle)
	{
		bool flag = _inputStyle != PlayerInputManager.InputStyle.Keyboard && this.dlcList.AllEntries().Count > this.dlcList.PageLength;
		base.GetChildById("LB_Icon").ViewComponent.IsVisible = (base.GetChildById("RB_Icon").ViewComponent.IsVisible = flag);
	}

	// Token: 0x04005300 RID: 21248
	public static string ID = "";

	// Token: 0x04005301 RID: 21249
	[XuiBindComponent("btnBack", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnBack;

	// Token: 0x04005302 RID: 21250
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_DlcList dlcList;
}

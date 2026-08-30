using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000E03 RID: 3587
[Preserve]
public class XUiC_DlcWindow : XUiController
{
	// Token: 0x06006DDB RID: 28123 RVA: 0x002AA85F File Offset: 0x002A8A5F
	public override void Init()
	{
		base.Init();
		XUiC_DlcWindow.ID = base.WindowGroup.Id;
		base.registerForInputStyleChanges();
	}

	// Token: 0x06006DDC RID: 28124 RVA: 0x002AA87D File Offset: 0x002A8A7D
	[XuiBindEvent("ListEntryClicked", "dlcList")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void entryClicked(XUiC_List<XUiC_DlcList.DlcEntry> _list, XUiC_DlcList.DlcEntry _entry)
	{
		EntitlementManager.Instance.OpenStore(_entry.DlcSet, delegate(EntitlementSetEnum _)
		{
			this.dlcList.RebuildList(false);
		});
	}

	// Token: 0x06006DDD RID: 28125 RVA: 0x002AA89B File Offset: 0x002A8A9B
	public override void OnOpen()
	{
		base.OnOpen();
		this.windowGroup.openWindowOnEsc = XUiC_MainMenu.ID;
		this.updatePagingButtonIcons(PlatformManager.NativePlatform.Input.CurrentInputStyle);
	}

	// Token: 0x06006DDE RID: 28126 RVA: 0x002AA8C8 File Offset: 0x002A8AC8
	[XuiBindEvent("OnPress", "btnBack")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnBack_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, true);
	}

	// Token: 0x06006DDF RID: 28127 RVA: 0x002AA8E8 File Offset: 0x002A8AE8
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

	// Token: 0x06006DE0 RID: 28128 RVA: 0x002AA965 File Offset: 0x002A8B65
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void inputStyleChanged(PlayerInputManager.InputStyle _oldStyle, PlayerInputManager.InputStyle _newStyle)
	{
		this.updatePagingButtonIcons(_newStyle);
	}

	// Token: 0x06006DE1 RID: 28129 RVA: 0x002AA970 File Offset: 0x002A8B70
	[PublicizedFrom(EAccessModifier.Private)]
	public void updatePagingButtonIcons(PlayerInputManager.InputStyle _inputStyle)
	{
		bool flag = _inputStyle != PlayerInputManager.InputStyle.Keyboard && this.dlcList.AllEntries().Count > this.dlcList.PageLength;
		base.GetChildById("LB_Icon").ViewComponent.IsVisible = (base.GetChildById("RB_Icon").ViewComponent.IsVisible = flag);
	}

	// Token: 0x040053A3 RID: 21411
	public static string ID = "";

	// Token: 0x040053A4 RID: 21412
	[XuiBindComponent("btnBack", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnBack;

	// Token: 0x040053A5 RID: 21413
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_DlcList dlcList;
}

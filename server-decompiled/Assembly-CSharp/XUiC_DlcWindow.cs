using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000E12 RID: 3602
[Preserve]
public class XUiC_DlcWindow : XUiController
{
	// Token: 0x06006DD3 RID: 28115 RVA: 0x002ADC07 File Offset: 0x002ABE07
	public override void Init()
	{
		base.Init();
		XUiC_DlcWindow.ID = base.WindowGroup.Id;
		base.registerForInputStyleChanges();
	}

	// Token: 0x06006DD4 RID: 28116 RVA: 0x002ADC25 File Offset: 0x002ABE25
	[XuiBindEvent("ListEntryClicked", "dlcList")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void entryClicked(XUiC_List<XUiC_DlcList.DlcEntry> _list, XUiC_DlcList.DlcEntry _entry)
	{
		EntitlementManager.Instance.OpenStore(_entry.DlcSet, delegate(EntitlementSetEnum _)
		{
			this.dlcList.RebuildList(false);
		});
	}

	// Token: 0x06006DD5 RID: 28117 RVA: 0x002ADC43 File Offset: 0x002ABE43
	public override void OnOpen()
	{
		base.OnOpen();
		this.windowGroup.openWindowOnEsc = XUiC_MainMenu.ID;
		this.updatePagingButtonIcons(PlatformManager.NativePlatform.Input.CurrentInputStyle);
	}

	// Token: 0x06006DD6 RID: 28118 RVA: 0x002ADC70 File Offset: 0x002ABE70
	[XuiBindEvent("OnPress", "btnBack")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnBack_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, true);
	}

	// Token: 0x06006DD7 RID: 28119 RVA: 0x002ADC90 File Offset: 0x002ABE90
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

	// Token: 0x06006DD8 RID: 28120 RVA: 0x002ADD0D File Offset: 0x002ABF0D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void inputStyleChanged(PlayerInputManager.InputStyle _oldStyle, PlayerInputManager.InputStyle _newStyle)
	{
		this.updatePagingButtonIcons(_newStyle);
	}

	// Token: 0x06006DD9 RID: 28121 RVA: 0x002ADD18 File Offset: 0x002ABF18
	[PublicizedFrom(EAccessModifier.Private)]
	public void updatePagingButtonIcons(PlayerInputManager.InputStyle _inputStyle)
	{
		bool flag = _inputStyle != PlayerInputManager.InputStyle.Keyboard && this.dlcList.AllEntries().Count > this.dlcList.PageLength;
		base.GetChildById("LB_Icon").ViewComponent.IsVisible = (base.GetChildById("RB_Icon").ViewComponent.IsVisible = flag);
	}

	// Token: 0x040053DA RID: 21466
	public static string ID = "";

	// Token: 0x040053DB RID: 21467
	[XuiBindComponent("btnBack", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnBack;

	// Token: 0x040053DC RID: 21468
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_DlcList dlcList;
}

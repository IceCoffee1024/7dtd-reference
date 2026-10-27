using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000E1D RID: 3613
[Preserve]
public class XUiC_DlcWindow : XUiController
{
	// Token: 0x06006DF8 RID: 28152 RVA: 0x002AF173 File Offset: 0x002AD373
	public override void Init()
	{
		base.Init();
		XUiC_DlcWindow.ID = base.WindowGroup.Id;
		base.registerForInputStyleChanges();
	}

	// Token: 0x06006DF9 RID: 28153 RVA: 0x002AF191 File Offset: 0x002AD391
	[XuiBindEvent("ListEntryClicked", "dlcList")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void entryClicked(XUiC_List<XUiC_DlcList.DlcEntry> _list, XUiC_DlcList.DlcEntry _entry)
	{
		EntitlementManager.Instance.OpenStore(_entry.DlcSet, delegate(EntitlementSetEnum _)
		{
			this.dlcList.RebuildList(false);
		});
	}

	// Token: 0x06006DFA RID: 28154 RVA: 0x002AF1AF File Offset: 0x002AD3AF
	public override void OnOpen()
	{
		base.OnOpen();
		this.windowGroup.openWindowOnEsc = XUiC_MainMenu.ID;
		this.updatePagingButtonIcons(PlatformManager.NativePlatform.Input.CurrentInputStyle);
	}

	// Token: 0x06006DFB RID: 28155 RVA: 0x002AF1DC File Offset: 0x002AD3DC
	[XuiBindEvent("OnPress", "btnBack")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnBack_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, true);
	}

	// Token: 0x06006DFC RID: 28156 RVA: 0x002AF1FC File Offset: 0x002AD3FC
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

	// Token: 0x06006DFD RID: 28157 RVA: 0x002AF279 File Offset: 0x002AD479
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void inputStyleChanged(PlayerInputManager.InputStyle _oldStyle, PlayerInputManager.InputStyle _newStyle)
	{
		this.updatePagingButtonIcons(_newStyle);
	}

	// Token: 0x06006DFE RID: 28158 RVA: 0x002AF284 File Offset: 0x002AD484
	[PublicizedFrom(EAccessModifier.Private)]
	public void updatePagingButtonIcons(PlayerInputManager.InputStyle _inputStyle)
	{
		bool flag = _inputStyle != PlayerInputManager.InputStyle.Keyboard && this.dlcList.AllEntries().Count > this.dlcList.PageLength;
		base.GetChildById("LB_Icon").ViewComponent.IsVisible = (base.GetChildById("RB_Icon").ViewComponent.IsVisible = flag);
	}

	// Token: 0x040053FF RID: 21503
	public static string ID = "";

	// Token: 0x04005400 RID: 21504
	[XuiBindComponent("btnBack", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnBack;

	// Token: 0x04005401 RID: 21505
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_DlcList dlcList;
}

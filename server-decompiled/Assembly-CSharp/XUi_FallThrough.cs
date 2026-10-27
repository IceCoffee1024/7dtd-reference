using System;
using GUI_2;
using InControl;
using UnityEngine;

// Token: 0x02001149 RID: 4425
public class XUi_FallThrough : MonoBehaviour
{
	// Token: 0x06008B6A RID: 35690 RVA: 0x00352A42 File Offset: 0x00350C42
	public void SetXUi(XUi _xui)
	{
		this.xui = _xui;
	}

	// Token: 0x06008B6B RID: 35691 RVA: 0x00352A4B File Offset: 0x00350C4B
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		UICamera.fallThrough = base.gameObject;
	}

	// Token: 0x06008B6C RID: 35692 RVA: 0x00352A58 File Offset: 0x00350C58
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDestroy()
	{
		if (UICamera.fallThrough == base.gameObject)
		{
			UICamera.fallThrough = null;
		}
	}

	// Token: 0x06008B6D RID: 35693 RVA: 0x00352A74 File Offset: 0x00350C74
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		bool flag = this.xui.DragAndDropWindow != null && !this.xui.DragAndDropWindow.CurrentStack.IsEmpty();
		bool flag2 = flag && UICamera.hoveredObject == this.xui.gameObject && this.xui.DragAndDropWindow.CurrentStack.itemValue.ItemClassOrMissing.CanDrop(null);
		int num = ((this.xui.DragAndDropWindow == null) ? 0 : this.xui.DragAndDropWindow.CurrentStack.count);
		bool flag3 = false;
		LocalPlayerUI playerUI = this.xui.playerUI;
		if (flag2 && null != playerUI && null != playerUI.uiCamera)
		{
			PlayerActionsLocal playerInput = playerUI.playerInput;
			if (((playerInput != null) ? playerInput.GUIActions : null) != null)
			{
				PlayerActionsGUI guiactions = playerUI.playerInput.GUIActions;
				bool flag4 = false;
				bool flag5 = false;
				if (guiactions.LastInputType == BindingSourceType.DeviceBindingSource)
				{
					flag4 |= guiactions.Submit.WasReleased;
					flag5 |= guiactions.HalfStack.WasReleased;
				}
				else
				{
					flag4 |= playerUI.CursorController.GetMouseButtonDown(UICamera.MouseButton.LeftButton);
					flag5 |= playerUI.CursorController.GetMouseButtonDown(UICamera.MouseButton.RightButton);
				}
				if (flag4 || (num == 1 && flag5))
				{
					this.xui.DragAndDropWindow.DropCurrentItem();
					flag3 = true;
				}
				else if (num > 1 && flag5)
				{
					this.xui.DragAndDropWindow.DropCurrentItem(1);
					num--;
					flag3 = true;
				}
			}
		}
		if (flag2 != this.canDrop || flag3)
		{
			this.canDrop = flag2;
			this.xui.CalloutWindow.ClearCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuHoverAir);
			if (flag && this.canDrop)
			{
				if (num > 1)
				{
					this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.FaceButtonSouth, "igcoDropAll", XUiC_GamepadCalloutWindow.CalloutType.MenuHoverAir);
					this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.FaceButtonWest, "igcoDropOne", XUiC_GamepadCalloutWindow.CalloutType.MenuHoverAir);
				}
				else
				{
					this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.FaceButtonSouth, "igcoDrop", XUiC_GamepadCalloutWindow.CalloutType.MenuHoverAir);
				}
				this.xui.CalloutWindow.DisableCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuHoverItem);
				this.xui.CalloutWindow.EnableCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuHoverAir, 0f);
				return;
			}
			this.xui.CalloutWindow.DisableCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuHoverAir);
			this.xui.CalloutWindow.EnableCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuHoverItem, 0f);
		}
	}

	// Token: 0x0400677B RID: 26491
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public XUi xui;

	// Token: 0x0400677C RID: 26492
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool canDrop;
}

using System;
using GUI_2;
using InControl;
using UnityEngine;

// Token: 0x02001108 RID: 4360
public class XUi_FallThrough : MonoBehaviour
{
	// Token: 0x060089B8 RID: 35256 RVA: 0x00347CC2 File Offset: 0x00345EC2
	public void SetXUi(XUi _xui)
	{
		this.xui = _xui;
	}

	// Token: 0x060089B9 RID: 35257 RVA: 0x00347CCB File Offset: 0x00345ECB
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		UICamera.fallThrough = base.gameObject;
	}

	// Token: 0x060089BA RID: 35258 RVA: 0x00347CD8 File Offset: 0x00345ED8
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDestroy()
	{
		if (UICamera.fallThrough == base.gameObject)
		{
			UICamera.fallThrough = null;
		}
	}

	// Token: 0x060089BB RID: 35259 RVA: 0x00347CF4 File Offset: 0x00345EF4
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

	// Token: 0x0400661F RID: 26143
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public XUi xui;

	// Token: 0x04006620 RID: 26144
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool canDrop;
}

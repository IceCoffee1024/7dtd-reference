using System;
using UnityEngine.Scripting;

// Token: 0x02001091 RID: 4241
[Preserve]
public class XUiC_TraderWindowGroup : XUiController
{
	// Token: 0x060086D2 RID: 34514 RVA: 0x00336C9C File Offset: 0x00334E9C
	public override void Init()
	{
		base.Init();
		XUiController xuiController = base.GetChildByType<XUiC_WindowNonPagingHeader>();
		if (xuiController != null)
		{
			this.nonPagingHeader = (XUiC_WindowNonPagingHeader)xuiController;
		}
		xuiController = base.GetChildByType<XUiC_TraderWindow>();
		if (xuiController != null)
		{
			this.TraderWindow = (XUiC_TraderWindow)xuiController;
		}
		xuiController = base.GetChildByType<XUiC_ServiceInfoWindow>();
		if (xuiController != null)
		{
			this.ServiceInfoWindow = (XUiC_ServiceInfoWindow)xuiController;
		}
	}

	// Token: 0x060086D3 RID: 34515 RVA: 0x00336CF4 File Offset: 0x00334EF4
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.windowGroup.isShowing)
		{
			if (!this.xui.playerUI.playerInput.PermanentActions.Activate.IsPressed)
			{
				this.wasReleased = true;
			}
			if (this.wasReleased)
			{
				if (this.xui.playerUI.playerInput.PermanentActions.Activate.IsPressed)
				{
					this.activeKeyDown = true;
				}
				if (this.xui.playerUI.playerInput.PermanentActions.Activate.WasReleased && this.activeKeyDown)
				{
					this.activeKeyDown = false;
					if (!this.xui.playerUI.windowManager.IsInputActive())
					{
						this.xui.playerUI.windowManager.CloseAllOpenModalWindows(null, false);
					}
				}
			}
		}
	}

	// Token: 0x060086D4 RID: 34516 RVA: 0x00336DD4 File Offset: 0x00334FD4
	public override void OnOpen()
	{
		this.xui.Trader.TraderWindowGroup = this;
		if (base.ViewComponent != null && !base.ViewComponent.IsVisible)
		{
			base.ViewComponent.OnOpen();
			base.ViewComponent.IsVisible = true;
		}
		if (this.nonPagingHeader != null)
		{
			this.nonPagingHeader.SetHeader((this.xui.Trader.Trader is TileEntityVendingMachine) ? Localization.Get("xuiVending", false, null) : Localization.Get("xuiTrader", false, null));
		}
		this.xui.RecenterWindowGroup(this.windowGroup, false);
		for (int i = 0; i < this.children.Count; i++)
		{
			this.children[i].OnOpen();
		}
		if (this.ServiceInfoWindow != null)
		{
			this.ServiceInfoWindow.ViewComponent.IsVisible = false;
		}
		EntityTrader entityTrader = this.xui.Trader.Trader as EntityTrader;
		if (entityTrader != null && this.xui.playerUI.entityPlayer.OverrideFOV != 30f)
		{
			this.xui.playerUI.entityPlayer.OverrideFOV = 30f;
			this.xui.playerUI.entityPlayer.OverrideLookAt = entityTrader.getHeadPosition();
		}
		this.xui.Dialog.KeepZoomOnClose = false;
		this.IsDirty = true;
	}

	// Token: 0x060086D5 RID: 34517 RVA: 0x00336F38 File Offset: 0x00335138
	public override void OnClose()
	{
		base.OnClose();
		this.wasReleased = false;
		this.activeKeyDown = false;
		this.xui.Trader.TraderWindowGroup = null;
		this.xui.playerUI.entityPlayer.OverrideFOV = -1f;
	}

	// Token: 0x060086D6 RID: 34518 RVA: 0x00336F84 File Offset: 0x00335184
	public void RefreshTraderItems()
	{
		this.TraderWindow.CompletedTransaction = true;
		this.TraderWindow.RefreshTraderItems();
	}

	// Token: 0x060086D7 RID: 34519 RVA: 0x00336F9D File Offset: 0x0033519D
	public void RefreshTraderWindow()
	{
		this.TraderWindow.RefreshBindings();
	}

	// Token: 0x0400643D RID: 25661
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeader;

	// Token: 0x0400643E RID: 25662
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TraderWindow TraderWindow;

	// Token: 0x0400643F RID: 25663
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ServiceInfoWindow ServiceInfoWindow;

	// Token: 0x04006440 RID: 25664
	public static string ID = "trader";

	// Token: 0x04006441 RID: 25665
	[PublicizedFrom(EAccessModifier.Private)]
	public bool activeKeyDown;

	// Token: 0x04006442 RID: 25666
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasReleased;
}

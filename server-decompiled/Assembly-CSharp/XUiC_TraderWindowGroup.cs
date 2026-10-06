using System;
using UnityEngine.Scripting;

// Token: 0x020010A2 RID: 4258
[Preserve]
public class XUiC_TraderWindowGroup : XUiController
{
	// Token: 0x06008721 RID: 34593 RVA: 0x0033AAB4 File Offset: 0x00338CB4
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

	// Token: 0x06008722 RID: 34594 RVA: 0x0033AB0C File Offset: 0x00338D0C
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

	// Token: 0x06008723 RID: 34595 RVA: 0x0033ABEC File Offset: 0x00338DEC
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

	// Token: 0x06008724 RID: 34596 RVA: 0x0033AD50 File Offset: 0x00338F50
	public override void OnClose()
	{
		base.OnClose();
		this.wasReleased = false;
		this.activeKeyDown = false;
		this.xui.Trader.TraderWindowGroup = null;
		this.xui.playerUI.entityPlayer.OverrideFOV = -1f;
	}

	// Token: 0x06008725 RID: 34597 RVA: 0x0033AD9C File Offset: 0x00338F9C
	public void RefreshTraderItems()
	{
		this.TraderWindow.CompletedTransaction = true;
		this.TraderWindow.RefreshTraderItems();
	}

	// Token: 0x06008726 RID: 34598 RVA: 0x0033ADB5 File Offset: 0x00338FB5
	public void RefreshTraderWindow()
	{
		this.TraderWindow.RefreshBindings();
	}

	// Token: 0x0400647F RID: 25727
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeader;

	// Token: 0x04006480 RID: 25728
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TraderWindow TraderWindow;

	// Token: 0x04006481 RID: 25729
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ServiceInfoWindow ServiceInfoWindow;

	// Token: 0x04006482 RID: 25730
	public static string ID = "trader";

	// Token: 0x04006483 RID: 25731
	[PublicizedFrom(EAccessModifier.Private)]
	public bool activeKeyDown;

	// Token: 0x04006484 RID: 25732
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasReleased;
}

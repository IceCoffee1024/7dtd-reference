using System;
using UnityEngine.Scripting;

// Token: 0x020010B2 RID: 4274
[Preserve]
public class XUiC_TraderWindowGroup : XUiController
{
	// Token: 0x06008774 RID: 34676 RVA: 0x0033CB38 File Offset: 0x0033AD38
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

	// Token: 0x06008775 RID: 34677 RVA: 0x0033CB90 File Offset: 0x0033AD90
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

	// Token: 0x06008776 RID: 34678 RVA: 0x0033CC70 File Offset: 0x0033AE70
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

	// Token: 0x06008777 RID: 34679 RVA: 0x0033CDD4 File Offset: 0x0033AFD4
	public override void OnClose()
	{
		base.OnClose();
		this.wasReleased = false;
		this.activeKeyDown = false;
		this.xui.Trader.TraderWindowGroup = null;
		this.xui.playerUI.entityPlayer.OverrideFOV = -1f;
	}

	// Token: 0x06008778 RID: 34680 RVA: 0x0033CE20 File Offset: 0x0033B020
	public void RefreshTraderItems()
	{
		this.TraderWindow.CompletedTransaction = true;
		this.TraderWindow.RefreshTraderItems();
	}

	// Token: 0x06008779 RID: 34681 RVA: 0x0033CE39 File Offset: 0x0033B039
	public void RefreshTraderWindow()
	{
		this.TraderWindow.RefreshBindings();
	}

	// Token: 0x040064BE RID: 25790
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeader;

	// Token: 0x040064BF RID: 25791
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TraderWindow TraderWindow;

	// Token: 0x040064C0 RID: 25792
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ServiceInfoWindow ServiceInfoWindow;

	// Token: 0x040064C1 RID: 25793
	public static string ID = "trader";

	// Token: 0x040064C2 RID: 25794
	[PublicizedFrom(EAccessModifier.Private)]
	public bool activeKeyDown;

	// Token: 0x040064C3 RID: 25795
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasReleased;
}

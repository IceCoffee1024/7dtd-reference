using System;
using UnityEngine.Scripting;

// Token: 0x02001078 RID: 4216
[Preserve]
public class XUiC_TraderWindowGroup : XUiController
{
	// Token: 0x060085E0 RID: 34272 RVA: 0x00332474 File Offset: 0x00330674
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

	// Token: 0x060085E1 RID: 34273 RVA: 0x003324CC File Offset: 0x003306CC
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

	// Token: 0x060085E2 RID: 34274 RVA: 0x003325AC File Offset: 0x003307AC
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

	// Token: 0x060085E3 RID: 34275 RVA: 0x00332710 File Offset: 0x00330910
	public override void OnClose()
	{
		base.OnClose();
		this.wasReleased = false;
		this.activeKeyDown = false;
		this.xui.Trader.TraderWindowGroup = null;
		this.xui.playerUI.entityPlayer.OverrideFOV = -1f;
	}

	// Token: 0x060085E4 RID: 34276 RVA: 0x0033275C File Offset: 0x0033095C
	public void RefreshTraderItems()
	{
		this.TraderWindow.CompletedTransaction = true;
		this.TraderWindow.RefreshTraderItems();
	}

	// Token: 0x060085E5 RID: 34277 RVA: 0x00332775 File Offset: 0x00330975
	public void RefreshTraderWindow()
	{
		this.TraderWindow.RefreshBindings();
	}

	// Token: 0x04006376 RID: 25462
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeader;

	// Token: 0x04006377 RID: 25463
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TraderWindow TraderWindow;

	// Token: 0x04006378 RID: 25464
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ServiceInfoWindow ServiceInfoWindow;

	// Token: 0x04006379 RID: 25465
	public static string ID = "trader";

	// Token: 0x0400637A RID: 25466
	[PublicizedFrom(EAccessModifier.Private)]
	public bool activeKeyDown;

	// Token: 0x0400637B RID: 25467
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasReleased;
}

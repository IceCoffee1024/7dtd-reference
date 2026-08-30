using System;
using UnityEngine.Scripting;

// Token: 0x02000EFB RID: 3835
[Preserve]
public class XUiC_CombineWindowGroup : XUiController
{
	// Token: 0x06007844 RID: 30788 RVA: 0x002D54D5 File Offset: 0x002D36D5
	public override void Init()
	{
		base.Init();
		if (base.GetChildByType<XUiC_WindowNonPagingHeader>() != null)
		{
			this.nonPagingHeaderWindow = base.GetChildByType<XUiC_WindowNonPagingHeader>();
		}
		this.combineGrid = base.GetChildByType<XUiC_CombineGrid>();
		this.AlwaysUpdate = true;
	}

	// Token: 0x06007845 RID: 30789 RVA: 0x002D5504 File Offset: 0x002D3704
	public void SetTileEntity(TEFeatureCombine _teCombine)
	{
		this.te = _teCombine;
		if (this.combineGrid != null)
		{
			this.combineGrid.SetTileEntity(_teCombine);
		}
	}

	// Token: 0x06007846 RID: 30790 RVA: 0x002D5524 File Offset: 0x002D3724
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

	// Token: 0x06007847 RID: 30791 RVA: 0x002D5604 File Offset: 0x002D3804
	public override void OnOpen()
	{
		if (base.ViewComponent != null && !base.ViewComponent.IsVisible)
		{
			base.ViewComponent.OnOpen();
			base.ViewComponent.IsVisible = true;
		}
		if (this.nonPagingHeaderWindow != null)
		{
			this.nonPagingHeaderWindow.SetHeader(Localization.Get("xuiCombineStation", false, null));
		}
		GUIWindowManager windowManager = this.xui.playerUI.windowManager;
		this.xui.RecenterWindowGroup(this.windowGroup, false);
		for (int i = 0; i < this.children.Count; i++)
		{
			this.children[i].OnOpen();
		}
		this.IsDirty = true;
		this.xui.CalloutWindow.EnableCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuShortcuts, 0f);
	}

	// Token: 0x06007848 RID: 30792 RVA: 0x002D56C4 File Offset: 0x002D38C4
	public override void OnClose()
	{
		this.wasReleased = false;
		this.activeKeyDown = false;
		base.OnClose();
		this.te.SetUserAccessing(false);
		LockManager.Instance.UnlockRequestLocal();
		this.te = null;
		this.xui.CalloutWindow.ClearCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuShortcuts);
	}

	// Token: 0x040058ED RID: 22765
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeaderWindow;

	// Token: 0x040058EE RID: 22766
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CombineGrid combineGrid;

	// Token: 0x040058EF RID: 22767
	[PublicizedFrom(EAccessModifier.Private)]
	public TEFeatureCombine te;

	// Token: 0x040058F0 RID: 22768
	[PublicizedFrom(EAccessModifier.Private)]
	public bool activeKeyDown;

	// Token: 0x040058F1 RID: 22769
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasReleased;
}

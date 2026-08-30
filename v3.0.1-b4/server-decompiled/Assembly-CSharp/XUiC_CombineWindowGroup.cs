using System;
using UnityEngine.Scripting;

// Token: 0x02000EE4 RID: 3812
[Preserve]
public class XUiC_CombineWindowGroup : XUiController
{
	// Token: 0x0600778C RID: 30604 RVA: 0x002D154A File Offset: 0x002CF74A
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

	// Token: 0x0600778D RID: 30605 RVA: 0x002D1579 File Offset: 0x002CF779
	public void SetTileEntity(TEFeatureCombine _teCombine)
	{
		this.te = _teCombine;
		if (this.combineGrid != null)
		{
			this.combineGrid.SetTileEntity(_teCombine);
		}
	}

	// Token: 0x0600778E RID: 30606 RVA: 0x002D1598 File Offset: 0x002CF798
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

	// Token: 0x0600778F RID: 30607 RVA: 0x002D1678 File Offset: 0x002CF878
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

	// Token: 0x06007790 RID: 30608 RVA: 0x002D1738 File Offset: 0x002CF938
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

	// Token: 0x0400583B RID: 22587
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeaderWindow;

	// Token: 0x0400583C RID: 22588
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CombineGrid combineGrid;

	// Token: 0x0400583D RID: 22589
	[PublicizedFrom(EAccessModifier.Private)]
	public TEFeatureCombine te;

	// Token: 0x0400583E RID: 22590
	[PublicizedFrom(EAccessModifier.Private)]
	public bool activeKeyDown;

	// Token: 0x0400583F RID: 22591
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasReleased;
}

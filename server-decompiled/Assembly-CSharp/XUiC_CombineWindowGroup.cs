using System;
using UnityEngine.Scripting;

// Token: 0x02000F15 RID: 3861
[Preserve]
public class XUiC_CombineWindowGroup : XUiController
{
	// Token: 0x06007865 RID: 30821 RVA: 0x002D9DC1 File Offset: 0x002D7FC1
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

	// Token: 0x06007866 RID: 30822 RVA: 0x002D9DF0 File Offset: 0x002D7FF0
	public void SetTileEntity(TEFeatureCombine _teCombine)
	{
		this.te = _teCombine;
		if (this.combineGrid != null)
		{
			this.combineGrid.SetTileEntity(_teCombine);
		}
	}

	// Token: 0x06007867 RID: 30823 RVA: 0x002D9E10 File Offset: 0x002D8010
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

	// Token: 0x06007868 RID: 30824 RVA: 0x002D9EF0 File Offset: 0x002D80F0
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

	// Token: 0x06007869 RID: 30825 RVA: 0x002D9FB0 File Offset: 0x002D81B0
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

	// Token: 0x0400594A RID: 22858
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeaderWindow;

	// Token: 0x0400594B RID: 22859
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CombineGrid combineGrid;

	// Token: 0x0400594C RID: 22860
	[PublicizedFrom(EAccessModifier.Private)]
	public TEFeatureCombine te;

	// Token: 0x0400594D RID: 22861
	[PublicizedFrom(EAccessModifier.Private)]
	public bool activeKeyDown;

	// Token: 0x0400594E RID: 22862
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasReleased;
}

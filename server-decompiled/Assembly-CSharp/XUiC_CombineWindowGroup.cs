using System;
using UnityEngine.Scripting;

// Token: 0x02000F0A RID: 3850
[Preserve]
public class XUiC_CombineWindowGroup : XUiController
{
	// Token: 0x0600783F RID: 30783 RVA: 0x002D8869 File Offset: 0x002D6A69
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

	// Token: 0x06007840 RID: 30784 RVA: 0x002D8898 File Offset: 0x002D6A98
	public void SetTileEntity(TEFeatureCombine _teCombine)
	{
		this.te = _teCombine;
		if (this.combineGrid != null)
		{
			this.combineGrid.SetTileEntity(_teCombine);
		}
	}

	// Token: 0x06007841 RID: 30785 RVA: 0x002D88B8 File Offset: 0x002D6AB8
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

	// Token: 0x06007842 RID: 30786 RVA: 0x002D8998 File Offset: 0x002D6B98
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

	// Token: 0x06007843 RID: 30787 RVA: 0x002D8A58 File Offset: 0x002D6C58
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

	// Token: 0x04005925 RID: 22821
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeaderWindow;

	// Token: 0x04005926 RID: 22822
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CombineGrid combineGrid;

	// Token: 0x04005927 RID: 22823
	[PublicizedFrom(EAccessModifier.Private)]
	public TEFeatureCombine te;

	// Token: 0x04005928 RID: 22824
	[PublicizedFrom(EAccessModifier.Private)]
	public bool activeKeyDown;

	// Token: 0x04005929 RID: 22825
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasReleased;
}

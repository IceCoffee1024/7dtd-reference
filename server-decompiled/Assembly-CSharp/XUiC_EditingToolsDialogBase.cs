using System;
using UnityEngine.Scripting;

// Token: 0x02000E15 RID: 3605
[Preserve]
public abstract class XUiC_EditingToolsDialogBase : XUiController
{
	// Token: 0x06006DEF RID: 28143 RVA: 0x002AE012 File Offset: 0x002AC212
	[XuiBindEvent("OnPress", "btnBack")]
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Event_BackOnPress(XUiController _sender, int _mouseButton)
	{
		this.close();
	}

	// Token: 0x06006DF0 RID: 28144 RVA: 0x002AE01C File Offset: 0x002AC21C
	public override void OnOpen()
	{
		this.xui.GetChildByType<XUiC_EditingTools>().SetWindow(this.windowGroup.Id);
		base.OnOpen();
		this.windowGroup.isEscClosable = false;
		this.xui.playerUI.windowManager.Close(XUiC_MainMenu.ID);
		this.xui.playerUI.windowManager.Open(XUiC_EditingTools.ID, false);
	}

	// Token: 0x06006DF1 RID: 28145 RVA: 0x002AE08B File Offset: 0x002AC28B
	public override void OnClose()
	{
		base.OnClose();
		this.xui.playerUI.windowManager.Close(XUiC_EditingTools.ID);
		this.xui.playerUI.windowManager.Open(XUiC_MainMenu.ID, true);
	}

	// Token: 0x06006DF2 RID: 28146 RVA: 0x002AE0C8 File Offset: 0x002AC2C8
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (base.handleDirtyUpdateDefault())
		{
			this.onDirtyUpdate();
		}
		if (XUiUtils.HotkeysAllowedFor(this.viewComponent ?? this.children[0].ViewComponent, false) && this.xui.playerUI.playerInput.PermanentActions.Cancel.WasReleased)
		{
			this.close();
		}
	}

	// Token: 0x06006DF3 RID: 28147 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void onDirtyUpdate()
	{
	}

	// Token: 0x06006DF4 RID: 28148 RVA: 0x00089715 File Offset: 0x00087915
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void close()
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x06006DF5 RID: 28149 RVA: 0x00089701 File Offset: 0x00087901
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_EditingToolsDialogBase()
	{
	}

	// Token: 0x040053E2 RID: 21474
	[XuiBindComponent("btnBack", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnBack;
}

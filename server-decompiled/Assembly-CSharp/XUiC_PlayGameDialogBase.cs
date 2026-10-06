using System;
using UnityEngine.Scripting;

// Token: 0x02000E3D RID: 3645
[Preserve]
public abstract class XUiC_PlayGameDialogBase : XUiController
{
	// Token: 0x06006F9B RID: 28571 RVA: 0x002B46D5 File Offset: 0x002B28D5
	[XuiBindEvent("OnPress", "btnBack")]
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Event_BackOnPress(XUiController _sender, int _mouseButton)
	{
		this.close();
	}

	// Token: 0x06006F9C RID: 28572 RVA: 0x002B46E0 File Offset: 0x002B28E0
	public override void OnOpen()
	{
		this.xui.GetChildByType<XUiC_PlayGameMenu>().SetWindow(this.windowGroup.Id);
		base.OnOpen();
		this.windowGroup.isEscClosable = false;
		this.xui.playerUI.windowManager.Close(XUiC_MainMenu.ID);
		this.xui.playerUI.windowManager.Open(XUiC_PlayGameMenu.ID, false);
	}

	// Token: 0x06006F9D RID: 28573 RVA: 0x002B474F File Offset: 0x002B294F
	public override void OnClose()
	{
		base.OnClose();
		this.xui.playerUI.windowManager.Close(XUiC_PlayGameMenu.ID);
		this.xui.playerUI.windowManager.Open(XUiC_MainMenu.ID, true);
	}

	// Token: 0x06006F9E RID: 28574 RVA: 0x002B478C File Offset: 0x002B298C
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

	// Token: 0x06006F9F RID: 28575 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void onDirtyUpdate()
	{
	}

	// Token: 0x06006FA0 RID: 28576 RVA: 0x00089715 File Offset: 0x00087915
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void close()
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x06006FA1 RID: 28577 RVA: 0x00089701 File Offset: 0x00087901
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_PlayGameDialogBase()
	{
	}

	// Token: 0x040054B5 RID: 21685
	[XuiBindComponent("btnBack", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnBack;
}

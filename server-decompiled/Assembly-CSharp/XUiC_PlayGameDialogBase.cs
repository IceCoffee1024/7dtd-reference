using System;
using UnityEngine.Scripting;

// Token: 0x02000E19 RID: 3609
[Preserve]
public abstract class XUiC_PlayGameDialogBase : XUiController
{
	// Token: 0x06006F03 RID: 28419 RVA: 0x002ADE11 File Offset: 0x002AC011
	[XuiBindEvent("OnPress", "btnBack")]
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Event_BackOnPress(XUiController _sender, int _mouseButton)
	{
		this.close();
	}

	// Token: 0x06006F04 RID: 28420 RVA: 0x002ADE1C File Offset: 0x002AC01C
	public override void OnOpen()
	{
		this.xui.GetChildByType<XUiC_PlayGameMenu>().SetWindow(this.windowGroup.Id);
		base.OnOpen();
		this.windowGroup.isEscClosable = false;
		this.xui.playerUI.windowManager.Close(XUiC_MainMenu.ID);
		this.xui.playerUI.windowManager.Open(XUiC_PlayGameMenu.ID, false);
	}

	// Token: 0x06006F05 RID: 28421 RVA: 0x002ADE8B File Offset: 0x002AC08B
	public override void OnClose()
	{
		base.OnClose();
		this.xui.playerUI.windowManager.Close(XUiC_PlayGameMenu.ID);
		this.xui.playerUI.windowManager.Open(XUiC_MainMenu.ID, true);
	}

	// Token: 0x06006F06 RID: 28422 RVA: 0x002ADEC8 File Offset: 0x002AC0C8
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

	// Token: 0x06006F07 RID: 28423 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void onDirtyUpdate()
	{
	}

	// Token: 0x06006F08 RID: 28424 RVA: 0x00088DF5 File Offset: 0x00086FF5
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void close()
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x06006F09 RID: 28425 RVA: 0x00088DE1 File Offset: 0x00086FE1
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_PlayGameDialogBase()
	{
	}

	// Token: 0x040053DA RID: 21466
	[XuiBindComponent("btnBack", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnBack;
}

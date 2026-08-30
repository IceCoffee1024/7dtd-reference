using System;
using UnityEngine.Scripting;

// Token: 0x02000E2E RID: 3630
[Preserve]
public abstract class XUiC_PlayGameDialogBase : XUiController
{
	// Token: 0x06006FA3 RID: 28579 RVA: 0x002B1325 File Offset: 0x002AF525
	[XuiBindEvent("OnPress", "btnBack")]
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Event_BackOnPress(XUiController _sender, int _mouseButton)
	{
		this.close();
	}

	// Token: 0x06006FA4 RID: 28580 RVA: 0x002B1330 File Offset: 0x002AF530
	public override void OnOpen()
	{
		this.xui.GetChildByType<XUiC_PlayGameMenu>().SetWindow(this.windowGroup.Id);
		base.OnOpen();
		this.windowGroup.isEscClosable = false;
		this.xui.playerUI.windowManager.Close(XUiC_MainMenu.ID);
		this.xui.playerUI.windowManager.Open(XUiC_PlayGameMenu.ID, false);
	}

	// Token: 0x06006FA5 RID: 28581 RVA: 0x002B139F File Offset: 0x002AF59F
	public override void OnClose()
	{
		base.OnClose();
		this.xui.playerUI.windowManager.Close(XUiC_PlayGameMenu.ID);
		this.xui.playerUI.windowManager.Open(XUiC_MainMenu.ID, true);
	}

	// Token: 0x06006FA6 RID: 28582 RVA: 0x002B13DC File Offset: 0x002AF5DC
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

	// Token: 0x06006FA7 RID: 28583 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void onDirtyUpdate()
	{
	}

	// Token: 0x06006FA8 RID: 28584 RVA: 0x00089495 File Offset: 0x00087695
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void close()
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x06006FA9 RID: 28585 RVA: 0x00089481 File Offset: 0x00087681
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_PlayGameDialogBase()
	{
	}

	// Token: 0x0400547E RID: 21630
	[XuiBindComponent("btnBack", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnBack;
}

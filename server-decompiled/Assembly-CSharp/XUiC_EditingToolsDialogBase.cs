using System;
using UnityEngine.Scripting;

// Token: 0x02000DF1 RID: 3569
[Preserve]
public abstract class XUiC_EditingToolsDialogBase : XUiController
{
	// Token: 0x06006D57 RID: 27991 RVA: 0x002A7798 File Offset: 0x002A5998
	[XuiBindEvent("OnPress", "btnBack")]
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Event_BackOnPress(XUiController _sender, int _mouseButton)
	{
		this.close();
	}

	// Token: 0x06006D58 RID: 27992 RVA: 0x002A77A0 File Offset: 0x002A59A0
	public override void OnOpen()
	{
		base.OnOpen();
		this.windowGroup.isEscClosable = false;
		this.xui.playerUI.windowManager.Open(XUiC_EditingTools.ID, false);
	}

	// Token: 0x06006D59 RID: 27993 RVA: 0x002A77CF File Offset: 0x002A59CF
	public override void OnClose()
	{
		base.OnClose();
		this.xui.playerUI.windowManager.Close(XUiC_EditingTools.ID);
		this.xui.playerUI.windowManager.Open(XUiC_MainMenu.ID, true);
	}

	// Token: 0x06006D5A RID: 27994 RVA: 0x002A780C File Offset: 0x002A5A0C
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

	// Token: 0x06006D5B RID: 27995 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void onDirtyUpdate()
	{
	}

	// Token: 0x06006D5C RID: 27996 RVA: 0x00088DF5 File Offset: 0x00086FF5
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void close()
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x06006D5D RID: 27997 RVA: 0x00088DE1 File Offset: 0x00086FE1
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_EditingToolsDialogBase()
	{
	}

	// Token: 0x04005308 RID: 21256
	[XuiBindComponent("btnBack", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnBack;
}

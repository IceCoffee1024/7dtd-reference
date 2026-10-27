using System;
using UnityEngine.Scripting;

// Token: 0x02000E48 RID: 3656
[Preserve]
public abstract class XUiC_PlayGameDialogBase : XUiController
{
	// Token: 0x06006FC0 RID: 28608 RVA: 0x002B5C41 File Offset: 0x002B3E41
	[XuiBindEvent("OnPress", "btnBack")]
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Event_BackOnPress(XUiController _sender, int _mouseButton)
	{
		this.close();
	}

	// Token: 0x06006FC1 RID: 28609 RVA: 0x002B5C4C File Offset: 0x002B3E4C
	public override void OnOpen()
	{
		this.xui.GetChildByType<XUiC_PlayGameMenu>().SetWindow(this.windowGroup.Id);
		base.OnOpen();
		this.windowGroup.isEscClosable = false;
		this.xui.playerUI.windowManager.Close(XUiC_MainMenu.ID);
		this.xui.playerUI.windowManager.Open(XUiC_PlayGameMenu.ID, false);
	}

	// Token: 0x06006FC2 RID: 28610 RVA: 0x002B5CBB File Offset: 0x002B3EBB
	public override void OnClose()
	{
		base.OnClose();
		this.xui.playerUI.windowManager.Close(XUiC_PlayGameMenu.ID);
		this.xui.playerUI.windowManager.Open(XUiC_MainMenu.ID, true);
	}

	// Token: 0x06006FC3 RID: 28611 RVA: 0x002B5CF8 File Offset: 0x002B3EF8
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

	// Token: 0x06006FC4 RID: 28612 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void onDirtyUpdate()
	{
	}

	// Token: 0x06006FC5 RID: 28613 RVA: 0x00089949 File Offset: 0x00087B49
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void close()
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x06006FC6 RID: 28614 RVA: 0x00089935 File Offset: 0x00087B35
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_PlayGameDialogBase()
	{
	}

	// Token: 0x040054DA RID: 21722
	[XuiBindComponent("btnBack", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnBack;
}

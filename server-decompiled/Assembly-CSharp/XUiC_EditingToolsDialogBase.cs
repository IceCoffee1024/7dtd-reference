using System;
using UnityEngine.Scripting;

// Token: 0x02000E20 RID: 3616
[Preserve]
public abstract class XUiC_EditingToolsDialogBase : XUiController
{
	// Token: 0x06006E14 RID: 28180 RVA: 0x002AF57E File Offset: 0x002AD77E
	[XuiBindEvent("OnPress", "btnBack")]
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Event_BackOnPress(XUiController _sender, int _mouseButton)
	{
		this.close();
	}

	// Token: 0x06006E15 RID: 28181 RVA: 0x002AF588 File Offset: 0x002AD788
	public override void OnOpen()
	{
		this.xui.GetChildByType<XUiC_EditingTools>().SetWindow(this.windowGroup.Id);
		base.OnOpen();
		this.windowGroup.isEscClosable = false;
		this.xui.playerUI.windowManager.Close(XUiC_MainMenu.ID);
		this.xui.playerUI.windowManager.Open(XUiC_EditingTools.ID, false);
	}

	// Token: 0x06006E16 RID: 28182 RVA: 0x002AF5F7 File Offset: 0x002AD7F7
	public override void OnClose()
	{
		base.OnClose();
		this.xui.playerUI.windowManager.Close(XUiC_EditingTools.ID);
		this.xui.playerUI.windowManager.Open(XUiC_MainMenu.ID, true);
	}

	// Token: 0x06006E17 RID: 28183 RVA: 0x002AF634 File Offset: 0x002AD834
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

	// Token: 0x06006E18 RID: 28184 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void onDirtyUpdate()
	{
	}

	// Token: 0x06006E19 RID: 28185 RVA: 0x00089949 File Offset: 0x00087B49
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void close()
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x06006E1A RID: 28186 RVA: 0x00089935 File Offset: 0x00087B35
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_EditingToolsDialogBase()
	{
	}

	// Token: 0x04005407 RID: 21511
	[XuiBindComponent("btnBack", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnBack;
}

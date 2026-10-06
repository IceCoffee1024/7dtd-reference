using System;
using UnityEngine.Scripting;

// Token: 0x02000EF5 RID: 3829
[Preserve]
public class XUiC_CharacterSheetWindowGroup : XUiController
{
	// Token: 0x060077C1 RID: 30657 RVA: 0x002D486D File Offset: 0x002D2A6D
	public override void Init()
	{
		base.Init();
		this.buffList = base.GetChildByType<XUiC_ActiveBuffList>();
	}

	// Token: 0x060077C2 RID: 30658 RVA: 0x002D4884 File Offset: 0x002D2A84
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.buffList != null)
		{
			this.buffList.setFirstEntry = true;
		}
		this.xui.playerUI.windowManager.Open("windowpaging", false);
		XUiC_WindowSelector childByType = this.xui.FindWindowGroupByName("windowpaging").GetChildByType<XUiC_WindowSelector>();
		if (childByType != null)
		{
			childByType.SetSelected("character");
		}
		if (this.xui.PlayerEquipment != null)
		{
			this.xui.PlayerEquipment.IsOpen = true;
		}
	}

	// Token: 0x060077C3 RID: 30659 RVA: 0x002D4908 File Offset: 0x002D2B08
	public override void OnClose()
	{
		base.OnClose();
		this.xui.playerUI.windowManager.Close("windowpaging");
		if (this.xui.PlayerEquipment != null)
		{
			this.xui.PlayerEquipment.IsOpen = false;
		}
	}

	// Token: 0x040058C0 RID: 22720
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ActiveBuffList buffList;
}

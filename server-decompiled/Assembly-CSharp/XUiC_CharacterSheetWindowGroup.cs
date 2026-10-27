using System;
using UnityEngine.Scripting;

// Token: 0x02000F00 RID: 3840
[Preserve]
public class XUiC_CharacterSheetWindowGroup : XUiController
{
	// Token: 0x060077E7 RID: 30695 RVA: 0x002D5DC5 File Offset: 0x002D3FC5
	public override void Init()
	{
		base.Init();
		this.buffList = base.GetChildByType<XUiC_ActiveBuffList>();
	}

	// Token: 0x060077E8 RID: 30696 RVA: 0x002D5DDC File Offset: 0x002D3FDC
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

	// Token: 0x060077E9 RID: 30697 RVA: 0x002D5E60 File Offset: 0x002D4060
	public override void OnClose()
	{
		base.OnClose();
		this.xui.playerUI.windowManager.Close("windowpaging");
		if (this.xui.PlayerEquipment != null)
		{
			this.xui.PlayerEquipment.IsOpen = false;
		}
	}

	// Token: 0x040058E5 RID: 22757
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ActiveBuffList buffList;
}

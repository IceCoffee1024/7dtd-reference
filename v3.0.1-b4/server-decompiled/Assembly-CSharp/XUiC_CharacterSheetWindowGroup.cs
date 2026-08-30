using System;
using UnityEngine.Scripting;

// Token: 0x02000ED1 RID: 3793
[Preserve]
public class XUiC_CharacterSheetWindowGroup : XUiController
{
	// Token: 0x06007724 RID: 30500 RVA: 0x002CDE75 File Offset: 0x002CC075
	public override void Init()
	{
		base.Init();
		this.buffList = base.GetChildByType<XUiC_ActiveBuffList>();
	}

	// Token: 0x06007725 RID: 30501 RVA: 0x002CDE8C File Offset: 0x002CC08C
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

	// Token: 0x06007726 RID: 30502 RVA: 0x002CDF10 File Offset: 0x002CC110
	public override void OnClose()
	{
		base.OnClose();
		this.xui.playerUI.windowManager.Close("windowpaging");
		if (this.xui.PlayerEquipment != null)
		{
			this.xui.PlayerEquipment.IsOpen = false;
		}
	}

	// Token: 0x040057E2 RID: 22498
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ActiveBuffList buffList;
}

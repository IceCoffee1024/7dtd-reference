using System;
using UnityEngine.Scripting;

// Token: 0x02000EE6 RID: 3814
[Preserve]
public class XUiC_CharacterSheetWindowGroup : XUiController
{
	// Token: 0x060077C6 RID: 30662 RVA: 0x002D14F9 File Offset: 0x002CF6F9
	public override void Init()
	{
		base.Init();
		this.buffList = base.GetChildByType<XUiC_ActiveBuffList>();
	}

	// Token: 0x060077C7 RID: 30663 RVA: 0x002D1510 File Offset: 0x002CF710
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

	// Token: 0x060077C8 RID: 30664 RVA: 0x002D1594 File Offset: 0x002CF794
	public override void OnClose()
	{
		base.OnClose();
		this.xui.playerUI.windowManager.Close("windowpaging");
		if (this.xui.PlayerEquipment != null)
		{
			this.xui.PlayerEquipment.IsOpen = false;
		}
	}

	// Token: 0x04005888 RID: 22664
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ActiveBuffList buffList;
}

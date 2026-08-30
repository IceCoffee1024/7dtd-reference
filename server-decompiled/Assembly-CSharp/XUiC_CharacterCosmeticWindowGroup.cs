using System;
using UnityEngine.Scripting;

// Token: 0x02000EE4 RID: 3812
[Preserve]
public class XUiC_CharacterCosmeticWindowGroup : XUiController
{
	// Token: 0x060077AD RID: 30637 RVA: 0x002CF818 File Offset: 0x002CDA18
	public override void Init()
	{
		base.Init();
		if (base.GetChildByType<XUiC_WindowNonPagingHeader>() != null)
		{
			this.nonPagingHeaderWindow = base.GetChildByType<XUiC_WindowNonPagingHeader>();
		}
		if (base.GetChildByType<XUiC_CharacterCosmeticWindow>() != null)
		{
			this.characterCosmeticWindow = base.GetChildByType<XUiC_CharacterCosmeticWindow>();
		}
		if (base.GetChildByType<XUiC_CharacterCosmeticsListWindow>() != null)
		{
			this.characterCosmeticListWindow = base.GetChildByType<XUiC_CharacterCosmeticsListWindow>();
		}
	}

	// Token: 0x060077AE RID: 30638 RVA: 0x002CF867 File Offset: 0x002CDA67
	public static void Open(XUi xui, EquipmentSlots slot)
	{
		GUIWindowManager windowManager = xui.playerUI.windowManager;
		windowManager.Open("cosmetics", true);
		((XUiC_CharacterCosmeticWindowGroup)((XUiWindowGroup)windowManager.GetWindow("cosmetics")).Controller).SetEquipSlot(slot);
	}

	// Token: 0x060077AF RID: 30639 RVA: 0x002CF8A0 File Offset: 0x002CDAA0
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.nonPagingHeaderWindow != null)
		{
			this.nonPagingHeaderWindow.SetHeader(Localization.Get("xuiCosmetics", false, null));
		}
		if (this.xui.PlayerEquipment != null)
		{
			this.xui.PlayerEquipment.IsOpen = true;
		}
	}

	// Token: 0x060077B0 RID: 30640 RVA: 0x002CF8F0 File Offset: 0x002CDAF0
	public void SetEquipSlot(EquipmentSlots equipSlot)
	{
		if (equipSlot != EquipmentSlots.Count)
		{
			this.characterCosmeticListWindow.SetCategory(equipSlot);
		}
	}

	// Token: 0x060077B1 RID: 30641 RVA: 0x002CF903 File Offset: 0x002CDB03
	public override void OnClose()
	{
		base.OnClose();
		if (this.xui.PlayerEquipment != null)
		{
			this.xui.PlayerEquipment.IsOpen = false;
		}
	}

	// Token: 0x060077B2 RID: 30642 RVA: 0x002CF929 File Offset: 0x002CDB29
	public void ResetPreview()
	{
		this.characterCosmeticWindow.MakePreview();
	}

	// Token: 0x0400585E RID: 22622
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeaderWindow;

	// Token: 0x0400585F RID: 22623
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CharacterCosmeticWindow characterCosmeticWindow;

	// Token: 0x04005860 RID: 22624
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CharacterCosmeticsListWindow characterCosmeticListWindow;
}

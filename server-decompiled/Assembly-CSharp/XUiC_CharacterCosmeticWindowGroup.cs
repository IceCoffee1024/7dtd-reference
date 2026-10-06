using System;
using UnityEngine.Scripting;

// Token: 0x02000EF3 RID: 3827
[Preserve]
public class XUiC_CharacterCosmeticWindowGroup : XUiController
{
	// Token: 0x060077A8 RID: 30632 RVA: 0x002D2BEC File Offset: 0x002D0DEC
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

	// Token: 0x060077A9 RID: 30633 RVA: 0x002D2C3B File Offset: 0x002D0E3B
	public static void Open(XUi xui, EquipmentSlots slot)
	{
		GUIWindowManager windowManager = xui.playerUI.windowManager;
		windowManager.Open("cosmetics", true);
		((XUiC_CharacterCosmeticWindowGroup)((XUiWindowGroup)windowManager.GetWindow("cosmetics")).Controller).SetEquipSlot(slot);
	}

	// Token: 0x060077AA RID: 30634 RVA: 0x002D2C74 File Offset: 0x002D0E74
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

	// Token: 0x060077AB RID: 30635 RVA: 0x002D2CC4 File Offset: 0x002D0EC4
	public void SetEquipSlot(EquipmentSlots equipSlot)
	{
		if (equipSlot != EquipmentSlots.Count)
		{
			this.characterCosmeticListWindow.SetCategory(equipSlot);
		}
	}

	// Token: 0x060077AC RID: 30636 RVA: 0x002D2CD7 File Offset: 0x002D0ED7
	public override void OnClose()
	{
		base.OnClose();
		if (this.xui.PlayerEquipment != null)
		{
			this.xui.PlayerEquipment.IsOpen = false;
		}
	}

	// Token: 0x060077AD RID: 30637 RVA: 0x002D2CFD File Offset: 0x002D0EFD
	public void ResetPreview()
	{
		this.characterCosmeticWindow.MakePreview();
	}

	// Token: 0x04005896 RID: 22678
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeaderWindow;

	// Token: 0x04005897 RID: 22679
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CharacterCosmeticWindow characterCosmeticWindow;

	// Token: 0x04005898 RID: 22680
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CharacterCosmeticsListWindow characterCosmeticListWindow;
}

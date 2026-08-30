using System;
using UnityEngine.Scripting;

// Token: 0x02000ECF RID: 3791
[Preserve]
public class XUiC_CharacterCosmeticWindowGroup : XUiController
{
	// Token: 0x0600770B RID: 30475 RVA: 0x002CC194 File Offset: 0x002CA394
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

	// Token: 0x0600770C RID: 30476 RVA: 0x002CC1E3 File Offset: 0x002CA3E3
	public static void Open(XUi xui, EquipmentSlots slot)
	{
		GUIWindowManager windowManager = xui.playerUI.windowManager;
		windowManager.Open("cosmetics", true);
		((XUiC_CharacterCosmeticWindowGroup)((XUiWindowGroup)windowManager.GetWindow("cosmetics")).Controller).SetEquipSlot(slot);
	}

	// Token: 0x0600770D RID: 30477 RVA: 0x002CC21C File Offset: 0x002CA41C
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

	// Token: 0x0600770E RID: 30478 RVA: 0x002CC26C File Offset: 0x002CA46C
	public void SetEquipSlot(EquipmentSlots equipSlot)
	{
		if (equipSlot != EquipmentSlots.Count)
		{
			this.characterCosmeticListWindow.SetCategory(equipSlot);
		}
	}

	// Token: 0x0600770F RID: 30479 RVA: 0x002CC27F File Offset: 0x002CA47F
	public override void OnClose()
	{
		base.OnClose();
		if (this.xui.PlayerEquipment != null)
		{
			this.xui.PlayerEquipment.IsOpen = false;
		}
	}

	// Token: 0x06007710 RID: 30480 RVA: 0x002CC2A5 File Offset: 0x002CA4A5
	public void ResetPreview()
	{
		this.characterCosmeticWindow.MakePreview();
	}

	// Token: 0x040057B8 RID: 22456
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeaderWindow;

	// Token: 0x040057B9 RID: 22457
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CharacterCosmeticWindow characterCosmeticWindow;

	// Token: 0x040057BA RID: 22458
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CharacterCosmeticsListWindow characterCosmeticListWindow;
}

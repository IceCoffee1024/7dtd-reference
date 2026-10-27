using System;
using UnityEngine.Scripting;

// Token: 0x02000EFE RID: 3838
[Preserve]
public class XUiC_CharacterCosmeticWindowGroup : XUiController
{
	// Token: 0x060077CE RID: 30670 RVA: 0x002D4144 File Offset: 0x002D2344
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

	// Token: 0x060077CF RID: 30671 RVA: 0x002D4193 File Offset: 0x002D2393
	public static void Open(XUi xui, EquipmentSlots slot)
	{
		GUIWindowManager windowManager = xui.playerUI.windowManager;
		windowManager.Open("cosmetics", true);
		((XUiC_CharacterCosmeticWindowGroup)((XUiWindowGroup)windowManager.GetWindow("cosmetics")).Controller).SetEquipSlot(slot);
	}

	// Token: 0x060077D0 RID: 30672 RVA: 0x002D41CC File Offset: 0x002D23CC
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

	// Token: 0x060077D1 RID: 30673 RVA: 0x002D421C File Offset: 0x002D241C
	public void SetEquipSlot(EquipmentSlots equipSlot)
	{
		if (equipSlot != EquipmentSlots.Count)
		{
			this.characterCosmeticListWindow.SetCategory(equipSlot);
		}
	}

	// Token: 0x060077D2 RID: 30674 RVA: 0x002D422F File Offset: 0x002D242F
	public override void OnClose()
	{
		base.OnClose();
		if (this.xui.PlayerEquipment != null)
		{
			this.xui.PlayerEquipment.IsOpen = false;
		}
	}

	// Token: 0x060077D3 RID: 30675 RVA: 0x002D4255 File Offset: 0x002D2455
	public void ResetPreview()
	{
		this.characterCosmeticWindow.MakePreview();
	}

	// Token: 0x040058BB RID: 22715
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeaderWindow;

	// Token: 0x040058BC RID: 22716
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CharacterCosmeticWindow characterCosmeticWindow;

	// Token: 0x040058BD RID: 22717
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CharacterCosmeticsListWindow characterCosmeticListWindow;
}

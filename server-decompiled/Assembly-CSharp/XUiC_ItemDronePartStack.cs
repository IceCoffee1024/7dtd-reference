using System;
using UnityEngine.Scripting;

// Token: 0x02000F2B RID: 3883
[Preserve]
public class XUiC_ItemDronePartStack : XUiC_ItemPartStack
{
	// Token: 0x060079DE RID: 31198 RVA: 0x002DE448 File Offset: 0x002DC648
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool CanSwap(ItemStack stack)
	{
		bool flag = base.CanSwap(stack);
		if (base.ItemClass != null && this.itemClass.HasAnyTags(EntityDrone.StorageModifierTags))
		{
			EntityDrone currentVehicleEntity = ((XUiC_DroneWindowGroup)this.windowGroup.Controller).CurrentVehicleEntity;
			if (!currentVehicleEntity.CanRemoveExtraStorage())
			{
				currentVehicleEntity.NotifyToManyStoredItems();
				return false;
			}
		}
		return flag;
	}

	// Token: 0x060079DF RID: 31199 RVA: 0x002DE4A0 File Offset: 0x002DC6A0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool CanRemove()
	{
		bool flag = base.CanRemove();
		if (this.itemClass != null && this.itemClass.HasAnyTags(EntityDrone.StorageModifierTags))
		{
			EntityDrone currentVehicleEntity = ((XUiC_DroneWindowGroup)this.windowGroup.Controller).CurrentVehicleEntity;
			if (!currentVehicleEntity.CanRemoveExtraStorage())
			{
				if (this.xui.playerUI.CursorController.GetMouseButtonUp(UICamera.MouseButton.LeftButton))
				{
					currentVehicleEntity.NotifyToManyStoredItems();
				}
				return false;
			}
		}
		return flag;
	}
}

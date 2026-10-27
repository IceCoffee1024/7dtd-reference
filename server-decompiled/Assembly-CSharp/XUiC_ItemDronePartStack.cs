using System;
using UnityEngine.Scripting;

// Token: 0x02000F49 RID: 3913
[Preserve]
public class XUiC_ItemDronePartStack : XUiC_ItemPartStack
{
	// Token: 0x06007A1E RID: 31262 RVA: 0x002E3104 File Offset: 0x002E1304
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

	// Token: 0x06007A1F RID: 31263 RVA: 0x002E315C File Offset: 0x002E135C
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

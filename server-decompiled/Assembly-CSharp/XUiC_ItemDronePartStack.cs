using System;
using UnityEngine.Scripting;

// Token: 0x02000F3A RID: 3898
[Preserve]
public class XUiC_ItemDronePartStack : XUiC_ItemPartStack
{
	// Token: 0x060079DB RID: 31195 RVA: 0x002E1814 File Offset: 0x002DFA14
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

	// Token: 0x060079DC RID: 31196 RVA: 0x002E186C File Offset: 0x002DFA6C
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

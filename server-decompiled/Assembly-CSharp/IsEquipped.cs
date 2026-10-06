using System;
using UnityEngine.Scripting;

// Token: 0x0200067F RID: 1663
[Preserve]
public class IsEquipped : TargetedCompareRequirementBase
{
	// Token: 0x06003544 RID: 13636 RVA: 0x0016020C File Offset: 0x0015E40C
	public override bool IsValid(MinEventParams _params)
	{
		if (base.IsValid(_params))
		{
			ItemValue itemValue = _params.ItemValue;
			if (itemValue != null && this.target != null)
			{
				if (itemValue.IsMod)
				{
					int slotCount = this.target.equipment.GetSlotCount();
					for (int i = 0; i < slotCount; i++)
					{
						ItemValue slotItem = this.target.equipment.GetSlotItem(i);
						if (!slotItem.IsEmpty() && slotItem.HasModSlots)
						{
							for (int j = 0; j < slotItem.ModificationCount; j++)
							{
								ItemValue modification = slotItem.GetModification(j);
								if (modification != null && modification == _params.ItemValue)
								{
									return true;
								}
							}
						}
					}
				}
				else
				{
					int slotCount2 = this.target.equipment.GetSlotCount();
					for (int k = 0; k < slotCount2; k++)
					{
						if (this.target.equipment.GetSlotItem(k) == itemValue)
						{
							return true;
						}
					}
				}
			}
		}
		return false;
	}
}

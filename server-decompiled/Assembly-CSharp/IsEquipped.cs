using System;
using UnityEngine.Scripting;

// Token: 0x02000686 RID: 1670
[Preserve]
public class IsEquipped : TargetedCompareRequirementBase
{
	// Token: 0x06003552 RID: 13650 RVA: 0x00160E00 File Offset: 0x0015F000
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

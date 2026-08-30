using System;
using UnityEngine.Scripting;

// Token: 0x0200067D RID: 1661
[Preserve]
public class IsEquipped : TargetedCompareRequirementBase
{
	// Token: 0x06003501 RID: 13569 RVA: 0x0015F8A8 File Offset: 0x0015DAA8
	public override bool IsValid(MinEventParams _params)
	{
		if (base.IsValid(_params))
		{
			ItemValue itemValue = _params.ItemValue;
			if (itemValue != null && this.target != null)
			{
				if (itemValue.IsMod)
				{
					foreach (ItemValue itemValue2 in this.target.equipment.GetItems())
					{
						if (itemValue2 != null && itemValue2.HasModSlots)
						{
							foreach (ItemValue itemValue3 in itemValue2.Modifications)
							{
								if (itemValue3 != null && itemValue3 == _params.ItemValue)
								{
									return true;
								}
							}
						}
					}
				}
				else
				{
					ItemValue[] array = this.target.equipment.GetItems();
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i] == itemValue)
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

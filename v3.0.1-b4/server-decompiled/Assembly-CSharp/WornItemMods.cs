using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006A6 RID: 1702
[Preserve]
public class WornItemMods : TargetedCompareRequirementBase
{
	// Token: 0x0600354E RID: 13646 RVA: 0x0015FBCC File Offset: 0x0015DDCC
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		int num = 0;
		int slotCount = this.target.equipment.GetSlotCount();
		for (int i = 0; i < slotCount; i++)
		{
			ItemValue slotItem = this.target.equipment.GetSlotItem(i);
			if (slotItem != null)
			{
				int num2 = slotItem.Modifications.Length;
				for (int j = 0; j < num2; j++)
				{
					ItemValue itemValue = slotItem.Modifications[j];
					if (itemValue != null)
					{
						ItemClass itemClass = itemValue.ItemClass;
						if (itemClass != null && itemClass.HasAnyTags(this.tags))
						{
							num++;
						}
					}
				}
			}
		}
		return this.invert != RequirementBase.compareValues((float)num, this.operation, this.value);
	}

	// Token: 0x0600354F RID: 13647 RVA: 0x0015FB49 File Offset: 0x0015DD49
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("WornItems: {0}{1} {2}", this.invert ? "NOT " : "", this.operation.ToStringCached<RequirementBase.OperationTypes>(), this.value.ToCultureInvariantString()));
	}

	// Token: 0x06003550 RID: 13648 RVA: 0x0015FC80 File Offset: 0x0015DE80
	public override bool ParseXAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "tags")
		{
			this.tags = FastTags<TagGroup.Global>.Parse(_attribute.Value);
			return true;
		}
		return flag;
	}

	// Token: 0x04002AEE RID: 10990
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> tags;
}

using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006B0 RID: 1712
[Preserve]
public class WornItemMods : TargetedCompareRequirementBase
{
	// Token: 0x060035A4 RID: 13732 RVA: 0x00161DA4 File Offset: 0x0015FFA4
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

	// Token: 0x060035A5 RID: 13733 RVA: 0x00161D21 File Offset: 0x0015FF21
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("WornItems: {0}{1} {2}", this.invert ? "NOT " : "", this.operation.ToStringCached<RequirementBase.OperationTypes>(), this.value.ToCultureInvariantString()));
	}

	// Token: 0x060035A6 RID: 13734 RVA: 0x00161E58 File Offset: 0x00160058
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

	// Token: 0x04002B62 RID: 11106
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> tags;
}

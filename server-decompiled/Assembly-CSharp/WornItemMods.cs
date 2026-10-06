using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006B2 RID: 1714
[Preserve]
public class WornItemMods : TargetedCompareRequirementBase
{
	// Token: 0x060035E7 RID: 13799 RVA: 0x00162710 File Offset: 0x00160910
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
			if (!slotItem.IsEmpty())
			{
				int modificationCount = slotItem.ModificationCount;
				for (int j = 0; j < modificationCount; j++)
				{
					ItemValue modification = slotItem.GetModification(j);
					if (modification != null)
					{
						ItemClass itemClass = modification.ItemClass;
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

	// Token: 0x060035E8 RID: 13800 RVA: 0x0016268E File Offset: 0x0016088E
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("WornItems: {0}{1} {2}", this.invert ? "NOT " : "", this.operation.ToStringCached<RequirementBase.OperationTypes>(), this.value.ToCultureInvariantString()));
	}

	// Token: 0x060035E9 RID: 13801 RVA: 0x001627C8 File Offset: 0x001609C8
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

	// Token: 0x04002B5B RID: 11099
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> tags;
}

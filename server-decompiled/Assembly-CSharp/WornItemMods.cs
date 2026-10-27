using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006B9 RID: 1721
[Preserve]
public class WornItemMods : TargetedCompareRequirementBase
{
	// Token: 0x060035F5 RID: 13813 RVA: 0x00163304 File Offset: 0x00161504
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

	// Token: 0x060035F6 RID: 13814 RVA: 0x00163282 File Offset: 0x00161482
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("WornItems: {0}{1} {2}", this.invert ? "NOT " : "", this.operation.ToStringCached<RequirementBase.OperationTypes>(), this.value.ToCultureInvariantString()));
	}

	// Token: 0x060035F7 RID: 13815 RVA: 0x001633BC File Offset: 0x001615BC
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

	// Token: 0x04002B76 RID: 11126
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> tags;
}

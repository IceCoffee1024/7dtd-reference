using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006B1 RID: 1713
[Preserve]
public class WornItems : TargetedCompareRequirementBase
{
	// Token: 0x060035E3 RID: 13795 RVA: 0x00162608 File Offset: 0x00160808
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
			if (!slotItem.IsEmpty() && slotItem.ItemClass.HasAnyTags(this.equipmentTags))
			{
				num++;
			}
		}
		return this.invert != RequirementBase.compareValues((float)num, this.operation, this.value);
	}

	// Token: 0x060035E4 RID: 13796 RVA: 0x0016268E File Offset: 0x0016088E
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("WornItems: {0}{1} {2}", this.invert ? "NOT " : "", this.operation.ToStringCached<RequirementBase.OperationTypes>(), this.value.ToCultureInvariantString()));
	}

	// Token: 0x060035E5 RID: 13797 RVA: 0x001626CC File Offset: 0x001608CC
	public override bool ParseXAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "tags")
		{
			this.equipmentTags = FastTags<TagGroup.Global>.Parse(_attribute.Value);
			return true;
		}
		return flag;
	}

	// Token: 0x04002B5A RID: 11098
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> equipmentTags;
}

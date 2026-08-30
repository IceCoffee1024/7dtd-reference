using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006AF RID: 1711
[Preserve]
public class WornItems : TargetedCompareRequirementBase
{
	// Token: 0x060035A0 RID: 13728 RVA: 0x00161CA0 File Offset: 0x0015FEA0
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
			if (slotItem != null && slotItem.ItemClass.HasAnyTags(this.equipmentTags))
			{
				num++;
			}
		}
		return this.invert != RequirementBase.compareValues((float)num, this.operation, this.value);
	}

	// Token: 0x060035A1 RID: 13729 RVA: 0x00161D21 File Offset: 0x0015FF21
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("WornItems: {0}{1} {2}", this.invert ? "NOT " : "", this.operation.ToStringCached<RequirementBase.OperationTypes>(), this.value.ToCultureInvariantString()));
	}

	// Token: 0x060035A2 RID: 13730 RVA: 0x00161D60 File Offset: 0x0015FF60
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

	// Token: 0x04002B61 RID: 11105
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> equipmentTags;
}

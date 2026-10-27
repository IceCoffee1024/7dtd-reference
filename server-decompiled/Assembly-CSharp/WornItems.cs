using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006B8 RID: 1720
[Preserve]
public class WornItems : TargetedCompareRequirementBase
{
	// Token: 0x060035F1 RID: 13809 RVA: 0x001631FC File Offset: 0x001613FC
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

	// Token: 0x060035F2 RID: 13810 RVA: 0x00163282 File Offset: 0x00161482
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("WornItems: {0}{1} {2}", this.invert ? "NOT " : "", this.operation.ToStringCached<RequirementBase.OperationTypes>(), this.value.ToCultureInvariantString()));
	}

	// Token: 0x060035F3 RID: 13811 RVA: 0x001632C0 File Offset: 0x001614C0
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

	// Token: 0x04002B75 RID: 11125
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> equipmentTags;
}

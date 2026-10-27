using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000695 RID: 1685
[Preserve]
public class RequirementItemModTier : RequirementBase
{
	// Token: 0x06003583 RID: 13699 RVA: 0x00161A60 File Offset: 0x0015FC60
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		if (_params.ItemValue == null)
		{
			return false;
		}
		if (!_params.ItemValue.HasModSlots)
		{
			return false;
		}
		ItemValue itemValue = ItemValue.None;
		for (int i = 0; i < _params.ItemValue.ModificationCount; i++)
		{
			ItemValue modification = _params.ItemValue.GetModification(i);
			if (modification != null && modification != ItemValue.None)
			{
				ItemClass itemClass = modification.ItemClass;
				if (itemClass != null && itemClass.Name != null && itemClass.Name.EqualsCaseInsensitive(this.modName))
				{
					itemValue = modification;
					break;
				}
			}
		}
		if (!this.invert)
		{
			return RequirementBase.compareValues((float)itemValue.Quality, this.operation, this.value);
		}
		return !RequirementBase.compareValues((float)itemValue.Quality, this.operation, this.value);
	}

	// Token: 0x06003584 RID: 13700 RVA: 0x00161B2A File Offset: 0x0015FD2A
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Item Mod tier {0}{1} {2}", this.invert ? "NOT " : "", this.operation.ToStringCached<RequirementBase.OperationTypes>(), this.value.ToCultureInvariantString()));
	}

	// Token: 0x06003585 RID: 13701 RVA: 0x00161B68 File Offset: 0x0015FD68
	public override bool ParseXAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "mod_name")
		{
			this.modName = _attribute.Value;
			return true;
		}
		return flag;
	}

	// Token: 0x04002B5E RID: 11102
	[PublicizedFrom(EAccessModifier.Private)]
	public string modName;
}

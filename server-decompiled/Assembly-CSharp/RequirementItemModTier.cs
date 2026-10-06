using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x0200068E RID: 1678
[Preserve]
public class RequirementItemModTier : RequirementBase
{
	// Token: 0x06003575 RID: 13685 RVA: 0x00160E6C File Offset: 0x0015F06C
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

	// Token: 0x06003576 RID: 13686 RVA: 0x00160F36 File Offset: 0x0015F136
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Item Mod tier {0}{1} {2}", this.invert ? "NOT " : "", this.operation.ToStringCached<RequirementBase.OperationTypes>(), this.value.ToCultureInvariantString()));
	}

	// Token: 0x06003577 RID: 13687 RVA: 0x00160F74 File Offset: 0x0015F174
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

	// Token: 0x04002B43 RID: 11075
	[PublicizedFrom(EAccessModifier.Private)]
	public string modName;
}

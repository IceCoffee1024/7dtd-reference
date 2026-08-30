using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x0200068C RID: 1676
[Preserve]
public class RequirementItemModTier : RequirementBase
{
	// Token: 0x06003532 RID: 13618 RVA: 0x001604E8 File Offset: 0x0015E6E8
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
		for (int i = 0; i < _params.ItemValue.Modifications.Length; i++)
		{
			ItemValue itemValue2 = _params.ItemValue.Modifications[i];
			if (itemValue2 != null && itemValue2 != ItemValue.None)
			{
				ItemClass itemClass = itemValue2.ItemClass;
				if (itemClass != null && itemClass.Name != null && itemClass.Name.EqualsCaseInsensitive(this.modName))
				{
					itemValue = itemValue2;
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

	// Token: 0x06003533 RID: 13619 RVA: 0x001605B5 File Offset: 0x0015E7B5
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Item Mod tier {0}{1} {2}", this.invert ? "NOT " : "", this.operation.ToStringCached<RequirementBase.OperationTypes>(), this.value.ToCultureInvariantString()));
	}

	// Token: 0x06003534 RID: 13620 RVA: 0x001605F4 File Offset: 0x0015E7F4
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

	// Token: 0x04002B4A RID: 11082
	[PublicizedFrom(EAccessModifier.Private)]
	public string modName;
}

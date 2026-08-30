using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000682 RID: 1666
[Preserve]
public class RequirementItemModTier : RequirementBase
{
	// Token: 0x060034DC RID: 13532 RVA: 0x0015E30C File Offset: 0x0015C50C
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
			if (itemValue2 != ItemValue.None && itemValue2.ItemClass != null && itemValue2.ItemClass.Name != null && itemValue2.ItemClass.Name.EqualsCaseInsensitive(this.modName))
			{
				itemValue = itemValue2;
				break;
			}
		}
		if (!this.invert)
		{
			return RequirementBase.compareValues((float)itemValue.Quality, this.operation, this.value);
		}
		return !RequirementBase.compareValues((float)itemValue.Quality, this.operation, this.value);
	}

	// Token: 0x060034DD RID: 13533 RVA: 0x0015E3DE File Offset: 0x0015C5DE
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Item Mod tier {0}{1} {2}", this.invert ? "NOT " : "", this.operation.ToStringCached<RequirementBase.OperationTypes>(), this.value.ToCultureInvariantString()));
	}

	// Token: 0x060034DE RID: 13534 RVA: 0x0015E41C File Offset: 0x0015C61C
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

	// Token: 0x04002AD6 RID: 10966
	[PublicizedFrom(EAccessModifier.Private)]
	public string modName;
}

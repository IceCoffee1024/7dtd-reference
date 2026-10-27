using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x0200068A RID: 1674
[Preserve]
public class ItemHasTags : TargetedCompareRequirementBase
{
	// Token: 0x0600355E RID: 13662 RVA: 0x001610E4 File Offset: 0x0015F2E4
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		bool flag = false;
		if (!_params.ItemValue.IsEmpty() && _params.ItemValue.ItemClass != null)
		{
			if (!this.hasAllTags)
			{
				flag = _params.ItemValue.ItemClass.HasAnyTags(this.currentItemTags);
			}
			else
			{
				flag = _params.ItemValue.ItemClass.HasAllTags(this.currentItemTags);
			}
		}
		if (!this.invert)
		{
			return flag;
		}
		return !flag;
	}

	// Token: 0x0600355F RID: 13663 RVA: 0x000027FC File Offset: 0x000009FC
	public override void GetInfoStrings(ref List<string> list)
	{
	}

	// Token: 0x06003560 RID: 13664 RVA: 0x00161160 File Offset: 0x0015F360
	public override bool ParseXAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXAttribute(_attribute);
		if (!flag)
		{
			string localName = _attribute.Name.LocalName;
			if (localName == "tags")
			{
				this.currentItemTags = FastTags<TagGroup.Global>.Parse(_attribute.Value);
				return true;
			}
			if (localName == "has_all_tags")
			{
				this.hasAllTags = StringParsers.ParseBool(_attribute.Value, 0, -1, true);
				return true;
			}
		}
		return flag;
	}

	// Token: 0x04002B4E RID: 11086
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> currentItemTags;

	// Token: 0x04002B4F RID: 11087
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasAllTags;
}

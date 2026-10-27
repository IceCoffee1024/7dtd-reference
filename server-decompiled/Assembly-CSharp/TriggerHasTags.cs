using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x0200068B RID: 1675
[Preserve]
public class TriggerHasTags : TargetedCompareRequirementBase
{
	// Token: 0x06003562 RID: 13666 RVA: 0x001611CC File Offset: 0x0015F3CC
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		bool flag;
		if (!this.hasAllTags)
		{
			flag = _params.Tags.Test_AnySet(this.currentItemTags);
		}
		else
		{
			flag = _params.Tags.Test_AllSet(this.currentItemTags);
		}
		if (!this.invert)
		{
			return flag;
		}
		return !flag;
	}

	// Token: 0x06003563 RID: 13667 RVA: 0x000027FC File Offset: 0x000009FC
	public override void GetInfoStrings(ref List<string> list)
	{
	}

	// Token: 0x06003564 RID: 13668 RVA: 0x00161224 File Offset: 0x0015F424
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

	// Token: 0x04002B50 RID: 11088
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> currentItemTags;

	// Token: 0x04002B51 RID: 11089
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasAllTags;
}

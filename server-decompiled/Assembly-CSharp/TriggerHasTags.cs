using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000684 RID: 1668
[Preserve]
public class TriggerHasTags : TargetedCompareRequirementBase
{
	// Token: 0x06003554 RID: 13652 RVA: 0x001605D8 File Offset: 0x0015E7D8
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

	// Token: 0x06003555 RID: 13653 RVA: 0x000027FC File Offset: 0x000009FC
	public override void GetInfoStrings(ref List<string> list)
	{
	}

	// Token: 0x06003556 RID: 13654 RVA: 0x00160630 File Offset: 0x0015E830
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

	// Token: 0x04002B35 RID: 11061
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> currentItemTags;

	// Token: 0x04002B36 RID: 11062
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasAllTags;
}

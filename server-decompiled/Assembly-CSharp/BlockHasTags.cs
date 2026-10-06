using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000685 RID: 1669
[Preserve]
public class BlockHasTags : TargetedCompareRequirementBase
{
	// Token: 0x06003558 RID: 13656 RVA: 0x0016069C File Offset: 0x0015E89C
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		bool flag = false;
		if (!_params.BlockValue.isair && _params.BlockValue.Block != null)
		{
			if (!this.hasAllTags)
			{
				flag = _params.BlockValue.Block.Tags.Test_AnySet(this.currentBlockTags);
			}
			else
			{
				flag = _params.BlockValue.Block.Tags.Test_AllSet(this.currentBlockTags);
			}
		}
		if (!this.invert)
		{
			return flag;
		}
		return !flag;
	}

	// Token: 0x06003559 RID: 13657 RVA: 0x000027FC File Offset: 0x000009FC
	public override void GetInfoStrings(ref List<string> list)
	{
	}

	// Token: 0x0600355A RID: 13658 RVA: 0x00160720 File Offset: 0x0015E920
	public override bool ParseXAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXAttribute(_attribute);
		if (!flag)
		{
			string localName = _attribute.Name.LocalName;
			if (localName == "tags")
			{
				this.currentBlockTags = FastTags<TagGroup.Global>.Parse(_attribute.Value);
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

	// Token: 0x04002B37 RID: 11063
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> currentBlockTags;

	// Token: 0x04002B38 RID: 11064
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasAllTags;
}

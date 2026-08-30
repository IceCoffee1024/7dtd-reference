using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000679 RID: 1657
[Preserve]
public class BlockHasTags : TargetedCompareRequirementBase
{
	// Token: 0x060034BF RID: 13503 RVA: 0x0015DB3C File Offset: 0x0015BD3C
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

	// Token: 0x060034C0 RID: 13504 RVA: 0x000027FC File Offset: 0x000009FC
	public override void GetInfoStrings(ref List<string> list)
	{
	}

	// Token: 0x060034C1 RID: 13505 RVA: 0x0015DBC0 File Offset: 0x0015BDC0
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

	// Token: 0x04002ACA RID: 10954
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> currentBlockTags;

	// Token: 0x04002ACB RID: 10955
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasAllTags;
}

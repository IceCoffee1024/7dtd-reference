using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x0200068C RID: 1676
[Preserve]
public class BlockHasTags : TargetedCompareRequirementBase
{
	// Token: 0x06003566 RID: 13670 RVA: 0x00161290 File Offset: 0x0015F490
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

	// Token: 0x06003567 RID: 13671 RVA: 0x000027FC File Offset: 0x000009FC
	public override void GetInfoStrings(ref List<string> list)
	{
	}

	// Token: 0x06003568 RID: 13672 RVA: 0x00161314 File Offset: 0x0015F514
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

	// Token: 0x04002B52 RID: 11090
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> currentBlockTags;

	// Token: 0x04002B53 RID: 11091
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasAllTags;
}

using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006B2 RID: 1714
[Preserve]
public class BlockStandingOn : TargetedCompareRequirementBase
{
	// Token: 0x060035DC RID: 13788 RVA: 0x00162D74 File Offset: 0x00160F74
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		bool flag;
		if (this.hasAllTags)
		{
			flag = this.target.blockValueStandingOn.Block.HasAllFastTags(this.blockTags);
		}
		else
		{
			flag = this.target.blockValueStandingOn.Block.HasAnyFastTags(this.blockTags);
		}
		if (!this.invert)
		{
			return flag;
		}
		return !flag;
	}

	// Token: 0x060035DD RID: 13789 RVA: 0x00160C3C File Offset: 0x0015EE3C
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Is {0}Male", this.invert ? "NOT " : ""));
	}

	// Token: 0x060035DE RID: 13790 RVA: 0x00162DE0 File Offset: 0x00160FE0
	public override bool ParseXAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXAttribute(_attribute);
		if (!flag)
		{
			string localName = _attribute.Name.LocalName;
			if (localName == "tags")
			{
				this.blockTags = FastTags<TagGroup.Global>.Parse(_attribute.Value);
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

	// Token: 0x04002B6E RID: 11118
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> blockTags;

	// Token: 0x04002B6F RID: 11119
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasAllTags;
}

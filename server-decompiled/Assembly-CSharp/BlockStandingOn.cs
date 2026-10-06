using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006AB RID: 1707
[Preserve]
public class BlockStandingOn : TargetedCompareRequirementBase
{
	// Token: 0x060035CE RID: 13774 RVA: 0x00162180 File Offset: 0x00160380
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

	// Token: 0x060035CF RID: 13775 RVA: 0x00160048 File Offset: 0x0015E248
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Is {0}Male", this.invert ? "NOT " : ""));
	}

	// Token: 0x060035D0 RID: 13776 RVA: 0x001621EC File Offset: 0x001603EC
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

	// Token: 0x04002B53 RID: 11091
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> blockTags;

	// Token: 0x04002B54 RID: 11092
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasAllTags;
}

using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x0200069F RID: 1695
[Preserve]
public class BlockStandingOn : TargetedCompareRequirementBase
{
	// Token: 0x06003535 RID: 13621 RVA: 0x0015F640 File Offset: 0x0015D840
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

	// Token: 0x06003536 RID: 13622 RVA: 0x0015D574 File Offset: 0x0015B774
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Is {0}Male", this.invert ? "NOT " : ""));
	}

	// Token: 0x06003537 RID: 13623 RVA: 0x0015F6AC File Offset: 0x0015D8AC
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

	// Token: 0x04002AE6 RID: 10982
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> blockTags;

	// Token: 0x04002AE7 RID: 10983
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasAllTags;
}

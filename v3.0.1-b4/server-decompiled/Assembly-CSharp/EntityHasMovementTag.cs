using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x0200067E RID: 1662
[Preserve]
public class EntityHasMovementTag : TargetedCompareRequirementBase
{
	// Token: 0x060034D0 RID: 13520 RVA: 0x0015DF78 File Offset: 0x0015C178
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		if (this.hasAllTags)
		{
			if (!this.invert)
			{
				return this.target.CurrentMovementTag.Test_AllSet(this.tagsToCompare);
			}
			return !this.target.CurrentMovementTag.Test_AllSet(this.tagsToCompare);
		}
		else
		{
			if (!this.invert)
			{
				return this.target.CurrentMovementTag.Test_AnySet(this.tagsToCompare);
			}
			return !this.target.CurrentMovementTag.Test_AnySet(this.tagsToCompare);
		}
	}

	// Token: 0x060034D1 RID: 13521 RVA: 0x0015E00C File Offset: 0x0015C20C
	public override bool ParseXAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXAttribute(_attribute);
		if (!flag)
		{
			string localName = _attribute.Name.LocalName;
			if (localName == "tags")
			{
				this.tagsToCompare = FastTags<TagGroup.Global>.Parse(_attribute.Value);
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

	// Token: 0x04002AD0 RID: 10960
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> tagsToCompare;

	// Token: 0x04002AD1 RID: 10961
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasAllTags;
}

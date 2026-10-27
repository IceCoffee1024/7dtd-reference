using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000691 RID: 1681
[Preserve]
public class EntityHasMovementTag : TargetedCompareRequirementBase
{
	// Token: 0x06003577 RID: 13687 RVA: 0x001616CC File Offset: 0x0015F8CC
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

	// Token: 0x06003578 RID: 13688 RVA: 0x00161760 File Offset: 0x0015F960
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

	// Token: 0x04002B58 RID: 11096
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> tagsToCompare;

	// Token: 0x04002B59 RID: 11097
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasAllTags;
}

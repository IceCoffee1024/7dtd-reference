using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000680 RID: 1664
[Preserve]
public class EntityTagCompare : TargetedCompareRequirementBase
{
	// Token: 0x060034D6 RID: 13526 RVA: 0x0015E178 File Offset: 0x0015C378
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
				return this.target.HasAllTags(this.tagsToCompare);
			}
			return !this.target.HasAllTags(this.tagsToCompare);
		}
		else
		{
			if (!this.invert)
			{
				return this.target.HasAnyTags(this.tagsToCompare);
			}
			return !this.target.HasAnyTags(this.tagsToCompare);
		}
	}

	// Token: 0x060034D7 RID: 13527 RVA: 0x0015E1F8 File Offset: 0x0015C3F8
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

	// Token: 0x04002AD4 RID: 10964
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> tagsToCompare;

	// Token: 0x04002AD5 RID: 10965
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasAllTags;
}

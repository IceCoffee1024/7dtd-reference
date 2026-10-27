using System;
using System.Xml.Linq;

// Token: 0x02000692 RID: 1682
public class EntityHasStanceTag : TargetedCompareRequirementBase
{
	// Token: 0x0600357A RID: 13690 RVA: 0x001617CC File Offset: 0x0015F9CC
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
				return this.target.CurrentStanceTag.Test_AllSet(this.tagsToCompare);
			}
			return !this.target.CurrentStanceTag.Test_AllSet(this.tagsToCompare);
		}
		else
		{
			if (!this.invert)
			{
				return this.target.CurrentStanceTag.Test_AnySet(this.tagsToCompare);
			}
			return !this.target.CurrentStanceTag.Test_AnySet(this.tagsToCompare);
		}
	}

	// Token: 0x0600357B RID: 13691 RVA: 0x00161860 File Offset: 0x0015FA60
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

	// Token: 0x04002B5A RID: 11098
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> tagsToCompare;

	// Token: 0x04002B5B RID: 11099
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasAllTags;
}

using System;
using System.Xml.Linq;

// Token: 0x0200068B RID: 1675
public class EntityHasStanceTag : TargetedCompareRequirementBase
{
	// Token: 0x0600356C RID: 13676 RVA: 0x00160BD8 File Offset: 0x0015EDD8
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

	// Token: 0x0600356D RID: 13677 RVA: 0x00160C6C File Offset: 0x0015EE6C
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

	// Token: 0x04002B3F RID: 11071
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> tagsToCompare;

	// Token: 0x04002B40 RID: 11072
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasAllTags;
}

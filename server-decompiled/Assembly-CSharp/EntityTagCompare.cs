using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000693 RID: 1683
[Preserve]
public class EntityTagCompare : TargetedCompareRequirementBase
{
	// Token: 0x0600357D RID: 13693 RVA: 0x001618CC File Offset: 0x0015FACC
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

	// Token: 0x0600357E RID: 13694 RVA: 0x0016194C File Offset: 0x0015FB4C
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

	// Token: 0x04002B5C RID: 11100
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> tagsToCompare;

	// Token: 0x04002B5D RID: 11101
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasAllTags;
}

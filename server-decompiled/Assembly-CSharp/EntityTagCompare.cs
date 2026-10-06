using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x0200068C RID: 1676
[Preserve]
public class EntityTagCompare : TargetedCompareRequirementBase
{
	// Token: 0x0600356F RID: 13679 RVA: 0x00160CD8 File Offset: 0x0015EED8
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

	// Token: 0x06003570 RID: 13680 RVA: 0x00160D58 File Offset: 0x0015EF58
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

	// Token: 0x04002B41 RID: 11073
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> tagsToCompare;

	// Token: 0x04002B42 RID: 11074
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasAllTags;
}

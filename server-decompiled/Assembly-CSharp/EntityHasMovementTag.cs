using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x0200068A RID: 1674
[Preserve]
public class EntityHasMovementTag : TargetedCompareRequirementBase
{
	// Token: 0x06003569 RID: 13673 RVA: 0x00160AD8 File Offset: 0x0015ECD8
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

	// Token: 0x0600356A RID: 13674 RVA: 0x00160B6C File Offset: 0x0015ED6C
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

	// Token: 0x04002B3D RID: 11069
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> tagsToCompare;

	// Token: 0x04002B3E RID: 11070
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasAllTags;
}

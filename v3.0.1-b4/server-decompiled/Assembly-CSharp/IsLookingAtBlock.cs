using System;
using System.Xml.Linq;

// Token: 0x0200068A RID: 1674
public class IsLookingAtBlock : RequirementBase
{
	// Token: 0x060034F6 RID: 13558 RVA: 0x0015E8BA File Offset: 0x0015CABA
	public override bool IsValid(MinEventParams _params)
	{
		return base.IsValid(_params);
	}

	// Token: 0x060034F7 RID: 13559 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void raycast()
	{
	}

	// Token: 0x060034F8 RID: 13560 RVA: 0x0015E8C8 File Offset: 0x0015CAC8
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

	// Token: 0x04002ADA RID: 10970
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> tagsToCompare;

	// Token: 0x04002ADB RID: 10971
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasAllTags;
}

using System;
using System.Xml.Linq;

// Token: 0x02000694 RID: 1684
public class IsLookingAtBlock : RequirementBase
{
	// Token: 0x0600354C RID: 13644 RVA: 0x00160A92 File Offset: 0x0015EC92
	public override bool IsValid(MinEventParams _params)
	{
		return base.IsValid(_params);
	}

	// Token: 0x0600354D RID: 13645 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void raycast()
	{
	}

	// Token: 0x0600354E RID: 13646 RVA: 0x00160AA0 File Offset: 0x0015ECA0
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

	// Token: 0x04002B4E RID: 11086
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> tagsToCompare;

	// Token: 0x04002B4F RID: 11087
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasAllTags;
}

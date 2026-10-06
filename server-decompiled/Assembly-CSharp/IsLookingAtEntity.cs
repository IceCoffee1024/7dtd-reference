using System;
using System.Xml.Linq;

// Token: 0x02000697 RID: 1687
public class IsLookingAtEntity : IsLookingAtBlock
{
	// Token: 0x06003593 RID: 13715 RVA: 0x0016148C File Offset: 0x0015F68C
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

	// Token: 0x04002B49 RID: 11081
	[PublicizedFrom(EAccessModifier.Private)]
	public new FastTags<TagGroup.Global> tagsToCompare;

	// Token: 0x04002B4A RID: 11082
	[PublicizedFrom(EAccessModifier.Private)]
	public new bool hasAllTags;
}

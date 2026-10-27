using System;
using System.Xml.Linq;

// Token: 0x0200069E RID: 1694
public class IsLookingAtEntity : IsLookingAtBlock
{
	// Token: 0x060035A1 RID: 13729 RVA: 0x00162080 File Offset: 0x00160280
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

	// Token: 0x04002B64 RID: 11108
	[PublicizedFrom(EAccessModifier.Private)]
	public new FastTags<TagGroup.Global> tagsToCompare;

	// Token: 0x04002B65 RID: 11109
	[PublicizedFrom(EAccessModifier.Private)]
	public new bool hasAllTags;
}

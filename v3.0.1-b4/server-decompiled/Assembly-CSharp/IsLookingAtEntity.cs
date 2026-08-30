using System;
using System.Xml.Linq;

// Token: 0x0200068B RID: 1675
public class IsLookingAtEntity : IsLookingAtBlock
{
	// Token: 0x060034FA RID: 13562 RVA: 0x0015E934 File Offset: 0x0015CB34
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

	// Token: 0x04002ADC RID: 10972
	[PublicizedFrom(EAccessModifier.Private)]
	public new FastTags<TagGroup.Global> tagsToCompare;

	// Token: 0x04002ADD RID: 10973
	[PublicizedFrom(EAccessModifier.Private)]
	public new bool hasAllTags;
}

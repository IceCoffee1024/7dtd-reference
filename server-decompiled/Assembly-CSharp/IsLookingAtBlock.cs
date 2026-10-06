using System;
using System.Xml.Linq;

// Token: 0x02000696 RID: 1686
public class IsLookingAtBlock : RequirementBase
{
	// Token: 0x0600358F RID: 13711 RVA: 0x00161412 File Offset: 0x0015F612
	public override bool IsValid(MinEventParams _params)
	{
		return base.IsValid(_params);
	}

	// Token: 0x06003590 RID: 13712 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void raycast()
	{
	}

	// Token: 0x06003591 RID: 13713 RVA: 0x00161420 File Offset: 0x0015F620
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

	// Token: 0x04002B47 RID: 11079
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> tagsToCompare;

	// Token: 0x04002B48 RID: 11080
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasAllTags;
}

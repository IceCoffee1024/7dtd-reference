using System;
using System.Xml.Linq;

// Token: 0x0200069D RID: 1693
public class IsLookingAtBlock : RequirementBase
{
	// Token: 0x0600359D RID: 13725 RVA: 0x00162006 File Offset: 0x00160206
	public override bool IsValid(MinEventParams _params)
	{
		return base.IsValid(_params);
	}

	// Token: 0x0600359E RID: 13726 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void raycast()
	{
	}

	// Token: 0x0600359F RID: 13727 RVA: 0x00162014 File Offset: 0x00160214
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

	// Token: 0x04002B62 RID: 11106
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> tagsToCompare;

	// Token: 0x04002B63 RID: 11107
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasAllTags;
}

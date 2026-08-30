using System;
using System.Xml.Linq;

// Token: 0x02000684 RID: 1668
public class ProjectileHasTags : TargetedCompareRequirementBase
{
	// Token: 0x06003519 RID: 13593 RVA: 0x0015FE08 File Offset: 0x0015E008
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		if (_params.ItemValue.IsEmpty() || _params.ItemValue.ItemClass == null)
		{
			return false;
		}
		bool flag;
		if (!this.hasAllTags)
		{
			flag = _params.ItemValue.ItemClass.HasAnyTags(this.itemTags);
		}
		else
		{
			flag = _params.ItemValue.ItemClass.HasAllTags(this.itemTags);
		}
		if (!this.invert)
		{
			return flag;
		}
		return !flag;
	}

	// Token: 0x0600351A RID: 13594 RVA: 0x0015FE84 File Offset: 0x0015E084
	public override bool ParseXAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXAttribute(_attribute);
		if (!flag)
		{
			string localName = _attribute.Name.LocalName;
			if (localName == "tags")
			{
				this.itemTags = FastTags<TagGroup.Global>.Parse(_attribute.Value);
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

	// Token: 0x04002B40 RID: 11072
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> itemTags;

	// Token: 0x04002B41 RID: 11073
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasAllTags;
}

using System;
using System.Xml.Linq;

// Token: 0x02000686 RID: 1670
public class ProjectileHasTags : TargetedCompareRequirementBase
{
	// Token: 0x0600355C RID: 13660 RVA: 0x0016078C File Offset: 0x0015E98C
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

	// Token: 0x0600355D RID: 13661 RVA: 0x00160808 File Offset: 0x0015EA08
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

	// Token: 0x04002B39 RID: 11065
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> itemTags;

	// Token: 0x04002B3A RID: 11066
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasAllTags;
}

using System;
using System.Xml.Linq;

// Token: 0x0200068D RID: 1677
public class ProjectileHasTags : TargetedCompareRequirementBase
{
	// Token: 0x0600356A RID: 13674 RVA: 0x00161380 File Offset: 0x0015F580
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

	// Token: 0x0600356B RID: 13675 RVA: 0x001613FC File Offset: 0x0015F5FC
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

	// Token: 0x04002B54 RID: 11092
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> itemTags;

	// Token: 0x04002B55 RID: 11093
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasAllTags;
}

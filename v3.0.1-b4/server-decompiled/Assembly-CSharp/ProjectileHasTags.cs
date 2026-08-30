using System;
using System.Xml.Linq;

// Token: 0x0200067A RID: 1658
public class ProjectileHasTags : TargetedCompareRequirementBase
{
	// Token: 0x060034C3 RID: 13507 RVA: 0x0015DC2C File Offset: 0x0015BE2C
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

	// Token: 0x060034C4 RID: 13508 RVA: 0x0015DCA8 File Offset: 0x0015BEA8
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

	// Token: 0x04002ACC RID: 10956
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> itemTags;

	// Token: 0x04002ACD RID: 10957
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasAllTags;
}

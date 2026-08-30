using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000682 RID: 1666
[Preserve]
public class TriggerHasTags : TargetedCompareRequirementBase
{
	// Token: 0x06003511 RID: 13585 RVA: 0x0015FC54 File Offset: 0x0015DE54
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		bool flag;
		if (!this.hasAllTags)
		{
			flag = _params.Tags.Test_AnySet(this.currentItemTags);
		}
		else
		{
			flag = _params.Tags.Test_AllSet(this.currentItemTags);
		}
		if (!this.invert)
		{
			return flag;
		}
		return !flag;
	}

	// Token: 0x06003512 RID: 13586 RVA: 0x000027FC File Offset: 0x000009FC
	public override void GetInfoStrings(ref List<string> list)
	{
	}

	// Token: 0x06003513 RID: 13587 RVA: 0x0015FCAC File Offset: 0x0015DEAC
	public override bool ParseXAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXAttribute(_attribute);
		if (!flag)
		{
			string localName = _attribute.Name.LocalName;
			if (localName == "tags")
			{
				this.currentItemTags = FastTags<TagGroup.Global>.Parse(_attribute.Value);
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

	// Token: 0x04002B3C RID: 11068
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> currentItemTags;

	// Token: 0x04002B3D RID: 11069
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasAllTags;
}

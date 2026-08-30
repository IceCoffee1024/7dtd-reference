using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000699 RID: 1689
[Preserve]
public class RecipeUnlocked : TargetedCompareRequirementBase
{
	// Token: 0x0600355D RID: 13661 RVA: 0x00160EA4 File Offset: 0x0015F0A4
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		if (this.item_name == null)
		{
			return false;
		}
		List<Recipe> nonScrapableRecipes = CraftingManager.GetNonScrapableRecipes(this.item_name);
		bool flag = false;
		for (int i = 0; i < nonScrapableRecipes.Count; i++)
		{
			if (nonScrapableRecipes[i].IsUnlocked(this.target as EntityPlayer))
			{
				flag = true;
				break;
			}
		}
		if (!this.invert)
		{
			return flag;
		}
		return !flag;
	}

	// Token: 0x0600355E RID: 13662 RVA: 0x00160F10 File Offset: 0x0015F110
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("is recipe {0} {1} unlocked", this.item_name, this.invert ? "NOT " : ""));
	}

	// Token: 0x0600355F RID: 13663 RVA: 0x00160F40 File Offset: 0x0015F140
	public override bool ParseXAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "item_name")
		{
			this.item_name = _attribute.Value;
			return true;
		}
		return flag;
	}

	// Token: 0x04002B56 RID: 11094
	[PublicizedFrom(EAccessModifier.Private)]
	public string item_name;
}

using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006A2 RID: 1698
[Preserve]
public class RecipeUnlocked : TargetedCompareRequirementBase
{
	// Token: 0x060035AE RID: 13742 RVA: 0x00162418 File Offset: 0x00160618
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

	// Token: 0x060035AF RID: 13743 RVA: 0x00162484 File Offset: 0x00160684
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("is recipe {0} {1} unlocked", this.item_name, this.invert ? "NOT " : ""));
	}

	// Token: 0x060035B0 RID: 13744 RVA: 0x001624B4 File Offset: 0x001606B4
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

	// Token: 0x04002B6A RID: 11114
	[PublicizedFrom(EAccessModifier.Private)]
	public string item_name;
}

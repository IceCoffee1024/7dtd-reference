using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x0200068F RID: 1679
[Preserve]
public class RecipeUnlocked : TargetedCompareRequirementBase
{
	// Token: 0x06003507 RID: 13575 RVA: 0x0015ECCC File Offset: 0x0015CECC
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

	// Token: 0x06003508 RID: 13576 RVA: 0x0015ED38 File Offset: 0x0015CF38
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("is recipe {0} {1} unlocked", this.item_name, this.invert ? "NOT " : ""));
	}

	// Token: 0x06003509 RID: 13577 RVA: 0x0015ED68 File Offset: 0x0015CF68
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

	// Token: 0x04002AE2 RID: 10978
	[PublicizedFrom(EAccessModifier.Private)]
	public string item_name;
}

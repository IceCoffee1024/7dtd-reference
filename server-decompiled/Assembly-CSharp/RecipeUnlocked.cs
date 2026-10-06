using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x0200069B RID: 1691
[Preserve]
public class RecipeUnlocked : TargetedCompareRequirementBase
{
	// Token: 0x060035A0 RID: 13728 RVA: 0x00161824 File Offset: 0x0015FA24
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

	// Token: 0x060035A1 RID: 13729 RVA: 0x00161890 File Offset: 0x0015FA90
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("is recipe {0} {1} unlocked", this.item_name, this.invert ? "NOT " : ""));
	}

	// Token: 0x060035A2 RID: 13730 RVA: 0x001618C0 File Offset: 0x0015FAC0
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

	// Token: 0x04002B4F RID: 11087
	[PublicizedFrom(EAccessModifier.Private)]
	public string item_name;
}

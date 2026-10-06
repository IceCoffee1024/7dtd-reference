using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020006B6 RID: 1718
[Preserve]
public class HasAttachedPrefab : TargetedCompareRequirementBase
{
	// Token: 0x060035F6 RID: 13814 RVA: 0x00162A34 File Offset: 0x00160C34
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		Transform transform = null;
		if (this.parent_transform_path != null)
		{
			transform = GameUtils.FindDeepChildActive(_params.Self.RootTransform, this.parent_transform_path);
		}
		Transform transform2;
		if (transform == null)
		{
			transform2 = GameUtils.FindDeepChildActive(_params.Self.RootTransform, "tempPrefab_" + this.prefabName);
		}
		else
		{
			transform2 = GameUtils.FindDeepChildActive(transform, "tempPrefab_" + this.prefabName);
		}
		if (transform2 != null)
		{
			return !this.invert;
		}
		return this.invert;
	}

	// Token: 0x060035F7 RID: 13815 RVA: 0x00162ACB File Offset: 0x00160CCB
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Does {0}Have Attached Prefab", this.invert ? "NOT " : ""));
	}

	// Token: 0x060035F8 RID: 13816 RVA: 0x00162AF4 File Offset: 0x00160CF4
	public override bool ParseXAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXAttribute(_attribute);
		if (!flag)
		{
			string localName = _attribute.Name.LocalName;
			if (localName == "prefab" || localName == "prefab_name")
			{
				this.prefabName = _attribute.Value;
				if (this.prefabName.Contains("/"))
				{
					this.prefabName = this.prefabName.Substring(this.prefabName.LastIndexOf("/") + 1);
				}
				return true;
			}
			if (localName == "parent_transform")
			{
				this.parent_transform_path = _attribute.Value;
				return true;
			}
		}
		return flag;
	}

	// Token: 0x04002B5E RID: 11102
	[PublicizedFrom(EAccessModifier.Private)]
	public string prefabName;

	// Token: 0x04002B5F RID: 11103
	[PublicizedFrom(EAccessModifier.Private)]
	public string parent_transform_path;
}

using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020006AA RID: 1706
[Preserve]
public class HasAttachedPrefab : TargetedCompareRequirementBase
{
	// Token: 0x0600355D RID: 13661 RVA: 0x0015FEEC File Offset: 0x0015E0EC
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

	// Token: 0x0600355E RID: 13662 RVA: 0x0015FF83 File Offset: 0x0015E183
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Does {0}Have Attached Prefab", this.invert ? "NOT " : ""));
	}

	// Token: 0x0600355F RID: 13663 RVA: 0x0015FFAC File Offset: 0x0015E1AC
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

	// Token: 0x04002AF1 RID: 10993
	[PublicizedFrom(EAccessModifier.Private)]
	public string prefabName;

	// Token: 0x04002AF2 RID: 10994
	[PublicizedFrom(EAccessModifier.Private)]
	public string parent_transform_path;
}

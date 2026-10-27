using System;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020006EE RID: 1774
[Preserve]
public class MinEventActionRemovePrefabFromEntity : MinEventActionTargetedBase
{
	// Token: 0x060036B2 RID: 14002 RVA: 0x00168DE4 File Offset: 0x00166FE4
	public override void Execute(MinEventParams _params)
	{
		if (_params.Self == null)
		{
			return;
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
		if (transform2 == null)
		{
			return;
		}
		global::UnityEngine.Object.Destroy(transform2.gameObject);
	}

	// Token: 0x060036B3 RID: 14003 RVA: 0x00168E7B File Offset: 0x0016707B
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && _params.Self != null && this.prefabName != null;
	}

	// Token: 0x060036B4 RID: 14004 RVA: 0x00168EA0 File Offset: 0x001670A0
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
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

	// Token: 0x04002C01 RID: 11265
	[PublicizedFrom(EAccessModifier.Private)]
	public string prefabName;

	// Token: 0x04002C02 RID: 11266
	[PublicizedFrom(EAccessModifier.Private)]
	public string parent_transform_path;
}

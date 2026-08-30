using System;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020006DA RID: 1754
[Preserve]
public class MinEventActionRemovePrefabFromEntity : MinEventActionTargetedBase
{
	// Token: 0x06003602 RID: 13826 RVA: 0x001654C0 File Offset: 0x001636C0
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

	// Token: 0x06003603 RID: 13827 RVA: 0x00165557 File Offset: 0x00163757
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && _params.Self != null && this.prefabName != null;
	}

	// Token: 0x06003604 RID: 13828 RVA: 0x0016557C File Offset: 0x0016377C
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

	// Token: 0x04002B78 RID: 11128
	[PublicizedFrom(EAccessModifier.Private)]
	public string prefabName;

	// Token: 0x04002B79 RID: 11129
	[PublicizedFrom(EAccessModifier.Private)]
	public string parent_transform_path;
}

using System;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020006E4 RID: 1764
[Preserve]
public class MinEventActionRemovePrefabFromEntity : MinEventActionTargetedBase
{
	// Token: 0x06003658 RID: 13912 RVA: 0x00167698 File Offset: 0x00165898
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

	// Token: 0x06003659 RID: 13913 RVA: 0x0016772F File Offset: 0x0016592F
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && _params.Self != null && this.prefabName != null;
	}

	// Token: 0x0600365A RID: 13914 RVA: 0x00167754 File Offset: 0x00165954
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

	// Token: 0x04002BEC RID: 11244
	[PublicizedFrom(EAccessModifier.Private)]
	public string prefabName;

	// Token: 0x04002BED RID: 11245
	[PublicizedFrom(EAccessModifier.Private)]
	public string parent_transform_path;
}

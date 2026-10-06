using System;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020006E6 RID: 1766
[Preserve]
public class MinEventActionRemovePrefabFromEntity : MinEventActionTargetedBase
{
	// Token: 0x0600369B RID: 13979 RVA: 0x00167FFC File Offset: 0x001661FC
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

	// Token: 0x0600369C RID: 13980 RVA: 0x00168093 File Offset: 0x00166293
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && _params.Self != null && this.prefabName != null;
	}

	// Token: 0x0600369D RID: 13981 RVA: 0x001680B8 File Offset: 0x001662B8
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

	// Token: 0x04002BE5 RID: 11237
	[PublicizedFrom(EAccessModifier.Private)]
	public string prefabName;

	// Token: 0x04002BE6 RID: 11238
	[PublicizedFrom(EAccessModifier.Private)]
	public string parent_transform_path;
}

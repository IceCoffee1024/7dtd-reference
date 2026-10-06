using System;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000707 RID: 1799
[Preserve]
public class MinEventActionSetTransformChildrenActive : MinEventActionBase
{
	// Token: 0x06003719 RID: 14105 RVA: 0x0016A89C File Offset: 0x00168A9C
	public override void Execute(MinEventParams _params)
	{
		Transform transform;
		if (this.parent_transform.EqualsCaseInsensitive("#HeldItemRoot") && _params.Transform != null)
		{
			transform = _params.Transform;
		}
		else if (this.parent_transform != "")
		{
			transform = GameUtils.FindDeepChildActive(_params.Self.RootTransform, this.parent_transform);
		}
		else
		{
			transform = _params.Self.RootTransform;
		}
		if (transform == null)
		{
			return;
		}
		Transform transform2 = GameUtils.FindDeepChildActive(transform, this.transformPath);
		if (transform2 == null)
		{
			return;
		}
		for (int i = 0; i < transform2.childCount; i++)
		{
			transform2.GetChild(i).gameObject.SetActive(this.isActive);
		}
	}

	// Token: 0x0600371A RID: 14106 RVA: 0x0016A954 File Offset: 0x00168B54
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && _params.Self != null && _params.ItemValue != null && this.transformPath != null && this.transformPath != "";
	}

	// Token: 0x0600371B RID: 14107 RVA: 0x0016A990 File Offset: 0x00168B90
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag)
		{
			string localName = _attribute.Name.LocalName;
			if (localName == "active")
			{
				this.isActive = StringParsers.ParseBool(_attribute.Value, 0, -1, true);
				return true;
			}
			if (localName == "parent_transform")
			{
				this.parent_transform = _attribute.Value;
				return true;
			}
			if (localName == "transform_path")
			{
				this.transformPath = _attribute.Value;
				return true;
			}
		}
		return flag;
	}

	// Token: 0x04002C3D RID: 11325
	[PublicizedFrom(EAccessModifier.Private)]
	public string transformPath;

	// Token: 0x04002C3E RID: 11326
	[PublicizedFrom(EAccessModifier.Private)]
	public string parent_transform = "";

	// Token: 0x04002C3F RID: 11327
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isActive;
}

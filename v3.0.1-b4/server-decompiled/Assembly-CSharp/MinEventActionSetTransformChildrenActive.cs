using System;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020006FB RID: 1787
[Preserve]
public class MinEventActionSetTransformChildrenActive : MinEventActionBase
{
	// Token: 0x06003680 RID: 13952 RVA: 0x00167D60 File Offset: 0x00165F60
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

	// Token: 0x06003681 RID: 13953 RVA: 0x00167E18 File Offset: 0x00166018
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && _params.Self != null && _params.ItemValue != null && this.transformPath != null && this.transformPath != "";
	}

	// Token: 0x06003682 RID: 13954 RVA: 0x00167E54 File Offset: 0x00166054
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

	// Token: 0x04002BD0 RID: 11216
	[PublicizedFrom(EAccessModifier.Private)]
	public string transformPath;

	// Token: 0x04002BD1 RID: 11217
	[PublicizedFrom(EAccessModifier.Private)]
	public string parent_transform = "";

	// Token: 0x04002BD2 RID: 11218
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isActive;
}

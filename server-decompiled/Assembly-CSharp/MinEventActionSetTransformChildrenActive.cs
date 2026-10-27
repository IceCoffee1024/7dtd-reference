using System;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200070F RID: 1807
[Preserve]
public class MinEventActionSetTransformChildrenActive : MinEventActionBase
{
	// Token: 0x06003730 RID: 14128 RVA: 0x0016B834 File Offset: 0x00169A34
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

	// Token: 0x06003731 RID: 14129 RVA: 0x0016B8EC File Offset: 0x00169AEC
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && _params.Self != null && _params.ItemValue != null && this.transformPath != null && this.transformPath != "";
	}

	// Token: 0x06003732 RID: 14130 RVA: 0x0016B928 File Offset: 0x00169B28
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

	// Token: 0x04002C59 RID: 11353
	[PublicizedFrom(EAccessModifier.Private)]
	public string transformPath;

	// Token: 0x04002C5A RID: 11354
	[PublicizedFrom(EAccessModifier.Private)]
	public string parent_transform = "";

	// Token: 0x04002C5B RID: 11355
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isActive;
}

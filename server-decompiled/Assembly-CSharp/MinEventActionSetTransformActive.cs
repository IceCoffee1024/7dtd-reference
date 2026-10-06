using System;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000706 RID: 1798
[Preserve]
public class MinEventActionSetTransformActive : MinEventActionBase
{
	// Token: 0x06003715 RID: 14101 RVA: 0x0016A718 File Offset: 0x00168918
	public override void Execute(MinEventParams _params)
	{
		Transform transform;
		if (this.parent_transform.EqualsCaseInsensitive("#HeldItemRoot"))
		{
			transform = _params.Self.inventory.GetHoldingItemTransform();
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
		Transform transform2 = GameUtils.FindDeepChild(transform, this.transformPath);
		if (transform2 == null)
		{
			return;
		}
		transform2.gameObject.SetActive(this.isActive);
		LightManager.LightChanged(transform2.position + Origin.position);
	}

	// Token: 0x06003716 RID: 14102 RVA: 0x0016A7CA File Offset: 0x001689CA
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && _params.Self != null && _params.ItemValue != null && this.transformPath != null && this.transformPath != "";
	}

	// Token: 0x06003717 RID: 14103 RVA: 0x0016A808 File Offset: 0x00168A08
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

	// Token: 0x04002C3A RID: 11322
	[PublicizedFrom(EAccessModifier.Private)]
	public string transformPath;

	// Token: 0x04002C3B RID: 11323
	[PublicizedFrom(EAccessModifier.Private)]
	public string parent_transform = "";

	// Token: 0x04002C3C RID: 11324
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isActive;
}

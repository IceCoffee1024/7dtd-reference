using System;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200070E RID: 1806
[Preserve]
public class MinEventActionSetTransformActive : MinEventActionBase
{
	// Token: 0x0600372C RID: 14124 RVA: 0x0016B6B0 File Offset: 0x001698B0
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

	// Token: 0x0600372D RID: 14125 RVA: 0x0016B762 File Offset: 0x00169962
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && _params.Self != null && _params.ItemValue != null && this.transformPath != null && this.transformPath != "";
	}

	// Token: 0x0600372E RID: 14126 RVA: 0x0016B7A0 File Offset: 0x001699A0
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

	// Token: 0x04002C56 RID: 11350
	[PublicizedFrom(EAccessModifier.Private)]
	public string transformPath;

	// Token: 0x04002C57 RID: 11351
	[PublicizedFrom(EAccessModifier.Private)]
	public string parent_transform = "";

	// Token: 0x04002C58 RID: 11352
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isActive;
}

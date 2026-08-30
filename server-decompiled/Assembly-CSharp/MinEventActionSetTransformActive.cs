using System;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000704 RID: 1796
[Preserve]
public class MinEventActionSetTransformActive : MinEventActionBase
{
	// Token: 0x060036D2 RID: 14034 RVA: 0x00169DB4 File Offset: 0x00167FB4
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

	// Token: 0x060036D3 RID: 14035 RVA: 0x00169E66 File Offset: 0x00168066
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && _params.Self != null && _params.ItemValue != null && this.transformPath != null && this.transformPath != "";
	}

	// Token: 0x060036D4 RID: 14036 RVA: 0x00169EA4 File Offset: 0x001680A4
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

	// Token: 0x04002C41 RID: 11329
	[PublicizedFrom(EAccessModifier.Private)]
	public string transformPath;

	// Token: 0x04002C42 RID: 11330
	[PublicizedFrom(EAccessModifier.Private)]
	public string parent_transform = "";

	// Token: 0x04002C43 RID: 11331
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isActive;
}

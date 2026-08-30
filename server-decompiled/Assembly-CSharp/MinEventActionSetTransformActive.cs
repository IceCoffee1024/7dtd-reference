using System;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020006FA RID: 1786
[Preserve]
public class MinEventActionSetTransformActive : MinEventActionBase
{
	// Token: 0x0600367C RID: 13948 RVA: 0x00167BDC File Offset: 0x00165DDC
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

	// Token: 0x0600367D RID: 13949 RVA: 0x00167C8E File Offset: 0x00165E8E
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && _params.Self != null && _params.ItemValue != null && this.transformPath != null && this.transformPath != "";
	}

	// Token: 0x0600367E RID: 13950 RVA: 0x00167CCC File Offset: 0x00165ECC
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

	// Token: 0x04002BCD RID: 11213
	[PublicizedFrom(EAccessModifier.Private)]
	public string transformPath;

	// Token: 0x04002BCE RID: 11214
	[PublicizedFrom(EAccessModifier.Private)]
	public string parent_transform = "";

	// Token: 0x04002BCF RID: 11215
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isActive;
}

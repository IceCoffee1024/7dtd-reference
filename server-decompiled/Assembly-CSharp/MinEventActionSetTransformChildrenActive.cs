using System;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000705 RID: 1797
[Preserve]
public class MinEventActionSetTransformChildrenActive : MinEventActionBase
{
	// Token: 0x060036D6 RID: 14038 RVA: 0x00169F38 File Offset: 0x00168138
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

	// Token: 0x060036D7 RID: 14039 RVA: 0x00169FF0 File Offset: 0x001681F0
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && _params.Self != null && _params.ItemValue != null && this.transformPath != null && this.transformPath != "";
	}

	// Token: 0x060036D8 RID: 14040 RVA: 0x0016A02C File Offset: 0x0016822C
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

	// Token: 0x04002C44 RID: 11332
	[PublicizedFrom(EAccessModifier.Private)]
	public string transformPath;

	// Token: 0x04002C45 RID: 11333
	[PublicizedFrom(EAccessModifier.Private)]
	public string parent_transform = "";

	// Token: 0x04002C46 RID: 11334
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isActive;
}

using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006E2 RID: 1762
[Preserve]
public class MinEventActionRemovePart : MinEventActionTargetedBase
{
	// Token: 0x06003650 RID: 13904 RVA: 0x0016739E File Offset: 0x0016559E
	public override void Execute(MinEventParams _params)
	{
		if (_params.Self == null)
		{
			return;
		}
		_params.Self.RemovePart(this.partName);
	}

	// Token: 0x06003651 RID: 13905 RVA: 0x001673C0 File Offset: 0x001655C0
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && _params.Self != null && this.partName != null;
	}

	// Token: 0x06003652 RID: 13906 RVA: 0x001673E8 File Offset: 0x001655E8
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "part")
		{
			this.partName = _attribute.Value;
			return true;
		}
		return flag;
	}

	// Token: 0x04002BE5 RID: 11237
	[PublicizedFrom(EAccessModifier.Private)]
	public string partName;
}

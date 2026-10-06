using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006E4 RID: 1764
[Preserve]
public class MinEventActionRemovePart : MinEventActionTargetedBase
{
	// Token: 0x06003693 RID: 13971 RVA: 0x00167D02 File Offset: 0x00165F02
	public override void Execute(MinEventParams _params)
	{
		if (_params.Self == null)
		{
			return;
		}
		_params.Self.RemovePart(this.partName);
	}

	// Token: 0x06003694 RID: 13972 RVA: 0x00167D24 File Offset: 0x00165F24
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && _params.Self != null && this.partName != null;
	}

	// Token: 0x06003695 RID: 13973 RVA: 0x00167D4C File Offset: 0x00165F4C
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

	// Token: 0x04002BDE RID: 11230
	[PublicizedFrom(EAccessModifier.Private)]
	public string partName;
}

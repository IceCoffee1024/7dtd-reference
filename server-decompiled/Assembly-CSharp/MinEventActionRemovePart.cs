using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006EC RID: 1772
[Preserve]
public class MinEventActionRemovePart : MinEventActionTargetedBase
{
	// Token: 0x060036AA RID: 13994 RVA: 0x00168AEA File Offset: 0x00166CEA
	public override void Execute(MinEventParams _params)
	{
		if (_params.Self == null)
		{
			return;
		}
		_params.Self.RemovePart(this.partName);
	}

	// Token: 0x060036AB RID: 13995 RVA: 0x00168B0C File Offset: 0x00166D0C
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && _params.Self != null && this.partName != null;
	}

	// Token: 0x060036AC RID: 13996 RVA: 0x00168B34 File Offset: 0x00166D34
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

	// Token: 0x04002BFA RID: 11258
	[PublicizedFrom(EAccessModifier.Private)]
	public string partName;
}

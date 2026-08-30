using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006D0 RID: 1744
[Preserve]
public class MinEventActionLogMessage : MinEventActionBase
{
	// Token: 0x060035E2 RID: 13794 RVA: 0x0016426E File Offset: 0x0016246E
	public override void Execute(MinEventParams _params)
	{
		Log.Out("MinEventLogMessage: {0}", new object[] { this.message });
	}

	// Token: 0x060035E3 RID: 13795 RVA: 0x0016428C File Offset: 0x0016248C
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "message")
		{
			this.message = _attribute.Value;
			return true;
		}
		return flag;
	}

	// Token: 0x04002B58 RID: 11096
	[PublicizedFrom(EAccessModifier.Private)]
	public string message;
}

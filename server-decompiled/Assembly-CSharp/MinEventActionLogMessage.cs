using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006DA RID: 1754
[Preserve]
public class MinEventActionLogMessage : MinEventActionBase
{
	// Token: 0x06003638 RID: 13880 RVA: 0x00166446 File Offset: 0x00164646
	public override void Execute(MinEventParams _params)
	{
		Log.Out("MinEventLogMessage: {0}", new object[] { this.message });
	}

	// Token: 0x06003639 RID: 13881 RVA: 0x00166464 File Offset: 0x00164664
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

	// Token: 0x04002BCC RID: 11212
	[PublicizedFrom(EAccessModifier.Private)]
	public string message;
}

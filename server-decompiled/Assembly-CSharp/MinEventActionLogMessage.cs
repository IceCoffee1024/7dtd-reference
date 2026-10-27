using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006E4 RID: 1764
[Preserve]
public class MinEventActionLogMessage : MinEventActionBase
{
	// Token: 0x06003692 RID: 13970 RVA: 0x00167B9E File Offset: 0x00165D9E
	public override void Execute(MinEventParams _params)
	{
		Log.Out("MinEventLogMessage: {0}", new object[] { this.message });
	}

	// Token: 0x06003693 RID: 13971 RVA: 0x00167BBC File Offset: 0x00165DBC
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

	// Token: 0x04002BE1 RID: 11233
	[PublicizedFrom(EAccessModifier.Private)]
	public string message;
}

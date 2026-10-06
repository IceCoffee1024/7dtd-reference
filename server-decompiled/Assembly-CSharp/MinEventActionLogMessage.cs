using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006DC RID: 1756
[Preserve]
public class MinEventActionLogMessage : MinEventActionBase
{
	// Token: 0x0600367B RID: 13947 RVA: 0x00166DB6 File Offset: 0x00164FB6
	public override void Execute(MinEventParams _params)
	{
		Log.Out("MinEventLogMessage: {0}", new object[] { this.message });
	}

	// Token: 0x0600367C RID: 13948 RVA: 0x00166DD4 File Offset: 0x00164FD4
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

	// Token: 0x04002BC5 RID: 11205
	[PublicizedFrom(EAccessModifier.Private)]
	public string message;
}

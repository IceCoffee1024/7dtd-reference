using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000712 RID: 1810
[Preserve]
public class MinEventActionRefreshPerks : MinEventActionBase
{
	// Token: 0x0600373F RID: 14143 RVA: 0x0016B9A1 File Offset: 0x00169BA1
	public override void Execute(MinEventParams _params)
	{
		if (_params.Self.Progression == null)
		{
			return;
		}
		_params.Self.Progression.RefreshPerks(this.attribute);
	}

	// Token: 0x06003740 RID: 14144 RVA: 0x0016B9C8 File Offset: 0x00169BC8
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "attribute")
		{
			this.attribute = _attribute.Value;
			return true;
		}
		return flag;
	}

	// Token: 0x04002C76 RID: 11382
	[PublicizedFrom(EAccessModifier.Private)]
	public string attribute = "";
}

using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x0200071A RID: 1818
[Preserve]
public class MinEventActionRefreshPerks : MinEventActionBase
{
	// Token: 0x06003756 RID: 14166 RVA: 0x0016C939 File Offset: 0x0016AB39
	public override void Execute(MinEventParams _params)
	{
		if (_params.Self.Progression == null)
		{
			return;
		}
		_params.Self.Progression.RefreshPerks(this.attribute);
	}

	// Token: 0x06003757 RID: 14167 RVA: 0x0016C960 File Offset: 0x0016AB60
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

	// Token: 0x04002C92 RID: 11410
	[PublicizedFrom(EAccessModifier.Private)]
	public string attribute = "";
}

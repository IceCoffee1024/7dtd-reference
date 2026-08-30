using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000706 RID: 1798
[Preserve]
public class MinEventActionRefreshPerks : MinEventActionBase
{
	// Token: 0x060036A6 RID: 13990 RVA: 0x00168E65 File Offset: 0x00167065
	public override void Execute(MinEventParams _params)
	{
		if (_params.Self.Progression == null)
		{
			return;
		}
		_params.Self.Progression.RefreshPerks(this.attribute);
	}

	// Token: 0x060036A7 RID: 13991 RVA: 0x00168E8C File Offset: 0x0016708C
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

	// Token: 0x04002C09 RID: 11273
	[PublicizedFrom(EAccessModifier.Private)]
	public string attribute = "";
}

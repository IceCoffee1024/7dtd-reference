using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000710 RID: 1808
[Preserve]
public class MinEventActionRefreshPerks : MinEventActionBase
{
	// Token: 0x060036FC RID: 14076 RVA: 0x0016B03D File Offset: 0x0016923D
	public override void Execute(MinEventParams _params)
	{
		if (_params.Self.Progression == null)
		{
			return;
		}
		_params.Self.Progression.RefreshPerks(this.attribute);
	}

	// Token: 0x060036FD RID: 14077 RVA: 0x0016B064 File Offset: 0x00169264
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

	// Token: 0x04002C7D RID: 11389
	[PublicizedFrom(EAccessModifier.Private)]
	public string attribute = "";
}

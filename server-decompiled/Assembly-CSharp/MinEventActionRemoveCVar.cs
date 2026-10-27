using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006E3 RID: 1763
[Preserve]
public class MinEventActionRemoveCVar : MinEventActionTargetedBase
{
	// Token: 0x0600368F RID: 13967 RVA: 0x00167AD4 File Offset: 0x00165CD4
	public override void Execute(MinEventParams _params)
	{
		for (int i = 0; i < this.targets.Count; i++)
		{
			for (int j = 0; j < this.cvarNames.Length; j++)
			{
				this.targets[i].Buffs.SetCustomVar(this.cvarNames[j], 0f, (this.targets[i].isEntityRemote && !_params.Self.isEntityRemote) || _params.IsLocal, CVarOperation.set, false);
			}
		}
	}

	// Token: 0x06003690 RID: 13968 RVA: 0x00167B58 File Offset: 0x00165D58
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "cvar")
		{
			this.cvarNames = _attribute.Value.Split(',', StringSplitOptions.None);
			return true;
		}
		return flag;
	}

	// Token: 0x04002BE0 RID: 11232
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] cvarNames;
}

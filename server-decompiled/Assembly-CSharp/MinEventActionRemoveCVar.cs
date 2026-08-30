using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006D9 RID: 1753
[Preserve]
public class MinEventActionRemoveCVar : MinEventActionTargetedBase
{
	// Token: 0x06003635 RID: 13877 RVA: 0x0016637C File Offset: 0x0016457C
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

	// Token: 0x06003636 RID: 13878 RVA: 0x00166400 File Offset: 0x00164600
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

	// Token: 0x04002BCB RID: 11211
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] cvarNames;
}

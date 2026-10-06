using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006DB RID: 1755
[Preserve]
public class MinEventActionRemoveCVar : MinEventActionTargetedBase
{
	// Token: 0x06003678 RID: 13944 RVA: 0x00166CEC File Offset: 0x00164EEC
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

	// Token: 0x06003679 RID: 13945 RVA: 0x00166D70 File Offset: 0x00164F70
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

	// Token: 0x04002BC4 RID: 11204
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] cvarNames;
}

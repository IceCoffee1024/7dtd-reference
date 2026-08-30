using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006CF RID: 1743
[Preserve]
public class MinEventActionRemoveCVar : MinEventActionTargetedBase
{
	// Token: 0x060035DF RID: 13791 RVA: 0x001641A4 File Offset: 0x001623A4
	public override void Execute(MinEventParams _params)
	{
		for (int i = 0; i < this.targets.Count; i++)
		{
			for (int j = 0; j < this.cvarNames.Length; j++)
			{
				this.targets[i].Buffs.SetCustomVar(this.cvarNames[j], 0f, (this.targets[i].isEntityRemote && !_params.Self.isEntityRemote) || _params.IsLocal, CVarOperation.set);
			}
		}
	}

	// Token: 0x060035E0 RID: 13792 RVA: 0x00164228 File Offset: 0x00162428
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

	// Token: 0x04002B57 RID: 11095
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] cvarNames;
}

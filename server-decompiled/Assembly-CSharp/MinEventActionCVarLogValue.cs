using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006CE RID: 1742
[Preserve]
public class MinEventActionCVarLogValue : MinEventActionTargetedBase
{
	// Token: 0x060035DC RID: 13788 RVA: 0x00164104 File Offset: 0x00162304
	public override void Execute(MinEventParams _params)
	{
		for (int i = 0; i < this.targets.Count; i++)
		{
			Log.Out("CVarLogValue: {0} == {1}", new object[]
			{
				this.cvarName,
				this.targets[i].Buffs.GetCustomVar(this.cvarName).ToCultureInvariantString()
			});
		}
	}

	// Token: 0x060035DD RID: 13789 RVA: 0x00164164 File Offset: 0x00162364
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "cvar")
		{
			this.cvarName = _attribute.Value;
			return true;
		}
		return flag;
	}

	// Token: 0x04002B56 RID: 11094
	[PublicizedFrom(EAccessModifier.Private)]
	public string cvarName;
}

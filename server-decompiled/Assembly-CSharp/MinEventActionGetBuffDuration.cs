using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006CA RID: 1738
[Preserve]
public class MinEventActionGetBuffDuration : MinEventActionTargetedBase
{
	// Token: 0x060035D1 RID: 13777 RVA: 0x00163BC4 File Offset: 0x00161DC4
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		if (base.CanExecute(_eventType, _params))
		{
			BuffValue buff = _params.Buff;
			if (((buff != null) ? buff.BuffClass : null) != null)
			{
				return !string.IsNullOrEmpty(this.reference);
			}
		}
		return false;
	}

	// Token: 0x060035D2 RID: 13778 RVA: 0x00163BF4 File Offset: 0x00161DF4
	public override void Execute(MinEventParams _params)
	{
		for (int i = 0; i < this.targets.Count; i++)
		{
			this.targets[i].Buffs.SetCustomVar(this.reference, _params.Buff.BuffClass.DurationMax, true, CVarOperation.set);
		}
	}

	// Token: 0x060035D3 RID: 13779 RVA: 0x00163C48 File Offset: 0x00161E48
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "reference")
		{
			this.reference = _attribute.Value;
			return true;
		}
		return flag;
	}

	// Token: 0x04002B42 RID: 11074
	[PublicizedFrom(EAccessModifier.Private)]
	public string reference;
}

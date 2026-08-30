using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006D4 RID: 1748
[Preserve]
public class MinEventActionGetBuffDuration : MinEventActionTargetedBase
{
	// Token: 0x06003627 RID: 13863 RVA: 0x00165D9C File Offset: 0x00163F9C
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

	// Token: 0x06003628 RID: 13864 RVA: 0x00165DCC File Offset: 0x00163FCC
	public override void Execute(MinEventParams _params)
	{
		for (int i = 0; i < this.targets.Count; i++)
		{
			this.targets[i].Buffs.SetCustomVar(this.reference, _params.Buff.BuffClass.DurationMax, true, CVarOperation.set, false);
		}
	}

	// Token: 0x06003629 RID: 13865 RVA: 0x00165E20 File Offset: 0x00164020
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

	// Token: 0x04002BB6 RID: 11190
	[PublicizedFrom(EAccessModifier.Private)]
	public string reference;
}

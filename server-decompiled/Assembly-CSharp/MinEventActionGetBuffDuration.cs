using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006D6 RID: 1750
[Preserve]
public class MinEventActionGetBuffDuration : MinEventActionTargetedBase
{
	// Token: 0x0600366A RID: 13930 RVA: 0x0016670C File Offset: 0x0016490C
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

	// Token: 0x0600366B RID: 13931 RVA: 0x0016673C File Offset: 0x0016493C
	public override void Execute(MinEventParams _params)
	{
		for (int i = 0; i < this.targets.Count; i++)
		{
			this.targets[i].Buffs.SetCustomVar(this.reference, _params.Buff.BuffClass.DurationMax, true, CVarOperation.set, false);
		}
	}

	// Token: 0x0600366C RID: 13932 RVA: 0x00166790 File Offset: 0x00164990
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

	// Token: 0x04002BAF RID: 11183
	[PublicizedFrom(EAccessModifier.Private)]
	public string reference;
}

using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006DE RID: 1758
[Preserve]
public class MinEventActionGetBuffDuration : MinEventActionTargetedBase
{
	// Token: 0x0600367B RID: 13947 RVA: 0x001674BD File Offset: 0x001656BD
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

	// Token: 0x0600367C RID: 13948 RVA: 0x001674F0 File Offset: 0x001656F0
	public override void Execute(MinEventParams _params)
	{
		for (int i = 0; i < this.targets.Count; i++)
		{
			this.targets[i].Buffs.SetCustomVar(this.reference, _params.Buff.BuffClass.DurationMax, true, CVarOperation.set, false);
		}
	}

	// Token: 0x0600367D RID: 13949 RVA: 0x00167544 File Offset: 0x00165744
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

	// Token: 0x04002BCB RID: 11211
	[PublicizedFrom(EAccessModifier.Private)]
	public string reference;
}

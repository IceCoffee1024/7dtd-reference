using System;
using System.Xml.Linq;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x020006F9 RID: 1785
[Preserve]
public class MinEventActionSetTwitchProgressionDisabled : MinEventActionTargetedBase
{
	// Token: 0x17000592 RID: 1426
	// (get) Token: 0x060036E9 RID: 14057 RVA: 0x0016A169 File Offset: 0x00168369
	// (set) Token: 0x060036EA RID: 14058 RVA: 0x0016A171 File Offset: 0x00168371
	public bool disabled
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x060036EB RID: 14059 RVA: 0x0016A17C File Offset: 0x0016837C
	public override void Execute(MinEventParams _params)
	{
		if (this.targets == null)
		{
			return;
		}
		for (int i = 0; i < this.targets.Count; i++)
		{
			EntityPlayerLocal entityPlayerLocal = this.targets[i] as EntityPlayerLocal;
			if (entityPlayerLocal != null && entityPlayerLocal.TwitchEnabled)
			{
				TwitchManager.Current.OverrideProgession = this.disabled;
			}
		}
	}

	// Token: 0x060036EC RID: 14060 RVA: 0x0016A1D5 File Offset: 0x001683D5
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "disabled")
		{
			this.disabled = StringParsers.ParseBool(_attribute.Value, 0, -1, true);
		}
		return flag;
	}
}

using System;
using System.Xml.Linq;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x020006F1 RID: 1777
[Preserve]
public class MinEventActionSetTwitchProgressionDisabled : MinEventActionTargetedBase
{
	// Token: 0x1700058E RID: 1422
	// (get) Token: 0x060036D2 RID: 14034 RVA: 0x001691D1 File Offset: 0x001673D1
	// (set) Token: 0x060036D3 RID: 14035 RVA: 0x001691D9 File Offset: 0x001673D9
	public bool disabled
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x060036D4 RID: 14036 RVA: 0x001691E4 File Offset: 0x001673E4
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

	// Token: 0x060036D5 RID: 14037 RVA: 0x0016923D File Offset: 0x0016743D
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

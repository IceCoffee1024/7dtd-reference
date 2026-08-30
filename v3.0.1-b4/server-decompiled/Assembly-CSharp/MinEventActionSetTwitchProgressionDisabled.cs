using System;
using System.Xml.Linq;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x020006E5 RID: 1765
[Preserve]
public class MinEventActionSetTwitchProgressionDisabled : MinEventActionTargetedBase
{
	// Token: 0x1700055F RID: 1375
	// (get) Token: 0x06003639 RID: 13881 RVA: 0x00166695 File Offset: 0x00164895
	// (set) Token: 0x0600363A RID: 13882 RVA: 0x0016669D File Offset: 0x0016489D
	public bool disabled
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x0600363B RID: 13883 RVA: 0x001666A8 File Offset: 0x001648A8
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

	// Token: 0x0600363C RID: 13884 RVA: 0x00166701 File Offset: 0x00164901
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

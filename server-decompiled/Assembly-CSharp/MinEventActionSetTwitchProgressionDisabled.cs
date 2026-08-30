using System;
using System.Xml.Linq;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x020006EF RID: 1775
[Preserve]
public class MinEventActionSetTwitchProgressionDisabled : MinEventActionTargetedBase
{
	// Token: 0x17000568 RID: 1384
	// (get) Token: 0x0600368F RID: 13967 RVA: 0x0016886D File Offset: 0x00166A6D
	// (set) Token: 0x06003690 RID: 13968 RVA: 0x00168875 File Offset: 0x00166A75
	public bool disabled
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06003691 RID: 13969 RVA: 0x00168880 File Offset: 0x00166A80
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

	// Token: 0x06003692 RID: 13970 RVA: 0x001688D9 File Offset: 0x00166AD9
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

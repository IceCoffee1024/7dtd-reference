using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006F8 RID: 1784
[Preserve]
public class MinEventActionSetTwitchCooldown : MinEventActionTargetedBase
{
	// Token: 0x17000591 RID: 1425
	// (get) Token: 0x060036E4 RID: 14052 RVA: 0x0016A0A8 File Offset: 0x001682A8
	// (set) Token: 0x060036E5 RID: 14053 RVA: 0x0016A0B0 File Offset: 0x001682B0
	public EntityPlayer.TwitchActionsStates state
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x060036E6 RID: 14054 RVA: 0x0016A0BC File Offset: 0x001682BC
	public override void Execute(MinEventParams _params)
	{
		if (this.targets == null)
		{
			return;
		}
		for (int i = 0; i < this.targets.Count; i++)
		{
			EntityPlayer entityPlayer = this.targets[i] as EntityPlayer;
			if (entityPlayer != null && entityPlayer.TwitchActionsEnabled != EntityPlayer.TwitchActionsStates.Disabled)
			{
				entityPlayer.TwitchActionsEnabled = this.state;
			}
		}
	}

	// Token: 0x060036E7 RID: 14055 RVA: 0x0016A118 File Offset: 0x00168318
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "state")
		{
			this.state = (EntityPlayer.TwitchActionsStates)StringParsers.ParseSInt32(_attribute.Value, 0, -1, NumberStyles.Integer);
			if (this.state == EntityPlayer.TwitchActionsStates.Disabled)
			{
				this.state = EntityPlayer.TwitchActionsStates.TempDisabled;
			}
		}
		return flag;
	}
}

using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006F0 RID: 1776
[Preserve]
public class MinEventActionSetTwitchCooldown : MinEventActionTargetedBase
{
	// Token: 0x1700058D RID: 1421
	// (get) Token: 0x060036CD RID: 14029 RVA: 0x00169110 File Offset: 0x00167310
	// (set) Token: 0x060036CE RID: 14030 RVA: 0x00169118 File Offset: 0x00167318
	public EntityPlayer.TwitchActionsStates state
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x060036CF RID: 14031 RVA: 0x00169124 File Offset: 0x00167324
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

	// Token: 0x060036D0 RID: 14032 RVA: 0x00169180 File Offset: 0x00167380
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

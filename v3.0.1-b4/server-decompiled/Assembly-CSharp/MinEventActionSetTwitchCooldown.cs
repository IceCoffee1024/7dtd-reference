using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006E4 RID: 1764
[Preserve]
public class MinEventActionSetTwitchCooldown : MinEventActionTargetedBase
{
	// Token: 0x1700055E RID: 1374
	// (get) Token: 0x06003634 RID: 13876 RVA: 0x001665D4 File Offset: 0x001647D4
	// (set) Token: 0x06003635 RID: 13877 RVA: 0x001665DC File Offset: 0x001647DC
	public EntityPlayer.TwitchActionsStates state
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06003636 RID: 13878 RVA: 0x001665E8 File Offset: 0x001647E8
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

	// Token: 0x06003637 RID: 13879 RVA: 0x00166644 File Offset: 0x00164844
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

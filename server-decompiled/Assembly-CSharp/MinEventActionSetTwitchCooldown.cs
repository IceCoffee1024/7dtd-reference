using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006EE RID: 1774
[Preserve]
public class MinEventActionSetTwitchCooldown : MinEventActionTargetedBase
{
	// Token: 0x17000567 RID: 1383
	// (get) Token: 0x0600368A RID: 13962 RVA: 0x001687AC File Offset: 0x001669AC
	// (set) Token: 0x0600368B RID: 13963 RVA: 0x001687B4 File Offset: 0x001669B4
	public EntityPlayer.TwitchActionsStates state
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x0600368C RID: 13964 RVA: 0x001687C0 File Offset: 0x001669C0
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

	// Token: 0x0600368D RID: 13965 RVA: 0x0016881C File Offset: 0x00166A1C
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

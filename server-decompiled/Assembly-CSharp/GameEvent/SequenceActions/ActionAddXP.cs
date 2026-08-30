using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001965 RID: 6501
	[Preserve]
	public class ActionAddXP : ActionBaseClientAction
	{
		// Token: 0x0600C7FF RID: 51199 RVA: 0x00495570 File Offset: 0x00493770
		public override void OnClientPerform(Entity target)
		{
			EntityPlayer entityPlayer = target as EntityPlayer;
			if (entityPlayer != null)
			{
				this.xpAmount = GameEventManager.GetIntValue(entityPlayer, this.xpAmountText, 0);
				if (this.xpAmount < 0)
				{
					this.xpAmount = 0;
				}
				entityPlayer.Progression.AddLevelExp(this.xpAmount, "_xpOther", Progression.XPTypes.Other, true, true, -1, null);
			}
		}

		// Token: 0x0600C800 RID: 51200 RVA: 0x004955C6 File Offset: 0x004937C6
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionAddXP.PropXPAmount, ref this.xpAmountText);
		}

		// Token: 0x0600C801 RID: 51201 RVA: 0x004955E0 File Offset: 0x004937E0
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionAddXP
			{
				xpAmountText = this.xpAmountText
			};
		}

		// Token: 0x040096C6 RID: 38598
		[PublicizedFrom(EAccessModifier.Protected)]
		public string xpAmountText;

		// Token: 0x040096C7 RID: 38599
		[PublicizedFrom(EAccessModifier.Protected)]
		public int xpAmount;

		// Token: 0x040096C8 RID: 38600
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropXPAmount = "xp_amount";
	}
}

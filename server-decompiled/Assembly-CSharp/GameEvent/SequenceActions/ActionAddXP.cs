using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001991 RID: 6545
	[Preserve]
	public class ActionAddXP : ActionBaseClientAction
	{
		// Token: 0x0600C957 RID: 51543 RVA: 0x00499A60 File Offset: 0x00497C60
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

		// Token: 0x0600C958 RID: 51544 RVA: 0x00499AB6 File Offset: 0x00497CB6
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionAddXP.PropXPAmount, ref this.xpAmountText);
		}

		// Token: 0x0600C959 RID: 51545 RVA: 0x00499AD0 File Offset: 0x00497CD0
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionAddXP
			{
				xpAmountText = this.xpAmountText
			};
		}

		// Token: 0x0400978C RID: 38796
		[PublicizedFrom(EAccessModifier.Protected)]
		public string xpAmountText;

		// Token: 0x0400978D RID: 38797
		[PublicizedFrom(EAccessModifier.Protected)]
		public int xpAmount;

		// Token: 0x0400978E RID: 38798
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropXPAmount = "xp_amount";
	}
}

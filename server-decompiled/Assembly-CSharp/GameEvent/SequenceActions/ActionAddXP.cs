using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200193F RID: 6463
	[Preserve]
	public class ActionAddXP : ActionBaseClientAction
	{
		// Token: 0x0600C678 RID: 50808 RVA: 0x0048F224 File Offset: 0x0048D424
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

		// Token: 0x0600C679 RID: 50809 RVA: 0x0048F27A File Offset: 0x0048D47A
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionAddXP.PropXPAmount, ref this.xpAmountText);
		}

		// Token: 0x0600C67A RID: 50810 RVA: 0x0048F294 File Offset: 0x0048D494
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionAddXP
			{
				xpAmountText = this.xpAmountText
			};
		}

		// Token: 0x0400959D RID: 38301
		[PublicizedFrom(EAccessModifier.Protected)]
		public string xpAmountText;

		// Token: 0x0400959E RID: 38302
		[PublicizedFrom(EAccessModifier.Protected)]
		public int xpAmount;

		// Token: 0x0400959F RID: 38303
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropXPAmount = "xp_amount";
	}
}

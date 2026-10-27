using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019C2 RID: 6594
	[Preserve]
	public class ActionAddXP : ActionBaseClientAction
	{
		// Token: 0x0600CA6B RID: 51819 RVA: 0x0049D560 File Offset: 0x0049B760
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

		// Token: 0x0600CA6C RID: 51820 RVA: 0x0049D5B6 File Offset: 0x0049B7B6
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionAddXP.PropXPAmount, ref this.xpAmountText);
		}

		// Token: 0x0600CA6D RID: 51821 RVA: 0x0049D5D0 File Offset: 0x0049B7D0
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionAddXP
			{
				xpAmountText = this.xpAmountText
			};
		}

		// Token: 0x04009875 RID: 39029
		[PublicizedFrom(EAccessModifier.Protected)]
		public string xpAmountText;

		// Token: 0x04009876 RID: 39030
		[PublicizedFrom(EAccessModifier.Protected)]
		public int xpAmount;

		// Token: 0x04009877 RID: 39031
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropXPAmount = "xp_amount";
	}
}

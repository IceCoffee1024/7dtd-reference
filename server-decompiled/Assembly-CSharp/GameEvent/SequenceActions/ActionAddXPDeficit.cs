using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019C3 RID: 6595
	[Preserve]
	public class ActionAddXPDeficit : ActionBaseClientAction
	{
		// Token: 0x0600CA70 RID: 51824 RVA: 0x0049D5F0 File Offset: 0x0049B7F0
		public override void OnClientPerform(Entity target)
		{
			EntityPlayer entityPlayer = target as EntityPlayer;
			if (entityPlayer != null)
			{
				this.xpAmount = GameEventManager.GetIntValue(entityPlayer, this.xpAmountText, 0);
				if (this.xpAmount == 0)
				{
					entityPlayer.Progression.AddXPDeficit();
					return;
				}
				entityPlayer.Progression.ExpDeficit += this.xpAmount;
			}
		}

		// Token: 0x0600CA71 RID: 51825 RVA: 0x0049D646 File Offset: 0x0049B846
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionAddXPDeficit.PropXPAmount, ref this.xpAmountText);
		}

		// Token: 0x0600CA72 RID: 51826 RVA: 0x0049D660 File Offset: 0x0049B860
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionAddXPDeficit
			{
				xpAmountText = this.xpAmountText
			};
		}

		// Token: 0x04009878 RID: 39032
		[PublicizedFrom(EAccessModifier.Protected)]
		public string xpAmountText;

		// Token: 0x04009879 RID: 39033
		[PublicizedFrom(EAccessModifier.Protected)]
		public int xpAmount;

		// Token: 0x0400987A RID: 39034
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropXPAmount = "xp_amount";
	}
}

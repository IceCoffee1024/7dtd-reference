using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001992 RID: 6546
	[Preserve]
	public class ActionAddXPDeficit : ActionBaseClientAction
	{
		// Token: 0x0600C95C RID: 51548 RVA: 0x00499AF0 File Offset: 0x00497CF0
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

		// Token: 0x0600C95D RID: 51549 RVA: 0x00499B46 File Offset: 0x00497D46
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionAddXPDeficit.PropXPAmount, ref this.xpAmountText);
		}

		// Token: 0x0600C95E RID: 51550 RVA: 0x00499B60 File Offset: 0x00497D60
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionAddXPDeficit
			{
				xpAmountText = this.xpAmountText
			};
		}

		// Token: 0x0400978F RID: 38799
		[PublicizedFrom(EAccessModifier.Protected)]
		public string xpAmountText;

		// Token: 0x04009790 RID: 38800
		[PublicizedFrom(EAccessModifier.Protected)]
		public int xpAmount;

		// Token: 0x04009791 RID: 38801
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropXPAmount = "xp_amount";
	}
}

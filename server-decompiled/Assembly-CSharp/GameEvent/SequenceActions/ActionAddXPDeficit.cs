using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001966 RID: 6502
	[Preserve]
	public class ActionAddXPDeficit : ActionBaseClientAction
	{
		// Token: 0x0600C804 RID: 51204 RVA: 0x00495600 File Offset: 0x00493800
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

		// Token: 0x0600C805 RID: 51205 RVA: 0x00495656 File Offset: 0x00493856
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionAddXPDeficit.PropXPAmount, ref this.xpAmountText);
		}

		// Token: 0x0600C806 RID: 51206 RVA: 0x00495670 File Offset: 0x00493870
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionAddXPDeficit
			{
				xpAmountText = this.xpAmountText
			};
		}

		// Token: 0x040096C9 RID: 38601
		[PublicizedFrom(EAccessModifier.Protected)]
		public string xpAmountText;

		// Token: 0x040096CA RID: 38602
		[PublicizedFrom(EAccessModifier.Protected)]
		public int xpAmount;

		// Token: 0x040096CB RID: 38603
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropXPAmount = "xp_amount";
	}
}

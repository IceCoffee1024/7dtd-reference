using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001940 RID: 6464
	[Preserve]
	public class ActionAddXPDeficit : ActionBaseClientAction
	{
		// Token: 0x0600C67D RID: 50813 RVA: 0x0048F2B4 File Offset: 0x0048D4B4
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

		// Token: 0x0600C67E RID: 50814 RVA: 0x0048F30A File Offset: 0x0048D50A
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionAddXPDeficit.PropXPAmount, ref this.xpAmountText);
		}

		// Token: 0x0600C67F RID: 50815 RVA: 0x0048F324 File Offset: 0x0048D524
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionAddXPDeficit
			{
				xpAmountText = this.xpAmountText
			};
		}

		// Token: 0x040095A0 RID: 38304
		[PublicizedFrom(EAccessModifier.Protected)]
		public string xpAmountText;

		// Token: 0x040095A1 RID: 38305
		[PublicizedFrom(EAccessModifier.Protected)]
		public int xpAmount;

		// Token: 0x040095A2 RID: 38306
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropXPAmount = "xp_amount";
	}
}

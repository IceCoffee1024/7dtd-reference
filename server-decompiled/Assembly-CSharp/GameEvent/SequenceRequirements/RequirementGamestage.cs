using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001953 RID: 6483
	[Preserve]
	public class RequirementGamestage : BaseOperationRequirement
	{
		// Token: 0x0600C826 RID: 51238 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C827 RID: 51239 RVA: 0x0049502C File Offset: 0x0049322C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			EntityPlayer entityPlayer = target as EntityPlayer;
			return (entityPlayer != null) ? entityPlayer.gameStage : 0;
		}

		// Token: 0x0600C828 RID: 51240 RVA: 0x00495051 File Offset: 0x00493251
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return GameEventManager.GetIntValue(target as EntityAlive, this.gamestageText, 0);
		}

		// Token: 0x0600C829 RID: 51241 RVA: 0x0049506A File Offset: 0x0049326A
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(RequirementGamestage.PropGamestage, ref this.gamestageText);
		}

		// Token: 0x0600C82A RID: 51242 RVA: 0x00495084 File Offset: 0x00493284
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementGamestage
			{
				Invert = this.Invert,
				operation = this.operation,
				gamestageText = this.gamestageText
			};
		}

		// Token: 0x04009699 RID: 38553
		[PublicizedFrom(EAccessModifier.Protected)]
		public string gamestageText;

		// Token: 0x0400969A RID: 38554
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGamestage = "game_stage";
	}
}

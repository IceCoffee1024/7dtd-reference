using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001984 RID: 6532
	[Preserve]
	public class RequirementGamestage : BaseOperationRequirement
	{
		// Token: 0x0600C93A RID: 51514 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C93B RID: 51515 RVA: 0x00498A20 File Offset: 0x00496C20
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			EntityPlayer entityPlayer = target as EntityPlayer;
			return (entityPlayer != null) ? entityPlayer.gameStage : 0;
		}

		// Token: 0x0600C93C RID: 51516 RVA: 0x00498A45 File Offset: 0x00496C45
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return GameEventManager.GetIntValue(target as EntityAlive, this.gamestageText, 0);
		}

		// Token: 0x0600C93D RID: 51517 RVA: 0x00498A5E File Offset: 0x00496C5E
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(RequirementGamestage.PropGamestage, ref this.gamestageText);
		}

		// Token: 0x0600C93E RID: 51518 RVA: 0x00498A78 File Offset: 0x00496C78
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

		// Token: 0x04009782 RID: 38786
		[PublicizedFrom(EAccessModifier.Protected)]
		public string gamestageText;

		// Token: 0x04009783 RID: 38787
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGamestage = "game_stage";
	}
}

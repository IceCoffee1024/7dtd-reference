using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001927 RID: 6439
	[Preserve]
	public class RequirementGamestage : BaseOperationRequirement
	{
		// Token: 0x0600C6CE RID: 50894 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C6CF RID: 50895 RVA: 0x00490B40 File Offset: 0x0048ED40
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			EntityPlayer entityPlayer = target as EntityPlayer;
			return (entityPlayer != null) ? entityPlayer.gameStage : 0;
		}

		// Token: 0x0600C6D0 RID: 50896 RVA: 0x00490B65 File Offset: 0x0048ED65
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return GameEventManager.GetIntValue(target as EntityAlive, this.gamestageText, 0);
		}

		// Token: 0x0600C6D1 RID: 50897 RVA: 0x00490B7E File Offset: 0x0048ED7E
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(RequirementGamestage.PropGamestage, ref this.gamestageText);
		}

		// Token: 0x0600C6D2 RID: 50898 RVA: 0x00490B98 File Offset: 0x0048ED98
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

		// Token: 0x040095D3 RID: 38355
		[PublicizedFrom(EAccessModifier.Protected)]
		public string gamestageText;

		// Token: 0x040095D4 RID: 38356
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGamestage = "game_stage";
	}
}

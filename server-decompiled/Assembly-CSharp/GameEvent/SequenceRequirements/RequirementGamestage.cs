using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001901 RID: 6401
	[Preserve]
	public class RequirementGamestage : BaseOperationRequirement
	{
		// Token: 0x0600C547 RID: 50503 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C548 RID: 50504 RVA: 0x0048A7F4 File Offset: 0x004889F4
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			EntityPlayer entityPlayer = target as EntityPlayer;
			return (entityPlayer != null) ? entityPlayer.gameStage : 0;
		}

		// Token: 0x0600C549 RID: 50505 RVA: 0x0048A819 File Offset: 0x00488A19
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return GameEventManager.GetIntValue(target as EntityAlive, this.gamestageText, 0);
		}

		// Token: 0x0600C54A RID: 50506 RVA: 0x0048A832 File Offset: 0x00488A32
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(RequirementGamestage.PropGamestage, ref this.gamestageText);
		}

		// Token: 0x0600C54B RID: 50507 RVA: 0x0048A84C File Offset: 0x00488A4C
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

		// Token: 0x040094AA RID: 38058
		[PublicizedFrom(EAccessModifier.Protected)]
		public string gamestageText;

		// Token: 0x040094AB RID: 38059
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGamestage = "game_stage";
	}
}

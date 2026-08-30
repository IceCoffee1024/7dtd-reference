using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x0200192A RID: 6442
	[Preserve]
	public class RequirementGameStatInt : BaseOperationRequirement
	{
		// Token: 0x0600C6E1 RID: 50913 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C6E2 RID: 50914 RVA: 0x00490D02 File Offset: 0x0048EF02
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			return GameStats.GetInt(this.GameStat);
		}

		// Token: 0x0600C6E3 RID: 50915 RVA: 0x00490D14 File Offset: 0x0048EF14
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return GameEventManager.GetIntValue(target as EntityAlive, this.valueText, 0);
		}

		// Token: 0x0600C6E4 RID: 50916 RVA: 0x00490D2D File Offset: 0x0048EF2D
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<EnumGameStats>(RequirementGameStatInt.PropGameStat, ref this.GameStat);
			properties.ParseString(RequirementGameStatInt.PropValue, ref this.valueText);
		}

		// Token: 0x0600C6E5 RID: 50917 RVA: 0x00490D58 File Offset: 0x0048EF58
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementGameStatInt
			{
				Invert = this.Invert,
				operation = this.operation,
				GameStat = this.GameStat,
				valueText = this.valueText
			};
		}

		// Token: 0x040095DB RID: 38363
		[PublicizedFrom(EAccessModifier.Protected)]
		public EnumGameStats GameStat = EnumGameStats.AnimalCount;

		// Token: 0x040095DC RID: 38364
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x040095DD RID: 38365
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGameStat = "gamestat";

		// Token: 0x040095DE RID: 38366
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";
	}
}

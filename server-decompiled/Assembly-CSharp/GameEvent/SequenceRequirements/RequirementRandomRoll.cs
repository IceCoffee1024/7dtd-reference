using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001942 RID: 6466
	[Preserve]
	public class RequirementRandomRoll : BaseOperationRequirement
	{
		// Token: 0x0600C74E RID: 51022 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C74F RID: 51023 RVA: 0x00491C2C File Offset: 0x0048FE2C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			float randomFloat = GameEventManager.Current.Random.RandomFloat;
			return Mathf.Lerp(this.minMax.x, this.minMax.y, randomFloat);
		}

		// Token: 0x0600C750 RID: 51024 RVA: 0x00491C6A File Offset: 0x0048FE6A
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return GameEventManager.GetFloatValue(target as EntityAlive, this.valueText, 0f);
		}

		// Token: 0x0600C751 RID: 51025 RVA: 0x00491C87 File Offset: 0x0048FE87
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseVec(RequirementRandomRoll.PropMinMax, ref this.minMax);
			properties.ParseString(RequirementRandomRoll.PropValue, ref this.valueText);
		}

		// Token: 0x0600C752 RID: 51026 RVA: 0x00491CB2 File Offset: 0x0048FEB2
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementRandomRoll
			{
				Invert = this.Invert,
				operation = this.operation,
				minMax = this.minMax,
				valueText = this.valueText
			};
		}

		// Token: 0x0400960B RID: 38411
		[PublicizedFrom(EAccessModifier.Protected)]
		public Vector2 minMax;

		// Token: 0x0400960C RID: 38412
		[PublicizedFrom(EAccessModifier.Protected)]
		public GameRandom rand;

		// Token: 0x0400960D RID: 38413
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x0400960E RID: 38414
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMinMax = "min_max";

		// Token: 0x0400960F RID: 38415
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";
	}
}

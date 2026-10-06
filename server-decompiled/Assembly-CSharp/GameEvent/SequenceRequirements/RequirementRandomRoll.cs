using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x0200196E RID: 6510
	[Preserve]
	public class RequirementRandomRoll : BaseOperationRequirement
	{
		// Token: 0x0600C8A6 RID: 51366 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C8A7 RID: 51367 RVA: 0x00496118 File Offset: 0x00494318
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			float randomFloat = GameEventManager.Current.Random.RandomFloat;
			return Mathf.Lerp(this.minMax.x, this.minMax.y, randomFloat);
		}

		// Token: 0x0600C8A8 RID: 51368 RVA: 0x00496156 File Offset: 0x00494356
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return GameEventManager.GetFloatValue(target as EntityAlive, this.valueText, 0f);
		}

		// Token: 0x0600C8A9 RID: 51369 RVA: 0x00496173 File Offset: 0x00494373
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseVec(RequirementRandomRoll.PropMinMax, ref this.minMax);
			properties.ParseString(RequirementRandomRoll.PropValue, ref this.valueText);
		}

		// Token: 0x0600C8AA RID: 51370 RVA: 0x0049619E File Offset: 0x0049439E
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

		// Token: 0x040096D1 RID: 38609
		[PublicizedFrom(EAccessModifier.Protected)]
		public Vector2 minMax;

		// Token: 0x040096D2 RID: 38610
		[PublicizedFrom(EAccessModifier.Protected)]
		public GameRandom rand;

		// Token: 0x040096D3 RID: 38611
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x040096D4 RID: 38612
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMinMax = "min_max";

		// Token: 0x040096D5 RID: 38613
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";
	}
}

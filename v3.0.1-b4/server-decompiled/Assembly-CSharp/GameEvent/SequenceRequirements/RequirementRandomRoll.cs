using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x0200191C RID: 6428
	[Preserve]
	public class RequirementRandomRoll : BaseOperationRequirement
	{
		// Token: 0x0600C5C7 RID: 50631 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C5C8 RID: 50632 RVA: 0x0048B8E0 File Offset: 0x00489AE0
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			float randomFloat = GameEventManager.Current.Random.RandomFloat;
			return Mathf.Lerp(this.minMax.x, this.minMax.y, randomFloat);
		}

		// Token: 0x0600C5C9 RID: 50633 RVA: 0x0048B91E File Offset: 0x00489B1E
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return GameEventManager.GetFloatValue(target as EntityAlive, this.valueText, 0f);
		}

		// Token: 0x0600C5CA RID: 50634 RVA: 0x0048B93B File Offset: 0x00489B3B
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseVec(RequirementRandomRoll.PropMinMax, ref this.minMax);
			properties.ParseString(RequirementRandomRoll.PropValue, ref this.valueText);
		}

		// Token: 0x0600C5CB RID: 50635 RVA: 0x0048B966 File Offset: 0x00489B66
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

		// Token: 0x040094E2 RID: 38114
		[PublicizedFrom(EAccessModifier.Protected)]
		public Vector2 minMax;

		// Token: 0x040094E3 RID: 38115
		[PublicizedFrom(EAccessModifier.Protected)]
		public GameRandom rand;

		// Token: 0x040094E4 RID: 38116
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x040094E5 RID: 38117
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMinMax = "min_max";

		// Token: 0x040094E6 RID: 38118
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";
	}
}

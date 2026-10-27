using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x0200199F RID: 6559
	[Preserve]
	public class RequirementRandomRoll : BaseOperationRequirement
	{
		// Token: 0x0600C9BA RID: 51642 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C9BB RID: 51643 RVA: 0x00499B0C File Offset: 0x00497D0C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			float randomFloat = GameEventManager.Current.Random.RandomFloat;
			return Mathf.Lerp(this.minMax.x, this.minMax.y, randomFloat);
		}

		// Token: 0x0600C9BC RID: 51644 RVA: 0x00499B4A File Offset: 0x00497D4A
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return GameEventManager.GetFloatValue(target as EntityAlive, this.valueText, 0f);
		}

		// Token: 0x0600C9BD RID: 51645 RVA: 0x00499B67 File Offset: 0x00497D67
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseVec(RequirementRandomRoll.PropMinMax, ref this.minMax);
			properties.ParseString(RequirementRandomRoll.PropValue, ref this.valueText);
		}

		// Token: 0x0600C9BE RID: 51646 RVA: 0x00499B92 File Offset: 0x00497D92
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

		// Token: 0x040097BA RID: 38842
		[PublicizedFrom(EAccessModifier.Protected)]
		public Vector2 minMax;

		// Token: 0x040097BB RID: 38843
		[PublicizedFrom(EAccessModifier.Protected)]
		public GameRandom rand;

		// Token: 0x040097BC RID: 38844
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x040097BD RID: 38845
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMinMax = "min_max";

		// Token: 0x040097BE RID: 38846
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";
	}
}

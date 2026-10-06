using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x0200196D RID: 6509
	[Preserve]
	public class RequirementProgression : BaseOperationRequirement
	{
		// Token: 0x0600C89F RID: 51359 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C8A0 RID: 51360 RVA: 0x0049601C File Offset: 0x0049421C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			EntityAlive entityAlive = target as EntityAlive;
			if (entityAlive != null && entityAlive.Progression != null)
			{
				this.pv = entityAlive.Progression.GetProgressionValue(this.progressionName);
				if (this.pv != null)
				{
					return this.pv.GetCalculatedLevel(entityAlive);
				}
			}
			return 0;
		}

		// Token: 0x0600C8A1 RID: 51361 RVA: 0x00496072 File Offset: 0x00494272
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return GameEventManager.GetIntValue(target as EntityAlive, this.valueText, 0);
		}

		// Token: 0x0600C8A2 RID: 51362 RVA: 0x0049608B File Offset: 0x0049428B
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(RequirementProgression.PropProgressionName, ref this.progressionName);
			properties.ParseString(RequirementProgression.PropValue, ref this.valueText);
		}

		// Token: 0x0600C8A3 RID: 51363 RVA: 0x004960B6 File Offset: 0x004942B6
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementProgression
			{
				Invert = this.Invert,
				operation = this.operation,
				progressionName = this.progressionName,
				valueText = this.valueText
			};
		}

		// Token: 0x040096CC RID: 38604
		[PublicizedFrom(EAccessModifier.Protected)]
		public string progressionName = "";

		// Token: 0x040096CD RID: 38605
		[PublicizedFrom(EAccessModifier.Protected)]
		public ProgressionValue pv;

		// Token: 0x040096CE RID: 38606
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x040096CF RID: 38607
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropProgressionName = "name";

		// Token: 0x040096D0 RID: 38608
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";
	}
}

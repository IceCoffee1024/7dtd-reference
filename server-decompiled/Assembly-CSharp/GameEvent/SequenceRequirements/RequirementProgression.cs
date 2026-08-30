using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x0200191B RID: 6427
	[Preserve]
	public class RequirementProgression : BaseOperationRequirement
	{
		// Token: 0x0600C5C0 RID: 50624 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C5C1 RID: 50625 RVA: 0x0048B7E4 File Offset: 0x004899E4
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

		// Token: 0x0600C5C2 RID: 50626 RVA: 0x0048B83A File Offset: 0x00489A3A
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return GameEventManager.GetIntValue(target as EntityAlive, this.valueText, 0);
		}

		// Token: 0x0600C5C3 RID: 50627 RVA: 0x0048B853 File Offset: 0x00489A53
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(RequirementProgression.PropProgressionName, ref this.progressionName);
			properties.ParseString(RequirementProgression.PropValue, ref this.valueText);
		}

		// Token: 0x0600C5C4 RID: 50628 RVA: 0x0048B87E File Offset: 0x00489A7E
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

		// Token: 0x040094DD RID: 38109
		[PublicizedFrom(EAccessModifier.Protected)]
		public string progressionName = "";

		// Token: 0x040094DE RID: 38110
		[PublicizedFrom(EAccessModifier.Protected)]
		public ProgressionValue pv;

		// Token: 0x040094DF RID: 38111
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x040094E0 RID: 38112
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropProgressionName = "name";

		// Token: 0x040094E1 RID: 38113
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";
	}
}

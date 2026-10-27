using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x0200199E RID: 6558
	[Preserve]
	public class RequirementProgression : BaseOperationRequirement
	{
		// Token: 0x0600C9B3 RID: 51635 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C9B4 RID: 51636 RVA: 0x00499A10 File Offset: 0x00497C10
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

		// Token: 0x0600C9B5 RID: 51637 RVA: 0x00499A66 File Offset: 0x00497C66
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return GameEventManager.GetIntValue(target as EntityAlive, this.valueText, 0);
		}

		// Token: 0x0600C9B6 RID: 51638 RVA: 0x00499A7F File Offset: 0x00497C7F
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(RequirementProgression.PropProgressionName, ref this.progressionName);
			properties.ParseString(RequirementProgression.PropValue, ref this.valueText);
		}

		// Token: 0x0600C9B7 RID: 51639 RVA: 0x00499AAA File Offset: 0x00497CAA
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

		// Token: 0x040097B5 RID: 38837
		[PublicizedFrom(EAccessModifier.Protected)]
		public string progressionName = "";

		// Token: 0x040097B6 RID: 38838
		[PublicizedFrom(EAccessModifier.Protected)]
		public ProgressionValue pv;

		// Token: 0x040097B7 RID: 38839
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x040097B8 RID: 38840
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropProgressionName = "name";

		// Token: 0x040097B9 RID: 38841
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";
	}
}

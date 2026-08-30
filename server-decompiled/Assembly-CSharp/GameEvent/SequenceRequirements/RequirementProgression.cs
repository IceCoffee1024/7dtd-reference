using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001941 RID: 6465
	[Preserve]
	public class RequirementProgression : BaseOperationRequirement
	{
		// Token: 0x0600C747 RID: 51015 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C748 RID: 51016 RVA: 0x00491B30 File Offset: 0x0048FD30
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

		// Token: 0x0600C749 RID: 51017 RVA: 0x00491B86 File Offset: 0x0048FD86
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return GameEventManager.GetIntValue(target as EntityAlive, this.valueText, 0);
		}

		// Token: 0x0600C74A RID: 51018 RVA: 0x00491B9F File Offset: 0x0048FD9F
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(RequirementProgression.PropProgressionName, ref this.progressionName);
			properties.ParseString(RequirementProgression.PropValue, ref this.valueText);
		}

		// Token: 0x0600C74B RID: 51019 RVA: 0x00491BCA File Offset: 0x0048FDCA
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

		// Token: 0x04009606 RID: 38406
		[PublicizedFrom(EAccessModifier.Protected)]
		public string progressionName = "";

		// Token: 0x04009607 RID: 38407
		[PublicizedFrom(EAccessModifier.Protected)]
		public ProgressionValue pv;

		// Token: 0x04009608 RID: 38408
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x04009609 RID: 38409
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropProgressionName = "name";

		// Token: 0x0400960A RID: 38410
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";
	}
}

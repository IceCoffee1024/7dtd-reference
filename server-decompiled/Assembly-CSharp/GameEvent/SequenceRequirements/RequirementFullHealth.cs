using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001926 RID: 6438
	[Preserve]
	public class RequirementFullHealth : BaseRequirement
	{
		// Token: 0x0600C6CA RID: 50890 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C6CB RID: 50891 RVA: 0x00490AD8 File Offset: 0x0048ECD8
		public override bool CanPerform(Entity target)
		{
			EntityAlive entityAlive = target as EntityAlive;
			if (entityAlive == null)
			{
				return false;
			}
			if (entityAlive.Stats.Health.Value == entityAlive.Stats.Health.Max)
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600C6CC RID: 50892 RVA: 0x00490B25 File Offset: 0x0048ED25
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementFullHealth
			{
				Invert = this.Invert
			};
		}
	}
}

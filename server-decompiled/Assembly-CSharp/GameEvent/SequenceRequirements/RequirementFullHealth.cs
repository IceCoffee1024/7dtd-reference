using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001952 RID: 6482
	[Preserve]
	public class RequirementFullHealth : BaseRequirement
	{
		// Token: 0x0600C822 RID: 51234 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C823 RID: 51235 RVA: 0x00494FC4 File Offset: 0x004931C4
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

		// Token: 0x0600C824 RID: 51236 RVA: 0x00495011 File Offset: 0x00493211
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

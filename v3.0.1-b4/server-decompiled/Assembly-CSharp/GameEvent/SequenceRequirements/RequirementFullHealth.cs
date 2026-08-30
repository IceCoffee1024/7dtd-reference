using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001900 RID: 6400
	[Preserve]
	public class RequirementFullHealth : BaseRequirement
	{
		// Token: 0x0600C543 RID: 50499 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C544 RID: 50500 RVA: 0x0048A78C File Offset: 0x0048898C
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

		// Token: 0x0600C545 RID: 50501 RVA: 0x0048A7D9 File Offset: 0x004889D9
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

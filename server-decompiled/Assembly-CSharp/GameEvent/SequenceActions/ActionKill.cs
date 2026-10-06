using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019B7 RID: 6583
	[Preserve]
	public class ActionKill : ActionBaseTargetAction
	{
		// Token: 0x0600C9F7 RID: 51703 RVA: 0x0049F34C File Offset: 0x0049D54C
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			EntityAlive entityAlive = target as EntityAlive;
			if (entityAlive != null)
			{
				entityAlive.DamageEntity(new DamageSource(EnumDamageSource.Internal, EnumDamageTypes.Suicide), 99999, false, 1f);
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C9F8 RID: 51704 RVA: 0x0049F384 File Offset: 0x0049D584
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionKill
			{
				targetGroup = this.targetGroup
			};
		}
	}
}

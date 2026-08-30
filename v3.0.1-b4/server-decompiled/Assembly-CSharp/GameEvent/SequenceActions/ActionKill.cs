using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001965 RID: 6501
	[Preserve]
	public class ActionKill : ActionBaseTargetAction
	{
		// Token: 0x0600C718 RID: 50968 RVA: 0x00494B4C File Offset: 0x00492D4C
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			EntityAlive entityAlive = target as EntityAlive;
			if (entityAlive != null)
			{
				entityAlive.DamageEntity(new DamageSource(EnumDamageSource.Internal, EnumDamageTypes.Suicide), 99999, false, 1f);
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C719 RID: 50969 RVA: 0x00494B84 File Offset: 0x00492D84
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

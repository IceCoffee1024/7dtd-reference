using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200198B RID: 6539
	[Preserve]
	public class ActionKill : ActionBaseTargetAction
	{
		// Token: 0x0600C89F RID: 51359 RVA: 0x0049AE88 File Offset: 0x00499088
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			EntityAlive entityAlive = target as EntityAlive;
			if (entityAlive != null)
			{
				entityAlive.DamageEntity(new DamageSource(EnumDamageSource.Internal, EnumDamageTypes.Suicide), 99999, false, 1f);
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C8A0 RID: 51360 RVA: 0x0049AEC0 File Offset: 0x004990C0
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

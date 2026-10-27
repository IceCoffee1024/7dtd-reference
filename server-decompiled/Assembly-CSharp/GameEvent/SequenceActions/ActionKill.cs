using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019E8 RID: 6632
	[Preserve]
	public class ActionKill : ActionBaseTargetAction
	{
		// Token: 0x0600CB0B RID: 51979 RVA: 0x004A2E4C File Offset: 0x004A104C
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			EntityAlive entityAlive = target as EntityAlive;
			if (entityAlive != null)
			{
				entityAlive.DamageEntity(new DamageSource(EnumDamageSource.Internal, EnumDamageTypes.Suicide), 99999, false, 1f);
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600CB0C RID: 51980 RVA: 0x004A2E84 File Offset: 0x004A1084
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

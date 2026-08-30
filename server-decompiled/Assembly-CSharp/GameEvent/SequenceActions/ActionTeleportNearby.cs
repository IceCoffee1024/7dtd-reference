using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019CC RID: 6604
	[Preserve]
	public class ActionTeleportNearby : ActionBaseTeleport
	{
		// Token: 0x0600C9CC RID: 51660 RVA: 0x004A074C File Offset: 0x0049E94C
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			World world = GameManager.Instance.World;
			if (base.Owner.TargetPosition != Vector3.zero)
			{
				Vector3 targetPosition = base.Owner.TargetPosition;
				base.TeleportEntity(target, targetPosition);
				return BaseAction.ActionCompleteStates.Complete;
			}
			return BaseAction.ActionCompleteStates.InComplete;
		}

		// Token: 0x0600C9CD RID: 51661 RVA: 0x004A0792 File Offset: 0x0049E992
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionTeleportNearby
			{
				targetGroup = this.targetGroup,
				teleportDelayText = this.teleportDelayText
			};
		}
	}
}

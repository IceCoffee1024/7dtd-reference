using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019F8 RID: 6648
	[Preserve]
	public class ActionTeleportNearby : ActionBaseTeleport
	{
		// Token: 0x0600CB24 RID: 52004 RVA: 0x004A4BF0 File Offset: 0x004A2DF0
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

		// Token: 0x0600CB25 RID: 52005 RVA: 0x004A4C36 File Offset: 0x004A2E36
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

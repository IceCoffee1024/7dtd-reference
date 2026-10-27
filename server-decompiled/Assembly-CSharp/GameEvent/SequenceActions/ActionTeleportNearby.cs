using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A29 RID: 6697
	[Preserve]
	public class ActionTeleportNearby : ActionBaseTeleport
	{
		// Token: 0x0600CC38 RID: 52280 RVA: 0x004A86EC File Offset: 0x004A68EC
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

		// Token: 0x0600CC39 RID: 52281 RVA: 0x004A8732 File Offset: 0x004A6932
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

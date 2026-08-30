using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001978 RID: 6520
	[Preserve]
	public class ActionRandomTeleport : ActionBaseTeleport
	{
		// Token: 0x0600C764 RID: 51044 RVA: 0x004961C4 File Offset: 0x004943C4
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			EntityPlayer entityPlayer = target as EntityPlayer;
			if (!(entityPlayer != null))
			{
				return BaseAction.ActionCompleteStates.Complete;
			}
			float num = GameManager.Instance.World.RandomRange(this.minDistance, this.maxDistance);
			this.position = ObjectiveRandomGoto.CalculateRandomPoint(entityPlayer.entityId, num, "", true, BiomeFilterTypes.SameBiome, "");
			if (this.position.y >= 0)
			{
				Vector3 vector = this.position.ToVector3();
				vector.y = -2000f;
				base.TeleportEntity(entityPlayer, vector);
				return BaseAction.ActionCompleteStates.Complete;
			}
			this.maxTries--;
			if (this.maxTries != 0)
			{
				return BaseAction.ActionCompleteStates.InComplete;
			}
			return BaseAction.ActionCompleteStates.InCompleteRefund;
		}

		// Token: 0x0600C765 RID: 51045 RVA: 0x00496269 File Offset: 0x00494469
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseFloat(ActionRandomTeleport.PropMinDistance, ref this.minDistance);
			properties.ParseFloat(ActionRandomTeleport.PropMaxDistance, ref this.maxDistance);
			properties.ParseInt(ActionRandomTeleport.PropMaxTries, ref this.maxTries);
		}

		// Token: 0x0600C766 RID: 51046 RVA: 0x004962A8 File Offset: 0x004944A8
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionRandomTeleport
			{
				targetGroup = this.targetGroup,
				minDistance = this.minDistance,
				maxDistance = this.maxDistance,
				maxTries = this.maxTries,
				teleportDelayText = this.teleportDelayText
			};
		}

		// Token: 0x04009714 RID: 38676
		[PublicizedFrom(EAccessModifier.Protected)]
		public float minDistance = 100f;

		// Token: 0x04009715 RID: 38677
		[PublicizedFrom(EAccessModifier.Protected)]
		public float maxDistance = 200f;

		// Token: 0x04009716 RID: 38678
		[PublicizedFrom(EAccessModifier.Protected)]
		public int maxTries = 20;

		// Token: 0x04009717 RID: 38679
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMinDistance = "min_distance";

		// Token: 0x04009718 RID: 38680
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMaxDistance = "max_distance";

		// Token: 0x04009719 RID: 38681
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMaxTries = "max_tries";

		// Token: 0x0400971A RID: 38682
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3i position;
	}
}

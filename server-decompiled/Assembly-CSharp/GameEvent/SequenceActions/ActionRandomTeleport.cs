using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200199E RID: 6558
	[Preserve]
	public class ActionRandomTeleport : ActionBaseTeleport
	{
		// Token: 0x0600C8EB RID: 51435 RVA: 0x0049C508 File Offset: 0x0049A708
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

		// Token: 0x0600C8EC RID: 51436 RVA: 0x0049C5AD File Offset: 0x0049A7AD
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseFloat(ActionRandomTeleport.PropMinDistance, ref this.minDistance);
			properties.ParseFloat(ActionRandomTeleport.PropMaxDistance, ref this.maxDistance);
			properties.ParseInt(ActionRandomTeleport.PropMaxTries, ref this.maxTries);
		}

		// Token: 0x0600C8ED RID: 51437 RVA: 0x0049C5EC File Offset: 0x0049A7EC
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

		// Token: 0x0400983D RID: 38973
		[PublicizedFrom(EAccessModifier.Protected)]
		public float minDistance = 100f;

		// Token: 0x0400983E RID: 38974
		[PublicizedFrom(EAccessModifier.Protected)]
		public float maxDistance = 200f;

		// Token: 0x0400983F RID: 38975
		[PublicizedFrom(EAccessModifier.Protected)]
		public int maxTries = 20;

		// Token: 0x04009840 RID: 38976
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMinDistance = "min_distance";

		// Token: 0x04009841 RID: 38977
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMaxDistance = "max_distance";

		// Token: 0x04009842 RID: 38978
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMaxTries = "max_tries";

		// Token: 0x04009843 RID: 38979
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3i position;
	}
}

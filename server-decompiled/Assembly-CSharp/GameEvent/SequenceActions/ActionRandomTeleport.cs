using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019FB RID: 6651
	[Preserve]
	public class ActionRandomTeleport : ActionBaseTeleport
	{
		// Token: 0x0600CB57 RID: 52055 RVA: 0x004A44CC File Offset: 0x004A26CC
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

		// Token: 0x0600CB58 RID: 52056 RVA: 0x004A4571 File Offset: 0x004A2771
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseFloat(ActionRandomTeleport.PropMinDistance, ref this.minDistance);
			properties.ParseFloat(ActionRandomTeleport.PropMaxDistance, ref this.maxDistance);
			properties.ParseInt(ActionRandomTeleport.PropMaxTries, ref this.maxTries);
		}

		// Token: 0x0600CB59 RID: 52057 RVA: 0x004A45B0 File Offset: 0x004A27B0
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

		// Token: 0x040099EC RID: 39404
		[PublicizedFrom(EAccessModifier.Protected)]
		public float minDistance = 100f;

		// Token: 0x040099ED RID: 39405
		[PublicizedFrom(EAccessModifier.Protected)]
		public float maxDistance = 200f;

		// Token: 0x040099EE RID: 39406
		[PublicizedFrom(EAccessModifier.Protected)]
		public int maxTries = 20;

		// Token: 0x040099EF RID: 39407
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMinDistance = "min_distance";

		// Token: 0x040099F0 RID: 39408
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMaxDistance = "max_distance";

		// Token: 0x040099F1 RID: 39409
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMaxTries = "max_tries";

		// Token: 0x040099F2 RID: 39410
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3i position;
	}
}

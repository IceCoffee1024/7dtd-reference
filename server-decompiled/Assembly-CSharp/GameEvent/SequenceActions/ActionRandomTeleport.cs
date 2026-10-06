using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019CA RID: 6602
	[Preserve]
	public class ActionRandomTeleport : ActionBaseTeleport
	{
		// Token: 0x0600CA43 RID: 51779 RVA: 0x004A09CC File Offset: 0x0049EBCC
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

		// Token: 0x0600CA44 RID: 51780 RVA: 0x004A0A71 File Offset: 0x0049EC71
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseFloat(ActionRandomTeleport.PropMinDistance, ref this.minDistance);
			properties.ParseFloat(ActionRandomTeleport.PropMaxDistance, ref this.maxDistance);
			properties.ParseInt(ActionRandomTeleport.PropMaxTries, ref this.maxTries);
		}

		// Token: 0x0600CA45 RID: 51781 RVA: 0x004A0AB0 File Offset: 0x0049ECB0
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

		// Token: 0x04009903 RID: 39171
		[PublicizedFrom(EAccessModifier.Protected)]
		public float minDistance = 100f;

		// Token: 0x04009904 RID: 39172
		[PublicizedFrom(EAccessModifier.Protected)]
		public float maxDistance = 200f;

		// Token: 0x04009905 RID: 39173
		[PublicizedFrom(EAccessModifier.Protected)]
		public int maxTries = 20;

		// Token: 0x04009906 RID: 39174
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMinDistance = "min_distance";

		// Token: 0x04009907 RID: 39175
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMaxDistance = "max_distance";

		// Token: 0x04009908 RID: 39176
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMaxTries = "max_tries";

		// Token: 0x04009909 RID: 39177
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3i position;
	}
}

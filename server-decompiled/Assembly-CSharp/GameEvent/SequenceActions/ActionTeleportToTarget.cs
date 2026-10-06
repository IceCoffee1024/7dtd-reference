using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019FC RID: 6652
	[Preserve]
	public class ActionTeleportToTarget : ActionBaseTeleport
	{
		// Token: 0x0600CB33 RID: 52019 RVA: 0x004A4EFC File Offset: 0x004A30FC
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			if (this.targetGroup != "")
			{
				List<Entity> entityGroup = base.Owner.GetEntityGroup(this.targetGroup);
				BaseAction.ActionCompleteStates actionCompleteStates = BaseAction.ActionCompleteStates.InComplete;
				for (int i = 0; i < entityGroup.Count; i++)
				{
					actionCompleteStates = this.HandleTeleportToTarget(entityGroup[i]);
					if (actionCompleteStates == BaseAction.ActionCompleteStates.InCompleteRefund)
					{
						return actionCompleteStates;
					}
				}
				return actionCompleteStates;
			}
			return this.HandleTeleportToTarget(base.Owner.Target);
		}

		// Token: 0x0600CB34 RID: 52020 RVA: 0x004A4F68 File Offset: 0x004A3168
		[PublicizedFrom(EAccessModifier.Private)]
		public BaseAction.ActionCompleteStates HandleTeleportToTarget(Entity target)
		{
			Vector3 zero = Vector3.zero;
			Entity entity;
			if (this.teleportToGroup != "")
			{
				entity = base.Owner.GetEntityGroup(this.teleportToGroup).RandomObject<Entity>() as EntityAlive;
			}
			else
			{
				entity = base.Owner.Target;
			}
			if (entity == target)
			{
				return BaseAction.ActionCompleteStates.InComplete;
			}
			if (ActionBaseSpawn.FindValidPosition(out zero, entity, this.minDistance, this.maxDistance, this.safeSpawn, this.yOffset, this.airSpawn))
			{
				base.TeleportEntity(target, zero);
				return BaseAction.ActionCompleteStates.Complete;
			}
			return BaseAction.ActionCompleteStates.InComplete;
		}

		// Token: 0x0600CB35 RID: 52021 RVA: 0x004A4FF8 File Offset: 0x004A31F8
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseFloat(ActionTeleportToTarget.PropMinDistance, ref this.minDistance);
			properties.ParseFloat(ActionTeleportToTarget.PropMaxDistance, ref this.maxDistance);
			properties.ParseBool(ActionTeleportToTarget.PropSpawnInSafe, ref this.safeSpawn);
			properties.ParseBool(ActionTeleportToTarget.PropSpawnInAir, ref this.airSpawn);
			properties.ParseFloat(ActionTeleportToTarget.PropYOffset, ref this.yOffset);
			properties.ParseString(ActionBaseTargetAction.PropTargetGroup, ref this.targetGroup);
			properties.ParseString(ActionTeleportToTarget.PropTeleportToGroup, ref this.teleportToGroup);
		}

		// Token: 0x0600CB36 RID: 52022 RVA: 0x004A5084 File Offset: 0x004A3284
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionTeleportToTarget
			{
				minDistance = this.minDistance,
				maxDistance = this.maxDistance,
				safeSpawn = this.safeSpawn,
				airSpawn = this.airSpawn,
				yOffset = this.yOffset,
				targetGroup = this.targetGroup,
				teleportToGroup = this.teleportToGroup,
				teleportDelayText = this.teleportDelayText
			};
		}

		// Token: 0x040099E2 RID: 39394
		[PublicizedFrom(EAccessModifier.Protected)]
		public float minDistance = 8f;

		// Token: 0x040099E3 RID: 39395
		[PublicizedFrom(EAccessModifier.Protected)]
		public float maxDistance = 12f;

		// Token: 0x040099E4 RID: 39396
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool safeSpawn;

		// Token: 0x040099E5 RID: 39397
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool airSpawn;

		// Token: 0x040099E6 RID: 39398
		[PublicizedFrom(EAccessModifier.Protected)]
		public float yOffset;

		// Token: 0x040099E7 RID: 39399
		[PublicizedFrom(EAccessModifier.Protected)]
		public string teleportToGroup = "";

		// Token: 0x040099E8 RID: 39400
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionTeleportToTarget.TargetTypes targetType;

		// Token: 0x040099E9 RID: 39401
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMinDistance = "min_distance";

		// Token: 0x040099EA RID: 39402
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMaxDistance = "max_distance";

		// Token: 0x040099EB RID: 39403
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSpawnInSafe = "safe_spawn";

		// Token: 0x040099EC RID: 39404
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSpawnInAir = "air_spawn";

		// Token: 0x040099ED RID: 39405
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropYOffset = "yoffset";

		// Token: 0x040099EE RID: 39406
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTeleportToGroup = "teleport_to_group";

		// Token: 0x020019FD RID: 6653
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum TargetTypes
		{
			// Token: 0x040099F0 RID: 39408
			Target,
			// Token: 0x040099F1 RID: 39409
			TargetGroup_Random
		}
	}
}

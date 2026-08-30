using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019D0 RID: 6608
	[Preserve]
	public class ActionTeleportToTarget : ActionBaseTeleport
	{
		// Token: 0x0600C9DB RID: 51675 RVA: 0x004A0A58 File Offset: 0x0049EC58
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

		// Token: 0x0600C9DC RID: 51676 RVA: 0x004A0AC4 File Offset: 0x0049ECC4
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

		// Token: 0x0600C9DD RID: 51677 RVA: 0x004A0B54 File Offset: 0x0049ED54
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

		// Token: 0x0600C9DE RID: 51678 RVA: 0x004A0BE0 File Offset: 0x0049EDE0
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

		// Token: 0x0400991C RID: 39196
		[PublicizedFrom(EAccessModifier.Protected)]
		public float minDistance = 8f;

		// Token: 0x0400991D RID: 39197
		[PublicizedFrom(EAccessModifier.Protected)]
		public float maxDistance = 12f;

		// Token: 0x0400991E RID: 39198
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool safeSpawn;

		// Token: 0x0400991F RID: 39199
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool airSpawn;

		// Token: 0x04009920 RID: 39200
		[PublicizedFrom(EAccessModifier.Protected)]
		public float yOffset;

		// Token: 0x04009921 RID: 39201
		[PublicizedFrom(EAccessModifier.Protected)]
		public string teleportToGroup = "";

		// Token: 0x04009922 RID: 39202
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionTeleportToTarget.TargetTypes targetType;

		// Token: 0x04009923 RID: 39203
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMinDistance = "min_distance";

		// Token: 0x04009924 RID: 39204
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMaxDistance = "max_distance";

		// Token: 0x04009925 RID: 39205
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSpawnInSafe = "safe_spawn";

		// Token: 0x04009926 RID: 39206
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSpawnInAir = "air_spawn";

		// Token: 0x04009927 RID: 39207
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropYOffset = "yoffset";

		// Token: 0x04009928 RID: 39208
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTeleportToGroup = "teleport_to_group";

		// Token: 0x020019D1 RID: 6609
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum TargetTypes
		{
			// Token: 0x0400992A RID: 39210
			Target,
			// Token: 0x0400992B RID: 39211
			TargetGroup_Random
		}
	}
}

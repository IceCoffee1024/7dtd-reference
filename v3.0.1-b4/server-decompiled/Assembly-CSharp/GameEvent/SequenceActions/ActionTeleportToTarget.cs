using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019AA RID: 6570
	[Preserve]
	public class ActionTeleportToTarget : ActionBaseTeleport
	{
		// Token: 0x0600C854 RID: 51284 RVA: 0x0049A700 File Offset: 0x00498900
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

		// Token: 0x0600C855 RID: 51285 RVA: 0x0049A76C File Offset: 0x0049896C
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

		// Token: 0x0600C856 RID: 51286 RVA: 0x0049A7FC File Offset: 0x004989FC
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

		// Token: 0x0600C857 RID: 51287 RVA: 0x0049A888 File Offset: 0x00498A88
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

		// Token: 0x040097F3 RID: 38899
		[PublicizedFrom(EAccessModifier.Protected)]
		public float minDistance = 8f;

		// Token: 0x040097F4 RID: 38900
		[PublicizedFrom(EAccessModifier.Protected)]
		public float maxDistance = 12f;

		// Token: 0x040097F5 RID: 38901
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool safeSpawn;

		// Token: 0x040097F6 RID: 38902
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool airSpawn;

		// Token: 0x040097F7 RID: 38903
		[PublicizedFrom(EAccessModifier.Protected)]
		public float yOffset;

		// Token: 0x040097F8 RID: 38904
		[PublicizedFrom(EAccessModifier.Protected)]
		public string teleportToGroup = "";

		// Token: 0x040097F9 RID: 38905
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionTeleportToTarget.TargetTypes targetType;

		// Token: 0x040097FA RID: 38906
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMinDistance = "min_distance";

		// Token: 0x040097FB RID: 38907
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMaxDistance = "max_distance";

		// Token: 0x040097FC RID: 38908
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSpawnInSafe = "safe_spawn";

		// Token: 0x040097FD RID: 38909
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSpawnInAir = "air_spawn";

		// Token: 0x040097FE RID: 38910
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropYOffset = "yoffset";

		// Token: 0x040097FF RID: 38911
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTeleportToGroup = "teleport_to_group";

		// Token: 0x020019AB RID: 6571
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum TargetTypes
		{
			// Token: 0x04009801 RID: 38913
			Target,
			// Token: 0x04009802 RID: 38914
			TargetGroup_Random
		}
	}
}

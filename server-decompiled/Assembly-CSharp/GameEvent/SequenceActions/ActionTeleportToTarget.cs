using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A2D RID: 6701
	[Preserve]
	public class ActionTeleportToTarget : ActionBaseTeleport
	{
		// Token: 0x0600CC47 RID: 52295 RVA: 0x004A89F8 File Offset: 0x004A6BF8
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

		// Token: 0x0600CC48 RID: 52296 RVA: 0x004A8A64 File Offset: 0x004A6C64
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

		// Token: 0x0600CC49 RID: 52297 RVA: 0x004A8AF4 File Offset: 0x004A6CF4
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

		// Token: 0x0600CC4A RID: 52298 RVA: 0x004A8B80 File Offset: 0x004A6D80
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

		// Token: 0x04009ACB RID: 39627
		[PublicizedFrom(EAccessModifier.Protected)]
		public float minDistance = 8f;

		// Token: 0x04009ACC RID: 39628
		[PublicizedFrom(EAccessModifier.Protected)]
		public float maxDistance = 12f;

		// Token: 0x04009ACD RID: 39629
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool safeSpawn;

		// Token: 0x04009ACE RID: 39630
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool airSpawn;

		// Token: 0x04009ACF RID: 39631
		[PublicizedFrom(EAccessModifier.Protected)]
		public float yOffset;

		// Token: 0x04009AD0 RID: 39632
		[PublicizedFrom(EAccessModifier.Protected)]
		public string teleportToGroup = "";

		// Token: 0x04009AD1 RID: 39633
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionTeleportToTarget.TargetTypes targetType;

		// Token: 0x04009AD2 RID: 39634
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMinDistance = "min_distance";

		// Token: 0x04009AD3 RID: 39635
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMaxDistance = "max_distance";

		// Token: 0x04009AD4 RID: 39636
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSpawnInSafe = "safe_spawn";

		// Token: 0x04009AD5 RID: 39637
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSpawnInAir = "air_spawn";

		// Token: 0x04009AD6 RID: 39638
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropYOffset = "yoffset";

		// Token: 0x04009AD7 RID: 39639
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTeleportToGroup = "teleport_to_group";

		// Token: 0x02001A2E RID: 6702
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum TargetTypes
		{
			// Token: 0x04009AD9 RID: 39641
			Target,
			// Token: 0x04009ADA RID: 39642
			TargetGroup_Random
		}
	}
}

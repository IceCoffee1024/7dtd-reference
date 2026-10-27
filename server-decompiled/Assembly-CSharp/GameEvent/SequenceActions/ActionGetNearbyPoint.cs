using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019E6 RID: 6630
	[Preserve]
	public class ActionGetNearbyPoint : BaseAction
	{
		// Token: 0x0600CB06 RID: 51974 RVA: 0x004A2C48 File Offset: 0x004A0E48
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			Vector3 zero = Vector3.zero;
			EntityAlive entityAlive = base.Owner.Target as EntityAlive;
			if (this.targetType == ActionGetNearbyPoint.TargetTypes.TargetGroup_Random && this.targetGroup != "")
			{
				entityAlive = base.Owner.GetEntityGroup(this.targetGroup).RandomObject<Entity>() as EntityAlive;
			}
			if (ActionBaseSpawn.FindValidPosition(out zero, entityAlive, this.minDistance, this.maxDistance, this.safeSpawn, this.yOffset, this.airSpawn))
			{
				base.Owner.TargetPosition = zero;
				return BaseAction.ActionCompleteStates.Complete;
			}
			return BaseAction.ActionCompleteStates.InComplete;
		}

		// Token: 0x0600CB07 RID: 51975 RVA: 0x004A2CDC File Offset: 0x004A0EDC
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseFloat(ActionGetNearbyPoint.PropMinDistance, ref this.minDistance);
			properties.ParseFloat(ActionGetNearbyPoint.PropMaxDistance, ref this.maxDistance);
			properties.ParseBool(ActionGetNearbyPoint.PropSpawnInSafe, ref this.safeSpawn);
			properties.ParseBool(ActionGetNearbyPoint.PropSpawnInAir, ref this.airSpawn);
			properties.ParseFloat(ActionGetNearbyPoint.PropYOffset, ref this.yOffset);
			properties.ParseString(ActionGetNearbyPoint.PropTargetGroup, ref this.targetGroup);
			properties.ParseEnum<ActionGetNearbyPoint.TargetTypes>(ActionGetNearbyPoint.PropTargetType, ref this.targetType);
		}

		// Token: 0x0600CB08 RID: 51976 RVA: 0x004A2D68 File Offset: 0x004A0F68
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionGetNearbyPoint
			{
				minDistance = this.minDistance,
				maxDistance = this.maxDistance,
				safeSpawn = this.safeSpawn,
				airSpawn = this.airSpawn,
				yOffset = this.yOffset,
				targetGroup = this.targetGroup,
				targetType = this.targetType
			};
		}

		// Token: 0x04009979 RID: 39289
		[PublicizedFrom(EAccessModifier.Protected)]
		public float minDistance = 8f;

		// Token: 0x0400997A RID: 39290
		[PublicizedFrom(EAccessModifier.Protected)]
		public float maxDistance = 12f;

		// Token: 0x0400997B RID: 39291
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool safeSpawn;

		// Token: 0x0400997C RID: 39292
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool airSpawn;

		// Token: 0x0400997D RID: 39293
		[PublicizedFrom(EAccessModifier.Protected)]
		public float yOffset;

		// Token: 0x0400997E RID: 39294
		[PublicizedFrom(EAccessModifier.Protected)]
		public string targetGroup = "";

		// Token: 0x0400997F RID: 39295
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionGetNearbyPoint.TargetTypes targetType;

		// Token: 0x04009980 RID: 39296
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMinDistance = "min_distance";

		// Token: 0x04009981 RID: 39297
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMaxDistance = "max_distance";

		// Token: 0x04009982 RID: 39298
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSpawnInSafe = "safe_spawn";

		// Token: 0x04009983 RID: 39299
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSpawnInAir = "air_spawn";

		// Token: 0x04009984 RID: 39300
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropYOffset = "yoffset";

		// Token: 0x04009985 RID: 39301
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetGroup = "target_group";

		// Token: 0x04009986 RID: 39302
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetType = "target_type";

		// Token: 0x020019E7 RID: 6631
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum TargetTypes
		{
			// Token: 0x04009988 RID: 39304
			Target,
			// Token: 0x04009989 RID: 39305
			TargetGroup_Random
		}
	}
}

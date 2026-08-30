using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001963 RID: 6499
	[Preserve]
	public class ActionGetNearbyPoint : BaseAction
	{
		// Token: 0x0600C713 RID: 50963 RVA: 0x00494948 File Offset: 0x00492B48
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

		// Token: 0x0600C714 RID: 50964 RVA: 0x004949DC File Offset: 0x00492BDC
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

		// Token: 0x0600C715 RID: 50965 RVA: 0x00494A68 File Offset: 0x00492C68
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

		// Token: 0x040096A1 RID: 38561
		[PublicizedFrom(EAccessModifier.Protected)]
		public float minDistance = 8f;

		// Token: 0x040096A2 RID: 38562
		[PublicizedFrom(EAccessModifier.Protected)]
		public float maxDistance = 12f;

		// Token: 0x040096A3 RID: 38563
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool safeSpawn;

		// Token: 0x040096A4 RID: 38564
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool airSpawn;

		// Token: 0x040096A5 RID: 38565
		[PublicizedFrom(EAccessModifier.Protected)]
		public float yOffset;

		// Token: 0x040096A6 RID: 38566
		[PublicizedFrom(EAccessModifier.Protected)]
		public string targetGroup = "";

		// Token: 0x040096A7 RID: 38567
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionGetNearbyPoint.TargetTypes targetType;

		// Token: 0x040096A8 RID: 38568
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMinDistance = "min_distance";

		// Token: 0x040096A9 RID: 38569
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMaxDistance = "max_distance";

		// Token: 0x040096AA RID: 38570
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSpawnInSafe = "safe_spawn";

		// Token: 0x040096AB RID: 38571
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSpawnInAir = "air_spawn";

		// Token: 0x040096AC RID: 38572
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropYOffset = "yoffset";

		// Token: 0x040096AD RID: 38573
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetGroup = "target_group";

		// Token: 0x040096AE RID: 38574
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetType = "target_type";

		// Token: 0x02001964 RID: 6500
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum TargetTypes
		{
			// Token: 0x040096B0 RID: 38576
			Target,
			// Token: 0x040096B1 RID: 38577
			TargetGroup_Random
		}
	}
}

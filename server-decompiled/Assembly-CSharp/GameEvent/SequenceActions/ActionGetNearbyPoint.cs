using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001989 RID: 6537
	[Preserve]
	public class ActionGetNearbyPoint : BaseAction
	{
		// Token: 0x0600C89A RID: 51354 RVA: 0x0049AC84 File Offset: 0x00498E84
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

		// Token: 0x0600C89B RID: 51355 RVA: 0x0049AD18 File Offset: 0x00498F18
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

		// Token: 0x0600C89C RID: 51356 RVA: 0x0049ADA4 File Offset: 0x00498FA4
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

		// Token: 0x040097CA RID: 38858
		[PublicizedFrom(EAccessModifier.Protected)]
		public float minDistance = 8f;

		// Token: 0x040097CB RID: 38859
		[PublicizedFrom(EAccessModifier.Protected)]
		public float maxDistance = 12f;

		// Token: 0x040097CC RID: 38860
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool safeSpawn;

		// Token: 0x040097CD RID: 38861
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool airSpawn;

		// Token: 0x040097CE RID: 38862
		[PublicizedFrom(EAccessModifier.Protected)]
		public float yOffset;

		// Token: 0x040097CF RID: 38863
		[PublicizedFrom(EAccessModifier.Protected)]
		public string targetGroup = "";

		// Token: 0x040097D0 RID: 38864
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionGetNearbyPoint.TargetTypes targetType;

		// Token: 0x040097D1 RID: 38865
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMinDistance = "min_distance";

		// Token: 0x040097D2 RID: 38866
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMaxDistance = "max_distance";

		// Token: 0x040097D3 RID: 38867
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSpawnInSafe = "safe_spawn";

		// Token: 0x040097D4 RID: 38868
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSpawnInAir = "air_spawn";

		// Token: 0x040097D5 RID: 38869
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropYOffset = "yoffset";

		// Token: 0x040097D6 RID: 38870
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetGroup = "target_group";

		// Token: 0x040097D7 RID: 38871
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetType = "target_type";

		// Token: 0x0200198A RID: 6538
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum TargetTypes
		{
			// Token: 0x040097D9 RID: 38873
			Target,
			// Token: 0x040097DA RID: 38874
			TargetGroup_Random
		}
	}
}

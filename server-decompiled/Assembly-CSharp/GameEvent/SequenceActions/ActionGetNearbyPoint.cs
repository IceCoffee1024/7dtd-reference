using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019B5 RID: 6581
	[Preserve]
	public class ActionGetNearbyPoint : BaseAction
	{
		// Token: 0x0600C9F2 RID: 51698 RVA: 0x0049F148 File Offset: 0x0049D348
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

		// Token: 0x0600C9F3 RID: 51699 RVA: 0x0049F1DC File Offset: 0x0049D3DC
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

		// Token: 0x0600C9F4 RID: 51700 RVA: 0x0049F268 File Offset: 0x0049D468
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

		// Token: 0x04009890 RID: 39056
		[PublicizedFrom(EAccessModifier.Protected)]
		public float minDistance = 8f;

		// Token: 0x04009891 RID: 39057
		[PublicizedFrom(EAccessModifier.Protected)]
		public float maxDistance = 12f;

		// Token: 0x04009892 RID: 39058
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool safeSpawn;

		// Token: 0x04009893 RID: 39059
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool airSpawn;

		// Token: 0x04009894 RID: 39060
		[PublicizedFrom(EAccessModifier.Protected)]
		public float yOffset;

		// Token: 0x04009895 RID: 39061
		[PublicizedFrom(EAccessModifier.Protected)]
		public string targetGroup = "";

		// Token: 0x04009896 RID: 39062
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionGetNearbyPoint.TargetTypes targetType;

		// Token: 0x04009897 RID: 39063
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMinDistance = "min_distance";

		// Token: 0x04009898 RID: 39064
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMaxDistance = "max_distance";

		// Token: 0x04009899 RID: 39065
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSpawnInSafe = "safe_spawn";

		// Token: 0x0400989A RID: 39066
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSpawnInAir = "air_spawn";

		// Token: 0x0400989B RID: 39067
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropYOffset = "yoffset";

		// Token: 0x0400989C RID: 39068
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetGroup = "target_group";

		// Token: 0x0400989D RID: 39069
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetType = "target_type";

		// Token: 0x020019B6 RID: 6582
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum TargetTypes
		{
			// Token: 0x0400989F RID: 39071
			Target,
			// Token: 0x040098A0 RID: 39072
			TargetGroup_Random
		}
	}
}

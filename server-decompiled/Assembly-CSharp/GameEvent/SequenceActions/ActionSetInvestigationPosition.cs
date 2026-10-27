using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A16 RID: 6678
	[Preserve]
	public class ActionSetInvestigationPosition : ActionBaseTargetAction
	{
		// Token: 0x0600CBE1 RID: 52193 RVA: 0x004A70D8 File Offset: 0x004A52D8
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			EntityAlive entityAlive = target as EntityAlive;
			if (entityAlive != null)
			{
				entityAlive.SetInvestigatePosition(base.Owner.TargetPosition, (int)(this.investigateTime * 20f), this.isAlert);
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600CBE2 RID: 52194 RVA: 0x004A711A File Offset: 0x004A531A
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseFloat(ActionSetInvestigationPosition.PropTime, ref this.investigateTime);
			properties.ParseBool(ActionSetInvestigationPosition.PropIsAlert, ref this.isAlert);
		}

		// Token: 0x0600CBE3 RID: 52195 RVA: 0x004A7145 File Offset: 0x004A5345
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionSetInvestigationPosition
			{
				investigateTime = this.investigateTime,
				isAlert = this.isAlert,
				targetGroup = this.targetGroup
			};
		}

		// Token: 0x04009A6F RID: 39535
		[PublicizedFrom(EAccessModifier.Protected)]
		public float investigateTime;

		// Token: 0x04009A70 RID: 39536
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool isAlert;

		// Token: 0x04009A71 RID: 39537
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTime = "time";

		// Token: 0x04009A72 RID: 39538
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropIsAlert = "is_alert";
	}
}

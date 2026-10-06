using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019E5 RID: 6629
	[Preserve]
	public class ActionSetInvestigationPosition : ActionBaseTargetAction
	{
		// Token: 0x0600CACD RID: 51917 RVA: 0x004A35D8 File Offset: 0x004A17D8
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			EntityAlive entityAlive = target as EntityAlive;
			if (entityAlive != null)
			{
				entityAlive.SetInvestigatePosition(base.Owner.TargetPosition, (int)(this.investigateTime * 20f), this.isAlert);
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600CACE RID: 51918 RVA: 0x004A361A File Offset: 0x004A181A
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseFloat(ActionSetInvestigationPosition.PropTime, ref this.investigateTime);
			properties.ParseBool(ActionSetInvestigationPosition.PropIsAlert, ref this.isAlert);
		}

		// Token: 0x0600CACF RID: 51919 RVA: 0x004A3645 File Offset: 0x004A1845
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

		// Token: 0x04009986 RID: 39302
		[PublicizedFrom(EAccessModifier.Protected)]
		public float investigateTime;

		// Token: 0x04009987 RID: 39303
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool isAlert;

		// Token: 0x04009988 RID: 39304
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTime = "time";

		// Token: 0x04009989 RID: 39305
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropIsAlert = "is_alert";
	}
}

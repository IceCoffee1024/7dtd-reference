using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001993 RID: 6547
	[Preserve]
	public class ActionSetInvestigationPosition : ActionBaseTargetAction
	{
		// Token: 0x0600C7EE RID: 51182 RVA: 0x00498D90 File Offset: 0x00496F90
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			EntityAlive entityAlive = target as EntityAlive;
			if (entityAlive != null)
			{
				entityAlive.SetInvestigatePosition(base.Owner.TargetPosition, (int)(this.investigateTime * 20f), this.isAlert);
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C7EF RID: 51183 RVA: 0x00498DD2 File Offset: 0x00496FD2
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseFloat(ActionSetInvestigationPosition.PropTime, ref this.investigateTime);
			properties.ParseBool(ActionSetInvestigationPosition.PropIsAlert, ref this.isAlert);
		}

		// Token: 0x0600C7F0 RID: 51184 RVA: 0x00498DFD File Offset: 0x00496FFD
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

		// Token: 0x04009797 RID: 38807
		[PublicizedFrom(EAccessModifier.Protected)]
		public float investigateTime;

		// Token: 0x04009798 RID: 38808
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool isAlert;

		// Token: 0x04009799 RID: 38809
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTime = "time";

		// Token: 0x0400979A RID: 38810
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropIsAlert = "is_alert";
	}
}

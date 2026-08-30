using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019B9 RID: 6585
	[Preserve]
	public class ActionSetInvestigationPosition : ActionBaseTargetAction
	{
		// Token: 0x0600C975 RID: 51573 RVA: 0x0049F0E8 File Offset: 0x0049D2E8
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			EntityAlive entityAlive = target as EntityAlive;
			if (entityAlive != null)
			{
				entityAlive.SetInvestigatePosition(base.Owner.TargetPosition, (int)(this.investigateTime * 20f), this.isAlert);
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C976 RID: 51574 RVA: 0x0049F12A File Offset: 0x0049D32A
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseFloat(ActionSetInvestigationPosition.PropTime, ref this.investigateTime);
			properties.ParseBool(ActionSetInvestigationPosition.PropIsAlert, ref this.isAlert);
		}

		// Token: 0x0600C977 RID: 51575 RVA: 0x0049F155 File Offset: 0x0049D355
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

		// Token: 0x040098C0 RID: 39104
		[PublicizedFrom(EAccessModifier.Protected)]
		public float investigateTime;

		// Token: 0x040098C1 RID: 39105
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool isAlert;

		// Token: 0x040098C2 RID: 39106
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTime = "time";

		// Token: 0x040098C3 RID: 39107
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropIsAlert = "is_alert";
	}
}

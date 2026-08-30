using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A0A RID: 6666
	[Preserve]
	public class WaitWhile : BaseWait
	{
		// Token: 0x0600CAF2 RID: 51954 RVA: 0x004A53B8 File Offset: 0x004A35B8
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			if (this.Requirements != null)
			{
				BaseWait.ConditionTypes conditionType = this.ConditionType;
				if (conditionType == BaseWait.ConditionTypes.Any)
				{
					for (int i = 0; i < this.Requirements.Count; i++)
					{
						if (this.Requirements[i].CanPerform(base.Owner.Target))
						{
							return BaseAction.ActionCompleteStates.InComplete;
						}
					}
					return BaseAction.ActionCompleteStates.Complete;
				}
				if (conditionType == BaseWait.ConditionTypes.All)
				{
					for (int j = 0; j < this.Requirements.Count; j++)
					{
						if (!this.Requirements[j].CanPerform(base.Owner.Target))
						{
							return BaseAction.ActionCompleteStates.Complete;
						}
					}
					return BaseAction.ActionCompleteStates.InComplete;
				}
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600CAF3 RID: 51955 RVA: 0x004A5451 File Offset: 0x004A3651
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new WaitWhile
			{
				ConditionType = this.ConditionType
			};
		}
	}
}

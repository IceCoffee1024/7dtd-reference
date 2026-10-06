using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A36 RID: 6710
	[Preserve]
	public class WaitWhile : BaseWait
	{
		// Token: 0x0600CC4A RID: 52298 RVA: 0x004A9884 File Offset: 0x004A7A84
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

		// Token: 0x0600CC4B RID: 52299 RVA: 0x004A991D File Offset: 0x004A7B1D
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

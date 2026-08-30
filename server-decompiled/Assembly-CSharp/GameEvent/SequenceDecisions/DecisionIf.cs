using System;
using GameEvent.SequenceActions;
using UnityEngine.Scripting;

namespace GameEvent.SequenceDecisions
{
	// Token: 0x02001954 RID: 6484
	[Preserve]
	public class DecisionIf : BaseDecision
	{
		// Token: 0x0600C7B2 RID: 51122 RVA: 0x00493A5D File Offset: 0x00491C5D
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			if (!this.runActions)
			{
				this.runActions = this.CheckCondition();
			}
			if (!this.runActions)
			{
				return BaseAction.ActionCompleteStates.Complete;
			}
			if (base.HandleActions() == BaseAction.ActionCompleteStates.Complete)
			{
				this.runActions = false;
				return BaseAction.ActionCompleteStates.Complete;
			}
			return BaseAction.ActionCompleteStates.InComplete;
		}

		// Token: 0x0600C7B3 RID: 51123 RVA: 0x00493A90 File Offset: 0x00491C90
		[PublicizedFrom(EAccessModifier.Private)]
		public bool CheckCondition()
		{
			if (this.Requirements != null)
			{
				DecisionIf.ConditionTypes conditionType = this.ConditionType;
				if (conditionType == DecisionIf.ConditionTypes.Any)
				{
					bool flag = false;
					for (int i = 0; i < this.Requirements.Count; i++)
					{
						if (this.Requirements[i].CanPerform(base.Owner.Target))
						{
							flag = true;
							break;
						}
					}
					return flag;
				}
				if (conditionType == DecisionIf.ConditionTypes.All)
				{
					for (int j = 0; j < this.Requirements.Count; j++)
					{
						if (!this.Requirements[j].CanPerform(base.Owner.Target))
						{
							return false;
						}
					}
				}
			}
			return true;
		}

		// Token: 0x0600C7B4 RID: 51124 RVA: 0x00493B30 File Offset: 0x00491D30
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<DecisionIf.ConditionTypes>(DecisionIf.PropConditionType, ref this.ConditionType);
		}

		// Token: 0x0600C7B5 RID: 51125 RVA: 0x00493B4A File Offset: 0x00491D4A
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new DecisionIf
			{
				ConditionType = this.ConditionType
			};
		}

		// Token: 0x0400965D RID: 38493
		public DecisionIf.ConditionTypes ConditionType = DecisionIf.ConditionTypes.All;

		// Token: 0x0400965E RID: 38494
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropConditionType = "condition_type";

		// Token: 0x0400965F RID: 38495
		[PublicizedFrom(EAccessModifier.Private)]
		public bool runActions;

		// Token: 0x02001955 RID: 6485
		public enum ConditionTypes
		{
			// Token: 0x04009661 RID: 38497
			Any,
			// Token: 0x04009662 RID: 38498
			All
		}
	}
}

using System;
using GameEvent.SequenceActions;
using UnityEngine.Scripting;

namespace GameEvent.SequenceDecisions
{
	// Token: 0x020019B1 RID: 6577
	[Preserve]
	public class DecisionIf : BaseDecision
	{
		// Token: 0x0600CA1E RID: 51742 RVA: 0x0049B93D File Offset: 0x00499B3D
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

		// Token: 0x0600CA1F RID: 51743 RVA: 0x0049B970 File Offset: 0x00499B70
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

		// Token: 0x0600CA20 RID: 51744 RVA: 0x0049BA10 File Offset: 0x00499C10
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<DecisionIf.ConditionTypes>(DecisionIf.PropConditionType, ref this.ConditionType);
		}

		// Token: 0x0600CA21 RID: 51745 RVA: 0x0049BA2A File Offset: 0x00499C2A
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new DecisionIf
			{
				ConditionType = this.ConditionType
			};
		}

		// Token: 0x0400980C RID: 38924
		public DecisionIf.ConditionTypes ConditionType = DecisionIf.ConditionTypes.All;

		// Token: 0x0400980D RID: 38925
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropConditionType = "condition_type";

		// Token: 0x0400980E RID: 38926
		[PublicizedFrom(EAccessModifier.Private)]
		public bool runActions;

		// Token: 0x020019B2 RID: 6578
		public enum ConditionTypes
		{
			// Token: 0x04009810 RID: 38928
			Any,
			// Token: 0x04009811 RID: 38929
			All
		}
	}
}

using System;
using GameEvent.SequenceActions;
using UnityEngine.Scripting;

namespace GameEvent.SequenceDecisions
{
	// Token: 0x02001980 RID: 6528
	[Preserve]
	public class DecisionIf : BaseDecision
	{
		// Token: 0x0600C90A RID: 51466 RVA: 0x00497F49 File Offset: 0x00496149
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

		// Token: 0x0600C90B RID: 51467 RVA: 0x00497F7C File Offset: 0x0049617C
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

		// Token: 0x0600C90C RID: 51468 RVA: 0x0049801C File Offset: 0x0049621C
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<DecisionIf.ConditionTypes>(DecisionIf.PropConditionType, ref this.ConditionType);
		}

		// Token: 0x0600C90D RID: 51469 RVA: 0x00498036 File Offset: 0x00496236
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new DecisionIf
			{
				ConditionType = this.ConditionType
			};
		}

		// Token: 0x04009723 RID: 38691
		public DecisionIf.ConditionTypes ConditionType = DecisionIf.ConditionTypes.All;

		// Token: 0x04009724 RID: 38692
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropConditionType = "condition_type";

		// Token: 0x04009725 RID: 38693
		[PublicizedFrom(EAccessModifier.Private)]
		public bool runActions;

		// Token: 0x02001981 RID: 6529
		public enum ConditionTypes
		{
			// Token: 0x04009727 RID: 38695
			Any,
			// Token: 0x04009728 RID: 38696
			All
		}
	}
}

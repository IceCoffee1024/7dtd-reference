using System;
using GameEvent.SequenceActions;
using UnityEngine.Scripting;

namespace GameEvent.SequenceLoops
{
	// Token: 0x02001926 RID: 6438
	[Preserve]
	public class LoopWhile : BaseLoop
	{
		// Token: 0x17001829 RID: 6185
		// (get) Token: 0x0600C608 RID: 50696 RVA: 0x00010E62 File Offset: 0x0000F062
		public override bool UseRequirements
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600C609 RID: 50697 RVA: 0x0048C304 File Offset: 0x0048A504
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			if (!this.runLoop)
			{
				this.runLoop = this.CheckCondition();
			}
			if (this.runLoop)
			{
				if (base.HandleActions() == BaseAction.ActionCompleteStates.Complete)
				{
					this.CurrentPhase = 0;
					for (int i = 0; i < this.Actions.Count; i++)
					{
						this.Actions[i].Reset();
					}
					this.runLoop = false;
				}
				return BaseAction.ActionCompleteStates.InComplete;
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C60A RID: 50698 RVA: 0x0048C370 File Offset: 0x0048A570
		[PublicizedFrom(EAccessModifier.Private)]
		public bool CheckCondition()
		{
			if (this.Requirements != null)
			{
				LoopWhile.ConditionTypes conditionType = this.ConditionType;
				if (conditionType == LoopWhile.ConditionTypes.Any)
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
				if (conditionType == LoopWhile.ConditionTypes.All)
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

		// Token: 0x0600C60B RID: 50699 RVA: 0x0048C410 File Offset: 0x0048A610
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<LoopWhile.ConditionTypes>(LoopWhile.PropConditionType, ref this.ConditionType);
		}

		// Token: 0x0600C60C RID: 50700 RVA: 0x0048C42A File Offset: 0x0048A62A
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new LoopWhile
			{
				ConditionType = this.ConditionType
			};
		}

		// Token: 0x04009508 RID: 38152
		public LoopWhile.ConditionTypes ConditionType = LoopWhile.ConditionTypes.All;

		// Token: 0x04009509 RID: 38153
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropConditionType = "condition_type";

		// Token: 0x0400950A RID: 38154
		[PublicizedFrom(EAccessModifier.Private)]
		public bool runLoop;

		// Token: 0x02001927 RID: 6439
		public enum ConditionTypes
		{
			// Token: 0x0400950C RID: 38156
			Any,
			// Token: 0x0400950D RID: 38157
			All
		}
	}
}

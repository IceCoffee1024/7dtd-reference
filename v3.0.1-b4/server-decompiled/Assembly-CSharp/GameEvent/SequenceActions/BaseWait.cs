using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019E0 RID: 6624
	[Preserve]
	public class BaseWait : BaseAction
	{
		// Token: 0x1700184E RID: 6222
		// (get) Token: 0x0600C960 RID: 51552 RVA: 0x00010E62 File Offset: 0x0000F062
		public override bool UseRequirements
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600C961 RID: 51553 RVA: 0x0049EE2C File Offset: 0x0049D02C
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

		// Token: 0x0600C962 RID: 51554 RVA: 0x0049EEC5 File Offset: 0x0049D0C5
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<BaseWait.ConditionTypes>(BaseWait.PropConditionType, ref this.ConditionType);
		}

		// Token: 0x0600C963 RID: 51555 RVA: 0x0049EEDF File Offset: 0x0049D0DF
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new BaseWait
			{
				ConditionType = this.ConditionType
			};
		}

		// Token: 0x040098ED RID: 39149
		public BaseWait.ConditionTypes ConditionType = BaseWait.ConditionTypes.All;

		// Token: 0x040098EE RID: 39150
		public static string PropConditionType = "condition_type";

		// Token: 0x020019E1 RID: 6625
		public enum ConditionTypes
		{
			// Token: 0x040098F0 RID: 39152
			Any,
			// Token: 0x040098F1 RID: 39153
			All
		}
	}
}

using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A64 RID: 6756
	[Preserve]
	public class BaseWait : BaseAction
	{
		// Token: 0x170019AA RID: 6570
		// (get) Token: 0x0600CD55 RID: 52565 RVA: 0x00010E62 File Offset: 0x0000F062
		public override bool UseRequirements
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600CD56 RID: 52566 RVA: 0x004AD1E8 File Offset: 0x004AB3E8
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

		// Token: 0x0600CD57 RID: 52567 RVA: 0x004AD281 File Offset: 0x004AB481
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<BaseWait.ConditionTypes>(BaseWait.PropConditionType, ref this.ConditionType);
		}

		// Token: 0x0600CD58 RID: 52568 RVA: 0x004AD29B File Offset: 0x004AB49B
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new BaseWait
			{
				ConditionType = this.ConditionType
			};
		}

		// Token: 0x04009BCD RID: 39885
		public BaseWait.ConditionTypes ConditionType = BaseWait.ConditionTypes.All;

		// Token: 0x04009BCE RID: 39886
		public static string PropConditionType = "condition_type";

		// Token: 0x02001A65 RID: 6757
		public enum ConditionTypes
		{
			// Token: 0x04009BD0 RID: 39888
			Any,
			// Token: 0x04009BD1 RID: 39889
			All
		}
	}
}

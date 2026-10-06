using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A33 RID: 6707
	[Preserve]
	public class BaseWait : BaseAction
	{
		// Token: 0x17001962 RID: 6498
		// (get) Token: 0x0600CC41 RID: 52289 RVA: 0x00010E62 File Offset: 0x0000F062
		public override bool UseRequirements
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600CC42 RID: 52290 RVA: 0x004A96EC File Offset: 0x004A78EC
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

		// Token: 0x0600CC43 RID: 52291 RVA: 0x004A9785 File Offset: 0x004A7985
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<BaseWait.ConditionTypes>(BaseWait.PropConditionType, ref this.ConditionType);
		}

		// Token: 0x0600CC44 RID: 52292 RVA: 0x004A979F File Offset: 0x004A799F
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new BaseWait
			{
				ConditionType = this.ConditionType
			};
		}

		// Token: 0x04009AE4 RID: 39652
		public BaseWait.ConditionTypes ConditionType = BaseWait.ConditionTypes.All;

		// Token: 0x04009AE5 RID: 39653
		public static string PropConditionType = "condition_type";

		// Token: 0x02001A34 RID: 6708
		public enum ConditionTypes
		{
			// Token: 0x04009AE7 RID: 39655
			Any,
			// Token: 0x04009AE8 RID: 39656
			All
		}
	}
}

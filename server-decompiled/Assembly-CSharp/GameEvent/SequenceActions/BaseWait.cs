using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A07 RID: 6663
	[Preserve]
	public class BaseWait : BaseAction
	{
		// Token: 0x170018C0 RID: 6336
		// (get) Token: 0x0600CAE9 RID: 51945 RVA: 0x00010E62 File Offset: 0x0000F062
		public override bool UseRequirements
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600CAEA RID: 51946 RVA: 0x004A5220 File Offset: 0x004A3420
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

		// Token: 0x0600CAEB RID: 51947 RVA: 0x004A52B9 File Offset: 0x004A34B9
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<BaseWait.ConditionTypes>(BaseWait.PropConditionType, ref this.ConditionType);
		}

		// Token: 0x0600CAEC RID: 51948 RVA: 0x004A52D3 File Offset: 0x004A34D3
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new BaseWait
			{
				ConditionType = this.ConditionType
			};
		}

		// Token: 0x04009A1E RID: 39454
		public BaseWait.ConditionTypes ConditionType = BaseWait.ConditionTypes.All;

		// Token: 0x04009A1F RID: 39455
		public static string PropConditionType = "condition_type";

		// Token: 0x02001A08 RID: 6664
		public enum ConditionTypes
		{
			// Token: 0x04009A21 RID: 39457
			Any,
			// Token: 0x04009A22 RID: 39458
			All
		}
	}
}

using System;
using GameEvent.SequenceActions;
using UnityEngine.Scripting;

namespace GameEvent.SequenceLoops
{
	// Token: 0x020019A9 RID: 6569
	[Preserve]
	public class LoopWhile : BaseLoop
	{
		// Token: 0x17001985 RID: 6533
		// (get) Token: 0x0600C9FB RID: 51707 RVA: 0x00010E62 File Offset: 0x0000F062
		public override bool UseRequirements
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600C9FC RID: 51708 RVA: 0x0049A530 File Offset: 0x00498730
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

		// Token: 0x0600C9FD RID: 51709 RVA: 0x0049A59C File Offset: 0x0049879C
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

		// Token: 0x0600C9FE RID: 51710 RVA: 0x0049A63C File Offset: 0x0049883C
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<LoopWhile.ConditionTypes>(LoopWhile.PropConditionType, ref this.ConditionType);
		}

		// Token: 0x0600C9FF RID: 51711 RVA: 0x0049A656 File Offset: 0x00498856
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new LoopWhile
			{
				ConditionType = this.ConditionType
			};
		}

		// Token: 0x040097E0 RID: 38880
		public LoopWhile.ConditionTypes ConditionType = LoopWhile.ConditionTypes.All;

		// Token: 0x040097E1 RID: 38881
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropConditionType = "condition_type";

		// Token: 0x040097E2 RID: 38882
		[PublicizedFrom(EAccessModifier.Private)]
		public bool runLoop;

		// Token: 0x020019AA RID: 6570
		public enum ConditionTypes
		{
			// Token: 0x040097E4 RID: 38884
			Any,
			// Token: 0x040097E5 RID: 38885
			All
		}
	}
}

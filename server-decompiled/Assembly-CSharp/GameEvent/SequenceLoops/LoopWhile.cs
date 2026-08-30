using System;
using GameEvent.SequenceActions;
using UnityEngine.Scripting;

namespace GameEvent.SequenceLoops
{
	// Token: 0x0200194C RID: 6476
	[Preserve]
	public class LoopWhile : BaseLoop
	{
		// Token: 0x1700189B RID: 6299
		// (get) Token: 0x0600C78F RID: 51087 RVA: 0x00010E62 File Offset: 0x0000F062
		public override bool UseRequirements
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600C790 RID: 51088 RVA: 0x00492650 File Offset: 0x00490850
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

		// Token: 0x0600C791 RID: 51089 RVA: 0x004926BC File Offset: 0x004908BC
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

		// Token: 0x0600C792 RID: 51090 RVA: 0x0049275C File Offset: 0x0049095C
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<LoopWhile.ConditionTypes>(LoopWhile.PropConditionType, ref this.ConditionType);
		}

		// Token: 0x0600C793 RID: 51091 RVA: 0x00492776 File Offset: 0x00490976
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new LoopWhile
			{
				ConditionType = this.ConditionType
			};
		}

		// Token: 0x04009631 RID: 38449
		public LoopWhile.ConditionTypes ConditionType = LoopWhile.ConditionTypes.All;

		// Token: 0x04009632 RID: 38450
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropConditionType = "condition_type";

		// Token: 0x04009633 RID: 38451
		[PublicizedFrom(EAccessModifier.Private)]
		public bool runLoop;

		// Token: 0x0200194D RID: 6477
		public enum ConditionTypes
		{
			// Token: 0x04009635 RID: 38453
			Any,
			// Token: 0x04009636 RID: 38454
			All
		}
	}
}

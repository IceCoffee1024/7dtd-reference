using System;
using GameEvent.SequenceActions;
using UnityEngine.Scripting;

namespace GameEvent.SequenceLoops
{
	// Token: 0x02001978 RID: 6520
	[Preserve]
	public class LoopWhile : BaseLoop
	{
		// Token: 0x1700193D RID: 6461
		// (get) Token: 0x0600C8E7 RID: 51431 RVA: 0x00010E62 File Offset: 0x0000F062
		public override bool UseRequirements
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600C8E8 RID: 51432 RVA: 0x00496B3C File Offset: 0x00494D3C
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

		// Token: 0x0600C8E9 RID: 51433 RVA: 0x00496BA8 File Offset: 0x00494DA8
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

		// Token: 0x0600C8EA RID: 51434 RVA: 0x00496C48 File Offset: 0x00494E48
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<LoopWhile.ConditionTypes>(LoopWhile.PropConditionType, ref this.ConditionType);
		}

		// Token: 0x0600C8EB RID: 51435 RVA: 0x00496C62 File Offset: 0x00494E62
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new LoopWhile
			{
				ConditionType = this.ConditionType
			};
		}

		// Token: 0x040096F7 RID: 38647
		public LoopWhile.ConditionTypes ConditionType = LoopWhile.ConditionTypes.All;

		// Token: 0x040096F8 RID: 38648
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropConditionType = "condition_type";

		// Token: 0x040096F9 RID: 38649
		[PublicizedFrom(EAccessModifier.Private)]
		public bool runLoop;

		// Token: 0x02001979 RID: 6521
		public enum ConditionTypes
		{
			// Token: 0x040096FB RID: 38651
			Any,
			// Token: 0x040096FC RID: 38652
			All
		}
	}
}

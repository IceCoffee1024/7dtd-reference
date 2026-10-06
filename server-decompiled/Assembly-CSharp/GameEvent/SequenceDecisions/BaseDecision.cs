using System;
using System.Collections.Generic;
using GameEvent.SequenceActions;
using UnityEngine.Scripting;

namespace GameEvent.SequenceDecisions
{
	// Token: 0x0200197F RID: 6527
	[Preserve]
	public class BaseDecision : BaseAction
	{
		// Token: 0x1700193F RID: 6463
		// (get) Token: 0x0600C902 RID: 51458 RVA: 0x00010E62 File Offset: 0x0000F062
		public override bool UseRequirements
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600C903 RID: 51459 RVA: 0x00497BA0 File Offset: 0x00495DA0
		public override void SetActionKeyData(int _actionIndex, BaseAction _parent, string prefix = "")
		{
			base.SetActionKeyData(_actionIndex, _parent, prefix);
			for (int i = 0; i < this.Actions.Count; i++)
			{
				this.Actions[i].SetActionKeyData(i, this, "");
			}
		}

		// Token: 0x0600C904 RID: 51460 RVA: 0x00497BE4 File Offset: 0x00495DE4
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
			base.OnInit();
			List<int> list = new List<int>();
			for (int i = 0; i < this.Actions.Count; i++)
			{
				if (!list.Contains(this.Actions[i].Phase))
				{
					list.Add(this.Actions[i].Phase);
				}
			}
			list.Sort();
			if (list.Count > 0)
			{
				this.phaseMax = list[list.Count - 1] + 1;
				return;
			}
			this.phaseMax = 0;
		}

		// Token: 0x0600C905 RID: 51461 RVA: 0x00497C70 File Offset: 0x00495E70
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnReset()
		{
			base.OnReset();
			for (int i = 0; i < this.Actions.Count; i++)
			{
				this.Actions[i].Reset();
			}
			this.currentPhase = 0;
		}

		// Token: 0x0600C906 RID: 51462 RVA: 0x00497CB4 File Offset: 0x00495EB4
		[PublicizedFrom(EAccessModifier.Protected)]
		public BaseAction.ActionCompleteStates HandleActions()
		{
			bool flag = false;
			int num = this.currentPhase;
			for (int i = 0; i < this.Actions.Count; i++)
			{
				if (this.Actions[i].Phase == this.currentPhase && !this.Actions[i].IsComplete)
				{
					flag = true;
					this.Actions[i].Owner = base.Owner;
					BaseAction.ActionCompleteStates actionCompleteStates = this.Actions[i].PerformAction();
					if (actionCompleteStates == BaseAction.ActionCompleteStates.Complete || (actionCompleteStates == BaseAction.ActionCompleteStates.InCompleteRefund && this.Actions[i].IgnoreRefund))
					{
						this.Actions[i].IsComplete = true;
						if (this.Actions[i].PhaseOnComplete != -1)
						{
							num = this.Actions[i].PhaseOnComplete;
						}
					}
					else if (actionCompleteStates == BaseAction.ActionCompleteStates.RequirementsNotMet)
					{
						this.Actions[i].IsComplete = true;
						if (this.Actions[i].PhaseOnDenied != -1)
						{
							num = this.Actions[i].PhaseOnDenied;
						}
					}
					else if (base.Owner.AllowRefunds && actionCompleteStates == BaseAction.ActionCompleteStates.InCompleteRefund)
					{
						return BaseAction.ActionCompleteStates.InCompleteRefund;
					}
				}
			}
			if (!flag)
			{
				this.currentPhase++;
			}
			else if (this.currentPhase != num)
			{
				this.currentPhase = num;
				for (int j = 0; j < this.Actions.Count; j++)
				{
					if (this.Actions[j].Phase >= this.currentPhase)
					{
						this.Actions[j].Reset();
					}
				}
			}
			if (this.currentPhase >= this.phaseMax)
			{
				this.IsComplete = true;
				return BaseAction.ActionCompleteStates.Complete;
			}
			return BaseAction.ActionCompleteStates.InComplete;
		}

		// Token: 0x0600C907 RID: 51463 RVA: 0x00497E6C File Offset: 0x0049606C
		public override void HandleTemplateInit(GameEventActionSequence seq)
		{
			base.HandleTemplateInit(seq);
			for (int i = 0; i < this.Actions.Count; i++)
			{
				seq.HandleVariablesForProperties(this.Actions[i].Properties);
				this.Actions[i].ParseProperties(this.Actions[i].Properties);
			}
		}

		// Token: 0x0600C908 RID: 51464 RVA: 0x00497ED0 File Offset: 0x004960D0
		public override BaseAction Clone()
		{
			BaseDecision baseDecision = (BaseDecision)base.Clone();
			for (int i = 0; i < this.Actions.Count; i++)
			{
				BaseAction baseAction = this.Actions[i].Clone();
				baseAction.Owner = base.Owner;
				baseDecision.Actions.Add(baseAction);
			}
			baseDecision.phaseMax = this.phaseMax;
			return baseDecision;
		}

		// Token: 0x04009720 RID: 38688
		public List<BaseAction> Actions = new List<BaseAction>();

		// Token: 0x04009721 RID: 38689
		[PublicizedFrom(EAccessModifier.Protected)]
		public int currentPhase;

		// Token: 0x04009722 RID: 38690
		[PublicizedFrom(EAccessModifier.Protected)]
		public int phaseMax;
	}
}

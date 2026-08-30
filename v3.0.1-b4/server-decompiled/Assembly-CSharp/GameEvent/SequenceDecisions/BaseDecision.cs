using System;
using System.Collections.Generic;
using GameEvent.SequenceActions;
using UnityEngine.Scripting;

namespace GameEvent.SequenceDecisions
{
	// Token: 0x0200192D RID: 6445
	[Preserve]
	public class BaseDecision : BaseAction
	{
		// Token: 0x1700182B RID: 6187
		// (get) Token: 0x0600C623 RID: 50723 RVA: 0x00010E62 File Offset: 0x0000F062
		public override bool UseRequirements
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600C624 RID: 50724 RVA: 0x0048D368 File Offset: 0x0048B568
		public override void SetActionKeyData(int _actionIndex, BaseAction _parent, string prefix = "")
		{
			base.SetActionKeyData(_actionIndex, _parent, prefix);
			for (int i = 0; i < this.Actions.Count; i++)
			{
				this.Actions[i].SetActionKeyData(i, this, "");
			}
		}

		// Token: 0x0600C625 RID: 50725 RVA: 0x0048D3AC File Offset: 0x0048B5AC
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

		// Token: 0x0600C626 RID: 50726 RVA: 0x0048D438 File Offset: 0x0048B638
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

		// Token: 0x0600C627 RID: 50727 RVA: 0x0048D47C File Offset: 0x0048B67C
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

		// Token: 0x0600C628 RID: 50728 RVA: 0x0048D634 File Offset: 0x0048B834
		public override void HandleTemplateInit(GameEventActionSequence seq)
		{
			base.HandleTemplateInit(seq);
			for (int i = 0; i < this.Actions.Count; i++)
			{
				seq.HandleVariablesForProperties(this.Actions[i].Properties);
				this.Actions[i].ParseProperties(this.Actions[i].Properties);
			}
		}

		// Token: 0x0600C629 RID: 50729 RVA: 0x0048D698 File Offset: 0x0048B898
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

		// Token: 0x04009531 RID: 38193
		public List<BaseAction> Actions = new List<BaseAction>();

		// Token: 0x04009532 RID: 38194
		[PublicizedFrom(EAccessModifier.Protected)]
		public int currentPhase;

		// Token: 0x04009533 RID: 38195
		[PublicizedFrom(EAccessModifier.Protected)]
		public int phaseMax;
	}
}

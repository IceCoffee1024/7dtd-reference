using System;
using System.Collections.Generic;
using GameEvent.SequenceActions;
using UnityEngine.Scripting;

namespace GameEvent.SequenceLoops
{
	// Token: 0x020019A7 RID: 6567
	[Preserve]
	public class BaseLoop : BaseAction
	{
		// Token: 0x0600C9EE RID: 51694 RVA: 0x0049A0B4 File Offset: 0x004982B4
		public override void SetActionKeyData(int _actionIndex, BaseAction _parent, string prefix = "")
		{
			base.SetActionKeyData(_actionIndex, _parent, prefix);
			for (int i = 0; i < this.Actions.Count; i++)
			{
				this.Actions[i].SetActionKeyData(i, this, "");
			}
		}

		// Token: 0x0600C9EF RID: 51695 RVA: 0x0049A0F8 File Offset: 0x004982F8
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
				this.PhaseMax = list[list.Count - 1] + 1;
				return;
			}
			this.PhaseMax = 0;
		}

		// Token: 0x0600C9F0 RID: 51696 RVA: 0x0049A184 File Offset: 0x00498384
		[PublicizedFrom(EAccessModifier.Protected)]
		public BaseAction.ActionCompleteStates HandleActions()
		{
			bool flag = false;
			int num = this.CurrentPhase;
			for (int i = 0; i < this.Actions.Count; i++)
			{
				if (this.Actions[i].Phase == this.CurrentPhase && !this.Actions[i].IsComplete)
				{
					BaseAction.ActionCompleteStates actionCompleteStates = BaseAction.ActionCompleteStates.InComplete;
					flag = true;
					if (!this.Actions[i].IsComplete)
					{
						this.Actions[i].Owner = base.Owner;
						actionCompleteStates = this.Actions[i].PerformAction();
					}
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
				this.CurrentPhase++;
			}
			else if (this.CurrentPhase != num)
			{
				this.CurrentPhase = num;
				for (int j = 0; j < this.Actions.Count; j++)
				{
					if (this.Actions[j].Phase >= this.CurrentPhase)
					{
						this.Actions[j].Reset();
					}
				}
			}
			if (this.CurrentPhase >= this.PhaseMax)
			{
				return BaseAction.ActionCompleteStates.Complete;
			}
			return BaseAction.ActionCompleteStates.InComplete;
		}

		// Token: 0x0600C9F1 RID: 51697 RVA: 0x0049A348 File Offset: 0x00498548
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnReset()
		{
			base.OnReset();
			this.CurrentPhase = 0;
			for (int i = 0; i < this.Actions.Count; i++)
			{
				this.Actions[i].Reset();
			}
		}

		// Token: 0x0600C9F2 RID: 51698 RVA: 0x0049A38C File Offset: 0x0049858C
		public override void HandleTemplateInit(GameEventActionSequence seq)
		{
			base.HandleTemplateInit(seq);
			for (int i = 0; i < this.Actions.Count; i++)
			{
				this.Actions[i].HandleTemplateInit(seq);
			}
		}

		// Token: 0x0600C9F3 RID: 51699 RVA: 0x0049A3C8 File Offset: 0x004985C8
		public override BaseAction Clone()
		{
			BaseLoop baseLoop = (BaseLoop)base.Clone();
			for (int i = 0; i < this.Actions.Count; i++)
			{
				baseLoop.Actions.Add(this.Actions[i].Clone());
			}
			baseLoop.PhaseMax = this.PhaseMax;
			return baseLoop;
		}

		// Token: 0x040097D9 RID: 38873
		public List<BaseAction> Actions = new List<BaseAction>();

		// Token: 0x040097DA RID: 38874
		public int PhaseMax = 1;

		// Token: 0x040097DB RID: 38875
		public int CurrentPhase;
	}
}

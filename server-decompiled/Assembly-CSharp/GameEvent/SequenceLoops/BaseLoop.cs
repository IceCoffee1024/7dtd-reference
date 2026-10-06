using System;
using System.Collections.Generic;
using GameEvent.SequenceActions;
using UnityEngine.Scripting;

namespace GameEvent.SequenceLoops
{
	// Token: 0x02001976 RID: 6518
	[Preserve]
	public class BaseLoop : BaseAction
	{
		// Token: 0x0600C8DA RID: 51418 RVA: 0x004966C0 File Offset: 0x004948C0
		public override void SetActionKeyData(int _actionIndex, BaseAction _parent, string prefix = "")
		{
			base.SetActionKeyData(_actionIndex, _parent, prefix);
			for (int i = 0; i < this.Actions.Count; i++)
			{
				this.Actions[i].SetActionKeyData(i, this, "");
			}
		}

		// Token: 0x0600C8DB RID: 51419 RVA: 0x00496704 File Offset: 0x00494904
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

		// Token: 0x0600C8DC RID: 51420 RVA: 0x00496790 File Offset: 0x00494990
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

		// Token: 0x0600C8DD RID: 51421 RVA: 0x00496954 File Offset: 0x00494B54
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

		// Token: 0x0600C8DE RID: 51422 RVA: 0x00496998 File Offset: 0x00494B98
		public override void HandleTemplateInit(GameEventActionSequence seq)
		{
			base.HandleTemplateInit(seq);
			for (int i = 0; i < this.Actions.Count; i++)
			{
				this.Actions[i].HandleTemplateInit(seq);
			}
		}

		// Token: 0x0600C8DF RID: 51423 RVA: 0x004969D4 File Offset: 0x00494BD4
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

		// Token: 0x040096F0 RID: 38640
		public List<BaseAction> Actions = new List<BaseAction>();

		// Token: 0x040096F1 RID: 38641
		public int PhaseMax = 1;

		// Token: 0x040096F2 RID: 38642
		public int CurrentPhase;
	}
}

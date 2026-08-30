using System;
using System.Collections.Generic;
using GameEvent.SequenceActions;
using UnityEngine.Scripting;

namespace GameEvent.SequenceLoops
{
	// Token: 0x0200194A RID: 6474
	[Preserve]
	public class BaseLoop : BaseAction
	{
		// Token: 0x0600C782 RID: 51074 RVA: 0x004921D4 File Offset: 0x004903D4
		public override void SetActionKeyData(int _actionIndex, BaseAction _parent, string prefix = "")
		{
			base.SetActionKeyData(_actionIndex, _parent, prefix);
			for (int i = 0; i < this.Actions.Count; i++)
			{
				this.Actions[i].SetActionKeyData(i, this, "");
			}
		}

		// Token: 0x0600C783 RID: 51075 RVA: 0x00492218 File Offset: 0x00490418
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

		// Token: 0x0600C784 RID: 51076 RVA: 0x004922A4 File Offset: 0x004904A4
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

		// Token: 0x0600C785 RID: 51077 RVA: 0x00492468 File Offset: 0x00490668
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

		// Token: 0x0600C786 RID: 51078 RVA: 0x004924AC File Offset: 0x004906AC
		public override void HandleTemplateInit(GameEventActionSequence seq)
		{
			base.HandleTemplateInit(seq);
			for (int i = 0; i < this.Actions.Count; i++)
			{
				this.Actions[i].HandleTemplateInit(seq);
			}
		}

		// Token: 0x0600C787 RID: 51079 RVA: 0x004924E8 File Offset: 0x004906E8
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

		// Token: 0x0400962A RID: 38442
		public List<BaseAction> Actions = new List<BaseAction>();

		// Token: 0x0400962B RID: 38443
		public int PhaseMax = 1;

		// Token: 0x0400962C RID: 38444
		public int CurrentPhase;
	}
}

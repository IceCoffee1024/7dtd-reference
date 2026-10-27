using System;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x0200196B RID: 6507
	[Preserve]
	public class ChallengeObjectiveSurvive : BaseChallengeObjective
	{
		// Token: 0x1700196E RID: 6510
		// (get) Token: 0x0600C886 RID: 51334 RVA: 0x00495C8E File Offset: 0x00493E8E
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Survive;
			}
		}

		// Token: 0x1700196F RID: 6511
		// (get) Token: 0x0600C887 RID: 51335 RVA: 0x00495C92 File Offset: 0x00493E92
		public override string DescriptionText
		{
			get
			{
				return Localization.Get("challengeObjectiveSurvive", false, null) + ":";
			}
		}

		// Token: 0x17001970 RID: 6512
		// (get) Token: 0x0600C888 RID: 51336 RVA: 0x00495CAA File Offset: 0x00493EAA
		public override string StatusText
		{
			get
			{
				return string.Format("{0}/{1}", XUiM_PlayerBuffs.GetTimeString((float)this.current * 60f), XUiM_PlayerBuffs.GetTimeString((float)this.MaxCount * 60f));
			}
		}

		// Token: 0x0600C889 RID: 51337 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Init()
		{
		}

		// Token: 0x0600C88A RID: 51338 RVA: 0x00495CDC File Offset: 0x00493EDC
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.TimeSurvive += this.Current_TimeSurvive;
			base.Current = (int)base.Player.longestLife;
			if (base.Current >= this.MaxCount)
			{
				base.Current = this.MaxCount;
				this.CheckObjectiveComplete(true);
			}
		}

		// Token: 0x0600C88B RID: 51339 RVA: 0x00495D33 File Offset: 0x00493F33
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.TimeSurvive -= this.Current_TimeSurvive;
		}

		// Token: 0x0600C88C RID: 51340 RVA: 0x00495D4B File Offset: 0x00493F4B
		[PublicizedFrom(EAccessModifier.Private)]
		public void Current_TimeSurvive(float val)
		{
			if (this.CheckBaseRequirements())
			{
				return;
			}
			base.Current = (int)val;
			if (base.Current >= this.MaxCount)
			{
				base.Current = this.MaxCount;
				this.CheckObjectiveComplete(true);
			}
		}

		// Token: 0x0600C88D RID: 51341 RVA: 0x00495D80 File Offset: 0x00493F80
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveSurvive();
		}
	}
}

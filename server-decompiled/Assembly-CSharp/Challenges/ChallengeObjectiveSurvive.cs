using System;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x0200190E RID: 6414
	[Preserve]
	public class ChallengeObjectiveSurvive : BaseChallengeObjective
	{
		// Token: 0x17001884 RID: 6276
		// (get) Token: 0x0600C61A RID: 50714 RVA: 0x0048DDAE File Offset: 0x0048BFAE
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Survive;
			}
		}

		// Token: 0x17001885 RID: 6277
		// (get) Token: 0x0600C61B RID: 50715 RVA: 0x0048DDB2 File Offset: 0x0048BFB2
		public override string DescriptionText
		{
			get
			{
				return Localization.Get("challengeObjectiveSurvive", false, null) + ":";
			}
		}

		// Token: 0x17001886 RID: 6278
		// (get) Token: 0x0600C61C RID: 50716 RVA: 0x0048DDCA File Offset: 0x0048BFCA
		public override string StatusText
		{
			get
			{
				return string.Format("{0}/{1}", XUiM_PlayerBuffs.GetTimeString((float)this.current * 60f), XUiM_PlayerBuffs.GetTimeString((float)this.MaxCount * 60f));
			}
		}

		// Token: 0x0600C61D RID: 50717 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Init()
		{
		}

		// Token: 0x0600C61E RID: 50718 RVA: 0x0048DDFC File Offset: 0x0048BFFC
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

		// Token: 0x0600C61F RID: 50719 RVA: 0x0048DE53 File Offset: 0x0048C053
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.TimeSurvive -= this.Current_TimeSurvive;
		}

		// Token: 0x0600C620 RID: 50720 RVA: 0x0048DE6B File Offset: 0x0048C06B
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

		// Token: 0x0600C621 RID: 50721 RVA: 0x0048DEA0 File Offset: 0x0048C0A0
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveSurvive();
		}
	}
}

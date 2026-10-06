using System;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x0200193A RID: 6458
	[Preserve]
	public class ChallengeObjectiveSurvive : BaseChallengeObjective
	{
		// Token: 0x17001926 RID: 6438
		// (get) Token: 0x0600C772 RID: 51058 RVA: 0x0049229A File Offset: 0x0049049A
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Survive;
			}
		}

		// Token: 0x17001927 RID: 6439
		// (get) Token: 0x0600C773 RID: 51059 RVA: 0x0049229E File Offset: 0x0049049E
		public override string DescriptionText
		{
			get
			{
				return Localization.Get("challengeObjectiveSurvive", false, null) + ":";
			}
		}

		// Token: 0x17001928 RID: 6440
		// (get) Token: 0x0600C774 RID: 51060 RVA: 0x004922B6 File Offset: 0x004904B6
		public override string StatusText
		{
			get
			{
				return string.Format("{0}/{1}", XUiM_PlayerBuffs.GetTimeString((float)this.current * 60f), XUiM_PlayerBuffs.GetTimeString((float)this.MaxCount * 60f));
			}
		}

		// Token: 0x0600C775 RID: 51061 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Init()
		{
		}

		// Token: 0x0600C776 RID: 51062 RVA: 0x004922E8 File Offset: 0x004904E8
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

		// Token: 0x0600C777 RID: 51063 RVA: 0x0049233F File Offset: 0x0049053F
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.TimeSurvive -= this.Current_TimeSurvive;
		}

		// Token: 0x0600C778 RID: 51064 RVA: 0x00492357 File Offset: 0x00490557
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

		// Token: 0x0600C779 RID: 51065 RVA: 0x0049238C File Offset: 0x0049058C
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveSurvive();
		}
	}
}

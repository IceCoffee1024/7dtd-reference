using System;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001958 RID: 6488
	[Preserve]
	public class ChallengeObjectiveBloodmoon : BaseChallengeObjective
	{
		// Token: 0x17001945 RID: 6469
		// (get) Token: 0x0600C7B2 RID: 51122 RVA: 0x00047076 File Offset: 0x00045276
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Bloodmoon;
			}
		}

		// Token: 0x17001946 RID: 6470
		// (get) Token: 0x0600C7B3 RID: 51123 RVA: 0x00492733 File Offset: 0x00490933
		public override string DescriptionText
		{
			get
			{
				return Localization.Get("challengeObjectiveBloodMoonCompleted", false, null) + ":";
			}
		}

		// Token: 0x17001947 RID: 6471
		// (get) Token: 0x0600C7B4 RID: 51124 RVA: 0x0049274B File Offset: 0x0049094B
		public override string StatusText
		{
			get
			{
				if (!EntityFactory.EnemySpawnMode)
				{
					return "--";
				}
				return base.StatusText;
			}
		}

		// Token: 0x0600C7B5 RID: 51125 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Init()
		{
		}

		// Token: 0x0600C7B6 RID: 51126 RVA: 0x00492760 File Offset: 0x00490960
		public override void HandleAddHooks()
		{
			if (!EntityFactory.EnemySpawnMode)
			{
				base.Current = this.MaxCount;
				base.Complete = true;
				this.Owner.HandleComplete(false, true);
				if (this.Owner.ChallengeState == Challenge.ChallengeStates.Completed)
				{
					this.Owner.AutoCompleted = true;
					this.Owner.ChallengeState = Challenge.ChallengeStates.Redeemed;
					return;
				}
			}
			else
			{
				QuestEventManager.Current.BloodMoonSurvive += this.Current_BloodMoonSurvive;
			}
		}

		// Token: 0x0600C7B7 RID: 51127 RVA: 0x004927D1 File Offset: 0x004909D1
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.BloodMoonSurvive -= this.Current_BloodMoonSurvive;
		}

		// Token: 0x0600C7B8 RID: 51128 RVA: 0x004927EC File Offset: 0x004909EC
		[PublicizedFrom(EAccessModifier.Private)]
		public void Current_BloodMoonSurvive()
		{
			if (this.CheckBaseRequirements())
			{
				return;
			}
			int num = base.Current;
			base.Current = num + 1;
			this.CheckObjectiveComplete(true);
		}

		// Token: 0x0600C7B9 RID: 51129 RVA: 0x0049281A File Offset: 0x00490A1A
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveBloodmoon();
		}
	}
}

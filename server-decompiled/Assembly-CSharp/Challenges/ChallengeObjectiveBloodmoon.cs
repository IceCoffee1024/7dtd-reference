using System;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001927 RID: 6439
	[Preserve]
	public class ChallengeObjectiveBloodmoon : BaseChallengeObjective
	{
		// Token: 0x170018FD RID: 6397
		// (get) Token: 0x0600C69E RID: 50846 RVA: 0x000470B6 File Offset: 0x000452B6
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Bloodmoon;
			}
		}

		// Token: 0x170018FE RID: 6398
		// (get) Token: 0x0600C69F RID: 50847 RVA: 0x0048ED3F File Offset: 0x0048CF3F
		public override string DescriptionText
		{
			get
			{
				return Localization.Get("challengeObjectiveBloodMoonCompleted", false, null) + ":";
			}
		}

		// Token: 0x170018FF RID: 6399
		// (get) Token: 0x0600C6A0 RID: 50848 RVA: 0x0048ED57 File Offset: 0x0048CF57
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

		// Token: 0x0600C6A1 RID: 50849 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Init()
		{
		}

		// Token: 0x0600C6A2 RID: 50850 RVA: 0x0048ED6C File Offset: 0x0048CF6C
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

		// Token: 0x0600C6A3 RID: 50851 RVA: 0x0048EDDD File Offset: 0x0048CFDD
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.BloodMoonSurvive -= this.Current_BloodMoonSurvive;
		}

		// Token: 0x0600C6A4 RID: 50852 RVA: 0x0048EDF8 File Offset: 0x0048CFF8
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

		// Token: 0x0600C6A5 RID: 50853 RVA: 0x0048EE26 File Offset: 0x0048D026
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveBloodmoon();
		}
	}
}

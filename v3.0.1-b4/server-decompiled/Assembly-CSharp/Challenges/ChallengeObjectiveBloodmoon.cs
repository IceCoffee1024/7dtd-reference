using System;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x020018D5 RID: 6357
	[Preserve]
	public class ChallengeObjectiveBloodmoon : BaseChallengeObjective
	{
		// Token: 0x170017E9 RID: 6121
		// (get) Token: 0x0600C3BF RID: 50111 RVA: 0x00046C7A File Offset: 0x00044E7A
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Bloodmoon;
			}
		}

		// Token: 0x170017EA RID: 6122
		// (get) Token: 0x0600C3C0 RID: 50112 RVA: 0x0048450B File Offset: 0x0048270B
		public override string DescriptionText
		{
			get
			{
				return Localization.Get("challengeObjectiveBloodMoonCompleted", false, null) + ":";
			}
		}

		// Token: 0x170017EB RID: 6123
		// (get) Token: 0x0600C3C1 RID: 50113 RVA: 0x00484523 File Offset: 0x00482723
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

		// Token: 0x0600C3C2 RID: 50114 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Init()
		{
		}

		// Token: 0x0600C3C3 RID: 50115 RVA: 0x00484538 File Offset: 0x00482738
		public override void HandleAddHooks()
		{
			if (!EntityFactory.EnemySpawnMode)
			{
				base.Current = this.MaxCount;
				base.Complete = true;
				this.Owner.HandleComplete(false);
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

		// Token: 0x0600C3C4 RID: 50116 RVA: 0x004845A8 File Offset: 0x004827A8
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.BloodMoonSurvive -= this.Current_BloodMoonSurvive;
		}

		// Token: 0x0600C3C5 RID: 50117 RVA: 0x004845C0 File Offset: 0x004827C0
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

		// Token: 0x0600C3C6 RID: 50118 RVA: 0x004845EE File Offset: 0x004827EE
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveBloodmoon();
		}
	}
}

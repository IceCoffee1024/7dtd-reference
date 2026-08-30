using System;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x020018FB RID: 6395
	[Preserve]
	public class ChallengeObjectiveBloodmoon : BaseChallengeObjective
	{
		// Token: 0x1700185B RID: 6235
		// (get) Token: 0x0600C546 RID: 50502 RVA: 0x00046EDA File Offset: 0x000450DA
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Bloodmoon;
			}
		}

		// Token: 0x1700185C RID: 6236
		// (get) Token: 0x0600C547 RID: 50503 RVA: 0x0048A84F File Offset: 0x00488A4F
		public override string DescriptionText
		{
			get
			{
				return Localization.Get("challengeObjectiveBloodMoonCompleted", false, null) + ":";
			}
		}

		// Token: 0x1700185D RID: 6237
		// (get) Token: 0x0600C548 RID: 50504 RVA: 0x0048A867 File Offset: 0x00488A67
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

		// Token: 0x0600C549 RID: 50505 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Init()
		{
		}

		// Token: 0x0600C54A RID: 50506 RVA: 0x0048A87C File Offset: 0x00488A7C
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

		// Token: 0x0600C54B RID: 50507 RVA: 0x0048A8ED File Offset: 0x00488AED
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.BloodMoonSurvive -= this.Current_BloodMoonSurvive;
		}

		// Token: 0x0600C54C RID: 50508 RVA: 0x0048A908 File Offset: 0x00488B08
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

		// Token: 0x0600C54D RID: 50509 RVA: 0x0048A936 File Offset: 0x00488B36
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveBloodmoon();
		}
	}
}

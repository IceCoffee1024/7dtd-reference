using System;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x020018E8 RID: 6376
	[Preserve]
	public class ChallengeObjectiveSurvive : BaseChallengeObjective
	{
		// Token: 0x17001812 RID: 6162
		// (get) Token: 0x0600C493 RID: 50323 RVA: 0x00487A62 File Offset: 0x00485C62
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Survive;
			}
		}

		// Token: 0x17001813 RID: 6163
		// (get) Token: 0x0600C494 RID: 50324 RVA: 0x00487A66 File Offset: 0x00485C66
		public override string DescriptionText
		{
			get
			{
				return Localization.Get("challengeObjectiveSurvive", false, null) + ":";
			}
		}

		// Token: 0x17001814 RID: 6164
		// (get) Token: 0x0600C495 RID: 50325 RVA: 0x00487A7E File Offset: 0x00485C7E
		public override string StatusText
		{
			get
			{
				return string.Format("{0}/{1}", XUiM_PlayerBuffs.GetTimeString((float)this.current * 60f), XUiM_PlayerBuffs.GetTimeString((float)this.MaxCount * 60f));
			}
		}

		// Token: 0x0600C496 RID: 50326 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Init()
		{
		}

		// Token: 0x0600C497 RID: 50327 RVA: 0x00487AB0 File Offset: 0x00485CB0
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

		// Token: 0x0600C498 RID: 50328 RVA: 0x00487B07 File Offset: 0x00485D07
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.TimeSurvive -= this.Current_TimeSurvive;
		}

		// Token: 0x0600C499 RID: 50329 RVA: 0x00487B1F File Offset: 0x00485D1F
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

		// Token: 0x0600C49A RID: 50330 RVA: 0x00487B54 File Offset: 0x00485D54
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveSurvive();
		}
	}
}

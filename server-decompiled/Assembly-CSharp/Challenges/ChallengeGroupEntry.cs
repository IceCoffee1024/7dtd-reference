using System;
using System.Collections.Generic;

namespace Challenges
{
	// Token: 0x02001921 RID: 6433
	public class ChallengeGroupEntry
	{
		// Token: 0x0600C64E RID: 50766 RVA: 0x0048DE09 File Offset: 0x0048C009
		public ChallengeGroupEntry(ChallengeGroup group)
		{
			this.ChallengeGroup = group;
		}

		// Token: 0x0600C64F RID: 50767 RVA: 0x0048DE1F File Offset: 0x0048C01F
		public void CreateChallenges(EntityPlayer player)
		{
			this.ResetChallenges(player);
			if (this.ChallengeGroup.DayReset != -1)
			{
				this.LastUpdateDay = GameUtils.WorldTimeToDays(GameManager.Instance.World.worldTime) + this.ChallengeGroup.DayReset;
			}
		}

		// Token: 0x0600C650 RID: 50768 RVA: 0x0048DE5C File Offset: 0x0048C05C
		public void Update(int day, EntityPlayer player)
		{
			if (this.ChallengeGroup.DayReset == -1)
			{
				return;
			}
			if (this.LastUpdateDay <= day)
			{
				this.ResetChallenges(player);
				this.LastUpdateDay = day + this.ChallengeGroup.DayReset;
			}
		}

		// Token: 0x0600C651 RID: 50769 RVA: 0x0048DE90 File Offset: 0x0048C090
		public void AddAnyMissingChallenges(EntityPlayer player)
		{
			ChallengeJournal challengeJournal = player.challengeJournal;
			if (!this.ChallengeGroup.IsRandom)
			{
				int activeChallengeCount = this.ChallengeGroup.ActiveChallengeCount;
				int num = 0;
				while (num < this.ChallengeGroup.ChallengeClasses.Count && num < activeChallengeCount)
				{
					if (!challengeJournal.ChallengeDictionary.ContainsKey(this.ChallengeGroup.ChallengeClasses[num].Name))
					{
						Challenge challenge = this.ChallengeGroup.ChallengeClasses[num].CreateChallenge(challengeJournal);
						challenge.ChallengeGroup = this.ChallengeGroup;
						challengeJournal.AddChallenge(challenge);
					}
					num++;
				}
			}
		}

		// Token: 0x0600C652 RID: 50770 RVA: 0x0048DF2C File Offset: 0x0048C12C
		public void ResetChallenges(EntityPlayer player)
		{
			ChallengeJournal challengeJournal = player.challengeJournal;
			if (this.ChallengeGroup.IsRandom)
			{
				challengeJournal.RemoveChallengesForGroup(this.ChallengeGroup);
				int activeChallengeCount = this.ChallengeGroup.ActiveChallengeCount;
				List<ChallengeClass> challengeClassesForCreate = this.ChallengeGroup.GetChallengeClassesForCreate();
				for (int i = 0; i < challengeClassesForCreate.Count; i++)
				{
					if (i >= activeChallengeCount)
					{
						return;
					}
					Challenge challenge = challengeClassesForCreate[i].CreateChallenge(challengeJournal);
					challenge.ChallengeGroup = this.ChallengeGroup;
					challengeJournal.AddChallenge(challenge);
					challenge.StartChallenge();
					if (challenge.IsTracked)
					{
						LocalPlayerUI.GetUIForPrimaryPlayer().xui.QuestTracker.TrackedChallenge = challenge;
					}
				}
			}
			else
			{
				int activeChallengeCount2 = this.ChallengeGroup.ActiveChallengeCount;
				this.ChallengeGroup.IsVisible(player);
				int num = 0;
				while (num < this.ChallengeGroup.ChallengeClasses.Count && num < activeChallengeCount2)
				{
					Challenge challenge2 = this.ChallengeGroup.ChallengeClasses[num].CreateChallenge(challengeJournal);
					challenge2.ChallengeGroup = this.ChallengeGroup;
					if (challengeJournal.Challenges.Count == 0)
					{
						challenge2.IsTracked = true;
					}
					challengeJournal.AddChallenge(challenge2);
					num++;
				}
			}
		}

		// Token: 0x040095B8 RID: 38328
		public ChallengeGroup ChallengeGroup;

		// Token: 0x040095B9 RID: 38329
		public int LastUpdateDay = -1;
	}
}

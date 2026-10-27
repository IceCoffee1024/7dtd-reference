using System;
using System.Collections.Generic;

namespace Challenges
{
	// Token: 0x02001952 RID: 6482
	public class ChallengeGroupEntry
	{
		// Token: 0x0600C762 RID: 51042 RVA: 0x004917FD File Offset: 0x0048F9FD
		public ChallengeGroupEntry(ChallengeGroup group)
		{
			this.ChallengeGroup = group;
		}

		// Token: 0x0600C763 RID: 51043 RVA: 0x00491813 File Offset: 0x0048FA13
		public void CreateChallenges(EntityPlayer player)
		{
			this.ResetChallenges(player);
			if (this.ChallengeGroup.DayReset != -1)
			{
				this.LastUpdateDay = GameUtils.WorldTimeToDays(GameManager.Instance.World.worldTime) + this.ChallengeGroup.DayReset;
			}
		}

		// Token: 0x0600C764 RID: 51044 RVA: 0x00491850 File Offset: 0x0048FA50
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

		// Token: 0x0600C765 RID: 51045 RVA: 0x00491884 File Offset: 0x0048FA84
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

		// Token: 0x0600C766 RID: 51046 RVA: 0x00491920 File Offset: 0x0048FB20
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

		// Token: 0x040096A1 RID: 38561
		public ChallengeGroup ChallengeGroup;

		// Token: 0x040096A2 RID: 38562
		public int LastUpdateDay = -1;
	}
}

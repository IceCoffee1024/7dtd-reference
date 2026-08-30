using System;
using System.Collections.Generic;

namespace Challenges
{
	// Token: 0x020018F5 RID: 6389
	public class ChallengeGroupEntry
	{
		// Token: 0x0600C4F6 RID: 50422 RVA: 0x00489919 File Offset: 0x00487B19
		public ChallengeGroupEntry(ChallengeGroup group)
		{
			this.ChallengeGroup = group;
		}

		// Token: 0x0600C4F7 RID: 50423 RVA: 0x0048992F File Offset: 0x00487B2F
		public void CreateChallenges(EntityPlayer player)
		{
			this.ResetChallenges(player);
			if (this.ChallengeGroup.DayReset != -1)
			{
				this.LastUpdateDay = GameUtils.WorldTimeToDays(GameManager.Instance.World.worldTime) + this.ChallengeGroup.DayReset;
			}
		}

		// Token: 0x0600C4F8 RID: 50424 RVA: 0x0048996C File Offset: 0x00487B6C
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

		// Token: 0x0600C4F9 RID: 50425 RVA: 0x004899A0 File Offset: 0x00487BA0
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

		// Token: 0x0600C4FA RID: 50426 RVA: 0x00489A3C File Offset: 0x00487C3C
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

		// Token: 0x040094F2 RID: 38130
		public ChallengeGroup ChallengeGroup;

		// Token: 0x040094F3 RID: 38131
		public int LastUpdateDay = -1;
	}
}

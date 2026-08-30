using System;
using Challenges;
using UnityEngine.Scripting;

// Token: 0x0200095E RID: 2398
[Preserve]
public class QuestActionTrackChallenge : BaseQuestAction
{
	// Token: 0x06004635 RID: 17973 RVA: 0x000027FC File Offset: 0x000009FC
	public override void SetupAction()
	{
	}

	// Token: 0x06004636 RID: 17974 RVA: 0x001BC45C File Offset: 0x001BA65C
	public override void PerformAction(Quest ownerQuest)
	{
		EntityPlayerLocal ownerPlayer = ownerQuest.OwnerJournal.OwnerPlayer;
		XUi xui = ownerPlayer.PlayerUI.xui;
		if (xui.QuestTracker.TrackedQuest == null && xui.QuestTracker.TrackedChallenge == null && xui.Recipes.TrackedRecipe == null && ownerPlayer.challengeJournal.ChallengeDictionary.ContainsKey(this.ID))
		{
			Challenge challenge = ownerPlayer.challengeJournal.ChallengeDictionary[this.ID];
			if (challenge.ChallengeGroup.IsActive)
			{
				xui.QuestTracker.TrackedChallenge = challenge;
			}
		}
	}

	// Token: 0x06004637 RID: 17975 RVA: 0x001BC4F0 File Offset: 0x001BA6F0
	public override BaseQuestAction Clone()
	{
		QuestActionTrackChallenge questActionTrackChallenge = new QuestActionTrackChallenge();
		base.CopyValues(questActionTrackChallenge);
		return questActionTrackChallenge;
	}

	// Token: 0x06004638 RID: 17976 RVA: 0x001BC50B File Offset: 0x001BA70B
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		properties.ParseString(QuestActionTrackChallenge.PropChallenge, ref this.ID);
	}

	// Token: 0x04003856 RID: 14422
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropChallenge = "challenge";
}

using System;
using Challenges;
using UnityEngine.Scripting;

// Token: 0x0200097F RID: 2431
[Preserve]
public class QuestActionTrackChallenge : BaseQuestAction
{
	// Token: 0x0600467F RID: 18047 RVA: 0x000027FC File Offset: 0x000009FC
	public override void SetupAction()
	{
	}

	// Token: 0x06004680 RID: 18048 RVA: 0x001C01DC File Offset: 0x001BE3DC
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

	// Token: 0x06004681 RID: 18049 RVA: 0x001C0270 File Offset: 0x001BE470
	public override BaseQuestAction Clone()
	{
		QuestActionTrackChallenge questActionTrackChallenge = new QuestActionTrackChallenge();
		base.CopyValues(questActionTrackChallenge);
		return questActionTrackChallenge;
	}

	// Token: 0x06004682 RID: 18050 RVA: 0x001C028B File Offset: 0x001BE48B
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		properties.ParseString(QuestActionTrackChallenge.PropChallenge, ref this.ID);
	}

	// Token: 0x04003903 RID: 14595
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropChallenge = "challenge";
}

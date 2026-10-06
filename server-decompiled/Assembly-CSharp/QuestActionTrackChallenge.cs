using System;
using Challenges;
using UnityEngine.Scripting;

// Token: 0x02000975 RID: 2421
[Preserve]
public class QuestActionTrackChallenge : BaseQuestAction
{
	// Token: 0x0600465C RID: 18012 RVA: 0x000027FC File Offset: 0x000009FC
	public override void SetupAction()
	{
	}

	// Token: 0x0600465D RID: 18013 RVA: 0x001BF158 File Offset: 0x001BD358
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

	// Token: 0x0600465E RID: 18014 RVA: 0x001BF1EC File Offset: 0x001BD3EC
	public override BaseQuestAction Clone()
	{
		QuestActionTrackChallenge questActionTrackChallenge = new QuestActionTrackChallenge();
		base.CopyValues(questActionTrackChallenge);
		return questActionTrackChallenge;
	}

	// Token: 0x0600465F RID: 18015 RVA: 0x001BF207 File Offset: 0x001BD407
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		properties.ParseString(QuestActionTrackChallenge.PropChallenge, ref this.ID);
	}

	// Token: 0x040038E3 RID: 14563
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropChallenge = "challenge";
}

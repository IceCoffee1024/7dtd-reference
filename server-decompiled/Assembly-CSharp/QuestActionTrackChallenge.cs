using System;
using Challenges;
using UnityEngine.Scripting;

// Token: 0x0200096F RID: 2415
[Preserve]
public class QuestActionTrackChallenge : BaseQuestAction
{
	// Token: 0x060046BB RID: 18107 RVA: 0x000027FC File Offset: 0x000009FC
	public override void SetupAction()
	{
	}

	// Token: 0x060046BC RID: 18108 RVA: 0x001BEC50 File Offset: 0x001BCE50
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

	// Token: 0x060046BD RID: 18109 RVA: 0x001BECE4 File Offset: 0x001BCEE4
	public override BaseQuestAction Clone()
	{
		QuestActionTrackChallenge questActionTrackChallenge = new QuestActionTrackChallenge();
		base.CopyValues(questActionTrackChallenge);
		return questActionTrackChallenge;
	}

	// Token: 0x060046BE RID: 18110 RVA: 0x001BECFF File Offset: 0x001BCEFF
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		properties.ParseString(QuestActionTrackChallenge.PropChallenge, ref this.ID);
	}

	// Token: 0x040038E7 RID: 14567
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropChallenge = "challenge";
}

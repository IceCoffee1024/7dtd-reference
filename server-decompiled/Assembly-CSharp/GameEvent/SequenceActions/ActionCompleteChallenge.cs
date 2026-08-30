using System;
using Challenges;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001975 RID: 6517
	[Preserve]
	public class ActionCompleteChallenge : ActionBaseClientAction
	{
		// Token: 0x0600C84D RID: 51277 RVA: 0x004974F0 File Offset: 0x004956F0
		public override void OnClientPerform(Entity target)
		{
			EntityPlayerLocal entityPlayerLocal = target as EntityPlayerLocal;
			if (entityPlayerLocal != null)
			{
				string[] array = this.ChallengeID.ToLower().Split(',', StringSplitOptions.None);
				for (int i = 0; i < array.Length; i++)
				{
					Challenge challenge = entityPlayerLocal.challengeJournal.ChallengeDictionary[array[i]];
					if (challenge != null)
					{
						challenge.CompleteChallenge(this.ForceRedeem, true, false);
					}
				}
				entityPlayerLocal.PlayerUI.xui.QuestTracker.TrackedChallenge = null;
			}
		}

		// Token: 0x0600C84E RID: 51278 RVA: 0x00497564 File Offset: 0x00495764
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionCompleteChallenge.PropChallengeID, ref this.ChallengeID);
			properties.ParseBool(ActionCompleteChallenge.PropForceRedeem, ref this.ForceRedeem);
		}

		// Token: 0x0600C84F RID: 51279 RVA: 0x0049758F File Offset: 0x0049578F
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionCompleteChallenge
			{
				ChallengeID = this.ChallengeID,
				ForceRedeem = this.ForceRedeem
			};
		}

		// Token: 0x0400972F RID: 38703
		public string ChallengeID = "";

		// Token: 0x04009730 RID: 38704
		public bool ForceRedeem;

		// Token: 0x04009731 RID: 38705
		public static string PropChallengeID = "challenges";

		// Token: 0x04009732 RID: 38706
		public static string PropForceRedeem = "force_redeem";
	}
}

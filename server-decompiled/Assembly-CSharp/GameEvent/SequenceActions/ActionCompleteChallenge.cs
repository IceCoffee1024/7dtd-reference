using System;
using Challenges;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019D2 RID: 6610
	[Preserve]
	public class ActionCompleteChallenge : ActionBaseClientAction
	{
		// Token: 0x0600CAB9 RID: 51897 RVA: 0x0049F4CC File Offset: 0x0049D6CC
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

		// Token: 0x0600CABA RID: 51898 RVA: 0x0049F540 File Offset: 0x0049D740
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionCompleteChallenge.PropChallengeID, ref this.ChallengeID);
			properties.ParseBool(ActionCompleteChallenge.PropForceRedeem, ref this.ForceRedeem);
		}

		// Token: 0x0600CABB RID: 51899 RVA: 0x0049F56B File Offset: 0x0049D76B
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionCompleteChallenge
			{
				ChallengeID = this.ChallengeID,
				ForceRedeem = this.ForceRedeem
			};
		}

		// Token: 0x040098DE RID: 39134
		public string ChallengeID = "";

		// Token: 0x040098DF RID: 39135
		public bool ForceRedeem;

		// Token: 0x040098E0 RID: 39136
		public static string PropChallengeID = "challenges";

		// Token: 0x040098E1 RID: 39137
		public static string PropForceRedeem = "force_redeem";
	}
}

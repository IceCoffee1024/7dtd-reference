using System;
using Challenges;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019A1 RID: 6561
	[Preserve]
	public class ActionCompleteChallenge : ActionBaseClientAction
	{
		// Token: 0x0600C9A5 RID: 51621 RVA: 0x0049B9CC File Offset: 0x00499BCC
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

		// Token: 0x0600C9A6 RID: 51622 RVA: 0x0049BA40 File Offset: 0x00499C40
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionCompleteChallenge.PropChallengeID, ref this.ChallengeID);
			properties.ParseBool(ActionCompleteChallenge.PropForceRedeem, ref this.ForceRedeem);
		}

		// Token: 0x0600C9A7 RID: 51623 RVA: 0x0049BA6B File Offset: 0x00499C6B
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionCompleteChallenge
			{
				ChallengeID = this.ChallengeID,
				ForceRedeem = this.ForceRedeem
			};
		}

		// Token: 0x040097F5 RID: 38901
		public string ChallengeID = "";

		// Token: 0x040097F6 RID: 38902
		public bool ForceRedeem;

		// Token: 0x040097F7 RID: 38903
		public static string PropChallengeID = "challenges";

		// Token: 0x040097F8 RID: 38904
		public static string PropForceRedeem = "force_redeem";
	}
}

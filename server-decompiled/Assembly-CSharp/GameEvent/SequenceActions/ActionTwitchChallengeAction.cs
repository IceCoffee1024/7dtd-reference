using System;
using Challenges;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A5B RID: 6747
	[Preserve]
	public class ActionTwitchChallengeAction : ActionBaseClientAction
	{
		// Token: 0x0600CD27 RID: 52519 RVA: 0x004ACAD0 File Offset: 0x004AACD0
		public override void OnClientPerform(Entity target)
		{
			QuestEventManager.Current.TwitchEventReceived(this.TwitchObjectiveType, this.param);
		}

		// Token: 0x0600CD28 RID: 52520 RVA: 0x004ACAE8 File Offset: 0x004AACE8
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<TwitchObjectiveTypes>(ActionTwitchChallengeAction.PropObjectiveType, ref this.TwitchObjectiveType);
			properties.ParseString(ActionTwitchChallengeAction.PropObjectiveParam, ref this.param);
		}

		// Token: 0x0600CD29 RID: 52521 RVA: 0x004ACB13 File Offset: 0x004AAD13
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionTwitchChallengeAction
			{
				TwitchObjectiveType = this.TwitchObjectiveType,
				param = this.param
			};
		}

		// Token: 0x04009BA8 RID: 39848
		[PublicizedFrom(EAccessModifier.Private)]
		public TwitchObjectiveTypes TwitchObjectiveType;

		// Token: 0x04009BA9 RID: 39849
		[PublicizedFrom(EAccessModifier.Private)]
		public string param = "";

		// Token: 0x04009BAA RID: 39850
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropObjectiveType = "objective_type";

		// Token: 0x04009BAB RID: 39851
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropObjectiveParam = "objective_param";
	}
}

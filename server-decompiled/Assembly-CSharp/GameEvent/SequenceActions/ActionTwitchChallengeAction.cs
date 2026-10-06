using System;
using Challenges;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A2A RID: 6698
	[Preserve]
	public class ActionTwitchChallengeAction : ActionBaseClientAction
	{
		// Token: 0x0600CC13 RID: 52243 RVA: 0x004A8FD4 File Offset: 0x004A71D4
		public override void OnClientPerform(Entity target)
		{
			QuestEventManager.Current.TwitchEventReceived(this.TwitchObjectiveType, this.param);
		}

		// Token: 0x0600CC14 RID: 52244 RVA: 0x004A8FEC File Offset: 0x004A71EC
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<TwitchObjectiveTypes>(ActionTwitchChallengeAction.PropObjectiveType, ref this.TwitchObjectiveType);
			properties.ParseString(ActionTwitchChallengeAction.PropObjectiveParam, ref this.param);
		}

		// Token: 0x0600CC15 RID: 52245 RVA: 0x004A9017 File Offset: 0x004A7217
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionTwitchChallengeAction
			{
				TwitchObjectiveType = this.TwitchObjectiveType,
				param = this.param
			};
		}

		// Token: 0x04009ABF RID: 39615
		[PublicizedFrom(EAccessModifier.Private)]
		public TwitchObjectiveTypes TwitchObjectiveType;

		// Token: 0x04009AC0 RID: 39616
		[PublicizedFrom(EAccessModifier.Private)]
		public string param = "";

		// Token: 0x04009AC1 RID: 39617
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropObjectiveType = "objective_type";

		// Token: 0x04009AC2 RID: 39618
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropObjectiveParam = "objective_param";
	}
}

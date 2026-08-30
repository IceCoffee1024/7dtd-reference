using System;
using Challenges;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019D7 RID: 6615
	[Preserve]
	public class ActionTwitchChallengeAction : ActionBaseClientAction
	{
		// Token: 0x0600C932 RID: 51506 RVA: 0x0049E714 File Offset: 0x0049C914
		public override void OnClientPerform(Entity target)
		{
			QuestEventManager.Current.TwitchEventReceived(this.TwitchObjectiveType, this.param);
		}

		// Token: 0x0600C933 RID: 51507 RVA: 0x0049E72C File Offset: 0x0049C92C
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<TwitchObjectiveTypes>(ActionTwitchChallengeAction.PropObjectiveType, ref this.TwitchObjectiveType);
			properties.ParseString(ActionTwitchChallengeAction.PropObjectiveParam, ref this.param);
		}

		// Token: 0x0600C934 RID: 51508 RVA: 0x0049E757 File Offset: 0x0049C957
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionTwitchChallengeAction
			{
				TwitchObjectiveType = this.TwitchObjectiveType,
				param = this.param
			};
		}

		// Token: 0x040098C8 RID: 39112
		[PublicizedFrom(EAccessModifier.Private)]
		public TwitchObjectiveTypes TwitchObjectiveType;

		// Token: 0x040098C9 RID: 39113
		[PublicizedFrom(EAccessModifier.Private)]
		public string param = "";

		// Token: 0x040098CA RID: 39114
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropObjectiveType = "objective_type";

		// Token: 0x040098CB RID: 39115
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropObjectiveParam = "objective_param";
	}
}

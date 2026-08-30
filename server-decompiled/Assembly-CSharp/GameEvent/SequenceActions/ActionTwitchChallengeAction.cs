using System;
using Challenges;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019FE RID: 6654
	[Preserve]
	public class ActionTwitchChallengeAction : ActionBaseClientAction
	{
		// Token: 0x0600CABB RID: 51899 RVA: 0x004A4B08 File Offset: 0x004A2D08
		public override void OnClientPerform(Entity target)
		{
			QuestEventManager.Current.TwitchEventReceived(this.TwitchObjectiveType, this.param);
		}

		// Token: 0x0600CABC RID: 51900 RVA: 0x004A4B20 File Offset: 0x004A2D20
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<TwitchObjectiveTypes>(ActionTwitchChallengeAction.PropObjectiveType, ref this.TwitchObjectiveType);
			properties.ParseString(ActionTwitchChallengeAction.PropObjectiveParam, ref this.param);
		}

		// Token: 0x0600CABD RID: 51901 RVA: 0x004A4B4B File Offset: 0x004A2D4B
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionTwitchChallengeAction
			{
				TwitchObjectiveType = this.TwitchObjectiveType,
				param = this.param
			};
		}

		// Token: 0x040099F9 RID: 39417
		[PublicizedFrom(EAccessModifier.Private)]
		public TwitchObjectiveTypes TwitchObjectiveType;

		// Token: 0x040099FA RID: 39418
		[PublicizedFrom(EAccessModifier.Private)]
		public string param = "";

		// Token: 0x040099FB RID: 39419
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropObjectiveType = "objective_type";

		// Token: 0x040099FC RID: 39420
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropObjectiveParam = "objective_param";
	}
}

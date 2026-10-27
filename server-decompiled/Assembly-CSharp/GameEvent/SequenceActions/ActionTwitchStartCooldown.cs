using System;
using Twitch;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A5E RID: 6750
	[Preserve]
	public class ActionTwitchStartCooldown : ActionBaseClientAction
	{
		// Token: 0x0600CD37 RID: 52535 RVA: 0x004ACD10 File Offset: 0x004AAF10
		public override void OnClientPerform(Entity target)
		{
			TwitchManager twitchManager = TwitchManager.Current;
			if (!twitchManager.TwitchActive)
			{
				return;
			}
			float floatValue = GameEventManager.GetFloatValue(target as EntityAlive, this.cooldownTimeLeft, 5f);
			twitchManager.SetCooldown(floatValue, TwitchManager.CooldownTypes.Time, false, true);
		}

		// Token: 0x0600CD38 RID: 52536 RVA: 0x004ACD4D File Offset: 0x004AAF4D
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionTwitchStartCooldown.PropTime, ref this.cooldownTimeLeft);
		}

		// Token: 0x0600CD39 RID: 52537 RVA: 0x004ACD67 File Offset: 0x004AAF67
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionTwitchStartCooldown
			{
				cooldownTimeLeft = this.cooldownTimeLeft
			};
		}

		// Token: 0x04009BB3 RID: 39859
		[PublicizedFrom(EAccessModifier.Protected)]
		public string cooldownTimeLeft;

		// Token: 0x04009BB4 RID: 39860
		public static string PropTime = "time";
	}
}

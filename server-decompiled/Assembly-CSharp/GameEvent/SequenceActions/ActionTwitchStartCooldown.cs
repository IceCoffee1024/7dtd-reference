using System;
using Twitch;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A2D RID: 6701
	[Preserve]
	public class ActionTwitchStartCooldown : ActionBaseClientAction
	{
		// Token: 0x0600CC23 RID: 52259 RVA: 0x004A9214 File Offset: 0x004A7414
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

		// Token: 0x0600CC24 RID: 52260 RVA: 0x004A9251 File Offset: 0x004A7451
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionTwitchStartCooldown.PropTime, ref this.cooldownTimeLeft);
		}

		// Token: 0x0600CC25 RID: 52261 RVA: 0x004A926B File Offset: 0x004A746B
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionTwitchStartCooldown
			{
				cooldownTimeLeft = this.cooldownTimeLeft
			};
		}

		// Token: 0x04009ACA RID: 39626
		[PublicizedFrom(EAccessModifier.Protected)]
		public string cooldownTimeLeft;

		// Token: 0x04009ACB RID: 39627
		public static string PropTime = "time";
	}
}

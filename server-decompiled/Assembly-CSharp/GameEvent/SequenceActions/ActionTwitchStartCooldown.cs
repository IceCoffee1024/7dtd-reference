using System;
using Twitch;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A01 RID: 6657
	[Preserve]
	public class ActionTwitchStartCooldown : ActionBaseClientAction
	{
		// Token: 0x0600CACB RID: 51915 RVA: 0x004A4D48 File Offset: 0x004A2F48
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

		// Token: 0x0600CACC RID: 51916 RVA: 0x004A4D85 File Offset: 0x004A2F85
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionTwitchStartCooldown.PropTime, ref this.cooldownTimeLeft);
		}

		// Token: 0x0600CACD RID: 51917 RVA: 0x004A4D9F File Offset: 0x004A2F9F
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionTwitchStartCooldown
			{
				cooldownTimeLeft = this.cooldownTimeLeft
			};
		}

		// Token: 0x04009A04 RID: 39428
		[PublicizedFrom(EAccessModifier.Protected)]
		public string cooldownTimeLeft;

		// Token: 0x04009A05 RID: 39429
		public static string PropTime = "time";
	}
}

using System;
using Twitch;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019DA RID: 6618
	[Preserve]
	public class ActionTwitchStartCooldown : ActionBaseClientAction
	{
		// Token: 0x0600C942 RID: 51522 RVA: 0x0049E954 File Offset: 0x0049CB54
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

		// Token: 0x0600C943 RID: 51523 RVA: 0x0049E991 File Offset: 0x0049CB91
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionTwitchStartCooldown.PropTime, ref this.cooldownTimeLeft);
		}

		// Token: 0x0600C944 RID: 51524 RVA: 0x0049E9AB File Offset: 0x0049CBAB
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionTwitchStartCooldown
			{
				cooldownTimeLeft = this.cooldownTimeLeft
			};
		}

		// Token: 0x040098D3 RID: 39123
		[PublicizedFrom(EAccessModifier.Protected)]
		public string cooldownTimeLeft;

		// Token: 0x040098D4 RID: 39124
		public static string PropTime = "time";
	}
}

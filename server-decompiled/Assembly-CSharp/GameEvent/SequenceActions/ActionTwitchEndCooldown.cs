using System;
using Twitch;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A2B RID: 6699
	[Preserve]
	public class ActionTwitchEndCooldown : ActionBaseClientAction
	{
		// Token: 0x0600CC18 RID: 52248 RVA: 0x004A905F File Offset: 0x004A725F
		public override void OnClientPerform(Entity target)
		{
			if (TwitchManager.HasInstance)
			{
				TwitchManager.Current.ForceEndCooldown(this.playSound);
			}
		}

		// Token: 0x0600CC19 RID: 52249 RVA: 0x004A9079 File Offset: 0x004A7279
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseBool(ActionTwitchEndCooldown.PropPlaySound, ref this.playSound);
		}

		// Token: 0x0600CC1A RID: 52250 RVA: 0x004A9093 File Offset: 0x004A7293
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionTwitchEndCooldown
			{
				playSound = this.playSound
			};
		}

		// Token: 0x04009AC3 RID: 39619
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool playSound = true;

		// Token: 0x04009AC4 RID: 39620
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropPlaySound = "play_sound";
	}
}

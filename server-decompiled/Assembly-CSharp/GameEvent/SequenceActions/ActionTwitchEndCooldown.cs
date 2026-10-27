using System;
using Twitch;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A5C RID: 6748
	[Preserve]
	public class ActionTwitchEndCooldown : ActionBaseClientAction
	{
		// Token: 0x0600CD2C RID: 52524 RVA: 0x004ACB5B File Offset: 0x004AAD5B
		public override void OnClientPerform(Entity target)
		{
			if (TwitchManager.HasInstance)
			{
				TwitchManager.Current.ForceEndCooldown(this.playSound);
			}
		}

		// Token: 0x0600CD2D RID: 52525 RVA: 0x004ACB75 File Offset: 0x004AAD75
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseBool(ActionTwitchEndCooldown.PropPlaySound, ref this.playSound);
		}

		// Token: 0x0600CD2E RID: 52526 RVA: 0x004ACB8F File Offset: 0x004AAD8F
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionTwitchEndCooldown
			{
				playSound = this.playSound
			};
		}

		// Token: 0x04009BAC RID: 39852
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool playSound = true;

		// Token: 0x04009BAD RID: 39853
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropPlaySound = "play_sound";
	}
}

using System;
using Twitch;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019FF RID: 6655
	[Preserve]
	public class ActionTwitchEndCooldown : ActionBaseClientAction
	{
		// Token: 0x0600CAC0 RID: 51904 RVA: 0x004A4B93 File Offset: 0x004A2D93
		public override void OnClientPerform(Entity target)
		{
			if (TwitchManager.HasInstance)
			{
				TwitchManager.Current.ForceEndCooldown(this.playSound);
			}
		}

		// Token: 0x0600CAC1 RID: 51905 RVA: 0x004A4BAD File Offset: 0x004A2DAD
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseBool(ActionTwitchEndCooldown.PropPlaySound, ref this.playSound);
		}

		// Token: 0x0600CAC2 RID: 51906 RVA: 0x004A4BC7 File Offset: 0x004A2DC7
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionTwitchEndCooldown
			{
				playSound = this.playSound
			};
		}

		// Token: 0x040099FD RID: 39421
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool playSound = true;

		// Token: 0x040099FE RID: 39422
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropPlaySound = "play_sound";
	}
}

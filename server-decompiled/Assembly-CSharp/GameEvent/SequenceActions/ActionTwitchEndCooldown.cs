using System;
using Twitch;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019D8 RID: 6616
	[Preserve]
	public class ActionTwitchEndCooldown : ActionBaseClientAction
	{
		// Token: 0x0600C937 RID: 51511 RVA: 0x0049E79F File Offset: 0x0049C99F
		public override void OnClientPerform(Entity target)
		{
			if (TwitchManager.HasInstance)
			{
				TwitchManager.Current.ForceEndCooldown(this.playSound);
			}
		}

		// Token: 0x0600C938 RID: 51512 RVA: 0x0049E7B9 File Offset: 0x0049C9B9
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseBool(ActionTwitchEndCooldown.PropPlaySound, ref this.playSound);
		}

		// Token: 0x0600C939 RID: 51513 RVA: 0x0049E7D3 File Offset: 0x0049C9D3
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionTwitchEndCooldown
			{
				playSound = this.playSound
			};
		}

		// Token: 0x040098CC RID: 39116
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool playSound = true;

		// Token: 0x040098CD RID: 39117
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropPlaySound = "play_sound";
	}
}

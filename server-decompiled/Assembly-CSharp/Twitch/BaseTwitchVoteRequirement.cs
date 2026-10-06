using System;

namespace Twitch
{
	// Token: 0x020018A0 RID: 6304
	public class BaseTwitchVoteRequirement
	{
		// Token: 0x0600C29D RID: 49821 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void OnInit()
		{
		}

		// Token: 0x0600C29E RID: 49822 RVA: 0x0047D5D0 File Offset: 0x0047B7D0
		public void Init()
		{
			this.OnInit();
		}

		// Token: 0x0600C29F RID: 49823 RVA: 0x00020119 File Offset: 0x0001E319
		public virtual bool CanPerform(EntityPlayer player)
		{
			return true;
		}

		// Token: 0x0600C2A0 RID: 49824 RVA: 0x0047D5D8 File Offset: 0x0047B7D8
		public virtual void ParseProperties(DynamicProperties properties)
		{
			if (properties.Values.ContainsKey(BaseTwitchVoteRequirement.PropInvert))
			{
				this.Invert = StringParsers.ParseBool(properties.Values[BaseTwitchVoteRequirement.PropInvert], 0, -1, true);
			}
		}

		// Token: 0x0400931E RID: 37662
		public TwitchVote Owner;

		// Token: 0x0400931F RID: 37663
		public bool Invert;

		// Token: 0x04009320 RID: 37664
		public static string PropInvert = "invert";
	}
}

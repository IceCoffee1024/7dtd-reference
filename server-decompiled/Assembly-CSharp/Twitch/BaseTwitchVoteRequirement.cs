using System;

namespace Twitch
{
	// Token: 0x0200184E RID: 6222
	public class BaseTwitchVoteRequirement
	{
		// Token: 0x0600BFC0 RID: 49088 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void OnInit()
		{
		}

		// Token: 0x0600BFC1 RID: 49089 RVA: 0x004732F8 File Offset: 0x004714F8
		public void Init()
		{
			this.OnInit();
		}

		// Token: 0x0600BFC2 RID: 49090 RVA: 0x0001FF5D File Offset: 0x0001E15D
		public virtual bool CanPerform(EntityPlayer player)
		{
			return true;
		}

		// Token: 0x0600BFC3 RID: 49091 RVA: 0x00473300 File Offset: 0x00471500
		public virtual void ParseProperties(DynamicProperties properties)
		{
			if (properties.Values.ContainsKey(BaseTwitchVoteRequirement.PropInvert))
			{
				this.Invert = StringParsers.ParseBool(properties.Values[BaseTwitchVoteRequirement.PropInvert], 0, -1, true);
			}
		}

		// Token: 0x0400913C RID: 37180
		public TwitchVote Owner;

		// Token: 0x0400913D RID: 37181
		public bool Invert;

		// Token: 0x0400913E RID: 37182
		public static string PropInvert = "invert";
	}
}

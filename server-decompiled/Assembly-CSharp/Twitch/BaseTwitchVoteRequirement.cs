using System;

namespace Twitch
{
	// Token: 0x020018D1 RID: 6353
	public class BaseTwitchVoteRequirement
	{
		// Token: 0x0600C3B1 RID: 50097 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void OnInit()
		{
		}

		// Token: 0x0600C3B2 RID: 50098 RVA: 0x00480F7C File Offset: 0x0047F17C
		public void Init()
		{
			this.OnInit();
		}

		// Token: 0x0600C3B3 RID: 50099 RVA: 0x00020119 File Offset: 0x0001E319
		public virtual bool CanPerform(EntityPlayer player)
		{
			return true;
		}

		// Token: 0x0600C3B4 RID: 50100 RVA: 0x00480F84 File Offset: 0x0047F184
		public virtual void ParseProperties(DynamicProperties properties)
		{
			if (properties.Values.ContainsKey(BaseTwitchVoteRequirement.PropInvert))
			{
				this.Invert = StringParsers.ParseBool(properties.Values[BaseTwitchVoteRequirement.PropInvert], 0, -1, true);
			}
		}

		// Token: 0x04009407 RID: 37895
		public TwitchVote Owner;

		// Token: 0x04009408 RID: 37896
		public bool Invert;

		// Token: 0x04009409 RID: 37897
		public static string PropInvert = "invert";
	}
}

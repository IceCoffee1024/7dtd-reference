using System;

namespace Twitch
{
	// Token: 0x02001874 RID: 6260
	public class BaseTwitchVoteRequirement
	{
		// Token: 0x0600C145 RID: 49477 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void OnInit()
		{
		}

		// Token: 0x0600C146 RID: 49478 RVA: 0x00479094 File Offset: 0x00477294
		public void Init()
		{
			this.OnInit();
		}

		// Token: 0x0600C147 RID: 49479 RVA: 0x00020021 File Offset: 0x0001E221
		public virtual bool CanPerform(EntityPlayer player)
		{
			return true;
		}

		// Token: 0x0600C148 RID: 49480 RVA: 0x0047909C File Offset: 0x0047729C
		public virtual void ParseProperties(DynamicProperties properties)
		{
			if (properties.Values.ContainsKey(BaseTwitchVoteRequirement.PropInvert))
			{
				this.Invert = StringParsers.ParseBool(properties.Values[BaseTwitchVoteRequirement.PropInvert], 0, -1, true);
			}
		}

		// Token: 0x04009258 RID: 37464
		public TwitchVote Owner;

		// Token: 0x04009259 RID: 37465
		public bool Invert;

		// Token: 0x0400925A RID: 37466
		public static string PropInvert = "invert";
	}
}

using System;

namespace Twitch.PubSub
{
	// Token: 0x020018ED RID: 6381
	public class TwitchTopic
	{
		// Token: 0x0600C44D RID: 50253 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Private)]
		public TwitchTopic()
		{
		}

		// Token: 0x170018BF RID: 6335
		// (get) Token: 0x0600C44E RID: 50254 RVA: 0x00482436 File Offset: 0x00480636
		// (set) Token: 0x0600C44F RID: 50255 RVA: 0x0048243E File Offset: 0x0048063E
		public string TopicString { get; set; }

		// Token: 0x0600C450 RID: 50256 RVA: 0x00482447 File Offset: 0x00480647
		public static TwitchTopic ChannelPoints(string channelId)
		{
			return new TwitchTopic
			{
				TopicString = string.Format("channel-points-channel-v1.{0}", channelId)
			};
		}

		// Token: 0x0600C451 RID: 50257 RVA: 0x0048245F File Offset: 0x0048065F
		public static TwitchTopic Bits(string channelId)
		{
			return new TwitchTopic
			{
				TopicString = string.Format("channel-bits-events-v2.{0}", channelId)
			};
		}

		// Token: 0x0600C452 RID: 50258 RVA: 0x00482477 File Offset: 0x00480677
		public static TwitchTopic Subscription(string channelId)
		{
			return new TwitchTopic
			{
				TopicString = string.Format("channel-subscribe-events-v1.{0}", channelId)
			};
		}

		// Token: 0x0600C453 RID: 50259 RVA: 0x0048248F File Offset: 0x0048068F
		public static TwitchTopic HypeTrain(string channelId)
		{
			return new TwitchTopic
			{
				TopicString = string.Format("hype-train-events-v1.{0}", channelId)
			};
		}

		// Token: 0x0600C454 RID: 50260 RVA: 0x004824A7 File Offset: 0x004806A7
		public static TwitchTopic CreatorGoal(string channelId)
		{
			return new TwitchTopic
			{
				TopicString = string.Format("creator-goals-events-v1.{0}", channelId)
			};
		}
	}
}

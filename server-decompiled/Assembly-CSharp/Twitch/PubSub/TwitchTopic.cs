using System;

namespace Twitch.PubSub
{
	// Token: 0x020018BC RID: 6332
	public class TwitchTopic
	{
		// Token: 0x0600C339 RID: 49977 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Private)]
		public TwitchTopic()
		{
		}

		// Token: 0x17001877 RID: 6263
		// (get) Token: 0x0600C33A RID: 49978 RVA: 0x0047EA8A File Offset: 0x0047CC8A
		// (set) Token: 0x0600C33B RID: 49979 RVA: 0x0047EA92 File Offset: 0x0047CC92
		public string TopicString { get; set; }

		// Token: 0x0600C33C RID: 49980 RVA: 0x0047EA9B File Offset: 0x0047CC9B
		public static TwitchTopic ChannelPoints(string channelId)
		{
			return new TwitchTopic
			{
				TopicString = string.Format("channel-points-channel-v1.{0}", channelId)
			};
		}

		// Token: 0x0600C33D RID: 49981 RVA: 0x0047EAB3 File Offset: 0x0047CCB3
		public static TwitchTopic Bits(string channelId)
		{
			return new TwitchTopic
			{
				TopicString = string.Format("channel-bits-events-v2.{0}", channelId)
			};
		}

		// Token: 0x0600C33E RID: 49982 RVA: 0x0047EACB File Offset: 0x0047CCCB
		public static TwitchTopic Subscription(string channelId)
		{
			return new TwitchTopic
			{
				TopicString = string.Format("channel-subscribe-events-v1.{0}", channelId)
			};
		}

		// Token: 0x0600C33F RID: 49983 RVA: 0x0047EAE3 File Offset: 0x0047CCE3
		public static TwitchTopic HypeTrain(string channelId)
		{
			return new TwitchTopic
			{
				TopicString = string.Format("hype-train-events-v1.{0}", channelId)
			};
		}

		// Token: 0x0600C340 RID: 49984 RVA: 0x0047EAFB File Offset: 0x0047CCFB
		public static TwitchTopic CreatorGoal(string channelId)
		{
			return new TwitchTopic
			{
				TopicString = string.Format("creator-goals-events-v1.{0}", channelId)
			};
		}
	}
}

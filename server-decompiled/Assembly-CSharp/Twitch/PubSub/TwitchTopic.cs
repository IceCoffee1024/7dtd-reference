using System;

namespace Twitch.PubSub
{
	// Token: 0x02001890 RID: 6288
	public class TwitchTopic
	{
		// Token: 0x0600C1E1 RID: 49633 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Private)]
		public TwitchTopic()
		{
		}

		// Token: 0x170017D5 RID: 6101
		// (get) Token: 0x0600C1E2 RID: 49634 RVA: 0x0047A54E File Offset: 0x0047874E
		// (set) Token: 0x0600C1E3 RID: 49635 RVA: 0x0047A556 File Offset: 0x00478756
		public string TopicString { get; set; }

		// Token: 0x0600C1E4 RID: 49636 RVA: 0x0047A55F File Offset: 0x0047875F
		public static TwitchTopic ChannelPoints(string channelId)
		{
			return new TwitchTopic
			{
				TopicString = string.Format("channel-points-channel-v1.{0}", channelId)
			};
		}

		// Token: 0x0600C1E5 RID: 49637 RVA: 0x0047A577 File Offset: 0x00478777
		public static TwitchTopic Bits(string channelId)
		{
			return new TwitchTopic
			{
				TopicString = string.Format("channel-bits-events-v2.{0}", channelId)
			};
		}

		// Token: 0x0600C1E6 RID: 49638 RVA: 0x0047A58F File Offset: 0x0047878F
		public static TwitchTopic Subscription(string channelId)
		{
			return new TwitchTopic
			{
				TopicString = string.Format("channel-subscribe-events-v1.{0}", channelId)
			};
		}

		// Token: 0x0600C1E7 RID: 49639 RVA: 0x0047A5A7 File Offset: 0x004787A7
		public static TwitchTopic HypeTrain(string channelId)
		{
			return new TwitchTopic
			{
				TopicString = string.Format("hype-train-events-v1.{0}", channelId)
			};
		}

		// Token: 0x0600C1E8 RID: 49640 RVA: 0x0047A5BF File Offset: 0x004787BF
		public static TwitchTopic CreatorGoal(string channelId)
		{
			return new TwitchTopic
			{
				TopicString = string.Format("creator-goals-events-v1.{0}", channelId)
			};
		}
	}
}

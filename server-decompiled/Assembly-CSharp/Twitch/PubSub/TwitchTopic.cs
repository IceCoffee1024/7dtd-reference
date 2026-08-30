using System;

namespace Twitch.PubSub
{
	// Token: 0x0200186A RID: 6250
	public class TwitchTopic
	{
		// Token: 0x0600C05C RID: 49244 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Private)]
		public TwitchTopic()
		{
		}

		// Token: 0x17001763 RID: 5987
		// (get) Token: 0x0600C05D RID: 49245 RVA: 0x004747B2 File Offset: 0x004729B2
		// (set) Token: 0x0600C05E RID: 49246 RVA: 0x004747BA File Offset: 0x004729BA
		public string TopicString { get; set; }

		// Token: 0x0600C05F RID: 49247 RVA: 0x004747C3 File Offset: 0x004729C3
		public static TwitchTopic ChannelPoints(string channelId)
		{
			return new TwitchTopic
			{
				TopicString = string.Format("channel-points-channel-v1.{0}", channelId)
			};
		}

		// Token: 0x0600C060 RID: 49248 RVA: 0x004747DB File Offset: 0x004729DB
		public static TwitchTopic Bits(string channelId)
		{
			return new TwitchTopic
			{
				TopicString = string.Format("channel-bits-events-v2.{0}", channelId)
			};
		}

		// Token: 0x0600C061 RID: 49249 RVA: 0x004747F3 File Offset: 0x004729F3
		public static TwitchTopic Subscription(string channelId)
		{
			return new TwitchTopic
			{
				TopicString = string.Format("channel-subscribe-events-v1.{0}", channelId)
			};
		}

		// Token: 0x0600C062 RID: 49250 RVA: 0x0047480B File Offset: 0x00472A0B
		public static TwitchTopic HypeTrain(string channelId)
		{
			return new TwitchTopic
			{
				TopicString = string.Format("hype-train-events-v1.{0}", channelId)
			};
		}

		// Token: 0x0600C063 RID: 49251 RVA: 0x00474823 File Offset: 0x00472A23
		public static TwitchTopic CreatorGoal(string channelId)
		{
			return new TwitchTopic
			{
				TopicString = string.Format("creator-goals-events-v1.{0}", channelId)
			};
		}
	}
}

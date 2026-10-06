using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Twitch.PubSub
{
	// Token: 0x020018B4 RID: 6324
	public class TwitchPubSub
	{
		// Token: 0x0600C315 RID: 49941 RVA: 0x0047DAB0 File Offset: 0x0047BCB0
		public void Connect(string userID)
		{
			if (this.cts != null)
			{
				this.cts.Cancel();
			}
			this.cts = new CancellationTokenSource();
			Task.Run(() => this.StartAsync(new TwitchTopic[]
			{
				TwitchTopic.ChannelPoints(userID),
				TwitchTopic.Bits(userID),
				TwitchTopic.Subscription(userID),
				TwitchTopic.HypeTrain(userID),
				TwitchTopic.CreatorGoal(userID)
			}, this.cts.Token));
		}

		// Token: 0x0600C316 RID: 49942 RVA: 0x0047DAFF File Offset: 0x0047BCFF
		public void Disconnect()
		{
			this.cts.Cancel();
			TwitchPubSub.reconnect = false;
		}

		// Token: 0x0600C317 RID: 49943 RVA: 0x0047DB14 File Offset: 0x0047BD14
		public async Task StartAsync(TwitchTopic[] newTopics, CancellationToken token)
		{
			TwitchPubSub.<>c__DisplayClass12_0 CS$<>8__locals1 = new TwitchPubSub.<>c__DisplayClass12_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.newTopics = newTopics;
			CS$<>8__locals1.token = token;
			this.topics = CS$<>8__locals1.newTopics;
			TwitchPubSub.reconnect = false;
			this.pingTimer = new global::System.Timers.Timer(TimeSpan.FromSeconds(30.0).TotalMilliseconds);
			this.pingTimer.Elapsed += this.PingTimer_Elapsed;
			this.pingTimer.Start();
			await this.StartListening(CS$<>8__locals1.newTopics);
			while (!CS$<>8__locals1.token.IsCancellationRequested)
			{
				ArraySegment<byte> messageBuffer = new ArraySegment<byte>(new byte[1024]);
				StringBuilder completeMessage = new StringBuilder();
				WebSocketReceiveResult webSocketReceiveResult = await this.socket.ReceiveAsync(messageBuffer, CS$<>8__locals1.token);
				completeMessage.Append(Encoding.UTF8.GetString(messageBuffer.Array));
				while (!webSocketReceiveResult.EndOfMessage)
				{
					messageBuffer = new ArraySegment<byte>(new byte[1024]);
					webSocketReceiveResult = await this.socket.ReceiveAsync(messageBuffer, CS$<>8__locals1.token);
					completeMessage.Append(Encoding.UTF8.GetString(messageBuffer.Array));
				}
				if (webSocketReceiveResult.MessageType == WebSocketMessageType.Close)
				{
					TwitchPubSub.reconnect = true;
					break;
				}
				try
				{
					string text = completeMessage.ToString();
					this.HandleMessage(text, text.Contains("hype-train-start") ? TwitchPubSub.MessageTypes.HypeStart : TwitchPubSub.MessageTypes.Standard);
				}
				catch (Exception ex)
				{
					Debug.LogWarning(completeMessage.ToString());
					Debug.LogWarning(ex.ToString());
					if (ex.InnerException != null)
					{
						Debug.LogWarning(ex.InnerException.ToString());
					}
					TwitchPubSub.reconnect = true;
				}
				if (TwitchPubSub.reconnect)
				{
					IEnumerable<TimeSpan> reconnectTimeouts = TwitchPubSub._ReconnectTimeouts;
					Func<TimeSpan, bool> func;
					if ((func = CS$<>8__locals1.<>9__1) == null)
					{
						TwitchPubSub.<>c__DisplayClass12_0 CS$<>8__locals2 = CS$<>8__locals1;
						Func<TimeSpan, bool> func2 = (TimeSpan t) => t.TotalMilliseconds == CS$<>8__locals1.<>4__this.reconnectTimer.Interval;
						CS$<>8__locals2.<>9__1 = func2;
						func = func2;
					}
					if (!reconnectTimeouts.Any<TimeSpan>(func))
					{
						this.reconnectTimer.Interval = TwitchPubSub._ReconnectTimeouts[0].TotalMilliseconds;
					}
					else if (TwitchPubSub._ReconnectTimeouts.Last<TimeSpan>().TotalMilliseconds == this.reconnectTimer.Interval)
					{
						TwitchPubSub.reconnect = false;
					}
					else
					{
						for (int i = 0; i < TwitchPubSub._ReconnectTimeouts.Length; i++)
						{
							if (TwitchPubSub._ReconnectTimeouts[i].TotalMilliseconds == this.reconnectTimer.Interval)
							{
								this.reconnectTimer.Interval = TwitchPubSub._ReconnectTimeouts[i + 1].TotalMilliseconds;
								break;
							}
						}
					}
					await Task.Delay((int)this.reconnectTimer.Interval);
					break;
				}
				messageBuffer = default(ArraySegment<byte>);
				completeMessage = null;
			}
			if (TwitchPubSub.reconnect)
			{
				Task.Run(() => CS$<>8__locals1.<>4__this.StartAsync(CS$<>8__locals1.newTopics, CS$<>8__locals1.token));
			}
		}

		// Token: 0x0600C318 RID: 49944 RVA: 0x0047DB68 File Offset: 0x0047BD68
		[PublicizedFrom(EAccessModifier.Private)]
		public void HandleMessage(string receivedMessage, TwitchPubSub.MessageTypes msgType)
		{
			if (msgType != TwitchPubSub.MessageTypes.Standard)
			{
				if (msgType == TwitchPubSub.MessageTypes.HypeStart)
				{
					TwitchManager.Current.StartHypeTrain();
				}
				return;
			}
			JObject jobject = JObject.Parse(receivedMessage);
			string text = jobject["type"].Value<string>();
			if (text == "RESPONSE" && jobject["error"].Value<string>() != "")
			{
				return;
			}
			if (text == "RESPONSE")
			{
				return;
			}
			if (this.HandlePongMessage(receivedMessage))
			{
				return;
			}
			if (this.HandleReconnectMessage(receivedMessage))
			{
				return;
			}
			this.HandleRedemptionsMessages(receivedMessage);
		}

		// Token: 0x0600C319 RID: 49945 RVA: 0x0047DBF4 File Offset: 0x0047BDF4
		[PublicizedFrom(EAccessModifier.Private)]
		public async Task StartListening(IEnumerable<TwitchTopic> topics)
		{
			this.socket = new ClientWebSocket();
			PubSubListenMessage msg = new PubSubListenMessage();
			PubSubListenMessage msg2 = msg;
			PubSubListenMessage.PubSubListenData pubSubListenData = new PubSubListenMessage.PubSubListenData();
			pubSubListenData.auth_token = TwitchManager.Current.Authentication.oauth.Substring(6);
			pubSubListenData.topics = topics.Select<TwitchTopic, string>((TwitchTopic t) => t.TopicString).ToArray<string>();
			msg2.data = pubSubListenData;
			await this.socket.ConnectAsync(new Uri("wss://pubsub-edge.twitch.tv"), CancellationToken.None).ContinueWith<Task>((Task t) => this.SendMessageOnSocket(JsonConvert.SerializeObject(msg)));
		}

		// Token: 0x0600C31A RID: 49946 RVA: 0x0047DC40 File Offset: 0x0047BE40
		[PublicizedFrom(EAccessModifier.Private)]
		public void PingTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			string text = "{ \"type\": \"PING\" }";
			this.SendMessageOnSocket(text).GetAwaiter().GetResult();
			this.pongTimer = new global::System.Timers.Timer(TimeSpan.FromSeconds(10.0).TotalMilliseconds);
			this.pongTimer.Elapsed += this.PongTimer_Elapsed;
			this.pongTimer.Start();
			this.pingAcknowledged = false;
		}

		// Token: 0x0600C31B RID: 49947 RVA: 0x0047DCB1 File Offset: 0x0047BEB1
		[PublicizedFrom(EAccessModifier.Private)]
		public void PongTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			if (!this.pingAcknowledged)
			{
				TwitchPubSub.reconnect = true;
				this.pongTimer.Dispose();
			}
		}

		// Token: 0x0600C31C RID: 49948 RVA: 0x0047DCCC File Offset: 0x0047BECC
		[PublicizedFrom(EAccessModifier.Private)]
		public Task SendMessageOnSocket(string message)
		{
			if (this.socket.State != WebSocketState.Open)
			{
				return Task.CompletedTask;
			}
			byte[] bytes = Encoding.ASCII.GetBytes(message);
			return this.socket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
		}

		// Token: 0x14000112 RID: 274
		// (add) Token: 0x0600C31D RID: 49949 RVA: 0x0047DD14 File Offset: 0x0047BF14
		// (remove) Token: 0x0600C31E RID: 49950 RVA: 0x0047DD4C File Offset: 0x0047BF4C
		public event EventHandler<PubSubBitRedemptionMessage.BitRedemptionData> OnBitsRedeemed;

		// Token: 0x14000113 RID: 275
		// (add) Token: 0x0600C31F RID: 49951 RVA: 0x0047DD84 File Offset: 0x0047BF84
		// (remove) Token: 0x0600C320 RID: 49952 RVA: 0x0047DDBC File Offset: 0x0047BFBC
		public event EventHandler<PubSubSubscriptionRedemptionMessage> OnSubscriptionRedeemed;

		// Token: 0x14000114 RID: 276
		// (add) Token: 0x0600C321 RID: 49953 RVA: 0x0047DDF4 File Offset: 0x0047BFF4
		// (remove) Token: 0x0600C322 RID: 49954 RVA: 0x0047DE2C File Offset: 0x0047C02C
		public event EventHandler<PubSubChannelPointMessage.ChannelRedemptionData> OnChannelPointsRedeemed;

		// Token: 0x14000115 RID: 277
		// (add) Token: 0x0600C323 RID: 49955 RVA: 0x0047DE64 File Offset: 0x0047C064
		// (remove) Token: 0x0600C324 RID: 49956 RVA: 0x0047DE9C File Offset: 0x0047C09C
		public event EventHandler<PubSubGoalMessage.Goal> OnGoalAchieved;

		// Token: 0x0600C325 RID: 49957 RVA: 0x0047DED1 File Offset: 0x0047C0D1
		[PublicizedFrom(EAccessModifier.Private)]
		public bool HandlePongMessage(string message)
		{
			if (message.Contains("\"PONG\""))
			{
				this.pingAcknowledged = true;
				this.pongTimer.Stop();
				this.pongTimer.Dispose();
				return true;
			}
			return false;
		}

		// Token: 0x0600C326 RID: 49958 RVA: 0x0047DF00 File Offset: 0x0047C100
		[PublicizedFrom(EAccessModifier.Private)]
		public bool HandleReconnectMessage(string message)
		{
			if (message.Contains("\"RECONNECT\""))
			{
				TwitchPubSub.reconnect = true;
				return true;
			}
			return false;
		}

		// Token: 0x0600C327 RID: 49959 RVA: 0x0047DF18 File Offset: 0x0047C118
		[PublicizedFrom(EAccessModifier.Private)]
		public bool HandleRedemptionsMessages(string message)
		{
			JObject jobject = JObject.Parse(message);
			if (jobject["type"].Value<string>() == "MESSAGE")
			{
				string text = jobject["data"]["topic"].Value<string>();
				if (text.StartsWith("channel-points-channel-v1"))
				{
					string text2 = jobject["data"]["message"].Value<string>();
					PubSubChannelPointMessage pubSubChannelPointMessage = null;
					try
					{
						pubSubChannelPointMessage = PubSubChannelPointMessage.Deserialize(text2);
					}
					catch (Exception ex)
					{
						Debug.LogError(ex.ToString());
						Debug.LogError(text2);
					}
					if (this.OnChannelPointsRedeemed != null && pubSubChannelPointMessage != null)
					{
						this.OnChannelPointsRedeemed(null, pubSubChannelPointMessage.data);
					}
					return true;
				}
				if (text.StartsWith("channel-bits-events"))
				{
					string text3 = jobject["data"]["message"].Value<string>();
					PubSubBitRedemptionMessage pubSubBitRedemptionMessage = null;
					try
					{
						pubSubBitRedemptionMessage = PubSubBitRedemptionMessage.Deserialize(text3);
					}
					catch (Exception ex2)
					{
						Debug.LogError(ex2.ToString());
						Debug.LogError(text3);
					}
					if (this.OnBitsRedeemed != null && pubSubBitRedemptionMessage != null)
					{
						this.OnBitsRedeemed(null, pubSubBitRedemptionMessage.data);
					}
					return true;
				}
				if (text.StartsWith("channel-subscribe-events"))
				{
					string text4 = jobject["data"]["message"].Value<string>();
					PubSubSubscriptionRedemptionMessage pubSubSubscriptionRedemptionMessage = null;
					try
					{
						pubSubSubscriptionRedemptionMessage = PubSubSubscriptionRedemptionMessage.Deserialize(text4);
					}
					catch (Exception ex3)
					{
						Debug.LogError(ex3.ToString());
						Debug.LogError(text4);
					}
					if (this.OnSubscriptionRedeemed != null && pubSubSubscriptionRedemptionMessage != null)
					{
						this.OnSubscriptionRedeemed(null, pubSubSubscriptionRedemptionMessage);
					}
					return true;
				}
				if (text.StartsWith("creator-goals-events"))
				{
					string text5 = jobject["data"]["message"].Value<string>();
					PubSubGoalMessage pubSubGoalMessage = null;
					try
					{
						pubSubGoalMessage = PubSubGoalMessage.Deserialize(text5);
					}
					catch (Exception ex4)
					{
						Debug.LogError(ex4.ToString());
						Debug.LogError(text5);
					}
					if (pubSubGoalMessage.type == "goal_achieved" && this.OnGoalAchieved != null && pubSubGoalMessage != null)
					{
						this.OnGoalAchieved(null, pubSubGoalMessage.data.goal);
					}
					return true;
				}
				if (text.StartsWith("hype-train-events-v1"))
				{
					try
					{
						string text6 = jobject["data"]["message"].ToString();
						Debug.LogWarning(text6);
						if (text6.Contains("hype-train-start"))
						{
							TwitchManager.Current.StartHypeTrain();
						}
						else if (text6.Contains("hype-train-level-up"))
						{
							TwitchManager.Current.IncrementHypeTrainLevel();
						}
						else if (text6.Contains("hype-train-end"))
						{
							TwitchManager.Current.EndHypeTrain();
						}
					}
					catch (Exception ex5)
					{
						Debug.LogWarning("Hype Train Error: " + message);
						Debug.LogWarning("Hype Train Exception: " + ex5.ToString());
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600C328 RID: 49960 RVA: 0x0047E20C File Offset: 0x0047C40C
		public void Cleanup()
		{
			if (this.pingTimer != null)
			{
				this.pingTimer.Dispose();
			}
			if (this.pongTimer != null)
			{
				this.pongTimer.Dispose();
			}
			if (this.socket != null)
			{
				this.socket.Dispose();
			}
		}

		// Token: 0x04009352 RID: 37714
		[PublicizedFrom(EAccessModifier.Private)]
		public ClientWebSocket socket;

		// Token: 0x04009353 RID: 37715
		[PublicizedFrom(EAccessModifier.Private)]
		public global::System.Timers.Timer pingTimer;

		// Token: 0x04009354 RID: 37716
		[PublicizedFrom(EAccessModifier.Private)]
		public global::System.Timers.Timer pongTimer;

		// Token: 0x04009355 RID: 37717
		[PublicizedFrom(EAccessModifier.Private)]
		public global::System.Timers.Timer reconnectTimer = new global::System.Timers.Timer();

		// Token: 0x04009356 RID: 37718
		[PublicizedFrom(EAccessModifier.Private)]
		public bool pingAcknowledged;

		// Token: 0x04009357 RID: 37719
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool reconnect = false;

		// Token: 0x04009358 RID: 37720
		[PublicizedFrom(EAccessModifier.Private)]
		public TwitchTopic[] topics;

		// Token: 0x04009359 RID: 37721
		[PublicizedFrom(EAccessModifier.Private)]
		public CancellationTokenSource cts;

		// Token: 0x0400935A RID: 37722
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly TimeSpan[] _ReconnectTimeouts = new TimeSpan[]
		{
			TimeSpan.FromSeconds(1.0),
			TimeSpan.FromSeconds(5.0),
			TimeSpan.FromSeconds(10.0),
			TimeSpan.FromSeconds(30.0),
			TimeSpan.FromMinutes(1.0),
			TimeSpan.FromMinutes(5.0)
		};

		// Token: 0x020018B5 RID: 6325
		[PublicizedFrom(EAccessModifier.Private)]
		public enum MessageTypes
		{
			// Token: 0x04009360 RID: 37728
			Standard,
			// Token: 0x04009361 RID: 37729
			HypeStart
		}
	}
}

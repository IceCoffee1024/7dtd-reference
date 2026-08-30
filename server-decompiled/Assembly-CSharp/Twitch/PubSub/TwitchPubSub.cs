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
	// Token: 0x02001888 RID: 6280
	public class TwitchPubSub
	{
		// Token: 0x0600C1BD RID: 49597 RVA: 0x00479574 File Offset: 0x00477774
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

		// Token: 0x0600C1BE RID: 49598 RVA: 0x004795C3 File Offset: 0x004777C3
		public void Disconnect()
		{
			this.cts.Cancel();
			TwitchPubSub.reconnect = false;
		}

		// Token: 0x0600C1BF RID: 49599 RVA: 0x004795D8 File Offset: 0x004777D8
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

		// Token: 0x0600C1C0 RID: 49600 RVA: 0x0047962C File Offset: 0x0047782C
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

		// Token: 0x0600C1C1 RID: 49601 RVA: 0x004796B8 File Offset: 0x004778B8
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

		// Token: 0x0600C1C2 RID: 49602 RVA: 0x00479704 File Offset: 0x00477904
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

		// Token: 0x0600C1C3 RID: 49603 RVA: 0x00479775 File Offset: 0x00477975
		[PublicizedFrom(EAccessModifier.Private)]
		public void PongTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			if (!this.pingAcknowledged)
			{
				TwitchPubSub.reconnect = true;
				this.pongTimer.Dispose();
			}
		}

		// Token: 0x0600C1C4 RID: 49604 RVA: 0x00479790 File Offset: 0x00477990
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

		// Token: 0x14000113 RID: 275
		// (add) Token: 0x0600C1C5 RID: 49605 RVA: 0x004797D8 File Offset: 0x004779D8
		// (remove) Token: 0x0600C1C6 RID: 49606 RVA: 0x00479810 File Offset: 0x00477A10
		public event EventHandler<PubSubBitRedemptionMessage.BitRedemptionData> OnBitsRedeemed;

		// Token: 0x14000114 RID: 276
		// (add) Token: 0x0600C1C7 RID: 49607 RVA: 0x00479848 File Offset: 0x00477A48
		// (remove) Token: 0x0600C1C8 RID: 49608 RVA: 0x00479880 File Offset: 0x00477A80
		public event EventHandler<PubSubSubscriptionRedemptionMessage> OnSubscriptionRedeemed;

		// Token: 0x14000115 RID: 277
		// (add) Token: 0x0600C1C9 RID: 49609 RVA: 0x004798B8 File Offset: 0x00477AB8
		// (remove) Token: 0x0600C1CA RID: 49610 RVA: 0x004798F0 File Offset: 0x00477AF0
		public event EventHandler<PubSubChannelPointMessage.ChannelRedemptionData> OnChannelPointsRedeemed;

		// Token: 0x14000116 RID: 278
		// (add) Token: 0x0600C1CB RID: 49611 RVA: 0x00479928 File Offset: 0x00477B28
		// (remove) Token: 0x0600C1CC RID: 49612 RVA: 0x00479960 File Offset: 0x00477B60
		public event EventHandler<PubSubGoalMessage.Goal> OnGoalAchieved;

		// Token: 0x0600C1CD RID: 49613 RVA: 0x00479995 File Offset: 0x00477B95
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

		// Token: 0x0600C1CE RID: 49614 RVA: 0x004799C4 File Offset: 0x00477BC4
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

		// Token: 0x0600C1CF RID: 49615 RVA: 0x004799DC File Offset: 0x00477BDC
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

		// Token: 0x0600C1D0 RID: 49616 RVA: 0x00479CD0 File Offset: 0x00477ED0
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

		// Token: 0x0400928C RID: 37516
		[PublicizedFrom(EAccessModifier.Private)]
		public ClientWebSocket socket;

		// Token: 0x0400928D RID: 37517
		[PublicizedFrom(EAccessModifier.Private)]
		public global::System.Timers.Timer pingTimer;

		// Token: 0x0400928E RID: 37518
		[PublicizedFrom(EAccessModifier.Private)]
		public global::System.Timers.Timer pongTimer;

		// Token: 0x0400928F RID: 37519
		[PublicizedFrom(EAccessModifier.Private)]
		public global::System.Timers.Timer reconnectTimer = new global::System.Timers.Timer();

		// Token: 0x04009290 RID: 37520
		[PublicizedFrom(EAccessModifier.Private)]
		public bool pingAcknowledged;

		// Token: 0x04009291 RID: 37521
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool reconnect = false;

		// Token: 0x04009292 RID: 37522
		[PublicizedFrom(EAccessModifier.Private)]
		public TwitchTopic[] topics;

		// Token: 0x04009293 RID: 37523
		[PublicizedFrom(EAccessModifier.Private)]
		public CancellationTokenSource cts;

		// Token: 0x04009294 RID: 37524
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

		// Token: 0x02001889 RID: 6281
		[PublicizedFrom(EAccessModifier.Private)]
		public enum MessageTypes
		{
			// Token: 0x0400929A RID: 37530
			Standard,
			// Token: 0x0400929B RID: 37531
			HypeStart
		}
	}
}

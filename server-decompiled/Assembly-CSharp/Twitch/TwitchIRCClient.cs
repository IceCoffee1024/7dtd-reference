using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

namespace Twitch
{
	// Token: 0x02001869 RID: 6249
	public class TwitchIRCClient
	{
		// Token: 0x1700181F RID: 6175
		// (get) Token: 0x0600C0B8 RID: 49336 RVA: 0x0046E344 File Offset: 0x0046C544
		public bool IsConnected
		{
			get
			{
				return this.tcpClient.Connected;
			}
		}

		// Token: 0x0600C0B9 RID: 49337 RVA: 0x0046E354 File Offset: 0x0046C554
		public TwitchIRCClient(string ip, int port, string channel, string password)
		{
			this.userName = channel;
			this.password = password;
			this.channel = channel;
			this.ip = ip;
			this.port = port;
			this.Reconnect();
		}

		// Token: 0x0600C0BA RID: 49338 RVA: 0x0046E3A8 File Offset: 0x0046C5A8
		public void Reconnect()
		{
			this.tcpClient = new TcpClient(this.ip, this.port);
			this.inputStream = new StreamReader(this.tcpClient.GetStream());
			this.outputStream = new StreamWriter(this.tcpClient.GetStream());
			this.outputStream.WriteLine("PASS " + this.password);
			this.outputStream.WriteLine("NICK " + this.userName);
			this.outputStream.WriteLine("JOIN #" + this.channel);
			this.pingTimerRunning = true;
			this.outputStream.Flush();
		}

		// Token: 0x0600C0BB RID: 49339 RVA: 0x0046E45B File Offset: 0x0046C65B
		public void Disconnect()
		{
			if (this.tcpClient != null)
			{
				this.tcpClient.Close();
			}
			if (this.inputStream != null)
			{
				this.inputStream.Close();
			}
			if (this.outputStream != null)
			{
				this.outputStream.Close();
			}
		}

		// Token: 0x0600C0BC RID: 49340 RVA: 0x0046E498 File Offset: 0x0046C698
		public bool Update(float deltaTime)
		{
			if (this.pingTimerRunning)
			{
				this.PingTimer -= deltaTime;
				if (this.PingTimer <= 0f)
				{
					if (this.tcpClient.Connected)
					{
						this.SendIrcMessage("PING irc.twitch.tv", false);
					}
					else
					{
						this.Reconnect();
						this.pingTimerRunning = false;
					}
					this.PingTimer = 250f;
				}
			}
			if (this.outputQueue.Count > 0)
			{
				this.outputStream.WriteLine(this.outputQueue[0]);
				this.outputQueue.RemoveAt(0);
				this.outputStream.Flush();
			}
			return true;
		}

		// Token: 0x0600C0BD RID: 49341 RVA: 0x0046E538 File Offset: 0x0046C738
		public void SendIrcMessage(string message, bool useQueue)
		{
			if (!this.tcpClient.Connected)
			{
				this.Reconnect();
			}
			if (useQueue)
			{
				this.outputQueue.Add(message);
				return;
			}
			this.outputStream.WriteLine(message);
			this.outputStream.Flush();
		}

		// Token: 0x0600C0BE RID: 49342 RVA: 0x0046E574 File Offset: 0x0046C774
		public void SendIrcMessages(List<string> messages, bool useQueue)
		{
			if (useQueue)
			{
				this.outputQueue.AddRange(messages);
				return;
			}
			for (int i = 0; i < messages.Count; i++)
			{
				this.outputStream.WriteLine(messages[i]);
			}
			this.outputStream.Flush();
		}

		// Token: 0x0600C0BF RID: 49343 RVA: 0x0046E5C0 File Offset: 0x0046C7C0
		public void SendChannelMessage(string message, bool useQueue)
		{
			if (useQueue)
			{
				this.outputQueue.Add("PRIVMSG #" + this.userName + " :/me " + message);
				return;
			}
			this.outputStream.WriteLine("PRIVMSG #" + this.userName + " :/me " + message);
			this.outputStream.Flush();
		}

		// Token: 0x0600C0C0 RID: 49344 RVA: 0x0046E620 File Offset: 0x0046C820
		public void SendChannelMessages(List<string> messages, bool useQueue)
		{
			if (useQueue)
			{
				for (int i = 0; i < messages.Count; i++)
				{
					this.outputQueue.Add("PRIVMSG #" + this.userName + " :/me " + messages[i]);
				}
				return;
			}
			for (int j = 0; j < messages.Count; j++)
			{
				this.outputStream.WriteLine("PRIVMSG #" + this.userName + " :/me " + messages[j]);
			}
			this.outputStream.Flush();
		}

		// Token: 0x0600C0C1 RID: 49345 RVA: 0x0046E6AC File Offset: 0x0046C8AC
		public bool AvailableMessage()
		{
			return this.tcpClient.Available > 0;
		}

		// Token: 0x0600C0C2 RID: 49346 RVA: 0x0046E6BC File Offset: 0x0046C8BC
		public TwitchIRCClient.TwitchChatMessage ReadMessage()
		{
			return this.ParseMessage();
		}

		// Token: 0x0600C0C3 RID: 49347 RVA: 0x0046E6C4 File Offset: 0x0046C8C4
		public TwitchIRCClient.TwitchChatMessage ParseMessage()
		{
			return new TwitchIRCClient.TwitchChatMessage(this.inputStream.ReadLine());
		}

		// Token: 0x0600C0C4 RID: 49348 RVA: 0x0046E6D6 File Offset: 0x0046C8D6
		public void SendChatMessage(string message)
		{
			this.SendIrcMessage(string.Format(":{0}!{0}@{0}.tmi.twitch.tv PRIVMSG #{1} :{2}", this.userName, this.channel, message), true);
		}

		// Token: 0x04009098 RID: 37016
		[PublicizedFrom(EAccessModifier.Private)]
		public string userName;

		// Token: 0x04009099 RID: 37017
		[PublicizedFrom(EAccessModifier.Private)]
		public string channel;

		// Token: 0x0400909A RID: 37018
		[PublicizedFrom(EAccessModifier.Private)]
		public string password;

		// Token: 0x0400909B RID: 37019
		[PublicizedFrom(EAccessModifier.Private)]
		public string ip;

		// Token: 0x0400909C RID: 37020
		[PublicizedFrom(EAccessModifier.Private)]
		public int port;

		// Token: 0x0400909D RID: 37021
		[PublicizedFrom(EAccessModifier.Private)]
		public TcpClient tcpClient;

		// Token: 0x0400909E RID: 37022
		[PublicizedFrom(EAccessModifier.Private)]
		public StreamReader inputStream;

		// Token: 0x0400909F RID: 37023
		[PublicizedFrom(EAccessModifier.Private)]
		public StreamWriter outputStream;

		// Token: 0x040090A0 RID: 37024
		[PublicizedFrom(EAccessModifier.Private)]
		public static float pingMaxTimer = 100f;

		// Token: 0x040090A1 RID: 37025
		[PublicizedFrom(EAccessModifier.Private)]
		public float PingTimer = TwitchIRCClient.pingMaxTimer;

		// Token: 0x040090A2 RID: 37026
		[PublicizedFrom(EAccessModifier.Private)]
		public bool pingTimerRunning;

		// Token: 0x040090A3 RID: 37027
		public List<string> outputQueue = new List<string>();

		// Token: 0x040090A4 RID: 37028
		[PublicizedFrom(EAccessModifier.Private)]
		public static string TWITCH_SYSTEM_STRING = "tmi.twitch.tv";

		// Token: 0x040090A5 RID: 37029
		[PublicizedFrom(EAccessModifier.Private)]
		public static string TWITCH_CONNECTION_STRING = ":tmi.twitch.tv 001";

		// Token: 0x040090A6 RID: 37030
		[PublicizedFrom(EAccessModifier.Private)]
		public static string PRIV_MSG_STRING = "PRIVMSG";

		// Token: 0x040090A7 RID: 37031
		[PublicizedFrom(EAccessModifier.Private)]
		public static string PRIV_MSG_STRING_PARSE = "PRIVMSG #";

		// Token: 0x040090A8 RID: 37032
		[PublicizedFrom(EAccessModifier.Private)]
		public static string MSG_RAID_STRING = "msg-id=raid";

		// Token: 0x040090A9 RID: 37033
		[PublicizedFrom(EAccessModifier.Private)]
		public static string MSG_CHARITY_STRING = "msg-id=charitydonation";

		// Token: 0x0200186A RID: 6250
		public class TwitchChatMessage
		{
			// Token: 0x17001820 RID: 6176
			// (get) Token: 0x0600C0C6 RID: 49350 RVA: 0x0046E74B File Offset: 0x0046C94B
			// (set) Token: 0x0600C0C7 RID: 49351 RVA: 0x0046E753 File Offset: 0x0046C953
			public virtual TwitchIRCClient.TwitchChatMessage.MessageTypes MessageType
			{
				get; [PublicizedFrom(EAccessModifier.Private)]
				set;
			}

			// Token: 0x0600C0C8 RID: 49352 RVA: 0x0046E75C File Offset: 0x0046C95C
			public TwitchChatMessage(string message)
			{
				if (message.IndexOf(TwitchIRCClient.TWITCH_SYSTEM_STRING) != -1)
				{
					if (message.StartsWith(TwitchIRCClient.TWITCH_CONNECTION_STRING))
					{
						this.Message = message;
						this.MessageType = TwitchIRCClient.TwitchChatMessage.MessageTypes.Authenticated;
						return;
					}
					int num = -1;
					int num2 = -1;
					if (message.Contains(TwitchIRCClient.PRIV_MSG_STRING))
					{
						string[] array = message.Split(';', StringSplitOptions.None);
						for (int i = 0; i < array.Length; i++)
						{
							if (array[i].StartsWith("@badge-info"))
							{
								if (array[i].Length >= 15 && (array[i][12] == 'f' || array[i][12] == 's'))
								{
									this.isSub = true;
								}
							}
							else if (array[i].StartsWith("badges"))
							{
								if (array[i].Contains("broadcaster"))
								{
									this.isSub = true;
									this.isMod = true;
									this.isVIP = true;
									this.isBroadcaster = true;
								}
								else if (array[i].Contains("vip"))
								{
									this.isVIP = true;
								}
							}
							else if (array[i].StartsWith("mod"))
							{
								if (array[i][4] == '1')
								{
									this.isMod = true;
								}
							}
							else if (array[i].StartsWith("user-type"))
							{
								message = message.Substring(message.IndexOf('@', 1) + 1);
							}
							else if (array[i].StartsWith("user-id"))
							{
								this.UserID = Convert.ToInt32(array[i].Substring(8));
							}
							else if (array[i].StartsWith("room-id"))
							{
								num2 = Convert.ToInt32(array[i].Substring(8));
							}
							else if (array[i].StartsWith("source-room-id"))
							{
								num = Convert.ToInt32(array[i].Substring(15));
							}
							else if (array[i].StartsWith("color"))
							{
								if (array[i].Length > 7)
								{
									this.UserNameColor = array[i].Substring(7);
								}
							}
							else if (array[i].StartsWith("reply-parent-msg-body"))
							{
								this.MessageType = TwitchIRCClient.TwitchChatMessage.MessageTypes.Invalid;
								return;
							}
						}
						if (num2 != num && num != -1)
						{
							this.MessageType = TwitchIRCClient.TwitchChatMessage.MessageTypes.Invalid;
							return;
						}
						message.IndexOf(TwitchIRCClient.PRIV_MSG_STRING_PARSE);
						int num3 = message.IndexOf('.', 1);
						string text = message.Substring(0, num3);
						num3 = message.IndexOf(":");
						message = message.Substring(num3 + 1);
						this.UserName = text;
						this.Message = message;
						this.MessageType = TwitchIRCClient.TwitchChatMessage.MessageTypes.Message;
						return;
					}
					else
					{
						if (message.Contains(TwitchIRCClient.MSG_RAID_STRING))
						{
							string[] array2 = message.Split(';', StringSplitOptions.None);
							for (int j = 0; j < array2.Length; j++)
							{
								if (array2[j].StartsWith("msg-param-displayName"))
								{
									this.UserName = array2[j].Substring(22);
								}
								else if (array2[j].StartsWith("msg-param-viewerCount"))
								{
									this.Message = array2[j].Substring(22);
								}
								else if (array2[j].StartsWith("user-id"))
								{
									this.UserID = Convert.ToInt32(array2[j].Substring(8));
								}
							}
							this.MessageType = TwitchIRCClient.TwitchChatMessage.MessageTypes.Raid;
							return;
						}
						if (message.Contains(TwitchIRCClient.MSG_CHARITY_STRING))
						{
							string[] array3 = message.Split(';', StringSplitOptions.None);
							for (int k = 0; k < array3.Length; k++)
							{
								if (array3[k].StartsWith("display-name"))
								{
									this.UserName = array3[k].Substring(13);
								}
								else if (array3[k].StartsWith("msg-param-donation-amount"))
								{
									this.Message = array3[k].Substring(26);
								}
								else if (array3[k].StartsWith("user-id"))
								{
									this.UserID = Convert.ToInt32(array3[k].Substring(8));
								}
							}
							this.MessageType = TwitchIRCClient.TwitchChatMessage.MessageTypes.Charity;
							return;
						}
					}
				}
				this.Message = message;
				this.MessageType = TwitchIRCClient.TwitchChatMessage.MessageTypes.Output;
			}

			// Token: 0x040090AB RID: 37035
			public bool isMod;

			// Token: 0x040090AC RID: 37036
			public bool isVIP;

			// Token: 0x040090AD RID: 37037
			public bool isSub;

			// Token: 0x040090AE RID: 37038
			public bool isBroadcaster;

			// Token: 0x040090AF RID: 37039
			public string UserName;

			// Token: 0x040090B0 RID: 37040
			public int UserID;

			// Token: 0x040090B1 RID: 37041
			public string UserNameColor = "FFFFFF";

			// Token: 0x040090B2 RID: 37042
			public string Message;

			// Token: 0x0200186B RID: 6251
			public enum MessageTypes
			{
				// Token: 0x040090B4 RID: 37044
				Invalid = -1,
				// Token: 0x040090B5 RID: 37045
				Message,
				// Token: 0x040090B6 RID: 37046
				Output,
				// Token: 0x040090B7 RID: 37047
				Authenticated,
				// Token: 0x040090B8 RID: 37048
				Raid,
				// Token: 0x040090B9 RID: 37049
				Charity
			}
		}
	}
}

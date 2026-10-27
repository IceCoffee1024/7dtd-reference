using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

namespace Twitch
{
	// Token: 0x0200189A RID: 6298
	public class TwitchIRCClient
	{
		// Token: 0x17001867 RID: 6247
		// (get) Token: 0x0600C1CC RID: 49612 RVA: 0x00471CF0 File Offset: 0x0046FEF0
		public bool IsConnected
		{
			get
			{
				return this.tcpClient.Connected;
			}
		}

		// Token: 0x0600C1CD RID: 49613 RVA: 0x00471D00 File Offset: 0x0046FF00
		public TwitchIRCClient(string ip, int port, string channel, string password)
		{
			this.userName = channel;
			this.password = password;
			this.channel = channel;
			this.ip = ip;
			this.port = port;
			this.Reconnect();
		}

		// Token: 0x0600C1CE RID: 49614 RVA: 0x00471D54 File Offset: 0x0046FF54
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

		// Token: 0x0600C1CF RID: 49615 RVA: 0x00471E07 File Offset: 0x00470007
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

		// Token: 0x0600C1D0 RID: 49616 RVA: 0x00471E44 File Offset: 0x00470044
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

		// Token: 0x0600C1D1 RID: 49617 RVA: 0x00471EE4 File Offset: 0x004700E4
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

		// Token: 0x0600C1D2 RID: 49618 RVA: 0x00471F20 File Offset: 0x00470120
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

		// Token: 0x0600C1D3 RID: 49619 RVA: 0x00471F6C File Offset: 0x0047016C
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

		// Token: 0x0600C1D4 RID: 49620 RVA: 0x00471FCC File Offset: 0x004701CC
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

		// Token: 0x0600C1D5 RID: 49621 RVA: 0x00472058 File Offset: 0x00470258
		public bool AvailableMessage()
		{
			return this.tcpClient.Available > 0;
		}

		// Token: 0x0600C1D6 RID: 49622 RVA: 0x00472068 File Offset: 0x00470268
		public TwitchIRCClient.TwitchChatMessage ReadMessage()
		{
			return this.ParseMessage();
		}

		// Token: 0x0600C1D7 RID: 49623 RVA: 0x00472070 File Offset: 0x00470270
		public TwitchIRCClient.TwitchChatMessage ParseMessage()
		{
			return new TwitchIRCClient.TwitchChatMessage(this.inputStream.ReadLine());
		}

		// Token: 0x0600C1D8 RID: 49624 RVA: 0x00472082 File Offset: 0x00470282
		public void SendChatMessage(string message)
		{
			this.SendIrcMessage(string.Format(":{0}!{0}@{0}.tmi.twitch.tv PRIVMSG #{1} :{2}", this.userName, this.channel, message), true);
		}

		// Token: 0x04009181 RID: 37249
		[PublicizedFrom(EAccessModifier.Private)]
		public string userName;

		// Token: 0x04009182 RID: 37250
		[PublicizedFrom(EAccessModifier.Private)]
		public string channel;

		// Token: 0x04009183 RID: 37251
		[PublicizedFrom(EAccessModifier.Private)]
		public string password;

		// Token: 0x04009184 RID: 37252
		[PublicizedFrom(EAccessModifier.Private)]
		public string ip;

		// Token: 0x04009185 RID: 37253
		[PublicizedFrom(EAccessModifier.Private)]
		public int port;

		// Token: 0x04009186 RID: 37254
		[PublicizedFrom(EAccessModifier.Private)]
		public TcpClient tcpClient;

		// Token: 0x04009187 RID: 37255
		[PublicizedFrom(EAccessModifier.Private)]
		public StreamReader inputStream;

		// Token: 0x04009188 RID: 37256
		[PublicizedFrom(EAccessModifier.Private)]
		public StreamWriter outputStream;

		// Token: 0x04009189 RID: 37257
		[PublicizedFrom(EAccessModifier.Private)]
		public static float pingMaxTimer = 100f;

		// Token: 0x0400918A RID: 37258
		[PublicizedFrom(EAccessModifier.Private)]
		public float PingTimer = TwitchIRCClient.pingMaxTimer;

		// Token: 0x0400918B RID: 37259
		[PublicizedFrom(EAccessModifier.Private)]
		public bool pingTimerRunning;

		// Token: 0x0400918C RID: 37260
		public List<string> outputQueue = new List<string>();

		// Token: 0x0400918D RID: 37261
		[PublicizedFrom(EAccessModifier.Private)]
		public static string TWITCH_SYSTEM_STRING = "tmi.twitch.tv";

		// Token: 0x0400918E RID: 37262
		[PublicizedFrom(EAccessModifier.Private)]
		public static string TWITCH_CONNECTION_STRING = ":tmi.twitch.tv 001";

		// Token: 0x0400918F RID: 37263
		[PublicizedFrom(EAccessModifier.Private)]
		public static string PRIV_MSG_STRING = "PRIVMSG";

		// Token: 0x04009190 RID: 37264
		[PublicizedFrom(EAccessModifier.Private)]
		public static string PRIV_MSG_STRING_PARSE = "PRIVMSG #";

		// Token: 0x04009191 RID: 37265
		[PublicizedFrom(EAccessModifier.Private)]
		public static string MSG_RAID_STRING = "msg-id=raid";

		// Token: 0x04009192 RID: 37266
		[PublicizedFrom(EAccessModifier.Private)]
		public static string MSG_CHARITY_STRING = "msg-id=charitydonation";

		// Token: 0x0200189B RID: 6299
		public class TwitchChatMessage
		{
			// Token: 0x17001868 RID: 6248
			// (get) Token: 0x0600C1DA RID: 49626 RVA: 0x004720F7 File Offset: 0x004702F7
			// (set) Token: 0x0600C1DB RID: 49627 RVA: 0x004720FF File Offset: 0x004702FF
			public virtual TwitchIRCClient.TwitchChatMessage.MessageTypes MessageType
			{
				get; [PublicizedFrom(EAccessModifier.Private)]
				set;
			}

			// Token: 0x0600C1DC RID: 49628 RVA: 0x00472108 File Offset: 0x00470308
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

			// Token: 0x04009194 RID: 37268
			public bool isMod;

			// Token: 0x04009195 RID: 37269
			public bool isVIP;

			// Token: 0x04009196 RID: 37270
			public bool isSub;

			// Token: 0x04009197 RID: 37271
			public bool isBroadcaster;

			// Token: 0x04009198 RID: 37272
			public string UserName;

			// Token: 0x04009199 RID: 37273
			public int UserID;

			// Token: 0x0400919A RID: 37274
			public string UserNameColor = "FFFFFF";

			// Token: 0x0400919B RID: 37275
			public string Message;

			// Token: 0x0200189C RID: 6300
			public enum MessageTypes
			{
				// Token: 0x0400919D RID: 37277
				Invalid = -1,
				// Token: 0x0400919E RID: 37278
				Message,
				// Token: 0x0400919F RID: 37279
				Output,
				// Token: 0x040091A0 RID: 37280
				Authenticated,
				// Token: 0x040091A1 RID: 37281
				Raid,
				// Token: 0x040091A2 RID: 37282
				Charity
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

namespace Twitch
{
	// Token: 0x0200183D RID: 6205
	public class TwitchIRCClient
	{
		// Token: 0x1700177D RID: 6013
		// (get) Token: 0x0600BF60 RID: 48992 RVA: 0x00469E08 File Offset: 0x00468008
		public bool IsConnected
		{
			get
			{
				return this.tcpClient.Connected;
			}
		}

		// Token: 0x0600BF61 RID: 48993 RVA: 0x00469E18 File Offset: 0x00468018
		public TwitchIRCClient(string ip, int port, string channel, string password)
		{
			this.userName = channel;
			this.password = password;
			this.channel = channel;
			this.ip = ip;
			this.port = port;
			this.Reconnect();
		}

		// Token: 0x0600BF62 RID: 48994 RVA: 0x00469E6C File Offset: 0x0046806C
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

		// Token: 0x0600BF63 RID: 48995 RVA: 0x00469F1F File Offset: 0x0046811F
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

		// Token: 0x0600BF64 RID: 48996 RVA: 0x00469F5C File Offset: 0x0046815C
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

		// Token: 0x0600BF65 RID: 48997 RVA: 0x00469FFC File Offset: 0x004681FC
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

		// Token: 0x0600BF66 RID: 48998 RVA: 0x0046A038 File Offset: 0x00468238
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

		// Token: 0x0600BF67 RID: 48999 RVA: 0x0046A084 File Offset: 0x00468284
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

		// Token: 0x0600BF68 RID: 49000 RVA: 0x0046A0E4 File Offset: 0x004682E4
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

		// Token: 0x0600BF69 RID: 49001 RVA: 0x0046A170 File Offset: 0x00468370
		public bool AvailableMessage()
		{
			return this.tcpClient.Available > 0;
		}

		// Token: 0x0600BF6A RID: 49002 RVA: 0x0046A180 File Offset: 0x00468380
		public TwitchIRCClient.TwitchChatMessage ReadMessage()
		{
			return this.ParseMessage();
		}

		// Token: 0x0600BF6B RID: 49003 RVA: 0x0046A188 File Offset: 0x00468388
		public TwitchIRCClient.TwitchChatMessage ParseMessage()
		{
			return new TwitchIRCClient.TwitchChatMessage(this.inputStream.ReadLine());
		}

		// Token: 0x0600BF6C RID: 49004 RVA: 0x0046A19A File Offset: 0x0046839A
		public void SendChatMessage(string message)
		{
			this.SendIrcMessage(string.Format(":{0}!{0}@{0}.tmi.twitch.tv PRIVMSG #{1} :{2}", this.userName, this.channel, message), true);
		}

		// Token: 0x04008FD2 RID: 36818
		[PublicizedFrom(EAccessModifier.Private)]
		public string userName;

		// Token: 0x04008FD3 RID: 36819
		[PublicizedFrom(EAccessModifier.Private)]
		public string channel;

		// Token: 0x04008FD4 RID: 36820
		[PublicizedFrom(EAccessModifier.Private)]
		public string password;

		// Token: 0x04008FD5 RID: 36821
		[PublicizedFrom(EAccessModifier.Private)]
		public string ip;

		// Token: 0x04008FD6 RID: 36822
		[PublicizedFrom(EAccessModifier.Private)]
		public int port;

		// Token: 0x04008FD7 RID: 36823
		[PublicizedFrom(EAccessModifier.Private)]
		public TcpClient tcpClient;

		// Token: 0x04008FD8 RID: 36824
		[PublicizedFrom(EAccessModifier.Private)]
		public StreamReader inputStream;

		// Token: 0x04008FD9 RID: 36825
		[PublicizedFrom(EAccessModifier.Private)]
		public StreamWriter outputStream;

		// Token: 0x04008FDA RID: 36826
		[PublicizedFrom(EAccessModifier.Private)]
		public static float pingMaxTimer = 100f;

		// Token: 0x04008FDB RID: 36827
		[PublicizedFrom(EAccessModifier.Private)]
		public float PingTimer = TwitchIRCClient.pingMaxTimer;

		// Token: 0x04008FDC RID: 36828
		[PublicizedFrom(EAccessModifier.Private)]
		public bool pingTimerRunning;

		// Token: 0x04008FDD RID: 36829
		public List<string> outputQueue = new List<string>();

		// Token: 0x04008FDE RID: 36830
		[PublicizedFrom(EAccessModifier.Private)]
		public static string TWITCH_SYSTEM_STRING = "tmi.twitch.tv";

		// Token: 0x04008FDF RID: 36831
		[PublicizedFrom(EAccessModifier.Private)]
		public static string TWITCH_CONNECTION_STRING = ":tmi.twitch.tv 001";

		// Token: 0x04008FE0 RID: 36832
		[PublicizedFrom(EAccessModifier.Private)]
		public static string PRIV_MSG_STRING = "PRIVMSG";

		// Token: 0x04008FE1 RID: 36833
		[PublicizedFrom(EAccessModifier.Private)]
		public static string PRIV_MSG_STRING_PARSE = "PRIVMSG #";

		// Token: 0x04008FE2 RID: 36834
		[PublicizedFrom(EAccessModifier.Private)]
		public static string MSG_RAID_STRING = "msg-id=raid";

		// Token: 0x04008FE3 RID: 36835
		[PublicizedFrom(EAccessModifier.Private)]
		public static string MSG_CHARITY_STRING = "msg-id=charitydonation";

		// Token: 0x0200183E RID: 6206
		public class TwitchChatMessage
		{
			// Token: 0x1700177E RID: 6014
			// (get) Token: 0x0600BF6E RID: 49006 RVA: 0x0046A20F File Offset: 0x0046840F
			// (set) Token: 0x0600BF6F RID: 49007 RVA: 0x0046A217 File Offset: 0x00468417
			public virtual TwitchIRCClient.TwitchChatMessage.MessageTypes MessageType
			{
				get; [PublicizedFrom(EAccessModifier.Private)]
				set;
			}

			// Token: 0x0600BF70 RID: 49008 RVA: 0x0046A220 File Offset: 0x00468420
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

			// Token: 0x04008FE5 RID: 36837
			public bool isMod;

			// Token: 0x04008FE6 RID: 36838
			public bool isVIP;

			// Token: 0x04008FE7 RID: 36839
			public bool isSub;

			// Token: 0x04008FE8 RID: 36840
			public bool isBroadcaster;

			// Token: 0x04008FE9 RID: 36841
			public string UserName;

			// Token: 0x04008FEA RID: 36842
			public int UserID;

			// Token: 0x04008FEB RID: 36843
			public string UserNameColor = "FFFFFF";

			// Token: 0x04008FEC RID: 36844
			public string Message;

			// Token: 0x0200183F RID: 6207
			public enum MessageTypes
			{
				// Token: 0x04008FEE RID: 36846
				Invalid = -1,
				// Token: 0x04008FEF RID: 36847
				Message,
				// Token: 0x04008FF0 RID: 36848
				Output,
				// Token: 0x04008FF1 RID: 36849
				Authenticated,
				// Token: 0x04008FF2 RID: 36850
				Raid,
				// Token: 0x04008FF3 RID: 36851
				Charity
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

namespace Twitch
{
	// Token: 0x02001817 RID: 6167
	public class TwitchIRCClient
	{
		// Token: 0x1700170B RID: 5899
		// (get) Token: 0x0600BDDB RID: 48603 RVA: 0x0046406C File Offset: 0x0046226C
		public bool IsConnected
		{
			get
			{
				return this.tcpClient.Connected;
			}
		}

		// Token: 0x0600BDDC RID: 48604 RVA: 0x0046407C File Offset: 0x0046227C
		public TwitchIRCClient(string ip, int port, string channel, string password)
		{
			this.userName = channel;
			this.password = password;
			this.channel = channel;
			this.ip = ip;
			this.port = port;
			this.Reconnect();
		}

		// Token: 0x0600BDDD RID: 48605 RVA: 0x004640D0 File Offset: 0x004622D0
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

		// Token: 0x0600BDDE RID: 48606 RVA: 0x00464183 File Offset: 0x00462383
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

		// Token: 0x0600BDDF RID: 48607 RVA: 0x004641C0 File Offset: 0x004623C0
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

		// Token: 0x0600BDE0 RID: 48608 RVA: 0x00464260 File Offset: 0x00462460
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

		// Token: 0x0600BDE1 RID: 48609 RVA: 0x0046429C File Offset: 0x0046249C
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

		// Token: 0x0600BDE2 RID: 48610 RVA: 0x004642E8 File Offset: 0x004624E8
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

		// Token: 0x0600BDE3 RID: 48611 RVA: 0x00464348 File Offset: 0x00462548
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

		// Token: 0x0600BDE4 RID: 48612 RVA: 0x004643D4 File Offset: 0x004625D4
		public bool AvailableMessage()
		{
			return this.tcpClient.Available > 0;
		}

		// Token: 0x0600BDE5 RID: 48613 RVA: 0x004643E4 File Offset: 0x004625E4
		public TwitchIRCClient.TwitchChatMessage ReadMessage()
		{
			return this.ParseMessage();
		}

		// Token: 0x0600BDE6 RID: 48614 RVA: 0x004643EC File Offset: 0x004625EC
		public TwitchIRCClient.TwitchChatMessage ParseMessage()
		{
			return new TwitchIRCClient.TwitchChatMessage(this.inputStream.ReadLine());
		}

		// Token: 0x0600BDE7 RID: 48615 RVA: 0x004643FE File Offset: 0x004625FE
		public void SendChatMessage(string message)
		{
			this.SendIrcMessage(string.Format(":{0}!{0}@{0}.tmi.twitch.tv PRIVMSG #{1} :{2}", this.userName, this.channel, message), true);
		}

		// Token: 0x04008EB6 RID: 36534
		[PublicizedFrom(EAccessModifier.Private)]
		public string userName;

		// Token: 0x04008EB7 RID: 36535
		[PublicizedFrom(EAccessModifier.Private)]
		public string channel;

		// Token: 0x04008EB8 RID: 36536
		[PublicizedFrom(EAccessModifier.Private)]
		public string password;

		// Token: 0x04008EB9 RID: 36537
		[PublicizedFrom(EAccessModifier.Private)]
		public string ip;

		// Token: 0x04008EBA RID: 36538
		[PublicizedFrom(EAccessModifier.Private)]
		public int port;

		// Token: 0x04008EBB RID: 36539
		[PublicizedFrom(EAccessModifier.Private)]
		public TcpClient tcpClient;

		// Token: 0x04008EBC RID: 36540
		[PublicizedFrom(EAccessModifier.Private)]
		public StreamReader inputStream;

		// Token: 0x04008EBD RID: 36541
		[PublicizedFrom(EAccessModifier.Private)]
		public StreamWriter outputStream;

		// Token: 0x04008EBE RID: 36542
		[PublicizedFrom(EAccessModifier.Private)]
		public static float pingMaxTimer = 100f;

		// Token: 0x04008EBF RID: 36543
		[PublicizedFrom(EAccessModifier.Private)]
		public float PingTimer = TwitchIRCClient.pingMaxTimer;

		// Token: 0x04008EC0 RID: 36544
		[PublicizedFrom(EAccessModifier.Private)]
		public bool pingTimerRunning;

		// Token: 0x04008EC1 RID: 36545
		public List<string> outputQueue = new List<string>();

		// Token: 0x04008EC2 RID: 36546
		[PublicizedFrom(EAccessModifier.Private)]
		public static string TWITCH_SYSTEM_STRING = "tmi.twitch.tv";

		// Token: 0x04008EC3 RID: 36547
		[PublicizedFrom(EAccessModifier.Private)]
		public static string TWITCH_CONNECTION_STRING = ":tmi.twitch.tv 001";

		// Token: 0x04008EC4 RID: 36548
		[PublicizedFrom(EAccessModifier.Private)]
		public static string PRIV_MSG_STRING = "PRIVMSG";

		// Token: 0x04008EC5 RID: 36549
		[PublicizedFrom(EAccessModifier.Private)]
		public static string PRIV_MSG_STRING_PARSE = "PRIVMSG #";

		// Token: 0x04008EC6 RID: 36550
		[PublicizedFrom(EAccessModifier.Private)]
		public static string MSG_RAID_STRING = "msg-id=raid";

		// Token: 0x04008EC7 RID: 36551
		[PublicizedFrom(EAccessModifier.Private)]
		public static string MSG_CHARITY_STRING = "msg-id=charitydonation";

		// Token: 0x02001818 RID: 6168
		public class TwitchChatMessage
		{
			// Token: 0x1700170C RID: 5900
			// (get) Token: 0x0600BDE9 RID: 48617 RVA: 0x00464473 File Offset: 0x00462673
			// (set) Token: 0x0600BDEA RID: 48618 RVA: 0x0046447B File Offset: 0x0046267B
			public virtual TwitchIRCClient.TwitchChatMessage.MessageTypes MessageType
			{
				get; [PublicizedFrom(EAccessModifier.Private)]
				set;
			}

			// Token: 0x0600BDEB RID: 48619 RVA: 0x00464484 File Offset: 0x00462684
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

			// Token: 0x04008EC9 RID: 36553
			public bool isMod;

			// Token: 0x04008ECA RID: 36554
			public bool isVIP;

			// Token: 0x04008ECB RID: 36555
			public bool isSub;

			// Token: 0x04008ECC RID: 36556
			public bool isBroadcaster;

			// Token: 0x04008ECD RID: 36557
			public string UserName;

			// Token: 0x04008ECE RID: 36558
			public int UserID;

			// Token: 0x04008ECF RID: 36559
			public string UserNameColor = "FFFFFF";

			// Token: 0x04008ED0 RID: 36560
			public string Message;

			// Token: 0x02001819 RID: 6169
			public enum MessageTypes
			{
				// Token: 0x04008ED2 RID: 36562
				Invalid = -1,
				// Token: 0x04008ED3 RID: 36563
				Message,
				// Token: 0x04008ED4 RID: 36564
				Output,
				// Token: 0x04008ED5 RID: 36565
				Authenticated,
				// Token: 0x04008ED6 RID: 36566
				Raid,
				// Token: 0x04008ED7 RID: 36567
				Charity
			}
		}
	}
}

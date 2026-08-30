using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

// Token: 0x020002CF RID: 719
public class TelnetConnection : ConsoleConnectionAbstract
{
	// Token: 0x1700024B RID: 587
	// (get) Token: 0x060014B8 RID: 5304 RVA: 0x0007D2AF File Offset: 0x0007B4AF
	public bool IsClosed
	{
		get
		{
			return this.closed;
		}
	}

	// Token: 0x1700024C RID: 588
	// (get) Token: 0x060014B9 RID: 5305 RVA: 0x0007D2B9 File Offset: 0x0007B4B9
	public bool IsAuthenticated
	{
		get
		{
			return !this.authEnabled || this.authenticated;
		}
	}

	// Token: 0x1700024D RID: 589
	// (get) Token: 0x060014BA RID: 5306 RVA: 0x0007D2CB File Offset: 0x0007B4CB
	public int EndPointHash { get; }

	// Token: 0x060014BB RID: 5307 RVA: 0x0007D2D4 File Offset: 0x0007B4D4
	public TelnetConnection(TelnetConsole _owner, TcpClient _client, int _addressHash, bool _authEnabled)
	{
		this.telnet = _owner;
		this.authEnabled = _authEnabled;
		this.client = _client;
		this.endpoint = _client.Client.RemoteEndPoint;
		this.EndPointHash = _addressHash;
		string text = "Telnet connection from: ";
		EndPoint endPoint = this.endpoint;
		Log.Out(text + ((endPoint != null) ? endPoint.ToString() : null));
		this.clientStream = _client.GetStream();
		this.reader = new StreamReader(this.clientStream, Encoding.UTF8);
		string text2 = "TelnetClient_";
		EndPoint endPoint2 = this.endpoint;
		ThreadManager.StartThread(text2 + ((endPoint2 != null) ? endPoint2.ToString() : null), null, new ThreadManager.ThreadFunctionLoopDelegate(this.HandlerThread), new ThreadManager.ThreadFunctionEndDelegate(this.ThreadEnd), null, null, false, false);
		if (_authEnabled)
		{
			this.toClientQueue.Enqueue("Please enter password:");
			return;
		}
		this.LoginMessage();
	}

	// Token: 0x060014BC RID: 5308 RVA: 0x0007D3E8 File Offset: 0x0007B5E8
	[PublicizedFrom(EAccessModifier.Private)]
	public void LoginMessage()
	{
		this.toClientQueue.Enqueue("*** Connected with 7DTD server.");
		this.toClientQueue.Enqueue("*** Server version: " + Constants.cVersionInformation.LongString + " Compatibility Version: " + Constants.cVersionInformation.LongStringNoBuild);
		this.toClientQueue.Enqueue("*** Dedicated server only build");
		this.toClientQueue.Enqueue(string.Empty);
		this.toClientQueue.Enqueue("Server IP:   " + (string.IsNullOrEmpty(GamePrefs.GetString(EnumGamePrefs.ServerIP)) ? "Any" : GamePrefs.GetString(EnumGamePrefs.ServerIP)));
		this.toClientQueue.Enqueue("Server port: " + GamePrefs.GetInt(EnumGamePrefs.ServerPort).ToString());
		this.toClientQueue.Enqueue("Max players: " + GamePrefs.GetInt(EnumGamePrefs.ServerMaxPlayerCount).ToString());
		this.toClientQueue.Enqueue("Game mode:   " + GamePrefs.GetString(EnumGamePrefs.GameMode));
		this.toClientQueue.Enqueue("World:       " + GamePrefs.GetString(EnumGamePrefs.GameWorld));
		this.toClientQueue.Enqueue("Game name:   " + GamePrefs.GetString(EnumGamePrefs.GameName));
		this.toClientQueue.Enqueue("Difficulty:  " + GamePrefs.GetInt(EnumGamePrefs.GameDifficulty).ToString());
		this.toClientQueue.Enqueue(string.Empty);
		this.toClientQueue.Enqueue("Press 'help' to get a list of all commands. Press 'exit' to end session.");
		this.toClientQueue.Enqueue(string.Empty);
	}

	// Token: 0x1700024E RID: 590
	// (get) Token: 0x060014BD RID: 5309 RVA: 0x0007D574 File Offset: 0x0007B774
	public bool ConnectionUsable
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.client.Connected && !this.closed;
		}
	}

	// Token: 0x060014BE RID: 5310 RVA: 0x0007D590 File Offset: 0x0007B790
	[PublicizedFrom(EAccessModifier.Private)]
	public int HandlerThread(ThreadManager.ThreadInfo _tInfo)
	{
		if (!this.ConnectionUsable || this.closeConnection)
		{
			return -1;
		}
		try
		{
			if (!this.handleReading())
			{
				return -1;
			}
			this.handleWriting();
		}
		catch (IOException ex)
		{
			SocketException ex2 = ex.InnerException as SocketException;
			if (ex2 != null && ex2.SocketErrorCode == SocketError.ConnectionAborted)
			{
				string text = "Connection closed by host in TelnetClient_";
				EndPoint endPoint = this.endpoint;
				Log.Warning(text + ((endPoint != null) ? endPoint.ToString() : null));
				return -1;
			}
			string text2 = "IOException in TelnetClient_";
			EndPoint endPoint2 = this.endpoint;
			Log.Error(text2 + ((endPoint2 != null) ? endPoint2.ToString() : null) + ": " + ex.Message);
			Log.Exception(ex);
			return -1;
		}
		return 25;
	}

	// Token: 0x060014BF RID: 5311 RVA: 0x0007D654 File Offset: 0x0007B854
	[PublicizedFrom(EAccessModifier.Private)]
	public bool handleReading()
	{
		int num;
		while (this.ConnectionUsable && this.clientStream.CanRead && this.client.Available > 0 && (num = this.reader.Read(this.charBuffer, 0, this.charBuffer.Length)) > 0)
		{
			for (int i = 0; i < num; i++)
			{
				char c = this.charBuffer[i];
				if (c == '\r' || c == '\n')
				{
					if (!this.submitInput())
					{
						return false;
					}
				}
				else
				{
					this.readStringBuilder.Append(c);
				}
			}
		}
		return true;
	}

	// Token: 0x060014C0 RID: 5312 RVA: 0x0007D6DC File Offset: 0x0007B8DC
	[PublicizedFrom(EAccessModifier.Private)]
	public bool submitInput()
	{
		this.readStringBuilder.Trim();
		if (this.readStringBuilder.Length <= 0)
		{
			return true;
		}
		string text = this.readStringBuilder.ToString();
		if (!this.IsAuthenticated)
		{
			this.authenticate(text);
		}
		else
		{
			if (text.EqualsCaseInsensitive("exit"))
			{
				return false;
			}
			SingletonMonoBehaviour<SdtdConsole>.Instance.ExecuteAsync(text, this);
		}
		this.readStringBuilder.Length = 0;
		return true;
	}

	// Token: 0x060014C1 RID: 5313 RVA: 0x0007D74C File Offset: 0x0007B94C
	[PublicizedFrom(EAccessModifier.Private)]
	public void authenticate(string _line)
	{
		if (_line.Equals(GamePrefs.GetString(EnumGamePrefs.TelnetPassword)))
		{
			this.authenticated = true;
			this.toClientQueue.Enqueue("Logon successful.");
			this.toClientQueue.Enqueue(string.Empty);
			this.toClientQueue.Enqueue(string.Empty);
			this.toClientQueue.Enqueue(string.Empty);
			this.LoginMessage();
			return;
		}
		if (this.telnet.RegisterFailedLogin(this))
		{
			this.toClientQueue.Enqueue("Password incorrect, please enter password:");
			return;
		}
		this.toClientQueue.Enqueue("Too many failed login attempts!");
		this.closeConnection = true;
	}

	// Token: 0x060014C2 RID: 5314 RVA: 0x0007D7EC File Offset: 0x0007B9EC
	[PublicizedFrom(EAccessModifier.Private)]
	public void handleWriting()
	{
		while (this.ConnectionUsable && this.clientStream.CanWrite && this.toClientQueue.HasData())
		{
			string text = this.toClientQueue.Dequeue();
			if (text == null)
			{
				this.clientStream.WriteByte(0);
			}
			else
			{
				int num;
				for (int i = 0; i < text.Length; i += num)
				{
					num = Math.Min(64, text.Length - i);
					int bytes = Encoding.UTF8.GetBytes(text, i, num, this.byteBuffer, 0);
					this.clientStream.Write(this.byteBuffer, 0, bytes);
				}
				this.clientStream.WriteByte(13);
				this.clientStream.WriteByte(10);
			}
		}
	}

	// Token: 0x060014C3 RID: 5315 RVA: 0x0007D8A3 File Offset: 0x0007BAA3
	[PublicizedFrom(EAccessModifier.Private)]
	public void ThreadEnd(ThreadManager.ThreadInfo _threadInfo, bool _exitForException)
	{
		this.Close(false);
	}

	// Token: 0x060014C4 RID: 5316 RVA: 0x0007D8AC File Offset: 0x0007BAAC
	public void Close(bool _kickedForLogins = false)
	{
		if (this.closed)
		{
			return;
		}
		this.closed = true;
		this.toClientQueue.Close();
		if (this.client.Connected)
		{
			this.client.GetStream().Close();
			this.client.Close();
		}
		this.telnet.ConnectionClosed(this);
		if (_kickedForLogins)
		{
			string text = "Telnet connection closed for too many login attempts: ";
			EndPoint endPoint = this.endpoint;
			Log.Out(text + ((endPoint != null) ? endPoint.ToString() : null));
			return;
		}
		string text2 = "Telnet connection closed: ";
		EndPoint endPoint2 = this.endpoint;
		Log.Out(text2 + ((endPoint2 != null) ? endPoint2.ToString() : null));
	}

	// Token: 0x060014C5 RID: 5317 RVA: 0x0007D952 File Offset: 0x0007BB52
	public override void SendLine(string _line)
	{
		if (!this.closed && this.IsAuthenticated)
		{
			this.toClientQueue.Enqueue(_line);
			return;
		}
		this.toClientQueue.Enqueue(null);
	}

	// Token: 0x060014C6 RID: 5318 RVA: 0x0007D980 File Offset: 0x0007BB80
	public override void SendLines(List<string> _output)
	{
		for (int i = 0; i < _output.Count; i++)
		{
			this.SendLine(_output[i]);
		}
	}

	// Token: 0x060014C7 RID: 5319 RVA: 0x0007D9AB File Offset: 0x0007BBAB
	public override void SendLog(string _formattedMessage, string _plainMessage, string _trace, LogType _type, DateTime _timestamp, long _uptime)
	{
		if (base.IsLogLevelEnabled(_type))
		{
			this.SendLine(_formattedMessage);
		}
	}

	// Token: 0x060014C8 RID: 5320 RVA: 0x0007D9C0 File Offset: 0x0007BBC0
	public override string GetDescription()
	{
		string text;
		if ((text = this.cachedDescription) == null)
		{
			string text2 = "Telnet from ";
			EndPoint endPoint = this.endpoint;
			text = (this.cachedDescription = text2 + ((endPoint != null) ? endPoint.ToString() : null));
		}
		return text;
	}

	// Token: 0x04000DBD RID: 3517
	[PublicizedFrom(EAccessModifier.Private)]
	public const int MAX_CHARS_PER_CONVERSION = 64;

	// Token: 0x04000DBE RID: 3518
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly BlockingQueue<string> toClientQueue = new BlockingQueue<string>();

	// Token: 0x04000DBF RID: 3519
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly TelnetConsole telnet;

	// Token: 0x04000DC0 RID: 3520
	[PublicizedFrom(EAccessModifier.Private)]
	public bool authenticated;

	// Token: 0x04000DC1 RID: 3521
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly bool authEnabled;

	// Token: 0x04000DC2 RID: 3522
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly TcpClient client;

	// Token: 0x04000DC3 RID: 3523
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly EndPoint endpoint;

	// Token: 0x04000DC4 RID: 3524
	[PublicizedFrom(EAccessModifier.Private)]
	public volatile bool closed;

	// Token: 0x04000DC5 RID: 3525
	[PublicizedFrom(EAccessModifier.Private)]
	public string cachedDescription;

	// Token: 0x04000DC6 RID: 3526
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly NetworkStream clientStream;

	// Token: 0x04000DC7 RID: 3527
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly StreamReader reader;

	// Token: 0x04000DC8 RID: 3528
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly StringBuilder readStringBuilder = new StringBuilder();

	// Token: 0x04000DC9 RID: 3529
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly char[] charBuffer = new char[64];

	// Token: 0x04000DCA RID: 3530
	[PublicizedFrom(EAccessModifier.Private)]
	public bool closeConnection;

	// Token: 0x04000DCB RID: 3531
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly byte[] byteBuffer = new byte[256];
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

// Token: 0x020002CB RID: 715
public class TelnetConnection : ConsoleConnectionAbstract
{
	// Token: 0x1700024A RID: 586
	// (get) Token: 0x060014A6 RID: 5286 RVA: 0x0007CC57 File Offset: 0x0007AE57
	public bool IsClosed
	{
		get
		{
			return this.closed;
		}
	}

	// Token: 0x1700024B RID: 587
	// (get) Token: 0x060014A7 RID: 5287 RVA: 0x0007CC61 File Offset: 0x0007AE61
	public bool IsAuthenticated
	{
		get
		{
			return !this.authEnabled || this.authenticated;
		}
	}

	// Token: 0x1700024C RID: 588
	// (get) Token: 0x060014A8 RID: 5288 RVA: 0x0007CC73 File Offset: 0x0007AE73
	public int EndPointHash { get; }

	// Token: 0x060014A9 RID: 5289 RVA: 0x0007CC7C File Offset: 0x0007AE7C
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

	// Token: 0x060014AA RID: 5290 RVA: 0x0007CD90 File Offset: 0x0007AF90
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

	// Token: 0x1700024D RID: 589
	// (get) Token: 0x060014AB RID: 5291 RVA: 0x0007CF1C File Offset: 0x0007B11C
	public bool ConnectionUsable
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.client.Connected && !this.closed;
		}
	}

	// Token: 0x060014AC RID: 5292 RVA: 0x0007CF38 File Offset: 0x0007B138
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

	// Token: 0x060014AD RID: 5293 RVA: 0x0007CFFC File Offset: 0x0007B1FC
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

	// Token: 0x060014AE RID: 5294 RVA: 0x0007D084 File Offset: 0x0007B284
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

	// Token: 0x060014AF RID: 5295 RVA: 0x0007D0F4 File Offset: 0x0007B2F4
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

	// Token: 0x060014B0 RID: 5296 RVA: 0x0007D194 File Offset: 0x0007B394
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

	// Token: 0x060014B1 RID: 5297 RVA: 0x0007D24B File Offset: 0x0007B44B
	[PublicizedFrom(EAccessModifier.Private)]
	public void ThreadEnd(ThreadManager.ThreadInfo _threadInfo, bool _exitForException)
	{
		this.Close(false);
	}

	// Token: 0x060014B2 RID: 5298 RVA: 0x0007D254 File Offset: 0x0007B454
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

	// Token: 0x060014B3 RID: 5299 RVA: 0x0007D2FA File Offset: 0x0007B4FA
	public override void SendLine(string _line)
	{
		if (!this.closed && this.IsAuthenticated)
		{
			this.toClientQueue.Enqueue(_line);
			return;
		}
		this.toClientQueue.Enqueue(null);
	}

	// Token: 0x060014B4 RID: 5300 RVA: 0x0007D328 File Offset: 0x0007B528
	public override void SendLines(List<string> _output)
	{
		for (int i = 0; i < _output.Count; i++)
		{
			this.SendLine(_output[i]);
		}
	}

	// Token: 0x060014B5 RID: 5301 RVA: 0x0007D353 File Offset: 0x0007B553
	public override void SendLog(string _formattedMessage, string _plainMessage, string _trace, LogType _type, DateTime _timestamp, long _uptime)
	{
		if (base.IsLogLevelEnabled(_type))
		{
			this.SendLine(_formattedMessage);
		}
	}

	// Token: 0x060014B6 RID: 5302 RVA: 0x0007D368 File Offset: 0x0007B568
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

	// Token: 0x04000DAC RID: 3500
	[PublicizedFrom(EAccessModifier.Private)]
	public const int MAX_CHARS_PER_CONVERSION = 64;

	// Token: 0x04000DAD RID: 3501
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly BlockingQueue<string> toClientQueue = new BlockingQueue<string>();

	// Token: 0x04000DAE RID: 3502
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly TelnetConsole telnet;

	// Token: 0x04000DAF RID: 3503
	[PublicizedFrom(EAccessModifier.Private)]
	public bool authenticated;

	// Token: 0x04000DB0 RID: 3504
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly bool authEnabled;

	// Token: 0x04000DB1 RID: 3505
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly TcpClient client;

	// Token: 0x04000DB2 RID: 3506
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly EndPoint endpoint;

	// Token: 0x04000DB3 RID: 3507
	[PublicizedFrom(EAccessModifier.Private)]
	public volatile bool closed;

	// Token: 0x04000DB4 RID: 3508
	[PublicizedFrom(EAccessModifier.Private)]
	public string cachedDescription;

	// Token: 0x04000DB5 RID: 3509
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly NetworkStream clientStream;

	// Token: 0x04000DB6 RID: 3510
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly StreamReader reader;

	// Token: 0x04000DB7 RID: 3511
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly StringBuilder readStringBuilder = new StringBuilder();

	// Token: 0x04000DB8 RID: 3512
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly char[] charBuffer = new char[64];

	// Token: 0x04000DB9 RID: 3513
	[PublicizedFrom(EAccessModifier.Private)]
	public bool closeConnection;

	// Token: 0x04000DBA RID: 3514
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly byte[] byteBuffer = new byte[256];
}

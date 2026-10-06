using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

// Token: 0x020002D3 RID: 723
public class TelnetConsole : IConsoleServer
{
	// Token: 0x060014C6 RID: 5318 RVA: 0x0007DCB0 File Offset: 0x0007BEB0
	public TelnetConsole()
	{
		try
		{
			int @int = GamePrefs.GetInt(EnumGamePrefs.TelnetPort);
			this.authEnabled = GamePrefs.GetString(EnumGamePrefs.TelnetPassword).Length != 0;
			this.listener = new TcpListener(this.authEnabled ? IPAddress.Any : IPAddress.Loopback, @int);
			TelnetConsole.maxLoginAttempts = GamePrefs.GetInt(EnumGamePrefs.TelnetFailedLoginLimit);
			TelnetConsole.blockTimeSeconds = GamePrefs.GetInt(EnumGamePrefs.TelnetFailedLoginsBlocktime);
			this.listener.Start();
			this.listener.BeginAcceptTcpClient(new AsyncCallback(this.AcceptClient), null);
			Log.Out("Started Telnet on " + @int.ToString());
		}
		catch (Exception ex)
		{
			string text = "Error in Telnet.ctor: ";
			Exception ex2 = ex;
			Log.Out(text + ((ex2 != null) ? ex2.ToString() : null));
		}
	}

	// Token: 0x060014C7 RID: 5319 RVA: 0x0007DDA0 File Offset: 0x0007BFA0
	[PublicizedFrom(EAccessModifier.Private)]
	public void AcceptClient(IAsyncResult _asyncResult)
	{
		TcpListener tcpListener = this.listener;
		if (((tcpListener != null) ? tcpListener.Server : null) == null || !this.listener.Server.IsBound)
		{
			return;
		}
		TcpClient tcpClient = this.listener.EndAcceptTcpClient(_asyncResult);
		EndPoint remoteEndPoint = tcpClient.Client.RemoteEndPoint;
		IPEndPoint ipendPoint = remoteEndPoint as IPEndPoint;
		int num;
		if (ipendPoint != null)
		{
			num = ipendPoint.Address.GetHashCode();
		}
		else
		{
			num = remoteEndPoint.GetHashCode();
			string text = "EndPoint is not an IPEndPoint but: ";
			Type type = remoteEndPoint.GetType();
			Log.Out(text + ((type != null) ? type.ToString() : null));
		}
		Dictionary<int, TelnetConsole.LoginAttempts> dictionary = this.loginAttemptsPerIP;
		lock (dictionary)
		{
			TelnetConsole.LoginAttempts loginAttempts;
			if (!this.loginAttemptsPerIP.TryGetValue(num, out loginAttempts))
			{
				loginAttempts = new TelnetConsole.LoginAttempts();
				this.loginAttemptsPerIP[num] = loginAttempts;
			}
			if (!loginAttempts.IsBanned())
			{
				TelnetConnection telnetConnection = new TelnetConnection(this, tcpClient, num, this.authEnabled);
				List<TelnetConnection> list = this.connections;
				lock (list)
				{
					this.connections.Add(telnetConnection);
					goto IL_0131;
				}
			}
			tcpClient.Close();
			string text2 = "Telnet connection not accepted for too many login attempts: ";
			EndPoint endPoint = remoteEndPoint;
			Log.Out(text2 + ((endPoint != null) ? endPoint.ToString() : null));
		}
		IL_0131:
		this.listener.BeginAcceptTcpClient(new AsyncCallback(this.AcceptClient), null);
	}

	// Token: 0x060014C8 RID: 5320 RVA: 0x0007DF14 File Offset: 0x0007C114
	public bool RegisterFailedLogin(TelnetConnection _con)
	{
		Dictionary<int, TelnetConsole.LoginAttempts> dictionary = this.loginAttemptsPerIP;
		bool flag2;
		lock (dictionary)
		{
			flag2 = this.loginAttemptsPerIP[_con.EndPointHash].LogAttempt();
		}
		return flag2;
	}

	// Token: 0x060014C9 RID: 5321 RVA: 0x0007DF68 File Offset: 0x0007C168
	public void ConnectionClosed(TelnetConnection _con)
	{
		List<TelnetConnection> list = this.connections;
		lock (list)
		{
			this.connections.Remove(_con);
		}
	}

	// Token: 0x060014CA RID: 5322 RVA: 0x0007DFB0 File Offset: 0x0007C1B0
	public void Disconnect()
	{
		try
		{
			if (this.listener != null)
			{
				this.listener.Stop();
				this.listener = null;
			}
			List<TelnetConnection> list = this.connections;
			List<TelnetConnection> list2;
			lock (list)
			{
				list2 = new List<TelnetConnection>(this.connections);
			}
			foreach (TelnetConnection telnetConnection in list2)
			{
				telnetConnection.Close(false);
			}
		}
		catch (Exception ex)
		{
			string text = "Error in Telnet.Disconnect: ";
			Exception ex2 = ex;
			Log.Out(text + ((ex2 != null) ? ex2.ToString() : null));
		}
	}

	// Token: 0x060014CB RID: 5323 RVA: 0x0007E07C File Offset: 0x0007C27C
	public void SendLine(string _line)
	{
		if (_line == null)
		{
			return;
		}
		List<TelnetConnection> list = this.connections;
		lock (list)
		{
			foreach (TelnetConnection telnetConnection in this.connections)
			{
				telnetConnection.SendLine(_line);
			}
		}
	}

	// Token: 0x060014CC RID: 5324 RVA: 0x0007E0F8 File Offset: 0x0007C2F8
	public void SendLog(string _formattedMessage, string _plainMessage, string _trace, LogType _type, DateTime _timestamp, long _uptime)
	{
		List<TelnetConnection> list = this.connections;
		lock (list)
		{
			foreach (TelnetConnection telnetConnection in this.connections)
			{
				telnetConnection.SendLog(_formattedMessage, _plainMessage, _trace, _type, _timestamp, _uptime);
			}
		}
	}

	// Token: 0x04000DC6 RID: 3526
	[PublicizedFrom(EAccessModifier.Private)]
	public static int maxLoginAttempts;

	// Token: 0x04000DC7 RID: 3527
	[PublicizedFrom(EAccessModifier.Private)]
	public static int blockTimeSeconds;

	// Token: 0x04000DC8 RID: 3528
	[PublicizedFrom(EAccessModifier.Private)]
	public TcpListener listener;

	// Token: 0x04000DC9 RID: 3529
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly bool authEnabled;

	// Token: 0x04000DCA RID: 3530
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<TelnetConnection> connections = new List<TelnetConnection>();

	// Token: 0x04000DCB RID: 3531
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<int, TelnetConsole.LoginAttempts> loginAttemptsPerIP = new Dictionary<int, TelnetConsole.LoginAttempts>();

	// Token: 0x020002D4 RID: 724
	[PublicizedFrom(EAccessModifier.Private)]
	public class LoginAttempts
	{
		// Token: 0x060014CD RID: 5325 RVA: 0x0007E178 File Offset: 0x0007C378
		public bool LogAttempt()
		{
			this.lastAttempt = DateTime.Now;
			this.count++;
			return this.count < TelnetConsole.maxLoginAttempts;
		}

		// Token: 0x060014CE RID: 5326 RVA: 0x0007E1A0 File Offset: 0x0007C3A0
		public bool IsBanned()
		{
			if ((DateTime.Now - this.lastAttempt).TotalSeconds > (double)TelnetConsole.blockTimeSeconds)
			{
				this.count = 0;
			}
			return this.count >= TelnetConsole.maxLoginAttempts;
		}

		// Token: 0x04000DCC RID: 3532
		[PublicizedFrom(EAccessModifier.Private)]
		public int count;

		// Token: 0x04000DCD RID: 3533
		[PublicizedFrom(EAccessModifier.Private)]
		public DateTime lastAttempt = new DateTime(0L);
	}
}

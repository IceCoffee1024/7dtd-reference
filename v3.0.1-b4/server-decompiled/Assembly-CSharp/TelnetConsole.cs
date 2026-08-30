using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

// Token: 0x020002CC RID: 716
public class TelnetConsole : IConsoleServer
{
	// Token: 0x060014B7 RID: 5303 RVA: 0x0007D3A4 File Offset: 0x0007B5A4
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

	// Token: 0x060014B8 RID: 5304 RVA: 0x0007D494 File Offset: 0x0007B694
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

	// Token: 0x060014B9 RID: 5305 RVA: 0x0007D608 File Offset: 0x0007B808
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

	// Token: 0x060014BA RID: 5306 RVA: 0x0007D65C File Offset: 0x0007B85C
	public void ConnectionClosed(TelnetConnection _con)
	{
		List<TelnetConnection> list = this.connections;
		lock (list)
		{
			this.connections.Remove(_con);
		}
	}

	// Token: 0x060014BB RID: 5307 RVA: 0x0007D6A4 File Offset: 0x0007B8A4
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

	// Token: 0x060014BC RID: 5308 RVA: 0x0007D770 File Offset: 0x0007B970
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

	// Token: 0x060014BD RID: 5309 RVA: 0x0007D7EC File Offset: 0x0007B9EC
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

	// Token: 0x04000DBC RID: 3516
	[PublicizedFrom(EAccessModifier.Private)]
	public static int maxLoginAttempts;

	// Token: 0x04000DBD RID: 3517
	[PublicizedFrom(EAccessModifier.Private)]
	public static int blockTimeSeconds;

	// Token: 0x04000DBE RID: 3518
	[PublicizedFrom(EAccessModifier.Private)]
	public TcpListener listener;

	// Token: 0x04000DBF RID: 3519
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly bool authEnabled;

	// Token: 0x04000DC0 RID: 3520
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<TelnetConnection> connections = new List<TelnetConnection>();

	// Token: 0x04000DC1 RID: 3521
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<int, TelnetConsole.LoginAttempts> loginAttemptsPerIP = new Dictionary<int, TelnetConsole.LoginAttempts>();

	// Token: 0x020002CD RID: 717
	[PublicizedFrom(EAccessModifier.Private)]
	public class LoginAttempts
	{
		// Token: 0x060014BE RID: 5310 RVA: 0x0007D86C File Offset: 0x0007BA6C
		public bool LogAttempt()
		{
			this.lastAttempt = DateTime.Now;
			this.count++;
			return this.count < TelnetConsole.maxLoginAttempts;
		}

		// Token: 0x060014BF RID: 5311 RVA: 0x0007D894 File Offset: 0x0007BA94
		public bool IsBanned()
		{
			if ((DateTime.Now - this.lastAttempt).TotalSeconds > (double)TelnetConsole.blockTimeSeconds)
			{
				this.count = 0;
			}
			return this.count >= TelnetConsole.maxLoginAttempts;
		}

		// Token: 0x04000DC2 RID: 3522
		[PublicizedFrom(EAccessModifier.Private)]
		public int count;

		// Token: 0x04000DC3 RID: 3523
		[PublicizedFrom(EAccessModifier.Private)]
		public DateTime lastAttempt = new DateTime(0L);
	}
}

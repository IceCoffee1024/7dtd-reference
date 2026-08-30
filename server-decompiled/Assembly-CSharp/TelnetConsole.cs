using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

// Token: 0x020002D0 RID: 720
public class TelnetConsole : IConsoleServer
{
	// Token: 0x060014C9 RID: 5321 RVA: 0x0007D9FC File Offset: 0x0007BBFC
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

	// Token: 0x060014CA RID: 5322 RVA: 0x0007DAEC File Offset: 0x0007BCEC
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

	// Token: 0x060014CB RID: 5323 RVA: 0x0007DC60 File Offset: 0x0007BE60
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

	// Token: 0x060014CC RID: 5324 RVA: 0x0007DCB4 File Offset: 0x0007BEB4
	public void ConnectionClosed(TelnetConnection _con)
	{
		List<TelnetConnection> list = this.connections;
		lock (list)
		{
			this.connections.Remove(_con);
		}
	}

	// Token: 0x060014CD RID: 5325 RVA: 0x0007DCFC File Offset: 0x0007BEFC
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

	// Token: 0x060014CE RID: 5326 RVA: 0x0007DDC8 File Offset: 0x0007BFC8
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

	// Token: 0x060014CF RID: 5327 RVA: 0x0007DE44 File Offset: 0x0007C044
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

	// Token: 0x04000DCD RID: 3533
	[PublicizedFrom(EAccessModifier.Private)]
	public static int maxLoginAttempts;

	// Token: 0x04000DCE RID: 3534
	[PublicizedFrom(EAccessModifier.Private)]
	public static int blockTimeSeconds;

	// Token: 0x04000DCF RID: 3535
	[PublicizedFrom(EAccessModifier.Private)]
	public TcpListener listener;

	// Token: 0x04000DD0 RID: 3536
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly bool authEnabled;

	// Token: 0x04000DD1 RID: 3537
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<TelnetConnection> connections = new List<TelnetConnection>();

	// Token: 0x04000DD2 RID: 3538
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<int, TelnetConsole.LoginAttempts> loginAttemptsPerIP = new Dictionary<int, TelnetConsole.LoginAttempts>();

	// Token: 0x020002D1 RID: 721
	[PublicizedFrom(EAccessModifier.Private)]
	public class LoginAttempts
	{
		// Token: 0x060014D0 RID: 5328 RVA: 0x0007DEC4 File Offset: 0x0007C0C4
		public bool LogAttempt()
		{
			this.lastAttempt = DateTime.Now;
			this.count++;
			return this.count < TelnetConsole.maxLoginAttempts;
		}

		// Token: 0x060014D1 RID: 5329 RVA: 0x0007DEEC File Offset: 0x0007C0EC
		public bool IsBanned()
		{
			if ((DateTime.Now - this.lastAttempt).TotalSeconds > (double)TelnetConsole.blockTimeSeconds)
			{
				this.count = 0;
			}
			return this.count >= TelnetConsole.maxLoginAttempts;
		}

		// Token: 0x04000DD3 RID: 3539
		[PublicizedFrom(EAccessModifier.Private)]
		public int count;

		// Token: 0x04000DD4 RID: 3540
		[PublicizedFrom(EAccessModifier.Private)]
		public DateTime lastAttempt = new DateTime(0L);
	}
}

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

// Token: 0x02000897 RID: 2199
public class ServerInformationTcpProvider
{
	// Token: 0x1700069F RID: 1695
	// (get) Token: 0x06003FD2 RID: 16338 RVA: 0x0018F798 File Offset: 0x0018D998
	public static ServerInformationTcpProvider Instance
	{
		get
		{
			ServerInformationTcpProvider serverInformationTcpProvider;
			if ((serverInformationTcpProvider = ServerInformationTcpProvider.instance) == null)
			{
				serverInformationTcpProvider = (ServerInformationTcpProvider.instance = new ServerInformationTcpProvider());
			}
			return serverInformationTcpProvider;
		}
	}

	// Token: 0x06003FD3 RID: 16339 RVA: 0x0018F7AE File Offset: 0x0018D9AE
	[PublicizedFrom(EAccessModifier.Private)]
	public ServerInformationTcpProvider()
	{
		this.gameInfoProviderCallback = new AsyncCallback(this.AcceptTcpClient);
	}

	// Token: 0x06003FD4 RID: 16340 RVA: 0x0018F7E4 File Offset: 0x0018D9E4
	public void StartServer()
	{
		object obj = this.gameInfoLock;
		lock (obj)
		{
			try
			{
				this.gameInfoProvider = new TcpListener(IPAddress.Any, GamePrefs.GetInt(EnumGamePrefs.ServerPort));
				this.gameInfoProvider.Start();
				this.gameInfoProvider.BeginAcceptTcpClient(this.gameInfoProviderCallback, null);
				if (SingletonMonoBehaviour<ConnectionManager>.Instance != null && SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo != null)
				{
					SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo.OnChangedAny += this.updateServer;
				}
			}
			catch (Exception ex)
			{
				Log.Exception(ex);
			}
		}
	}

	// Token: 0x06003FD5 RID: 16341 RVA: 0x0018F89C File Offset: 0x0018DA9C
	public void StopServer()
	{
		object obj = this.gameInfoLock;
		lock (obj)
		{
			if (this.gameInfoProvider != null)
			{
				if (SingletonMonoBehaviour<ConnectionManager>.Instance != null && SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo != null)
				{
					SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo.OnChangedAny -= this.updateServer;
				}
				this.gameInfoProvider.Stop();
				this.bufferLength = 0;
				this.gameInfoProvider = null;
			}
		}
	}

	// Token: 0x06003FD6 RID: 16342 RVA: 0x0018F930 File Offset: 0x0018DB30
	public string GetServerPorts()
	{
		return GamePrefs.GetInt(EnumGamePrefs.ServerPort).ToString() + "/TCP";
	}

	// Token: 0x06003FD7 RID: 16343 RVA: 0x0018F958 File Offset: 0x0018DB58
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateServer(GameServerInfo _gameServerInfo)
	{
		object obj = this.gameInfoLock;
		lock (obj)
		{
			if (this.gameInfoProvider != null)
			{
				this.bufferLength = 0;
			}
		}
	}

	// Token: 0x06003FD8 RID: 16344 RVA: 0x0018F9A4 File Offset: 0x0018DBA4
	[PublicizedFrom(EAccessModifier.Private)]
	public void AcceptTcpClient(IAsyncResult _asyncResult)
	{
		object obj = this.gameInfoLock;
		lock (obj)
		{
			if (this.gameInfoProvider != null && this.gameInfoProvider.Server.IsBound)
			{
				TcpClient tcpClient = null;
				bool flag2 = false;
				try
				{
					tcpClient = this.gameInfoProvider.EndAcceptTcpClient(_asyncResult);
					this.gameInfoProvider.BeginAcceptTcpClient(this.gameInfoProviderCallback, null);
					flag2 = true;
				}
				catch (SocketException ex)
				{
					Log.Warning(string.Format("[NET] Info Provider exception while waiting for a client to connect: {0}", ex));
					return;
				}
				catch (ObjectDisposedException)
				{
					return;
				}
				finally
				{
					if (!flag2 && tcpClient != null)
					{
						tcpClient.Dispose();
					}
				}
				using (TcpClient tcpClient2 = tcpClient)
				{
					tcpClient2.SendTimeout = 50;
					tcpClient2.LingerState = new LingerOption(true, 1);
					NetworkStream stream = tcpClient2.GetStream();
					if (this.bufferLength <= 0)
					{
						string text = SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo.ToString(true);
						try
						{
							int byteCount = Encoding.UTF8.GetByteCount(text);
							if (byteCount > this.buffer.Length)
							{
								Log.Error(string.Format("[NET] Can not provide server information on the info port: Server info size ({0}) exceeds buffer size ({1}), probably due to ServerDescription and/or ServerLoginConfirmationText", byteCount, this.buffer.Length));
							}
							else
							{
								this.bufferLength = Encoding.UTF8.GetBytes(text, 0, text.Length, this.buffer, 0);
							}
						}
						catch (Exception ex2)
						{
							Log.Error("[NET] Could not provide server information on the info port:");
							Log.Exception(ex2);
						}
					}
					stream.WriteByte((byte)(this.bufferLength / 10000 % 10 + 48));
					stream.WriteByte((byte)(this.bufferLength / 1000 % 10 + 48));
					stream.WriteByte((byte)(this.bufferLength / 100 % 10 + 48));
					stream.WriteByte((byte)(this.bufferLength / 10 % 10 + 48));
					stream.WriteByte((byte)(this.bufferLength / 1 % 10 + 48));
					stream.WriteByte(13);
					stream.WriteByte(10);
					if (this.bufferLength > 0)
					{
						stream.Write(this.buffer, 0, this.bufferLength);
					}
					stream.Flush();
					stream.Close(100);
					tcpClient2.Close();
				}
			}
		}
	}

	// Token: 0x04003352 RID: 13138
	public const int BufferSize = 32768;

	// Token: 0x04003353 RID: 13139
	[PublicizedFrom(EAccessModifier.Private)]
	public static ServerInformationTcpProvider instance;

	// Token: 0x04003354 RID: 13140
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object gameInfoLock = new object();

	// Token: 0x04003355 RID: 13141
	[PublicizedFrom(EAccessModifier.Private)]
	public TcpListener gameInfoProvider;

	// Token: 0x04003356 RID: 13142
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly AsyncCallback gameInfoProviderCallback;

	// Token: 0x04003357 RID: 13143
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly byte[] buffer = new byte[32768];

	// Token: 0x04003358 RID: 13144
	[PublicizedFrom(EAccessModifier.Private)]
	public int bufferLength;
}

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

// Token: 0x020008A6 RID: 2214
public class ServerInformationTcpProvider
{
	// Token: 0x170006CA RID: 1738
	// (get) Token: 0x06003FC5 RID: 16325 RVA: 0x00191658 File Offset: 0x0018F858
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

	// Token: 0x06003FC6 RID: 16326 RVA: 0x0019166E File Offset: 0x0018F86E
	[PublicizedFrom(EAccessModifier.Private)]
	public ServerInformationTcpProvider()
	{
		this.gameInfoProviderCallback = new AsyncCallback(this.AcceptTcpClient);
	}

	// Token: 0x06003FC7 RID: 16327 RVA: 0x001916A4 File Offset: 0x0018F8A4
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

	// Token: 0x06003FC8 RID: 16328 RVA: 0x0019175C File Offset: 0x0018F95C
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

	// Token: 0x06003FC9 RID: 16329 RVA: 0x001917F0 File Offset: 0x0018F9F0
	public string GetServerPorts()
	{
		return GamePrefs.GetInt(EnumGamePrefs.ServerPort).ToString() + "/TCP";
	}

	// Token: 0x06003FCA RID: 16330 RVA: 0x00191818 File Offset: 0x0018FA18
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

	// Token: 0x06003FCB RID: 16331 RVA: 0x00191864 File Offset: 0x0018FA64
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

	// Token: 0x040033BE RID: 13246
	public const int BufferSize = 32768;

	// Token: 0x040033BF RID: 13247
	[PublicizedFrom(EAccessModifier.Private)]
	public static ServerInformationTcpProvider instance;

	// Token: 0x040033C0 RID: 13248
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object gameInfoLock = new object();

	// Token: 0x040033C1 RID: 13249
	[PublicizedFrom(EAccessModifier.Private)]
	public TcpListener gameInfoProvider;

	// Token: 0x040033C2 RID: 13250
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly AsyncCallback gameInfoProviderCallback;

	// Token: 0x040033C3 RID: 13251
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly byte[] buffer = new byte[32768];

	// Token: 0x040033C4 RID: 13252
	[PublicizedFrom(EAccessModifier.Private)]
	public int bufferLength;
}

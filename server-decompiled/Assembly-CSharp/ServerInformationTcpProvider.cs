using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

// Token: 0x020008A3 RID: 2211
public class ServerInformationTcpProvider
{
	// Token: 0x170006AB RID: 1707
	// (get) Token: 0x06004037 RID: 16439 RVA: 0x00191868 File Offset: 0x0018FA68
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

	// Token: 0x06004038 RID: 16440 RVA: 0x0019187E File Offset: 0x0018FA7E
	[PublicizedFrom(EAccessModifier.Private)]
	public ServerInformationTcpProvider()
	{
		this.gameInfoProviderCallback = new AsyncCallback(this.AcceptTcpClient);
	}

	// Token: 0x06004039 RID: 16441 RVA: 0x001918B4 File Offset: 0x0018FAB4
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

	// Token: 0x0600403A RID: 16442 RVA: 0x0019196C File Offset: 0x0018FB6C
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

	// Token: 0x0600403B RID: 16443 RVA: 0x00191A00 File Offset: 0x0018FC00
	public string GetServerPorts()
	{
		return GamePrefs.GetInt(EnumGamePrefs.ServerPort).ToString() + "/TCP";
	}

	// Token: 0x0600403C RID: 16444 RVA: 0x00191A28 File Offset: 0x0018FC28
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

	// Token: 0x0600403D RID: 16445 RVA: 0x00191A74 File Offset: 0x0018FC74
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

	// Token: 0x040033D3 RID: 13267
	public const int BufferSize = 32768;

	// Token: 0x040033D4 RID: 13268
	[PublicizedFrom(EAccessModifier.Private)]
	public static ServerInformationTcpProvider instance;

	// Token: 0x040033D5 RID: 13269
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object gameInfoLock = new object();

	// Token: 0x040033D6 RID: 13270
	[PublicizedFrom(EAccessModifier.Private)]
	public TcpListener gameInfoProvider;

	// Token: 0x040033D7 RID: 13271
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly AsyncCallback gameInfoProviderCallback;

	// Token: 0x040033D8 RID: 13272
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly byte[] buffer = new byte[32768];

	// Token: 0x040033D9 RID: 13273
	[PublicizedFrom(EAccessModifier.Private)]
	public int bufferLength;
}

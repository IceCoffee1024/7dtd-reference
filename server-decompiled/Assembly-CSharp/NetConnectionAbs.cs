using System;
using System.Collections.Generic;
using System.IO;
using Noemax.GZip;
using UnityEngine.Profiling;

// Token: 0x02000772 RID: 1906
public abstract class NetConnectionAbs : INetConnection
{
	// Token: 0x060038B7 RID: 14519 RVA: 0x00173B68 File Offset: 0x00171D68
	[PublicizedFrom(EAccessModifier.Protected)]
	public NetConnectionAbs(int _channel, ClientInfo _clientInfo, INetworkClient _netClient, string _uniqueId)
	{
		this.channel = _channel;
		this.cInfo = _clientInfo;
		this.netClient = _netClient;
		this.isServer = _clientInfo != null;
		NetConnectionAbs.encryptedStreamReceived = false;
		this.connectionIdentifier = (this.isServer ? (_uniqueId + "_" + _channel.ToString()) : _channel.ToString());
		NetPackageLogger.BeginLog(this.isServer);
	}

	// Token: 0x060038B8 RID: 14520 RVA: 0x00173C0D File Offset: 0x00171E0D
	public void SetEncryptionModule(IEncryptionModule _module)
	{
		this.encryptionModule = _module;
	}

	// Token: 0x060038B9 RID: 14521 RVA: 0x00173C18 File Offset: 0x00171E18
	public virtual void Disconnect(bool _kick)
	{
		if (!this.bDisconnected)
		{
			NetPackageLogger.EndLog();
		}
		this.bDisconnected = true;
		if (_kick && this.cInfo != null)
		{
			this.cInfo.disconnecting = true;
			SingletonMonoBehaviour<ConnectionManager>.Instance.DisconnectClient(this.cInfo, false, false);
		}
	}

	// Token: 0x060038BA RID: 14522 RVA: 0x00173C66 File Offset: 0x00171E66
	public virtual bool IsDisconnected()
	{
		return this.bDisconnected;
	}

	// Token: 0x060038BB RID: 14523 RVA: 0x00173C70 File Offset: 0x00171E70
	public virtual void GetPackages(List<NetPackage> _dstBuf)
	{
		_dstBuf.Clear();
		if (this.receivedPackages.Count == 0)
		{
			return;
		}
		List<NetPackage> list = this.receivedPackages;
		lock (list)
		{
			_dstBuf.AddRange(this.receivedPackages);
			this.receivedPackages.Clear();
		}
	}

	// Token: 0x060038BC RID: 14524 RVA: 0x00173CD8 File Offset: 0x00171ED8
	public virtual void AddToSendQueue(List<NetPackage> _packages)
	{
		for (int i = 0; i < _packages.Count; i++)
		{
			this.AddToSendQueue(_packages[i]);
		}
	}

	// Token: 0x060038BD RID: 14525 RVA: 0x00173D03 File Offset: 0x00171F03
	public virtual void UpgradeToFullConnection()
	{
		this.InitStreams(true);
		this.allowCompression = true;
	}

	// Token: 0x060038BE RID: 14526 RVA: 0x00173D13 File Offset: 0x00171F13
	public virtual NetConnectionStatistics GetStats()
	{
		return this.stats;
	}

	// Token: 0x060038BF RID: 14527 RVA: 0x00173D1C File Offset: 0x00171F1C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool Compress(bool _compress, MemoryStream _uncompressedSourceStream, DeflateOutputStream _zipTargetStream, MemoryStream _compressedTargetStream, byte[] _copyBuffer, int _packageCount)
	{
		if (_compress)
		{
			_compressedTargetStream.SetLength(0L);
			try
			{
				StreamUtils.StreamCopy(_uncompressedSourceStream, _zipTargetStream, _copyBuffer, true);
			}
			catch (Exception)
			{
				Log.Error(string.Concat(new string[]
				{
					"Compressed buffer size too small: Source stream size (",
					_uncompressedSourceStream.Length.ToString(),
					") > compressed stream capacity (",
					_compressedTargetStream.Capacity.ToString(),
					"), packages: ",
					_packageCount.ToString()
				}));
				throw;
			}
			_zipTargetStream.Restart();
			_compressedTargetStream.Position = 0L;
		}
		return _compress;
	}

	// Token: 0x060038C0 RID: 14528 RVA: 0x00173DBC File Offset: 0x00171FBC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool Decompress(bool _compressed, MemoryStream _uncompressedTargetStream, DeflateInputStream _unzipSourceStream, byte[] _copyBuffer)
	{
		if (_compressed)
		{
			_uncompressedTargetStream.SetLength(0L);
			_unzipSourceStream.Restart();
			try
			{
				StreamUtils.StreamCopy(_unzipSourceStream, _uncompressedTargetStream, _copyBuffer, true);
			}
			catch (Exception ex)
			{
				Log.Exception(ex);
				throw;
			}
			_uncompressedTargetStream.Position = 0L;
		}
		return _compressed;
	}

	// Token: 0x060038C1 RID: 14529 RVA: 0x00173E08 File Offset: 0x00172008
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool EnableEncryptData()
	{
		return this.encryptionModule != null && (this.isServer || NetConnectionAbs.encryptedStreamReceived);
	}

	// Token: 0x060038C2 RID: 14530 RVA: 0x00173E23 File Offset: 0x00172023
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool ExpectEncryptedData()
	{
		return this.encryptionModule != null && this.isServer && this.cInfo.loginDone;
	}

	// Token: 0x060038C3 RID: 14531 RVA: 0x00173E44 File Offset: 0x00172044
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool Encrypt(MemoryStream _stream)
	{
		if (this.EnableEncryptData())
		{
			bool flag = this.encryptionModule.EncryptStream(this.cInfo, _stream);
			_stream.Position = 0L;
			return flag;
		}
		return false;
	}

	// Token: 0x060038C4 RID: 14532 RVA: 0x00173E6C File Offset: 0x0017206C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool Decrypt(bool _bEncrypted, MemoryStream _stream)
	{
		if (_bEncrypted)
		{
			NetConnectionAbs.encryptedStreamReceived = true;
			bool flag = this.encryptionModule.DecryptStream(this.cInfo, _stream);
			_stream.Position = 0L;
			if (!flag)
			{
				if (this.isServer)
				{
					GameUtils.KickPlayerForClientInfo(this.cInfo, new GameUtils.KickPlayerData(GameUtils.EKickReason.EncryptionFailure, 0, default(DateTime), ""));
					return flag;
				}
				GameUtils.ForceDisconnect(new GameUtils.KickPlayerData(GameUtils.EKickReason.EncryptionFailure, 0, default(DateTime), ""));
			}
			return flag;
		}
		if (!this.ExpectEncryptedData())
		{
			return true;
		}
		Log.Error(string.Format("[NET] Client logged in but sent unencrypted message, dropping! {0}", this.cInfo));
		this.cInfo.loginDone = false;
		GameUtils.KickPlayerForClientInfo(this.cInfo, new GameUtils.KickPlayerData(GameUtils.EKickReason.EncryptionFailure, 0, default(DateTime), ""));
		return false;
	}

	// Token: 0x060038C5 RID: 14533 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void FlushSendQueue()
	{
	}

	// Token: 0x060038C6 RID: 14534
	public abstract void AddToSendQueue(NetPackage _package);

	// Token: 0x060038C7 RID: 14535
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void InitStreams(bool _full);

	// Token: 0x060038C8 RID: 14536
	public abstract void AppendToReaderStream(byte[] _data, int _size);

	// Token: 0x04002E86 RID: 11910
	public const int PROCESSING_BUFFER_SIZE = 2097152;

	// Token: 0x04002E87 RID: 11911
	public const int COMPRESSED_BUFFER_SIZE = 2097152;

	// Token: 0x04002E88 RID: 11912
	[PublicizedFrom(EAccessModifier.Protected)]
	public const int PREAUTH_BUFFER_SIZE = 32768;

	// Token: 0x04002E89 RID: 11913
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly int channel;

	// Token: 0x04002E8A RID: 11914
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly ClientInfo cInfo;

	// Token: 0x04002E8B RID: 11915
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly INetworkClient netClient;

	// Token: 0x04002E8C RID: 11916
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly bool isServer;

	// Token: 0x04002E8D RID: 11917
	[PublicizedFrom(EAccessModifier.Private)]
	public IEncryptionModule encryptionModule;

	// Token: 0x04002E8E RID: 11918
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly string connectionIdentifier;

	// Token: 0x04002E8F RID: 11919
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool fullConnection;

	// Token: 0x04002E90 RID: 11920
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool allowCompression;

	// Token: 0x04002E91 RID: 11921
	[PublicizedFrom(EAccessModifier.Protected)]
	public static bool encryptedStreamReceived;

	// Token: 0x04002E92 RID: 11922
	[PublicizedFrom(EAccessModifier.Protected)]
	public volatile bool bDisconnected;

	// Token: 0x04002E93 RID: 11923
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly NetConnectionStatistics stats = new NetConnectionStatistics();

	// Token: 0x04002E94 RID: 11924
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly List<NetPackage> receivedPackages = new List<NetPackage>();

	// Token: 0x04002E95 RID: 11925
	[PublicizedFrom(EAccessModifier.Private)]
	public CustomSampler threadSamplerEncrypt = CustomSampler.Create("Encrypt", false);

	// Token: 0x04002E96 RID: 11926
	[PublicizedFrom(EAccessModifier.Private)]
	public CustomSampler threadSamplerDecrypt = CustomSampler.Create("Decrypt", false);
}

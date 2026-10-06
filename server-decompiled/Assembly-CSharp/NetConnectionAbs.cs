using System;
using System.Collections.Generic;
using System.IO;
using Noemax.GZip;
using UnityEngine.Profiling;

// Token: 0x02000774 RID: 1908
public abstract class NetConnectionAbs : INetConnection
{
	// Token: 0x060038FA RID: 14586 RVA: 0x00174494 File Offset: 0x00172694
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

	// Token: 0x060038FB RID: 14587 RVA: 0x00174539 File Offset: 0x00172739
	public void SetEncryptionModule(IEncryptionModule _module)
	{
		this.encryptionModule = _module;
	}

	// Token: 0x060038FC RID: 14588 RVA: 0x00174544 File Offset: 0x00172744
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

	// Token: 0x060038FD RID: 14589 RVA: 0x00174592 File Offset: 0x00172792
	public virtual bool IsDisconnected()
	{
		return this.bDisconnected;
	}

	// Token: 0x060038FE RID: 14590 RVA: 0x0017459C File Offset: 0x0017279C
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

	// Token: 0x060038FF RID: 14591 RVA: 0x00174604 File Offset: 0x00172804
	public virtual void AddToSendQueue(List<NetPackage> _packages)
	{
		for (int i = 0; i < _packages.Count; i++)
		{
			this.AddToSendQueue(_packages[i]);
		}
	}

	// Token: 0x06003900 RID: 14592 RVA: 0x0017462F File Offset: 0x0017282F
	public virtual void UpgradeToFullConnection()
	{
		this.InitStreams(true);
		this.allowCompression = true;
	}

	// Token: 0x06003901 RID: 14593 RVA: 0x0017463F File Offset: 0x0017283F
	public virtual NetConnectionStatistics GetStats()
	{
		return this.stats;
	}

	// Token: 0x06003902 RID: 14594 RVA: 0x00174648 File Offset: 0x00172848
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

	// Token: 0x06003903 RID: 14595 RVA: 0x001746E8 File Offset: 0x001728E8
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

	// Token: 0x06003904 RID: 14596 RVA: 0x00174734 File Offset: 0x00172934
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool EnableEncryptData()
	{
		return this.encryptionModule != null && (this.isServer || NetConnectionAbs.encryptedStreamReceived);
	}

	// Token: 0x06003905 RID: 14597 RVA: 0x0017474F File Offset: 0x0017294F
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool ExpectEncryptedData()
	{
		return this.encryptionModule != null && this.isServer && this.cInfo.loginDone;
	}

	// Token: 0x06003906 RID: 14598 RVA: 0x00174770 File Offset: 0x00172970
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

	// Token: 0x06003907 RID: 14599 RVA: 0x00174798 File Offset: 0x00172998
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

	// Token: 0x06003908 RID: 14600 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void FlushSendQueue()
	{
	}

	// Token: 0x06003909 RID: 14601
	public abstract void AddToSendQueue(NetPackage _package);

	// Token: 0x0600390A RID: 14602
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void InitStreams(bool _full);

	// Token: 0x0600390B RID: 14603
	public abstract void AppendToReaderStream(byte[] _data, int _size);

	// Token: 0x04002E7F RID: 11903
	public const int PROCESSING_BUFFER_SIZE = 2097152;

	// Token: 0x04002E80 RID: 11904
	public const int COMPRESSED_BUFFER_SIZE = 2097152;

	// Token: 0x04002E81 RID: 11905
	[PublicizedFrom(EAccessModifier.Protected)]
	public const int PREAUTH_BUFFER_SIZE = 32768;

	// Token: 0x04002E82 RID: 11906
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly int channel;

	// Token: 0x04002E83 RID: 11907
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly ClientInfo cInfo;

	// Token: 0x04002E84 RID: 11908
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly INetworkClient netClient;

	// Token: 0x04002E85 RID: 11909
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly bool isServer;

	// Token: 0x04002E86 RID: 11910
	[PublicizedFrom(EAccessModifier.Private)]
	public IEncryptionModule encryptionModule;

	// Token: 0x04002E87 RID: 11911
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly string connectionIdentifier;

	// Token: 0x04002E88 RID: 11912
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool fullConnection;

	// Token: 0x04002E89 RID: 11913
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool allowCompression;

	// Token: 0x04002E8A RID: 11914
	[PublicizedFrom(EAccessModifier.Protected)]
	public static bool encryptedStreamReceived;

	// Token: 0x04002E8B RID: 11915
	[PublicizedFrom(EAccessModifier.Protected)]
	public volatile bool bDisconnected;

	// Token: 0x04002E8C RID: 11916
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly NetConnectionStatistics stats = new NetConnectionStatistics();

	// Token: 0x04002E8D RID: 11917
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly List<NetPackage> receivedPackages = new List<NetPackage>();

	// Token: 0x04002E8E RID: 11918
	[PublicizedFrom(EAccessModifier.Private)]
	public CustomSampler threadSamplerEncrypt = CustomSampler.Create("Encrypt", false);

	// Token: 0x04002E8F RID: 11919
	[PublicizedFrom(EAccessModifier.Private)]
	public CustomSampler threadSamplerDecrypt = CustomSampler.Create("Decrypt", false);
}

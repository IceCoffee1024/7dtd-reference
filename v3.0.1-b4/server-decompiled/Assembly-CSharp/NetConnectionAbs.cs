using System;
using System.Collections.Generic;
using System.IO;
using Noemax.GZip;
using UnityEngine.Profiling;

// Token: 0x02000768 RID: 1896
public abstract class NetConnectionAbs : INetConnection
{
	// Token: 0x0600385F RID: 14431 RVA: 0x001718E8 File Offset: 0x0016FAE8
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

	// Token: 0x06003860 RID: 14432 RVA: 0x0017198D File Offset: 0x0016FB8D
	public void SetEncryptionModule(IEncryptionModule _module)
	{
		this.encryptionModule = _module;
	}

	// Token: 0x06003861 RID: 14433 RVA: 0x00171998 File Offset: 0x0016FB98
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

	// Token: 0x06003862 RID: 14434 RVA: 0x001719E6 File Offset: 0x0016FBE6
	public virtual bool IsDisconnected()
	{
		return this.bDisconnected;
	}

	// Token: 0x06003863 RID: 14435 RVA: 0x001719F0 File Offset: 0x0016FBF0
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

	// Token: 0x06003864 RID: 14436 RVA: 0x00171A58 File Offset: 0x0016FC58
	public virtual void AddToSendQueue(List<NetPackage> _packages)
	{
		for (int i = 0; i < _packages.Count; i++)
		{
			this.AddToSendQueue(_packages[i]);
		}
	}

	// Token: 0x06003865 RID: 14437 RVA: 0x00171A83 File Offset: 0x0016FC83
	public virtual void UpgradeToFullConnection()
	{
		this.InitStreams(true);
		this.allowCompression = true;
	}

	// Token: 0x06003866 RID: 14438 RVA: 0x00171A93 File Offset: 0x0016FC93
	public virtual NetConnectionStatistics GetStats()
	{
		return this.stats;
	}

	// Token: 0x06003867 RID: 14439 RVA: 0x00171A9C File Offset: 0x0016FC9C
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

	// Token: 0x06003868 RID: 14440 RVA: 0x00171B3C File Offset: 0x0016FD3C
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

	// Token: 0x06003869 RID: 14441 RVA: 0x00171B88 File Offset: 0x0016FD88
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool EnableEncryptData()
	{
		return this.encryptionModule != null && (this.isServer || NetConnectionAbs.encryptedStreamReceived);
	}

	// Token: 0x0600386A RID: 14442 RVA: 0x00171BA3 File Offset: 0x0016FDA3
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool ExpectEncryptedData()
	{
		return this.encryptionModule != null && this.isServer && this.cInfo.loginDone;
	}

	// Token: 0x0600386B RID: 14443 RVA: 0x00171BC4 File Offset: 0x0016FDC4
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

	// Token: 0x0600386C RID: 14444 RVA: 0x00171BEC File Offset: 0x0016FDEC
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

	// Token: 0x0600386D RID: 14445 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void FlushSendQueue()
	{
	}

	// Token: 0x0600386E RID: 14446
	public abstract void AddToSendQueue(NetPackage _package);

	// Token: 0x0600386F RID: 14447
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void InitStreams(bool _full);

	// Token: 0x06003870 RID: 14448
	public abstract void AppendToReaderStream(byte[] _data, int _size);

	// Token: 0x04002E0D RID: 11789
	public const int PROCESSING_BUFFER_SIZE = 2097152;

	// Token: 0x04002E0E RID: 11790
	public const int COMPRESSED_BUFFER_SIZE = 2097152;

	// Token: 0x04002E0F RID: 11791
	[PublicizedFrom(EAccessModifier.Protected)]
	public const int PREAUTH_BUFFER_SIZE = 32768;

	// Token: 0x04002E10 RID: 11792
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly int channel;

	// Token: 0x04002E11 RID: 11793
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly ClientInfo cInfo;

	// Token: 0x04002E12 RID: 11794
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly INetworkClient netClient;

	// Token: 0x04002E13 RID: 11795
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly bool isServer;

	// Token: 0x04002E14 RID: 11796
	[PublicizedFrom(EAccessModifier.Private)]
	public IEncryptionModule encryptionModule;

	// Token: 0x04002E15 RID: 11797
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly string connectionIdentifier;

	// Token: 0x04002E16 RID: 11798
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool fullConnection;

	// Token: 0x04002E17 RID: 11799
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool allowCompression;

	// Token: 0x04002E18 RID: 11800
	[PublicizedFrom(EAccessModifier.Protected)]
	public static bool encryptedStreamReceived;

	// Token: 0x04002E19 RID: 11801
	[PublicizedFrom(EAccessModifier.Protected)]
	public volatile bool bDisconnected;

	// Token: 0x04002E1A RID: 11802
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly NetConnectionStatistics stats = new NetConnectionStatistics();

	// Token: 0x04002E1B RID: 11803
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly List<NetPackage> receivedPackages = new List<NetPackage>();

	// Token: 0x04002E1C RID: 11804
	[PublicizedFrom(EAccessModifier.Private)]
	public CustomSampler threadSamplerEncrypt = CustomSampler.Create("Encrypt", false);

	// Token: 0x04002E1D RID: 11805
	[PublicizedFrom(EAccessModifier.Private)]
	public CustomSampler threadSamplerDecrypt = CustomSampler.Create("Decrypt", false);
}

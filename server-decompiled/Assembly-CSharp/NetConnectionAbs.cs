using System;
using System.Collections.Generic;
using System.IO;
using Noemax.GZip;
using UnityEngine.Profiling;

// Token: 0x0200077C RID: 1916
public abstract class NetConnectionAbs : INetConnection
{
	// Token: 0x06003911 RID: 14609 RVA: 0x0017542C File Offset: 0x0017362C
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

	// Token: 0x06003912 RID: 14610 RVA: 0x001754D1 File Offset: 0x001736D1
	public void SetEncryptionModule(IEncryptionModule _module)
	{
		this.encryptionModule = _module;
	}

	// Token: 0x06003913 RID: 14611 RVA: 0x001754DC File Offset: 0x001736DC
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

	// Token: 0x06003914 RID: 14612 RVA: 0x0017552A File Offset: 0x0017372A
	public virtual bool IsDisconnected()
	{
		return this.bDisconnected;
	}

	// Token: 0x06003915 RID: 14613 RVA: 0x00175534 File Offset: 0x00173734
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

	// Token: 0x06003916 RID: 14614 RVA: 0x0017559C File Offset: 0x0017379C
	public virtual void AddToSendQueue(List<NetPackage> _packages)
	{
		for (int i = 0; i < _packages.Count; i++)
		{
			this.AddToSendQueue(_packages[i]);
		}
	}

	// Token: 0x06003917 RID: 14615 RVA: 0x001755C7 File Offset: 0x001737C7
	public virtual void UpgradeToFullConnection()
	{
		this.InitStreams(true);
		this.allowCompression = true;
	}

	// Token: 0x06003918 RID: 14616 RVA: 0x001755D7 File Offset: 0x001737D7
	public virtual NetConnectionStatistics GetStats()
	{
		return this.stats;
	}

	// Token: 0x06003919 RID: 14617 RVA: 0x001755E0 File Offset: 0x001737E0
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

	// Token: 0x0600391A RID: 14618 RVA: 0x00175680 File Offset: 0x00173880
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

	// Token: 0x0600391B RID: 14619 RVA: 0x001756CC File Offset: 0x001738CC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool EnableEncryptData()
	{
		return this.encryptionModule != null && (this.isServer || NetConnectionAbs.encryptedStreamReceived);
	}

	// Token: 0x0600391C RID: 14620 RVA: 0x001756E7 File Offset: 0x001738E7
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool ExpectEncryptedData()
	{
		return this.encryptionModule != null && this.isServer && this.cInfo.loginDone;
	}

	// Token: 0x0600391D RID: 14621 RVA: 0x00175708 File Offset: 0x00173908
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

	// Token: 0x0600391E RID: 14622 RVA: 0x00175730 File Offset: 0x00173930
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

	// Token: 0x0600391F RID: 14623 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void FlushSendQueue()
	{
	}

	// Token: 0x06003920 RID: 14624
	public abstract void AddToSendQueue(NetPackage _package);

	// Token: 0x06003921 RID: 14625
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void InitStreams(bool _full);

	// Token: 0x06003922 RID: 14626
	public abstract void AppendToReaderStream(byte[] _data, int _size);

	// Token: 0x04002E9B RID: 11931
	public const int PROCESSING_BUFFER_SIZE = 2097152;

	// Token: 0x04002E9C RID: 11932
	public const int COMPRESSED_BUFFER_SIZE = 2097152;

	// Token: 0x04002E9D RID: 11933
	[PublicizedFrom(EAccessModifier.Protected)]
	public const int PREAUTH_BUFFER_SIZE = 32768;

	// Token: 0x04002E9E RID: 11934
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly int channel;

	// Token: 0x04002E9F RID: 11935
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly ClientInfo cInfo;

	// Token: 0x04002EA0 RID: 11936
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly INetworkClient netClient;

	// Token: 0x04002EA1 RID: 11937
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly bool isServer;

	// Token: 0x04002EA2 RID: 11938
	[PublicizedFrom(EAccessModifier.Private)]
	public IEncryptionModule encryptionModule;

	// Token: 0x04002EA3 RID: 11939
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly string connectionIdentifier;

	// Token: 0x04002EA4 RID: 11940
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool fullConnection;

	// Token: 0x04002EA5 RID: 11941
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool allowCompression;

	// Token: 0x04002EA6 RID: 11942
	[PublicizedFrom(EAccessModifier.Protected)]
	public static bool encryptedStreamReceived;

	// Token: 0x04002EA7 RID: 11943
	[PublicizedFrom(EAccessModifier.Protected)]
	public volatile bool bDisconnected;

	// Token: 0x04002EA8 RID: 11944
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly NetConnectionStatistics stats = new NetConnectionStatistics();

	// Token: 0x04002EA9 RID: 11945
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly List<NetPackage> receivedPackages = new List<NetPackage>();

	// Token: 0x04002EAA RID: 11946
	[PublicizedFrom(EAccessModifier.Private)]
	public CustomSampler threadSamplerEncrypt = CustomSampler.Create("Encrypt", false);

	// Token: 0x04002EAB RID: 11947
	[PublicizedFrom(EAccessModifier.Private)]
	public CustomSampler threadSamplerDecrypt = CustomSampler.Create("Decrypt", false);
}

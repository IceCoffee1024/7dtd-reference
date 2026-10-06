using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Noemax.GZip;

// Token: 0x02000778 RID: 1912
public class NetConnectionSteam : NetConnectionAbs
{
	// Token: 0x06003924 RID: 14628 RVA: 0x00175F40 File Offset: 0x00174140
	public NetConnectionSteam(int _channel, ClientInfo _clientInfo, INetworkClient _netClient, string _uniqueId)
		: base(_channel, _clientInfo, _netClient, _uniqueId)
	{
		this.copyBufferReader = new byte[4096];
		this.copyBufferWriter = new byte[4096];
		if (_clientInfo != null)
		{
			if (_channel == 0)
			{
				this.InitStreams(false);
			}
		}
		else
		{
			this.InitStreams(true);
		}
		this.readerThreadInfo = ThreadManager.StartThread("NCSteam_Reader_" + this.connectionIdentifier, new ThreadManager.ThreadFunctionDelegate(this.Task_CommReader), null, null, true, false);
		this.writerThreadInfo = ThreadManager.StartThread("NCSteam_Writer_" + this.connectionIdentifier, new ThreadManager.ThreadFunctionDelegate(this.Task_CommWriter), null, null, true, false);
	}

	// Token: 0x06003925 RID: 14629 RVA: 0x00176020 File Offset: 0x00174220
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InitStreams(bool _full)
	{
		if (this.fullConnection)
		{
			return;
		}
		if (_full)
		{
			byte[] array = new byte[2097152];
			this.receiveStreamCompressed = new MemoryStream(array, 0, array.Length, true, true);
			this.receiveStreamCompressed.SetLength(0L);
			byte[] array2 = new byte[2097152];
			this.receiveStreamUncompressed = new MemoryStream(array2, 0, array2.Length, true, true);
			this.receiveZipStream = new DeflateInputStream(this.receiveStreamCompressed, true);
			byte[] array3 = new byte[2097152];
			this.sendStreamUncompressed = new MemoryStream(array3, 0, array3.Length, true, true);
			if (this.sendStreamWriter == null)
			{
				this.sendStreamWriter = new PooledBinaryWriter();
			}
			this.sendStreamWriter.SetBaseStream(this.sendStreamUncompressed);
			byte[] array4 = new byte[2097152];
			this.sendStreamCompressed = new MemoryStream(array4, 0, array4.Length, true, true);
			this.sendZipStream = new DeflateOutputStream(this.sendStreamCompressed, 3, true);
			this.fullConnection = true;
			return;
		}
		byte[] array5 = new byte[32768];
		this.receiveStreamCompressed = new MemoryStream(array5, 0, array5.Length, true, true);
		this.receiveStreamCompressed.SetLength(0L);
		byte[] array6 = new byte[32768];
		this.sendStreamUncompressed = new MemoryStream(array6, 0, array6.Length, true, true);
		this.sendStreamWriter = new PooledBinaryWriter();
		this.sendStreamWriter.SetBaseStream(this.sendStreamUncompressed);
	}

	// Token: 0x06003926 RID: 14630 RVA: 0x00176177 File Offset: 0x00174377
	public override void Disconnect(bool _kick)
	{
		base.Disconnect(_kick);
		this.readerTriggerEvent.Set();
		this.writerTriggerEvent.Set();
	}

	// Token: 0x06003927 RID: 14631 RVA: 0x00176198 File Offset: 0x00174398
	public override void AddToSendQueue(NetPackage _package)
	{
		_package.RegisterSendQueue();
		Queue<NetPackage> queue = this.packetsToSend;
		lock (queue)
		{
			this.packetsToSend.Enqueue(_package);
		}
		this.writerTriggerEvent.Set();
	}

	// Token: 0x06003928 RID: 14632 RVA: 0x001761F0 File Offset: 0x001743F0
	public override void AppendToReaderStream(byte[] _data, int _size)
	{
		if (this.bDisconnected)
		{
			return;
		}
		ArrayListMP<byte> arrayListMP = new ArrayListMP<byte>(MemoryPools.poolByte, _size);
		Array.Copy(_data, arrayListMP.Items, _size);
		arrayListMP.Count = _size;
		MemoryPools.poolByte.Free(_data);
		this.bufsToRead.Enqueue(arrayListMP);
		this.readerTriggerEvent.Set();
	}

	// Token: 0x06003929 RID: 14633 RVA: 0x0017624C File Offset: 0x0017444C
	[PublicizedFrom(EAccessModifier.Private)]
	public void Task_CommReader(ThreadManager.ThreadInfo _threadInfo)
	{
		try
		{
			while (!this.bDisconnected)
			{
				this.readerTriggerEvent.WaitOne();
				if (this.bDisconnected)
				{
					break;
				}
				if (!this.bufsToRead.HasData())
				{
					this.readerTriggerEvent.Reset();
				}
				else
				{
					ArrayListMP<byte> arrayListMP = this.bufsToRead.Dequeue();
					this.stats.RegisterReceivedData(1, arrayListMP.Count);
					bool flag = arrayListMP.Items[0] == 1;
					bool flag2 = arrayListMP.Items[1] == 1;
					MemoryStream memoryStream = new MemoryStream(arrayListMP.Items, 2, arrayListMP.Count - 2);
					this.receiveStreamCompressed.SetLength(0L);
					StreamUtils.StreamCopy(memoryStream, this.receiveStreamCompressed, this.copyBufferReader, true);
					this.receiveStreamCompressed.Position = 0L;
					if (this.Decrypt(flag2, this.receiveStreamCompressed))
					{
						this.receiveStreamReader.SetBaseStream(this.Decompress(flag, this.receiveStreamUncompressed, this.receiveZipStream, this.copyBufferReader) ? this.receiveStreamUncompressed : this.receiveStreamCompressed);
						NetPackage netPackage;
						try
						{
							netPackage = NetPackageManager.ParsePackage(this.receiveStreamReader, this.cInfo);
						}
						catch (KeyNotFoundException)
						{
							this.receiveStreamReader.BaseStream.Position = 0L;
							Log.Error("[NET] Trying to parse package failed, package id {0} unknown!", new object[] { this.receiveStreamReader.ReadByte() });
							Log.Error("Length: {0}, Compressed: {1}", new object[]
							{
								arrayListMP.Count - 1,
								flag
							});
							string text = "Orig data: {0}";
							object[] array = new object[1];
							array[0] = string.Join(",", Array.ConvertAll<byte, string>(arrayListMP.Items, (byte _val) => _val.ToString("X2")));
							Log.Error(text, array);
							throw;
						}
						this.stats.RegisterReceivedPackage(netPackage.PackageId, (int)memoryStream.Length);
						List<NetPackage> receivedPackages = this.receivedPackages;
						lock (receivedPackages)
						{
							this.receivedPackages.Add(netPackage);
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			if (this.cInfo != null)
			{
				Log.Error(string.Format("Task_CommReaderSteam (cl={0}, ch={1}):", this.cInfo.PlatformId.CombinedString, this.channel));
			}
			else
			{
				Log.Error(string.Format("Task_CommReaderSteam (ch={0}):", this.channel));
			}
			Log.Exception(ex);
			this.Disconnect(true);
		}
	}

	// Token: 0x0600392A RID: 14634 RVA: 0x00176514 File Offset: 0x00174714
	[PublicizedFrom(EAccessModifier.Private)]
	public void Task_CommWriter(ThreadManager.ThreadInfo _threadInfo)
	{
		try
		{
			while (!this.bDisconnected)
			{
				NetPackage netPackage = null;
				this.writerTriggerEvent.WaitOne();
				if (this.bDisconnected)
				{
					break;
				}
				Queue<NetPackage> queue = this.packetsToSend;
				lock (queue)
				{
					if (this.packetsToSend.Count == 0)
					{
						this.writerTriggerEvent.Reset();
						continue;
					}
					netPackage = this.packetsToSend.Dequeue();
				}
				try
				{
					this.sendStreamUncompressed.Position = 0L;
					this.sendStreamUncompressed.SetLength(0L);
					netPackage.write(this.sendStreamWriter);
					this.stats.RegisterSentPackage(netPackage.PackageId, (int)this.sendStreamUncompressed.Length);
					this.sendStreamUncompressed.Position = 0L;
					MemoryStream memoryStream = this.sendStreamUncompressed;
					bool flag2 = this.allowCompression && netPackage.Compress;
					if (this.Compress(flag2, this.sendStreamUncompressed, this.sendZipStream, this.sendStreamCompressed, this.copyBufferWriter, 1))
					{
						memoryStream = this.sendStreamCompressed;
					}
					bool flag3 = this.Encrypt(memoryStream);
					this.stats.RegisterSentData(1, (int)memoryStream.Length);
					int num = (int)(memoryStream.Length + 3L);
					ArrayListMP<byte> arrayListMP = new ArrayListMP<byte>(MemoryPools.poolByte, num);
					arrayListMP.Items[0] = (flag2 ? 1 : 0);
					arrayListMP.Items[1] = (flag3 ? 1 : 0);
					try
					{
						memoryStream.Read(arrayListMP.Items, 2, (int)memoryStream.Length);
					}
					catch (ArgumentException)
					{
						string[] array = new string[6];
						array[0] = "Buffer size: ";
						array[1] = arrayListMP.Items.Length.ToString();
						array[2] = " - Stream length: ";
						array[3] = memoryStream.Length.ToString();
						array[4] = " - Package: ";
						int num2 = 5;
						NetPackage netPackage2 = netPackage;
						array[num2] = ((netPackage2 != null) ? netPackage2.ToString() : null);
						Log.Out(string.Concat(array));
						throw;
					}
					arrayListMP.Count = num;
					if (this.isServer)
					{
						this.cInfo.network.SendData(this.cInfo, this.channel, arrayListMP, netPackage.ReliableDelivery);
					}
					else
					{
						this.netClient.SendData(this.channel, arrayListMP);
					}
				}
				finally
				{
					netPackage.SendQueueHandled();
				}
			}
		}
		catch (Exception ex)
		{
			if (this.cInfo != null)
			{
				Log.Error(string.Format("Task_CommWriterSteam (cl={0}, ch={1}):", this.cInfo.PlatformId.CombinedString, this.channel));
			}
			else
			{
				Log.Error(string.Format("Task_CommWriterSteam (ch={0}):", this.channel));
			}
			Log.Exception(ex);
			this.Disconnect(true);
		}
	}

	// Token: 0x04002EBE RID: 11966
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Queue<NetPackage> packetsToSend = new Queue<NetPackage>();

	// Token: 0x04002EBF RID: 11967
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ManualResetEvent writerTriggerEvent = new ManualResetEvent(false);

	// Token: 0x04002EC0 RID: 11968
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ThreadManager.ThreadInfo writerThreadInfo;

	// Token: 0x04002EC1 RID: 11969
	[PublicizedFrom(EAccessModifier.Private)]
	public MemoryStream sendStreamUncompressed;

	// Token: 0x04002EC2 RID: 11970
	[PublicizedFrom(EAccessModifier.Private)]
	public MemoryStream sendStreamCompressed;

	// Token: 0x04002EC3 RID: 11971
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledBinaryWriter sendStreamWriter;

	// Token: 0x04002EC4 RID: 11972
	[PublicizedFrom(EAccessModifier.Private)]
	public DeflateOutputStream sendZipStream;

	// Token: 0x04002EC5 RID: 11973
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly byte[] copyBufferWriter;

	// Token: 0x04002EC6 RID: 11974
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly BlockingQueue<ArrayListMP<byte>> bufsToRead = new BlockingQueue<ArrayListMP<byte>>();

	// Token: 0x04002EC7 RID: 11975
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ManualResetEvent readerTriggerEvent = new ManualResetEvent(false);

	// Token: 0x04002EC8 RID: 11976
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ThreadManager.ThreadInfo readerThreadInfo;

	// Token: 0x04002EC9 RID: 11977
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly byte[] copyBufferReader;

	// Token: 0x04002ECA RID: 11978
	[PublicizedFrom(EAccessModifier.Private)]
	public MemoryStream receiveStreamUncompressed;

	// Token: 0x04002ECB RID: 11979
	[PublicizedFrom(EAccessModifier.Private)]
	public MemoryStream receiveStreamCompressed;

	// Token: 0x04002ECC RID: 11980
	[PublicizedFrom(EAccessModifier.Private)]
	public DeflateInputStream receiveZipStream;

	// Token: 0x04002ECD RID: 11981
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly PooledBinaryReader receiveStreamReader = new PooledBinaryReader();
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Noemax.GZip;
using UnityEngine.Networking;

// Token: 0x02000775 RID: 1909
public class NetConnectionSimple : NetConnectionAbs
{
	// Token: 0x0600390C RID: 14604 RVA: 0x00174860 File Offset: 0x00172A60
	public NetConnectionSimple(int _channel, ClientInfo _clientInfo, INetworkClient _netClient, string _uniqueId, int _reservedHeaderBytes = 0, int _maxPacketSize = 0)
		: base(_channel, _clientInfo, _netClient, _uniqueId)
	{
		this.reservedHeaderBytes = _reservedHeaderBytes;
		this.maxPacketSize = _maxPacketSize;
		if (this.maxPacketSize > 0)
		{
			this.maxPayloadPerPacket = this.maxPacketSize - this.reservedHeaderBytes;
		}
		this.readerBuffer = new byte[4096];
		this.writerBuffer = new byte[4096];
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
		this.readerThreadInfo = ThreadManager.StartThread("NCS_Reader_" + this.connectionIdentifier, new ThreadManager.ThreadFunctionDelegate(this.taskDeserialize), null, null, true, false);
		this.writerThreadInfo = ThreadManager.StartThread("NCS_Writer_" + this.connectionIdentifier, new ThreadManager.ThreadFunctionDelegate(this.taskSerialize), null, null, true, false);
	}

	// Token: 0x0600390D RID: 14605 RVA: 0x0017498C File Offset: 0x00172B8C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InitStreams(bool _full)
	{
		if (this.fullConnection)
		{
			return;
		}
		if (_full)
		{
			byte[] array = new byte[2096128];
			this.packageStagingStream = new MemoryStream(array, 0, array.Length, true, true);
			this.packageStagingStream.SetLength(0L);
			this.packageStagingStreamWriter = new PooledBinaryWriter();
			this.packageStagingStreamWriter.SetBaseStream(this.packageStagingStream);
			byte[] array2 = new byte[2097152];
			this.receiveStreamCompressed = new MemoryStream(array2, 0, array2.Length, true, true);
			this.receiveStreamCompressed.SetLength(0L);
			byte[] array3 = new byte[2097152];
			this.receiveStreamUncompressed = new MemoryStream(array3, 0, array3.Length, true, true);
			this.receiveZipStream = new DeflateInputStream(this.receiveStreamCompressed, true);
			byte[] array4 = new byte[2097152];
			this.reliableSendStreamUncompressed = new MemoryStream(array4, 0, array4.Length, true, true);
			this.reliableSendStreamWriter = new PooledBinaryWriter();
			this.reliableSendStreamWriter.SetBaseStream(this.reliableSendStreamUncompressed);
			byte[] array5 = new byte[2097152];
			this.unreliableSendStreamUncompressed = new MemoryStream(array5, 0, array5.Length, true, true);
			this.unreliableSendStreamWriter = new PooledBinaryWriter();
			this.unreliableSendStreamWriter.SetBaseStream(this.unreliableSendStreamUncompressed);
			byte[] array6 = new byte[2097152];
			this.sendStreamCompressed = new MemoryStream(array6, 0, array6.Length, true, true);
			this.sendZipStream = new DeflateOutputStream(this.sendStreamCompressed, 3, true);
			this.writerStream = new MemoryStream(new byte[2097152]);
			this.writerStream.SetLength(0L);
			this.fullConnection = true;
			return;
		}
		byte[] array7 = new byte[32256];
		this.packageStagingStream = new MemoryStream(array7, 0, array7.Length, true, true);
		this.packageStagingStream.SetLength(0L);
		this.packageStagingStreamWriter = new PooledBinaryWriter();
		this.packageStagingStreamWriter.SetBaseStream(this.packageStagingStream);
		byte[] array8 = new byte[32768];
		this.receiveStreamCompressed = new MemoryStream(array8, 0, array8.Length, true, true);
		this.receiveStreamCompressed.SetLength(0L);
		byte[] array9 = new byte[32768];
		this.reliableSendStreamUncompressed = new MemoryStream(array9, 0, array9.Length, true, true);
		this.reliableSendStreamWriter = new PooledBinaryWriter();
		this.reliableSendStreamWriter.SetBaseStream(this.reliableSendStreamUncompressed);
		byte[] array10 = new byte[32768];
		this.unreliableSendStreamUncompressed = new MemoryStream(array10, 0, array10.Length, true, true);
		this.unreliableSendStreamWriter = new PooledBinaryWriter();
		this.unreliableSendStreamWriter.SetBaseStream(this.unreliableSendStreamUncompressed);
		this.writerStream = new MemoryStream(new byte[32768]);
		this.writerStream.SetLength(0L);
	}

	// Token: 0x0600390E RID: 14606 RVA: 0x00174C29 File Offset: 0x00172E29
	public override void Disconnect(bool _kick)
	{
		base.Disconnect(_kick);
		this.readerTriggerEvent.Set();
		this.writerTriggerEvent.Set();
	}

	// Token: 0x0600390F RID: 14607 RVA: 0x00174C4C File Offset: 0x00172E4C
	public override void AddToSendQueue(NetPackage _package)
	{
		_package.RegisterSendQueue();
		object obj = this.writerListLockObj;
		lock (obj)
		{
			this.writerListPending.Add(_package);
		}
	}

	// Token: 0x06003910 RID: 14608 RVA: 0x00174C98 File Offset: 0x00172E98
	public override void FlushSendQueue()
	{
		object obj = this.writerListLockObj;
		lock (obj)
		{
			this.writerTriggerEvent.Set();
		}
	}

	// Token: 0x06003911 RID: 14609 RVA: 0x00174CE0 File Offset: 0x00172EE0
	public override void AppendToReaderStream(byte[] _data, int _dataSize)
	{
		if (this.bDisconnected)
		{
			return;
		}
		Queue<NetConnectionSimple.RecvBuffer> queue = this.receivedBuffers;
		lock (queue)
		{
			this.receivedBuffers.Enqueue(new NetConnectionSimple.RecvBuffer(_data, _dataSize));
		}
		this.readerTriggerEvent.Set();
	}

	// Token: 0x06003912 RID: 14610 RVA: 0x00174D44 File Offset: 0x00172F44
	[PublicizedFrom(EAccessModifier.Private)]
	public void taskDeserialize(ThreadManager.ThreadInfo _threadInfo)
	{
		bool flag = false;
		int num = 0;
		bool flag2 = false;
		bool flag3 = false;
		int num2 = 0;
		int num3 = 0;
		try
		{
			while (!this.bDisconnected && !_threadInfo.TerminationRequested())
			{
				this.readerTriggerEvent.WaitOne(8);
				if (this.bDisconnected)
				{
					break;
				}
				Queue<NetConnectionSimple.RecvBuffer> queue = this.receivedBuffers;
				NetConnectionSimple.RecvBuffer recvBuffer;
				lock (queue)
				{
					if (this.receivedBuffers.Count == 0)
					{
						this.readerTriggerEvent.Reset();
						continue;
					}
					recvBuffer = this.receivedBuffers.Dequeue();
				}
				int num4 = this.reservedHeaderBytes;
				if (!flag)
				{
					num3 = 0;
					num = StreamUtils.ReadInt32(recvBuffer.Data, ref num4);
					this.receiveStreamCompressed.Position = 0L;
					if (num > this.receiveStreamCompressed.Capacity)
					{
						if (this.cInfo != null)
						{
							Log.Error(string.Format("NCSimple_Deserializer (cl={0}, ch={1}) Received message with size {2} > capacity {3}", new object[]
							{
								this.cInfo.InternalId.CombinedString,
								this.channel,
								num,
								this.receiveStreamCompressed.Capacity
							}));
						}
						else
						{
							Log.Error(string.Format("NCSimple_Deserializer (ch={0}) Received message with size {1} > capacity {2}", this.channel, num, this.receiveStreamCompressed.Capacity));
						}
					}
					this.receiveStreamCompressed.SetLength((long)num);
					flag2 = StreamUtils.ReadByte(recvBuffer.Data, ref num4) == 1;
					flag3 = StreamUtils.ReadByte(recvBuffer.Data, ref num4) == 1;
					num2 = (int)StreamUtils.ReadUInt16(recvBuffer.Data, ref num4);
					if (num2 == 0)
					{
						continue;
					}
					flag = true;
				}
				while (num3 < num && num4 < recvBuffer.Size)
				{
					int num5 = recvBuffer.Size - num4;
					this.receiveStreamCompressed.Write(recvBuffer.Data, num4, num5);
					num3 += num5;
					num4 += num5;
				}
				MemoryPools.poolByte.Free(recvBuffer.Data);
				if (num3 >= num)
				{
					this.receiveStreamCompressed.Position = 0L;
					flag = false;
					num3 = 0;
					this.stats.RegisterReceivedData(num2, num);
					if (this.Decrypt(flag3, this.receiveStreamCompressed))
					{
						this.receiveStreamReader.SetBaseStream(this.Decompress(flag2, this.receiveStreamUncompressed, this.receiveZipStream, this.readerBuffer) ? this.receiveStreamUncompressed : this.receiveStreamCompressed);
						for (int i = 0; i < num2; i++)
						{
							int num6 = this.receiveStreamReader.ReadInt32();
							long position = this.receiveStreamReader.BaseStream.Position;
							NetPackage netPackage = NetPackageManager.ParsePackage(this.receiveStreamReader, this.cInfo);
							int num7 = (int)(this.receiveStreamReader.BaseStream.Position - position);
							if (num7 != num6)
							{
								string[] array = new string[6];
								array[0] = "Parsed data size (";
								array[1] = num7.ToString();
								array[2] = ") does not match expected size (";
								array[3] = num6.ToString();
								array[4] = ") in ";
								int num8 = 5;
								NetPackage netPackage2 = netPackage;
								array[num8] = ((netPackage2 != null) ? netPackage2.ToString() : null);
								throw new InvalidDataException(string.Concat(array));
							}
							List<NetPackage> receivedPackages = this.receivedPackages;
							lock (receivedPackages)
							{
								this.receivedPackages.Add(netPackage);
							}
							int packageId = netPackage.PackageId;
							this.stats.RegisterReceivedPackage(packageId, num7);
							NetPackageLogger.LogPackage(false, this.cInfo, netPackage, this.channel, num7, flag3, flag2, i + 1, num2);
							if (ConnectionManager.VerboseNetLogging)
							{
								if (this.cInfo != null)
								{
									Log.Out("NCSimple deserialized (cl={3}, ch={0}): {1}, size={2}", new object[]
									{
										this.channel,
										NetPackageManager.GetPackageName(packageId),
										num7,
										this.cInfo.InternalId.CombinedString
									});
								}
								else
								{
									Log.Out("NCSimple deserialized (ch={0}): {1}, size={2}", new object[]
									{
										this.channel,
										NetPackageManager.GetPackageName(packageId),
										num7
									});
								}
							}
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			if (this.cInfo != null)
			{
				Log.Error(string.Format("NCSimple_Deserializer (cl={0}, ch={1}) Message: {2}", this.cInfo.InternalId.CombinedString, this.channel, ex.Message));
			}
			else
			{
				Log.Error(string.Format("NCSimple_Deserializer (ch={0}) Message: {1}", this.channel, ex.Message));
			}
			Log.Exception(ex);
			if (this.isServer)
			{
				this.Disconnect(true);
			}
			else
			{
				GameUtils.ForceDisconnect();
			}
		}
	}

	// Token: 0x06003913 RID: 14611 RVA: 0x00175224 File Offset: 0x00173424
	[PublicizedFrom(EAccessModifier.Private)]
	public void taskSerialize(ThreadManager.ThreadInfo _threadInfo)
	{
		bool flag = false;
		MicroStopwatch microStopwatch = new MicroStopwatch();
		try
		{
			while (!this.bDisconnected && !_threadInfo.TerminationRequested())
			{
				if ((!flag || microStopwatch.ElapsedMilliseconds > 500L) && (this.reliableBufsToSend.Count > 0 || this.unreliableBufsToSend.Count > 0))
				{
					try
					{
						NetworkError networkError = this.SendBuffers();
						flag = networkError > NetworkError.Ok;
						switch (networkError)
						{
						case NetworkError.Ok:
							goto IL_015C;
						case NetworkError.WrongHost:
						case NetworkError.WrongChannel:
							goto IL_00F4;
						case NetworkError.WrongConnection:
							break;
						case NetworkError.NoResources:
							if (this.isServer)
							{
								Log.Warning("NET No resources to send data to client ({0}) on channel {2}, backing off for {1} ms", new object[]
								{
									this.cInfo.ToString(),
									500,
									this.channel
								});
							}
							else
							{
								Log.Warning("NET No resources to send data to server on channel {1}, backing off for {0} ms", new object[] { 500, this.channel });
							}
							microStopwatch.ResetAndRestart();
							goto IL_015C;
						default:
							if (networkError != NetworkError.WrongOperation)
							{
								goto IL_00F4;
							}
							break;
						}
						this.Disconnect(true);
						goto IL_015C;
						IL_00F4:
						if (this.isServer)
						{
							Log.Warning("NET Unexpected result '{2}' trying to send data to client ({0}) on channel {1}", new object[]
							{
								this.cInfo.ToString(),
								this.channel,
								networkError.ToStringCached<NetworkError>()
							});
						}
						else
						{
							Log.Warning("NET Unexpected result '{1}' trying to send data to server on channel {0}", new object[]
							{
								this.channel,
								networkError.ToStringCached<NetworkError>()
							});
						}
						IL_015C:;
					}
					catch (Exception ex)
					{
						Log.Exception(ex);
						this.Disconnect(true);
					}
				}
				this.writerTriggerEvent.WaitOne(4);
				if (this.bDisconnected)
				{
					break;
				}
				object obj = this.writerListLockObj;
				int num;
				lock (obj)
				{
					num = this.writerListPending.Count;
				}
				if (num == 0)
				{
					this.writerTriggerEvent.Reset();
				}
				else
				{
					this.reliableSendStreamUncompressed.Position = 0L;
					this.reliableSendStreamUncompressed.SetLength(0L);
					this.unreliableSendStreamUncompressed.Position = 0L;
					this.unreliableSendStreamUncompressed.SetLength(0L);
					int num2 = 0;
					int num3 = 0;
					bool flag3 = false;
					for (int i = 0; i < num; i++)
					{
						obj = this.writerListLockObj;
						NetPackage netPackage;
						lock (obj)
						{
							netPackage = this.writerListPending[i];
						}
						bool flag4 = netPackage.ReliableDelivery || !GameManager.unreliableNetPackets;
						if (!flag4 || !flag3)
						{
							int num4;
							int num5;
							try
							{
								num4 = this.WriteToPackageStagingStream(netPackage);
								num5 = (int)this.packageStagingStream.Length;
							}
							catch (Exception ex2)
							{
								string text = ((this.cInfo != null) ? string.Format("(cl={0}, ch={1})", this.cInfo.InternalId.CombinedString, this.channel) : string.Format("(ch={0})", this.channel));
								Log.Error(string.Concat(new string[]
								{
									"NCSimple ",
									text,
									" dropping package ",
									netPackage.GetType().Name,
									" serialization failed, or it exceeds the send buffer and must be chunked smaller."
								}));
								Log.Exception(ex2);
								netPackage.SendQueueHandled();
								obj = this.writerListLockObj;
								lock (obj)
								{
									this.writerListPending.RemoveAt(i);
								}
								num--;
								i--;
								goto IL_0476;
							}
							MemoryStream memoryStream;
							if (flag4)
							{
								memoryStream = this.reliableSendStreamUncompressed;
							}
							else
							{
								memoryStream = this.unreliableSendStreamUncompressed;
								if (this.cInfo != null)
								{
									int maximumPacketSize = this.cInfo.network.GetMaximumPacketSize(this.cInfo, false);
									if (maximumPacketSize > 0)
									{
										int num6 = maximumPacketSize - 8 - this.reservedHeaderBytes;
										if ((int)this.unreliableSendStreamUncompressed.Length + num5 > num6)
										{
											flag4 = true;
											memoryStream = this.reliableSendStreamUncompressed;
										}
									}
								}
							}
							if (memoryStream.Capacity - (int)memoryStream.Length < num5)
							{
								flag3 = flag3 || flag4;
							}
							else
							{
								memoryStream.Write(this.packageStagingStream.GetBuffer(), 0, num5);
								if (flag4)
								{
									num2++;
									NetPackageLogger.LogPackage(true, this.cInfo, netPackage, this.channel, num4, this.EnableEncryptData(), false, num2, -1);
								}
								else
								{
									num3++;
									NetPackageLogger.LogPackage(true, this.cInfo, netPackage, this.channel, num4, this.EnableEncryptData(), false, num3, -1);
								}
								netPackage.SendQueueHandled();
								obj = this.writerListLockObj;
								lock (obj)
								{
									this.writerListPending.RemoveAt(i);
								}
								num--;
								i--;
							}
						}
						IL_0476:;
					}
					if (this.reliableSendStreamUncompressed.Length > 0L)
					{
						this.StreamToBuffer(ref this.reliableSendStreamUncompressed, true, ref num2, microStopwatch.ElapsedMilliseconds);
					}
					if (this.unreliableSendStreamUncompressed.Length > 0L)
					{
						this.StreamToBuffer(ref this.unreliableSendStreamUncompressed, false, ref num3, microStopwatch.ElapsedMilliseconds);
					}
				}
			}
		}
		catch (Exception ex3)
		{
			if (this.cInfo != null)
			{
				Log.Error(string.Format("NCSimple_Serializer (cl={0}, ch={1}) Message: {2}", this.cInfo.InternalId.CombinedString, this.channel, ex3.Message));
			}
			else
			{
				Log.Error(string.Format("NCSimple_Serializer (ch={0}) Message: {1}", this.channel, ex3.Message));
			}
			Log.Exception(ex3);
			if (this.isServer)
			{
				this.Disconnect(true);
			}
			else
			{
				GameUtils.ForceDisconnect();
			}
		}
	}

	// Token: 0x06003914 RID: 14612 RVA: 0x00175840 File Offset: 0x00173A40
	[PublicizedFrom(EAccessModifier.Private)]
	public int WriteToPackageStagingStream(NetPackage package)
	{
		this.packageStagingStream.SetLength(0L);
		this.packageStagingStreamWriter.Write(-1);
		package.write(this.packageStagingStreamWriter);
		int num = (int)(this.packageStagingStreamWriter.BaseStream.Position - 4L);
		this.packageStagingStream.Position = 0L;
		this.packageStagingStreamWriter.Write(num);
		this.packageStagingStream.Position = this.packageStagingStream.Length;
		int packageId = package.PackageId;
		this.stats.RegisterSentPackage(packageId, num);
		if (ConnectionManager.VerboseNetLogging)
		{
			if (this.cInfo != null)
			{
				Log.Out("NCSimple serialized (cl={3}, ch={0}): {1}, size={2}", new object[]
				{
					this.channel,
					NetPackageManager.GetPackageName(packageId),
					num,
					this.cInfo.InternalId.CombinedString
				});
			}
			else
			{
				Log.Out("NCSimple serialized (ch={0}): {1}, size={2}", new object[]
				{
					this.channel,
					NetPackageManager.GetPackageName(packageId),
					num
				});
			}
		}
		return num;
	}

	// Token: 0x06003915 RID: 14613 RVA: 0x00175950 File Offset: 0x00173B50
	[PublicizedFrom(EAccessModifier.Private)]
	public void StreamToBuffer(ref MemoryStream uncompressedStream, bool sendReliable, ref int packagesToSend, long milliseconds)
	{
		if (uncompressedStream.Length == 0L)
		{
			return;
		}
		uncompressedStream.Position = 0L;
		long length = uncompressedStream.Length;
		bool flag = this.allowCompression && uncompressedStream.Length > 500L;
		MemoryStream memoryStream = uncompressedStream;
		if (this.Compress(flag, uncompressedStream, this.sendZipStream, this.sendStreamCompressed, this.writerBuffer, packagesToSend))
		{
			memoryStream = this.sendStreamCompressed;
			long length2 = this.sendStreamCompressed.Length;
		}
		bool flag2 = this.Encrypt(memoryStream);
		this.stats.RegisterSentData(packagesToSend, (int)memoryStream.Length);
		StreamUtils.Write(this.writerStream, (int)memoryStream.Length);
		StreamUtils.Write(this.writerStream, flag ? 1 : 0);
		StreamUtils.Write(this.writerStream, flag2 ? 1 : 0);
		StreamUtils.Write(this.writerStream, (ushort)packagesToSend);
		if (memoryStream.Length > (long)this.writerStream.Capacity - this.writerStream.Length)
		{
			Log.Error(string.Format("Source stream size ({0}) exceeds remaining writer stream capacity ({1}), packages: {2}, compressed: {3}", new object[]
			{
				memoryStream.Length,
				(long)this.writerStream.Capacity - this.writerStream.Length,
				packagesToSend,
				flag
			}));
		}
		StreamUtils.StreamCopy(memoryStream, this.writerStream, this.writerBuffer, true);
		this.writerStream.Position = 0L;
		ArrayListMP<byte> arrayListMP = new ArrayListMP<byte>(MemoryPools.poolByte, (int)this.writerStream.Length + this.reservedHeaderBytes);
		this.writerStream.Read(arrayListMP.Items, this.reservedHeaderBytes, (int)this.writerStream.Length);
		arrayListMP.Count = (int)(this.writerStream.Length + (long)this.reservedHeaderBytes);
		if (sendReliable)
		{
			this.reliableBufsToSend.AddLast(arrayListMP);
		}
		else
		{
			this.unreliableBufsToSend.AddLast(arrayListMP);
		}
		this.writerStream.SetLength(0L);
	}

	// Token: 0x06003916 RID: 14614 RVA: 0x00175B48 File Offset: 0x00173D48
	[PublicizedFrom(EAccessModifier.Private)]
	public NetworkError SendBuffers()
	{
		NetworkError networkError;
		if ((networkError = this.sendBuffersFromQueue(this.reliableBufsToSend, true)) != NetworkError.Ok)
		{
			return networkError;
		}
		if ((networkError = this.sendBuffersFromQueue(this.unreliableBufsToSend, false)) != NetworkError.Ok)
		{
			return networkError;
		}
		return NetworkError.Ok;
	}

	// Token: 0x06003917 RID: 14615 RVA: 0x00175B7C File Offset: 0x00173D7C
	[PublicizedFrom(EAccessModifier.Private)]
	public NetworkError sendBuffersFromQueue(LinkedList<ArrayListMP<byte>> _sendQueue, bool _reliableDelivery)
	{
		while (_sendQueue.Count > 0)
		{
			ArrayListMP<byte> arrayListMP = _sendQueue.First.Value;
			_sendQueue.RemoveFirst();
			NetworkError networkError = NetworkError.Ok;
			if (!this.bDisconnected)
			{
				if (this.maxPacketSize > 0 && arrayListMP.Count > this.maxPacketSize)
				{
					arrayListMP = this.splitSendBuffer(arrayListMP, _sendQueue);
				}
				if (this.isServer)
				{
					networkError = this.cInfo.network.SendData(this.cInfo, this.channel, arrayListMP, _reliableDelivery);
				}
				else
				{
					networkError = this.netClient.SendData(this.channel, arrayListMP);
				}
			}
			if (networkError != NetworkError.Ok)
			{
				_sendQueue.AddFirst(arrayListMP);
				return networkError;
			}
		}
		return NetworkError.Ok;
	}

	// Token: 0x06003918 RID: 14616 RVA: 0x00175C24 File Offset: 0x00173E24
	[PublicizedFrom(EAccessModifier.Private)]
	public ArrayListMP<byte> splitSendBuffer(ArrayListMP<byte> _inBuf, LinkedList<ArrayListMP<byte>> _sendQueue)
	{
		int num = _inBuf.Count - this.reservedHeaderBytes;
		int num2 = num / this.maxPayloadPerPacket;
		if (num2 * this.maxPayloadPerPacket < num)
		{
			num2++;
		}
		for (int i = num2 - 1; i >= 0; i--)
		{
			int num3 = i * this.maxPayloadPerPacket;
			int num4;
			if (i == num2 - 1)
			{
				num4 = num - num3;
			}
			else
			{
				num4 = this.maxPayloadPerPacket;
			}
			ArrayListMP<byte> arrayListMP = new ArrayListMP<byte>(MemoryPools.poolByte, num4 + this.reservedHeaderBytes);
			Array.Copy(_inBuf.Items, num3 + this.reservedHeaderBytes, arrayListMP.Items, 1, num4);
			arrayListMP.Count = num4 + this.reservedHeaderBytes;
			if (i <= 0)
			{
				return arrayListMP;
			}
			_sendQueue.AddFirst(arrayListMP);
		}
		return null;
	}

	// Token: 0x04002E90 RID: 11920
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int reservedHeaderBytes;

	// Token: 0x04002E91 RID: 11921
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int maxPacketSize;

	// Token: 0x04002E92 RID: 11922
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int maxPayloadPerPacket;

	// Token: 0x04002E93 RID: 11923
	[PublicizedFrom(EAccessModifier.Private)]
	public const int sendFrameHeaderBytes = 8;

	// Token: 0x04002E94 RID: 11924
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object writerListLockObj = new object();

	// Token: 0x04002E95 RID: 11925
	[PublicizedFrom(EAccessModifier.Private)]
	public List<NetPackage> writerListPending = new List<NetPackage>();

	// Token: 0x04002E96 RID: 11926
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ManualResetEvent writerTriggerEvent = new ManualResetEvent(false);

	// Token: 0x04002E97 RID: 11927
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly byte[] writerBuffer;

	// Token: 0x04002E98 RID: 11928
	[PublicizedFrom(EAccessModifier.Private)]
	public ThreadManager.ThreadInfo writerThreadInfo;

	// Token: 0x04002E99 RID: 11929
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledBinaryWriter reliableSendStreamWriter;

	// Token: 0x04002E9A RID: 11930
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledBinaryWriter unreliableSendStreamWriter;

	// Token: 0x04002E9B RID: 11931
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledBinaryWriter packageStagingStreamWriter;

	// Token: 0x04002E9C RID: 11932
	[PublicizedFrom(EAccessModifier.Private)]
	public MemoryStream packageStagingStream;

	// Token: 0x04002E9D RID: 11933
	[PublicizedFrom(EAccessModifier.Private)]
	public MemoryStream reliableSendStreamUncompressed;

	// Token: 0x04002E9E RID: 11934
	[PublicizedFrom(EAccessModifier.Private)]
	public MemoryStream unreliableSendStreamUncompressed;

	// Token: 0x04002E9F RID: 11935
	[PublicizedFrom(EAccessModifier.Private)]
	public MemoryStream sendStreamCompressed;

	// Token: 0x04002EA0 RID: 11936
	[PublicizedFrom(EAccessModifier.Private)]
	public DeflateOutputStream sendZipStream;

	// Token: 0x04002EA1 RID: 11937
	[PublicizedFrom(EAccessModifier.Private)]
	public MemoryStream writerStream;

	// Token: 0x04002EA2 RID: 11938
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly LinkedList<ArrayListMP<byte>> reliableBufsToSend = new LinkedList<ArrayListMP<byte>>();

	// Token: 0x04002EA3 RID: 11939
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly LinkedList<ArrayListMP<byte>> unreliableBufsToSend = new LinkedList<ArrayListMP<byte>>();

	// Token: 0x04002EA4 RID: 11940
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ManualResetEvent readerTriggerEvent = new ManualResetEvent(false);

	// Token: 0x04002EA5 RID: 11941
	[PublicizedFrom(EAccessModifier.Private)]
	public ThreadManager.ThreadInfo readerThreadInfo;

	// Token: 0x04002EA6 RID: 11942
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly byte[] readerBuffer;

	// Token: 0x04002EA7 RID: 11943
	[PublicizedFrom(EAccessModifier.Private)]
	public MemoryStream receiveStreamCompressed;

	// Token: 0x04002EA8 RID: 11944
	[PublicizedFrom(EAccessModifier.Private)]
	public MemoryStream receiveStreamUncompressed;

	// Token: 0x04002EA9 RID: 11945
	[PublicizedFrom(EAccessModifier.Private)]
	public DeflateInputStream receiveZipStream;

	// Token: 0x04002EAA RID: 11946
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly PooledBinaryReader receiveStreamReader = new PooledBinaryReader();

	// Token: 0x04002EAB RID: 11947
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Queue<NetConnectionSimple.RecvBuffer> receivedBuffers = new Queue<NetConnectionSimple.RecvBuffer>();

	// Token: 0x04002EAC RID: 11948
	[PublicizedFrom(EAccessModifier.Private)]
	public const int networkErrorCooldownMs = 500;

	// Token: 0x02000776 RID: 1910
	[PublicizedFrom(EAccessModifier.Private)]
	public struct RecvBuffer
	{
		// Token: 0x06003919 RID: 14617 RVA: 0x00175CD8 File Offset: 0x00173ED8
		public RecvBuffer(byte[] _data, int _size)
		{
			this.Data = _data;
			this.Size = _size;
		}

		// Token: 0x04002EAD RID: 11949
		public readonly byte[] Data;

		// Token: 0x04002EAE RID: 11950
		public readonly int Size;
	}
}

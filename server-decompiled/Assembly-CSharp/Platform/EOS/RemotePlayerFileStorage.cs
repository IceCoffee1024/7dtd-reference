using System;
using System.Collections.Generic;
using Epic.OnlineServices;
using Epic.OnlineServices.PlayerDataStorage;

namespace Platform.EOS
{
	// Token: 0x02001D35 RID: 7477
	public class RemotePlayerFileStorage : IRemotePlayerFileStorage
	{
		// Token: 0x17001C1E RID: 7198
		// (get) Token: 0x0600DDCA RID: 56778 RVA: 0x004F0F30 File Offset: 0x004EF130
		public bool IsReady
		{
			get
			{
				IPlatform platform = this.owner;
				if (platform == null)
				{
					return false;
				}
				IUserClient user = platform.User;
				EUserStatus? euserStatus = ((user != null) ? new EUserStatus?(user.UserStatus) : null);
				EUserStatus euserStatus2 = EUserStatus.LoggedIn;
				return (euserStatus.GetValueOrDefault() == euserStatus2) & (euserStatus != null);
			}
		}

		// Token: 0x0600DDCB RID: 56779 RVA: 0x004F0F7C File Offset: 0x004EF17C
		public void Init(IPlatform _owner)
		{
			this.owner = _owner;
			this.owner.Api.ClientApiInitialized += this.OnClientApiInitialized;
		}

		// Token: 0x0600DDCC RID: 56780 RVA: 0x004F0FA1 File Offset: 0x004EF1A1
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnClientApiInitialized()
		{
			this.playerDataStorage = ((Api)this.owner.Api).PlatformInterface.GetPlayerDataStorageInterface();
		}

		// Token: 0x0600DDCD RID: 56781 RVA: 0x004F0FC4 File Offset: 0x004EF1C4
		[PublicizedFrom(EAccessModifier.Private)]
		public void ProcessNextOperation()
		{
			object obj = this.queueLock;
			lock (obj)
			{
				if (!this.queueProcessing)
				{
					RemotePlayerFileStorage.StorageOperation storageOperation;
					if (this.operationQueue.TryDequeue(out storageOperation))
					{
						this.queueProcessing = true;
						if (storageOperation.ToRead)
						{
							this.ProcessReadOperation(storageOperation.Filename, storageOperation.OverwriteCache, storageOperation.ReadCallback);
						}
						else
						{
							this.ProcessWriteOperation(storageOperation.Filename, storageOperation.Data, storageOperation.OverwriteCache, storageOperation.WriteCallback);
						}
					}
				}
			}
		}

		// Token: 0x0600DDCE RID: 56782 RVA: 0x004F1060 File Offset: 0x004EF260
		[PublicizedFrom(EAccessModifier.Private)]
		public void CompleteActiveOperation()
		{
			object obj = this.queueLock;
			lock (obj)
			{
				this.queueProcessing = false;
				this.ProcessNextOperation();
			}
		}

		// Token: 0x0600DDCF RID: 56783 RVA: 0x004F10A8 File Offset: 0x004EF2A8
		public void ReadRemoteData(string _filename, bool _overwriteCache, IRemotePlayerFileStorage.FileReadCompleteCallback _callback)
		{
			object obj = this.queueLock;
			lock (obj)
			{
				this.operationQueue.Enqueue(new RemotePlayerFileStorage.StorageOperation(_filename, _overwriteCache, _callback));
				this.ProcessNextOperation();
			}
		}

		// Token: 0x0600DDD0 RID: 56784 RVA: 0x004F10FC File Offset: 0x004EF2FC
		[PublicizedFrom(EAccessModifier.Private)]
		public void ProcessReadOperation(string _filename, bool _overwriteCache, IRemotePlayerFileStorage.FileReadCompleteCallback _callback)
		{
			if (_callback == null)
			{
				Log.Warning("[EOS] PlayerDataStorage Read Operation failed as no callback supplied.");
				this.CompleteActiveOperation();
				return;
			}
			if (!this.IsReady)
			{
				Log.Warning("[EOS] Tried to read from PlayerDataStorage user is not logged in.");
				_callback(IRemotePlayerFileStorage.CallbackResult.NoConnection, null);
				this.CompleteActiveOperation();
				return;
			}
			if (this.playerDataStorage == null)
			{
				Log.Warning("[EOS] Tried to read from PlayerDataStorage but it was null.");
				_callback(IRemotePlayerFileStorage.CallbackResult.NoConnection, null);
				this.CompleteActiveOperation();
				return;
			}
			if (string.IsNullOrEmpty(_filename))
			{
				Log.Warning("[EOS] Supplied filename was null or empty.");
				_callback(IRemotePlayerFileStorage.CallbackResult.FileNotFound, null);
				this.CompleteActiveOperation();
				return;
			}
			ProductUserId productUserId = ((UserIdentifierEos)PlatformManager.CrossplatformPlatform.User.PlatformUserId).ProductUserId;
			RemotePlayerFileStorage.ReadRequestDetails readRequestDetails = new RemotePlayerFileStorage.ReadRequestDetails(_callback, _overwriteCache);
			ReadFileOptions readFileOptions = new ReadFileOptions
			{
				Filename = _filename,
				LocalUserId = productUserId,
				ReadChunkLengthBytes = 524288U,
				ReadFileDataCallback = new OnReadFileDataCallback(this.ReadChunkCallback)
			};
			object lockObject = AntiCheatCommon.LockObject;
			lock (lockObject)
			{
				this.playerDataStorage.ReadFile(ref readFileOptions, readRequestDetails, new OnReadFileCompleteCallback(this.ReadFileCompleteCallback));
			}
		}

		// Token: 0x0600DDD1 RID: 56785 RVA: 0x004F1234 File Offset: 0x004EF434
		[PublicizedFrom(EAccessModifier.Private)]
		public ReadResult ReadChunkCallback(ref ReadFileDataCallbackInfo _callbackData)
		{
			((RemotePlayerFileStorage.ReadRequestDetails)_callbackData.ClientData).Chunks.Add(_callbackData.DataChunk);
			return ReadResult.ContinueReading;
		}

		// Token: 0x0600DDD2 RID: 56786 RVA: 0x004F1254 File Offset: 0x004EF454
		[PublicizedFrom(EAccessModifier.Private)]
		public void ReadFileCompleteCallback(ref ReadFileCallbackInfo _callbackData)
		{
			try
			{
				RemotePlayerFileStorage.ReadRequestDetails readRequestDetails = (RemotePlayerFileStorage.ReadRequestDetails)_callbackData.ClientData;
				if (_callbackData.ResultCode != Result.Success)
				{
					if (_callbackData.ResultCode == Result.NotFound)
					{
						readRequestDetails.Callback(IRemotePlayerFileStorage.CallbackResult.FileNotFound, null);
					}
					else if (_callbackData.ResultCode != Result.OperationWillRetry)
					{
						Log.Warning(string.Format("[EOS] Read from PlayerDataStorage failed ({0}): {1}", _callbackData.Filename, _callbackData.ResultCode.ToStringCached<Result>()));
						readRequestDetails.Callback(IRemotePlayerFileStorage.CallbackResult.Other, null);
					}
				}
				else
				{
					int num = 0;
					foreach (ArraySegment<byte> arraySegment in readRequestDetails.Chunks)
					{
						num += arraySegment.Count;
					}
					byte[] array = new byte[num];
					int num2 = 0;
					for (int i = 0; i < readRequestDetails.Chunks.Count; i++)
					{
						Array.Copy(readRequestDetails.Chunks[i].Array, readRequestDetails.Chunks[i].Offset, array, num2, readRequestDetails.Chunks[i].Count);
						num2 += readRequestDetails.Chunks[i].Count;
					}
					if (readRequestDetails.OverwriteCache)
					{
						IRemotePlayerFileStorage.WriteCachedObject(this.owner.User, _callbackData.Filename, array);
					}
					Log.Out(string.Format("[EOS] Read ({0}) completed: {1}, received {2} bytes", _callbackData.Filename, _callbackData.ResultCode, num));
					readRequestDetails.Callback(IRemotePlayerFileStorage.CallbackResult.Success, array);
				}
			}
			finally
			{
				this.CompleteActiveOperation();
			}
		}

		// Token: 0x0600DDD3 RID: 56787 RVA: 0x004F1424 File Offset: 0x004EF624
		public void WriteRemoteData(string _filename, byte[] _data, bool _overwriteCache, IRemotePlayerFileStorage.FileWriteCompleteCallback _callback)
		{
			object obj = this.queueLock;
			lock (obj)
			{
				this.operationQueue.Enqueue(new RemotePlayerFileStorage.StorageOperation(_filename, _data, _overwriteCache, _callback));
				this.ProcessNextOperation();
			}
		}

		// Token: 0x0600DDD4 RID: 56788 RVA: 0x004F147C File Offset: 0x004EF67C
		public void ProcessWriteOperation(string _filename, byte[] _data, bool _overwriteCache, IRemotePlayerFileStorage.FileWriteCompleteCallback _callback)
		{
			if (_callback == null)
			{
				Log.Warning("[EOS] PlayerDataStorage Write Operation failed as no callback supplied.");
				this.CompleteActiveOperation();
				return;
			}
			if (!this.IsReady)
			{
				Log.Warning("[EOS] Tried to write to PlayerDataStorage user is not logged in.");
				this.CompleteActiveOperation();
				_callback(IRemotePlayerFileStorage.CallbackResult.NoConnection);
				return;
			}
			if (this.playerDataStorage == null)
			{
				Log.Warning("[EOS] Tried to write to PlayerDataStorage but it was null.");
				this.CompleteActiveOperation();
				_callback(IRemotePlayerFileStorage.CallbackResult.NoConnection);
				return;
			}
			if (string.IsNullOrEmpty(_filename))
			{
				Log.Warning("[EOS] Supplied filename was null or empty.");
				this.CompleteActiveOperation();
				_callback(IRemotePlayerFileStorage.CallbackResult.FileNotFound);
				return;
			}
			if (_data == null)
			{
				Log.Warning("[EOS] Supplied data to store was null.");
				this.CompleteActiveOperation();
				_callback(IRemotePlayerFileStorage.CallbackResult.MalformedData);
				return;
			}
			RemotePlayerFileStorage.WriteRequestDetails writeRequestDetails = new RemotePlayerFileStorage.WriteRequestDetails(_overwriteCache, _data, _callback);
			ProductUserId productUserId = ((UserIdentifierEos)PlatformManager.CrossplatformPlatform.User.PlatformUserId).ProductUserId;
			WriteFileOptions writeFileOptions = new WriteFileOptions
			{
				Filename = _filename,
				LocalUserId = productUserId,
				ChunkLengthBytes = 524288U,
				WriteFileDataCallback = new OnWriteFileDataCallback(this.WriteFileChunkCallback)
			};
			object lockObject = AntiCheatCommon.LockObject;
			lock (lockObject)
			{
				this.playerDataStorage.WriteFile(ref writeFileOptions, writeRequestDetails, new OnWriteFileCompleteCallback(this.WriteFileCompleteCallback));
			}
		}

		// Token: 0x0600DDD5 RID: 56789 RVA: 0x004F15D4 File Offset: 0x004EF7D4
		[PublicizedFrom(EAccessModifier.Private)]
		public WriteResult WriteFileChunkCallback(ref WriteFileDataCallbackInfo _callbackData, out ArraySegment<byte> _outDataBuffer)
		{
			RemotePlayerFileStorage.WriteRequestDetails writeRequestDetails = (RemotePlayerFileStorage.WriteRequestDetails)_callbackData.ClientData;
			_outDataBuffer = writeRequestDetails.GetNextChunk() ?? null;
			if (writeRequestDetails.HasNextChunk())
			{
				return WriteResult.ContinueWriting;
			}
			return WriteResult.CompleteRequest;
		}

		// Token: 0x0600DDD6 RID: 56790 RVA: 0x004F1620 File Offset: 0x004EF820
		[PublicizedFrom(EAccessModifier.Private)]
		public void WriteFileCompleteCallback(ref WriteFileCallbackInfo _callbackData)
		{
			try
			{
				RemotePlayerFileStorage.WriteRequestDetails writeRequestDetails = (RemotePlayerFileStorage.WriteRequestDetails)_callbackData.ClientData;
				IRemotePlayerFileStorage.CallbackResult callbackResult = IRemotePlayerFileStorage.CallbackResult.Success;
				if (_callbackData.ResultCode != Result.Success && _callbackData.ResultCode != Result.OperationWillRetry)
				{
					Log.Warning(string.Format("[EOS] Write to PlayerDataStorage failed ({0}): {1}", _callbackData.Filename, _callbackData.ResultCode.ToStringCached<Result>()));
					callbackResult = IRemotePlayerFileStorage.CallbackResult.Other;
				}
				if (writeRequestDetails.WriteToCache && !IRemotePlayerFileStorage.WriteCachedObject(this.owner.User, _callbackData.Filename, writeRequestDetails.Data))
				{
					Log.Warning(string.Format("[EOS] Write to PlayerDataStorage succeeded ({0}), but failed while saving to local cache.", _callbackData.Filename));
				}
				IRemotePlayerFileStorage.FileWriteCompleteCallback callback = writeRequestDetails.Callback;
				if (callback != null)
				{
					callback(callbackResult);
				}
			}
			finally
			{
				this.CompleteActiveOperation();
			}
		}

		// Token: 0x0400A754 RID: 42836
		[PublicizedFrom(EAccessModifier.Private)]
		public const int cReadWriteByteLimit = 524288;

		// Token: 0x0400A755 RID: 42837
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A756 RID: 42838
		[PublicizedFrom(EAccessModifier.Private)]
		public PlayerDataStorageInterface playerDataStorage;

		// Token: 0x0400A757 RID: 42839
		[PublicizedFrom(EAccessModifier.Private)]
		public Queue<RemotePlayerFileStorage.StorageOperation> operationQueue = new Queue<RemotePlayerFileStorage.StorageOperation>();

		// Token: 0x0400A758 RID: 42840
		[PublicizedFrom(EAccessModifier.Private)]
		public object queueLock = new object();

		// Token: 0x0400A759 RID: 42841
		[PublicizedFrom(EAccessModifier.Private)]
		public bool queueProcessing;

		// Token: 0x02001D36 RID: 7478
		[PublicizedFrom(EAccessModifier.Private)]
		public struct StorageOperation
		{
			// Token: 0x0600DDD8 RID: 56792 RVA: 0x004F16FA File Offset: 0x004EF8FA
			public StorageOperation(string _filename, bool _overwriteCache, IRemotePlayerFileStorage.FileReadCompleteCallback _callback)
			{
				this.ToRead = true;
				this.Filename = _filename;
				this.OverwriteCache = _overwriteCache;
				this.Data = null;
				this.ReadCallback = _callback;
				this.WriteCallback = null;
			}

			// Token: 0x0600DDD9 RID: 56793 RVA: 0x004F1726 File Offset: 0x004EF926
			public StorageOperation(string _filename, byte[] _data, bool _overwriteCache, IRemotePlayerFileStorage.FileWriteCompleteCallback _callback)
			{
				this.ToRead = false;
				this.Filename = _filename;
				this.Data = _data;
				this.OverwriteCache = _overwriteCache;
				this.ReadCallback = null;
				this.WriteCallback = _callback;
			}

			// Token: 0x0400A75A RID: 42842
			public bool ToRead;

			// Token: 0x0400A75B RID: 42843
			public string Filename;

			// Token: 0x0400A75C RID: 42844
			public bool OverwriteCache;

			// Token: 0x0400A75D RID: 42845
			public byte[] Data;

			// Token: 0x0400A75E RID: 42846
			public IRemotePlayerFileStorage.FileReadCompleteCallback ReadCallback;

			// Token: 0x0400A75F RID: 42847
			public IRemotePlayerFileStorage.FileWriteCompleteCallback WriteCallback;
		}

		// Token: 0x02001D37 RID: 7479
		[PublicizedFrom(EAccessModifier.Private)]
		public class ReadRequestDetails
		{
			// Token: 0x0600DDDA RID: 56794 RVA: 0x004F1753 File Offset: 0x004EF953
			public ReadRequestDetails(IRemotePlayerFileStorage.FileReadCompleteCallback _callback, bool _overwriteCache)
			{
				this.Callback = _callback;
				this.OverwriteCache = _overwriteCache;
				this.Chunks = new List<ArraySegment<byte>>();
			}

			// Token: 0x0400A760 RID: 42848
			public readonly IRemotePlayerFileStorage.FileReadCompleteCallback Callback;

			// Token: 0x0400A761 RID: 42849
			public List<ArraySegment<byte>> Chunks;

			// Token: 0x0400A762 RID: 42850
			public bool OverwriteCache;
		}

		// Token: 0x02001D38 RID: 7480
		[PublicizedFrom(EAccessModifier.Private)]
		public class WriteRequestDetails
		{
			// Token: 0x0600DDDB RID: 56795 RVA: 0x004F1774 File Offset: 0x004EF974
			public WriteRequestDetails(bool _writeToCache, byte[] _data, IRemotePlayerFileStorage.FileWriteCompleteCallback _callback)
			{
				this.WriteToCache = _writeToCache;
				this.Data = _data;
				this.Callback = _callback;
			}

			// Token: 0x0600DDDC RID: 56796 RVA: 0x004F1794 File Offset: 0x004EF994
			public ArraySegment<byte>? GetNextChunk()
			{
				if (this.DataPointer >= this.Data.Length)
				{
					return null;
				}
				int num = this.Data.Length - this.DataPointer;
				int num2 = Math.Min(524288, num);
				ArraySegment<byte> arraySegment = new ArraySegment<byte>(this.Data, this.DataPointer, num2);
				this.DataPointer += num2;
				return new ArraySegment<byte>?(arraySegment);
			}

			// Token: 0x0600DDDD RID: 56797 RVA: 0x004F17FC File Offset: 0x004EF9FC
			public bool HasNextChunk()
			{
				return this.DataPointer < this.Data.Length;
			}

			// Token: 0x0400A763 RID: 42851
			public bool WriteToCache;

			// Token: 0x0400A764 RID: 42852
			public byte[] Data;

			// Token: 0x0400A765 RID: 42853
			public readonly IRemotePlayerFileStorage.FileWriteCompleteCallback Callback;

			// Token: 0x0400A766 RID: 42854
			[PublicizedFrom(EAccessModifier.Private)]
			public int DataPointer;
		}
	}
}

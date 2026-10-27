using System;
using System.Collections.Generic;
using Epic.OnlineServices;
using Epic.OnlineServices.PlayerDataStorage;

namespace Platform.EOS
{
	// Token: 0x02001D66 RID: 7526
	public class RemotePlayerFileStorage : IRemotePlayerFileStorage
	{
		// Token: 0x17001C66 RID: 7270
		// (get) Token: 0x0600DEDE RID: 57054 RVA: 0x004F4AD0 File Offset: 0x004F2CD0
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

		// Token: 0x0600DEDF RID: 57055 RVA: 0x004F4B1C File Offset: 0x004F2D1C
		public void Init(IPlatform _owner)
		{
			this.owner = _owner;
			this.owner.Api.ClientApiInitialized += this.OnClientApiInitialized;
		}

		// Token: 0x0600DEE0 RID: 57056 RVA: 0x004F4B41 File Offset: 0x004F2D41
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnClientApiInitialized()
		{
			this.playerDataStorage = ((Api)this.owner.Api).PlatformInterface.GetPlayerDataStorageInterface();
		}

		// Token: 0x0600DEE1 RID: 57057 RVA: 0x004F4B64 File Offset: 0x004F2D64
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

		// Token: 0x0600DEE2 RID: 57058 RVA: 0x004F4C00 File Offset: 0x004F2E00
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

		// Token: 0x0600DEE3 RID: 57059 RVA: 0x004F4C48 File Offset: 0x004F2E48
		public void ReadRemoteData(string _filename, bool _overwriteCache, IRemotePlayerFileStorage.FileReadCompleteCallback _callback)
		{
			object obj = this.queueLock;
			lock (obj)
			{
				this.operationQueue.Enqueue(new RemotePlayerFileStorage.StorageOperation(_filename, _overwriteCache, _callback));
				this.ProcessNextOperation();
			}
		}

		// Token: 0x0600DEE4 RID: 57060 RVA: 0x004F4C9C File Offset: 0x004F2E9C
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

		// Token: 0x0600DEE5 RID: 57061 RVA: 0x004F4DD4 File Offset: 0x004F2FD4
		[PublicizedFrom(EAccessModifier.Private)]
		public ReadResult ReadChunkCallback(ref ReadFileDataCallbackInfo _callbackData)
		{
			((RemotePlayerFileStorage.ReadRequestDetails)_callbackData.ClientData).Chunks.Add(_callbackData.DataChunk);
			return ReadResult.ContinueReading;
		}

		// Token: 0x0600DEE6 RID: 57062 RVA: 0x004F4DF4 File Offset: 0x004F2FF4
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

		// Token: 0x0600DEE7 RID: 57063 RVA: 0x004F4FC4 File Offset: 0x004F31C4
		public void WriteRemoteData(string _filename, byte[] _data, bool _overwriteCache, IRemotePlayerFileStorage.FileWriteCompleteCallback _callback)
		{
			object obj = this.queueLock;
			lock (obj)
			{
				this.operationQueue.Enqueue(new RemotePlayerFileStorage.StorageOperation(_filename, _data, _overwriteCache, _callback));
				this.ProcessNextOperation();
			}
		}

		// Token: 0x0600DEE8 RID: 57064 RVA: 0x004F501C File Offset: 0x004F321C
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

		// Token: 0x0600DEE9 RID: 57065 RVA: 0x004F5174 File Offset: 0x004F3374
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

		// Token: 0x0600DEEA RID: 57066 RVA: 0x004F51C0 File Offset: 0x004F33C0
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

		// Token: 0x0400A83D RID: 43069
		[PublicizedFrom(EAccessModifier.Private)]
		public const int cReadWriteByteLimit = 524288;

		// Token: 0x0400A83E RID: 43070
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A83F RID: 43071
		[PublicizedFrom(EAccessModifier.Private)]
		public PlayerDataStorageInterface playerDataStorage;

		// Token: 0x0400A840 RID: 43072
		[PublicizedFrom(EAccessModifier.Private)]
		public Queue<RemotePlayerFileStorage.StorageOperation> operationQueue = new Queue<RemotePlayerFileStorage.StorageOperation>();

		// Token: 0x0400A841 RID: 43073
		[PublicizedFrom(EAccessModifier.Private)]
		public object queueLock = new object();

		// Token: 0x0400A842 RID: 43074
		[PublicizedFrom(EAccessModifier.Private)]
		public bool queueProcessing;

		// Token: 0x02001D67 RID: 7527
		[PublicizedFrom(EAccessModifier.Private)]
		public struct StorageOperation
		{
			// Token: 0x0600DEEC RID: 57068 RVA: 0x004F529A File Offset: 0x004F349A
			public StorageOperation(string _filename, bool _overwriteCache, IRemotePlayerFileStorage.FileReadCompleteCallback _callback)
			{
				this.ToRead = true;
				this.Filename = _filename;
				this.OverwriteCache = _overwriteCache;
				this.Data = null;
				this.ReadCallback = _callback;
				this.WriteCallback = null;
			}

			// Token: 0x0600DEED RID: 57069 RVA: 0x004F52C6 File Offset: 0x004F34C6
			public StorageOperation(string _filename, byte[] _data, bool _overwriteCache, IRemotePlayerFileStorage.FileWriteCompleteCallback _callback)
			{
				this.ToRead = false;
				this.Filename = _filename;
				this.Data = _data;
				this.OverwriteCache = _overwriteCache;
				this.ReadCallback = null;
				this.WriteCallback = _callback;
			}

			// Token: 0x0400A843 RID: 43075
			public bool ToRead;

			// Token: 0x0400A844 RID: 43076
			public string Filename;

			// Token: 0x0400A845 RID: 43077
			public bool OverwriteCache;

			// Token: 0x0400A846 RID: 43078
			public byte[] Data;

			// Token: 0x0400A847 RID: 43079
			public IRemotePlayerFileStorage.FileReadCompleteCallback ReadCallback;

			// Token: 0x0400A848 RID: 43080
			public IRemotePlayerFileStorage.FileWriteCompleteCallback WriteCallback;
		}

		// Token: 0x02001D68 RID: 7528
		[PublicizedFrom(EAccessModifier.Private)]
		public class ReadRequestDetails
		{
			// Token: 0x0600DEEE RID: 57070 RVA: 0x004F52F3 File Offset: 0x004F34F3
			public ReadRequestDetails(IRemotePlayerFileStorage.FileReadCompleteCallback _callback, bool _overwriteCache)
			{
				this.Callback = _callback;
				this.OverwriteCache = _overwriteCache;
				this.Chunks = new List<ArraySegment<byte>>();
			}

			// Token: 0x0400A849 RID: 43081
			public readonly IRemotePlayerFileStorage.FileReadCompleteCallback Callback;

			// Token: 0x0400A84A RID: 43082
			public List<ArraySegment<byte>> Chunks;

			// Token: 0x0400A84B RID: 43083
			public bool OverwriteCache;
		}

		// Token: 0x02001D69 RID: 7529
		[PublicizedFrom(EAccessModifier.Private)]
		public class WriteRequestDetails
		{
			// Token: 0x0600DEEF RID: 57071 RVA: 0x004F5314 File Offset: 0x004F3514
			public WriteRequestDetails(bool _writeToCache, byte[] _data, IRemotePlayerFileStorage.FileWriteCompleteCallback _callback)
			{
				this.WriteToCache = _writeToCache;
				this.Data = _data;
				this.Callback = _callback;
			}

			// Token: 0x0600DEF0 RID: 57072 RVA: 0x004F5334 File Offset: 0x004F3534
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

			// Token: 0x0600DEF1 RID: 57073 RVA: 0x004F539C File Offset: 0x004F359C
			public bool HasNextChunk()
			{
				return this.DataPointer < this.Data.Length;
			}

			// Token: 0x0400A84C RID: 43084
			public bool WriteToCache;

			// Token: 0x0400A84D RID: 43085
			public byte[] Data;

			// Token: 0x0400A84E RID: 43086
			public readonly IRemotePlayerFileStorage.FileWriteCompleteCallback Callback;

			// Token: 0x0400A84F RID: 43087
			[PublicizedFrom(EAccessModifier.Private)]
			public int DataPointer;
		}
	}
}

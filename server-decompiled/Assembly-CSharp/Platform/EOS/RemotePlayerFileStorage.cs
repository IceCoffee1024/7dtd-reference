using System;
using System.Collections.Generic;
using Epic.OnlineServices;
using Epic.OnlineServices.PlayerDataStorage;

namespace Platform.EOS
{
	// Token: 0x02001D03 RID: 7427
	public class RemotePlayerFileStorage : IRemotePlayerFileStorage
	{
		// Token: 0x17001B78 RID: 7032
		// (get) Token: 0x0600DC4F RID: 56399 RVA: 0x004EC2E8 File Offset: 0x004EA4E8
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

		// Token: 0x0600DC50 RID: 56400 RVA: 0x004EC334 File Offset: 0x004EA534
		public void Init(IPlatform _owner)
		{
			this.owner = _owner;
			this.owner.Api.ClientApiInitialized += this.OnClientApiInitialized;
		}

		// Token: 0x0600DC51 RID: 56401 RVA: 0x004EC359 File Offset: 0x004EA559
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnClientApiInitialized()
		{
			this.playerDataStorage = ((Api)this.owner.Api).PlatformInterface.GetPlayerDataStorageInterface();
		}

		// Token: 0x0600DC52 RID: 56402 RVA: 0x004EC37C File Offset: 0x004EA57C
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

		// Token: 0x0600DC53 RID: 56403 RVA: 0x004EC418 File Offset: 0x004EA618
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

		// Token: 0x0600DC54 RID: 56404 RVA: 0x004EC460 File Offset: 0x004EA660
		public void ReadRemoteData(string _filename, bool _overwriteCache, IRemotePlayerFileStorage.FileReadCompleteCallback _callback)
		{
			object obj = this.queueLock;
			lock (obj)
			{
				this.operationQueue.Enqueue(new RemotePlayerFileStorage.StorageOperation(_filename, _overwriteCache, _callback));
				this.ProcessNextOperation();
			}
		}

		// Token: 0x0600DC55 RID: 56405 RVA: 0x004EC4B4 File Offset: 0x004EA6B4
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

		// Token: 0x0600DC56 RID: 56406 RVA: 0x004EC5EC File Offset: 0x004EA7EC
		[PublicizedFrom(EAccessModifier.Private)]
		public ReadResult ReadChunkCallback(ref ReadFileDataCallbackInfo _callbackData)
		{
			((RemotePlayerFileStorage.ReadRequestDetails)_callbackData.ClientData).Chunks.Add(_callbackData.DataChunk);
			return ReadResult.ContinueReading;
		}

		// Token: 0x0600DC57 RID: 56407 RVA: 0x004EC60C File Offset: 0x004EA80C
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

		// Token: 0x0600DC58 RID: 56408 RVA: 0x004EC7DC File Offset: 0x004EA9DC
		public void WriteRemoteData(string _filename, byte[] _data, bool _overwriteCache, IRemotePlayerFileStorage.FileWriteCompleteCallback _callback)
		{
			object obj = this.queueLock;
			lock (obj)
			{
				this.operationQueue.Enqueue(new RemotePlayerFileStorage.StorageOperation(_filename, _data, _overwriteCache, _callback));
				this.ProcessNextOperation();
			}
		}

		// Token: 0x0600DC59 RID: 56409 RVA: 0x004EC834 File Offset: 0x004EAA34
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

		// Token: 0x0600DC5A RID: 56410 RVA: 0x004EC98C File Offset: 0x004EAB8C
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

		// Token: 0x0600DC5B RID: 56411 RVA: 0x004EC9D8 File Offset: 0x004EABD8
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

		// Token: 0x0400A67D RID: 42621
		[PublicizedFrom(EAccessModifier.Private)]
		public const int cReadWriteByteLimit = 524288;

		// Token: 0x0400A67E RID: 42622
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A67F RID: 42623
		[PublicizedFrom(EAccessModifier.Private)]
		public PlayerDataStorageInterface playerDataStorage;

		// Token: 0x0400A680 RID: 42624
		[PublicizedFrom(EAccessModifier.Private)]
		public Queue<RemotePlayerFileStorage.StorageOperation> operationQueue = new Queue<RemotePlayerFileStorage.StorageOperation>();

		// Token: 0x0400A681 RID: 42625
		[PublicizedFrom(EAccessModifier.Private)]
		public object queueLock = new object();

		// Token: 0x0400A682 RID: 42626
		[PublicizedFrom(EAccessModifier.Private)]
		public bool queueProcessing;

		// Token: 0x02001D04 RID: 7428
		[PublicizedFrom(EAccessModifier.Private)]
		public struct StorageOperation
		{
			// Token: 0x0600DC5D RID: 56413 RVA: 0x004ECAB2 File Offset: 0x004EACB2
			public StorageOperation(string _filename, bool _overwriteCache, IRemotePlayerFileStorage.FileReadCompleteCallback _callback)
			{
				this.ToRead = true;
				this.Filename = _filename;
				this.OverwriteCache = _overwriteCache;
				this.Data = null;
				this.ReadCallback = _callback;
				this.WriteCallback = null;
			}

			// Token: 0x0600DC5E RID: 56414 RVA: 0x004ECADE File Offset: 0x004EACDE
			public StorageOperation(string _filename, byte[] _data, bool _overwriteCache, IRemotePlayerFileStorage.FileWriteCompleteCallback _callback)
			{
				this.ToRead = false;
				this.Filename = _filename;
				this.Data = _data;
				this.OverwriteCache = _overwriteCache;
				this.ReadCallback = null;
				this.WriteCallback = _callback;
			}

			// Token: 0x0400A683 RID: 42627
			public bool ToRead;

			// Token: 0x0400A684 RID: 42628
			public string Filename;

			// Token: 0x0400A685 RID: 42629
			public bool OverwriteCache;

			// Token: 0x0400A686 RID: 42630
			public byte[] Data;

			// Token: 0x0400A687 RID: 42631
			public IRemotePlayerFileStorage.FileReadCompleteCallback ReadCallback;

			// Token: 0x0400A688 RID: 42632
			public IRemotePlayerFileStorage.FileWriteCompleteCallback WriteCallback;
		}

		// Token: 0x02001D05 RID: 7429
		[PublicizedFrom(EAccessModifier.Private)]
		public class ReadRequestDetails
		{
			// Token: 0x0600DC5F RID: 56415 RVA: 0x004ECB0B File Offset: 0x004EAD0B
			public ReadRequestDetails(IRemotePlayerFileStorage.FileReadCompleteCallback _callback, bool _overwriteCache)
			{
				this.Callback = _callback;
				this.OverwriteCache = _overwriteCache;
				this.Chunks = new List<ArraySegment<byte>>();
			}

			// Token: 0x0400A689 RID: 42633
			public readonly IRemotePlayerFileStorage.FileReadCompleteCallback Callback;

			// Token: 0x0400A68A RID: 42634
			public List<ArraySegment<byte>> Chunks;

			// Token: 0x0400A68B RID: 42635
			public bool OverwriteCache;
		}

		// Token: 0x02001D06 RID: 7430
		[PublicizedFrom(EAccessModifier.Private)]
		public class WriteRequestDetails
		{
			// Token: 0x0600DC60 RID: 56416 RVA: 0x004ECB2C File Offset: 0x004EAD2C
			public WriteRequestDetails(bool _writeToCache, byte[] _data, IRemotePlayerFileStorage.FileWriteCompleteCallback _callback)
			{
				this.WriteToCache = _writeToCache;
				this.Data = _data;
				this.Callback = _callback;
			}

			// Token: 0x0600DC61 RID: 56417 RVA: 0x004ECB4C File Offset: 0x004EAD4C
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

			// Token: 0x0600DC62 RID: 56418 RVA: 0x004ECBB4 File Offset: 0x004EADB4
			public bool HasNextChunk()
			{
				return this.DataPointer < this.Data.Length;
			}

			// Token: 0x0400A68C RID: 42636
			public bool WriteToCache;

			// Token: 0x0400A68D RID: 42637
			public byte[] Data;

			// Token: 0x0400A68E RID: 42638
			public readonly IRemotePlayerFileStorage.FileWriteCompleteCallback Callback;

			// Token: 0x0400A68F RID: 42639
			[PublicizedFrom(EAccessModifier.Private)]
			public int DataPointer;
		}
	}
}

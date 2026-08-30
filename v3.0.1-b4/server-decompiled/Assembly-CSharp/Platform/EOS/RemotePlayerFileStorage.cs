using System;
using System.Collections.Generic;
using Epic.OnlineServices;
using Epic.OnlineServices.PlayerDataStorage;

namespace Platform.EOS
{
	// Token: 0x02001CDC RID: 7388
	public class RemotePlayerFileStorage : IRemotePlayerFileStorage
	{
		// Token: 0x17001B06 RID: 6918
		// (get) Token: 0x0600DAB3 RID: 55987 RVA: 0x004E5CA8 File Offset: 0x004E3EA8
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

		// Token: 0x0600DAB4 RID: 55988 RVA: 0x004E5CF4 File Offset: 0x004E3EF4
		public void Init(IPlatform _owner)
		{
			this.owner = _owner;
			this.owner.Api.ClientApiInitialized += this.OnClientApiInitialized;
		}

		// Token: 0x0600DAB5 RID: 55989 RVA: 0x004E5D19 File Offset: 0x004E3F19
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnClientApiInitialized()
		{
			this.playerDataStorage = ((Api)this.owner.Api).PlatformInterface.GetPlayerDataStorageInterface();
		}

		// Token: 0x0600DAB6 RID: 55990 RVA: 0x004E5D3C File Offset: 0x004E3F3C
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

		// Token: 0x0600DAB7 RID: 55991 RVA: 0x004E5DD8 File Offset: 0x004E3FD8
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

		// Token: 0x0600DAB8 RID: 55992 RVA: 0x004E5E20 File Offset: 0x004E4020
		public void ReadRemoteData(string _filename, bool _overwriteCache, IRemotePlayerFileStorage.FileReadCompleteCallback _callback)
		{
			object obj = this.queueLock;
			lock (obj)
			{
				this.operationQueue.Enqueue(new RemotePlayerFileStorage.StorageOperation(_filename, _overwriteCache, _callback));
				this.ProcessNextOperation();
			}
		}

		// Token: 0x0600DAB9 RID: 55993 RVA: 0x004E5E74 File Offset: 0x004E4074
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

		// Token: 0x0600DABA RID: 55994 RVA: 0x004E5FAC File Offset: 0x004E41AC
		[PublicizedFrom(EAccessModifier.Private)]
		public ReadResult ReadChunkCallback(ref ReadFileDataCallbackInfo _callbackData)
		{
			((RemotePlayerFileStorage.ReadRequestDetails)_callbackData.ClientData).Chunks.Add(_callbackData.DataChunk);
			return ReadResult.ContinueReading;
		}

		// Token: 0x0600DABB RID: 55995 RVA: 0x004E5FCC File Offset: 0x004E41CC
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

		// Token: 0x0600DABC RID: 55996 RVA: 0x004E619C File Offset: 0x004E439C
		public void WriteRemoteData(string _filename, byte[] _data, bool _overwriteCache, IRemotePlayerFileStorage.FileWriteCompleteCallback _callback)
		{
			object obj = this.queueLock;
			lock (obj)
			{
				this.operationQueue.Enqueue(new RemotePlayerFileStorage.StorageOperation(_filename, _data, _overwriteCache, _callback));
				this.ProcessNextOperation();
			}
		}

		// Token: 0x0600DABD RID: 55997 RVA: 0x004E61F4 File Offset: 0x004E43F4
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

		// Token: 0x0600DABE RID: 55998 RVA: 0x004E634C File Offset: 0x004E454C
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

		// Token: 0x0600DABF RID: 55999 RVA: 0x004E6398 File Offset: 0x004E4598
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

		// Token: 0x0400A54A RID: 42314
		[PublicizedFrom(EAccessModifier.Private)]
		public const int cReadWriteByteLimit = 524288;

		// Token: 0x0400A54B RID: 42315
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A54C RID: 42316
		[PublicizedFrom(EAccessModifier.Private)]
		public PlayerDataStorageInterface playerDataStorage;

		// Token: 0x0400A54D RID: 42317
		[PublicizedFrom(EAccessModifier.Private)]
		public Queue<RemotePlayerFileStorage.StorageOperation> operationQueue = new Queue<RemotePlayerFileStorage.StorageOperation>();

		// Token: 0x0400A54E RID: 42318
		[PublicizedFrom(EAccessModifier.Private)]
		public object queueLock = new object();

		// Token: 0x0400A54F RID: 42319
		[PublicizedFrom(EAccessModifier.Private)]
		public bool queueProcessing;

		// Token: 0x02001CDD RID: 7389
		[PublicizedFrom(EAccessModifier.Private)]
		public struct StorageOperation
		{
			// Token: 0x0600DAC1 RID: 56001 RVA: 0x004E6472 File Offset: 0x004E4672
			public StorageOperation(string _filename, bool _overwriteCache, IRemotePlayerFileStorage.FileReadCompleteCallback _callback)
			{
				this.ToRead = true;
				this.Filename = _filename;
				this.OverwriteCache = _overwriteCache;
				this.Data = null;
				this.ReadCallback = _callback;
				this.WriteCallback = null;
			}

			// Token: 0x0600DAC2 RID: 56002 RVA: 0x004E649E File Offset: 0x004E469E
			public StorageOperation(string _filename, byte[] _data, bool _overwriteCache, IRemotePlayerFileStorage.FileWriteCompleteCallback _callback)
			{
				this.ToRead = false;
				this.Filename = _filename;
				this.Data = _data;
				this.OverwriteCache = _overwriteCache;
				this.ReadCallback = null;
				this.WriteCallback = _callback;
			}

			// Token: 0x0400A550 RID: 42320
			public bool ToRead;

			// Token: 0x0400A551 RID: 42321
			public string Filename;

			// Token: 0x0400A552 RID: 42322
			public bool OverwriteCache;

			// Token: 0x0400A553 RID: 42323
			public byte[] Data;

			// Token: 0x0400A554 RID: 42324
			public IRemotePlayerFileStorage.FileReadCompleteCallback ReadCallback;

			// Token: 0x0400A555 RID: 42325
			public IRemotePlayerFileStorage.FileWriteCompleteCallback WriteCallback;
		}

		// Token: 0x02001CDE RID: 7390
		[PublicizedFrom(EAccessModifier.Private)]
		public class ReadRequestDetails
		{
			// Token: 0x0600DAC3 RID: 56003 RVA: 0x004E64CB File Offset: 0x004E46CB
			public ReadRequestDetails(IRemotePlayerFileStorage.FileReadCompleteCallback _callback, bool _overwriteCache)
			{
				this.Callback = _callback;
				this.OverwriteCache = _overwriteCache;
				this.Chunks = new List<ArraySegment<byte>>();
			}

			// Token: 0x0400A556 RID: 42326
			public readonly IRemotePlayerFileStorage.FileReadCompleteCallback Callback;

			// Token: 0x0400A557 RID: 42327
			public List<ArraySegment<byte>> Chunks;

			// Token: 0x0400A558 RID: 42328
			public bool OverwriteCache;
		}

		// Token: 0x02001CDF RID: 7391
		[PublicizedFrom(EAccessModifier.Private)]
		public class WriteRequestDetails
		{
			// Token: 0x0600DAC4 RID: 56004 RVA: 0x004E64EC File Offset: 0x004E46EC
			public WriteRequestDetails(bool _writeToCache, byte[] _data, IRemotePlayerFileStorage.FileWriteCompleteCallback _callback)
			{
				this.WriteToCache = _writeToCache;
				this.Data = _data;
				this.Callback = _callback;
			}

			// Token: 0x0600DAC5 RID: 56005 RVA: 0x004E650C File Offset: 0x004E470C
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

			// Token: 0x0600DAC6 RID: 56006 RVA: 0x004E6574 File Offset: 0x004E4774
			public bool HasNextChunk()
			{
				return this.DataPointer < this.Data.Length;
			}

			// Token: 0x0400A559 RID: 42329
			public bool WriteToCache;

			// Token: 0x0400A55A RID: 42330
			public byte[] Data;

			// Token: 0x0400A55B RID: 42331
			public readonly IRemotePlayerFileStorage.FileWriteCompleteCallback Callback;

			// Token: 0x0400A55C RID: 42332
			[PublicizedFrom(EAccessModifier.Private)]
			public int DataPointer;
		}
	}
}

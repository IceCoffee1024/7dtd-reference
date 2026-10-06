using System;
using System.Collections.Generic;
using System.IO;
using Epic.OnlineServices;
using Epic.OnlineServices.TitleStorage;

namespace Platform.EOS
{
	// Token: 0x02001D33 RID: 7475
	public class RemoteFileStorage : IRemoteFileStorage
	{
		// Token: 0x0600DDB1 RID: 56753 RVA: 0x004F0460 File Offset: 0x004EE660
		public void Init(IPlatform _owner)
		{
			this.owner = _owner;
			this.owner.Api.ClientApiInitialized += this.OnClientApiInitialized;
			if (this.owner.User != null)
			{
				this.owner.User.UserLoggedIn += this.OnUserLoggedIn;
			}
		}

		// Token: 0x0600DDB2 RID: 56754 RVA: 0x004F04B9 File Offset: 0x004EE6B9
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnUserLoggedIn(IPlatform _obj)
		{
			if (this.IsReady)
			{
				this.clearCache();
			}
		}

		// Token: 0x0600DDB3 RID: 56755 RVA: 0x004F04C9 File Offset: 0x004EE6C9
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnClientApiInitialized()
		{
			this.titleStorageInterface = ((Api)this.owner.Api).PlatformInterface.GetTitleStorageInterface();
		}

		// Token: 0x17001C1A RID: 7194
		// (get) Token: 0x0600DDB4 RID: 56756 RVA: 0x004F04EC File Offset: 0x004EE6EC
		public bool IsReady
		{
			get
			{
				if (!(this.titleStorageInterface != null))
				{
					return false;
				}
				if (GameManager.IsDedicatedServer)
				{
					return true;
				}
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

		// Token: 0x17001C1B RID: 7195
		// (get) Token: 0x0600DDB5 RID: 56757 RVA: 0x004F0554 File Offset: 0x004EE754
		public bool Unavailable
		{
			get
			{
				bool flag = !this.IsReady;
				if (flag)
				{
					IPlatform platform = this.owner;
					EUserStatus? euserStatus;
					if (platform == null)
					{
						euserStatus = null;
					}
					else
					{
						IUserClient user = platform.User;
						euserStatus = ((user != null) ? new EUserStatus?(user.UserStatus) : null);
					}
					EUserStatus? euserStatus2 = euserStatus;
					bool flag2;
					if (euserStatus2 != null)
					{
						EUserStatus valueOrDefault = euserStatus2.GetValueOrDefault();
						if (valueOrDefault == EUserStatus.OfflineMode || valueOrDefault - EUserStatus.PermanentError <= 1)
						{
							flag2 = true;
							goto IL_0064;
						}
					}
					flag2 = false;
					IL_0064:
					flag = flag2;
				}
				return flag;
			}
		}

		// Token: 0x0600DDB6 RID: 56758 RVA: 0x004F05C8 File Offset: 0x004EE7C8
		public void GetFile(string _filename, IRemoteFileStorage.FileDownloadCompleteCallback _callback)
		{
			if (_callback == null)
			{
				return;
			}
			if (string.IsNullOrEmpty(_filename))
			{
				_callback(IRemoteFileStorage.EFileDownloadResult.EmptyFilename, null, null);
				return;
			}
			IUserClient user = this.owner.User;
			PlatformUserIdentifierAbs platformUserIdentifierAbs;
			if ((platformUserIdentifierAbs = ((user != null) ? user.PlatformUserId : null)) == null)
			{
				IUserClient userServer = this.owner.UserServer;
				platformUserIdentifierAbs = ((userServer != null) ? userServer.PlatformUserId : null);
			}
			UserIdentifierEos userIdentifierEos = (UserIdentifierEos)platformUserIdentifierAbs;
			ProductUserId productUserId = ((userIdentifierEos != null) ? userIdentifierEos.ProductUserId : null);
			object obj = this.requestsLock;
			bool flag2;
			lock (obj)
			{
				RemoteFileStorage.RequestDetails requestDetails;
				flag2 = !this.requests.TryGetValue(_filename, out requestDetails);
				if (flag2)
				{
					Log.Out("[EOS] Created RFS Request: " + _filename);
					requestDetails = new RemoteFileStorage.RequestDetails(_filename, productUserId, _callback);
					this.requests.Add(_filename, requestDetails);
					this.requestQueue.Enqueue(requestDetails);
				}
				else
				{
					Log.Out("[EOS] Adding callback to existing RFS Request: " + _filename);
					requestDetails.Callback += _callback;
				}
			}
			if (flag2)
			{
				EosHelpers.AssertMainThread("RFS.Get");
				this.TryProcessNextRequest();
			}
		}

		// Token: 0x0600DDB7 RID: 56759 RVA: 0x004F06D4 File Offset: 0x004EE8D4
		[PublicizedFrom(EAccessModifier.Private)]
		public void TryProcessNextRequest()
		{
			object obj = this.requestsLock;
			lock (obj)
			{
				while (this.activeRequests < 16 && this.requestQueue.Count > 0)
				{
					RemoteFileStorage.RequestDetails requestDetails = this.requestQueue.Dequeue();
					if (!requestDetails.Cancelled)
					{
						this.activeRequests++;
						this.getMetadata(requestDetails);
					}
				}
			}
		}

		// Token: 0x0600DDB8 RID: 56760 RVA: 0x004F0754 File Offset: 0x004EE954
		public bool CancelGetFile(string fileName, string cancelReason)
		{
			object lockObject = this.requestsLock;
			RemoteFileStorage.RequestDetails requestDetails;
			lock (lockObject)
			{
				if (!this.requests.TryGetValue(fileName, out requestDetails))
				{
					return false;
				}
				requestDetails.Cancelled = true;
			}
			if (requestDetails.Transfer != null)
			{
				lockObject = AntiCheatCommon.LockObject;
				Result result;
				lock (lockObject)
				{
					result = requestDetails.Transfer.CancelRequest();
				}
				if (result != Result.Success && result != Result.NoChange)
				{
					Log.Warning("[EOS] CancelRequest (" + fileName + ") returned: " + result.ToStringCached<Result>());
				}
			}
			this.CompleteRequest(requestDetails, IRemoteFileStorage.EFileDownloadResult.Other, cancelReason, null);
			return true;
		}

		// Token: 0x0600DDB9 RID: 56761 RVA: 0x004F0820 File Offset: 0x004EEA20
		[PublicizedFrom(EAccessModifier.Private)]
		public void getMetadata(RemoteFileStorage.RequestDetails _details)
		{
			QueryFileOptions queryFileOptions = new QueryFileOptions
			{
				Filename = _details.Filename,
				LocalUserId = _details.LocalUserId
			};
			object lockObject = AntiCheatCommon.LockObject;
			lock (lockObject)
			{
				this.titleStorageInterface.QueryFile(ref queryFileOptions, _details, new OnQueryFileCompleteCallback(this.queryFileCallback));
			}
		}

		// Token: 0x0600DDBA RID: 56762 RVA: 0x004F089C File Offset: 0x004EEA9C
		[PublicizedFrom(EAccessModifier.Private)]
		public void queryFileCallback(ref QueryFileCallbackInfo _callbackData)
		{
			RemoteFileStorage.RequestDetails requestDetails = (RemoteFileStorage.RequestDetails)_callbackData.ClientData;
			if (requestDetails.Cancelled)
			{
				return;
			}
			if (_callbackData.ResultCode != Result.Success)
			{
				Log.Error("[EOS] QueryFile (" + requestDetails.Filename + ") failed: " + _callbackData.ResultCode.ToStringCached<Result>());
				this.CompleteRequest(requestDetails, IRemoteFileStorage.EFileDownloadResult.FileNotFound, _callbackData.ResultCode.ToStringCached<Result>(), null);
				return;
			}
			CopyFileMetadataByFilenameOptions copyFileMetadataByFilenameOptions = new CopyFileMetadataByFilenameOptions
			{
				Filename = requestDetails.Filename,
				LocalUserId = requestDetails.LocalUserId
			};
			object lockObject = AntiCheatCommon.LockObject;
			Result result;
			lock (lockObject)
			{
				FileMetadata? fileMetadata;
				result = this.titleStorageInterface.CopyFileMetadataByFilename(ref copyFileMetadataByFilenameOptions, out fileMetadata);
			}
			if (result != Result.Success)
			{
				Log.Error("[EOS] CopyFileMetadataByFilename (" + requestDetails.Filename + ") failed: " + result.ToStringCached<Result>());
				this.CompleteRequest(requestDetails, IRemoteFileStorage.EFileDownloadResult.Other, result.ToStringCached<Result>(), null);
				return;
			}
			this.readFile(requestDetails);
		}

		// Token: 0x0600DDBB RID: 56763 RVA: 0x004F09A4 File Offset: 0x004EEBA4
		[PublicizedFrom(EAccessModifier.Private)]
		public void readFile(RemoteFileStorage.RequestDetails _details)
		{
			ReadFileOptions readFileOptions = new ReadFileOptions
			{
				Filename = _details.Filename,
				LocalUserId = _details.LocalUserId,
				ReadFileDataCallback = new OnReadFileDataCallback(this.readFileDataCallback),
				ReadChunkLengthBytes = 524288U
			};
			object lockObject = AntiCheatCommon.LockObject;
			lock (lockObject)
			{
				_details.Transfer = this.titleStorageInterface.ReadFile(ref readFileOptions, _details, new OnReadFileCompleteCallback(this.readCompletedCallback));
			}
		}

		// Token: 0x0600DDBC RID: 56764 RVA: 0x004F0A48 File Offset: 0x004EEC48
		[PublicizedFrom(EAccessModifier.Private)]
		public void fileTransferProgressCallback(ref FileTransferProgressCallbackInfo _callbackData)
		{
			RemoteFileStorage.RequestDetails requestDetails = (RemoteFileStorage.RequestDetails)_callbackData.ClientData;
			Log.Out(string.Format("[EOS] TransferProgress: {0}, {1} / {2}", _callbackData.Filename, _callbackData.BytesTransferred, _callbackData.TotalFileSizeBytes));
		}

		// Token: 0x0600DDBD RID: 56765 RVA: 0x004F0A81 File Offset: 0x004EEC81
		[PublicizedFrom(EAccessModifier.Private)]
		public ReadResult readFileDataCallback(ref ReadFileDataCallbackInfo _callbackData)
		{
			((RemoteFileStorage.RequestDetails)_callbackData.ClientData).Chunks.Add(_callbackData.DataChunk);
			return ReadResult.RrContinueReading;
		}

		// Token: 0x0600DDBE RID: 56766 RVA: 0x004F0AA0 File Offset: 0x004EECA0
		[PublicizedFrom(EAccessModifier.Private)]
		public void readCompletedCallback(ref ReadFileCallbackInfo _callbackData)
		{
			RemoteFileStorage.RequestDetails requestDetails = (RemoteFileStorage.RequestDetails)_callbackData.ClientData;
			if (requestDetails.Cancelled)
			{
				return;
			}
			if (_callbackData.ResultCode == Result.Success)
			{
				int num = 0;
				foreach (ArraySegment<byte> arraySegment in requestDetails.Chunks)
				{
					num += arraySegment.Count;
				}
				byte[] array = new byte[num];
				int num2 = 0;
				for (int i = 0; i < requestDetails.Chunks.Count; i++)
				{
					Array.Copy(requestDetails.Chunks[i].Array, requestDetails.Chunks[i].Offset, array, num2, requestDetails.Chunks[i].Count);
					num2 += requestDetails.Chunks[i].Count;
				}
				Log.Out(string.Format("[EOS] Read ({0}) completed: {1}, received {2} bytes", _callbackData.Filename, _callbackData.ResultCode, num));
				this.cacheFile(requestDetails.Filename, array);
				this.CompleteRequest(requestDetails, IRemoteFileStorage.EFileDownloadResult.Ok, null, array);
				return;
			}
			if (_callbackData.ResultCode == Result.TooManyRequests)
			{
				Log.Error(string.Concat(new string[]
				{
					"[EOS] Read (",
					requestDetails.Filename,
					") failed: ",
					_callbackData.ResultCode.ToStringCached<Result>(),
					". Try lowering the MaxConcurrentReads value."
				}));
				this.CompleteRequest(requestDetails, IRemoteFileStorage.EFileDownloadResult.Other, _callbackData.ResultCode.ToStringCached<Result>(), null);
				return;
			}
			if (_callbackData.ResultCode != Result.OperationWillRetry)
			{
				Log.Error("[EOS] Read (" + requestDetails.Filename + ") failed: " + _callbackData.ResultCode.ToStringCached<Result>());
				this.CompleteRequest(requestDetails, IRemoteFileStorage.EFileDownloadResult.Other, _callbackData.ResultCode.ToStringCached<Result>(), null);
			}
		}

		// Token: 0x0600DDBF RID: 56767 RVA: 0x004F0C80 File Offset: 0x004EEE80
		[PublicizedFrom(EAccessModifier.Private)]
		public void CompleteRequest(RemoteFileStorage.RequestDetails _details, IRemoteFileStorage.EFileDownloadResult _result, string _errorName, byte[] _data)
		{
			object obj = this.requestsLock;
			lock (obj)
			{
				RemoteFileStorage.RequestDetails requestDetails;
				if (!this.requests.TryGetValue(_details.Filename, out requestDetails) || requestDetails != _details)
				{
					Log.Out(string.Concat(new string[]
					{
						"[EOS] Ignoring completion of RFS request no longer tracked: ",
						_details.Filename,
						" (",
						_result.ToStringCached<IRemoteFileStorage.EFileDownloadResult>(),
						")"
					}));
					return;
				}
				this.requests.Remove(_details.Filename);
				this.activeRequests = Math.Max(0, this.activeRequests - 1);
				this.TryProcessNextRequest();
			}
			_details.ExecuteCallback(_result, _errorName, _data);
		}

		// Token: 0x17001C1C RID: 7196
		// (get) Token: 0x0600DDC0 RID: 56768 RVA: 0x004F0D48 File Offset: 0x004EEF48
		public string CacheFilePrefix
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return "Rfs_";
			}
		}

		// Token: 0x17001C1D RID: 7197
		// (get) Token: 0x0600DDC1 RID: 56769 RVA: 0x004F0D4F File Offset: 0x004EEF4F
		public string CacheFolder
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return GameIO.GetDeviceLocalUserGameDataDir() + "/RfsCache";
			}
		}

		// Token: 0x0600DDC2 RID: 56770 RVA: 0x004F0D60 File Offset: 0x004EEF60
		[PublicizedFrom(EAccessModifier.Private)]
		public void clearCache()
		{
			if (!SdDirectory.Exists(this.CacheFolder))
			{
				return;
			}
			foreach (string text in SdDirectory.GetFiles(this.CacheFolder))
			{
				if (Path.GetFileName(text).StartsWith(this.CacheFilePrefix))
				{
					SdFile.Delete(text);
				}
			}
		}

		// Token: 0x0600DDC3 RID: 56771 RVA: 0x004F0DB2 File Offset: 0x004EEFB2
		[PublicizedFrom(EAccessModifier.Private)]
		public void cacheFile(string _filename, byte[] _data)
		{
			SdDirectory.CreateDirectory(this.CacheFolder);
			SdFile.WriteAllBytes(this.CacheFolder + "/" + this.CacheFilePrefix + _filename, _data);
		}

		// Token: 0x0600DDC4 RID: 56772 RVA: 0x004F0DE0 File Offset: 0x004EEFE0
		public void GetCachedFile(string _filename, IRemoteFileStorage.FileDownloadCompleteCallback _callback)
		{
			if (_callback == null)
			{
				return;
			}
			if (string.IsNullOrEmpty(_filename))
			{
				_callback(IRemoteFileStorage.EFileDownloadResult.EmptyFilename, null, null);
				return;
			}
			string text = this.CacheFolder + "/" + this.CacheFilePrefix + _filename;
			if (!SdFile.Exists(text))
			{
				_callback(IRemoteFileStorage.EFileDownloadResult.FileNotFound, "File not found", null);
				return;
			}
			byte[] array = SdFile.ReadAllBytes(text);
			Log.Out(string.Format("[EOS] Read cached ({0}) completed: {1} bytes", _filename, array.Length));
			_callback(IRemoteFileStorage.EFileDownloadResult.Ok, null, array);
		}

		// Token: 0x0400A746 RID: 42822
		public const string CacheFolderName = "RfsCache";

		// Token: 0x0400A747 RID: 42823
		[PublicizedFrom(EAccessModifier.Private)]
		public const int MaxConcurrentReads = 16;

		// Token: 0x0400A748 RID: 42824
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A749 RID: 42825
		[PublicizedFrom(EAccessModifier.Private)]
		public TitleStorageInterface titleStorageInterface;

		// Token: 0x0400A74A RID: 42826
		[PublicizedFrom(EAccessModifier.Private)]
		public object requestsLock = new object();

		// Token: 0x0400A74B RID: 42827
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<string, RemoteFileStorage.RequestDetails> requests = new Dictionary<string, RemoteFileStorage.RequestDetails>();

		// Token: 0x0400A74C RID: 42828
		[PublicizedFrom(EAccessModifier.Private)]
		public Queue<RemoteFileStorage.RequestDetails> requestQueue = new Queue<RemoteFileStorage.RequestDetails>();

		// Token: 0x0400A74D RID: 42829
		[PublicizedFrom(EAccessModifier.Private)]
		public int activeRequests;

		// Token: 0x02001D34 RID: 7476
		[PublicizedFrom(EAccessModifier.Private)]
		public class RequestDetails
		{
			// Token: 0x14000140 RID: 320
			// (add) Token: 0x0600DDC6 RID: 56774 RVA: 0x004F0E84 File Offset: 0x004EF084
			// (remove) Token: 0x0600DDC7 RID: 56775 RVA: 0x004F0EBC File Offset: 0x004EF0BC
			public event IRemoteFileStorage.FileDownloadCompleteCallback Callback;

			// Token: 0x0600DDC8 RID: 56776 RVA: 0x004F0EF1 File Offset: 0x004EF0F1
			public RequestDetails(string _filename, ProductUserId _localUserId, IRemoteFileStorage.FileDownloadCompleteCallback _callback)
			{
				this.Filename = _filename;
				this.LocalUserId = _localUserId;
				this.Callback = _callback;
			}

			// Token: 0x0600DDC9 RID: 56777 RVA: 0x004F0F19 File Offset: 0x004EF119
			public void ExecuteCallback(IRemoteFileStorage.EFileDownloadResult _result, string _errorName, byte[] _data)
			{
				IRemoteFileStorage.FileDownloadCompleteCallback callback = this.Callback;
				if (callback == null)
				{
					return;
				}
				callback(_result, _errorName, _data);
			}

			// Token: 0x0400A74E RID: 42830
			public readonly string Filename;

			// Token: 0x0400A74F RID: 42831
			public readonly ProductUserId LocalUserId;

			// Token: 0x0400A751 RID: 42833
			public readonly List<ArraySegment<byte>> Chunks = new List<ArraySegment<byte>>();

			// Token: 0x0400A752 RID: 42834
			public TitleStorageFileTransferRequest Transfer;

			// Token: 0x0400A753 RID: 42835
			public bool Cancelled;
		}
	}
}

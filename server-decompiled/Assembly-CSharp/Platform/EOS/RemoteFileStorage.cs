using System;
using System.Collections.Generic;
using System.IO;
using Epic.OnlineServices;
using Epic.OnlineServices.TitleStorage;

namespace Platform.EOS
{
	// Token: 0x02001D01 RID: 7425
	public class RemoteFileStorage : IRemoteFileStorage
	{
		// Token: 0x0600DC36 RID: 56374 RVA: 0x004EB918 File Offset: 0x004E9B18
		public void Init(IPlatform _owner)
		{
			this.owner = _owner;
			this.owner.Api.ClientApiInitialized += this.OnClientApiInitialized;
			if (this.owner.User != null)
			{
				this.owner.User.UserLoggedIn += this.OnUserLoggedIn;
			}
		}

		// Token: 0x0600DC37 RID: 56375 RVA: 0x004EB971 File Offset: 0x004E9B71
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnUserLoggedIn(IPlatform _obj)
		{
			if (this.IsReady)
			{
				this.clearCache();
			}
		}

		// Token: 0x0600DC38 RID: 56376 RVA: 0x004EB981 File Offset: 0x004E9B81
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnClientApiInitialized()
		{
			this.titleStorageInterface = ((Api)this.owner.Api).PlatformInterface.GetTitleStorageInterface();
		}

		// Token: 0x17001B74 RID: 7028
		// (get) Token: 0x0600DC39 RID: 56377 RVA: 0x004EB9A4 File Offset: 0x004E9BA4
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

		// Token: 0x17001B75 RID: 7029
		// (get) Token: 0x0600DC3A RID: 56378 RVA: 0x004EBA0C File Offset: 0x004E9C0C
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

		// Token: 0x0600DC3B RID: 56379 RVA: 0x004EBA80 File Offset: 0x004E9C80
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

		// Token: 0x0600DC3C RID: 56380 RVA: 0x004EBB8C File Offset: 0x004E9D8C
		[PublicizedFrom(EAccessModifier.Private)]
		public void TryProcessNextRequest()
		{
			object obj = this.requestsLock;
			lock (obj)
			{
				while (this.activeRequests < 16 && this.requestQueue.Count > 0)
				{
					RemoteFileStorage.RequestDetails requestDetails = this.requestQueue.Dequeue();
					this.activeRequests++;
					this.getMetadata(requestDetails);
				}
			}
		}

		// Token: 0x0600DC3D RID: 56381 RVA: 0x004EBC04 File Offset: 0x004E9E04
		public bool CancelGetFile(string fileName, string cancelReason)
		{
			RemoteFileStorage.RequestDetails requestDetails;
			if (this.requests.TryGetValue(fileName, out requestDetails))
			{
				this.CompleteRequest(requestDetails, IRemoteFileStorage.EFileDownloadResult.Other, cancelReason, null);
				return true;
			}
			return false;
		}

		// Token: 0x0600DC3E RID: 56382 RVA: 0x004EBC30 File Offset: 0x004E9E30
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

		// Token: 0x0600DC3F RID: 56383 RVA: 0x004EBCAC File Offset: 0x004E9EAC
		[PublicizedFrom(EAccessModifier.Private)]
		public void queryFileCallback(ref QueryFileCallbackInfo _callbackData)
		{
			RemoteFileStorage.RequestDetails requestDetails = (RemoteFileStorage.RequestDetails)_callbackData.ClientData;
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
				this.CompleteRequest(requestDetails, IRemoteFileStorage.EFileDownloadResult.Other, _callbackData.ResultCode.ToStringCached<Result>(), null);
				return;
			}
			this.readFile(requestDetails);
		}

		// Token: 0x0600DC40 RID: 56384 RVA: 0x004EBDB0 File Offset: 0x004E9FB0
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
				this.titleStorageInterface.ReadFile(ref readFileOptions, _details, new OnReadFileCompleteCallback(this.readCompletedCallback));
			}
		}

		// Token: 0x0600DC41 RID: 56385 RVA: 0x004EBE4C File Offset: 0x004EA04C
		[PublicizedFrom(EAccessModifier.Private)]
		public void fileTransferProgressCallback(ref FileTransferProgressCallbackInfo _callbackData)
		{
			RemoteFileStorage.RequestDetails requestDetails = (RemoteFileStorage.RequestDetails)_callbackData.ClientData;
			Log.Out(string.Format("[EOS] TransferProgress: {0}, {1} / {2}", _callbackData.Filename, _callbackData.BytesTransferred, _callbackData.TotalFileSizeBytes));
		}

		// Token: 0x0600DC42 RID: 56386 RVA: 0x004EBE85 File Offset: 0x004EA085
		[PublicizedFrom(EAccessModifier.Private)]
		public ReadResult readFileDataCallback(ref ReadFileDataCallbackInfo _callbackData)
		{
			((RemoteFileStorage.RequestDetails)_callbackData.ClientData).Chunks.Add(_callbackData.DataChunk);
			return ReadResult.RrContinueReading;
		}

		// Token: 0x0600DC43 RID: 56387 RVA: 0x004EBEA4 File Offset: 0x004EA0A4
		[PublicizedFrom(EAccessModifier.Private)]
		public void readCompletedCallback(ref ReadFileCallbackInfo _callbackData)
		{
			RemoteFileStorage.RequestDetails requestDetails = (RemoteFileStorage.RequestDetails)_callbackData.ClientData;
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

		// Token: 0x0600DC44 RID: 56388 RVA: 0x004EC078 File Offset: 0x004EA278
		[PublicizedFrom(EAccessModifier.Private)]
		public void CompleteRequest(RemoteFileStorage.RequestDetails _details, IRemoteFileStorage.EFileDownloadResult _result, string _errorName, byte[] _data)
		{
			object obj = this.requestsLock;
			lock (obj)
			{
				if (!this.requests.Remove(_details.Filename))
				{
					Log.Warning("[EOS] Unexpected RFS request being completed: " + _details.Filename);
				}
				this.activeRequests = Math.Max(0, this.activeRequests - 1);
				this.TryProcessNextRequest();
			}
			_details.ExecuteCallback(_result, _errorName, _data);
		}

		// Token: 0x17001B76 RID: 7030
		// (get) Token: 0x0600DC45 RID: 56389 RVA: 0x004EC100 File Offset: 0x004EA300
		public string CacheFilePrefix
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return "Rfs_";
			}
		}

		// Token: 0x17001B77 RID: 7031
		// (get) Token: 0x0600DC46 RID: 56390 RVA: 0x004EC107 File Offset: 0x004EA307
		public string CacheFolder
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return GameIO.GetDeviceLocalUserGameDataDir() + "/RfsCache";
			}
		}

		// Token: 0x0600DC47 RID: 56391 RVA: 0x004EC118 File Offset: 0x004EA318
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

		// Token: 0x0600DC48 RID: 56392 RVA: 0x004EC16A File Offset: 0x004EA36A
		[PublicizedFrom(EAccessModifier.Private)]
		public void cacheFile(string _filename, byte[] _data)
		{
			SdDirectory.CreateDirectory(this.CacheFolder);
			SdFile.WriteAllBytes(this.CacheFolder + "/" + this.CacheFilePrefix + _filename, _data);
		}

		// Token: 0x0600DC49 RID: 56393 RVA: 0x004EC198 File Offset: 0x004EA398
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

		// Token: 0x0400A671 RID: 42609
		public const string CacheFolderName = "RfsCache";

		// Token: 0x0400A672 RID: 42610
		[PublicizedFrom(EAccessModifier.Private)]
		public const int MaxConcurrentReads = 16;

		// Token: 0x0400A673 RID: 42611
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A674 RID: 42612
		[PublicizedFrom(EAccessModifier.Private)]
		public TitleStorageInterface titleStorageInterface;

		// Token: 0x0400A675 RID: 42613
		[PublicizedFrom(EAccessModifier.Private)]
		public object requestsLock = new object();

		// Token: 0x0400A676 RID: 42614
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<string, RemoteFileStorage.RequestDetails> requests = new Dictionary<string, RemoteFileStorage.RequestDetails>();

		// Token: 0x0400A677 RID: 42615
		[PublicizedFrom(EAccessModifier.Private)]
		public Queue<RemoteFileStorage.RequestDetails> requestQueue = new Queue<RemoteFileStorage.RequestDetails>();

		// Token: 0x0400A678 RID: 42616
		[PublicizedFrom(EAccessModifier.Private)]
		public int activeRequests;

		// Token: 0x02001D02 RID: 7426
		[PublicizedFrom(EAccessModifier.Private)]
		public class RequestDetails
		{
			// Token: 0x14000141 RID: 321
			// (add) Token: 0x0600DC4B RID: 56395 RVA: 0x004EC23C File Offset: 0x004EA43C
			// (remove) Token: 0x0600DC4C RID: 56396 RVA: 0x004EC274 File Offset: 0x004EA474
			public event IRemoteFileStorage.FileDownloadCompleteCallback Callback;

			// Token: 0x0600DC4D RID: 56397 RVA: 0x004EC2A9 File Offset: 0x004EA4A9
			public RequestDetails(string _filename, ProductUserId _localUserId, IRemoteFileStorage.FileDownloadCompleteCallback _callback)
			{
				this.Filename = _filename;
				this.LocalUserId = _localUserId;
				this.Callback = _callback;
			}

			// Token: 0x0600DC4E RID: 56398 RVA: 0x004EC2D1 File Offset: 0x004EA4D1
			public void ExecuteCallback(IRemoteFileStorage.EFileDownloadResult _result, string _errorName, byte[] _data)
			{
				IRemoteFileStorage.FileDownloadCompleteCallback callback = this.Callback;
				if (callback == null)
				{
					return;
				}
				callback(_result, _errorName, _data);
			}

			// Token: 0x0400A679 RID: 42617
			public readonly string Filename;

			// Token: 0x0400A67A RID: 42618
			public readonly ProductUserId LocalUserId;

			// Token: 0x0400A67C RID: 42620
			public readonly List<ArraySegment<byte>> Chunks = new List<ArraySegment<byte>>();
		}
	}
}

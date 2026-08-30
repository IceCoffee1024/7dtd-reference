using System;
using System.Collections.Generic;
using System.IO;
using Epic.OnlineServices;
using Epic.OnlineServices.TitleStorage;

namespace Platform.EOS
{
	// Token: 0x02001CDA RID: 7386
	public class RemoteFileStorage : IRemoteFileStorage
	{
		// Token: 0x0600DA9B RID: 55963 RVA: 0x004E5304 File Offset: 0x004E3504
		public void Init(IPlatform _owner)
		{
			this.owner = _owner;
			this.owner.Api.ClientApiInitialized += this.OnClientApiInitialized;
			if (this.owner.User != null)
			{
				this.owner.User.UserLoggedIn += this.OnUserLoggedIn;
			}
		}

		// Token: 0x0600DA9C RID: 55964 RVA: 0x004E535D File Offset: 0x004E355D
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnUserLoggedIn(IPlatform _obj)
		{
			if (this.IsReady)
			{
				this.clearCache();
			}
		}

		// Token: 0x0600DA9D RID: 55965 RVA: 0x004E536D File Offset: 0x004E356D
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnClientApiInitialized()
		{
			this.titleStorageInterface = ((Api)this.owner.Api).PlatformInterface.GetTitleStorageInterface();
		}

		// Token: 0x17001B02 RID: 6914
		// (get) Token: 0x0600DA9E RID: 55966 RVA: 0x004E5390 File Offset: 0x004E3590
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

		// Token: 0x17001B03 RID: 6915
		// (get) Token: 0x0600DA9F RID: 55967 RVA: 0x004E53F8 File Offset: 0x004E35F8
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

		// Token: 0x0600DAA0 RID: 55968 RVA: 0x004E546C File Offset: 0x004E366C
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

		// Token: 0x0600DAA1 RID: 55969 RVA: 0x004E5578 File Offset: 0x004E3778
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

		// Token: 0x0600DAA2 RID: 55970 RVA: 0x004E55F0 File Offset: 0x004E37F0
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

		// Token: 0x0600DAA3 RID: 55971 RVA: 0x004E566C File Offset: 0x004E386C
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

		// Token: 0x0600DAA4 RID: 55972 RVA: 0x004E5770 File Offset: 0x004E3970
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

		// Token: 0x0600DAA5 RID: 55973 RVA: 0x004E580C File Offset: 0x004E3A0C
		[PublicizedFrom(EAccessModifier.Private)]
		public void fileTransferProgressCallback(ref FileTransferProgressCallbackInfo _callbackData)
		{
			RemoteFileStorage.RequestDetails requestDetails = (RemoteFileStorage.RequestDetails)_callbackData.ClientData;
			Log.Out(string.Format("[EOS] TransferProgress: {0}, {1} / {2}", _callbackData.Filename, _callbackData.BytesTransferred, _callbackData.TotalFileSizeBytes));
		}

		// Token: 0x0600DAA6 RID: 55974 RVA: 0x004E5845 File Offset: 0x004E3A45
		[PublicizedFrom(EAccessModifier.Private)]
		public ReadResult readFileDataCallback(ref ReadFileDataCallbackInfo _callbackData)
		{
			((RemoteFileStorage.RequestDetails)_callbackData.ClientData).Chunks.Add(_callbackData.DataChunk);
			return ReadResult.RrContinueReading;
		}

		// Token: 0x0600DAA7 RID: 55975 RVA: 0x004E5864 File Offset: 0x004E3A64
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

		// Token: 0x0600DAA8 RID: 55976 RVA: 0x004E5A38 File Offset: 0x004E3C38
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
			_details.ExecuteCallback(IRemoteFileStorage.EFileDownloadResult.Ok, null, _data);
		}

		// Token: 0x17001B04 RID: 6916
		// (get) Token: 0x0600DAA9 RID: 55977 RVA: 0x004E5AC0 File Offset: 0x004E3CC0
		public string CacheFilePrefix
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return "Rfs_";
			}
		}

		// Token: 0x17001B05 RID: 6917
		// (get) Token: 0x0600DAAA RID: 55978 RVA: 0x004E5AC7 File Offset: 0x004E3CC7
		public string CacheFolder
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return GameIO.GetDeviceLocalUserGameDataDir() + "/RfsCache";
			}
		}

		// Token: 0x0600DAAB RID: 55979 RVA: 0x004E5AD8 File Offset: 0x004E3CD8
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

		// Token: 0x0600DAAC RID: 55980 RVA: 0x004E5B2A File Offset: 0x004E3D2A
		[PublicizedFrom(EAccessModifier.Private)]
		public void cacheFile(string _filename, byte[] _data)
		{
			SdDirectory.CreateDirectory(this.CacheFolder);
			SdFile.WriteAllBytes(this.CacheFolder + "/" + this.CacheFilePrefix + _filename, _data);
		}

		// Token: 0x0600DAAD RID: 55981 RVA: 0x004E5B58 File Offset: 0x004E3D58
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

		// Token: 0x0400A53E RID: 42302
		public const string CacheFolderName = "RfsCache";

		// Token: 0x0400A53F RID: 42303
		[PublicizedFrom(EAccessModifier.Private)]
		public const int MaxConcurrentReads = 16;

		// Token: 0x0400A540 RID: 42304
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A541 RID: 42305
		[PublicizedFrom(EAccessModifier.Private)]
		public TitleStorageInterface titleStorageInterface;

		// Token: 0x0400A542 RID: 42306
		[PublicizedFrom(EAccessModifier.Private)]
		public object requestsLock = new object();

		// Token: 0x0400A543 RID: 42307
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<string, RemoteFileStorage.RequestDetails> requests = new Dictionary<string, RemoteFileStorage.RequestDetails>();

		// Token: 0x0400A544 RID: 42308
		[PublicizedFrom(EAccessModifier.Private)]
		public Queue<RemoteFileStorage.RequestDetails> requestQueue = new Queue<RemoteFileStorage.RequestDetails>();

		// Token: 0x0400A545 RID: 42309
		[PublicizedFrom(EAccessModifier.Private)]
		public int activeRequests;

		// Token: 0x02001CDB RID: 7387
		[PublicizedFrom(EAccessModifier.Private)]
		public class RequestDetails
		{
			// Token: 0x14000140 RID: 320
			// (add) Token: 0x0600DAAF RID: 55983 RVA: 0x004E5BFC File Offset: 0x004E3DFC
			// (remove) Token: 0x0600DAB0 RID: 55984 RVA: 0x004E5C34 File Offset: 0x004E3E34
			public event IRemoteFileStorage.FileDownloadCompleteCallback Callback;

			// Token: 0x0600DAB1 RID: 55985 RVA: 0x004E5C69 File Offset: 0x004E3E69
			public RequestDetails(string _filename, ProductUserId _localUserId, IRemoteFileStorage.FileDownloadCompleteCallback _callback)
			{
				this.Filename = _filename;
				this.LocalUserId = _localUserId;
				this.Callback = _callback;
			}

			// Token: 0x0600DAB2 RID: 55986 RVA: 0x004E5C91 File Offset: 0x004E3E91
			public void ExecuteCallback(IRemoteFileStorage.EFileDownloadResult _result, string _errorName, byte[] _data)
			{
				IRemoteFileStorage.FileDownloadCompleteCallback callback = this.Callback;
				if (callback == null)
				{
					return;
				}
				callback(_result, _errorName, _data);
			}

			// Token: 0x0400A546 RID: 42310
			public readonly string Filename;

			// Token: 0x0400A547 RID: 42311
			public readonly ProductUserId LocalUserId;

			// Token: 0x0400A549 RID: 42313
			public readonly List<ArraySegment<byte>> Chunks = new List<ArraySegment<byte>>();
		}
	}
}

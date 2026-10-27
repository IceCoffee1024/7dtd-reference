using System;
using System.Collections.Generic;
using System.IO;
using Epic.OnlineServices;
using Epic.OnlineServices.TitleStorage;

namespace Platform.EOS
{
	// Token: 0x02001D64 RID: 7524
	public class RemoteFileStorage : IRemoteFileStorage
	{
		// Token: 0x0600DEC5 RID: 57029 RVA: 0x004F4000 File Offset: 0x004F2200
		public void Init(IPlatform _owner)
		{
			this.owner = _owner;
			this.owner.Api.ClientApiInitialized += this.OnClientApiInitialized;
			if (this.owner.User != null)
			{
				this.owner.User.UserLoggedIn += this.OnUserLoggedIn;
			}
		}

		// Token: 0x0600DEC6 RID: 57030 RVA: 0x004F4059 File Offset: 0x004F2259
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnUserLoggedIn(IPlatform _obj)
		{
			if (this.IsReady)
			{
				this.clearCache();
			}
		}

		// Token: 0x0600DEC7 RID: 57031 RVA: 0x004F4069 File Offset: 0x004F2269
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnClientApiInitialized()
		{
			this.titleStorageInterface = ((Api)this.owner.Api).PlatformInterface.GetTitleStorageInterface();
		}

		// Token: 0x17001C62 RID: 7266
		// (get) Token: 0x0600DEC8 RID: 57032 RVA: 0x004F408C File Offset: 0x004F228C
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

		// Token: 0x17001C63 RID: 7267
		// (get) Token: 0x0600DEC9 RID: 57033 RVA: 0x004F40F4 File Offset: 0x004F22F4
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

		// Token: 0x0600DECA RID: 57034 RVA: 0x004F4168 File Offset: 0x004F2368
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

		// Token: 0x0600DECB RID: 57035 RVA: 0x004F4274 File Offset: 0x004F2474
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

		// Token: 0x0600DECC RID: 57036 RVA: 0x004F42F4 File Offset: 0x004F24F4
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

		// Token: 0x0600DECD RID: 57037 RVA: 0x004F43C0 File Offset: 0x004F25C0
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

		// Token: 0x0600DECE RID: 57038 RVA: 0x004F443C File Offset: 0x004F263C
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

		// Token: 0x0600DECF RID: 57039 RVA: 0x004F4544 File Offset: 0x004F2744
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

		// Token: 0x0600DED0 RID: 57040 RVA: 0x004F45E8 File Offset: 0x004F27E8
		[PublicizedFrom(EAccessModifier.Private)]
		public void fileTransferProgressCallback(ref FileTransferProgressCallbackInfo _callbackData)
		{
			RemoteFileStorage.RequestDetails requestDetails = (RemoteFileStorage.RequestDetails)_callbackData.ClientData;
			Log.Out(string.Format("[EOS] TransferProgress: {0}, {1} / {2}", _callbackData.Filename, _callbackData.BytesTransferred, _callbackData.TotalFileSizeBytes));
		}

		// Token: 0x0600DED1 RID: 57041 RVA: 0x004F4621 File Offset: 0x004F2821
		[PublicizedFrom(EAccessModifier.Private)]
		public ReadResult readFileDataCallback(ref ReadFileDataCallbackInfo _callbackData)
		{
			((RemoteFileStorage.RequestDetails)_callbackData.ClientData).Chunks.Add(_callbackData.DataChunk);
			return ReadResult.RrContinueReading;
		}

		// Token: 0x0600DED2 RID: 57042 RVA: 0x004F4640 File Offset: 0x004F2840
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

		// Token: 0x0600DED3 RID: 57043 RVA: 0x004F4820 File Offset: 0x004F2A20
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

		// Token: 0x17001C64 RID: 7268
		// (get) Token: 0x0600DED4 RID: 57044 RVA: 0x004F48E8 File Offset: 0x004F2AE8
		public string CacheFilePrefix
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return "Rfs_";
			}
		}

		// Token: 0x17001C65 RID: 7269
		// (get) Token: 0x0600DED5 RID: 57045 RVA: 0x004F48EF File Offset: 0x004F2AEF
		public string CacheFolder
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return GameIO.GetDeviceLocalUserGameDataDir() + "/RfsCache";
			}
		}

		// Token: 0x0600DED6 RID: 57046 RVA: 0x004F4900 File Offset: 0x004F2B00
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

		// Token: 0x0600DED7 RID: 57047 RVA: 0x004F4952 File Offset: 0x004F2B52
		[PublicizedFrom(EAccessModifier.Private)]
		public void cacheFile(string _filename, byte[] _data)
		{
			SdDirectory.CreateDirectory(this.CacheFolder);
			SdFile.WriteAllBytes(this.CacheFolder + "/" + this.CacheFilePrefix + _filename, _data);
		}

		// Token: 0x0600DED8 RID: 57048 RVA: 0x004F4980 File Offset: 0x004F2B80
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

		// Token: 0x0400A82F RID: 43055
		public const string CacheFolderName = "RfsCache";

		// Token: 0x0400A830 RID: 43056
		[PublicizedFrom(EAccessModifier.Private)]
		public const int MaxConcurrentReads = 16;

		// Token: 0x0400A831 RID: 43057
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A832 RID: 43058
		[PublicizedFrom(EAccessModifier.Private)]
		public TitleStorageInterface titleStorageInterface;

		// Token: 0x0400A833 RID: 43059
		[PublicizedFrom(EAccessModifier.Private)]
		public object requestsLock = new object();

		// Token: 0x0400A834 RID: 43060
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<string, RemoteFileStorage.RequestDetails> requests = new Dictionary<string, RemoteFileStorage.RequestDetails>();

		// Token: 0x0400A835 RID: 43061
		[PublicizedFrom(EAccessModifier.Private)]
		public Queue<RemoteFileStorage.RequestDetails> requestQueue = new Queue<RemoteFileStorage.RequestDetails>();

		// Token: 0x0400A836 RID: 43062
		[PublicizedFrom(EAccessModifier.Private)]
		public int activeRequests;

		// Token: 0x02001D65 RID: 7525
		[PublicizedFrom(EAccessModifier.Private)]
		public class RequestDetails
		{
			// Token: 0x14000140 RID: 320
			// (add) Token: 0x0600DEDA RID: 57050 RVA: 0x004F4A24 File Offset: 0x004F2C24
			// (remove) Token: 0x0600DEDB RID: 57051 RVA: 0x004F4A5C File Offset: 0x004F2C5C
			public event IRemoteFileStorage.FileDownloadCompleteCallback Callback;

			// Token: 0x0600DEDC RID: 57052 RVA: 0x004F4A91 File Offset: 0x004F2C91
			public RequestDetails(string _filename, ProductUserId _localUserId, IRemoteFileStorage.FileDownloadCompleteCallback _callback)
			{
				this.Filename = _filename;
				this.LocalUserId = _localUserId;
				this.Callback = _callback;
			}

			// Token: 0x0600DEDD RID: 57053 RVA: 0x004F4AB9 File Offset: 0x004F2CB9
			public void ExecuteCallback(IRemoteFileStorage.EFileDownloadResult _result, string _errorName, byte[] _data)
			{
				IRemoteFileStorage.FileDownloadCompleteCallback callback = this.Callback;
				if (callback == null)
				{
					return;
				}
				callback(_result, _errorName, _data);
			}

			// Token: 0x0400A837 RID: 43063
			public readonly string Filename;

			// Token: 0x0400A838 RID: 43064
			public readonly ProductUserId LocalUserId;

			// Token: 0x0400A83A RID: 43066
			public readonly List<ArraySegment<byte>> Chunks = new List<ArraySegment<byte>>();

			// Token: 0x0400A83B RID: 43067
			public TitleStorageFileTransferRequest Transfer;

			// Token: 0x0400A83C RID: 43068
			public bool Cancelled;
		}
	}
}

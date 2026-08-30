using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

// Token: 0x0200125A RID: 4698
public static class LoadManager
{
	// Token: 0x06009583 RID: 38275 RVA: 0x003869F0 File Offset: 0x00384BF0
	public static void Init()
	{
		LoadManager.forceLoadSync = false;
		LoadManager.rootGroup = new LoadManager.LoadGroup(null);
		LoadManager.loadRequests = new WorkBatch<LoadManager.LoadTask>();
		LoadManager.deferedLoadRequests = new List<LoadManager.LoadTask>();
		LoadManager.updateRequestAction = new Action<LoadManager.LoadTask>(LoadManager.UpdateRequest);
		Addressables.InitializeAsync().WaitForCompletion();
		LoadManager.addressablesCaseMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
		foreach (IResourceLocator resourceLocator in Addressables.ResourceLocators)
		{
			foreach (object obj in resourceLocator.Keys)
			{
				string text = obj as string;
				if (text != null && !LoadManager.addressablesCaseMap.TryAdd(text, text))
				{
					Log.Error("Error adding " + text + " to Addressables dictionary, case-insensitive duplicate found.");
				}
			}
		}
	}

	// Token: 0x06009584 RID: 38276 RVA: 0x00386AE8 File Offset: 0x00384CE8
	public static void InitSync()
	{
		LoadManager.Init();
		LoadManager.forceLoadSync = true;
	}

	// Token: 0x06009585 RID: 38277 RVA: 0x00386AF8 File Offset: 0x00384CF8
	public static void Update()
	{
		List<LoadManager.LoadTask> list = LoadManager.deferedLoadRequests;
		lock (list)
		{
			int num = LoadManager.loadRequests.Count();
			if (LoadManager.deferedLoadRequests.Count > 0 && num < 64)
			{
				int num2 = Mathf.Min(64 - num, LoadManager.deferedLoadRequests.Count);
				for (int i = 0; i < num2; i++)
				{
					LoadManager.AddTask(LoadManager.deferedLoadRequests[i].Group, LoadManager.deferedLoadRequests[i]);
				}
				LoadManager.deferedLoadRequests.RemoveRange(0, num2);
			}
		}
		LoadManager.loadRequests.DoWork(LoadManager.updateRequestAction);
	}

	// Token: 0x06009586 RID: 38278 RVA: 0x00386BB0 File Offset: 0x00384DB0
	[PublicizedFrom(EAccessModifier.Private)]
	public static void UpdateRequest(LoadManager.LoadTask _request)
	{
		if (_request.INTERNAL_IsPending)
		{
			_request.Update();
			LoadManager.loadRequests.Add(_request);
			return;
		}
		_request.Complete();
		_request.Group.DecrementPending();
	}

	// Token: 0x06009587 RID: 38279 RVA: 0x00386BDD File Offset: 0x00384DDD
	[PublicizedFrom(EAccessModifier.Private)]
	public static void RemoveRequest(LoadManager.LoadTask _request)
	{
		if (LoadManager.loadRequests.Remove(_request))
		{
			_request.Group.DecrementPending();
		}
	}

	// Token: 0x06009588 RID: 38280 RVA: 0x00386BF7 File Offset: 0x00384DF7
	public static void Destroy()
	{
		LoadManager.loadRequests.Clear();
	}

	// Token: 0x06009589 RID: 38281 RVA: 0x00386C03 File Offset: 0x00384E03
	public static LoadManager.LoadGroup CreateGroup()
	{
		return new LoadManager.LoadGroup(LoadManager.rootGroup);
	}

	// Token: 0x0600958A RID: 38282 RVA: 0x00386C0F File Offset: 0x00384E0F
	public static LoadManager.LoadGroup CreateGroup(LoadManager.LoadGroup _parent)
	{
		return new LoadManager.LoadGroup(_parent);
	}

	// Token: 0x0600958B RID: 38283 RVA: 0x00386C18 File Offset: 0x00384E18
	public static LoadManager.CoroutineTask AddTask(IEnumerator _task, LoadManager.CompletionCallback _callback = null, LoadManager.LoadGroup _lg = null)
	{
		if (_lg == null)
		{
			_lg = LoadManager.rootGroup;
		}
		_lg.IncrementPending();
		LoadManager.CoroutineTask coroutineTask = new LoadManager.CoroutineTask(_lg, _task, _callback);
		LoadManager.loadRequests.Add(coroutineTask);
		return coroutineTask;
	}

	// Token: 0x0600958C RID: 38284 RVA: 0x00386C4C File Offset: 0x00384E4C
	public static LoadManager.AssetRequestTask<T> LoadAsset<T>(DataLoader.DataPathIdentifier _identifier, Action<T> _callback = null, LoadManager.LoadGroup _lg = null, bool _deferLoading = false, bool _loadSync = false, bool _ignoreDlcEntitlements = false) where T : global::UnityEngine.Object
	{
		if (ThreadManager.IsInSyncCoroutine)
		{
			_loadSync = true;
		}
		if (_identifier.IsBundle)
		{
			AssetBundleManager.Instance.LoadAssetBundle(_identifier.BundlePath, _identifier.FromMod);
			return LoadManager.LoadAssetFromBundle<T>(_identifier, _callback, _lg, _deferLoading, _loadSync);
		}
		if (_identifier.Location == DataLoader.DataPathIdentifier.AssetLocation.Addressable)
		{
			return LoadManager.LoadAssetFromAddressables<T>(_identifier.AssetName, _callback, _lg, _deferLoading, _loadSync, _ignoreDlcEntitlements);
		}
		return LoadManager.LoadAssetFromResources<T>(_identifier.AssetName, _callback, _lg, _deferLoading, _loadSync);
	}

	// Token: 0x0600958D RID: 38285 RVA: 0x00386CBA File Offset: 0x00384EBA
	public static LoadManager.AssetRequestTask<T> LoadAsset<T>(string _uri, Action<T> _callback = null, LoadManager.LoadGroup _lg = null, bool _deferLoading = false, bool _loadSync = false, bool _ignoreDlcEntitlements = false) where T : global::UnityEngine.Object
	{
		return LoadManager.LoadAsset<T>(DataLoader.ParseDataPathIdentifier(_uri), _callback, _lg, _deferLoading, _loadSync, _ignoreDlcEntitlements);
	}

	// Token: 0x0600958E RID: 38286 RVA: 0x00386CCE File Offset: 0x00384ECE
	public static LoadManager.AssetRequestTask<T> LoadAsset<T>(string _bundlePath, string _assetName, Action<T> _callback = null, LoadManager.LoadGroup _lg = null, bool _deferLoading = false, bool _loadSync = false) where T : global::UnityEngine.Object
	{
		return LoadManager.LoadAsset<T>(new DataLoader.DataPathIdentifier(_assetName, _bundlePath, false), _callback, _lg, _deferLoading, _loadSync, false);
	}

	// Token: 0x0600958F RID: 38287 RVA: 0x00386CE4 File Offset: 0x00384EE4
	public static LoadManager.ResourceRequestTask<T> LoadAssetFromResources<T>(string _resourcePath, Action<T> _callback = null, LoadManager.LoadGroup _lg = null, bool _deferLoading = false, bool _loadSync = false) where T : global::UnityEngine.Object
	{
		if (_lg == null)
		{
			_lg = LoadManager.rootGroup;
		}
		if (ThreadManager.IsInSyncCoroutine && ThreadManager.IsMainThread())
		{
			_loadSync = true;
		}
		LoadManager.ResourceRequestTask<T> resourceRequestTask = new LoadManager.ResourceRequestTask<T>(_lg, !_loadSync, _resourcePath, _callback);
		LoadManager.addOrExecLoadTask(_lg, resourceRequestTask, _deferLoading, _loadSync);
		return resourceRequestTask;
	}

	// Token: 0x06009590 RID: 38288 RVA: 0x00386D28 File Offset: 0x00384F28
	[PublicizedFrom(EAccessModifier.Private)]
	public static LoadManager.AssetBundleRequestTask<T> LoadAssetFromBundle<T>(DataLoader.DataPathIdentifier _identifier, Action<T> _callback = null, LoadManager.LoadGroup _lg = null, bool _deferLoading = false, bool _loadSync = false) where T : global::UnityEngine.Object
	{
		if (_lg == null)
		{
			_lg = LoadManager.rootGroup;
		}
		LoadManager.AssetBundleRequestTask<T> assetBundleRequestTask = new LoadManager.AssetBundleRequestTask<T>(_lg, !_loadSync, _identifier, _callback);
		LoadManager.addOrExecLoadTask(_lg, assetBundleRequestTask, _deferLoading, _loadSync);
		return assetBundleRequestTask;
	}

	// Token: 0x06009591 RID: 38289 RVA: 0x00386D58 File Offset: 0x00384F58
	public static LoadManager.AddressableRequestTask<T> LoadAssetFromAddressables<T>(object _key, Action<T> _callback = null, LoadManager.LoadGroup _lg = null, bool _deferLoading = false, bool _loadSync = false, bool _ignoreDlcEntitlements = false) where T : global::UnityEngine.Object
	{
		if (_lg == null)
		{
			_lg = LoadManager.rootGroup;
		}
		if (!_ignoreDlcEntitlements && !EntitlementManager.Instance.HasEntitlement(_key))
		{
			Log.Error(string.Format("Tried to load asset without proper DLC entitlement. Missing {0}", EntitlementManager.Instance.GetSetForAsset(_key)));
			return LoadManager.AddressableRequestTask<T>.Empty();
		}
		LoadManager.LoadGroup loadGroup = _lg;
		bool flag = !_loadSync;
		string text = _key as string;
		LoadManager.AddressableRequestTask<T> addressableRequestTask = new LoadManager.AddressableRequestTask<T>(loadGroup, flag, (text != null) ? LoadManager.GetAddressablesCase(text) : _key, _callback);
		LoadManager.addOrExecLoadTask(_lg, addressableRequestTask, _deferLoading, _loadSync);
		return addressableRequestTask;
	}

	// Token: 0x06009592 RID: 38290 RVA: 0x00386DD0 File Offset: 0x00384FD0
	public static LoadManager.AddressableRequestTask<T> LoadAssetFromAddressables<T>(string _folderAddress, string _assetPath, Action<T> _callback = null, LoadManager.LoadGroup _lg = null, bool _deferLoading = false, bool _loadSync = false, bool _ignoreDlcEntitlements = false) where T : global::UnityEngine.Object
	{
		return LoadManager.LoadAssetFromAddressables<T>(_folderAddress + "/" + _assetPath, _callback, _lg, _deferLoading, _loadSync, _ignoreDlcEntitlements);
	}

	// Token: 0x06009593 RID: 38291 RVA: 0x00386DEC File Offset: 0x00384FEC
	public static LoadManager.AddressableAssetsRequestTask<T> LoadAssetsFromAddressables<T>(string _label, Func<string, bool> _addressFilter = null, LoadManager.LoadGroup _lg = null, bool _deferLoading = false, bool _loadSync = false) where T : global::UnityEngine.Object
	{
		if (_lg == null)
		{
			_lg = LoadManager.rootGroup;
		}
		LoadManager.AddressableAssetsRequestTask<T> addressableAssetsRequestTask = new LoadManager.AddressableAssetsRequestTask<T>(_lg, !_loadSync, _label, _addressFilter);
		LoadManager.addOrExecLoadTask(_lg, addressableAssetsRequestTask, _deferLoading, _loadSync);
		return addressableAssetsRequestTask;
	}

	// Token: 0x06009594 RID: 38292 RVA: 0x00386E1C File Offset: 0x0038501C
	public static void ReleaseAddressable<T>(T _obj)
	{
		Addressables.Release<T>(_obj);
	}

	// Token: 0x06009595 RID: 38293 RVA: 0x00386E24 File Offset: 0x00385024
	public static LoadManager.FileLoadTask LoadFile(string _path, LoadManager.FileLoadCallback _callback = null, LoadManager.LoadGroup _lg = null, bool _deferLoading = false, bool _loadSync = false)
	{
		if (_lg == null)
		{
			_lg = LoadManager.rootGroup;
		}
		LoadManager.FileLoadTask fileLoadTask = new LoadManager.FileLoadTask(_lg, !_loadSync, _path, _callback);
		LoadManager.addOrExecLoadTask(_lg, fileLoadTask, _deferLoading, _loadSync);
		return fileLoadTask;
	}

	// Token: 0x06009596 RID: 38294 RVA: 0x00386E54 File Offset: 0x00385054
	public static IEnumerator WaitAll(IEnumerable<LoadManager.LoadTask> _tasks)
	{
		foreach (LoadManager.LoadTask task in _tasks)
		{
			while (!task.IsDone)
			{
				yield return null;
			}
			task = null;
		}
		IEnumerator<LoadManager.LoadTask> enumerator = null;
		yield break;
		yield break;
	}

	// Token: 0x06009597 RID: 38295 RVA: 0x00386E64 File Offset: 0x00385064
	[PublicizedFrom(EAccessModifier.Private)]
	public static void addOrExecLoadTask(LoadManager.LoadGroup _lg, LoadManager.LoadTask _task, bool _deferLoading = false, bool _loadSync = false)
	{
		if (!LoadManager.forceLoadSync && !_loadSync)
		{
			_lg.IncrementPending();
			if (!_deferLoading)
			{
				LoadManager.AddTask(_lg, _task);
				return;
			}
			List<LoadManager.LoadTask> list = LoadManager.deferedLoadRequests;
			lock (list)
			{
				LoadManager.deferedLoadRequests.Add(_task);
				return;
			}
		}
		_task.LoadSync();
	}

	// Token: 0x06009598 RID: 38296 RVA: 0x00386ECC File Offset: 0x003850CC
	[PublicizedFrom(EAccessModifier.Private)]
	public static void AddTask(LoadManager.LoadGroup _lg, LoadManager.LoadTask _task)
	{
		if (GameManager.IsDedicatedServer)
		{
			_task.LoadSync();
			_lg.DecrementPending();
			return;
		}
		if (_task.Load())
		{
			LoadManager.loadRequests.Add(_task);
			return;
		}
		_lg.DecrementPending();
		_task.Complete();
	}

	// Token: 0x06009599 RID: 38297 RVA: 0x00386F04 File Offset: 0x00385104
	public static string GetAddressablesCase(string key)
	{
		if (LoadManager.addressablesCaseMap == null)
		{
			Log.Error("Addressables Case Map not initialised - are you calling GetAddressablesCase before LoadManager.Init?");
		}
		if (string.IsNullOrEmpty(key))
		{
			return key;
		}
		string text;
		if (LoadManager.addressablesCaseMap.TryGetValue(key, out text))
		{
			return text;
		}
		return key;
	}

	// Token: 0x04007038 RID: 28728
	[PublicizedFrom(EAccessModifier.Private)]
	public static Action<LoadManager.LoadTask> updateRequestAction;

	// Token: 0x04007039 RID: 28729
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<string, string> addressablesCaseMap;

	// Token: 0x0400703A RID: 28730
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool forceLoadSync;

	// Token: 0x0400703B RID: 28731
	[PublicizedFrom(EAccessModifier.Private)]
	public static LoadManager.LoadGroup rootGroup;

	// Token: 0x0400703C RID: 28732
	[PublicizedFrom(EAccessModifier.Private)]
	public static WorkBatch<LoadManager.LoadTask> loadRequests;

	// Token: 0x0400703D RID: 28733
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<LoadManager.LoadTask> deferedLoadRequests;

	// Token: 0x0200125B RID: 4699
	// (Invoke) Token: 0x0600959B RID: 38299
	public delegate void CompletionCallback();

	// Token: 0x0200125C RID: 4700
	// (Invoke) Token: 0x0600959F RID: 38303
	public delegate void FileLoadCallback(byte[] _content);

	// Token: 0x0200125D RID: 4701
	public class LoadGroup
	{
		// Token: 0x170011BD RID: 4541
		// (get) Token: 0x060095A2 RID: 38306 RVA: 0x00386F3E File Offset: 0x0038513E
		public bool Pending
		{
			get
			{
				return Interlocked.CompareExchange(ref this.pending, 0, 0) != 0;
			}
		}

		// Token: 0x060095A3 RID: 38307 RVA: 0x00386F50 File Offset: 0x00385150
		[PublicizedFrom(EAccessModifier.Internal)]
		public LoadGroup(LoadManager.LoadGroup _parent)
		{
			this.parent = _parent;
			this.pending = 0;
		}

		// Token: 0x060095A4 RID: 38308 RVA: 0x00386F68 File Offset: 0x00385168
		[PublicizedFrom(EAccessModifier.Internal)]
		public void IncrementPending()
		{
			if (GameManager.IsDedicatedServer)
			{
				return;
			}
			for (LoadManager.LoadGroup loadGroup = this; loadGroup != null; loadGroup = loadGroup.parent)
			{
				Interlocked.Increment(ref loadGroup.pending);
			}
		}

		// Token: 0x060095A5 RID: 38309 RVA: 0x00386F98 File Offset: 0x00385198
		[PublicizedFrom(EAccessModifier.Internal)]
		public void DecrementPending()
		{
			if (GameManager.IsDedicatedServer)
			{
				return;
			}
			for (LoadManager.LoadGroup loadGroup = this; loadGroup != null; loadGroup = loadGroup.parent)
			{
				Interlocked.Decrement(ref loadGroup.pending);
			}
		}

		// Token: 0x0400703E RID: 28734
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly LoadManager.LoadGroup parent;

		// Token: 0x0400703F RID: 28735
		[PublicizedFrom(EAccessModifier.Private)]
		public int pending;
	}

	// Token: 0x0200125E RID: 4702
	public abstract class LoadTask : CustomYieldInstruction
	{
		// Token: 0x060095A6 RID: 38310 RVA: 0x00386FC7 File Offset: 0x003851C7
		[PublicizedFrom(EAccessModifier.Protected)]
		public LoadTask(LoadManager.LoadGroup _group, bool _loadAsync)
		{
			this.group = _group;
			this.loadAsync = _loadAsync;
		}

		// Token: 0x170011BE RID: 4542
		// (get) Token: 0x060095A7 RID: 38311
		public abstract bool IsDone { get; }

		// Token: 0x170011BF RID: 4543
		// (get) Token: 0x060095A8 RID: 38312
		public abstract bool INTERNAL_IsPending { get; }

		// Token: 0x170011C0 RID: 4544
		// (get) Token: 0x060095A9 RID: 38313 RVA: 0x00386FDD File Offset: 0x003851DD
		public override bool keepWaiting
		{
			get
			{
				return this.INTERNAL_IsPending;
			}
		}

		// Token: 0x170011C1 RID: 4545
		// (get) Token: 0x060095AA RID: 38314 RVA: 0x00386FE5 File Offset: 0x003851E5
		public LoadManager.LoadGroup Group
		{
			get
			{
				return this.group;
			}
		}

		// Token: 0x060095AB RID: 38315
		public abstract bool Load();

		// Token: 0x060095AC RID: 38316
		public abstract void LoadSync();

		// Token: 0x060095AD RID: 38317 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void Update()
		{
		}

		// Token: 0x060095AE RID: 38318
		public abstract void Complete();

		// Token: 0x060095AF RID: 38319 RVA: 0x00386FED File Offset: 0x003851ED
		public void WaitForComplete()
		{
			this.CompleteNow();
			LoadManager.RemoveRequest(this);
		}

		// Token: 0x060095B0 RID: 38320
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract void CompleteNow();

		// Token: 0x04007040 RID: 28736
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly LoadManager.LoadGroup group;

		// Token: 0x04007041 RID: 28737
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly bool loadAsync;

		// Token: 0x04007042 RID: 28738
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool loadStarted;
	}

	// Token: 0x0200125F RID: 4703
	public class AssetBundleLoadTask : LoadManager.LoadTask
	{
		// Token: 0x170011C2 RID: 4546
		// (get) Token: 0x060095B1 RID: 38321 RVA: 0x00020021 File Offset: 0x0001E221
		public override bool INTERNAL_IsPending
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170011C3 RID: 4547
		// (get) Token: 0x060095B2 RID: 38322 RVA: 0x00010E62 File Offset: 0x0000F062
		public override bool IsDone
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060095B3 RID: 38323 RVA: 0x00386FFB File Offset: 0x003851FB
		public AssetBundleLoadTask(LoadManager.LoadGroup _group, bool _loadAsync, Action<AssetBundle> _callback)
			: base(_group, _loadAsync)
		{
			this.callback = _callback;
		}

		// Token: 0x060095B4 RID: 38324 RVA: 0x00087DB8 File Offset: 0x00085FB8
		public override bool Load()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060095B5 RID: 38325 RVA: 0x00087DB8 File Offset: 0x00085FB8
		public override void LoadSync()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060095B6 RID: 38326 RVA: 0x00087DB8 File Offset: 0x00085FB8
		public override void Complete()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060095B7 RID: 38327 RVA: 0x00087DB8 File Offset: 0x00085FB8
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void CompleteNow()
		{
			throw new NotImplementedException();
		}

		// Token: 0x04007043 RID: 28739
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Action<AssetBundle> callback;
	}

	// Token: 0x02001260 RID: 4704
	public abstract class AssetRequestTask<T> : LoadManager.LoadTask where T : global::UnityEngine.Object
	{
		// Token: 0x170011C4 RID: 4548
		// (get) Token: 0x060095B8 RID: 38328 RVA: 0x0038700C File Offset: 0x0038520C
		public override bool keepWaiting
		{
			get
			{
				return this.INTERNAL_IsPending || !this.assetRetrieved;
			}
		}

		// Token: 0x170011C5 RID: 4549
		// (get) Token: 0x060095B9 RID: 38329 RVA: 0x00387021 File Offset: 0x00385221
		public override bool IsDone
		{
			get
			{
				return this.assetRetrieved;
			}
		}

		// Token: 0x170011C6 RID: 4550
		// (get) Token: 0x060095BA RID: 38330 RVA: 0x00387029 File Offset: 0x00385229
		public T Asset
		{
			get
			{
				return this.asset;
			}
		}

		// Token: 0x060095BB RID: 38331 RVA: 0x00387031 File Offset: 0x00385231
		[PublicizedFrom(EAccessModifier.Protected)]
		public AssetRequestTask(LoadManager.LoadGroup _group, bool _loadAsync, Action<T> _callback)
			: base(_group, _loadAsync)
		{
			this.callback = _callback;
		}

		// Token: 0x060095BC RID: 38332 RVA: 0x00387042 File Offset: 0x00385242
		public virtual LoadManager.IAssetHandle GetHandle()
		{
			return LoadManager.DummyHandle.Instance;
		}

		// Token: 0x060095BD RID: 38333 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void Release()
		{
		}

		// Token: 0x04007044 RID: 28740
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly Action<T> callback;

		// Token: 0x04007045 RID: 28741
		[PublicizedFrom(EAccessModifier.Protected)]
		public T asset;

		// Token: 0x04007046 RID: 28742
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool assetRetrieved;
	}

	// Token: 0x02001261 RID: 4705
	public abstract class AssetsRequestTask<T> : LoadManager.LoadTask where T : global::UnityEngine.Object
	{
		// Token: 0x170011C7 RID: 4551
		// (get) Token: 0x060095BE RID: 38334 RVA: 0x00387049 File Offset: 0x00385249
		public override bool keepWaiting
		{
			get
			{
				return this.INTERNAL_IsPending || !this.assetsRetrieved;
			}
		}

		// Token: 0x170011C8 RID: 4552
		// (get) Token: 0x060095BF RID: 38335 RVA: 0x0038705E File Offset: 0x0038525E
		public override bool IsDone
		{
			get
			{
				return this.assetsRetrieved;
			}
		}

		// Token: 0x060095C0 RID: 38336 RVA: 0x00387066 File Offset: 0x00385266
		[PublicizedFrom(EAccessModifier.Protected)]
		public AssetsRequestTask(LoadManager.LoadGroup _group, bool _loadAsync)
			: base(_group, _loadAsync)
		{
		}

		// Token: 0x060095C1 RID: 38337
		public abstract void CollectResults(List<T> _results);

		// Token: 0x04007047 RID: 28743
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool assetsRetrieved;
	}

	// Token: 0x02001262 RID: 4706
	public class ResourceRequestTask<T> : LoadManager.AssetRequestTask<T> where T : global::UnityEngine.Object
	{
		// Token: 0x170011C9 RID: 4553
		// (get) Token: 0x060095C2 RID: 38338 RVA: 0x00387070 File Offset: 0x00385270
		public override bool INTERNAL_IsPending
		{
			get
			{
				return this.loadAsync && !this.asyncCancelled && (!this.loadStarted || (this.request != null && !this.request.isDone));
			}
		}

		// Token: 0x060095C3 RID: 38339 RVA: 0x003870A6 File Offset: 0x003852A6
		public ResourceRequestTask(LoadManager.LoadGroup _group, bool _loadAsync, string _assetPath, Action<T> _callback)
			: base(_group, _loadAsync, _callback)
		{
			this.assetPath = _assetPath;
		}

		// Token: 0x060095C4 RID: 38340 RVA: 0x003870B9 File Offset: 0x003852B9
		public override bool Load()
		{
			this.request = Resources.LoadAsync<T>(this.assetPath);
			this.loadStarted = true;
			return this.request != null;
		}

		// Token: 0x060095C5 RID: 38341 RVA: 0x003870DC File Offset: 0x003852DC
		public override void LoadSync()
		{
			this.asset = Resources.Load<T>(this.assetPath);
			this.assetRetrieved = true;
			if (this.callback != null)
			{
				this.callback(this.asset);
			}
		}

		// Token: 0x060095C6 RID: 38342 RVA: 0x00387110 File Offset: 0x00385310
		public override void Complete()
		{
			if (this.INTERNAL_IsPending)
			{
				throw new Exception("ResourceRequestTask still pending.");
			}
			this.asset = this.request.asset as T;
			this.assetRetrieved = true;
			if (this.callback == null)
			{
				return;
			}
			this.callback(this.asset);
		}

		// Token: 0x060095C7 RID: 38343 RVA: 0x0038716C File Offset: 0x0038536C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void CompleteNow()
		{
			if (this.loadAsync)
			{
				if (this.assetRetrieved)
				{
					throw new Exception("ResourceRequestTask async already completed.");
				}
				if (this.request != null)
				{
					Log.Warning("ResourceRequestTask async WaitForComplete is not supported, asset " + this.assetPath + " will be loaded again sync");
				}
				this.asyncCancelled = true;
				this.LoadSync();
			}
		}

		// Token: 0x04007048 RID: 28744
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string assetPath;

		// Token: 0x04007049 RID: 28745
		[PublicizedFrom(EAccessModifier.Private)]
		public ResourceRequest request;

		// Token: 0x0400704A RID: 28746
		[PublicizedFrom(EAccessModifier.Private)]
		public bool asyncCancelled;
	}

	// Token: 0x02001263 RID: 4707
	public class AssetBundleRequestTask<T> : LoadManager.AssetRequestTask<T> where T : global::UnityEngine.Object
	{
		// Token: 0x170011CA RID: 4554
		// (get) Token: 0x060095C8 RID: 38344 RVA: 0x003871C3 File Offset: 0x003853C3
		public override bool INTERNAL_IsPending
		{
			get
			{
				return this.loadAsync && !this.asyncCancelled && (!this.loadStarted || (this.request != null && !this.request.IsDone));
			}
		}

		// Token: 0x060095C9 RID: 38345 RVA: 0x003871F9 File Offset: 0x003853F9
		public AssetBundleRequestTask(LoadManager.LoadGroup _group, bool _loadAsync, DataLoader.DataPathIdentifier _identifier, Action<T> _callback)
			: base(_group, _loadAsync, _callback)
		{
			this.identifier = _identifier;
			if (typeof(T) == typeof(Transform))
			{
				this.isGameObject = true;
			}
		}

		// Token: 0x060095CA RID: 38346 RVA: 0x00387230 File Offset: 0x00385430
		public override bool Load()
		{
			if (this.isGameObject)
			{
				this.request = AssetBundleManager.Instance.GetAsync<GameObject>(this.identifier.BundlePath, this.identifier.AssetName, this.identifier.FromMod);
			}
			else
			{
				this.request = AssetBundleManager.Instance.GetAsync<T>(this.identifier.BundlePath, this.identifier.AssetName, this.identifier.FromMod);
			}
			this.loadStarted = true;
			return this.request != null;
		}

		// Token: 0x060095CB RID: 38347 RVA: 0x003872BC File Offset: 0x003854BC
		public override void LoadSync()
		{
			if (this.isGameObject)
			{
				GameObject gameObject = AssetBundleManager.Instance.Get<GameObject>(this.identifier.BundlePath, this.identifier.AssetName, this.identifier.FromMod);
				if (gameObject != null)
				{
					this.asset = gameObject.GetComponent<T>();
				}
			}
			else
			{
				this.asset = AssetBundleManager.Instance.Get<T>(this.identifier.BundlePath, this.identifier.AssetName, this.identifier.FromMod);
			}
			this.assetRetrieved = true;
			if (this.callback != null)
			{
				this.callback(this.asset);
			}
		}

		// Token: 0x060095CC RID: 38348 RVA: 0x00387368 File Offset: 0x00385568
		public override void Complete()
		{
			if (this.INTERNAL_IsPending)
			{
				throw new Exception("AssetBundleRequestTask still pending.");
			}
			if (this.isGameObject)
			{
				GameObject gameObject = this.request.Asset as GameObject;
				if (gameObject != null)
				{
					this.asset = gameObject.GetComponent<T>();
				}
			}
			else
			{
				this.asset = this.request.Asset as T;
			}
			this.assetRetrieved = true;
			if (this.callback == null)
			{
				return;
			}
			this.callback(this.asset);
		}

		// Token: 0x060095CD RID: 38349 RVA: 0x003873F4 File Offset: 0x003855F4
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void CompleteNow()
		{
			if (this.loadAsync)
			{
				if (this.assetRetrieved)
				{
					throw new Exception("AssetBundleRequestTask async already completed.");
				}
				if (this.request != null)
				{
					Log.Warning(string.Concat(new string[]
					{
						"AssetBundleRequestTask async WaitForComplete is not supported, asset ",
						this.identifier.AssetName,
						" from ",
						this.identifier.BundlePath,
						" will be loaded again sync"
					}));
				}
				this.asyncCancelled = true;
				this.LoadSync();
			}
		}

		// Token: 0x0400704B RID: 28747
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly DataLoader.DataPathIdentifier identifier;

		// Token: 0x0400704C RID: 28748
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool isGameObject;

		// Token: 0x0400704D RID: 28749
		[PublicizedFrom(EAccessModifier.Private)]
		public AssetBundleManager.AssetBundleRequestTFP request;

		// Token: 0x0400704E RID: 28750
		[PublicizedFrom(EAccessModifier.Private)]
		public bool asyncCancelled;
	}

	// Token: 0x02001264 RID: 4708
	public class AddressableRequestTask<T> : LoadManager.AssetRequestTask<T> where T : global::UnityEngine.Object
	{
		// Token: 0x170011CB RID: 4555
		// (get) Token: 0x060095CE RID: 38350 RVA: 0x00387478 File Offset: 0x00385678
		public override bool INTERNAL_IsPending
		{
			get
			{
				return this.loadAsync && (!this.loadStarted || (this.request.IsValid() && !this.request.IsDone) || (this.gameObjectRequest.IsValid() && !this.gameObjectRequest.IsDone));
			}
		}

		// Token: 0x060095CF RID: 38351 RVA: 0x003874D0 File Offset: 0x003856D0
		public AddressableRequestTask(LoadManager.LoadGroup _group, bool _loadAsync, object _key, Action<T> _callback)
			: base(_group, _loadAsync, _callback)
		{
			this.key = _key;
			if (typeof(T) == typeof(Transform))
			{
				this.isGameObject = true;
			}
		}

		// Token: 0x060095D0 RID: 38352 RVA: 0x00387505 File Offset: 0x00385705
		[PublicizedFrom(EAccessModifier.Private)]
		public void StartRequest()
		{
			if (this.loadStarted)
			{
				return;
			}
			if (this.isGameObject)
			{
				this.gameObjectRequest = Addressables.LoadAssetAsync<GameObject>(this.key);
			}
			else
			{
				this.request = Addressables.LoadAssetAsync<T>(this.key);
			}
			this.loadStarted = true;
		}

		// Token: 0x060095D1 RID: 38353 RVA: 0x00387543 File Offset: 0x00385743
		public override bool Load()
		{
			this.StartRequest();
			return this.request.IsValid() || this.gameObjectRequest.IsValid();
		}

		// Token: 0x060095D2 RID: 38354 RVA: 0x00387568 File Offset: 0x00385768
		public override void LoadSync()
		{
			this.StartRequest();
			if (this.isGameObject)
			{
				this.gameObjectRequest.WaitForCompletion();
				GameObject result = this.gameObjectRequest.Result;
				if (result != null)
				{
					this.asset = result.GetComponent<T>();
				}
			}
			else
			{
				this.request.WaitForCompletion();
				this.asset = this.request.Result;
			}
			this.assetRetrieved = true;
			if (this.callback != null)
			{
				this.callback(this.asset);
			}
		}

		// Token: 0x060095D3 RID: 38355 RVA: 0x003875F0 File Offset: 0x003857F0
		public override void Complete()
		{
			if (this.INTERNAL_IsPending)
			{
				throw new Exception("AddressableRequestTask still pending.");
			}
			if (this.isGameObject)
			{
				GameObject result = this.gameObjectRequest.Result;
				if (result != null)
				{
					this.asset = result.GetComponent<T>();
				}
			}
			else
			{
				this.asset = this.request.Result;
			}
			this.assetRetrieved = true;
			if (this.callback == null)
			{
				return;
			}
			this.callback(this.asset);
		}

		// Token: 0x060095D4 RID: 38356 RVA: 0x0038766D File Offset: 0x0038586D
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void CompleteNow()
		{
			if (this.loadAsync)
			{
				if (this.assetRetrieved)
				{
					throw new Exception("AddressableRequestTask async already completed.");
				}
				this.LoadSync();
			}
		}

		// Token: 0x060095D5 RID: 38357 RVA: 0x00387690 File Offset: 0x00385890
		public override LoadManager.IAssetHandle GetHandle()
		{
			if (this.isGameObject)
			{
				return new LoadManager.AddressableAssetHandle<GameObject>(this.gameObjectRequest);
			}
			return new LoadManager.AddressableAssetHandle<T>(this.request);
		}

		// Token: 0x060095D6 RID: 38358 RVA: 0x003876B1 File Offset: 0x003858B1
		public override void Release()
		{
			this.asset = default(T);
			if (this.isGameObject)
			{
				Addressables.Release<GameObject>(this.gameObjectRequest);
				return;
			}
			Addressables.Release<T>(this.request);
		}

		// Token: 0x060095D7 RID: 38359 RVA: 0x003876DE File Offset: 0x003858DE
		public static LoadManager.AddressableRequestTask<T> Empty()
		{
			return LoadManager.AddressableRequestTask<T>.emptyInstance;
		}

		// Token: 0x0400704F RID: 28751
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly object key;

		// Token: 0x04007050 RID: 28752
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool isGameObject;

		// Token: 0x04007051 RID: 28753
		[PublicizedFrom(EAccessModifier.Private)]
		public AsyncOperationHandle<T> request;

		// Token: 0x04007052 RID: 28754
		[PublicizedFrom(EAccessModifier.Private)]
		public AsyncOperationHandle<GameObject> gameObjectRequest;

		// Token: 0x04007053 RID: 28755
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly LoadManager.EmptyAddressableRequestTask<T> emptyInstance = new LoadManager.EmptyAddressableRequestTask<T>();
	}

	// Token: 0x02001265 RID: 4709
	[PublicizedFrom(EAccessModifier.Private)]
	public class EmptyAddressableRequestTask<T> : LoadManager.AddressableRequestTask<T> where T : global::UnityEngine.Object
	{
		// Token: 0x060095D9 RID: 38361 RVA: 0x003876F1 File Offset: 0x003858F1
		public EmptyAddressableRequestTask()
			: base(null, true, null, null)
		{
		}

		// Token: 0x170011CC RID: 4556
		// (get) Token: 0x060095DA RID: 38362 RVA: 0x00010E62 File Offset: 0x0000F062
		public override bool INTERNAL_IsPending
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170011CD RID: 4557
		// (get) Token: 0x060095DB RID: 38363 RVA: 0x00020021 File Offset: 0x0001E221
		public override bool IsDone
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060095DC RID: 38364 RVA: 0x00010E62 File Offset: 0x0000F062
		public override bool Load()
		{
			return false;
		}

		// Token: 0x060095DD RID: 38365 RVA: 0x000027FC File Offset: 0x000009FC
		public override void LoadSync()
		{
		}

		// Token: 0x060095DE RID: 38366 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Complete()
		{
		}

		// Token: 0x060095DF RID: 38367 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Release()
		{
		}

		// Token: 0x060095E0 RID: 38368 RVA: 0x00387042 File Offset: 0x00385242
		public override LoadManager.IAssetHandle GetHandle()
		{
			return LoadManager.DummyHandle.Instance;
		}
	}

	// Token: 0x02001266 RID: 4710
	public class AddressableAssetsRequestTask<T> : LoadManager.AssetsRequestTask<T> where T : global::UnityEngine.Object
	{
		// Token: 0x170011CE RID: 4558
		// (get) Token: 0x060095E1 RID: 38369 RVA: 0x003876FD File Offset: 0x003858FD
		public override bool INTERNAL_IsPending
		{
			get
			{
				return this.loadAsync && (!this.loadStarted || !this.assetRequestStarted || (this.assetsRequest.IsValid() && !this.assetsRequest.IsDone));
			}
		}

		// Token: 0x060095E2 RID: 38370 RVA: 0x00387738 File Offset: 0x00385938
		public AddressableAssetsRequestTask(LoadManager.LoadGroup _group, bool _loadAsync, string _label, Func<string, bool> _addressFilter = null)
			: base(_group, _loadAsync)
		{
			this.label = _label;
			this.addressFilter = _addressFilter;
		}

		// Token: 0x060095E3 RID: 38371 RVA: 0x00387751 File Offset: 0x00385951
		[PublicizedFrom(EAccessModifier.Private)]
		public void StartLocationsRequest()
		{
			if (this.loadStarted)
			{
				return;
			}
			this.loadStarted = true;
			this.locationRequest = Addressables.LoadResourceLocationsAsync(this.label, typeof(T));
		}

		// Token: 0x060095E4 RID: 38372 RVA: 0x00387780 File Offset: 0x00385980
		[PublicizedFrom(EAccessModifier.Private)]
		public void StartAssetsRequest()
		{
			if (this.assetRequestStarted)
			{
				return;
			}
			IList<IResourceLocation> list;
			if (this.addressFilter != null)
			{
				list = new List<IResourceLocation>();
				using (IEnumerator<IResourceLocation> enumerator = this.locationRequest.Result.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IResourceLocation resourceLocation = enumerator.Current;
						if (this.addressFilter(resourceLocation.PrimaryKey))
						{
							list.Add(resourceLocation);
						}
					}
					goto IL_006B;
				}
			}
			list = this.locationRequest.Result;
			IL_006B:
			if (list.Count > 0)
			{
				this.assetsRequest = Addressables.LoadAssetsAsync<T>(list, null);
			}
			this.assetRequestStarted = true;
			if (!this.assetsRequest.IsValid() || this.assetsRequest.IsDone)
			{
				this.Complete();
			}
		}

		// Token: 0x060095E5 RID: 38373 RVA: 0x00387848 File Offset: 0x00385A48
		public override void Update()
		{
			base.Update();
			if (!this.loadAsync)
			{
				return;
			}
			if (this.locationRequest.IsValid() && this.locationRequest.IsDone)
			{
				this.StartAssetsRequest();
			}
		}

		// Token: 0x060095E6 RID: 38374 RVA: 0x00387879 File Offset: 0x00385A79
		public override bool Load()
		{
			this.StartLocationsRequest();
			return this.locationRequest.IsValid();
		}

		// Token: 0x060095E7 RID: 38375 RVA: 0x0038788C File Offset: 0x00385A8C
		public override void LoadSync()
		{
			this.StartLocationsRequest();
			if (this.locationRequest.IsValid())
			{
				this.locationRequest.WaitForCompletion();
				this.StartAssetsRequest();
				if (this.assetsRequest.IsValid())
				{
					this.assetsRequest.WaitForCompletion();
				}
			}
			this.Complete();
		}

		// Token: 0x060095E8 RID: 38376 RVA: 0x003878DD File Offset: 0x00385ADD
		public override void Complete()
		{
			if (this.INTERNAL_IsPending)
			{
				throw new Exception("AddressableAssetsRequestTask still pending.");
			}
			this.assetsRetrieved = true;
		}

		// Token: 0x060095E9 RID: 38377 RVA: 0x003878F9 File Offset: 0x00385AF9
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void CompleteNow()
		{
			if (this.loadAsync)
			{
				if (this.assetsRetrieved)
				{
					throw new Exception("AddressableAssetsRequestTask async already completed.");
				}
				this.LoadSync();
			}
		}

		// Token: 0x060095EA RID: 38378 RVA: 0x0038791C File Offset: 0x00385B1C
		public override void CollectResults(List<T> _results)
		{
			if (!this.assetsRetrieved)
			{
				Log.Warning("Collecting Addressable assets request results before operation has completed");
			}
			if (this.assetsRequest.IsValid() && this.assetsRetrieved)
			{
				foreach (T t in this.assetsRequest.Result)
				{
					_results.Add(t);
				}
			}
		}

		// Token: 0x04007054 RID: 28756
		[PublicizedFrom(EAccessModifier.Private)]
		public string label;

		// Token: 0x04007055 RID: 28757
		[PublicizedFrom(EAccessModifier.Private)]
		public Func<string, bool> addressFilter;

		// Token: 0x04007056 RID: 28758
		[PublicizedFrom(EAccessModifier.Private)]
		public AsyncOperationHandle<IList<IResourceLocation>> locationRequest;

		// Token: 0x04007057 RID: 28759
		[PublicizedFrom(EAccessModifier.Private)]
		public bool assetRequestStarted;

		// Token: 0x04007058 RID: 28760
		[PublicizedFrom(EAccessModifier.Private)]
		public AsyncOperationHandle<IList<T>> assetsRequest;
	}

	// Token: 0x02001267 RID: 4711
	public class CoroutineTask : LoadManager.LoadTask
	{
		// Token: 0x170011CF RID: 4559
		// (get) Token: 0x060095EB RID: 38379 RVA: 0x00387998 File Offset: 0x00385B98
		public override bool INTERNAL_IsPending
		{
			get
			{
				return !this.isDone;
			}
		}

		// Token: 0x170011D0 RID: 4560
		// (get) Token: 0x060095EC RID: 38380 RVA: 0x003879A3 File Offset: 0x00385BA3
		public override bool IsDone
		{
			get
			{
				return !this.INTERNAL_IsPending;
			}
		}

		// Token: 0x060095ED RID: 38381 RVA: 0x003879AE File Offset: 0x00385BAE
		public CoroutineTask(LoadManager.LoadGroup _group, IEnumerator _task, LoadManager.CompletionCallback _callback)
			: base(_group, true)
		{
			this.task = _task;
			this.callback = _callback;
		}

		// Token: 0x060095EE RID: 38382 RVA: 0x00020021 File Offset: 0x0001E221
		public override bool Load()
		{
			return true;
		}

		// Token: 0x060095EF RID: 38383 RVA: 0x003879C6 File Offset: 0x00385BC6
		public override void LoadSync()
		{
			throw new Exception("CoroutineTask doesn't support synchronous loading.");
		}

		// Token: 0x060095F0 RID: 38384 RVA: 0x003879D2 File Offset: 0x00385BD2
		public override void Update()
		{
			this.isDone = !this.task.MoveNext();
		}

		// Token: 0x060095F1 RID: 38385 RVA: 0x003879E8 File Offset: 0x00385BE8
		public override void Complete()
		{
			if (this.callback != null)
			{
				this.callback();
			}
		}

		// Token: 0x060095F2 RID: 38386 RVA: 0x003879C6 File Offset: 0x00385BC6
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void CompleteNow()
		{
			throw new Exception("CoroutineTask doesn't support synchronous loading.");
		}

		// Token: 0x04007059 RID: 28761
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly IEnumerator task;

		// Token: 0x0400705A RID: 28762
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly LoadManager.CompletionCallback callback;

		// Token: 0x0400705B RID: 28763
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isDone;
	}

	// Token: 0x02001268 RID: 4712
	public class FileLoadTask : LoadManager.LoadTask
	{
		// Token: 0x170011D1 RID: 4561
		// (get) Token: 0x060095F3 RID: 38387 RVA: 0x003879FD File Offset: 0x00385BFD
		public override bool INTERNAL_IsPending
		{
			get
			{
				return !this.isDone;
			}
		}

		// Token: 0x170011D2 RID: 4562
		// (get) Token: 0x060095F4 RID: 38388 RVA: 0x003879A3 File Offset: 0x00385BA3
		public override bool IsDone
		{
			get
			{
				return !this.INTERNAL_IsPending;
			}
		}

		// Token: 0x060095F5 RID: 38389 RVA: 0x00387A0A File Offset: 0x00385C0A
		public FileLoadTask(LoadManager.LoadGroup _group, bool _loadAsync, string _path, LoadManager.FileLoadCallback _callback)
			: base(_group, _loadAsync)
		{
			this.path = _path;
			this.callback = _callback;
		}

		// Token: 0x060095F6 RID: 38390 RVA: 0x00387A23 File Offset: 0x00385C23
		public override bool Load()
		{
			this.asyncLoadTask = ThreadManager.AddSingleTask(delegate(ThreadManager.TaskInfo _threadInfo)
			{
				try
				{
					this.content = SdFile.ReadAllBytes(this.path);
				}
				catch (Exception ex)
				{
					Log.Out("LoadManager.FileLoadTask.Load - Failed to load file: " + ex.Message);
					Log.Out(ex.StackTrace);
				}
				this.isDone = true;
			}, null, null, true);
			this.loadStarted = true;
			return true;
		}

		// Token: 0x060095F7 RID: 38391 RVA: 0x00387A48 File Offset: 0x00385C48
		public override void LoadSync()
		{
			try
			{
				this.content = SdFile.ReadAllBytes(this.path);
			}
			catch (Exception ex)
			{
				Log.Out("LoadManager.FileLoadTask.LoadSync - Failed to load file: " + ex.Message);
				Log.Out(ex.StackTrace);
			}
			this.isDone = true;
			if (this.callback != null)
			{
				this.callback(this.content);
			}
			this.isCompleted = true;
		}

		// Token: 0x060095F8 RID: 38392 RVA: 0x00387AC4 File Offset: 0x00385CC4
		public override void Complete()
		{
			if (this.isDone)
			{
				if (this.callback != null)
				{
					this.callback(this.content);
				}
				this.isCompleted = true;
				return;
			}
			throw new Exception("[LoadManager] FileLoadTask still pending.");
		}

		// Token: 0x060095F9 RID: 38393 RVA: 0x00387AFC File Offset: 0x00385CFC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void CompleteNow()
		{
			if (this.loadAsync)
			{
				if (this.isCompleted)
				{
					throw new Exception("AssetBundleRequestTask async already completed.");
				}
				if (this.asyncLoadTask == null)
				{
					throw new Exception("AssetBundleRequestTask async has not started.");
				}
				this.asyncLoadTask.WaitForEnd();
				this.Complete();
			}
		}

		// Token: 0x0400705C RID: 28764
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string path;

		// Token: 0x0400705D RID: 28765
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly LoadManager.FileLoadCallback callback;

		// Token: 0x0400705E RID: 28766
		[PublicizedFrom(EAccessModifier.Private)]
		public byte[] content;

		// Token: 0x0400705F RID: 28767
		[PublicizedFrom(EAccessModifier.Private)]
		public volatile bool isDone;

		// Token: 0x04007060 RID: 28768
		[PublicizedFrom(EAccessModifier.Private)]
		public ThreadManager.TaskInfo asyncLoadTask;

		// Token: 0x04007061 RID: 28769
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isCompleted;
	}

	// Token: 0x02001269 RID: 4713
	public interface IAssetHandle
	{
		// Token: 0x060095FB RID: 38395
		LoadManager.IAssetHandle Copy();

		// Token: 0x060095FC RID: 38396
		void Release();
	}

	// Token: 0x0200126A RID: 4714
	public class AddressableAssetHandle<T> : LoadManager.IAssetHandle where T : global::UnityEngine.Object
	{
		// Token: 0x060095FD RID: 38397 RVA: 0x00387BA4 File Offset: 0x00385DA4
		public AddressableAssetHandle(AsyncOperationHandle<T> _handle)
		{
			this.handle = Addressables.ResourceManager.Acquire<T>(_handle);
		}

		// Token: 0x060095FE RID: 38398 RVA: 0x00387BBD File Offset: 0x00385DBD
		public LoadManager.IAssetHandle Copy()
		{
			return new LoadManager.AddressableAssetHandle<T>(this.handle);
		}

		// Token: 0x060095FF RID: 38399 RVA: 0x00387BCA File Offset: 0x00385DCA
		public void Release()
		{
			this.handle.Release();
		}

		// Token: 0x06009600 RID: 38400 RVA: 0x00387BD7 File Offset: 0x00385DD7
		public override string ToString()
		{
			return this.handle.DebugName ?? "";
		}

		// Token: 0x04007062 RID: 28770
		[PublicizedFrom(EAccessModifier.Private)]
		public AsyncOperationHandle<T> handle;
	}

	// Token: 0x0200126B RID: 4715
	public class DummyHandle : LoadManager.IAssetHandle
	{
		// Token: 0x06009601 RID: 38401 RVA: 0x00387042 File Offset: 0x00385242
		public LoadManager.IAssetHandle Copy()
		{
			return LoadManager.DummyHandle.Instance;
		}

		// Token: 0x06009602 RID: 38402 RVA: 0x000027FC File Offset: 0x000009FC
		public void Release()
		{
		}

		// Token: 0x04007063 RID: 28771
		public static LoadManager.DummyHandle Instance = new LoadManager.DummyHandle();
	}
}

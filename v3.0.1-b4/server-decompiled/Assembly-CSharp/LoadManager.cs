using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

// Token: 0x0200123E RID: 4670
public static class LoadManager
{
	// Token: 0x0600947F RID: 38015 RVA: 0x003816E0 File Offset: 0x0037F8E0
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

	// Token: 0x06009480 RID: 38016 RVA: 0x003817D8 File Offset: 0x0037F9D8
	public static void InitSync()
	{
		LoadManager.Init();
		LoadManager.forceLoadSync = true;
	}

	// Token: 0x06009481 RID: 38017 RVA: 0x003817E8 File Offset: 0x0037F9E8
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

	// Token: 0x06009482 RID: 38018 RVA: 0x003818A0 File Offset: 0x0037FAA0
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

	// Token: 0x06009483 RID: 38019 RVA: 0x003818CD File Offset: 0x0037FACD
	[PublicizedFrom(EAccessModifier.Private)]
	public static void RemoveRequest(LoadManager.LoadTask _request)
	{
		if (LoadManager.loadRequests.Remove(_request))
		{
			_request.Group.DecrementPending();
		}
	}

	// Token: 0x06009484 RID: 38020 RVA: 0x003818E7 File Offset: 0x0037FAE7
	public static void Destroy()
	{
		LoadManager.loadRequests.Clear();
	}

	// Token: 0x06009485 RID: 38021 RVA: 0x003818F3 File Offset: 0x0037FAF3
	public static LoadManager.LoadGroup CreateGroup()
	{
		return new LoadManager.LoadGroup(LoadManager.rootGroup);
	}

	// Token: 0x06009486 RID: 38022 RVA: 0x003818FF File Offset: 0x0037FAFF
	public static LoadManager.LoadGroup CreateGroup(LoadManager.LoadGroup _parent)
	{
		return new LoadManager.LoadGroup(_parent);
	}

	// Token: 0x06009487 RID: 38023 RVA: 0x00381908 File Offset: 0x0037FB08
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

	// Token: 0x06009488 RID: 38024 RVA: 0x0038193C File Offset: 0x0037FB3C
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

	// Token: 0x06009489 RID: 38025 RVA: 0x003819AA File Offset: 0x0037FBAA
	public static LoadManager.AssetRequestTask<T> LoadAsset<T>(string _uri, Action<T> _callback = null, LoadManager.LoadGroup _lg = null, bool _deferLoading = false, bool _loadSync = false, bool _ignoreDlcEntitlements = false) where T : global::UnityEngine.Object
	{
		return LoadManager.LoadAsset<T>(DataLoader.ParseDataPathIdentifier(_uri), _callback, _lg, _deferLoading, _loadSync, _ignoreDlcEntitlements);
	}

	// Token: 0x0600948A RID: 38026 RVA: 0x003819BE File Offset: 0x0037FBBE
	public static LoadManager.AssetRequestTask<T> LoadAsset<T>(string _bundlePath, string _assetName, Action<T> _callback = null, LoadManager.LoadGroup _lg = null, bool _deferLoading = false, bool _loadSync = false) where T : global::UnityEngine.Object
	{
		return LoadManager.LoadAsset<T>(new DataLoader.DataPathIdentifier(_assetName, _bundlePath, false), _callback, _lg, _deferLoading, _loadSync, false);
	}

	// Token: 0x0600948B RID: 38027 RVA: 0x003819D4 File Offset: 0x0037FBD4
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

	// Token: 0x0600948C RID: 38028 RVA: 0x00381A18 File Offset: 0x0037FC18
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

	// Token: 0x0600948D RID: 38029 RVA: 0x00381A48 File Offset: 0x0037FC48
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

	// Token: 0x0600948E RID: 38030 RVA: 0x00381AC0 File Offset: 0x0037FCC0
	public static LoadManager.AddressableRequestTask<T> LoadAssetFromAddressables<T>(string _folderAddress, string _assetPath, Action<T> _callback = null, LoadManager.LoadGroup _lg = null, bool _deferLoading = false, bool _loadSync = false, bool _ignoreDlcEntitlements = false) where T : global::UnityEngine.Object
	{
		return LoadManager.LoadAssetFromAddressables<T>(_folderAddress + "/" + _assetPath, _callback, _lg, _deferLoading, _loadSync, _ignoreDlcEntitlements);
	}

	// Token: 0x0600948F RID: 38031 RVA: 0x00381ADC File Offset: 0x0037FCDC
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

	// Token: 0x06009490 RID: 38032 RVA: 0x00381B0C File Offset: 0x0037FD0C
	public static void ReleaseAddressable<T>(T _obj)
	{
		Addressables.Release<T>(_obj);
	}

	// Token: 0x06009491 RID: 38033 RVA: 0x00381B14 File Offset: 0x0037FD14
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

	// Token: 0x06009492 RID: 38034 RVA: 0x00381B44 File Offset: 0x0037FD44
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

	// Token: 0x06009493 RID: 38035 RVA: 0x00381B54 File Offset: 0x0037FD54
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

	// Token: 0x06009494 RID: 38036 RVA: 0x00381BBC File Offset: 0x0037FDBC
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

	// Token: 0x06009495 RID: 38037 RVA: 0x00381BF4 File Offset: 0x0037FDF4
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

	// Token: 0x04006F5F RID: 28511
	[PublicizedFrom(EAccessModifier.Private)]
	public static Action<LoadManager.LoadTask> updateRequestAction;

	// Token: 0x04006F60 RID: 28512
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<string, string> addressablesCaseMap;

	// Token: 0x04006F61 RID: 28513
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool forceLoadSync;

	// Token: 0x04006F62 RID: 28514
	[PublicizedFrom(EAccessModifier.Private)]
	public static LoadManager.LoadGroup rootGroup;

	// Token: 0x04006F63 RID: 28515
	[PublicizedFrom(EAccessModifier.Private)]
	public static WorkBatch<LoadManager.LoadTask> loadRequests;

	// Token: 0x04006F64 RID: 28516
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<LoadManager.LoadTask> deferedLoadRequests;

	// Token: 0x0200123F RID: 4671
	// (Invoke) Token: 0x06009497 RID: 38039
	public delegate void CompletionCallback();

	// Token: 0x02001240 RID: 4672
	// (Invoke) Token: 0x0600949B RID: 38043
	public delegate void FileLoadCallback(byte[] _content);

	// Token: 0x02001241 RID: 4673
	public class LoadGroup
	{
		// Token: 0x17001182 RID: 4482
		// (get) Token: 0x0600949E RID: 38046 RVA: 0x00381C2E File Offset: 0x0037FE2E
		public bool Pending
		{
			get
			{
				return Interlocked.CompareExchange(ref this.pending, 0, 0) != 0;
			}
		}

		// Token: 0x0600949F RID: 38047 RVA: 0x00381C40 File Offset: 0x0037FE40
		[PublicizedFrom(EAccessModifier.Internal)]
		public LoadGroup(LoadManager.LoadGroup _parent)
		{
			this.parent = _parent;
			this.pending = 0;
		}

		// Token: 0x060094A0 RID: 38048 RVA: 0x00381C58 File Offset: 0x0037FE58
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

		// Token: 0x060094A1 RID: 38049 RVA: 0x00381C88 File Offset: 0x0037FE88
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

		// Token: 0x04006F65 RID: 28517
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly LoadManager.LoadGroup parent;

		// Token: 0x04006F66 RID: 28518
		[PublicizedFrom(EAccessModifier.Private)]
		public int pending;
	}

	// Token: 0x02001242 RID: 4674
	public abstract class LoadTask : CustomYieldInstruction
	{
		// Token: 0x060094A2 RID: 38050 RVA: 0x00381CB7 File Offset: 0x0037FEB7
		[PublicizedFrom(EAccessModifier.Protected)]
		public LoadTask(LoadManager.LoadGroup _group, bool _loadAsync)
		{
			this.group = _group;
			this.loadAsync = _loadAsync;
		}

		// Token: 0x17001183 RID: 4483
		// (get) Token: 0x060094A3 RID: 38051
		public abstract bool IsDone { get; }

		// Token: 0x17001184 RID: 4484
		// (get) Token: 0x060094A4 RID: 38052
		public abstract bool INTERNAL_IsPending { get; }

		// Token: 0x17001185 RID: 4485
		// (get) Token: 0x060094A5 RID: 38053 RVA: 0x00381CCD File Offset: 0x0037FECD
		public override bool keepWaiting
		{
			get
			{
				return this.INTERNAL_IsPending;
			}
		}

		// Token: 0x17001186 RID: 4486
		// (get) Token: 0x060094A6 RID: 38054 RVA: 0x00381CD5 File Offset: 0x0037FED5
		public LoadManager.LoadGroup Group
		{
			get
			{
				return this.group;
			}
		}

		// Token: 0x060094A7 RID: 38055
		public abstract bool Load();

		// Token: 0x060094A8 RID: 38056
		public abstract void LoadSync();

		// Token: 0x060094A9 RID: 38057 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void Update()
		{
		}

		// Token: 0x060094AA RID: 38058
		public abstract void Complete();

		// Token: 0x060094AB RID: 38059 RVA: 0x00381CDD File Offset: 0x0037FEDD
		public void WaitForComplete()
		{
			this.CompleteNow();
			LoadManager.RemoveRequest(this);
		}

		// Token: 0x060094AC RID: 38060
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract void CompleteNow();

		// Token: 0x04006F67 RID: 28519
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly LoadManager.LoadGroup group;

		// Token: 0x04006F68 RID: 28520
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly bool loadAsync;

		// Token: 0x04006F69 RID: 28521
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool loadStarted;
	}

	// Token: 0x02001243 RID: 4675
	public class AssetBundleLoadTask : LoadManager.LoadTask
	{
		// Token: 0x17001187 RID: 4487
		// (get) Token: 0x060094AD RID: 38061 RVA: 0x0001FF5D File Offset: 0x0001E15D
		public override bool INTERNAL_IsPending
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001188 RID: 4488
		// (get) Token: 0x060094AE RID: 38062 RVA: 0x00010E62 File Offset: 0x0000F062
		public override bool IsDone
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060094AF RID: 38063 RVA: 0x00381CEB File Offset: 0x0037FEEB
		public AssetBundleLoadTask(LoadManager.LoadGroup _group, bool _loadAsync, Action<AssetBundle> _callback)
			: base(_group, _loadAsync)
		{
			this.callback = _callback;
		}

		// Token: 0x060094B0 RID: 38064 RVA: 0x000876FD File Offset: 0x000858FD
		public override bool Load()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060094B1 RID: 38065 RVA: 0x000876FD File Offset: 0x000858FD
		public override void LoadSync()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060094B2 RID: 38066 RVA: 0x000876FD File Offset: 0x000858FD
		public override void Complete()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060094B3 RID: 38067 RVA: 0x000876FD File Offset: 0x000858FD
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void CompleteNow()
		{
			throw new NotImplementedException();
		}

		// Token: 0x04006F6A RID: 28522
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Action<AssetBundle> callback;
	}

	// Token: 0x02001244 RID: 4676
	public abstract class AssetRequestTask<T> : LoadManager.LoadTask where T : global::UnityEngine.Object
	{
		// Token: 0x17001189 RID: 4489
		// (get) Token: 0x060094B4 RID: 38068 RVA: 0x00381CFC File Offset: 0x0037FEFC
		public override bool keepWaiting
		{
			get
			{
				return this.INTERNAL_IsPending || !this.assetRetrieved;
			}
		}

		// Token: 0x1700118A RID: 4490
		// (get) Token: 0x060094B5 RID: 38069 RVA: 0x00381D11 File Offset: 0x0037FF11
		public override bool IsDone
		{
			get
			{
				return this.assetRetrieved;
			}
		}

		// Token: 0x1700118B RID: 4491
		// (get) Token: 0x060094B6 RID: 38070 RVA: 0x00381D19 File Offset: 0x0037FF19
		public T Asset
		{
			get
			{
				return this.asset;
			}
		}

		// Token: 0x060094B7 RID: 38071 RVA: 0x00381D21 File Offset: 0x0037FF21
		[PublicizedFrom(EAccessModifier.Protected)]
		public AssetRequestTask(LoadManager.LoadGroup _group, bool _loadAsync, Action<T> _callback)
			: base(_group, _loadAsync)
		{
			this.callback = _callback;
		}

		// Token: 0x060094B8 RID: 38072 RVA: 0x00381D32 File Offset: 0x0037FF32
		public virtual LoadManager.IAssetHandle GetHandle()
		{
			return LoadManager.DummyHandle.Instance;
		}

		// Token: 0x060094B9 RID: 38073 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void Release()
		{
		}

		// Token: 0x04006F6B RID: 28523
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly Action<T> callback;

		// Token: 0x04006F6C RID: 28524
		[PublicizedFrom(EAccessModifier.Protected)]
		public T asset;

		// Token: 0x04006F6D RID: 28525
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool assetRetrieved;
	}

	// Token: 0x02001245 RID: 4677
	public abstract class AssetsRequestTask<T> : LoadManager.LoadTask where T : global::UnityEngine.Object
	{
		// Token: 0x1700118C RID: 4492
		// (get) Token: 0x060094BA RID: 38074 RVA: 0x00381D39 File Offset: 0x0037FF39
		public override bool keepWaiting
		{
			get
			{
				return this.INTERNAL_IsPending || !this.assetsRetrieved;
			}
		}

		// Token: 0x1700118D RID: 4493
		// (get) Token: 0x060094BB RID: 38075 RVA: 0x00381D4E File Offset: 0x0037FF4E
		public override bool IsDone
		{
			get
			{
				return this.assetsRetrieved;
			}
		}

		// Token: 0x060094BC RID: 38076 RVA: 0x00381D56 File Offset: 0x0037FF56
		[PublicizedFrom(EAccessModifier.Protected)]
		public AssetsRequestTask(LoadManager.LoadGroup _group, bool _loadAsync)
			: base(_group, _loadAsync)
		{
		}

		// Token: 0x060094BD RID: 38077
		public abstract void CollectResults(List<T> _results);

		// Token: 0x04006F6E RID: 28526
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool assetsRetrieved;
	}

	// Token: 0x02001246 RID: 4678
	public class ResourceRequestTask<T> : LoadManager.AssetRequestTask<T> where T : global::UnityEngine.Object
	{
		// Token: 0x1700118E RID: 4494
		// (get) Token: 0x060094BE RID: 38078 RVA: 0x00381D60 File Offset: 0x0037FF60
		public override bool INTERNAL_IsPending
		{
			get
			{
				return this.loadAsync && !this.asyncCancelled && (!this.loadStarted || (this.request != null && !this.request.isDone));
			}
		}

		// Token: 0x060094BF RID: 38079 RVA: 0x00381D96 File Offset: 0x0037FF96
		public ResourceRequestTask(LoadManager.LoadGroup _group, bool _loadAsync, string _assetPath, Action<T> _callback)
			: base(_group, _loadAsync, _callback)
		{
			this.assetPath = _assetPath;
		}

		// Token: 0x060094C0 RID: 38080 RVA: 0x00381DA9 File Offset: 0x0037FFA9
		public override bool Load()
		{
			this.request = Resources.LoadAsync<T>(this.assetPath);
			this.loadStarted = true;
			return this.request != null;
		}

		// Token: 0x060094C1 RID: 38081 RVA: 0x00381DCC File Offset: 0x0037FFCC
		public override void LoadSync()
		{
			this.asset = Resources.Load<T>(this.assetPath);
			this.assetRetrieved = true;
			if (this.callback != null)
			{
				this.callback(this.asset);
			}
		}

		// Token: 0x060094C2 RID: 38082 RVA: 0x00381E00 File Offset: 0x00380000
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

		// Token: 0x060094C3 RID: 38083 RVA: 0x00381E5C File Offset: 0x0038005C
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

		// Token: 0x04006F6F RID: 28527
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string assetPath;

		// Token: 0x04006F70 RID: 28528
		[PublicizedFrom(EAccessModifier.Private)]
		public ResourceRequest request;

		// Token: 0x04006F71 RID: 28529
		[PublicizedFrom(EAccessModifier.Private)]
		public bool asyncCancelled;
	}

	// Token: 0x02001247 RID: 4679
	public class AssetBundleRequestTask<T> : LoadManager.AssetRequestTask<T> where T : global::UnityEngine.Object
	{
		// Token: 0x1700118F RID: 4495
		// (get) Token: 0x060094C4 RID: 38084 RVA: 0x00381EB3 File Offset: 0x003800B3
		public override bool INTERNAL_IsPending
		{
			get
			{
				return this.loadAsync && !this.asyncCancelled && (!this.loadStarted || (this.request != null && !this.request.IsDone));
			}
		}

		// Token: 0x060094C5 RID: 38085 RVA: 0x00381EE9 File Offset: 0x003800E9
		public AssetBundleRequestTask(LoadManager.LoadGroup _group, bool _loadAsync, DataLoader.DataPathIdentifier _identifier, Action<T> _callback)
			: base(_group, _loadAsync, _callback)
		{
			this.identifier = _identifier;
			if (typeof(T) == typeof(Transform))
			{
				this.isGameObject = true;
			}
		}

		// Token: 0x060094C6 RID: 38086 RVA: 0x00381F20 File Offset: 0x00380120
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

		// Token: 0x060094C7 RID: 38087 RVA: 0x00381FAC File Offset: 0x003801AC
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

		// Token: 0x060094C8 RID: 38088 RVA: 0x00382058 File Offset: 0x00380258
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

		// Token: 0x060094C9 RID: 38089 RVA: 0x003820E4 File Offset: 0x003802E4
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

		// Token: 0x04006F72 RID: 28530
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly DataLoader.DataPathIdentifier identifier;

		// Token: 0x04006F73 RID: 28531
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool isGameObject;

		// Token: 0x04006F74 RID: 28532
		[PublicizedFrom(EAccessModifier.Private)]
		public AssetBundleManager.AssetBundleRequestTFP request;

		// Token: 0x04006F75 RID: 28533
		[PublicizedFrom(EAccessModifier.Private)]
		public bool asyncCancelled;
	}

	// Token: 0x02001248 RID: 4680
	public class AddressableRequestTask<T> : LoadManager.AssetRequestTask<T> where T : global::UnityEngine.Object
	{
		// Token: 0x17001190 RID: 4496
		// (get) Token: 0x060094CA RID: 38090 RVA: 0x00382168 File Offset: 0x00380368
		public override bool INTERNAL_IsPending
		{
			get
			{
				return this.loadAsync && (!this.loadStarted || (this.request.IsValid() && !this.request.IsDone) || (this.gameObjectRequest.IsValid() && !this.gameObjectRequest.IsDone));
			}
		}

		// Token: 0x060094CB RID: 38091 RVA: 0x003821C0 File Offset: 0x003803C0
		public AddressableRequestTask(LoadManager.LoadGroup _group, bool _loadAsync, object _key, Action<T> _callback)
			: base(_group, _loadAsync, _callback)
		{
			this.key = _key;
			if (typeof(T) == typeof(Transform))
			{
				this.isGameObject = true;
			}
		}

		// Token: 0x060094CC RID: 38092 RVA: 0x003821F5 File Offset: 0x003803F5
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

		// Token: 0x060094CD RID: 38093 RVA: 0x00382233 File Offset: 0x00380433
		public override bool Load()
		{
			this.StartRequest();
			return this.request.IsValid() || this.gameObjectRequest.IsValid();
		}

		// Token: 0x060094CE RID: 38094 RVA: 0x00382258 File Offset: 0x00380458
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

		// Token: 0x060094CF RID: 38095 RVA: 0x003822E0 File Offset: 0x003804E0
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

		// Token: 0x060094D0 RID: 38096 RVA: 0x0038235D File Offset: 0x0038055D
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

		// Token: 0x060094D1 RID: 38097 RVA: 0x00382380 File Offset: 0x00380580
		public override LoadManager.IAssetHandle GetHandle()
		{
			if (this.isGameObject)
			{
				return new LoadManager.AddressableAssetHandle<GameObject>(this.gameObjectRequest);
			}
			return new LoadManager.AddressableAssetHandle<T>(this.request);
		}

		// Token: 0x060094D2 RID: 38098 RVA: 0x003823A1 File Offset: 0x003805A1
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

		// Token: 0x060094D3 RID: 38099 RVA: 0x003823CE File Offset: 0x003805CE
		public static LoadManager.AddressableRequestTask<T> Empty()
		{
			return LoadManager.AddressableRequestTask<T>.emptyInstance;
		}

		// Token: 0x04006F76 RID: 28534
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly object key;

		// Token: 0x04006F77 RID: 28535
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool isGameObject;

		// Token: 0x04006F78 RID: 28536
		[PublicizedFrom(EAccessModifier.Private)]
		public AsyncOperationHandle<T> request;

		// Token: 0x04006F79 RID: 28537
		[PublicizedFrom(EAccessModifier.Private)]
		public AsyncOperationHandle<GameObject> gameObjectRequest;

		// Token: 0x04006F7A RID: 28538
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly LoadManager.EmptyAddressableRequestTask<T> emptyInstance = new LoadManager.EmptyAddressableRequestTask<T>();
	}

	// Token: 0x02001249 RID: 4681
	[PublicizedFrom(EAccessModifier.Private)]
	public class EmptyAddressableRequestTask<T> : LoadManager.AddressableRequestTask<T> where T : global::UnityEngine.Object
	{
		// Token: 0x060094D5 RID: 38101 RVA: 0x003823E1 File Offset: 0x003805E1
		public EmptyAddressableRequestTask()
			: base(null, true, null, null)
		{
		}

		// Token: 0x17001191 RID: 4497
		// (get) Token: 0x060094D6 RID: 38102 RVA: 0x00010E62 File Offset: 0x0000F062
		public override bool INTERNAL_IsPending
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001192 RID: 4498
		// (get) Token: 0x060094D7 RID: 38103 RVA: 0x0001FF5D File Offset: 0x0001E15D
		public override bool IsDone
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060094D8 RID: 38104 RVA: 0x00010E62 File Offset: 0x0000F062
		public override bool Load()
		{
			return false;
		}

		// Token: 0x060094D9 RID: 38105 RVA: 0x000027FC File Offset: 0x000009FC
		public override void LoadSync()
		{
		}

		// Token: 0x060094DA RID: 38106 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Complete()
		{
		}

		// Token: 0x060094DB RID: 38107 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Release()
		{
		}

		// Token: 0x060094DC RID: 38108 RVA: 0x00381D32 File Offset: 0x0037FF32
		public override LoadManager.IAssetHandle GetHandle()
		{
			return LoadManager.DummyHandle.Instance;
		}
	}

	// Token: 0x0200124A RID: 4682
	public class AddressableAssetsRequestTask<T> : LoadManager.AssetsRequestTask<T> where T : global::UnityEngine.Object
	{
		// Token: 0x17001193 RID: 4499
		// (get) Token: 0x060094DD RID: 38109 RVA: 0x003823ED File Offset: 0x003805ED
		public override bool INTERNAL_IsPending
		{
			get
			{
				return this.loadAsync && (!this.loadStarted || !this.assetRequestStarted || (this.assetsRequest.IsValid() && !this.assetsRequest.IsDone));
			}
		}

		// Token: 0x060094DE RID: 38110 RVA: 0x00382428 File Offset: 0x00380628
		public AddressableAssetsRequestTask(LoadManager.LoadGroup _group, bool _loadAsync, string _label, Func<string, bool> _addressFilter = null)
			: base(_group, _loadAsync)
		{
			this.label = _label;
			this.addressFilter = _addressFilter;
		}

		// Token: 0x060094DF RID: 38111 RVA: 0x00382441 File Offset: 0x00380641
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

		// Token: 0x060094E0 RID: 38112 RVA: 0x00382470 File Offset: 0x00380670
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

		// Token: 0x060094E1 RID: 38113 RVA: 0x00382538 File Offset: 0x00380738
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

		// Token: 0x060094E2 RID: 38114 RVA: 0x00382569 File Offset: 0x00380769
		public override bool Load()
		{
			this.StartLocationsRequest();
			return this.locationRequest.IsValid();
		}

		// Token: 0x060094E3 RID: 38115 RVA: 0x0038257C File Offset: 0x0038077C
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

		// Token: 0x060094E4 RID: 38116 RVA: 0x003825CD File Offset: 0x003807CD
		public override void Complete()
		{
			if (this.INTERNAL_IsPending)
			{
				throw new Exception("AddressableAssetsRequestTask still pending.");
			}
			this.assetsRetrieved = true;
		}

		// Token: 0x060094E5 RID: 38117 RVA: 0x003825E9 File Offset: 0x003807E9
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

		// Token: 0x060094E6 RID: 38118 RVA: 0x0038260C File Offset: 0x0038080C
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

		// Token: 0x04006F7B RID: 28539
		[PublicizedFrom(EAccessModifier.Private)]
		public string label;

		// Token: 0x04006F7C RID: 28540
		[PublicizedFrom(EAccessModifier.Private)]
		public Func<string, bool> addressFilter;

		// Token: 0x04006F7D RID: 28541
		[PublicizedFrom(EAccessModifier.Private)]
		public AsyncOperationHandle<IList<IResourceLocation>> locationRequest;

		// Token: 0x04006F7E RID: 28542
		[PublicizedFrom(EAccessModifier.Private)]
		public bool assetRequestStarted;

		// Token: 0x04006F7F RID: 28543
		[PublicizedFrom(EAccessModifier.Private)]
		public AsyncOperationHandle<IList<T>> assetsRequest;
	}

	// Token: 0x0200124B RID: 4683
	public class CoroutineTask : LoadManager.LoadTask
	{
		// Token: 0x17001194 RID: 4500
		// (get) Token: 0x060094E7 RID: 38119 RVA: 0x00382688 File Offset: 0x00380888
		public override bool INTERNAL_IsPending
		{
			get
			{
				return !this.isDone;
			}
		}

		// Token: 0x17001195 RID: 4501
		// (get) Token: 0x060094E8 RID: 38120 RVA: 0x00382693 File Offset: 0x00380893
		public override bool IsDone
		{
			get
			{
				return !this.INTERNAL_IsPending;
			}
		}

		// Token: 0x060094E9 RID: 38121 RVA: 0x0038269E File Offset: 0x0038089E
		public CoroutineTask(LoadManager.LoadGroup _group, IEnumerator _task, LoadManager.CompletionCallback _callback)
			: base(_group, true)
		{
			this.task = _task;
			this.callback = _callback;
		}

		// Token: 0x060094EA RID: 38122 RVA: 0x0001FF5D File Offset: 0x0001E15D
		public override bool Load()
		{
			return true;
		}

		// Token: 0x060094EB RID: 38123 RVA: 0x003826B6 File Offset: 0x003808B6
		public override void LoadSync()
		{
			throw new Exception("CoroutineTask doesn't support synchronous loading.");
		}

		// Token: 0x060094EC RID: 38124 RVA: 0x003826C2 File Offset: 0x003808C2
		public override void Update()
		{
			this.isDone = !this.task.MoveNext();
		}

		// Token: 0x060094ED RID: 38125 RVA: 0x003826D8 File Offset: 0x003808D8
		public override void Complete()
		{
			if (this.callback != null)
			{
				this.callback();
			}
		}

		// Token: 0x060094EE RID: 38126 RVA: 0x003826B6 File Offset: 0x003808B6
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void CompleteNow()
		{
			throw new Exception("CoroutineTask doesn't support synchronous loading.");
		}

		// Token: 0x04006F80 RID: 28544
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly IEnumerator task;

		// Token: 0x04006F81 RID: 28545
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly LoadManager.CompletionCallback callback;

		// Token: 0x04006F82 RID: 28546
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isDone;
	}

	// Token: 0x0200124C RID: 4684
	public class FileLoadTask : LoadManager.LoadTask
	{
		// Token: 0x17001196 RID: 4502
		// (get) Token: 0x060094EF RID: 38127 RVA: 0x003826ED File Offset: 0x003808ED
		public override bool INTERNAL_IsPending
		{
			get
			{
				return !this.isDone;
			}
		}

		// Token: 0x17001197 RID: 4503
		// (get) Token: 0x060094F0 RID: 38128 RVA: 0x00382693 File Offset: 0x00380893
		public override bool IsDone
		{
			get
			{
				return !this.INTERNAL_IsPending;
			}
		}

		// Token: 0x060094F1 RID: 38129 RVA: 0x003826FA File Offset: 0x003808FA
		public FileLoadTask(LoadManager.LoadGroup _group, bool _loadAsync, string _path, LoadManager.FileLoadCallback _callback)
			: base(_group, _loadAsync)
		{
			this.path = _path;
			this.callback = _callback;
		}

		// Token: 0x060094F2 RID: 38130 RVA: 0x00382713 File Offset: 0x00380913
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

		// Token: 0x060094F3 RID: 38131 RVA: 0x00382738 File Offset: 0x00380938
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

		// Token: 0x060094F4 RID: 38132 RVA: 0x003827B4 File Offset: 0x003809B4
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

		// Token: 0x060094F5 RID: 38133 RVA: 0x003827EC File Offset: 0x003809EC
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

		// Token: 0x04006F83 RID: 28547
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string path;

		// Token: 0x04006F84 RID: 28548
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly LoadManager.FileLoadCallback callback;

		// Token: 0x04006F85 RID: 28549
		[PublicizedFrom(EAccessModifier.Private)]
		public byte[] content;

		// Token: 0x04006F86 RID: 28550
		[PublicizedFrom(EAccessModifier.Private)]
		public volatile bool isDone;

		// Token: 0x04006F87 RID: 28551
		[PublicizedFrom(EAccessModifier.Private)]
		public ThreadManager.TaskInfo asyncLoadTask;

		// Token: 0x04006F88 RID: 28552
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isCompleted;
	}

	// Token: 0x0200124D RID: 4685
	public interface IAssetHandle
	{
		// Token: 0x060094F7 RID: 38135
		LoadManager.IAssetHandle Copy();

		// Token: 0x060094F8 RID: 38136
		void Release();
	}

	// Token: 0x0200124E RID: 4686
	public class AddressableAssetHandle<T> : LoadManager.IAssetHandle where T : global::UnityEngine.Object
	{
		// Token: 0x060094F9 RID: 38137 RVA: 0x00382894 File Offset: 0x00380A94
		public AddressableAssetHandle(AsyncOperationHandle<T> _handle)
		{
			this.handle = Addressables.ResourceManager.Acquire<T>(_handle);
		}

		// Token: 0x060094FA RID: 38138 RVA: 0x003828AD File Offset: 0x00380AAD
		public LoadManager.IAssetHandle Copy()
		{
			return new LoadManager.AddressableAssetHandle<T>(this.handle);
		}

		// Token: 0x060094FB RID: 38139 RVA: 0x003828BA File Offset: 0x00380ABA
		public void Release()
		{
			this.handle.Release();
		}

		// Token: 0x060094FC RID: 38140 RVA: 0x003828C7 File Offset: 0x00380AC7
		public override string ToString()
		{
			return this.handle.DebugName ?? "";
		}

		// Token: 0x04006F89 RID: 28553
		[PublicizedFrom(EAccessModifier.Private)]
		public AsyncOperationHandle<T> handle;
	}

	// Token: 0x0200124F RID: 4687
	public class DummyHandle : LoadManager.IAssetHandle
	{
		// Token: 0x060094FD RID: 38141 RVA: 0x00381D32 File Offset: 0x0037FF32
		public LoadManager.IAssetHandle Copy()
		{
			return LoadManager.DummyHandle.Instance;
		}

		// Token: 0x060094FE RID: 38142 RVA: 0x000027FC File Offset: 0x000009FC
		public void Release()
		{
		}

		// Token: 0x04006F8A RID: 28554
		public static LoadManager.DummyHandle Instance = new LoadManager.DummyHandle();
	}
}

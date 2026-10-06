using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

// Token: 0x02001272 RID: 4722
public static class LoadManager
{
	// Token: 0x06009610 RID: 38416 RVA: 0x0038B238 File Offset: 0x00389438
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

	// Token: 0x06009611 RID: 38417 RVA: 0x0038B330 File Offset: 0x00389530
	public static void InitSync()
	{
		LoadManager.Init();
		LoadManager.forceLoadSync = true;
	}

	// Token: 0x06009612 RID: 38418 RVA: 0x0038B340 File Offset: 0x00389540
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

	// Token: 0x06009613 RID: 38419 RVA: 0x0038B3F8 File Offset: 0x003895F8
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

	// Token: 0x06009614 RID: 38420 RVA: 0x0038B425 File Offset: 0x00389625
	[PublicizedFrom(EAccessModifier.Private)]
	public static void RemoveRequest(LoadManager.LoadTask _request)
	{
		if (LoadManager.loadRequests.Remove(_request))
		{
			_request.Group.DecrementPending();
		}
	}

	// Token: 0x06009615 RID: 38421 RVA: 0x0038B43F File Offset: 0x0038963F
	public static void Destroy()
	{
		LoadManager.loadRequests.Clear();
	}

	// Token: 0x06009616 RID: 38422 RVA: 0x0038B44B File Offset: 0x0038964B
	public static LoadManager.LoadGroup CreateGroup()
	{
		return new LoadManager.LoadGroup(LoadManager.rootGroup);
	}

	// Token: 0x06009617 RID: 38423 RVA: 0x0038B457 File Offset: 0x00389657
	public static LoadManager.LoadGroup CreateGroup(LoadManager.LoadGroup _parent)
	{
		return new LoadManager.LoadGroup(_parent);
	}

	// Token: 0x06009618 RID: 38424 RVA: 0x0038B460 File Offset: 0x00389660
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

	// Token: 0x06009619 RID: 38425 RVA: 0x0038B494 File Offset: 0x00389694
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

	// Token: 0x0600961A RID: 38426 RVA: 0x0038B502 File Offset: 0x00389702
	public static LoadManager.AssetRequestTask<T> LoadAsset<T>(string _uri, Action<T> _callback = null, LoadManager.LoadGroup _lg = null, bool _deferLoading = false, bool _loadSync = false, bool _ignoreDlcEntitlements = false) where T : global::UnityEngine.Object
	{
		return LoadManager.LoadAsset<T>(DataLoader.ParseDataPathIdentifier(_uri), _callback, _lg, _deferLoading, _loadSync, _ignoreDlcEntitlements);
	}

	// Token: 0x0600961B RID: 38427 RVA: 0x0038B516 File Offset: 0x00389716
	public static LoadManager.AssetRequestTask<T> LoadAsset<T>(string _bundlePath, string _assetName, Action<T> _callback = null, LoadManager.LoadGroup _lg = null, bool _deferLoading = false, bool _loadSync = false) where T : global::UnityEngine.Object
	{
		return LoadManager.LoadAsset<T>(new DataLoader.DataPathIdentifier(_assetName, _bundlePath, false), _callback, _lg, _deferLoading, _loadSync, false);
	}

	// Token: 0x0600961C RID: 38428 RVA: 0x0038B52C File Offset: 0x0038972C
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

	// Token: 0x0600961D RID: 38429 RVA: 0x0038B570 File Offset: 0x00389770
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

	// Token: 0x0600961E RID: 38430 RVA: 0x0038B5A0 File Offset: 0x003897A0
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

	// Token: 0x0600961F RID: 38431 RVA: 0x0038B618 File Offset: 0x00389818
	public static LoadManager.AddressableRequestTask<T> LoadAssetFromAddressables<T>(string _folderAddress, string _assetPath, Action<T> _callback = null, LoadManager.LoadGroup _lg = null, bool _deferLoading = false, bool _loadSync = false, bool _ignoreDlcEntitlements = false) where T : global::UnityEngine.Object
	{
		return LoadManager.LoadAssetFromAddressables<T>(_folderAddress + "/" + _assetPath, _callback, _lg, _deferLoading, _loadSync, _ignoreDlcEntitlements);
	}

	// Token: 0x06009620 RID: 38432 RVA: 0x0038B634 File Offset: 0x00389834
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

	// Token: 0x06009621 RID: 38433 RVA: 0x0038B664 File Offset: 0x00389864
	public static void ReleaseAddressable<T>(T _obj)
	{
		Addressables.Release<T>(_obj);
	}

	// Token: 0x06009622 RID: 38434 RVA: 0x0038B66C File Offset: 0x0038986C
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

	// Token: 0x06009623 RID: 38435 RVA: 0x0038B69C File Offset: 0x0038989C
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

	// Token: 0x06009624 RID: 38436 RVA: 0x0038B6AC File Offset: 0x003898AC
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

	// Token: 0x06009625 RID: 38437 RVA: 0x0038B714 File Offset: 0x00389914
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

	// Token: 0x06009626 RID: 38438 RVA: 0x0038B74C File Offset: 0x0038994C
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

	// Token: 0x040070A2 RID: 28834
	[PublicizedFrom(EAccessModifier.Private)]
	public static Action<LoadManager.LoadTask> updateRequestAction;

	// Token: 0x040070A3 RID: 28835
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<string, string> addressablesCaseMap;

	// Token: 0x040070A4 RID: 28836
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool forceLoadSync;

	// Token: 0x040070A5 RID: 28837
	[PublicizedFrom(EAccessModifier.Private)]
	public static LoadManager.LoadGroup rootGroup;

	// Token: 0x040070A6 RID: 28838
	[PublicizedFrom(EAccessModifier.Private)]
	public static WorkBatch<LoadManager.LoadTask> loadRequests;

	// Token: 0x040070A7 RID: 28839
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<LoadManager.LoadTask> deferedLoadRequests;

	// Token: 0x02001273 RID: 4723
	// (Invoke) Token: 0x06009628 RID: 38440
	public delegate void CompletionCallback();

	// Token: 0x02001274 RID: 4724
	// (Invoke) Token: 0x0600962C RID: 38444
	public delegate void FileLoadCallback(byte[] _content);

	// Token: 0x02001275 RID: 4725
	public class LoadGroup
	{
		// Token: 0x17001201 RID: 4609
		// (get) Token: 0x0600962F RID: 38447 RVA: 0x0038B786 File Offset: 0x00389986
		public bool Pending
		{
			get
			{
				return Interlocked.CompareExchange(ref this.pending, 0, 0) != 0;
			}
		}

		// Token: 0x06009630 RID: 38448 RVA: 0x0038B798 File Offset: 0x00389998
		[PublicizedFrom(EAccessModifier.Internal)]
		public LoadGroup(LoadManager.LoadGroup _parent)
		{
			this.parent = _parent;
			this.pending = 0;
		}

		// Token: 0x06009631 RID: 38449 RVA: 0x0038B7B0 File Offset: 0x003899B0
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

		// Token: 0x06009632 RID: 38450 RVA: 0x0038B7E0 File Offset: 0x003899E0
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

		// Token: 0x040070A8 RID: 28840
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly LoadManager.LoadGroup parent;

		// Token: 0x040070A9 RID: 28841
		[PublicizedFrom(EAccessModifier.Private)]
		public int pending;
	}

	// Token: 0x02001276 RID: 4726
	public abstract class LoadTask : CustomYieldInstruction
	{
		// Token: 0x06009633 RID: 38451 RVA: 0x0038B80F File Offset: 0x00389A0F
		[PublicizedFrom(EAccessModifier.Protected)]
		public LoadTask(LoadManager.LoadGroup _group, bool _loadAsync)
		{
			this.group = _group;
			this.loadAsync = _loadAsync;
		}

		// Token: 0x17001202 RID: 4610
		// (get) Token: 0x06009634 RID: 38452
		public abstract bool IsDone { get; }

		// Token: 0x17001203 RID: 4611
		// (get) Token: 0x06009635 RID: 38453
		public abstract bool INTERNAL_IsPending { get; }

		// Token: 0x17001204 RID: 4612
		// (get) Token: 0x06009636 RID: 38454 RVA: 0x0038B825 File Offset: 0x00389A25
		public override bool keepWaiting
		{
			get
			{
				return this.INTERNAL_IsPending;
			}
		}

		// Token: 0x17001205 RID: 4613
		// (get) Token: 0x06009637 RID: 38455 RVA: 0x0038B82D File Offset: 0x00389A2D
		public LoadManager.LoadGroup Group
		{
			get
			{
				return this.group;
			}
		}

		// Token: 0x06009638 RID: 38456
		public abstract bool Load();

		// Token: 0x06009639 RID: 38457
		public abstract void LoadSync();

		// Token: 0x0600963A RID: 38458 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void Update()
		{
		}

		// Token: 0x0600963B RID: 38459
		public abstract void Complete();

		// Token: 0x0600963C RID: 38460 RVA: 0x0038B835 File Offset: 0x00389A35
		public void WaitForComplete()
		{
			this.CompleteNow();
			LoadManager.RemoveRequest(this);
		}

		// Token: 0x0600963D RID: 38461
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract void CompleteNow();

		// Token: 0x040070AA RID: 28842
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly LoadManager.LoadGroup group;

		// Token: 0x040070AB RID: 28843
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly bool loadAsync;

		// Token: 0x040070AC RID: 28844
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool loadStarted;
	}

	// Token: 0x02001277 RID: 4727
	public class AssetBundleLoadTask : LoadManager.LoadTask
	{
		// Token: 0x17001206 RID: 4614
		// (get) Token: 0x0600963E RID: 38462 RVA: 0x00020119 File Offset: 0x0001E319
		public override bool INTERNAL_IsPending
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001207 RID: 4615
		// (get) Token: 0x0600963F RID: 38463 RVA: 0x00010E62 File Offset: 0x0000F062
		public override bool IsDone
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06009640 RID: 38464 RVA: 0x0038B843 File Offset: 0x00389A43
		public AssetBundleLoadTask(LoadManager.LoadGroup _group, bool _loadAsync, Action<AssetBundle> _callback)
			: base(_group, _loadAsync)
		{
			this.callback = _callback;
		}

		// Token: 0x06009641 RID: 38465 RVA: 0x0008806C File Offset: 0x0008626C
		public override bool Load()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06009642 RID: 38466 RVA: 0x0008806C File Offset: 0x0008626C
		public override void LoadSync()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06009643 RID: 38467 RVA: 0x0008806C File Offset: 0x0008626C
		public override void Complete()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06009644 RID: 38468 RVA: 0x0008806C File Offset: 0x0008626C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void CompleteNow()
		{
			throw new NotImplementedException();
		}

		// Token: 0x040070AD RID: 28845
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Action<AssetBundle> callback;
	}

	// Token: 0x02001278 RID: 4728
	public abstract class AssetRequestTask<T> : LoadManager.LoadTask where T : global::UnityEngine.Object
	{
		// Token: 0x17001208 RID: 4616
		// (get) Token: 0x06009645 RID: 38469 RVA: 0x0038B854 File Offset: 0x00389A54
		public override bool keepWaiting
		{
			get
			{
				return this.INTERNAL_IsPending || !this.assetRetrieved;
			}
		}

		// Token: 0x17001209 RID: 4617
		// (get) Token: 0x06009646 RID: 38470 RVA: 0x0038B869 File Offset: 0x00389A69
		public override bool IsDone
		{
			get
			{
				return this.assetRetrieved;
			}
		}

		// Token: 0x1700120A RID: 4618
		// (get) Token: 0x06009647 RID: 38471 RVA: 0x0038B871 File Offset: 0x00389A71
		public T Asset
		{
			get
			{
				return this.asset;
			}
		}

		// Token: 0x06009648 RID: 38472 RVA: 0x0038B879 File Offset: 0x00389A79
		[PublicizedFrom(EAccessModifier.Protected)]
		public AssetRequestTask(LoadManager.LoadGroup _group, bool _loadAsync, Action<T> _callback)
			: base(_group, _loadAsync)
		{
			this.callback = _callback;
		}

		// Token: 0x06009649 RID: 38473 RVA: 0x0038B88A File Offset: 0x00389A8A
		public virtual LoadManager.IAssetHandle GetHandle()
		{
			return LoadManager.DummyHandle.Instance;
		}

		// Token: 0x0600964A RID: 38474 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void Release()
		{
		}

		// Token: 0x040070AE RID: 28846
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly Action<T> callback;

		// Token: 0x040070AF RID: 28847
		[PublicizedFrom(EAccessModifier.Protected)]
		public T asset;

		// Token: 0x040070B0 RID: 28848
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool assetRetrieved;
	}

	// Token: 0x02001279 RID: 4729
	public abstract class AssetsRequestTask<T> : LoadManager.LoadTask where T : global::UnityEngine.Object
	{
		// Token: 0x1700120B RID: 4619
		// (get) Token: 0x0600964B RID: 38475 RVA: 0x0038B891 File Offset: 0x00389A91
		public override bool keepWaiting
		{
			get
			{
				return this.INTERNAL_IsPending || !this.assetsRetrieved;
			}
		}

		// Token: 0x1700120C RID: 4620
		// (get) Token: 0x0600964C RID: 38476 RVA: 0x0038B8A6 File Offset: 0x00389AA6
		public override bool IsDone
		{
			get
			{
				return this.assetsRetrieved;
			}
		}

		// Token: 0x0600964D RID: 38477 RVA: 0x0038B8AE File Offset: 0x00389AAE
		[PublicizedFrom(EAccessModifier.Protected)]
		public AssetsRequestTask(LoadManager.LoadGroup _group, bool _loadAsync)
			: base(_group, _loadAsync)
		{
		}

		// Token: 0x0600964E RID: 38478
		public abstract void CollectResults(List<T> _results);

		// Token: 0x040070B1 RID: 28849
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool assetsRetrieved;
	}

	// Token: 0x0200127A RID: 4730
	public class ResourceRequestTask<T> : LoadManager.AssetRequestTask<T> where T : global::UnityEngine.Object
	{
		// Token: 0x1700120D RID: 4621
		// (get) Token: 0x0600964F RID: 38479 RVA: 0x0038B8B8 File Offset: 0x00389AB8
		public override bool INTERNAL_IsPending
		{
			get
			{
				return this.loadAsync && !this.asyncCancelled && (!this.loadStarted || (this.request != null && !this.request.isDone));
			}
		}

		// Token: 0x06009650 RID: 38480 RVA: 0x0038B8EE File Offset: 0x00389AEE
		public ResourceRequestTask(LoadManager.LoadGroup _group, bool _loadAsync, string _assetPath, Action<T> _callback)
			: base(_group, _loadAsync, _callback)
		{
			this.assetPath = _assetPath;
		}

		// Token: 0x06009651 RID: 38481 RVA: 0x0038B901 File Offset: 0x00389B01
		public override bool Load()
		{
			this.request = Resources.LoadAsync<T>(this.assetPath);
			this.loadStarted = true;
			return this.request != null;
		}

		// Token: 0x06009652 RID: 38482 RVA: 0x0038B924 File Offset: 0x00389B24
		public override void LoadSync()
		{
			this.asset = Resources.Load<T>(this.assetPath);
			this.assetRetrieved = true;
			if (this.callback != null)
			{
				this.callback(this.asset);
			}
		}

		// Token: 0x06009653 RID: 38483 RVA: 0x0038B958 File Offset: 0x00389B58
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

		// Token: 0x06009654 RID: 38484 RVA: 0x0038B9B4 File Offset: 0x00389BB4
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

		// Token: 0x040070B2 RID: 28850
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string assetPath;

		// Token: 0x040070B3 RID: 28851
		[PublicizedFrom(EAccessModifier.Private)]
		public ResourceRequest request;

		// Token: 0x040070B4 RID: 28852
		[PublicizedFrom(EAccessModifier.Private)]
		public bool asyncCancelled;
	}

	// Token: 0x0200127B RID: 4731
	public class AssetBundleRequestTask<T> : LoadManager.AssetRequestTask<T> where T : global::UnityEngine.Object
	{
		// Token: 0x1700120E RID: 4622
		// (get) Token: 0x06009655 RID: 38485 RVA: 0x0038BA0B File Offset: 0x00389C0B
		public override bool INTERNAL_IsPending
		{
			get
			{
				return this.loadAsync && !this.asyncCancelled && (!this.loadStarted || (this.request != null && !this.request.IsDone));
			}
		}

		// Token: 0x06009656 RID: 38486 RVA: 0x0038BA41 File Offset: 0x00389C41
		public AssetBundleRequestTask(LoadManager.LoadGroup _group, bool _loadAsync, DataLoader.DataPathIdentifier _identifier, Action<T> _callback)
			: base(_group, _loadAsync, _callback)
		{
			this.identifier = _identifier;
			if (typeof(T) == typeof(Transform))
			{
				this.isGameObject = true;
			}
		}

		// Token: 0x06009657 RID: 38487 RVA: 0x0038BA78 File Offset: 0x00389C78
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

		// Token: 0x06009658 RID: 38488 RVA: 0x0038BB04 File Offset: 0x00389D04
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

		// Token: 0x06009659 RID: 38489 RVA: 0x0038BBB0 File Offset: 0x00389DB0
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

		// Token: 0x0600965A RID: 38490 RVA: 0x0038BC3C File Offset: 0x00389E3C
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

		// Token: 0x040070B5 RID: 28853
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly DataLoader.DataPathIdentifier identifier;

		// Token: 0x040070B6 RID: 28854
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool isGameObject;

		// Token: 0x040070B7 RID: 28855
		[PublicizedFrom(EAccessModifier.Private)]
		public AssetBundleManager.AssetBundleRequestTFP request;

		// Token: 0x040070B8 RID: 28856
		[PublicizedFrom(EAccessModifier.Private)]
		public bool asyncCancelled;
	}

	// Token: 0x0200127C RID: 4732
	public class AddressableRequestTask<T> : LoadManager.AssetRequestTask<T> where T : global::UnityEngine.Object
	{
		// Token: 0x1700120F RID: 4623
		// (get) Token: 0x0600965B RID: 38491 RVA: 0x0038BCC0 File Offset: 0x00389EC0
		public override bool INTERNAL_IsPending
		{
			get
			{
				return this.loadAsync && (!this.loadStarted || (this.request.IsValid() && !this.request.IsDone) || (this.gameObjectRequest.IsValid() && !this.gameObjectRequest.IsDone));
			}
		}

		// Token: 0x0600965C RID: 38492 RVA: 0x0038BD18 File Offset: 0x00389F18
		public AddressableRequestTask(LoadManager.LoadGroup _group, bool _loadAsync, object _key, Action<T> _callback)
			: base(_group, _loadAsync, _callback)
		{
			this.key = _key;
			if (typeof(T) == typeof(Transform))
			{
				this.isGameObject = true;
			}
		}

		// Token: 0x0600965D RID: 38493 RVA: 0x0038BD4D File Offset: 0x00389F4D
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

		// Token: 0x0600965E RID: 38494 RVA: 0x0038BD8B File Offset: 0x00389F8B
		public override bool Load()
		{
			this.StartRequest();
			return this.request.IsValid() || this.gameObjectRequest.IsValid();
		}

		// Token: 0x0600965F RID: 38495 RVA: 0x0038BDB0 File Offset: 0x00389FB0
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

		// Token: 0x06009660 RID: 38496 RVA: 0x0038BE38 File Offset: 0x0038A038
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

		// Token: 0x06009661 RID: 38497 RVA: 0x0038BEB5 File Offset: 0x0038A0B5
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

		// Token: 0x06009662 RID: 38498 RVA: 0x0038BED8 File Offset: 0x0038A0D8
		public override LoadManager.IAssetHandle GetHandle()
		{
			if (this.isGameObject)
			{
				return new LoadManager.AddressableAssetHandle<GameObject>(this.gameObjectRequest);
			}
			return new LoadManager.AddressableAssetHandle<T>(this.request);
		}

		// Token: 0x06009663 RID: 38499 RVA: 0x0038BEF9 File Offset: 0x0038A0F9
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

		// Token: 0x06009664 RID: 38500 RVA: 0x0038BF26 File Offset: 0x0038A126
		public static LoadManager.AddressableRequestTask<T> Empty()
		{
			return LoadManager.AddressableRequestTask<T>.emptyInstance;
		}

		// Token: 0x040070B9 RID: 28857
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly object key;

		// Token: 0x040070BA RID: 28858
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool isGameObject;

		// Token: 0x040070BB RID: 28859
		[PublicizedFrom(EAccessModifier.Private)]
		public AsyncOperationHandle<T> request;

		// Token: 0x040070BC RID: 28860
		[PublicizedFrom(EAccessModifier.Private)]
		public AsyncOperationHandle<GameObject> gameObjectRequest;

		// Token: 0x040070BD RID: 28861
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly LoadManager.EmptyAddressableRequestTask<T> emptyInstance = new LoadManager.EmptyAddressableRequestTask<T>();
	}

	// Token: 0x0200127D RID: 4733
	[PublicizedFrom(EAccessModifier.Private)]
	public class EmptyAddressableRequestTask<T> : LoadManager.AddressableRequestTask<T> where T : global::UnityEngine.Object
	{
		// Token: 0x06009666 RID: 38502 RVA: 0x0038BF39 File Offset: 0x0038A139
		public EmptyAddressableRequestTask()
			: base(null, true, null, null)
		{
		}

		// Token: 0x17001210 RID: 4624
		// (get) Token: 0x06009667 RID: 38503 RVA: 0x00010E62 File Offset: 0x0000F062
		public override bool INTERNAL_IsPending
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001211 RID: 4625
		// (get) Token: 0x06009668 RID: 38504 RVA: 0x00020119 File Offset: 0x0001E319
		public override bool IsDone
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06009669 RID: 38505 RVA: 0x00010E62 File Offset: 0x0000F062
		public override bool Load()
		{
			return false;
		}

		// Token: 0x0600966A RID: 38506 RVA: 0x000027FC File Offset: 0x000009FC
		public override void LoadSync()
		{
		}

		// Token: 0x0600966B RID: 38507 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Complete()
		{
		}

		// Token: 0x0600966C RID: 38508 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Release()
		{
		}

		// Token: 0x0600966D RID: 38509 RVA: 0x0038B88A File Offset: 0x00389A8A
		public override LoadManager.IAssetHandle GetHandle()
		{
			return LoadManager.DummyHandle.Instance;
		}
	}

	// Token: 0x0200127E RID: 4734
	public class AddressableAssetsRequestTask<T> : LoadManager.AssetsRequestTask<T> where T : global::UnityEngine.Object
	{
		// Token: 0x17001212 RID: 4626
		// (get) Token: 0x0600966E RID: 38510 RVA: 0x0038BF45 File Offset: 0x0038A145
		public override bool INTERNAL_IsPending
		{
			get
			{
				return this.loadAsync && (!this.loadStarted || !this.assetRequestStarted || (this.assetsRequest.IsValid() && !this.assetsRequest.IsDone));
			}
		}

		// Token: 0x0600966F RID: 38511 RVA: 0x0038BF80 File Offset: 0x0038A180
		public AddressableAssetsRequestTask(LoadManager.LoadGroup _group, bool _loadAsync, string _label, Func<string, bool> _addressFilter = null)
			: base(_group, _loadAsync)
		{
			this.label = _label;
			this.addressFilter = _addressFilter;
		}

		// Token: 0x06009670 RID: 38512 RVA: 0x0038BF99 File Offset: 0x0038A199
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

		// Token: 0x06009671 RID: 38513 RVA: 0x0038BFC8 File Offset: 0x0038A1C8
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

		// Token: 0x06009672 RID: 38514 RVA: 0x0038C090 File Offset: 0x0038A290
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

		// Token: 0x06009673 RID: 38515 RVA: 0x0038C0C1 File Offset: 0x0038A2C1
		public override bool Load()
		{
			this.StartLocationsRequest();
			return this.locationRequest.IsValid();
		}

		// Token: 0x06009674 RID: 38516 RVA: 0x0038C0D4 File Offset: 0x0038A2D4
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

		// Token: 0x06009675 RID: 38517 RVA: 0x0038C125 File Offset: 0x0038A325
		public override void Complete()
		{
			if (this.INTERNAL_IsPending)
			{
				throw new Exception("AddressableAssetsRequestTask still pending.");
			}
			this.assetsRetrieved = true;
		}

		// Token: 0x06009676 RID: 38518 RVA: 0x0038C141 File Offset: 0x0038A341
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

		// Token: 0x06009677 RID: 38519 RVA: 0x0038C164 File Offset: 0x0038A364
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

		// Token: 0x040070BE RID: 28862
		[PublicizedFrom(EAccessModifier.Private)]
		public string label;

		// Token: 0x040070BF RID: 28863
		[PublicizedFrom(EAccessModifier.Private)]
		public Func<string, bool> addressFilter;

		// Token: 0x040070C0 RID: 28864
		[PublicizedFrom(EAccessModifier.Private)]
		public AsyncOperationHandle<IList<IResourceLocation>> locationRequest;

		// Token: 0x040070C1 RID: 28865
		[PublicizedFrom(EAccessModifier.Private)]
		public bool assetRequestStarted;

		// Token: 0x040070C2 RID: 28866
		[PublicizedFrom(EAccessModifier.Private)]
		public AsyncOperationHandle<IList<T>> assetsRequest;
	}

	// Token: 0x0200127F RID: 4735
	public class CoroutineTask : LoadManager.LoadTask
	{
		// Token: 0x17001213 RID: 4627
		// (get) Token: 0x06009678 RID: 38520 RVA: 0x0038C1E0 File Offset: 0x0038A3E0
		public override bool INTERNAL_IsPending
		{
			get
			{
				return !this.isDone;
			}
		}

		// Token: 0x17001214 RID: 4628
		// (get) Token: 0x06009679 RID: 38521 RVA: 0x0038C1EB File Offset: 0x0038A3EB
		public override bool IsDone
		{
			get
			{
				return !this.INTERNAL_IsPending;
			}
		}

		// Token: 0x0600967A RID: 38522 RVA: 0x0038C1F6 File Offset: 0x0038A3F6
		public CoroutineTask(LoadManager.LoadGroup _group, IEnumerator _task, LoadManager.CompletionCallback _callback)
			: base(_group, true)
		{
			this.task = _task;
			this.callback = _callback;
		}

		// Token: 0x0600967B RID: 38523 RVA: 0x00020119 File Offset: 0x0001E319
		public override bool Load()
		{
			return true;
		}

		// Token: 0x0600967C RID: 38524 RVA: 0x0038C20E File Offset: 0x0038A40E
		public override void LoadSync()
		{
			throw new Exception("CoroutineTask doesn't support synchronous loading.");
		}

		// Token: 0x0600967D RID: 38525 RVA: 0x0038C21A File Offset: 0x0038A41A
		public override void Update()
		{
			this.isDone = !this.task.MoveNext();
		}

		// Token: 0x0600967E RID: 38526 RVA: 0x0038C230 File Offset: 0x0038A430
		public override void Complete()
		{
			if (this.callback != null)
			{
				this.callback();
			}
		}

		// Token: 0x0600967F RID: 38527 RVA: 0x0038C20E File Offset: 0x0038A40E
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void CompleteNow()
		{
			throw new Exception("CoroutineTask doesn't support synchronous loading.");
		}

		// Token: 0x040070C3 RID: 28867
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly IEnumerator task;

		// Token: 0x040070C4 RID: 28868
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly LoadManager.CompletionCallback callback;

		// Token: 0x040070C5 RID: 28869
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isDone;
	}

	// Token: 0x02001280 RID: 4736
	public class FileLoadTask : LoadManager.LoadTask
	{
		// Token: 0x17001215 RID: 4629
		// (get) Token: 0x06009680 RID: 38528 RVA: 0x0038C245 File Offset: 0x0038A445
		public override bool INTERNAL_IsPending
		{
			get
			{
				return !this.isDone;
			}
		}

		// Token: 0x17001216 RID: 4630
		// (get) Token: 0x06009681 RID: 38529 RVA: 0x0038C1EB File Offset: 0x0038A3EB
		public override bool IsDone
		{
			get
			{
				return !this.INTERNAL_IsPending;
			}
		}

		// Token: 0x06009682 RID: 38530 RVA: 0x0038C252 File Offset: 0x0038A452
		public FileLoadTask(LoadManager.LoadGroup _group, bool _loadAsync, string _path, LoadManager.FileLoadCallback _callback)
			: base(_group, _loadAsync)
		{
			this.path = _path;
			this.callback = _callback;
		}

		// Token: 0x06009683 RID: 38531 RVA: 0x0038C26B File Offset: 0x0038A46B
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

		// Token: 0x06009684 RID: 38532 RVA: 0x0038C290 File Offset: 0x0038A490
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

		// Token: 0x06009685 RID: 38533 RVA: 0x0038C30C File Offset: 0x0038A50C
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

		// Token: 0x06009686 RID: 38534 RVA: 0x0038C344 File Offset: 0x0038A544
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

		// Token: 0x040070C6 RID: 28870
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string path;

		// Token: 0x040070C7 RID: 28871
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly LoadManager.FileLoadCallback callback;

		// Token: 0x040070C8 RID: 28872
		[PublicizedFrom(EAccessModifier.Private)]
		public byte[] content;

		// Token: 0x040070C9 RID: 28873
		[PublicizedFrom(EAccessModifier.Private)]
		public volatile bool isDone;

		// Token: 0x040070CA RID: 28874
		[PublicizedFrom(EAccessModifier.Private)]
		public ThreadManager.TaskInfo asyncLoadTask;

		// Token: 0x040070CB RID: 28875
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isCompleted;
	}

	// Token: 0x02001281 RID: 4737
	public interface IAssetHandle
	{
		// Token: 0x06009688 RID: 38536
		LoadManager.IAssetHandle Copy();

		// Token: 0x06009689 RID: 38537
		void Release();
	}

	// Token: 0x02001282 RID: 4738
	public class AddressableAssetHandle<T> : LoadManager.IAssetHandle where T : global::UnityEngine.Object
	{
		// Token: 0x0600968A RID: 38538 RVA: 0x0038C3EC File Offset: 0x0038A5EC
		public AddressableAssetHandle(AsyncOperationHandle<T> _handle)
		{
			this.handle = Addressables.ResourceManager.Acquire<T>(_handle);
		}

		// Token: 0x0600968B RID: 38539 RVA: 0x0038C405 File Offset: 0x0038A605
		public LoadManager.IAssetHandle Copy()
		{
			return new LoadManager.AddressableAssetHandle<T>(this.handle);
		}

		// Token: 0x0600968C RID: 38540 RVA: 0x0038C412 File Offset: 0x0038A612
		public void Release()
		{
			this.handle.Release();
		}

		// Token: 0x0600968D RID: 38541 RVA: 0x0038C41F File Offset: 0x0038A61F
		public override string ToString()
		{
			return this.handle.DebugName ?? "";
		}

		// Token: 0x040070CC RID: 28876
		[PublicizedFrom(EAccessModifier.Private)]
		public AsyncOperationHandle<T> handle;
	}

	// Token: 0x02001283 RID: 4739
	public class DummyHandle : LoadManager.IAssetHandle
	{
		// Token: 0x0600968E RID: 38542 RVA: 0x0038B88A File Offset: 0x00389A8A
		public LoadManager.IAssetHandle Copy()
		{
			return LoadManager.DummyHandle.Instance;
		}

		// Token: 0x0600968F RID: 38543 RVA: 0x000027FC File Offset: 0x000009FC
		public void Release()
		{
		}

		// Token: 0x040070CD RID: 28877
		public static LoadManager.DummyHandle Instance = new LoadManager.DummyHandle();
	}
}

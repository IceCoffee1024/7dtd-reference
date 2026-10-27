using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

// Token: 0x02001284 RID: 4740
public static class LoadManager
{
	// Token: 0x1700120C RID: 4620
	// (get) Token: 0x06009667 RID: 38503 RVA: 0x0038D633 File Offset: 0x0038B833
	public static bool IsInitialised
	{
		get
		{
			return LoadManager.addressablesCaseMap != null;
		}
	}

	// Token: 0x06009668 RID: 38504 RVA: 0x0038D640 File Offset: 0x0038B840
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

	// Token: 0x06009669 RID: 38505 RVA: 0x0038D738 File Offset: 0x0038B938
	public static void InitSync()
	{
		LoadManager.Init();
		LoadManager.forceLoadSync = true;
	}

	// Token: 0x0600966A RID: 38506 RVA: 0x0038D748 File Offset: 0x0038B948
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

	// Token: 0x0600966B RID: 38507 RVA: 0x0038D800 File Offset: 0x0038BA00
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

	// Token: 0x0600966C RID: 38508 RVA: 0x0038D82D File Offset: 0x0038BA2D
	[PublicizedFrom(EAccessModifier.Private)]
	public static void RemoveRequest(LoadManager.LoadTask _request)
	{
		if (LoadManager.loadRequests.Remove(_request))
		{
			_request.Group.DecrementPending();
		}
	}

	// Token: 0x0600966D RID: 38509 RVA: 0x0038D847 File Offset: 0x0038BA47
	public static void Destroy()
	{
		LoadManager.loadRequests.Clear();
	}

	// Token: 0x0600966E RID: 38510 RVA: 0x0038D853 File Offset: 0x0038BA53
	public static LoadManager.LoadGroup CreateGroup()
	{
		return new LoadManager.LoadGroup(LoadManager.rootGroup);
	}

	// Token: 0x0600966F RID: 38511 RVA: 0x0038D85F File Offset: 0x0038BA5F
	public static LoadManager.LoadGroup CreateGroup(LoadManager.LoadGroup _parent)
	{
		return new LoadManager.LoadGroup(_parent);
	}

	// Token: 0x06009670 RID: 38512 RVA: 0x0038D868 File Offset: 0x0038BA68
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

	// Token: 0x06009671 RID: 38513 RVA: 0x0038D89C File Offset: 0x0038BA9C
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

	// Token: 0x06009672 RID: 38514 RVA: 0x0038D90A File Offset: 0x0038BB0A
	public static LoadManager.AssetRequestTask<T> LoadAsset<T>(string _uri, Action<T> _callback = null, LoadManager.LoadGroup _lg = null, bool _deferLoading = false, bool _loadSync = false, bool _ignoreDlcEntitlements = false) where T : global::UnityEngine.Object
	{
		return LoadManager.LoadAsset<T>(DataLoader.ParseDataPathIdentifier(_uri), _callback, _lg, _deferLoading, _loadSync, _ignoreDlcEntitlements);
	}

	// Token: 0x06009673 RID: 38515 RVA: 0x0038D91E File Offset: 0x0038BB1E
	public static LoadManager.AssetRequestTask<T> LoadAsset<T>(string _bundlePath, string _assetName, Action<T> _callback = null, LoadManager.LoadGroup _lg = null, bool _deferLoading = false, bool _loadSync = false) where T : global::UnityEngine.Object
	{
		return LoadManager.LoadAsset<T>(new DataLoader.DataPathIdentifier(_assetName, _bundlePath, false), _callback, _lg, _deferLoading, _loadSync, false);
	}

	// Token: 0x06009674 RID: 38516 RVA: 0x0038D934 File Offset: 0x0038BB34
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

	// Token: 0x06009675 RID: 38517 RVA: 0x0038D978 File Offset: 0x0038BB78
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

	// Token: 0x06009676 RID: 38518 RVA: 0x0038D9A8 File Offset: 0x0038BBA8
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

	// Token: 0x06009677 RID: 38519 RVA: 0x0038DA20 File Offset: 0x0038BC20
	public static LoadManager.AddressableRequestTask<T> LoadAssetFromAddressables<T>(string _folderAddress, string _assetPath, Action<T> _callback = null, LoadManager.LoadGroup _lg = null, bool _deferLoading = false, bool _loadSync = false, bool _ignoreDlcEntitlements = false) where T : global::UnityEngine.Object
	{
		return LoadManager.LoadAssetFromAddressables<T>(_folderAddress + "/" + _assetPath, _callback, _lg, _deferLoading, _loadSync, _ignoreDlcEntitlements);
	}

	// Token: 0x06009678 RID: 38520 RVA: 0x0038DA3C File Offset: 0x0038BC3C
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

	// Token: 0x06009679 RID: 38521 RVA: 0x0038DA6C File Offset: 0x0038BC6C
	public static void ReleaseAddressable<T>(T _obj)
	{
		Addressables.Release<T>(_obj);
	}

	// Token: 0x0600967A RID: 38522 RVA: 0x0038DA74 File Offset: 0x0038BC74
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

	// Token: 0x0600967B RID: 38523 RVA: 0x0038DAA4 File Offset: 0x0038BCA4
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

	// Token: 0x0600967C RID: 38524 RVA: 0x0038DAB4 File Offset: 0x0038BCB4
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

	// Token: 0x0600967D RID: 38525 RVA: 0x0038DB1C File Offset: 0x0038BD1C
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

	// Token: 0x0600967E RID: 38526 RVA: 0x0038DB54 File Offset: 0x0038BD54
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

	// Token: 0x040070E3 RID: 28899
	[PublicizedFrom(EAccessModifier.Private)]
	public static Action<LoadManager.LoadTask> updateRequestAction;

	// Token: 0x040070E4 RID: 28900
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<string, string> addressablesCaseMap;

	// Token: 0x040070E5 RID: 28901
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool forceLoadSync;

	// Token: 0x040070E6 RID: 28902
	[PublicizedFrom(EAccessModifier.Private)]
	public static LoadManager.LoadGroup rootGroup;

	// Token: 0x040070E7 RID: 28903
	[PublicizedFrom(EAccessModifier.Private)]
	public static WorkBatch<LoadManager.LoadTask> loadRequests;

	// Token: 0x040070E8 RID: 28904
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<LoadManager.LoadTask> deferedLoadRequests;

	// Token: 0x02001285 RID: 4741
	// (Invoke) Token: 0x06009680 RID: 38528
	public delegate void CompletionCallback();

	// Token: 0x02001286 RID: 4742
	// (Invoke) Token: 0x06009684 RID: 38532
	public delegate void FileLoadCallback(byte[] _content);

	// Token: 0x02001287 RID: 4743
	public class LoadGroup
	{
		// Token: 0x1700120D RID: 4621
		// (get) Token: 0x06009687 RID: 38535 RVA: 0x0038DB8E File Offset: 0x0038BD8E
		public bool Pending
		{
			get
			{
				return Interlocked.CompareExchange(ref this.pending, 0, 0) != 0;
			}
		}

		// Token: 0x06009688 RID: 38536 RVA: 0x0038DBA0 File Offset: 0x0038BDA0
		[PublicizedFrom(EAccessModifier.Internal)]
		public LoadGroup(LoadManager.LoadGroup _parent)
		{
			this.parent = _parent;
			this.pending = 0;
		}

		// Token: 0x06009689 RID: 38537 RVA: 0x0038DBB8 File Offset: 0x0038BDB8
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

		// Token: 0x0600968A RID: 38538 RVA: 0x0038DBE8 File Offset: 0x0038BDE8
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

		// Token: 0x040070E9 RID: 28905
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly LoadManager.LoadGroup parent;

		// Token: 0x040070EA RID: 28906
		[PublicizedFrom(EAccessModifier.Private)]
		public int pending;
	}

	// Token: 0x02001288 RID: 4744
	public abstract class LoadTask : CustomYieldInstruction
	{
		// Token: 0x0600968B RID: 38539 RVA: 0x0038DC17 File Offset: 0x0038BE17
		[PublicizedFrom(EAccessModifier.Protected)]
		public LoadTask(LoadManager.LoadGroup _group, bool _loadAsync)
		{
			this.group = _group;
			this.loadAsync = _loadAsync;
		}

		// Token: 0x1700120E RID: 4622
		// (get) Token: 0x0600968C RID: 38540
		public abstract bool IsDone { get; }

		// Token: 0x1700120F RID: 4623
		// (get) Token: 0x0600968D RID: 38541
		public abstract bool INTERNAL_IsPending { get; }

		// Token: 0x17001210 RID: 4624
		// (get) Token: 0x0600968E RID: 38542 RVA: 0x0038DC2D File Offset: 0x0038BE2D
		public override bool keepWaiting
		{
			get
			{
				return this.INTERNAL_IsPending;
			}
		}

		// Token: 0x17001211 RID: 4625
		// (get) Token: 0x0600968F RID: 38543 RVA: 0x0038DC35 File Offset: 0x0038BE35
		public LoadManager.LoadGroup Group
		{
			get
			{
				return this.group;
			}
		}

		// Token: 0x06009690 RID: 38544
		public abstract bool Load();

		// Token: 0x06009691 RID: 38545
		public abstract void LoadSync();

		// Token: 0x06009692 RID: 38546 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void Update()
		{
		}

		// Token: 0x06009693 RID: 38547
		public abstract void Complete();

		// Token: 0x06009694 RID: 38548 RVA: 0x0038DC3D File Offset: 0x0038BE3D
		public void WaitForComplete()
		{
			this.CompleteNow();
			LoadManager.RemoveRequest(this);
		}

		// Token: 0x06009695 RID: 38549
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract void CompleteNow();

		// Token: 0x040070EB RID: 28907
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly LoadManager.LoadGroup group;

		// Token: 0x040070EC RID: 28908
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly bool loadAsync;

		// Token: 0x040070ED RID: 28909
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool loadStarted;
	}

	// Token: 0x02001289 RID: 4745
	public class AssetBundleLoadTask : LoadManager.LoadTask
	{
		// Token: 0x17001212 RID: 4626
		// (get) Token: 0x06009696 RID: 38550 RVA: 0x00020119 File Offset: 0x0001E319
		public override bool INTERNAL_IsPending
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001213 RID: 4627
		// (get) Token: 0x06009697 RID: 38551 RVA: 0x00010E62 File Offset: 0x0000F062
		public override bool IsDone
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06009698 RID: 38552 RVA: 0x0038DC4B File Offset: 0x0038BE4B
		public AssetBundleLoadTask(LoadManager.LoadGroup _group, bool _loadAsync, Action<AssetBundle> _callback)
			: base(_group, _loadAsync)
		{
			this.callback = _callback;
		}

		// Token: 0x06009699 RID: 38553 RVA: 0x000882A0 File Offset: 0x000864A0
		public override bool Load()
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600969A RID: 38554 RVA: 0x000882A0 File Offset: 0x000864A0
		public override void LoadSync()
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600969B RID: 38555 RVA: 0x000882A0 File Offset: 0x000864A0
		public override void Complete()
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600969C RID: 38556 RVA: 0x000882A0 File Offset: 0x000864A0
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void CompleteNow()
		{
			throw new NotImplementedException();
		}

		// Token: 0x040070EE RID: 28910
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Action<AssetBundle> callback;
	}

	// Token: 0x0200128A RID: 4746
	public abstract class AssetRequestTask<T> : LoadManager.LoadTask where T : global::UnityEngine.Object
	{
		// Token: 0x17001214 RID: 4628
		// (get) Token: 0x0600969D RID: 38557 RVA: 0x0038DC5C File Offset: 0x0038BE5C
		public override bool keepWaiting
		{
			get
			{
				return this.INTERNAL_IsPending || !this.assetRetrieved;
			}
		}

		// Token: 0x17001215 RID: 4629
		// (get) Token: 0x0600969E RID: 38558 RVA: 0x0038DC71 File Offset: 0x0038BE71
		public override bool IsDone
		{
			get
			{
				return this.assetRetrieved;
			}
		}

		// Token: 0x17001216 RID: 4630
		// (get) Token: 0x0600969F RID: 38559 RVA: 0x0038DC79 File Offset: 0x0038BE79
		public T Asset
		{
			get
			{
				return this.asset;
			}
		}

		// Token: 0x060096A0 RID: 38560 RVA: 0x0038DC81 File Offset: 0x0038BE81
		[PublicizedFrom(EAccessModifier.Protected)]
		public AssetRequestTask(LoadManager.LoadGroup _group, bool _loadAsync, Action<T> _callback)
			: base(_group, _loadAsync)
		{
			this.callback = _callback;
		}

		// Token: 0x060096A1 RID: 38561 RVA: 0x0038DC92 File Offset: 0x0038BE92
		public virtual LoadManager.IAssetHandle GetHandle()
		{
			return LoadManager.DummyHandle.Instance;
		}

		// Token: 0x060096A2 RID: 38562 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void Release()
		{
		}

		// Token: 0x040070EF RID: 28911
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly Action<T> callback;

		// Token: 0x040070F0 RID: 28912
		[PublicizedFrom(EAccessModifier.Protected)]
		public T asset;

		// Token: 0x040070F1 RID: 28913
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool assetRetrieved;
	}

	// Token: 0x0200128B RID: 4747
	public abstract class AssetsRequestTask<T> : LoadManager.LoadTask where T : global::UnityEngine.Object
	{
		// Token: 0x17001217 RID: 4631
		// (get) Token: 0x060096A3 RID: 38563 RVA: 0x0038DC99 File Offset: 0x0038BE99
		public override bool keepWaiting
		{
			get
			{
				return this.INTERNAL_IsPending || !this.assetsRetrieved;
			}
		}

		// Token: 0x17001218 RID: 4632
		// (get) Token: 0x060096A4 RID: 38564 RVA: 0x0038DCAE File Offset: 0x0038BEAE
		public override bool IsDone
		{
			get
			{
				return this.assetsRetrieved;
			}
		}

		// Token: 0x060096A5 RID: 38565 RVA: 0x0038DCB6 File Offset: 0x0038BEB6
		[PublicizedFrom(EAccessModifier.Protected)]
		public AssetsRequestTask(LoadManager.LoadGroup _group, bool _loadAsync)
			: base(_group, _loadAsync)
		{
		}

		// Token: 0x060096A6 RID: 38566
		public abstract void CollectResults(List<T> _results);

		// Token: 0x040070F2 RID: 28914
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool assetsRetrieved;
	}

	// Token: 0x0200128C RID: 4748
	public class ResourceRequestTask<T> : LoadManager.AssetRequestTask<T> where T : global::UnityEngine.Object
	{
		// Token: 0x17001219 RID: 4633
		// (get) Token: 0x060096A7 RID: 38567 RVA: 0x0038DCC0 File Offset: 0x0038BEC0
		public override bool INTERNAL_IsPending
		{
			get
			{
				return this.loadAsync && !this.asyncCancelled && (!this.loadStarted || (this.request != null && !this.request.isDone));
			}
		}

		// Token: 0x060096A8 RID: 38568 RVA: 0x0038DCF6 File Offset: 0x0038BEF6
		public ResourceRequestTask(LoadManager.LoadGroup _group, bool _loadAsync, string _assetPath, Action<T> _callback)
			: base(_group, _loadAsync, _callback)
		{
			this.assetPath = _assetPath;
		}

		// Token: 0x060096A9 RID: 38569 RVA: 0x0038DD09 File Offset: 0x0038BF09
		public override bool Load()
		{
			this.request = Resources.LoadAsync<T>(this.assetPath);
			this.loadStarted = true;
			return this.request != null;
		}

		// Token: 0x060096AA RID: 38570 RVA: 0x0038DD2C File Offset: 0x0038BF2C
		public override void LoadSync()
		{
			this.asset = Resources.Load<T>(this.assetPath);
			this.assetRetrieved = true;
			if (this.callback != null)
			{
				this.callback(this.asset);
			}
		}

		// Token: 0x060096AB RID: 38571 RVA: 0x0038DD60 File Offset: 0x0038BF60
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

		// Token: 0x060096AC RID: 38572 RVA: 0x0038DDBC File Offset: 0x0038BFBC
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

		// Token: 0x040070F3 RID: 28915
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string assetPath;

		// Token: 0x040070F4 RID: 28916
		[PublicizedFrom(EAccessModifier.Private)]
		public ResourceRequest request;

		// Token: 0x040070F5 RID: 28917
		[PublicizedFrom(EAccessModifier.Private)]
		public bool asyncCancelled;
	}

	// Token: 0x0200128D RID: 4749
	public class AssetBundleRequestTask<T> : LoadManager.AssetRequestTask<T> where T : global::UnityEngine.Object
	{
		// Token: 0x1700121A RID: 4634
		// (get) Token: 0x060096AD RID: 38573 RVA: 0x0038DE13 File Offset: 0x0038C013
		public override bool INTERNAL_IsPending
		{
			get
			{
				return this.loadAsync && !this.asyncCancelled && (!this.loadStarted || (this.request != null && !this.request.IsDone));
			}
		}

		// Token: 0x060096AE RID: 38574 RVA: 0x0038DE49 File Offset: 0x0038C049
		public AssetBundleRequestTask(LoadManager.LoadGroup _group, bool _loadAsync, DataLoader.DataPathIdentifier _identifier, Action<T> _callback)
			: base(_group, _loadAsync, _callback)
		{
			this.identifier = _identifier;
			if (typeof(T) == typeof(Transform))
			{
				this.isGameObject = true;
			}
		}

		// Token: 0x060096AF RID: 38575 RVA: 0x0038DE80 File Offset: 0x0038C080
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

		// Token: 0x060096B0 RID: 38576 RVA: 0x0038DF0C File Offset: 0x0038C10C
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

		// Token: 0x060096B1 RID: 38577 RVA: 0x0038DFB8 File Offset: 0x0038C1B8
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

		// Token: 0x060096B2 RID: 38578 RVA: 0x0038E044 File Offset: 0x0038C244
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

		// Token: 0x040070F6 RID: 28918
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly DataLoader.DataPathIdentifier identifier;

		// Token: 0x040070F7 RID: 28919
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool isGameObject;

		// Token: 0x040070F8 RID: 28920
		[PublicizedFrom(EAccessModifier.Private)]
		public AssetBundleManager.AssetBundleRequestTFP request;

		// Token: 0x040070F9 RID: 28921
		[PublicizedFrom(EAccessModifier.Private)]
		public bool asyncCancelled;
	}

	// Token: 0x0200128E RID: 4750
	public class AddressableRequestTask<T> : LoadManager.AssetRequestTask<T> where T : global::UnityEngine.Object
	{
		// Token: 0x1700121B RID: 4635
		// (get) Token: 0x060096B3 RID: 38579 RVA: 0x0038E0C8 File Offset: 0x0038C2C8
		public override bool INTERNAL_IsPending
		{
			get
			{
				return this.loadAsync && (!this.loadStarted || (this.request.IsValid() && !this.request.IsDone) || (this.gameObjectRequest.IsValid() && !this.gameObjectRequest.IsDone));
			}
		}

		// Token: 0x060096B4 RID: 38580 RVA: 0x0038E120 File Offset: 0x0038C320
		public AddressableRequestTask(LoadManager.LoadGroup _group, bool _loadAsync, object _key, Action<T> _callback)
			: base(_group, _loadAsync, _callback)
		{
			this.key = _key;
			if (typeof(T) == typeof(Transform))
			{
				this.isGameObject = true;
			}
		}

		// Token: 0x060096B5 RID: 38581 RVA: 0x0038E155 File Offset: 0x0038C355
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

		// Token: 0x060096B6 RID: 38582 RVA: 0x0038E193 File Offset: 0x0038C393
		public override bool Load()
		{
			this.StartRequest();
			return this.request.IsValid() || this.gameObjectRequest.IsValid();
		}

		// Token: 0x060096B7 RID: 38583 RVA: 0x0038E1B8 File Offset: 0x0038C3B8
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

		// Token: 0x060096B8 RID: 38584 RVA: 0x0038E240 File Offset: 0x0038C440
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

		// Token: 0x060096B9 RID: 38585 RVA: 0x0038E2BD File Offset: 0x0038C4BD
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

		// Token: 0x060096BA RID: 38586 RVA: 0x0038E2E0 File Offset: 0x0038C4E0
		public override LoadManager.IAssetHandle GetHandle()
		{
			if (this.isGameObject)
			{
				return new LoadManager.AddressableAssetHandle<GameObject>(this.gameObjectRequest);
			}
			return new LoadManager.AddressableAssetHandle<T>(this.request);
		}

		// Token: 0x060096BB RID: 38587 RVA: 0x0038E301 File Offset: 0x0038C501
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

		// Token: 0x060096BC RID: 38588 RVA: 0x0038E32E File Offset: 0x0038C52E
		public static LoadManager.AddressableRequestTask<T> Empty()
		{
			return LoadManager.AddressableRequestTask<T>.emptyInstance;
		}

		// Token: 0x040070FA RID: 28922
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly object key;

		// Token: 0x040070FB RID: 28923
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool isGameObject;

		// Token: 0x040070FC RID: 28924
		[PublicizedFrom(EAccessModifier.Private)]
		public AsyncOperationHandle<T> request;

		// Token: 0x040070FD RID: 28925
		[PublicizedFrom(EAccessModifier.Private)]
		public AsyncOperationHandle<GameObject> gameObjectRequest;

		// Token: 0x040070FE RID: 28926
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly LoadManager.EmptyAddressableRequestTask<T> emptyInstance = new LoadManager.EmptyAddressableRequestTask<T>();
	}

	// Token: 0x0200128F RID: 4751
	[PublicizedFrom(EAccessModifier.Private)]
	public class EmptyAddressableRequestTask<T> : LoadManager.AddressableRequestTask<T> where T : global::UnityEngine.Object
	{
		// Token: 0x060096BE RID: 38590 RVA: 0x0038E341 File Offset: 0x0038C541
		public EmptyAddressableRequestTask()
			: base(null, true, null, null)
		{
		}

		// Token: 0x1700121C RID: 4636
		// (get) Token: 0x060096BF RID: 38591 RVA: 0x00010E62 File Offset: 0x0000F062
		public override bool INTERNAL_IsPending
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700121D RID: 4637
		// (get) Token: 0x060096C0 RID: 38592 RVA: 0x00020119 File Offset: 0x0001E319
		public override bool IsDone
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060096C1 RID: 38593 RVA: 0x00010E62 File Offset: 0x0000F062
		public override bool Load()
		{
			return false;
		}

		// Token: 0x060096C2 RID: 38594 RVA: 0x000027FC File Offset: 0x000009FC
		public override void LoadSync()
		{
		}

		// Token: 0x060096C3 RID: 38595 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Complete()
		{
		}

		// Token: 0x060096C4 RID: 38596 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Release()
		{
		}

		// Token: 0x060096C5 RID: 38597 RVA: 0x0038DC92 File Offset: 0x0038BE92
		public override LoadManager.IAssetHandle GetHandle()
		{
			return LoadManager.DummyHandle.Instance;
		}
	}

	// Token: 0x02001290 RID: 4752
	public class AddressableAssetsRequestTask<T> : LoadManager.AssetsRequestTask<T> where T : global::UnityEngine.Object
	{
		// Token: 0x1700121E RID: 4638
		// (get) Token: 0x060096C6 RID: 38598 RVA: 0x0038E34D File Offset: 0x0038C54D
		public override bool INTERNAL_IsPending
		{
			get
			{
				return this.loadAsync && (!this.loadStarted || !this.assetRequestStarted || (this.assetsRequest.IsValid() && !this.assetsRequest.IsDone));
			}
		}

		// Token: 0x060096C7 RID: 38599 RVA: 0x0038E388 File Offset: 0x0038C588
		public AddressableAssetsRequestTask(LoadManager.LoadGroup _group, bool _loadAsync, string _label, Func<string, bool> _addressFilter = null)
			: base(_group, _loadAsync)
		{
			this.label = _label;
			this.addressFilter = _addressFilter;
		}

		// Token: 0x060096C8 RID: 38600 RVA: 0x0038E3A1 File Offset: 0x0038C5A1
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

		// Token: 0x060096C9 RID: 38601 RVA: 0x0038E3D0 File Offset: 0x0038C5D0
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

		// Token: 0x060096CA RID: 38602 RVA: 0x0038E498 File Offset: 0x0038C698
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

		// Token: 0x060096CB RID: 38603 RVA: 0x0038E4C9 File Offset: 0x0038C6C9
		public override bool Load()
		{
			this.StartLocationsRequest();
			return this.locationRequest.IsValid();
		}

		// Token: 0x060096CC RID: 38604 RVA: 0x0038E4DC File Offset: 0x0038C6DC
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

		// Token: 0x060096CD RID: 38605 RVA: 0x0038E52D File Offset: 0x0038C72D
		public override void Complete()
		{
			if (this.INTERNAL_IsPending)
			{
				throw new Exception("AddressableAssetsRequestTask still pending.");
			}
			this.assetsRetrieved = true;
		}

		// Token: 0x060096CE RID: 38606 RVA: 0x0038E549 File Offset: 0x0038C749
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

		// Token: 0x060096CF RID: 38607 RVA: 0x0038E56C File Offset: 0x0038C76C
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

		// Token: 0x040070FF RID: 28927
		[PublicizedFrom(EAccessModifier.Private)]
		public string label;

		// Token: 0x04007100 RID: 28928
		[PublicizedFrom(EAccessModifier.Private)]
		public Func<string, bool> addressFilter;

		// Token: 0x04007101 RID: 28929
		[PublicizedFrom(EAccessModifier.Private)]
		public AsyncOperationHandle<IList<IResourceLocation>> locationRequest;

		// Token: 0x04007102 RID: 28930
		[PublicizedFrom(EAccessModifier.Private)]
		public bool assetRequestStarted;

		// Token: 0x04007103 RID: 28931
		[PublicizedFrom(EAccessModifier.Private)]
		public AsyncOperationHandle<IList<T>> assetsRequest;
	}

	// Token: 0x02001291 RID: 4753
	public class CoroutineTask : LoadManager.LoadTask
	{
		// Token: 0x1700121F RID: 4639
		// (get) Token: 0x060096D0 RID: 38608 RVA: 0x0038E5E8 File Offset: 0x0038C7E8
		public override bool INTERNAL_IsPending
		{
			get
			{
				return !this.isDone;
			}
		}

		// Token: 0x17001220 RID: 4640
		// (get) Token: 0x060096D1 RID: 38609 RVA: 0x0038E5F3 File Offset: 0x0038C7F3
		public override bool IsDone
		{
			get
			{
				return !this.INTERNAL_IsPending;
			}
		}

		// Token: 0x060096D2 RID: 38610 RVA: 0x0038E5FE File Offset: 0x0038C7FE
		public CoroutineTask(LoadManager.LoadGroup _group, IEnumerator _task, LoadManager.CompletionCallback _callback)
			: base(_group, true)
		{
			this.task = _task;
			this.callback = _callback;
		}

		// Token: 0x060096D3 RID: 38611 RVA: 0x00020119 File Offset: 0x0001E319
		public override bool Load()
		{
			return true;
		}

		// Token: 0x060096D4 RID: 38612 RVA: 0x0038E616 File Offset: 0x0038C816
		public override void LoadSync()
		{
			throw new Exception("CoroutineTask doesn't support synchronous loading.");
		}

		// Token: 0x060096D5 RID: 38613 RVA: 0x0038E622 File Offset: 0x0038C822
		public override void Update()
		{
			this.isDone = !this.task.MoveNext();
		}

		// Token: 0x060096D6 RID: 38614 RVA: 0x0038E638 File Offset: 0x0038C838
		public override void Complete()
		{
			if (this.callback != null)
			{
				this.callback();
			}
		}

		// Token: 0x060096D7 RID: 38615 RVA: 0x0038E616 File Offset: 0x0038C816
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void CompleteNow()
		{
			throw new Exception("CoroutineTask doesn't support synchronous loading.");
		}

		// Token: 0x04007104 RID: 28932
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly IEnumerator task;

		// Token: 0x04007105 RID: 28933
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly LoadManager.CompletionCallback callback;

		// Token: 0x04007106 RID: 28934
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isDone;
	}

	// Token: 0x02001292 RID: 4754
	public class FileLoadTask : LoadManager.LoadTask
	{
		// Token: 0x17001221 RID: 4641
		// (get) Token: 0x060096D8 RID: 38616 RVA: 0x0038E64D File Offset: 0x0038C84D
		public override bool INTERNAL_IsPending
		{
			get
			{
				return !this.isDone;
			}
		}

		// Token: 0x17001222 RID: 4642
		// (get) Token: 0x060096D9 RID: 38617 RVA: 0x0038E5F3 File Offset: 0x0038C7F3
		public override bool IsDone
		{
			get
			{
				return !this.INTERNAL_IsPending;
			}
		}

		// Token: 0x060096DA RID: 38618 RVA: 0x0038E65A File Offset: 0x0038C85A
		public FileLoadTask(LoadManager.LoadGroup _group, bool _loadAsync, string _path, LoadManager.FileLoadCallback _callback)
			: base(_group, _loadAsync)
		{
			this.path = _path;
			this.callback = _callback;
		}

		// Token: 0x060096DB RID: 38619 RVA: 0x0038E673 File Offset: 0x0038C873
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

		// Token: 0x060096DC RID: 38620 RVA: 0x0038E698 File Offset: 0x0038C898
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

		// Token: 0x060096DD RID: 38621 RVA: 0x0038E714 File Offset: 0x0038C914
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

		// Token: 0x060096DE RID: 38622 RVA: 0x0038E74C File Offset: 0x0038C94C
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

		// Token: 0x04007107 RID: 28935
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string path;

		// Token: 0x04007108 RID: 28936
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly LoadManager.FileLoadCallback callback;

		// Token: 0x04007109 RID: 28937
		[PublicizedFrom(EAccessModifier.Private)]
		public byte[] content;

		// Token: 0x0400710A RID: 28938
		[PublicizedFrom(EAccessModifier.Private)]
		public volatile bool isDone;

		// Token: 0x0400710B RID: 28939
		[PublicizedFrom(EAccessModifier.Private)]
		public ThreadManager.TaskInfo asyncLoadTask;

		// Token: 0x0400710C RID: 28940
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isCompleted;
	}

	// Token: 0x02001293 RID: 4755
	public interface IAssetHandle
	{
		// Token: 0x060096E0 RID: 38624
		LoadManager.IAssetHandle Copy();

		// Token: 0x060096E1 RID: 38625
		void Release();
	}

	// Token: 0x02001294 RID: 4756
	public class AddressableAssetHandle<T> : LoadManager.IAssetHandle where T : global::UnityEngine.Object
	{
		// Token: 0x060096E2 RID: 38626 RVA: 0x0038E7F4 File Offset: 0x0038C9F4
		public AddressableAssetHandle(AsyncOperationHandle<T> _handle)
		{
			this.handle = Addressables.ResourceManager.Acquire<T>(_handle);
		}

		// Token: 0x060096E3 RID: 38627 RVA: 0x0038E80D File Offset: 0x0038CA0D
		public LoadManager.IAssetHandle Copy()
		{
			return new LoadManager.AddressableAssetHandle<T>(this.handle);
		}

		// Token: 0x060096E4 RID: 38628 RVA: 0x0038E81A File Offset: 0x0038CA1A
		public void Release()
		{
			this.handle.Release();
		}

		// Token: 0x060096E5 RID: 38629 RVA: 0x0038E827 File Offset: 0x0038CA27
		public override string ToString()
		{
			return this.handle.DebugName ?? "";
		}

		// Token: 0x0400710D RID: 28941
		[PublicizedFrom(EAccessModifier.Private)]
		public AsyncOperationHandle<T> handle;
	}

	// Token: 0x02001295 RID: 4757
	public class DummyHandle : LoadManager.IAssetHandle
	{
		// Token: 0x060096E6 RID: 38630 RVA: 0x0038DC92 File Offset: 0x0038BE92
		public LoadManager.IAssetHandle Copy()
		{
			return LoadManager.DummyHandle.Instance;
		}

		// Token: 0x060096E7 RID: 38631 RVA: 0x000027FC File Offset: 0x000009FC
		public void Release()
		{
		}

		// Token: 0x0400710E RID: 28942
		public static LoadManager.DummyHandle Instance = new LoadManager.DummyHandle();
	}
}

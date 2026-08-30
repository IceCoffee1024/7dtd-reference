using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x02001385 RID: 4997
public class AssetBundleManager
{
	// Token: 0x170012A0 RID: 4768
	// (get) Token: 0x06009DAD RID: 40365 RVA: 0x003B9332 File Offset: 0x003B7532
	public static AssetBundleManager Instance
	{
		get
		{
			AssetBundleManager assetBundleManager;
			if ((assetBundleManager = AssetBundleManager.instance) == null)
			{
				assetBundleManager = (AssetBundleManager.instance = new AssetBundleManager());
			}
			return assetBundleManager;
		}
	}

	// Token: 0x06009DAE RID: 40366 RVA: 0x003B9348 File Offset: 0x003B7548
	[PublicizedFrom(EAccessModifier.Private)]
	public AssetBundleManager()
	{
	}

	// Token: 0x06009DAF RID: 40367 RVA: 0x003B935C File Offset: 0x003B755C
	public void LoadAssetBundle(string _name, bool _forceBundle = false)
	{
		string text;
		if (Path.IsPathRooted(_name))
		{
			text = _name;
		}
		else
		{
			text = string.Concat(new string[]
			{
				GameIO.GetApplicationPath(),
				"/Data/Bundles/Standalone",
				BundleTags.Tag,
				"/",
				_name
			});
		}
		string text2 = _name + 1.ToString();
		if (!this.dictAssetBundleRefs.ContainsKey(text2))
		{
			string directoryName = Path.GetDirectoryName(text);
			if (!Directory.Exists(directoryName))
			{
				Log.Error("Loading AssetBundle \"" + text + "\" failed: Parent folder not found!");
				return;
			}
			string fileName = Path.GetFileName(text);
			text = null;
			foreach (string text3 in Directory.EnumerateFiles(directoryName))
			{
				if (Path.GetFileName(text3).EqualsCaseInsensitive(fileName))
				{
					text = text3;
					break;
				}
			}
			if (text == null)
			{
				Log.Error("Loading AssetBundle \"" + fileName + "\" failed: File not found!");
				return;
			}
			AssetBundle assetBundle = AssetBundle.LoadFromFile(text);
			if (assetBundle == null)
			{
				Log.Error("Loading AssetBundle \"" + text + "\" failed!");
				return;
			}
			AssetBundleManager.AssetBundleRef assetBundleRef = new AssetBundleManager.AssetBundleRef(text, 1);
			assetBundleRef.assetBundle = assetBundle;
			this.dictAssetBundleRefs.Add(text2, assetBundleRef);
		}
	}

	// Token: 0x06009DB0 RID: 40368 RVA: 0x003B94AC File Offset: 0x003B76AC
	public T Get<T>(string _bundleName, string _objName, bool _forceBundle = false) where T : global::UnityEngine.Object
	{
		return this._get<T>(_bundleName, _objName, _forceBundle, false);
	}

	// Token: 0x06009DB1 RID: 40369 RVA: 0x003B94B8 File Offset: 0x003B76B8
	public T Get<T>(DataLoader.DataPathIdentifier _dpi, bool _useRelativePath, bool _forceBundle = false) where T : global::UnityEngine.Object
	{
		return this._get<T>(_dpi.BundlePath, _dpi.AssetName, _forceBundle, _useRelativePath);
	}

	// Token: 0x06009DB2 RID: 40370 RVA: 0x003B94D0 File Offset: 0x003B76D0
	[PublicizedFrom(EAccessModifier.Private)]
	public T _get<T>(string _bundleName, string _objName, bool _forceBundle = false, bool _useRelativePath = false) where T : global::UnityEngine.Object
	{
		string text = _bundleName + 1.ToString();
		AssetBundleManager.AssetBundleRef assetBundleRef;
		if (!this.dictAssetBundleRefs.TryGetValue(text, out assetBundleRef))
		{
			return default(T);
		}
		if (!_useRelativePath)
		{
			if (_objName.IndexOf('/') > 0)
			{
				_objName = _objName.Substring(_objName.LastIndexOf('/') + 1);
			}
			_objName = GameIO.RemoveFileExtension(_objName);
		}
		return assetBundleRef.assetBundle.LoadAsset<T>(_objName);
	}

	// Token: 0x06009DB3 RID: 40371 RVA: 0x003B953C File Offset: 0x003B773C
	public AssetBundleManager.AssetBundleRequestTFP GetAsync<T>(string _bundleName, string _objName, bool _forceBundle = false) where T : global::UnityEngine.Object
	{
		string text = _bundleName + 1.ToString();
		AssetBundleManager.AssetBundleRef assetBundleRef;
		if (!this.dictAssetBundleRefs.TryGetValue(text, out assetBundleRef))
		{
			return null;
		}
		if (_objName.IndexOf('/') > 0)
		{
			_objName = _objName.Substring(_objName.LastIndexOf('/') + 1);
		}
		return new AssetBundleManager.AssetBundleRequestTFP(assetBundleRef.assetBundle.LoadAssetAsync<T>(GameIO.RemoveFileExtension(_objName)));
	}

	// Token: 0x06009DB4 RID: 40372 RVA: 0x003B95A0 File Offset: 0x003B77A0
	public bool Contains(string _bundleName, string _objName, bool _forceBundle = false)
	{
		string text = _bundleName + 1.ToString();
		AssetBundleManager.AssetBundleRef assetBundleRef;
		if (!this.dictAssetBundleRefs.TryGetValue(text, out assetBundleRef))
		{
			return false;
		}
		if (_objName.IndexOf('/') > 0)
		{
			_objName = _objName.Substring(_objName.LastIndexOf('/') + 1);
		}
		return assetBundleRef.assetBundle.Contains(GameIO.RemoveFileExtension(_objName));
	}

	// Token: 0x06009DB5 RID: 40373 RVA: 0x003B9600 File Offset: 0x003B7800
	public T[] GetAllObjects<T>(string _bundleName, string _subpath = null, bool _forceBundle = false) where T : global::UnityEngine.Object
	{
		string text = _bundleName + 1.ToString();
		AssetBundleManager.AssetBundleRef assetBundleRef;
		if (!this.dictAssetBundleRefs.TryGetValue(text, out assetBundleRef))
		{
			return null;
		}
		return assetBundleRef.assetBundle.LoadAllAssets<T>();
	}

	// Token: 0x06009DB6 RID: 40374 RVA: 0x003B963C File Offset: 0x003B783C
	public AssetBundleManager.AssetBundleMassRequestTFP GetAllObjectsAsync<T>(string _bundleName, string _subpath = null, bool _forceBundle = false) where T : global::UnityEngine.Object
	{
		string text = _bundleName + 1.ToString();
		AssetBundleManager.AssetBundleRef assetBundleRef;
		if (!this.dictAssetBundleRefs.TryGetValue(text, out assetBundleRef))
		{
			return null;
		}
		return new AssetBundleManager.AssetBundleMassRequestTFP(assetBundleRef.assetBundle.LoadAllAssetsAsync<T>());
	}

	// Token: 0x06009DB7 RID: 40375 RVA: 0x003B967C File Offset: 0x003B787C
	public string[] GetAllAssetNames(string _bundleName, bool _forceBundle = false)
	{
		string text = _bundleName + 1.ToString();
		AssetBundleManager.AssetBundleRef assetBundleRef;
		if (!this.dictAssetBundleRefs.TryGetValue(text, out assetBundleRef))
		{
			return null;
		}
		return assetBundleRef.assetBundle.GetAllAssetNames();
	}

	// Token: 0x06009DB8 RID: 40376 RVA: 0x003B96B8 File Offset: 0x003B78B8
	public void Unload(string _name, bool _forceBundle = false)
	{
		string text = _name + 1.ToString();
		AssetBundleManager.AssetBundleRef assetBundleRef;
		if (this.dictAssetBundleRefs.TryGetValue(text, out assetBundleRef))
		{
			assetBundleRef.assetBundle.Unload(true);
			assetBundleRef.assetBundle = null;
			this.dictAssetBundleRefs.Remove(text);
		}
	}

	// Token: 0x06009DB9 RID: 40377 RVA: 0x003B9708 File Offset: 0x003B7908
	public void UnloadAll(bool _forceBundle = false)
	{
		foreach (string text in this.dictAssetBundleRefs.Keys)
		{
			this.dictAssetBundleRefs[text].assetBundle.Unload(true);
			this.dictAssetBundleRefs[text].assetBundle = null;
		}
		this.dictAssetBundleRefs.Clear();
	}

	// Token: 0x040077BC RID: 30652
	[PublicizedFrom(EAccessModifier.Private)]
	public static AssetBundleManager instance;

	// Token: 0x040077BD RID: 30653
	[PublicizedFrom(EAccessModifier.Private)]
	public const int version = 1;

	// Token: 0x040077BE RID: 30654
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<string, AssetBundleManager.AssetBundleRef> dictAssetBundleRefs = new CaseInsensitiveStringDictionary<AssetBundleManager.AssetBundleRef>();

	// Token: 0x02001386 RID: 4998
	public class AssetBundleRequestTFP : CustomYieldInstruction
	{
		// Token: 0x170012A1 RID: 4769
		// (get) Token: 0x06009DBA RID: 40378 RVA: 0x003B9790 File Offset: 0x003B7990
		public global::UnityEngine.Object Asset
		{
			get
			{
				if (!this.IsBundleLoad)
				{
					return this.asset;
				}
				return this.request.asset;
			}
		}

		// Token: 0x170012A2 RID: 4770
		// (get) Token: 0x06009DBB RID: 40379 RVA: 0x003B97AC File Offset: 0x003B79AC
		public bool IsDone
		{
			get
			{
				return !this.IsBundleLoad || this.request.isDone;
			}
		}

		// Token: 0x170012A3 RID: 4771
		// (get) Token: 0x06009DBC RID: 40380 RVA: 0x003B97C3 File Offset: 0x003B79C3
		public override bool keepWaiting
		{
			get
			{
				return this.IsBundleLoad && !this.request.isDone;
			}
		}

		// Token: 0x170012A4 RID: 4772
		// (get) Token: 0x06009DBD RID: 40381 RVA: 0x003B97DD File Offset: 0x003B79DD
		public bool IsBundleLoad
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.request != null;
			}
		}

		// Token: 0x06009DBE RID: 40382 RVA: 0x003B97E8 File Offset: 0x003B79E8
		public AssetBundleRequestTFP(global::UnityEngine.Object _asset)
		{
			this.asset = _asset;
		}

		// Token: 0x06009DBF RID: 40383 RVA: 0x003B97F7 File Offset: 0x003B79F7
		public AssetBundleRequestTFP(AssetBundleRequest _request)
		{
			this.request = _request;
		}

		// Token: 0x040077BF RID: 30655
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly global::UnityEngine.Object asset;

		// Token: 0x040077C0 RID: 30656
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly AssetBundleRequest request;
	}

	// Token: 0x02001387 RID: 4999
	public class AssetBundleMassRequestTFP : CustomYieldInstruction
	{
		// Token: 0x170012A5 RID: 4773
		// (get) Token: 0x06009DC0 RID: 40384 RVA: 0x003B9806 File Offset: 0x003B7A06
		public global::UnityEngine.Object[] Assets
		{
			get
			{
				if (!this.IsBundleLoad)
				{
					return this.assets;
				}
				return this.request.allAssets;
			}
		}

		// Token: 0x170012A6 RID: 4774
		// (get) Token: 0x06009DC1 RID: 40385 RVA: 0x003B9822 File Offset: 0x003B7A22
		public bool IsDone
		{
			get
			{
				return !this.IsBundleLoad || this.request.isDone;
			}
		}

		// Token: 0x170012A7 RID: 4775
		// (get) Token: 0x06009DC2 RID: 40386 RVA: 0x003B9839 File Offset: 0x003B7A39
		public override bool keepWaiting
		{
			get
			{
				return this.IsBundleLoad && !this.request.isDone;
			}
		}

		// Token: 0x170012A8 RID: 4776
		// (get) Token: 0x06009DC3 RID: 40387 RVA: 0x003B9853 File Offset: 0x003B7A53
		public bool IsBundleLoad
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.request != null;
			}
		}

		// Token: 0x06009DC4 RID: 40388 RVA: 0x003B985E File Offset: 0x003B7A5E
		public AssetBundleMassRequestTFP(List<global::UnityEngine.Object> _assets)
		{
			this.assets = _assets.ToArray();
		}

		// Token: 0x06009DC5 RID: 40389 RVA: 0x003B9872 File Offset: 0x003B7A72
		public AssetBundleMassRequestTFP(AssetBundleRequest _request)
		{
			this.request = _request;
		}

		// Token: 0x040077C1 RID: 30657
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly global::UnityEngine.Object[] assets;

		// Token: 0x040077C2 RID: 30658
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly AssetBundleRequest request;
	}

	// Token: 0x02001388 RID: 5000
	[PublicizedFrom(EAccessModifier.Private)]
	public class AssetBundleRef
	{
		// Token: 0x06009DC6 RID: 40390 RVA: 0x003B9881 File Offset: 0x003B7A81
		public AssetBundleRef(string _url, int _version)
		{
			this.url = _url;
			this.version = _version;
		}

		// Token: 0x040077C3 RID: 30659
		public AssetBundle assetBundle;

		// Token: 0x040077C4 RID: 30660
		public int version;

		// Token: 0x040077C5 RID: 30661
		public string url;
	}
}

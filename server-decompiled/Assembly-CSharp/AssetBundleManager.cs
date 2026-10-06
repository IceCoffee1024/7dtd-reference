using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x020013A5 RID: 5029
public class AssetBundleManager
{
	// Token: 0x170012E5 RID: 4837
	// (get) Token: 0x06009E58 RID: 40536 RVA: 0x003BDEAE File Offset: 0x003BC0AE
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

	// Token: 0x06009E59 RID: 40537 RVA: 0x003BDEC4 File Offset: 0x003BC0C4
	[PublicizedFrom(EAccessModifier.Private)]
	public AssetBundleManager()
	{
	}

	// Token: 0x06009E5A RID: 40538 RVA: 0x003BDED8 File Offset: 0x003BC0D8
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

	// Token: 0x06009E5B RID: 40539 RVA: 0x003BE028 File Offset: 0x003BC228
	public T Get<T>(string _bundleName, string _objName, bool _forceBundle = false) where T : global::UnityEngine.Object
	{
		return this._get<T>(_bundleName, _objName, _forceBundle, false);
	}

	// Token: 0x06009E5C RID: 40540 RVA: 0x003BE034 File Offset: 0x003BC234
	public T Get<T>(DataLoader.DataPathIdentifier _dpi, bool _useRelativePath, bool _forceBundle = false) where T : global::UnityEngine.Object
	{
		return this._get<T>(_dpi.BundlePath, _dpi.AssetName, _forceBundle, _useRelativePath);
	}

	// Token: 0x06009E5D RID: 40541 RVA: 0x003BE04C File Offset: 0x003BC24C
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

	// Token: 0x06009E5E RID: 40542 RVA: 0x003BE0B8 File Offset: 0x003BC2B8
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

	// Token: 0x06009E5F RID: 40543 RVA: 0x003BE11C File Offset: 0x003BC31C
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

	// Token: 0x06009E60 RID: 40544 RVA: 0x003BE17C File Offset: 0x003BC37C
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

	// Token: 0x06009E61 RID: 40545 RVA: 0x003BE1B8 File Offset: 0x003BC3B8
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

	// Token: 0x06009E62 RID: 40546 RVA: 0x003BE1F8 File Offset: 0x003BC3F8
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

	// Token: 0x06009E63 RID: 40547 RVA: 0x003BE234 File Offset: 0x003BC434
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

	// Token: 0x06009E64 RID: 40548 RVA: 0x003BE284 File Offset: 0x003BC484
	public void UnloadAll(bool _forceBundle = false)
	{
		foreach (string text in this.dictAssetBundleRefs.Keys)
		{
			this.dictAssetBundleRefs[text].assetBundle.Unload(true);
			this.dictAssetBundleRefs[text].assetBundle = null;
		}
		this.dictAssetBundleRefs.Clear();
	}

	// Token: 0x04007837 RID: 30775
	[PublicizedFrom(EAccessModifier.Private)]
	public static AssetBundleManager instance;

	// Token: 0x04007838 RID: 30776
	[PublicizedFrom(EAccessModifier.Private)]
	public const int version = 1;

	// Token: 0x04007839 RID: 30777
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<string, AssetBundleManager.AssetBundleRef> dictAssetBundleRefs = new CaseInsensitiveStringDictionary<AssetBundleManager.AssetBundleRef>();

	// Token: 0x020013A6 RID: 5030
	public class AssetBundleRequestTFP : CustomYieldInstruction
	{
		// Token: 0x170012E6 RID: 4838
		// (get) Token: 0x06009E65 RID: 40549 RVA: 0x003BE30C File Offset: 0x003BC50C
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

		// Token: 0x170012E7 RID: 4839
		// (get) Token: 0x06009E66 RID: 40550 RVA: 0x003BE328 File Offset: 0x003BC528
		public bool IsDone
		{
			get
			{
				return !this.IsBundleLoad || this.request.isDone;
			}
		}

		// Token: 0x170012E8 RID: 4840
		// (get) Token: 0x06009E67 RID: 40551 RVA: 0x003BE33F File Offset: 0x003BC53F
		public override bool keepWaiting
		{
			get
			{
				return this.IsBundleLoad && !this.request.isDone;
			}
		}

		// Token: 0x170012E9 RID: 4841
		// (get) Token: 0x06009E68 RID: 40552 RVA: 0x003BE359 File Offset: 0x003BC559
		public bool IsBundleLoad
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.request != null;
			}
		}

		// Token: 0x06009E69 RID: 40553 RVA: 0x003BE364 File Offset: 0x003BC564
		public AssetBundleRequestTFP(global::UnityEngine.Object _asset)
		{
			this.asset = _asset;
		}

		// Token: 0x06009E6A RID: 40554 RVA: 0x003BE373 File Offset: 0x003BC573
		public AssetBundleRequestTFP(AssetBundleRequest _request)
		{
			this.request = _request;
		}

		// Token: 0x0400783A RID: 30778
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly global::UnityEngine.Object asset;

		// Token: 0x0400783B RID: 30779
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly AssetBundleRequest request;
	}

	// Token: 0x020013A7 RID: 5031
	public class AssetBundleMassRequestTFP : CustomYieldInstruction
	{
		// Token: 0x170012EA RID: 4842
		// (get) Token: 0x06009E6B RID: 40555 RVA: 0x003BE382 File Offset: 0x003BC582
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

		// Token: 0x170012EB RID: 4843
		// (get) Token: 0x06009E6C RID: 40556 RVA: 0x003BE39E File Offset: 0x003BC59E
		public bool IsDone
		{
			get
			{
				return !this.IsBundleLoad || this.request.isDone;
			}
		}

		// Token: 0x170012EC RID: 4844
		// (get) Token: 0x06009E6D RID: 40557 RVA: 0x003BE3B5 File Offset: 0x003BC5B5
		public override bool keepWaiting
		{
			get
			{
				return this.IsBundleLoad && !this.request.isDone;
			}
		}

		// Token: 0x170012ED RID: 4845
		// (get) Token: 0x06009E6E RID: 40558 RVA: 0x003BE3CF File Offset: 0x003BC5CF
		public bool IsBundleLoad
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.request != null;
			}
		}

		// Token: 0x06009E6F RID: 40559 RVA: 0x003BE3DA File Offset: 0x003BC5DA
		public AssetBundleMassRequestTFP(List<global::UnityEngine.Object> _assets)
		{
			this.assets = _assets.ToArray();
		}

		// Token: 0x06009E70 RID: 40560 RVA: 0x003BE3EE File Offset: 0x003BC5EE
		public AssetBundleMassRequestTFP(AssetBundleRequest _request)
		{
			this.request = _request;
		}

		// Token: 0x0400783C RID: 30780
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly global::UnityEngine.Object[] assets;

		// Token: 0x0400783D RID: 30781
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly AssetBundleRequest request;
	}

	// Token: 0x020013A8 RID: 5032
	[PublicizedFrom(EAccessModifier.Private)]
	public class AssetBundleRef
	{
		// Token: 0x06009E71 RID: 40561 RVA: 0x003BE3FD File Offset: 0x003BC5FD
		public AssetBundleRef(string _url, int _version)
		{
			this.url = _url;
			this.version = _version;
		}

		// Token: 0x0400783E RID: 30782
		public AssetBundle assetBundle;

		// Token: 0x0400783F RID: 30783
		public int version;

		// Token: 0x04007840 RID: 30784
		public string url;
	}
}

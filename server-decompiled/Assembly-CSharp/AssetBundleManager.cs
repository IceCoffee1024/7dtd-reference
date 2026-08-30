using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x0200136B RID: 4971
public class AssetBundleManager
{
	// Token: 0x17001267 RID: 4711
	// (get) Token: 0x06009CB2 RID: 40114 RVA: 0x003B407E File Offset: 0x003B227E
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

	// Token: 0x06009CB3 RID: 40115 RVA: 0x003B4094 File Offset: 0x003B2294
	[PublicizedFrom(EAccessModifier.Private)]
	public AssetBundleManager()
	{
	}

	// Token: 0x06009CB4 RID: 40116 RVA: 0x003B40A8 File Offset: 0x003B22A8
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

	// Token: 0x06009CB5 RID: 40117 RVA: 0x003B41F8 File Offset: 0x003B23F8
	public T Get<T>(string _bundleName, string _objName, bool _forceBundle = false) where T : global::UnityEngine.Object
	{
		return this._get<T>(_bundleName, _objName, _forceBundle, false);
	}

	// Token: 0x06009CB6 RID: 40118 RVA: 0x003B4204 File Offset: 0x003B2404
	public T Get<T>(DataLoader.DataPathIdentifier _dpi, bool _useRelativePath, bool _forceBundle = false) where T : global::UnityEngine.Object
	{
		return this._get<T>(_dpi.BundlePath, _dpi.AssetName, _forceBundle, _useRelativePath);
	}

	// Token: 0x06009CB7 RID: 40119 RVA: 0x003B421C File Offset: 0x003B241C
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

	// Token: 0x06009CB8 RID: 40120 RVA: 0x003B4288 File Offset: 0x003B2488
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

	// Token: 0x06009CB9 RID: 40121 RVA: 0x003B42EC File Offset: 0x003B24EC
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

	// Token: 0x06009CBA RID: 40122 RVA: 0x003B434C File Offset: 0x003B254C
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

	// Token: 0x06009CBB RID: 40123 RVA: 0x003B4388 File Offset: 0x003B2588
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

	// Token: 0x06009CBC RID: 40124 RVA: 0x003B43C8 File Offset: 0x003B25C8
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

	// Token: 0x06009CBD RID: 40125 RVA: 0x003B4404 File Offset: 0x003B2604
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

	// Token: 0x06009CBE RID: 40126 RVA: 0x003B4454 File Offset: 0x003B2654
	public void UnloadAll(bool _forceBundle = false)
	{
		foreach (string text in this.dictAssetBundleRefs.Keys)
		{
			this.dictAssetBundleRefs[text].assetBundle.Unload(true);
			this.dictAssetBundleRefs[text].assetBundle = null;
		}
		this.dictAssetBundleRefs.Clear();
	}

	// Token: 0x040076E4 RID: 30436
	[PublicizedFrom(EAccessModifier.Private)]
	public static AssetBundleManager instance;

	// Token: 0x040076E5 RID: 30437
	[PublicizedFrom(EAccessModifier.Private)]
	public const int version = 1;

	// Token: 0x040076E6 RID: 30438
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<string, AssetBundleManager.AssetBundleRef> dictAssetBundleRefs = new CaseInsensitiveStringDictionary<AssetBundleManager.AssetBundleRef>();

	// Token: 0x0200136C RID: 4972
	public class AssetBundleRequestTFP : CustomYieldInstruction
	{
		// Token: 0x17001268 RID: 4712
		// (get) Token: 0x06009CBF RID: 40127 RVA: 0x003B44DC File Offset: 0x003B26DC
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

		// Token: 0x17001269 RID: 4713
		// (get) Token: 0x06009CC0 RID: 40128 RVA: 0x003B44F8 File Offset: 0x003B26F8
		public bool IsDone
		{
			get
			{
				return !this.IsBundleLoad || this.request.isDone;
			}
		}

		// Token: 0x1700126A RID: 4714
		// (get) Token: 0x06009CC1 RID: 40129 RVA: 0x003B450F File Offset: 0x003B270F
		public override bool keepWaiting
		{
			get
			{
				return this.IsBundleLoad && !this.request.isDone;
			}
		}

		// Token: 0x1700126B RID: 4715
		// (get) Token: 0x06009CC2 RID: 40130 RVA: 0x003B4529 File Offset: 0x003B2729
		public bool IsBundleLoad
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.request != null;
			}
		}

		// Token: 0x06009CC3 RID: 40131 RVA: 0x003B4534 File Offset: 0x003B2734
		public AssetBundleRequestTFP(global::UnityEngine.Object _asset)
		{
			this.asset = _asset;
		}

		// Token: 0x06009CC4 RID: 40132 RVA: 0x003B4543 File Offset: 0x003B2743
		public AssetBundleRequestTFP(AssetBundleRequest _request)
		{
			this.request = _request;
		}

		// Token: 0x040076E7 RID: 30439
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly global::UnityEngine.Object asset;

		// Token: 0x040076E8 RID: 30440
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly AssetBundleRequest request;
	}

	// Token: 0x0200136D RID: 4973
	public class AssetBundleMassRequestTFP : CustomYieldInstruction
	{
		// Token: 0x1700126C RID: 4716
		// (get) Token: 0x06009CC5 RID: 40133 RVA: 0x003B4552 File Offset: 0x003B2752
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

		// Token: 0x1700126D RID: 4717
		// (get) Token: 0x06009CC6 RID: 40134 RVA: 0x003B456E File Offset: 0x003B276E
		public bool IsDone
		{
			get
			{
				return !this.IsBundleLoad || this.request.isDone;
			}
		}

		// Token: 0x1700126E RID: 4718
		// (get) Token: 0x06009CC7 RID: 40135 RVA: 0x003B4585 File Offset: 0x003B2785
		public override bool keepWaiting
		{
			get
			{
				return this.IsBundleLoad && !this.request.isDone;
			}
		}

		// Token: 0x1700126F RID: 4719
		// (get) Token: 0x06009CC8 RID: 40136 RVA: 0x003B459F File Offset: 0x003B279F
		public bool IsBundleLoad
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.request != null;
			}
		}

		// Token: 0x06009CC9 RID: 40137 RVA: 0x003B45AA File Offset: 0x003B27AA
		public AssetBundleMassRequestTFP(List<global::UnityEngine.Object> _assets)
		{
			this.assets = _assets.ToArray();
		}

		// Token: 0x06009CCA RID: 40138 RVA: 0x003B45BE File Offset: 0x003B27BE
		public AssetBundleMassRequestTFP(AssetBundleRequest _request)
		{
			this.request = _request;
		}

		// Token: 0x040076E9 RID: 30441
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly global::UnityEngine.Object[] assets;

		// Token: 0x040076EA RID: 30442
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly AssetBundleRequest request;
	}

	// Token: 0x0200136E RID: 4974
	[PublicizedFrom(EAccessModifier.Private)]
	public class AssetBundleRef
	{
		// Token: 0x06009CCB RID: 40139 RVA: 0x003B45CD File Offset: 0x003B27CD
		public AssetBundleRef(string _url, int _version)
		{
			this.url = _url;
			this.version = _version;
		}

		// Token: 0x040076EB RID: 30443
		public AssetBundle assetBundle;

		// Token: 0x040076EC RID: 30444
		public int version;

		// Token: 0x040076ED RID: 30445
		public string url;
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x020013B7 RID: 5047
public class AssetBundleManager
{
	// Token: 0x170012F1 RID: 4849
	// (get) Token: 0x06009EB0 RID: 40624 RVA: 0x003C02B2 File Offset: 0x003BE4B2
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

	// Token: 0x06009EB1 RID: 40625 RVA: 0x003C02C8 File Offset: 0x003BE4C8
	[PublicizedFrom(EAccessModifier.Private)]
	public AssetBundleManager()
	{
	}

	// Token: 0x06009EB2 RID: 40626 RVA: 0x003C02DC File Offset: 0x003BE4DC
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

	// Token: 0x06009EB3 RID: 40627 RVA: 0x003C042C File Offset: 0x003BE62C
	public T Get<T>(string _bundleName, string _objName, bool _forceBundle = false) where T : global::UnityEngine.Object
	{
		return this._get<T>(_bundleName, _objName, _forceBundle, false);
	}

	// Token: 0x06009EB4 RID: 40628 RVA: 0x003C0438 File Offset: 0x003BE638
	public T Get<T>(DataLoader.DataPathIdentifier _dpi, bool _useRelativePath, bool _forceBundle = false) where T : global::UnityEngine.Object
	{
		return this._get<T>(_dpi.BundlePath, _dpi.AssetName, _forceBundle, _useRelativePath);
	}

	// Token: 0x06009EB5 RID: 40629 RVA: 0x003C0450 File Offset: 0x003BE650
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

	// Token: 0x06009EB6 RID: 40630 RVA: 0x003C04BC File Offset: 0x003BE6BC
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

	// Token: 0x06009EB7 RID: 40631 RVA: 0x003C0520 File Offset: 0x003BE720
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

	// Token: 0x06009EB8 RID: 40632 RVA: 0x003C0580 File Offset: 0x003BE780
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

	// Token: 0x06009EB9 RID: 40633 RVA: 0x003C05BC File Offset: 0x003BE7BC
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

	// Token: 0x06009EBA RID: 40634 RVA: 0x003C05FC File Offset: 0x003BE7FC
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

	// Token: 0x06009EBB RID: 40635 RVA: 0x003C0638 File Offset: 0x003BE838
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

	// Token: 0x06009EBC RID: 40636 RVA: 0x003C0688 File Offset: 0x003BE888
	public void UnloadAll(bool _forceBundle = false)
	{
		foreach (string text in this.dictAssetBundleRefs.Keys)
		{
			this.dictAssetBundleRefs[text].assetBundle.Unload(true);
			this.dictAssetBundleRefs[text].assetBundle = null;
		}
		this.dictAssetBundleRefs.Clear();
	}

	// Token: 0x04007878 RID: 30840
	[PublicizedFrom(EAccessModifier.Private)]
	public static AssetBundleManager instance;

	// Token: 0x04007879 RID: 30841
	[PublicizedFrom(EAccessModifier.Private)]
	public const int version = 1;

	// Token: 0x0400787A RID: 30842
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<string, AssetBundleManager.AssetBundleRef> dictAssetBundleRefs = new CaseInsensitiveStringDictionary<AssetBundleManager.AssetBundleRef>();

	// Token: 0x020013B8 RID: 5048
	public class AssetBundleRequestTFP : CustomYieldInstruction
	{
		// Token: 0x170012F2 RID: 4850
		// (get) Token: 0x06009EBD RID: 40637 RVA: 0x003C0710 File Offset: 0x003BE910
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

		// Token: 0x170012F3 RID: 4851
		// (get) Token: 0x06009EBE RID: 40638 RVA: 0x003C072C File Offset: 0x003BE92C
		public bool IsDone
		{
			get
			{
				return !this.IsBundleLoad || this.request.isDone;
			}
		}

		// Token: 0x170012F4 RID: 4852
		// (get) Token: 0x06009EBF RID: 40639 RVA: 0x003C0743 File Offset: 0x003BE943
		public override bool keepWaiting
		{
			get
			{
				return this.IsBundleLoad && !this.request.isDone;
			}
		}

		// Token: 0x170012F5 RID: 4853
		// (get) Token: 0x06009EC0 RID: 40640 RVA: 0x003C075D File Offset: 0x003BE95D
		public bool IsBundleLoad
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.request != null;
			}
		}

		// Token: 0x06009EC1 RID: 40641 RVA: 0x003C0768 File Offset: 0x003BE968
		public AssetBundleRequestTFP(global::UnityEngine.Object _asset)
		{
			this.asset = _asset;
		}

		// Token: 0x06009EC2 RID: 40642 RVA: 0x003C0777 File Offset: 0x003BE977
		public AssetBundleRequestTFP(AssetBundleRequest _request)
		{
			this.request = _request;
		}

		// Token: 0x0400787B RID: 30843
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly global::UnityEngine.Object asset;

		// Token: 0x0400787C RID: 30844
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly AssetBundleRequest request;
	}

	// Token: 0x020013B9 RID: 5049
	public class AssetBundleMassRequestTFP : CustomYieldInstruction
	{
		// Token: 0x170012F6 RID: 4854
		// (get) Token: 0x06009EC3 RID: 40643 RVA: 0x003C0786 File Offset: 0x003BE986
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

		// Token: 0x170012F7 RID: 4855
		// (get) Token: 0x06009EC4 RID: 40644 RVA: 0x003C07A2 File Offset: 0x003BE9A2
		public bool IsDone
		{
			get
			{
				return !this.IsBundleLoad || this.request.isDone;
			}
		}

		// Token: 0x170012F8 RID: 4856
		// (get) Token: 0x06009EC5 RID: 40645 RVA: 0x003C07B9 File Offset: 0x003BE9B9
		public override bool keepWaiting
		{
			get
			{
				return this.IsBundleLoad && !this.request.isDone;
			}
		}

		// Token: 0x170012F9 RID: 4857
		// (get) Token: 0x06009EC6 RID: 40646 RVA: 0x003C07D3 File Offset: 0x003BE9D3
		public bool IsBundleLoad
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.request != null;
			}
		}

		// Token: 0x06009EC7 RID: 40647 RVA: 0x003C07DE File Offset: 0x003BE9DE
		public AssetBundleMassRequestTFP(List<global::UnityEngine.Object> _assets)
		{
			this.assets = _assets.ToArray();
		}

		// Token: 0x06009EC8 RID: 40648 RVA: 0x003C07F2 File Offset: 0x003BE9F2
		public AssetBundleMassRequestTFP(AssetBundleRequest _request)
		{
			this.request = _request;
		}

		// Token: 0x0400787D RID: 30845
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly global::UnityEngine.Object[] assets;

		// Token: 0x0400787E RID: 30846
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly AssetBundleRequest request;
	}

	// Token: 0x020013BA RID: 5050
	[PublicizedFrom(EAccessModifier.Private)]
	public class AssetBundleRef
	{
		// Token: 0x06009EC9 RID: 40649 RVA: 0x003C0801 File Offset: 0x003BEA01
		public AssetBundleRef(string _url, int _version)
		{
			this.url = _url;
			this.version = _version;
		}

		// Token: 0x0400787F RID: 30847
		public AssetBundle assetBundle;

		// Token: 0x04007880 RID: 30848
		public int version;

		// Token: 0x04007881 RID: 30849
		public string url;
	}
}

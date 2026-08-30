using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;

// Token: 0x020013A7 RID: 5031
public static class DataLoader
{
	// Token: 0x06009E27 RID: 40487 RVA: 0x003BADBC File Offset: 0x003B8FBC
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsInResources(string _uri)
	{
		return _uri.IndexOf('#') < 0 && _uri.IndexOf('@') < 0;
	}

	// Token: 0x06009E28 RID: 40488 RVA: 0x003BADD8 File Offset: 0x003B8FD8
	public static DataLoader.DataPathIdentifier ParseDataPathIdentifier(string _inputUri)
	{
		if (_inputUri == null)
		{
			return new DataLoader.DataPathIdentifier(null, DataLoader.DataPathIdentifier.AssetLocation.Resources, false);
		}
		string text = ModManager.PatchModPathString(_inputUri);
		if (text != null)
		{
			_inputUri = text;
		}
		if (_inputUri.IndexOf('#') == 0 && _inputUri.IndexOf('?') > 0)
		{
			int num = _inputUri.IndexOf('?');
			string text2 = _inputUri.Substring(1, num - 1);
			_inputUri = _inputUri.Substring(num + 1);
			return new DataLoader.DataPathIdentifier(_inputUri, text2, text != null);
		}
		if (_inputUri.IndexOf("@:") == 0)
		{
			return new DataLoader.DataPathIdentifier(_inputUri.Substring(2), DataLoader.DataPathIdentifier.AssetLocation.Addressable, text != null);
		}
		return new DataLoader.DataPathIdentifier(_inputUri, DataLoader.DataPathIdentifier.AssetLocation.Resources, false);
	}

	// Token: 0x06009E29 RID: 40489 RVA: 0x003BAE65 File Offset: 0x003B9065
	public static T LoadAsset<T>(DataLoader.DataPathIdentifier _identifier, bool _ignoreDlcEntitlements = false) where T : global::UnityEngine.Object
	{
		return LoadManager.LoadAsset<T>(_identifier, null, null, false, true, _ignoreDlcEntitlements).Asset;
	}

	// Token: 0x06009E2A RID: 40490 RVA: 0x003BAE77 File Offset: 0x003B9077
	public static T LoadAsset<T>(string _uri, bool _ignoreDlcEntitlements = false) where T : global::UnityEngine.Object
	{
		return DataLoader.LoadAsset<T>(DataLoader.ParseDataPathIdentifier(_uri), _ignoreDlcEntitlements);
	}

	// Token: 0x06009E2B RID: 40491 RVA: 0x003BAE85 File Offset: 0x003B9085
	public static T LoadAsset<T>(AssetReference assetReference, bool _ignoreDlcEntitlements = false) where T : global::UnityEngine.Object
	{
		return LoadManager.LoadAssetFromAddressables<T>(assetReference, null, null, false, true, _ignoreDlcEntitlements).Asset;
	}

	// Token: 0x06009E2C RID: 40492 RVA: 0x003BAE97 File Offset: 0x003B9097
	public static void UnloadAsset(DataLoader.DataPathIdentifier _srcIdentifier, global::UnityEngine.Object _obj)
	{
		if (_srcIdentifier.IsBundle)
		{
			Resources.UnloadUnusedAssets();
			return;
		}
		Resources.UnloadAsset(_obj);
		if (_srcIdentifier.Location == DataLoader.DataPathIdentifier.AssetLocation.Addressable)
		{
			LoadManager.ReleaseAddressable<global::UnityEngine.Object>(_obj);
		}
	}

	// Token: 0x06009E2D RID: 40493 RVA: 0x003BAEBE File Offset: 0x003B90BE
	public static void UnloadAsset(string _uri, global::UnityEngine.Object _obj)
	{
		DataLoader.UnloadAsset(DataLoader.ParseDataPathIdentifier(_uri), _obj);
	}

	// Token: 0x06009E2E RID: 40494 RVA: 0x003BAECC File Offset: 0x003B90CC
	public static void PreloadBundle(DataLoader.DataPathIdentifier _identifier)
	{
		if (_identifier.IsBundle)
		{
			AssetBundleManager.Instance.LoadAssetBundle(_identifier.BundlePath, _identifier.FromMod);
		}
	}

	// Token: 0x06009E2F RID: 40495 RVA: 0x003BAEED File Offset: 0x003B90ED
	public static void PreloadBundle(string _uri)
	{
		DataLoader.PreloadBundle(DataLoader.ParseDataPathIdentifier(_uri));
	}

	// Token: 0x020013A8 RID: 5032
	public struct DataPathIdentifier
	{
		// Token: 0x1700128E RID: 4750
		// (get) Token: 0x06009E30 RID: 40496 RVA: 0x003BAEFA File Offset: 0x003B90FA
		public bool IsBundle
		{
			get
			{
				return this.Location == DataLoader.DataPathIdentifier.AssetLocation.Bundle;
			}
		}

		// Token: 0x06009E31 RID: 40497 RVA: 0x003BAF05 File Offset: 0x003B9105
		public DataPathIdentifier(string _assetName, DataLoader.DataPathIdentifier.AssetLocation _location = DataLoader.DataPathIdentifier.AssetLocation.Resources, bool _fromMod = false)
		{
			this.BundlePath = null;
			this.AssetName = _assetName;
			this.Location = _location;
			this.FromMod = _fromMod;
		}

		// Token: 0x06009E32 RID: 40498 RVA: 0x003BAF23 File Offset: 0x003B9123
		public DataPathIdentifier(string _assetName, string _bundlePath, bool _fromMod = false)
		{
			this.BundlePath = _bundlePath;
			this.AssetName = _assetName;
			this.Location = DataLoader.DataPathIdentifier.AssetLocation.Bundle;
			this.FromMod = _fromMod;
		}

		// Token: 0x040077B7 RID: 30647
		public readonly DataLoader.DataPathIdentifier.AssetLocation Location;

		// Token: 0x040077B8 RID: 30648
		public readonly string BundlePath;

		// Token: 0x040077B9 RID: 30649
		public readonly string AssetName;

		// Token: 0x040077BA RID: 30650
		public readonly bool FromMod;

		// Token: 0x020013A9 RID: 5033
		public enum AssetLocation
		{
			// Token: 0x040077BC RID: 30652
			Resources,
			// Token: 0x040077BD RID: 30653
			Bundle,
			// Token: 0x040077BE RID: 30654
			Addressable
		}
	}
}

using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;

// Token: 0x020013C2 RID: 5058
public static class DataLoader
{
	// Token: 0x06009F24 RID: 40740 RVA: 0x003C0100 File Offset: 0x003BE300
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsInResources(string _uri)
	{
		return _uri.IndexOf('#') < 0 && _uri.IndexOf('@') < 0;
	}

	// Token: 0x06009F25 RID: 40741 RVA: 0x003C011C File Offset: 0x003BE31C
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

	// Token: 0x06009F26 RID: 40742 RVA: 0x003C01A9 File Offset: 0x003BE3A9
	public static T LoadAsset<T>(DataLoader.DataPathIdentifier _identifier, bool _ignoreDlcEntitlements = false) where T : global::UnityEngine.Object
	{
		return LoadManager.LoadAsset<T>(_identifier, null, null, false, true, _ignoreDlcEntitlements).Asset;
	}

	// Token: 0x06009F27 RID: 40743 RVA: 0x003C01BB File Offset: 0x003BE3BB
	public static T LoadAsset<T>(string _uri, bool _ignoreDlcEntitlements = false) where T : global::UnityEngine.Object
	{
		return DataLoader.LoadAsset<T>(DataLoader.ParseDataPathIdentifier(_uri), _ignoreDlcEntitlements);
	}

	// Token: 0x06009F28 RID: 40744 RVA: 0x003C01C9 File Offset: 0x003BE3C9
	public static T LoadAsset<T>(AssetReference assetReference, bool _ignoreDlcEntitlements = false) where T : global::UnityEngine.Object
	{
		return LoadManager.LoadAssetFromAddressables<T>(assetReference, null, null, false, true, _ignoreDlcEntitlements).Asset;
	}

	// Token: 0x06009F29 RID: 40745 RVA: 0x003C01DB File Offset: 0x003BE3DB
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

	// Token: 0x06009F2A RID: 40746 RVA: 0x003C0202 File Offset: 0x003BE402
	public static void UnloadAsset(string _uri, global::UnityEngine.Object _obj)
	{
		DataLoader.UnloadAsset(DataLoader.ParseDataPathIdentifier(_uri), _obj);
	}

	// Token: 0x06009F2B RID: 40747 RVA: 0x003C0210 File Offset: 0x003BE410
	public static void PreloadBundle(DataLoader.DataPathIdentifier _identifier)
	{
		if (_identifier.IsBundle)
		{
			AssetBundleManager.Instance.LoadAssetBundle(_identifier.BundlePath, _identifier.FromMod);
		}
	}

	// Token: 0x06009F2C RID: 40748 RVA: 0x003C0231 File Offset: 0x003BE431
	public static void PreloadBundle(string _uri)
	{
		DataLoader.PreloadBundle(DataLoader.ParseDataPathIdentifier(_uri));
	}

	// Token: 0x020013C3 RID: 5059
	public struct DataPathIdentifier
	{
		// Token: 0x170012C7 RID: 4807
		// (get) Token: 0x06009F2D RID: 40749 RVA: 0x003C023E File Offset: 0x003BE43E
		public bool IsBundle
		{
			get
			{
				return this.Location == DataLoader.DataPathIdentifier.AssetLocation.Bundle;
			}
		}

		// Token: 0x06009F2E RID: 40750 RVA: 0x003C0249 File Offset: 0x003BE449
		public DataPathIdentifier(string _assetName, DataLoader.DataPathIdentifier.AssetLocation _location = DataLoader.DataPathIdentifier.AssetLocation.Resources, bool _fromMod = false)
		{
			this.BundlePath = null;
			this.AssetName = _assetName;
			this.Location = _location;
			this.FromMod = _fromMod;
		}

		// Token: 0x06009F2F RID: 40751 RVA: 0x003C0267 File Offset: 0x003BE467
		public DataPathIdentifier(string _assetName, string _bundlePath, bool _fromMod = false)
		{
			this.BundlePath = _bundlePath;
			this.AssetName = _assetName;
			this.Location = DataLoader.DataPathIdentifier.AssetLocation.Bundle;
			this.FromMod = _fromMod;
		}

		// Token: 0x04007890 RID: 30864
		public readonly DataLoader.DataPathIdentifier.AssetLocation Location;

		// Token: 0x04007891 RID: 30865
		public readonly string BundlePath;

		// Token: 0x04007892 RID: 30866
		public readonly string AssetName;

		// Token: 0x04007893 RID: 30867
		public readonly bool FromMod;

		// Token: 0x020013C4 RID: 5060
		public enum AssetLocation
		{
			// Token: 0x04007895 RID: 30869
			Resources,
			// Token: 0x04007896 RID: 30870
			Bundle,
			// Token: 0x04007897 RID: 30871
			Addressable
		}
	}
}

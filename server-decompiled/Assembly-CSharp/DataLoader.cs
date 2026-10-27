using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;

// Token: 0x020013F4 RID: 5108
public static class DataLoader
{
	// Token: 0x0600A027 RID: 40999 RVA: 0x003C70A0 File Offset: 0x003C52A0
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsInResources(string _uri)
	{
		return _uri.IndexOf('#') < 0 && _uri.IndexOf('@') < 0;
	}

	// Token: 0x0600A028 RID: 41000 RVA: 0x003C70BC File Offset: 0x003C52BC
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

	// Token: 0x0600A029 RID: 41001 RVA: 0x003C7149 File Offset: 0x003C5349
	public static T LoadAsset<T>(DataLoader.DataPathIdentifier _identifier, bool _ignoreDlcEntitlements = false) where T : global::UnityEngine.Object
	{
		return LoadManager.LoadAsset<T>(_identifier, null, null, false, true, _ignoreDlcEntitlements).Asset;
	}

	// Token: 0x0600A02A RID: 41002 RVA: 0x003C715B File Offset: 0x003C535B
	public static T LoadAsset<T>(string _uri, bool _ignoreDlcEntitlements = false) where T : global::UnityEngine.Object
	{
		return DataLoader.LoadAsset<T>(DataLoader.ParseDataPathIdentifier(_uri), _ignoreDlcEntitlements);
	}

	// Token: 0x0600A02B RID: 41003 RVA: 0x003C7169 File Offset: 0x003C5369
	public static T LoadAsset<T>(AssetReference assetReference, bool _ignoreDlcEntitlements = false) where T : global::UnityEngine.Object
	{
		return LoadManager.LoadAssetFromAddressables<T>(assetReference, null, null, false, true, _ignoreDlcEntitlements).Asset;
	}

	// Token: 0x0600A02C RID: 41004 RVA: 0x003C717B File Offset: 0x003C537B
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

	// Token: 0x0600A02D RID: 41005 RVA: 0x003C71A2 File Offset: 0x003C53A2
	public static void UnloadAsset(string _uri, global::UnityEngine.Object _obj)
	{
		DataLoader.UnloadAsset(DataLoader.ParseDataPathIdentifier(_uri), _obj);
	}

	// Token: 0x0600A02E RID: 41006 RVA: 0x003C71B0 File Offset: 0x003C53B0
	public static void PreloadBundle(DataLoader.DataPathIdentifier _identifier)
	{
		if (_identifier.IsBundle)
		{
			AssetBundleManager.Instance.LoadAssetBundle(_identifier.BundlePath, _identifier.FromMod);
		}
	}

	// Token: 0x0600A02F RID: 41007 RVA: 0x003C71D1 File Offset: 0x003C53D1
	public static void PreloadBundle(string _uri)
	{
		DataLoader.PreloadBundle(DataLoader.ParseDataPathIdentifier(_uri));
	}

	// Token: 0x020013F5 RID: 5109
	public struct DataPathIdentifier
	{
		// Token: 0x17001318 RID: 4888
		// (get) Token: 0x0600A030 RID: 41008 RVA: 0x003C71DE File Offset: 0x003C53DE
		public bool IsBundle
		{
			get
			{
				return this.Location == DataLoader.DataPathIdentifier.AssetLocation.Bundle;
			}
		}

		// Token: 0x0600A031 RID: 41009 RVA: 0x003C71E9 File Offset: 0x003C53E9
		public DataPathIdentifier(string _assetName, DataLoader.DataPathIdentifier.AssetLocation _location = DataLoader.DataPathIdentifier.AssetLocation.Resources, bool _fromMod = false)
		{
			this.BundlePath = null;
			this.AssetName = _assetName;
			this.Location = _location;
			this.FromMod = _fromMod;
		}

		// Token: 0x0600A032 RID: 41010 RVA: 0x003C7207 File Offset: 0x003C5407
		public DataPathIdentifier(string _assetName, string _bundlePath, bool _fromMod = false)
		{
			this.BundlePath = _bundlePath;
			this.AssetName = _assetName;
			this.Location = DataLoader.DataPathIdentifier.AssetLocation.Bundle;
			this.FromMod = _fromMod;
		}

		// Token: 0x0400794C RID: 31052
		public readonly DataLoader.DataPathIdentifier.AssetLocation Location;

		// Token: 0x0400794D RID: 31053
		public readonly string BundlePath;

		// Token: 0x0400794E RID: 31054
		public readonly string AssetName;

		// Token: 0x0400794F RID: 31055
		public readonly bool FromMod;

		// Token: 0x020013F6 RID: 5110
		public enum AssetLocation
		{
			// Token: 0x04007951 RID: 31057
			Resources,
			// Token: 0x04007952 RID: 31058
			Bundle,
			// Token: 0x04007953 RID: 31059
			Addressable
		}
	}
}

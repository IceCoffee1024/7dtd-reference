using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;

// Token: 0x020013E2 RID: 5090
public static class DataLoader
{
	// Token: 0x06009FCF RID: 40911 RVA: 0x003C4C9C File Offset: 0x003C2E9C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsInResources(string _uri)
	{
		return _uri.IndexOf('#') < 0 && _uri.IndexOf('@') < 0;
	}

	// Token: 0x06009FD0 RID: 40912 RVA: 0x003C4CB8 File Offset: 0x003C2EB8
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

	// Token: 0x06009FD1 RID: 40913 RVA: 0x003C4D45 File Offset: 0x003C2F45
	public static T LoadAsset<T>(DataLoader.DataPathIdentifier _identifier, bool _ignoreDlcEntitlements = false) where T : global::UnityEngine.Object
	{
		return LoadManager.LoadAsset<T>(_identifier, null, null, false, true, _ignoreDlcEntitlements).Asset;
	}

	// Token: 0x06009FD2 RID: 40914 RVA: 0x003C4D57 File Offset: 0x003C2F57
	public static T LoadAsset<T>(string _uri, bool _ignoreDlcEntitlements = false) where T : global::UnityEngine.Object
	{
		return DataLoader.LoadAsset<T>(DataLoader.ParseDataPathIdentifier(_uri), _ignoreDlcEntitlements);
	}

	// Token: 0x06009FD3 RID: 40915 RVA: 0x003C4D65 File Offset: 0x003C2F65
	public static T LoadAsset<T>(AssetReference assetReference, bool _ignoreDlcEntitlements = false) where T : global::UnityEngine.Object
	{
		return LoadManager.LoadAssetFromAddressables<T>(assetReference, null, null, false, true, _ignoreDlcEntitlements).Asset;
	}

	// Token: 0x06009FD4 RID: 40916 RVA: 0x003C4D77 File Offset: 0x003C2F77
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

	// Token: 0x06009FD5 RID: 40917 RVA: 0x003C4D9E File Offset: 0x003C2F9E
	public static void UnloadAsset(string _uri, global::UnityEngine.Object _obj)
	{
		DataLoader.UnloadAsset(DataLoader.ParseDataPathIdentifier(_uri), _obj);
	}

	// Token: 0x06009FD6 RID: 40918 RVA: 0x003C4DAC File Offset: 0x003C2FAC
	public static void PreloadBundle(DataLoader.DataPathIdentifier _identifier)
	{
		if (_identifier.IsBundle)
		{
			AssetBundleManager.Instance.LoadAssetBundle(_identifier.BundlePath, _identifier.FromMod);
		}
	}

	// Token: 0x06009FD7 RID: 40919 RVA: 0x003C4DCD File Offset: 0x003C2FCD
	public static void PreloadBundle(string _uri)
	{
		DataLoader.PreloadBundle(DataLoader.ParseDataPathIdentifier(_uri));
	}

	// Token: 0x020013E3 RID: 5091
	public struct DataPathIdentifier
	{
		// Token: 0x1700130C RID: 4876
		// (get) Token: 0x06009FD8 RID: 40920 RVA: 0x003C4DDA File Offset: 0x003C2FDA
		public bool IsBundle
		{
			get
			{
				return this.Location == DataLoader.DataPathIdentifier.AssetLocation.Bundle;
			}
		}

		// Token: 0x06009FD9 RID: 40921 RVA: 0x003C4DE5 File Offset: 0x003C2FE5
		public DataPathIdentifier(string _assetName, DataLoader.DataPathIdentifier.AssetLocation _location = DataLoader.DataPathIdentifier.AssetLocation.Resources, bool _fromMod = false)
		{
			this.BundlePath = null;
			this.AssetName = _assetName;
			this.Location = _location;
			this.FromMod = _fromMod;
		}

		// Token: 0x06009FDA RID: 40922 RVA: 0x003C4E03 File Offset: 0x003C3003
		public DataPathIdentifier(string _assetName, string _bundlePath, bool _fromMod = false)
		{
			this.BundlePath = _bundlePath;
			this.AssetName = _assetName;
			this.Location = DataLoader.DataPathIdentifier.AssetLocation.Bundle;
			this.FromMod = _fromMod;
		}

		// Token: 0x0400790B RID: 30987
		public readonly DataLoader.DataPathIdentifier.AssetLocation Location;

		// Token: 0x0400790C RID: 30988
		public readonly string BundlePath;

		// Token: 0x0400790D RID: 30989
		public readonly string AssetName;

		// Token: 0x0400790E RID: 30990
		public readonly bool FromMod;

		// Token: 0x020013E4 RID: 5092
		public enum AssetLocation
		{
			// Token: 0x04007910 RID: 30992
			Resources,
			// Token: 0x04007911 RID: 30993
			Bundle,
			// Token: 0x04007912 RID: 30994
			Addressable
		}
	}
}

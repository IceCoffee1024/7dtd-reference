using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C7A RID: 3194
public class TextureLoadingManager : MonoBehaviour
{
	// Token: 0x06006085 RID: 24709 RVA: 0x00266762 File Offset: 0x00264962
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		TextureLoadingManager.Instance = this;
	}

	// Token: 0x06006086 RID: 24710 RVA: 0x0026676C File Offset: 0x0026496C
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (Time.time - this.lastTimeChecked < 1f)
		{
			return;
		}
		this.lastTimeChecked = Time.time;
		for (int i = this.runningRequests.Count - 1; i >= 0; i--)
		{
			TextureLoadingManager.AsyncLoadInfo asyncLoadInfo = this.runningRequests[i];
			TextureLoadingManager.TextureInfo textureInfo;
			if (asyncLoadInfo.resRequest.isDone && this.availableTextures.TryGetValue(asyncLoadInfo.fullPath, out textureInfo))
			{
				textureInfo.tex = (Texture)asyncLoadInfo.resRequest.asset;
				textureInfo.bPending = false;
				if (asyncLoadInfo.material)
				{
					asyncLoadInfo.material.SetTexture(asyncLoadInfo.propName, textureInfo.tex);
				}
				this.runningRequests.RemoveAt(i);
			}
		}
	}

	// Token: 0x06006087 RID: 24711 RVA: 0x00266832 File Offset: 0x00264A32
	public void Cleanup()
	{
		this.availableTextures.Clear();
		this.runningRequests.Clear();
	}

	// Token: 0x06006088 RID: 24712 RVA: 0x0026684C File Offset: 0x00264A4C
	public void LoadTexture(Material _m, string _propName, string _assetPath, string _texName, Texture _lowResTexture)
	{
		if (!Application.isPlaying)
		{
			Texture texture = Resources.Load<Texture2D>(_assetPath + _texName);
			_m.SetTexture(_propName, texture);
			return;
		}
		TextureLoadingManager.TextureInfo textureInfo = null;
		string text = _assetPath + _texName;
		if (this.availableTextures.TryGetValue(text, out textureInfo) && !textureInfo.bPending)
		{
			_m.SetTexture(_propName, textureInfo.tex);
			textureInfo.refCounts++;
			return;
		}
		ResourceRequest resourceRequest = Resources.LoadAsync<Texture2D>(text);
		TextureLoadingManager.AsyncLoadInfo asyncLoadInfo = default(TextureLoadingManager.AsyncLoadInfo);
		asyncLoadInfo.resRequest = resourceRequest;
		asyncLoadInfo.propName = _propName;
		asyncLoadInfo.material = _m;
		asyncLoadInfo.fullPath = text;
		asyncLoadInfo.lowResTexture = _lowResTexture;
		this.runningRequests.Add(asyncLoadInfo);
		if (textureInfo == null)
		{
			textureInfo = new TextureLoadingManager.TextureInfo();
			textureInfo.bPending = true;
			textureInfo.refCounts = 1;
			this.availableTextures.Add(text, textureInfo);
			return;
		}
		textureInfo.refCounts++;
	}

	// Token: 0x06006089 RID: 24713 RVA: 0x00266930 File Offset: 0x00264B30
	public bool UnloadTexture(string _assetPath, string _texName)
	{
		string text = _assetPath + _texName;
		TextureLoadingManager.TextureInfo textureInfo;
		if (this.availableTextures.TryGetValue(text, out textureInfo))
		{
			textureInfo.refCounts--;
			if (textureInfo.refCounts == 0)
			{
				this.availableTextures.Remove(text);
				Resources.UnloadAsset(textureInfo.tex);
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600608A RID: 24714 RVA: 0x00266986 File Offset: 0x00264B86
	public int GetHiResTextureCount()
	{
		return this.availableTextures.Count;
	}

	// Token: 0x04004BFA RID: 19450
	public static TextureLoadingManager Instance;

	// Token: 0x04004BFB RID: 19451
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Dictionary<string, TextureLoadingManager.TextureInfo> availableTextures = new Dictionary<string, TextureLoadingManager.TextureInfo>();

	// Token: 0x04004BFC RID: 19452
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<TextureLoadingManager.AsyncLoadInfo> runningRequests = new List<TextureLoadingManager.AsyncLoadInfo>();

	// Token: 0x04004BFD RID: 19453
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float lastTimeChecked;

	// Token: 0x02000C7B RID: 3195
	[PublicizedFrom(EAccessModifier.Private)]
	public struct AsyncLoadInfo
	{
		// Token: 0x04004BFE RID: 19454
		public ResourceRequest resRequest;

		// Token: 0x04004BFF RID: 19455
		public string propName;

		// Token: 0x04004C00 RID: 19456
		public Material material;

		// Token: 0x04004C01 RID: 19457
		public string fullPath;

		// Token: 0x04004C02 RID: 19458
		public Texture lowResTexture;
	}

	// Token: 0x02000C7C RID: 3196
	[PublicizedFrom(EAccessModifier.Private)]
	public class TextureInfo
	{
		// Token: 0x04004C03 RID: 19459
		public bool bPending;

		// Token: 0x04004C04 RID: 19460
		public int refCounts;

		// Token: 0x04004C05 RID: 19461
		public Texture tex;
	}
}

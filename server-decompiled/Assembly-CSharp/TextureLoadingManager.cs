using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C5D RID: 3165
public class TextureLoadingManager : MonoBehaviour
{
	// Token: 0x06006047 RID: 24647 RVA: 0x00261F3E File Offset: 0x0026013E
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		TextureLoadingManager.Instance = this;
	}

	// Token: 0x06006048 RID: 24648 RVA: 0x00261F48 File Offset: 0x00260148
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

	// Token: 0x06006049 RID: 24649 RVA: 0x0026200E File Offset: 0x0026020E
	public void Cleanup()
	{
		this.availableTextures.Clear();
		this.runningRequests.Clear();
	}

	// Token: 0x0600604A RID: 24650 RVA: 0x00262028 File Offset: 0x00260228
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

	// Token: 0x0600604B RID: 24651 RVA: 0x0026210C File Offset: 0x0026030C
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

	// Token: 0x0600604C RID: 24652 RVA: 0x00262162 File Offset: 0x00260362
	public int GetHiResTextureCount()
	{
		return this.availableTextures.Count;
	}

	// Token: 0x04004B96 RID: 19350
	public static TextureLoadingManager Instance;

	// Token: 0x04004B97 RID: 19351
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Dictionary<string, TextureLoadingManager.TextureInfo> availableTextures = new Dictionary<string, TextureLoadingManager.TextureInfo>();

	// Token: 0x04004B98 RID: 19352
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<TextureLoadingManager.AsyncLoadInfo> runningRequests = new List<TextureLoadingManager.AsyncLoadInfo>();

	// Token: 0x04004B99 RID: 19353
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float lastTimeChecked;

	// Token: 0x02000C5E RID: 3166
	[PublicizedFrom(EAccessModifier.Private)]
	public struct AsyncLoadInfo
	{
		// Token: 0x04004B9A RID: 19354
		public ResourceRequest resRequest;

		// Token: 0x04004B9B RID: 19355
		public string propName;

		// Token: 0x04004B9C RID: 19356
		public Material material;

		// Token: 0x04004B9D RID: 19357
		public string fullPath;

		// Token: 0x04004B9E RID: 19358
		public Texture lowResTexture;
	}

	// Token: 0x02000C5F RID: 3167
	[PublicizedFrom(EAccessModifier.Private)]
	public class TextureInfo
	{
		// Token: 0x04004B9F RID: 19359
		public bool bPending;

		// Token: 0x04004BA0 RID: 19360
		public int refCounts;

		// Token: 0x04004BA1 RID: 19361
		public Texture tex;
	}
}

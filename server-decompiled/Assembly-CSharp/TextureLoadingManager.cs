using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C4B RID: 3147
public class TextureLoadingManager : MonoBehaviour
{
	// Token: 0x06005FBD RID: 24509 RVA: 0x0025F09E File Offset: 0x0025D29E
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		TextureLoadingManager.Instance = this;
	}

	// Token: 0x06005FBE RID: 24510 RVA: 0x0025F0A8 File Offset: 0x0025D2A8
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

	// Token: 0x06005FBF RID: 24511 RVA: 0x0025F16E File Offset: 0x0025D36E
	public void Cleanup()
	{
		this.availableTextures.Clear();
		this.runningRequests.Clear();
	}

	// Token: 0x06005FC0 RID: 24512 RVA: 0x0025F188 File Offset: 0x0025D388
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

	// Token: 0x06005FC1 RID: 24513 RVA: 0x0025F26C File Offset: 0x0025D46C
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

	// Token: 0x06005FC2 RID: 24514 RVA: 0x0025F2C2 File Offset: 0x0025D4C2
	public int GetHiResTextureCount()
	{
		return this.availableTextures.Count;
	}

	// Token: 0x04004AF9 RID: 19193
	public static TextureLoadingManager Instance;

	// Token: 0x04004AFA RID: 19194
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Dictionary<string, TextureLoadingManager.TextureInfo> availableTextures = new Dictionary<string, TextureLoadingManager.TextureInfo>();

	// Token: 0x04004AFB RID: 19195
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<TextureLoadingManager.AsyncLoadInfo> runningRequests = new List<TextureLoadingManager.AsyncLoadInfo>();

	// Token: 0x04004AFC RID: 19196
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float lastTimeChecked;

	// Token: 0x02000C4C RID: 3148
	[PublicizedFrom(EAccessModifier.Private)]
	public struct AsyncLoadInfo
	{
		// Token: 0x04004AFD RID: 19197
		public ResourceRequest resRequest;

		// Token: 0x04004AFE RID: 19198
		public string propName;

		// Token: 0x04004AFF RID: 19199
		public Material material;

		// Token: 0x04004B00 RID: 19200
		public string fullPath;

		// Token: 0x04004B01 RID: 19201
		public Texture lowResTexture;
	}

	// Token: 0x02000C4D RID: 3149
	[PublicizedFrom(EAccessModifier.Private)]
	public class TextureInfo
	{
		// Token: 0x04004B02 RID: 19202
		public bool bPending;

		// Token: 0x04004B03 RID: 19203
		public int refCounts;

		// Token: 0x04004B04 RID: 19204
		public Texture tex;
	}
}

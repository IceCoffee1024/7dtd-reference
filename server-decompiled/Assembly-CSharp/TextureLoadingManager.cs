using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C6F RID: 3183
public class TextureLoadingManager : MonoBehaviour
{
	// Token: 0x06006060 RID: 24672 RVA: 0x0026523A File Offset: 0x0026343A
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		TextureLoadingManager.Instance = this;
	}

	// Token: 0x06006061 RID: 24673 RVA: 0x00265244 File Offset: 0x00263444
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

	// Token: 0x06006062 RID: 24674 RVA: 0x0026530A File Offset: 0x0026350A
	public void Cleanup()
	{
		this.availableTextures.Clear();
		this.runningRequests.Clear();
	}

	// Token: 0x06006063 RID: 24675 RVA: 0x00265324 File Offset: 0x00263524
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

	// Token: 0x06006064 RID: 24676 RVA: 0x00265408 File Offset: 0x00263608
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

	// Token: 0x06006065 RID: 24677 RVA: 0x0026545E File Offset: 0x0026365E
	public int GetHiResTextureCount()
	{
		return this.availableTextures.Count;
	}

	// Token: 0x04004BD5 RID: 19413
	public static TextureLoadingManager Instance;

	// Token: 0x04004BD6 RID: 19414
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Dictionary<string, TextureLoadingManager.TextureInfo> availableTextures = new Dictionary<string, TextureLoadingManager.TextureInfo>();

	// Token: 0x04004BD7 RID: 19415
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<TextureLoadingManager.AsyncLoadInfo> runningRequests = new List<TextureLoadingManager.AsyncLoadInfo>();

	// Token: 0x04004BD8 RID: 19416
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float lastTimeChecked;

	// Token: 0x02000C70 RID: 3184
	[PublicizedFrom(EAccessModifier.Private)]
	public struct AsyncLoadInfo
	{
		// Token: 0x04004BD9 RID: 19417
		public ResourceRequest resRequest;

		// Token: 0x04004BDA RID: 19418
		public string propName;

		// Token: 0x04004BDB RID: 19419
		public Material material;

		// Token: 0x04004BDC RID: 19420
		public string fullPath;

		// Token: 0x04004BDD RID: 19421
		public Texture lowResTexture;
	}

	// Token: 0x02000C71 RID: 3185
	[PublicizedFrom(EAccessModifier.Private)]
	public class TextureInfo
	{
		// Token: 0x04004BDE RID: 19422
		public bool bPending;

		// Token: 0x04004BDF RID: 19423
		public int refCounts;

		// Token: 0x04004BE0 RID: 19424
		public Texture tex;
	}
}

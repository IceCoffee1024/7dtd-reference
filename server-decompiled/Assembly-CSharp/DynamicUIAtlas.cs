using System;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Collections;
using UnityEngine;

// Token: 0x0200129C RID: 4764
public class DynamicUIAtlas : UIAtlas
{
	// Token: 0x140000FC RID: 252
	// (add) Token: 0x06009742 RID: 38722 RVA: 0x0038F9B4 File Offset: 0x0038DBB4
	// (remove) Token: 0x06009743 RID: 38723 RVA: 0x0038F9EC File Offset: 0x0038DBEC
	public event Action AtlasUpdatedEv;

	// Token: 0x06009744 RID: 38724 RVA: 0x0038FA24 File Offset: 0x0038DC24
	public void Awake()
	{
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		if (this.PrebakedAtlas.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
		{
			this.PrebakedAtlas = this.PrebakedAtlas.Substring(0, this.PrebakedAtlas.Length - 4);
		}
		if (!DynamicUIAtlasTools.ReadPrebakedAtlasDescriptor(this.PrebakedAtlas, out this.origSpriteData, out this.elementWidth, out this.elementHeight, out this.paddingSize))
		{
			global::UnityEngine.Object.Destroy(this);
			return;
		}
		base.spriteMaterial = new Material(this.shader);
		base.spriteList = new List<UISpriteData>();
		this.ResetAtlas();
		base.pixelSize = 1f;
		stopwatch.Stop();
		Log.Out("Atlas load took " + stopwatch.ElapsedMilliseconds.ToString() + " ms");
		if (this.AtlasUpdatedEv != null)
		{
			this.AtlasUpdatedEv();
		}
	}

	// Token: 0x06009745 RID: 38725 RVA: 0x0038FB04 File Offset: 0x0038DD04
	[PublicizedFrom(EAccessModifier.Private)]
	public void LoadBaseTexture()
	{
		Texture2D texture2D;
		if (!DynamicUIAtlasTools.ReadPrebakedAtlasTexture(this.PrebakedAtlas, out texture2D))
		{
			global::UnityEngine.Object.Destroy(this);
			return;
		}
		this.currentTex = new Texture2D(texture2D.width, texture2D.height, texture2D.format, texture2D.mipmapCount > 1);
		NativeArray<byte> rawTextureData = texture2D.GetRawTextureData<byte>();
		NativeArray<byte> rawTextureData2 = this.currentTex.GetRawTextureData<byte>();
		rawTextureData.CopyTo(rawTextureData2);
		DynamicUIAtlasTools.UnloadTex(this.PrebakedAtlas, texture2D);
	}

	// Token: 0x06009746 RID: 38726 RVA: 0x0038FB74 File Offset: 0x0038DD74
	public void LoadAdditionalSprites(Dictionary<string, Texture2D> _nameToTex)
	{
		DynamicUIAtlasTools.AddSprites(this.elementWidth, this.elementHeight, this.paddingSize, _nameToTex, ref this.currentTex, base.spriteList);
		base.spriteMaterial.mainTexture = this.currentTex;
		this.currentTex.Apply();
		if (this.AtlasUpdatedEv != null)
		{
			this.AtlasUpdatedEv();
		}
	}

	// Token: 0x06009747 RID: 38727 RVA: 0x0038FBD4 File Offset: 0x0038DDD4
	public void ResetAtlas()
	{
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		if (this.currentTex != null)
		{
			global::UnityEngine.Object.Destroy(this.currentTex);
		}
		base.spriteList.Clear();
		this.LoadBaseTexture();
		base.spriteMaterial.mainTexture = this.currentTex;
		this.currentTex.Apply();
		base.spriteList.AddRange(this.origSpriteData);
		stopwatch.Stop();
		Log.Out("Atlas reset took " + stopwatch.ElapsedMilliseconds.ToString() + " ms");
		if (this.AtlasUpdatedEv != null)
		{
			this.AtlasUpdatedEv();
		}
	}

	// Token: 0x06009748 RID: 38728 RVA: 0x0038FC7F File Offset: 0x0038DE7F
	public void Compress()
	{
		this.currentTex.Compress(true);
		this.currentTex.Apply(false, true);
	}

	// Token: 0x06009749 RID: 38729 RVA: 0x0038FC9C File Offset: 0x0038DE9C
	public static DynamicUIAtlas Create(GameObject _parent, string _prebakedAtlasResourceName, Shader _shader)
	{
		string text = _prebakedAtlasResourceName;
		int num;
		if ((num = _prebakedAtlasResourceName.IndexOf('?')) >= 0)
		{
			text = text.Substring(num + 1);
		}
		GameObject gameObject = new GameObject(text);
		gameObject.transform.parent = _parent.transform;
		gameObject.SetActive(false);
		DynamicUIAtlas dynamicUIAtlas = gameObject.AddComponent<DynamicUIAtlas>();
		dynamicUIAtlas.PrebakedAtlas = _prebakedAtlasResourceName;
		dynamicUIAtlas.shader = _shader;
		gameObject.SetActive(true);
		return dynamicUIAtlas;
	}

	// Token: 0x0400714E RID: 29006
	public Shader shader;

	// Token: 0x0400714F RID: 29007
	public string PrebakedAtlas;

	// Token: 0x04007151 RID: 29009
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int elementWidth;

	// Token: 0x04007152 RID: 29010
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int elementHeight;

	// Token: 0x04007153 RID: 29011
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int paddingSize;

	// Token: 0x04007154 RID: 29012
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<UISpriteData> origSpriteData;

	// Token: 0x04007155 RID: 29013
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Texture2D currentTex;
}

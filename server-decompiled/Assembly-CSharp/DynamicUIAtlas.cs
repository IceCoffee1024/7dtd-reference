using System;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Collections;
using UnityEngine;

// Token: 0x02001284 RID: 4740
public class DynamicUIAtlas : UIAtlas
{
	// Token: 0x140000FD RID: 253
	// (add) Token: 0x060096B5 RID: 38581 RVA: 0x0038B16C File Offset: 0x0038936C
	// (remove) Token: 0x060096B6 RID: 38582 RVA: 0x0038B1A4 File Offset: 0x003893A4
	public event Action AtlasUpdatedEv;

	// Token: 0x060096B7 RID: 38583 RVA: 0x0038B1DC File Offset: 0x003893DC
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

	// Token: 0x060096B8 RID: 38584 RVA: 0x0038B2BC File Offset: 0x003894BC
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

	// Token: 0x060096B9 RID: 38585 RVA: 0x0038B32C File Offset: 0x0038952C
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

	// Token: 0x060096BA RID: 38586 RVA: 0x0038B38C File Offset: 0x0038958C
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

	// Token: 0x060096BB RID: 38587 RVA: 0x0038B437 File Offset: 0x00389637
	public void Compress()
	{
		this.currentTex.Compress(true);
		this.currentTex.Apply(false, true);
	}

	// Token: 0x060096BC RID: 38588 RVA: 0x0038B454 File Offset: 0x00389654
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

	// Token: 0x040070E4 RID: 28900
	public Shader shader;

	// Token: 0x040070E5 RID: 28901
	public string PrebakedAtlas;

	// Token: 0x040070E7 RID: 28903
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int elementWidth;

	// Token: 0x040070E8 RID: 28904
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int elementHeight;

	// Token: 0x040070E9 RID: 28905
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int paddingSize;

	// Token: 0x040070EA RID: 28906
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<UISpriteData> origSpriteData;

	// Token: 0x040070EB RID: 28907
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Texture2D currentTex;
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Collections;
using UnityEngine;

// Token: 0x0200126A RID: 4714
public class DynamicUIAtlas : UIAtlas
{
	// Token: 0x140000FC RID: 252
	// (add) Token: 0x060095BA RID: 38330 RVA: 0x00386010 File Offset: 0x00384210
	// (remove) Token: 0x060095BB RID: 38331 RVA: 0x00386048 File Offset: 0x00384248
	public event Action AtlasUpdatedEv;

	// Token: 0x060095BC RID: 38332 RVA: 0x00386080 File Offset: 0x00384280
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

	// Token: 0x060095BD RID: 38333 RVA: 0x00386160 File Offset: 0x00384360
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

	// Token: 0x060095BE RID: 38334 RVA: 0x003861D0 File Offset: 0x003843D0
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

	// Token: 0x060095BF RID: 38335 RVA: 0x00386230 File Offset: 0x00384430
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

	// Token: 0x060095C0 RID: 38336 RVA: 0x003862DB File Offset: 0x003844DB
	public void Compress()
	{
		this.currentTex.Compress(true);
		this.currentTex.Apply(false, true);
	}

	// Token: 0x060095C1 RID: 38337 RVA: 0x003862F8 File Offset: 0x003844F8
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

	// Token: 0x0400700F RID: 28687
	public Shader shader;

	// Token: 0x04007010 RID: 28688
	public string PrebakedAtlas;

	// Token: 0x04007012 RID: 28690
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int elementWidth;

	// Token: 0x04007013 RID: 28691
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int elementHeight;

	// Token: 0x04007014 RID: 28692
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int paddingSize;

	// Token: 0x04007015 RID: 28693
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<UISpriteData> origSpriteData;

	// Token: 0x04007016 RID: 28694
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Texture2D currentTex;
}

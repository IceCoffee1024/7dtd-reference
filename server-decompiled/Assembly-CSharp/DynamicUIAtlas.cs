using System;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Collections;
using UnityEngine;

// Token: 0x020012AE RID: 4782
public class DynamicUIAtlas : UIAtlas
{
	// Token: 0x140000FC RID: 252
	// (add) Token: 0x0600979A RID: 38810 RVA: 0x00391DBC File Offset: 0x0038FFBC
	// (remove) Token: 0x0600979B RID: 38811 RVA: 0x00391DF4 File Offset: 0x0038FFF4
	public event Action AtlasUpdatedEv;

	// Token: 0x0600979C RID: 38812 RVA: 0x00391E2C File Offset: 0x0039002C
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

	// Token: 0x0600979D RID: 38813 RVA: 0x00391F0C File Offset: 0x0039010C
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

	// Token: 0x0600979E RID: 38814 RVA: 0x00391F7C File Offset: 0x0039017C
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

	// Token: 0x0600979F RID: 38815 RVA: 0x00391FDC File Offset: 0x003901DC
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

	// Token: 0x060097A0 RID: 38816 RVA: 0x00392087 File Offset: 0x00390287
	public void Compress()
	{
		this.currentTex.Compress(true);
		this.currentTex.Apply(false, true);
	}

	// Token: 0x060097A1 RID: 38817 RVA: 0x003920A4 File Offset: 0x003902A4
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

	// Token: 0x0400718F RID: 29071
	public Shader shader;

	// Token: 0x04007190 RID: 29072
	public string PrebakedAtlas;

	// Token: 0x04007192 RID: 29074
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int elementWidth;

	// Token: 0x04007193 RID: 29075
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int elementHeight;

	// Token: 0x04007194 RID: 29076
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int paddingSize;

	// Token: 0x04007195 RID: 29077
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<UISpriteData> origSpriteData;

	// Token: 0x04007196 RID: 29078
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Texture2D currentTex;
}

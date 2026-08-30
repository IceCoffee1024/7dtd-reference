using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000BAE RID: 2990
[Preserve]
public class TextureAtlasBlocks : TextureAtlas
{
	// Token: 0x06005A64 RID: 23140 RVA: 0x0022CA78 File Offset: 0x0022AC78
	public override bool LoadTextureAtlas(int _idx, MeshDescriptionCollection _tac, bool _bLoadTextures)
	{
		try
		{
			this.LoadTextureAtlasFromMetadata(_idx, _tac);
		}
		catch (Exception ex)
		{
			Log.Error(string.Concat(new string[]
			{
				"Parsing file xml file for texture atlas ",
				_tac.name,
				" (",
				_idx.ToString(),
				"): ",
				ex.Message,
				")"
			}));
			Log.Exception(ex);
			Log.Error("Loading of textures aborted due to errors!");
			return false;
		}
		base.LoadTextureAtlas(_idx, _tac, _bLoadTextures);
		return true;
	}

	// Token: 0x06005A65 RID: 23141 RVA: 0x0022CB10 File Offset: 0x0022AD10
	[PublicizedFrom(EAccessModifier.Private)]
	public void LoadTextureAtlasFromMetadata(int _idx, MeshDescriptionCollection _tac)
	{
		XElement root = new XmlFile(_tac.meshes[_idx].MetaData).XmlDoc.Root;
		int num = 0;
		foreach (XElement xelement in root.Elements("uv"))
		{
			num = Math.Max(num, int.Parse(xelement.GetAttribute("id")));
		}
		this.uvMapping = new UVRectTiling[num + 1];
		foreach (XElement xelement2 in root.Elements("uv"))
		{
			int num2 = int.Parse(xelement2.GetAttribute("id"));
			UVRectTiling uvrectTiling = default(UVRectTiling);
			uvrectTiling.FromXML(xelement2);
			this.uvMapping[num2] = uvrectTiling;
		}
	}

	// Token: 0x06005A66 RID: 23142 RVA: 0x0022CC24 File Offset: 0x0022AE24
	public override void Cleanup()
	{
		if (this.diffuseTexture != null)
		{
			Resources.UnloadAsset(this.diffuseTexture);
			this.diffuseTexture = null;
		}
		if (this.normalTexture != null)
		{
			Resources.UnloadAsset(this.normalTexture);
			this.normalTexture = null;
		}
		if (this.maskTexture != null)
		{
			Resources.UnloadAsset(this.maskTexture);
			this.maskTexture = null;
		}
		if (this.maskNormalTexture != null)
		{
			Resources.UnloadAsset(this.maskNormalTexture);
			this.maskNormalTexture = null;
		}
		if (this.emissionTexture != null)
		{
			Resources.UnloadAsset(this.emissionTexture);
			this.emissionTexture = null;
		}
		if (this.specularTexture != null)
		{
			Resources.UnloadAsset(this.specularTexture);
			this.specularTexture = null;
		}
		if (this.heightTexture != null)
		{
			Resources.UnloadAsset(this.heightTexture);
			this.heightTexture = null;
		}
		if (this.occlusionTexture != null)
		{
			Resources.UnloadAsset(this.occlusionTexture);
			this.occlusionTexture = null;
		}
	}

	// Token: 0x040045F6 RID: 17910
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cTextureArraySize = 512;

	// Token: 0x040045F7 RID: 17911
	[PublicizedFrom(EAccessModifier.Private)]
	public static int cTextureBorder = 34;

	// Token: 0x040045F8 RID: 17912
	[PublicizedFrom(EAccessModifier.Private)]
	public static int cTextureSize = 8192;

	// Token: 0x02000BAF RID: 2991
	public enum WrapMode
	{
		// Token: 0x040045FA RID: 17914
		Mirror,
		// Token: 0x040045FB RID: 17915
		Wrap,
		// Token: 0x040045FC RID: 17916
		TransparentEdges
	}

	// Token: 0x02000BB0 RID: 2992
	[PublicizedFrom(EAccessModifier.Private)]
	public struct BlockUVRect
	{
		// Token: 0x06005A68 RID: 23144 RVA: 0x0022CD44 File Offset: 0x0022AF44
		public BlockUVRect(int _textureId, int _x, int _y, int _width, int _height, int _blocksW, int _blocksH, Color _color, bool _bGlobalUV)
		{
			this.textureId = _textureId;
			this.x = _x;
			this.y = _y;
			this.width = _width;
			this.height = _height;
			this.blocksW = _blocksW;
			this.blocksH = _blocksH;
			this.color = _color;
			this.bGlobalUV = _bGlobalUV;
		}

		// Token: 0x06005A69 RID: 23145 RVA: 0x0022CD98 File Offset: 0x0022AF98
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"texId=",
				this.textureId.ToString(),
				" x/y=",
				this.x.ToString(),
				"/",
				this.y.ToString(),
				" w/h=",
				this.width.ToString(),
				"/",
				this.height.ToString(),
				" block W/H=",
				this.blocksW.ToString(),
				"/",
				this.blocksH.ToString()
			});
		}

		// Token: 0x040045FD RID: 17917
		public int textureId;

		// Token: 0x040045FE RID: 17918
		public int x;

		// Token: 0x040045FF RID: 17919
		public int y;

		// Token: 0x04004600 RID: 17920
		public int width;

		// Token: 0x04004601 RID: 17921
		public int height;

		// Token: 0x04004602 RID: 17922
		public int blocksW;

		// Token: 0x04004603 RID: 17923
		public int blocksH;

		// Token: 0x04004604 RID: 17924
		public Color color;

		// Token: 0x04004605 RID: 17925
		public bool bGlobalUV;
	}

	// Token: 0x02000BB1 RID: 2993
	[PublicizedFrom(EAccessModifier.Private)]
	public class BlocksTexture
	{
		// Token: 0x06005A6A RID: 23146 RVA: 0x0022CE50 File Offset: 0x0022B050
		public override string ToString()
		{
			string[] array = new string[7];
			array[0] = "diffuse=";
			int num = 1;
			Texture2D texture2D = this.diffuse;
			array[num] = ((texture2D != null) ? texture2D.ToString() : null);
			array[2] = " normal=";
			int num2 = 3;
			Texture2D texture2D2 = this.normal;
			array[num2] = ((texture2D2 != null) ? texture2D2.ToString() : null);
			array[4] = " count=";
			array[5] = this.blocks.Count.ToString();
			array[6] = "\n";
			string text = string.Concat(array);
			for (int i = 0; i < this.blocks.Count; i++)
			{
				TextureAtlasBlocks.BlockUVRect blockUVRect = this.blocks[i];
				string text2 = text;
				TextureAtlasBlocks.BlockUVRect blockUVRect2 = blockUVRect;
				text = text2 + blockUVRect2.ToString() + "\n";
			}
			return text;
		}

		// Token: 0x04004606 RID: 17926
		public List<TextureAtlasBlocks.BlockUVRect> blocks = new List<TextureAtlasBlocks.BlockUVRect>();

		// Token: 0x04004607 RID: 17927
		public string textureName;

		// Token: 0x04004608 RID: 17928
		public Texture2D diffuse;

		// Token: 0x04004609 RID: 17929
		public Texture2D normal;

		// Token: 0x0400460A RID: 17930
		public Texture2D specular;

		// Token: 0x0400460B RID: 17931
		public Texture2D height;

		// Token: 0x0400460C RID: 17932
		public TextureAtlasBlocks.WrapMode wrapMode;

		// Token: 0x0400460D RID: 17933
		public int diffuseW;

		// Token: 0x0400460E RID: 17934
		public int diffuseH;

		// Token: 0x0400460F RID: 17935
		public Color color;
	}
}

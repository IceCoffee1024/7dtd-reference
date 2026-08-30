using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000BC0 RID: 3008
[Preserve]
public class TextureAtlasBlocks : TextureAtlas
{
	// Token: 0x06005AEC RID: 23276 RVA: 0x0022F7E0 File Offset: 0x0022D9E0
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

	// Token: 0x06005AED RID: 23277 RVA: 0x0022F878 File Offset: 0x0022DA78
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

	// Token: 0x06005AEE RID: 23278 RVA: 0x0022F98C File Offset: 0x0022DB8C
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

	// Token: 0x04004692 RID: 18066
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cTextureArraySize = 512;

	// Token: 0x04004693 RID: 18067
	[PublicizedFrom(EAccessModifier.Private)]
	public static int cTextureBorder = 34;

	// Token: 0x04004694 RID: 18068
	[PublicizedFrom(EAccessModifier.Private)]
	public static int cTextureSize = 8192;

	// Token: 0x02000BC1 RID: 3009
	public enum WrapMode
	{
		// Token: 0x04004696 RID: 18070
		Mirror,
		// Token: 0x04004697 RID: 18071
		Wrap,
		// Token: 0x04004698 RID: 18072
		TransparentEdges
	}

	// Token: 0x02000BC2 RID: 3010
	[PublicizedFrom(EAccessModifier.Private)]
	public struct BlockUVRect
	{
		// Token: 0x06005AF0 RID: 23280 RVA: 0x0022FAAC File Offset: 0x0022DCAC
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

		// Token: 0x06005AF1 RID: 23281 RVA: 0x0022FB00 File Offset: 0x0022DD00
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

		// Token: 0x04004699 RID: 18073
		public int textureId;

		// Token: 0x0400469A RID: 18074
		public int x;

		// Token: 0x0400469B RID: 18075
		public int y;

		// Token: 0x0400469C RID: 18076
		public int width;

		// Token: 0x0400469D RID: 18077
		public int height;

		// Token: 0x0400469E RID: 18078
		public int blocksW;

		// Token: 0x0400469F RID: 18079
		public int blocksH;

		// Token: 0x040046A0 RID: 18080
		public Color color;

		// Token: 0x040046A1 RID: 18081
		public bool bGlobalUV;
	}

	// Token: 0x02000BC3 RID: 3011
	[PublicizedFrom(EAccessModifier.Private)]
	public class BlocksTexture
	{
		// Token: 0x06005AF2 RID: 23282 RVA: 0x0022FBB8 File Offset: 0x0022DDB8
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

		// Token: 0x040046A2 RID: 18082
		public List<TextureAtlasBlocks.BlockUVRect> blocks = new List<TextureAtlasBlocks.BlockUVRect>();

		// Token: 0x040046A3 RID: 18083
		public string textureName;

		// Token: 0x040046A4 RID: 18084
		public Texture2D diffuse;

		// Token: 0x040046A5 RID: 18085
		public Texture2D normal;

		// Token: 0x040046A6 RID: 18086
		public Texture2D specular;

		// Token: 0x040046A7 RID: 18087
		public Texture2D height;

		// Token: 0x040046A8 RID: 18088
		public TextureAtlasBlocks.WrapMode wrapMode;

		// Token: 0x040046A9 RID: 18089
		public int diffuseW;

		// Token: 0x040046AA RID: 18090
		public int diffuseH;

		// Token: 0x040046AB RID: 18091
		public Color color;
	}
}

using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000BDD RID: 3037
[Preserve]
public class TextureAtlasBlocks : TextureAtlas
{
	// Token: 0x06005B2A RID: 23338 RVA: 0x00234038 File Offset: 0x00232238
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

	// Token: 0x06005B2B RID: 23339 RVA: 0x002340D0 File Offset: 0x002322D0
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

	// Token: 0x06005B2C RID: 23340 RVA: 0x002341E4 File Offset: 0x002323E4
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

	// Token: 0x040046F6 RID: 18166
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cTextureArraySize = 512;

	// Token: 0x040046F7 RID: 18167
	[PublicizedFrom(EAccessModifier.Private)]
	public static int cTextureBorder = 34;

	// Token: 0x040046F8 RID: 18168
	[PublicizedFrom(EAccessModifier.Private)]
	public static int cTextureSize = 8192;

	// Token: 0x02000BDE RID: 3038
	public enum WrapMode
	{
		// Token: 0x040046FA RID: 18170
		Mirror,
		// Token: 0x040046FB RID: 18171
		Wrap,
		// Token: 0x040046FC RID: 18172
		TransparentEdges
	}

	// Token: 0x02000BDF RID: 3039
	[PublicizedFrom(EAccessModifier.Private)]
	public struct BlockUVRect
	{
		// Token: 0x06005B2E RID: 23342 RVA: 0x00234304 File Offset: 0x00232504
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

		// Token: 0x06005B2F RID: 23343 RVA: 0x00234358 File Offset: 0x00232558
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

		// Token: 0x040046FD RID: 18173
		public int textureId;

		// Token: 0x040046FE RID: 18174
		public int x;

		// Token: 0x040046FF RID: 18175
		public int y;

		// Token: 0x04004700 RID: 18176
		public int width;

		// Token: 0x04004701 RID: 18177
		public int height;

		// Token: 0x04004702 RID: 18178
		public int blocksW;

		// Token: 0x04004703 RID: 18179
		public int blocksH;

		// Token: 0x04004704 RID: 18180
		public Color color;

		// Token: 0x04004705 RID: 18181
		public bool bGlobalUV;
	}

	// Token: 0x02000BE0 RID: 3040
	[PublicizedFrom(EAccessModifier.Private)]
	public class BlocksTexture
	{
		// Token: 0x06005B30 RID: 23344 RVA: 0x00234410 File Offset: 0x00232610
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

		// Token: 0x04004706 RID: 18182
		public List<TextureAtlasBlocks.BlockUVRect> blocks = new List<TextureAtlasBlocks.BlockUVRect>();

		// Token: 0x04004707 RID: 18183
		public string textureName;

		// Token: 0x04004708 RID: 18184
		public Texture2D diffuse;

		// Token: 0x04004709 RID: 18185
		public Texture2D normal;

		// Token: 0x0400470A RID: 18186
		public Texture2D specular;

		// Token: 0x0400470B RID: 18187
		public Texture2D height;

		// Token: 0x0400470C RID: 18188
		public TextureAtlasBlocks.WrapMode wrapMode;

		// Token: 0x0400470D RID: 18189
		public int diffuseW;

		// Token: 0x0400470E RID: 18190
		public int diffuseH;

		// Token: 0x0400470F RID: 18191
		public Color color;
	}
}

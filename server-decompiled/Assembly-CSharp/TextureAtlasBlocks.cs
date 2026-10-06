using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000BD2 RID: 3026
[Preserve]
public class TextureAtlasBlocks : TextureAtlas
{
	// Token: 0x06005B05 RID: 23301 RVA: 0x00232ADC File Offset: 0x00230CDC
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

	// Token: 0x06005B06 RID: 23302 RVA: 0x00232B74 File Offset: 0x00230D74
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

	// Token: 0x06005B07 RID: 23303 RVA: 0x00232C88 File Offset: 0x00230E88
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

	// Token: 0x040046D1 RID: 18129
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cTextureArraySize = 512;

	// Token: 0x040046D2 RID: 18130
	[PublicizedFrom(EAccessModifier.Private)]
	public static int cTextureBorder = 34;

	// Token: 0x040046D3 RID: 18131
	[PublicizedFrom(EAccessModifier.Private)]
	public static int cTextureSize = 8192;

	// Token: 0x02000BD3 RID: 3027
	public enum WrapMode
	{
		// Token: 0x040046D5 RID: 18133
		Mirror,
		// Token: 0x040046D6 RID: 18134
		Wrap,
		// Token: 0x040046D7 RID: 18135
		TransparentEdges
	}

	// Token: 0x02000BD4 RID: 3028
	[PublicizedFrom(EAccessModifier.Private)]
	public struct BlockUVRect
	{
		// Token: 0x06005B09 RID: 23305 RVA: 0x00232DA8 File Offset: 0x00230FA8
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

		// Token: 0x06005B0A RID: 23306 RVA: 0x00232DFC File Offset: 0x00230FFC
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

		// Token: 0x040046D8 RID: 18136
		public int textureId;

		// Token: 0x040046D9 RID: 18137
		public int x;

		// Token: 0x040046DA RID: 18138
		public int y;

		// Token: 0x040046DB RID: 18139
		public int width;

		// Token: 0x040046DC RID: 18140
		public int height;

		// Token: 0x040046DD RID: 18141
		public int blocksW;

		// Token: 0x040046DE RID: 18142
		public int blocksH;

		// Token: 0x040046DF RID: 18143
		public Color color;

		// Token: 0x040046E0 RID: 18144
		public bool bGlobalUV;
	}

	// Token: 0x02000BD5 RID: 3029
	[PublicizedFrom(EAccessModifier.Private)]
	public class BlocksTexture
	{
		// Token: 0x06005B0B RID: 23307 RVA: 0x00232EB4 File Offset: 0x002310B4
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

		// Token: 0x040046E1 RID: 18145
		public List<TextureAtlasBlocks.BlockUVRect> blocks = new List<TextureAtlasBlocks.BlockUVRect>();

		// Token: 0x040046E2 RID: 18146
		public string textureName;

		// Token: 0x040046E3 RID: 18147
		public Texture2D diffuse;

		// Token: 0x040046E4 RID: 18148
		public Texture2D normal;

		// Token: 0x040046E5 RID: 18149
		public Texture2D specular;

		// Token: 0x040046E6 RID: 18150
		public Texture2D height;

		// Token: 0x040046E7 RID: 18151
		public TextureAtlasBlocks.WrapMode wrapMode;

		// Token: 0x040046E8 RID: 18152
		public int diffuseW;

		// Token: 0x040046E9 RID: 18153
		public int diffuseH;

		// Token: 0x040046EA RID: 18154
		public Color color;
	}
}

using System;
using UnityEngine;

// Token: 0x020014FB RID: 5371
public class TextureAtlas
{
	// Token: 0x0600A814 RID: 43028 RVA: 0x003E945B File Offset: 0x003E765B
	public TextureAtlas()
		: this(true)
	{
	}

	// Token: 0x0600A815 RID: 43029 RVA: 0x003E9464 File Offset: 0x003E7664
	public TextureAtlas(bool _bDestroyTextures)
	{
		this.bDestroyTextures = _bDestroyTextures;
	}

	// Token: 0x0600A816 RID: 43030 RVA: 0x003E9480 File Offset: 0x003E7680
	public virtual bool LoadTextureAtlas(int _idx, MeshDescriptionCollection _tac, bool _bLoadTextures)
	{
		if (_bLoadTextures)
		{
			MeshDescription meshDescription = _tac.Meshes[_idx];
			this.diffuseTexture = meshDescription.TexDiffuse;
			this.normalTexture = meshDescription.TexNormal;
			this.specularTexture = meshDescription.TexSpecular;
			this.emissionTexture = meshDescription.TexEmission;
			this.heightTexture = meshDescription.TexHeight;
			this.occlusionTexture = meshDescription.TexOcclusion;
			this.maskTexture = meshDescription.TexMask;
			this.maskNormalTexture = meshDescription.TexMaskNormal;
		}
		return true;
	}

	// Token: 0x0600A817 RID: 43031 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Cleanup()
	{
	}

	// Token: 0x04007CC5 RID: 31941
	public Texture diffuseTexture;

	// Token: 0x04007CC6 RID: 31942
	public Texture normalTexture;

	// Token: 0x04007CC7 RID: 31943
	public Texture specularTexture;

	// Token: 0x04007CC8 RID: 31944
	public Texture emissionTexture;

	// Token: 0x04007CC9 RID: 31945
	public Texture heightTexture;

	// Token: 0x04007CCA RID: 31946
	public Texture occlusionTexture;

	// Token: 0x04007CCB RID: 31947
	public Texture2D maskTexture;

	// Token: 0x04007CCC RID: 31948
	public Texture2D maskNormalTexture;

	// Token: 0x04007CCD RID: 31949
	public UVRectTiling[] uvMapping = new UVRectTiling[0];

	// Token: 0x04007CCE RID: 31950
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool bDestroyTextures;
}

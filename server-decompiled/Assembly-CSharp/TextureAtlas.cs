using System;
using UnityEngine;

// Token: 0x020014AC RID: 5292
public class TextureAtlas
{
	// Token: 0x0600A60C RID: 42508 RVA: 0x003DCEC3 File Offset: 0x003DB0C3
	public TextureAtlas()
		: this(true)
	{
	}

	// Token: 0x0600A60D RID: 42509 RVA: 0x003DCECC File Offset: 0x003DB0CC
	public TextureAtlas(bool _bDestroyTextures)
	{
		this.bDestroyTextures = _bDestroyTextures;
	}

	// Token: 0x0600A60E RID: 42510 RVA: 0x003DCEE8 File Offset: 0x003DB0E8
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

	// Token: 0x0600A60F RID: 42511 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Cleanup()
	{
	}

	// Token: 0x04007B28 RID: 31528
	public Texture diffuseTexture;

	// Token: 0x04007B29 RID: 31529
	public Texture normalTexture;

	// Token: 0x04007B2A RID: 31530
	public Texture specularTexture;

	// Token: 0x04007B2B RID: 31531
	public Texture emissionTexture;

	// Token: 0x04007B2C RID: 31532
	public Texture heightTexture;

	// Token: 0x04007B2D RID: 31533
	public Texture occlusionTexture;

	// Token: 0x04007B2E RID: 31534
	public Texture2D maskTexture;

	// Token: 0x04007B2F RID: 31535
	public Texture2D maskNormalTexture;

	// Token: 0x04007B30 RID: 31536
	public UVRectTiling[] uvMapping = new UVRectTiling[0];

	// Token: 0x04007B31 RID: 31537
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool bDestroyTextures;
}

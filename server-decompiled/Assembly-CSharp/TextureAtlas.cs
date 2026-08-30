using System;
using UnityEngine;

// Token: 0x020014C7 RID: 5319
public class TextureAtlas
{
	// Token: 0x0600A709 RID: 42761 RVA: 0x003E2203 File Offset: 0x003E0403
	public TextureAtlas()
		: this(true)
	{
	}

	// Token: 0x0600A70A RID: 42762 RVA: 0x003E220C File Offset: 0x003E040C
	public TextureAtlas(bool _bDestroyTextures)
	{
		this.bDestroyTextures = _bDestroyTextures;
	}

	// Token: 0x0600A70B RID: 42763 RVA: 0x003E2228 File Offset: 0x003E0428
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

	// Token: 0x0600A70C RID: 42764 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Cleanup()
	{
	}

	// Token: 0x04007C01 RID: 31745
	public Texture diffuseTexture;

	// Token: 0x04007C02 RID: 31746
	public Texture normalTexture;

	// Token: 0x04007C03 RID: 31747
	public Texture specularTexture;

	// Token: 0x04007C04 RID: 31748
	public Texture emissionTexture;

	// Token: 0x04007C05 RID: 31749
	public Texture heightTexture;

	// Token: 0x04007C06 RID: 31750
	public Texture occlusionTexture;

	// Token: 0x04007C07 RID: 31751
	public Texture2D maskTexture;

	// Token: 0x04007C08 RID: 31752
	public Texture2D maskNormalTexture;

	// Token: 0x04007C09 RID: 31753
	public UVRectTiling[] uvMapping = new UVRectTiling[0];

	// Token: 0x04007C0A RID: 31754
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool bDestroyTextures;
}

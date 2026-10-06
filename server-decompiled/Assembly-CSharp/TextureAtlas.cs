using System;
using UnityEngine;

// Token: 0x020014E9 RID: 5353
public class TextureAtlas
{
	// Token: 0x0600A7BC RID: 42940 RVA: 0x003E7057 File Offset: 0x003E5257
	public TextureAtlas()
		: this(true)
	{
	}

	// Token: 0x0600A7BD RID: 42941 RVA: 0x003E7060 File Offset: 0x003E5260
	public TextureAtlas(bool _bDestroyTextures)
	{
		this.bDestroyTextures = _bDestroyTextures;
	}

	// Token: 0x0600A7BE RID: 42942 RVA: 0x003E707C File Offset: 0x003E527C
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

	// Token: 0x0600A7BF RID: 42943 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Cleanup()
	{
	}

	// Token: 0x04007C84 RID: 31876
	public Texture diffuseTexture;

	// Token: 0x04007C85 RID: 31877
	public Texture normalTexture;

	// Token: 0x04007C86 RID: 31878
	public Texture specularTexture;

	// Token: 0x04007C87 RID: 31879
	public Texture emissionTexture;

	// Token: 0x04007C88 RID: 31880
	public Texture heightTexture;

	// Token: 0x04007C89 RID: 31881
	public Texture occlusionTexture;

	// Token: 0x04007C8A RID: 31882
	public Texture2D maskTexture;

	// Token: 0x04007C8B RID: 31883
	public Texture2D maskNormalTexture;

	// Token: 0x04007C8C RID: 31884
	public UVRectTiling[] uvMapping = new UVRectTiling[0];

	// Token: 0x04007C8D RID: 31885
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool bDestroyTextures;
}

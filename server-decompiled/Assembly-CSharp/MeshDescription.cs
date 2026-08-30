using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

// Token: 0x02000B6C RID: 2924
[Serializable]
public class MeshDescription
{
	// Token: 0x06005883 RID: 22659 RVA: 0x0021C91C File Offset: 0x0021AB1C
	public IEnumerator Init(int _idx, TextureAtlas _ta)
	{
		this.textureAtlas = _ta;
		if (!GameManager.IsDedicatedServer)
		{
			if (this.UseSplatmap(_idx))
			{
				this.materials = new Material[1];
				LoadManager.AddressableRequestTask<Material> assetRequestTask = LoadManager.LoadAssetFromAddressables<Material>("TerrainTextures", "Microsplat/MicroSplatTerrainInGame.mat", null, null, false, ThreadManager.IsInSyncCoroutine, false);
				yield return assetRequestTask;
				this.materials[0] = global::UnityEngine.Object.Instantiate<Material>(assetRequestTask.Asset);
				this.materials[0].name = "Near Terrain";
				this.materials[0].SetFloat("_ShaderMode", 2f);
				this.materialDistant = new Material(this.materials[0]);
				this.materialDistant.SetFloat("_ShaderMode", 1f);
				this.materialDistant.name = "Distant Terrain";
				this.ReloadTextureArrays(true);
				assetRequestTask.Release();
				if (_idx == 5)
				{
					yield return this.setupPrefabTerrainMaterials(_idx, _ta);
				}
				assetRequestTask = null;
			}
			else if (_idx == 2)
			{
				if (_ta == null)
				{
					yield break;
				}
				Material material = new Material(this.BaseMaterial);
				Material material2 = new Material(this.SecondaryMaterial);
				material.SetTexture("_Albedo", _ta.diffuseTexture);
				material.SetTexture("_NormalMap", _ta.normalTexture);
				material.SetTexture("_GME", _ta.specularTexture);
				material2.SetTexture("_Albedo", _ta.diffuseTexture);
				this.materials = new Material[2];
				this.materials[0] = material2;
				this.materials[1] = material;
			}
			else
			{
				AssetReference secondaryShader = this.SecondaryShader;
				if (secondaryShader != null && secondaryShader.RuntimeKeyIsValid())
				{
					this.materials = new Material[2];
				}
				else
				{
					this.materials = new Material[1];
				}
				if (_ta == null)
				{
					yield break;
				}
				Material material3 = this.BaseMaterial;
				if (!material3)
				{
					if (this.PrimaryShader == null)
					{
						Log.Out("Null PrimaryShader for " + this.Name);
					}
					Shader shader = DataLoader.LoadAsset<Shader>(this.PrimaryShader, false);
					if (shader == null)
					{
						string text = "Can't find shader: ";
						object runtimeKey = this.PrimaryShader.RuntimeKey;
						Log.Error(text + ((runtimeKey != null) ? runtimeKey.ToString() : null));
					}
					material3 = new Material(shader);
				}
				this.materials[0] = material3;
				if (_idx == 3)
				{
					material3.SetTexture("_Albedo", _ta.diffuseTexture);
					material3.SetTexture("_Normal", _ta.normalTexture);
					material3.SetTexture("_Gloss_AO_SSS", _ta.specularTexture);
				}
				else
				{
					if (_idx != 5 || _ta.diffuseTexture is Texture2D)
					{
						material3.SetTexture("_MainTex", _ta.diffuseTexture);
					}
					if (_idx != 5 || _ta.normalTexture is Texture2D)
					{
						material3.SetTexture("_BumpMap", _ta.normalTexture);
					}
					material3.SetTexture("_MetallicGlossMap", _ta.specularTexture);
					material3.SetTexture("_OcclusionMap", _ta.occlusionTexture);
					material3.SetTexture("_MaskTex", _ta.maskTexture);
					material3.SetTexture("_MaskBumpMapTex", _ta.maskNormalTexture);
					material3.SetTexture("_EmissionMap", _ta.emissionTexture);
				}
				if (this.BlendMode != MeshDescription.EnumRenderMode.Default)
				{
					MeshDescription.SetupMaterialWithBlendMode(material3, this.BlendMode);
				}
				AssetReference distantShader = this.DistantShader;
				if (distantShader != null && distantShader.RuntimeKeyIsValid())
				{
					this.materialDistant = new Material(DataLoader.LoadAsset<Shader>(this.DistantShader, false));
					if (_idx != 5 || _ta.diffuseTexture is Texture2D)
					{
						this.materialDistant.SetTexture("_MainTex", _ta.diffuseTexture);
					}
					if (_idx != 5 || _ta.normalTexture is Texture2D)
					{
						this.materialDistant.SetTexture("_BumpMap", _ta.normalTexture);
					}
					this.materialDistant.SetTexture("_MetallicGlossMap", _ta.specularTexture);
					this.materialDistant.SetTexture("_OcclusionMap", _ta.occlusionTexture);
					this.materialDistant.SetTexture("_MaskTex", _ta.maskTexture);
					this.materialDistant.SetTexture("_MaskBumpMapTex", _ta.maskNormalTexture);
					this.materialDistant.SetTexture("_EmissionMap", _ta.emissionTexture);
					if (this.BlendMode != MeshDescription.EnumRenderMode.Default)
					{
						MeshDescription.SetupMaterialWithBlendMode(this.materialDistant, this.BlendMode);
					}
				}
				AssetReference secondaryShader2 = this.SecondaryShader;
				if (secondaryShader2 != null && secondaryShader2.RuntimeKeyIsValid())
				{
					Shader shader2 = DataLoader.LoadAsset<Shader>(this.SecondaryShader, false);
					if (shader2 == null)
					{
						string text2 = "Can't find secondary shader: ";
						object runtimeKey2 = this.SecondaryShader.RuntimeKey;
						Log.Error(text2 + ((runtimeKey2 != null) ? runtimeKey2.ToString() : null));
					}
					this.materials[1] = new Material(shader2);
					this.materials[1].CopyPropertiesFromMaterial(this.materials[0]);
				}
			}
		}
		yield break;
	}

	// Token: 0x06005884 RID: 22660 RVA: 0x0021C939 File Offset: 0x0021AB39
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator setupPrefabTerrainMaterials(int _idx, TextureAtlas _ta)
	{
		yield return null;
		AssetReference secondaryShader = this.SecondaryShader;
		if (secondaryShader != null && secondaryShader.RuntimeKeyIsValid())
		{
			this.prefabTerrainMaterials = new Material[2];
		}
		else
		{
			this.prefabTerrainMaterials = new Material[1];
		}
		if (_ta == null)
		{
			yield break;
		}
		Material material = this.BaseMaterial;
		if (!material)
		{
			Shader shader = DataLoader.LoadAsset<Shader>(this.PrimaryShader, false);
			if (shader == null)
			{
				string text = "Can't find shader: ";
				object runtimeKey = this.PrimaryShader.RuntimeKey;
				Log.Error(text + ((runtimeKey != null) ? runtimeKey.ToString() : null));
			}
			material = new Material(shader);
		}
		this.prefabTerrainMaterials[0] = material;
		if (_idx != 5 || _ta.diffuseTexture is Texture2D)
		{
			material.SetTexture("_MainTex", _ta.diffuseTexture);
		}
		if (_idx != 5 || _ta.normalTexture is Texture2D)
		{
			material.SetTexture("_BumpMap", _ta.normalTexture);
		}
		material.SetTexture("_MetallicGlossMap", _ta.specularTexture);
		material.SetTexture("_OcclusionMap", _ta.occlusionTexture);
		material.SetTexture("_MaskTex", _ta.maskTexture);
		material.SetTexture("_MaskBumpMapTex", _ta.maskNormalTexture);
		material.SetTexture("_EmissionMap", _ta.emissionTexture);
		if (this.BlendMode != MeshDescription.EnumRenderMode.Default)
		{
			MeshDescription.SetupMaterialWithBlendMode(material, this.BlendMode);
		}
		AssetReference distantShader = this.DistantShader;
		if (distantShader != null && distantShader.RuntimeKeyIsValid())
		{
			this.materialDistant = new Material(DataLoader.LoadAsset<Shader>(this.DistantShader, false));
			if (_idx != 5 || _ta.diffuseTexture is Texture2D)
			{
				this.materialDistant.SetTexture("_MainTex", _ta.diffuseTexture);
			}
			if (_idx != 5 || _ta.normalTexture is Texture2D)
			{
				this.materialDistant.SetTexture("_BumpMap", _ta.normalTexture);
			}
			this.materialDistant.SetTexture("_MetallicGlossMap", _ta.specularTexture);
			this.materialDistant.SetTexture("_OcclusionMap", _ta.occlusionTexture);
			this.materialDistant.SetTexture("_MaskTex", _ta.maskTexture);
			this.materialDistant.SetTexture("_MaskBumpMapTex", _ta.maskNormalTexture);
			this.materialDistant.SetTexture("_EmissionMap", _ta.emissionTexture);
			if (this.BlendMode != MeshDescription.EnumRenderMode.Default)
			{
				MeshDescription.SetupMaterialWithBlendMode(this.materialDistant, this.BlendMode);
			}
		}
		AssetReference secondaryShader2 = this.SecondaryShader;
		if (secondaryShader2 != null && secondaryShader2.RuntimeKeyIsValid())
		{
			Shader shader2 = DataLoader.LoadAsset<Shader>(this.SecondaryShader, false);
			if (shader2 == null)
			{
				string text2 = "Can't find secondary shader: ";
				object runtimeKey2 = this.SecondaryShader.RuntimeKey;
				Log.Error(text2 + ((runtimeKey2 != null) ? runtimeKey2.ToString() : null));
			}
			this.prefabTerrainMaterials[1] = new Material(shader2);
			this.prefabTerrainMaterials[1].CopyPropertiesFromMaterial(this.materials[0]);
		}
		yield break;
	}

	// Token: 0x06005885 RID: 22661 RVA: 0x0021C958 File Offset: 0x0021AB58
	public void ReloadTextureArrays(bool _isSplatmap)
	{
		if (_isSplatmap)
		{
			if (this.material != null)
			{
				this.material.SetTexture("_Diffuse", this.TexDiffuse);
				this.material.SetTexture("_NormalSAO", this.TexNormal);
				this.material.SetTexture("_SmoothAO", this.TexSpecular);
				string text = "Set Microsplat diffuse: ";
				Texture texDiffuse = this.TexDiffuse;
				Log.Out(text + ((texDiffuse != null) ? texDiffuse.ToString() : null));
				string text2 = "Set Microsplat normals: ";
				Texture texNormal = this.TexNormal;
				Log.Out(text2 + ((texNormal != null) ? texNormal.ToString() : null));
				string text3 = "Set Microsplat smooth:  ";
				Texture texSpecular = this.TexSpecular;
				Log.Out(text3 + ((texSpecular != null) ? texSpecular.ToString() : null));
			}
			if (this.materialDistant != null)
			{
				this.materialDistant.SetTexture("_Diffuse", this.TexDiffuse);
				this.materialDistant.SetTexture("_NormalSAO", this.TexNormal);
				this.materialDistant.SetTexture("_SmoothAO", this.TexSpecular);
				return;
			}
		}
		else if (this.bTextureArray && this.materials != null && this.materials.Length != 0 && this.materials[0] != null)
		{
			this.materials[0].mainTexture = this.textureAtlas.diffuseTexture;
			this.materials[0].SetTexture("_BumpMap", this.textureAtlas.normalTexture);
			this.materials[0].SetTexture("_MetallicGlossMap", this.textureAtlas.specularTexture);
			this.materials[0].SetTexture("_OcclusionMap", this.textureAtlas.occlusionTexture);
			this.materials[0].SetTexture("_MaskTex", this.textureAtlas.maskTexture);
			this.materials[0].SetTexture("_MaskBumpMapTex", this.textureAtlas.maskNormalTexture);
			this.materials[0].SetTexture("_EmissionMap", this.textureAtlas.emissionTexture);
			if (this.materialDistant != null)
			{
				this.materialDistant.mainTexture = this.textureAtlas.diffuseTexture;
				this.materialDistant.SetTexture("_BumpMap", this.textureAtlas.normalTexture);
				this.materialDistant.SetTexture("_MetallicGlossMap", this.textureAtlas.specularTexture);
				this.materialDistant.SetTexture("_OcclusionMap", this.textureAtlas.occlusionTexture);
				this.materialDistant.SetTexture("_MaskTex", this.textureAtlas.maskTexture);
				this.materialDistant.SetTexture("_MaskBumpMapTex", this.textureAtlas.maskNormalTexture);
				this.materialDistant.SetTexture("_EmissionMap", this.textureAtlas.emissionTexture);
			}
			if (this.materials.Length > 1 && this.materials[1] != null)
			{
				this.materials[1].CopyPropertiesFromMaterial(this.materials[0]);
			}
		}
	}

	// Token: 0x1700094E RID: 2382
	// (get) Token: 0x06005886 RID: 22662 RVA: 0x0021CC69 File Offset: 0x0021AE69
	// (set) Token: 0x06005887 RID: 22663 RVA: 0x0021CC7D File Offset: 0x0021AE7D
	public Material material
	{
		get
		{
			if (this.materials != null)
			{
				return this.materials[0];
			}
			return null;
		}
		set
		{
			if (this.materials != null)
			{
				this.materials[0] = value;
			}
		}
	}

	// Token: 0x1700094F RID: 2383
	// (get) Token: 0x06005888 RID: 22664 RVA: 0x0021CC90 File Offset: 0x0021AE90
	// (set) Token: 0x06005889 RID: 22665 RVA: 0x0021CCA4 File Offset: 0x0021AEA4
	public Material prefabPreviewMaterial
	{
		get
		{
			if (this.prefabTerrainMaterials != null)
			{
				return this.prefabTerrainMaterials[0];
			}
			return null;
		}
		set
		{
			if (this.prefabTerrainMaterials != null)
			{
				this.prefabTerrainMaterials[0] = value;
			}
		}
	}

	// Token: 0x0600588A RID: 22666 RVA: 0x0021CCB7 File Offset: 0x0021AEB7
	public bool IsSplatmap(int _index)
	{
		return _index == 5;
	}

	// Token: 0x0600588B RID: 22667 RVA: 0x0021CCBD File Offset: 0x0021AEBD
	public bool UseSplatmap(int _index)
	{
		return GameManager.IsSplatMapAvailable() && this.IsSplatmap(_index);
	}

	// Token: 0x0600588C RID: 22668 RVA: 0x0021CCD0 File Offset: 0x0021AED0
	public static void SetDebugStabilityShader(bool _bOn)
	{
		if (_bOn)
		{
			Shader shader = Shader.Find("Game/Debug/Stability");
			foreach (MeshDescription meshDescription in MeshDescription.meshes)
			{
				if (meshDescription.bUseDebugStabilityShader)
				{
					meshDescription.materials[0].shader = shader;
				}
			}
			using (LinkedList<Chunk>.Enumerator enumerator = GameManager.Instance.World.ChunkCache.GetChunkArray().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Chunk chunk = enumerator.Current;
					chunk.NeedsRegeneration = true;
				}
				goto IL_00BF;
			}
		}
		foreach (MeshDescription meshDescription2 in MeshDescription.meshes)
		{
			if (meshDescription2.bUseDebugStabilityShader)
			{
				meshDescription2.materials[0].shader = DataLoader.LoadAsset<Shader>(meshDescription2.PrimaryShader, false);
			}
		}
		IL_00BF:
		MeshDescription.bDebugStability = _bOn;
		Camera main = Camera.main;
		if (main)
		{
			LightViewer component = main.GetComponent<LightViewer>();
			if (component != null)
			{
				if (MeshDescription.bDebugStability)
				{
					component.TurnOffAllLights();
					return;
				}
				component.TurnOnAllLights();
			}
		}
	}

	// Token: 0x0600588D RID: 22669 RVA: 0x0021CDE8 File Offset: 0x0021AFE8
	public static void SetGrassQuality()
	{
		if (GameManager.IsDedicatedServer)
		{
			return;
		}
		if (MeshDescription.meshes != null && 3 < MeshDescription.meshes.Length)
		{
			MeshDescription.GrassQualityPlanes = 0;
			float num = 45f;
			switch (GamePrefs.GetInt(EnumGamePrefs.OptionsGfxGrassDistance))
			{
			case 1:
				num = 66f;
				break;
			case 2:
				MeshDescription.GrassQualityPlanes = 1;
				num = 102f;
				break;
			case 3:
				MeshDescription.GrassQualityPlanes = 1;
				num = 123f;
				break;
			}
			MeshDescription.meshes[3].material.SetFloat("_FadeDistance", num);
		}
	}

	// Token: 0x0600588E RID: 22670 RVA: 0x0021CE74 File Offset: 0x0021B074
	public static void SetWaterQuality()
	{
		if (GameManager.IsDedicatedServer)
		{
			return;
		}
		if (MeshDescription.meshes != null && 1 < MeshDescription.meshes.Length)
		{
			Material material = MeshDescription.meshes[1].materials[0];
			int @int = GamePrefs.GetInt(EnumGamePrefs.OptionsGfxWaterQuality);
			if (@int == 0)
			{
				material.shader = GlobalAssets.FindShader("Game/Water Distant Surface");
				material.SetFloat("_MinAlpha", 0f);
				return;
			}
			if (@int != 1)
			{
			}
			material.shader = GlobalAssets.FindShader("Game/Water Surface");
		}
	}

	// Token: 0x0600588F RID: 22671 RVA: 0x0021CEEC File Offset: 0x0021B0EC
	public static void Cleanup()
	{
		foreach (MeshDescription meshDescription in MeshDescription.meshes)
		{
			meshDescription.textureAtlas.Cleanup();
			meshDescription.CleanupMats();
		}
		MeshDescription.meshes = new MeshDescription[0];
	}

	// Token: 0x06005890 RID: 22672 RVA: 0x0021CF2C File Offset: 0x0021B12C
	[PublicizedFrom(EAccessModifier.Private)]
	public void CleanupMats()
	{
		if (this.materials != null)
		{
			for (int i = 0; i < this.materials.Length; i++)
			{
				Material material = this.materials[i];
				if (material && material != this.BaseMaterial && material != this.SecondaryMaterial)
				{
					global::UnityEngine.Object.Destroy(material);
					this.materials[i] = null;
				}
			}
		}
		global::UnityEngine.Object.Destroy(this.materialDistant);
	}

	// Token: 0x06005891 RID: 22673 RVA: 0x0021CF9B File Offset: 0x0021B19B
	public MeshDescription()
	{
	}

	// Token: 0x06005892 RID: 22674 RVA: 0x0021CFB8 File Offset: 0x0021B1B8
	public MeshDescription(MeshDescription other)
	{
		this.Name = other.Name;
		this.Tag = other.Tag;
		this.meshType = other.meshType;
		this.bCastShadows = other.bCastShadows;
		this.bReceiveShadows = other.bReceiveShadows;
		this.bHasLODs = other.bHasLODs;
		this.bUseDebugStabilityShader = other.bUseDebugStabilityShader;
		this.bTerrain = other.bTerrain;
		this.bTextureArray = other.bTextureArray;
		this.bSpecularIsBlack = other.bSpecularIsBlack;
		this.CreateTextureAtlas = other.CreateTextureAtlas;
		this.CreateSpecularMap = other.CreateSpecularMap;
		this.CreateNormalMap = other.CreateNormalMap;
		this.CreateEmissionMap = other.CreateEmissionMap;
		this.CreateHeightMap = other.CreateHeightMap;
		this.CreateOcclusionMap = other.CreateOcclusionMap;
		this.MeshLayerName = other.MeshLayerName;
		this.ColliderLayerName = other.ColliderLayerName;
		this.PrimaryShader = new AssetReference(other.PrimaryShader.AssetGUID);
		this.SecondaryShader = new AssetReference(other.SecondaryShader.AssetGUID);
		this.DistantShader = new AssetReference(other.DistantShader.AssetGUID);
		this.BlendMode = other.BlendMode;
		this.BaseMaterial = other.BaseMaterial;
		this.SecondaryMaterial = other.SecondaryMaterial;
		this.TextureAtlasClass = other.TextureAtlasClass;
		this.TexDiffuse = other.TexDiffuse;
		this.TexNormal = other.TexNormal;
		this.TexSpecular = other.TexSpecular;
		this.TexEmission = other.TexEmission;
		this.TexHeight = other.TexHeight;
		this.TexOcclusion = other.TexOcclusion;
		this.TexMask = other.TexMask;
		this.TexMaskNormal = other.TexMaskNormal;
		this.MetaData = other.MetaData;
	}

	// Token: 0x06005893 RID: 22675 RVA: 0x0021D198 File Offset: 0x0021B398
	[PublicizedFrom(EAccessModifier.Private)]
	public static void SetupMaterialWithBlendMode(Material material, MeshDescription.EnumRenderMode blendMode)
	{
		material.SetFloat("_Mode", (float)blendMode);
		switch (blendMode)
		{
		case MeshDescription.EnumRenderMode.Opaque:
			material.SetInt("_SrcBlend", 1);
			material.SetInt("_DstBlend", 0);
			material.SetInt("_ZWrite", 1);
			material.DisableKeyword("_ALPHATEST_ON");
			material.DisableKeyword("_ALPHABLEND_ON");
			material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			material.renderQueue = -1;
			break;
		case MeshDescription.EnumRenderMode.Cutout:
			material.SetInt("_SrcBlend", 1);
			material.SetInt("_DstBlend", 0);
			material.SetInt("_ZWrite", 1);
			material.EnableKeyword("_ALPHATEST_ON");
			material.DisableKeyword("_ALPHABLEND_ON");
			material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			material.renderQueue = 2450;
			break;
		case MeshDescription.EnumRenderMode.Fade:
			material.SetInt("_SrcBlend", 5);
			material.SetInt("_DstBlend", 10);
			material.SetInt("_ZWrite", 0);
			material.DisableKeyword("_ALPHATEST_ON");
			material.EnableKeyword("_ALPHABLEND_ON");
			material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			material.renderQueue = 3000;
			break;
		case MeshDescription.EnumRenderMode.Transparent:
			material.SetOverrideTag("RenderType", "Transparent");
			material.SetInt("_SrcBlend", 1);
			material.SetInt("_DstBlend", 10);
			material.SetInt("_ZWrite", 0);
			material.DisableKeyword("_ALPHATEST_ON");
			material.DisableKeyword("_ALPHABLEND_ON");
			material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
			material.renderQueue = 3000;
			break;
		}
		MeshDescription.SetMaterialKeywords(material);
	}

	// Token: 0x06005894 RID: 22676 RVA: 0x0021D330 File Offset: 0x0021B530
	[PublicizedFrom(EAccessModifier.Private)]
	public static void SetMaterialKeywords(Material material)
	{
		if (material.HasTexture("_BumpMap") || material.HasTexture("_DetailNormalMap"))
		{
			MeshDescription.SetKeyword(material, "_NORMALMAP", true);
		}
		else
		{
			MeshDescription.SetKeyword(material, "_NORMALMAP", false);
		}
		if (material.HasTexture("_MetallicGlossMap"))
		{
			MeshDescription.SetKeyword(material, "_METALLICGLOSSMAP", true);
		}
		else
		{
			MeshDescription.SetKeyword(material, "_METALLICGLOSSMAP", false);
		}
		int num = Shader.PropertyToID("_DetailAlbedoMap");
		int num2 = Shader.PropertyToID("_DetailNormalMap");
		MeshDescription.SetKeyword(material, "_DETAIL_MULX2", (material.HasProperty(num) && material.GetTexture(num)) || (material.HasProperty(num2) && material.GetTexture(num2)));
	}

	// Token: 0x06005895 RID: 22677 RVA: 0x0021D3EB File Offset: 0x0021B5EB
	[PublicizedFrom(EAccessModifier.Private)]
	public static void SetKeyword(Material m, string keyword, bool state)
	{
		if (state)
		{
			m.EnableKeyword(keyword);
			return;
		}
		m.DisableKeyword(keyword);
	}

	// Token: 0x06005896 RID: 22678 RVA: 0x0021D400 File Offset: 0x0021B600
	public static Material GetOpaqueMaterial()
	{
		if (MeshDescription.meshes.Length == 0)
		{
			return new Material(Shader.Find("Diffuse"));
		}
		MeshDescription meshDescription = MeshDescription.meshes[0];
		Material material;
		if (!meshDescription.bTextureArray)
		{
			material = global::UnityEngine.Object.Instantiate<Material>(Resources.Load<Material>("Materials/DistantPOI"));
		}
		else
		{
			material = global::UnityEngine.Object.Instantiate<Material>(Resources.Load<Material>("Materials/DistantPOI_TA"));
		}
		material.SetTexture("_MainTex", meshDescription.TexDiffuse);
		material.SetTexture("_Normal", meshDescription.TexNormal);
		material.SetTexture("_MetallicGlossMap", meshDescription.TexSpecular);
		material.SetTexture("_OcclusionMap", meshDescription.TexOcclusion);
		return material;
	}

	// Token: 0x06005897 RID: 22679 RVA: 0x0021D49C File Offset: 0x0021B69C
	public IEnumerator LoadTextureArraysForQuality(MeshDescriptionCollection _meshDescriptionCollection, int _index, int _quality, bool _isReload = false)
	{
		bool isSplatmap = this.IsSplatmap(_index);
		if (isSplatmap || this.bTextureArray)
		{
			yield return this.loadSingleArray(_quality, isSplatmap, MeshDescription.ETextureType.Diffuse);
			yield return null;
			yield return this.loadSingleArray(_quality, isSplatmap, MeshDescription.ETextureType.Normal);
			yield return null;
			yield return this.loadSingleArray(_quality, isSplatmap, MeshDescription.ETextureType.Specular);
			yield return null;
			if (_isReload)
			{
				this.ReloadTextureArrays(isSplatmap);
				if (MeshDescription.meshes.Length != 0)
				{
					this.textureAtlas.LoadTextureAtlas(_index, _meshDescriptionCollection, !GameManager.IsDedicatedServer);
					this.ReloadTextureArrays(isSplatmap);
				}
			}
		}
		yield break;
	}

	// Token: 0x06005898 RID: 22680 RVA: 0x0021D4C8 File Offset: 0x0021B6C8
	public void UnloadTextureArrays(int _index)
	{
		if (this.IsSplatmap(_index) || this.bTextureArray)
		{
			this.Unload(ref this.TexDiffuse);
			this.Unload(ref this.TexNormal);
			this.Unload(ref this.TexSpecular);
		}
	}

	// Token: 0x06005899 RID: 22681 RVA: 0x0021D4FF File Offset: 0x0021B6FF
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator loadSingleArray(int _quality, bool _isSplatmap, MeshDescription.ETextureType _texType)
	{
		string folderAddress = (_isSplatmap ? "TerrainTextures" : "BlockTextureAtlases");
		string path = (_isSplatmap ? ("Microsplat/MicroSplatConfig_" + this.GetFileSuffixForTextureType(_texType, _isSplatmap) + "_tarray") : ("TextureArrays/" + Constants.cPrefixAtlas + this.Name + this.GetFileSuffixForTextureType(_texType, _isSplatmap)));
		while (_quality >= 0)
		{
			string text = path + this.GetFileSuffixForQuality(_quality, _isSplatmap) + ".asset";
			Texture2DArray texture2DArray;
			if (ThreadManager.IsInSyncCoroutine)
			{
				texture2DArray = LoadManager.LoadAssetFromAddressables<Texture2DArray>(folderAddress, text, null, null, false, true, false).Asset;
			}
			else
			{
				LoadManager.AddressableRequestTask<Texture2DArray> request = LoadManager.LoadAssetFromAddressables<Texture2DArray>(folderAddress, text, null, null, false, false, false);
				while (!request.IsDone)
				{
					yield return null;
				}
				texture2DArray = request.Asset;
				request = null;
			}
			if (texture2DArray != null)
			{
				if (!Application.isEditor && texture2DArray.isReadable)
				{
					texture2DArray.Apply(false, true);
				}
				switch (_texType)
				{
				case MeshDescription.ETextureType.Diffuse:
					this.TexDiffuse = texture2DArray;
					break;
				case MeshDescription.ETextureType.Normal:
					this.TexNormal = texture2DArray;
					break;
				case MeshDescription.ETextureType.Specular:
					this.TexSpecular = texture2DArray;
					break;
				}
				yield break;
			}
			int num = _quality;
			_quality = num - 1;
		}
		throw new Exception("No Texture2DArray found for " + this.Name + " " + _texType.ToStringCached<MeshDescription.ETextureType>());
		yield break;
	}

	// Token: 0x0600589A RID: 22682 RVA: 0x0021D524 File Offset: 0x0021B724
	[PublicizedFrom(EAccessModifier.Private)]
	public string GetFileSuffixForTextureType(MeshDescription.ETextureType _type, bool _isSplatmap)
	{
		switch (_type)
		{
		case MeshDescription.ETextureType.Diffuse:
			if (!_isSplatmap)
			{
				return "";
			}
			return "diff";
		case MeshDescription.ETextureType.Normal:
			if (!_isSplatmap)
			{
				return "_n";
			}
			return "normal";
		case MeshDescription.ETextureType.Specular:
			if (!_isSplatmap)
			{
				return "_s";
			}
			return "smoothAO";
		default:
			throw new ArgumentOutOfRangeException("_type", _type, null);
		}
	}

	// Token: 0x0600589B RID: 22683 RVA: 0x0021D583 File Offset: 0x0021B783
	[PublicizedFrom(EAccessModifier.Private)]
	public string GetFileSuffixForQuality(int _quality, bool _isSplatmap)
	{
		if (!_isSplatmap)
		{
			return "_" + _quality.ToString();
		}
		if (_quality != 0)
		{
			return "_" + _quality.ToString();
		}
		return "";
	}

	// Token: 0x0600589C RID: 22684 RVA: 0x0021D5B4 File Offset: 0x0021B7B4
	[PublicizedFrom(EAccessModifier.Private)]
	public void Unload(ref Texture tex)
	{
		if (tex)
		{
			Log.Out("Unload {0}", new object[] { tex });
			Resources.UnloadAsset(tex);
			LoadManager.ReleaseAddressable<Texture>(tex);
			tex = null;
		}
	}

	// Token: 0x0600589D RID: 22685 RVA: 0x0021D5E5 File Offset: 0x0021B7E5
	public void SetTextureFilter(int _index, int anisoLevel)
	{
		if (this.IsSplatmap(_index) || this.bTextureArray)
		{
			this.SetAF(this.TexDiffuse, anisoLevel);
			this.SetAF(this.TexNormal, anisoLevel);
			this.SetAF(this.TexSpecular, anisoLevel);
		}
	}

	// Token: 0x0600589E RID: 22686 RVA: 0x0021D61F File Offset: 0x0021B81F
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetAF(Texture tex, int anisoLevel)
	{
		if (tex)
		{
			tex.anisoLevel = anisoLevel;
		}
	}

	// Token: 0x0400447C RID: 17532
	public const int cIndexOpaque = 0;

	// Token: 0x0400447D RID: 17533
	public const int cIndexWater = 1;

	// Token: 0x0400447E RID: 17534
	public const int cIndexTransparent = 2;

	// Token: 0x0400447F RID: 17535
	public const int cIndexGrass = 3;

	// Token: 0x04004480 RID: 17536
	public const int cIndexDecals = 4;

	// Token: 0x04004481 RID: 17537
	public const int cIndexTerrain = 5;

	// Token: 0x04004482 RID: 17538
	public const int cIndexCount = 6;

	// Token: 0x04004483 RID: 17539
	public const int MESH_OPAQUE = 0;

	// Token: 0x04004484 RID: 17540
	public const int MESH_WATER = 1;

	// Token: 0x04004485 RID: 17541
	public const int MESH_TRANSPARENT = 2;

	// Token: 0x04004486 RID: 17542
	public const int MESH_GRASS = 3;

	// Token: 0x04004487 RID: 17543
	public const int MESH_DECALS = 4;

	// Token: 0x04004488 RID: 17544
	public const int MESH_TERRAIN = 5;

	// Token: 0x04004489 RID: 17545
	public const int MESH_MODELS = 0;

	// Token: 0x0400448A RID: 17546
	public static MeshDescription[] meshes = new MeshDescription[0];

	// Token: 0x0400448B RID: 17547
	public static int GrassQualityPlanes;

	// Token: 0x0400448C RID: 17548
	public string Name;

	// Token: 0x0400448D RID: 17549
	public string Tag;

	// Token: 0x0400448E RID: 17550
	public VoxelMesh.EnumMeshType meshType;

	// Token: 0x0400448F RID: 17551
	public bool bCastShadows;

	// Token: 0x04004490 RID: 17552
	public bool bReceiveShadows;

	// Token: 0x04004491 RID: 17553
	public bool bHasLODs;

	// Token: 0x04004492 RID: 17554
	public bool bUseDebugStabilityShader;

	// Token: 0x04004493 RID: 17555
	public bool bTerrain;

	// Token: 0x04004494 RID: 17556
	public bool bTextureArray;

	// Token: 0x04004495 RID: 17557
	public bool bSpecularIsBlack;

	// Token: 0x04004496 RID: 17558
	public bool CreateTextureAtlas = true;

	// Token: 0x04004497 RID: 17559
	public bool CreateSpecularMap = true;

	// Token: 0x04004498 RID: 17560
	public bool CreateNormalMap = true;

	// Token: 0x04004499 RID: 17561
	public bool CreateEmissionMap;

	// Token: 0x0400449A RID: 17562
	public bool CreateHeightMap;

	// Token: 0x0400449B RID: 17563
	public bool CreateOcclusionMap;

	// Token: 0x0400449C RID: 17564
	public string MeshLayerName;

	// Token: 0x0400449D RID: 17565
	public string ColliderLayerName;

	// Token: 0x0400449E RID: 17566
	public AssetReference PrimaryShader;

	// Token: 0x0400449F RID: 17567
	public AssetReference SecondaryShader;

	// Token: 0x040044A0 RID: 17568
	public AssetReference DistantShader;

	// Token: 0x040044A1 RID: 17569
	public MeshDescription.EnumRenderMode BlendMode;

	// Token: 0x040044A2 RID: 17570
	public Material BaseMaterial;

	// Token: 0x040044A3 RID: 17571
	public Material SecondaryMaterial;

	// Token: 0x040044A4 RID: 17572
	public string TextureAtlasClass;

	// Token: 0x040044A5 RID: 17573
	public Texture TexDiffuse;

	// Token: 0x040044A6 RID: 17574
	public Texture TexNormal;

	// Token: 0x040044A7 RID: 17575
	public Texture TexSpecular;

	// Token: 0x040044A8 RID: 17576
	public Texture TexEmission;

	// Token: 0x040044A9 RID: 17577
	public Texture TexHeight;

	// Token: 0x040044AA RID: 17578
	public Texture TexOcclusion;

	// Token: 0x040044AB RID: 17579
	public Texture2D TexMask;

	// Token: 0x040044AC RID: 17580
	public Texture2D TexMaskNormal;

	// Token: 0x040044AD RID: 17581
	public TextAsset MetaData;

	// Token: 0x040044AE RID: 17582
	[HideInInspector]
	public TextureAtlas textureAtlas;

	// Token: 0x040044AF RID: 17583
	[NonSerialized]
	public Material[] materials;

	// Token: 0x040044B0 RID: 17584
	[NonSerialized]
	public Material materialDistant;

	// Token: 0x040044B1 RID: 17585
	[NonSerialized]
	public Material[] prefabTerrainMaterials;

	// Token: 0x040044B2 RID: 17586
	[NonSerialized]
	public Material prefabTerrainMaterialDistant;

	// Token: 0x040044B3 RID: 17587
	public static bool bDebugStability;

	// Token: 0x02000B6D RID: 2925
	public enum EnumRenderMode
	{
		// Token: 0x040044B5 RID: 17589
		Default = -1,
		// Token: 0x040044B6 RID: 17590
		Opaque,
		// Token: 0x040044B7 RID: 17591
		Cutout,
		// Token: 0x040044B8 RID: 17592
		Fade,
		// Token: 0x040044B9 RID: 17593
		Transparent
	}

	// Token: 0x02000B6E RID: 2926
	[PublicizedFrom(EAccessModifier.Private)]
	public enum ETextureType
	{
		// Token: 0x040044BB RID: 17595
		Diffuse,
		// Token: 0x040044BC RID: 17596
		Normal,
		// Token: 0x040044BD RID: 17597
		Specular
	}
}

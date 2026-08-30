using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

// Token: 0x02000B5A RID: 2906
[Serializable]
public class MeshDescription
{
	// Token: 0x060057FB RID: 22523 RVA: 0x00219C00 File Offset: 0x00217E00
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

	// Token: 0x060057FC RID: 22524 RVA: 0x00219C1D File Offset: 0x00217E1D
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

	// Token: 0x060057FD RID: 22525 RVA: 0x00219C3C File Offset: 0x00217E3C
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

	// Token: 0x1700093C RID: 2364
	// (get) Token: 0x060057FE RID: 22526 RVA: 0x00219F4D File Offset: 0x0021814D
	// (set) Token: 0x060057FF RID: 22527 RVA: 0x00219F61 File Offset: 0x00218161
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

	// Token: 0x1700093D RID: 2365
	// (get) Token: 0x06005800 RID: 22528 RVA: 0x00219F74 File Offset: 0x00218174
	// (set) Token: 0x06005801 RID: 22529 RVA: 0x00219F88 File Offset: 0x00218188
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

	// Token: 0x06005802 RID: 22530 RVA: 0x00219F9B File Offset: 0x0021819B
	public bool IsSplatmap(int _index)
	{
		return _index == 5;
	}

	// Token: 0x06005803 RID: 22531 RVA: 0x00219FA1 File Offset: 0x002181A1
	public bool UseSplatmap(int _index)
	{
		return GameManager.IsSplatMapAvailable() && this.IsSplatmap(_index);
	}

	// Token: 0x06005804 RID: 22532 RVA: 0x00219FB4 File Offset: 0x002181B4
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

	// Token: 0x06005805 RID: 22533 RVA: 0x0021A0CC File Offset: 0x002182CC
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

	// Token: 0x06005806 RID: 22534 RVA: 0x0021A158 File Offset: 0x00218358
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

	// Token: 0x06005807 RID: 22535 RVA: 0x0021A1D0 File Offset: 0x002183D0
	public static void Cleanup()
	{
		foreach (MeshDescription meshDescription in MeshDescription.meshes)
		{
			meshDescription.textureAtlas.Cleanup();
			meshDescription.CleanupMats();
		}
		MeshDescription.meshes = new MeshDescription[0];
	}

	// Token: 0x06005808 RID: 22536 RVA: 0x0021A210 File Offset: 0x00218410
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

	// Token: 0x06005809 RID: 22537 RVA: 0x0021A27F File Offset: 0x0021847F
	public MeshDescription()
	{
	}

	// Token: 0x0600580A RID: 22538 RVA: 0x0021A29C File Offset: 0x0021849C
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

	// Token: 0x0600580B RID: 22539 RVA: 0x0021A47C File Offset: 0x0021867C
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

	// Token: 0x0600580C RID: 22540 RVA: 0x0021A614 File Offset: 0x00218814
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

	// Token: 0x0600580D RID: 22541 RVA: 0x0021A6CF File Offset: 0x002188CF
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

	// Token: 0x0600580E RID: 22542 RVA: 0x0021A6E4 File Offset: 0x002188E4
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

	// Token: 0x0600580F RID: 22543 RVA: 0x0021A780 File Offset: 0x00218980
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

	// Token: 0x06005810 RID: 22544 RVA: 0x0021A7AC File Offset: 0x002189AC
	public void UnloadTextureArrays(int _index)
	{
		if (this.IsSplatmap(_index) || this.bTextureArray)
		{
			this.Unload(ref this.TexDiffuse);
			this.Unload(ref this.TexNormal);
			this.Unload(ref this.TexSpecular);
		}
	}

	// Token: 0x06005811 RID: 22545 RVA: 0x0021A7E3 File Offset: 0x002189E3
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

	// Token: 0x06005812 RID: 22546 RVA: 0x0021A808 File Offset: 0x00218A08
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

	// Token: 0x06005813 RID: 22547 RVA: 0x0021A867 File Offset: 0x00218A67
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

	// Token: 0x06005814 RID: 22548 RVA: 0x0021A898 File Offset: 0x00218A98
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

	// Token: 0x06005815 RID: 22549 RVA: 0x0021A8C9 File Offset: 0x00218AC9
	public void SetTextureFilter(int _index, int anisoLevel)
	{
		if (this.IsSplatmap(_index) || this.bTextureArray)
		{
			this.SetAF(this.TexDiffuse, anisoLevel);
			this.SetAF(this.TexNormal, anisoLevel);
			this.SetAF(this.TexSpecular, anisoLevel);
		}
	}

	// Token: 0x06005816 RID: 22550 RVA: 0x0021A903 File Offset: 0x00218B03
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetAF(Texture tex, int anisoLevel)
	{
		if (tex)
		{
			tex.anisoLevel = anisoLevel;
		}
	}

	// Token: 0x040043E1 RID: 17377
	public const int cIndexOpaque = 0;

	// Token: 0x040043E2 RID: 17378
	public const int cIndexWater = 1;

	// Token: 0x040043E3 RID: 17379
	public const int cIndexTransparent = 2;

	// Token: 0x040043E4 RID: 17380
	public const int cIndexGrass = 3;

	// Token: 0x040043E5 RID: 17381
	public const int cIndexDecals = 4;

	// Token: 0x040043E6 RID: 17382
	public const int cIndexTerrain = 5;

	// Token: 0x040043E7 RID: 17383
	public const int cIndexCount = 6;

	// Token: 0x040043E8 RID: 17384
	public const int MESH_OPAQUE = 0;

	// Token: 0x040043E9 RID: 17385
	public const int MESH_WATER = 1;

	// Token: 0x040043EA RID: 17386
	public const int MESH_TRANSPARENT = 2;

	// Token: 0x040043EB RID: 17387
	public const int MESH_GRASS = 3;

	// Token: 0x040043EC RID: 17388
	public const int MESH_DECALS = 4;

	// Token: 0x040043ED RID: 17389
	public const int MESH_TERRAIN = 5;

	// Token: 0x040043EE RID: 17390
	public const int MESH_MODELS = 0;

	// Token: 0x040043EF RID: 17391
	public static MeshDescription[] meshes = new MeshDescription[0];

	// Token: 0x040043F0 RID: 17392
	public static int GrassQualityPlanes;

	// Token: 0x040043F1 RID: 17393
	public string Name;

	// Token: 0x040043F2 RID: 17394
	public string Tag;

	// Token: 0x040043F3 RID: 17395
	public VoxelMesh.EnumMeshType meshType;

	// Token: 0x040043F4 RID: 17396
	public bool bCastShadows;

	// Token: 0x040043F5 RID: 17397
	public bool bReceiveShadows;

	// Token: 0x040043F6 RID: 17398
	public bool bHasLODs;

	// Token: 0x040043F7 RID: 17399
	public bool bUseDebugStabilityShader;

	// Token: 0x040043F8 RID: 17400
	public bool bTerrain;

	// Token: 0x040043F9 RID: 17401
	public bool bTextureArray;

	// Token: 0x040043FA RID: 17402
	public bool bSpecularIsBlack;

	// Token: 0x040043FB RID: 17403
	public bool CreateTextureAtlas = true;

	// Token: 0x040043FC RID: 17404
	public bool CreateSpecularMap = true;

	// Token: 0x040043FD RID: 17405
	public bool CreateNormalMap = true;

	// Token: 0x040043FE RID: 17406
	public bool CreateEmissionMap;

	// Token: 0x040043FF RID: 17407
	public bool CreateHeightMap;

	// Token: 0x04004400 RID: 17408
	public bool CreateOcclusionMap;

	// Token: 0x04004401 RID: 17409
	public string MeshLayerName;

	// Token: 0x04004402 RID: 17410
	public string ColliderLayerName;

	// Token: 0x04004403 RID: 17411
	public AssetReference PrimaryShader;

	// Token: 0x04004404 RID: 17412
	public AssetReference SecondaryShader;

	// Token: 0x04004405 RID: 17413
	public AssetReference DistantShader;

	// Token: 0x04004406 RID: 17414
	public MeshDescription.EnumRenderMode BlendMode;

	// Token: 0x04004407 RID: 17415
	public Material BaseMaterial;

	// Token: 0x04004408 RID: 17416
	public Material SecondaryMaterial;

	// Token: 0x04004409 RID: 17417
	public string TextureAtlasClass;

	// Token: 0x0400440A RID: 17418
	public Texture TexDiffuse;

	// Token: 0x0400440B RID: 17419
	public Texture TexNormal;

	// Token: 0x0400440C RID: 17420
	public Texture TexSpecular;

	// Token: 0x0400440D RID: 17421
	public Texture TexEmission;

	// Token: 0x0400440E RID: 17422
	public Texture TexHeight;

	// Token: 0x0400440F RID: 17423
	public Texture TexOcclusion;

	// Token: 0x04004410 RID: 17424
	public Texture2D TexMask;

	// Token: 0x04004411 RID: 17425
	public Texture2D TexMaskNormal;

	// Token: 0x04004412 RID: 17426
	public TextAsset MetaData;

	// Token: 0x04004413 RID: 17427
	[HideInInspector]
	public TextureAtlas textureAtlas;

	// Token: 0x04004414 RID: 17428
	[NonSerialized]
	public Material[] materials;

	// Token: 0x04004415 RID: 17429
	[NonSerialized]
	public Material materialDistant;

	// Token: 0x04004416 RID: 17430
	[NonSerialized]
	public Material[] prefabTerrainMaterials;

	// Token: 0x04004417 RID: 17431
	[NonSerialized]
	public Material prefabTerrainMaterialDistant;

	// Token: 0x04004418 RID: 17432
	public static bool bDebugStability;

	// Token: 0x02000B5B RID: 2907
	public enum EnumRenderMode
	{
		// Token: 0x0400441A RID: 17434
		Default = -1,
		// Token: 0x0400441B RID: 17435
		Opaque,
		// Token: 0x0400441C RID: 17436
		Cutout,
		// Token: 0x0400441D RID: 17437
		Fade,
		// Token: 0x0400441E RID: 17438
		Transparent
	}

	// Token: 0x02000B5C RID: 2908
	[PublicizedFrom(EAccessModifier.Private)]
	public enum ETextureType
	{
		// Token: 0x04004420 RID: 17440
		Diffuse,
		// Token: 0x04004421 RID: 17441
		Normal,
		// Token: 0x04004422 RID: 17442
		Specular
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

// Token: 0x02000B89 RID: 2953
[Serializable]
public class MeshDescription
{
	// Token: 0x060058C2 RID: 22722 RVA: 0x00221104 File Offset: 0x0021F304
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

	// Token: 0x060058C3 RID: 22723 RVA: 0x00221121 File Offset: 0x0021F321
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

	// Token: 0x060058C4 RID: 22724 RVA: 0x00221140 File Offset: 0x0021F340
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

	// Token: 0x17000976 RID: 2422
	// (get) Token: 0x060058C5 RID: 22725 RVA: 0x00221451 File Offset: 0x0021F651
	// (set) Token: 0x060058C6 RID: 22726 RVA: 0x00221465 File Offset: 0x0021F665
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

	// Token: 0x17000977 RID: 2423
	// (get) Token: 0x060058C7 RID: 22727 RVA: 0x00221478 File Offset: 0x0021F678
	// (set) Token: 0x060058C8 RID: 22728 RVA: 0x0022148C File Offset: 0x0021F68C
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

	// Token: 0x060058C9 RID: 22729 RVA: 0x0022149F File Offset: 0x0021F69F
	public bool IsSplatmap(int _index)
	{
		return _index == 5;
	}

	// Token: 0x060058CA RID: 22730 RVA: 0x002214A5 File Offset: 0x0021F6A5
	public bool UseSplatmap(int _index)
	{
		return GameManager.IsSplatMapAvailable() && this.IsSplatmap(_index);
	}

	// Token: 0x060058CB RID: 22731 RVA: 0x002214B8 File Offset: 0x0021F6B8
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

	// Token: 0x060058CC RID: 22732 RVA: 0x002215D0 File Offset: 0x0021F7D0
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

	// Token: 0x060058CD RID: 22733 RVA: 0x0022165C File Offset: 0x0021F85C
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

	// Token: 0x060058CE RID: 22734 RVA: 0x002216D4 File Offset: 0x0021F8D4
	public static void Cleanup()
	{
		foreach (MeshDescription meshDescription in MeshDescription.meshes)
		{
			meshDescription.textureAtlas.Cleanup();
			meshDescription.CleanupMats();
		}
		MeshDescription.meshes = new MeshDescription[0];
	}

	// Token: 0x060058CF RID: 22735 RVA: 0x00221714 File Offset: 0x0021F914
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

	// Token: 0x060058D0 RID: 22736 RVA: 0x00221783 File Offset: 0x0021F983
	public MeshDescription()
	{
	}

	// Token: 0x060058D1 RID: 22737 RVA: 0x002217A0 File Offset: 0x0021F9A0
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

	// Token: 0x060058D2 RID: 22738 RVA: 0x00221980 File Offset: 0x0021FB80
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

	// Token: 0x060058D3 RID: 22739 RVA: 0x00221B18 File Offset: 0x0021FD18
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

	// Token: 0x060058D4 RID: 22740 RVA: 0x00221BD3 File Offset: 0x0021FDD3
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

	// Token: 0x060058D5 RID: 22741 RVA: 0x00221BE8 File Offset: 0x0021FDE8
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

	// Token: 0x060058D6 RID: 22742 RVA: 0x00221C84 File Offset: 0x0021FE84
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

	// Token: 0x060058D7 RID: 22743 RVA: 0x00221CB0 File Offset: 0x0021FEB0
	public void UnloadTextureArrays(int _index)
	{
		if (this.IsSplatmap(_index) || this.bTextureArray)
		{
			this.Unload(ref this.TexDiffuse);
			this.Unload(ref this.TexNormal);
			this.Unload(ref this.TexSpecular);
		}
	}

	// Token: 0x060058D8 RID: 22744 RVA: 0x00221CE7 File Offset: 0x0021FEE7
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

	// Token: 0x060058D9 RID: 22745 RVA: 0x00221D0C File Offset: 0x0021FF0C
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

	// Token: 0x060058DA RID: 22746 RVA: 0x00221D6B File Offset: 0x0021FF6B
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

	// Token: 0x060058DB RID: 22747 RVA: 0x00221D9C File Offset: 0x0021FF9C
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

	// Token: 0x060058DC RID: 22748 RVA: 0x00221DCD File Offset: 0x0021FFCD
	public void SetTextureFilter(int _index, int anisoLevel)
	{
		if (this.IsSplatmap(_index) || this.bTextureArray)
		{
			this.SetAF(this.TexDiffuse, anisoLevel);
			this.SetAF(this.TexNormal, anisoLevel);
			this.SetAF(this.TexSpecular, anisoLevel);
		}
	}

	// Token: 0x060058DD RID: 22749 RVA: 0x00221E07 File Offset: 0x00220007
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetAF(Texture tex, int anisoLevel)
	{
		if (tex)
		{
			tex.anisoLevel = anisoLevel;
		}
	}

	// Token: 0x040044DF RID: 17631
	public const int cIndexOpaque = 0;

	// Token: 0x040044E0 RID: 17632
	public const int cIndexWater = 1;

	// Token: 0x040044E1 RID: 17633
	public const int cIndexTransparent = 2;

	// Token: 0x040044E2 RID: 17634
	public const int cIndexGrass = 3;

	// Token: 0x040044E3 RID: 17635
	public const int cIndexDecals = 4;

	// Token: 0x040044E4 RID: 17636
	public const int cIndexTerrain = 5;

	// Token: 0x040044E5 RID: 17637
	public const int cIndexCount = 6;

	// Token: 0x040044E6 RID: 17638
	public const int MESH_OPAQUE = 0;

	// Token: 0x040044E7 RID: 17639
	public const int MESH_WATER = 1;

	// Token: 0x040044E8 RID: 17640
	public const int MESH_TRANSPARENT = 2;

	// Token: 0x040044E9 RID: 17641
	public const int MESH_GRASS = 3;

	// Token: 0x040044EA RID: 17642
	public const int MESH_DECALS = 4;

	// Token: 0x040044EB RID: 17643
	public const int MESH_TERRAIN = 5;

	// Token: 0x040044EC RID: 17644
	public const int MESH_MODELS = 0;

	// Token: 0x040044ED RID: 17645
	public static MeshDescription[] meshes = new MeshDescription[0];

	// Token: 0x040044EE RID: 17646
	public static int GrassQualityPlanes;

	// Token: 0x040044EF RID: 17647
	public string Name;

	// Token: 0x040044F0 RID: 17648
	public string Tag;

	// Token: 0x040044F1 RID: 17649
	public VoxelMesh.EnumMeshType meshType;

	// Token: 0x040044F2 RID: 17650
	public bool bCastShadows;

	// Token: 0x040044F3 RID: 17651
	public bool bReceiveShadows;

	// Token: 0x040044F4 RID: 17652
	public bool bHasLODs;

	// Token: 0x040044F5 RID: 17653
	public bool bUseDebugStabilityShader;

	// Token: 0x040044F6 RID: 17654
	public bool bTerrain;

	// Token: 0x040044F7 RID: 17655
	public bool bTextureArray;

	// Token: 0x040044F8 RID: 17656
	public bool bSpecularIsBlack;

	// Token: 0x040044F9 RID: 17657
	public bool CreateTextureAtlas = true;

	// Token: 0x040044FA RID: 17658
	public bool CreateSpecularMap = true;

	// Token: 0x040044FB RID: 17659
	public bool CreateNormalMap = true;

	// Token: 0x040044FC RID: 17660
	public bool CreateEmissionMap;

	// Token: 0x040044FD RID: 17661
	public bool CreateHeightMap;

	// Token: 0x040044FE RID: 17662
	public bool CreateOcclusionMap;

	// Token: 0x040044FF RID: 17663
	public string MeshLayerName;

	// Token: 0x04004500 RID: 17664
	public string ColliderLayerName;

	// Token: 0x04004501 RID: 17665
	public AssetReference PrimaryShader;

	// Token: 0x04004502 RID: 17666
	public AssetReference SecondaryShader;

	// Token: 0x04004503 RID: 17667
	public AssetReference DistantShader;

	// Token: 0x04004504 RID: 17668
	public MeshDescription.EnumRenderMode BlendMode;

	// Token: 0x04004505 RID: 17669
	public Material BaseMaterial;

	// Token: 0x04004506 RID: 17670
	public Material SecondaryMaterial;

	// Token: 0x04004507 RID: 17671
	public string TextureAtlasClass;

	// Token: 0x04004508 RID: 17672
	public Texture TexDiffuse;

	// Token: 0x04004509 RID: 17673
	public Texture TexNormal;

	// Token: 0x0400450A RID: 17674
	public Texture TexSpecular;

	// Token: 0x0400450B RID: 17675
	public Texture TexEmission;

	// Token: 0x0400450C RID: 17676
	public Texture TexHeight;

	// Token: 0x0400450D RID: 17677
	public Texture TexOcclusion;

	// Token: 0x0400450E RID: 17678
	public Texture2D TexMask;

	// Token: 0x0400450F RID: 17679
	public Texture2D TexMaskNormal;

	// Token: 0x04004510 RID: 17680
	public TextAsset MetaData;

	// Token: 0x04004511 RID: 17681
	[HideInInspector]
	public TextureAtlas textureAtlas;

	// Token: 0x04004512 RID: 17682
	[NonSerialized]
	public Material[] materials;

	// Token: 0x04004513 RID: 17683
	[NonSerialized]
	public Material materialDistant;

	// Token: 0x04004514 RID: 17684
	[NonSerialized]
	public Material[] prefabTerrainMaterials;

	// Token: 0x04004515 RID: 17685
	[NonSerialized]
	public Material prefabTerrainMaterialDistant;

	// Token: 0x04004516 RID: 17686
	public static bool bDebugStability;

	// Token: 0x02000B8A RID: 2954
	public enum EnumRenderMode
	{
		// Token: 0x04004518 RID: 17688
		Default = -1,
		// Token: 0x04004519 RID: 17689
		Opaque,
		// Token: 0x0400451A RID: 17690
		Cutout,
		// Token: 0x0400451B RID: 17691
		Fade,
		// Token: 0x0400451C RID: 17692
		Transparent
	}

	// Token: 0x02000B8B RID: 2955
	[PublicizedFrom(EAccessModifier.Private)]
	public enum ETextureType
	{
		// Token: 0x0400451E RID: 17694
		Diffuse,
		// Token: 0x0400451F RID: 17695
		Normal,
		// Token: 0x04004520 RID: 17696
		Specular
	}
}

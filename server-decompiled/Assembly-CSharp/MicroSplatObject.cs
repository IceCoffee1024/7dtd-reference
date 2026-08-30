using System;
using UnityEngine;

// Token: 0x02000029 RID: 41
[ExecuteAlways]
public class MicroSplatObject : MonoBehaviour
{
	// Token: 0x06000117 RID: 279 RVA: 0x0000C7F0 File Offset: 0x0000A9F0
	[PublicizedFrom(EAccessModifier.Protected)]
	public long GetOverrideHash()
	{
		long num = 3L * (long)(((this.propData == null) ? 3 : this.propData.GetHashCode()) * 3) * (((this.perPixelNormal == null) ? 7L : this.perPixelNormal.GetNativeTexturePtr().ToInt64()) * 7L) * (long)(((this.keywordSO == null) ? 11 : this.keywordSO.GetHashCode()) * 11) * (((this.procBiomeMask == null) ? 13L : this.procBiomeMask.GetNativeTexturePtr().ToInt64()) * 13L) * (((this.procBiomeMask2 == null) ? 81L : this.procBiomeMask2.GetNativeTexturePtr().ToInt64()) * 81L) * (((this.cavityMap == null) ? 17L : this.cavityMap.GetNativeTexturePtr().ToInt64()) * 17L) * (long)(((this.procTexCfg == null) ? 19 : this.procTexCfg.GetHashCode()) * 19) * (((this.streamTexture == null) ? 41L : this.streamTexture.GetNativeTexturePtr().ToInt64()) * 41L) * (((this.terrainDesc == null) ? 43L : this.terrainDesc.GetNativeTexturePtr().ToInt64()) * 43L) * (((this.geoTextureOverride == null) ? 47L : this.geoTextureOverride.GetNativeTexturePtr().ToInt64()) * 47L) * (((this.globalNormalOverride == null) ? 53L : this.globalNormalOverride.GetNativeTexturePtr().ToInt64()) * 53L) * (((this.globalSAOMOverride == null) ? 59L : this.globalSAOMOverride.GetNativeTexturePtr().ToInt64()) * 59L) * (((this.globalEmisOverride == null) ? 61L : this.globalEmisOverride.GetNativeTexturePtr().ToInt64()) * 61L) * (((this.tintMapOverride == null) ? 71L : this.tintMapOverride.GetNativeTexturePtr().ToInt64()) * 71L);
		if (num == 0L)
		{
			Debug.Log("Override hash returned 0, this should not happen");
		}
		return num;
	}

	// Token: 0x06000118 RID: 280 RVA: 0x0000CA47 File Offset: 0x0000AC47
	[PublicizedFrom(EAccessModifier.Protected)]
	public void SetMap(Material m, string name, Texture2D tex)
	{
		if (m.HasProperty(name) && tex != null)
		{
			m.SetTexture(name, tex);
		}
	}

	// Token: 0x06000119 RID: 281 RVA: 0x0000CA64 File Offset: 0x0000AC64
	[PublicizedFrom(EAccessModifier.Protected)]
	public void ApplyMaps(Material m)
	{
		this.SetMap(m, "_PerPixelNormal", this.perPixelNormal);
		this.SetMap(m, "_StreamControl", this.streamTexture);
		this.SetMap(m, "_GeoTex", this.geoTextureOverride);
		this.SetMap(m, "_GlobalTintTex", this.tintMapOverride);
		this.SetMap(m, "_GlobalNormalTex", this.globalNormalOverride);
		this.SetMap(m, "_GlobalSAOMTex", this.globalSAOMOverride);
		this.SetMap(m, "_GlobalEmisTex", this.globalEmisOverride);
		if (m.HasProperty("_GeoCurve") && this.propData != null)
		{
			m.SetTexture("_GeoCurve", this.propData.GetGeoCurve());
		}
		if (m.HasProperty("_GeoSlopeTex") && this.propData != null)
		{
			m.SetTexture("_GeoSlopeTex", this.propData.GetGeoSlopeFilter());
		}
		if (m.HasProperty("_GlobalSlopeTex") && this.propData != null)
		{
			m.SetTexture("_GlobalSlopeTex", this.propData.GetGlobalSlopeFilter());
		}
		if (this.propData != null)
		{
			m.SetTexture("_PerTexProps", this.propData.GetTexture());
		}
		if (this.procTexCfg != null)
		{
			if (m.HasProperty("_ProcTexCurves"))
			{
				m.SetTexture("_ProcTexCurves", this.procTexCfg.GetCurveTexture());
				m.SetTexture("_ProcTexParams", this.procTexCfg.GetParamTexture());
				m.SetInt("_PCLayerCount", this.procTexCfg.layers.Count);
				if (this.procBiomeMask != null && m.HasProperty("_ProcTexBiomeMask"))
				{
					m.SetTexture("_ProcTexBiomeMask", this.procBiomeMask);
				}
				if (this.procBiomeMask2 != null && m.HasProperty("_ProcTexBiomeMask2"))
				{
					m.SetTexture("_ProcTexBiomeMask2", this.procBiomeMask2);
				}
			}
			if (m.HasProperty("_PCHeightGradients"))
			{
				m.SetTexture("_PCHeightGradients", this.procTexCfg.GetHeightGradientTexture());
			}
			if (m.HasProperty("_PCHeightHSV"))
			{
				m.SetTexture("_PCHeightHSV", this.procTexCfg.GetHeightHSVTexture());
			}
			if (m.HasProperty("_CavityMap"))
			{
				m.SetTexture("_CavityMap", this.cavityMap);
			}
			if (m.HasProperty("_PCSlopeGradients"))
			{
				m.SetTexture("_PCSlopeGradients", this.procTexCfg.GetSlopeGradientTexture());
			}
			if (m.HasProperty("_PCSlopeHSV"))
			{
				m.SetTexture("_PCSlopeHSV", this.procTexCfg.GetSlopeHSVTexture());
			}
		}
	}

	// Token: 0x0600011A RID: 282 RVA: 0x0000CD10 File Offset: 0x0000AF10
	[PublicizedFrom(EAccessModifier.Protected)]
	public void ApplyControlTextures(Texture2D[] controls, Material m)
	{
		m.SetTexture("_Control0", (controls.Length != 0) ? controls[0] : Texture2D.blackTexture);
		m.SetTexture("_Control1", (controls.Length > 1) ? controls[1] : Texture2D.blackTexture);
		m.SetTexture("_Control2", (controls.Length > 2) ? controls[2] : Texture2D.blackTexture);
		m.SetTexture("_Control3", (controls.Length > 3) ? controls[3] : Texture2D.blackTexture);
		m.SetTexture("_Control4", (controls.Length > 4) ? controls[4] : Texture2D.blackTexture);
		m.SetTexture("_Control5", (controls.Length > 5) ? controls[5] : Texture2D.blackTexture);
		m.SetTexture("_Control6", (controls.Length > 6) ? controls[6] : Texture2D.blackTexture);
		m.SetTexture("_Control7", (controls.Length > 7) ? controls[7] : Texture2D.blackTexture);
	}

	// Token: 0x0600011B RID: 283 RVA: 0x0000CDF4 File Offset: 0x0000AFF4
	[PublicizedFrom(EAccessModifier.Protected)]
	public void SyncBlendMat(Vector3 size)
	{
		if (this.blendMatInstance != null && this.matInstance != null)
		{
			this.blendMatInstance.CopyPropertiesFromMaterial(this.matInstance);
			Vector4 vector = default(Vector4);
			vector.z = size.x;
			vector.w = size.z;
			vector.x = base.transform.position.x;
			vector.y = base.transform.position.z;
			this.blendMatInstance.SetVector("_TerrainBounds", vector);
			this.blendMatInstance.SetTexture("_TerrainDesc", this.terrainDesc);
		}
	}

	// Token: 0x0600011C RID: 284 RVA: 0x0000CEAC File Offset: 0x0000B0AC
	public virtual Bounds GetBounds()
	{
		return default(Bounds);
	}

	// Token: 0x0600011D RID: 285 RVA: 0x0000CEC4 File Offset: 0x0000B0C4
	public Material GetBlendMatInstance()
	{
		if (this.blendMat != null && this.terrainDesc != null)
		{
			if (this.blendMatInstance == null)
			{
				this.blendMatInstance = new Material(this.blendMat);
				this.SyncBlendMat(this.GetBounds().size);
			}
			if (this.blendMatInstance.shader != this.blendMat.shader)
			{
				this.blendMatInstance.shader = this.blendMat.shader;
				this.SyncBlendMat(this.GetBounds().size);
			}
		}
		return this.blendMatInstance;
	}

	// Token: 0x0600011E RID: 286 RVA: 0x0000CF70 File Offset: 0x0000B170
	[PublicizedFrom(EAccessModifier.Protected)]
	public void ApplyBlendMap()
	{
		if (this.blendMat != null && this.terrainDesc != null)
		{
			if (this.blendMatInstance == null)
			{
				this.blendMatInstance = new Material(this.blendMat);
			}
			this.SyncBlendMat(this.GetBounds().size);
		}
	}

	// Token: 0x0600011F RID: 287 RVA: 0x000027FC File Offset: 0x000009FC
	public void RevisionFromMat()
	{
	}

	// Token: 0x06000120 RID: 288 RVA: 0x0000CFCC File Offset: 0x0000B1CC
	public static void SyncAll()
	{
		MicroSplatTerrain.SyncAll();
	}

	// Token: 0x04000148 RID: 328
	[HideInInspector]
	public Material templateMaterial;

	// Token: 0x04000149 RID: 329
	[HideInInspector]
	[NonSerialized]
	public Material matInstance;

	// Token: 0x0400014A RID: 330
	[HideInInspector]
	public Material blendMat;

	// Token: 0x0400014B RID: 331
	[HideInInspector]
	public Material blendMatInstance;

	// Token: 0x0400014C RID: 332
	[HideInInspector]
	public MicroSplatKeywords keywordSO;

	// Token: 0x0400014D RID: 333
	[HideInInspector]
	public Texture2D perPixelNormal;

	// Token: 0x0400014E RID: 334
	[HideInInspector]
	public Texture2D terrainDesc;

	// Token: 0x0400014F RID: 335
	public MicroSplatObject.DescriptorFormat descriptorFormat;

	// Token: 0x04000150 RID: 336
	[HideInInspector]
	public Texture2D streamTexture;

	// Token: 0x04000151 RID: 337
	[HideInInspector]
	public Texture2D cavityMap;

	// Token: 0x04000152 RID: 338
	[HideInInspector]
	public MicroSplatProceduralTextureConfig procTexCfg;

	// Token: 0x04000153 RID: 339
	[HideInInspector]
	public Texture2D procBiomeMask;

	// Token: 0x04000154 RID: 340
	[HideInInspector]
	public Texture2D procBiomeMask2;

	// Token: 0x04000155 RID: 341
	[HideInInspector]
	public Texture2D tintMapOverride;

	// Token: 0x04000156 RID: 342
	[HideInInspector]
	public Texture2D globalNormalOverride;

	// Token: 0x04000157 RID: 343
	[HideInInspector]
	public Texture2D globalSAOMOverride;

	// Token: 0x04000158 RID: 344
	[HideInInspector]
	public Texture2D globalEmisOverride;

	// Token: 0x04000159 RID: 345
	[HideInInspector]
	public Texture2D geoTextureOverride;

	// Token: 0x0400015A RID: 346
	[HideInInspector]
	public MicroSplatPropData propData;

	// Token: 0x0200002A RID: 42
	public enum DescriptorFormat
	{
		// Token: 0x0400015C RID: 348
		RGBAHalf,
		// Token: 0x0400015D RID: 349
		RGBAFloat
	}
}

using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200002F RID: 47
[ExecuteInEditMode]
[RequireComponent(typeof(Terrain))]
[DisallowMultipleComponent]
public class MicroSplatTerrain : MicroSplatObject
{
	// Token: 0x14000001 RID: 1
	// (add) Token: 0x0600012F RID: 303 RVA: 0x0000D4E8 File Offset: 0x0000B6E8
	// (remove) Token: 0x06000130 RID: 304 RVA: 0x0000D51C File Offset: 0x0000B71C
	public static event MicroSplatTerrain.MaterialSyncAll OnMaterialSyncAll;

	// Token: 0x14000002 RID: 2
	// (add) Token: 0x06000131 RID: 305 RVA: 0x0000D550 File Offset: 0x0000B750
	// (remove) Token: 0x06000132 RID: 306 RVA: 0x0000D588 File Offset: 0x0000B788
	public event MicroSplatTerrain.MaterialSync OnMaterialSync;

	// Token: 0x06000133 RID: 307 RVA: 0x0000D5BD File Offset: 0x0000B7BD
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.terrain = base.GetComponent<Terrain>();
	}

	// Token: 0x06000134 RID: 308 RVA: 0x0000D5CB File Offset: 0x0000B7CB
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		this.terrain = base.GetComponent<Terrain>();
		MicroSplatTerrain.sInstances.Add(this);
		if (this.reenabled)
		{
			this.Sync();
		}
	}

	// Token: 0x06000135 RID: 309 RVA: 0x0000D5F2 File Offset: 0x0000B7F2
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.Sync();
	}

	// Token: 0x06000136 RID: 310 RVA: 0x0000D5FA File Offset: 0x0000B7FA
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		MicroSplatTerrain.sInstances.Remove(this);
		this.Cleanup();
		this.reenabled = true;
	}

	// Token: 0x06000137 RID: 311 RVA: 0x0000D615 File Offset: 0x0000B815
	[PublicizedFrom(EAccessModifier.Private)]
	public void Cleanup()
	{
		if (this.matInstance != null && this.matInstance != this.templateMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.matInstance);
			this.terrain.materialTemplate = null;
		}
	}

	// Token: 0x06000138 RID: 312 RVA: 0x0000D650 File Offset: 0x0000B850
	public void Sync()
	{
		if (this.templateMaterial == null)
		{
			return;
		}
		Material material;
		if (this.terrain.materialTemplate == this.matInstance && this.matInstance != null)
		{
			this.terrain.materialTemplate.CopyPropertiesFromMaterial(this.templateMaterial);
			material = this.terrain.materialTemplate;
		}
		else
		{
			material = new Material(this.templateMaterial);
		}
		if (this.terrain.drawInstanced && this.keywordSO.IsKeywordEnabled("_TESSDISTANCE") && this.keywordSO.IsKeywordEnabled("_MSRENDERLOOP_SURFACESHADER"))
		{
			Debug.LogWarning("Disabling terrain instancing when tessellation is enabled, as Unity has not made surface shader tessellation compatible with terrain instancing");
			this.terrain.drawInstanced = false;
		}
		material.hideFlags = HideFlags.HideAndDontSave;
		this.terrain.materialTemplate = material;
		this.matInstance = material;
		base.ApplyMaps(material);
		if (this.keywordSO.IsKeywordEnabled("_CUSTOMSPLATTEXTURES"))
		{
			material.SetTexture("_CustomControl0", (this.customControl0 != null) ? this.customControl0 : Texture2D.blackTexture);
			material.SetTexture("_CustomControl1", (this.customControl1 != null) ? this.customControl1 : Texture2D.blackTexture);
			material.SetTexture("_CustomControl2", (this.customControl2 != null) ? this.customControl2 : Texture2D.blackTexture);
			material.SetTexture("_CustomControl3", (this.customControl3 != null) ? this.customControl3 : Texture2D.blackTexture);
			material.SetTexture("_CustomControl4", (this.customControl4 != null) ? this.customControl4 : Texture2D.blackTexture);
			material.SetTexture("_CustomControl5", (this.customControl5 != null) ? this.customControl5 : Texture2D.blackTexture);
			material.SetTexture("_CustomControl6", (this.customControl6 != null) ? this.customControl6 : Texture2D.blackTexture);
			material.SetTexture("_CustomControl7", (this.customControl7 != null) ? this.customControl7 : Texture2D.blackTexture);
		}
		else
		{
			if (this.terrain == null || this.terrain.terrainData == null)
			{
				Debug.LogError("Terrain or terrain data is null, cannot sync");
				return;
			}
			Texture2D[] alphamapTextures = this.terrain.terrainData.alphamapTextures;
			base.ApplyControlTextures(alphamapTextures, material);
		}
		base.ApplyBlendMap();
		if (this.OnMaterialSync != null)
		{
			this.OnMaterialSync(material);
		}
	}

	// Token: 0x06000139 RID: 313 RVA: 0x0000D8D3 File Offset: 0x0000BAD3
	public override Bounds GetBounds()
	{
		return this.terrain.terrainData.bounds;
	}

	// Token: 0x0600013A RID: 314 RVA: 0x0000D8E8 File Offset: 0x0000BAE8
	public new static void SyncAll()
	{
		for (int i = 0; i < MicroSplatTerrain.sInstances.Count; i++)
		{
			MicroSplatTerrain.sInstances[i].Sync();
		}
		if (MicroSplatTerrain.OnMaterialSyncAll != null)
		{
			MicroSplatTerrain.OnMaterialSyncAll();
		}
	}

	// Token: 0x0400019F RID: 415
	[HideInInspector]
	public Shader addPass;

	// Token: 0x040001A2 RID: 418
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static List<MicroSplatTerrain> sInstances = new List<MicroSplatTerrain>();

	// Token: 0x040001A3 RID: 419
	public Terrain terrain;

	// Token: 0x040001A4 RID: 420
	[HideInInspector]
	public Texture2D customControl0;

	// Token: 0x040001A5 RID: 421
	[HideInInspector]
	public Texture2D customControl1;

	// Token: 0x040001A6 RID: 422
	[HideInInspector]
	public Texture2D customControl2;

	// Token: 0x040001A7 RID: 423
	[HideInInspector]
	public Texture2D customControl3;

	// Token: 0x040001A8 RID: 424
	[HideInInspector]
	public Texture2D customControl4;

	// Token: 0x040001A9 RID: 425
	[HideInInspector]
	public Texture2D customControl5;

	// Token: 0x040001AA RID: 426
	[HideInInspector]
	public Texture2D customControl6;

	// Token: 0x040001AB RID: 427
	[HideInInspector]
	public Texture2D customControl7;

	// Token: 0x040001AC RID: 428
	[HideInInspector]
	public bool reenabled;

	// Token: 0x02000030 RID: 48
	// (Invoke) Token: 0x0600013E RID: 318
	public delegate void MaterialSyncAll();

	// Token: 0x02000031 RID: 49
	// (Invoke) Token: 0x06000142 RID: 322
	public delegate void MaterialSync(Material m);
}

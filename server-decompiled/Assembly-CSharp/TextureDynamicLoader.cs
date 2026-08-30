using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C5C RID: 3164
public class TextureDynamicLoader : MonoBehaviour
{
	// Token: 0x06006039 RID: 24633 RVA: 0x002618F4 File Offset: 0x0025FAF4
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (!this.DistanceChecks)
		{
			return;
		}
		if (Time.time - this.lastTimeChecked < (float)(this.bLastTimeFarAwayCamera ? 5 : 1))
		{
			return;
		}
		this.lastTimeChecked = Time.time;
		if (this.mainCamera == null)
		{
			this.mainCamera = Camera.main;
		}
		if (this.mainCamera == null)
		{
			return;
		}
		float num = Vector3.Distance(this.mainCamera.transform.position, base.transform.position);
		bool flag = num > (float)this.LoResDistance;
		if (this.bHiResLoaded && flag)
		{
			this.bHiResLoaded = false;
			this.SetLoResTexture(false);
		}
		else if (!this.bHiResLoaded && !flag)
		{
			this.bHiResLoaded = true;
			this.SetHiResTexture();
		}
		this.bLastTimeFarAwayCamera = num > 50f;
	}

	// Token: 0x0600603A RID: 24634 RVA: 0x002619C5 File Offset: 0x0025FBC5
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		if (!this.DistanceChecks && !this.bHiResLoaded)
		{
			this.bHiResLoaded = true;
			this.SetHiResTexture();
		}
	}

	// Token: 0x0600603B RID: 24635 RVA: 0x002619E4 File Offset: 0x0025FBE4
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		this.mainCamera = null;
		if (this.bHiResLoaded)
		{
			this.SetLoResTexture(false);
			this.bHiResLoaded = false;
		}
	}

	// Token: 0x0600603C RID: 24636 RVA: 0x00261A04 File Offset: 0x0025FC04
	public bool IsHiResTextureLoaded(out bool _bHires)
	{
		_bHires = false;
		this.checkMaterials();
		if (this.materials.Count == 0 || this.materials[0].Length == 0 || !this.materials[0][0])
		{
			return false;
		}
		Texture texture = this.materials[0][0].GetTexture(TextureDynamicLoader.textureTypes[0]);
		if (texture == null)
		{
			return false;
		}
		string name = texture.name;
		_bHires = !name.EndsWith("_LOW");
		return true;
	}

	// Token: 0x0600603D RID: 24637 RVA: 0x00261A8C File Offset: 0x0025FC8C
	public void SetHiResTexture()
	{
		this.checkMaterials();
		for (int i = 0; i < this.materials.Count; i++)
		{
			for (int j = 0; j < this.materials[i].Length; j++)
			{
				for (int k = 0; k < TextureDynamicLoader.textureTypes.Length; k++)
				{
					Material material = this.materials[i][j];
					if (material.HasProperty(TextureDynamicLoader.textureTypes[k]))
					{
						this.setHiResTexture(material, TextureDynamicLoader.textureTypes[k]);
					}
				}
			}
		}
	}

	// Token: 0x0600603E RID: 24638 RVA: 0x00261B0C File Offset: 0x0025FD0C
	public void SetLoResTexture(bool _bFindFolderAndCreateLoResTex = false)
	{
		this.checkMaterials();
		if (!Application.isPlaying && _bFindFolderAndCreateLoResTex)
		{
			string text = this.determineAssetsFolder();
			if (this.CreateLowResTexture)
			{
				for (int i = 0; i < this.materials.Count; i++)
				{
					for (int j = 0; j < this.materials[i].Length; j++)
					{
						for (int k = 0; k < TextureDynamicLoader.textureTypes.Length; k++)
						{
							Material material = this.materials[i][j];
							if (!(material == null) && material.HasProperty(TextureDynamicLoader.textureTypes[k]))
							{
								Texture texture = material.GetTexture(TextureDynamicLoader.textureTypes[k]);
								if (!(texture == null) && texture is Texture2D)
								{
									this.createLoResTexture(texture as Texture2D, text);
								}
							}
						}
					}
				}
			}
		}
		for (int l = 0; l < this.materials.Count; l++)
		{
			for (int m = 0; m < this.materials[l].Length; m++)
			{
				for (int n = 0; n < TextureDynamicLoader.textureTypes.Length; n++)
				{
					Material material2 = this.materials[l][m];
					if (!(material2 == null) && material2.HasProperty(TextureDynamicLoader.textureTypes[n]))
					{
						this.setLoResTexture(material2, TextureDynamicLoader.textureTypes[n]);
					}
				}
			}
		}
	}

	// Token: 0x0600603F RID: 24639 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void createLoResTexture(Texture2D _tex, string _path)
	{
	}

	// Token: 0x06006040 RID: 24640 RVA: 0x00261C74 File Offset: 0x0025FE74
	public static void SaveTexture(Texture2D _texture, string _fileName)
	{
		byte[] array = _texture.EncodeToPNG();
		SdFile.WriteAllBytes(_fileName, array);
	}

	// Token: 0x06006041 RID: 24641 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
	[PublicizedFrom(EAccessModifier.Private)]
	public string determineAssetsFolder()
	{
		return null;
	}

	// Token: 0x06006042 RID: 24642 RVA: 0x00261C90 File Offset: 0x0025FE90
	[PublicizedFrom(EAccessModifier.Private)]
	public void checkMaterials()
	{
		if (Application.isPlaying && this.bGotMaterials)
		{
			return;
		}
		this.bGotMaterials = true;
		base.GetComponentsInChildren<Renderer>(true, this.renderes);
		this.materials.Clear();
		this.materials.Capacity = this.renderes.Count;
		for (int i = 0; i < this.renderes.Count; i++)
		{
			Renderer renderer = this.renderes[i];
			bool flag = true;
			int num = 0;
			while (this.ExcludedRenderers != null && num < this.ExcludedRenderers.Length)
			{
				if (renderer == this.ExcludedRenderers[num])
				{
					flag = false;
					break;
				}
				num++;
			}
			if (flag)
			{
				if (Application.isPlaying && this.UseInstancedMaterial)
				{
					this.materials.Add(renderer.materials);
				}
				else
				{
					this.materials.Add(renderer.sharedMaterials);
				}
			}
		}
	}

	// Token: 0x06006043 RID: 24643 RVA: 0x00261D70 File Offset: 0x0025FF70
	[PublicizedFrom(EAccessModifier.Private)]
	public void setHiResTexture(Material _m, string _propName)
	{
		if (!_m.HasProperty(_propName))
		{
			return;
		}
		Texture texture = _m.GetTexture(_propName);
		if (texture == null)
		{
			return;
		}
		string text = texture.name;
		if (!text.EndsWith("_LOW"))
		{
			if (Application.isPlaying)
			{
				TextureLoadingManager.Instance.LoadTexture(_m, _propName, this.AssetPath, text, texture);
			}
			return;
		}
		text = text.Substring(0, text.Length - "_LOW".Length);
		if (Application.isPlaying)
		{
			TextureLoadingManager.Instance.LoadTexture(_m, _propName, this.AssetPath, text, texture);
			return;
		}
		Texture texture2 = Resources.Load<Texture2D>(this.AssetPath + text);
		_m.SetTexture(_propName, texture2);
	}

	// Token: 0x06006044 RID: 24644 RVA: 0x00261E18 File Offset: 0x00260018
	[PublicizedFrom(EAccessModifier.Private)]
	public void setLoResTexture(Material _m, string _propName)
	{
		Texture texture = _m.GetTexture(_propName);
		if (texture == null)
		{
			return;
		}
		string text = texture.name;
		if (text.EndsWith("_LOW"))
		{
			if (Application.isPlaying)
			{
				TextureLoadingManager.Instance.UnloadTexture(this.AssetPath, text.Substring(0, text.Length - "_LOW".Length));
			}
			return;
		}
		text += "_LOW";
		bool flag = true;
		if (Application.isPlaying)
		{
			flag = TextureLoadingManager.Instance.UnloadTexture(this.AssetPath, texture.name);
		}
		if (flag || this.UseInstancedMaterial)
		{
			Texture texture2 = Resources.Load<Texture2D>(this.AssetPath + text);
			_m.SetTexture(_propName, texture2);
		}
	}

	// Token: 0x04004B81 RID: 19329
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int cWidthLoRes = 64;

	// Token: 0x04004B82 RID: 19330
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int cDelayNearCamera = 1;

	// Token: 0x04004B83 RID: 19331
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int cDelayFarAwayCamera = 5;

	// Token: 0x04004B84 RID: 19332
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int cDefaultDistanceLoRes = 20;

	// Token: 0x04004B85 RID: 19333
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int cDistFarAwayCamera = 50;

	// Token: 0x04004B86 RID: 19334
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const string cPrefixPath = "Assets/Resources";

	// Token: 0x04004B87 RID: 19335
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const string cPostFixLoResTex = "_LOW";

	// Token: 0x04004B88 RID: 19336
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static string[] textureTypes = new string[] { "_MainTex", "_BumpMap", "_MetallicGlossMap", "_SpecGlossMap", "_OcclusionMap", "_EmissionMap" };

	// Token: 0x04004B89 RID: 19337
	public int LoResDistance = 20;

	// Token: 0x04004B8A RID: 19338
	public string AssetPath;

	// Token: 0x04004B8B RID: 19339
	public bool DistanceChecks = true;

	// Token: 0x04004B8C RID: 19340
	public bool UseInstancedMaterial = true;

	// Token: 0x04004B8D RID: 19341
	public Renderer[] ExcludedRenderers;

	// Token: 0x04004B8E RID: 19342
	public bool CreateLowResTexture;

	// Token: 0x04004B8F RID: 19343
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Material[]> materials = new List<Material[]>();

	// Token: 0x04004B90 RID: 19344
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Renderer> renderes = new List<Renderer>();

	// Token: 0x04004B91 RID: 19345
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bGotMaterials;

	// Token: 0x04004B92 RID: 19346
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float lastTimeChecked;

	// Token: 0x04004B93 RID: 19347
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Camera mainCamera;

	// Token: 0x04004B94 RID: 19348
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bLastTimeFarAwayCamera;

	// Token: 0x04004B95 RID: 19349
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bHiResLoaded;
}

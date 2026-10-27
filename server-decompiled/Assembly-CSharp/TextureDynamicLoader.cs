using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C79 RID: 3193
public class TextureDynamicLoader : MonoBehaviour
{
	// Token: 0x06006077 RID: 24695 RVA: 0x00266118 File Offset: 0x00264318
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

	// Token: 0x06006078 RID: 24696 RVA: 0x002661E9 File Offset: 0x002643E9
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		if (!this.DistanceChecks && !this.bHiResLoaded)
		{
			this.bHiResLoaded = true;
			this.SetHiResTexture();
		}
	}

	// Token: 0x06006079 RID: 24697 RVA: 0x00266208 File Offset: 0x00264408
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

	// Token: 0x0600607A RID: 24698 RVA: 0x00266228 File Offset: 0x00264428
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

	// Token: 0x0600607B RID: 24699 RVA: 0x002662B0 File Offset: 0x002644B0
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

	// Token: 0x0600607C RID: 24700 RVA: 0x00266330 File Offset: 0x00264530
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

	// Token: 0x0600607D RID: 24701 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void createLoResTexture(Texture2D _tex, string _path)
	{
	}

	// Token: 0x0600607E RID: 24702 RVA: 0x00266498 File Offset: 0x00264698
	public static void SaveTexture(Texture2D _texture, string _fileName)
	{
		byte[] array = _texture.EncodeToPNG();
		SdFile.WriteAllBytes(_fileName, array);
	}

	// Token: 0x0600607F RID: 24703 RVA: 0x000200DA File Offset: 0x0001E2DA
	[PublicizedFrom(EAccessModifier.Private)]
	public string determineAssetsFolder()
	{
		return null;
	}

	// Token: 0x06006080 RID: 24704 RVA: 0x002664B4 File Offset: 0x002646B4
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

	// Token: 0x06006081 RID: 24705 RVA: 0x00266594 File Offset: 0x00264794
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

	// Token: 0x06006082 RID: 24706 RVA: 0x0026663C File Offset: 0x0026483C
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

	// Token: 0x04004BE5 RID: 19429
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int cWidthLoRes = 64;

	// Token: 0x04004BE6 RID: 19430
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int cDelayNearCamera = 1;

	// Token: 0x04004BE7 RID: 19431
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int cDelayFarAwayCamera = 5;

	// Token: 0x04004BE8 RID: 19432
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int cDefaultDistanceLoRes = 20;

	// Token: 0x04004BE9 RID: 19433
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int cDistFarAwayCamera = 50;

	// Token: 0x04004BEA RID: 19434
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const string cPrefixPath = "Assets/Resources";

	// Token: 0x04004BEB RID: 19435
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const string cPostFixLoResTex = "_LOW";

	// Token: 0x04004BEC RID: 19436
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static string[] textureTypes = new string[] { "_MainTex", "_BumpMap", "_MetallicGlossMap", "_SpecGlossMap", "_OcclusionMap", "_EmissionMap" };

	// Token: 0x04004BED RID: 19437
	public int LoResDistance = 20;

	// Token: 0x04004BEE RID: 19438
	public string AssetPath;

	// Token: 0x04004BEF RID: 19439
	public bool DistanceChecks = true;

	// Token: 0x04004BF0 RID: 19440
	public bool UseInstancedMaterial = true;

	// Token: 0x04004BF1 RID: 19441
	public Renderer[] ExcludedRenderers;

	// Token: 0x04004BF2 RID: 19442
	public bool CreateLowResTexture;

	// Token: 0x04004BF3 RID: 19443
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Material[]> materials = new List<Material[]>();

	// Token: 0x04004BF4 RID: 19444
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Renderer> renderes = new List<Renderer>();

	// Token: 0x04004BF5 RID: 19445
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bGotMaterials;

	// Token: 0x04004BF6 RID: 19446
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float lastTimeChecked;

	// Token: 0x04004BF7 RID: 19447
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Camera mainCamera;

	// Token: 0x04004BF8 RID: 19448
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bLastTimeFarAwayCamera;

	// Token: 0x04004BF9 RID: 19449
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bHiResLoaded;
}

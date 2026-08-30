using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C4A RID: 3146
public class TextureDynamicLoader : MonoBehaviour
{
	// Token: 0x06005FAF RID: 24495 RVA: 0x0025EA54 File Offset: 0x0025CC54
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

	// Token: 0x06005FB0 RID: 24496 RVA: 0x0025EB25 File Offset: 0x0025CD25
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		if (!this.DistanceChecks && !this.bHiResLoaded)
		{
			this.bHiResLoaded = true;
			this.SetHiResTexture();
		}
	}

	// Token: 0x06005FB1 RID: 24497 RVA: 0x0025EB44 File Offset: 0x0025CD44
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

	// Token: 0x06005FB2 RID: 24498 RVA: 0x0025EB64 File Offset: 0x0025CD64
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

	// Token: 0x06005FB3 RID: 24499 RVA: 0x0025EBEC File Offset: 0x0025CDEC
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

	// Token: 0x06005FB4 RID: 24500 RVA: 0x0025EC6C File Offset: 0x0025CE6C
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

	// Token: 0x06005FB5 RID: 24501 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void createLoResTexture(Texture2D _tex, string _path)
	{
	}

	// Token: 0x06005FB6 RID: 24502 RVA: 0x0025EDD4 File Offset: 0x0025CFD4
	public static void SaveTexture(Texture2D _texture, string _fileName)
	{
		byte[] array = _texture.EncodeToPNG();
		SdFile.WriteAllBytes(_fileName, array);
	}

	// Token: 0x06005FB7 RID: 24503 RVA: 0x0001FF1E File Offset: 0x0001E11E
	[PublicizedFrom(EAccessModifier.Private)]
	public string determineAssetsFolder()
	{
		return null;
	}

	// Token: 0x06005FB8 RID: 24504 RVA: 0x0025EDF0 File Offset: 0x0025CFF0
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

	// Token: 0x06005FB9 RID: 24505 RVA: 0x0025EED0 File Offset: 0x0025D0D0
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

	// Token: 0x06005FBA RID: 24506 RVA: 0x0025EF78 File Offset: 0x0025D178
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

	// Token: 0x04004AE4 RID: 19172
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int cWidthLoRes = 64;

	// Token: 0x04004AE5 RID: 19173
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int cDelayNearCamera = 1;

	// Token: 0x04004AE6 RID: 19174
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int cDelayFarAwayCamera = 5;

	// Token: 0x04004AE7 RID: 19175
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int cDefaultDistanceLoRes = 20;

	// Token: 0x04004AE8 RID: 19176
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int cDistFarAwayCamera = 50;

	// Token: 0x04004AE9 RID: 19177
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const string cPrefixPath = "Assets/Resources";

	// Token: 0x04004AEA RID: 19178
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const string cPostFixLoResTex = "_LOW";

	// Token: 0x04004AEB RID: 19179
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static string[] textureTypes = new string[] { "_MainTex", "_BumpMap", "_MetallicGlossMap", "_SpecGlossMap", "_OcclusionMap", "_EmissionMap" };

	// Token: 0x04004AEC RID: 19180
	public int LoResDistance = 20;

	// Token: 0x04004AED RID: 19181
	public string AssetPath;

	// Token: 0x04004AEE RID: 19182
	public bool DistanceChecks = true;

	// Token: 0x04004AEF RID: 19183
	public bool UseInstancedMaterial = true;

	// Token: 0x04004AF0 RID: 19184
	public Renderer[] ExcludedRenderers;

	// Token: 0x04004AF1 RID: 19185
	public bool CreateLowResTexture;

	// Token: 0x04004AF2 RID: 19186
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Material[]> materials = new List<Material[]>();

	// Token: 0x04004AF3 RID: 19187
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Renderer> renderes = new List<Renderer>();

	// Token: 0x04004AF4 RID: 19188
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bGotMaterials;

	// Token: 0x04004AF5 RID: 19189
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float lastTimeChecked;

	// Token: 0x04004AF6 RID: 19190
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Camera mainCamera;

	// Token: 0x04004AF7 RID: 19191
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bLastTimeFarAwayCamera;

	// Token: 0x04004AF8 RID: 19192
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bHiResLoaded;
}
